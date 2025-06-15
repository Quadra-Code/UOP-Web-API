using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UOP.Application.Common.DTOs;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Web.API.Controllers.Common
{
    [ApiController]
    [Route("api/states")]
    [Authorize()]
    public class StateController : ControllerBase
    {
        private readonly IGenericService<State, CreateStateDTO, StateDTO, Guid> _stateService;
        public StateController(IGenericService<State, CreateStateDTO, StateDTO, Guid> stateService)
        {
            _stateService = stateService;
        }


        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<StateDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string orderBy = "Order", string orderDirection = "asc", Guid? countryId = null, string? searchableLetters = null)
        {
            Expression<Func<State, bool>> filter = s => (searchableLetters == null || s.Name_En.Contains(searchableLetters) || s.Name_Ar.Contains(searchableLetters)) && (countryId == null || s.CountryId == countryId);

            var result = await _stateService.GetByFilterAsync(filter, pageIndex, pageSize, orderBy, orderDirection, default);

            return Ok(result.Value);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<StateDTO>> GetById(Guid id)
        {
            var result = await _stateService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }
        [HttpPost]

        public async Task<ActionResult<StateDTO>> Create([FromBody] CreateStateDTO StateDTO)
        {
            var result = await _stateService.AddAsync(StateDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return StatusCode(201, result.Value);
        }
        [HttpPut("{id}")]

        public async Task<ActionResult<StateDTO>> Update(Guid id, [FromBody] StateDTO StateDTO)
        {
            if (id != StateDTO.Id)
            {
                return BadRequest(new string[] { "The id in the request body does not match the id in the URL." });
            }

            var result = await _stateService.UpdateAsync(StateDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _stateService.RemoveAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
