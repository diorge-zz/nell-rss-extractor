using System;

namespace RssExtractor.Util {

	public static class MathExtension {

		public static int Clamp (this int value, int minimum, int maximum) {
			if (value <= minimum)
				return minimum;
			if (value >= maximum)
				return maximum;
			return value;
		}

	}
}

