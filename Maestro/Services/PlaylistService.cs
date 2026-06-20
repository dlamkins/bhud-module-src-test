using System;
using System.Collections.Generic;
using Maestro.Models;

namespace Maestro.Services
{
	public class PlaylistService
	{
		private readonly List<Song> _queue = new List<Song>();

		private readonly Random _random = new Random();

		private List<Song> _shuffle;

		private RepeatMode _repeat;

		private bool _shuffleOn;

		private int _index = -1;

		public IReadOnlyList<Song> Queue => _queue;

		public int Count => _queue.Count;

		public bool HasItems => _queue.Count > 0;

		private List<Song> Active
		{
			get
			{
				if (!_shuffleOn)
				{
					return _queue;
				}
				return _shuffle;
			}
		}

		public Song Current
		{
			get
			{
				if (_index < 0 || _index >= Active.Count)
				{
					return null;
				}
				return Active[_index];
			}
		}

		public int CurrentIndex
		{
			get
			{
				Song cur = Current;
				if (cur == null)
				{
					return -1;
				}
				return _queue.IndexOf(cur);
			}
		}

		public RepeatMode Repeat
		{
			get
			{
				return _repeat;
			}
			set
			{
				if (_repeat != value)
				{
					_repeat = value;
					this.RepeatModeChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public bool Shuffle
		{
			get
			{
				return _shuffleOn;
			}
			set
			{
				if (_shuffleOn != value)
				{
					Song cur = Current;
					_shuffleOn = value;
					if (_shuffleOn)
					{
						BuildShuffle(cur);
					}
					else
					{
						_shuffle = null;
						_index = ((cur != null) ? _queue.IndexOf(cur) : (-1));
					}
					this.ShuffleChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler QueueChanged;

		public event EventHandler CurrentChanged;

		public event EventHandler RepeatModeChanged;

		public event EventHandler ShuffleChanged;

		public void Add(Song song)
		{
			if (song != null)
			{
				_queue.Add(song);
				if (_shuffleOn)
				{
					_shuffle.Add(song);
				}
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Remove(Song song)
		{
			int visibleIndex = _queue.IndexOf(song);
			if (visibleIndex >= 0)
			{
				RemoveAt(visibleIndex);
			}
		}

		public void RemoveAt(int visibleIndex)
		{
			if (visibleIndex >= 0 && visibleIndex < _queue.Count)
			{
				Song song = _queue[visibleIndex];
				bool num = Current == song;
				int activeIndex = Active.IndexOf(song);
				_queue.RemoveAt(visibleIndex);
				if (_shuffleOn)
				{
					_shuffle.Remove(song);
				}
				if (activeIndex >= 0 && activeIndex <= _index)
				{
					_index--;
				}
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
				if (num)
				{
					this.CurrentChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public void Move(int fromIndex, int toIndex)
		{
			if (fromIndex >= 0 && fromIndex < _queue.Count && toIndex >= 0 && toIndex < _queue.Count && fromIndex != toIndex)
			{
				Song cur = Current;
				Song item = _queue[fromIndex];
				_queue.RemoveAt(fromIndex);
				_queue.Insert(toIndex, item);
				if (!_shuffleOn && cur != null)
				{
					_index = _queue.IndexOf(cur);
				}
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			if (_queue.Count != 0)
			{
				_queue.Clear();
				_shuffle?.Clear();
				_index = -1;
				this.QueueChanged?.Invoke(this, EventArgs.Empty);
				this.CurrentChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public int IndexOf(Song song)
		{
			return _queue.IndexOf(song);
		}

		public void StartPlayback()
		{
			if (_shuffleOn)
			{
				BuildShuffle(null);
			}
			_index = ((_queue.Count <= 0) ? (-1) : 0);
			this.CurrentChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool MoveNext()
		{
			if (_queue.Count == 0)
			{
				_index = -1;
				return false;
			}
			if (_repeat == RepeatMode.One && Current != null)
			{
				return true;
			}
			int next = _index + 1;
			if (next >= Active.Count)
			{
				if (_repeat != RepeatMode.All)
				{
					_index = Active.Count;
					this.CurrentChanged?.Invoke(this, EventArgs.Empty);
					return false;
				}
				if (_shuffleOn)
				{
					BuildShuffle(null);
				}
				next = 0;
			}
			_index = next;
			this.CurrentChanged?.Invoke(this, EventArgs.Empty);
			return true;
		}

		private void BuildShuffle(Song first)
		{
			_shuffle = new List<Song>(_queue);
			for (int i = _shuffle.Count - 1; i > 0; i--)
			{
				int j = _random.Next(i + 1);
				Song tmp = _shuffle[i];
				_shuffle[i] = _shuffle[j];
				_shuffle[j] = tmp;
			}
			if (first != null && _shuffle.Remove(first))
			{
				_shuffle.Insert(0, first);
				_index = 0;
			}
		}
	}
}
