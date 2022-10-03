using Microsoft.EntityFrameworkCore;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class CourseCommentService : ICourseCommentService
    {
        private readonly IRepository<Course> courseRepository;
        private readonly IRepository<CourseComment> courseCommentRepository;

        public CourseCommentService(
            IRepository<Course> courseRepository,
            IRepository<CourseComment> courseCommentRepository)
        {
            this.courseRepository = courseRepository;
            this.courseCommentRepository = courseCommentRepository;
        }

        public async ValueTask<CourseComment> AddAsync(long courseId, string message, long? parentId = null)
        {
            if (HttpContextHelper.UserId is null)
                throw new ZaminEducationException(401, "Unauthorized");

            var course = await courseRepository.GetAsync(c => c.Id == courseId);

            if (course == null)
                throw new ZaminEducationException(404, "Course not found");

            if (parentId is not null)
            {
                var parentComment = await courseCommentRepository.GetAsync(cc => cc.Id == parentId);

                if (parentComment is null)
                    throw new ZaminEducationException(404, "Comment not found to reply");

                parentComment.IsReplied = true;
                courseCommentRepository.Update(parentComment);
            }

            var comment = await courseCommentRepository.AddAsync(new CourseComment()
            {
                Text = message,
                CourseId = courseId,
                UserId = (long)HttpContextHelper.UserId,
                CreatedBy = (long)HttpContextHelper.UserId,
                ParentId = parentId
            });

            await courseCommentRepository.SaveChangesAsync();

            return comment;
        }

        public async ValueTask<bool> DeleteAsync(long id)
        {
            var comment = await courseCommentRepository.GetAsync(cc => cc.Id == id);

            if (comment is null)
                throw new ZaminEducationException(404, "Comment not found");

            if (HttpContextHelper.UserRole == "Admin" || comment.UserId == HttpContextHelper.UserId)
            {
                courseCommentRepository.Delete(comment);
                await courseCommentRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async ValueTask<IEnumerable<CourseComment>> GetAllAsync(PaginationParams @params, long courseId)
        {
            var comments = 
                courseCommentRepository.GetAll(cc => cc.CourseId == courseId && cc.ParentId == null).ToPagedList(@params);

            return await comments.ToListAsync();
        }

        public async ValueTask<CourseComment> GetAsync(long id)
        {
            var comment = await courseCommentRepository.GetAsync(cc => cc.Id == id);

            if (comment is null)
                throw new ZaminEducationException(404, "Comment not found");

            return comment;
        }

        public async ValueTask<IEnumerable<CourseComment>> GetRepliedComments(long id)
        {
            var comment = await courseCommentRepository.GetAsync(cc => cc.Id == id);

            if (comment is null)
                throw new ZaminEducationException(404, "Comment not found");

            var comments = await courseCommentRepository.GetAll(cc => cc.ParentId == id).ToListAsync();

            return comments;
        }

        public async ValueTask<CourseComment> UpdateAsync(long id, string message)
        {
            if (HttpContextHelper.UserId is null)
                throw new ZaminEducationException(401, "Unauthorized");

            var existComment = await courseCommentRepository.GetAsync(cc => cc.Id == id);

            if (existComment is null)
                throw new ZaminEducationException(404, "Comment not found");

            if (HttpContextHelper.UserId != existComment.UserId)
                throw new ZaminEducationException(403, "Forbidden");

            existComment.Text = message;
            existComment.Update();

            var updatedComment = courseCommentRepository.Update(existComment);
            await courseCommentRepository.SaveChangesAsync();

            return updatedComment;
        }

        public async ValueTask<IEnumerable<CourseComment>> GetAllAsync(PaginationParams @params, string search) =>
            await courseCommentRepository.GetAll(includes: new string[] { "User", "Course" },
                expression: cc => cc.Id.ToString() == search
                || cc.User.FirstName == search
                || cc.User.Username == search
                || cc.Course.Name == search)
                ?.ToPagedList(@params).ToListAsync();
    }
}