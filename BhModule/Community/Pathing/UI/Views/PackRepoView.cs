using BhModule.Community.Pathing.UI.Presenter;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Views
{
	public class PackRepoView : View
	{
		public FlowPanel RepoFlowPanel { get; private set; }

		public PackRepoView()
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new PackRepoPresenter(this, PathingModule.Instance.MarkerPackRepo));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Control)val).set_Top(0);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(0f, 15f));
			val.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val).set_Parent(buildPanel);
			RepoFlowPanel = val;
		}
	}
}
