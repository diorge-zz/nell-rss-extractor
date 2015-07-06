using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RssExtractor.Rss;
using RssExtractor.Util;

namespace RssExtractor.Tests {

	[TestFixture]
	public class Test {

		private string WellFormedRss;

		[TestFixtureSetUp]
		public void Setup () {
			var assembly = Assembly.GetExecutingAssembly();
			WellFormedRss = assembly.GetEmbeddedFileText("RssExtractor.Tests", "TestData/WellFormedRss.xml");
		}

		[Test]
		public void FeedEnumerateNews () {
			Feed feed = new Feed("");
			feed.SetContent(WellFormedRss);
			INews[] news = feed.GetNews().ToArray();
			Assert.AreEqual(40, news.Length);
			Assert.AreEqual("http://g1.globo.com/sp/presidente-prudente-regiao/noticia/2015/03/pres-prudente-tem-59-oportunidades-de-trabalho-pelo-emprega-sao-paulo.html", news[0].Address);
			Assert.AreEqual("http://g1.globo.com/am/amazonas/noticia/2015/03/em-2-anos-mais-de-40-mil-casos-de-desvio-de-agua-sao-registrados-no-am.html", news[39].Address);
		}

	}
}

