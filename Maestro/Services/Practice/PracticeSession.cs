using System;
using Maestro.Models;

namespace Maestro.Services.Practice
{
	public class PracticeSession
	{
		public const int PerfectWindowMs = 50;

		public const int GoodWindowMs = 120;

		private readonly IKeySender _sender;

		private readonly bool _autoOctave;

		private readonly int _minOctave;

		private readonly int _maxOctave;

		private readonly bool[] _wasHit;

		private readonly bool[] _firedShifts;

		private int _currentOctave;

		public NoteTimeline Timeline { get; }

		public PracticeClock Clock { get; }

		public int PerfectCount { get; private set; }

		public int GoodCount { get; private set; }

		public int MissCount { get; private set; }

		public int WrongCount { get; private set; }

		public int Combo { get; private set; }

		public int MaxCombo { get; private set; }

		public bool IsCompleted { get; private set; }

		public event Action<Judgement> OnJudgement;

		public event Action<int> OnOctaveShiftFired;

		public event Action OnCountdownComplete;

		public event Action<PracticeResult> OnCompleted;

		public PracticeSession(NoteTimeline timeline, IKeySender sender, bool autoOctave, int countdownMs, int minOctave = -1, int maxOctave = 1)
		{
			Timeline = timeline;
			_sender = sender;
			_autoOctave = autoOctave;
			_minOctave = minOctave;
			_maxOctave = maxOctave;
			_wasHit = new bool[timeline.Notes.Count];
			_firedShifts = new bool[timeline.OctaveShiftPoints.Count];
			Clock = new PracticeClock();
			Clock.Seek(-countdownMs);
		}

		public void Tick(int elapsedMs)
		{
			if (IsCompleted)
			{
				return;
			}
			int currentMs = Clock.CurrentMs;
			Clock.Advance(elapsedMs);
			int after = Clock.CurrentMs;
			if (currentMs < 0 && after >= 0)
			{
				this.OnCountdownComplete?.Invoke();
			}
			if (_autoOctave && after >= 0)
			{
				for (int j = 0; j < Timeline.OctaveShiftPoints.Count; j++)
				{
					if (!_firedShifts[j])
					{
						OctaveShiftPoint point = Timeline.OctaveShiftPoints[j];
						if (point.AtMs <= after)
						{
							_firedShifts[j] = true;
							SendOctaveDelta(point.Delta);
						}
					}
				}
			}
			for (int i = 0; i < Timeline.Notes.Count; i++)
			{
				if (!_wasHit[i])
				{
					TimelineNote note = Timeline.Notes[i];
					if (note.StartMs + 120 < after)
					{
						_wasHit[i] = true;
						MissCount++;
						BreakCombo();
						this.OnJudgement?.Invoke(new Judgement
						{
							Verdict = JudgementVerdict.Miss,
							DeltaMs = after - note.StartMs,
							TimelineIndex = i,
							Lane = note.Lane
						});
					}
				}
			}
			if (!IsCompleted && after >= Timeline.TotalDurationMs + 120)
			{
				IsCompleted = true;
				this.OnCompleted?.Invoke(BuildResult());
			}
		}

		public void OnPlayerNotePressed(int lane, bool isSharp, int nowMs)
		{
			if (IsCompleted || nowMs < 0 || lane < 1 || lane > 8)
			{
				return;
			}
			Judgement? result = HitGrader.Grade(lane, isSharp, nowMs, Timeline, _wasHit, 50, 120);
			if (!result.HasValue)
			{
				WrongCount++;
				BreakCombo();
				this.OnJudgement?.Invoke(new Judgement
				{
					Verdict = JudgementVerdict.Wrong,
					DeltaMs = 0,
					TimelineIndex = -1,
					Lane = lane
				});
				return;
			}
			Judgement i = result.Value;
			_wasHit[i.TimelineIndex] = true;
			if (i.Verdict == JudgementVerdict.Perfect)
			{
				PerfectCount++;
			}
			else if (i.Verdict == JudgementVerdict.Good)
			{
				GoodCount++;
			}
			Combo++;
			if (Combo > MaxCombo)
			{
				MaxCombo = Combo;
			}
			this.OnJudgement?.Invoke(i);
		}

		public void Restart(int countdownMs)
		{
			for (int j = 0; j < _wasHit.Length; j++)
			{
				_wasHit[j] = false;
			}
			for (int i = 0; i < _firedShifts.Length; i++)
			{
				_firedShifts[i] = false;
			}
			int num2 = (WrongCount = 0);
			int num4 = (MissCount = num2);
			int num7 = (PerfectCount = (GoodCount = num4));
			num7 = (Combo = (MaxCombo = 0));
			IsCompleted = false;
			_currentOctave = 0;
			Clock.Seek(-countdownMs);
		}

		public void LoopSeek(int fromMs, int toMs)
		{
			for (int i = 0; i < Timeline.Notes.Count; i++)
			{
				TimelineNote j = Timeline.Notes[i];
				if (j.StartMs >= fromMs && j.StartMs <= toMs)
				{
					_wasHit[i] = false;
				}
			}
			SyncOctaveForSeek(fromMs);
			Clock.Seek(fromMs);
		}

		public void SeekTo(int ms)
		{
			ms = Math.Max(0, Math.Min(ms, Timeline.TotalDurationMs));
			for (int i = 0; i < Timeline.Notes.Count; i++)
			{
				_wasHit[i] = Timeline.Notes[i].StartMs + 120 < ms;
			}
			SyncOctaveForSeek(ms);
			Clock.Seek(ms);
		}

		private void SyncOctaveForSeek(int ms)
		{
			for (int i = 0; i < Timeline.OctaveShiftPoints.Count; i++)
			{
				_firedShifts[i] = Timeline.OctaveShiftPoints[i].AtMs <= ms;
			}
			if (_autoOctave)
			{
				int target = TimelineOctaveAt(ms);
				while (_currentOctave != target)
				{
					SendOctaveDelta((target > _currentOctave) ? 1 : (-1));
				}
			}
		}

		private int TimelineOctaveAt(int ms)
		{
			int octave = 0;
			for (int i = 0; i < Timeline.OctaveShiftPoints.Count; i++)
			{
				if (Timeline.OctaveShiftPoints[i].AtMs <= ms)
				{
					octave += Timeline.OctaveShiftPoints[i].Delta;
				}
			}
			return Math.Max(_minOctave, Math.Min(_maxOctave, octave));
		}

		private void SendOctaveDelta(int delta)
		{
			int next = Math.Max(_minOctave, Math.Min(_maxOctave, _currentOctave + delta));
			if (next != _currentOctave)
			{
				_currentOctave = next;
				if (delta > 0)
				{
					_sender.SendOctaveUp();
				}
				else
				{
					_sender.SendOctaveDown();
				}
				this.OnOctaveShiftFired?.Invoke(delta);
			}
		}

		public PracticeResult BuildResult()
		{
			return new PracticeResult
			{
				PerfectCount = PerfectCount,
				GoodCount = GoodCount,
				MissCount = MissCount,
				WrongCount = WrongCount,
				MaxCombo = MaxCombo
			};
		}

		private void BreakCombo()
		{
			Combo = 0;
		}
	}
}
