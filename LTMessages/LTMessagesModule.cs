using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LTMessages
{
	[Export(typeof(Module))]
	public class LTMessagesModule : Module
	{
		private enum ChatFocus
		{
			ShiftEnter,
			Enter
		}

		private enum ChatAction
		{
			PasteOnly,
			Send
		}

		private enum ChatCommand
		{
			Default,
			Squad,
			Subgroup,
			Party1,
			Party2,
			Party3,
			Party4,
			Party5,
			Guild,
			Guild1,
			Guild2,
			Guild3,
			Guild4,
			Guild5,
			Guild6,
			Say,
			Map,
			Party,
			Team
		}

		private enum SendDelay
		{
			_5ms = 5,
			_8ms = 8,
			_10ms = 10,
			_12ms = 12,
			_15ms = 0xF,
			_20ms = 20,
			_25ms = 25,
			_30ms = 30,
			_35ms = 35,
			_40ms = 40,
			_50ms = 50,
			_60ms = 60,
			_75ms = 75,
			_100ms = 100,
			_125ms = 125,
			_150ms = 150
		}

		private struct INPUT
		{
			public uint type;

			public INPUTUNION U;
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

		private static class NativeMethods
		{
			public const int ASFW_ANY = -1;

			[DllImport("user32.dll")]
			public static extern bool AllowSetForegroundWindow(int dwProcessId);
		}

		private static readonly Logger Logger = Logger.GetLogger<LTMessagesModule>();

		private SettingEntry<ChatFocus> _chatFocus;

		private SettingEntry<ChatAction> _chatAction;

		private SettingEntry<ChatCommand> _chatCommand;

		private SettingEntry<SendDelay> _sendDelay;

		private SettingEntry<bool> _showCornerIcon;

		private SettingEntry<KeyBinding> _popupKeybind;

		private SettingEntry<KeyBinding> _toggleLTModeKeybind;

		private SettingEntry<KeyBinding> _openEditorKeybind;

		private SettingEntry<bool> _ltModeEnabled;

		private int _activeMessageList;

		private Dictionary<int, string> _listNames = new Dictionary<int, string>();

		private CornerIcon _cornerIcon;

		private Panel _popupWindow;

		private StandardButton _popupCloseButton;

		private FlowPanel _messageFlowPanel;

		private Panel _editorWindow;

		private Panel _editorTabContent;

		private Panel _aboutTabContent;

		private FlowPanel _editorFlowPanel;

		private Dropdown _listSelectorDropdown;

		private Panel _editDialogWindow;

		private TextBox _editTitleTextBox;

		private TextBox _editMessageTextBox;

		private MessageEntry _editingMessage;

		private int _editingMessageIndex = -1;

		private bool _suppressListSwitchWarning;

		private List<MessageEntry> _messages = new List<MessageEntry>();

		private FileSystemWatcher _fileWatcher;

		private int _currentListIndex;

		private const int MessageListCount = 6;

		private static readonly string DefaultFilePathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2\\addons\\blishhud\\ltmessages\\");

		private const int DefaultMaxMessageLength = 199;

		private const uint INPUT_KEYBOARD = 1u;

		private const uint KEYEVENTF_KEYDOWN = 0u;

		private const uint KEYEVENTF_KEYUP = 2u;

		private const ushort VK_RETURN = 13;

		private const ushort VK_SHIFT = 16;

		private const ushort VK_CONTROL = 17;

		private const ushort VK_V = 86;

		private const ushort VK_SLASH = 191;

		private TextBox _renameTextBox;

		private Panel _renameDialogWindow;

		private Panel _helpDialogWindow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		[DllImport("user32.dll")]
		private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

		[DllImport("user32.dll")]
		private static extern ushort MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		[DllImport("user32.dll")]
		private static extern short VkKeyScan(char ch);

		private static void SendKeyPress(ushort keyCode, bool shift = false, bool ctrl = false)
		{
			List<INPUT> inputs = new List<INPUT>();
			if (shift)
			{
				inputs.Add(CreateKeyInput(16, keyDown: true));
			}
			if (ctrl)
			{
				inputs.Add(CreateKeyInput(17, keyDown: true));
			}
			inputs.Add(CreateKeyInput(keyCode, keyDown: true));
			inputs.Add(CreateKeyInput(keyCode, keyDown: false));
			if (ctrl)
			{
				inputs.Add(CreateKeyInput(17, keyDown: false));
			}
			if (shift)
			{
				inputs.Add(CreateKeyInput(16, keyDown: false));
			}
			uint result = SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
			if (result == 0)
			{
				Logger.Warn($"SendInput failed for keyCode {keyCode}, shift={shift}, ctrl={ctrl}");
			}
			else
			{
				Logger.Debug($"SendInput sent {result} inputs successfully");
			}
		}

		private static async Task TypeString(string text, int delayMs = 20)
		{
			foreach (char c in text)
			{
				short vkAndShift = VkKeyScan(c);
				if (vkAndShift == -1)
				{
					Logger.Warn($"Could not get virtual key for character: {c}");
					continue;
				}
				byte vk = (byte)((uint)vkAndShift & 0xFFu);
				bool needShift = ((byte)((vkAndShift >> 8) & 0xFF) & 1) != 0;
				SendKeyPress(vk, needShift);
				await Task.Delay(delayMs);
			}
		}

		private static INPUT CreateKeyInput(ushort keyCode, bool keyDown)
		{
			ushort scanCode = MapVirtualKey(keyCode, 0u);
			INPUT result = default(INPUT);
			result.type = 1u;
			result.U = new INPUTUNION
			{
				ki = new KEYBDINPUT
				{
					wVk = keyCode,
					wScan = scanCode,
					dwFlags = ((!keyDown) ? 2u : 0u),
					time = 0u,
					dwExtraInfo = IntPtr.Zero
				}
			};
			return result;
		}

		private static IntPtr FindGW2Window()
		{
			IntPtr hwnd = GetForegroundWindow();
			StringBuilder title = new StringBuilder(256);
			GetWindowText(hwnd, title, title.Capacity);
			title.ToString().Contains("Guild Wars 2");
			return hwnd;
		}

		private static void FocusGameWindow()
		{
			try
			{
				IntPtr gameWindow = FindGW2Window();
				if (gameWindow != IntPtr.Zero)
				{
					SetForegroundWindow(gameWindow);
					Logger.Debug("Focused GW2 window");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to focus game window");
			}
		}

		[ImportingConstructor]
		public LTMessagesModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			_popupKeybind = settings.DefineSetting<KeyBinding>("PopupKeybind", new KeyBinding(), (Func<string>)(() => "Popup Keybind"), (Func<string>)(() => "Press this key to show the message popup at your cursor (optional - no default binding)"));
			_toggleLTModeKeybind = settings.DefineSetting<KeyBinding>("ToggleLTModeKeybind", new KeyBinding(), (Func<string>)(() => "Toggle LT Mode Keybind"), (Func<string>)(() => "Press this key to toggle LT Mode on/off (optional - no default binding)"));
			_openEditorKeybind = settings.DefineSetting<KeyBinding>("OpenEditorKeybind", new KeyBinding(), (Func<string>)(() => "Open Editor Keybind"), (Func<string>)(() => "Press this key to open the message editor window (optional - no default binding)"));
			_showCornerIcon = settings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show Corner Icon"), (Func<string>)(() => "Display an icon in the Blish HUD menu for alternative access"));
			_ltModeEnabled = settings.DefineSetting<bool>("LTModeEnabled", true, (Func<string>)(() => "LT Mode Enabled"), (Func<string>)(() => "Enable this when you are a Lieutenant or Commander. Messages won't send when disabled."));
			_chatFocus = settings.DefineSetting<ChatFocus>("ChatFocus", ChatFocus.ShiftEnter, (Func<string>)(() => "Chat Focus"), (Func<string>)(() => "Shift+Enter = Open squad chat directly | Enter = Open last used chat"));
			_chatAction = settings.DefineSetting<ChatAction>("ChatAction", ChatAction.Send, (Func<string>)(() => "Chat Action"), (Func<string>)(() => "Send = Type and send message automatically | Paste Only = Copy to clipboard"));
			_chatCommand = settings.DefineSetting<ChatCommand>("ChatCommand", ChatCommand.Default, (Func<string>)(() => "Chat Command"), (Func<string>)(() => "Which chat channel to send to. Default = Use whatever channel is already active."));
			_sendDelay = settings.DefineSetting<SendDelay>("SendDelay", SendDelay._40ms, (Func<string>)(() => "Typing Delay (ms)"), (Func<string>)(() => "Delay between keystrokes when typing messages (adjust if messages don't send reliably)"));
			if (!Enum.IsDefined(typeof(SendDelay), _sendDelay.get_Value()))
			{
				Logger.Info("Migrating invalid send delay value to default (40ms)");
				_sendDelay.set_Value(SendDelay._40ms);
			}
			SettingEntry<bool> openEditorButton = settings.DefineSetting<bool>("OpenEditorButton", false, (Func<string>)(() => "Open Message Editor"), (Func<string>)(() => "Toggle this on to open the in-game message editor"));
			openEditorButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue() && !e.get_PreviousValue())
				{
					ShowEditorWindow();
					Task.Run(async delegate
					{
						await Task.Delay(100);
						openEditorButton.set_Value(false);
					});
				}
			});
			SettingEntry<bool> openHelpButton = settings.DefineSetting<bool>("OpenHelpButton", false, (Func<string>)(() => "Show Help"), (Func<string>)(() => "Toggle this on to see help and configuration guide"));
			openHelpButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue() && !e.get_PreviousValue())
				{
					ShowHelpDialog();
					Task.Run(async delegate
					{
						await Task.Delay(100);
						openHelpButton.set_Value(false);
					});
				}
			});
			SettingEntry<bool> openDocsButton = settings.DefineSetting<bool>("OpenDocsButton", false, (Func<string>)(() => "Documentation"), (Func<string>)(() => "Open the full LT Messages documentation at senzall.com/ltmessages"));
			openDocsButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue() && !e.get_PreviousValue())
				{
					OpenUrl("https://senzall.com/ltmessages");
					Task.Run(async delegate
					{
						await Task.Delay(100);
						openDocsButton.set_Value(false);
					});
				}
			});
			SettingEntry<bool> openKoFiButton = settings.DefineSetting<bool>("OpenKoFiButton", false, (Func<string>)(() => "Support on Ko-fi ☕"), (Func<string>)(() => "If LT Messages has been useful, a coffee is always appreciated!"));
			openKoFiButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue() && !e.get_PreviousValue())
				{
					OpenUrl("https://ko-fi.com/senzall");
					Task.Run(async delegate
					{
						await Task.Delay(100);
						openKoFiButton.set_Value(false);
					});
				}
			});
		}

		protected override async Task LoadAsync()
		{
			Logger.Info("Loading LT Messages module...");
			LoadInternalData();
			_currentListIndex = _activeMessageList;
			LoadMessagesFromFile();
			Logger.Info($"Loaded {GetListDisplayName(_currentListIndex)} with {_messages.Count} messages");
			SetupFileWatcher();
			if (_showCornerIcon.get_Value())
			{
				CreateCornerIcon();
			}
			_showCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCornerIconChanged);
			CreatePopupWindow();
			_popupKeybind.get_Value().set_Enabled(true);
			_popupKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnPopupKeybindActivated);
			_toggleLTModeKeybind.get_Value().set_Enabled(true);
			_toggleLTModeKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleLTModeKeybindActivated);
			_openEditorKeybind.get_Value().set_Enabled(true);
			_openEditorKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnOpenEditorKeybindActivated);
			Logger.Info("LT Messages module loaded successfully.");
			await Task.CompletedTask;
		}

		protected override void Update(GameTime gameTime)
		{
			if (_popupWindow != null && ((Control)_popupWindow).get_Visible() && GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)27))
			{
				HidePopup();
			}
		}

		protected override void Unload()
		{
			Logger.Info("Unloading LT Messages module...");
			SaveInternalData();
			_showCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCornerIconChanged);
			if (_popupKeybind?.get_Value() != null)
			{
				_popupKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnPopupKeybindActivated);
				_popupKeybind.get_Value().set_Enabled(false);
			}
			if (_toggleLTModeKeybind?.get_Value() != null)
			{
				_toggleLTModeKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleLTModeKeybindActivated);
				_toggleLTModeKeybind.get_Value().set_Enabled(false);
			}
			if (_openEditorKeybind?.get_Value() != null)
			{
				_openEditorKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnOpenEditorKeybindActivated);
				_openEditorKeybind.get_Value().set_Enabled(false);
			}
			_fileWatcher?.Dispose();
			if (GameService.Graphics.get_SpriteScreen() != null)
			{
				((Control)GameService.Graphics.get_SpriteScreen()).remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnScreenClicked);
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Panel popupWindow = _popupWindow;
			if (popupWindow != null)
			{
				((Control)popupWindow).Dispose();
			}
			Panel editorWindow = _editorWindow;
			if (editorWindow != null)
			{
				((Control)editorWindow).Dispose();
			}
			Panel editDialogWindow = _editDialogWindow;
			if (editDialogWindow != null)
			{
				((Control)editDialogWindow).Dispose();
			}
			Panel renameDialogWindow = _renameDialogWindow;
			if (renameDialogWindow != null)
			{
				((Control)renameDialogWindow).Dispose();
			}
			Panel helpDialogWindow = _helpDialogWindow;
			if (helpDialogWindow != null)
			{
				((Control)helpDialogWindow).Dispose();
			}
			_messages.Clear();
			Logger.Info("LT Messages module unloaded.");
		}

		private List<MessageEntry> GetDefaultMessages()
		{
			return new List<MessageEntry>
			{
				new MessageEntry("Stack", "Stack on Tag"),
				new MessageEntry("Wait", "Wait for the squad!"),
				new MessageEntry("Buffs", "Buffs dropped near tag"),
				new MessageEntry("Stealth", "Stack for stealth share"),
				new MessageEntry("Blast", "Blast the field for might/stealth"),
				new MessageEntry("Moving", "Tag is moving"),
				new MessageEntry("Stop", "Stop! Hold position"),
				new MessageEntry("Port", "Port is on the marker"),
				new MessageEntry("Portal", "Portal is up at marker"),
				new MessageEntry("Unlock-WP", "Unlock the Waypoint!"),
				new MessageEntry("Take-WP", "Take the Waypoint in chat"),
				new MessageEntry("Link-WP", "Link the Waypoint in chat"),
				new MessageEntry("Focus", "Focus the target"),
				new MessageEntry("Kill-Adds", "Kill the adds"),
				new MessageEntry("Spread", "Spread out!"),
				new MessageEntry("Dodge", "Dodge the AoE attacks!"),
				new MessageEntry("Rez", "Rez downed players!"),
				new MessageEntry("Mount-CC", "Springer or Warclaw up for CC"),
				new MessageEntry("Need-CC", "We need CC!"),
				new MessageEntry("Safe", "Area is clear - all safe"),
				new MessageEntry("HP-Combat", "Please let Tag start the combat HP!"),
				new MessageEntry("HP-Commune", "Commune with HP and then stack on tag!"),
				new MessageEntry("F-Vista", "F the Vista and then stack on tag!"),
				new MessageEntry("POI-Tag", "Point of Interest on Tag!"),
				new MessageEntry("POI-Marker", "Point of Interest on Marker"),
				new MessageEntry("Guard", "We need 1-2 people to guard this spot"),
				new MessageEntry("Loot", "F for loot! Some chests need manual looting"),
				new MessageEntry("Help", "If you get lost ask for help!"),
				new MessageEntry("No-Drop", "Please don't drop items. Let Commander set up stations."),
				new MessageEntry("Break", "We are taking a short break. BRB")
			};
		}

		private List<MessageEntry> GetSampleMessage()
		{
			return new List<MessageEntry>
			{
				new MessageEntry("Stack", "Stack on Tag")
			};
		}

		private string GetFilePathForList(int listIndex)
		{
			if (listIndex == 0)
			{
				return Path.Combine(DefaultFilePathBase, "messages.txt");
			}
			return Path.Combine(DefaultFilePathBase, $"messages_{listIndex}.txt");
		}

		private void LoadInternalData()
		{
			_listNames.Clear();
			string dataFile = Path.Combine(DefaultFilePathBase, "module_data.txt");
			try
			{
				if (File.Exists(dataFile))
				{
					string[] array = File.ReadAllLines(dataFile);
					foreach (string line in array)
					{
						if (line.StartsWith("ActiveList="))
						{
							if (int.TryParse(line.Substring(11), out var listIndex))
							{
								_activeMessageList = listIndex;
							}
						}
						else if (line.StartsWith("ListName:"))
						{
							string[] parts = line.Substring(9).Split(new char[1] { ':' }, 2);
							if (parts.Length == 2 && int.TryParse(parts[0], out var index))
							{
								_listNames[index] = parts[1];
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load internal data");
			}
			for (int i = 0; i < 6; i++)
			{
				if (!_listNames.ContainsKey(i))
				{
					_listNames[i] = ((i == 0) ? "Default" : $"List {i}");
				}
			}
			if (_activeMessageList < 0 || _activeMessageList >= 6)
			{
				_activeMessageList = 0;
			}
		}

		private void SaveInternalData()
		{
			string dataFile = Path.Combine(DefaultFilePathBase, "module_data.txt");
			try
			{
				string directory = Path.GetDirectoryName(dataFile);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				List<string> lines = new List<string>();
				lines.Add($"ActiveList={_activeMessageList}");
				foreach (KeyValuePair<int, string> kvp in _listNames)
				{
					lines.Add($"ListName:{kvp.Key}:{kvp.Value}");
				}
				File.WriteAllLines(dataFile, lines);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save internal data");
			}
		}

		private string GetListDisplayName(int listIndex)
		{
			if (_listNames.ContainsKey(listIndex))
			{
				return _listNames[listIndex];
			}
			if (listIndex != 0)
			{
				return $"List {listIndex}";
			}
			return "Default";
		}

		private string GetChatCommandString(ChatCommand command)
		{
			return command switch
			{
				ChatCommand.Default => "", 
				ChatCommand.Squad => "/squad", 
				ChatCommand.Subgroup => "/subgroup", 
				ChatCommand.Party1 => "/1", 
				ChatCommand.Party2 => "/2", 
				ChatCommand.Party3 => "/3", 
				ChatCommand.Party4 => "/4", 
				ChatCommand.Party5 => "/5", 
				ChatCommand.Guild => "/guild", 
				ChatCommand.Guild1 => "/g1", 
				ChatCommand.Guild2 => "/g2", 
				ChatCommand.Guild3 => "/g3", 
				ChatCommand.Guild4 => "/g4", 
				ChatCommand.Guild5 => "/g5", 
				ChatCommand.Guild6 => "/g6", 
				ChatCommand.Say => "/say", 
				ChatCommand.Map => "/map", 
				ChatCommand.Party => "/party", 
				ChatCommand.Team => "/team", 
				_ => "", 
			};
		}

		private void LoadMessagesFromFile()
		{
			string filePath = GetFilePathForList(_currentListIndex);
			try
			{
				string directory = Path.GetDirectoryName(filePath);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
					Logger.Info("Created directory: " + directory);
				}
				if (!File.Exists(filePath))
				{
					CreateDefaultMessageFile(filePath, _currentListIndex);
					_messages = ((_currentListIndex == 0) ? GetDefaultMessages() : GetSampleMessage());
					RefreshMessageUI();
					return;
				}
				string[] array = File.ReadAllLines(filePath);
				List<MessageEntry> newMessages = new List<MessageEntry>();
				string[] array2 = array;
				foreach (string line in array2)
				{
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
					{
						continue;
					}
					string[] parts = line.Split(new char[1] { ',' }, 2);
					if (parts.Length != 2)
					{
						Logger.Warn("Malformed line in message file (expected 'Title,Message'): " + line);
						continue;
					}
					string title = parts[0].Trim();
					string message = parts[1].Trim();
					if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
					{
						Logger.Warn("Skipping line with empty title or message: " + line);
						continue;
					}
					if (message.Length > 199)
					{
						message = message.Substring(0, 199);
						Logger.Warn($"Message truncated to {199} characters: {message}");
					}
					MessageEntry entry = new MessageEntry(title, message);
					if (title.Length > 16)
					{
						Logger.Warn("Title '" + title + "' truncated to 16 characters: '" + entry.Title + "'");
					}
					newMessages.Add(entry);
				}
				_messages = newMessages;
				Logger.Info($"Loaded {_messages.Count} messages from {filePath}");
				RefreshMessageUI();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error loading messages from " + filePath);
				_messages = GetDefaultMessages();
				Logger.Info("Loaded embedded default messages as fallback");
				ScreenNotification.ShowNotification("LT Messages: Failed to load message file. Using default messages. Check logs for details.", (NotificationType)1, (Texture2D)null, 4);
				RefreshMessageUI();
			}
		}

		private void CreateDefaultMessageFile(string filePath, int listIndex)
		{
			try
			{
				List<string> defaultMessages = ((listIndex != 0) ? new List<string>
				{
					"# ========================================",
					$"# LT Messages Configuration File - List {listIndex}",
					"# ========================================",
					"# ",
					"# TO EDIT: Open this file in Notepad, VSCode, or any text editor",
					"# FILE LOCATION: " + filePath,
					"# ",
					"# Use this list for specific event types (WvW, Metas, HP Trains, etc.)",
					"# Add your custom messages below.",
					"# ",
					"# FORMAT: Title,Message",
					"#   Title: max 16 characters (shown in popup menu)",
					"#   Message: max 199 characters (GW2 chat limit)",
					"# ",
					"# Lines starting with # are comments and ignored",
					"# ========================================",
					"",
					"Stack,Stack on Tag"
				} : new List<string>
				{
					"# ========================================",
					"# LT Messages Configuration File - List 0 (Default)",
					"# ========================================",
					"# ",
					"# TO EDIT: Open this file in Notepad, VSCode, or any text editor",
					"# FILE LOCATION: " + filePath,
					"# ",
					"# After editing, save the file - changes reload automatically!",
					"# ",
					"# FORMAT: Title,Message",
					"#   Title: max 16 characters (shown in popup menu)",
					"#   Message: max 199 characters (GW2 chat limit)",
					"# ",
					"# Lines starting with # are comments and ignored",
					"# ========================================",
					"",
					"# Pre-Movement & Positioning",
					"Stack,Stack on Tag",
					"Wait,Wait for the squad!",
					"Buffs,Buffs dropped near tag",
					"Stealth,Stack for stealth share",
					"Blast,Blast the field for might/stealth",
					"",
					"# Movement Commands",
					"Moving,Tag is moving",
					"Stop,Stop! Hold position",
					"Port,Port is on the marker",
					"Portal,Portal is up at marker",
					"Unlock-WP,Unlock the Waypoint!",
					"Take-WP,Take the Waypoint in chat",
					"Link-WP,Link the Waypoint in chat",
					"",
					"# Combat - Priority Actions",
					"Focus,Focus the target",
					"Kill-Adds,Kill the adds",
					"Spread,Spread out!",
					"Dodge,Dodge the AoE attacks!",
					"Rez,Rez downed players!",
					"Mount-CC,Springer or Warclaw up for CC",
					"Need-CC,We need CC!",
					"Safe,Area is clear - all safe",
					"",
					"# Objectives",
					"HP-Combat,Please let Tag start the combat HP!",
					"HP-Commune,Commune with HP and then stack on tag!",
					"F-Vista,F the Vista and then stack on tag!",
					"POI-Tag,Point of Interest on Tag!",
					"POI-Marker,Point of Interest on Marker",
					"",
					"# Squad Management",
					"Guard,We need 1-2 people to guard this spot",
					"Loot,F for loot! Some chests need manual looting",
					"Help,If you get lost ask for help!",
					"No-Drop,Please don't drop items. Let Commander set up stations.",
					"Break,We are taking a short break. BRB"
				});
				File.WriteAllLines(filePath, defaultMessages);
				Logger.Info($"Created default message file at {filePath} (List {listIndex})");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to create default message file at " + filePath);
			}
		}

		private void SetupFileWatcher()
		{
			try
			{
				string filePath = GetFilePathForList(_activeMessageList);
				string directory = Path.GetDirectoryName(filePath);
				string fileName = Path.GetFileName(filePath);
				if (!string.IsNullOrEmpty(directory) && !string.IsNullOrEmpty(fileName))
				{
					_fileWatcher?.Dispose();
					_fileWatcher = new FileSystemWatcher
					{
						Path = directory,
						Filter = fileName,
						NotifyFilter = (NotifyFilters.Size | NotifyFilters.LastWrite)
					};
					_fileWatcher.Changed += OnFileChanged;
					_fileWatcher.EnableRaisingEvents = true;
					Logger.Info($"File watcher setup for {filePath} (List {_activeMessageList})");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to setup file watcher");
			}
		}

		private void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			Thread.Sleep(100);
			Logger.Info($"Message file changed for List {_activeMessageList}, reloading...");
			if (_activeMessageList == _currentListIndex)
			{
				LoadMessagesFromFile();
			}
		}

		private void CreateCornerIcon()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			try
			{
				Texture2D iconTexture = ContentsManager.GetTexture("icon.png") ?? AsyncTexture2D.op_Implicit(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156027));
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(iconTexture));
				((Control)val).set_BasicTooltipText("LT Messages - Click to show messages");
				val.set_Priority(1645843599);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
				((Control)_cornerIcon).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnCornerIconRightClicked);
				UpdateCornerIconTooltip();
				_ltModeEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
				{
					UpdateCornerIconTooltip();
				});
				_chatFocus.add_SettingChanged((EventHandler<ValueChangedEventArgs<ChatFocus>>)delegate
				{
					UpdateCornerIconTooltip();
				});
				_chatAction.add_SettingChanged((EventHandler<ValueChangedEventArgs<ChatAction>>)delegate
				{
					UpdateCornerIconTooltip();
				});
				_chatCommand.add_SettingChanged((EventHandler<ValueChangedEventArgs<ChatCommand>>)delegate
				{
					UpdateCornerIconTooltip();
				});
				Logger.Info("Corner icon created");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to create corner icon");
			}
		}

		private void UpdateCornerIconTooltip()
		{
			if (_cornerIcon != null)
			{
				string status = (_ltModeEnabled.get_Value() ? "ENABLED" : "DISABLED");
				string focus = ((_chatFocus.get_Value() == ChatFocus.ShiftEnter) ? "Shift+Enter" : "Enter");
				string action = ((_chatAction.get_Value() == ChatAction.Send) ? "Send" : "Paste");
				string command = GetChatCommandDisplayName(_chatCommand.get_Value());
				((Control)_cornerIcon).set_BasicTooltipText("LT Messages\nLeft-click: Show messages\nRight-click: Open editor\nLT Mode: " + status + "\nMode: " + focus + " | " + action + " | " + command);
			}
		}

		private string GetChatCommandDisplayName(ChatCommand command)
		{
			return command switch
			{
				ChatCommand.Default => "Default", 
				ChatCommand.Squad => "Squad", 
				ChatCommand.Subgroup => "Subgroup", 
				ChatCommand.Map => "Map", 
				ChatCommand.Say => "Say", 
				ChatCommand.Party => "Party", 
				ChatCommand.Team => "Team", 
				ChatCommand.Guild => "Guild", 
				ChatCommand.Guild1 => "Guild1", 
				ChatCommand.Guild2 => "Guild2", 
				ChatCommand.Guild3 => "Guild3", 
				ChatCommand.Guild4 => "Guild4", 
				ChatCommand.Guild5 => "Guild5", 
				ChatCommand.Guild6 => "Guild6", 
				ChatCommand.Party1 => "Party1", 
				ChatCommand.Party2 => "Party2", 
				ChatCommand.Party3 => "Party3", 
				ChatCommand.Party4 => "Party4", 
				ChatCommand.Party5 => "Party5", 
				_ => command.ToString(), 
			};
		}

		private void CreatePopupWindow()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Size(new Point(220, 300));
			((Control)val).set_Location(new Point(100, 100));
			((Control)val).set_Visible(false);
			((Control)val).set_ZIndex(9999);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 220));
			val.set_ShowBorder(true);
			_popupWindow = val;
			Label val2 = new Label();
			val2.set_Text("Messages");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 8));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			val2.set_ShowShadow(true);
			((Control)val2).set_Parent((Container)(object)_popupWindow);
			StandardButton val3 = new StandardButton();
			val3.set_Text("X");
			((Control)val3).set_Width(25);
			((Control)val3).set_Height(25);
			((Control)val3).set_Location(new Point(((Control)_popupWindow).get_Width() - 35, 5));
			((Control)val3).set_Parent((Container)(object)_popupWindow);
			((Control)val3).set_ZIndex(10);
			_popupCloseButton = val3;
			((Control)_popupCloseButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				HidePopup();
			});
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			((Panel)val4).set_CanScroll(true);
			((Control)val4).set_Location(new Point(6, 40));
			((Control)val4).set_Size(new Point(204, 260));
			((Control)val4).set_Parent((Container)(object)_popupWindow);
			val4.set_OuterControlPadding(new Vector2(5f, 10f));
			val4.set_ControlPadding(new Vector2(0f, 3f));
			_messageFlowPanel = val4;
			((Control)GameService.Graphics.get_SpriteScreen()).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnScreenClicked);
			RefreshMessageUI();
		}

		private void RefreshMessageUI()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			if (_messageFlowPanel == null)
			{
				return;
			}
			((Container)_messageFlowPanel).ClearChildren();
			foreach (MessageEntry message in _messages)
			{
				Label val = new Label();
				val.set_Text(message.Title);
				((Control)val).set_Width(194);
				((Control)val).set_Height(28);
				val.set_TextColor(new Color(220, 200, 150, 255));
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_ShowShadow(true);
				val.set_HorizontalAlignment((HorizontalAlignment)0);
				val.set_VerticalAlignment((VerticalAlignment)1);
				val.set_AutoSizeHeight(false);
				val.set_AutoSizeWidth(false);
				val.set_WrapText(false);
				((Control)val).set_BackgroundColor(new Color(40, 35, 30, 180));
				((Control)val).set_Parent((Container)(object)_messageFlowPanel);
				Label label = val;
				MessageEntry capturedMessage = message;
				((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnMessageSelected(capturedMessage);
					HidePopup();
				});
				((Control)label).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					((Control)label).set_BackgroundColor(new Color(60, 50, 40, 200));
					label.set_TextColor(Color.get_Yellow());
				});
				((Control)label).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					((Control)label).set_BackgroundColor(new Color(40, 35, 30, 180));
					label.set_TextColor(new Color(220, 200, 150, 255));
				});
			}
			int num = Math.Min(_messages.Count, 15);
			int itemHeight = 28;
			int itemPadding = 3;
			int outerPadding = 20;
			int flowPanelHeight = num * (itemHeight + itemPadding) + outerPadding;
			int minFlowPanelHeight = 3 * (itemHeight + itemPadding) + outerPadding;
			flowPanelHeight = Math.Max(flowPanelHeight, minFlowPanelHeight);
			((Control)_messageFlowPanel).set_Size(new Point(204, flowPanelHeight));
			int popupHeight = 40 + flowPanelHeight + 10;
			((Control)_popupWindow).set_Size(new Point(220, popupHeight));
			((Control)_popupCloseButton).set_Location(new Point(((Control)_popupWindow).get_Width() - 35, 5));
			((Control)_messageFlowPanel).Invalidate();
		}

		private void OnShowCornerIconChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue() && _cornerIcon == null)
			{
				CreateCornerIcon();
			}
			else if (!e.get_NewValue() && _cornerIcon != null)
			{
				((Control)_cornerIcon).Dispose();
				_cornerIcon = null;
			}
		}

		private void OnPopupKeybindActivated(object sender, EventArgs e)
		{
			ShowPopupAtCursor();
		}

		private void OnCornerIconClicked(object sender, MouseEventArgs e)
		{
			ShowPopupAtCursor();
		}

		private void OnCornerIconRightClicked(object sender, MouseEventArgs e)
		{
			ShowEditorWindow();
			Logger.Info("Editor window opened via corner icon right-click");
		}

		private void OnToggleLTModeKeybindActivated(object sender, EventArgs e)
		{
			_ltModeEnabled.set_Value(!_ltModeEnabled.get_Value());
			string status = (_ltModeEnabled.get_Value() ? "ENABLED" : "DISABLED");
			ScreenNotification.ShowNotification("LT Messages: LT Mode " + status, (NotificationType)0, (Texture2D)null, 4);
			Logger.Info("LT Mode toggled via keybind: " + status);
		}

		private void OnOpenEditorKeybindActivated(object sender, EventArgs e)
		{
			ShowEditorWindow();
			Logger.Info("Editor window opened via keybind");
		}

		private void OnScreenClicked(object sender, MouseEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (_popupWindow != null && ((Control)_popupWindow).get_Visible())
			{
				Point mousePos = GameService.Input.get_Mouse().get_Position();
				Rectangle popupBounds = ((Control)_popupWindow).get_AbsoluteBounds();
				if (!((Rectangle)(ref popupBounds)).Contains(mousePos))
				{
					HidePopup();
				}
			}
		}

		private void ShowPopupAtCursor()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			if (_popupWindow == null || _messages.Count == 0)
			{
				return;
			}
			try
			{
				RefreshMessageUI();
				Point position = GameService.Input.get_Mouse().get_Position();
				int x = position.X;
				int y = position.Y;
				if (x + ((Control)_popupWindow).get_Width() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Width())
				{
					x = ((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_popupWindow).get_Width();
				}
				if (y + ((Control)_popupWindow).get_Height() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Height())
				{
					y = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_popupWindow).get_Height();
				}
				if (x < 0)
				{
					x = 0;
				}
				if (y < 0)
				{
					y = 0;
				}
				((Control)_popupWindow).set_Location(new Point(x, y));
				((Control)_popupWindow).Show();
				Logger.Debug($"Showing popup at {x}, {y}");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to show popup at cursor");
			}
		}

		private void HidePopup()
		{
			if (_popupWindow != null)
			{
				((Control)_popupWindow).Hide();
			}
		}

		private void ShowEditorWindow()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (_editorWindow == null)
			{
				CreateEditorWindow();
			}
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_editorWindow).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_editorWindow).get_Height()) / 2;
			((Control)_editorWindow).set_Location(new Point(x, y));
			((Control)_editorWindow).Show();
			RefreshEditorUI();
		}

		private void CreateEditorWindow()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Expected O, but got Unknown
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Expected O, but got Unknown
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Expected O, but got Unknown
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0651: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_066e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(500, 400));
			((Control)val).set_ZIndex(10000);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 240));
			val.set_ShowBorder(true);
			val.set_CanScroll(false);
			_editorWindow = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("\ud83d\udcdd Editor");
			((Control)val2).set_Width(82);
			((Control)val2).set_Location(new Point(10, 8));
			((Control)val2).set_Parent((Container)(object)_editorWindow);
			StandardButton editorTabButton = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("ℹ\ufe0f About");
			((Control)val3).set_Width(75);
			((Control)val3).set_Location(new Point(100, 8));
			((Control)val3).set_Parent((Container)(object)_editorWindow);
			StandardButton aboutTabButton = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Close");
			((Control)val4).set_Width(70);
			((Control)val4).set_Location(new Point(420, 8));
			((Control)val4).set_Parent((Container)(object)_editorWindow);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseEditor();
			});
			Panel val5 = new Panel();
			((Control)val5).set_Location(new Point(0, 36));
			((Control)val5).set_Size(new Point(500, 364));
			((Control)val5).set_Parent((Container)(object)_editorWindow);
			val5.set_CanScroll(false);
			((Control)val5).set_Visible(true);
			_editorTabContent = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Location(new Point(10, 4));
			((Control)val6).set_Width(150);
			((Control)val6).set_Parent((Container)(object)_editorTabContent);
			_listSelectorDropdown = val6;
			for (int i = 0; i < 6; i++)
			{
				_listSelectorDropdown.get_Items().Add(GetListDisplayName(i));
			}
			_listSelectorDropdown.set_SelectedItem(GetListDisplayName(_currentListIndex));
			_listSelectorDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!_suppressListSwitchWarning && _editDialogWindow != null && ((Control)_editDialogWindow).get_Visible())
				{
					string selectedItem = _listSelectorDropdown.get_SelectedItem();
					_suppressListSwitchWarning = true;
					_listSelectorDropdown.set_SelectedItem(GetListDisplayName(_currentListIndex));
					_suppressListSwitchWarning = false;
					ShowUnsavedChangesDialog(selectedItem);
				}
				else
				{
					string selectedItem2 = _listSelectorDropdown.get_SelectedItem();
					for (int j = 0; j < 6; j++)
					{
						if (GetListDisplayName(j) == selectedItem2)
						{
							_currentListIndex = j;
							Logger.Info($"Switched to editing {selectedItem2} (index {j})");
							LoadMessagesFromFile();
							RefreshEditorUI();
							break;
						}
					}
				}
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Defaults");
			((Control)val7).set_Width(70);
			((Control)val7).set_Location(new Point(170, 2));
			((Control)val7).set_Parent((Container)(object)_editorTabContent);
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RestoreDefaultMessages();
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text("Reset All");
			((Control)val8).set_Width(70);
			((Control)val8).set_Location(new Point(248, 2));
			((Control)val8).set_Parent((Container)(object)_editorTabContent);
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ResetAllListsToDefaults();
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text("Add");
			((Control)val9).set_Width(70);
			((Control)val9).set_Location(new Point(326, 2));
			((Control)val9).set_Parent((Container)(object)_editorTabContent);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowEditDialog(-1, null);
			});
			StandardButton val10 = new StandardButton();
			val10.set_Text("Rename");
			((Control)val10).set_Width(70);
			((Control)val10).set_Location(new Point(170, 32));
			((Control)val10).set_Parent((Container)(object)_editorTabContent);
			((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowRenameListDialog();
			});
			StandardButton val11 = new StandardButton();
			val11.set_Text("Help");
			((Control)val11).set_Width(70);
			((Control)val11).set_Location(new Point(416, 32));
			((Control)val11).set_Parent((Container)(object)_editorTabContent);
			((Control)val11).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowHelpDialog();
			});
			FlowPanel val12 = new FlowPanel();
			val12.set_FlowDirection((ControlFlowDirection)3);
			((Container)val12).set_WidthSizingMode((SizingMode)2);
			((Container)val12).set_HeightSizingMode((SizingMode)2);
			((Panel)val12).set_CanScroll(true);
			((Control)val12).set_Location(new Point(10, 65));
			((Control)val12).set_Size(new Point(480, 315));
			((Control)val12).set_Parent((Container)(object)_editorTabContent);
			val12.set_OuterControlPadding(new Vector2(5f, 5f));
			val12.set_ControlPadding(new Vector2(0f, 3f));
			_editorFlowPanel = val12;
			Panel val13 = new Panel();
			((Control)val13).set_Location(new Point(0, 36));
			((Control)val13).set_Size(new Point(500, 240));
			((Control)val13).set_Parent((Container)(object)_editorWindow);
			val13.set_CanScroll(false);
			((Control)val13).set_Visible(false);
			_aboutTabContent = val13;
			Label val14 = new Label();
			val14.set_Text("LT Messages");
			val14.set_Font(GameService.Content.get_DefaultFont18());
			val14.set_AutoSizeHeight(true);
			val14.set_AutoSizeWidth(true);
			((Control)val14).set_Location(new Point(15, 10));
			val14.set_TextColor(new Color(220, 200, 150, 255));
			val14.set_ShowShadow(true);
			((Control)val14).set_Parent((Container)(object)_aboutTabContent);
			Label val15 = new Label();
			val15.set_Text("v1.0.1  ·  Built by Senzall  ·  MIT Licence");
			val15.set_Font(GameService.Content.get_DefaultFont14());
			val15.set_AutoSizeHeight(true);
			val15.set_AutoSizeWidth(true);
			((Control)val15).set_Location(new Point(15, 42));
			val15.set_TextColor(new Color(150, 140, 120, 200));
			((Control)val15).set_Parent((Container)(object)_aboutTabContent);
			Label val16 = new Label();
			val16.set_Text("No gems, no paywall — tools built by a fellow player.");
			val16.set_Font(GameService.Content.get_DefaultFont14());
			val16.set_AutoSizeHeight(true);
			val16.set_AutoSizeWidth(true);
			((Control)val16).set_Location(new Point(15, 64));
			val16.set_TextColor(new Color(180, 170, 150, 180));
			((Control)val16).set_Parent((Container)(object)_aboutTabContent);
			Label val17 = new Label();
			val17.set_Text("Links open in your browser:");
			val17.set_AutoSizeHeight(true);
			val17.set_AutoSizeWidth(true);
			((Control)val17).set_Location(new Point(15, 88));
			val17.set_TextColor(new Color(140, 130, 110, 180));
			((Control)val17).set_Parent((Container)(object)_aboutTabContent);
			StandardButton val18 = new StandardButton();
			val18.set_Text("\ud83d\udcd6 Documentation");
			((Control)val18).set_Width(160);
			((Control)val18).set_Location(new Point(15, 108));
			((Control)val18).set_Parent((Container)(object)_aboutTabContent);
			((Control)val18).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenUrl("https://senzall.com/ltmessages");
			});
			StandardButton val19 = new StandardButton();
			val19.set_Text("\ud83c\udf10 senzall.com");
			((Control)val19).set_Width(140);
			((Control)val19).set_Location(new Point(185, 108));
			((Control)val19).set_Parent((Container)(object)_aboutTabContent);
			((Control)val19).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenUrl("https://senzall.com");
			});
			StandardButton val20 = new StandardButton();
			val20.set_Text("☕  Support on Ko-fi");
			((Control)val20).set_Width(200);
			((Control)val20).set_Location(new Point(15, 142));
			((Control)val20).set_Parent((Container)(object)_aboutTabContent);
			((Control)val20).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenUrl("https://ko-fi.com/senzall");
			});
			((Control)editorTabButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_editorTabContent).set_Visible(true);
				((Control)_aboutTabContent).set_Visible(false);
				RefreshEditorUI();
			});
			((Control)aboutTabButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)_editorTabContent).set_Visible(false);
				((Control)_aboutTabContent).set_Visible(true);
				((Control)_editorWindow).set_Size(new Point(500, 276));
			});
		}

		private void RefreshEditorUI()
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			if (_editorFlowPanel == null || (_editorTabContent != null && !((Control)_editorTabContent).get_Visible()))
			{
				return;
			}
			((Container)_editorFlowPanel).ClearChildren();
			int num = Math.Min(_messages.Count, 6);
			int itemHeight = 60;
			int itemPadding = 3;
			int outerPadding = 10;
			int flowPanelHeight = num * (itemHeight + itemPadding) + outerPadding;
			int minFlowPanelHeight = 3 * (itemHeight + itemPadding) + outerPadding;
			flowPanelHeight = Math.Max(flowPanelHeight, minFlowPanelHeight);
			((Control)_editorFlowPanel).set_Size(new Point(480, flowPanelHeight));
			int tabContentHeight = 65 + flowPanelHeight + 15;
			if (_editorTabContent != null)
			{
				((Control)_editorTabContent).set_Size(new Point(500, tabContentHeight));
			}
			((Control)_editorWindow).set_Size(new Point(500, 36 + tabContentHeight));
			for (int i = 0; i < _messages.Count; i++)
			{
				MessageEntry message = _messages[i];
				int index = i;
				Panel val = new Panel();
				((Control)val).set_Width(460);
				((Control)val).set_Height(60);
				((Control)val).set_BackgroundColor(new Color(40, 35, 30, 180));
				val.set_ShowBorder(true);
				((Control)val).set_Parent((Container)(object)_editorFlowPanel);
				Panel itemPanel = val;
				Label val2 = new Label();
				val2.set_Text("Title: " + message.Title);
				((Control)val2).set_Width(300);
				((Control)val2).set_Location(new Point(5, 5));
				val2.set_TextColor(new Color(220, 200, 150, 255));
				val2.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val2).set_Parent((Container)(object)itemPanel);
				Label val3 = new Label();
				val3.set_Text("Message: " + message.Message);
				((Control)val3).set_Width(300);
				((Control)val3).set_Location(new Point(5, 25));
				val3.set_TextColor(Color.get_White());
				val3.set_Font(GameService.Content.get_DefaultFont12());
				((Control)val3).set_Parent((Container)(object)itemPanel);
				StandardButton val4 = new StandardButton();
				val4.set_Text("Edit");
				((Control)val4).set_Width(60);
				((Control)val4).set_Location(new Point(320, 10));
				((Control)val4).set_Parent((Container)(object)itemPanel);
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ShowEditDialog(index, message);
				});
				StandardButton val5 = new StandardButton();
				val5.set_Text("Delete");
				((Control)val5).set_Width(60);
				((Control)val5).set_Location(new Point(390, 10));
				((Control)val5).set_Parent((Container)(object)itemPanel);
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					DeleteMessage(index);
				});
			}
			((Control)_editorFlowPanel).Invalidate();
		}

		private void ShowEditDialog(int messageIndex, MessageEntry message)
		{
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			_editingMessageIndex = messageIndex;
			_editingMessage = message;
			if (_editDialogWindow == null)
			{
				CreateEditDialog();
			}
			if (message != null)
			{
				((TextInputBase)_editTitleTextBox).set_Text(message.Title);
				((TextInputBase)_editMessageTextBox).set_Text(message.Message);
			}
			else
			{
				((TextInputBase)_editTitleTextBox).set_Text("");
				((TextInputBase)_editMessageTextBox).set_Text("");
			}
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_editDialogWindow).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_editDialogWindow).get_Height()) / 2;
			((Control)_editDialogWindow).set_Location(new Point(x, y));
			((Control)_editDialogWindow).Show();
		}

		private void CreateEditDialog()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(400, 250));
			((Control)val).set_ZIndex(10001);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
			val.set_ShowBorder(true);
			_editDialogWindow = val;
			Label val2 = new Label();
			val2.set_Text("Edit Message");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			((Control)val2).set_Parent((Container)(object)_editDialogWindow);
			Label val3 = new Label();
			val3.set_Text("Title (max 16 chars):");
			((Control)val3).set_Location(new Point(10, 45));
			((Control)val3).set_Width(200);
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Parent((Container)(object)_editDialogWindow);
			TextBox val4 = new TextBox();
			((Control)val4).set_Location(new Point(10, 65));
			((Control)val4).set_Width(380);
			((TextInputBase)val4).set_MaxLength(16);
			((Control)val4).set_Parent((Container)(object)_editDialogWindow);
			_editTitleTextBox = val4;
			Label val5 = new Label();
			val5.set_Text("Message (max 199 chars):");
			((Control)val5).set_Location(new Point(10, 100));
			((Control)val5).set_Width(200);
			val5.set_TextColor(Color.get_White());
			((Control)val5).set_Parent((Container)(object)_editDialogWindow);
			TextBox val6 = new TextBox();
			((Control)val6).set_Location(new Point(10, 120));
			((Control)val6).set_Width(380);
			((TextInputBase)val6).set_MaxLength(199);
			((Control)val6).set_Parent((Container)(object)_editDialogWindow);
			_editMessageTextBox = val6;
			Label val7 = new Label();
			val7.set_Text("0 / 199");
			((Control)val7).set_Location(new Point(10, 145));
			((Control)val7).set_Width(100);
			val7.set_TextColor(Color.get_Gray());
			val7.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val7).set_Parent((Container)(object)_editDialogWindow);
			Label charCountLabel = val7;
			((TextInputBase)_editMessageTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				charCountLabel.set_Text($"{((TextInputBase)_editMessageTextBox).get_Text().Length} / 199");
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text("Save");
			((Control)val8).set_Width(80);
			((Control)val8).set_Location(new Point(220, 200));
			((Control)val8).set_Parent((Container)(object)_editDialogWindow);
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveEditedMessage();
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text("Cancel");
			((Control)val9).set_Width(80);
			((Control)val9).set_Location(new Point(310, 200));
			((Control)val9).set_Parent((Container)(object)_editDialogWindow);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_editDialogWindow).Hide();
			});
		}

		private void SaveEditedMessage()
		{
			string title = ((TextInputBase)_editTitleTextBox).get_Text().Trim();
			string messageText = ((TextInputBase)_editMessageTextBox).get_Text().Trim();
			if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(messageText))
			{
				ScreenNotification.ShowNotification("LT Messages: Title and message cannot be empty", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			MessageEntry newEntry = new MessageEntry(title, messageText);
			if (_editingMessageIndex >= 0)
			{
				_messages[_editingMessageIndex] = newEntry;
			}
			else
			{
				_messages.Add(newEntry);
			}
			((Control)_editDialogWindow).Hide();
			RefreshEditorUI();
			RefreshMessageUI();
			SaveMessagesToFile();
		}

		private void DeleteMessage(int index)
		{
			if (index >= 0 && index < _messages.Count)
			{
				_messages.RemoveAt(index);
				RefreshEditorUI();
				RefreshMessageUI();
			}
		}

		private void ShowUnsavedChangesDialog(string targetListName)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(450, 180));
			((Control)val).set_ZIndex(10002);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
			val.set_ShowBorder(true);
			Panel confirmDialog = val;
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)confirmDialog).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)confirmDialog).get_Height()) / 2;
			((Control)confirmDialog).set_Location(new Point(x, y));
			Label val2 = new Label();
			val2.set_Text("Unsaved Changes");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			((Control)val2).set_Parent((Container)(object)confirmDialog);
			Label val3 = new Label();
			val3.set_Text("You have unsaved changes in the message editor.\nWhat would you like to do?");
			((Control)val3).set_Location(new Point(10, 45));
			((Control)val3).set_Width(430);
			((Control)val3).set_Height(50);
			val3.set_TextColor(Color.get_White());
			val3.set_WrapText(true);
			((Control)val3).set_Parent((Container)(object)confirmDialog);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Save & Switch");
			((Control)val4).set_Width(130);
			((Control)val4).set_Location(new Point(10, 130));
			((Control)val4).set_Parent((Container)(object)confirmDialog);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveEditedMessage();
				((Control)confirmDialog).Dispose();
				SwitchToList(targetListName);
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("Discard & Switch");
			((Control)val5).set_Width(130);
			((Control)val5).set_Location(new Point(150, 130));
			((Control)val5).set_Parent((Container)(object)confirmDialog);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_editDialogWindow).Hide();
				((Control)confirmDialog).Dispose();
				SwitchToList(targetListName);
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Cancel");
			((Control)val6).set_Width(130);
			((Control)val6).set_Location(new Point(290, 130));
			((Control)val6).set_Parent((Container)(object)confirmDialog);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)confirmDialog).Dispose();
			});
			((Control)confirmDialog).Show();
		}

		private void SwitchToList(string listName)
		{
			_suppressListSwitchWarning = true;
			_listSelectorDropdown.set_SelectedItem(listName);
			_suppressListSwitchWarning = false;
			for (int i = 0; i < 6; i++)
			{
				if (GetListDisplayName(i) == listName)
				{
					_currentListIndex = i;
					Logger.Info($"Switched to editing {listName} (index {i})");
					LoadMessagesFromFile();
					RefreshEditorUI();
					break;
				}
			}
		}

		private void CloseEditor()
		{
			if (_editDialogWindow != null && ((Control)_editDialogWindow).get_Visible())
			{
				ShowCloseEditorConfirmation();
			}
			else
			{
				((Control)_editorWindow).Hide();
			}
		}

		private void ShowCloseEditorConfirmation()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(450, 180));
			((Control)val).set_ZIndex(10002);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
			val.set_ShowBorder(true);
			Panel confirmDialog = val;
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)confirmDialog).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)confirmDialog).get_Height()) / 2;
			((Control)confirmDialog).set_Location(new Point(x, y));
			Label val2 = new Label();
			val2.set_Text("Unsaved Changes");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			((Control)val2).set_Parent((Container)(object)confirmDialog);
			Label val3 = new Label();
			val3.set_Text("You have unsaved changes in the message editor.\nWhat would you like to do?");
			((Control)val3).set_Location(new Point(10, 45));
			((Control)val3).set_Width(430);
			((Control)val3).set_Height(50);
			val3.set_TextColor(Color.get_White());
			val3.set_WrapText(true);
			((Control)val3).set_Parent((Container)(object)confirmDialog);
			StandardButton val4 = new StandardButton();
			val4.set_Text("Save & Close");
			((Control)val4).set_Width(130);
			((Control)val4).set_Location(new Point(10, 130));
			((Control)val4).set_Parent((Container)(object)confirmDialog);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveEditedMessage();
				((Control)confirmDialog).Dispose();
				((Control)_editorWindow).Hide();
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("Discard & Close");
			((Control)val5).set_Width(130);
			((Control)val5).set_Location(new Point(150, 130));
			((Control)val5).set_Parent((Container)(object)confirmDialog);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_editDialogWindow).Hide();
				((Control)confirmDialog).Dispose();
				((Control)_editorWindow).Hide();
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Cancel");
			((Control)val6).set_Width(130);
			((Control)val6).set_Location(new Point(290, 130));
			((Control)val6).set_Parent((Container)(object)confirmDialog);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)confirmDialog).Dispose();
			});
			((Control)confirmDialog).Show();
		}

		private void ShowRenameListDialog()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if (_renameDialogWindow == null)
			{
				CreateRenameDialog();
			}
			((TextInputBase)_renameTextBox).set_Text(GetListDisplayName(_currentListIndex));
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_renameDialogWindow).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_renameDialogWindow).get_Height()) / 2;
			((Control)_renameDialogWindow).set_Location(new Point(x, y));
			((Control)_renameDialogWindow).Show();
			((TextInputBase)_renameTextBox).set_Focused(true);
		}

		private void CreateRenameDialog()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Expected O, but got Unknown
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(350, 150));
			((Control)val).set_ZIndex(10002);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
			val.set_ShowBorder(true);
			_renameDialogWindow = val;
			Label val2 = new Label();
			val2.set_Text("Rename " + GetListDisplayName(_currentListIndex));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			((Control)val2).set_Parent((Container)(object)_renameDialogWindow);
			Label val3 = new Label();
			val3.set_Text("List Name (max 30 chars):");
			((Control)val3).set_Location(new Point(10, 45));
			((Control)val3).set_Width(200);
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Parent((Container)(object)_renameDialogWindow);
			TextBox val4 = new TextBox();
			((Control)val4).set_Location(new Point(10, 65));
			((Control)val4).set_Width(330);
			((TextInputBase)val4).set_MaxLength(30);
			((Control)val4).set_Parent((Container)(object)_renameDialogWindow);
			_renameTextBox = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Save");
			((Control)val5).set_Width(80);
			((Control)val5).set_Location(new Point(170, 100));
			((Control)val5).set_Parent((Container)(object)_renameDialogWindow);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveListRename();
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Cancel");
			((Control)val6).set_Width(80);
			((Control)val6).set_Location(new Point(260, 100));
			((Control)val6).set_Parent((Container)(object)_renameDialogWindow);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_renameDialogWindow).Hide();
			});
		}

		private void SaveListRename()
		{
			string newName = ((TextInputBase)_renameTextBox).get_Text().Trim();
			if (string.IsNullOrEmpty(newName))
			{
				ScreenNotification.ShowNotification("LT Messages: List name cannot be empty", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			_listNames[_currentListIndex] = newName;
			SaveInternalData();
			_listSelectorDropdown.get_Items().Clear();
			for (int i = 0; i < 6; i++)
			{
				_listSelectorDropdown.get_Items().Add(GetListDisplayName(i));
			}
			_listSelectorDropdown.set_SelectedItem(GetListDisplayName(_currentListIndex));
			((Control)_renameDialogWindow).Hide();
			ScreenNotification.ShowNotification("LT Messages: List renamed to '" + newName + "'", (NotificationType)0, (Texture2D)null, 4);
			Logger.Info($"Renamed list {_currentListIndex} to '{newName}'");
		}

		private void ShowHelpDialog()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (_helpDialogWindow == null)
			{
				CreateHelpDialog();
			}
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_helpDialogWindow).get_Width()) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_helpDialogWindow).get_Height()) / 2;
			((Control)_helpDialogWindow).set_Location(new Point(x, y));
			((Control)_helpDialogWindow).Show();
		}

		private void CreateHelpDialog()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(600, 500));
			((Control)val).set_ZIndex(10003);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
			val.set_ShowBorder(true);
			val.set_CanScroll(false);
			_helpDialogWindow = val;
			Label val2 = new Label();
			val2.set_Text("LT Messages - Help");
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			((Control)val2).set_Parent((Container)(object)_helpDialogWindow);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Close");
			((Control)val3).set_Width(80);
			((Control)val3).set_Location(new Point(510, 8));
			((Control)val3).set_Parent((Container)(object)_helpDialogWindow);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_helpDialogWindow).Hide();
			});
			Panel val4 = new Panel();
			((Control)val4).set_Location(new Point(10, 45));
			((Control)val4).set_Size(new Point(580, 445));
			((Control)val4).set_Parent((Container)(object)_helpDialogWindow);
			val4.set_CanScroll(true);
			val4.set_ShowBorder(false);
			Panel contentPanel = val4;
			string helpText = "QUICK START\r\n• Left-click [LT] icon to see your messages\r\n• Click a message to send it\r\n• Right-click [LT] icon to open this editor\r\n\r\nMODULE SETTINGS\r\nAll operational settings are found in:\r\nBlish HUD → Manage Modules → LT Messages\r\n\r\nCHAT SETTINGS\r\n\r\nChat Focus - How to open chat:\r\n• Shift+Enter: Opens squad chat directly\r\n• Enter: Opens last used chat (map, say, etc.)\r\n\r\nChat Action - What to do with message:\r\n• Send: Automatically types and sends\r\n• Paste Only: Copies to clipboard (Ctrl+V to paste)\r\n\r\nChat Command - Which channel to send to:\r\n• Default: Uses your current active channel\r\n• /squad: Squad broadcast\r\n• /map: Map chat\r\n• /party: Party chat\r\n• /guild: Guild chat\r\n• /say: Local say\r\n• /1 through /5: Whisper to party members\r\n• /g1 through /g6: Guild channels 1-6\r\n• And more!\r\n\r\nCOMMON CONFIGURATIONS\r\n\r\nFor Squad Broadcast:\r\n• Chat Focus: Shift+Enter\r\n• Chat Action: Send\r\n• Chat Command: Default (or /squad)\r\n\r\nFor Map Chat:\r\n• Chat Focus: Enter\r\n• Chat Action: Send\r\n• Chat Command: /map\r\n\r\nFor Manual Control (Clipboard):\r\n• Chat Action: Paste Only\r\n• (Chat Focus and Command don't matter)\r\n\r\nMESSAGE LISTS\r\n\r\n• Use 6 different lists for different events\r\n• Click dropdown to switch lists\r\n• Click \"Rename\" to give lists custom names\r\n  (e.g., \"WvW\", \"Metas\", \"HP Trains\")\r\n\r\nEDITING MESSAGES\r\n\r\n• Add: Create new message\r\n• Edit: Modify existing message (click in list)\r\n• Delete: Remove message (click Delete on message)\r\n• Save: Saves to file automatically\r\n• Defaults: Restore 30 default commander messages\r\n\r\nTYPING DELAY\r\n\r\n• Adjust if messages don't send reliably\r\n• Lower = faster typing (5-40ms recommended)\r\n• Higher = more reliable on slow systems\r\n• Default: 40ms works for most people\r\n\r\nTROUBLESHOOTING\r\n\r\nMessages not sending?\r\n• Check that LT Mode is enabled\r\n• Set Chat Action to \"Send\"\r\n• Try higher Typing Delay\r\n• Verify Chat Command is correct\r\n\r\nNeed more help?\r\n• Docs: senzall.com/ltmessages\r\n• GitHub: github.com/senzal/LTMessages\r\n• Blish Discord: discord.gg/FYKN3qh\r\n\r\n---\r\n\r\nTHANK YOU!\r\n\r\nThanks to the amazing Guild Wars 2 community\r\nand the Blish HUD community for using and\r\ncreating incredible plugins that make our\r\ngaming experience even better!\r\n\r\nIf LT Messages has been useful, you can\r\nsupport development on Ko-fi:\r\nko-fi.com/senzall ☕\r\n\r\nHappy commanding! ♥";
			Label val5 = new Label();
			val5.set_Text(helpText);
			((Control)val5).set_Location(new Point(5, 5));
			((Control)val5).set_Width(555);
			val5.set_TextColor(Color.get_White());
			val5.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val5).set_Parent((Container)(object)contentPanel);
			val5.set_WrapText(true);
			val5.set_AutoSizeHeight(true);
		}

		private void SaveMessagesToFile()
		{
			try
			{
				string filePath = GetFilePathForList(_currentListIndex);
				string directory = Path.GetDirectoryName(filePath);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				List<string> lines = new List<string>
				{
					"# ========================================",
					$"# LT Messages Configuration File - List {_currentListIndex}",
					"# ========================================",
					"# ",
					"# Format: Title,Message",
					"# Title: max 16 characters (shown in popup menu)",
					"# Message: max 200 characters (default GW2 chat limit)",
					"# ",
					"# Lines starting with # are comments",
					"# ========================================",
					""
				};
				foreach (MessageEntry message in _messages)
				{
					lines.Add(message.Title + "," + message.Message);
				}
				File.WriteAllLines(filePath, lines);
				Logger.Info($"Saved {_messages.Count} messages to {filePath} ({GetListDisplayName(_currentListIndex)})");
				ScreenNotification.ShowNotification($"LT Messages: Saved {_messages.Count} messages to {GetListDisplayName(_currentListIndex)}", (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save messages to file");
				ScreenNotification.ShowNotification("LT Messages: Failed to save messages to file", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void RestoreDefaultMessages()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected O, but got Unknown
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Panel val = new Panel();
				((Control)val).set_Size(new Point(450, 220));
				((Control)val).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 450) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 220) / 2));
				((Control)val).set_ZIndex(15000);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)val).set_BackgroundColor(new Color(25, 20, 15, 250));
				val.set_ShowBorder(true);
				Panel confirmDialog = val;
				Label val2 = new Label();
				val2.set_Text("⚠ Load Defaults for " + GetListDisplayName(_currentListIndex) + "?");
				val2.set_Font(GameService.Content.get_DefaultFont18());
				val2.set_AutoSizeHeight(true);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Location(new Point(20, 20));
				val2.set_TextColor(new Color(255, 200, 0, 255));
				val2.set_ShowShadow(true);
				((Control)val2).set_Parent((Container)(object)confirmDialog);
				string messageText = ((_currentListIndex == 0) ? $"WARNING: This will replace ALL messages in {GetListDisplayName(_currentListIndex)}\nwith the 30 default messages.\n\nYour current {_messages.Count} message(s) will be lost!\n\nThis action CANNOT be undone." : $"WARNING: This will replace ALL messages in {GetListDisplayName(_currentListIndex)}\nwith one sample message: 'Stack on Tag'.\n\nYour current {_messages.Count} message(s) will be lost!\n\nThis action CANNOT be undone.");
				Label val3 = new Label();
				val3.set_Text(messageText);
				((Control)val3).set_Width(410);
				((Control)val3).set_Height(100);
				((Control)val3).set_Location(new Point(20, 60));
				val3.set_TextColor(Color.get_White());
				val3.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val3).set_Parent((Container)(object)confirmDialog);
				StandardButton val4 = new StandardButton();
				val4.set_Text("Yes, Load Defaults");
				((Control)val4).set_Width(140);
				((Control)val4).set_Location(new Point(20, 170));
				((Control)val4).set_Parent((Container)(object)confirmDialog);
				StandardButton yesButton = val4;
				StandardButton val5 = new StandardButton();
				val5.set_Text("Cancel");
				((Control)val5).set_Width(100);
				((Control)val5).set_Location(new Point(170, 170));
				((Control)val5).set_Parent((Container)(object)confirmDialog);
				((Control)yesButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_messages = ((_currentListIndex == 0) ? GetDefaultMessages() : GetSampleMessage());
					RefreshEditorUI();
					RefreshMessageUI();
					SaveMessagesToFile();
					((Control)confirmDialog).Dispose();
					int count = _messages.Count;
					ScreenNotification.ShowNotification($"LT Messages: Loaded {count} default message(s) for {GetListDisplayName(_currentListIndex)}", (NotificationType)0, (Texture2D)null, 4);
					Logger.Info("Loaded default messages for " + GetListDisplayName(_currentListIndex));
				});
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((Control)confirmDialog).Dispose();
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to restore default messages");
				ScreenNotification.ShowNotification("LT Messages: Failed to load defaults", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void ResetAllListsToDefaults()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Panel val = new Panel();
				((Control)val).set_Size(new Point(500, 280));
				((Control)val).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 500) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 280) / 2));
				((Control)val).set_ZIndex(15000);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)val).set_BackgroundColor(new Color(40, 10, 10, 250));
				val.set_ShowBorder(true);
				Panel confirmDialog = val;
				Label val2 = new Label();
				val2.set_Text("⚠⚠⚠ RESET ALL LISTS? ⚠⚠⚠");
				val2.set_Font(GameService.Content.get_DefaultFont18());
				val2.set_AutoSizeHeight(true);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Location(new Point(20, 20));
				val2.set_TextColor(new Color(255, 100, 100, 255));
				val2.set_ShowShadow(true);
				((Control)val2).set_Parent((Container)(object)confirmDialog);
				Label val3 = new Label();
				val3.set_Text("DANGER: This will reset ALL 6 message lists (0-5)\nback to their default states!\n\n• List 0: 30 default messages\n• Lists 1-5: Single sample message\n\nALL your custom messages in ALL lists will be lost!\n\nThis action CANNOT be undone!\n\nAre you absolutely sure?");
				((Control)val3).set_Width(460);
				((Control)val3).set_Height(160);
				((Control)val3).set_Location(new Point(20, 60));
				val3.set_TextColor(Color.get_White());
				val3.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val3).set_Parent((Container)(object)confirmDialog);
				StandardButton val4 = new StandardButton();
				val4.set_Text("YES, RESET ALL LISTS");
				((Control)val4).set_Width(180);
				((Control)val4).set_Location(new Point(20, 230));
				((Control)val4).set_Parent((Container)(object)confirmDialog);
				StandardButton yesButton = val4;
				StandardButton val5 = new StandardButton();
				val5.set_Text("Cancel (Recommended)");
				((Control)val5).set_Width(160);
				((Control)val5).set_Location(new Point(210, 230));
				((Control)val5).set_Parent((Container)(object)confirmDialog);
				((Control)yesButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					try
					{
						for (int i = 0; i < 6; i++)
						{
							string filePathForList = GetFilePathForList(i);
							string directoryName = Path.GetDirectoryName(filePathForList);
							if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							CreateDefaultMessageFile(filePathForList, i);
							Logger.Info($"Reset List {i} to defaults");
						}
						LoadMessagesFromFile();
						RefreshEditorUI();
						RefreshMessageUI();
						((Control)confirmDialog).Dispose();
						ScreenNotification.ShowNotification("LT Messages: All lists reset to defaults!", (NotificationType)0, (Texture2D)null, 4);
						Logger.Info("Reset all message lists to defaults");
					}
					catch (Exception ex2)
					{
						Logger.Error(ex2, "Failed to reset all lists");
						ScreenNotification.ShowNotification("LT Messages: Failed to reset all lists", (NotificationType)2, (Texture2D)null, 4);
						((Control)confirmDialog).Dispose();
					}
				});
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((Control)confirmDialog).Dispose();
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to show reset dialog");
				ScreenNotification.ShowNotification("LT Messages: Failed to show reset dialog", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void OnMessageSelected(MessageEntry message)
		{
			Logger.Info("Message selected: " + message.Title);
			if (!_ltModeEnabled.get_Value())
			{
				ScreenNotification.ShowNotification("LT Messages: LT Mode is disabled. Enable it in settings to send messages.", (NotificationType)1, (Texture2D)null, 4);
				Logger.Warn("Message send blocked - LT Mode is disabled");
				HidePopup();
			}
			else if (_chatAction.get_Value() == ChatAction.Send)
			{
				SendMessageAutomatic(message);
			}
			else
			{
				SendMessageClipboard(message);
			}
		}

		private void SendMessageClipboard(MessageEntry message)
		{
			try
			{
				string originalClipboard = null;
				string commandString = GetChatCommandString(_chatCommand.get_Value());
				string clipboardText;
				if (!string.IsNullOrEmpty(commandString))
				{
					clipboardText = commandString + " " + message.Message;
				}
				else
				{
					clipboardText = message.Message;
				}
				Thread thread = new Thread((ThreadStart)delegate
				{
					try
					{
						originalClipboard = Clipboard.GetText();
						Clipboard.SetText(clipboardText);
					}
					catch (Exception ex3)
					{
						Logger.Error(ex3, "Clipboard operation failed");
						throw;
					}
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
				Logger.Info("Message copied to clipboard: " + clipboardText);
				Task.Run(async delegate
				{
					await Task.Delay(10000);
					if (!string.IsNullOrEmpty(originalClipboard))
					{
						Thread thread2 = new Thread((ThreadStart)delegate
						{
							try
							{
								Clipboard.SetText(originalClipboard);
							}
							catch (Exception ex2)
							{
								Logger.Warn(ex2, "Failed to restore clipboard");
							}
						});
						thread2.SetApartmentState(ApartmentState.STA);
						thread2.Start();
					}
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to copy message to clipboard");
				ScreenNotification.ShowNotification("LT Messages: Failed to copy - " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void SendMessageAutomatic(MessageEntry message)
		{
			Task.Run(async delegate
			{
				_ = 3;
				try
				{
					FocusGameWindow();
					await Task.Delay(100);
					string commandString = GetChatCommandString(_chatCommand.get_Value());
					string fullMessage;
					if (!string.IsNullOrEmpty(commandString))
					{
						fullMessage = commandString + " " + message.Message;
						Logger.Info("Sending message: " + fullMessage);
					}
					else
					{
						fullMessage = message.Message;
						Logger.Info("Sending message to active channel: " + message.Message);
					}
					if (_chatFocus.get_Value() == ChatFocus.ShiftEnter)
					{
						Logger.Debug("Sending Shift+Enter to open squad chat");
						SendKeyPress(13, shift: true);
					}
					else
					{
						Logger.Debug("Sending Enter to open last used chat");
						SendKeyPress(13);
					}
					await Task.Delay(150);
					Logger.Debug("Typing: " + fullMessage);
					await TypeString(fullMessage, (int)_sendDelay.get_Value());
					await Task.Delay(50);
					Logger.Debug("Sending Enter to send message");
					SendKeyPress(13);
					HidePopup();
					Logger.Info("Message auto-sent: " + message.Message);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to auto-send message");
					ScreenNotification.ShowNotification("LT Messages: Failed to send - " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			});
		}

		private static void OpenUrl(string url)
		{
			try
			{
				NativeMethods.AllowSetForegroundWindow(-1);
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to open URL: " + url);
			}
		}
	}
}
