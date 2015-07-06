using System;

namespace RssExtractor.Downloader {

	public interface IAsyncDownloadRequest {

		TimeSpan Timeout { get; }

		bool IsComplete { get; }

		string Content { get; }

		event EventHandler<DownloadCompleteEventArgs> Completed;

		event EventHandler<DownloadAbortedEventArgs> Aborted;

	}
}

