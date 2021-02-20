using System.Net.Http;
using System.Threading;

namespace Health {

	/// <summary>
	/// checks if a requst returns with a success response
	/// </summary>
	class UrlChecker {

		private readonly string url;
		private int graceperiod;
		private readonly int interval;
		readonly HttpClientHandler handler;
		readonly HttpClient httpClient;

		public UrlChecker(ArgumentExtractor ae, HttpMessageHandler mh = null) {
			url = ae.Url;
			interval = ae.Interval;
			graceperiod = ae.GracePeriod;
			handler = new HttpClientHandler();
			if (mh == null) {
				httpClient = new HttpClient(handler);
			} else {
				httpClient = new HttpClient(mh);
			}
			httpClient.Timeout = new System.TimeSpan(0, 0, ae.Timeout);
		}

		/// <summary>
		/// using arguments provided by the argument extractor for delays check if the provided url is available
		/// </summary>
		/// <returns>true if within grace period or actually available</returns>
		public bool IsAvailable() {
			bool available = CheckURL();
			if (graceperiod > 0) {
				graceperiod -= interval;
				if (!available) {
					available = true;
				}
				else {
					graceperiod = 0;
				}
			}
			if (available) {
				Thread.Sleep(interval);
			}
			return available;
		}

		/// <summary>
		/// makes an asynchronus request for the provided url
		/// </summary>
		/// <returns>if the status code is a success status code</returns>
		bool CheckURL() {
		HttpResponseMessage result;
			try {
				result = httpClient.GetAsync(url).Result;
			}
			catch {
				return false;
			}

			return result.IsSuccessStatusCode;
		}
	}
}
