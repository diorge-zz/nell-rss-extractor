using System;

namespace RssExtractor.Rss {

	public interface IModule {

		void Apply(INews news);

	}
}

