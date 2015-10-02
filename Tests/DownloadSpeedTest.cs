using System;
using System.IO;
using System.Reflection;
using RssExtractor.Util;
using RssExtractor.Rss;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using RssExtractor.Downloader;
using System.Threading.Tasks;

namespace RssExtractor.Tests {
	public class DownloadSpeedTest {

		private class TestCase {
			public string TestName { get; set; }

			public Action<IEnumerable<string>> DownloadFunction { get; set; }

			public TimeSpan Time(IEnumerable<string> newsAddresses) {
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				DownloadFunction(newsAddresses);
				stopwatch.Stop();
				return stopwatch.Elapsed;
			}
		}

		public static void Main() {
			var test = new DownloadSpeedTest();

			var cases = new [] {
				new TestCase {
					TestName = "Download Client",
					DownloadFunction = addrs =>
						Parallel.ForEach(addrs,
							addr => new DownloadClient(addr)
						                 .Download())
				},
				new TestCase {
					TestName = "Download Request",
					DownloadFunction = addrs =>
						Parallel.ForEach(addrs,
							addr => new DownloadRequest(addr)
						                 .Download())
				},
				new TestCase {
					TestName = "Async Download Request",
					DownloadFunction = addrs => {
						var reqs = addrs.Select(addr => new AsyncDownloadRequest(addr));
						foreach(var req in reqs) {
							req.BeginDownloadingData();
						}
						foreach(var req in reqs) {
							req.WaitUntilDownloadFinishes();
						}
					}
				}
			};
			
			var assembly = Assembly.GetExecutingAssembly();
			var wellFormedRss = assembly.GetEmbeddedFileText("RssExtractor.Tests", "TestData/WellFormedRss.xml");
			var feed = new Feed("");
			feed.SetContent(wellFormedRss);
			var news = feed.GetNews().ToArray();

			test.Run(cases, news.Select(n => n.Address).ToArray());
		}

		private void Run(IEnumerable<TestCase> testSuite, IEnumerable<string> addresses) {
			foreach(var testCase in testSuite) {
				var elapsed = testCase.Time(addresses);
				Console.WriteLine("{0}: {1}ms", testCase.TestName, elapsed.TotalMilliseconds);
			}
		}
	}
}

