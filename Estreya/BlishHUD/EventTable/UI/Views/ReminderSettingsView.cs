using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ReminderSettingsView : BaseSettingsView
	{
		private static readonly Estreya.BlishHUD.EventTable.Models.Event _globalChangeTempEvent;

		private readonly Func<List<EventCategory>> _getEvents;

		private readonly ModuleSettings _moduleSettings;

		private StandardWindow _manageEventsWindow;

		private StandardWindow _manageReminderTimesWindow;

		static ReminderSettingsView()
		{
			_globalChangeTempEvent = new Estreya.BlishHUD.EventTable.Models.Event();
			_globalChangeTempEvent.UpdateReminderTimes(new TimeSpan[1] { TimeSpan.Zero });
		}

		public ReminderSettingsView(ModuleSettings moduleSettings, Func<List<EventCategory>> getEvents, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, settingEventService, font)
		{
			_moduleSettings = moduleSettings;
			_getEvents = getEvents;
		}

		protected override void BuildView(FlowPanel parent)
		{
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RemindersEnabled);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderPosition.X);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderPosition.Y);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderDuration);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderOpacity);
			RenderEmptyLine((Panel)(object)parent);
			RenderButton((Panel)(object)parent, base.TranslationService.GetTranslation("reminderSettingsView-btn-manageReminders", "Manage Reminders"), delegate
			{
				if (_manageEventsWindow == null)
				{
					_manageEventsWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Manage Events", ((object)this).GetType(), Guid.Parse("37e3f99c-f413-469c-b0f5-e2e6e31e4789"), base.IconService);
				}
				if (_manageEventsWindow.CurrentView != null)
				{
					(_manageEventsWindow.CurrentView as ManageEventsView).EventChanged -= ManageView_EventChanged;
				}
				ManageEventsView manageEventsView = new ManageEventsView(_getEvents(), new Dictionary<string, object> { 
				{
					"customActions",
					new List<ManageEventsView.CustomActionDefinition>
					{
						new ManageEventsView.CustomActionDefinition
						{
							Name = base.TranslationService.GetTranslation("reminderSettingsView-btn-changeTimes-title", "Change Times"),
							Tooltip = base.TranslationService.GetTranslation("reminderSettingsView-btn-changeTimes-tooltip", "Click to change the times at which reminders happen."),
							Icon = "1466345.png",
							Action = ManageReminderTimes
						}
					}
				} }, () => _moduleSettings.ReminderDisabledForEvents.get_Value(), _moduleSettings, base.APIManager, base.IconService, base.TranslationService);
				manageEventsView.EventChanged += ManageView_EventChanged;
				_manageEventsWindow.Show((IView)(object)manageEventsView);
			});
			RenderButton((Panel)(object)parent, base.TranslationService.GetTranslation("reminderSettingsView-btn-testReminder", "Test Reminder"), delegate
			{
				EventNotification eventNotification = new EventNotification(new Estreya.BlishHUD.EventTable.Models.Event
				{
					Name = "Test Event",
					Icon = "textures/maintenance.png"
				}, "Test description!", _moduleSettings.ReminderPosition.X.get_Value(), _moduleSettings.ReminderPosition.Y.get_Value(), base.IconService);
				eventNotification.BackgroundOpacity = _moduleSettings.ReminderOpacity.get_Value();
				eventNotification.Show(TimeSpan.FromSeconds(_moduleSettings.ReminderDuration.get_Value()));
			});
			RenderButton((Panel)(object)parent, base.TranslationService.GetTranslation("reminderSettingsView-btn-changeAllTimes", "Change all Reminder Times"), delegate
			{
				ManageReminderTimes(_globalChangeTempEvent);
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersOnMissingMumbleTicks);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersOnOpenMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInCombat);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvE_OpenWorld);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvE_Competetive);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInWvW);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvP);
		}

		private void ManageView_EventChanged(object sender, ManageEventsView.EventChangedArgs e)
		{
			_moduleSettings.ReminderDisabledForEvents.set_Value(e.NewService ? new List<string>(from s in _moduleSettings.ReminderDisabledForEvents.get_Value()
				where s != e.EventSettingKey
				select s) : new List<string>(_moduleSettings.ReminderDisabledForEvents.get_Value()) { e.EventSettingKey });
		}

		private void ManageReminderTimes(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (_manageReminderTimesWindow == null)
			{
				_manageReminderTimesWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Manage Reminder Times", ((object)this).GetType(), Guid.Parse("930702ac-bf87-416c-b5ba-cdf9e0266bf7"), base.IconService, base.IconService.GetIcon("1466345.png"));
			}
			_manageReminderTimesWindow.Size = new Point(450, ((Control)_manageReminderTimesWindow).get_Height());
			ManageReminderTimesView mrtv = _manageReminderTimesWindow?.CurrentView as ManageReminderTimesView;
			if (mrtv != null)
			{
				mrtv.CancelClicked -= ManageReminderTimesView_CancelClicked;
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>)>(ManageReminderTimesView_SaveClicked);
			}
			ManageReminderTimesView view = new ManageReminderTimesView(ev, base.APIManager, base.IconService, base.TranslationService);
			view.CancelClicked += ManageReminderTimesView_CancelClicked;
			view.SaveClicked += new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>)>(ManageReminderTimesView_SaveClicked);
			_manageReminderTimesWindow.Show((IView)(object)view);
		}

		private void ManageReminderTimesView_SaveClicked(object sender, (Estreya.BlishHUD.EventTable.Models.Event Event, List<TimeSpan> ReminderTimes) e)
		{
			if (e.Event == _globalChangeTempEvent)
			{
				foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in from ev in _getEvents().SelectMany((EventCategory ec) => ec.Events)
					where !ev.Filler
					select ev)
				{
					_moduleSettings.ReminderTimesOverride.get_Value()[ev2.SettingKey] = e.ReminderTimes;
					ev2.UpdateReminderTimes(e.ReminderTimes.ToArray());
				}
				_moduleSettings.ReminderTimesOverride.set_Value(new Dictionary<string, List<TimeSpan>>(_moduleSettings.ReminderTimesOverride.get_Value()));
				_globalChangeTempEvent.UpdateReminderTimes(e.ReminderTimes.ToArray());
			}
			else
			{
				_moduleSettings.ReminderTimesOverride.get_Value()[e.Event.SettingKey] = e.ReminderTimes;
				_moduleSettings.ReminderTimesOverride.set_Value(new Dictionary<string, List<TimeSpan>>(_moduleSettings.ReminderTimesOverride.get_Value()));
				e.Event.UpdateReminderTimes(e.ReminderTimes.ToArray());
			}
			StandardWindow manageReminderTimesWindow = _manageReminderTimesWindow;
			if (manageReminderTimesWindow != null)
			{
				((Control)manageReminderTimesWindow).Hide();
			}
		}

		private void ManageReminderTimesView_CancelClicked(object sender, EventArgs e)
		{
			StandardWindow manageReminderTimesWindow = _manageReminderTimesWindow;
			if (manageReminderTimesWindow != null)
			{
				((Control)manageReminderTimesWindow).Hide();
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void Unload()
		{
			base.Unload();
			if (_manageEventsWindow?.CurrentView != null)
			{
				(_manageEventsWindow.CurrentView as ManageEventsView).EventChanged -= ManageView_EventChanged;
			}
			StandardWindow manageEventsWindow = _manageEventsWindow;
			if (manageEventsWindow != null)
			{
				((Control)manageEventsWindow).Dispose();
			}
			_manageEventsWindow = null;
			ManageReminderTimesView mrtv = _manageReminderTimesWindow?.CurrentView as ManageReminderTimesView;
			if (mrtv != null)
			{
				mrtv.CancelClicked -= ManageReminderTimesView_CancelClicked;
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>)>(ManageReminderTimesView_SaveClicked);
			}
			StandardWindow manageReminderTimesWindow = _manageReminderTimesWindow;
			if (manageReminderTimesWindow != null)
			{
				((Control)manageReminderTimesWindow).Dispose();
			}
			_manageReminderTimesWindow = null;
		}
	}
}
