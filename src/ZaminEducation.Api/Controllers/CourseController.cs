using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;

namespace ZaminEducation.Api.Controllers;

//[Authorize(Policy = "AdminPolicy")]
public class CourseController : BaseController
{
    private readonly ICourseService courseService;
    private readonly ICourseCategoryService courseCategoryService;
    public CourseController(ICourseService courseService, ICourseCategoryService courseCategoryService)
    {
        this.courseService = courseService;
        this.courseCategoryService = courseCategoryService;
    }

    /// <summary>
    /// Create cource
    /// </summary>
    /// <param name="courseDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromForm] CourseForCreationDto courseDto)
        => Ok(await this.courseService.CreateAsync(courseDto));

    /// <summary>
    /// Create course category
    /// </summary>
    /// <param name="categoryDto"></param>
    /// <returns></returns>
    [HttpPost("category")]
    public async ValueTask<IActionResult> CreateCategoryAsync(CourseCategoryForCreationDto categoryDto)
        => Ok(await this.courseCategoryService.CreateAsync(categoryDto));

    /// <summary>
    /// Generate course link
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [HttpPost("link")]
    public async ValueTask<IActionResult> GenerateLinkAsync(long courseId)
        => Ok(await this.courseService.GenerateLinkAsync(courseId));

    /// <summary>
    /// Select all of course
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>

    [HttpGet, AllowAnonymous]
    public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(await this.courseService.GetAllAsync(@params));

    /// <summary>
    /// Select all of course category
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [HttpGet("category"), AllowAnonymous]
    public async ValueTask<IActionResult> GetAllCategory([FromQuery] PaginationParams @params)
        => Ok(await this.courseCategoryService.GetAllAsync(@params));

    /// <summary>
    /// Select course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetByIdAsync(long id)
        => Ok(await this.courseService.GetAsync(course => course.Id.Equals(id)));

    /// <summary>
    /// Select course category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("category/{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetCategoryAsync([FromRoute] long id)
        => Ok(await this.courseCategoryService.GetAsync(category => category.Id.Equals(id)));

    /// <summary>
    /// Update course by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="courseDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async ValueTask<IActionResult> UpdateAsync(long id, [FromForm] CourseForCreationDto courseDto)
        => Ok(await this.courseService.UpdateAsync(course => course.Id.Equals(id), courseDto));

    /// <summary>
    /// Update course category by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="categoryDto"></param>
    /// <returns></returns>
    [HttpPut("category/{id}")]
    public async ValueTask<IActionResult> UpdateCategoryAsync(long id, CourseCategoryForCreationDto categoryDto)
        => Ok(await this.courseCategoryService.UpdateAsync(id, categoryDto));

    /// <summary>
    /// Delete course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> DeleteAsync(long id)
        => Ok(await this.courseService.DeleteAsync(course => course.Id.Equals(id)));

    /// <summary>
    /// Delete course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("category/{id}"), Authorize(Roles = "Admin")]
    public async ValueTask<ActionResult<bool>> DeleteCategoryAsync([FromRoute] long id) =>
        Ok(await courseCategoryService.DeleteAsync(user => user.Id == id));

    /// <summary>
    /// Select videos of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("videos{id}")]
    public async ValueTask<IActionResult> GetCourseVideosAsync(long id)
        => Ok(await this.courseService.GetCourseVideosAsync(video => video.Id.Equals(id)));

    /// <summary>
    /// Select targets of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("targets{id}")]
    public async ValueTask<IActionResult> GetCourseTargetsAsync(long id)
        => Ok(await this.courseService.GetCourseTargetsAsync(target => target.Id.Equals(id)));

    /// <summary>
    /// Select moduls of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("models{id}")]
    public async ValueTask<IActionResult> GetCourseModulesAsync(long id)
        => Ok(await this.courseService.GetCourseModulesAsync(model => model.Id.Equals(id)));
}
