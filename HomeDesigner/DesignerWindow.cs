using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using HomeDesigner.Views;
using Microsoft.Xna.Framework;

namespace HomeDesigner
{
	public class DesignerWindow : TabbedWindow2
	{
		private RendererControl rendererControl;

		private BlueprintRenderer blueprintRenderer;

		private ContentsManager contents;

		private Tab mergerTab;

		private Tab differTab;

		private Tab designerTab;

		public DesignerView designerView;

		public TemplateMergerView templateMergerView;

		public TemplateDifferenceView templateDifferenceView;

		public DesignerWindow(ContentsManager contents, RendererControl rendererControl, BlueprintRenderer blueprintRenderer)
			: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 700), new Rectangle(70, 40, 865, 650))
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			this.rendererControl = rendererControl;
			this.blueprintRenderer = blueprintRenderer;
			this.contents = contents;
			((WindowBase2)this).set_Title("Home Designer");
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(700, 750));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_Id("HomeDesigner.MainWindow");
			((Control)this).set_ZIndex(0);
			designerView = new DesignerView(rendererControl, blueprintRenderer, contents);
			templateMergerView = new TemplateMergerView(contents);
			templateDifferenceView = new TemplateDifferenceView(contents);
			designerTab = new Tab(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Designer.png")), (Func<IView>)(() => (IView)(object)designerView), "Designer", (int?)null);
			mergerTab = new Tab(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Merge.png")), (Func<IView>)(() => (IView)(object)templateMergerView), "Template Merger", (int?)null);
			differTab = new Tab(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Differ.png")), (Func<IView>)(() => (IView)(object)templateDifferenceView), "Template Cutter", (int?)null);
			((TabbedWindow2)this).get_Tabs().Add(designerTab);
			((TabbedWindow2)this).get_Tabs().Add(mergerTab);
			((TabbedWindow2)this).get_Tabs().Add(differTab);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)resized);
		}

		private void resized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			RectangleExtension.WithSetDimension(((Container)this).get_ContentRegion(), (int?)(((WindowBase2)this).get_WindowRegion().X + 30), (int?)(((WindowBase2)this).get_WindowRegion().Y + 40), (int?)(((WindowBase2)this).get_WindowRegion().Width - 60), (int?)(((WindowBase2)this).get_WindowRegion().Height - 80));
		}

		public void unload()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)resized);
			designerView?.unload();
			AsyncTexture2D icon = designerTab.get_Icon();
			if (icon != null)
			{
				icon.Dispose();
			}
			AsyncTexture2D icon2 = mergerTab.get_Icon();
			if (icon2 != null)
			{
				icon2.Dispose();
			}
			AsyncTexture2D icon3 = differTab.get_Icon();
			if (icon3 != null)
			{
				icon3.Dispose();
			}
			((View<IPresenter>)(object)designerView)?.DoUnload();
			((View<IPresenter>)(object)templateMergerView)?.DoUnload();
			((View<IPresenter>)(object)templateDifferenceView)?.DoUnload();
		}
	}
}
