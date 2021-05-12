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
using FSTest.ChuckNorris.Application.Queries.CategoryDetailQuery;


namespace FSTest.ChuckNorris.Tests
{
    public class CategoyDetailsQueryTests
    {
        private readonly CategoryDetailViewModel categoryDetailViewModel = new CategoryDetailViewModel
        {
            Value = "Value",
            Id = "asfagsa"
        };

        private readonly GetCategoryDetailQuery _getCategoryDetailQuery = new GetCategoryDetailQuery
        {
            Category = "animal"
        };

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private GetCategoryDetailQuery.GetCategoriesQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Chuck:CategoryDetailEndpoint")]).Returns("https://local.com");
        }

        [Test]
        public async Task GivenCategoryThenReturnsCategoryDetails()
        {
            var jsonObject = JsonConvert.SerializeObject(categoryDetailViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoryDetailQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);
            var response = await _queryHandler.Handle(_getCategoryDetailQuery, It.IsAny<CancellationToken>());
            Assert.NotNull(response.Value);
            Assert.NotNull(response.Id);
        }

        [Test]
        public async Task GivenInvalidCategoryThenThrowsException()
        {
            var jsonObject = JsonConvert.SerializeObject(categoryDetailViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.NotFound,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoryDetailQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);

            var response = await _queryHandler.Handle(_getCategoryDetailQuery, It.IsAny<CancellationToken>());
            Assert.AreEqual(response.Value, null);
            Assert.AreEqual(response.Id, null);
        }

        [Test]
        public void GivenCategoryAndRequestIsInvalidThenThrows400BadRequest()
        {
            var jsonObject = JsonConvert.SerializeObject(categoryDetailViewModel);
            _mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest,
                      Content = new StringContent(jsonObject, Encoding.UTF8, "application/json")
                  });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _queryHandler = new GetCategoryDetailQuery.GetCategoriesQueryHandler(_mockConfiguration.Object, client);
             Assert.ThrowsAsync<HttpRequestException>(() => _queryHandler.Handle(_getCategoryDetailQuery, It.IsAny<CancellationToken>()));
        }
    }
}