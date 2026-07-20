using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EventTimersSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Func<Task<List<EventCategory>>> _getAllEvents;

		private readonly AccountService _accountService;

		private StandardWindow _manageEventsWindow;

		public EventTimersSettingsView(ModuleSettings moduleSettings, Func<Task<List<EventCategory>>> getAllEvents, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, AccountService accountService)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
			_getAllEvents = getAllEvents;
			_accountService = accountService;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderButtonAsync((Panel)(object)parent, base.TranslationService.GetTranslation("eventTimersSettingsView-btn-manageEvents", "Manage Events"), async delegate
			{
				if (_manageEventsWindow == null)
				{
					_manageEventsWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Manage Events", ((object)this).GetType(), Guid.Parse("328bf66c-364e-40ae-9ffc-140e002afb32"), base.IconService);
					((Control)_manageEventsWindow).set_Width(1060);
				}
				if (_manageEventsWindow.CurrentView != null)
				{
					(_manageEventsWindow.CurrentView as ManageEventsView).EventChanged -= ManageView_EventChanged;
				}
				ManageEventsView view = new ManageEventsView(await _getAllEvents(), null, () => _moduleSettings.DisabledEventTimerSettingKeys.get_Value(), _moduleSettings, _accountService, base.APIManager, base.IconService, base.TranslationService);
				view.EventChanged += ManageView_EventChanged;
				await _manageEventsWindow.Show((IView)(object)view);
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowEventTimersOnMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowEventTimersInWorld);
			RenderEmptyLine((Panel)(object)parent);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.ShowEventTimersOnMapKeybinding);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.ShowEventTimersInWorldKeybinding);
			RenderEmptyLine((Panel)(object)parent);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.EventTimersRenderDistance);
			RenderEmptyLine((Panel)(object)parent);
			RenderNameSection(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderDurationSection(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderRepeatSection(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderRemainingSection(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderStartsInSection(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderNextOccurrenceSection(parent);
		}

		private void RenderNameSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Name Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersNameTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersNameTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersNameTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private void RenderDurationSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Duration Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersDurationTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersDurationTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersDurationTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private void RenderRepeatSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Repeat Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersRepeatTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersRepeatTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersRepeatTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private void RenderRemainingSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Remaining Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersRemainingTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersRemainingTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersRemainingTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private void RenderStartsInSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Starts in Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersStartsInTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersStartsInTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersStartsInTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private void RenderNextOccurrenceSection(FlowPanel parent)
		{
			FlowPanel panel = GetTextSectionPanel(parent, "Next Occurrence Section");
			RenderColorSetting((Panel)(object)panel, _moduleSettings.EventTimersNextOccurenceTextColor).label.set_Text("Text Color");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersNextOccurrenceTextureWidth).label.set_Text("X Resolution");
			RenderIntSetting((Panel)(object)panel, _moduleSettings.EventTimersNextOccurrenceTextureHeight).label.set_Text("Y Resolution");
			RenderEmptyLine((Panel)(object)panel, 20);
		}

		private static FlowPanel GetTextSectionPanel(FlowPanel parent, string title)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Panel)val).set_Title(title);
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - (int)(parent.get_OuterControlPadding().X * 2.5f));
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			return val;
		}

		private void ManageView_EventChanged(object sender, ManageEventsView.EventChangedArgs e)
		{
			_moduleSettings.DisabledEventTimerSettingKeys.set_Value(e.NewState ? new List<string>(from x in _moduleSettings.DisabledEventTimerSettingKeys.get_Value()
				where x != e.EventSettingKey
				select x) : new List<string>(_moduleSettings.DisabledEventTimerSettingKeys.get_Value()) { e.EventSettingKey });
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void Unload()
		{
			base.Unload();
			StandardWindow manageEventsWindow = _manageEventsWindow;
			if (manageEventsWindow != null)
			{
				((Control)manageEventsWindow).Dispose();
			}
			_manageEventsWindow = null;
		}
	}
}
