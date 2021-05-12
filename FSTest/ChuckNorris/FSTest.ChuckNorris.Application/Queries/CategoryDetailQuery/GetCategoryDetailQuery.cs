using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using FSTest.ChuckNorris.Application.Queries.CategoryDetailQuery;

namespace FSTest.ChuckNorris.Application.Queries.CategoryDetailQuery
{
    public class GetCategoryDetailQuery : IRequest<CategoryDetailViewModel>
    {
        public string Category { get; set; }
        public class GetCategoriesQueryHandler : IRequestHandler<GetCategoryDetailQuery, CategoryDetailViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly HttpClient _httpClient;
            public GetCategoriesQueryHandler(IConfiguration configuration, HttpClient httpClient)
            {
                _configuration = configuration;
                _httpClient = httpClient;
            }

            public async Task<CategoryDetailViewModel> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
            {
                var url = _configuration["Chuck:CategoryDetailEndpoint"] + request.Category;
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new CategoryDetailViewModel();
                }
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<CategoryDetailViewModel>();
            }
        }
    }
}
