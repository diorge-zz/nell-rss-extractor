using System;
using System.Collections.Generic;
using System.Linq;
using RssExtractor.Downloader;

namespace RssExtractor.Rss {

	public class Extractor {

		private IModule[] modules;
		private IFeedSelector feedSelector;

		public Extractor (IEnumerable<IModule> modules, IFeedSelector feedSelector) {
			this.modules = modules.ToArray();
			this.feedSelector = feedSelector;
		}

		public void Run () {
			foreach (var feed in feedSelector.GetFeedStream()) {

				var req = new DownloadRequest(feed.GetUri());
				feed.SetContent(req.Download());

				foreach (var news in feed.GetNews()) {
					foreach (var module in modules) {
						module.Apply(news);
					}
				}
			}
		}

	}
}

