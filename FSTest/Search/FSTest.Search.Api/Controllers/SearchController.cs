using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using MediatR;
using FSTest.Search.Application.Queries;

namespace FSTest.Search.Api.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IMediator _mediator;

        public SearchController(ILogger<SearchController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(typeof(JokesAndPeopleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] SearchJokesAndPeopleQuery searchJokesAndPeopleQuery)
        {
            if (string.IsNullOrEmpty(searchJokesAndPeopleQuery.Query) || searchJokesAndPeopleQuery.Query?.Length < 3 || searchJokesAndPeopleQuery.Query?.Length > 120)
            {
                return BadRequest(new { Query = "Size must be between 3 and 120" });
            }

            try
            {
                var result = await _mediator.Send(searchJokesAndPeopleQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // logging 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
