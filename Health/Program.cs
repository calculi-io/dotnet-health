using System;
using System.Threading;

namespace Health {

	class Program {

		static void Main(string[] args) {
			Mutex singleInstance;
			ArgumentExtractor ae;
			UrlChecker uc;

			singleInstance = new System.Threading.Mutex(true, "f84eced6-09e9-47e4-ad98-f6fd7903a36f-health", out bool created);
			if (!created) {
				return;
			}

			ae = new ArgumentExtractor(args);
			if (ae.HasErrors()) {
				return;
			}
			uc = new UrlChecker(ae);
			while (uc.IsAvailable()) {/* wait until failure */}
			singleInstance.ReleaseMutex();
		}

		/// <summary>
		/// calls the main function with the intent for testing
		/// </summary>
		/// <param name="args">arguments that replicate what would be passed in from the command line</param>
		public void WhileUrlHealthy(string[] args) {
			Main(args);
		}
	}
}
