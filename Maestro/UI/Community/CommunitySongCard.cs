using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Community;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Community
{
	public class CommunitySongCard : Panel
	{
		public static class Layout
		{
			public const int Height = 70;

			public const int IndicatorWidth = 4;

			public const int LabelsX = 12;

			public const int InstrumentY = 4;

			public const int TitleY = 22;

			public const int DetailsY = 40;

			public const int ButtonWidth = 90;

			public const int ButtonHeight = 26;

			public const int ButtonY = 18;

			public const int ButtonRightMargin = 15;

			public static int LabelRightMargin => 115;
		}

		private readonly CommunitySong _song;

		private readonly Panel _indicator;

		private readonly Label _instrumentLabel;

		private readonly Label _titleLabel;

		private readonly Label _detailsLabel;

		private readonly StandardButton _actionButton;

		private readonly Label _progressLabel;

		private readonly ScrollingHighlightEffect _highlightEffect;

		private readonly ContextMenuStrip _contextMenu;

		private DownloadState _downloadState;

		private int _downloadProgress;

		public CommunitySong Song => _song;

		public bool IsDownloaded
		{
			get
			{
				return _downloadState == DownloadState.Completed;
			}
			set
			{
				if (value)
				{
					_downloadState = DownloadState.Completed;
					UpdateVisualState();
				}
			}
		}

		public event EventHandler<CommunitySong> DownloadRequested;

		public event EventHandler<CommunitySong> DeleteRequested;

		public CommunitySongCard(CommunitySong song, int width, bool isDownloaded = false)
			: this()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Expected O, but got Unknown
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Expected O, but got Unknown
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Expected O, but got Unknown
			_song = song;
			_downloadState = (isDownloaded ? DownloadState.Completed : DownloadState.Idle);
			((Control)this).set_Size(new Point(width, 70));
			((Control)this).set_BackgroundColor(MaestroTheme.PanelBackground);
			_highlightEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_highlightEffect);
			Color instrumentColor = GetInstrumentColor(song.InstrumentType);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(4, 70));
			((Control)val).set_BackgroundColor(instrumentColor);
			_indicator = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("[" + song.Instrument + "]");
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
			string detailsText = song.Artist + " - " + song.Transcriber;
			if (!string.IsNullOrEmpty(song.DisplayDuration))
			{
				detailsText = detailsText + " | " + song.DisplayDuration;
			}
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(detailsText);
			((Control)val4).set_Location(new Point(12, 40));
			((Control)val4).set_Width(width - Layout.LabelRightMargin);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(MaestroTheme.MutedCream);
			_detailsLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Download");
			((Control)val5).set_Location(new Point(width - 90 - 15, 18));
			((Control)val5).set_Width(90);
			_actionButton = val5;
			((Control)_actionButton).add_Click((EventHandler<MouseEventArgs>)OnActionButtonClicked);
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("");
			((Control)val6).set_Location(new Point(width - 90 - 15, 23));
			((Control)val6).set_Width(90);
			((Control)val6).set_Height(26);
			val6.set_Font(GameService.Content.get_DefaultFont12());
			val6.set_TextColor(MaestroTheme.CreamWhite);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val6).set_Visible(false);
			_progressLabel = val6;
			_contextMenu = new ContextMenuStrip();
			((Control)_contextMenu.AddMenuItem("Delete Song")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DeleteRequested?.Invoke(this, _song);
			});
			UpdateVisualState();
		}

		private void OnActionButtonClicked(object sender, MouseEventArgs e)
		{
			if (_downloadState == DownloadState.Idle)
			{
				this.DownloadRequested?.Invoke(this, _song);
			}
		}

		public void UpdateDownloadProgress(int progress, DownloadState state)
		{
			_downloadProgress = progress;
			_downloadState = state;
			UpdateVisualState();
		}

		private void UpdateVisualState()
		{
			bool isDownloaded = _downloadState == DownloadState.Completed;
			((Control)this).set_Menu(isDownloaded ? _contextMenu : null);
			((Control)this).set_BasicTooltipText(isDownloaded ? "Right-click for options" : null);
			((Control)_indicator).set_BasicTooltipText(isDownloaded ? "Right-click for options" : null);
			((Control)_instrumentLabel).set_BasicTooltipText(isDownloaded ? "Right-click for options" : null);
			((Control)_titleLabel).set_BasicTooltipText(isDownloaded ? "Right-click for options" : null);
			((Control)_detailsLabel).set_BasicTooltipText(isDownloaded ? "Right-click for options" : null);
			switch (_downloadState)
			{
			case DownloadState.Idle:
				_actionButton.set_Text("Download");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(true);
				((Control)_progressLabel).set_Visible(false);
				break;
			case DownloadState.Downloading:
				((Control)_actionButton).set_Visible(false);
				_progressLabel.set_Text($"{_downloadProgress}%");
				((Control)_progressLabel).set_Visible(true);
				break;
			case DownloadState.Completed:
				_actionButton.set_Text("Downloaded");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(false);
				((Control)_progressLabel).set_Visible(false);
				break;
			case DownloadState.Failed:
				_actionButton.set_Text("Retry");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(true);
				((Control)_progressLabel).set_Visible(false);
				break;
			case DownloadState.Cancelled:
				_actionButton.set_Text("Download");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(true);
				((Control)_progressLabel).set_Visible(false);
				break;
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
			Label detailsLabel = _detailsLabel;
			if (detailsLabel != null)
			{
				((Control)detailsLabel).Dispose();
			}
			StandardButton actionButton = _actionButton;
			if (actionButton != null)
			{
				((Control)actionButton).Dispose();
			}
			Label progressLabel = _progressLabel;
			if (progressLabel != null)
			{
				((Control)progressLabel).Dispose();
			}
			ContextMenuStrip contextMenu = _contextMenu;
			if (contextMenu != null)
			{
				((Control)contextMenu).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
