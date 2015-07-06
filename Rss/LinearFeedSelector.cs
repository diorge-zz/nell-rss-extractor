using System;
using System.Collections.Generic;
using System.Linq;

namespace RssExtractor.Rss {

	public class LinearFeedSelector : IFeedSelector {

		private IFeed[] feeds;

		public LinearFeedSelector (IEnumerable<IFeed> feeds) {
			this.feeds = feeds.ToArray();
		}		


		public IEnumerable<IFeed> GetFeedStream () {
			while (true) {
				foreach(var feed in feeds) {
					yield return feed;
				}
				break; // TODO: remove this line
			}
		}

	}
}

