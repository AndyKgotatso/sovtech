using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace FSTest.ChuckNorris.Application.Queries
{
    public class GetCategoriesQuery : IRequest<CategoriesViewModel>
    {
        public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly HttpClient _httpClient;
            public GetCategoriesQueryHandler(IConfiguration configuration, HttpClient httpClient)
            {
                _configuration = configuration;
                _httpClient = httpClient;
            }

            public async Task<CategoriesViewModel> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
            {
                /// Could move this logic to its own service file if the get more complex/bigger
                /// Service will be at the Infrastructure layer if needed to be moved
                var url = _configuration["Chuck:CategoriesEndpoint"];
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var categories = await  response.Content.ReadFromJsonAsync<List<string>>();

                return new CategoriesViewModel { Categories = categories };
            }
        }
    }
}
