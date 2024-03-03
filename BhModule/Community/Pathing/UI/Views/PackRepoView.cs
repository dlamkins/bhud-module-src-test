using System;
using BhModule.Community.Pathing.UI.Controls;
using BhModule.Community.Pathing.UI.Presenter;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Views
{
	public class PackRepoView : View
	{
		private TextBox _searchBox;

		public FlowPanel RepoFlowPanel { get; private set; }

		public PackRepoView(PathingModule module)
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new PackRepoPresenter(this, module));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search marker packs...");
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(20, 10));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 40);
			_searchBox = val;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchBoxTextChanged);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height - ((Control)_searchBox).get_Bottom() - 12));
			((Control)val2).set_Top(((Control)_searchBox).get_Bottom() + 12);
			((Panel)val2).set_CanScroll(true);
			val2.set_ControlPadding(new Vector2(0f, 15f));
			val2.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val2).set_Parent(buildPanel);
			RepoFlowPanel = val2;
		}

		private void SearchBoxTextChanged(object sender, EventArgs e)
		{
			string searchText = ((TextInputBase)_searchBox).get_Text().ToLowerInvariant();
			RepoFlowPanel.FilterChildren<MarkerPackHero>((Func<MarkerPackHero, bool>)((MarkerPackHero hero) => hero.MarkerPackPkg.Name.ToLowerInvariant().Contains(searchText) || (hero.MarkerPackPkg.Description ?? "").ToLowerInvariant().Contains(searchText) || (hero.MarkerPackPkg.Categories ?? "").ToLowerInvariant().Contains(searchText)));
		}
	}
}
