using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using HexedHero.Blish_HUD.MarkerPackAssistant.Objects;
using Microsoft.Xna.Framework.Input;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Managers
{
	public class ModuleSettingsManager
	{
		private static Lazy<ModuleSettingsManager> instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());

		public static ModuleSettingsManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
				}
				return instance.Value;
			}
		}

		public SettingCollection Settings { get; private set; }

		public ModuleSettings ModuleSettings { get; private set; }

		public SettingEntry<KeyBinding> KeyBindCopyMap { get; private set; }

		public SettingEntry<KeyBinding> KeyBindCopyXYZ { get; private set; }

		public SettingEntry<KeyBinding> KeyBindCopyGUID { get; private set; }

		public SettingEntry<KeyBinding> KeyBindCopyPOI { get; private set; }

		public SettingEntry<KeyBinding> KeyBindRun { get; private set; }

		private ModuleSettingsManager()
		{
			Load();
		}

		private void Load()
		{
		}

		public void Unload()
		{
			DisableKeybinds();
			KeyBindCopyMap.get_Value().remove_Activated((EventHandler<EventArgs>)TriggerCopyMap);
			KeyBindCopyXYZ.get_Value().remove_Activated((EventHandler<EventArgs>)TriggerCopyXYZ);
			KeyBindCopyGUID.get_Value().remove_Activated((EventHandler<EventArgs>)TriggerCopyGUID);
			KeyBindCopyPOI.get_Value().remove_Activated((EventHandler<EventArgs>)TriggerCopyPOI);
			KeyBindRun.get_Value().remove_Activated((EventHandler<EventArgs>)TriggerRun);
			instance = null;
		}

		public void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = new ModuleSettings(settings);
			InitKeyBindSettings(settings);
		}

		private void InitKeyBindSettings(SettingCollection settings)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Expected O, but got Unknown
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Expected O, but got Unknown
			KeyBindCopyMap = settings.DefineSetting<KeyBinding>("KeyBindCopyMap", new KeyBinding((ModifierKeys)6, (Keys)81), (Func<string>)(() => "Copy Map ID"), (Func<string>)(() => ""));
			KeyBindCopyXYZ = settings.DefineSetting<KeyBinding>("KeyBindCopyXYZ", new KeyBinding((ModifierKeys)6, (Keys)87), (Func<string>)(() => "Copy XYZ Coordinates"), (Func<string>)(() => ""));
			KeyBindCopyGUID = settings.DefineSetting<KeyBinding>("KeyBindCopyGUID", new KeyBinding((ModifierKeys)6, (Keys)69), (Func<string>)(() => "Copy New GUID"), (Func<string>)(() => ""));
			KeyBindCopyPOI = settings.DefineSetting<KeyBinding>("KeyBindCopyPOI", new KeyBinding((ModifierKeys)6, (Keys)82), (Func<string>)(() => "Copy POI"), (Func<string>)(() => ""));
			KeyBindRun = settings.DefineSetting<KeyBinding>("KeyBindRun", new KeyBinding((ModifierKeys)6, (Keys)84), (Func<string>)(() => "Run .bat"), (Func<string>)(() => ""));
			HandleKeybinds();
		}

		private void HandleKeybinds()
		{
			DisableKeybinds();
			KeyBindCopyMap.get_Value().add_Activated((EventHandler<EventArgs>)TriggerCopyMap);
			KeyBindCopyXYZ.get_Value().add_Activated((EventHandler<EventArgs>)TriggerCopyXYZ);
			KeyBindCopyGUID.get_Value().add_Activated((EventHandler<EventArgs>)TriggerCopyGUID);
			KeyBindCopyPOI.get_Value().add_Activated((EventHandler<EventArgs>)TriggerCopyPOI);
			KeyBindRun.get_Value().add_Activated((EventHandler<EventArgs>)TriggerRun);
		}

		private void TriggerCopyMap(object sender, EventArgs e)
		{
			WindowManager.Instance.AssistanceView.CopyMapID();
		}

		private void TriggerCopyXYZ(object sender, EventArgs e)
		{
			WindowManager.Instance.AssistanceView.CopyCords();
		}

		private void TriggerCopyGUID(object sender, EventArgs e)
		{
			WindowManager.Instance.AssistanceView.CopyRandomGUID();
		}

		private void TriggerCopyPOI(object sender, EventArgs e)
		{
			WindowManager.Instance.AssistanceView.CopyPOI();
		}

		private void TriggerRun(object sender, EventArgs e)
		{
			WindowManager.Instance.AssistanceView.RunBat();
		}

		public void EnableKeybinds()
		{
			if (KeyBindCopyMap != null)
			{
				KeyBindCopyMap.get_Value().set_Enabled(true);
				KeyBindCopyXYZ.get_Value().set_Enabled(true);
				KeyBindCopyGUID.get_Value().set_Enabled(true);
				KeyBindCopyPOI.get_Value().set_Enabled(true);
				KeyBindRun.get_Value().set_Enabled(true);
			}
		}

		public void DisableKeybinds()
		{
			if (KeyBindCopyMap != null)
			{
				KeyBindCopyMap.get_Value().set_Enabled(false);
				KeyBindCopyXYZ.get_Value().set_Enabled(false);
				KeyBindCopyGUID.get_Value().set_Enabled(false);
				KeyBindCopyPOI.get_Value().set_Enabled(false);
				KeyBindRun.get_Value().set_Enabled(false);
			}
		}
	}
}
