using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using RssExtractor.Util;

namespace RssExtractor.Rss {

	public class Feed : IFeed {

		public string SourceAddress { get; private set; }

		public string FeedContent { get; private set; }

		public DataContainer Data { get; private set; }

		public Feed (string sourceAddress) {
			this.SourceAddress = sourceAddress;
			this.FeedContent = null;
			this.Data = new DataContainer();
		}

		public IEnumerable<INews> GetNews () {
			return XElement.Parse(FeedContent).Elements().Elements("item").Select(item => new News(item.Element("link").Value));
		}


		public void SetContent (string xmlContent) {
			FeedContent = xmlContent;
		}		

		public string GetUri () {
			return SourceAddress;
		}



	}
}

