using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Community;
using Maestro.UI.Controls;
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

			public const int ButtonWidth = 40;

			public const int ButtonHeight = 26;

			public const int ButtonY = 18;

			public const int ButtonRightMargin = 15;

			public static int LabelRightMargin => 65;
		}

		private readonly CommunitySong _song;

		private readonly Panel _indicator;

		private readonly Label _instrumentLabel;

		private readonly Label _titleLabel;

		private readonly Label _detailsLabel;

		private readonly IconButton _actionButton;

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
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Expected O, but got Unknown
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Expected O, but got Unknown
			_song = song;
			_downloadState = (isDownloaded ? DownloadState.Completed : DownloadState.Idle);
			((Control)this).set_Size(new Point(width, 70));
			((Control)this).set_BackgroundColor(MaestroTheme.PanelBackground);
			_highlightEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_highlightEffect);
			Color instrumentColor = MaestroTheme.GetInstrumentAccent(song.InstrumentType);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(4, 70));
			((Control)val).set_BackgroundColor(instrumentColor);
			_indicator = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(song.InstrumentType.DisplayName());
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
			IconButton iconButton = new IconButton(MaestroIcons.Download, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Download");
			((Control)iconButton).set_Location(new Point(width - 40 - 15, 18));
			((Control)iconButton).set_Width(40);
			((Control)iconButton).set_Height(26);
			_actionButton = iconButton;
			((Control)_actionButton).add_Click((EventHandler<MouseEventArgs>)OnActionButtonClicked);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("");
			((Control)val5).set_Location(new Point(width - 40 - 15, 23));
			((Control)val5).set_Width(40);
			((Control)val5).set_Height(26);
			val5.set_Font(GameService.Content.get_DefaultFont12());
			val5.set_TextColor(MaestroTheme.CreamWhite);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val5).set_Visible(false);
			_progressLabel = val5;
			_contextMenu = new ContextMenuStrip();
			((Control)_contextMenu.AddMenuItem("Delete Song")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.DeleteRequested?.Invoke(this, _song);
			});
			UpdateVisualState();
		}

		private void OnActionButtonClicked(object sender, MouseEventArgs e)
		{
			if (_downloadState == DownloadState.Idle || _downloadState == DownloadState.Failed || _downloadState == DownloadState.Cancelled)
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
				_actionButton.IconTexture = MaestroIcons.Download;
				((Control)_actionButton).set_BasicTooltipText("Download");
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
				_actionButton.IconTexture = MaestroIcons.Check;
				((Control)_actionButton).set_BasicTooltipText("Downloaded");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(false);
				((Control)_progressLabel).set_Visible(false);
				break;
			case DownloadState.Failed:
				_actionButton.IconTexture = MaestroIcons.Refresh;
				((Control)_actionButton).set_BasicTooltipText("Retry");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(true);
				((Control)_progressLabel).set_Visible(false);
				break;
			case DownloadState.Cancelled:
				_actionButton.IconTexture = MaestroIcons.Download;
				((Control)_actionButton).set_BasicTooltipText("Download");
				((Control)_actionButton).set_Visible(true);
				((Control)_actionButton).set_Enabled(true);
				((Control)_progressLabel).set_Visible(false);
				break;
			}
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
			IconButton actionButton = _actionButton;
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
