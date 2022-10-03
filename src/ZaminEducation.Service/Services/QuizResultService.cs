using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Service.DTOs.Quizzes;
using ZaminEducation.Service.DTOs.UserCourses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.ViewModels.Quizzes;

namespace ZaminEducation.Service.Services;

public class QuizResultService : IQuizResultService
{
    private readonly IRepository<QuestionAnswer> _questionAnswerRepository;
    private readonly IRepository<QuizResult> _quizResultRepository;
    private readonly IRepository<Quiz> _quizRepository;
    private readonly IConfiguration _configuration;

    private readonly ICertificateService _certificateService;
    private QuestionAnswer questionAnswer;
    private int countOfCorrectAnswers = 0;
    private readonly IMapper _mapper;
    private long courseId;

    public QuizResultService(IRepository<Quiz> quizRepository,
        IRepository<QuestionAnswer> questionAnswerRepository,
        IMapper mapper, IConfiguration configuration,
        IRepository<QuizResult> quizResultRepository,
        ICertificateService certificateService)
    {
        _questionAnswerRepository = questionAnswerRepository;
        _quizResultRepository = quizResultRepository;
        _certificateService = certificateService;
        _quizRepository = quizRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async ValueTask<IEnumerable<QuizResult>> GetAllAsync(
        Expression<Func<QuizResult, bool>> expression,
        PaginationParams @params)
    {
        var pagedList =
            _quizResultRepository.GetAll(expression, new string[] { "User", "Course" }, false).ToPagedList(@params);

        return await pagedList.ToListAsync();
    }
    public async ValueTask<QuizResult> GetAsync(Expression<Func<QuizResult, bool>> expression)
    {
        var existQuizResult = await _quizResultRepository.GetAsync(
            expression, new string[] { "User", "Course" });
            
        if (existQuizResult is null)
            throw new ZaminEducationException(404, "QuizResult not found.");

        return existQuizResult;
    }

    public async ValueTask<UserQuizzesResultViewModel> CreateAsync(IEnumerable<UserSelectionDto> dto)
    {
        var results = await CheckAsync(dto);

        double allowCertificatePersentage = double.Parse(_configuration["AllowCertificatePersentage"]);
        double userResult = GetTotalPersentage(dto.Count(), countOfCorrectAnswers);

        // certificate generation
        if (allowCertificatePersentage <= userResult)
            await _certificateService.CreateAsync(new CertificateForCreationDto()
            {
                CourseId = courseId,
                UserId = HttpContextHelper.UserId,
                Result = new CertificateResultDto()
                {
                    PassedPoint = $"{countOfCorrectAnswers}/{dto.Count()}",
                    Percentage = userResult
                }
            });

        // add result to database
        var quizResult = new QuizResultForCreationDto()
        {
            CourseId = courseId,
            Percentage = userResult,
            UserId = HttpContextHelper.UserId
        };

        await _quizResultRepository.AddAsync(_mapper.Map<QuizResult>(quizResult));
        await _quizResultRepository.SaveChangesAsync();

        return new()
        {
            Persentage = quizResult.Percentage,
            AcceptedQuizzes = countOfCorrectAnswers,
            quizResultViewModels = results,
            QuizzesCount = dto.Count()
        };
    }

    private async ValueTask<IEnumerable<QuizResultViewModel>> CheckAsync(IEnumerable<UserSelectionDto> dto)
    {
        if (dto is null)
            throw new ZaminEducationException(400, "Quiz must not be empty.");

        ICollection<QuizResultViewModel> results = new List<QuizResultViewModel>();

        var quiz = await _quizRepository.GetAsync(q => q.Id == dto.First().QuizId);

        courseId = quiz.CourseId;

        string[] includes = new[] { "QuizContent", "Answers" };
        var quizzes = _quizRepository.GetAll(c => c.Id == courseId, includes);

        if (quizzes.All(q => q.Id == quiz.Id))
            throw new ZaminEducationException(400, "Quiz must be belong to this course.");

        foreach (var userSelectionDto in dto)
        {
            bool isCorrect = false;

            questionAnswer = 
                await _questionAnswerRepository.GetAsync(a => a.Id == userSelectionDto.AnswerId);

            if (questionAnswer is not null
                && questionAnswer.QuizId == userSelectionDto.QuizId
                && questionAnswer.IsCorrect)
            {
                countOfCorrectAnswers++;
                isCorrect = true;
            }
            else if (questionAnswer is not null && questionAnswer.QuizId != userSelectionDto.QuizId)
                throw new ZaminEducationException(400, "Answers are incorrect.");

            results.Add(new QuizResultViewModel()
            {
                IsCorrect = isCorrect,
                Choice = questionAnswer,
                Quiz = quizzes.FirstOrDefault(q => q.Id == userSelectionDto.QuizId)
            });
        }
        return results;
    }

    private double GetTotalPersentage(long total, int value)
        => Math.Round((double)value * 100 / (double)total, 2);
}
