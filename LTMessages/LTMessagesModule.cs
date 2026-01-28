using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
		private enum ChatMethod
		{
			ShiftEnter,
			SlashCommand
		}

		private enum ChatCommand
		{
			Squad,
			Subgroup
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

		private static readonly Logger Logger = Logger.GetLogger<LTMessagesModule>();

		private SettingEntry<string> _messageFilePath;

		private SettingEntry<bool> _autoSendEnabled;

		private SettingEntry<int> _sendDelayMs;

		private SettingEntry<bool> _showCornerIcon;

		private SettingEntry<KeyBinding> _popupKeybind;

		private SettingEntry<KeyBinding> _toggleLTModeKeybind;

		private SettingEntry<KeyBinding> _openEditorKeybind;

		private SettingEntry<ChatMethod> _chatMethod;

		private SettingEntry<ChatCommand> _chatCommand;

		private SettingEntry<int> _maxMessageLength;

		private SettingEntry<bool> _ltModeEnabled;

		private CornerIcon _cornerIcon;

		private Panel _popupWindow;

		private StandardButton _popupCloseButton;

		private FlowPanel _messageFlowPanel;

		private Panel _editorWindow;

		private FlowPanel _editorFlowPanel;

		private Panel _editDialogWindow;

		private TextBox _editTitleTextBox;

		private TextBox _editMessageTextBox;

		private MessageEntry _editingMessage;

		private int _editingMessageIndex = -1;

		private List<MessageEntry> _messages = new List<MessageEntry>();

		private FileSystemWatcher _fileWatcher;

		private static readonly string DefaultFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2\\addons\\blishhud\\ltmessages\\messages.txt");

		private const int DefaultSendDelayMs = 200;

		private const int DefaultMaxMessageLength = 200;

		private const uint INPUT_KEYBOARD = 1u;

		private const uint KEYEVENTF_KEYDOWN = 0u;

		private const uint KEYEVENTF_KEYUP = 2u;

		private const ushort VK_RETURN = 13;

		private const ushort VK_SHIFT = 16;

		private const ushort VK_CONTROL = 17;

		private const ushort VK_V = 86;

		private const ushort VK_SLASH = 191;

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
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			_messageFilePath = settings.DefineSetting<string>("MessageFilePath", DefaultFilePath, (Func<string>)(() => "Message File Path"), (Func<string>)(() => "Path to messages.txt - Edit this file with any text editor to customize your messages. File auto-reloads when changed."));
			_popupKeybind = settings.DefineSetting<KeyBinding>("PopupKeybind", new KeyBinding(), (Func<string>)(() => "Popup Keybind"), (Func<string>)(() => "Press this key to show the message popup at your cursor (optional - no default binding)"));
			_toggleLTModeKeybind = settings.DefineSetting<KeyBinding>("ToggleLTModeKeybind", new KeyBinding(), (Func<string>)(() => "Toggle LT Mode Keybind"), (Func<string>)(() => "Press this key to toggle LT Mode on/off (optional - no default binding)"));
			_openEditorKeybind = settings.DefineSetting<KeyBinding>("OpenEditorKeybind", new KeyBinding(), (Func<string>)(() => "Open Editor Keybind"), (Func<string>)(() => "Press this key to open the message editor window (optional - no default binding)"));
			_showCornerIcon = settings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show Corner Icon"), (Func<string>)(() => "Display an icon in the Blish HUD menu for alternative access"));
			_ltModeEnabled = settings.DefineSetting<bool>("LTModeEnabled", true, (Func<string>)(() => "LT Mode Enabled"), (Func<string>)(() => "Enable this when you are a Lieutenant or Commander. Messages won't send when disabled."));
			_autoSendEnabled = settings.DefineSetting<bool>("AutoSendEnabled", false, (Func<string>)(() => "Auto-send messages"), (Func<string>)(() => "Automatically send messages to chat (if disabled, copies to clipboard only)"));
			_sendDelayMs = settings.DefineSetting<int>("SendDelayMs", 200, (Func<string>)(() => "Send delay (ms)"), (Func<string>)(() => "Delay between keystrokes when auto-sending (adjust if messages don't send reliably)"));
			SettingComplianceExtensions.SetRange(_sendDelayMs, 50, 500);
			_chatMethod = settings.DefineSetting<ChatMethod>("ChatMethod", ChatMethod.ShiftEnter, (Func<string>)(() => "Chat Method"), (Func<string>)(() => "Shift+Enter = Direct squad chat | Shift+/ = Use chat command"));
			_chatCommand = settings.DefineSetting<ChatCommand>("ChatCommand", ChatCommand.Squad, (Func<string>)(() => "Chat Command"), (Func<string>)(() => "Which command to use when Chat Method is set to Shift+/ (squad or subgroup)"));
			_maxMessageLength = settings.DefineSetting<int>("MaxMessageLength", 200, (Func<string>)(() => "Max Message Length"), (Func<string>)(() => "Maximum characters per message (GW2 limit is around 200)"));
			SettingComplianceExtensions.SetRange(_maxMessageLength, 50, 500);
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
			_messageFilePath.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnFilePathChanged);
		}

		protected override async Task LoadAsync()
		{
			Logger.Info("Loading LT Messages module...");
			_messages = GetDefaultMessages();
			Logger.Info($"Initialized with {_messages.Count} default messages");
			LoadMessagesFromFile();
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
			_messageFilePath.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnFilePathChanged);
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
			_messages.Clear();
			Logger.Info("LT Messages module unloaded.");
		}

		private List<MessageEntry> GetDefaultMessages()
		{
			return new List<MessageEntry>
			{
				new MessageEntry("Moving", "Tag is moving"),
				new MessageEntry("Stack", "Stack on Tag"),
				new MessageEntry("HP-Combat", "Please let Tag start the combat hp!"),
				new MessageEntry("HP-Commune", "Commune with HP and then stack on tag!"),
				new MessageEntry("Port", "Port is on the marker"),
				new MessageEntry("F-Vist", "F the Vista and then Stack on Tag"),
				new MessageEntry("POI", "Point of Interest on Tag!"),
				new MessageEntry("Bunny", "Bunny up for CC"),
				new MessageEntry("Take-WP", "Take the Waypoint."),
				new MessageEntry("Woosh-WP", "Woosh the Waypoint"),
				new MessageEntry("Red", "If it is red make it dead!"),
				new MessageEntry("Red-Circles", "Don't stand in the red circles"),
				new MessageEntry("Mech", "Watch for the bounty mechanics"),
				new MessageEntry("Help", "If you get lost ask for help!"),
				new MessageEntry("Guard", "We need 1-2 people to guard this spot"),
				new MessageEntry("Specials", "Special Squad can come get their loot"),
				new MessageEntry("No-Drop", "Please don't drop EMPs or other items. Let Commander setup stations.")
			};
		}

		private void LoadMessagesFromFile()
		{
			string filePath = _messageFilePath.get_Value();
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
					CreateDefaultMessageFile(filePath);
					_messages = GetDefaultMessages();
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
					int maxLen = _maxMessageLength?.get_Value() ?? 200;
					if (message.Length > maxLen)
					{
						message = message.Substring(0, maxLen);
						Logger.Warn($"Message truncated to {maxLen} characters: {message}");
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

		private void CreateDefaultMessageFile(string filePath)
		{
			try
			{
				string[] defaultMessages = new string[33]
				{
					"# ========================================",
					"# LT Messages Configuration File",
					"# ========================================",
					"# ",
					"# TO EDIT: Open this file in Notepad, VSCode, or any text editor",
					"# FILE LOCATION: " + filePath,
					"# ",
					"# After editing, save the file - changes reload automatically!",
					"# ",
					"# FORMAT: Title,Message",
					"#   Title: max 16 characters (shown in popup menu)",
					"#   Message: max 200 characters (default GW2 chat limit)",
					"# ",
					"# Lines starting with # are comments and ignored",
					"# ========================================",
					"",
					"Moving,Tag is moving",
					"Stack,Stack on Tag",
					"HP-Combat,Please let Tag start the combat hp!",
					"HP-Commune,Commune with HP and then stack on tag!",
					"Port,Port is on the marker",
					"F-Vist, F the Vista and then Stack on Tag",
					"POI,Point of Interest on Tag!",
					"Bunny,Bunny up for CC",
					"Take-WP,Take the Waypoint.",
					"Woosh-WP,Woosh the Waypoint",
					"Red,If it is red make it dead!",
					"Red-Circles,Don't stand in the red circles",
					"Mech,Watch for the bounty mechanics",
					"Help,If you get lost ask for help!",
					"Guard,We need 1-2 people to guard this spot",
					"Specials,Special Squad can come get their loot",
					"No-Drop,Please don't drop EMPs or other items. Let Commander setup stations."
				};
				File.WriteAllLines(filePath, defaultMessages);
				Logger.Info("Created default message file at " + filePath);
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
				string filePath = _messageFilePath.get_Value();
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
					Logger.Info("File watcher setup for " + filePath);
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
			Logger.Info("Message file changed, reloading...");
			LoadMessagesFromFile();
		}

		private void OnFilePathChanged(object sender, ValueChangedEventArgs<string> e)
		{
			LoadMessagesFromFile();
			SetupFileWatcher();
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
				((Control)_cornerIcon).set_BasicTooltipText("LT Messages\nLeft-click: Show messages\nRight-click: Open editor\nLT Mode: " + status);
			}
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
			((Control)val3).set_Location(new Point(((Control)_popupWindow).get_Width() - 30, 5));
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
			((Control)val4).set_Location(new Point(5, 35));
			((Control)val4).set_Size(new Point(210, 260));
			((Control)val4).set_Parent((Container)(object)_popupWindow);
			val4.set_OuterControlPadding(new Vector2(8f, 8f));
			val4.set_ControlPadding(new Vector2(0f, 2f));
			_messageFlowPanel = val4;
			((Control)GameService.Graphics.get_SpriteScreen()).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnScreenClicked);
			RefreshMessageUI();
		}

		private void RefreshMessageUI()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			if (_messageFlowPanel == null)
			{
				return;
			}
			((Container)_messageFlowPanel).ClearChildren();
			foreach (MessageEntry message in _messages)
			{
				Label val = new Label();
				val.set_Text(message.Title);
				((Control)val).set_Width(200);
				((Control)val).set_Height(26);
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
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			if (_popupWindow == null || _messages.Count == 0)
			{
				return;
			}
			try
			{
				Point position = GameService.Input.get_Mouse().get_Position();
				int itemHeight = 25;
				int windowHeight = Math.Min(_messages.Count * itemHeight + 60, 440);
				((Control)_popupWindow).set_Size(new Point(200, windowHeight));
				if (_popupCloseButton != null)
				{
					((Control)_popupCloseButton).set_Location(new Point(((Control)_popupWindow).get_Width() - 30, 5));
				}
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
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Size(new Point(500, 400));
			((Control)val).set_ZIndex(10000);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(25, 20, 15, 240));
			val.set_ShowBorder(true);
			val.set_CanScroll(false);
			_editorWindow = val;
			Label val2 = new Label();
			val2.set_Text("LT Messages Editor");
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(10, 10));
			val2.set_TextColor(new Color(220, 200, 150, 255));
			val2.set_ShowShadow(true);
			((Control)val2).set_Parent((Container)(object)_editorWindow);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Close");
			((Control)val3).set_Width(80);
			((Control)val3).set_Location(new Point(410, 8));
			((Control)val3).set_Parent((Container)(object)_editorWindow);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_editorWindow).Hide();
			});
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			((Panel)val4).set_CanScroll(true);
			((Control)val4).set_Location(new Point(10, 40));
			((Control)val4).set_Size(new Point(480, 310));
			((Control)val4).set_Parent((Container)(object)_editorWindow);
			val4.set_OuterControlPadding(new Vector2(5f, 5f));
			val4.set_ControlPadding(new Vector2(0f, 3f));
			_editorFlowPanel = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Add New Message");
			((Control)val5).set_Width(150);
			((Control)val5).set_Location(new Point(10, 360));
			((Control)val5).set_Parent((Container)(object)_editorWindow);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowEditDialog(-1, null);
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Save to File");
			((Control)val6).set_Width(120);
			((Control)val6).set_Location(new Point(170, 360));
			((Control)val6).set_Parent((Container)(object)_editorWindow);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveMessagesToFile();
			});
		}

		private void RefreshEditorUI()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			if (_editorFlowPanel == null)
			{
				return;
			}
			((Container)_editorFlowPanel).ClearChildren();
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
			val5.set_Text("Message (max 200 chars):");
			((Control)val5).set_Location(new Point(10, 100));
			((Control)val5).set_Width(200);
			val5.set_TextColor(Color.get_White());
			((Control)val5).set_Parent((Container)(object)_editDialogWindow);
			TextBox val6 = new TextBox();
			((Control)val6).set_Location(new Point(10, 120));
			((Control)val6).set_Width(380);
			((TextInputBase)val6).set_MaxLength(200);
			((Control)val6).set_Parent((Container)(object)_editDialogWindow);
			_editMessageTextBox = val6;
			Label val7 = new Label();
			val7.set_Text("0 / 200");
			((Control)val7).set_Location(new Point(10, 145));
			((Control)val7).set_Width(100);
			val7.set_TextColor(Color.get_Gray());
			val7.set_Font(GameService.Content.get_DefaultFont12());
			((Control)val7).set_Parent((Container)(object)_editDialogWindow);
			Label charCountLabel = val7;
			((TextInputBase)_editMessageTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				charCountLabel.set_Text($"{((TextInputBase)_editMessageTextBox).get_Text().Length} / 200");
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

		private void SaveMessagesToFile()
		{
			try
			{
				string filePath = _messageFilePath.get_Value();
				string directory = Path.GetDirectoryName(filePath);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				List<string> lines = new List<string>
				{
					"# ========================================", "# LT Messages Configuration File", "# ========================================", "# ", "# Format: Title,Message", "# Title: max 16 characters (shown in popup menu)", "# Message: max 200 characters (default GW2 chat limit)", "# ", "# Lines starting with # are comments", "# ========================================",
					""
				};
				foreach (MessageEntry message in _messages)
				{
					lines.Add(message.Title + "," + message.Message);
				}
				File.WriteAllLines(filePath, lines);
				Logger.Info($"Saved {_messages.Count} messages to {filePath}");
				ScreenNotification.ShowNotification($"LT Messages: Saved {_messages.Count} messages", (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to save messages to file");
				ScreenNotification.ShowNotification("LT Messages: Failed to save messages to file", (NotificationType)2, (Texture2D)null, 4);
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
			else if (_autoSendEnabled.get_Value())
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
				string clipboardText;
				if (_chatMethod.get_Value() == ChatMethod.SlashCommand)
				{
					string command = ((_chatCommand.get_Value() == ChatCommand.Squad) ? "squad" : "subgroup");
					clipboardText = "/" + command + " " + message.Message;
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
				_ = 5;
				try
				{
					FocusGameWindow();
					await Task.Delay(100);
					if (_chatMethod.get_Value() == ChatMethod.SlashCommand)
					{
						string command = ((_chatCommand.get_Value() == ChatCommand.Squad) ? "squad" : "subgroup");
						Logger.Info("Sending message: /" + command + " " + message.Message);
						Logger.Debug("Sending Shift+/ to open chat with /");
						SendKeyPress(191, shift: true);
						await Task.Delay(150);
						Logger.Debug("Typing: " + command + " " + message.Message);
						await TypeString(command + " " + message.Message);
					}
					else
					{
						Logger.Info("Sending message to squad chat: " + message.Message);
						Logger.Debug("Sending Shift+Enter to open squad chat");
						SendKeyPress(13, shift: true);
						await Task.Delay(150);
						Logger.Debug("Typing: " + message.Message);
						await TypeString(message.Message);
					}
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
	}
}
