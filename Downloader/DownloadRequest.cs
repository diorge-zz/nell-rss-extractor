using System;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;

namespace RssExtractor.Downloader {

	/// <summary>
	/// Makes a synchronous HTTP download request
	/// </summary>
	public class DownloadRequest : IDownloadRequest {

		private const int BufferSize = 1024;

		/// <summary>
		/// Gets the download address
		/// </summary>
		public string Uri { get; private set; }

		/// <summary>
		/// Gets the content of the download, or null if there is no data available
		/// </summary>
		public string Content { get; private set; }

		/// <summary>
		/// Gets or sets the time before the download expires
		/// Default is 2 minutes
		/// </summary>
		public TimeSpan Timeout { get; set; }

		private ManualResetEvent asyncCounter;

		public DownloadRequest (string uri) {
			this.Uri = uri;
			this.Timeout = TimeSpan.FromMinutes(2);
			this.Content = null;
			this.asyncCounter = new ManualResetEvent(false);
		}

		/// <summary>
		/// Performs the download operation
		/// </summary>
		public string Download () {
			AsyncDownload();
			return Content;
		}

		private void AsyncDownload () {
			AsyncRequestState state = null;
			try {
				state = new AsyncRequestState();
				var req = GetRequest();
				state.request = req;

				// creates asynchronous request
				var result = (IAsyncResult)req.BeginGetResponse(new AsyncCallback(ResponseAsyncCallback), state);

				// creates timeout thread
				ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), req, Timeout, true);
				asyncCounter.WaitOne();

			} finally {
				if (state != null) {
					if (state.response != null) {
						state.response.Close();
					}
				}
			}
		}

		private HttpWebRequest GetRequest () {
			return (HttpWebRequest)WebRequest.Create(Uri);
		}

		private void ResponseAsyncCallback (IAsyncResult result) {
			try {
				var state = (AsyncRequestState)result.AsyncState;
				var request = state.request;
				state.response = (HttpWebResponse)request.EndGetResponse(result);
				state.streamResponse = state.response.GetResponseStream();
				state.streamResponse.BeginRead(
					state.BufferRead, 0, BufferSize, new AsyncCallback(ReadAsyncCallback), state);
				return;
			} catch {
				// TODO: logging
				// throw;
				Console.WriteLine("ERROR DOWNLOADING " + this.Uri);
			}
			asyncCounter.Set();
		}

		private void TimeoutCallback (object state, bool timedOut) {
			if (timedOut) {
				var request = state as HttpWebRequest;
				if (request != null) {
					request.Abort();
				}
			}
		}

		private void ReadAsyncCallback (IAsyncResult result) {
			var state = (AsyncRequestState)result.AsyncState;
			var stream = state.streamResponse;
			int read = stream.EndRead(result);

			try {
				if (read > 0) {
					// read at most BUFFER_SIZE bytes, then call the read async callback again to read the remaining bytes
					state.requestData.Append(Encoding.UTF8.GetString(state.BufferRead, 0, read));
					stream.BeginRead(state.BufferRead, 0, BufferSize, new AsyncCallback(ReadAsyncCallback), state);
					return;
				} else {
					Content = state.requestData.ToString();
					stream.Close();
				}
			} catch {
				// TODO: logging
				throw;
			}
			asyncCounter.Set();
		}


		private class AsyncRequestState {
			public StringBuilder requestData;
			public byte[] BufferRead;
			public HttpWebRequest request;
			public HttpWebResponse response;
			public Stream streamResponse;

			public AsyncRequestState () {
				BufferRead = new byte[BufferSize];
				requestData = new StringBuilder(String.Empty);
				request = null;
				streamResponse = null;
			}
		}
	}
}

