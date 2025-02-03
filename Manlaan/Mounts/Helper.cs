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
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mounts.Settings;
using Taimi.UndaDaSea_BlishHUD;

namespace Manlaan.Mounts
{
	public class Helper
	{
		private static readonly Logger Logger = Logger.GetLogger<Helper>();

		private Dictionary<string, Thing> StoredThingForLater = new Dictionary<string, Thing>();

		private Thing _storedRangedThing;

		private readonly Dictionary<(Keys, ModifierKeys), SemaphoreSlim> _semaphores;

		private float _lastZPosition;

		private double _lastUpdateSeconds;

		private bool _isPlayerGlidingOrFalling;

		private Gw2ApiManager Gw2ApiManager;

		private bool? _isCombatLaunchUnlocked;

		private string _isCombatLaunchUnlockedReason;

		private DateTime lastTimeJumped = DateTime.MinValue;

		public SkyLake _lake;

		public Thing StoredRangedThing
		{
			get
			{
				return _storedRangedThing;
			}
			set
			{
				if (this.RangedThingUpdated != null)
				{
					this.RangedThingUpdated(this, new ValueChangedEventArgs<Thing>(_storedRangedThing, value));
				}
				Logger.Debug("Setting StoredRangedThing to: " + value?.Name);
				_storedRangedThing = value;
			}
		}

		public event EventHandler<ValueChangedEventArgs<Thing>> RangedThingUpdated;

		public event EventHandler<ValueChangedEventArgs<Dictionary<string, Thing>>> StoredThingForLaterUpdated;

		public Helper(Gw2ApiManager gw2ApiManager)
		{
			_semaphores = new Dictionary<(Keys, ModifierKeys), SemaphoreSlim>();
			Gw2ApiManager = gw2ApiManager;
			Module._debug.Add("StoreThingForLaterActivation", () => string.Join(", ", StoredThingForLater.Select((KeyValuePair<string, Thing> x) => x.Key + "=" + x.Value.Name).ToArray()) ?? "");
			Module._debug.Add("Lake", () => $"name: {_lake?.Name} surface {_lake?.WaterSurface} Z {GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z} distance {_lake?.Distance}");
			Module._debug.Add("IsCombatLaunchUnlocked", () => _isCombatLaunchUnlockedReason ?? "");
		}

		public bool IsCombatLaunchUnlocked()
		{
			if (!_isCombatLaunchUnlocked.HasValue || _isCombatLaunchUnlocked.Value)
			{
				return Module._settingCombatLaunchMasteryUnlocked.get_Value();
			}
			return false;
		}

		public async Task IsCombatLaunchUnlockedAsync()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)new List<TokenPermission> { (TokenPermission)1 }))
			{
				_isCombatLaunchUnlockedReason = "API permissions \"account\" is not enabled.";
				Logger.Error("IsCombatLaunchUnlockedAsync " + _isCombatLaunchUnlockedReason);
				return;
			}
			try
			{
				_isCombatLaunchUnlocked = ((IEnumerable<AccountMastery>)(await ((IBlobClient<IApiV2ObjectList<AccountMastery>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Masteries()).GetAsync(default(CancellationToken)))).Any((AccountMastery m) => m.get_Id() == 36 && m.get_Level() >= 4);
				_isCombatLaunchUnlockedReason = "API call succeeded at " + DateTime.Now.ToString("HH:mm:ss") + ", CombatLaunch is " + (_isCombatLaunchUnlocked.Value ? "unlocked" : "not unlocked");
				Logger.Info("IsCombatLaunchUnlockedAsync " + _isCombatLaunchUnlockedReason);
			}
			catch (Exception ex)
			{
				_isCombatLaunchUnlockedReason = "something went wrong, see log for error";
				Logger.Error(ex, "IsCombatLaunchUnlockedAsync " + _isCombatLaunchUnlockedReason);
			}
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			float waterSurface = GetRelevantWaterSurface(playerPosition);
			return (double)playerPosition.Z <= (double)waterSurface - 1.2;
		}

		private float GetRelevantWaterSurface(Vector3 playerPosition)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			int currentMap = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			SkyLake lake2 = (_lake = (from lake in Module._skyLakes
				where lake.Map == currentMap
				where lake.IsNearby(playerPosition)
				orderby lake.Distance
				select lake).FirstOrDefault());
			float waterSurface = 0f;
			if (lake2 != null && lake2.IsInWater(playerPosition))
			{
				waterSurface = lake2.WaterSurface;
			}
			return waterSurface;
		}

		public bool IsPlayerOnWaterSurface()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			float zpos = playerPosition.Z;
			float waterSurface = GetRelevantWaterSurface(playerPosition);
			if ((double)zpos > (double)waterSurface - 1.2)
			{
				return zpos < waterSurface;
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
			if (!IsPlayerMounted())
			{
				lastTimeJumped = DateTime.UtcNow;
			}
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
			if (NewStuff(zPositionDiff, secondsDiff))
			{
				_lastZPosition = currentZPosition;
				_lastUpdateSeconds = currentUpdateSeconds;
			}
		}

		private bool NewStuff(float zPositionDiff, double secondsDiff)
		{
			double velocity = (double)zPositionDiff / secondsDiff;
			if (secondsDiff < (double)Module._settingFallingOrGlidingUpdateFrequency.get_Value())
			{
				return false;
			}
			Module._debug.Add("DidPlayerJumpRecently", () => $"{DidPlayerJumpRecently()}.");
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

		private SemaphoreSlim GetOrCreateSemaphore(KeyBinding keyBinding)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			lock (_semaphores)
			{
				if (!_semaphores.TryGetValue((keyBinding.get_PrimaryKey(), keyBinding.get_ModifierKeys()), out var semaphore))
				{
					semaphore = new SemaphoreSlim(1, 1);
					_semaphores[(keyBinding.get_PrimaryKey(), keyBinding.get_ModifierKeys())] = semaphore;
				}
				return semaphore;
			}
		}

		public async Task TriggerKeybind(SettingEntry<KeyBinding> keybindingSetting, WhichKeybindToRun whichKeyBindToRun)
		{
			SemaphoreSlim semaphore = GetOrCreateSemaphore(keybindingSetting.get_Value());
			await semaphore.WaitAsync();
			try
			{
				Logger.Debug("TriggerKeybind entered");
				if (whichKeyBindToRun == WhichKeybindToRun.Both || whichKeyBindToRun == WhichKeybindToRun.Press)
				{
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
				}
				if (whichKeyBindToRun != 0 && whichKeyBindToRun != WhichKeybindToRun.Release)
				{
					return;
				}
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
			finally
			{
				semaphore.Release();
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

		internal async Task DoRangedThing()
		{
			if (StoredRangedThing != null)
			{
				Thing thing = StoredRangedThing;
				Logger.Debug("DoRangedThing " + thing?.Name);
				await (thing?.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: false));
				StoredRangedThing = null;
			}
		}

		internal void StoreThingForLaterActivation(Thing thing, string reason)
		{
			string characterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			Logger.Debug("StoreThingForLaterActivation: " + thing.Name + " for character: " + characterName + " with reason: " + reason);
			StoredThingForLater[characterName] = thing;
			if (this.StoredThingForLaterUpdated != null)
			{
				this.StoredThingForLaterUpdated(this, new ValueChangedEventArgs<Dictionary<string, Thing>>((Dictionary<string, Thing>)null, StoredThingForLater));
			}
		}

		internal Thing IsSomethingStoredForLaterActivation()
		{
			string characterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			StoredThingForLater.TryGetValue(characterName, out var result);
			Logger.Debug("IsSomethingStoredForLaterActivation for character " + characterName + " : " + result?.Name);
			return result;
		}

		internal void ClearSomethingStoredForLaterActivation()
		{
			string characterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			Logger.Debug("ClearSomethingStoredForLaterActivation for character: " + characterName);
			StoredThingForLater.Remove(characterName);
			if (this.StoredThingForLaterUpdated != null)
			{
				this.StoredThingForLaterUpdated(this, new ValueChangedEventArgs<Dictionary<string, Thing>>((Dictionary<string, Thing>)null, StoredThingForLater));
			}
		}

		internal async Task DoThingActionForLaterActivation()
		{
			string characterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			Thing thing = StoredThingForLater[characterName];
			Logger.Debug("DoThingActionForLaterActivation " + thing?.Name + " for character: " + characterName);
			await (thing?.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: false));
			ClearSomethingStoredForLaterActivation();
		}

		internal ContextualRadialThingSettings GetApplicableContextualRadialThingSettings()
		{
			return Module.ContextualRadialSettings.OrderBy((ContextualRadialThingSettings c) => c.Order).FirstOrDefault((ContextualRadialThingSettings c) => c.IsEnabled.get_Value() && c.IsApplicable());
		}

		internal ContextualRadialThingSettings GetApplicableTriggeringContextualRadialThingSettings()
		{
			return Module.ContextualRadialSettings.OrderBy((ContextualRadialThingSettings c) => c.Order).FirstOrDefault((ContextualRadialThingSettings c) => c.IsEnabled.get_Value() && c.IsApplicable() && c.GetKeybind().get_Value().get_IsTriggering());
		}

		internal IEnumerable<RadialThingSettings> GetAllGenericRadialThingSettings()
		{
			List<RadialThingSettings> first = Module.ContextualRadialSettings.ConvertAll((Converter<ContextualRadialThingSettings, RadialThingSettings>)((ContextualRadialThingSettings x) => x));
			List<RadialThingSettings> userDefinedRadialSettingsCasted = Module.UserDefinedRadialSettings.ConvertAll((Converter<UserDefinedRadialThingSettings, RadialThingSettings>)((UserDefinedRadialThingSettings x) => x));
			return first.Concat(userDefinedRadialSettingsCasted);
		}

		internal RadialThingSettings GetTriggeredRadialSettings()
		{
			ContextualRadialThingSettings contextual = GetApplicableTriggeringContextualRadialThingSettings();
			if (contextual != null)
			{
				return contextual;
			}
			IEnumerable<UserDefinedRadialThingSettings> userdefinedList = Module.UserDefinedRadialSettings.Where((UserDefinedRadialThingSettings s) => s.GetKeybind().get_Value().get_IsTriggering());
			if (userdefinedList.Count() == 1)
			{
				return userdefinedList.Single();
			}
			return null;
		}
	}
}
