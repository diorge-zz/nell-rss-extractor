using System;
using System.Net;

namespace RssExtractor.Downloader {

	public class DownloadClient : IDownloadRequest {

		private readonly Uri uri;

		public DownloadClient(string uriString) {
			this.uri = new Uri(uriString);
		}

		public DownloadClient(Uri uri) {
			this.uri = uri;
		}

		public string Content { get; private set; }

		public TimeSpan Timeout {
			get {
				throw new NotImplementedException("No timeout for download clients");
			}
		}

		public string Download() {
			var client = new WebClient();
			return client.DownloadString(uri);
		}

	}
}

