using System;

namespace RssExtractor.Downloader {

	public interface IDownloadRequest {

		string Content { get; }

		TimeSpan Timeout { get; }

		string Download();

	}
}

