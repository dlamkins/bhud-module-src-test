using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal sealed class MessageBuffer : IDisposable
	{
		private sealed class LinkedBuffer
		{
			private struct Enumerable : IEnumerable<(ReadOnlyMemory<byte>, long)>, IEnumerable
			{
				private readonly LinkedBuffer _linkedBuffer;

				public Enumerable(LinkedBuffer linkedBuffer)
				{
					_linkedBuffer = linkedBuffer;
				}

				public IEnumerator<(ReadOnlyMemory<byte>, long)> GetEnumerator()
				{
					return new Enumerator(_linkedBuffer);
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					throw new NotImplementedException();
				}
			}

			private struct Enumerator : IEnumerator<(ReadOnlyMemory<byte>, long)>, IDisposable, IEnumerator
			{
				private LinkedBuffer _linkedBuffer;

				private int _index;

				public (ReadOnlyMemory<byte>, long) Current
				{
					get
					{
						if (_linkedBuffer == null)
						{
							return (null, long.MinValue);
						}
						int num = _index - 1;
						int num2 = _linkedBuffer._ackedIndex + 1;
						if (num2 + num < 10)
						{
							return (_linkedBuffer._messages[num2 + num], _linkedBuffer._startingSequenceId + num2 + num);
						}
						return (null, long.MinValue);
					}
				}

				object IEnumerator.Current
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				public Enumerator(LinkedBuffer linkedBuffer)
				{
					_index = 0;
					_linkedBuffer = linkedBuffer;
				}

				public void Dispose()
				{
					_linkedBuffer = null;
				}

				public bool MoveNext()
				{
					if (_linkedBuffer == null)
					{
						return false;
					}
					int num = _linkedBuffer._ackedIndex + 1;
					if (num + _index >= 10)
					{
						_linkedBuffer = _linkedBuffer._next;
						_index = 1;
					}
					else if (_linkedBuffer._messages[num + _index].Length == 0)
					{
						_linkedBuffer = null;
					}
					else
					{
						_index++;
					}
					return _linkedBuffer != null;
				}

				public void Reset()
				{
					throw new NotImplementedException();
				}
			}

			private const int BufferLength = 10;

			private int _currentIndex = -1;

			private int _ackedIndex = -1;

			private long _startingSequenceId = long.MinValue;

			private LinkedBuffer _next;

			private readonly ReadOnlyMemory<byte>[] _messages = new ReadOnlyMemory<byte>[10];

			public void AddMessage(ReadOnlyMemory<byte> hubMessage, long sequenceId, Stack<LinkedBuffer> pool)
			{
				if (_startingSequenceId < 0)
				{
					_startingSequenceId = sequenceId;
				}
				if (_currentIndex < 9)
				{
					_currentIndex++;
					_messages[_currentIndex] = hubMessage;
				}
				else if (_next == null)
				{
					if (pool.Count != 0)
					{
						_next = pool.Pop();
					}
					else
					{
						_next = new LinkedBuffer();
					}
					_next.AddMessage(hubMessage, sequenceId, pool);
				}
				else
				{
					LinkedBuffer next = _next;
					while (next._next != null)
					{
						next = next._next;
					}
					next.AddMessage(hubMessage, sequenceId, pool);
				}
			}

			public (LinkedBuffer Buffer, int ReturnCredit) RemoveMessages(long sequenceId, Stack<LinkedBuffer> pool)
			{
				return RemoveMessagesCore(this, sequenceId, pool);
			}

			private static (LinkedBuffer Buffer, int ReturnCredit) RemoveMessagesCore(LinkedBuffer linkedBuffer, long sequenceId, Stack<LinkedBuffer> pool)
			{
				int num = 0;
				while (linkedBuffer._startingSequenceId <= sequenceId)
				{
					int num2 = (int)Math.Min(10L, Math.Max(1L, sequenceId - (linkedBuffer._startingSequenceId - 1)));
					for (int i = 0; i < num2; i++)
					{
						num += linkedBuffer._messages[i].Length;
						linkedBuffer._messages[i] = null;
					}
					linkedBuffer._ackedIndex = num2 - 1;
					if (num2 == 10)
					{
						if (linkedBuffer._next == null)
						{
							linkedBuffer.Reset();
							return (linkedBuffer, num);
						}
						LinkedBuffer linkedBuffer2 = linkedBuffer;
						linkedBuffer = linkedBuffer._next;
						linkedBuffer2.Reset();
						if (pool.Count < 10)
						{
							pool.Push(linkedBuffer2);
						}
						continue;
					}
					return (linkedBuffer, num);
				}
				return (linkedBuffer, num);
			}

			private void Reset()
			{
				_startingSequenceId = long.MinValue;
				_currentIndex = -1;
				_ackedIndex = -1;
				_next = null;
				Array.Clear(_messages, 0, 10);
			}

			public IEnumerable<(ReadOnlyMemory<byte> HubMessage, long SequenceId)> GetMessages()
			{
				return new Enumerable(this);
			}
		}

		private static readonly TaskCompletionSource<FlushResult> _completedTCS;

		private const int PoolLimit = 10;

		private readonly IHubProtocol _protocol;

		private readonly long _bufferLimit;

		private readonly ILogger _logger;

		private readonly AckMessage _ackMessage = new AckMessage(0L);

		private readonly SequenceMessage _sequenceMessage = new SequenceMessage(0L);

		private readonly Channel<long> _waitForAck = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
		{
			FullMode = BoundedChannelFullMode.DropOldest
		});

		private readonly TimerAwaitable _timer = new TimerAwaitable(AckRate, AckRate);

		private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);

		private PipeWriter _writer;

		private long _totalMessageCount;

		private long _currentReceivingSequenceId = 1L;

		private long _latestReceivedSequenceId = long.MinValue;

		private long _lastAckedId = long.MinValue;

		private TaskCompletionSource<FlushResult> _resend = _completedTCS;

		private readonly Stack<LinkedBuffer> _pool = new Stack<LinkedBuffer>();

		private LinkedBuffer _buffer;

		private long _bufferedByteCount;

		public static TimeSpan AckRate => TimeSpan.FromSeconds(1.0);

		static MessageBuffer()
		{
			_completedTCS = new TaskCompletionSource<FlushResult>();
			_completedTCS.SetResult(default(FlushResult));
		}

		public MessageBuffer(ConnectionContext connection, IHubProtocol protocol, long bufferLimit, ILogger logger)
			: this(connection, protocol, bufferLimit, logger, TimeProvider.System)
		{
		}

		public MessageBuffer(ConnectionContext connection, IHubProtocol protocol, long bufferLimit, ILogger logger, TimeProvider timeProvider)
		{
			_buffer = new LinkedBuffer();
			_writer = connection.Transport.Output;
			_protocol = protocol;
			_bufferLimit = bufferLimit;
			_logger = logger;
			_timer.Start();
			RunTimer();
		}

		private async Task RunTimer()
		{
			using (_timer)
			{
				while (await _timer)
				{
					if (_lastAckedId < _latestReceivedSequenceId)
					{
						long sequenceId = _latestReceivedSequenceId;
						_ackMessage.SequenceId = sequenceId;
						await _writeLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
						try
						{
							_protocol.WriteMessage(_ackMessage, _writer);
							await _writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
							_lastAckedId = sequenceId;
						}
						finally
						{
							_writeLock.Release();
						}
					}
				}
			}
		}

		public ValueTask<FlushResult> WriteAsync(SerializedHubMessage hubMessage, CancellationToken cancellationToken)
		{
			return WriteAsyncCore(hubMessage.Message, hubMessage.GetSerializedMessage(_protocol), cancellationToken);
		}

		public ValueTask<FlushResult> WriteAsync(HubMessage hubMessage, CancellationToken cancellationToken)
		{
			return WriteAsyncCore(hubMessage, _protocol.GetMessageBytes(hubMessage), cancellationToken);
		}

		private async ValueTask<FlushResult> WriteAsyncCore(HubMessage hubMessage, ReadOnlyMemory<byte> messageBytes, CancellationToken cancellationToken)
		{
			if (_bufferedByteCount > _bufferLimit)
			{
				long item;
				while (await _waitForAck.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false) && (!_waitForAck.Reader.TryRead(out item) || item >= _bufferLimit))
				{
				}
			}
			_waitForAck.Reader.TryRead(out var _);
			await _resend.Task.ConfigureAwait(continueOnCapturedContext: false);
			await _writeLock.WaitAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
			ValueTask<FlushResult> valueTask;
			try
			{
				if (hubMessage is HubInvocationMessage)
				{
					_totalMessageCount++;
					_bufferedByteCount += messageBytes.Length;
					_buffer.AddMessage(messageBytes, _totalMessageCount, _pool);
					valueTask = _writer.WriteAsync(messageBytes, cancellationToken);
				}
				else
				{
					valueTask = _writer.WriteAsync(messageBytes, cancellationToken);
				}
			}
			finally
			{
				_writeLock.Release();
			}
			return await valueTask.ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task AckAsync(AckMessage ackMessage)
		{
			long num = -1L;
			await _writeLock.WaitAsync(default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				(LinkedBuffer, int) tuple = _buffer.RemoveMessages(ackMessage.SequenceId, _pool);
				(_buffer, _) = tuple;
				_bufferedByteCount -= tuple.Item2;
				num = _bufferedByteCount;
			}
			finally
			{
				_writeLock.Release();
			}
			if (num >= 0)
			{
				_waitForAck.Writer.TryWrite(num);
			}
		}

		internal bool ShouldProcessMessage(HubMessage message)
		{
			SequenceMessage sequenceMessage = message as SequenceMessage;
			if (sequenceMessage != null)
			{
				if (sequenceMessage.SequenceId > _currentReceivingSequenceId)
				{
					throw new InvalidOperationException("Sequence ID greater than amount of messages we've received.");
				}
				_currentReceivingSequenceId = sequenceMessage.SequenceId;
				return true;
			}
			if (!(message is HubInvocationMessage))
			{
				return true;
			}
			long currentReceivingSequenceId = _currentReceivingSequenceId;
			_currentReceivingSequenceId++;
			if (currentReceivingSequenceId <= _latestReceivedSequenceId)
			{
				return false;
			}
			_latestReceivedSequenceId = currentReceivingSequenceId;
			return true;
		}

		internal async Task ResendAsync(PipeWriter writer)
		{
			TaskCompletionSource<FlushResult> tcs = (_resend = new TaskCompletionSource<FlushResult>(TaskCreationOptions.RunContinuationsAsynchronously));
			FlushResult finalResult = default(FlushResult);
			await _writeLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				_writer.Complete();
				_writer = writer;
				_sequenceMessage.SequenceId = _totalMessageCount + 1;
				bool isFirst = true;
				foreach (var message in _buffer.GetMessages())
				{
					if (message.SequenceId > 0)
					{
						if (isFirst)
						{
							_sequenceMessage.SequenceId = message.SequenceId;
							_protocol.WriteMessage(_sequenceMessage, _writer);
							isFirst = false;
						}
						finalResult = await _writer.WriteAsync(message.HubMessage).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				if (isFirst)
				{
					_protocol.WriteMessage(_sequenceMessage, _writer);
					finalResult = await _writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception exception)
			{
				tcs.SetException(exception);
				_ = tcs.Task.Exception;
				_logger.LogDebug(exception, "Failure while resending messages after a reconnect.");
			}
			finally
			{
				_writeLock.Release();
				tcs.TrySetResult(finalResult);
			}
		}

		public void Dispose()
		{
			((IDisposable)_timer).Dispose();
		}
	}
}
