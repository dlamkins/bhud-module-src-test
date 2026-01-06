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
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			_song = song;
			base.Size = new Point(width, 70);
			base.BackgroundColor = MaestroColors.PanelBackground;
			Color instrumentColor = GetInstrumentColor(song.Instrument);
			_indicator = new Panel
			{
				Parent = this,
				Location = new Point(0, 0),
				Size = new Point(4, 70),
				BackgroundColor = instrumentColor
			};
			_instrumentLabel = new Label
			{
				Parent = this,
				Text = $"[{song.Instrument}]",
				Location = new Point(12, 4),
				Font = GameService.Content.DefaultFont12,
				TextColor = instrumentColor
			};
			_titleLabel = new Label
			{
				Parent = this,
				Text = song.Name,
				Location = new Point(12, 22),
				Width = width - Layout.LabelRightMargin,
				Font = GameService.Content.DefaultFont14,
				TextColor = MaestroColors.CreamWhite
			};
			_artistLabel = new Label
			{
				Parent = this,
				Text = song.Artist,
				Location = new Point(12, 40),
				Width = width - Layout.LabelRightMargin,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroColors.MutedCream
			};
			_playButton = new StandardButton
			{
				Parent = this,
				Text = ">",
				Location = new Point(width - 40 - 15, 14),
				Width = 40
			};
			_playButton.Click += delegate(object s, MouseEventArgs e)
			{
				this.PlayClicked?.Invoke(this, e);
			};
			base.Click += delegate(object s, MouseEventArgs e)
			{
				this.CardClicked?.Invoke(this, e);
			};
			base.MouseEntered += OnMouseEntered;
			base.MouseLeft += OnMouseLeft;
		}

		private void OnMouseEntered(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (!_isSelected)
			{
				base.BackgroundColor = MaestroColors.PanelHover;
			}
		}

		private void OnMouseLeft(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (!_isSelected)
			{
				base.BackgroundColor = MaestroColors.PanelBackground;
			}
		}

		private void UpdateVisualState()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (_isSelected)
			{
				base.BackgroundColor = MaestroColors.PanelSelected;
			}
			else
			{
				base.BackgroundColor = MaestroColors.PanelBackground;
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
			base.MouseEntered -= OnMouseEntered;
			base.MouseLeft -= OnMouseLeft;
			_indicator?.Dispose();
			_instrumentLabel?.Dispose();
			_titleLabel?.Dispose();
			_artistLabel?.Dispose();
			_playButton?.Dispose();
			base.DisposeControl();
		}
	}
}
