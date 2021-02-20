using System.Diagnostics;
using Xunit;

namespace Health.Tests {

	public class ProgramTests {
		
		[Fact]
		public void TestCallMainBadURL() {
			Program p = new Program();
			Stopwatch sw = new Stopwatch();
			string[] args = { "-Url", "http://localhost:8080/", "-Interval", "1", "-GracePeriod", "1", "-Timeout", "1" };
			sw.Start();
			p.WhileUrlHealthy(args);
			sw.Stop();
			Assert.True(sw.Elapsed.TotalSeconds > 3.0);
			Assert.True(sw.Elapsed.TotalSeconds < 5.0);
		}

	}

}
