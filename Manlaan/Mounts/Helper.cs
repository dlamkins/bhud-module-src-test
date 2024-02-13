using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mounts.Settings;

namespace Manlaan.Mounts
{
	public class Helper
	{
		private static readonly Logger Logger = Logger.GetLogger<Helper>();

		private Dictionary<string, Thing> StoredThingForLater = new Dictionary<string, Thing>();

		private float _lastZPosition;

		private double _lastUpdateSeconds;

		private bool _isPlayerGlidingOrFalling;

		private Gw2ApiManager Gw2ApiManager;

		private bool _isCombatLaunchUnlocked;

		private DateTime lastTimeJumped = DateTime.MinValue;

		public Helper(Gw2ApiManager gw2ApiManager)
		{
			Gw2ApiManager = gw2ApiManager;
			Module._debug.Add("StoreThingForLaterActivation", () => string.Join(", ", StoredThingForLater.Select((KeyValuePair<string, Thing> x) => x.Key + "=" + x.Value.Name).ToArray()) ?? "");
		}

		public bool IsCombatLaunchUnlocked()
		{
			if (!_isCombatLaunchUnlocked)
			{
				return Module._settingCombatLaunchMasteryUnlocked.get_Value();
			}
			return true;
		}

		public async Task IsCombatLaunchUnlockedAsync()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)new List<TokenPermission> { (TokenPermission)6 }))
			{
				_isCombatLaunchUnlocked = false;
			}
			_isCombatLaunchUnlocked = ((IEnumerable<Mastery>)(await ((IAllExpandableClient<Mastery>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Masteries()).AllAsync(default(CancellationToken)))).Any((Mastery m) => m.get_Name() == "Combat Launch");
		}

		public bool IsPlayerInWvwMap()
		{
			MapType[] array = new MapType[6];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			return Array.Exists((MapType[])(object)array, (MapType mapType) => mapType == GameService.Gw2Mumble.get_CurrentMap().get_Type());
		}

		public bool IsPlayerUnderWater()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return (double)GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z <= -1.2;
		}

		public bool IsPlayerOnWaterSurface()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			float zpos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
			if ((double)zpos > -1.2)
			{
				return zpos < 0f;
			}
			return false;
		}

		public bool IsPlayerInCombat()
		{
			return GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
		}

		public bool IsPlayerMounted()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			return (int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() > 0;
		}

		public bool IsPlayerGlidingOrFalling()
		{
			return _isPlayerGlidingOrFalling;
		}

		public void UpdateLastJumped()
		{
			lastTimeJumped = DateTime.UtcNow;
		}

		private bool DidPlayerJumpRecently()
		{
			return DateTime.UtcNow.Subtract(lastTimeJumped).TotalMilliseconds < 5000.0;
		}

		public void UpdatePlayerGlidingOrFalling(GameTime gameTime)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			float currentZPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
			double currentUpdateSeconds = gameTime.get_TotalGameTime().TotalSeconds;
			double secondsDiff = currentUpdateSeconds - _lastUpdateSeconds;
			float zPositionDiff = currentZPosition - _lastZPosition;
			bool shouldUpdate = false;
			if (NewStuff(zPositionDiff, secondsDiff))
			{
				_lastZPosition = currentZPosition;
				_lastUpdateSeconds = currentUpdateSeconds;
			}
		}

		private bool NewStuff(float zPositionDiff, double secondsDiff)
		{
			double velocity = (double)zPositionDiff / secondsDiff;
			if (secondsDiff < 0.10000000149011612)
			{
				return false;
			}
			if (velocity > 10.0 || velocity < -10.0)
			{
				_isPlayerGlidingOrFalling = true;
			}
			else if (DidPlayerJumpRecently() && velocity < -2.0)
			{
				_isPlayerGlidingOrFalling = true;
			}
			else
			{
				_isPlayerGlidingOrFalling = false;
			}
			return true;
		}

		private bool OldStuff(float zPositionDiff, double secondsDiff)
		{
			if ((double)zPositionDiff < -0.0001 && secondsDiff != 0.0)
			{
				double velocity = (double)zPositionDiff / secondsDiff;
				_isPlayerGlidingOrFalling = velocity < -2.5;
			}
			else
			{
				_isPlayerGlidingOrFalling = false;
			}
			return true;
		}

		public static async Task TriggerKeybind(SettingEntry<KeyBinding> keybindingSetting)
		{
			Logger.Debug("TriggerKeybind entered");
			if ((int)keybindingSetting.get_Value().get_ModifierKeys() != 0)
			{
				Logger.Debug($"TriggerKeybind press modifiers {keybindingSetting.get_Value().get_ModifierKeys()}");
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Press((VirtualKeyShort)18, false);
				}
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Press((VirtualKeyShort)17, false);
				}
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Press((VirtualKeyShort)16, false);
				}
			}
			Logger.Debug($"TriggerKeybind press PrimaryKey {keybindingSetting.get_Value().get_PrimaryKey()}");
			Keyboard.Press(ToVirtualKey(keybindingSetting.get_Value().get_PrimaryKey()), false);
			await Task.Delay(50);
			Logger.Debug($"TriggerKeybind release PrimaryKey {keybindingSetting.get_Value().get_PrimaryKey()}");
			Keyboard.Release(ToVirtualKey(keybindingSetting.get_Value().get_PrimaryKey()), false);
			if ((int)keybindingSetting.get_Value().get_ModifierKeys() != 0)
			{
				Logger.Debug($"TriggerKeybind release modifiers {keybindingSetting.get_Value().get_ModifierKeys()}");
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Release((VirtualKeyShort)16, false);
				}
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Release((VirtualKeyShort)17, false);
				}
				if (((Enum)keybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Release((VirtualKeyShort)18, false);
				}
			}
		}

		private static VirtualKeyShort ToVirtualKey(Keys key)
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

		internal Thing GetQueuedThing()
		{
			return (from m in Module._things
				where m.QueuedTimestamp.HasValue
				orderby m.QueuedTimestamp descending
				select m).FirstOrDefault();
		}

		internal void StoreThingForLaterActivation(Thing thing, string characterName, string reason)
		{
			Logger.Debug("StoreThingForLaterActivation: " + thing.Name + " for character: " + characterName + " with reason: " + reason);
			StoredThingForLater[characterName] = thing;
		}

		internal bool IsSomethingStoredForLaterActivation(string characterName)
		{
			bool result = StoredThingForLater.ContainsKey(characterName);
			Logger.Debug(string.Format("{0} for character {1} : {2}", "IsSomethingStoredForLaterActivation", characterName, result));
			return result;
		}

		internal void ClearSomethingStoredForLaterActivation(string characterName)
		{
			Logger.Debug("ClearSomethingStoredForLaterActivation for character: " + characterName);
			StoredThingForLater.Remove(characterName);
		}

		internal async Task DoThingActionForLaterActivation(string characterName)
		{
			Thing thing = StoredThingForLater[characterName];
			Logger.Debug("ClearSomethingStoredForLaterActivation " + thing?.Name + " for character: " + characterName);
			await (thing?.DoAction(unconditionallyDoAction: false));
			ClearSomethingStoredForLaterActivation(characterName);
		}

		internal ContextualRadialThingSettings GetApplicableContextualRadialThingSettings()
		{
			return Module.ContextualRadialSettings.OrderBy((ContextualRadialThingSettings c) => c.Order).FirstOrDefault((ContextualRadialThingSettings c) => c.IsEnabled.get_Value() && c.IsApplicable());
		}

		internal RadialThingSettings GetTriggeredRadialSettings()
		{
			if (!((SettingEntry)Module._settingDefaultMountBinding).get_IsNull() && Module._settingDefaultMountBinding.get_Value().get_IsTriggering())
			{
				return GetApplicableContextualRadialThingSettings();
			}
			IEnumerable<UserDefinedRadialThingSettings> userdefinedList = Module.UserDefinedRadialSettings.Where((UserDefinedRadialThingSettings s) => !((SettingEntry)s.Keybind).get_IsNull() && s.Keybind.get_Value().get_IsTriggering());
			if (userdefinedList.Count() == 1)
			{
				return userdefinedList.Single();
			}
			return null;
		}
	}
}
