using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Moq;
using Moq.Protected;


namespace Health.Tests {

	public class UrlCheckerTests {

		public Mock GenerateHttpMessageHandlerMock(HttpStatusCode []codes) {
			var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			foreach (HttpStatusCode code in codes) {
				messageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage() { StatusCode = code })
				.Verifiable();
			}

			return messageHandlerMock;
		}

		[Fact]
		public void TestGoodURL() {
			string[] args = { "-Url", "http://localhost:8080/", "-Interval", "1", "-GracePeriod", "0", "-Timeout", "1" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			HttpMessageHandler mh = (HttpMessageHandler)GenerateHttpMessageHandlerMock(new HttpStatusCode[]{HttpStatusCode.OK}).Object;
			UrlChecker uc = new UrlChecker(ae, mh);
			Assert.True(uc.IsAvailable());
		}

		[Fact]
		public void TestBadURL() {
			string[] args = { "-Url", "http://localhost:8080/", "-Interval", "1", "-GracePeriod", "0", "-Timeout", "1" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			HttpMessageHandler mh = (HttpMessageHandler)GenerateHttpMessageHandlerMock(new HttpStatusCode[] { HttpStatusCode.NotFound }).Object;
			UrlChecker uc = new UrlChecker(ae, mh);
			Assert.False(uc.IsAvailable());
		}

		[Fact]
		public void TestGoodURLFoundDuringGracePeriod() {
			string[] args = { "-Url", "http://localhost:8080/", "-Interval", "1", "-GracePeriod", "1", "-Timeout", "1" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			HttpMessageHandler mh = (HttpMessageHandler)GenerateHttpMessageHandlerMock(new HttpStatusCode[] { HttpStatusCode.NotFound, HttpStatusCode.NotFound }).Object;
			UrlChecker uc = new UrlChecker(ae, mh);
			Assert.True(uc.IsAvailable());
			Assert.False(uc.IsAvailable());
		}

		[Fact]
		public void TestGoodURLGracePeriod() {
			string[] args = { "-Url", "http://localhost:8080/", "-Interval", "1", "-GracePeriod", "1", "-Timeout", "1" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			HttpMessageHandler mh = (HttpMessageHandler)GenerateHttpMessageHandlerMock(new HttpStatusCode[] { HttpStatusCode.NotFound, HttpStatusCode.OK }).Object;
			UrlChecker uc = new UrlChecker(ae, mh);
			Assert.True(uc.IsAvailable());
			Thread.Sleep(1000);
			Assert.True(uc.IsAvailable());
		}

	}

}
