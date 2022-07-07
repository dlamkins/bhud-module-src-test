using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts
{
	public class Helper
	{
		private static SemaphoreSlim keybindSemaphore = new SemaphoreSlim(1, 1);

		private static readonly Logger Logger = Logger.GetLogger<Helper>();

		private MapType[] warclawOnlyMaps;

		private bool IsPlayerInWvWMap()
		{
			return Array.Exists(warclawOnlyMaps, (MapType mapType) => mapType == GameService.Gw2Mumble.get_CurrentMap().get_Type());
		}

		private bool IsPlayerUnderOrCloseToWater()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z <= 0f;
		}

		private static Mount GetWaterMount()
		{
			return Module._mounts.SingleOrDefault((Mount m) => m.IsWaterMount && m.Name == Module._settingDefaultWaterMountChoice.get_Value());
		}

		internal Mount GetInstantMount()
		{
			if (IsPlayerInWvWMap())
			{
				return Module._mounts.Single((Mount m) => m.IsWvWMount);
			}
			if (IsPlayerUnderOrCloseToWater())
			{
				return GetWaterMount();
			}
			return null;
		}

		internal Mount GetCenterMount()
		{
			if (Module._settingMountRadialCenterMountBehavior.get_Value() == "Default")
			{
				return GetDefaultMount();
			}
			if (Module._settingMountRadialCenterMountBehavior.get_Value() == "LastUsed")
			{
				return GetLastUsedMount();
			}
			return null;
		}

		internal Mount GetDefaultMount()
		{
			return Module._mounts.SingleOrDefault((Mount m) => m.Name == Module._settingDefaultMountChoice.get_Value());
		}

		internal Mount GetLastUsedMount()
		{
			return (from m in Module._mounts
				where m.LastUsedTimestamp.HasValue
				orderby m.LastUsedTimestamp descending
				select m).FirstOrDefault();
		}

		public bool IsKeybindBeingTriggered()
		{
			return keybindSemaphore.CurrentCount != 1;
		}

		public async Task TriggerKeybind(SettingEntry<KeyBinding> keybindingSetting)
		{
			_ = 1;
			try
			{
				await keybindSemaphore.WaitAsync();
				Logger.Debug("TriggerKeybind entered");
				if ((int)keybindingSetting.get_Value().get_ModifierKeys() != 0)
				{
					Logger.Debug($"TriggerKeybind press modifiers {keybindingSetting.get_Value().get_ModifierKeys()}");
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
					{
						Keyboard.Press((VirtualKeyShort)18, true);
					}
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
					{
						Keyboard.Press((VirtualKeyShort)17, true);
					}
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
					{
						Keyboard.Press((VirtualKeyShort)16, true);
					}
				}
				Logger.Debug($"TriggerKeybind press PrimaryKey {keybindingSetting.get_Value().get_PrimaryKey()}");
				Keyboard.Press(ToVirtualKey(keybindingSetting.get_Value().get_PrimaryKey()), true);
				await Task.Delay(50);
				Logger.Debug($"TriggerKeybind release PrimaryKey {keybindingSetting.get_Value().get_PrimaryKey()}");
				Keyboard.Release(ToVirtualKey(keybindingSetting.get_Value().get_PrimaryKey()), true);
				if ((int)keybindingSetting.get_Value().get_ModifierKeys() != 0)
				{
					Logger.Debug($"TriggerKeybind release modifiers {keybindingSetting.get_Value().get_ModifierKeys()}");
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
					{
						Keyboard.Release((VirtualKeyShort)16, true);
					}
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
					{
						Keyboard.Release((VirtualKeyShort)17, true);
					}
					if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
					{
						Keyboard.Release((VirtualKeyShort)18, true);
					}
				}
			}
			finally
			{
				keybindSemaphore.Release();
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return (VirtualKeyShort)(short)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}

		public Helper()
		{
			MapType[] array = new MapType[6];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			warclawOnlyMaps = (MapType[])(object)array;
			base._002Ector();
		}
	}
}
