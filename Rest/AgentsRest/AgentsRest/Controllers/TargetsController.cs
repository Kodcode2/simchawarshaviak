﻿// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(
        ApplicationDbContext dbContext, 
        ITargetService targetService,
        ILogger<TargetsController> logger
    ) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TargetModel>>> GetAllTargetsAsync()
        {
            try
            {
                List<TargetModel> targets = await targetService.GetAllTargetsAsync();
                return Ok(targets);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TargetModel>> GetTargetByIdAsync(int id)
        {
            try
            {
                TargetModel? target = await targetService.GetTargetByIdAsync(id); // Try find the target
                if (!await targetService.IsTargetExistAsync(id)) { return NotFound($"Target with id {id} does not exist."); } // return 404 if not exist
                return Ok(target); // return 200 with the target
            }
            catch (Exception ex) // Handling exceptions
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel>> CreateTargetAsync([FromBody] TargetDto targetDto)
        {
            try
            {
                TargetModel? target = await targetService.CreateTargetAsync(targetDto);
                return Ok(target);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PinTargetLocation(int id, [FromBody] LocationDto location)
        {
            try
            {
                if (id == null || location == null) { return BadRequest(); }

                if (!await targetService.IsTargetExistAsync(id)) { return NotFound($"Agent with id: {id} not found."); }

                TargetModel? target = await targetService.PlaceTargetAsync(id, location);

                return Ok(target);
            }
            catch (Exception ex) 
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }

        }

        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> MoveTargetLocation(int id, [FromBody] DirectionDto direction)
        {
            try
            {
                if (!await targetService.IsTargetExistAsync(id))
                { return NotFound($"Target with id {id} does not exist."); } // return 404 if not exist

                // if (locationDto == null) { return BadRequest("Location not valid."); }

                await targetService.MoveTargetAsync(id, direction.Direction);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex) // Handling exceptions
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching Location set. {ex.Message}"
                );
            }
        }
    }
}
