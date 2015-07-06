using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RssExtractor.Rss {

	public static class FeedCollection {

		public static IEnumerable<IFeed> FromFile(string fileName) {
			return File.ReadLines(fileName).Select(line => new Feed(line));
		}

	}
}

