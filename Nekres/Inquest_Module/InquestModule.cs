using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Inquest_Module.Core.Controllers;
using Nekres.Inquest_Module.UI.Models;
using Nekres.Inquest_Module.UI.Views;

namespace Nekres.Inquest_Module
{
	[Export(typeof(Module))]
	public class InquestModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(InquestModule));

		internal static InquestModule ModuleInstance;

		internal SettingEntry<KeyBinding> AutoClickHoldKeySetting;

		internal SettingEntry<bool> HoldKeyWithLeftClickEnabledSetting;

		internal SettingEntry<KeyBinding> AutoClickToggleKeySetting;

		internal SettingEntry<bool> AutoClickSoundDisabledSetting;

		internal SettingEntry<float> AutoClickSoundVolume;

		internal SettingEntry<KeyBinding> JumpKeyBindingSetting;

		internal SettingEntry<KeyBinding> DodgeKeyBindingSetting;

		internal SettingEntry<KeyBinding> DodgeJumpKeyBindingSetting;

		internal SettingEntry<double> AutoClickToggleInterval;

		private CornerIcon _moduleIcon;

		private AutoClickController _autoClickController;

		private DateTime _nextDodgeJump = DateTime.UtcNow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public InquestModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Expected O, but got Unknown
			SettingCollection hotkeys = settings.AddSubCollection("Control Options", true, false);
			AutoClickHoldKeySetting = hotkeys.DefineSetting<KeyBinding>("autoClickHoldKeyBinding", new KeyBinding((Keys)188), (Func<string>)(() => "Hold Double Clicking"), (Func<string>)(() => "Perform Double Clicks at the current cursor position while the key is being pressed."));
			AutoClickToggleKeySetting = hotkeys.DefineSetting<KeyBinding>("autoClickToggleKeyBinding", new KeyBinding((Keys)219), (Func<string>)(() => "Toggle Double Clicking"), (Func<string>)(() => "Perform Double Clicks in an interval at the position of the cursor at the time of pressing the key."));
			DodgeJumpKeyBindingSetting = hotkeys.DefineSetting<KeyBinding>("dodgeJumpKeyBinding", new KeyBinding((ModifierKeys)1, (Keys)32), (Func<string>)(() => "Dodge-Jump"), (Func<string>)(() => "Perform a dodge roll and a jump simultaneously."));
			SettingCollection controlOptions = hotkeys.AddSubCollection("Movement Keys to Trigger on Dodge-Jump", true, false);
			DodgeKeyBindingSetting = controlOptions.DefineSetting<KeyBinding>("dodgeKeyBinding", new KeyBinding((Keys)86), (Func<string>)(() => "Dodge"), (Func<string>)(() => "Do an evasive dodge roll, negating damage, in the direction your character is moving (backward if stationary)."));
			JumpKeyBindingSetting = controlOptions.DefineSetting<KeyBinding>("jumpKeyBinding", new KeyBinding((Keys)32), (Func<string>)(() => "Jump"), (Func<string>)(() => "Press to jump over obstacles."));
			HoldKeyWithLeftClickEnabledSetting = hotkeys.DefineSetting<bool>("holdKeyWithLeftClick", false, (Func<string>)(() => "Hold Double Clicking + Left Mouse Button"), (Func<string>)(() => "Perform Double clicks at the current cursor position while Hold Double Clicking and Left Mouse Button is being pressed."));
			SettingCollection audio = settings.AddSubCollection("Sound Options", true, false);
			AutoClickSoundDisabledSetting = audio.DefineSetting<bool>("autoClickSoundsDisabled", false, (Func<string>)(() => "Disable Clicking Sounds"), (Func<string>)(() => "Disables the sound alert when an auto click is performed."));
			AutoClickSoundVolume = audio.DefineSetting<float>("autoClickSoundsVolume", 80f, (Func<string>)(() => "Clicking Sounds Volume"), (Func<string>)(() => "Sets the audio volume of the clicking alerts."));
			SettingCollection hiddenSettingsCache = settings.AddSubCollection("hiddenSettingsCache", false, false);
			AutoClickToggleInterval = hiddenSettingsCache.DefineSetting<double>("autoClickToggleInterval", 0.0, (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Invalid comparison between Unknown and I4
			_autoClickController = new AutoClickController();
			_autoClickController.SoundVolume = AutoClickSoundVolume.get_Value() / 1000f;
			JumpKeyBindingSetting.get_Value().set_Enabled(false);
			DodgeKeyBindingSetting.get_Value().set_Enabled(false);
			DodgeJumpKeyBindingSetting.get_Value().set_Enabled(true);
			ToggleDodgeJump((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() == 0);
			AutoClickToggleKeySetting.get_Value().set_Enabled(true);
			AutoClickHoldKeySetting.get_Value().set_Enabled(true);
			AutoClickSoundVolume.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnAutoClickSoundVolumeSettingChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)OnCurrentMountChanged);
			GameService.Gw2Mumble.get_UI().add_IsTextInputFocusedChanged((EventHandler<ValueEventArgs<bool>>)OnIsTextInputFocusChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override async Task LoadAsync()
		{
		}

		private void OnAutoClickSoundVolumeSettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_autoClickController.SoundVolume = e.get_NewValue() / 1000f;
		}

		private void OnDodgeJumpKeyActivated(object o, EventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			if (DateTime.UtcNow < _nextDodgeJump)
			{
				return;
			}
			if (DodgeKeyBindingSetting.get_Value().get_PrimaryKey() == DodgeJumpKeyBindingSetting.get_Value().get_PrimaryKey() && DodgeKeyBindingSetting.get_Value().get_ModifierKeys() == DodgeJumpKeyBindingSetting.get_Value().get_ModifierKeys())
			{
				ScreenNotification.ShowNotification("Endless Loop Error. Dodge-Jump key cannot be the same as Dodge.", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			if (JumpKeyBindingSetting.get_Value().get_PrimaryKey() == DodgeJumpKeyBindingSetting.get_Value().get_PrimaryKey() && JumpKeyBindingSetting.get_Value().get_ModifierKeys() == DodgeJumpKeyBindingSetting.get_Value().get_ModifierKeys())
			{
				ScreenNotification.ShowNotification("Endless Loop Error. Dodge-Jump key cannot be the same as Jump.", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			_nextDodgeJump = DateTime.UtcNow.AddMilliseconds(80.0);
			if ((int)DodgeKeyBindingSetting.get_Value().get_PrimaryKey() != 0)
			{
				Keyboard.Stroke((VirtualKeyShort)(short)DodgeKeyBindingSetting.get_Value().get_PrimaryKey(), false);
			}
			if ((int)JumpKeyBindingSetting.get_Value().get_PrimaryKey() != 0)
			{
				Keyboard.Stroke((VirtualKeyShort)(short)JumpKeyBindingSetting.get_Value().get_PrimaryKey(), false);
			}
		}

		private void OnCurrentMountChanged(object o, ValueEventArgs<MountType> e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			ToggleDodgeJump((int)e.get_Value() == 0);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleDodgeJump(e.get_Value());
		}

		private void OnIsTextInputFocusChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleDodgeJump(!e.get_Value());
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleDodgeJump(!e.get_Value());
		}

		private void ToggleDodgeJump(bool enabled)
		{
			DodgeJumpKeyBindingSetting.get_Value().set_BlockSequenceFromGw2(enabled);
			if (enabled)
			{
				DodgeJumpKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnDodgeJumpKeyActivated);
			}
			else
			{
				DodgeJumpKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnDodgeJumpKeyActivated);
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			_autoClickController?.Update();
		}

		protected override void Unload()
		{
			AutoClickToggleKeySetting.get_Value().set_Enabled(false);
			AutoClickHoldKeySetting.get_Value().set_Enabled(false);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)OnCurrentMountChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			GameService.Gw2Mumble.get_UI().remove_IsTextInputFocusedChanged((EventHandler<ValueEventArgs<bool>>)OnIsTextInputFocusChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			DodgeJumpKeyBindingSetting.get_Value().set_Enabled(false);
			DodgeJumpKeyBindingSetting.get_Value().set_BlockSequenceFromGw2(false);
			_autoClickController?.Dispose();
			DodgeJumpKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnDodgeJumpKeyActivated);
			AutoClickSoundVolume.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnAutoClickSoundVolumeSettingChanged);
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			ModuleInstance = null;
		}
	}
}
