using AutoMapper;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.MainPages;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.DTOs.HomePage;
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
            CreateMap<ZCApplicant, ZCApplicantForCreationDto>().ReverseMap();
            CreateMap<ZCApplicantDirection, ZCDirectionForCreationDto>().ReverseMap();

            // course
            CreateMap<CourseVideo, CourseVideoForCreationDto>().ReverseMap();
            CreateMap<CourseTarget, CourseTargetForCreationDto>().ReverseMap();
            CreateMap<CourseModule, CourseModuleForCreationDto>().ReverseMap();
            CreateMap<CourseForCreationDto, Course>().ForMember(p => p.Image, config => config.Ignore()).ReverseMap();
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
            CreateMap<HomePage, HomePageHeaderForCreationDto>().ReverseMap();
            CreateMap<InfoAboutProject, InfoAboutProjectForCreationDto>().ReverseMap();
            CreateMap<OfferedOpportunities, OfferedOpportunitesForCreationDto>().ReverseMap();
            CreateMap<Reason, ReasonForCreationDto>().ReverseMap();
            CreateMap<SocialNetworks, SocialNetworksForCreationDto>().ReverseMap();
        }
    }
}
