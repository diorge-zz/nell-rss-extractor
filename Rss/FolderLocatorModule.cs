using System;

namespace RssExtractor.Rss {

	/// <summary>
	/// This module locates which file the news should be saved
	/// Input: news must have Language data set
	/// Output: news have DestinationFolder data set
	/// </summary>
	public class FolderLocatorModule : IModule {

		public void Apply (INews news) {
			var lang = news.Data.GetOrDefault<string>("Language");
			var path = System.IO.Path.Combine("News", lang) + "/";
			news.Data.Set("DestinationFolder", path);
		}

	}
}

