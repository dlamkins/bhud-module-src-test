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
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Guild;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.Audio;
using Estreya.BlishHUD.Shared.Threading;
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

		private readonly AudioService _audioService;

		private readonly ModuleSettings _moduleSettings;

		private StandardWindow _manageEventsWindow;

		private StandardWindow _manageReminderTimesWindow;

		private SynchronizedCollection<EventNotification> _activeTestNotifications;

		public event AsyncEventHandler SyncEnabledEventsToAreas;

		static ReminderSettingsView()
		{
			_globalChangeTempEvent = new Estreya.BlishHUD.EventTable.Models.Event();
			_globalChangeTempEvent.Key = "test";
			_globalChangeTempEvent.Load(new EventCategory
			{
				Key = "reminderSettingsView"
			}, delegate
			{
				throw new NotImplementedException();
			});
			_globalChangeTempEvent.UpdateReminderTimes(new TimeSpan[1] { TimeSpan.Zero });
		}

		public ReminderSettingsView(ModuleSettings moduleSettings, Func<List<EventCategory>> getEvents, Func<List<string>> getAreaNames, AccountService accountService, AudioService audioService, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
			_getEvents = getEvents;
			_getAreaNames = getAreaNames;
			_accountService = accountService;
			_audioService = audioService;
			CONTROL_WIDTH = 500;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Expected O, but got Unknown
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
						},
						new ManageEventsView.CustomActionDefinition
						{
							Name = base.TranslationService.GetTranslation("reminderSettingsView-btn-uploadEventSoundFile-title", "Upload Sound File"),
							Tooltip = base.TranslationService.GetTranslation("reminderSettingsView-btn-uploadEventSoundFile-tooltip", "Click to upload a specific sound file for this event."),
							Icon = "156764.png",
							Action = UploadEventSoundFile
						}
					}
				} }, () => _moduleSettings.ReminderDisabledForEvents.get_Value(), _moduleSettings, _accountService, base.APIManager, base.IconService, base.TranslationService);
				manageEventsView.EventChanged += ManageView_EventChanged;
				_manageEventsWindow.Show((IView)(object)manageEventsView);
			});
			RenderButtonAsync((Panel)(object)manageFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-uploadRemindersSoundFile", "Upload Sound File"), async delegate
			{
				AsyncFileDialog<OpenFileDialog> ofd = new AsyncFileDialog<OpenFileDialog>(new OpenFileDialog
				{
					Filter = "wav files (*.wav)|*.wav",
					Multiselect = false,
					CheckFileExists = true
				});
				if (await ofd.ShowAsync() == DialogResult.OK)
				{
					await _audioService.UploadFile(ofd.Dialog.FileName, EventNotification.GetSoundFileName(), EventNotification.GetAudioServiceBaseSubfolder());
				}
			});
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel testRemindersFlowPanel = val2;
			Func<bool, bool, Task> addTestReminder = async delegate(bool permanentControl, bool awaitAudio)
			{
				string title = "Test Event";
				string message = "Test starts in " + TimeSpan.FromHours(5.0).Add(TimeSpan.FromMinutes(21.0).Add(TimeSpan.FromSeconds(23.0))).Humanize(6, null, TimeUnit.Week, _moduleSettings.ReminderMinTimeUnit.get_Value()) + "!";
				AsyncTexture2D icon = base.IconService.GetIcon("textures/maintenance.png");
				ReminderType value = _moduleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Control || value == ReminderType.Both) ? true : false)
				{
					if (permanentControl)
					{
						EventNotification reminder = EventNotification.ShowAsControlTest(title, message, icon, base.IconService, _moduleSettings);
						_activeTestNotifications.Add(reminder);
					}
					else
					{
						EventNotification.ShowAsControl(title, message, icon, base.IconService, _moduleSettings);
					}
					Task audioTask = EventNotification.PlaySound(_audioService, _globalChangeTempEvent);
					if (awaitAudio)
					{
						await audioTask;
					}
				}
				value = _moduleSettings.ReminderType.get_Value();
				if ((value == ReminderType.Windows || value == ReminderType.Both) ? true : false)
				{
					await EventNotification.ShowAsWindowsNotification(title, message, icon);
				}
			};
			RenderButtonAsync((Panel)(object)testRemindersFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-addTestReminderPermanent", "Add Test Reminder"), async delegate
			{
				await addTestReminder(arg1: false, arg2: false);
			});
			RenderButtonAsync((Panel)(object)testRemindersFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-addTestReminderPermanent", "Add Test Reminder (Permanent)"), async delegate
			{
				await addTestReminder(arg1: true, arg2: false);
			});
			RenderButton((Panel)(object)testRemindersFlowPanel, base.TranslationService.GetTranslation("reminderSettingsView-btn-clearTestReminder", "Clear Permanent Test Reminders"), delegate
			{
				foreach (EventNotification activeTestNotification in _activeTestNotifications)
				{
					((Control)activeTestNotification).Dispose();
				}
			});
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)parent);
			((Container)val3).set_WidthSizingMode((SizingMode)1);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel changeTimesFlowPanel = val3;
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
			RenderEnumSetting<EventReminderStackDirection>((Panel)(object)parent, _moduleSettings.ReminderStackDirection);
			RenderEnumSetting<EventReminderStackDirection>((Panel)(object)parent, _moduleSettings.ReminderOverflowStackDirection);
			RenderEmptyLine((Panel)(object)parent);
			base.RenderEnumSetting<FontSize>((Panel)(object)parent, _moduleSettings.ReminderFonts.TitleSize);
			base.RenderEnumSetting<FontSize>((Panel)(object)parent, _moduleSettings.ReminderFonts.MessageSize);
			RenderEmptyLine((Panel)(object)parent);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.ReminderColors.Background);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.ReminderColors.TitleText);
			RenderColorSetting((Panel)(object)parent, _moduleSettings.ReminderColors.MessageText);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderBackgroundOpacity);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderTitleOpacity);
			RenderFloatSetting((Panel)(object)parent, _moduleSettings.ReminderMessageOpacity);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<TimeUnit>((Panel)(object)parent, _moduleSettings.ReminderMinTimeUnit);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<ReminderType>((Panel)(object)parent, _moduleSettings.ReminderType);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<LeftClickAction>((Panel)(object)parent, _moduleSettings.ReminderLeftClickAction);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.AcceptWaypointPrompt);
			RenderEnumSetting<EventReminderRightClickAction>((Panel)(object)parent, _moduleSettings.ReminderRightClickAction);
			RenderEnumSetting<ChatChannel>((Panel)(object)parent, _moduleSettings.ReminderWaypointSendingChannel);
			RenderEnumSetting<GuildNumber>((Panel)(object)parent, _moduleSettings.ReminderWaypointSendingGuild);
			RenderEnumSetting<EventChatFormat>((Panel)(object)parent, _moduleSettings.ReminderEventChatFormat);
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

		private async Task UploadEventSoundFile(Estreya.BlishHUD.EventTable.Models.Event ev)
		{
			_ = 1;
			try
			{
				AsyncFileDialog<OpenFileDialog> ofd = new AsyncFileDialog<OpenFileDialog>(new OpenFileDialog
				{
					Filter = "wav files (*.wav)|*.wav",
					Multiselect = false,
					CheckFileExists = true
				});
				if (await ofd.ShowAsync() == DialogResult.OK)
				{
					await _audioService.UploadFile(ofd.Dialog.FileName, ev.SettingKey, EventNotification.GetAudioServiceEventsSubfolder());
				}
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}

		private Task ManageReminderTimes(Estreya.BlishHUD.EventTable.Models.Event ev)
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
			return Task.CompletedTask;
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
			_activeTestNotifications = new SynchronizedCollection<EventNotification>();
			return Task.FromResult(result: true);
		}

		protected override void Unload()
		{
			base.Unload();
			if (_activeTestNotifications != null)
			{
				foreach (EventNotification activeTestNotification in _activeTestNotifications)
				{
					((Control)activeTestNotification).Dispose();
				}
				_activeTestNotifications.Clear();
				_activeTestNotifications = null;
			}
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
