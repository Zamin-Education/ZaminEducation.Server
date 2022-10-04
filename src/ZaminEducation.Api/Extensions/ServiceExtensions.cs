using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Data.Repositories;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;
using ZaminEducation.Service.Services;
using ZaminEducation.Service.Services.Courses;

namespace ZaminEducation.Api
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            // repositories
            services.AddScoped<IRepository<Attachment>, Repository<Attachment>>();
            services.AddScoped<IRepository<Course>, Repository<Course>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<QuizResult>, Repository<QuizResult>>();
            services.AddScoped<IRepository<CourseVideo>, Repository<CourseVideo>>();
            services.AddScoped<IRepository<CourseComment>, Repository<CourseComment>>();
            services.AddScoped<IRepository<Certificate>, Repository<Certificate>>();
            services.AddScoped<IRepository<SavedCourse>, Repository<SavedCourse>>();
            services.AddScoped<IRepository<Quiz>, Repository<Quiz>>();
            services.AddScoped<IRepository<QuestionAnswer>, Repository<QuestionAnswer>>();
            services.AddScoped<IRepository<CourseRate>, Repository<CourseRate>>();
            services.AddScoped<IRepository<ApplicantUser>, Repository<ApplicantUser>>();
            services.AddScoped<IRepository<Direction>, Repository<Direction>>();
            services.AddScoped<IRepository<HomePagesInfo>, Repository<HomePagesInfo>>();
            services.AddScoped<IRepository<UserAsset>, Repository<UserAsset>>();

            // services
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuizResultService, QuizResultService>();
            services.AddScoped<IYouTubeService, YouTubeService>();
            services.AddScoped<ICourseCommentService, CourseCommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ISavedCoursesService, SavedCoursesService>();
            services.AddScoped<IApplicantUserService, ApplicantUserService>();
            services.AddScoped<IDirectionService, DirectionService>();
            services.AddScoped<IHomePagesInfoService, HomePagesInfoService>();
            services.AddScoped<IUserAssetService, UserAssetService>();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            string key = jwtSettings.GetSection("Key").Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))

                };
            });
        }

        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(p =>
            {
                var xmlFile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                p.IncludeXmlComments(xmlPath);

                p.ResolveConflictingActions(ad => ad.First());
                p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                p.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
