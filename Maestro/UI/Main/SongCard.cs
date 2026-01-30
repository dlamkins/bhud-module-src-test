using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Main
{
	public class SongCard : Panel
	{
		private static class Layout
		{
			public const int Height = 70;

			public const int IndicatorWidth = 4;

			public const int LabelsX = 12;

			public const int InstrumentY = 4;

			public const int TitleY = 22;

			public const int ArtistY = 40;

			public const int PlayButtonWidth = 40;

			public const int PlayButtonHeight = 26;

			public const int PlayButtonY = 22;

			public const int PlayButtonRightMargin = 15;

			public static int LabelRightMargin => 55;
		}

		private readonly Panel _indicator;

		private readonly Label _instrumentLabel;

		private readonly Label _titleLabel;

		private readonly Label _artistLabel;

		private readonly StandardButton _playButton;

		private readonly ScrollingHighlightEffect _highlightEffect;

		private bool _isSelected;

		private bool _isPlaying;

		public Song Song { get; }

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

		public event EventHandler DeleteRequested;

		public event EventHandler EditRequested;

		public event EventHandler AddToQueueRequested;

		public SongCard(Song song, int width)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Expected O, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Expected O, but got Unknown
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			Song = song;
			((Control)this).set_Size(new Point(width, 70));
			((Control)this).set_BackgroundColor(MaestroTheme.PanelBackground);
			_highlightEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_highlightEffect);
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
			val3.set_TextColor(MaestroTheme.CreamWhite);
			_titleLabel = val3;
			string artistText = (string.IsNullOrWhiteSpace(song.Transcriber) ? song.Artist : (song.Artist + " - " + song.Transcriber));
			if (!string.IsNullOrEmpty(song.DisplayDuration))
			{
				artistText = artistText + " | " + song.DisplayDuration;
			}
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(artistText);
			((Control)val4).set_Location(new Point(12, 40));
			((Control)val4).set_Width(width - Layout.LabelRightMargin);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(MaestroTheme.MutedCream);
			_artistLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(">");
			((Control)val5).set_Location(new Point(width - 40 - 15, 22));
			((Control)val5).set_Width(40);
			_playButton = val5;
			((Control)_playButton).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				this.PlayClicked?.Invoke(this, e);
			});
			ContextMenuStrip contextMenu = new ContextMenuStrip();
			((Control)contextMenu.AddMenuItem("Add to Queue")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.AddToQueueRequested?.Invoke(this, EventArgs.Empty);
			});
			if (song.IsUserImported || song.IsCreated)
			{
				((Control)contextMenu.AddMenuItem("Edit Song")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					this.EditRequested?.Invoke(this, EventArgs.Empty);
				});
			}
			if (song.IsUserImported || song.IsCreated || song.IsCommunityDownloaded)
			{
				((Control)contextMenu.AddMenuItem("Delete Song")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					this.DeleteRequested?.Invoke(this, EventArgs.Empty);
				});
			}
			((Control)this).set_Menu(contextMenu);
			((Control)this).set_BasicTooltipText("Right-click for options");
			((Control)_indicator).set_BasicTooltipText("Right-click for options");
			((Control)_instrumentLabel).set_BasicTooltipText("Right-click for options");
			((Control)_titleLabel).set_BasicTooltipText("Right-click for options");
			((Control)_artistLabel).set_BasicTooltipText("Right-click for options");
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
			{
				this.CardClicked?.Invoke(this, e);
			});
		}

		private void UpdateVisualState()
		{
			_highlightEffect.set_ForceActive(_isSelected);
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
				InstrumentType.Piano => MaestroTheme.Piano, 
				InstrumentType.Harp => MaestroTheme.Harp, 
				InstrumentType.Lute => MaestroTheme.Lute, 
				InstrumentType.Bass => MaestroTheme.Bass, 
				_ => MaestroTheme.AmberGold, 
			});
		}

		protected override void DisposeControl()
		{
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
