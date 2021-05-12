using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FSTest.Search.Application.Interfaces;
using JW;

namespace FSTest.Search.Application.Queries
{
    public class SearchJokesAndPeopleQuery : IRequest<JokesAndPeopleViewModel>
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public class GetJokesAndPeopleQueryHandler : IRequestHandler<SearchJokesAndPeopleQuery, JokesAndPeopleViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly IChuckNorrisService _chuckNorrisService;
            private readonly IStarWarsService _starWarsService;
            public GetJokesAndPeopleQueryHandler(IConfiguration configuration, IChuckNorrisService chuckNorrisService, IStarWarsService starWarsService)
            {
                _configuration = configuration;
                _chuckNorrisService = chuckNorrisService;
                _starWarsService = starWarsService;
            }

            public async Task<JokesAndPeopleViewModel> Handle(SearchJokesAndPeopleQuery request, CancellationToken cancellationToken)
            {
                var query = request.Query;
                var page = request.Page == 0 ? 1 : request.Page;
                var pageSize = 10;

                // Paged result
                var chuckResponse = await _chuckNorrisService.GetJokes(query);
                Pager chuckPager = new Pager(chuckResponse.Total, page, pageSize, chuckResponse.Total / pageSize);
                chuckResponse.Result = _chuckNorrisService.PageJokesResponseResult(chuckResponse.Result, chuckPager);

                var response = await _starWarsService.GetPeople(query, page);
                return new JokesAndPeopleViewModel
                {
                    Swapi = response,
                    ChuckPager = chuckPager,
                    Chuck = chuckResponse
                };
            }
        }
    }
}
