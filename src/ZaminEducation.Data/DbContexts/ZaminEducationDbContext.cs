using Microsoft.EntityFrameworkCore;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.MainPages;
using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;

namespace ZaminEducation.Data.DbContexts
{
    public class ZaminEducationDbContext : DbContext
    {
        public ZaminEducationDbContext(DbContextOptions<ZaminEducationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseCategory> CourseCategories { get; set; }
        public virtual DbSet<CourseModule> CourseModules { get; set; }
        public virtual DbSet<CourseTarget> CourseTargets { get; set; }
        public virtual DbSet<CourseVideo> CourseVideos { get; set; }
        public virtual DbSet<HashTag> HashTags { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<QuizAsset> QuizAssets { get; set; }
        public virtual DbSet<QuizContent> QuizContents { get; set; }
        public virtual DbSet<QuizResult> QuizResults { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<CourseComment> CourseComments { get; set; }
        public virtual DbSet<CourseRate> CourseRates { get; set; }
        public virtual DbSet<SavedCourse> SavedCourses { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSocialNetwork> UserSocialNetworks { get; set; }
        public virtual DbSet<ReferralLink> ReferralLinks { get; set; }
        public virtual DbSet<ZCApplicant> ZCApplicants { get; set; }
        public virtual DbSet<ZCApplicantAsset> ZCApplicantsAssets { get; set; }
        public virtual DbSet<ZCApplicantDirection> ZCApplicantDirections { get; set; }

        public virtual DbSet<HomePage> HomePages { get; set; }
        public virtual DbSet<HomePageHeader> HomePageHeaders { get; set; }
        public virtual DbSet<InfoAboutProject> InfoAboutProjects { get; set; }
        public virtual DbSet<OfferedOpportunities> OfferedOpportunities { get; set; }
        public virtual DbSet<PhotoGallery> PhotoGalleries { get; set; } 
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<SocialNetworks> SocialNetworks { get; set; }
        public virtual DbSet<PhotoGalleryAttachment> PhotoGalleryAttachments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Region)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Image)
                .WithMany()
                .HasForeignKey(c => c.ImageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseComment>()
                .HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseRate>()
               .HasOne(c => c.User)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<QuizResult>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // saved courses
            modelBuilder.Entity<SavedCourse>()
                .HasKey(sc => new { sc.CourseId, sc.UserId });

            modelBuilder.Entity<SavedCourse>()
                .HasOne(sc => sc.User)
                .WithMany()
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<SavedCourse>()
                .HasOne(sc => sc.Course)
                .WithMany()
                .HasForeignKey(sc => sc.CourseId);

            // question
            modelBuilder.Entity<QuestionAnswer>()
                .HasOne(q => q.Question)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<QuestionAnswer>()
                .HasOne(q => q.Content)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            // course
            modelBuilder.Entity<CourseModule>()
                .HasOne(cm => cm.Course)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Author)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferedOpportunities>()
                .Navigation(o => o.Opportunities)
                    .AutoInclude();

        }
    }
}
