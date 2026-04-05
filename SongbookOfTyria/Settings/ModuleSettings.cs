using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using SongbookOfTyria.Models.Api;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.Settings
{
	public sealed class ModuleSettings : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettings>();

		private readonly SettingEntry<bool> _enableGuildAuthSetting;

		private readonly SettingEntry<string> _gw2ApiKeySetting;

		private readonly SettingCollection _settingsForView;

		private readonly object _verifyLock = new object();

		private TabsService _tabsService;

		private TextureService _textureService;

		private GuildAuthService _guildAuthService;

		private volatile bool _isVerifyingApiKey;

		public SettingEntry<KeyBinding> NoteC { get; private set; }

		public SettingEntry<KeyBinding> NoteD { get; private set; }

		public SettingEntry<KeyBinding> NoteE { get; private set; }

		public SettingEntry<KeyBinding> NoteF { get; private set; }

		public SettingEntry<KeyBinding> NoteG { get; private set; }

		public SettingEntry<KeyBinding> NoteA { get; private set; }

		public SettingEntry<KeyBinding> NoteB { get; private set; }

		public SettingEntry<KeyBinding> NoteCHigh { get; private set; }

		public SettingEntry<KeyBinding> OctaveDown { get; private set; }

		public SettingEntry<KeyBinding> OctaveUp { get; private set; }

		public SettingEntry<KeyBinding> SharpCs { get; private set; }

		public SettingEntry<KeyBinding> SharpDs { get; private set; }

		public SettingEntry<KeyBinding> SharpFs { get; private set; }

		public SettingEntry<KeyBinding> SharpGs { get; private set; }

		public SettingEntry<KeyBinding> SharpAs { get; private set; }

		public bool EnableGuildAuth => _enableGuildAuthSetting.get_Value();

		public string Gw2ApiKey => _gw2ApiKeySetting.get_Value();

		public SettingCollection SettingsForView => _settingsForView;

		public event EventHandler<StatusChangedEventArgs> CacheStatusChanged;

		public event EventHandler<StatusChangedEventArgs> AuthStatusChanged;

		public void SetEnableGuildAuth(bool value)
		{
			_enableGuildAuthSetting.set_Value(value);
		}

		public void SetGw2ApiKey(string value)
		{
			_gw2ApiKeySetting.set_Value(value);
		}

		public ModuleSettings(SettingCollection settings)
		{
			_settingsForView = settings.AddSubCollection("GuildAuth", false);
			_enableGuildAuthSetting = _settingsForView.DefineSetting<bool>("EnableGuildAuth", false, (Func<string>)(() => "Enable OPUS Guild Authentication"), (Func<string>)(() => "When enabled, uses your GW2 API key to verify OPUS guild membership and unlock private tabs."));
			_gw2ApiKeySetting = _settingsForView.DefineSetting<string>("Gw2ApiKey", string.Empty, (Func<string>)(() => "GW2 API Key"), (Func<string>)(() => "Enter your GW2 API key with 'account' permission. Get one at https://account.arena.net/applications"));
			_enableGuildAuthSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableGuildAuthSettingChanged);
			_gw2ApiKeySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnGw2ApiKeySettingChanged);
		}

		private void DefineInstrumentKeys(SettingCollection settings)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Expected O, but got Unknown
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Expected O, but got Unknown
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Expected O, but got Unknown
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Expected O, but got Unknown
			SettingCollection instrumentKeys = settings.AddSubCollection("InstrumentKeys", true, (Func<string>)(() => "Instrument Keys"));
			NoteC = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteC", new KeyBinding((Keys)49), (Func<string>)(() => "Note 1 (C)"), (Func<string>)(() => "Key for note C - match to Weapon Skill 1"));
			NoteD = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteD", new KeyBinding((Keys)50), (Func<string>)(() => "Note 2 (D)"), (Func<string>)(() => "Key for note D - match to Weapon Skill 2"));
			NoteE = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteE", new KeyBinding((Keys)51), (Func<string>)(() => "Note 3 (E)"), (Func<string>)(() => "Key for note E - match to Weapon Skill 3"));
			NoteF = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteF", new KeyBinding((Keys)52), (Func<string>)(() => "Note 4 (F)"), (Func<string>)(() => "Key for note F - match to Weapon Skill 4"));
			NoteG = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteG", new KeyBinding((Keys)53), (Func<string>)(() => "Note 5 (G)"), (Func<string>)(() => "Key for note G - match to Weapon Skill 5"));
			NoteA = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteA", new KeyBinding((Keys)54), (Func<string>)(() => "Note 6 (A)"), (Func<string>)(() => "Key for note A - match to Healing Skill"));
			NoteB = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteB", new KeyBinding((Keys)55), (Func<string>)(() => "Note 7 (B)"), (Func<string>)(() => "Key for note B - match to Utility Skill 1"));
			NoteCHigh = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteCHigh", new KeyBinding((Keys)56), (Func<string>)(() => "Note 8 (C High)"), (Func<string>)(() => "Key for high C - match to Utility Skill 2"));
			OctaveDown = instrumentKeys.DefineSetting<KeyBinding>("KeyOctaveDown", new KeyBinding((Keys)57), (Func<string>)(() => "Octave Down"), (Func<string>)(() => "Key to shift octave down - match to Utility Skill 3"));
			OctaveUp = instrumentKeys.DefineSetting<KeyBinding>("KeyOctaveUp", new KeyBinding((Keys)48), (Func<string>)(() => "Octave Up"), (Func<string>)(() => "Key to shift octave up - match to Elite Skill"));
		}

		private void DefinePianoSharps(SettingCollection settings)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Expected O, but got Unknown
			SettingCollection pianoSharps = settings.AddSubCollection("PianoSharps", true, (Func<string>)(() => "Piano Sharp Notes"));
			SharpCs = pianoSharps.DefineSetting<KeyBinding>("KeySharpCs", new KeyBinding((ModifierKeys)4, (Keys)49), (Func<string>)(() => "F1 - C#/Db"), (Func<string>)(() => "Key for C sharp / D flat"));
			SharpDs = pianoSharps.DefineSetting<KeyBinding>("KeySharpDs", new KeyBinding((ModifierKeys)4, (Keys)50), (Func<string>)(() => "F2 - D#/Eb"), (Func<string>)(() => "Key for D sharp / E flat"));
			SharpFs = pianoSharps.DefineSetting<KeyBinding>("KeySharpFs", new KeyBinding((ModifierKeys)4, (Keys)51), (Func<string>)(() => "F3 - F#/Gb"), (Func<string>)(() => "Key for F sharp / G flat"));
			SharpGs = pianoSharps.DefineSetting<KeyBinding>("KeySharpGs", new KeyBinding((ModifierKeys)4, (Keys)53), (Func<string>)(() => "F4 - G#/Ab"), (Func<string>)(() => "Key for G sharp / A flat"));
			SharpAs = pianoSharps.DefineSetting<KeyBinding>("KeySharpAs", new KeyBinding((ModifierKeys)4, (Keys)54), (Func<string>)(() => "F5 - A#/Bb"), (Func<string>)(() => "Key for A sharp / B flat"));
		}

		public int? GetMidiNoteFromKey(Keys key, ModifierKeys modifiers, int octaveOffset)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			if (MatchesKeyBinding(OctaveDown.get_Value(), key, modifiers))
			{
				return -100;
			}
			if (MatchesKeyBinding(OctaveUp.get_Value(), key, modifiers))
			{
				return -101;
			}
			int? baseNote = null;
			if (((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)4) || ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				if (MatchesKeyBinding(SharpCs.get_Value(), key, modifiers))
				{
					baseNote = 49;
				}
				else if (MatchesKeyBinding(SharpDs.get_Value(), key, modifiers))
				{
					baseNote = 51;
				}
				else if (MatchesKeyBinding(SharpFs.get_Value(), key, modifiers))
				{
					baseNote = 54;
				}
				else if (MatchesKeyBinding(SharpGs.get_Value(), key, modifiers))
				{
					baseNote = 56;
				}
				else if (MatchesKeyBinding(SharpAs.get_Value(), key, modifiers))
				{
					baseNote = 58;
				}
			}
			if (!baseNote.HasValue)
			{
				if (MatchesKeyBinding(NoteC.get_Value(), key, modifiers))
				{
					baseNote = 48;
				}
				else if (MatchesKeyBinding(NoteD.get_Value(), key, modifiers))
				{
					baseNote = 50;
				}
				else if (MatchesKeyBinding(NoteE.get_Value(), key, modifiers))
				{
					baseNote = 52;
				}
				else if (MatchesKeyBinding(NoteF.get_Value(), key, modifiers))
				{
					baseNote = 53;
				}
				else if (MatchesKeyBinding(NoteG.get_Value(), key, modifiers))
				{
					baseNote = 55;
				}
				else if (MatchesKeyBinding(NoteA.get_Value(), key, modifiers))
				{
					baseNote = 57;
				}
				else if (MatchesKeyBinding(NoteB.get_Value(), key, modifiers))
				{
					baseNote = 59;
				}
				else if (MatchesKeyBinding(NoteCHigh.get_Value(), key, modifiers))
				{
					baseNote = 60;
				}
			}
			if (baseNote.HasValue)
			{
				return baseNote.Value + octaveOffset * 12;
			}
			return null;
		}

		private static bool MatchesKeyBinding(KeyBinding binding, Keys key, ModifierKeys activeModifiers)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (binding.get_PrimaryKey() != key)
			{
				return false;
			}
			if ((int)binding.get_ModifierKeys() != 0)
			{
				return ((Enum)activeModifiers).HasFlag((Enum)(object)binding.get_ModifierKeys());
			}
			return true;
		}

		public void InitializeServices(TabsService tabsService, TextureService textureService, GuildAuthService guildAuthService)
		{
			_tabsService = tabsService;
			_textureService = textureService;
			_guildAuthService = guildAuthService;
		}

		public async Task InitializeGuildAuthAsync()
		{
			try
			{
				if (_enableGuildAuthSetting.get_Value() && !string.IsNullOrWhiteSpace(_gw2ApiKeySetting.get_Value()))
				{
					await VerifyGuildMembershipAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "InitializeGuildAuthAsync: Error");
			}
		}

		public async Task RefreshDataAsync()
		{
			if (_tabsService != null)
			{
				await _tabsService.RefreshTabsAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async void OnEnableGuildAuthSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				if (string.IsNullOrWhiteSpace(_gw2ApiKeySetting.get_Value()))
				{
					RaiseAuthStatus("Please enter your GW2 API key to enable guild authentication.", StatusType.Warning);
				}
				else
				{
					await VerifyGuildMembershipAsync();
				}
			}
			else
			{
				_guildAuthService?.ClearAuth();
				RaiseAuthStatus("Guild authentication disabled.", StatusType.Info);
			}
		}

		private async void OnGw2ApiKeySettingChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (_enableGuildAuthSetting.get_Value() && !string.IsNullOrWhiteSpace(e.get_NewValue()))
			{
				await VerifyGuildMembershipAsync();
			}
			else if (string.IsNullOrWhiteSpace(e.get_NewValue()))
			{
				_guildAuthService?.ClearAuth();
				RaiseAuthStatus("", StatusType.Info);
			}
		}

		private async Task VerifyGuildMembershipAsync()
		{
			lock (_verifyLock)
			{
				if (_isVerifyingApiKey || _guildAuthService == null)
				{
					return;
				}
				_isVerifyingApiKey = true;
			}
			RaiseAuthStatus("Verifying API key...", StatusType.Info);
			try
			{
				string apiKey = _gw2ApiKeySetting.get_Value();
				if (string.IsNullOrWhiteSpace(apiKey))
				{
					Logger.Warn("VerifyGuildMembershipAsync: No API key configured");
					RaiseAuthStatus("No API key configured.", StatusType.Warning);
					return;
				}
				AuthVerifyResponse response = await _guildAuthService.VerifyApiKeyAsync(apiKey);
				if (response == null)
				{
					RaiseAuthStatus("Failed to verify API key.", StatusType.Error);
				}
				else if (!response.Valid)
				{
					RaiseAuthStatus(response.Message ?? "Invalid API key.", StatusType.Error);
				}
				else if (response.InOpusGuild)
				{
					RaiseAuthStatus("Welcome, " + response.AccountName + "! Private tabs unlocked.", StatusType.Success);
				}
				else
				{
					RaiseAuthStatus("Verified as " + response.AccountName + ", but not an OPUS guild member.", StatusType.Warning);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "VerifyGuildMembershipAsync: Error");
				RaiseAuthStatus("Error verifying guild membership.", StatusType.Error);
			}
			finally
			{
				_isVerifyingApiKey = false;
			}
		}

		private void RaiseAuthStatus(string message, StatusType type)
		{
			this.AuthStatusChanged?.Invoke(this, new StatusChangedEventArgs(message, type));
		}

		public void RaiseCacheStatus(string message, StatusType type)
		{
			this.CacheStatusChanged?.Invoke(this, new StatusChangedEventArgs(message, type));
		}

		public void Dispose()
		{
			if (_enableGuildAuthSetting != null)
			{
				_enableGuildAuthSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableGuildAuthSettingChanged);
			}
			if (_gw2ApiKeySetting != null)
			{
				_gw2ApiKeySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnGw2ApiKeySettingChanged);
			}
		}
	}
}
