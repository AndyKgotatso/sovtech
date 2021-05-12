using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace FSTest.StarWars.Application.Queries
{
    public class GetPeopleQuery : IRequest<PeopleViewModel>
    {
        public int Page { get; set; }
        public class GetPeopleQueryHandler : IRequestHandler<GetPeopleQuery, PeopleViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly HttpClient _httpClient;

            public GetPeopleQueryHandler(IConfiguration configuration, HttpClient httpClient)
            {
                _configuration = configuration;
                _httpClient = httpClient;
            }

            public async Task<PeopleViewModel> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
            {
                /// Could move this logic to its own service file if the get more complex/bigger
                /// Service will be at the Infrastructure layer if needed to be moved
                var page = request.Page == 0 ? 1 : request.Page;
                var url = _configuration["Star:PeopleEndpoint"] + page;
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new PeopleViewModel();
                }
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<PeopleViewModel>();
            }
        }
    }
}
