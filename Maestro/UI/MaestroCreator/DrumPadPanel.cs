using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class DrumPadPanel : Panel
	{
		private static class Layout
		{
			public const int PadHeight = 36;

			public const int RowGap = 6;

			public const int PadGap = 4;

			public const int SideMargin = 6;

			public const int TopMargin = 6;

			public const int RestHeight = 24;

			public const int RailWidth = 3;
		}

		private static readonly DrumSound[] Row1 = new DrumSound[4]
		{
			DrumSound.Bass,
			DrumSound.Snare,
			DrumSound.CrossStick,
			DrumSound.Ghost
		};

		private static readonly DrumSound[] Row2 = new DrumSound[3]
		{
			DrumSound.HighTom,
			DrumSound.MidTom,
			DrumSound.FloorTom
		};

		private static readonly DrumSound[] Row3 = new DrumSound[5]
		{
			DrumSound.Crash,
			DrumSound.Ride,
			DrumSound.HatClosed,
			DrumSound.HatOpen,
			DrumSound.HatFoot
		};

		private readonly List<Panel> _pads = new List<Panel>();

		private readonly List<Panel> _padRails = new List<Panel>();

		private readonly List<Label> _padLabels = new List<Label>();

		private readonly Panel _restButton;

		private readonly Label _restLabel;

		private Color _accent = MaestroTheme.AmberGold;

		public event EventHandler<DrumSound> PadPressed;

		public event EventHandler RestPressed;

		public DrumPadPanel(int width)
			: this()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, PianoKeyboard.Layout.TotalHeight));
			((Control)this).set_BackgroundColor(new Color(0, 0, 0, 65));
			int y = 6;
			BuildRow(Row1, y, width);
			y += 42;
			BuildRow(Row2, y, width);
			y += 42;
			BuildRow(Row3, y, width);
			y += 42;
			int restWidth = width - 12;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(6, y));
			((Control)val).set_Size(new Point(restWidth, 24));
			((Control)val).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			_restButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_restButton);
			val2.set_Text("REST");
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(restWidth, 24));
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_TextColor(MaestroTheme.GhostButtonText);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_restLabel = val2;
			((Control)_restButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_restButton).set_BackgroundColor(MaestroTheme.GhostButtonHover);
			});
			((Control)_restButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_restButton).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			});
			((Control)_restButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				this.RestPressed?.Invoke(this, EventArgs.Empty);
			});
		}

		public void Configure(InstrumentType instrument)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_accent = MaestroTheme.GetInstrumentAccent(instrument);
		}

		private void BuildRow(DrumSound[] sounds, int y, int width)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			int padW = (width - 12 - 4 * (sounds.Length - 1)) / sounds.Length;
			for (int i = 0; i < sounds.Length; i++)
			{
				DrumSoundInfo info = DrumMapping.Get(sounds[i]);
				DrumGroup group = info.Group;
				Color resting = MaestroTheme.DrumPadResting(group);
				Color hover = MaestroTheme.DrumPadHover(group);
				int x = 6 + i * (padW + 4);
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Location(new Point(x, y));
				((Control)val).set_Size(new Point(padW, 36));
				((Control)val).set_BackgroundColor(resting);
				((Control)val).set_BasicTooltipText(info.DisplayName);
				Panel pad = val;
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)pad);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Size(new Point(3, 36));
				((Control)val2).set_BackgroundColor(MaestroTheme.GetDrumGroupColor(group));
				Panel rail = val2;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)pad);
				val3.set_Text(info.DisplayName);
				((Control)val3).set_Location(new Point(3, 0));
				((Control)val3).set_Size(new Point(padW - 3, 36));
				val3.set_Font(GameService.Content.get_DefaultFont12());
				val3.set_TextColor(MaestroTheme.GhostButtonText);
				val3.set_HorizontalAlignment((HorizontalAlignment)1);
				val3.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val3).set_BasicTooltipText(info.DisplayName);
				Label label = val3;
				DrumSound captured = sounds[i];
				((Control)pad).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)pad).set_BackgroundColor(hover);
				});
				((Control)pad).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)pad).set_BackgroundColor(resting);
				});
				((Control)pad).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					((Control)pad).set_BackgroundColor(_accent);
				});
				((Control)pad).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)pad).set_BackgroundColor(hover);
					this.PadPressed?.Invoke(this, captured);
				});
				_pads.Add(pad);
				_padRails.Add(rail);
				_padLabels.Add(label);
			}
		}

		protected override void DisposeControl()
		{
			foreach (Label padLabel in _padLabels)
			{
				if (padLabel != null)
				{
					((Control)padLabel).Dispose();
				}
			}
			foreach (Panel padRail in _padRails)
			{
				if (padRail != null)
				{
					((Control)padRail).Dispose();
				}
			}
			foreach (Panel pad in _pads)
			{
				if (pad != null)
				{
					((Control)pad).Dispose();
				}
			}
			Label restLabel = _restLabel;
			if (restLabel != null)
			{
				((Control)restLabel).Dispose();
			}
			Panel restButton = _restButton;
			if (restButton != null)
			{
				((Control)restButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
