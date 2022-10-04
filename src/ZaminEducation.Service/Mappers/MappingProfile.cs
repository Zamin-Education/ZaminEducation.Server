using AutoMapper;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.DTOs.Quizzes;
using ZaminEducation.Service.DTOs.UserCourses;

using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.ViewModels;

namespace ZaminEducation.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // user 
            CreateMap<UserSocialNetwork, UserSocialNetworkForCreationDto>().ReverseMap();
            CreateMap<User, UserForCreationDto>().ReverseMap();
            CreateMap<Region, RegionForCreationDto>().ReverseMap();
            CreateMap<Address, AddressForCreationDto>().ReverseMap();
            CreateMap<ApplicantUser, ApplicantUserForCreationDto>().ReverseMap();
            CreateMap<Direction, DirectionForCreationDto>().ReverseMap();

            // course
            CreateMap<CourseVideo, CourseVideoForCreationDto>().ReverseMap();
            CreateMap<CourseTarget, CourseTargetForCreationDto>().ReverseMap();
            CreateMap<CourseModule, CourseModuleForCreationDto>().ReverseMap();
            CreateMap<Course, CourseForCreationDto>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryForCreationDto>().ReverseMap();
            CreateMap<SavedCourse, SavedCourseForCreationDto>().ReverseMap();
            CreateMap<CourseRate, CourseRateForCreationDto>().ReverseMap();
            CreateMap<Course, CourseViewModel>().ReverseMap();
            CreateMap<CourseComment, CourseCommentForCreationDto>().ReverseMap();
            CreateMap<Certificate, CertificateForCreationDto>().ReverseMap();

            // quiz
            CreateMap<QuizResult, QuizResultForCreationDto>().ReverseMap();
            CreateMap<Quiz, QuizForCreationDto>().ReverseMap();
            CreateMap<QuizContent, QuizContentForCreationDto>().ReverseMap();
            CreateMap<QuizAsset, QuizAssetForCreationDto>().ReverseMap();
            CreateMap<QuestionAnswer, QuestionAnswerForCreationDto>().ReverseMap();
            CreateMap<HashTag, HashTagForCreationDto>().ReverseMap();

            // another
            CreateMap<Attachment, AttachmentForCreationDto>().ReverseMap();
        }
    }
}
