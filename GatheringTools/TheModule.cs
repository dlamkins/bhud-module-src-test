using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using GatheringTools.LogoutOverlay;
using GatheringTools.ToolSearch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GatheringTools
{
	[Export(typeof(Module))]
	public class TheModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<TheModule>();

		private double _runningTime;

		private ToolSearchStandardWindow _toolSearchStandardWindow;

		private ReminderContainer _reminderContainer;

		private SettingEntry<KeyBinding> _logoutKeyBindingSetting;

		private SettingEntry<KeyBinding> _toolSearchKeyBindingSetting;

		private SettingEntry<DisplayDuration> _reminderDisplayDurationInSecondsSetting;

		private SettingEntry<string> _reminderTextSetting;

		private SettingEntry<bool> _reminderIsVisibleForSetupSetting;

		private SettingEntry<bool> _escIsHidingReminderSetting;

		private SettingEntry<bool> _enterIsHidingReminderSetting;

		private SettingEntry<bool> _showOnlyUnlimitedToolsSetting;

		private SettingEntry<bool> _showToolSearchCornerIconSetting;

		private SettingEntry<int> _reminderWindowSizeSetting;

		private SettingEntry<int> _reminderTextFontSizeIndexSetting;

		private SettingEntry<int> _reminderIconSizeSetting;

		private SettingCollection _internalSettingSubCollection;

		private KeyBinding _escKeyBinding;

		private KeyBinding _enterKeyBinding;

		private Texture2D _sickleTexture;

		private Texture2D _windowBackgroundTexture;

		private CornerIconService _cornerIconService;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public TheModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			_logoutKeyBindingSetting = settings.DefineSetting<KeyBinding>("Logout key binding", new KeyBinding((Keys)123), (Func<string>)(() => "Logout (must match ingame key)"), (Func<string>)(() => "Double-click to change it. Logout key has to match the ingame logout key (default F12)."));
			_toolSearchKeyBindingSetting = settings.DefineSetting<KeyBinding>("tool search key binding", new KeyBinding((ModifierKeys)2, (Keys)67), (Func<string>)(() => "tool search window"), (Func<string>)(() => "Double-click to change it. Will show or hide the tool search window."));
			_reminderTextSetting = settings.DefineSetting<string>("text (logout overlay)", "shared inventory", (Func<string>)(() => "reminder text"), (Func<string>)(() => "text shown inside the reminder window"));
			_reminderDisplayDurationInSecondsSetting = settings.DefineSetting<DisplayDuration>("display duration (logout overlay)", DisplayDuration.Seconds3, (Func<string>)(() => "reminder display duration (1-10s)"), (Func<string>)(() => "The reminder will disappear automatically after this time has expired"));
			_reminderWindowSizeSetting = settings.DefineSetting<int>("window size (logout overlay)", 34, (Func<string>)(() => "reminder size"), (Func<string>)(() => "Change reminder window size to fit to the size of the logout dialog with your current screen settings"));
			SettingComplianceExtensions.SetRange(_reminderWindowSizeSetting, 1, 100);
			_reminderTextFontSizeIndexSetting = FontService.CreateFontSizeIndexSetting(settings);
			_reminderIconSizeSetting = settings.DefineSetting<int>("reminder icon size (logout overlay)", 60, (Func<string>)(() => "icon size"), (Func<string>)(() => "Change size of the icons in the reminder window"));
			SettingComplianceExtensions.SetRange(_reminderIconSizeSetting, 10, 300);
			_escIsHidingReminderSetting = settings.DefineSetting<bool>("hide on ESC", true, (Func<string>)(() => "hide on ESC"), (Func<string>)(() => "When you press ESC to close the logout dialog, the reminder will be hidden, too"));
			_enterIsHidingReminderSetting = settings.DefineSetting<bool>("hide on ENTER", true, (Func<string>)(() => "hide on ENTER"), (Func<string>)(() => "When you press ENTER to switch to the character selection, the reminder will be hidden"));
			_reminderIsVisibleForSetupSetting = settings.DefineSetting<bool>("show reminder for setup", false, (Func<string>)(() => "show reminder permanently for setup"), (Func<string>)(() => "show reminder for easier setup of position etc. This will ignore display duration and ESC or ENTER being pressed. Do not forget to uncheck after you set up everything."));
			_showToolSearchCornerIconSetting = settings.DefineSetting<bool>("show tool search corner icon", true, (Func<string>)(() => "show sickle icon"), (Func<string>)(() => "Show sickle icon at the top left of GW2 next to other menu icons. Icon can be clicked to show/hide the gathering tool search window"));
			_internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)", false);
			_showOnlyUnlimitedToolsSetting = _internalSettingSubCollection.DefineSetting<bool>("only unlimited tools", true, (Func<string>)(() => "only unlimited tools"), (Func<string>)(() => "show only unlimited tools in the tool search window."));
		}

		protected override void Initialize()
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Expected O, but got Unknown
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			_windowBackgroundTexture = ContentsManager.GetTexture("155985.png");
			_sickleTexture = ContentsManager.GetTexture("sickle.png");
			ToolSearchStandardWindow toolSearchStandardWindow = new ToolSearchStandardWindow(_showOnlyUnlimitedToolsSetting, _windowBackgroundTexture, Gw2ApiManager, Logger);
			((WindowBase2)toolSearchStandardWindow).set_Emblem(_sickleTexture);
			((WindowBase2)toolSearchStandardWindow).set_Title("Tools");
			((Control)toolSearchStandardWindow).set_BasicTooltipText("Shows which character has gathering tools equipped.");
			((Control)toolSearchStandardWindow).set_Location(new Point(300, 300));
			((WindowBase2)toolSearchStandardWindow).set_SavesPosition(true);
			((WindowBase2)toolSearchStandardWindow).set_Id("tool search window 6f48189f-0a38-4fad-bc6a-10d323e7f1c4");
			((Control)toolSearchStandardWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_toolSearchStandardWindow = toolSearchStandardWindow;
			_reminderContainer = new ReminderContainer(ContentsManager);
			_reminderContainer.UpdateReminderText(_reminderTextSetting.get_Value());
			_reminderContainer.UpdateReminderTextFontSize(_reminderTextFontSizeIndexSetting.get_Value());
			_reminderContainer.UpdateContainerSizeAndMoveAboveLogoutDialog(_reminderWindowSizeSetting.get_Value());
			_reminderContainer.UpdateIconSize(_reminderIconSizeSetting.get_Value());
			if (_reminderIsVisibleForSetupSetting.get_Value())
			{
				ShowReminderAndResetRunningTime();
			}
			else
			{
				HideReminderAndResetRunningTime();
			}
			((SettingEntry)_reminderIsVisibleForSetupSetting).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_reminderIsVisibleForSetupSetting.get_Value())
				{
					ShowReminderAndResetRunningTime();
				}
				else
				{
					HideReminderAndResetRunningTime();
				}
			});
			_reminderTextFontSizeIndexSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				_reminderContainer.UpdateReminderTextFontSize(e.get_NewValue());
			});
			_reminderTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				_reminderContainer.UpdateReminderText(e.get_NewValue());
			});
			_reminderWindowSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				_reminderContainer.UpdateContainerSizeAndMoveAboveLogoutDialog(e.get_NewValue());
			});
			_reminderIconSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				_reminderContainer.UpdateIconSize(e.get_NewValue());
			});
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			_escKeyBinding = new KeyBinding((Keys)27);
			_escKeyBinding.add_Activated((EventHandler<EventArgs>)OnEscKeyBindingActivated);
			_escKeyBinding.set_Enabled(true);
			_enterKeyBinding = new KeyBinding((Keys)13);
			_enterKeyBinding.add_Activated((EventHandler<EventArgs>)OnEnterKeyBindingActivated);
			_enterKeyBinding.set_Enabled(true);
			_logoutKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnLogoutKeyBindingActivated);
			_logoutKeyBindingSetting.get_Value().set_Enabled(true);
			_toolSearchKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await _toolSearchStandardWindow.ToggleVisibility();
			});
			_toolSearchKeyBindingSetting.get_Value().set_Enabled(true);
			_cornerIconService = new CornerIconService(_showToolSearchCornerIconSetting, _toolSearchStandardWindow, _sickleTexture);
		}

		protected override void Update(GameTime gameTime)
		{
			if (!_reminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible())
			{
				_runningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
				if (_runningTime > (double)(1000 * (int)_reminderDisplayDurationInSecondsSetting.get_Value()))
				{
					HideReminderAndResetRunningTime();
				}
			}
		}

		protected override void Unload()
		{
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			_escKeyBinding.remove_Activated((EventHandler<EventArgs>)OnEscKeyBindingActivated);
			_logoutKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnLogoutKeyBindingActivated);
			Texture2D windowBackgroundTexture = _windowBackgroundTexture;
			if (windowBackgroundTexture != null)
			{
				((GraphicsResource)windowBackgroundTexture).Dispose();
			}
			Texture2D sickleTexture = _sickleTexture;
			if (sickleTexture != null)
			{
				((GraphicsResource)sickleTexture).Dispose();
			}
			ToolSearchStandardWindow toolSearchStandardWindow = _toolSearchStandardWindow;
			if (toolSearchStandardWindow != null)
			{
				((Control)toolSearchStandardWindow).Dispose();
			}
			ReminderContainer reminderContainer = _reminderContainer;
			if (reminderContainer != null)
			{
				((Control)reminderContainer).Dispose();
			}
			_cornerIconService?.RemoveCornerIcon();
		}

		private void OnSpriteScreenResized(object sender, ResizedEventArgs e)
		{
			_reminderContainer.MoveAboveLogoutDialog();
		}

		private void OnEscKeyBindingActivated(object sender, EventArgs e)
		{
			if (!_reminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible() && _escIsHidingReminderSetting.get_Value())
			{
				HideReminderAndResetRunningTime();
			}
		}

		private void OnEnterKeyBindingActivated(object sender, EventArgs e)
		{
			if (!_reminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible() && _enterIsHidingReminderSetting.get_Value())
			{
				HideReminderAndResetRunningTime();
			}
		}

		private void OnLogoutKeyBindingActivated(object sender, EventArgs e)
		{
			if (!((Control)_reminderContainer).get_Visible())
			{
				ShowReminderAndResetRunningTime();
			}
		}

		private void HideReminderAndResetRunningTime()
		{
			_runningTime = 0.0;
			((Control)_reminderContainer).Hide();
		}

		private void ShowReminderAndResetRunningTime()
		{
			_runningTime = 0.0;
			((Control)_reminderContainer).Show();
			ScreenNotification.ShowNotification(_reminderTextSetting.get_Value(), (NotificationType)2, (Texture2D)null, 4);
		}
	}
}
