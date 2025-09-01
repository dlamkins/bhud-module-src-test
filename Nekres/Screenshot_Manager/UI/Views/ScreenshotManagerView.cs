using System;
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
			base.WithPresenter(new ScreenshotManagerPresenter(this, model));
		}

		public ScreenshotManagerView()
		{
		}

		protected override async void Build(Container buildPanel)
		{
			ScreenshotManagerView screenshotManagerView = this;
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			int x = ((Rectangle)(ref contentRegion)).get_Size().X;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(new Point(x, ((Rectangle)(ref contentRegion)).get_Size().Y - 90));
			((Control)val).set_Location(new Point(0, 50));
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val).set_CanCollapse(false);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_Collapsed(false);
			((Panel)val).set_ShowTint(true);
			((Panel)val).set_ShowBorder(true);
			screenshotManagerView.ThumbnailFlowPanel = val;
			ScreenshotManagerView screenshotManagerView2 = this;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(((Control)ThumbnailFlowPanel).get_Location().X, ((Control)ThumbnailFlowPanel).get_Location().Y - 40));
			((Control)val2).set_Size(new Point(200, 40));
			((TextInputBase)val2).set_PlaceholderText(Resources.Search___);
			screenshotManagerView2.SearchBox = val2;
			((TextInputBase)SearchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>((Comparison<ResponsiveThumbnail>)base.get_Presenter().SortThumbnails);
			});
			foreach (string fileName in ((Presenter<ScreenshotManagerView, ScreenshotManagerModel>)base.get_Presenter()).get_Model().FileWatcherFactory.Index)
			{
				AsyncTexture2D texture = new AsyncTexture2D();
				base.get_Presenter().CreateThumbnail(ThumbnailFlowPanel, texture, fileName);
				await base.get_Presenter().LoadTexture(texture, fileName);
				ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>((Comparison<ResponsiveThumbnail>)base.get_Presenter().SortThumbnails);
			}
		}

		protected override void Unload()
		{
		}
	}
}
