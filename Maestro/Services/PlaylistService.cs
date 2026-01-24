using System;
using System.Collections.Generic;
using Maestro.Models;

namespace Maestro.Services
{
	public class PlaylistService
	{
		private readonly List<Song> _queue = new List<Song>();

		public IReadOnlyList<Song> Queue => _queue;

		public int Count => _queue.Count;

		public bool HasItems => _queue.Count > 0;

		public event EventHandler QueueChanged;

		public void Add(Song song)
		{
			if (song != null)
			{
				_queue.Add(song);
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Remove(Song song)
		{
			if (_queue.Remove(song))
			{
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < _queue.Count)
			{
				_queue.RemoveAt(index);
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Move(int fromIndex, int toIndex)
		{
			if (fromIndex >= 0 && fromIndex < _queue.Count && toIndex >= 0 && toIndex < _queue.Count && fromIndex != toIndex)
			{
				Song item = _queue[fromIndex];
				_queue.RemoveAt(fromIndex);
				_queue.Insert(toIndex, item);
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			if (_queue.Count > 0)
			{
				_queue.Clear();
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public Song Dequeue()
		{
			if (_queue.Count == 0)
			{
				return null;
			}
			Song result = _queue[0];
			_queue.RemoveAt(0);
			EventHandler queueChanged = this.QueueChanged;
			if (queueChanged != null)
			{
				queueChanged(this, EventArgs.Empty);
				return result;
			}
			return result;
		}

		public Song Peek()
		{
			if (_queue.Count <= 0)
			{
				return null;
			}
			return _queue[0];
		}

		public int IndexOf(Song song)
		{
			return _queue.IndexOf(song);
		}
	}
}
