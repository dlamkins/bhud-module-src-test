using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.UI.Configs;

namespace Nekres.ProofLogix.Core.UI.LookingForOpener
{
	public class LfoView : View<LfoPresenter>
	{
		public LfoView(LfoConfig model)
		{
			base.WithPresenter(new LfoPresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Bottom(buildPanel.get_ContentRegion().Height);
			val.set_FlowDirection((ControlFlowDirection)0);
			((Control)val).set_Height(20);
			FlowPanel footer = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(buildPanel.get_ContentRegion().Height - ((Control)footer).get_Height());
			val2.set_CanScroll(true);
			val2.set_Title("Select an Encounter");
			Panel menuPanel = val2;
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent(buildPanel);
			((Panel)val3).set_ShowBorder(true);
			((Control)val3).set_Left(((Control)menuPanel).get_Width() + 4);
			((Control)val3).set_Width(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width());
			((Control)val3).set_Height(buildPanel.get_ContentRegion().Height - ((Control)footer).get_Height());
			ViewContainer resultContainer = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)footer);
			((Control)val4).set_Width(50);
			val4.set_StrokeText(true);
			val4.set_Text("Region:");
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Parent((Container)(object)footer);
			((Control)val5).set_Width(50);
			((Control)val5).set_Height(((Container)footer).get_ContentRegion().Height);
			Dropdown regionSelect = val5;
			string[] names = Enum.GetNames(typeof(Opener.ServerRegion));
			foreach (string region in names)
			{
				regionSelect.get_Items().Add(region);
			}
			regionSelect.set_SelectedItem(ProofLogix.Instance.LfoConfig.get_Value().Region.ToString());
			regionSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object _, ValueChangedEventArgs e)
			{
				base.get_Presenter().SetRegion(e.get_CurrentValue());
			});
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				((Control)menuPanel).set_Height(e.get_CurrentRegion().Height - ((Control)footer).get_Height());
				((Control)resultContainer).set_Height(e.get_CurrentRegion().Height - ((Control)footer).get_Height());
				((Control)resultContainer).set_Width(e.get_CurrentRegion().Width - ((Control)menuPanel).get_Width());
				((Control)footer).set_Width(e.get_CurrentRegion().Width);
			});
			Menu val6 = new Menu();
			((Control)val6).set_Parent((Container)(object)menuPanel);
			((Control)val6).set_Width(((Container)menuPanel).get_ContentRegion().Width);
			((Control)val6).set_Height(((Container)menuPanel).get_ContentRegion().Height);
			Menu menu = val6;
			((Container)menuPanel).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				((Control)menu).set_Height(e.get_CurrentRegion().Height);
				((Control)menu).set_Width(e.get_CurrentRegion().Width);
			});
			List<Raid.Wing> wings = ProofLogix.Instance.Resources.GetWings();
			int wingNr = 0;
			foreach (Raid.Wing item in wings)
			{
				wingNr++;
				MenuItem val7 = new MenuItem();
				((Control)val7).set_Parent((Container)(object)menu);
				val7.set_Text($"Wing {wingNr}");
				MenuItem wingItem = val7;
				((Control)wingItem).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ProofLogix.Instance.Resources.PlayMenuClick();
				});
				foreach (Raid.Wing.Event encounter in item.Events)
				{
					MenuItem val8 = new MenuItem();
					((Control)val8).set_Parent((Container)(object)wingItem);
					val8.set_Text(encounter.Name);
					val8.set_Icon(encounter.Icon);
					((Control)val8).set_Width(((Container)menu).get_ContentRegion().Width);
					((Control)val8).add_Click((EventHandler<MouseEventArgs>)async delegate
					{
						ProofLogix.Instance.Resources.PlayMenuItemClick();
						resultContainer.Show((IView)(object)new LoadingView("Searching…"));
						ViewContainer val9 = resultContainer;
						string id = encounter.Id;
						val9.Show((IView)(object)new LfoResultView(new LfoResults(id, await base.get_Presenter().GetOpener(encounter.Id))));
					});
				}
			}
			base.Build(buildPanel);
		}
	}
}
