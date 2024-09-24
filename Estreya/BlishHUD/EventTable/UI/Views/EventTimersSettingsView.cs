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
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersRemainingTextColor);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersStartsInTextColor);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersNameTextColor);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersRemainingTextColor);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersNextOccurenceTextColor);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.EventTimersRepeatTextColor);
			RenderEmptyLine((Panel)(object)parent);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.EventTimersRenderDistance);
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
