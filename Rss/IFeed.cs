using System;
using RssExtractor.Util;
using System.Collections.Generic;

namespace RssExtractor.Rss {

	public interface IFeed {

		/// <summary>
		/// Stores data related to a specific feed
		/// Should not be internally used, only used by modules
		/// To avoid collisions, use the module name as a prefix
		/// </summary>
		DataContainer Data { get; }

		/// <summary>
		/// Gets the news list of the feed
		/// </summary>
		IEnumerable<INews> GetNews();

		/// <summary>
		/// Sets the content of the feed, in plain XML (not URI)
		/// </summary>
		void SetContent(string xmlContent);

		/// <summary>
		/// Gets the URI address
		/// </summary>
		string GetUri();
	}

	public static class FeedExtension {

		private const string MeaningfulResult = "MeaningfulResult";

		public static void ProducedMeaningfulResult (this IFeed feed, bool value) {
			feed.Data.Set<bool>(MeaningfulResult, value);
		}

		public static bool ProducedMeaningfulResult (this IFeed feed) {
			return feed.Data.Exists(MeaningfulResult) && feed.Data.Get<bool>(MeaningfulResult);
		}
	}

}

