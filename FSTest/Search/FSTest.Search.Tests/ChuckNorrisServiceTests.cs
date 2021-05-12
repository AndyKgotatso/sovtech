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
using FSTest.Search.Infrastructure;
using FSTest.Search.Domain.Entities;
using JW;

namespace FSTest.ChuckNorris.Tests
{
    public class ChuckNorrisServiceTests
    {
        private readonly JokesResponse jokesResponse = new JokesResponse
        {
            Total = 5
        };

        private readonly Pager chuckPager = new Pager(50, 1, 5, 10);

        private readonly List<JokeResult> jokeResults = new List<JokeResult>();

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private ChuckNorrisService _chuckNorrisService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Chuck:SearchJokesEndpoint")]).Returns("https://local.com/page=");

            for (int i = 1; i <= 50; i++)
            {
                jokeResults.Add(new JokeResult { Id = "sgkdg", Value = "Joke" });
            }
        }

        [Test]
        public async Task GivenQueryStreingThenReturnsJokesResponse()
        {
            var jsonObject = JsonConvert.SerializeObject(jokesResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });


            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _chuckNorrisService = new ChuckNorrisService(_mockConfiguration.Object, client);

            var response = await _chuckNorrisService.GetJokes("query");
            Assert.NotZero(response.Total);
        }

        [Test]
        public async Task GivenQueryStringWhenJokesAreNotFoundThenReturnThrows()
        {
            var jsonObject = JsonConvert.SerializeObject(jokesResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NotFound,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _chuckNorrisService = new ChuckNorrisService(_mockConfiguration.Object, client);
            Assert.ThrowsAsync<HttpRequestException>(() => _chuckNorrisService.GetJokes("query"));
        }

        [Test]
        public void GivenQueryStringWhenRequestIsInvalidThenThrows400BadRequest()
        {
            var jsonObject = JsonConvert.SerializeObject(jokesResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });



            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _chuckNorrisService = new ChuckNorrisService(_mockConfiguration.Object, client);
            Assert.ThrowsAsync<HttpRequestException>(() => _chuckNorrisService.GetJokes("query"));
        }

        [Test]
        public void GivenCategoryAndRequestIsInvalidThenThrows400BadRequest()
        {
            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _chuckNorrisService = new ChuckNorrisService(_mockConfiguration.Object, client);
            var pagedResult = _chuckNorrisService.PageJokesResponseResult(jokeResults, chuckPager);
            Assert.AreEqual(jokeResults.Count, 50);
            Assert.AreEqual(pagedResult.Count, 5);
        }
    }
}