using System;
using System.Diagnostics;
using BhModule.Community.Pathing.MarkerPackRepo;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Flurl;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class MarkerPackHero : Container
	{
		private const int DEFAULT_WIDTH = 500;

		private const int DEFAULT_HEIGHT = 170;

		private const int EDGE_PADDING = 20;

		private const double FADE_DURATION = 150.0;

		private static readonly Texture2D _textureHeroBackground;

		private readonly PathingModule _module;

		private readonly MarkerPackPkg _markerPackPkg;

		private readonly BlueButton _downloadButton;

		private readonly BlueButton _infoButton;

		private readonly BlueButton _deleteButton;

		private readonly Checkbox _keepUpdatedCheckbox;

		private readonly string _lastUpdateStr = "";

		private double _hoverTick;

		private bool _isUpToDate;

		static MarkerPackHero()
		{
			_textureHeroBackground = PathingModule.Instance.ContentsManager.GetTexture("png\\controls\\155209.png");
		}

		public MarkerPackHero(PathingModule module, MarkerPackPkg markerPackPkg)
			: this()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			_module = module;
			_markerPackPkg = markerPackPkg;
			if (markerPackPkg.LastUpdate != default(DateTime))
			{
				_lastUpdateStr = "Last update " + markerPackPkg.LastUpdate.Humanize();
			}
			((Control)this).SuspendLayout();
			Checkbox val = new Checkbox();
			val.set_Text("Keep Updated");
			((Control)val).set_BasicTooltipText("If checked, new pack versions will be automatically downloaded on launch.");
			((Control)val).set_Parent((Container)(object)this);
			val.set_Checked(markerPackPkg.AutoUpdate.get_Value());
			((Control)val).set_Enabled(markerPackPkg.CurrentDownloadDate != default(DateTime));
			_keepUpdatedCheckbox = val;
			BlueButton obj = new BlueButton
			{
				Text = Strings.Repo_Download
			};
			((Control)obj).set_Width(90);
			((Control)obj).set_Parent((Container)(object)this);
			_downloadButton = obj;
			if (_markerPackPkg.Size > 0f)
			{
				((Control)_downloadButton).set_BasicTooltipText(Math.Round(_markerPackPkg.Size, 2).Megabytes().Humanize());
			}
			BlueButton obj2 = new BlueButton
			{
				Text = Strings.Repo_Info
			};
			((Control)obj2).set_Width(90);
			((Control)obj2).set_Visible(_markerPackPkg.Info != null);
			((Control)obj2).set_BasicTooltipText(_markerPackPkg.Info);
			((Control)obj2).set_Parent((Container)(object)this);
			_infoButton = obj2;
			BlueButton obj3 = new BlueButton
			{
				Text = "Delete"
			};
			((Control)obj3).set_Width(90);
			((Control)obj3).set_Parent((Container)(object)this);
			_deleteButton = obj3;
			if (_markerPackPkg.TotalDownloads > 0)
			{
				((Control)this).set_BasicTooltipText($"Approx. {_markerPackPkg.TotalDownloads:n0} Downloads");
			}
			((Control)_downloadButton).add_Click((EventHandler<MouseEventArgs>)DownloadButtonOnClick);
			((Control)_infoButton).add_Click((EventHandler<MouseEventArgs>)InfoButtonOnClick);
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)DeleteButtonOnClick);
			_keepUpdatedCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)KeepUpdatedCheckboxOnChecked);
			markerPackPkg.AutoUpdate.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoUpdateOnSettingChanged);
			((Control)this).set_Size(new Point(500, 170));
			((Control)this).set_Padding(new Thickness(13f, 0f, 0f, 9f));
			((Control)this).ResumeLayout(true);
		}

		private void AutoUpdateOnSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			_keepUpdatedCheckbox.set_Checked(e.get_NewValue());
		}

		private void DeleteButtonOnClick(object sender, MouseEventArgs e)
		{
			PackHandlingUtil.DeletePack(_module, _markerPackPkg);
		}

		private void KeepUpdatedCheckboxOnChecked(object sender, CheckChangedEvent e)
		{
			_markerPackPkg.AutoUpdate.set_Value(e.get_Checked());
		}

		private void DownloadButtonOnClick(object sender, MouseEventArgs e)
		{
			((Control)_downloadButton).set_Enabled(false);
			PackHandlingUtil.DownloadPack(_module, _markerPackPkg, OnComplete);
		}

		private static void OnComplete(MarkerPackPkg markerPackPkg, bool success)
		{
			markerPackPkg.IsDownloading = false;
			if (success)
			{
				markerPackPkg.CurrentDownloadDate = DateTime.UtcNow;
			}
		}

		private void UpdateControlStates()
		{
			string downloadText = "Download";
			bool downloadEnabled = true;
			_isUpToDate = false;
			if (_markerPackPkg.CurrentDownloadDate != default(DateTime))
			{
				downloadEnabled = false;
				((Control)_deleteButton).set_Visible(true);
				((Control)_deleteButton).set_Enabled(true);
				if (_module.PackInitiator.IsLoading)
				{
					downloadText = "Loading...";
				}
				else if (_markerPackPkg.LastUpdate > _markerPackPkg.CurrentDownloadDate)
				{
					downloadText = "Update";
					downloadEnabled = true;
				}
				else
				{
					downloadText = "Up to Date";
					_isUpToDate = true;
				}
			}
			else
			{
				((Control)_deleteButton).set_Visible(false);
			}
			if (_markerPackPkg.IsDownloading)
			{
				downloadText = "Downloading...";
				downloadEnabled = false;
				((Control)_deleteButton).set_Enabled(false);
			}
			_downloadButton.Text = downloadText;
			((Control)_downloadButton).set_Enabled(downloadEnabled);
			((Control)_downloadButton).set_Visible(!_markerPackPkg.IsDownloading && !_isUpToDate);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			UpdateControlStates();
			((Container)this).UpdateContainer(gameTime);
		}

		private void InfoButtonOnClick(object sender, MouseEventArgs e)
		{
			if (Url.IsValid(_markerPackPkg.Info))
			{
				Process.Start(_markerPackPkg.Info);
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			_hoverTick = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_hoverTick = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			((Control)this).OnMouseLeft(e);
		}

		private float GetHoverFade()
		{
			double duration = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _hoverTick;
			float offset = MathHelper.Lerp(0f, 0.4f, MathHelper.Clamp((float)(duration / 150.0), 0f, 1f));
			if (!((Control)this).get_MouseOver())
			{
				return 0.8f - offset;
			}
			return 0.4f + offset;
		}

		public override void RecalculateLayout()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			((Control)_downloadButton).set_Location(new Point(((Control)this).get_Width() - ((Control)_downloadButton).get_Width() - 10, ((Control)this).get_Height() - 20 - ((Control)_downloadButton).get_Height() / 2));
			((Control)_infoButton).set_Location(new Point(((Control)_downloadButton).get_Left() - ((Control)_infoButton).get_Width() - 5, ((Control)this).get_Height() - 20 - ((Control)_downloadButton).get_Height() / 2));
			((Control)_deleteButton).set_Location(new Point(((Control)_infoButton).get_Left() - ((Control)_deleteButton).get_Width() - 5, ((Control)this).get_Height() - 20 - ((Control)_downloadButton).get_Height() / 2));
			((Control)_keepUpdatedCheckbox).set_Location(new Point(((Control)this).get_Width() - ((Control)_keepUpdatedCheckbox).get_Width() - 20, 20));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureHeroBackground, new Rectangle(-9, -13, _textureHeroBackground.get_Width(), _textureHeroBackground.get_Height()), Color.get_White() * GetHoverFade());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _markerPackPkg.Name.Replace(" ", "  "), GameService.Content.get_DefaultFont18(), new Rectangle(20, 10, bounds.Width - 40, 40), Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _markerPackPkg.Description.Replace("\\n", "\n"), GameService.Content.get_DefaultFont14(), new Rectangle(20, 50, bounds.Width - 20, bounds.Height - 200), StandardColors.get_Default(), true, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _lastUpdateStr, GameService.Content.get_DefaultFont14(), new Rectangle(20, 10, ((Control)_keepUpdatedCheckbox).get_Left() - 40, 35), Colors.Chardonnay, false, (HorizontalAlignment)2, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, bounds.Height - 40, bounds.Width, 40), Color.get_Black() * 0.8f);
			if (_markerPackPkg.DownloadError == null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _markerPackPkg.Categories ?? "", GameService.Content.get_DefaultFont12(), new Rectangle(20, bounds.Height - 40, ((Control)_infoButton).get_Left() - 10, 40), StandardColors.get_Default(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			else
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Error: " + _markerPackPkg.DownloadError, GameService.Content.get_DefaultFont12(), new Rectangle(20, bounds.Height - 40, ((Control)_infoButton).get_Left() - 10, 40), StandardColors.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (_markerPackPkg.IsDownloading)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"{Math.Min(_markerPackPkg.DownloadProgress, 99)}%", GameService.Content.get_DefaultFont14(), ((Control)_downloadButton).get_LocalBounds(), StandardColors.get_Default(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			else if (_isUpToDate)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Up to Date", GameService.Content.get_DefaultFont14(), ((Control)_downloadButton).get_LocalBounds(), StandardColors.get_Default(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		protected override void DisposeControl()
		{
			_markerPackPkg.AutoUpdate.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)AutoUpdateOnSettingChanged);
			((Container)this).DisposeControl();
		}
	}
}
