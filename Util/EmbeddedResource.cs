using System;
using System.Reflection;
using System.IO;

namespace RssExtractor.Util {

	public static class EmbeddedResource {

		public static string GetEmbeddedFileText(this Assembly executingAssembly, string location) {
			using (var stream = executingAssembly.GetManifestResourceStream(location)) {
				using (var reader = new StreamReader(stream)) {
					return reader.ReadToEnd();
				}
			}
		}

		public static string GetEmbeddedFileText(this Assembly executingAssembly, string projectName, string fileName) {
			return executingAssembly.GetEmbeddedFileText(String.Concat(projectName, ".", fileName.Replace("/", ".")));
		}

	}
}

