using System;
using System.Collections.Generic;
using System.Linq;

namespace RssExtractor.Rss {

	public class ExtractorBuilder {

		private List<IModule> modules;
		private IFeedSelector feedSelector;
		private IEnumerable<IFeed> feeds;

		public ExtractorBuilder() {
			modules = new List<IModule>();
			feedSelector = null;
			feeds = null;
		}

		public ExtractorModulesBuilder WithModules() {
			return new ExtractorModulesBuilder(this);
		}

		public ExtractorBuilder FeedSelector (IFeedSelector selector) {
			this.feedSelector = selector;
			return this;
		}

		public ExtractorBuilder Feeds (IEnumerable<IFeed> feeds) {
			this.feeds = feeds;
			return this;
		}

		public Extractor Build () {
			if (modules.Count == 0) {
				throw new InvalidOperationException("No modules added");
			}
			if (feedSelector == null) {
				throw new InvalidOperationException("No feed selector given");
			}
			if (feeds == null || feeds.Count() == 0) {
				throw new InvalidOperationException("No feeds given");
			}
			return new Extractor(modules, feeds, feedSelector);
		}

		public class ExtractorModulesBuilder {

			private ExtractorBuilder parent;

			public ExtractorModulesBuilder(ExtractorBuilder parent) {
				this.parent = parent;
			}

			public ExtractorModulesBuilder Module(IModule module) {
				parent.modules.Add(module);
				return this;
			}

			public ExtractorBuilder EndModules() {
				return parent;
			}

		}

	}
}