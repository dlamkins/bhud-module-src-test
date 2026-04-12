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

		private readonly Color _chipColor;

		private readonly Color _hoverColor;

		public string NoteString { get; }

		public NoteChip(string noteString, int index)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			NoteString = noteString;
			base.Index = index;
			_chipColor = GetNoteColor(noteString);
			_hoverColor = MaestroTheme.Brighten(_chipColor);
			_currentColor = _chipColor;
			((Control)this).set_Size(new Point(114, 26));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
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
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				_currentColor = _hoverColor;
				((Control)this).Invalidate();
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				_currentColor = _chipColor;
				((Control)this).Invalidate();
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
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (noteString.StartsWith("R"))
			{
				return MaestroTheme.ChipRest;
			}
			bool isSharp = noteString.Contains("#");
			if (noteString.Contains("-"))
			{
				if (!isSharp)
				{
					return MaestroTheme.ChipLowerOctave;
				}
				return MaestroTheme.ChipLowerOctaveSharp;
			}
			if (noteString.Contains("+"))
			{
				if (!isSharp)
				{
					return MaestroTheme.ChipUpperOctave;
				}
				return MaestroTheme.ChipUpperOctaveSharp;
			}
			if (!isSharp)
			{
				return MaestroTheme.ChipMiddleOctave;
			}
			return MaestroTheme.ChipMiddleOctaveSharp;
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
