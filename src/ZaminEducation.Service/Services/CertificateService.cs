using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.UserCourses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
#pragma warning disable
    public class CertificateService : ICertificateService
    {
        private readonly IAttachmentService attachmentService;
        private readonly IRepository<Course> courseRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Certificate> certificateRepository;
        public CertificateService(IAttachmentService attachmentService,
            IRepository<Course> courseRepository,
            IRepository<User> userRepository,
            IRepository<Certificate> certificateRepository)
        {
            this.attachmentService = attachmentService;
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.certificateRepository = certificateRepository;
        }

        public async ValueTask<Attachment> CreateAsync(CertificateForCreationDto certificateForCreation)
        {
            var course = await courseRepository.GetAsync(c => c.Id.Equals(certificateForCreation.CourseId));

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            var user = await userRepository.GetAsync(u => u.Id.Equals(certificateForCreation.UserId));

            if (course is null)
                throw new ZaminEducationException(404, "User not found");

            var certifcate = await GenerateAsync(user.FirstName + " " + user.LastName,
                course.Name, certificateForCreation.Result.PassedPoint, certificateForCreation.Result.Percentage);

            var attachment = await attachmentService.CreateAsync(certifcate.fileName, certifcate.filePath);

            return attachment;
        }

        public async ValueTask<IEnumerable<Certificate>> GetAllAsync(
            PaginationParams @params,
            Expression<Func<Certificate, bool>> expression = null)
        {
            var certificates = certificateRepository.GetAll(expression, new string[] { "Attachment" });

            return await certificates.Where(c => c.UserId.Equals(HttpContextHelper.UserId)).ToPagedList(@params).ToListAsync();
        }

        public async ValueTask<Certificate> GetAsync(Expression<Func<Certificate, bool>> expression)
        {
            var certificate = await certificateRepository.GetAsync(expression, new string[] { "Attachment" });

            if (certificate is null)
                throw new ZaminEducationException(404, "Certificate not found");

            return certificate;
        }

        private ValueTask<(string fileName, string filePath)> GenerateAsync(string fullName, string courseName,
            string passedPoint, double percentage)
        {
            string filePath = Path.Combine(EnvironmentHelper.WebRootPath, "certificate.png");

            Bitmap bitmap = new Bitmap(filePath);

            // determine the level
            string result = string.Empty;

            if (percentage <= 75)
                result = Enum.GetName(typeof(CertificateLevel), 0);
            else if (percentage > 75 && percentage < 90)
                result = Enum.GetName(typeof(CertificateLevel), 1);
            else if (percentage >= 90)
                result = Enum.GetName(typeof(CertificateLevel), 2);

            // initialize Graphics class object
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            Brush brush = new SolidBrush(Color.FromKnownColor(KnownColor.Black));
            Brush brushForLevel = new SolidBrush(Color.FromArgb(36, 38, 93));

            // set text font
            Font arialOfName = new Font("Arial", 30, FontStyle.Italic);
            Font arialOfCourseName = new Font("Arial", 16, FontStyle.Italic);
            Font arialOfLevel = new Font("Arial", 25, FontStyle.Italic);
            Font arialOfPassedPoint = new Font("Arial", 10, FontStyle.Italic);

            // draw text
            SizeF sizeOfName = graphics.MeasureString(fullName, arialOfName);
            SizeF sizeOfCourseName = graphics.MeasureString(courseName, arialOfCourseName);
            SizeF sizeOfPassedPoint = graphics.MeasureString(passedPoint, arialOfName);
            SizeF sizeOfLevel = graphics.MeasureString(result, arialOfLevel);

            graphics.DrawString(fullName, arialOfName, brush, new PointF((bitmap.Width - sizeOfName.Width) / 2, 725));
            graphics.DrawString(courseName, arialOfCourseName, brush, new PointF((bitmap.Width - sizeOfCourseName.Width) / 2, 920));
            graphics.DrawString(result, arialOfLevel, brushForLevel, new PointF((bitmap.Width - sizeOfLevel.Width) / 2, 1000));
            graphics.DrawString(passedPoint, arialOfPassedPoint, brush, new PointF((bitmap.Width - sizeOfPassedPoint.Width) / 2, 1030));

            string outputFileName = Guid.NewGuid().ToString("N") + ".png";
            string staticPath = Path.Combine(EnvironmentHelper.CertificatePath, outputFileName);
            string outputFilePath = Path.Combine(EnvironmentHelper.WebRootPath, staticPath);

            bitmap.Save(outputFilePath, ImageFormat.Png);

            return ValueTask.FromResult((outputFileName, staticPath));
        }
    }
}
