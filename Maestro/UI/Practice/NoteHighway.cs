using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Maestro.Models;
using Maestro.Services.Practice;
using Maestro.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Maestro.UI.Practice
{
	public class NoteHighway : Control
	{
		private static class Layout
		{
			public const int LaneCount = 8;

			public const int HitLineFromBottom = 54;

			public const int KeyHintStripHeight = 24;

			public const int LaneHeaderHeight = 22;

			public const int FloaterLifetimeMs = 600;

			public const int MinNoteHeight = 18;

			public const int HeadHeight = 14;

			public const int MaxTickDeltaMs = 100;

			public const int TailGraceMs = 3000;
		}

		private struct SharpStyle
		{
			public Color Color;

			public bool DarkText;
		}

		private struct FloatingJudgement
		{
			public string Text;

			public Color Color;

			public int Lane;

			public int SpawnTimeMs;
		}

		private static readonly Color[] LaneColors = (Color[])(object)new Color[8]
		{
			new Color(105, 200, 115),
			new Color(215, 95, 110),
			new Color(230, 200, 85),
			new Color(190, 110, 210),
			new Color(185, 145, 85),
			new Color(125, 125, 225),
			new Color(235, 130, 200),
			new Color(240, 235, 245)
		};

		private static readonly Color HitLineColor = new Color(255, 220, 80);

		private static readonly Color HitWindowColor = new Color(255, 255, 255);

		private static readonly Color KeyStripBackground = new Color(8, 8, 12);

		private static readonly Color SharpTextColor = new Color(255, 255, 255);

		private static readonly Color NaturalTextColor = new Color(20, 20, 25);

		private readonly PracticeSession _session;

		private readonly float _lookaheadSeconds;

		private readonly ModuleSettings _keySettings;

		private readonly List<FloatingJudgement> _floaters = new List<FloatingJudgement>();

		private double _lastTotalMs;

		private double _hitGlowAlpha;

		public Action OnAfterTick { get; set; }

		private static SharpStyle GetSharpStyle(int lane)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			SharpStyle result;
			switch (lane)
			{
			case 1:
				result = default(SharpStyle);
				result.Color = new Color(90, 150, 230);
				result.DarkText = false;
				return result;
			case 2:
				result = default(SharpStyle);
				result.Color = new Color(225, 180, 70);
				result.DarkText = true;
				return result;
			case 4:
				result = default(SharpStyle);
				result.Color = new Color(210, 90, 190);
				result.DarkText = false;
				return result;
			case 5:
				result = default(SharpStyle);
				result.Color = new Color(230, 130, 60);
				result.DarkText = false;
				return result;
			case 6:
				result = default(SharpStyle);
				result.Color = new Color(110, 190, 100);
				result.DarkText = true;
				return result;
			default:
				result = default(SharpStyle);
				result.Color = new Color(200, 200, 200);
				result.DarkText = true;
				return result;
			}
		}

		public NoteHighway(PracticeSession session, float lookaheadSeconds, ModuleSettings keySettings)
			: this()
		{
			_session = session ?? throw new ArgumentNullException("session");
			_lookaheadSeconds = ((lookaheadSeconds > 0f) ? lookaheadSeconds : 2f);
			_keySettings = keySettings ?? throw new ArgumentNullException("keySettings");
			_session.OnJudgement += HandleJudgement;
		}

		private void HandleJudgement(Judgement j)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			string text = j.Verdict switch
			{
				JudgementVerdict.Perfect => "PERFECT", 
				JudgementVerdict.Good => "GOOD", 
				JudgementVerdict.Miss => "MISS", 
				_ => "X", 
			};
			_floaters.Add(new FloatingJudgement
			{
				Text = text,
				Color = VerdictColor(j.Verdict),
				Lane = j.Lane,
				SpawnTimeMs = _session.Clock.CurrentMs
			});
			if (j.Verdict == JudgementVerdict.Perfect)
			{
				_hitGlowAlpha = 1.0;
			}
		}

		private static Color VerdictColor(JudgementVerdict v)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(v switch
			{
				JudgementVerdict.Perfect => new Color(120, 255, 160), 
				JudgementVerdict.Good => new Color(120, 180, 255), 
				JudgementVerdict.Miss => new Color(255, 120, 120), 
				_ => new Color(200, 200, 200), 
			});
		}

		protected override void DisposeControl()
		{
			_session.OnJudgement -= HandleJudgement;
			((Control)this).DisposeControl();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			double now = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			int delta = ((_lastTotalMs != 0.0) ? ((int)Math.Max(0.0, Math.Min(100.0, now - _lastTotalMs))) : 0);
			_lastTotalMs = now;
			_session.Tick(delta);
			OnAfterTick?.Invoke();
			int clockMs = _session.Clock.CurrentMs;
			int laneWidth = bounds.Width / 8;
			if (laneWidth <= 0)
			{
				return;
			}
			int highwayTop = ((Rectangle)(ref bounds)).get_Top() + 22;
			int highwayBottom = ((Rectangle)(ref bounds)).get_Bottom() - 24;
			int highwayHeight = highwayBottom - highwayTop;
			if (highwayHeight <= 0)
			{
				return;
			}
			int hitLineY = highwayBottom - 54;
			int scrollSpan = Math.Max(1, hitLineY - highwayTop);
			int lookaheadMs = Math.Max(1, (int)(_lookaheadSeconds * 1000f));
			BitmapFont font2 = GameService.Content.get_DefaultFont12();
			Rectangle rect = default(Rectangle);
			Rectangle headerCell = default(Rectangle);
			for (int k = 0; k < 8; k++)
			{
				((Rectangle)(ref rect))._002Ector(((Rectangle)(ref bounds)).get_Left() + k * laneWidth, highwayTop, laneWidth, highwayHeight);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, LaneColors[k] * 0.08f);
				((Rectangle)(ref headerCell))._002Ector(((Rectangle)(ref bounds)).get_Left() + k * laneWidth, ((Rectangle)(ref bounds)).get_Top(), laneWidth, 22);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, NoteTimeline.LaneLetter(k + 1).ToString(), font2, headerCell, LaneColors[k], false, false, 0, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			int goodPx = Math.Max(4, 120 * scrollSpan / lookaheadMs);
			Rectangle bandRect = default(Rectangle);
			((Rectangle)(ref bandRect))._002Ector(((Rectangle)(ref bounds)).get_Left(), hitLineY - goodPx, bounds.Width, goodPx * 2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bandRect, HitWindowColor * 0.07f);
			Rectangle labelRect = default(Rectangle);
			foreach (int idx in _session.Timeline.GetNoteIndicesInWindow(clockMs - 3000, clockMs + lookaheadMs))
			{
				TimelineNote note = _session.Timeline.Notes[idx];
				if (note.Lane < 1 || note.Lane > 8)
				{
					continue;
				}
				int lane = note.Lane - 1;
				float progress = (float)(note.StartMs - clockMs) / (float)lookaheadMs;
				int leadingEdgeY = hitLineY - (int)(progress * (float)scrollSpan);
				int height = Math.Max(18, note.DurationMs * scrollSpan / lookaheadMs);
				int tileTop = leadingEdgeY - height;
				if (tileTop <= highwayBottom && leadingEdgeY >= highwayTop)
				{
					SharpStyle sharpStyle = (note.IsSharp ? GetSharpStyle(note.Lane) : default(SharpStyle));
					Color tileColor = (note.IsSharp ? sharpStyle.Color : LaneColors[lane]);
					int left = ((Rectangle)(ref bounds)).get_Left() + lane * laneWidth + 4;
					int width = laneWidth - 8;
					int headHeight = Math.Min(14, height);
					Rectangle tailRect = ClampToHighway(new Rectangle(left, tileTop, width, height - headHeight), highwayTop, highwayBottom);
					if (tailRect.Height > 0)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), tailRect, tileColor * 0.4f);
					}
					Rectangle headRect = ClampToHighway(new Rectangle(left, leadingEdgeY - headHeight, width, headHeight), highwayTop, highwayBottom);
					if (headRect.Height > 0)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), headRect, tileColor);
						string label = NoteTimeline.LaneLetter(note.Lane) + (note.IsSharp ? "#" : "");
						((Rectangle)(ref labelRect))._002Ector(left, headRect.Y - 2, width, 18);
						Color textColor = ((!note.IsSharp) ? NaturalTextColor : (sharpStyle.DarkText ? NaturalTextColor : SharpTextColor));
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, font2, labelRect, textColor, false, false, 0, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}
			Rectangle hitLineRect = default(Rectangle);
			((Rectangle)(ref hitLineRect))._002Ector(((Rectangle)(ref bounds)).get_Left(), hitLineY, bounds.Width, 3);
			Color glow = HitLineColor * (float)Math.Max(0.6, _hitGlowAlpha);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), hitLineRect, glow);
			_hitGlowAlpha = Math.Max(0.0, _hitGlowAlpha - 0.05);
			Rectangle keyStripRect = default(Rectangle);
			((Rectangle)(ref keyStripRect))._002Ector(((Rectangle)(ref bounds)).get_Left(), highwayBottom, bounds.Width, 24);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), keyStripRect, KeyStripBackground);
			Rectangle cell = default(Rectangle);
			for (int j = 0; j < 8; j++)
			{
				((Rectangle)(ref cell))._002Ector(((Rectangle)(ref bounds)).get_Left() + j * laneWidth, highwayBottom, laneWidth, 24);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, LaneKeyLabel(j + 1), GameService.Content.get_DefaultFont14(), cell, Color.get_White(), false, false, 0, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			Rectangle rect2 = default(Rectangle);
			for (int i = _floaters.Count - 1; i >= 0; i--)
			{
				FloatingJudgement f = _floaters[i];
				int age = clockMs - f.SpawnTimeMs;
				if (age < 0 || age > 600)
				{
					_floaters.RemoveAt(i);
				}
				else
				{
					float alpha = 1f - (float)age / 600f;
					int rise = (int)((double)age * 0.05);
					((Rectangle)(ref rect2))._002Ector(((Rectangle)(ref bounds)).get_Left() + (f.Lane - 1) * laneWidth, hitLineY - 24 - rise, laneWidth, 20);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, f.Text, GameService.Content.get_DefaultFont14(), rect2, f.Color * alpha, false, false, 0, (HorizontalAlignment)1, (VerticalAlignment)1);
				}
			}
			if (clockMs < 0)
			{
				string text = ((clockMs > -1000) ? "1" : ((clockMs > -2000) ? "2" : "3"));
				Rectangle cdRect = default(Rectangle);
				((Rectangle)(ref cdRect))._002Ector(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, bounds.Height);
				BitmapFont font = GameService.Content.get_DefaultFont32() ?? GameService.Content.get_DefaultFont18();
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, font, cdRect, Color.get_White(), false, false, 0, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		private static Rectangle ClampToHighway(Rectangle rect, int top, int bottom)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			int y = Math.Max(rect.Y, top);
			int end = Math.Min(rect.Y + rect.Height, bottom);
			return new Rectangle(rect.X, y, rect.Width, Math.Max(0, end - y));
		}

		private string LaneKeyLabel(int lane)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			KeyBinding binding = LaneEntry(lane)?.get_Value();
			if (binding == null || (int)binding.get_PrimaryKey() == 0)
			{
				return "-";
			}
			string label = KeyName(binding.get_PrimaryKey());
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				label = "C+" + label;
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				label = "A+" + label;
			}
			if (((Enum)binding.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				label = "S+" + label;
			}
			return label;
		}

		private SettingEntry<KeyBinding> LaneEntry(int lane)
		{
			return (SettingEntry<KeyBinding>)(lane switch
			{
				1 => _keySettings.NoteC, 
				2 => _keySettings.NoteD, 
				3 => _keySettings.NoteE, 
				4 => _keySettings.NoteF, 
				5 => _keySettings.NoteG, 
				6 => _keySettings.NoteA, 
				7 => _keySettings.NoteB, 
				8 => _keySettings.NoteCHigh, 
				_ => null, 
			});
		}

		private static string KeyName(Keys key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected I4, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected I4, but got Unknown
			if ((int)key >= 48 && (int)key <= 57)
			{
				return (key - 48).ToString();
			}
			if ((int)key >= 96 && (int)key <= 105)
			{
				return "N" + (key - 96);
			}
			return ((object)(Keys)(ref key)).ToString();
		}
	}
}
