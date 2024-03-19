using System;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;

namespace DanceDanceRotationModule.Util
{
	public static class KeysExtensions
	{
		public static string NotesString(KeyBinding keyBinding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			string text = NotesString(keyBinding.get_ModifierKeys());
			string primary = NotesString(keyBinding.get_PrimaryKey());
			return text + primary;
		}

		public static string NotesString(ModifierKeys modifierKeys)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			string retval = "";
			if (((Enum)modifierKeys).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				retval += "C+";
			}
			if (((Enum)modifierKeys).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				retval += "A+";
			}
			if (((Enum)modifierKeys).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				retval += "S+";
			}
			return retval;
		}

		public static string NotesString(Keys keys)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Expected I4, but got Unknown
			return (int)keys switch
			{
				0 => "", 
				8 => "Back", 
				9 => "Tab", 
				13 => "Enter", 
				20 => "Caps", 
				27 => "Escape", 
				32 => "Space", 
				33 => "PageUp", 
				34 => "PageDown", 
				35 => "End", 
				36 => "Home", 
				37 => "Left", 
				38 => "Up", 
				39 => "Right", 
				40 => "Down", 
				41 => "Select", 
				42 => "Print", 
				43 => "Execute", 
				44 => "PrintScreen", 
				45 => "Insert", 
				46 => "Delete", 
				47 => "Help", 
				48 => "0", 
				49 => "1", 
				50 => "2", 
				51 => "3", 
				52 => "4", 
				53 => "5", 
				54 => "6", 
				55 => "7", 
				56 => "8", 
				57 => "9", 
				65 => "A", 
				66 => "B", 
				67 => "C", 
				68 => "D", 
				69 => "E", 
				70 => "F", 
				71 => "G", 
				72 => "H", 
				73 => "I", 
				74 => "J", 
				75 => "K", 
				76 => "L", 
				77 => "M", 
				78 => "N", 
				79 => "O", 
				80 => "P", 
				81 => "Q", 
				82 => "R", 
				83 => "S", 
				84 => "T", 
				85 => "U", 
				86 => "V", 
				87 => "W", 
				88 => "X", 
				89 => "Y", 
				90 => "Z", 
				91 => "LeftWindows", 
				92 => "RightWindows", 
				93 => "Apps", 
				95 => "Sleep", 
				96 => "N0", 
				97 => "N1", 
				98 => "N2", 
				99 => "N3", 
				100 => "N4", 
				101 => "N5", 
				102 => "N6", 
				103 => "N7", 
				104 => "N8", 
				105 => "N9", 
				106 => "*", 
				107 => "+", 
				108 => "Separator", 
				109 => "-", 
				110 => ".", 
				111 => "/", 
				112 => "F1", 
				113 => "F2", 
				114 => "F3", 
				115 => "F4", 
				116 => "F5", 
				117 => "F6", 
				118 => "F7", 
				119 => "F8", 
				120 => "F9", 
				121 => "F10", 
				122 => "F11", 
				123 => "F12", 
				124 => "F13", 
				125 => "F14", 
				126 => "F15", 
				127 => "F16", 
				128 => "F17", 
				129 => "F18", 
				130 => "F19", 
				131 => "F20", 
				132 => "F21", 
				133 => "F22", 
				134 => "F23", 
				135 => "F24", 
				144 => "NumLock", 
				145 => "Scroll", 
				160 => "LeftShift", 
				161 => "RightShift", 
				162 => "LeftControl", 
				163 => "RightControl", 
				164 => "LeftAlt", 
				165 => "RightAlt", 
				166 => "BrowserBack", 
				167 => "BrowserForward", 
				168 => "BrowserRefresh", 
				169 => "BrowserStop", 
				170 => "BrowserSearch", 
				171 => "BrowserFavorites", 
				172 => "BrowserHome", 
				173 => "VolumeMute", 
				174 => "VolumeDown", 
				175 => "VolumeUp", 
				176 => "MediaNextTrack", 
				177 => "MediaPreviousTrack", 
				178 => "MediaStop", 
				179 => "MediaPlayPause", 
				180 => "LaunchMail", 
				181 => "SelectMedia", 
				182 => "LaunchApplication1", 
				183 => "LaunchApplication2", 
				186 => ";", 
				187 => "+", 
				188 => ",", 
				189 => "-", 
				190 => ".", 
				191 => "?", 
				192 => "~", 
				219 => "{", 
				220 => "|", 
				221 => "}", 
				222 => "\"", 
				223 => "Oem8", 
				226 => "\\", 
				229 => "ProcessKey", 
				246 => "Attn", 
				247 => "Crsel", 
				248 => "Exsel", 
				249 => "EraseEof", 
				250 => "Play", 
				251 => "Zoom", 
				253 => "Pa1", 
				254 => "OemClear", 
				202 => "ChatPadGreen", 
				203 => "ChatPadOrange", 
				19 => "Pause", 
				28 => "ImeConvert", 
				29 => "ImeNoConvert", 
				21 => "Kana", 
				25 => "Kanji", 
				243 => "OemAuto", 
				242 => "OemCopy", 
				244 => "OemEnlW", 
				_ => "?", 
			};
		}
	}
}
