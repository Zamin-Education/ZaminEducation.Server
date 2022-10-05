using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.Interfaces.Courses;

namespace ZaminEducation.Api.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class CoursesController : BaseController
{
    private readonly ICourseService courseService;
    public CoursesController(ICourseService courseService)
    {
        this.courseService = courseService;
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
    /// Select all of course
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>

    [HttpGet, AllowAnonymous]
    public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params) 
        => Ok(await this.courseService.GetAllAsync(@params));

    /// <summary>
    /// Select course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetByIdAsync(long id) 
        => Ok(await this.courseService.GetAsync(course => course.Id.Equals(id)));

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
    /// Delete course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> DeleteAsync(long id) 
        => Ok(await this.courseService.DeleteAsync(course => course.Id.Equals(id)));


    /// <summary>
    /// Search course
    /// </summary>
    /// <param name="params"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("search"), AllowAnonymous]
    public async ValueTask<IActionResult> SearchAsync([FromQuery] PaginationParams @params, string search) 
        => Ok(await this.courseService.SearchAsync(@params, search));

    /// <summary>
    /// Select videos of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("videos{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetCourseVideosAsync(long id) 
        => Ok(await this.courseService.GetCourseVideosAsync(video => video.Id.Equals(id)));

    /// <summary>
    /// Select targets of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("targets{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetCourseTargetsAsync(long id) 
        => Ok(await this.courseService.GetCourseTargetsAsync(target => target.Id.Equals(id)));

    /// <summary>
    /// Select moduls of course by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("models{id}"), AllowAnonymous]
    public async ValueTask<IActionResult> GetCourseModulesAsync(long id) 
        => Ok(await this.courseService.GetCourseModulesAsync(model => model.Id.Equals(id)));
}
