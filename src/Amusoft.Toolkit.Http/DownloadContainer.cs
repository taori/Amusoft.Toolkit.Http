using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.Toolkit.Http.Delegates;

namespace Amusoft.Toolkit.Http;

internal class DownloadContainer
{
	private readonly HttpClient _httpClient;
	private readonly string _destinationFilePath;
	private readonly Func<HttpRequestMessage> _requestMessageBuilder;
	private readonly int _bufferSize;

	public event DownloadProgressHandler? ProgressChanged;

	internal DownloadContainer(HttpClient httpClient, string destinationFilePath, Func<HttpRequestMessage> requestMessageBuilder, int bufferSize = 8192)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		_destinationFilePath = destinationFilePath ?? throw new ArgumentNullException(nameof(destinationFilePath));
		_requestMessageBuilder = requestMessageBuilder ?? throw new ArgumentNullException(nameof(requestMessageBuilder));
		_bufferSize = bufferSize <= 0 ? throw new ArgumentException("bufferSize must be greater than 0", nameof(bufferSize)) : bufferSize;
	}

	public async Task StartDownload(CancellationToken cancellationToken)
	{
		using var requestMessage = _requestMessageBuilder.Invoke();
		using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
		await DownloadAsync(response, cancellationToken).ConfigureAwait(false);
	}

	private async Task DownloadAsync(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		response.EnsureSuccessStatusCode();

		var totalBytes = response.Content.Headers.ContentLength;
		if (totalBytes is null)
			throw new NotSupportedException("Headers does not contain information about ContentLength and therefore does not support downloading with progress");
		if (totalBytes.Value <= 0)
			throw new NotSupportedException("ContentLength <= 0 is not supported for downloads with progress.");

#if NET5_0_OR_GREATER
		using (var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken))
#else
		using (var contentStream = await response.Content.ReadAsStreamAsync())
#endif
			await ProcessContentStream(totalBytes.Value, contentStream, cancellationToken).ConfigureAwait(false);
	}

	private async Task ProcessContentStream(long totalDownloadSize, Stream contentStream, CancellationToken cancellationToken)
	{
		var totalBytesRead = 0L;
		var readCount = 0L;
		var buffer = ArrayPool<byte>.Shared.Rent(_bufferSize);
		var isMoreToRead = true;

		using (var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, _bufferSize, true))
		{
			do
			{
				var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
				if (bytesRead == 0)
				{
					isMoreToRead = false;
					ReportProgress(totalDownloadSize, totalBytesRead);
					continue;
				}

				await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);

				totalBytesRead += bytesRead;
				readCount += 1;

				if (readCount % 100 == 0)
					ReportProgress(totalDownloadSize, totalBytesRead);
			}
			while (isMoreToRead);
		}

		ArrayPool<byte>.Shared.Return(buffer);
	}

	internal void ReportProgress(long totalDownloadSize, long totalBytesRead)
	{
		var progress = (double)totalBytesRead / totalDownloadSize;

		ProgressChanged?.Invoke(new DownloadProgressEventArgs()
		{
			Downloaded = totalBytesRead,
			ProgressMax100 = Math.Round(progress * 100, 2),
			Progress = Math.Round(progress, 2),
			TotalFileSize = totalDownloadSize,
		});
	}
}