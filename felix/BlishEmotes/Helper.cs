using System.Resources;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes
{
	public class Helper
	{
		private static readonly Logger Logger = Logger.GetLogger<Helper>();

		public ResourceManager EmotesResourceManager { get; private set; }

		public static bool IsDebugEnabled()
		{
			if (0 == 0)
			{
				return GameService.Debug.EnableAdditionalDebugDisplay.Value;
			}
			return true;
		}

		public Helper()
		{
			EmotesResourceManager = new ResourceManager("felix.BlishEmotes.Strings.Emotes", typeof(Common).Assembly);
		}

		public void SendEmoteCommand(Emote emote)
		{
			if (emote.Locked)
			{
				Logger.Debug("SendEmoteCommand: Emote locked.");
			}
			else if (GameService.GameIntegration.Gw2Instance.IsInGame && !GameService.Gw2Mumble.UI.IsMapOpen)
			{
				GameService.GameIntegration.Chat.Send(emote.Command);
			}
		}

		public async Task TriggerKeybind(SettingEntry<KeyBinding> keybindingSetting)
		{
			Logger.Debug("TriggerKeybind entered");
			if (keybindingSetting.Value.ModifierKeys != 0)
			{
				Logger.Debug($"TriggerKeybind press modifiers {keybindingSetting.Value.ModifierKeys}");
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Alt))
				{
					Blish_HUD.Controls.Intern.Keyboard.Press(VirtualKeyShort.MENU);
				}
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Ctrl))
				{
					Blish_HUD.Controls.Intern.Keyboard.Press(VirtualKeyShort.CONTROL);
				}
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Shift))
				{
					Blish_HUD.Controls.Intern.Keyboard.Press(VirtualKeyShort.SHIFT);
				}
			}
			Logger.Debug($"TriggerKeybind press PrimaryKey {keybindingSetting.Value.PrimaryKey}");
			Blish_HUD.Controls.Intern.Keyboard.Press(ToVirtualKey(keybindingSetting.Value.PrimaryKey));
			await Task.Delay(50);
			Logger.Debug($"TriggerKeybind release PrimaryKey {keybindingSetting.Value.PrimaryKey}");
			Blish_HUD.Controls.Intern.Keyboard.Release(ToVirtualKey(keybindingSetting.Value.PrimaryKey));
			if (keybindingSetting.Value.ModifierKeys != 0)
			{
				Logger.Debug($"TriggerKeybind release modifiers {keybindingSetting.Value.ModifierKeys}");
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Shift))
				{
					Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.SHIFT);
				}
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Ctrl))
				{
					Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.CONTROL);
				}
				if (keybindingSetting.Value.ModifierKeys.HasFlag(ModifierKeys.Alt))
				{
					Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.MENU);
				}
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			try
			{
				return (VirtualKeyShort)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}
	}
}
