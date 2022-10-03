using AutoMapper;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Service.DTOs.Quizzes;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;

namespace ZaminEducation.Service.Services
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<QuizContent> quizContentRepository;
        private readonly IRepository<QuestionAnswer> answerRepository;
        private readonly IRepository<QuizAsset> quizAssetRepository;
        private readonly IAttachmentService attachmentService;
        private readonly IRepository<Quiz> quizRepository;
        private readonly ICourseService courseService;
        private readonly IMapper mapper;

        public QuizService(
            IRepository<QuizContent> quizContentRepository,
            IRepository<QuestionAnswer> answerRepository,
            IRepository<QuizAsset> quizAssetRepository,
            IAttachmentService attachmentService,
            IRepository<Quiz> quizRepository,
            ICourseService courseService,
            IMapper mapper)
        {
            this.quizContentRepository = quizContentRepository;
            this.quizAssetRepository = quizAssetRepository;
            this.attachmentService = attachmentService;
            this.answerRepository = answerRepository;
            this.quizRepository = quizRepository;
            this.courseService = courseService;
            this.mapper = mapper;
        }

        public async ValueTask<Quiz> CreateAsync(
            long courseId,
            QuizForCreationDto quizDto,
            QuizContentForCreationDto questionDto)
        {
            var course = await courseService.GetAsync(c => c.Id == courseId);

            if (course is null)
                throw new ZaminEducationException(400, "Bad request");

            var content = mapper.Map<QuizContent>(questionDto);
            content.Create();

            content = await quizContentRepository.AddAsync(content);

            var quiz = mapper.Map<Quiz>(quizDto);
            quiz.QuestionId = content.Id;
            quiz.CourseId = courseId;
            quiz.Create();

            await quizRepository.AddAsync(quiz);
            await quizRepository.SaveChangesAsync();

            return quiz;
        }

        public async ValueTask<QuestionAnswer> CreateAnswerAsync(QuestionAnswerForCreationDto dto)
        {
            var content = await quizContentRepository.GetAsync(c => c.Id == dto.ContentId);
            var quiz = await quizRepository.GetAsync(q => q.Id == dto.QuizId);
            var answer = await answerRepository.GetAsync(a => a.Id == dto.QuestionId);

            if (answer is null || quiz is null || content is null)
                throw new ZaminEducationException(400, "Bad request");

            answer = mapper.Map<QuestionAnswer>(dto);
            answer.Create();

            await answerRepository.AddAsync(answer);
            await answerRepository.SaveChangesAsync();

            return answer;
        }

        public async ValueTask<QuizAsset> CreateAssetsAsync(QuizAssetForCreationDto dto)
        {
            var content = await quizContentRepository.GetAsync(c => c.Id == dto.ContentId);
            var attachent = await attachmentService.GetAsync(a => a.Id == dto.FileId);

            if (content is null && attachent is null)
                throw new ZaminEducationException(400, "Bad request");

            var asset = mapper.Map<QuizAsset>(dto);
            asset.Create();

            asset = await quizAssetRepository.AddAsync(asset);
            await quizAssetRepository.SaveChangesAsync();

            return asset;
        }
    }
}
