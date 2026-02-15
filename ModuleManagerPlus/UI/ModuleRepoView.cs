using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Flurl.Http;
using Microsoft.Xna.Framework;
using ModuleManagerPlus.Data;
using ModuleManagerPlus.Services;

namespace ModuleManagerPlus.UI
{
	internal class ModuleRepoView : IView
	{
		public FlowPanel _panel;

		private readonly TextureLoader _textureLoader;

		private readonly ModuleInstallService _installService;

		private readonly TextBox _searchBox;

		private readonly Label _sortLbl;

		private readonly Dropdown _sortOrderDd;

		private readonly Checkbox _prereleaseCb;

		private readonly StandardButton _updateAllBttn;

		private readonly Label _moduleCountLbl;

		public event EventHandler<EventArgs> Loaded;

		public event EventHandler<EventArgs> Built;

		public event EventHandler<EventArgs> Unloaded;

		public ModuleRepoView(ContentsManager contentsManager)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			_textureLoader = new TextureLoader(contentsManager);
			_installService = new ModuleInstallService();
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search Modules...");
			_searchBox = val;
			Label val2 = new Label();
			val2.set_Text("Sort");
			val2.set_AutoSizeWidth(true);
			_sortLbl = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Width(150);
			val3.set_SelectedItem("Downloads");
			_sortOrderDd = val3;
			_sortOrderDd.get_Items().Add("Downloads");
			_sortOrderDd.get_Items().Add("A to Z");
			_sortOrderDd.get_Items().Add("Z to A");
			Checkbox val4 = new Checkbox();
			val4.set_Text("Show Prereleases");
			_prereleaseCb = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Update All");
			((Control)val5).set_Width(96);
			_updateAllBttn = val5;
			Label val6 = new Label();
			val6.set_Text("Loading modules...");
			val6.set_AutoSizeWidth(true);
			_moduleCountLbl = val6;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)_searchBox_TextChanged);
			_sortOrderDd.add_ValueChanged((EventHandler<ValueChangedEventArgs>)_sortOrderDd_ValueChanged);
			_prereleaseCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)_prereleaseCb_CheckedChanged);
			((Control)_updateAllBttn).add_Click((EventHandler<MouseEventArgs>)_updateAllBttn_Click);
			FlowPanel val7 = new FlowPanel();
			val7.set_OuterControlPadding(new Vector2(0f, 0f));
			val7.set_ControlPadding(new Vector2(0f, 30f));
			((Container)val7).set_HeightSizingMode((SizingMode)0);
			((Container)val7).set_WidthSizingMode((SizingMode)0);
			_panel = val7;
		}

		private void UpdateModuleFilter()
		{
			string searchNeedle = ((TextInputBase)_searchBox).get_Text().ToLowerInvariant();
			_panel.FilterChildren<HModuleCard>((Func<HModuleCard, bool>)((HModuleCard mc) => (string.IsNullOrWhiteSpace(searchNeedle) || mc.Module.Name.ToLowerInvariant().Contains(searchNeedle) || mc.Module.Description.Contains(searchNeedle)) && (_prereleaseCb.get_Checked() || mc.Module.Releases.Any((Release r) => !r.IsPrerelease))));
			_moduleCountLbl.set_Text($"Showing {(from card in ((Container)_panel).GetChildrenOfType<HModuleCard>()
				where ((Control)card).get_Visible()
				select card).Count()} Modules");
		}

		private void UpdateModuleSortOrder()
		{
			string selectedItem = _sortOrderDd.get_SelectedItem();
			switch (selectedItem)
			{
			default:
				_ = selectedItem == "Last Updated";
				break;
			case "Downloads":
				_panel.SortChildren<HModuleCard>((Comparison<HModuleCard>)((HModuleCard a, HModuleCard b) => b.Module.TotalDownloads.CompareTo(a.Module.TotalDownloads)));
				break;
			case "A to Z":
				_panel.SortChildren<HModuleCard>((Comparison<HModuleCard>)((HModuleCard a, HModuleCard b) => string.Compare(a.Module.Name, b.Module.Name, StringComparison.OrdinalIgnoreCase)));
				break;
			case "Z to A":
				_panel.SortChildren<HModuleCard>((Comparison<HModuleCard>)((HModuleCard a, HModuleCard b) => string.Compare(b.Module.Name, a.Module.Name, StringComparison.OrdinalIgnoreCase)));
				break;
			}
		}

		private void _sortOrderDd_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			UpdateModuleSortOrder();
		}

		private void _searchBox_TextChanged(object sender, EventArgs e)
		{
			UpdateModuleFilter();
		}

		public void DoBuild(Container buildPanel)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			((Control)_sortLbl).set_Parent(buildPanel);
			((Control)_sortOrderDd).set_Parent(buildPanel);
			int sortAreaWidth = ((Control)_sortLbl).get_Width() + 5 + ((Control)_sortOrderDd).get_Width();
			((Control)_searchBox).set_Width(((Control)buildPanel).get_Width() - sortAreaWidth - 10);
			((Control)_searchBox).set_Parent(buildPanel);
			((Control)_sortLbl).set_Location(new Point(((Control)_searchBox).get_Right() + 10, ((Control)_searchBox).get_Top() + ((Control)_searchBox).get_Height() / 2 - ((Control)_sortLbl).get_Height() / 2));
			((Control)_sortOrderDd).set_Location(new Point(((Control)_sortLbl).get_Right() + 5, ((Control)_searchBox).get_Top() + ((Control)_searchBox).get_Height() / 2 - ((Control)_sortOrderDd).get_Height() / 2));
			((Control)_moduleCountLbl).set_Location(new Point(((Control)_searchBox).get_Left() + 5, ((Control)_searchBox).get_Bottom() + 10));
			((Control)_moduleCountLbl).set_Parent(buildPanel);
			((Control)_updateAllBttn).set_Location(new Point(((Control)_sortOrderDd).get_Right() - ((Control)_updateAllBttn).get_Width(), ((Control)_searchBox).get_Bottom() + 10));
			((Control)_updateAllBttn).set_Parent(buildPanel);
			((Control)_prereleaseCb).set_Location(new Point(((Control)_updateAllBttn).get_Left() - ((Control)_prereleaseCb).get_Width() - 5, ((Control)_updateAllBttn).get_Bottom() - ((Control)_updateAllBttn).get_Height() / 2 - ((Control)_prereleaseCb).get_Height() / 2));
			((Control)_prereleaseCb).set_Parent(buildPanel);
			((Control)_panel).set_Top(((Control)_moduleCountLbl).get_Bottom() + 20);
			((Control)_panel).set_Parent(buildPanel);
			((Control)_panel).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height() - ((Control)_moduleCountLbl).get_Bottom() - 20));
			((Panel)_panel).set_CanScroll(true);
			this.Built?.Invoke(this, EventArgs.Empty);
		}

		public async Task<bool> DoLoad(IProgress<string> progress)
		{
			try
			{
				progress.Report("Fetching module data...");
				PkgRoot pkgRoot = await "https://pkgs.blishhud.com/packagesv2.json".GetJsonAsync<PkgRoot>(default(CancellationToken), (HttpCompletionOption)0);
				progress.Report("Loading UI...");
				((Control)_panel).SuspendLayout();
				foreach (Module module in pkgRoot.Modules.OrderByDescending((Module m) => m.TotalDownloads))
				{
					if (!(module.Namespace == "fs.modulemanagerplus"))
					{
						((Control)new HModuleCard(module, pkgRoot.Authors[module.AuthorId], _textureLoader, _installService)).set_Parent((Container)(object)_panel);
					}
				}
				((Control)_panel).ResumeLayout(true);
				UpdateModuleSortOrder();
				UpdateModuleFilter();
				this.Built?.Invoke(this, EventArgs.Empty);
				return true;
			}
			catch (Exception ex)
			{
				progress.Report("Loading repo failed: " + ex.Message);
				return false;
			}
			finally
			{
				progress.Report("");
			}
		}

		private void _prereleaseCb_CheckedChanged(object sender, CheckChangedEvent e)
		{
			bool showPrereleases = _prereleaseCb.get_Checked();
			foreach (HModuleCard item in ((IEnumerable)((Container)_panel).get_Children()).OfType<HModuleCard>())
			{
				item.PopulateVersionDropdown(showPrereleases);
			}
			UpdateModuleFilter();
		}

		private async void _updateAllBttn_Click(object sender, MouseEventArgs e)
		{
			((Control)_updateAllBttn).set_Enabled(false);
			_updateAllBttn.set_Text("Updating...");
			List<HModuleCard> cardsToUpdate = (from c in ((IEnumerable)((Container)_panel).get_Children()).OfType<HModuleCard>()
				where c.HasUpdate
				select c).ToList();
			int updated = 0;
			foreach (HModuleCard item in cardsToUpdate)
			{
				if (await item.PerformUpdate())
				{
					updated++;
				}
			}
			_updateAllBttn.set_Text("Update All");
			((Control)_updateAllBttn).set_Enabled(true);
		}

		public void DoUnload()
		{
			this.Unloaded?.Invoke(this, EventArgs.Empty);
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((TextInputBase)searchBox).remove_TextChanged((EventHandler<EventArgs>)_searchBox_TextChanged);
			}
			Dropdown sortOrderDd = _sortOrderDd;
			if (sortOrderDd != null)
			{
				sortOrderDd.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)_sortOrderDd_ValueChanged);
			}
			Checkbox prereleaseCb = _prereleaseCb;
			if (prereleaseCb != null)
			{
				prereleaseCb.remove_CheckedChanged((EventHandler<CheckChangedEvent>)_prereleaseCb_CheckedChanged);
			}
			StandardButton updateAllBttn = _updateAllBttn;
			if (updateAllBttn != null)
			{
				((Control)updateAllBttn).remove_Click((EventHandler<MouseEventArgs>)_updateAllBttn_Click);
			}
			_textureLoader?.Unload();
		}
	}
}
