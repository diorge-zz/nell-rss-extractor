using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace RssExtractor.Downloader {

	/// <summary>
	/// Makes a HTTP request asynchronously
	/// </summary>
	/// <remarks>
	/// Avoid using. Using Parallel.For in a synchronous download provides a better partitioning than creating a thread for each download operation.
	/// </remarks>
	public sealed class AsyncDownloadRequest : IAsyncDownloadRequest {

		private const int BUFFER_SIZE = 1024;

		/// <summary>
		/// Gets the download address
		/// </summary>
		public string Uri { get; private set; }

		/// <summary>
		/// Gets or sets the timeout
		/// Default is 2 minutes
		/// </summary>
		public TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets a value indicating whether the download is complete
		/// </summary>
		/// <value>
		/// <c>true</c> if the download has already been completed (and there's no ongoing download); otherwise, <c>false</c>.
		/// </value>
		public bool IsComplete { get; private set; }

		/// <summary>
		/// Occurs when completed
		/// </summary>
		public event EventHandler<DownloadCompleteEventArgs> Completed;

		/// <summary>
		/// Occurs when aborted
		/// </summary>
		public event EventHandler<DownloadAbortedEventArgs> Aborted;

		public string Content {
			get {
				if (!IsComplete)
					throw new InvalidOperationException("The content is not yet available");
				return contentData;
			}
		}

		private string contentData;
		private Thread runningThread;
		private object lockObj;
		private ManualResetEvent asyncCounter;
		private Exception threadException;

		public AsyncDownloadRequest (string uri) {
			this.Uri = uri;
			this.Timeout = TimeSpan.FromMinutes(2);
			this.lockObj = new object();
			this.asyncCounter = new ManualResetEvent(false);
			this.IsComplete = false;
		}

		/// <summary>
		/// Begins the download
		/// </summary>
		/// <exception cref="InvalidOperationException">If there is already a download running</exception>
		public void BeginDownloadingData () {
			lock (lockObj) {
				if (runningThread == null || !runningThread.IsAlive) {
					runningThread = new Thread(AsyncDownloadAndRaise);
					threadException = null;
					IsComplete = false;
					runningThread.Start();
				} else {
					throw new InvalidOperationException("There is already a download thread running");
				}
			}
		}

		/// <summary>
		/// Waits the until download finishes.
		/// This is done by waiting the current running thread. No exception is thrown if there is no running thread
		/// </summary>
		public void WaitUntilDownloadFinishes () {
			lock (lockObj) {
				if (runningThread != null && runningThread.IsAlive) {
					runningThread.Join();
				}
			}
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

			} catch (Exception ex) {
				threadException = ex;
			} finally {
				if (state != null) {
					if (state.response != null) {
						state.response.Close();
					}
				}
			}
		}

		private void RaiseCompleted () {
			IsComplete = true;
			if (Completed != null) {
				Completed(this, new DownloadCompleteEventArgs(Uri, Content));
			}
		}

		private void RaiseAborted () {
			if (Aborted != null) {
				Aborted(this, new DownloadAbortedEventArgs(Uri, threadException));
			}
		}

		private void AsyncDownloadAndRaise () {
			AsyncDownload();
			if (threadException == null) {
				RaiseCompleted();
			} else {
				RaiseAborted();
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
					state.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadAsyncCallback), state);
				return;
			} catch (Exception ex) {
				threadException = ex;
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
					stream.BeginRead(state.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadAsyncCallback), state);
					return;
				} else {
					contentData = state.requestData.ToString();
					stream.Close();
				}
			} catch (Exception ex) {
				threadException = ex;
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
				BufferRead = new byte[BUFFER_SIZE];
				requestData = new StringBuilder(String.Empty);
				request = null;
				streamResponse = null;
			}
		}
	}
}

