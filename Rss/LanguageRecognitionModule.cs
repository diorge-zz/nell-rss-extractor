using System;
using IvanAkcheurov.NTextCat.Lib;
using System.Linq;

namespace RssExtractor.Rss {

	/// <summary>
	/// This module recognizes the language of the text
	/// Input: news must have Content set
	/// Output: news have Language data set
	/// </summary>
	public class LanguageRecognitionModule : IModule {

		public const string RecognitionFile = "Core14.profile.xml";

		public void Apply (INews news) {
			var factory = new RankedLanguageIdentifierFactory();
			var identifier = factory.Load(RecognitionFile);
			var lang = identifier.Identify(news.Content);
			news.Data.Set("Language", lang.First().Item1.Iso639_3);
		}

	}
}

