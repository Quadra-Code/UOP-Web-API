using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UOP.Application.Common.DTOs;
using UOP.Domain.Entities;
using UOP.Domain.Interfaces;
using UOP.Domain.Models;

namespace UOP.Web.API.Controllers.Common
{
    [ApiController]
    [Route("api/countries")]
    [Authorize()]
    public class CountryController : ControllerBase
    {
        private readonly IGenericService<Country, CreateCountryDTO, CountryDTO, Guid> _countryService;
        public CountryController(IGenericService<Country, CreateCountryDTO, CountryDTO, Guid> countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CountryDTO>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string orderBy = "Order", string orderDirection = "asc", string? searchableLetters = null)
        {
            Expression<Func<Country, bool>> filter = c =>
            searchableLetters == null ||
            c.Name_En.Contains(searchableLetters) ||
            c.Name_Ar.Contains(searchableLetters);

            var result = await _countryService.GetByFilterAsync(filter, pageIndex, pageSize, orderBy, orderDirection, default);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetById(Guid id)
        {

            var result = await _countryService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<CountryDTO>> Create([FromBody] CreateCountryDTO cityDTO)
        {
            var result = await _countryService.AddAsync(cityDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
            return StatusCode(201, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CountryDTO>> Update(Guid id, [FromBody] CountryDTO countryDTO)
        {
            if (id != countryDTO.Id)
            {
                return BadRequest(new string[] { "The id in the request body does not match the id in the URL." });
            }

            var result = await _countryService.UpdateAsync(countryDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var result = await _countryService.RemoveAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();

        }

    }
}
