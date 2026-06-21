using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using DavidRice.BlishHud.MidiControl.Core;
using DavidRice.BlishHud.MidiControl.Input;
using DavidRice.BlishHud.MidiControl.Keymaps;
using DavidRice.BlishHud.MidiControl.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidRice.BlishHud.MidiControl
{
	[Export(typeof(Module))]
	public class MidiModule : Module
	{
		private sealed class MidiSettingsTabView : IView
		{
			private static readonly Logger Logger = Logger.GetLogger<MidiSettingsTabView>();

			private readonly MidiModule _module;

			private MidiSettingsView? _view;

			public bool WithPresenter => false;

			public event EventHandler<EventArgs>? Built;

			public event EventHandler<EventArgs>? Loaded;

			public event EventHandler<EventArgs>? Unloaded;

			public MidiSettingsTabView(MidiModule module)
			{
				_module = module;
			}

			public Task<bool> DoLoad(IProgress<string> progress)
			{
				progress.Report("MIDI Control settings loaded.");
				return Task.FromResult(result: true);
			}

			public void DoBuild(Container buildPanel)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				Panel val = new Panel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Size(((Control)buildPanel).get_Size());
				Panel panel = val;
				try
				{
					_view = new MidiSettingsView(_module);
					_view!.Build(panel);
				}
				catch (Exception ex)
				{
					Logger.Error("Build threw.", new object[1] { ex });
				}
			}

			public void DoUnload()
			{
				_view?.Unload();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MidiModule>();

		private SettingCollection _settingsCollection;

		private SettingEntry<string> _selectedMidiDeviceName;

		private SettingEntry<string> _selectedKeymapId;

		private SettingEntry<bool> _sendNotes;

		private SettingEntry<bool> _enableKeyHold;

		private SettingEntry<bool> _autoSwapOctave;

		private SettingEntry<int> _multipleOctaveShiftDelay;

		private SettingEntry<bool> _focusGuard;

		private SettingEntry<KeyBinding> _toggleSendNotesKeybind;

		private readonly ConcurrentQueue<MidiNoteEvent> _midiQueue = new ConcurrentQueue<MidiNoteEvent>();

		private MidiInputManager _midiInputManager;

		private KeymapRegistry _keymapRegistry;

		private KeySendThread _keySendThread;

		private KeySender _keySender;

		private readonly Queue<string> _recentSendLog = new Queue<string>(10);

		private CornerIcon? _cornerIcon;

		private Texture2D? _activeIconTexture;

		private Texture2D? _mutedIconTexture;

		private Texture2D? _disconnectedIconTexture;

		private TabbedWindow2? _settingsWindow;

		private AsyncTexture2D? _settingsTabIcon;

		private AsyncTexture2D? _layoutTabIcon;

		private bool _wasRetrying;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal Label? StatusLabel { get; set; }

		public IReadOnlyList<string> AvailableMidiDevices => MidiInputManager.AvailableDevices;

		public IReadOnlyList<Keymap> AvailableKeymaps => _keymapRegistry.AllKeymaps;

		public int CustomKeymapCount => _keymapRegistry.CustomKeymapCount;

		public IReadOnlyList<string> KeymapLoadErrors => _keymapRegistry.LoadErrors;

		public string SelectedMidiDeviceName => _selectedMidiDeviceName.get_Value();

		public string SelectedKeymapId => _selectedKeymapId.get_Value();

		public bool SendNotesEnabled
		{
			get
			{
				return _sendNotes.get_Value();
			}
			set
			{
				_sendNotes.set_Value(value);
			}
		}

		public bool EnableKeyHoldEnabled
		{
			get
			{
				return _enableKeyHold.get_Value();
			}
			set
			{
				_enableKeyHold.set_Value(value);
			}
		}

		public bool AutoSwapOctaveEnabled
		{
			get
			{
				return _autoSwapOctave.get_Value();
			}
			set
			{
				_autoSwapOctave.set_Value(value);
			}
		}

		public bool FocusGuardEnabled
		{
			get
			{
				return _focusGuard.get_Value();
			}
			set
			{
				_focusGuard.set_Value(value);
			}
		}

		public int MultipleOctaveShiftDelay
		{
			get
			{
				return _multipleOctaveShiftDelay.get_Value();
			}
			set
			{
				_multipleOctaveShiftDelay.set_Value(value);
			}
		}

		public string MidiDeviceStatus
		{
			get
			{
				if (_midiInputManager?.IsRetryingConnection ?? false)
				{
					return "Disconnected — retrying (was: " + _selectedMidiDeviceName.get_Value() + ")";
				}
				if ((_midiInputManager?.IsDeviceOpen ?? false) && !string.IsNullOrEmpty(_midiInputManager.ActiveDeviceName))
				{
					return "Connected: " + _midiInputManager.ActiveDeviceName;
				}
				if (!string.IsNullOrEmpty(_selectedMidiDeviceName.get_Value()))
				{
					return "Not connected. Saved: " + _selectedMidiDeviceName.get_Value();
				}
				return "No device selected.";
			}
		}

		public string LastSendLog
		{
			get
			{
				if (_recentSendLog.Count == 0)
				{
					return "No sends yet.";
				}
				return string.Join("\n", _recentSendLog.Reverse());
			}
		}

		public event Action? RecentSendLogUpdated;

		public event Action<bool>? SendNotesEnabledChanged;

		public event Action<string>? SelectedKeymapChanged;

		[ImportingConstructor]
		public MidiModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Expected O, but got Unknown
			_settingsCollection = settings;
			_selectedMidiDeviceName = settings.DefineSetting<string>("SelectedMidiDeviceName", string.Empty, (Func<string>)(() => "Selected MIDI Device"), (Func<string>)(() => "Name of the MIDI input device used for playing."));
			_selectedKeymapId = settings.DefineSetting<string>("SelectedKeymapId", "minstrel-auto", (Func<string>)(() => "Selected Keymap"), (Func<string>)(() => "The active keymap that maps MIDI notes to in-game keys."));
			_sendNotes = settings.DefineSetting<bool>("SendNotes", true, (Func<string>)(() => "Send Notes"), (Func<string>)(() => "If enabled, MIDI notes are sent as GW2 keyboard keypresses."));
			_sendNotes.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSendNotesChanged);
			_selectedKeymapId.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnKeymapChanged);
			_enableKeyHold = settings.DefineSetting<bool>("EnableKeyHold", false, (Func<string>)(() => "Enable Key Hold"), (Func<string>)(() => "When enabled, MIDI note-on sends a key-down and note-off sends a key-up. Otherwise notes are tapped."));
			_enableKeyHold.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableKeyHoldChanged);
			_autoSwapOctave = settings.DefineSetting<bool>("AutoSwapOctave", true, (Func<string>)(() => "Auto Swap Octave"), (Func<string>)(() => "Automatically shift octaves when playing notes outside the current range."));
			_multipleOctaveShiftDelay = settings.DefineSetting<int>("MultipleOctaveShiftDelay", 75, (Func<string>)(() => "Multi-Octave Shift Delay (ms)"), (Func<string>)(() => "Delay between octave shift keypresses when shifting multiple octaves."));
			_focusGuard = settings.DefineSetting<bool>("FocusGuard", true, (Func<string>)(() => "Focus Guard"), (Func<string>)(() => "Block key sending when Guild Wars 2 is not in focus."));
			_toggleSendNotesKeybind = settings.DefineSetting<KeyBinding>("ToggleSendNotesKeybind", new KeyBinding(), (Func<string>)(() => "Toggle Send Notes"), (Func<string>)(() => "Keybind to toggle Send Notes on or off."));
		}

		protected override void Initialize()
		{
			_keymapRegistry = new KeymapRegistry();
			string keymapsDir = DirectoriesManager.GetFullDirectoryPath("midi-keymaps");
			_keymapRegistry.LoadCustomKeymaps(keymapsDir);
			_midiInputManager = new MidiInputManager(_midiQueue);
			_recentSendLog.Enqueue("No sends yet.");
		}

		protected override async Task LoadAsync()
		{
			_keySendThread = new KeySendThread(new Action<SendAction>(HandleSendAction));
			_keySendThread.Start();
			_keySender = new KeySender(_keySendThread);
			_keySender.NoteProcessed += new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
			_toggleSendNotesKeybind.get_Value().set_Enabled(true);
			_toggleSendNotesKeybind.get_Value().set_BlockSequenceFromGw2(true);
			_toggleSendNotesKeybind.get_Value().set_IgnoreWhenInTextField(false);
			_toggleSendNotesKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleSendNotesKeybind);
			CreateCornerIcon();
			UpdateCornerIconState();
			if (!string.IsNullOrWhiteSpace(_selectedMidiDeviceName.get_Value()) && !_midiInputManager.Open(_selectedMidiDeviceName.get_Value()))
			{
				Logger.Warn("Could not re-open MIDI device '" + _selectedMidiDeviceName.get_Value() + "'.");
			}
			await Task.CompletedTask;
		}

		protected override void Update(GameTime gameTime)
		{
			bool isRetrying = _midiInputManager.IsRetryingConnection;
			bool isDeviceOpen = _midiInputManager.IsDeviceOpen;
			_midiInputManager.CheckConnection(_selectedMidiDeviceName.get_Value());
			if (isDeviceOpen && !_midiInputManager.IsDeviceOpen)
			{
				ReleaseAllKeys();
			}
			if (isRetrying != _wasRetrying)
			{
				_wasRetrying = isRetrying;
				UpdateCornerIconState();
			}
			if (StatusLabel != null)
			{
				StatusLabel!.set_Text(MidiDeviceStatus);
			}
			MidiNoteEvent noteEvent;
			while (_midiQueue.TryDequeue(out noteEvent))
			{
				if (_sendNotes.get_Value() && (!_focusGuard.get_Value() || GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning()))
				{
					Keymap keymap = GetActiveKeymap();
					if (keymap != null)
					{
						_keySender.Send(noteEvent, keymap, _autoSwapOctave.get_Value(), _multipleOctaveShiftDelay.get_Value(), _enableKeyHold.get_Value());
					}
				}
			}
		}

		protected override void Unload()
		{
			_sendNotes.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSendNotesChanged);
			_selectedKeymapId.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnKeymapChanged);
			_enableKeyHold.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableKeyHoldChanged);
			_keySender.NoteProcessed -= new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
			_toggleSendNotesKeybind.get_Value().set_Enabled(false);
			_toggleSendNotesKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleSendNotesKeybind);
			TabbedWindow2? settingsWindow = _settingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			_settingsWindow = null;
			StatusLabel = null;
			CornerIcon? cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			_cornerIcon = null;
			Texture2D? activeIconTexture = _activeIconTexture;
			if (activeIconTexture != null)
			{
				((GraphicsResource)activeIconTexture).Dispose();
			}
			_activeIconTexture = null;
			Texture2D? mutedIconTexture = _mutedIconTexture;
			if (mutedIconTexture != null)
			{
				((GraphicsResource)mutedIconTexture).Dispose();
			}
			_mutedIconTexture = null;
			Texture2D? disconnectedIconTexture = _disconnectedIconTexture;
			if (disconnectedIconTexture != null)
			{
				((GraphicsResource)disconnectedIconTexture).Dispose();
			}
			_disconnectedIconTexture = null;
			_keySendThread?.Dispose();
			_keySendThread = null;
			_midiInputManager?.Dispose();
			_midiInputManager = null;
			SafetyReleaseAllKeys();
		}

		private void CreateCornerIcon()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			try
			{
				_activeIconTexture = ContentsManager.GetTexture("icon.png");
				_mutedIconTexture = ContentsManager.GetTexture("icon_off.png");
				_disconnectedIconTexture = ContentsManager.GetTexture("icon_disconnected.png");
				_settingsTabIcon = new AsyncTexture2D(ContentsManager.GetTexture("icon.png"));
				_layoutTabIcon = new AsyncTexture2D(ContentsManager.GetTexture("layout.png"));
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(_activeIconTexture));
				((Control)val).set_BasicTooltipText(((Module)this).get_Name() + " — Active");
				val.set_Priority(1645843523);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					//IL_0039: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_0075: Expected O, but got Unknown
					//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
					//IL_00aa: Expected O, but got Unknown
					//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00df: Expected O, but got Unknown
					try
					{
						if (_settingsWindow == null)
						{
							AsyncTexture2D val2 = AsyncTexture2D.FromAssetId(155997);
							TabbedWindow2 val3 = new TabbedWindow2(val2, new Rectangle(24, 30, 545, 630), new Rectangle(82, 30, 467, 600));
							((Control)val3).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
							((WindowBase2)val3).set_Title("MIDI Control");
							((WindowBase2)val3).set_SavesPosition(true);
							((WindowBase2)val3).set_Id("MidiModule_Settings_6a2b3c4d");
							_settingsWindow = val3;
							_settingsWindow!.get_Tabs().Add(new Tab(_settingsTabIcon, (Func<IView>)(() => (IView)(object)new MidiSettingsTabView(this)), "Settings", (int?)null));
							_settingsWindow!.get_Tabs().Add(new Tab(_layoutTabIcon, (Func<IView>)(() => (IView)(object)new KeymapLayoutTabView(this)), "Keymap Layout", (int?)null));
						}
						((WindowBase2)_settingsWindow).ToggleWindow();
					}
					catch (Exception ex2)
					{
						Logger.Error("Corner icon click failed.", new object[1] { ex2 });
					}
				});
				((Control)_cornerIcon).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					_sendNotes.set_Value(!_sendNotes.get_Value());
				});
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to create corner icon.", new object[1] { ex });
			}
		}

		private void UpdateCornerIconState()
		{
			if (_cornerIcon != null)
			{
				if (_midiInputManager?.IsRetryingConnection ?? false)
				{
					_cornerIcon!.set_Icon(AsyncTexture2D.op_Implicit(_disconnectedIconTexture));
					((Control)_cornerIcon).set_BasicTooltipText(((Module)this).get_Name() + " — Disconnected");
				}
				else
				{
					_cornerIcon!.set_Icon(AsyncTexture2D.op_Implicit(_sendNotes.get_Value() ? _activeIconTexture : _mutedIconTexture));
					((Control)_cornerIcon).set_BasicTooltipText(((Module)this).get_Name() + " — " + (_sendNotes.get_Value() ? "Active" : "Muted") + "\nRight-click to toggle");
				}
			}
		}

		private void OnSendNotesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			UpdateCornerIconState();
			this.SendNotesEnabledChanged?.Invoke(e.get_NewValue());
			if (!e.get_NewValue())
			{
				ReleaseAllKeys();
			}
		}

		private void OnEnableKeyHoldChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ReleaseAllKeys();
		}

		private void OnToggleSendNotesKeybind(object sender, EventArgs e)
		{
			_sendNotes.set_Value(!_sendNotes.get_Value());
		}

		private void OnKeymapChanged(object sender, ValueChangedEventArgs<string> e)
		{
			ReleaseAllKeys();
			if (_keySendThread != null)
			{
				_keySender.NoteProcessed -= new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
				_keySender = new KeySender(_keySendThread);
				_keySender.NoteProcessed += new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
			}
		}

		private void OnNoteProcessed(MidiNoteEvent noteEvent, KeySendResult result)
		{
			if (result.SentKeyNames.Length == 0 && !result.WasSuppressed)
			{
				return;
			}
			string noteName = MidiNote.GetNoteName(noteEvent.NoteNumber);
			string keys = string.Join(" + ", result.SentKeyNames);
			if (result.WasSuppressed)
			{
				Keymap keymap = GetActiveKeymap();
				string mappedNoteName = MidiNote.GetNoteName(noteEvent.NoteNumber);
				if (keymap != null && keymap.Notes.TryGetValue(mappedNoteName, out var definition) && definition.Key != null)
				{
					string skippedKey = definition.Key + " (skipped)";
					keys = (string.IsNullOrEmpty(keys) ? skippedKey : (keys + " + " + skippedKey));
				}
			}
			string octavePrefix = ((result.PreviousOctave == result.NewOctave) ? $"oct {result.NewOctave}" : $"oct {result.PreviousOctave}→{result.NewOctave}");
			string suffix = string.Empty;
			if (_enableKeyHold.get_Value())
			{
				suffix = ((!result.WasSuppressed) ? (noteEvent.IsNoteOn ? " (down)" : " (up)") : (noteEvent.IsNoteOn ? " (down - skipped)" : " (up - skipped)"));
			}
			string desc = octavePrefix + ": " + noteName + " → " + keys + suffix;
			if (_recentSendLog.Count > 0 && _recentSendLog.Peek() == "No sends yet.")
			{
				_recentSendLog.Dequeue();
			}
			_recentSendLog.Enqueue(desc);
			while (_recentSendLog.Count > 10)
			{
				_recentSendLog.Dequeue();
			}
			this.RecentSendLogUpdated?.Invoke();
		}

		private Keymap? GetActiveKeymap()
		{
			string id = _selectedKeymapId.get_Value();
			Keymap keymap = _keymapRegistry.FindById(id);
			if (keymap != null)
			{
				return keymap;
			}
			Logger.Warn("Keymap '" + id + "' not found; falling back to 'minstrel-auto'.");
			keymap = _keymapRegistry.FindById("minstrel-auto");
			if (keymap != null)
			{
				return keymap;
			}
			Logger.Error("Fallback keymap 'minstrel-auto' not found. No notes will be sent.");
			return null;
		}

		public void OpenMidiDevice(string deviceName)
		{
			if (_midiInputManager.Open(deviceName))
			{
				_selectedMidiDeviceName.set_Value(deviceName);
				Logger.Info("MIDI device opened: " + deviceName);
			}
			else
			{
				Logger.Warn("Failed to open MIDI device: " + deviceName);
			}
		}

		public void SelectKeymap(string id)
		{
			_selectedKeymapId.set_Value(id);
			this.SelectedKeymapChanged?.Invoke(id);
		}

		public void ReloadKeymaps()
		{
			string keymapsDir = DirectoriesManager.GetFullDirectoryPath("midi-keymaps");
			_keymapRegistry.LoadCustomKeymaps(keymapsDir);
			string currentId = _selectedKeymapId.get_Value();
			if (_keymapRegistry.FindById(currentId) == null)
			{
				Logger.Warn("Selected keymap '" + currentId + "' no longer exists. Falling back to 'minstrel-auto'.");
				_selectedKeymapId.set_Value("minstrel-auto");
				this.SelectedKeymapChanged?.Invoke("minstrel-auto");
				ReleaseAllKeys();
				if (_keySendThread != null)
				{
					_keySender.NoteProcessed -= new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
					_keySender = new KeySender(_keySendThread);
					_keySender.NoteProcessed += new Action<MidiNoteEvent, KeySendResult>(OnNoteProcessed);
				}
			}
		}

		private void HandleSendAction(SendAction action)
		{
			switch (action.EventType)
			{
			case KeyEventType.KeyDown:
				SendInputApi.SendKeyDown(action.ScanCode);
				break;
			case KeyEventType.KeyUp:
				SendInputApi.SendKeyUp(action.ScanCode);
				break;
			default:
				SendInputApi.SendKeyTap(action.ScanCode);
				break;
			}
		}

		public void ReleaseAllKeys()
		{
			_keySender?.ReleaseAllHeldKeys();
			SafetyReleaseAllKeys();
		}

		private static void SafetyReleaseAllKeys()
		{
			uint[] array = new uint[10] { 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u, 10u, 11u };
			foreach (uint sc in array)
			{
				try
				{
					SendInputApi.SendKeyUp(sc);
				}
				catch
				{
				}
			}
		}
	}
}
