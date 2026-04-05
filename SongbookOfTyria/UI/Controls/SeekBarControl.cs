using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace SongbookOfTyria.UI.Controls
{
	internal class SeekBarControl : Control
	{
		private const int MarkerPinHeight = 20;

		private const int TrackBarHeight = 32;

		private const int TrackMarginVertical = 5;

		private const int TimeLabelWidth = 80;

		private const int SectionDividerWidth = 1;

		private const int MarkerPinWidth = 14;

		private const int SectionLabelHeight = 16;

		private static readonly Color TrackBackgroundColor = new Color(40, 40, 40);

		private static readonly Color TrackBorderColor = new Color(80, 80, 80);

		private static readonly Color ProgressColor = new Color(100, 100, 100, 180);

		private static readonly Color SectionDividerColor = new Color(60, 60, 60);

		private static readonly Color SectionLabelBackgroundColor = new Color(50, 50, 50);

		private static readonly Color SectionLabelTextColor = Color.get_White();

		private static readonly Color PositionLineColor = Color.get_White();

		private static readonly Color TimeLabelColor = new Color(180, 180, 180);

		private List<SectionInfo> _sections = new List<SectionInfo>();

		private List<MarkerInfo> _markers = new List<MarkerInfo>();

		private double _duration;

		private double _currentPosition;

		private bool _isDragging;

		private bool _isSeekingTrack;

		private int? _draggingMarkerIndex;

		public double Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		public double CurrentPosition
		{
			get
			{
				return _currentPosition;
			}
			set
			{
				_currentPosition = value;
			}
		}

		private int MarkerTop => 0;

		private int TrackTop => 25;

		public event EventHandler<double> SeekRequested;

		public event EventHandler<int> MarkerRemoved;

		public event EventHandler<int> MarkerMoved;

		public SeekBarControl()
			: this()
		{
			((Control)this).set_Height(62);
		}

		public void SetSections(IEnumerable<SectionInfo> sections)
		{
			_sections = sections?.ToList() ?? new List<SectionInfo>();
		}

		public void SetMarkers(IEnumerable<MarkerInfo> markers)
		{
			_markers = markers?.ToList() ?? new List<MarkerInfo>();
		}

		private Rectangle GetTrackBounds()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			int trackWidth = ((Control)this).get_Width() - 80 - 10;
			return new Rectangle(0, TrackTop, trackWidth, 32);
		}

		private float TimeToX(double time, Rectangle trackBounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (_duration <= 0.0)
			{
				return trackBounds.X;
			}
			float ratio = (float)(time / _duration);
			return (float)trackBounds.X + ratio * (float)trackBounds.Width;
		}

		private double XToTime(float x, Rectangle trackBounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (trackBounds.Width <= 0)
			{
				return 0.0;
			}
			return (double)MathHelper.Clamp((x - (float)trackBounds.X) / (float)trackBounds.Width, 0f, 1f) * _duration;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Point mousePosition = e.get_MousePosition();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point relativePos = mousePosition - ((Rectangle)(ref absoluteBounds)).get_Location();
			Rectangle trackBounds = GetTrackBounds();
			int? markerIndex = GetMarkerAtPosition(relativePos, trackBounds);
			if (markerIndex.HasValue)
			{
				_isDragging = true;
				_draggingMarkerIndex = markerIndex.Value;
			}
			else if (IsPositionInTrack(relativePos, trackBounds))
			{
				_isSeekingTrack = true;
				double seekTime = XToTime(relativePos.X, trackBounds);
				this.SeekRequested?.Invoke(this, seekTime);
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			_isDragging = false;
			_isSeekingTrack = false;
			_draggingMarkerIndex = null;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			Point mousePosition = e.get_MousePosition();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point relativePos = mousePosition - ((Rectangle)(ref absoluteBounds)).get_Location();
			Rectangle trackBounds = GetTrackBounds();
			if (_isDragging && _draggingMarkerIndex.HasValue && _draggingMarkerIndex.Value < _markers.Count)
			{
				double newTime = XToTime(relativePos.X, trackBounds);
				_markers[_draggingMarkerIndex.Value].Time = MathHelper.Clamp((float)newTime, 0f, (float)_duration);
				this.MarkerMoved?.Invoke(this, _draggingMarkerIndex.Value);
			}
			else if (_isSeekingTrack)
			{
				double seekTime = XToTime(relativePos.X, trackBounds);
				this.SeekRequested?.Invoke(this, seekTime);
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnRightMouseButtonPressed(e);
			Point mousePosition = e.get_MousePosition();
			Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
			Point relativePos = mousePosition - ((Rectangle)(ref absoluteBounds)).get_Location();
			Rectangle trackBounds = GetTrackBounds();
			int? markerIndex = GetMarkerAtPosition(relativePos, trackBounds);
			if (markerIndex.HasValue)
			{
				this.MarkerRemoved?.Invoke(this, markerIndex.Value);
			}
		}

		private int? GetMarkerAtPosition(Point relativePos, Rectangle trackBounds)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			int pinTop = MarkerTop;
			int pinBottom = pinTop + 20 + 5;
			for (int i = 0; i < _markers.Count; i++)
			{
				MarkerInfo marker = _markers[i];
				float num = TimeToX(marker.Time, trackBounds);
				float pinLeft = num - 7f;
				float pinRight = num + 7f;
				if ((float)relativePos.X >= pinLeft && (float)relativePos.X <= pinRight && relativePos.Y >= pinTop && relativePos.Y <= pinBottom)
				{
					return i;
				}
			}
			return null;
		}

		private bool IsPositionInTrack(Point relativePos, Rectangle trackBounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (relativePos.X >= trackBounds.X && relativePos.X <= ((Rectangle)(ref trackBounds)).get_Right() && relativePos.Y >= trackBounds.Y)
			{
				return relativePos.Y <= ((Rectangle)(ref trackBounds)).get_Bottom();
			}
			return false;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Rectangle trackBounds = GetTrackBounds();
			Texture2D pixel = Textures.get_Pixel();
			DrawTrackBackground(spriteBatch, pixel, trackBounds);
			DrawProgress(spriteBatch, pixel, trackBounds);
			DrawSectionDividers(spriteBatch, pixel, trackBounds);
			DrawSectionLabels(spriteBatch, pixel, trackBounds);
			DrawMarkers(spriteBatch, pixel, trackBounds);
			DrawTimeLabel(spriteBatch, trackBounds);
		}

		private void DrawSectionLabels(SpriteBatch spriteBatch, Texture2D pixel, Rectangle trackBounds)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont12();
			Rectangle bgRect = default(Rectangle);
			foreach (SectionInfo section in _sections)
			{
				float sectionX = TimeToX(section.StartTime, trackBounds);
				string labelText = section.Label;
				Size2 textSize = font.MeasureString(labelText);
				int labelWidth = (int)textSize.Width + 8;
				int labelX = Math.Max(0, (int)sectionX - 8);
				int labelY = MarkerTop;
				((Rectangle)(ref bgRect))._002Ector(labelX, labelY, labelWidth, 16);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bgRect, SectionLabelBackgroundColor);
				DrawBorder(spriteBatch, pixel, bgRect, TrackBorderColor);
				int textY = labelY + (16 - (int)textSize.Height) / 2 - 3;
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, labelText, font, new Rectangle(labelX + 4, textY, labelWidth, 16), SectionLabelTextColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		private void DrawTrackBackground(SpriteBatch spriteBatch, Texture2D pixel, Rectangle trackBounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, trackBounds, TrackBackgroundColor);
			DrawBorder(spriteBatch, pixel, trackBounds, TrackBorderColor);
		}

		private void DrawBorder(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color color)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X, ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, rect.Y, 1, rect.Height), color);
		}

		private void DrawProgress(SpriteBatch spriteBatch, Texture2D pixel, Rectangle trackBounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if (!(_duration <= 0.0))
			{
				float progressRatio = (float)(_currentPosition / _duration);
				int progressWidth = (int)((float)trackBounds.Width * progressRatio);
				if (progressWidth > 0)
				{
					Rectangle progressRect = default(Rectangle);
					((Rectangle)(ref progressRect))._002Ector(trackBounds.X + 1, trackBounds.Y + 1, progressWidth - 1, trackBounds.Height - 2);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, progressRect, ProgressColor);
				}
				int positionX = (int)TimeToX(_currentPosition, trackBounds);
				Rectangle lineRect = default(Rectangle);
				((Rectangle)(ref lineRect))._002Ector(positionX, trackBounds.Y, 2, trackBounds.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, lineRect, PositionLineColor);
			}
		}

		private void DrawSectionDividers(SpriteBatch spriteBatch, Texture2D pixel, Rectangle trackBounds)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			Rectangle dividerRect = default(Rectangle);
			foreach (SectionInfo section in _sections)
			{
				if (!(section.StartTime <= 0.0))
				{
					int dividerX = (int)TimeToX(section.StartTime, trackBounds);
					((Rectangle)(ref dividerRect))._002Ector(dividerX, trackBounds.Y, 1, trackBounds.Height);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, dividerRect, SectionDividerColor);
				}
			}
		}

		private void DrawMarkers(SpriteBatch spriteBatch, Texture2D pixel, Rectangle trackBounds)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			foreach (MarkerInfo marker in _markers)
			{
				float markerX = TimeToX(marker.Time, trackBounds);
				DrawMarkerPin(spriteBatch, pixel, markerX, marker.Color);
			}
		}

		private void DrawMarkerPin(SpriteBatch spriteBatch, Texture2D pixel, float centerX, Color pinColor)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			int pinBodyHeight = 14;
			int pinLeft = (int)centerX - 7;
			Rectangle bodyRect = default(Rectangle);
			((Rectangle)(ref bodyRect))._002Ector(pinLeft + 2, MarkerTop, 10, pinBodyHeight);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, bodyRect, pinColor);
			Color outlineColor = pinColor * 0.5f;
			DrawBorder(spriteBatch, pixel, bodyRect, outlineColor);
			Rectangle triangleRow = default(Rectangle);
			for (int row = 0; row < 6; row++)
			{
				int y = MarkerTop + pinBodyHeight + row;
				int halfWidth = (6 - row) / 2;
				if (halfWidth > 0)
				{
					((Rectangle)(ref triangleRow))._002Ector((int)centerX - halfWidth, y, halfWidth * 2, 1);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, triangleRow, pinColor);
				}
			}
		}

		private void DrawTimeLabel(SpriteBatch spriteBatch, Rectangle trackBounds)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont12();
			string timeText = FormatTime(_currentPosition) + " / " + FormatTime(_duration);
			int labelX = ((Rectangle)(ref trackBounds)).get_Right() + 10;
			int labelY = trackBounds.Y + (trackBounds.Height - font.get_LineHeight()) / 2 - 5;
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, timeText, font, new Rectangle(labelX, labelY, 80, 20), TimeLabelColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		private static string FormatTime(double seconds)
		{
			TimeSpan ts = TimeSpan.FromSeconds(Math.Max(0.0, seconds));
			if (!(ts.TotalMinutes >= 1.0))
			{
				return $"0:{ts.Seconds:D2}";
			}
			return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
		}
	}
}
