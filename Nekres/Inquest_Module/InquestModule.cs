using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Inquest_Module.Core.Controllers;

namespace Nekres.Inquest_Module
{
	[Export(typeof(Module))]
	public class InquestModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(InquestModule));

		internal static InquestModule ModuleInstance;

		internal SettingEntry<KeyBinding> AutoClickHoldKeySetting;

		internal SettingEntry<KeyBinding> AutoClickToggleKeySetting;

		internal SettingEntry<KeyBinding> JumpKeyBindingSetting;

		internal SettingEntry<KeyBinding> DodgeKeyBindingSetting;

		internal SettingEntry<KeyBinding> DodgeJumpKeyBindingSetting;

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
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			AutoClickHoldKeySetting = settings.DefineSetting<KeyBinding>("autoClickHoldKeyBinding", new KeyBinding((Keys)188), (Func<string>)(() => "Hold Double Clicking"), (Func<string>)(() => "Perform Double Clicks at the current cursor position while the key is being held down."));
			AutoClickToggleKeySetting = settings.DefineSetting<KeyBinding>("autoClickToggleKeyBinding", new KeyBinding((Keys)0), (Func<string>)(() => "Toggle Double Clicking"), (Func<string>)(() => "Perform Double Clicks in an interval at the current cursor position until the key is pressed again."));
			DodgeJumpKeyBindingSetting = settings.DefineSetting<KeyBinding>("dodgeJumpKeyBinding", new KeyBinding((ModifierKeys)4, (Keys)32), (Func<string>)(() => "Dodge-Jump"), (Func<string>)(() => "Perform a dodge roll and a jump simultaneously."));
			SettingCollection controlOptions = settings.AddSubCollection("Movement", true, false);
			DodgeKeyBindingSetting = controlOptions.DefineSetting<KeyBinding>("dodgeKeyBinding", new KeyBinding((Keys)86), (Func<string>)(() => "Dodge"), (Func<string>)(() => "Do an evasive dodge roll, negating damage, in the direction your character is moving (backward if stationary)."));
			JumpKeyBindingSetting = controlOptions.DefineSetting<KeyBinding>("jumpKeyBinding", new KeyBinding((Keys)32), (Func<string>)(() => "Jump"), (Func<string>)(() => "Press to jump over obstacles."));
		}

		protected override void Initialize()
		{
			_autoClickController = new AutoClickController();
			JumpKeyBindingSetting.get_Value().set_Enabled(false);
			DodgeKeyBindingSetting.get_Value().set_Enabled(false);
			DodgeJumpKeyBindingSetting.get_Value().set_Enabled(true);
			DodgeJumpKeyBindingSetting.get_Value().add_Activated((EventHandler<EventArgs>)OnDodgeJumpKeyActivated);
		}

		protected override async Task LoadAsync()
		{
		}

		private void OnDodgeJumpKeyActivated(object o, EventArgs e)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			if (!(DateTime.UtcNow < _nextDodgeJump) && (int)DodgeKeyBindingSetting.get_Value().get_PrimaryKey() != 0 && (int)JumpKeyBindingSetting.get_Value().get_PrimaryKey() != 0)
			{
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
				_nextDodgeJump = DateTime.UtcNow.AddMilliseconds(100.0);
				Keyboard.Stroke((VirtualKeyShort)(short)DodgeKeyBindingSetting.get_Value().get_PrimaryKey(), true);
				Keyboard.Stroke((VirtualKeyShort)(short)JumpKeyBindingSetting.get_Value().get_PrimaryKey(), true);
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
			_autoClickController?.Dispose();
			DodgeJumpKeyBindingSetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnDodgeJumpKeyActivated);
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			ModuleInstance = null;
		}
	}
}
