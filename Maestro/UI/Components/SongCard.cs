using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class SongCard : Panel
	{
		public static class Layout
		{
			public const int Height = 70;

			public const int IndicatorWidth = 4;

			public const int LabelsX = 12;

			public const int InstrumentY = 4;

			public const int TitleY = 22;

			public const int ArtistY = 40;

			public const int PlayButtonWidth = 40;

			public const int PlayButtonY = 14;

			public const int PlayButtonRightMargin = 15;

			public static int LabelRightMargin => 55;
		}

		private readonly Song _song;

		private readonly Panel _indicator;

		private readonly Label _instrumentLabel;

		private readonly Label _titleLabel;

		private readonly Label _artistLabel;

		private readonly StandardButton _playButton;

		private bool _isSelected;

		private bool _isPlaying;

		public Song Song => _song;

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				UpdateVisualState();
			}
		}

		public bool IsPlaying
		{
			get
			{
				return _isPlaying;
			}
			set
			{
				_isPlaying = value;
				UpdateVisualState();
			}
		}

		public event EventHandler<MouseEventArgs> PlayClicked;

		public event EventHandler<MouseEventArgs> CardClicked;

		public SongCard(Song song, int width)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Expected O, but got Unknown
			_song = song;
			((Control)this).set_Size(new Point(width, 70));
			((Control)this).set_BackgroundColor(MaestroColors.PanelBackground);
			Color instrumentColor = GetInstrumentColor(song.Instrument);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(4, 70));
			((Control)val).set_BackgroundColor(instrumentColor);
			_indicator = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text($"[{song.Instrument}]");
			((Control)val2).set_Location(new Point(12, 4));
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_TextColor(instrumentColor);
			_instrumentLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(song.Name);
			((Control)val3).set_Location(new Point(12, 22));
			((Control)val3).set_Width(width - Layout.LabelRightMargin);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(MaestroColors.CreamWhite);
			_titleLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(song.Artist);
			((Control)val4).set_Location(new Point(12, 40));
			((Control)val4).set_Width(width - Layout.LabelRightMargin);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(MaestroColors.MutedCream);
			_artistLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(">");
			((Control)val5).set_Location(new Point(width - 40 - 15, 14));
			((Control)val5).set_Width(40);
			_playButton = val5;
			((Control)_playButton).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				this.PlayClicked?.Invoke(this, e);
			});
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				this.CardClicked?.Invoke(this, e);
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)OnMouseEntered);
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)OnMouseLeft);
		}

		private void OnMouseEntered(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (!_isSelected)
			{
				((Control)this).set_BackgroundColor(MaestroColors.PanelHover);
			}
		}

		private void OnMouseLeft(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (!_isSelected)
			{
				((Control)this).set_BackgroundColor(MaestroColors.PanelBackground);
			}
		}

		private void UpdateVisualState()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (_isSelected)
			{
				((Control)this).set_BackgroundColor(MaestroColors.PanelSelected);
			}
			else
			{
				((Control)this).set_BackgroundColor(MaestroColors.PanelBackground);
			}
		}

		private static Color GetInstrumentColor(InstrumentType instrument)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(instrument switch
			{
				InstrumentType.Piano => MaestroColors.Piano, 
				InstrumentType.Harp => MaestroColors.Harp, 
				InstrumentType.Lute => MaestroColors.Lute, 
				InstrumentType.Bass => MaestroColors.Bass, 
				_ => MaestroColors.AmberGold, 
			});
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_MouseEntered((EventHandler<MouseEventArgs>)OnMouseEntered);
			((Control)this).remove_MouseLeft((EventHandler<MouseEventArgs>)OnMouseLeft);
			Panel indicator = _indicator;
			if (indicator != null)
			{
				((Control)indicator).Dispose();
			}
			Label instrumentLabel = _instrumentLabel;
			if (instrumentLabel != null)
			{
				((Control)instrumentLabel).Dispose();
			}
			Label titleLabel = _titleLabel;
			if (titleLabel != null)
			{
				((Control)titleLabel).Dispose();
			}
			Label artistLabel = _artistLabel;
			if (artistLabel != null)
			{
				((Control)artistLabel).Dispose();
			}
			StandardButton playButton = _playButton;
			if (playButton != null)
			{
				((Control)playButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
