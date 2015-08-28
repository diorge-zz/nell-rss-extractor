using System;
using System.IO;
using System.Reflection;
using RssExtractor.Util;
using RssExtractor.Rss;
using System.Linq;

namespace RssExtractor.Tests {

	public class DownloadSpeedTest {

		public static void Main() {
			var test = new DownloadSpeedTest();
			test.Run();
		}

		public void Run() {
			var assembly = Assembly.GetExecutingAssembly();
			var wellFormedRss = assembly.GetEmbeddedFileText("RssExtractor.Tests", "TestData/WellFormedRss.xml");
			var feed = new Feed("");
			var news = feed.GetNews().ToArray();
		}

	}
}

