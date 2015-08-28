using System;
using System.Linq;
using RssExtractor.Downloader;
using RssExtractor.Rss;

namespace RssExtractor {

	class MainClass {

		public static void Main (string[] args) {

			var modules = new IModule[] {
				new ContentDownloadModule(),
				new BoilerpipeModule(),
				new LanguageRecognitionModule(),
				new FolderLocatorModule(),
				new DebugModule()
			};

			var urls = new[] {
				"http://g1.globo.com/dynamo/rss2.xml",
				"http://rss.uol.com.br/feed/noticias.xml"
			};

			var feeds = urls.Select(url => new Feed(url));

			var extractor = new Extractor(modules, new LinearFeedSelector(feeds));

			extractor.Run();

			Console.WriteLine("Finished");
		}

		class DebugModule : IModule {

			public void Apply (INews news) {
				Console.WriteLine("--------");
				Console.WriteLine(news.Address);
				Console.WriteLine(news.Data.GetOrDefault<string>("Language"));
				Console.WriteLine(news.Content.Length);
				Console.WriteLine(news.Content);
				Console.WriteLine("--------");
			}

		}

	}
}
