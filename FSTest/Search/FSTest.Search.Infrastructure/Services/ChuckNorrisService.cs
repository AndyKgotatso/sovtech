using FSTest.Search.Application.Interfaces;
using FSTest.Search.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using JW;

namespace FSTest.Search.Infrastructure
{
    public class ChuckNorrisService : IChuckNorrisService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public ChuckNorrisService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<JokesResponse> GetJokes(string query)
        {
            var url = _configuration["Chuck:SearchJokesEndpoint"] + query;
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<JokesResponse>();           
        }

        public List<JokeResult> PageJokesResponseResult(List<JokeResult> jokeResultList, Pager chuckPager)
        {
            var chuckPagedResult = jokeResultList.Skip((chuckPager.CurrentPage - 1) * chuckPager.PageSize)
                .Take(chuckPager.PageSize).ToList();
            return chuckPagedResult;
        }
    }
}
