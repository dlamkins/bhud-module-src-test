using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class NoteChip : Panel
	{
		public static class Layout
		{
			public const int Height = 24;

			public const int MinWidth = 50;

			public const int MaxWidth = 155;

			public const int CloseButtonSize = 16;

			public const int Padding = 4;

			public const int CharWidthEstimate = 8;

			public const int CharWidthTruncate = 7;

			public const int CloseButtonMargin = 2;
		}

		private readonly Label _noteLabel;

		private readonly Label _closeButton;

		public string NoteString { get; }

		public int Index { get; set; }

		public event EventHandler RemoveClicked;

		public NoteChip(string noteString, int index)
			: this()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			NoteChip noteChip = this;
			NoteString = noteString;
			Index = index;
			int textWidth = noteString.Length * 8 + 8 + 16;
			int width = Math.Max(50, Math.Min(textWidth, 155));
			((Control)this).set_Size(new Point(width, 24));
			((Control)this).set_BackgroundColor(GetNoteColor(noteString));
			string displayText = noteString;
			int maxTextWidth = width - 16 - 8;
			int maxChars = maxTextWidth / 7;
			if (noteString.Length > maxChars && maxChars > 3)
			{
				displayText = noteString.Substring(0, maxChars - 2) + "..";
			}
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(displayText);
			((Control)val).set_Location(new Point(4, 2));
			((Control)val).set_Size(new Point(maxTextWidth, 20));
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.CreamWhite);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_BasicTooltipText(noteString);
			_noteLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("×");
			((Control)val2).set_Location(new Point(width - 16 - 2, 2));
			((Control)val2).set_Size(new Point(16, 20));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(MaestroTheme.MutedCream);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_closeButton = val2;
			((Control)_closeButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				noteChip._closeButton.set_TextColor(MaestroTheme.Error);
			});
			((Control)_closeButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				noteChip._closeButton.set_TextColor(MaestroTheme.MutedCream);
			});
			((Control)_closeButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				noteChip.RemoveClicked?.Invoke(noteChip, EventArgs.Empty);
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				((Control)noteChip).set_BackgroundColor(MaestroTheme.WithAlpha(noteChip.GetNoteColor(noteString), 255));
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				((Control)noteChip).set_BackgroundColor(noteChip.GetNoteColor(noteString));
			});
		}

		private Color GetNoteColor(string noteString)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (noteString.StartsWith("R"))
			{
				return MaestroTheme.ChipRest;
			}
			if (noteString.Contains("-"))
			{
				return MaestroTheme.ChipLowerOctave;
			}
			if (noteString.Contains("+"))
			{
				return MaestroTheme.ChipUpperOctave;
			}
			return MaestroTheme.ChipMiddleOctave;
		}

		protected override void DisposeControl()
		{
			Label noteLabel = _noteLabel;
			if (noteLabel != null)
			{
				((Control)noteLabel).Dispose();
			}
			Label closeButton = _closeButton;
			if (closeButton != null)
			{
				((Control)closeButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
