using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
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

			public const int PlayButtonHeight = 26;

			public const int PlayButtonY = 22;

			public const int PlayButtonRightMargin = 15;

			public static int LabelRightMargin => 55;
		}

		private readonly Song _song;

		private readonly Panel _indicator;

		private readonly Label _instrumentLabel;

		private readonly Label _titleLabel;

		private readonly Label _artistLabel;

		private readonly StandardButton _playButton;

		private readonly ScrollingHighlightEffect _highlightEffect;

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

		public event EventHandler DeleteRequested;

		public SongCard(Song song, int width)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			_song = song;
			base.Size = new Point(width, 70);
			base.BackgroundColor = MaestroTheme.PanelBackground;
			_highlightEffect = new ScrollingHighlightEffect(this);
			base.EffectBehind = _highlightEffect;
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
				TextColor = MaestroTheme.CreamWhite
			};
			_artistLabel = new Label
			{
				Parent = this,
				Text = song.Artist,
				Location = new Point(12, 40),
				Width = width - Layout.LabelRightMargin,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroTheme.MutedCream
			};
			_playButton = new StandardButton
			{
				Parent = this,
				Text = ">",
				Location = new Point(width - 40 - 15, 22),
				Width = 40
			};
			_playButton.Click += delegate(object s, MouseEventArgs e)
			{
				this.PlayClicked?.Invoke(this, e);
			};
			if (song.IsUserImported)
			{
				ContextMenuStrip contextMenu = new ContextMenuStrip();
				contextMenu.AddMenuItem("Delete Song").Click += delegate
				{
					this.DeleteRequested?.Invoke(this, EventArgs.Empty);
				};
				base.Menu = contextMenu;
				base.BasicTooltipText = "Right-click for options";
				_indicator.BasicTooltipText = "Right-click for options";
				_instrumentLabel.BasicTooltipText = "Right-click for options";
				_titleLabel.BasicTooltipText = "Right-click for options";
				_artistLabel.BasicTooltipText = "Right-click for options";
			}
			base.Click += delegate(object s, MouseEventArgs e)
			{
				this.CardClicked?.Invoke(this, e);
			};
		}

		private void UpdateVisualState()
		{
			_highlightEffect.ForceActive = _isSelected;
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
			_indicator?.Dispose();
			_instrumentLabel?.Dispose();
			_titleLabel?.Dispose();
			_artistLabel?.Dispose();
			_playButton?.Dispose();
			base.DisposeControl();
		}
	}
}
