using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Net;
using Moq.Protected;
using System.Text;
using System.Collections.Generic;
using FSTest.Search.Application.Queries;
using FSTest.Search.Application.Interfaces;
using FSTest.Search.Domain.Entities;

namespace FSTest.ChuckNorris.Tests
{
    public class SearchQueryTests
    {
        private readonly JokesResponse jokesResponse = new JokesResponse
        {
           Total = 50,
        };

        private readonly PeopleResponse peopleResponse = new PeopleResponse
        {
            Count = 15,
        };

        private readonly SearchJokesAndPeopleQuery _searchJokesAndPeopleQuery = new SearchJokesAndPeopleQuery()
        {
            Query = "query",
            Page = 0
        };

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IChuckNorrisService> _mockChuckNorrisService;
        private Mock<IStarWarsService> _mockStarWarsService;
        private SearchJokesAndPeopleQuery.GetJokesAndPeopleQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>(); 
            _mockChuckNorrisService = new Mock<IChuckNorrisService>();
            _mockStarWarsService = new Mock<IStarWarsService>();

            jokesResponse.Result = new List<JokeResult>();
            peopleResponse.Results = new List<Result>();

            for (int i = 1; i <= jokesResponse.Total; i++)
            {
                jokesResponse.Result.Add(new JokeResult { Id = "sgkdg", Value = "Joke" });
            }

            for (int i = 1; i <= peopleResponse.Count; i++)
            {
                peopleResponse.Results.Add(new Result { Name = "sgkdg", Height = "12kg" });
            }
        }

        [Test]
        public async Task GivenSearchJokesAndPeopleQueryThenReturnsJokesAndPeople()
        {
            _mockChuckNorrisService.Setup(x => x.GetJokes(It.IsAny<string>())).ReturnsAsync(jokesResponse);
            _mockStarWarsService.Setup(x => x.GetPeople(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(peopleResponse).Verifiable();

            _queryHandler = new SearchJokesAndPeopleQuery.GetJokesAndPeopleQueryHandler(_mockConfiguration.Object, _mockChuckNorrisService.Object, 
                _mockStarWarsService.Object);
            var response = await _queryHandler.Handle(_searchJokesAndPeopleQuery, It.IsAny<CancellationToken>());

            Assert.AreEqual(response.Swapi.Count, peopleResponse.Count);
            Assert.AreEqual(response.ChuckPager.TotalItems, jokesResponse.Total);
            Assert.Greater(response.ChuckPager.CurrentPage,0);
            Assert.AreEqual(response.Chuck.Total, jokesResponse.Total);

            _mockStarWarsService.Verify(x => x.GetPeople(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }


        [Test]
        public async Task GivenSearchJokesAndPeopleQueryWhenGetJokesThrows()
        {
            for (int i = 1; i <= jokesResponse.Total; i++)
            {
                jokesResponse.Result.Add(new JokeResult { Id = "sgkdg", Value = "Joke" });
            }
            _mockChuckNorrisService.Setup(x => x.GetJokes(It.IsAny<string>())).ThrowsAsync(new Exception());
            _mockStarWarsService.Setup(x => x.GetPeople(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(peopleResponse).Verifiable();

            _queryHandler = new SearchJokesAndPeopleQuery.GetJokesAndPeopleQueryHandler(_mockConfiguration.Object, _mockChuckNorrisService.Object,
                _mockStarWarsService.Object);

            Assert.ThrowsAsync<Exception>(()=> _queryHandler.Handle(_searchJokesAndPeopleQuery, It.IsAny<CancellationToken>()));

            _mockStarWarsService.Verify(x => x.GetPeople(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}