using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;
using SongbookOfTyria.Models;
using SongbookOfTyria.Settings;

namespace SongbookOfTyria.Services
{
	public sealed class PracticeFeedbackService : IDisposable
	{
		private class NoteFeedback
		{
			public NoteFeedbackState State { get; }

			public DateTime Timestamp { get; }

			public NoteFeedback(NoteFeedbackState state, DateTime timestamp)
			{
				State = state;
				Timestamp = timestamp;
			}
		}

		private const double HitWindowMs = 200.0;

		private const double FeedbackDisplayDurationMs = 400.0;

		private const double MaxLookBehindMs = 500.0;

		private const int OctaveDownMarker = -100;

		private const int OctaveUpMarker = -101;

		private static readonly Logger Logger = Logger.GetLogger<PracticeFeedbackService>();

		private readonly MidiPlaybackService _playbackService;

		private readonly ModuleSettings _settings;

		private MidiTrack _activeTrack;

		private bool _isEnabled;

		private bool _disposed;

		private int _currentOctaveOffset;

		private double _lastProcessedPosition = -1.0;

		private readonly Dictionary<int, NoteFeedback> _noteFeedback = new Dictionary<int, NoteFeedback>();

		private readonly HashSet<int> _hitNoteIndices = new HashSet<int>();

		private readonly object _feedbackLock = new object();

		public int CurrentOctaveOffset => _currentOctaveOffset;

		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				if (_isEnabled != value)
				{
					_isEnabled = value;
					if (_isEnabled)
					{
						SubscribeToKeyboard();
						return;
					}
					UnsubscribeFromKeyboard();
					ClearFeedback();
				}
			}
		}

		public event EventHandler<PracticeFeedbackEventArgs> FeedbackChanged;

		public PracticeFeedbackService(MidiPlaybackService playbackService, ModuleSettings settings)
		{
			_playbackService = playbackService ?? throw new ArgumentNullException("playbackService");
			_settings = settings ?? throw new ArgumentNullException("settings");
		}

		public void SetActiveTrack(MidiTrack track)
		{
			_activeTrack = track;
			_currentOctaveOffset = 0;
			ClearFeedback();
		}

		public void ClearFeedback()
		{
			lock (_feedbackLock)
			{
				_noteFeedback.Clear();
				_hitNoteIndices.Clear();
				_lastProcessedPosition = -1.0;
			}
			this.FeedbackChanged?.Invoke(this, new PracticeFeedbackEventArgs(new Dictionary<int, NoteFeedbackState>()));
		}

		public void Update()
		{
			if (!_isEnabled || _activeTrack?.Notes == null)
			{
				return;
			}
			DateTime now = DateTime.UtcNow;
			double currentPosition = _playbackService.CurrentPosition;
			bool feedbackChanged = false;
			lock (_feedbackLock)
			{
				List<int> expiredKeys = new List<int>();
				foreach (KeyValuePair<int, NoteFeedback> kvp in _noteFeedback)
				{
					if ((now - kvp.Value.Timestamp).TotalMilliseconds > 400.0)
					{
						expiredKeys.Add(kvp.Key);
					}
				}
				foreach (int key in expiredKeys)
				{
					_noteFeedback.Remove(key);
				}
				feedbackChanged = expiredKeys.Count > 0 || CheckMissedNotes(currentPosition);
			}
			if (feedbackChanged)
			{
				RaiseFeedbackChanged();
			}
		}

		private bool CheckMissedNotes(double currentPosition)
		{
			if (_activeTrack?.Notes == null)
			{
				return false;
			}
			double hitWindowSeconds = 0.2;
			double maxLookBehindSeconds = 0.5;
			if (_lastProcessedPosition < 0.0 || currentPosition < _lastProcessedPosition)
			{
				_lastProcessedPosition = currentPosition;
				return false;
			}
			double checkWindowStart = Math.Max(_lastProcessedPosition - hitWindowSeconds, currentPosition - maxLookBehindSeconds);
			bool anyMissed = false;
			for (int i = FindNoteIndexAtOrAfter(checkWindowStart - hitWindowSeconds); i < _activeTrack.Notes.Count; i++)
			{
				MidiNote midiNote = _activeTrack.Notes[i];
				double noteEndTime = midiNote.Time + hitWindowSeconds;
				if (midiNote.Time > currentPosition)
				{
					break;
				}
				if (!_hitNoteIndices.Contains(i) && !_noteFeedback.ContainsKey(i) && !(noteEndTime < checkWindowStart) && currentPosition > noteEndTime)
				{
					_noteFeedback[i] = new NoteFeedback(NoteFeedbackState.Missed, DateTime.UtcNow);
					anyMissed = true;
				}
			}
			_lastProcessedPosition = currentPosition;
			return anyMissed;
		}

		private int FindNoteIndexAtOrAfter(double time)
		{
			if (_activeTrack?.Notes == null || _activeTrack.Notes.Count == 0)
			{
				return 0;
			}
			int left = 0;
			int right = _activeTrack.Notes.Count - 1;
			while (left < right)
			{
				int mid = left + (right - left) / 2;
				if (_activeTrack.Notes[mid].Time < time)
				{
					left = mid + 1;
				}
				else
				{
					right = mid;
				}
			}
			return left;
		}

		private void SubscribeToKeyboard()
		{
			InputService input = GameService.Input;
			if (((input != null) ? input.get_Keyboard() : null) != null)
			{
				GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			}
		}

		private void UnsubscribeFromKeyboard()
		{
			InputService input = GameService.Input;
			if (((input != null) ? input.get_Keyboard() : null) != null)
			{
				GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			}
		}

		private void OnKeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!_isEnabled || _activeTrack?.Notes == null)
			{
				return;
			}
			ModifierKeys activeModifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
			int? midiNote = _settings.GetMidiNoteFromKey(e.get_Key(), activeModifiers, _currentOctaveOffset);
			if (!midiNote.HasValue)
			{
				return;
			}
			if (midiNote.Value == -100)
			{
				_currentOctaveOffset = Math.Max(-2, _currentOctaveOffset - 1);
			}
			else if (midiNote.Value == -101)
			{
				_currentOctaveOffset = Math.Min(2, _currentOctaveOffset + 1);
			}
			else
			{
				if (!_playbackService.IsPlaying)
				{
					return;
				}
				double currentPosition = _playbackService.CurrentPosition;
				double hitWindowSeconds = 0.2;
				DateTime now = DateTime.UtcNow;
				bool feedbackChanged = false;
				lock (_feedbackLock)
				{
					for (int i = FindNoteIndexAtOrAfter(currentPosition - hitWindowSeconds); i < _activeTrack.Notes.Count; i++)
					{
						MidiNote note = _activeTrack.Notes[i];
						if (note.Time > currentPosition + hitWindowSeconds)
						{
							break;
						}
						if (!_hitNoteIndices.Contains(i) && Math.Abs(currentPosition - note.Time) <= hitWindowSeconds && note.Midi == midiNote.Value)
						{
							_hitNoteIndices.Add(i);
							_noteFeedback[i] = new NoteFeedback(NoteFeedbackState.Correct, now);
							feedbackChanged = true;
							break;
						}
					}
					if (!feedbackChanged)
					{
						int closestNoteIndex = FindClosestNoteIndex(currentPosition, hitWindowSeconds);
						if (closestNoteIndex >= 0 && !_hitNoteIndices.Contains(closestNoteIndex))
						{
							_noteFeedback[closestNoteIndex] = new NoteFeedback(NoteFeedbackState.Wrong, now);
							feedbackChanged = true;
						}
					}
				}
				if (feedbackChanged)
				{
					RaiseFeedbackChanged();
				}
			}
		}

		private int FindClosestNoteIndex(double currentPosition, double windowSeconds)
		{
			if (_activeTrack?.Notes == null)
			{
				return -1;
			}
			int closestIndex = -1;
			double closestDiff = double.MaxValue;
			for (int i = FindNoteIndexAtOrAfter(currentPosition - windowSeconds); i < _activeTrack.Notes.Count; i++)
			{
				MidiNote note = _activeTrack.Notes[i];
				if (note.Time > currentPosition + windowSeconds)
				{
					break;
				}
				if (!_hitNoteIndices.Contains(i))
				{
					double timeDiff = Math.Abs(currentPosition - note.Time);
					if (timeDiff < closestDiff)
					{
						closestDiff = timeDiff;
						closestIndex = i;
					}
				}
			}
			return closestIndex;
		}

		private void RaiseFeedbackChanged()
		{
			EventHandler<PracticeFeedbackEventArgs> handler = this.FeedbackChanged;
			if (handler == null)
			{
				return;
			}
			Dictionary<int, NoteFeedbackState> feedbackCopy = new Dictionary<int, NoteFeedbackState>();
			lock (_feedbackLock)
			{
				foreach (KeyValuePair<int, NoteFeedback> kvp in _noteFeedback)
				{
					feedbackCopy[kvp.Key] = kvp.Value.State;
				}
			}
			handler(this, new PracticeFeedbackEventArgs(feedbackCopy));
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				UnsubscribeFromKeyboard();
			}
		}
	}
}
