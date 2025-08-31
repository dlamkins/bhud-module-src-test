using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Nekres.Screenshot_Manager.Properties;
using Nekres.Screenshot_Manager.UI.Controls;
using Nekres.Screenshot_Manager.UI.Models;
using Nekres.Screenshot_Manager.UI.Presenters;

namespace Nekres.Screenshot_Manager.UI.Views
{
	public class ScreenshotManagerView : View<ScreenshotManagerPresenter>
	{
		public FlowPanel ThumbnailFlowPanel { get; private set; }

		public TextBox SearchBox { get; private set; }

		public ScreenshotManagerView(ScreenshotManagerModel model)
		{
			WithPresenter(new ScreenshotManagerPresenter(this, model));
		}

		public ScreenshotManagerView()
		{
		}

		protected override async void Build(Container buildPanel)
		{
			ScreenshotManagerView screenshotManagerView = this;
			FlowPanel obj = new FlowPanel
			{
				Parent = buildPanel
			};
			Rectangle contentRegion = buildPanel.ContentRegion;
			int x = ((Rectangle)(ref contentRegion)).get_Size().X;
			contentRegion = buildPanel.ContentRegion;
			obj.Size = new Point(x, ((Rectangle)(ref contentRegion)).get_Size().Y - 90);
			obj.Location = new Point(0, 50);
			obj.FlowDirection = ControlFlowDirection.LeftToRight;
			obj.ControlPadding = new Vector2(5f, 5f);
			obj.CanCollapse = false;
			obj.CanScroll = true;
			obj.Collapsed = false;
			obj.ShowTint = true;
			obj.ShowBorder = true;
			screenshotManagerView.ThumbnailFlowPanel = obj;
			SearchBox = new TextBox
			{
				Parent = buildPanel,
				Location = new Point(ThumbnailFlowPanel.Location.X, ThumbnailFlowPanel.Location.Y - 40),
				Size = new Point(200, 40),
				PlaceholderText = Resources.Search___
			};
			SearchBox.TextChanged += delegate
			{
				ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>(base.Presenter.SortThumbnails);
			};
			foreach (string fileName in base.Presenter.Model.FileWatcherFactory.Index)
			{
				AsyncTexture2D texture = new AsyncTexture2D();
				base.Presenter.CreateThumbnail(ThumbnailFlowPanel, texture, fileName);
				await base.Presenter.LoadTexture(texture, fileName);
				ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>(base.Presenter.SortThumbnails);
			}
		}

		protected override void Unload()
		{
		}
	}
}
