using System;

namespace RssExtractor.Downloader {

	public class DownloadCompleteEventArgs : EventArgs {

		public readonly string Uri;

		public readonly string Content;

		public DownloadCompleteEventArgs (string uri, string content) {
			this.Uri = uri;
			this.Content = content;
		}
	}
}

