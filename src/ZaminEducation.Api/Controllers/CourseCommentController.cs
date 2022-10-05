using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Api.Controllers;

[Authorize(Policy = "AllPolicy")]
public class CourseCommentController : BaseController
{
    private readonly ICourseCommentService courseCommentService;
    public CourseCommentController(ICourseCommentService courseCommentService)
    {
        this.courseCommentService = courseCommentService;
    }

    /// <summary>
    /// Create comment
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="message"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync(long courseId, string message, long? parentId) =>
        Ok(await this.courseCommentService.AddAsync(courseId, message, parentId));

    /// <summary>
    /// Select all comments of course by id
    /// </summary>
    /// <param name="params"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [HttpGet]
    public async ValueTask<IActionResult> GetAllAsync(
        [FromQuery] PaginationParams @params, long courseId) =>
            Ok(await this.courseCommentService.GetAllAsync(@params, courseId));

    /// <summary>
    /// Select comment of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async ValueTask<IActionResult> GetAsync(long id) =>
        Ok(await this.courseCommentService.GetAsync(id));

    /// <summary>
    /// Update comment
    /// </summary>
    /// <param name="id"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> UpdateAsync(long id, string message) =>
        Ok(await this.courseCommentService.UpdateAsync(id, message));

    /// <summary>
    /// Delete comment of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("id")]
    public async ValueTask<IActionResult> DeleteAsync(long id) =>
        Ok(await this.courseCommentService.DeleteAsync(id));

    /// <summary>
    /// Select replied comment of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("repliedcomment/{id}")]
    public async ValueTask<IActionResult> GetRepliedComments(long id) =>
        Ok(await this.courseCommentService.GetRepliedComments(id));
}
