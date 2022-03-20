using System;
using System.Diagnostics;
using BhModule.Community.Pathing.UI.Presenter;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Flurl;
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

		private readonly PackRepoPresenter.MarkerPackPkg _markerPackPkg;

		private readonly BlueButton _downloadButton;

		private readonly BlueButton _infoButton;

		private double _hoverTick;

		static MarkerPackHero()
		{
			_textureHeroBackground = PathingModule.Instance.ContentsManager.GetTexture("png\\controls\\155209.png");
		}

		public MarkerPackHero(PackRepoPresenter.MarkerPackPkg markerPackPkg)
			: this()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			_markerPackPkg = markerPackPkg;
			((Control)this).SuspendLayout();
			BlueButton obj = new BlueButton
			{
				Text = Strings.Repo_Download
			};
			((Control)obj).set_Width(90);
			((Control)obj).set_Parent((Container)(object)this);
			_downloadButton = obj;
			BlueButton obj2 = new BlueButton
			{
				Text = Strings.Repo_Info
			};
			((Control)obj2).set_Width(90);
			((Control)obj2).set_Visible(_markerPackPkg.Info != null);
			((Control)obj2).set_Parent((Container)(object)this);
			_infoButton = obj2;
			((Control)_downloadButton).add_Click((EventHandler<MouseEventArgs>)DownloadButtonOnClick);
			((Control)_infoButton).add_Click((EventHandler<MouseEventArgs>)InfoButtonOnClick);
			((Control)this).set_Size(new Point(500, 170));
			((Control)this).set_Padding(new Thickness(13f, 0f, 0f, 9f));
			((Control)this).ResumeLayout(true);
		}

		private void DownloadButtonOnClick(object sender, MouseEventArgs e)
		{
			((Control)_downloadButton).set_Enabled(false);
			_downloadButton.Text = "Downloading...";
			PackHandlingUtil.DownloadPack(_markerPackPkg);
			_downloadButton.Text = "Downloaded";
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
			((Control)_downloadButton).set_Location(new Point(((Control)this).get_Width() - ((Control)_downloadButton).get_Width() - 10, ((Control)this).get_Height() - 20 - ((Control)_downloadButton).get_Height() / 2));
			((Control)_infoButton).set_Location(new Point(((Control)_downloadButton).get_Left() - ((Control)_infoButton).get_Width() - 5, ((Control)this).get_Height() - 20 - ((Control)_downloadButton).get_Height() / 2));
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
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureHeroBackground, new Rectangle(-9, -13, _textureHeroBackground.get_Width(), _textureHeroBackground.get_Height()), Color.get_White() * GetHoverFade());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _markerPackPkg.Name.Replace(" ", "  "), GameService.Content.get_DefaultFont18(), new Rectangle(20, 10, bounds.Width - 40, 40), Colors.Chardonnay, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _markerPackPkg.Description.Replace("\\n", "\n"), GameService.Content.get_DefaultFont14(), new Rectangle(20, 50, bounds.Width - 40, bounds.Height - 200), StandardColors.get_Default(), true, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, bounds.Height - 40, bounds.Width, 40), Color.get_Black() * 0.8f);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Strings.Repo_Categories + ": " + _markerPackPkg.Categories, GameService.Content.get_DefaultFont12(), new Rectangle(20, bounds.Height - 40, ((Control)_infoButton).get_Left() - 10, 40), StandardColors.get_Default(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
