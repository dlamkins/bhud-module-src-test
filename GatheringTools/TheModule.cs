using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using GatheringTools.LogoutOverlay;
using GatheringTools.Services;
using GatheringTools.ToolSearch.Controls;
using GatheringTools.ToolSearch.Model;
using GatheringTools.ToolSearch.Services;
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

		private KeyBinding _escKeyBinding;

		private KeyBinding _enterKeyBinding;

		private SettingService _settingService;

		private TextureService _textureService;

		private CornerIconService _cornerIconService;

		private readonly List<GatheringTool> _allGatheringTools = new List<GatheringTool>();

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
			_settingService = new SettingService(settings);
		}

		protected override async Task LoadAsync()
		{
			_textureService = new TextureService(ContentsManager);
			_reminderContainer = new ReminderContainer(_textureService, _settingService);
			if (_settingService.ReminderIsVisibleForSetupSetting.get_Value())
			{
				ShowReminderAndResetRunningTime();
			}
			else
			{
				HideReminderAndResetRunningTime();
			}
			((SettingEntry)_settingService.ReminderIsVisibleForSetupSetting).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_settingService.ReminderIsVisibleForSetupSetting.get_Value())
				{
					ShowReminderAndResetRunningTime();
				}
				else
				{
					HideReminderAndResetRunningTime();
				}
			});
			_escKeyBinding = new KeyBinding((Keys)27);
			_escKeyBinding.add_Activated((EventHandler<EventArgs>)OnEscKeyBindingActivated);
			_escKeyBinding.set_Enabled(true);
			_enterKeyBinding = new KeyBinding((Keys)13);
			_enterKeyBinding.add_Activated((EventHandler<EventArgs>)OnEnterKeyBindingActivated);
			_enterKeyBinding.set_Enabled(true);
			_settingService.LogoutKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnLogoutKeyBindingActivated);
			_settingService.LogoutKeyBindingSetting.get_Value().set_Enabled(true);
			IEnumerable<GatheringTool> allGatheringTools = await FileService.GetAllGatheringToolsFromFiles(ContentsManager, Logger);
			_allGatheringTools.AddRange(allGatheringTools);
			TheModule theModule = this;
			ToolSearchStandardWindow toolSearchStandardWindow = new ToolSearchStandardWindow(_textureService, _settingService, _allGatheringTools, Gw2ApiManager, Logger);
			((WindowBase2)toolSearchStandardWindow).set_Emblem(_textureService.ToolSearchWindowEmblem);
			((WindowBase2)toolSearchStandardWindow).set_Title("Tools");
			((Control)toolSearchStandardWindow).set_Location(new Point(300, 300));
			((WindowBase2)toolSearchStandardWindow).set_SavesPosition(true);
			((WindowBase2)toolSearchStandardWindow).set_Id("tool search window 6f48189f-0a38-4fad-bc6a-10d323e7f1c4");
			((Control)toolSearchStandardWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			theModule._toolSearchStandardWindow = toolSearchStandardWindow;
			_settingService.ToolSearchKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				await _toolSearchStandardWindow.ToggleVisibility();
			});
			_settingService.ToolSearchKeyBindingSetting.get_Value().set_Enabled(true);
			_cornerIconService = new CornerIconService(_settingService.ShowToolSearchCornerIconSetting, _toolSearchStandardWindow, _textureService);
		}

		protected override void Update(GameTime gameTime)
		{
			if (!_settingService.ReminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible())
			{
				_runningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
				if (_runningTime > (double)(1000 * (int)_settingService.ReminderDisplayDurationInSecondsSetting.get_Value()))
				{
					HideReminderAndResetRunningTime();
				}
			}
		}

		protected override void Unload()
		{
			_escKeyBinding.remove_Activated((EventHandler<EventArgs>)OnEscKeyBindingActivated);
			_settingService.LogoutKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnLogoutKeyBindingActivated);
			_textureService?.Dispose();
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

		private void OnEscKeyBindingActivated(object sender, EventArgs e)
		{
			if (!_settingService.ReminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible() && _settingService.EscIsHidingReminderSetting.get_Value())
			{
				HideReminderAndResetRunningTime();
			}
		}

		private void OnEnterKeyBindingActivated(object sender, EventArgs e)
		{
			if (!_settingService.ReminderIsVisibleForSetupSetting.get_Value() && ((Control)_reminderContainer).get_Visible() && _settingService.EnterIsHidingReminderSetting.get_Value())
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
			ScreenNotification.ShowNotification(_settingService.ReminderTextSetting.get_Value(), (NotificationType)2, (Texture2D)null, 4);
		}
	}
}
