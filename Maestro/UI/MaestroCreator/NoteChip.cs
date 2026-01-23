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

			public const int FixedWidth = 118;

			public const int CloseButtonSize = 16;

			public const int Padding = 4;

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
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			NoteChip noteChip = this;
			NoteString = noteString;
			Index = index;
			((Control)this).set_Size(new Point(118, 24));
			((Control)this).set_BackgroundColor(GetNoteColor(noteString));
			int maxTextWidth = 92;
			int maxChars = maxTextWidth / 7;
			string displayText = noteString;
			bool needsTooltip = false;
			if (noteString.Length > maxChars)
			{
				displayText = noteString.Substring(0, maxChars - 2) + "..";
				needsTooltip = true;
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
			((Control)val).set_BasicTooltipText(needsTooltip ? noteString : null);
			_noteLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("×");
			((Control)val2).set_Location(new Point(100, 2));
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
