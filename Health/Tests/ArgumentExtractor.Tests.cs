using Xunit;
using Xunit.Abstractions;

namespace Health.Tests {

	public class ArgumentExtractorTest {

		[Fact]
		public void TestCorrectArguments() {
			string[] args = { "-Url", "http://something.io/", "-Interval", "5", "-GracePeriod", "6", "-Timeout", "5" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			Assert.False(ae.HasErrors());
			Assert.Equal(5000, ae.Interval);
			Assert.Equal(6000, ae.GracePeriod);
			Assert.Equal(5, ae.Timeout);
			Assert.Equal("http://something.io/", ae.Url);
		}

		[Fact]
		public void TestEmptyArguments() {
			string[] args = { };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			Assert.True(ae.HasErrors());
			Assert.Equal(30000, ae.Interval);
			Assert.Equal(0, ae.GracePeriod);
			Assert.Equal(10, ae.Timeout);
			Assert.Equal("", ae.Url);
		}

		[Fact]
		public void TestBadArguments() {
			string[] args = { "-Url", "asdf", "-Interval", "asdf", "-GracePeriod", "asdf", "-Timeout", "asdf" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			Assert.True(ae.HasErrors());
			Assert.Equal(30000, ae.Interval);
			Assert.Equal(0, ae.GracePeriod);
			Assert.Equal(10, ae.Timeout);
			Assert.Equal("", ae.Url);
		}

		[Fact]
		public void TestMissingArgumentValue() {
			string[] args = { "-Url", "asdf", "-Interval" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			Assert.True(ae.HasErrors());
			Assert.Equal(30000, ae.Interval);
			Assert.Equal(0, ae.GracePeriod);
			Assert.Equal(10, ae.Timeout);
			Assert.Equal("", ae.Url);
		}

		[Fact]
		public void TestOverrideBadArguments() {
			string[] args = { "-Url", "https://calculi.calculi.io", "-Interval", "-1", "-GracePeriod", "-1", "-Timeout", "-1" };
			ArgumentExtractor ae = new ArgumentExtractor(args);
			Assert.False(ae.HasErrors());
			Assert.Equal(30000, ae.Interval);
			Assert.Equal(0, ae.GracePeriod);
			Assert.Equal(10, ae.Timeout);
			Assert.Equal("https://calculi.calculi.io", ae.Url);
		}

	}
}
