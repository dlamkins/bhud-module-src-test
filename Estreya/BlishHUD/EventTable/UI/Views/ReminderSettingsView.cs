using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Controls.Input;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ReminderSettingsView : BaseSettingsView
	{
		private static readonly Estreya.BlishHUD.EventTable.Models.Event _globalChangeTempEvent;

		private readonly Func<List<EventCategory>> _getEvents;

		private readonly Func<List<string>> _getAreaNames;

		private readonly AccountService _accountService;

		private readonly ModuleSettings _moduleSettings;

		private StandardWindow _manageEventsWindow;

		private StandardWindow _manageReminderTimesWindow;

		public event AsyncEventHandler SyncEnabledEventsToAreas;

		static ReminderSettingsView()
		{
			_globalChangeTempEvent = new Estreya.BlishHUD.EventTable.Models.Event();
			_globalChangeTempEvent.UpdateReminderTimes(new TimeSpan[1] { TimeSpan.Zero });
		}

		public ReminderSettingsView(ModuleSettings moduleSettings, Func<List<EventCategory>> getEvents, Func<List<string>> getAreaNames, AccountService accountService, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
			_getEvents = getEvents;
			_getAreaNames = getAreaNames;
			_accountService = accountService;
			CONTROL_WIDTH = 500;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel manageFlowPanel = val;
			RenderButton((Panel)(object)manageFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-manageReminders", "Manage Reminders"), delegate
			{
				if (_manageEventsWindow == null)
				{
					_manageEventsWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Manage Events", ((object)this).GetType(), Guid.Parse("37e3f99c-f413-469c-b0f5-e2e6e31e4789"), base.IconService);
					((Control)_manageEventsWindow).set_Width(1060);
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
				} }, () => _moduleSettings.ReminderDisabledForEvents.get_Value(), _moduleSettings, _accountService, base.APIManager, base.IconService, base.TranslationService);
				manageEventsView.EventChanged += ManageView_EventChanged;
				_manageEventsWindow.Show((IView)(object)manageEventsView);
			});
			RenderButtonAsync((Panel)(object)manageFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-testReminder", "Test Reminder"), async delegate
			{
				string title = "Test Event";
				string message = "Test starts in " + TimeSpan.FromHours(5.0).Add(TimeSpan.FromMinutes(21.0).Add(TimeSpan.FromSeconds(23.0))).Humanize(6, null, TimeUnit.Week, _moduleSettings.ReminderMinTimeUnit.get_Value()) + "!";
				AsyncTexture2D icon = base.IconService.GetIcon("textures/maintenance.png");
				ReminderType value = _moduleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Control || value == ReminderType.Both) ? true : false)
				{
					EventNotification eventNotification = new EventNotification(null, title, message, icon, _moduleSettings.ReminderPosition.X.get_Value(), _moduleSettings.ReminderPosition.Y.get_Value(), _moduleSettings.ReminderSize.X.get_Value(), _moduleSettings.ReminderSize.Y.get_Value(), _moduleSettings.ReminderSize.Icon.get_Value(), _moduleSettings.ReminderStackDirection.get_Value(), _moduleSettings.ReminderOverflowStackDirection.get_Value(), _moduleSettings.ReminderFonts.TitleSize.get_Value(), _moduleSettings.ReminderFonts.MessageSize.get_Value(), base.IconService);
					eventNotification.BackgroundOpacity = _moduleSettings.ReminderOpacity.get_Value();
					eventNotification.Show(TimeSpan.FromSeconds(_moduleSettings.ReminderDuration.get_Value()));
				}
				value = _moduleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Windows || value == ReminderType.Both) ? true : false)
				{
					await EventNotification.ShowAsWindowsNotification(title, message, icon);
				}
			});
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel changeTimesFlowPanel = val2;
			RenderButton((Panel)(object)changeTimesFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-changeAllTimes", "Change all Reminder Times"), delegate
			{
				ManageReminderTimes(_globalChangeTempEvent);
			});
			RenderButton((Panel)(object)changeTimesFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-resetAllTimes", "Reset all Reminder Times"), delegate
			{
				ManageReminderTimesView_SaveClicked(this, (_globalChangeTempEvent, new List<TimeSpan> { TimeSpan.FromMinutes(10.0) }, false));
			});
			RenderButtonAsync((Panel)(object)parent, base.TranslationService.GetTranslation("reminderSettingsView-btn-syncEnabledEventsToAreas", "Sync enabled Events to Areas"), async delegate
			{
				if (await new ConfirmDialog("Synchronizing", "You are in the process of synchronizing the enabled events of reminders to all event areas.\n\nThis will override all previously configured enabled/disabled settings in event areas.", base.IconService)
				{
					SelectedButtonIndex = 1
				}.ShowDialog() == DialogResult.OK)
				{
					await (this.SyncEnabledEventsToAreas?.Invoke(this) ?? Task.FromException(new NotImplementedException()));
					ScreenNotification.ShowNotification("Synchronization complete!", (NotificationType)0, (Texture2D)null, 4);
				}
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RemindersEnabled);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.DisableRemindersWhenEventFinished);
			RenderEmptyLine((Panel)(object)parent);
			RenderDisableRemindersWhenEventFinishedArea(parent);
			RenderEmptyLine((Panel)(object)parent);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderPosition.X);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderPosition.Y);
			RenderEmptyLine((Panel)(object)parent);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderSize.X);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderSize.Y);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.ReminderSize.Icon);
			RenderEmptyLine((Panel)(object)parent);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderDuration);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderOpacity);
			RenderEnumSetting<EventReminderStackDirection>((Panel)(object)parent, _moduleSettings.ReminderStackDirection);
			RenderEnumSetting<EventReminderStackDirection>((Panel)(object)parent, _moduleSettings.ReminderOverflowStackDirection);
			RenderEmptyLine((Panel)(object)parent);
			base.RenderEnumSetting<FontSize>((Panel)(object)parent, _moduleSettings.ReminderFonts.TitleSize);
			base.RenderEnumSetting<FontSize>((Panel)(object)parent, _moduleSettings.ReminderFonts.MessageSize);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<TimeUnit>((Panel)(object)parent, _moduleSettings.ReminderMinTimeUnit);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<ReminderType>((Panel)(object)parent, _moduleSettings.ReminderType);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<LeftClickAction>((Panel)(object)parent, _moduleSettings.ReminderLeftClickAction);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.AcceptWaypointPrompt);
			RenderEnumSetting<EventReminderRightClickAction>((Panel)(object)parent, _moduleSettings.ReminderRightClickAction);
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersOnMissingMumbleTicks);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersOnOpenMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInCombat);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvE_OpenWorld);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvE_Competetive);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInWvW);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.HideRemindersInPvP);
		}

		private void RenderDisableRemindersWhenEventFinishedArea(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel panel = val;
			RenderLabel(panel, ((SettingEntry)_moduleSettings.DisableRemindersWhenEventFinishedArea).get_DisplayName());
			List<string> values = new List<string> { "Any" };
			values.AddRange(_getAreaNames());
			((Control)RenderDropdown(panel, base.CONTROL_LOCATION, CONTROL_WIDTH, values.ToArray(), _moduleSettings.DisableRemindersWhenEventFinishedArea.get_Value(), delegate(string newVal)
			{
				_moduleSettings.DisableRemindersWhenEventFinishedArea.set_Value(newVal);
			})).set_BasicTooltipText(((SettingEntry)_moduleSettings.DisableRemindersWhenEventFinishedArea).get_Description());
		}

		private void ManageView_EventChanged(object sender, ManageEventsView.EventChangedArgs e)
		{
			_moduleSettings.ReminderDisabledForEvents.set_Value(e.NewState ? new List<string>(from s in _moduleSettings.ReminderDisabledForEvents.get_Value()
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
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>, bool)>(ManageReminderTimesView_SaveClicked);
			}
			ManageReminderTimesView view = new ManageReminderTimesView(ev, ev == _globalChangeTempEvent, base.APIManager, base.IconService, base.TranslationService);
			view.CancelClicked += ManageReminderTimesView_CancelClicked;
			view.SaveClicked += new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>, bool)>(ManageReminderTimesView_SaveClicked);
			_manageReminderTimesWindow.Show((IView)(object)view);
		}

		private void ManageReminderTimesView_SaveClicked(object sender, (Estreya.BlishHUD.EventTable.Models.Event Event, List<TimeSpan> ReminderTimes, bool KeepCustomized) e)
		{
			if (e.Event == _globalChangeTempEvent)
			{
				foreach (Estreya.BlishHUD.EventTable.Models.Event ev2 in from ev in _getEvents().SelectMany((EventCategory ec) => ec.Events)
					where !ev.Filler
					select ev)
				{
					if (!_moduleSettings.ReminderTimesOverride.get_Value().ContainsKey(ev2.SettingKey) || !e.KeepCustomized)
					{
						_moduleSettings.ReminderTimesOverride.get_Value()[ev2.SettingKey] = e.ReminderTimes;
						ev2.UpdateReminderTimes(e.ReminderTimes.ToArray());
					}
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
				mrtv.SaveClicked -= new EventHandler<(Estreya.BlishHUD.EventTable.Models.Event, List<TimeSpan>, bool)>(ManageReminderTimesView_SaveClicked);
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
