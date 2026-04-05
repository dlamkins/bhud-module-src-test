using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace SongbookOfTyria.UI.Controls.Notation
{
	public class NotationControl : Control
	{
		private readonly struct TextSegment
		{
			public readonly string Text;

			public readonly BitmapFont Font;

			public readonly Color Color;

			public readonly int X;

			public readonly int Y;

			public readonly int CharWidth;

			public readonly int LineHeight;

			public readonly Color? BackgroundColor;

			public TextSegment(string text, BitmapFont font, Color color, int x, int y, int charWidth = 0, int lineHeight = 0, Color? backgroundColor = null)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Text = text;
				Font = font;
				Color = color;
				X = x;
				Y = y;
				CharWidth = charWidth;
				LineHeight = lineHeight;
				BackgroundColor = backgroundColor;
			}
		}

		private class NoteFeedbackInfo
		{
			public NoteFeedbackType Type { get; }

			public DateTime Timestamp { get; }

			public NoteFeedbackInfo(NoteFeedbackType type, DateTime timestamp)
			{
				Type = type;
				Timestamp = timestamp;
			}
		}

		private readonly struct FeedbackIndicator
		{
			public readonly NoteFeedbackType Type;

			public readonly int X;

			public readonly int Y;

			public readonly DateTime Timestamp;

			public FeedbackIndicator(NoteFeedbackType type, int x, int y, DateTime timestamp)
			{
				Type = type;
				X = x;
				Y = y;
				Timestamp = timestamp;
			}
		}

		private readonly struct HighlightGroupInfo
		{
			public readonly int MinY;

			public readonly int Height;

			public HighlightGroupInfo(int minY, int height)
			{
				MinY = minY;
				Height = height;
			}
		}

		private const float BoldOffset = 0.5f;

		private const double HighlightTimeoutMs = 500.0;

		private const double FeedbackTimeoutMs = 600.0;

		private const int FeedbackIndicatorOffsetY = -4;

		private const int FeedbackIndicatorSize = 8;

		private static readonly Color HighlightColor = new Color(255, 255, 100) * 0.35f;

		private static readonly Color CorrectIndicatorColor = new Color(100, 255, 100);

		private static readonly Color WrongIndicatorColor = new Color(255, 80, 80);

		private static readonly Color MissedIndicatorColor = new Color(255, 180, 50);

		private const bool ShowDebugBounds = false;

		private static readonly Color[] DebugColors = (Color[])(object)new Color[6]
		{
			Color.get_Red() * 0.3f,
			Color.get_Green() * 0.3f,
			Color.get_Blue() * 0.3f,
			Color.get_Yellow() * 0.3f,
			Color.get_Cyan() * 0.3f,
			Color.get_Magenta() * 0.3f
		};

		private readonly List<TextSegment> _segments = new List<TextSegment>();

		private readonly Dictionary<int, int> _noteIndexToSegment = new Dictionary<int, int>();

		private HashSet<int> _highlightedNoteIndices;

		private HashSet<int> _lastHighlightedNoteIndices;

		private DateTime _lastHighlightTime;

		private int _noteCounter;

		private Dictionary<int, NoteFeedbackInfo> _noteFeedback = new Dictionary<int, NoteFeedbackInfo>();

		private readonly List<FeedbackIndicator> _feedbackIndicators = new List<FeedbackIndicator>();

		private List<NotationMarker> _markers = new List<NotationMarker>();

		public bool SmoothScrolling { get; set; }

		public void AddSegment(string text, BitmapFont font, Color color, int x, int y, int charWidth = 0, int lineHeight = 0, Color? backgroundColor = null)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			int segmentIndex = _segments.Count;
			_segments.Add(new TextSegment(text, font, color, x, y, charWidth, lineHeight, backgroundColor));
			if (text.Length == 1 && IsNoteCharacter(text[0]))
			{
				_noteIndexToSegment[_noteCounter] = segmentIndex;
				_noteCounter++;
			}
		}

		public void ClearSegments()
		{
			_segments.Clear();
			_noteIndexToSegment.Clear();
			_noteCounter = 0;
			_highlightedNoteIndices = null;
			_lastHighlightedNoteIndices = null;
			_noteFeedback.Clear();
			_feedbackIndicators.Clear();
			_markers.Clear();
		}

		public void SetMarkers(List<NotationMarker> markers)
		{
			_markers = markers ?? new List<NotationMarker>();
			((Control)this).Invalidate();
		}

		public void SetHighlightedNoteIndices(HashSet<int> noteIndices)
		{
			if (noteIndices != null && noteIndices.Count > 0)
			{
				_highlightedNoteIndices = noteIndices;
				_lastHighlightedNoteIndices = new HashSet<int>(noteIndices);
			}
			else
			{
				if (_highlightedNoteIndices != null && _highlightedNoteIndices.Count > 0)
				{
					_lastHighlightTime = DateTime.UtcNow;
				}
				_highlightedNoteIndices = null;
			}
			((Control)this).Invalidate();
		}

		public void SetNoteFeedback(Dictionary<int, NoteFeedbackType> feedback)
		{
			DateTime now = DateTime.UtcNow;
			Dictionary<int, NoteFeedbackInfo> newFeedback = new Dictionary<int, NoteFeedbackInfo>();
			if (feedback != null)
			{
				foreach (KeyValuePair<int, NoteFeedbackType> kvp in feedback)
				{
					if (_noteFeedback.TryGetValue(kvp.Key, out var existingInfo) && existingInfo.Type == kvp.Value)
					{
						newFeedback[kvp.Key] = existingInfo;
						continue;
					}
					newFeedback[kvp.Key] = new NoteFeedbackInfo(kvp.Value, now);
					if (_noteIndexToSegment.TryGetValue(kvp.Key, out var segIdx) && segIdx < _segments.Count)
					{
						TextSegment segment = _segments[segIdx];
						_feedbackIndicators.Add(new FeedbackIndicator(kvp.Value, segment.X + segment.CharWidth / 2 - 4, segment.Y + -4, now));
					}
				}
			}
			_noteFeedback = newFeedback;
			((Control)this).Invalidate();
		}

		public void ClearNoteFeedback()
		{
			_noteFeedback.Clear();
			_feedbackIndicators.Clear();
			((Control)this).Invalidate();
		}

		private static bool IsNoteCharacter(char c)
		{
			if (c < '1' || c > '8')
			{
				if (c >= '①')
				{
					return c <= '⓿';
				}
				return false;
			}
			return true;
		}

		private static bool IsAdjacentSymbol(char c)
		{
			if (c != '/' && c != '[' && c != ']' && c != '(')
			{
				return c == ')';
			}
			return true;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (_lastHighlightedNoteIndices != null && _highlightedNoteIndices == null && (DateTime.UtcNow - _lastHighlightTime).TotalMilliseconds >= 500.0)
			{
				_lastHighlightedNoteIndices = null;
				((Control)this).Invalidate();
			}
			CleanupExpiredFeedbackIndicators();
		}

		private void CleanupExpiredFeedbackIndicators()
		{
			if (_feedbackIndicators.Count == 0)
			{
				return;
			}
			DateTime now = DateTime.UtcNow;
			bool anyRemoved = false;
			for (int i = _feedbackIndicators.Count - 1; i >= 0; i--)
			{
				if ((now - _feedbackIndicators[i].Timestamp).TotalMilliseconds >= 600.0)
				{
					_feedbackIndicators.RemoveAt(i);
					anyRemoved = true;
				}
			}
			if (anyRemoved)
			{
				((Control)this).Invalidate();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			float uiScale = Control.get_Graphics().get_UIScaleMultiplier();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			float opacity = ((Control)this).AbsoluteOpacity();
			HashSet<int> highlightedSegments = BuildHighlightedSegmentSet();
			Dictionary<int, HighlightGroupInfo> highlightGroups = BuildContiguousHighlightGroups(highlightedSegments);
			int colorIdx = 0;
			Rectangle hlRect = default(Rectangle);
			Rectangle bgRect = default(Rectangle);
			Vector2 position = default(Vector2);
			for (int segIdx = 0; segIdx < _segments.Count; segIdx++)
			{
				TextSegment segment = _segments[segIdx];
				float num = segment.X + absoluteBounds.X;
				float absY = segment.Y + absoluteBounds.Y;
				float alignedX = (float)(int)(num * uiScale) / uiScale;
				float finalY = (SmoothScrolling ? absY : ((float)(int)(absY * uiScale) / uiScale));
				if (highlightedSegments != null && highlightedSegments.Contains(segIdx) && segment.CharWidth > 0 && segment.LineHeight > 0)
				{
					bool prevHighlighted = segIdx > 0 && highlightedSegments.Contains(segIdx - 1);
					bool nextHighlighted = segIdx < _segments.Count - 1 && highlightedSegments.Contains(segIdx + 1);
					int hlX = (prevHighlighted ? segment.X : (segment.X - 1));
					int hlWidth = segment.CharWidth + ((!prevHighlighted) ? 1 : 0) + ((!nextHighlighted) ? 1 : 0);
					int hlY = segment.Y;
					int hlHeight = segment.LineHeight;
					if (highlightGroups.TryGetValue(segIdx, out var groupInfo))
					{
						hlY = groupInfo.MinY;
						hlHeight = groupInfo.Height;
					}
					((Rectangle)(ref hlRect))._002Ector(hlX, hlY, hlWidth, hlHeight);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), hlRect, HighlightColor * opacity);
				}
				if (segment.BackgroundColor.HasValue && segment.CharWidth > 0 && segment.LineHeight > 0)
				{
					((Rectangle)(ref bgRect))._002Ector(segment.X, segment.Y, segment.CharWidth, segment.LineHeight);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bgRect, segment.BackgroundColor.Value * opacity);
				}
				((Vector2)(ref position))._002Ector(alignedX, finalY);
				Color color = segment.Color * opacity;
				BitmapFontExtensions.DrawString(spriteBatch, segment.Font, segment.Text, position + new Vector2(0.5f, 0f), color, (Rectangle?)null);
				BitmapFontExtensions.DrawString(spriteBatch, segment.Font, segment.Text, position, color, (Rectangle?)null);
			}
			DrawMarkers(spriteBatch, opacity);
			DrawFeedbackIndicators(spriteBatch, opacity);
		}

		private void DrawMarkers(SpriteBatch spriteBatch, float opacity)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if (_markers == null || _markers.Count == 0)
			{
				return;
			}
			Rectangle markerRect = default(Rectangle);
			foreach (NotationMarker marker in _markers)
			{
				if (_noteIndexToSegment.TryGetValue(marker.NoteIndex, out var segIdx) && segIdx < _segments.Count)
				{
					TextSegment segment = _segments[segIdx];
					if (segment.LineHeight > 0)
					{
						((Rectangle)(ref markerRect))._002Ector(segment.X - 4 - 2, segment.Y + 2, 4, segment.LineHeight - 4);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), markerRect, marker.Color * opacity);
					}
				}
			}
		}

		private void DrawFeedbackIndicators(SpriteBatch spriteBatch, float opacity)
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			if (_feedbackIndicators.Count == 0)
			{
				return;
			}
			DateTime now = DateTime.UtcNow;
			Rectangle dotRect = default(Rectangle);
			foreach (FeedbackIndicator indicator in _feedbackIndicators)
			{
				double elapsed = (now - indicator.Timestamp).TotalMilliseconds;
				if (!(elapsed >= 600.0))
				{
					float fadeProgress = (float)(elapsed / 600.0);
					float alpha = 1f - fadeProgress * fadeProgress;
					float floatOffset = (float)(elapsed / 600.0) * -10f;
					Color indicatorColor;
					switch (indicator.Type)
					{
					case NoteFeedbackType.Correct:
						indicatorColor = CorrectIndicatorColor;
						break;
					case NoteFeedbackType.Wrong:
						indicatorColor = WrongIndicatorColor;
						break;
					case NoteFeedbackType.Missed:
						indicatorColor = MissedIndicatorColor;
						break;
					default:
						continue;
					}
					Color finalColor = indicatorColor * opacity * alpha;
					int dotY = Math.Max(2, (int)((float)indicator.Y + floatOffset));
					((Rectangle)(ref dotRect))._002Ector(indicator.X, dotY, 8, 8);
					DrawFilledCircle(spriteBatch, dotRect, finalColor);
				}
			}
		}

		private void DrawFilledCircle(SpriteBatch spriteBatch, Rectangle bounds, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			int centerX = bounds.X + bounds.Width / 2;
			int centerY = bounds.Y + bounds.Height / 2;
			int radius = bounds.Width / 2;
			Rectangle pixelRect = default(Rectangle);
			for (int y = -radius; y <= radius; y++)
			{
				for (int x = -radius; x <= radius; x++)
				{
					if (x * x + y * y <= radius * radius)
					{
						((Rectangle)(ref pixelRect))._002Ector(centerX + x, centerY + y, 1, 1);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), pixelRect, color);
					}
				}
			}
		}

		private HashSet<int> BuildHighlightedSegmentSet()
		{
			HashSet<int> activeIndices = _highlightedNoteIndices;
			if (activeIndices == null || activeIndices.Count == 0)
			{
				if (_lastHighlightedNoteIndices == null || _lastHighlightedNoteIndices.Count <= 0)
				{
					return null;
				}
				if (!((DateTime.UtcNow - _lastHighlightTime).TotalMilliseconds < 500.0))
				{
					_lastHighlightedNoteIndices = null;
					return null;
				}
				activeIndices = _lastHighlightedNoteIndices;
			}
			HashSet<int> result = new HashSet<int>();
			foreach (int noteIdx in activeIndices)
			{
				if (_noteIndexToSegment.TryGetValue(noteIdx, out var segIdx))
				{
					result.Add(segIdx);
					AddAdjacentSymbolSegments(segIdx, result);
				}
			}
			if (result.Count <= 0)
			{
				return null;
			}
			return result;
		}

		private Dictionary<int, HighlightGroupInfo> BuildContiguousHighlightGroups(HashSet<int> highlightedSegments)
		{
			Dictionary<int, HighlightGroupInfo> result = new Dictionary<int, HighlightGroupInfo>();
			if (highlightedSegments == null || highlightedSegments.Count == 0)
			{
				return result;
			}
			List<int> list = new List<int>(highlightedSegments);
			list.Sort();
			List<int> currentGroup = new List<int>();
			int minY = int.MaxValue;
			int maxBottom = 0;
			foreach (int segIdx in list)
			{
				TextSegment segment = _segments[segIdx];
				bool num = currentGroup.Count == 0 || segIdx == currentGroup[currentGroup.Count - 1] + 1;
				int firstGroupY = ((currentGroup.Count == 0) ? segment.Y : _segments[currentGroup[0]].Y);
				bool sameLine = currentGroup.Count == 0 || Math.Abs(segment.Y - firstGroupY) <= 10;
				if (num && sameLine)
				{
					currentGroup.Add(segIdx);
					if (segment.Y < minY)
					{
						minY = segment.Y;
					}
					int bottom = segment.Y + segment.LineHeight;
					if (bottom > maxBottom)
					{
						maxBottom = bottom;
					}
					continue;
				}
				HighlightGroupInfo info2 = new HighlightGroupInfo(minY, maxBottom - minY);
				foreach (int idx2 in currentGroup)
				{
					result[idx2] = info2;
				}
				currentGroup.Clear();
				currentGroup.Add(segIdx);
				minY = segment.Y;
				maxBottom = segment.Y + segment.LineHeight;
			}
			if (currentGroup.Count > 0)
			{
				HighlightGroupInfo info = new HighlightGroupInfo(minY, maxBottom - minY);
				{
					foreach (int idx in currentGroup)
					{
						result[idx] = info;
					}
					return result;
				}
			}
			return result;
		}

		private void AddAdjacentSymbolSegments(int noteSegmentIndex, HashSet<int> result)
		{
			for (int j = noteSegmentIndex - 1; j >= 0; j--)
			{
				TextSegment seg2 = _segments[j];
				if (seg2.Text.Length != 1 || !IsAdjacentSymbol(seg2.Text[0]))
				{
					break;
				}
				result.Add(j);
			}
			for (int i = noteSegmentIndex + 1; i < _segments.Count; i++)
			{
				TextSegment seg = _segments[i];
				if (seg.Text.Length == 1 && IsAdjacentSymbol(seg.Text[0]))
				{
					result.Add(i);
					continue;
				}
				break;
			}
		}

		public NotationControl()
			: this()
		{
		}
	}
}
