using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UOP.Application.Common.DTOs;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Web.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    //[Authorize]
    public class CityController : ControllerBase
    {
        private readonly IGenericService<City, CreateCityDTO, CityDTO, Guid> _cityService;
        public CityController(IGenericService<City, CreateCityDTO, CityDTO, Guid> cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CityDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string orderBy = "Order", string orderDirection = "asc", Guid? stateId = null, Guid? countryId = null, string? searchableLetters = null)
        {
            Expression<Func<City, bool>> filter = c =>
            (searchableLetters == null || c.Name_En.Contains(searchableLetters)
            || c.Name_Ar.Contains(searchableLetters))
            && (stateId == null || c.StateId == stateId)
            && (countryId == null || c.State.CountryId == countryId);

            var result = await _cityService.GetByFilterAsync(filter, pageIndex, pageSize, orderBy, orderDirection, default);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDTO>> GetById(Guid id)
        {

            var result = await _cityService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<CityDTO>> Create([FromBody] CreateCityDTO cityDTO)
        {
            var result = await _cityService.AddAsync(cityDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return StatusCode(201, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CityDTO>> Update(Guid id, [FromBody] CityDTO cityDTO)
        {
            if (id != cityDTO.Id)
            {
                return BadRequest(new string[] { "The id in the request body does not match the id in the URL." });
            }

            var result = await _cityService.UpdateAsync(cityDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var result = await _cityService.RemoveAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();

        }

    }
}
