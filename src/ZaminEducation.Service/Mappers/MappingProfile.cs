using AutoMapper;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.CoursesDtos;
using ZaminEducation.Service.DTOs.QuizzesDtos;
using ZaminEducation.Service.DTOs.UserCoursesDtos;
using ZaminEducation.Service.DTOs.UsersDtos;

namespace ZaminEducation.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserSocialNetwork, UserSocialNetworkForCreationDto>().ReverseMap();
            CreateMap<User, UserForCreationDto>().ReverseMap();
            CreateMap<Region, RegionForCreationDto>().ReverseMap();
            CreateMap<Address, AddressForCreationDto>().ReverseMap();
            CreateMap<SavedCourse, SavedCourseForCreationDto>().ReverseMap();
            CreateMap<CourseRate, CourseRateForCreationDto>().ReverseMap();
            CreateMap<CourseComment, CourseCommentForCreationDto>().ReverseMap();
            CreateMap<Certificate, CertificateForCreationDto>().ReverseMap();
            CreateMap<QuizResult, QuizResultForCreationDto>().ReverseMap();
            CreateMap<Quiz, QuizForCreationDto>().ReverseMap();
            CreateMap<QuizContent, QuizContentForCreationDto>().ReverseMap();
            CreateMap<QuizAsset, QuizAssetForCreationDto>().ReverseMap();
            CreateMap<QuestionAnswer, QuestionAnswerForCreationDto>().ReverseMap();
            CreateMap<HashTag, HashTagForCreationDto>().ReverseMap();
            CreateMap<CourseVideo, CourseVideoForCreationDto>().ReverseMap();
            CreateMap<CourseTarget, CourseTargetForCreationDto>().ReverseMap();
            CreateMap<CourseModule, CourseModuleForCreationDto>().ReverseMap();
            CreateMap<Course, CourseForCreationDto>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryForCreationDto>().ReverseMap();
        }
    }
}
