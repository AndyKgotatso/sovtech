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

namespace FSTest.ChuckNorris.Tests
{
    public class StarWarsServiceTests
    {
        private readonly PeopleResponse peopleResponse = new PeopleResponse
        {
            Count = 5,
            Next = "https://local.com/2"
        };

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private StarWarsService _starWarsService;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Star:SearchPeopleEndpoint")]).Returns("https://local.com/page=");
        }

        [Test]
        public async Task GivenQueryStringThenGetPeopleThatMatcheTheQuery()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });


            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _starWarsService = new StarWarsService(_mockConfiguration.Object, client);

            var response = await _starWarsService.GetPeople("query", 1);
            Assert.NotNull(response.Next);
            Assert.NotZero(response.Count);
        }

        [Test]
        public async Task GivenQueryStringWhenPeopleNotFoundThenReturnsZeroAndNullProperties()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NotFound,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _starWarsService = new StarWarsService(_mockConfiguration.Object, client);

            var response = await _starWarsService.GetPeople("query", 1);
            Assert.IsNull(response.Next);
            Assert.Zero(response.Count);
        }

        [Test]
        public void GivenQueryStringAndRequestIsInvalidThenThrows400BadRequest()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleResponse);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _starWarsService = new StarWarsService(_mockConfiguration.Object, client);
            Assert.ThrowsAsync<HttpRequestException>(() => _starWarsService.GetPeople("query", 1));
        }
    }
}