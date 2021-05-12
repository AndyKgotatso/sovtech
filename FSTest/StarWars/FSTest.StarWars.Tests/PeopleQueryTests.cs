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
using FSTest.StarWars.Application.Queries;

namespace FSTest.StarWars.Tests
{
    public class PeopleQueryTests
    {
        private readonly PeopleViewModel peopleViewModel = new PeopleViewModel
        {
           Count = 5,
           Next = "https://local.com/2"
        };

        private readonly GetPeopleQuery _getPeopleQuery = new GetPeopleQuery {
            Page = 0
        };

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private GetPeopleQuery.GetPeopleQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Star:PeopleEndpoint")]).Returns("https://local.com");
        }

        [Test]
        public async Task GivenPageNumberThenReturnsPeopleList()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetPeopleQuery.GetPeopleQueryHandler(_mockConfiguration.Object, client);
            var response = await _queryHandler.Handle(_getPeopleQuery, It.IsAny<CancellationToken>());
            Assert.NotZero(response.Count);
        }

        [Test]
        public async Task GivenPageNumberWhenPageNotFoundThenReturnsNullProperties()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NotFound,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetPeopleQuery.GetPeopleQueryHandler(_mockConfiguration.Object, client);
            var response = await _queryHandler.Handle(_getPeopleQuery, It.IsAny<CancellationToken>());

            Assert.Null(response.Next);
        }

        [Test]
        public void GivenPageNumberWhenExceptionIsThrownThenThrowsBadrequest()
        {
            var jsonObject = JsonConvert.SerializeObject(peopleViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetPeopleQuery.GetPeopleQueryHandler(_mockConfiguration.Object, client);
            Assert.ThrowsAsync<HttpRequestException>(() => _queryHandler.Handle(_getPeopleQuery, It.IsAny<CancellationToken>()));
        }
    }
}