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
    public class SleepController(IAppBll bll, UserManager<AppUser> userManager, IMapper autoMapper) : ControllerBase 
    {
        private readonly BllDtoMapper<BLL.DTO.SleepDuration, DTO.v1_0.Sleep> _mapper = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.SleepDuration, DTO.v1_0.SleepCreate> _mapperCreate = new(autoMapper);
        
        /// <summary>
        /// Get user sleep information
        /// </summary>
        /// <returns>List of user sleeping times</returns>
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.Sleep>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.Sleep>>> GetSleepTimes()
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

            var sleeps = await bll.Sleeps.GetAllSleepWithTotalAsync(userId);
            var mapped = sleeps.Select(c => _mapper.Map(c)).OfType<Sleep>().ToList();
            return Ok(mapped);
        }

        /// <summary>
        /// Get user sleep time
        /// </summary>
        /// <param name="id">sleep id</param>
        /// <returns>user sleep</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<Sleep>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<DTO.v1_0.Sleep>> GetSleep(Guid id)
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

            var sleep = await bll.Sleeps.GetSleepWithTotalAsync(id, (Guid)userId);
            var mapped = _mapper.Map(sleep);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep time not found"
                });
            }

            return Ok(mapped);
        }

        /// <summary>
        /// Create new sleep time
        /// </summary>
        /// <param name="sleepCreate"></param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<Sleep>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<Sleep>> Create([FromBody] SleepCreate sleepCreate)
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
            
            var mapped = _mapperCreate.Map(sleepCreate);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep time not added"
                });
            }

            mapped.AppUserId = userId;
            var added = bll.Sleeps.Add(mapped);
            
            if (added == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep time not added"
                });
            }

            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.Sleeps.ExistsAsync(added.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding sleep time failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                nameof(GetSleep),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    id = added.Id
                },
                _mapper.Map(added)
                );
        }

        /// <summary>
        /// Edit user sleep
        /// </summary>
        /// <param name="id">user sleep id(guid)</param>
        /// <param name="sleep"></param>
        /// <returns>Status code 204 on success</returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Sleep sleep)
        {
            if (id != sleep.Id)
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

            var mapped = _mapper.Map(sleep);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep not updated"
                });
            }

            mapped.AppUserId = userId;

            var updated = bll.Sleeps.Update(mapped);
            if (updated == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep not updated"
                });
            }
            
            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.Sleeps.ExistsAsync(updated.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating sleep failed"
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
        /// Delete sleep time
        /// </summary>
        /// <param name="id">sleep id(guid)</param>
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

            var exists = await bll.Sleeps.ExistsAsync(id, userId);
            if (!exists)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Sleep not found"
                });
            }

            await bll.Sleeps.RemoveAsync(id, userId);

            return NoContent();
        }

    }
}