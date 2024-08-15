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
    public class HobbyController(IAppBll bll, UserManager<AppUser> userManager, IMapper autoMapper) : ControllerBase 
    {
        private readonly BllDtoMapper<BLL.DTO.UserHobby, DTO.v1_0.UserHobby> _mapper = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.UserHobby, DTO.v1_0.UserHobbyCreate> _mapperCreate = new(autoMapper);
        
        /// <summary>
        /// Get all user hobbies
        /// </summary>
        /// <param name="language">language - current supported values are: en (English) and et (Estonian)</param>
        /// <returns>List of user hobbies</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.UserHobby>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.UserHobby>>> GetHobbies(
            [FromQuery]
            string language = "en")
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

            var hobbies = await bll.UserHobbies.GetAllUserHobbiesAsync(userId, language);
            var mapped = hobbies.Select(c => _mapper.Map(c)).OfType<UserHobby>().ToList();
            return Ok(mapped);
        }
        
        /// <summary>
        /// Get user hobby
        /// </summary>
        /// <param name="id">hobby id(guid)</param>
        /// <param name="language">language - current supported values are: en (English) and et (Estonian)</param>
        /// <returns>user hobby</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserHobby>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<DTO.v1_0.UserHobby>> GetHobby(Guid id, 
            [FromQuery] string language = "en")
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

            var hobby = await bll.UserHobbies.GetUserHobbyAsync((Guid)userId, id, language);
            var mapped = _mapper.Map(hobby);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not found"
                });
            }

            return Ok(mapped);
        }
        
        /// <summary>
        /// Add time spent to hobby
        /// </summary>
        /// <param name="id">Hobby id (guid)</param>
        /// <param name="addTime">Id and TimeSpent</param>
        /// <returns>Status code 204 on success</returns>
        [HttpPatch("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddTime(Guid id, [FromBody] UserHobbyAddTime addTime)
        {
            if (id != addTime.Id)
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
            
            var success = await bll.UserHobbies.AddTime((Guid) userId, addTime.Id, addTime.TimeSpent);
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
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserHobbies.ExistsAsync(id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding time failed"
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
        /// Create new user hobby
        /// </summary>
        /// <param name="userHobby"></param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<UserHobby>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<UserHobby>> Create([FromBody] UserHobbyCreate userHobby)
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
            
            var mapped = _mapperCreate.Map(userHobby);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not added"
                });
            }

            mapped.AppUserId = userId;
            var added = await bll.UserHobbies.AddUserHobby(mapped);
            
            if (added == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not added"
                });
            }

            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserHobbies.ExistsAsync(added.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding hobby failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                nameof(GetHobby),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    id = added.Id
                },
                _mapper.Map(added)
                );
        }
        
        /// <summary>
        /// Edit user hobby
        /// </summary>
        /// <param name="id">user hobby id(guid)</param>
        /// <param name="userHobby"></param>
        /// <returns>Status code 204 on success</returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserHobby userHobby)
        {
            if (id != userHobby.Id)
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

            var mapped = _mapper.Map(userHobby);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not updated"
                });
            }

            mapped.AppUserId = userId;

            var updated = await bll.UserHobbies.UpdateUserHobby(mapped);
            if (updated == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not updated"
                });
            }
            
            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserHobbies.ExistsAsync(updated.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating hobby failed"
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
        /// Delete hobby
        /// </summary>
        /// <param name="id">Hobby id(guid)</param>
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

            var exists = await bll.UserHobbies.ExistsAsync(id, userId);
            if (!exists)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Hobby not found"
                });
            }

            await bll.UserHobbies.RemoveAsync(id, userId);

            return NoContent();
        }
    }
}