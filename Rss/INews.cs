using System;
using RssExtractor.Util;

namespace RssExtractor.Rss {

	public interface INews {

		string Address { get; }

		DataContainer Data { get; }

		string Content { get; set; }

	}
}

