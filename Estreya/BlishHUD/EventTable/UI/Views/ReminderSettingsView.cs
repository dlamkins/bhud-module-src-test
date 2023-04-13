using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ReminderSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Func<List<EventCategory>> _getEvents;

		private StandardWindow _manageEventsWindow;

		private StandardWindow _manageReminderTimesWindow;

		private static Estreya.BlishHUD.EventTable.Models.Event _globalChangeTempEvent;

		static ReminderSettingsView()
		{
			_globalChangeTempEvent = new Estreya.BlishHUD.EventTable.Models.Event();
			_globalChangeTempEvent.UpdateReminderTimes(new TimeSpan[1] { TimeSpan.Zero });
		}

		public ReminderSettingsView(ModuleSettings moduleSettings, Func<List<EventCategory>> getEvents, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, settingEventState, font)
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
			RenderButton((Panel)(object)parent, base.TranslationState.GetTranslation("reminderSettingsView-manageReminders-btn", "Manage Reminders"), delegate
			{
				if (_manageEventsWindow == null)
				{
					_manageEventsWindow = WindowUtil.CreateStandardWindow("Manage Events", ((object)this).GetType(), Guid.Parse("37e3f99c-f413-469c-b0f5-e2e6e31e4789"), base.IconState);
				}
				if (((WindowBase2)_manageEventsWindow).get_CurrentView() != null)
				{
					(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageEventsView).EventChanged -= ManageView_EventChanged;
				}
				ManageEventsView manageEventsView = new ManageEventsView(_getEvents(), new Dictionary<string, object> { 
				{
					"customActions",
					new List<ManageEventsView.CustomActionDefinition>
					{
						new ManageEventsView.CustomActionDefinition
						{
							Name = "Change Times",
							Tooltip = "Click to change the times at which reminders happen.",
							Icon = "1466345.png",
							Action = delegate(Estreya.BlishHUD.EventTable.Models.Event ev)
							{
								ManageReminderTimes(ev);
							}
						}
					}
				} }, () => _moduleSettings.ReminderDisabledForEvents.get_Value(), _moduleSettings, base.APIManager, base.IconState, base.TranslationState);
				manageEventsView.EventChanged += ManageView_EventChanged;
				_manageEventsWindow.Show((IView)(object)manageEventsView);
			});
			RenderButton((Panel)(object)parent, base.TranslationState.GetTranslation("reminderSettingsView-testReminder-btn", "Test Reminder"), delegate
			{
				EventNotification eventNotification = new EventNotification(new Estreya.BlishHUD.EventTable.Models.Event
				{
					Name = "Test Event",
					Icon = "textures/maintenance.png"
				}, "Test description!", _moduleSettings.ReminderPosition.X.get_Value(), _moduleSettings.ReminderPosition.Y.get_Value(), base.IconState);
				eventNotification.BackgroundOpacity = _moduleSettings.ReminderOpacity.get_Value();
				eventNotification.Show(TimeSpan.FromSeconds(_moduleSettings.ReminderDuration.get_Value()));
			});
			RenderButton((Panel)(object)parent, "Change all Reminder Times", delegate
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
			_moduleSettings.ReminderDisabledForEvents.set_Value(e.NewState ? new List<string>(from s in _moduleSettings.ReminderDisabledForEvents.get_Value()
				where s != e.EventSettingKey
				select s) : new List<string>(_moduleSettings.ReminderDisabledForEvents.get_Value()) { e.EventSettingKey });
		}

		private void ManageReminderTimes(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (_manageReminderTimesWindow == null)
			{
				_manageReminderTimesWindow = WindowUtil.CreateStandardWindow("Manage Reminder Times", ((object)this).GetType(), Guid.Parse("930702ac-bf87-416c-b5ba-cdf9e0266bf7"), base.IconState, base.IconState.GetIcon("1466345.png"));
			}
			((Control)_manageReminderTimesWindow).set_Size(new Point(450, ((Control)_manageReminderTimesWindow).get_Height()));
			StandardWindow manageReminderTimesWindow = _manageReminderTimesWindow;
			ManageReminderTimesView mrtv = ((manageReminderTimesWindow != null) ? ((WindowBase2)manageReminderTimesWindow).get_CurrentView() : null) as ManageReminderTimesView;
			if (mrtv != null)
			{
				mrtv.CancelClicked -= ManageReminderTimesView_CancelClicked;
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>)>(ManageReminderTimesView_SaveClicked);
			}
			ManageReminderTimesView view = new ManageReminderTimesView(ev, base.APIManager, base.IconState, base.TranslationState);
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
			StandardWindow manageEventsWindow = _manageEventsWindow;
			if (((manageEventsWindow != null) ? ((WindowBase2)manageEventsWindow).get_CurrentView() : null) != null)
			{
				(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageEventsView).EventChanged -= ManageView_EventChanged;
			}
			StandardWindow manageEventsWindow2 = _manageEventsWindow;
			if (manageEventsWindow2 != null)
			{
				((Control)manageEventsWindow2).Dispose();
			}
			_manageEventsWindow = null;
			StandardWindow manageReminderTimesWindow = _manageReminderTimesWindow;
			ManageReminderTimesView mrtv = ((manageReminderTimesWindow != null) ? ((WindowBase2)manageReminderTimesWindow).get_CurrentView() : null) as ManageReminderTimesView;
			if (mrtv != null)
			{
				mrtv.CancelClicked -= ManageReminderTimesView_CancelClicked;
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>)>(ManageReminderTimesView_SaveClicked);
			}
			StandardWindow manageReminderTimesWindow2 = _manageReminderTimesWindow;
			if (manageReminderTimesWindow2 != null)
			{
				((Control)manageReminderTimesWindow2).Dispose();
			}
			_manageReminderTimesWindow = null;
		}
	}
}
