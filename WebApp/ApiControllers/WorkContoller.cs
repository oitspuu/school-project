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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class WorkController(IAppBll bll, UserManager<AppUser> userManager, IMapper autoMapper)
        : ControllerBase
    {
        private readonly BllDtoMapper<BLL.DTO.UserWork, DTO.v1_0.UserWork> _mapper = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.UserWork, DTO.v1_0.UserWorkCreate> _mapperCreate = new(autoMapper);
        
        /// <summary>
        /// Get all user workplaces
        /// </summary>
        /// <returns>List of user workplaces</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.UserWork>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.UserWork>>> GetWorkplaces()
        {
            if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) {
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

            var workplaces = await bll.UserWorkplaces.GetAllUserWorkAsync(userId);
            var mapped = workplaces.Select(c => _mapper.Map(c)).OfType<UserWork>().ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Get user workplace
        /// </summary>
        /// <param name="id">userWork id</param>
        /// <returns>user work</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserWork>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<DTO.v1_0.UserWork>> GetWork(Guid id)
        {
            if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) {
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

            var work = await bll.UserWorkplaces.GetUserWorkAsync(userId, id);
            var mapped = _mapper.Map(work);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Workplace not found"
                });
            }

            return Ok(mapped);
        }

        /// <summary>
        /// Create new user work
        /// </summary>
        /// <param name="userWork"></param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserWork>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<UserWork>> Create([FromBody] UserWorkCreate userWork)
        {
            if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) {
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
            
            var mapped = _mapperCreate.Map(userWork);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Workplace not added"
                });
            }

            mapped.AppUserId = userId;
            var added = bll.UserWorkplaces.AddUserWork(mapped);
            
            if (added == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Workplace not added"
                });
            }

            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserWorkplaces.ExistsAsync(added.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding workplace failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                nameof(GetWork),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    id = added.Id
                },
                _mapper.Map(added)
                );
        }

        /// <summary>
        /// Edit user work
        /// </summary>
        /// <param name="id">user work id(guid)</param>
        /// <param name="userWork"></param>
        /// <returns>Status code 204 on success</returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserWork userWork)
        {
            if (id != userWork.Id)
            {
                return BadRequest();
            }
            
            if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) {
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

            var mapped = _mapper.Map(userWork);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work not updated"
                });
            }

            mapped.AppUserId = userId;

            var updated = await bll.UserWorkplaces.UpdateUserWork(mapped);
            if (updated == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work not updated"
                });
            }
            
            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserWorkplaces.ExistsAsync(updated.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating work failed"
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
        /// Delete user work
        /// </summary>
        /// <param name="id">user work id(guid)</param>
        /// <returns>Status code 204 on success</returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) {
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

            var exists = await bll.UserWorkplaces.ExistsAsync(id, userId);
            if (!exists)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "User work not found"
                });
            }

            await bll.UserWorkplaces.RemoveAsync(id, userId);

            return NoContent();
        }
    }
}