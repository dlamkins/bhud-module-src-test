using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace roguishpanda.AB_Bauble_Farm
{
	[Export(typeof(Module))]
	public class MainWindowModule : Module
	{
		private struct INPUT
		{
			public uint type;

			public INPUTUNION u;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct INPUTUNION
		{
			[FieldOffset(0)]
			public MOUSEINPUT mi;

			[FieldOffset(0)]
			public KEYBDINPUT ki;
		}

		private struct KEYBDINPUT
		{
			public ushort wVk;

			public ushort wScan;

			public uint dwFlags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		private struct MOUSEINPUT
		{
			public int dx;

			public int dy;

			public uint mouseData;

			public uint dwFlags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		private static readonly Logger Logger = Logger.GetLogger<MainWindowModule>();

		internal static MainWindowModule ModuleInstance;

		public Label[] _timerLabelDescriptions;

		public Label[] _timerLabels;

		public Label _statusValue;

		public Label _startTimeValue;

		public Label _endTimeValue;

		public List<List<string>> _timerWaypoints;

		public Checkbox _InOrdercheckbox;

		public DateTime elapsedDateTime;

		public DateTime initialDateTime;

		public int TimerRowNum;

		public int StaticRowNum;

		public StandardButton _stopButton;

		public StandardButton[] _stopButtons;

		public StandardButton[] _resetButtons;

		public Dropdown[] _customDropdownTimers;

		public DateTime?[] _timerStartTimes;

		public bool[] _timerRunning;

		public bool[] _timerTTSTriggered;

		public TimeSpan[] _timerDurationDefaults;

		public List<List<string>> _staticWaypoints;

		public bool[] _staticRunning;

		public Label[] _staticLabelDescriptions;

		public Image[] _staticNotesIcon;

		public Image[] _staticWaypointIcon;

		public Checkbox[] _staticCheckboxes;

		public TimeSpan[] _timerDurationOverride;

		public Panel[] _TimerWindowsOrdered;

		public Panel[] _StaticWindowsOrdered;

		public Panel _infoPanel;

		public Panel _timerPanel;

		public Panel _SettingsPanel;

		public StandardWindow _TimerWindow;

		private Panel _timerBackgroundPanel;

		public StandardWindow _StaticWindow;

		public Panel _staticBackgroundPanel;

		public Panel _staticPanel;

		public StandardWindow _InfoWindow;

		public TabbedWindow2 _SettingsWindow;

		public Panel _timerSettingsPanel;

		public CornerIcon _cornerIcon;

		public SettingEntry<KeyBinding> _toggleTimerWindowKeybind;

		public SettingEntry<KeyBinding> _toggleStaticWindowKeybind;

		public SettingEntry<KeyBinding> _stoneheadKeybind;

		public SettingEntry<KeyBinding> _postNotesKeybind;

		public SettingEntry<KeyBinding> _cancelNotesKeybind;

		public SettingCollection _MainSettingsCollection;

		public SettingCollection _PackageSettingsCollection;

		public SettingEntry<bool> _InOrdercheckboxDefault;

		public SettingEntry<bool> _hideStaticEventsDefault;

		private SettingEntry<bool> _DisableStartDefault;

		public SettingEntry<float> _OpacityDefault;

		public SettingEntry<int> _timerLowDefault;

		private SettingEntry<int> _timerIntermediateLowDefault;

		private SettingEntry<TargetChats> _TargetChatDefault;

		private SettingEntry<TimerColors> _TimerColorDefault;

		private SettingEntry<TimerColors> _LowTimerColorDefault;

		private SettingEntry<TimerColors> _IntermediateLowTimerColorDefault;

		public AsyncTexture2D _asyncTimertexture;

		public AsyncTexture2D _asyncGeneralSettingstexture;

		public AsyncTexture2D _asyncNotesSettingstexture;

		public Panel _inputPanel;

		public Label _instructionLabel;

		public Image[] _timerNotesIcon;

		public Image[] _timerWaypointIcon;

		public double[] _TimerMinutes;

		public double[] _TimerSeconds;

		public int[] _TimerID;

		public SettingEntry<string> _CurrentPackageSelection;

		public SettingCollection _settings;

		public List<PackageData> _PackageData;

		public List<TimerDetailData> _timerEvents;

		public List<StaticDetailData> _staticEvents;

		public Checkbox _hideStaticEventsCheckbox;

		public StandardButton _resetStaticEventsButton;

		public SettingEntry<string> _PackageSettingEntry;

		private Dictionary<TimerColors, Color> _colorMap;

		public string _CurrentPackage;

		private SettingEntry<int> _timerTTSVolumeDefault;

		private SettingEntry<int> _timerTTSSpeedDefault;

		public readonly JsonSerializerOptions _jsonOptions;

		private const uint KEYEVENTF_KEYUP = 2u;

		private const uint VK_SHIFT = 16u;

		private const uint VK_RETURN = 13u;

		private const uint VK_CONTROL = 17u;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public MainWindowModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			JsonSerializerOptions val = new JsonSerializerOptions();
			val.set_WriteIndented(true);
			_jsonOptions = val;
			((Module)this)._002Ector(moduleParameters);
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Expected O, but got Unknown
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Expected O, but got Unknown
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			_MainSettingsCollection = settings.AddSubCollection("MainSettings", false);
			_PackageSettingsCollection = settings.AddSubCollection("PackageSettings", false);
			_InOrdercheckboxDefault = _MainSettingsCollection.DefineSetting<bool>("InOrdercheckboxDefault", false, (Func<string>)(() => "Order by Timer"), (Func<string>)(() => "Check this box if you want to order your timers by time."));
			_hideStaticEventsDefault = _MainSettingsCollection.DefineSetting<bool>("hideStaticEventsDefault", false, (Func<string>)(() => "Hide Static Events"), (Func<string>)(() => "Check this box to hide static events that are completed."));
			_DisableStartDefault = _MainSettingsCollection.DefineSetting<bool>("_DisableStartDefault", false, (Func<string>)(() => "Disable Start Button When Pressed"), (Func<string>)(() => "Check this box to change Start button into Resart button."));
			_timerLowDefault = _MainSettingsCollection.DefineSetting<int>("LowTimerDefaultTimer", 30, (Func<string>)(() => "Low Timer"), (Func<string>)(() => "This timer setting (in seconds) will trigger when the low timer value is lower than the current timer value."));
			SettingComplianceExtensions.SetRange(_timerLowDefault, 1, 120);
			_timerIntermediateLowDefault = _MainSettingsCollection.DefineSetting<int>("IntermediateLowTimerDefaultTimer", 60, (Func<string>)(() => "Intermediate Timer"), (Func<string>)(() => "This timer setting (in seconds) will trigger when the low and intermediate combined values are lower than the current timer value."));
			SettingComplianceExtensions.SetRange(_timerIntermediateLowDefault, 1, 120);
			_OpacityDefault = _MainSettingsCollection.DefineSetting<float>("OpacityDefault", 1f, (Func<string>)(() => "Window Opacity"), (Func<string>)(() => "Changing the opacity will adjust how translucent the windows are."));
			SettingComplianceExtensions.SetRange(_OpacityDefault, 0.1f, 1f);
			_OpacityDefault.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)ChangeOpacity_Activated);
			_timerTTSVolumeDefault = _MainSettingsCollection.DefineSetting<int>("TTSVolumeDefaultTimer", 50, (Func<string>)(() => "TTS Volume"), (Func<string>)(() => "This controls the TTS volume."));
			SettingComplianceExtensions.SetRange(_timerTTSVolumeDefault, 0, 100);
			_timerTTSSpeedDefault = _MainSettingsCollection.DefineSetting<int>("TTSSpeedDefaultTimer", 0, (Func<string>)(() => "TTS Speed"), (Func<string>)(() => "This controls the TTS speaker's speed."));
			SettingComplianceExtensions.SetRange(_timerTTSSpeedDefault, -10, 10);
			_toggleTimerWindowKeybind = _MainSettingsCollection.DefineSetting<KeyBinding>("TimerKeybinding", new KeyBinding((ModifierKeys)4, (Keys)76), (Func<string>)(() => "Timer Window"), (Func<string>)(() => "Keybind to show or hide the Timer window."));
			_toggleTimerWindowKeybind.get_Value().set_BlockSequenceFromGw2(true);
			_toggleTimerWindowKeybind.get_Value().set_Enabled(true);
			_toggleTimerWindowKeybind.get_Value().add_Activated((EventHandler<EventArgs>)ToggleTimerWindowKeybind_Activated);
			_toggleStaticWindowKeybind = _MainSettingsCollection.DefineSetting<KeyBinding>("StaticKeybinding", new KeyBinding((ModifierKeys)4, (Keys)186), (Func<string>)(() => "Static Window"), (Func<string>)(() => "Keybind to show or hide the Static window."));
			_toggleStaticWindowKeybind.get_Value().set_BlockSequenceFromGw2(true);
			_toggleStaticWindowKeybind.get_Value().set_Enabled(true);
			_toggleStaticWindowKeybind.get_Value().add_Activated((EventHandler<EventArgs>)ToggleStaticWindowKeybind_Activated);
			_TargetChatDefault = _MainSettingsCollection.DefineSetting<TargetChats>("TargetChatDefault", TargetChats.None, (Func<string>)(() => "Target Chat"), (Func<string>)(() => "Pick the default chat shorts targeted chat."));
			_TimerColorDefault = _MainSettingsCollection.DefineSetting<TimerColors>("TimerColorDefault", TimerColors.Green, (Func<string>)(() => "Timer Color"), (Func<string>)(() => "Pick the color for the timer."));
			_TimerColorDefault.add_SettingChanged((EventHandler<ValueChangedEventArgs<TimerColors>>)_TimerColorDefault_SettingChanged);
			_LowTimerColorDefault = _MainSettingsCollection.DefineSetting<TimerColors>("LowTimerColorDefault", TimerColors.Red, (Func<string>)(() => "Low Timer Color"), (Func<string>)(() => "Pick the color for the low timer."));
			_IntermediateLowTimerColorDefault = _MainSettingsCollection.DefineSetting<TimerColors>("IntermediateLowTimerColorDefault", TimerColors.Orange, (Func<string>)(() => "Intermediate Timer Color"), (Func<string>)(() => "Pick the color for the intermediate timer."));
			_colorMap = new Dictionary<TimerColors, Color>
			{
				{
					TimerColors.Red,
					Color.get_Red()
				},
				{
					TimerColors.Green,
					Color.get_GreenYellow()
				},
				{
					TimerColors.Orange,
					Color.get_Orange()
				},
				{
					TimerColors.Blue,
					Color.get_LightBlue()
				},
				{
					TimerColors.Yellow,
					Color.get_Yellow()
				},
				{
					TimerColors.White,
					Color.get_White()
				}
			};
			_CurrentPackageSelection = _PackageSettingsCollection.DefineSetting<string>("CurrentPackageSelection", "Default", (Func<string>)(() => "Current Package"), (Func<string>)(() => "This is the current package selection"));
			_settings = settings;
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView();
		}

		private void _TimerColorDefault_SettingChanged(object sender, ValueChangedEventArgs<TimerColors> e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			for (int timerIndex = 0; timerIndex < TimerRowNum; timerIndex++)
			{
				TimerColors selectedEnum = _TimerColorDefault.get_Value();
				Color actualColor = _colorMap[selectedEnum];
				_timerLabels[timerIndex].set_TextColor(actualColor);
			}
		}

		private void CancelNotes_BindingChanged(object sender, EventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)_cancelNotesKeybind.get_Value().get_PrimaryKey() == 0)
			{
				for (int j = 0; j < _timerNotesIcon.Count(); j++)
				{
					((Control)_timerNotesIcon[j]).Hide();
				}
				for (int l = 0; l < _staticNotesIcon.Count(); l++)
				{
					((Control)_staticNotesIcon[l]).Hide();
				}
			}
			else
			{
				for (int i = 0; i < _timerNotesIcon.Count(); i++)
				{
					((Control)_timerNotesIcon[i]).Show();
				}
				for (int k = 0; k < _staticNotesIcon.Count(); k++)
				{
					((Control)_staticNotesIcon[k]).Show();
				}
			}
		}

		private void PostNotes_BindingChanged(object sender, EventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)_postNotesKeybind.get_Value().get_PrimaryKey() == 0)
			{
				for (int j = 0; j < _timerNotesIcon.Count(); j++)
				{
					((Control)_timerNotesIcon[j]).Hide();
				}
				for (int l = 0; l < _staticNotesIcon.Count(); l++)
				{
					((Control)_staticNotesIcon[l]).Hide();
				}
			}
			else
			{
				for (int i = 0; i < _timerNotesIcon.Count(); i++)
				{
					((Control)_timerNotesIcon[i]).Show();
				}
				for (int k = 0; k < _staticNotesIcon.Count(); k++)
				{
					((Control)_staticNotesIcon[k]).Show();
				}
			}
		}

		private void ToggleTimerWindowKeybind_Activated(object sender, EventArgs e)
		{
			if (((Control)_TimerWindow).get_Visible())
			{
				((Control)_TimerWindow).Hide();
			}
			else
			{
				((Control)_TimerWindow).Show();
			}
		}

		private void ToggleStaticWindowKeybind_Activated(object sender, EventArgs e)
		{
			if (((Control)_StaticWindow).get_Visible())
			{
				((Control)_StaticWindow).Hide();
			}
			else
			{
				((Control)_StaticWindow).Show();
			}
		}

		private void ChangeOpacity_Activated(object sender, EventArgs e)
		{
			((Control)_infoPanel).set_Opacity(_OpacityDefault.get_Value());
			((Control)_timerBackgroundPanel).set_Opacity(_OpacityDefault.get_Value());
			((Control)_staticBackgroundPanel).set_Opacity(_OpacityDefault.get_Value());
		}

		private void timerKeybinds(int timerIndex)
		{
			if (((Control)_resetButtons[timerIndex]).get_Enabled())
			{
				ResetButton_Click(timerIndex);
			}
			else
			{
				stopButtons_Click(timerIndex);
			}
		}

		public void LoadTimerDefaults(int TotalEvents)
		{
			for (int i = 0; i < TotalEvents; i++)
			{
				int count = i;
				SettingCollection obj = _settings.AddSubCollection(_CurrentPackage + "_PackageInfo", false).AddSubCollection("TimerInfo_" + _timerEvents[i].ID, false);
				SettingEntry<KeyBinding> KeybindSettingEntry = null;
				obj.TryGetSetting<KeyBinding>("Keybind", ref KeybindSettingEntry);
				SettingEntry<int> MintuesSettingEntry = null;
				obj.TryGetSetting<int>("TimerMinutes", ref MintuesSettingEntry);
				SettingEntry<int> SecondsSettingEntry = null;
				obj.TryGetSetting<int>("TimerSeconds", ref SecondsSettingEntry);
				if (KeybindSettingEntry != null)
				{
					KeybindSettingEntry.get_Value().set_BlockSequenceFromGw2(true);
					KeybindSettingEntry.get_Value().set_Enabled(true);
					KeybindSettingEntry.get_Value().add_Activated((EventHandler<EventArgs>)delegate
					{
						timerKeybinds(count);
					});
				}
				TimeSpan Minutes = TimeSpan.FromMinutes(_TimerMinutes[i]);
				TimeSpan Seconds = TimeSpan.FromSeconds(_TimerSeconds[i]);
				_timerDurationDefaults[i] = Minutes + Seconds;
				_timerLabels[i].set_Text(_timerDurationDefaults[i].ToString("mm\\:ss"));
			}
		}

		protected override void Initialize()
		{
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		private static void SendKey(uint virtualKey)
		{
			INPUT[] inputs = new INPUT[2];
			INPUT iNPUT = new INPUT
			{
				type = 1u
			};
			INPUTUNION u = default(INPUTUNION);
			KEYBDINPUT ki = new KEYBDINPUT
			{
				wVk = (ushort)virtualKey,
				wScan = (ushort)MapVirtualKey(virtualKey, 0u),
				dwFlags = 0u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[0] = iNPUT;
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = (ushort)virtualKey,
				wScan = (ushort)MapVirtualKey(virtualKey, 0u),
				dwFlags = 2u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[1] = iNPUT;
			SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(10);
		}

		private static void SendTwoKeys(uint keyone, uint keytwo)
		{
			INPUT[] inputs = new INPUT[4];
			INPUT iNPUT = new INPUT
			{
				type = 1u
			};
			INPUTUNION u = default(INPUTUNION);
			KEYBDINPUT ki = new KEYBDINPUT
			{
				wVk = (ushort)keyone,
				wScan = (ushort)MapVirtualKey(keyone, 0u),
				dwFlags = 0u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[0] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[0] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = (ushort)keytwo,
				wScan = (ushort)MapVirtualKey(keytwo, 0u),
				dwFlags = 0u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[1] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[1] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = (ushort)keytwo,
				wScan = (ushort)MapVirtualKey(keytwo, 0u),
				dwFlags = 2u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[2] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[2] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = (ushort)keyone,
				wScan = (ushort)MapVirtualKey(keyone, 0u),
				dwFlags = 2u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[3] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[3] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
		}

		private static void SendCtrlV()
		{
			INPUT[] inputs = new INPUT[4];
			INPUT iNPUT = new INPUT
			{
				type = 1u
			};
			INPUTUNION u = default(INPUTUNION);
			KEYBDINPUT ki = new KEYBDINPUT
			{
				wVk = 17,
				wScan = (ushort)MapVirtualKey(17u, 0u),
				dwFlags = 0u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[0] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[0] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = 86,
				wScan = (ushort)MapVirtualKey(86u, 0u),
				dwFlags = 0u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[1] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[1] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = 86,
				wScan = (ushort)MapVirtualKey(86u, 0u),
				dwFlags = 2u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[2] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[2] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
			iNPUT = new INPUT
			{
				type = 1u
			};
			u = default(INPUTUNION);
			ki = new KEYBDINPUT
			{
				wVk = 17,
				wScan = (ushort)MapVirtualKey(17u, 0u),
				dwFlags = 2u,
				time = 0u,
				dwExtraInfo = IntPtr.Zero
			};
			u.ki = ki;
			iNPUT.u = u;
			inputs[3] = iNPUT;
			SendInput(1u, new INPUT[1] { inputs[3] }, Marshal.SizeOf(typeof(INPUT)));
			Thread.Sleep(50);
		}

		public static void CopyToClipboard(string text)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				Clipboard.SetText(text);
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		private void ClipboardPaste(NotesData notesData)
		{
			if (notesData.Broadcast)
			{
				SendTwoKeys(16u, 13u);
				Thread.Sleep(100);
			}
			else
			{
				SendKey(13u);
			}
			Thread.Sleep(100);
			string TargetChat = _TargetChatDefault.get_Value().ToString();
			if (TargetChat != "None")
			{
				string chatPrefix = "";
				switch (TargetChat)
				{
				case "Squad":
					chatPrefix = "/d ";
					break;
				case "Party":
					chatPrefix = "/p ";
					break;
				case "Guild":
					chatPrefix = "/g ";
					break;
				case "Map":
					chatPrefix = "/m ";
					break;
				case "Say":
					chatPrefix = "/s ";
					break;
				}
				CopyToClipboard(chatPrefix + notesData.Notes);
			}
			else
			{
				CopyToClipboard(notesData.Notes);
			}
			Thread.Sleep(100);
			SendCtrlV();
			Thread.Sleep(100);
			SendKey(13u);
			Thread.Sleep(100);
		}

		private void NotesIcon_Click(int index, string eventType)
		{
			List<NotesData> notesData = new List<NotesData>();
			notesData = ((!(eventType == "Static")) ? _timerEvents[index].NotesData : _staticEvents[index].NotesData);
			for (int m = 0; m < _staticNotesIcon.Count(); m++)
			{
				((Control)_staticNotesIcon[m]).set_Enabled(false);
				((Control)_staticWaypointIcon[m]).set_Enabled(false);
			}
			for (int l = 0; l < _timerNotesIcon.Count(); l++)
			{
				((Control)_timerNotesIcon[l]).set_Enabled(false);
				((Control)_timerWaypointIcon[l]).set_Enabled(false);
			}
			for (int k = 0; k < notesData.Count; k++)
			{
				string message = notesData[k].Notes;
				if (message != null && message.Length > 0)
				{
					ClipboardPaste(notesData[k]);
				}
			}
			for (int j = 0; j < _staticNotesIcon.Count(); j++)
			{
				((Control)_staticNotesIcon[j]).set_Enabled(true);
				((Control)_staticWaypointIcon[j]).set_Enabled(true);
			}
			for (int i = 0; i < _timerNotesIcon.Count(); i++)
			{
				((Control)_timerNotesIcon[i]).set_Enabled(true);
				((Control)_timerWaypointIcon[i]).set_Enabled(true);
			}
		}

		private void WaypointIcon_Click(int index, string eventType)
		{
			List<NotesData> waypointData = new List<NotesData>();
			waypointData = ((!(eventType == "Static")) ? _timerEvents[index].WaypointData : _staticEvents[index].WaypointData);
			for (int m = 0; m < _staticNotesIcon.Count(); m++)
			{
				((Control)_staticNotesIcon[m]).set_Enabled(false);
				((Control)_staticWaypointIcon[m]).set_Enabled(false);
			}
			for (int l = 0; l < _timerNotesIcon.Count(); l++)
			{
				((Control)_timerNotesIcon[l]).set_Enabled(false);
				((Control)_timerWaypointIcon[l]).set_Enabled(false);
			}
			for (int k = 0; k < waypointData.Count; k++)
			{
				string message = waypointData[k].Notes;
				if (message != null && message.Length > 0)
				{
					ClipboardPaste(waypointData[k]);
				}
			}
			for (int j = 0; j < _staticNotesIcon.Count(); j++)
			{
				((Control)_staticNotesIcon[j]).set_Enabled(true);
				((Control)_staticWaypointIcon[j]).set_Enabled(true);
			}
			for (int i = 0; i < _timerNotesIcon.Count(); i++)
			{
				((Control)_timerNotesIcon[i]).set_Enabled(true);
				((Control)_timerWaypointIcon[i]).set_Enabled(true);
			}
		}

		private void ShowInputPanel(string Title)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Width(300);
			((Control)val).set_Height(40);
			((Control)val).set_BackgroundColor(Color.get_Black());
			((Control)val).set_Opacity(0.9f);
			_inputPanel = val;
			((Control)_inputPanel).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X - ((Control)_inputPanel).get_Size().X) / 2, 30));
			Label val2 = new Label();
			val2.set_Text("------" + Title + "------\n Press (" + _postNotesKeybind.get_Value().GetBindingDisplayText() + ") to continue... OR (" + _cancelNotesKeybind.get_Value().GetBindingDisplayText() + ") to cancel");
			((Control)val2).set_Size(new Point(500, 40));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Parent((Container)(object)_inputPanel);
			val2.set_Font(GameService.Content.get_DefaultFont12());
			_instructionLabel = val2;
			((Control)_instructionLabel).set_Location(new Point((((Control)_inputPanel).get_Size().X - ((Control)_instructionLabel).get_Size().X) / 2, (((Control)_inputPanel).get_Size().Y / 2 - ((Control)_instructionLabel).get_Size().Y) / 2));
		}

		private Task<bool> WaitForKeybindAsync()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			Keyboard.GetState();
			EventHandler<EventArgs> handler = null;
			EventHandler<EventArgs> handler2 = null;
			handler = delegate
			{
				tcs.TrySetResult(result: true);
				_postNotesKeybind.get_Value().remove_Activated(handler);
				_cancelNotesKeybind.get_Value().remove_Activated(handler2);
			};
			_postNotesKeybind.get_Value().add_Activated(handler);
			handler2 = delegate
			{
				tcs.TrySetResult(result: false);
				_postNotesKeybind.get_Value().remove_Activated(handler);
				_cancelNotesKeybind.get_Value().remove_Activated(handler2);
			};
			_cancelNotesKeybind.get_Value().add_Activated(handler2);
			return tcs.Task;
		}

		public async Task WaitForShiftKeyUpAsync()
		{
			InputService input = GameService.Input;
			TaskCompletionSource<bool> tcs;
			if (((Enum)input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				tcs = new TaskCompletionSource<bool>();
				input.get_Keyboard().add_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyStateChanged);
				await tcs.Task;
			}
			void KeyStateChanged(object sender, KeyboardEventArgs e)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				if (!((Enum)input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					input.get_Keyboard().remove_KeyStateChanged((EventHandler<KeyboardEventArgs>)KeyStateChanged);
					tcs.SetResult(result: true);
				}
			}
		}

		private void InfoIcon_Click(object sender, MouseEventArgs e)
		{
			if (((Control)_InfoWindow).get_Visible())
			{
				((Control)_InfoWindow).Hide();
			}
			else
			{
				((Control)_InfoWindow).Show();
			}
		}

		private void SettingsIcon_Click(object sender, MouseEventArgs e)
		{
			if (((Control)_SettingsWindow).get_Visible())
			{
				((Control)_SettingsWindow).Hide();
			}
			else
			{
				((Control)_SettingsWindow).Show();
			}
		}

		private void CornerIcon_Click(object sender, MouseEventArgs e)
		{
			if (((Control)_TimerWindow).get_Visible())
			{
				((Control)_TimerWindow).Hide();
				((Control)_StaticWindow).Hide();
			}
			else
			{
				((Control)_TimerWindow).Show();
				((Control)_StaticWindow).Show();
			}
		}

		private (DateTime NextBaubleStartDate, DateTime EndofBaubleWeek, string FarmStatus, Color Statuscolor) GetBaubleInformation()
		{
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
			DateTime currentTime = DateTime.Now;
			DateTime originBaubleStartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse("2025-08-28 20:00:00"), localTimeZone);
			int weekInterval = 3;
			int currentIntervalNumber = (int)Math.Floor((double)(int)Math.Floor((currentTime - originBaubleStartTime).TotalDays / 7.0) / (double)weekInterval);
			DateTime currentIntervalStartDate = originBaubleStartTime.AddDays(currentIntervalNumber * weekInterval * 7);
			DateTime nextThirdWeekIntervalStartDate = currentIntervalStartDate.AddDays(weekInterval * 7);
			DateTime NextBaubleStartDate = default(DateTime);
			DateTime EndofBaubleWeek = default(DateTime);
			DateTime oneWeekAheadcurrent = currentIntervalStartDate.AddDays(7.0);
			DateTime oneWeekAheadnext = nextThirdWeekIntervalStartDate.AddDays(7.0);
			string FarmStatus = "";
			Color Statuscolor = Color.get_Red();
			if (currentIntervalStartDate >= currentTime || currentTime <= oneWeekAheadcurrent)
			{
				NextBaubleStartDate = currentIntervalStartDate;
				EndofBaubleWeek = oneWeekAheadcurrent;
				FarmStatus = "ON";
				Statuscolor = Color.get_LimeGreen();
			}
			else
			{
				NextBaubleStartDate = nextThirdWeekIntervalStartDate;
				EndofBaubleWeek = oneWeekAheadnext;
				FarmStatus = "OFF";
				Statuscolor = Color.get_Red();
			}
			return (NextBaubleStartDate, EndofBaubleWeek, FarmStatus, Statuscolor);
		}

		private void ResetButton_Click(int timerIndex)
		{
			string DropdownValue = _customDropdownTimers[timerIndex].get_SelectedItem();
			_timerStartTimes[timerIndex] = DateTime.Now;
			_timerRunning[timerIndex] = true;
			_timerTTSTriggered[timerIndex] = true;
			if (_DisableStartDefault.get_Value())
			{
				((Control)_resetButtons[timerIndex]).set_Enabled(false);
				((Control)_customDropdownTimers[timerIndex]).set_Enabled(false);
			}
			if (DropdownValue != "Default" && int.TryParse(DropdownValue, out var totalMinutes))
			{
				_timerDurationOverride[timerIndex] = TimeSpan.FromMinutes(totalMinutes);
			}
			UpdateTimerJsonEvents();
		}

		private void stopButtons_Click(int timerIndex)
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			string DropdownValue = _customDropdownTimers[timerIndex].get_SelectedItem();
			if (_timerStartTimes[timerIndex].HasValue)
			{
				if (DropdownValue == "Default")
				{
					_timerLabels[timerIndex].set_Text($"{_timerDurationDefaults[timerIndex]:mm\\:ss}");
				}
				else
				{
					_timerLabels[timerIndex].set_Text($"{_timerDurationOverride[timerIndex]:mm\\:ss}");
				}
				_timerRunning[timerIndex] = false;
				_timerTTSTriggered[timerIndex] = false;
				TimerColors selectedEnum = _TimerColorDefault.get_Value();
				Color actualColor = _colorMap[selectedEnum];
				_timerLabels[timerIndex].set_TextColor(actualColor);
				((Control)_resetButtons[timerIndex]).set_Enabled(true);
				((Control)_customDropdownTimers[timerIndex]).set_Enabled(true);
			}
			UpdateTimerJsonEvents();
		}

		private void StopButton_Click()
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			for (int timerIndex = 0; timerIndex < TimerRowNum; timerIndex++)
			{
				string DropdownValue = _customDropdownTimers[timerIndex].get_SelectedItem();
				if (_timerStartTimes[timerIndex].HasValue)
				{
					if (DropdownValue == "Default")
					{
						_timerLabels[timerIndex].set_Text($"{_timerDurationDefaults[timerIndex]:mm\\:ss}");
					}
					else
					{
						_timerLabels[timerIndex].set_Text($"{_timerDurationOverride[timerIndex]:mm\\:ss}");
					}
					_timerRunning[timerIndex] = false;
					_timerTTSTriggered[timerIndex] = false;
					TimerColors selectedEnum = _TimerColorDefault.get_Value();
					Color actualColor = _colorMap[selectedEnum];
					_timerLabels[timerIndex].set_TextColor(actualColor);
					((Control)_resetButtons[timerIndex]).set_Enabled(true);
					((Control)_customDropdownTimers[timerIndex]).set_Enabled(true);
				}
			}
			UpdateTimerJsonEvents();
		}

		private void dropdownChanged_Click(int timerIndex)
		{
			string DropdownValue = _customDropdownTimers[timerIndex].get_SelectedItem();
			if (DropdownValue == "Default")
			{
				_timerLabels[timerIndex].set_Text($"{_timerDurationDefaults[timerIndex]:mm\\:ss}");
				return;
			}
			if (int.TryParse(DropdownValue, out var totalMinutes))
			{
				_timerDurationOverride[timerIndex] = TimeSpan.FromMinutes(totalMinutes);
			}
			_timerLabels[timerIndex].set_Text($"{_timerDurationOverride[timerIndex]:mm\\:ss}");
		}

		private void UpdateTimerJsonEvents()
		{
			List<TimerLogData> eventDataList = new List<TimerLogData>();
			string jsonFilePath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Event_Timers.json");
			for (int i = 0; i < TimerRowNum; i++)
			{
				DateTime? startTime = null;
				if (_timerRunning[i])
				{
					startTime = _timerStartTimes[i];
				}
				eventDataList.Add(new TimerLogData
				{
					ID = i,
					Description = (_timerLabelDescriptions[i].get_Text() ?? ""),
					StartTime = startTime,
					IsActive = _timerRunning[i],
					TTSTriggered = _timerTTSTriggered[i]
				});
			}
			try
			{
				string jsonContent = JsonSerializer.Serialize<List<TimerLogData>>(eventDataList, _jsonOptions);
				File.WriteAllText(jsonFilePath, jsonContent);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to save JSON file: " + ex.Message);
			}
		}

		private void CreateJsonEventsDefaults()
		{
			try
			{
				string jsonFilePath = "Defaults\\Package_Defaults.json";
				Stream json = ContentsManager.GetFileStream(jsonFilePath);
				using FileStream fileStream = File.Create(Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Package_Defaults.json"));
				json.CopyTo(fileStream);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to copy JSON event default file: " + ex.Message);
			}
		}

		protected override async Task LoadAsync()
		{
			try
			{
				_PackageData = new List<PackageData>();
				_timerEvents = new List<TimerDetailData>();
				_staticEvents = new List<StaticDetailData>();
				string jsonFilePath2 = Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Package_Defaults.json");
				if (!File.Exists(jsonFilePath2))
				{
					CreateJsonEventsDefaults();
				}
				using (StreamReader streamReader = new StreamReader(jsonFilePath2))
				{
					_PackageData = JsonSerializer.Deserialize<List<PackageData>>(await streamReader.ReadToEndAsync(), _jsonOptions);
				}
				int index3 = 0;
				int Defaultindex = _PackageData.FindIndex((PackageData p) => p.PackageName == _CurrentPackageSelection.get_Value());
				if (Defaultindex >= 0)
				{
					index3 = Defaultindex;
				}
				_timerEvents = _PackageData[index3].TimerDetailData;
				_staticEvents = _PackageData[index3].StaticDetailData;
				List<TimerDetailData> timerNotesData = _PackageData[index3].TimerDetailData;
				List<StaticDetailData> staticNotesData = _PackageData[index3].StaticDetailData;
				int TimerCount = _timerEvents.Count();
				int StaticCount = _staticEvents.Count();
				TimerRowNum = TimerCount;
				StaticRowNum = StaticCount;
				_CurrentPackage = "Undefined";
				SettingCollection PackageSettings = _settings.AddSubCollection("PackageSettings", false);
				if (PackageSettings != null)
				{
					_PackageSettingEntry = null;
					PackageSettings.TryGetSetting<string>("CurrentPackageSelection", ref _PackageSettingEntry);
					if (_PackageSettingEntry != null)
					{
						_CurrentPackage = _PackageSettingEntry.get_Value().ToString();
					}
				}
				_timerStartTimes = new DateTime?[TimerRowNum];
				_timerRunning = new bool[TimerRowNum];
				_timerTTSTriggered = new bool[TimerRowNum];
				_timerLabelDescriptions = (Label[])(object)new Label[TimerRowNum];
				_timerNotesIcon = (Image[])(object)new Image[TimerRowNum];
				_timerWaypointIcon = (Image[])(object)new Image[TimerRowNum];
				_timerLabels = (Label[])(object)new Label[TimerRowNum];
				_resetButtons = (StandardButton[])(object)new StandardButton[TimerRowNum];
				_stopButtons = (StandardButton[])(object)new StandardButton[TimerRowNum];
				_customDropdownTimers = (Dropdown[])(object)new Dropdown[TimerRowNum];
				_TimerWindowsOrdered = (Panel[])(object)new Panel[TimerRowNum];
				_timerDurationOverride = new TimeSpan[TimerRowNum];
				_timerDurationDefaults = new TimeSpan[TimerRowNum];
				_staticRunning = new bool[StaticRowNum];
				_staticLabelDescriptions = (Label[])(object)new Label[StaticRowNum];
				_staticNotesIcon = (Image[])(object)new Image[StaticRowNum];
				_staticWaypointIcon = (Image[])(object)new Image[StaticRowNum];
				_staticCheckboxes = (Checkbox[])(object)new Checkbox[StaticRowNum];
				_StaticWindowsOrdered = (Panel[])(object)new Panel[StaticRowNum];
				_TimerMinutes = new double[TimerRowNum];
				_TimerSeconds = new double[TimerRowNum];
				_TimerID = new int[TimerRowNum];
				for (int k = 0; k < TimerRowNum; k++)
				{
					_timerLabelDescriptions[k] = new Label();
					_timerLabelDescriptions[k].set_Text(timerNotesData[k].Description);
					_TimerMinutes[k] = timerNotesData[k].Minutes;
					_TimerSeconds[k] = timerNotesData[k].Seconds;
					_TimerID[k] = timerNotesData[k].ID;
				}
				for (int j2 = 0; j2 < StaticRowNum; j2++)
				{
					_staticLabelDescriptions[j2] = new Label();
					_staticLabelDescriptions[j2].set_Text(staticNotesData[j2].Description);
				}
			}
			catch (Exception ex4)
			{
				Logger.Warn("Failed to load Package_Defaults JSON file: " + ex4.Message);
			}
			for (int l = 0; l < TimerRowNum; l++)
			{
				_timerDurationDefaults[l] = TimeSpan.FromMinutes(0.0);
				_timerLabels[l] = new Label();
				_timerStartTimes[l] = null;
				_timerRunning[l] = false;
				_timerTTSTriggered[l] = false;
			}
			LoadTimerDefaults(TimerRowNum);
			for (int m = 0; m < StaticRowNum; m++)
			{
				_staticRunning[m] = false;
			}
			try
			{
				_asyncTimertexture = AsyncTexture2D.FromAssetId(155985);
				_asyncGeneralSettingstexture = AsyncTexture2D.FromAssetId(156701);
				_asyncNotesSettingstexture = AsyncTexture2D.FromAssetId(1654244);
				AsyncTexture2D NoTexture = new AsyncTexture2D();
				MainWindowModule mainWindowModule = this;
				StandardWindow val = new StandardWindow(NoTexture, new Rectangle(0, 0, 340, 220), new Rectangle(0, -10, 340, 220));
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val).set_Title("");
				((WindowBase2)val).set_SavesPosition(true);
				((WindowBase2)val).set_CanResize(true);
				((WindowBase2)val).set_Id("MainWindowModule_TimerWindow_38d37290-b5f9-447d-97ea-45b0b50e5f56");
				mainWindowModule._TimerWindow = val;
				((Control)_TimerWindow).add_Resized((EventHandler<ResizedEventArgs>)_TimerWindow_Resized);
				MainWindowModule mainWindowModule2 = this;
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)_TimerWindow);
				((Control)val2).set_Size(new Point(((Control)_TimerWindow).get_Size().X, ((Control)_TimerWindow).get_Size().Y));
				Rectangle contentRegion = ((Container)_TimerWindow).get_ContentRegion();
				int x = ((Rectangle)(ref contentRegion)).get_Location().X;
				contentRegion = ((Container)_TimerWindow).get_ContentRegion();
				((Control)val2).set_Location(new Point(x, ((Rectangle)(ref contentRegion)).get_Location().Y));
				((Control)val2).set_BackgroundColor(Color.get_Black());
				((Control)val2).set_Opacity(_OpacityDefault.get_Value());
				mainWindowModule2._timerBackgroundPanel = val2;
				double panelTimerScaleHeight = ((Control)_TimerWindow).get_Size().Y - 100;
				MainWindowModule mainWindowModule3 = this;
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)_TimerWindow);
				((Control)val3).set_Size(new Point(((Control)_TimerWindow).get_Size().X, (int)panelTimerScaleHeight));
				contentRegion = ((Container)_TimerWindow).get_ContentRegion();
				int x2 = ((Rectangle)(ref contentRegion)).get_Location().X;
				contentRegion = ((Container)_TimerWindow).get_ContentRegion();
				((Control)val3).set_Location(new Point(x2, ((Rectangle)(ref contentRegion)).get_Location().Y + 50));
				val3.set_CanScroll(true);
				mainWindowModule3._timerPanel = val3;
				MainWindowModule mainWindowModule4 = this;
				StandardWindow val4 = new StandardWindow(NoTexture, new Rectangle(0, 0, 340, 220), new Rectangle(0, -10, 340, 220));
				((Control)val4).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val4).set_Title("");
				((WindowBase2)val4).set_SavesPosition(true);
				((WindowBase2)val4).set_CanResize(true);
				((WindowBase2)val4).set_Id("MainWindowModule_StaticWindow_38d37290-b5f9-447d-97ea-45b0b50e5f56");
				mainWindowModule4._StaticWindow = val4;
				((Control)_StaticWindow).add_Resized((EventHandler<ResizedEventArgs>)_StaticWindow_Resized);
				MainWindowModule mainWindowModule5 = this;
				Panel val5 = new Panel();
				((Control)val5).set_Parent((Container)(object)_StaticWindow);
				((Control)val5).set_Size(new Point(((Control)_StaticWindow).get_Size().X, ((Control)_StaticWindow).get_Size().Y));
				contentRegion = ((Container)_StaticWindow).get_ContentRegion();
				int x3 = ((Rectangle)(ref contentRegion)).get_Location().X;
				contentRegion = ((Container)_StaticWindow).get_ContentRegion();
				((Control)val5).set_Location(new Point(x3, ((Rectangle)(ref contentRegion)).get_Location().Y));
				((Control)val5).set_BackgroundColor(Color.get_Black());
				((Control)val5).set_Opacity(_OpacityDefault.get_Value());
				mainWindowModule5._staticBackgroundPanel = val5;
				double panelStaticScaleHeight = ((Control)_StaticWindow).get_Size().Y - 100;
				MainWindowModule mainWindowModule6 = this;
				Panel val6 = new Panel();
				((Control)val6).set_Parent((Container)(object)_StaticWindow);
				((Control)val6).set_Size(new Point(((Control)_StaticWindow).get_Size().X, (int)panelStaticScaleHeight));
				contentRegion = ((Container)_StaticWindow).get_ContentRegion();
				int x4 = ((Rectangle)(ref contentRegion)).get_Location().X;
				contentRegion = ((Container)_StaticWindow).get_ContentRegion();
				((Control)val6).set_Location(new Point(x4, ((Rectangle)(ref contentRegion)).get_Location().Y + 50));
				val6.set_CanScroll(true);
				mainWindowModule6._staticPanel = val6;
				MainWindowModule mainWindowModule7 = this;
				StandardWindow val7 = new StandardWindow(NoTexture, new Rectangle(0, 0, 320, 130), new Rectangle(0, -10, 320, 130));
				((Control)val7).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val7).set_Title("Information");
				((WindowBase2)val7).set_SavesPosition(true);
				((WindowBase2)val7).set_Id("MainWindowModule_InfoWindow_38d37290-b5f9-447d-97ea-45b0b50e5f56");
				mainWindowModule7._InfoWindow = val7;
				MainWindowModule mainWindowModule8 = this;
				Panel val8 = new Panel();
				((Control)val8).set_Parent((Container)(object)_InfoWindow);
				contentRegion = ((Container)_InfoWindow).get_ContentRegion();
				int num = ((Rectangle)(ref contentRegion)).get_Size().X + 500;
				contentRegion = ((Container)_InfoWindow).get_ContentRegion();
				((Control)val8).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y + 500));
				contentRegion = ((Container)_InfoWindow).get_ContentRegion();
				((Control)val8).set_Location(((Rectangle)(ref contentRegion)).get_Location());
				((Control)val8).set_BackgroundColor(Color.get_Black());
				((Control)val8).set_Opacity(_OpacityDefault.get_Value());
				mainWindowModule8._infoPanel = val8;
				AsyncTexture2D cornertexture = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\1010539-modified.png"));
				MainWindowModule mainWindowModule9 = this;
				CornerIcon val9 = new CornerIcon();
				val9.set_Icon(cornertexture);
				((Control)val9).set_Size(new Point(32, 32));
				((Control)val9).set_BasicTooltipText("Custom Timers & Events");
				((Control)val9).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				mainWindowModule9._cornerIcon = val9;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
				(DateTime NextBaubleStartDate, DateTime EndofBaubleWeek, string FarmStatus, Color Statuscolor) baubleInformation = GetBaubleInformation();
				DateTime NextBaubleStartDate = baubleInformation.NextBaubleStartDate;
				DateTime EndofBaubleWeek = baubleInformation.EndofBaubleWeek;
				string FarmStatus = baubleInformation.FarmStatus;
				Color Statuscolor = baubleInformation.Statuscolor;
				initialDateTime = DateTime.Now;
				Label val10 = new Label();
				val10.set_Text("Bauble Farm Status :");
				((Control)val10).set_Size(new Point(180, 30));
				((Control)val10).set_Location(new Point(30, 30));
				val10.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val10).set_Parent((Container)(object)_InfoWindow);
				MainWindowModule mainWindowModule10 = this;
				Label val11 = new Label();
				val11.set_Text(FarmStatus);
				((Control)val11).set_Size(new Point(230, 30));
				((Control)val11).set_Location(new Point(190, 30));
				val11.set_Font(GameService.Content.get_DefaultFont16());
				val11.set_TextColor(Statuscolor);
				((Control)val11).set_Parent((Container)(object)_InfoWindow);
				mainWindowModule10._statusValue = val11;
				Label val12 = new Label();
				val12.set_Text("Start ->");
				((Control)val12).set_Size(new Point(100, 30));
				((Control)val12).set_Location(new Point(30, 60));
				val12.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val12).set_Parent((Container)(object)_InfoWindow);
				MainWindowModule mainWindowModule11 = this;
				Label val13 = new Label();
				val13.set_Text(NextBaubleStartDate.ToString("hh:mm tt (MMMM dd, yyyy)"));
				((Control)val13).set_Size(new Point(230, 30));
				((Control)val13).set_Location(new Point(90, 60));
				val13.set_Font(GameService.Content.get_DefaultFont16());
				val13.set_StrokeText(true);
				val13.set_TextColor(Color.get_DodgerBlue());
				((Control)val13).set_Parent((Container)(object)_InfoWindow);
				mainWindowModule11._startTimeValue = val13;
				Label val14 = new Label();
				val14.set_Text("End ->");
				((Control)val14).set_Size(new Point(100, 30));
				((Control)val14).set_Location(new Point(30, 90));
				val14.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val14).set_Parent((Container)(object)_InfoWindow);
				MainWindowModule mainWindowModule12 = this;
				Label val15 = new Label();
				val15.set_Text(EndofBaubleWeek.ToString("hh:mm tt (MMMM dd, yyyy)"));
				((Control)val15).set_Size(new Point(230, 30));
				((Control)val15).set_Location(new Point(80, 90));
				val15.set_Font(GameService.Content.get_DefaultFont16());
				val15.set_StrokeText(true);
				val15.set_TextColor(Color.get_DodgerBlue());
				((Control)val15).set_Parent((Container)(object)_InfoWindow);
				mainWindowModule12._endTimeValue = val15;
				MainWindowModule mainWindowModule13 = this;
				StandardButton val16 = new StandardButton();
				val16.set_Text("Stop All Timers");
				((Control)val16).set_Size(new Point(120, 30));
				((Control)val16).set_Location(new Point(0, 30));
				((Control)val16).set_Parent((Container)(object)_TimerWindow);
				mainWindowModule13._stopButton = val16;
				((Control)_stopButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					StopButton_Click();
				});
				MainWindowModule mainWindowModule14 = this;
				Checkbox val17 = new Checkbox();
				val17.set_Text("Order by Timer");
				((Control)val17).set_Size(new Point(120, 30));
				((Control)val17).set_Location(new Point(130, 30));
				((Control)val17).set_Parent((Container)(object)_TimerWindow);
				mainWindowModule14._InOrdercheckbox = val17;
				_InOrdercheckbox.set_Checked(_InOrdercheckboxDefault.get_Value());
				((Control)_InOrdercheckbox).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					InOrdercheckbox_Click();
				});
				Label val18 = new Label();
				val18.set_Text("Add an event OR select a package\nin the settings");
				((Control)val18).set_Size(new Point(250, 60));
				((Control)val18).set_Location(new Point(50, 80));
				val18.set_Font(GameService.Content.get_DefaultFont16());
				val18.set_StrokeText(true);
				val18.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val18).set_Visible(false);
				val18.set_TextColor(Color.get_Gold());
				((Control)val18).set_Parent((Container)(object)_TimerWindow);
				Label eventsLabel = val18;
				if (TimerRowNum > 0)
				{
					((Control)eventsLabel).set_Visible(false);
					((Control)_timerPanel).set_Visible(true);
				}
				else
				{
					((Control)eventsLabel).set_Visible(true);
					((Control)_timerPanel).set_Visible(false);
				}
				AsyncTexture2D infoTexture = AsyncTexture2D.FromAssetId(440023);
				Image val19 = new Image();
				val19.set_Texture(infoTexture);
				((Control)val19).set_Location(new Point(270, 30));
				((Control)val19).set_Size(new Point(32, 32));
				((Control)val19).set_Opacity(0.7f);
				((Control)val19).set_Visible(false);
				((Control)val19).set_Parent((Container)(object)_TimerWindow);
				Image infoIcon = val19;
				((Control)infoIcon).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)infoIcon).set_Location(new Point(266, 26));
					((Control)infoIcon).set_Size(new Point(40, 40));
					((Control)infoIcon).set_Opacity(1f);
				});
				((Control)infoIcon).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)infoIcon).set_Location(new Point(270, 30));
					((Control)infoIcon).set_Size(new Point(32, 32));
					((Control)infoIcon).set_Opacity(0.7f);
				});
				((Control)infoIcon).add_Click((EventHandler<MouseEventArgs>)InfoIcon_Click);
				AsyncTexture2D geartexture = AsyncTexture2D.FromAssetId(155052);
				Image val20 = new Image();
				val20.set_Texture(geartexture);
				((Control)val20).set_Location(new Point(300, 30));
				((Control)val20).set_Size(new Point(32, 32));
				((Control)val20).set_Opacity(0.7f);
				((Control)val20).set_Parent((Container)(object)_TimerWindow);
				Image settingsIcon = val20;
				((Control)settingsIcon).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)settingsIcon).set_Location(new Point(296, 26));
					((Control)settingsIcon).set_Size(new Point(40, 40));
					((Control)settingsIcon).set_Opacity(1f);
				});
				((Control)settingsIcon).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)settingsIcon).set_Location(new Point(300, 30));
					((Control)settingsIcon).set_Size(new Point(32, 32));
					((Control)settingsIcon).set_Opacity(0.7f);
				});
				((Control)settingsIcon).add_Click((EventHandler<MouseEventArgs>)SettingsIcon_Click);
				AsyncTexture2D waypointTexture = AsyncTexture2D.FromAssetId(102348);
				AsyncTexture2D notesTexture = AsyncTexture2D.FromAssetId(2604584);
				for (int n = 0; n < TimerRowNum; n++)
				{
					int index2 = n;
					Panel[] timerWindowsOrdered = _TimerWindowsOrdered;
					int num2 = n;
					Panel val21 = new Panel();
					((Control)val21).set_Parent((Container)(object)_timerPanel);
					((Control)val21).set_Size(new Point(390, 30));
					((Control)val21).set_Location(new Point(0, n * 30));
					timerWindowsOrdered[num2] = val21;
					Image[] timerWaypointIcon = _timerWaypointIcon;
					int num3 = n;
					Image val22 = new Image();
					val22.set_Texture(waypointTexture);
					((Control)val22).set_Location(new Point(0, 0));
					((Control)val22).set_Size(new Point(32, 32));
					((Control)val22).set_Opacity(0.7f);
					((Control)val22).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					timerWaypointIcon[num3] = val22;
					((Control)_timerWaypointIcon[n]).add_MouseEntered((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						object obj8 = ((sender is Image) ? sender : null);
						((Control)obj8).set_Location(new Point(-2, -2));
						((Control)obj8).set_Size(new Point(36, 36));
						((Control)obj8).set_Opacity(1f);
					});
					((Control)_timerWaypointIcon[n]).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_0009: Unknown result type (might be due to invalid IL or missing references)
						//IL_0018: Unknown result type (might be due to invalid IL or missing references)
						object obj7 = ((sender is Image) ? sender : null);
						((Control)obj7).set_Location(new Point(0, 0));
						((Control)obj7).set_Size(new Point(32, 32));
						((Control)obj7).set_Opacity(0.7f);
					});
					((Control)_timerWaypointIcon[n]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						WaypointIcon_Click(index2, "Timer");
					});
					Image[] timerNotesIcon = _timerNotesIcon;
					int num4 = n;
					Image val23 = new Image();
					val23.set_Texture(notesTexture);
					((Control)val23).set_Location(new Point(30, 0));
					((Control)val23).set_Size(new Point(32, 32));
					((Control)val23).set_Opacity(0.7f);
					((Control)val23).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					timerNotesIcon[num4] = val23;
					((Control)_timerNotesIcon[n]).add_MouseEntered((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						object obj6 = ((sender is Image) ? sender : null);
						((Control)obj6).set_Location(new Point(28, -2));
						((Control)obj6).set_Size(new Point(36, 36));
						((Control)obj6).set_Opacity(1f);
					});
					((Control)_timerNotesIcon[n]).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						object obj5 = ((sender is Image) ? sender : null);
						((Control)obj5).set_Location(new Point(30, 0));
						((Control)obj5).set_Size(new Point(32, 32));
						((Control)obj5).set_Opacity(0.7f);
					});
					((Control)_timerNotesIcon[n]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						NotesIcon_Click(index2, "Timer");
					});
					bool waypointNote = false;
					for (int k3 = 0; k3 < _timerEvents[n].WaypointData.Count; k3++)
					{
						if (_timerEvents[n].WaypointData[k3].Notes != "")
						{
							waypointNote = true;
						}
					}
					if (!waypointNote)
					{
						((Control)_timerWaypointIcon[n]).set_Visible(false);
					}
					bool notesNote = false;
					for (int k2 = 0; k2 < _timerEvents[n].NotesData.Count; k2++)
					{
						if (_timerEvents[n].NotesData[k2].Notes != "")
						{
							notesNote = true;
						}
					}
					if (!notesNote)
					{
						((Control)_timerNotesIcon[n]).set_Visible(false);
					}
					((Control)_timerLabelDescriptions[n]).set_Size(new Point(100, 30));
					((Control)_timerLabelDescriptions[n]).set_Location(new Point(60, 0));
					((Control)_timerLabelDescriptions[n]).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					_timerLabels[n].set_Text(_timerDurationDefaults[n].ToString("mm\\:ss"));
					((Control)_timerLabels[n]).set_Size(new Point(100, 30));
					((Control)_timerLabels[n]).set_Location(new Point(130, 0));
					_timerLabels[n].set_HorizontalAlignment((HorizontalAlignment)1);
					_timerLabels[n].set_Font(GameService.Content.get_DefaultFont16());
					((Control)_timerLabels[n]).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					TimerColors selectedEnum = _TimerColorDefault.get_Value();
					Color actualColor = _colorMap[selectedEnum];
					_timerLabels[n].set_TextColor(actualColor);
					StandardButton[] resetButtons = _resetButtons;
					int num5 = n;
					StandardButton val24 = new StandardButton();
					val24.set_Text("Start");
					((Control)val24).set_Size(new Point(50, 30));
					((Control)val24).set_Location(new Point(210, 0));
					((Control)val24).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					resetButtons[num5] = val24;
					((Control)_resetButtons[n]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ResetButton_Click(index2);
					});
					StandardButton[] stopButtons = _stopButtons;
					int num6 = n;
					StandardButton val25 = new StandardButton();
					val25.set_Text("Stop");
					((Control)val25).set_Size(new Point(50, 30));
					((Control)val25).set_Location(new Point(260, 0));
					((Control)val25).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					stopButtons[num6] = val25;
					((Control)_stopButtons[n]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						stopButtons_Click(index2);
					});
					Dropdown[] customDropdownTimers = _customDropdownTimers;
					int num7 = n;
					Dropdown val26 = new Dropdown();
					val26.get_Items().Add("Default");
					val26.get_Items().Add("1");
					val26.get_Items().Add("2");
					val26.get_Items().Add("3");
					val26.get_Items().Add("4");
					val26.get_Items().Add("5");
					val26.get_Items().Add("6");
					val26.get_Items().Add("7");
					val26.get_Items().Add("8");
					val26.get_Items().Add("9");
					val26.get_Items().Add("10");
					val26.get_Items().Add("11");
					val26.get_Items().Add("12");
					val26.get_Items().Add("13");
					val26.get_Items().Add("14");
					val26.get_Items().Add("15");
					((Control)val26).set_Size(new Point(80, 30));
					((Control)val26).set_Location(new Point(310, 0));
					((Control)val26).set_Visible(false);
					((Control)val26).set_Parent((Container)(object)_TimerWindowsOrdered[n]);
					customDropdownTimers[num7] = val26;
					_customDropdownTimers[n].add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
					{
						dropdownChanged_Click(index2);
					});
				}
				MainWindowModule mainWindowModule15 = this;
				StandardButton val27 = new StandardButton();
				val27.set_Text("Reset Events");
				((Control)val27).set_Size(new Point(120, 30));
				((Control)val27).set_Location(new Point(0, 30));
				((Control)val27).set_Parent((Container)(object)_StaticWindow);
				mainWindowModule15._resetStaticEventsButton = val27;
				((Control)_resetStaticEventsButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_resetStaticEventsButton_Click();
				});
				MainWindowModule mainWindowModule16 = this;
				Checkbox val28 = new Checkbox();
				val28.set_Text("Hide Completions");
				((Control)val28).set_Size(new Point(120, 30));
				((Control)val28).set_Location(new Point(130, 30));
				((Control)val28).set_Parent((Container)(object)_StaticWindow);
				mainWindowModule16._hideStaticEventsCheckbox = val28;
				_hideStaticEventsCheckbox.set_Checked(_hideStaticEventsDefault.get_Value());
				((Control)_hideStaticEventsCheckbox).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_hideStaticEventsCheckbox_Click();
				});
				Label val29 = new Label();
				val29.set_Text("Add an event OR select a package\nin the settings");
				((Control)val29).set_Size(new Point(250, 60));
				((Control)val29).set_Location(new Point(50, 80));
				val29.set_Font(GameService.Content.get_DefaultFont16());
				val29.set_StrokeText(true);
				val29.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val29).set_Visible(false);
				val29.set_TextColor(Color.get_Gold());
				((Control)val29).set_Parent((Container)(object)_StaticWindow);
				Label staticEventsLabel = val29;
				if (StaticRowNum > 0)
				{
					((Control)staticEventsLabel).set_Visible(false);
					((Control)_staticPanel).set_Visible(true);
				}
				else
				{
					((Control)staticEventsLabel).set_Visible(true);
					((Control)_staticPanel).set_Visible(false);
				}
				Image val30 = new Image();
				val30.set_Texture(infoTexture);
				((Control)val30).set_Location(new Point(270, 30));
				((Control)val30).set_Size(new Point(32, 32));
				((Control)val30).set_Visible(false);
				((Control)val30).set_Opacity(0.7f);
				((Control)val30).set_Parent((Container)(object)_StaticWindow);
				Image infoIcon2 = val30;
				((Control)infoIcon2).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)infoIcon2).set_Location(new Point(266, 26));
					((Control)infoIcon2).set_Size(new Point(40, 40));
					((Control)infoIcon2).set_Opacity(1f);
				});
				((Control)infoIcon2).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)infoIcon2).set_Location(new Point(270, 30));
					((Control)infoIcon2).set_Size(new Point(32, 32));
					((Control)infoIcon2).set_Opacity(0.7f);
				});
				((Control)infoIcon2).add_Click((EventHandler<MouseEventArgs>)InfoIcon_Click);
				Image val31 = new Image();
				val31.set_Texture(geartexture);
				((Control)val31).set_Location(new Point(300, 30));
				((Control)val31).set_Size(new Point(32, 32));
				((Control)val31).set_Opacity(0.7f);
				((Control)val31).set_Parent((Container)(object)_StaticWindow);
				Image settingsIcon2 = val31;
				((Control)settingsIcon2).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)settingsIcon2).set_Location(new Point(296, 26));
					((Control)settingsIcon2).set_Size(new Point(40, 40));
					((Control)settingsIcon2).set_Opacity(1f);
				});
				((Control)settingsIcon2).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)settingsIcon2).set_Location(new Point(300, 30));
					((Control)settingsIcon2).set_Size(new Point(32, 32));
					((Control)settingsIcon2).set_Opacity(0.7f);
				});
				((Control)settingsIcon2).add_Click((EventHandler<MouseEventArgs>)SettingsIcon_Click);
				for (int j3 = 0; j3 < StaticRowNum; j3++)
				{
					int index = j3;
					Panel[] staticWindowsOrdered = _StaticWindowsOrdered;
					int num8 = j3;
					Panel val32 = new Panel();
					((Control)val32).set_Parent((Container)(object)_staticPanel);
					((Control)val32).set_Size(new Point(390, 30));
					((Control)val32).set_Location(new Point(0, j3 * 30));
					staticWindowsOrdered[num8] = val32;
					Image[] staticWaypointIcon = _staticWaypointIcon;
					int num9 = j3;
					Image val33 = new Image();
					val33.set_Texture(waypointTexture);
					((Control)val33).set_Location(new Point(0, 0));
					((Control)val33).set_Size(new Point(32, 32));
					((Control)val33).set_Opacity(0.7f);
					((Control)val33).set_Parent((Container)(object)_StaticWindowsOrdered[j3]);
					staticWaypointIcon[num9] = val33;
					((Control)_staticWaypointIcon[j3]).add_MouseEntered((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						object obj4 = ((sender is Image) ? sender : null);
						((Control)obj4).set_Location(new Point(-2, -2));
						((Control)obj4).set_Size(new Point(36, 36));
						((Control)obj4).set_Opacity(1f);
					});
					((Control)_staticWaypointIcon[j3]).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_0009: Unknown result type (might be due to invalid IL or missing references)
						//IL_0018: Unknown result type (might be due to invalid IL or missing references)
						object obj3 = ((sender is Image) ? sender : null);
						((Control)obj3).set_Location(new Point(0, 0));
						((Control)obj3).set_Size(new Point(32, 32));
						((Control)obj3).set_Opacity(0.7f);
					});
					((Control)_staticWaypointIcon[j3]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						WaypointIcon_Click(index, "Static");
					});
					Image[] staticNotesIcon = _staticNotesIcon;
					int num10 = j3;
					Image val34 = new Image();
					val34.set_Texture(notesTexture);
					((Control)val34).set_Location(new Point(30, 0));
					((Control)val34).set_Size(new Point(32, 32));
					((Control)val34).set_Opacity(0.7f);
					((Control)val34).set_Parent((Container)(object)_StaticWindowsOrdered[j3]);
					staticNotesIcon[num10] = val34;
					((Control)_staticNotesIcon[j3]).add_MouseEntered((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						object obj2 = ((sender is Image) ? sender : null);
						((Control)obj2).set_Location(new Point(28, -2));
						((Control)obj2).set_Size(new Point(36, 36));
						((Control)obj2).set_Opacity(1f);
					});
					((Control)_staticNotesIcon[j3]).add_MouseLeft((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
					{
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						object obj = ((sender is Image) ? sender : null);
						((Control)obj).set_Location(new Point(30, 0));
						((Control)obj).set_Size(new Point(32, 32));
						((Control)obj).set_Opacity(0.7f);
					});
					((Control)_staticNotesIcon[j3]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						NotesIcon_Click(index, "Static");
					});
					Checkbox[] staticCheckboxes = _staticCheckboxes;
					int num11 = j3;
					Checkbox val35 = new Checkbox();
					((Control)val35).set_Location(new Point(70, 0));
					((Control)val35).set_Size(new Point(32, 32));
					((Control)val35).set_Parent((Container)(object)_StaticWindowsOrdered[j3]);
					staticCheckboxes[num11] = val35;
					_staticCheckboxes[j3].add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
					{
						_StaticEventsCheckbox_Click(index);
					});
					bool waypointNote2 = false;
					for (int k5 = 0; k5 < _staticEvents[j3].WaypointData.Count; k5++)
					{
						if (_staticEvents[j3].WaypointData[k5].Notes != "")
						{
							waypointNote2 = true;
						}
					}
					if (!waypointNote2)
					{
						((Control)_staticWaypointIcon[j3]).set_Visible(false);
					}
					bool notesNote2 = false;
					for (int k4 = 0; k4 < _staticEvents[j3].NotesData.Count; k4++)
					{
						if (_staticEvents[j3].NotesData[k4].Notes != "")
						{
							notesNote2 = true;
						}
					}
					if (!notesNote2)
					{
						((Control)_staticNotesIcon[j3]).set_Visible(false);
					}
					((Control)_staticLabelDescriptions[j3]).set_Size(new Point(200, 30));
					((Control)_staticLabelDescriptions[j3]).set_Location(new Point(100, 0));
					((Control)_staticLabelDescriptions[j3]).set_Parent((Container)(object)_StaticWindowsOrdered[j3]);
				}
				MainWindowModule mainWindowModule17 = this;
				TabbedWindow2 val36 = new TabbedWindow2(NoTexture, new Rectangle(0, 0, 1050, 650), new Rectangle(0, 0, 1050, 650));
				((Control)val36).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val36).set_Title("Settings");
				((Control)val36).set_Location(new Point(300, 300));
				((WindowBase2)val36).set_SavesPosition(true);
				((Control)val36).set_Visible(false);
				((WindowBase2)val36).set_Id("MainWindowModule_BaubleFarmTimerSettingsWindow_38d37290-b5f9-447d-97ea-45b0b50e5f56");
				mainWindowModule17._SettingsWindow = val36;
				AsyncTexture2D packageTexture = AsyncTexture2D.FromAssetId(156701);
				_SettingsWindow.get_Tabs().Add(new Tab(packageTexture, (Func<IView>)(() => (IView)(object)new PackageSettingsTabView()), "Packages", (int?)null));
				AsyncTexture2D clockTexture = AsyncTexture2D.FromAssetId(155156);
				_SettingsWindow.get_Tabs().Add(new Tab(clockTexture, (Func<IView>)(() => (IView)(object)new TimerSettingsTabView()), "Timer Events", (int?)null));
				AsyncTexture2D staticTexture = AsyncTexture2D.FromAssetId(156909);
				_SettingsWindow.get_Tabs().Add(new Tab(staticTexture, (Func<IView>)(() => (IView)(object)new StaticEventSettingsTabView()), "Static Events", (int?)null));
				AsyncTexture2D listTexture = AsyncTexture2D.FromAssetId(157109);
				_SettingsWindow.get_Tabs().Add(new Tab(listTexture, (Func<IView>)(() => (IView)(object)new ListSettingsTabView()), "General Settings", (int?)null));
			}
			catch (Exception ex3)
			{
				Logger.Error("Failed to load Time UI: " + ex3.Message);
			}
			List<TimerLogData> eventDataList = new List<TimerLogData>();
			string jsonFilePath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Event_Timers.json");
			if (File.Exists(jsonFilePath))
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(jsonFilePath))
					{
						eventDataList = JsonSerializer.Deserialize<List<TimerLogData>>(await streamReader.ReadToEndAsync(), _jsonOptions);
					}
					List<TimerLogData> eventData2 = eventDataList;
					for (int j = 0; j < TimerRowNum; j++)
					{
						DateTime? startTime = eventData2[j].StartTime;
						if (eventData2[j].IsActive = startTime.HasValue && eventData2[j].Description == _timerLabelDescriptions[j].get_Text())
						{
							if ((DateTime.Now - startTime.Value).TotalSeconds < 3600.0)
							{
								_timerStartTimes[j] = eventData2[j].StartTime;
								_timerRunning[j] = eventData2[j].IsActive;
								_timerTTSTriggered[j] = eventData2[j].TTSTriggered;
								((Control)_resetButtons[j]).set_Enabled(false);
								((Control)_customDropdownTimers[j]).set_Enabled(false);
							}
							else
							{
								_timerStartTimes[j] = null;
								_timerRunning[j] = false;
								_timerTTSTriggered[j] = false;
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.Info("Failed to load Event_Timers JSON file: " + ex2.Message);
				}
			}
			else
			{
				Logger.Info("No Event_Timers JSON file found.");
			}
			List<StaticLogData> staticEventDataList = new List<StaticLogData>();
			string staticJsonFilePath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Static_Events.json");
			if (File.Exists(staticJsonFilePath))
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(staticJsonFilePath))
					{
						staticEventDataList = JsonSerializer.Deserialize<List<StaticLogData>>(await streamReader.ReadToEndAsync(), _jsonOptions);
					}
					List<StaticLogData> eventData = staticEventDataList;
					for (int i = 0; i < StaticRowNum; i++)
					{
						if (eventData[i].IsActive && eventData[i].Description == _staticLabelDescriptions[i].get_Text())
						{
							_staticCheckboxes[i].set_Checked(true);
							_staticRunning[i] = true;
						}
						else
						{
							_staticCheckboxes[i].set_Checked(false);
							_staticRunning[i] = false;
						}
					}
					_hideStaticEvents();
				}
				catch (Exception ex)
				{
					Logger.Info("Failed to load Static_Events JSON file: " + ex.Message);
				}
			}
			else
			{
				Logger.Info("No Static_Events JSON file found.");
			}
		}

		private void _TimerWindow_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			double newHeight = ((Control)_TimerWindow).get_Size().Y - 100;
			((Control)_timerPanel).set_Size(new Point(((Control)_TimerWindow).get_Size().X, (int)newHeight));
			((Control)_timerBackgroundPanel).set_Size(new Point(((Control)_TimerWindow).get_Size().X, ((Control)_TimerWindow).get_Size().Y));
		}

		private void _StaticWindow_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			double newHeight = ((Control)_StaticWindow).get_Size().Y - 100;
			((Control)_staticPanel).set_Size(new Point(((Control)_StaticWindow).get_Size().X, (int)newHeight));
			((Control)_staticBackgroundPanel).set_Size(new Point(((Control)_StaticWindow).get_Size().X, ((Control)_StaticWindow).get_Size().Y));
		}

		private void _StaticEventsCheckbox_Click(int index)
		{
			if (_staticCheckboxes[index].get_Checked())
			{
				_staticRunning[index] = true;
			}
			else
			{
				_staticRunning[index] = false;
			}
			_hideStaticEvents();
			UpdateStaticJsonEvents();
		}

		private void _hideStaticEvents()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			int countVisible = 0;
			for (int i = 0; i < StaticRowNum; i++)
			{
				if (_hideStaticEventsCheckbox.get_Checked())
				{
					if (_staticCheckboxes[i].get_Checked())
					{
						((Control)_StaticWindowsOrdered[i]).set_Visible(false);
						continue;
					}
					((Control)_StaticWindowsOrdered[i]).set_Visible(true);
					((Control)_StaticWindowsOrdered[i]).set_Location(new Point(0, countVisible * 30));
					countVisible++;
				}
				else
				{
					((Control)_StaticWindowsOrdered[i]).set_Visible(true);
					((Control)_StaticWindowsOrdered[i]).set_Location(new Point(0, countVisible * 30));
					countVisible++;
				}
			}
		}

		private void _hideStaticEventsCheckbox_Click()
		{
			_hideStaticEvents();
		}

		private void _resetStaticEventsButton_Click()
		{
			for (int staticIndex = 0; staticIndex < StaticRowNum; staticIndex++)
			{
				_staticRunning[staticIndex] = false;
				_staticCheckboxes[staticIndex].set_Checked(false);
			}
			UpdateStaticJsonEvents();
		}

		private void UpdateStaticJsonEvents()
		{
			List<StaticLogData> eventDataList = new List<StaticLogData>();
			string jsonFilePath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Static_Events.json");
			for (int i = 0; i < StaticRowNum; i++)
			{
				eventDataList.Add(new StaticLogData
				{
					ID = i,
					Description = (_staticLabelDescriptions[i].get_Text() ?? ""),
					IsActive = _staticRunning[i]
				});
			}
			try
			{
				string jsonContent = JsonSerializer.Serialize<List<StaticLogData>>(eventDataList, _jsonOptions);
				File.WriteAllText(jsonFilePath, jsonContent);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to save JSON file: " + ex.Message);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			elapsedDateTime = DateTime.Now;
			if (elapsedDateTime - initialDateTime >= TimeSpan.FromMinutes(1.0))
			{
				var (NextBaubleStartDate, EndofBaubleWeek, FarmStatus, Statuscolor) = GetBaubleInformation();
				_statusValue.set_Text(FarmStatus);
				_statusValue.set_TextColor(Statuscolor);
				_startTimeValue.set_Text(NextBaubleStartDate.ToString("hh:mm tt (MMMM dd, yyyy)"));
				_endTimeValue.set_Text(EndofBaubleWeek.ToString("hh:mm tt (MMMM dd, yyyy)"));
				initialDateTime = DateTime.Now;
			}
			try
			{
				TimeSpan[] CurrentElapsedTime = new TimeSpan[TimerRowNum];
				for (int i = 0; i < TimerRowNum; i++)
				{
					string DropdownValue = _customDropdownTimers[i].get_SelectedItem();
					TimeSpan remaining = TimeSpan.FromMinutes(0.0);
					if (_timerRunning[i] && _timerStartTimes[i].HasValue)
					{
						TimeSpan elapsed = DateTime.Now - _timerStartTimes[i].Value;
						remaining = ((!(DropdownValue == "Default")) ? (_timerDurationOverride[i] - elapsed) : (_timerDurationDefaults[i] - elapsed));
						_timerLabels[i].set_Text($"{remaining:mm\\:ss}");
						if (remaining.TotalSeconds <= -3600.0)
						{
							if (DropdownValue == "Default")
							{
								_timerLabels[i].set_Text($"{_timerDurationDefaults[i]:mm\\:ss}");
							}
							else
							{
								_timerLabels[i].set_Text($"{_timerDurationOverride[i]:mm\\:ss}");
							}
							_timerRunning[i] = false;
							TimerColors selectedEnum4 = _TimerColorDefault.get_Value();
							Color actualColor4 = _colorMap[selectedEnum4];
							_timerLabels[i].set_TextColor(actualColor4);
							((Control)_resetButtons[i]).set_Enabled(true);
						}
						else if (remaining.TotalSeconds <= 0.0)
						{
							_timerLabels[i].set_Text("-" + _timerLabels[i].get_Text());
						}
						if (remaining.TotalSeconds <= 0.0 && _timerTTSTriggered[i])
						{
							TTSAlert(i);
							_timerTTSTriggered[i] = false;
							UpdateTimerJsonEvents();
						}
					}
					if (!_timerRunning[i])
					{
						if (DropdownValue == "Default")
						{
							CurrentElapsedTime[i] = _timerDurationDefaults[i];
						}
						else
						{
							CurrentElapsedTime[i] = _timerDurationOverride[i];
						}
						continue;
					}
					CurrentElapsedTime[i] = remaining;
					if (remaining.TotalSeconds < (double)_timerLowDefault.get_Value())
					{
						TimerColors selectedEnum3 = _LowTimerColorDefault.get_Value();
						Color actualColor3 = _colorMap[selectedEnum3];
						_timerLabels[i].set_TextColor(actualColor3);
					}
					else if (remaining.TotalSeconds < (double)(_timerLowDefault.get_Value() + _timerIntermediateLowDefault.get_Value()))
					{
						TimerColors selectedEnum2 = _IntermediateLowTimerColorDefault.get_Value();
						Color actualColor2 = _colorMap[selectedEnum2];
						_timerLabels[i].set_TextColor(actualColor2);
					}
					else
					{
						TimerColors selectedEnum = _TimerColorDefault.get_Value();
						Color actualColor = _colorMap[selectedEnum];
						_timerLabels[i].set_TextColor(actualColor);
					}
				}
				if (_InOrdercheckbox.get_Checked())
				{
					OrderPanelsByTime(CurrentElapsedTime);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed GameUpdate timer calculation error: " + ex.Message);
			}
		}

		private void TTSAlert(int index)
		{
			try
			{
				string TTSText = _timerEvents[index].TTSText;
				if (_timerEvents[index].TTSActive && TTSText != "" && TTSText != null)
				{
					SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
					speechSynthesizer.Rate = _timerTTSSpeedDefault.get_Value();
					speechSynthesizer.Volume = _timerTTSVolumeDefault.get_Value();
					speechSynthesizer.SpeakAsync(TTSText);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("TTS Failed due to: " + ex.Message);
			}
		}

		private void OrderPanelsByTime(TimeSpan[] CurrentElapsedTime)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			List<(TimeSpan, int)> sortedWithIndices = (from item in CurrentElapsedTime.Select((TimeSpan value, int index) => (value, index))
				orderby item.Value
				select item).ToList();
			for (int i = 0; i < TimerRowNum; i++)
			{
				((Control)_TimerWindowsOrdered[sortedWithIndices[i].Item2]).set_Location(new Point(0, i * 30));
			}
		}

		private void InOrdercheckbox_Click()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (!_InOrdercheckbox.get_Checked())
			{
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_TimerWindowsOrdered[i]).set_Location(new Point(0, i * 30));
				}
			}
		}

		protected override void Unload()
		{
			ModuleInstance = null;
			for (int j = 0; j < TimerRowNum; j++)
			{
				Label obj = _timerLabelDescriptions[j];
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				StandardButton obj2 = _resetButtons[j];
				if (obj2 != null)
				{
					((Control)obj2).Dispose();
				}
				StandardButton obj3 = _stopButtons[j];
				if (obj3 != null)
				{
					((Control)obj3).Dispose();
				}
				Image obj4 = _timerWaypointIcon[j];
				if (obj4 != null)
				{
					((Control)obj4).Dispose();
				}
				Image obj5 = _timerNotesIcon[j];
				if (obj5 != null)
				{
					((Control)obj5).Dispose();
				}
				Label obj6 = _timerLabels[j];
				if (obj6 != null)
				{
					((Control)obj6).Dispose();
				}
			}
			StandardButton stopButton = _stopButton;
			if (stopButton != null)
			{
				((Control)stopButton).Dispose();
			}
			Checkbox inOrdercheckbox = _InOrdercheckbox;
			if (inOrdercheckbox != null)
			{
				((Control)inOrdercheckbox).Dispose();
			}
			if (_toggleStaticWindowKeybind != null)
			{
				_toggleStaticWindowKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleStaticWindowKeybind_Activated);
			}
			StandardWindow infoWindow = _InfoWindow;
			if (infoWindow != null)
			{
				((Control)infoWindow).Dispose();
			}
			_InfoWindow = null;
			for (int i = 0; i < StaticRowNum; i++)
			{
				Label obj7 = _staticLabelDescriptions[i];
				if (obj7 != null)
				{
					((Control)obj7).Dispose();
				}
				Image obj8 = _staticWaypointIcon[i];
				if (obj8 != null)
				{
					((Control)obj8).Dispose();
				}
				Image obj9 = _staticNotesIcon[i];
				if (obj9 != null)
				{
					((Control)obj9).Dispose();
				}
			}
			StandardWindow staticWindow = _StaticWindow;
			if (staticWindow != null)
			{
				((Control)staticWindow).Dispose();
			}
			_StaticWindow = null;
			((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			if (_toggleTimerWindowKeybind != null)
			{
				_toggleTimerWindowKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleTimerWindowKeybind_Activated);
			}
			StandardWindow timerWindow = _TimerWindow;
			if (timerWindow != null)
			{
				((Control)timerWindow).Dispose();
			}
		}

		public void Restart()
		{
			((Module)this).Unload();
			ModuleInstance = this;
			((Module)this).LoadAsync();
		}
	}
}
