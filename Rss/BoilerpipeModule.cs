using System;
using de.l3s.boilerpipe.extractors;
using java.net;

namespace RssExtractor.Rss {

	/// <summary>
	/// Extracts only relevant text from an article
	/// Input: news with HTML content
	/// Output: the HTML content will be on Raw, the clear content will be the new content
	/// </summary>
	public class BoilerpipeModule : IModule {

		public void Apply(INews news) {
			news.Data.Set("Raw", news.Content);
			news.Content = ArticleExtractor.INSTANCE.getText(news.Content);
		}

	}
}

