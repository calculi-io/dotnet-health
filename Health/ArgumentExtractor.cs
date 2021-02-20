using System;

namespace Health {

	/// <summary>
	/// expects the command line arguments as an array of strings and
	/// sets values appropriately
	/// </summary>
	class ArgumentExtractor {

		public string Url { get; }
		public int Interval { get; }
		public int GracePeriod { get; }
		public int Timeout { get; }

		/// <summary>
		/// converts command line arguments to appropriate values
		/// </summary>
		/// <param name="args">arguments directly passed from the command line</param>
		public ArgumentExtractor(string[] args) {
			Interval = 30;
			Url = "";
			Timeout = 10;

			for (int i = 0; i < args.Length; ++i) {
				string aval = GetVal(args, i);
				switch (args[i].ToLower()) {
					case "-url":
						Url = aval;
						if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute) || !Uri.TryCreate(Url, UriKind.Absolute, out Uri result)) {
							Url = "";
						}
						break;
					case "-interval":
						try {
							Interval = int.Parse(aval);
							if (Interval <= 0) {
								Interval = 30;
							}
						}
						catch { /*do not set if error*/ }
						break;
					case "-graceperiod":
						try {
							GracePeriod = int.Parse(aval);
							if (GracePeriod < 0) {
								GracePeriod = 0;
							}
						}
						catch { /*do not set if error*/ }
						break;
					case "-timeout":
						try {
							Timeout = int.Parse(aval);
							if (Timeout <= 0) {
								Timeout = 10;
							}
						}
						catch { /*do not set if error*/ }
						break;
					default:
						break;
				}
			}

			Interval *= 1000;
			GracePeriod *= 1000;
		}

		/// <summary>
		/// returns the next command line argument value from the given index if there is one.
		/// </summary>
		/// <param name="args">arguments from the command line</param>
		/// <param name="index">index of the array to treat as the key</param>
		/// <returns>the next item in the array if it exists, otherwise an anpty string</returns>
		private string GetVal(string[] args, int index) {
			if (index + 1 < args.Length) {
				return args[index + 1];
			}
			return "";
		}

		/// <summary>
		/// detects if any invalid values exist
		/// </summary>
		/// <returns>true if any invalid values are detected</returns>
		public bool HasErrors() {
			return Url == "" || Interval < 1000 || GracePeriod < 0 || Timeout < 1;
		}
	}
}
