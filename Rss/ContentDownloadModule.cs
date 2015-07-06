using System;
using RssExtractor.Downloader;

namespace RssExtractor.Rss {

	/// <summary>
	/// Downloads the content of a news
	/// Input news must have an Address
	/// Output: the Content is set
	/// </summary>
	public class ContentDownloadModule : IModule {

		public void Apply (INews news) {
			var req = new DownloadRequest(news.Address);
			news.Content = req.Download();
		}

	}
}

