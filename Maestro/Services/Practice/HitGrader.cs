using System;
using Maestro.Models;

namespace Maestro.Services.Practice
{
	public static class HitGrader
	{
		public static Judgement? Grade(int lane, bool isSharp, int nowMs, NoteTimeline timeline, bool[] wasHit, int perfectMs, int goodMs)
		{
			int bestIndex = -1;
			int bestAbsDelta = int.MaxValue;
			foreach (int idx in timeline.GetNoteIndicesInWindow(nowMs - goodMs, nowMs + goodMs))
			{
				if (wasHit[idx])
				{
					continue;
				}
				TimelineNote note = timeline.Notes[idx];
				if (note.Lane == lane && note.IsSharp == isSharp)
				{
					int abs = Math.Abs(nowMs - note.StartMs);
					if (abs <= goodMs && (abs < bestAbsDelta || (abs == bestAbsDelta && note.StartMs < timeline.Notes[bestIndex].StartMs)))
					{
						bestAbsDelta = abs;
						bestIndex = idx;
					}
				}
			}
			if (bestIndex < 0)
			{
				return null;
			}
			TimelineNote target = timeline.Notes[bestIndex];
			int deltaMs = nowMs - target.StartMs;
			JudgementVerdict verdict = ((Math.Abs(deltaMs) > perfectMs) ? JudgementVerdict.Good : JudgementVerdict.Perfect);
			Judgement value = default(Judgement);
			value.Verdict = verdict;
			value.DeltaMs = deltaMs;
			value.TimelineIndex = bestIndex;
			value.Lane = target.Lane;
			return value;
		}
	}
}
