using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MediatR;
using FSTest.ChuckNorris.Application.Queries;
using FSTest.ChuckNorris.Application.Queries.CategoryDetailQuery;

namespace FSTest.ChuckNorris.Api.Controllers
{
    [ApiController]
    public class ChuckController : ControllerBase
    {
        private readonly ILogger<ChuckController> _logger;
        private readonly IMediator _mediator;

        public ChuckController(ILogger<ChuckController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("[controller]/categories")]
        [ProducesResponseType(typeof(CategoriesViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var catergories = await _mediator.Send(new GetCategoriesQuery());
                return Ok(catergories);
            }catch(Exception ex)
            {
                // logging 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("[controller]/category/{category}")]
        [ProducesResponseType(typeof(CategoryDetailViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDetail(string category)
        {
            try
            {
                var query = new GetCategoryDetailQuery();
                query.Category = category.ToLower();
                var catergories = await _mediator.Send(query);
                return Ok(catergories);
            }
            catch (Exception ex)
            {
                // logging 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
