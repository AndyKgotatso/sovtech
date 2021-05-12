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
using FSTest.ChuckNorris.Application.Queries;
using System.Collections.Generic;

namespace FSTest.ChuckNorris.Tests
{
    public class CategoriesQueryTests
    {
        private readonly List<string> categoriesList = new List<string>
        {
            "Value",
            "asfagsa"
        };

        private readonly GetCategoriesQuery _getCategoriesQuery = new GetCategoriesQuery();

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private GetCategoriesQuery.GetCategoriesQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Chuck:CategoriesEndpoint")]).Returns("https://local.com");
        }

        [Test]
        public async Task GivenGetCategoriesQueryThenReturnsListOfCategories()
        {
            var jsonObject = JsonConvert.SerializeObject(categoriesList);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoriesQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);
            var response = await _queryHandler.Handle(_getCategoriesQuery, It.IsAny<CancellationToken>());
            Assert.NotNull(response.Categories);
            Assert.NotZero(response.Categories.Count);
        }

        [Test]
        public async Task GivenGetCategoriesQueryAndThereIsAnExceptionThenThrows400BadRequest()
        {
            var jsonObject = JsonConvert.SerializeObject(categoriesList);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NotFound,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoriesQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);

            Assert.ThrowsAsync<HttpRequestException>(() => _queryHandler.Handle(_getCategoriesQuery, It.IsAny<CancellationToken>()));
        }

        [Test]
        public void GivenCategoryAndRequestIsInvalidThenThrows400BadRequest()
        {
            var jsonObject = JsonConvert.SerializeObject(categoriesList);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoriesQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);
            Assert.ThrowsAsync<HttpRequestException>(() => _queryHandler.Handle(_getCategoriesQuery, It.IsAny<CancellationToken>()));
        }
    }
}