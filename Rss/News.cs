using System;
using RssExtractor.Downloader;
using RssExtractor.Util;

namespace RssExtractor.Rss {

	public class News : INews {

		public string Address { get; private set; }

		public DataContainer Data { get; private set; }

		public string Content { get; set; }

		public News (string address) {
			this.Address = address;
			this.Data = new DataContainer();
			this.Content = null;
		}


	}
}

