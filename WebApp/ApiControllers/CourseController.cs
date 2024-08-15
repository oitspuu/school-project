using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using BLL.Interfaces;
using Domain.Identity;
using DTO;
using DTO.v1_0;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class CourseController (IAppBll bll, UserManager<AppUser> userManager, IMapper autoMapper) : ControllerBase
    {
        private readonly IAppBll _bll = bll;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly BllDtoMapper<BLL.DTO.UserCourse, DTO.v1_0.UserCourse> _mapper = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.UserCourse, DTO.v1_0.UserCourseBasic> _mapperBasic = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.UserCourse, DTO.v1_0.UserCourseCreate> _mapperCreate = new(autoMapper);
        
        /// <summary>
        /// Get all user courses with full information
        /// </summary>
        /// <param name="language">language - current supported values are: en (English) and et (Estonian)</param>
        /// <returns>List of user courses</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.UserCourse>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.UserCourse>>> GetCourses(
            [FromQuery]
            string language = "en")
        {
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            var courses = await _bll.UserCourses.GetAllUserCoursesAsync(userId, language);
            var mapped = courses.Select(c => _mapper.Map(c)).OfType<UserCourse>().ToList();
            return Ok(mapped);
        }
        
        /// <summary>
        /// Get all user courses with basic information
        /// </summary>
        /// <param name="language">language - current supported values are: en (English) and et (Estonian)</param>
        /// <returns>List of user courses</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.UserCourseBasic>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.UserCourseBasic>>> GetCoursesBasic(
            [FromQuery] string language = "en")
        {
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            var courses = await _bll.UserCourses.GetAllUserCoursesAsync(userId, language);
            var mapped = courses.Select(c => _mapperBasic.Map(c)).OfType<UserCourseBasic>().ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Get user course
        /// </summary>
        /// <param name="id">course id</param>
        /// <param name="language">language - current supported values are: en (English) and et (Estonian)</param>
        /// <returns>user course</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserCourse>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<DTO.v1_0.UserCourse>> GetCourse(Guid id, 
            [FromQuery] string language = "en")
        {
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            var course = await _bll.UserCourses.GetUserCourseAsync((Guid)userId, id, language);
            var mapped = _mapper.Map(course);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not found"
                });
            }

            return Ok(mapped);
        }

        /// <summary>
        /// Add time spent to course
        /// </summary>
        /// <param name="id">Course id (guid)</param>
        /// <param name="addTime">Id and TimeSpent</param>
        /// <returns>Status code 204 on success</returns>
        [HttpPatch("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddTime(Guid id, [FromBody] UserCourseAddTime addTime)
        {
            if (id != addTime.Id)
            {
                return BadRequest();
            }
            
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }
            
            var success = await _bll.UserCourses.AddTime((Guid) userId, addTime.Id, addTime.TimeSpent);
            if (!success)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Adding time failed"
                });
            }
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.UserCourses.ExistsAsync(id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating time failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Create new user course
        /// </summary>
        /// <param name="userCourse"></param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserCourse>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<UserCourse>> Create([FromBody] UserCourseCreate userCourse)
        {
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }
            
            var mapped = _mapperCreate.Map(userCourse);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not added"
                });
            }

            mapped.AppUserId = userId;
            var added = await _bll.UserCourses.AddUserCourse(mapped);
            
            if (added == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not added"
                });
            }

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.UserCourses.ExistsAsync(added.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding course failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                nameof(GetCourse),
                new
                {
                    version = "1.0",
                    id = added.Id
                },
                _mapper.Map(added)
                );
        }

        /// <summary>
        /// Edit user course
        /// </summary>
        /// <param name="id">user course id(guid)</param>
        /// <param name="userCourse"></param>
        /// <returns>Status code 204 on success</returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserCourse userCourse)
        {
            if (id != userCourse.Id)
            {
                return BadRequest();
            }
            
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            var mapped = _mapper.Map(userCourse);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not updated"
                });
            }

            mapped.AppUserId = userId;

            var updated = await _bll.UserCourses.UpdateUserCourse(mapped);
            if (updated == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not updated"
                });
            }
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.UserCourses.ExistsAsync(updated.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating course failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="id">Course id(guid)</param>
        /// <returns>Status code 204 on success</returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(System.Guid id)
        {
            if (!Guid.TryParse(_userManager.GetUserId(User), out var userId)) {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            if (userId == Guid.Empty)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User not found"
                });
            }

            var exists = await _bll.UserCourses.ExistsAsync(id, userId);
            if (!exists)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Course not found"
                });
            }

            await _bll.UserCourses.RemoveAsync(id, userId);

            return NoContent();
        }
    }
}