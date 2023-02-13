using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ReminderSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Func<List<EventCategory>> _getEvents;

		private StandardWindow _manageEventsWindow;

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
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Expected O, but got Unknown
				if (_manageEventsWindow == null)
				{
					Texture2D val = AsyncTexture2D.op_Implicit(base.IconState.GetIcon("textures\\setting_window_background.png"));
					Rectangle val2 = default(Rectangle);
					((Rectangle)(ref val2))._002Ector(35, 26, 1100, 714);
					int num = val2.Y - 15;
					int x = val2.X;
					Rectangle val3 = default(Rectangle);
					((Rectangle)(ref val3))._002Ector(x, num, val2.Width - 6, val2.Height - num);
					StandardWindow val4 = new StandardWindow(val, val2, val3);
					((Control)val4).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
					((WindowBase2)val4).set_Title("Manage Events");
					((WindowBase2)val4).set_SavesPosition(true);
					((WindowBase2)val4).set_Id(((object)this).GetType().Name + "_7dc52c82-67ae-4cfb-9fe3-a16a8b30892c");
					_manageEventsWindow = val4;
				}
				if (((WindowBase2)_manageEventsWindow).get_CurrentView() != null)
				{
					(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageEventsView).EventChanged -= ManageView_EventChanged;
				}
				ManageEventsView manageEventsView = new ManageEventsView(_getEvents(), null, () => _moduleSettings.ReminderDisabledForEvents.get_Value(), base.APIManager, base.IconState, base.TranslationState);
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
		}

		private void ManageView_EventChanged(object sender, EventChangedArgs e)
		{
			_moduleSettings.ReminderDisabledForEvents.set_Value(e.NewState ? new List<string>(from s in _moduleSettings.ReminderDisabledForEvents.get_Value()
				where s != e.EventSettingKey
				select s) : new List<string>(_moduleSettings.ReminderDisabledForEvents.get_Value()) { e.EventSettingKey });
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
		}
	}
}
