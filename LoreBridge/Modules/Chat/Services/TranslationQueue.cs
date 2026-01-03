using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LoreBridge.Modules.Chat.Models;
using LoreBridge.Services;

namespace LoreBridge.Modules.Chat.Services
{
	public class TranslationQueue
	{
		[CompilerGenerated]
		private Messages _003Cmessages_003EP;

		private readonly ConcurrentQueue<Message> _taskQueue;

		private readonly object _processingLock;

		private bool _isProcessing;

		public TranslationQueue(Messages messages)
		{
			_003Cmessages_003EP = messages;
			_taskQueue = new ConcurrentQueue<Message>();
			_processingLock = new object();
			base._002Ector();
		}

		public void Add(Message message)
		{
			_taskQueue.Enqueue(message);
			lock (_processingLock)
			{
				if (!_isProcessing)
				{
					_isProcessing = true;
					ProcessQueueAsync();
				}
			}
		}

		private async Task ProcessQueueAsync()
		{
			while (_isProcessing)
			{
				await Task.Delay(100);
				List<Message> tasksToProcess = new List<Message>();
				Message message;
				while (_taskQueue.TryDequeue(out message))
				{
					tasksToProcess.Add(message);
				}
				if (tasksToProcess.Count > 0)
				{
					tasksToProcess.Sort((Message x, Message y) => x.TimeStamp.CompareTo(y.TimeStamp));
					foreach (Message message2 in tasksToProcess)
					{
						await ProcessTranslationAsync(message2).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				lock (_processingLock)
				{
					if (!_taskQueue.IsEmpty)
					{
						continue;
					}
					_isProcessing = false;
					return;
				}
			}
		}

		private async Task ProcessTranslationAsync(Message message)
		{
			try
			{
				string translation = await Service.Translation.TranslateAsync(message.Text).ConfigureAwait(continueOnCapturedContext: false);
				if (!string.IsNullOrWhiteSpace(translation))
				{
					message.Text = translation;
					_003Cmessages_003EP.Add(message);
				}
			}
			catch (Exception e)
			{
				_003Cmessages_003EP.Add(new Message
				{
					Text = e.Message
				});
			}
		}
	}
}
