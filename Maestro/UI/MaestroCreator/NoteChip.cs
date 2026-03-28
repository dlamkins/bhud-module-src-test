using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class NoteChip : BaseChip
	{
		public static class Layout
		{
			public const int Height = 26;

			public const int FixedWidth = 114;

			public const int CloseButtonSize = 16;

			public const int Padding = 4;

			public const int CloseButtonMargin = 2;
		}

		private readonly Label _noteLabel;

		private readonly Label _closeButton;

		public string NoteString { get; }

		public NoteChip(string noteString, int index)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			NoteString = noteString;
			base.Index = index;
			Color baseColor = GetNoteColor(noteString);
			((Control)this).set_Size(new Point(114, 26));
			((Control)this).set_BackgroundColor(baseColor);
			int maxTextWidth = 88;
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
			((Control)val).set_Size(new Point(maxTextWidth, 22));
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(MaestroTheme.CreamWhite);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_BasicTooltipText(needsTooltip ? noteString : null);
			_noteLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("×");
			((Control)val2).set_Location(new Point(96, 2));
			((Control)val2).set_Size(new Point(16, 22));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(MaestroTheme.MutedCream);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_closeButton = val2;
			((Control)_closeButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_closeButton.set_TextColor(MaestroTheme.Error);
			});
			((Control)_closeButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_closeButton.set_TextColor(MaestroTheme.MutedCream);
			});
			((Control)_closeButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				FireRemoveClicked();
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_BackgroundColor(MaestroTheme.WithAlpha(baseColor, 255));
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_BackgroundColor(baseColor);
			});
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_RelativeMousePosition().X < 96)
			{
				FireChipClicked(e);
			}
			((Panel)this).OnClick(e);
		}

		private static Color GetNoteColor(string noteString)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (noteString.StartsWith("R"))
			{
				return MaestroTheme.ChipRest;
			}
			bool num = noteString.Contains("#");
			Color baseColor = (noteString.Contains("-") ? MaestroTheme.ChipLowerOctave : ((!noteString.Contains("+")) ? MaestroTheme.ChipMiddleOctave : MaestroTheme.ChipUpperOctave));
			if (!num)
			{
				return baseColor;
			}
			return MaestroTheme.Darken(baseColor, 0.7f);
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
