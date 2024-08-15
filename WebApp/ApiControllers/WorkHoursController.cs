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
    public class WorkHoursController(IAppBll bll, UserManager<AppUser> userManager, IMapper autoMapper) : ControllerBase
    {
        private readonly BllDtoMapper<BLL.DTO.WorkHours, DTO.v1_0.WorkHours> _mapper = new(autoMapper);
        private readonly BllDtoMapper<BLL.DTO.WorkHours, DTO.v1_0.WorkHoursCreate> _mapperCreate = new(autoMapper);
        
        /// <summary>
        /// Get all workhours connected to a workplace
        /// </summary>
        /// <returns>List of work hours</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<List<DTO.v1_0.WorkHours>>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<List<DTO.v1_0.WorkHours>>> GetWorkHours(Guid id)
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

            var workHours = await bll.WorkHours.GetWorkDaysAsync(userId, id);
            var mapped = workHours.Select(c => _mapper.Map(c)).OfType<WorkHours>().ToList();
            return Ok(mapped);
        }
        
        /// <summary>
        /// Get workhours entry
        /// </summary>
        /// <returns>work hour</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<DTO.v1_0.WorkHours>((int) HttpStatusCode.OK)]
        public async Task<ActionResult<DTO.v1_0.WorkHours>> GetWorkHour(Guid id)
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

            var workHour = await bll.WorkHours.GetWorkDayAsync(userId, id);
            var mapped = _mapper.Map(workHour);
            return Ok(mapped);
        }
        
        /// <summary>
        /// Create new workhours entry
        /// </summary>
        /// <param name="workHours"></param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType<WorkHours>((int) HttpStatusCode.Created)]
        public async Task<ActionResult<WorkHours>> Create([FromBody] WorkHoursCreate workHours)
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
            
            var mapped = _mapperCreate.Map(workHours);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work hours entry not added"
                });
            }

            mapped.Id = Guid.NewGuid();
            
            var added = bll.WorkHours.Add(mapped);
            
            if (added == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work hours entry not added"
                });
            }

            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.WorkHours.ExistsAsync(added.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Adding work hours entry failed"
                    });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                nameof(GetWorkHour),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()?.ToString(),
                    id = added.Id
                },
                _mapper.Map(added)
                );
        }
        
        /// <summary>
        /// Edit work hours entry
        /// </summary>
        /// <param name="id">work hours id(guid)</param>
        /// <param name="workHours"></param>
        /// <returns>Status code 204 on success</returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType<RestApiErrorResponse>((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Edit(Guid id, [FromBody] WorkHours workHours)
        {
            if (id != workHours.Id)
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

            var mapped = _mapper.Map(workHours);
            if (mapped == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work hours not updated"
                });
            }
            
            var updated = bll.WorkHours.Update(mapped);
            if (updated == null)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work hours not updated"
                });
            }
            
            try
            {
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.WorkHours.ExistsAsync(updated.Id))
                {
                    return NotFound(new RestApiErrorResponse()
                    {
                        Status = HttpStatusCode.NotFound,
                        Error = "Updating work hours failed"
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
        /// Delete workhours entry
        /// </summary>
        /// <param name="id">user workhours id(guid)</param>
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

            var exists = await bll.WorkHours.ExistsAsync(id, userId);
            if (!exists)
            {
                return NotFound(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Error = "Work hours entry not found"
                });
            }

            await bll.WorkHours.RemoveAsync(id, userId);

            return NoContent();
        }
    }
}

