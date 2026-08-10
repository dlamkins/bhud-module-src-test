using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal static class SendUtils
	{
		private sealed class ReadOnlySequenceContent : HttpContent
		{
			private readonly ReadOnlySequence<byte> _buffer;

			public ReadOnlySequenceContent(in ReadOnlySequence<byte> buffer)
			{
				_buffer = buffer;
			}

			protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
			{
				return stream.WriteAsync(_buffer).AsTask();
			}

			protected override bool TryComputeLength(out long length)
			{
				length = _buffer.Length;
				return true;
			}
		}

		private static class Log
		{
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendStartedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(100, "SendStarted"), "Starting the send loop.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendCanceledCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(102, "SendCanceled"), "Send loop canceled.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SendStoppedCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(101, "SendStopped"), "Send loop stopped.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, long, Uri, Exception> __SendingMessagesCallback = LoggerMessage.Define<long, Uri>(LogLevel.Debug, new EventId(103, "SendingMessages"), "Sending {Count} bytes to the server using url: {Url}.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __SentSuccessfullyCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(104, "SentSuccessfully"), "Message(s) sent successfully.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Exception> __NoMessagesCallback = LoggerMessage.Define(LogLevel.Debug, new EventId(105, "NoMessages"), "No messages in batch to send.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			private static readonly Action<ILogger, Uri, Exception> __ErrorSendingCallback = LoggerMessage.Define<Uri>(LogLevel.Error, new EventId(106, "ErrorSending"), "Error while sending to '{Url}'.", new LogDefineOptions
			{
				SkipEnabledCheck = true
			});

			[LoggerMessage(100, LogLevel.Debug, "Starting the send loop.", EventName = "SendStarted")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendStarted(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendStartedCallback(logger, null);
				}
			}

			[LoggerMessage(102, LogLevel.Debug, "Send loop canceled.", EventName = "SendCanceled")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendCanceled(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendCanceledCallback(logger, null);
				}
			}

			[LoggerMessage(101, LogLevel.Debug, "Send loop stopped.", EventName = "SendStopped")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendStopped(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendStoppedCallback(logger, null);
				}
			}

			[LoggerMessage(103, LogLevel.Debug, "Sending {Count} bytes to the server using url: {Url}.", EventName = "SendingMessages")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SendingMessages(ILogger logger, long count, Uri url)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SendingMessagesCallback(logger, count, url, null);
				}
			}

			[LoggerMessage(104, LogLevel.Debug, "Message(s) sent successfully.", EventName = "SentSuccessfully")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void SentSuccessfully(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__SentSuccessfullyCallback(logger, null);
				}
			}

			[LoggerMessage(105, LogLevel.Debug, "No messages in batch to send.", EventName = "NoMessages")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void NoMessages(ILogger logger)
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					__NoMessagesCallback(logger, null);
				}
			}

			[LoggerMessage(106, LogLevel.Error, "Error while sending to '{Url}'.", EventName = "ErrorSending")]
			[GeneratedCode("Microsoft.Extensions.Logging.Generators", "8.0.10.46610")]
			public static void ErrorSending(ILogger logger, Uri url, Exception exception)
			{
				if (logger.IsEnabled(LogLevel.Error))
				{
					__ErrorSendingCallback(logger, url, exception);
				}
			}
		}

		public static async Task SendMessages(Uri sendUrl, IDuplexPipe application, HttpClient httpClient, ILogger logger, CancellationToken cancellationToken = default(CancellationToken))
		{
			Log.SendStarted(logger);
			try
			{
				while (true)
				{
					ReadResult readResult = await application.Input.ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					ReadOnlySequence<byte> buffer = readResult.Buffer;
					try
					{
						if (readResult.IsCanceled)
						{
							Log.SendCanceled(logger);
							break;
						}
						if (!buffer.IsEmpty)
						{
							Log.SendingMessages(logger, buffer.Length, sendUrl);
							HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, sendUrl);
							httpRequestMessage.Content = new ReadOnlySequenceContent(in buffer);
							using (HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
							{
								httpResponseMessage.EnsureSuccessStatusCode();
							}
							Log.SentSuccessfully(logger);
							continue;
						}
						if (readResult.IsCompleted)
						{
							break;
						}
						Log.NoMessages(logger);
						continue;
					}
					finally
					{
						application.Input.AdvanceTo(buffer.End);
					}
				}
			}
			catch (OperationCanceledException)
			{
				Log.SendCanceled(logger);
			}
			catch (Exception exception)
			{
				Log.ErrorSending(logger, sendUrl, exception);
				throw;
			}
			finally
			{
				application.Input.Complete();
			}
			Log.SendStopped(logger);
		}
	}
}
