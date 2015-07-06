using System;
using System.Collections.Generic;

namespace RssExtractor.Util {

	public sealed class DataContainer : IEnumerable<string> {

		private Dictionary<string, object> data;

		public DataContainer() {
			data = new Dictionary<string, object>();
		}

		public void Set<T> (string key, T value) {
			if (value == null && data.ContainsKey(key)) {
				data.Remove(key);
			} else {
				data[key] = value;
			}
		}

		public T Get<T>(string key) {
			return (T)data[key];
		}

		public T GetOrDefault<T> (string key) {
			return Exists(key) ? Get<T>(key) : default(T);
		}

		public bool Exists(string key) {
			return data.ContainsKey(key);
		}


		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return data.Keys.GetEnumerator();
		}

		IEnumerator<string> IEnumerable<string>.GetEnumerator () {
			return data.Keys.GetEnumerator();
		}


	}
}

