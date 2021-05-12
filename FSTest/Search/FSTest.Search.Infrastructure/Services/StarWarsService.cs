using FSTest.Search.Application.Interfaces;
using FSTest.Search.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FSTest.Search.Infrastructure
{
    public class StarWarsService : IStarWarsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public StarWarsService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<PeopleResponse> GetPeople(string query, int page)
        {
            var url = _configuration["Star:SearchPeopleEndpoint"] + query + "&page=" + page;
            var response = await _httpClient.GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new PeopleResponse();
            }
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PeopleResponse>();
        }
    }
}
