using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace FarmingTracker
{
	public class DebugTabView : View
	{
		private readonly Model _model;

		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		public DebugTabView(Model model, Services services)
			: this()
		{
			_model = model;
			_services = services;
		}

		protected override void Unload()
		{
			FlowPanel? rootFlowPanel = _rootFlowPanel;
			if (rootFlowPanel != null)
			{
				((Control)rootFlowPanel).Dispose();
			}
			_rootFlowPanel = null;
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(20f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			new HintLabel((Container?)(object)_rootFlowPanel, "If you can see this tab, you are running a debug instead of a release version of this module.\nDo not change any settings here. They will not speed up or improve anything.\nThey will rather break the module.\nThis tab just helps the developer to test the module. :-)");
			CreateDrfDebugPanel((Container)(object)_rootFlowPanel);
			_services.DateTimeService.CreateDateTimeDebugPanel((Container)(object)_rootFlowPanel);
		}

		private void CreateDrfDebugPanel(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title("DRF Debug");
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.3f);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			FlowPanel debugDrfFlowPanel = val;
			new SettingControl((Container)(object)debugDrfFlowPanel, (SettingEntry)(object)_services.SettingService.IsFakeDrfServerUsedSetting);
			StandardButton val2 = new StandardButton();
			val2.set_Text("Copy stats to clipboard as DRF drop");
			((Control)val2).set_Width(300);
			((Control)val2).set_Parent((Container)(object)debugDrfFlowPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				string drfMessageAsString = JsonConvert.SerializeObject((object)ConvertToDrfMessage(_model));
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync("'" + drfMessageAsString + "',");
			});
		}

		private static DrfMessage ConvertToDrfMessage(Model model)
		{
			StatsSnapshot statsSnapshot = model.Stats.StatsSnapshot;
			List<Stat> items = statsSnapshot.ItemById.Values.ToList();
			List<Stat> currencies = statsSnapshot.CurrencyById.Values.ToList();
			DrfMessage drfMessage = new DrfMessage();
			drfMessage.Kind = "data";
			drfMessage.Payload.Character = "1";
			foreach (Stat item in items.Where((Stat s) => s.Count != 0))
			{
				drfMessage.Payload.Drop.Items.Add(item.ApiId, item.Count);
			}
			foreach (Stat currency in currencies.Where((Stat s) => s.Count != 0).Take(10))
			{
				drfMessage.Payload.Drop.Currencies.Add(currency.ApiId, currency.Count);
			}
			return drfMessage;
		}
	}
}
