using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MediatR;
using FSTest.StarWars.Application.Queries;

namespace FSTest.StartWars.Api.Controllers
{
    [ApiController]
    public class SwapiController : ControllerBase
    {
        private readonly ILogger<SwapiController> _logger;
        private readonly IMediator _mediator;

        public SwapiController(ILogger<SwapiController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]/people")]
        [ProducesResponseType(typeof(PeopleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] GetPeopleQuery getPeopleQuery)
        {
            try
            {
                var people = await _mediator.Send(getPeopleQuery);
                return Ok(people);
            }
            catch (Exception ex)
            {
                // logging 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
