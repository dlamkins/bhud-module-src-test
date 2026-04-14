using System;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Controls;
using BhModule.Community.Pathing.UI.Presenter;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Views
{
	public class PackRepoView : View
	{
		private TextBox _searchBox;

		private Checkbox _onlyShowCurrentMap;

		private int _currentMap = -1;

		public FlowPanel RepoFlowPanel { get; private set; }

		public PackRepoView(PathingModule module)
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new PackRepoPresenter(this, module));
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			_currentMap = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapMapChanged);
			return ((View<IPresenter>)this).Load(progress);
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
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search marker packs...");
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(20, 10));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 40);
			_searchBox = val;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchBoxTextChanged);
			Checkbox val2 = new Checkbox();
			val2.set_Text("Current Map Only");
			((Control)val2).set_BasicTooltipText("If checked, only marker packs with marker or trails for the current map will be shown.");
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(((Control)_searchBox).get_Left() + 8, ((Control)_searchBox).get_Bottom() + 8));
			_onlyShowCurrentMap = val2;
			_onlyShowCurrentMap.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnlyShowCurrentMapCheckedChanged);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height - ((Control)_onlyShowCurrentMap).get_Bottom() - 12));
			((Control)val3).set_Top(((Control)_onlyShowCurrentMap).get_Bottom() + 12);
			((Panel)val3).set_CanScroll(true);
			val3.set_ControlPadding(new Vector2(0f, 15f));
			val3.set_OuterControlPadding(new Vector2(20f, 5f));
			((Control)val3).set_Parent(buildPanel);
			RepoFlowPanel = val3;
		}

		private void CurrentMapMapChanged(object sender, ValueEventArgs<int> e)
		{
			_currentMap = e.get_Value();
			UpdateFilter();
		}

		private void OnlyShowCurrentMapCheckedChanged(object sender, CheckChangedEvent e)
		{
			UpdateFilter();
		}

		private void SearchBoxTextChanged(object sender, EventArgs e)
		{
			UpdateFilter();
		}

		private void UpdateFilter()
		{
			string searchText = ((TextInputBase)_searchBox).get_Text().ToLowerInvariant();
			RepoFlowPanel.FilterChildren<MarkerPackHero>((Func<MarkerPackHero, bool>)((MarkerPackHero hero) => (hero.MarkerPackPkg.Name.ToLowerInvariant().Contains(searchText) || (hero.MarkerPackPkg.Description ?? "").ToLowerInvariant().Contains(searchText) || (hero.MarkerPackPkg.Categories ?? "").ToLowerInvariant().Contains(searchText)) && (!_onlyShowCurrentMap.get_Checked() || hero.MarkerPackPkg.MapIds.Contains(_currentMap))));
		}

		protected override void Unload()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMapMapChanged);
			((View<IPresenter>)this).Unload();
		}
	}
}
