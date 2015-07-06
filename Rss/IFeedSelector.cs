using System;
using System.Collections.Generic;

namespace RssExtractor.Rss {

	public interface IFeedSelector {

		IEnumerable<IFeed> GetFeedStream();

	}

}

