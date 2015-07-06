using System;

namespace RssExtractor.Downloader {
	public class DownloadAbortedEventArgs : EventArgs {

		public readonly string Uri;

		public readonly Exception InnerException;

		public DownloadAbortedEventArgs (string uri, Exception cause) {
			this.Uri = uri;
			this.InnerException = cause;
		}
	}
}

