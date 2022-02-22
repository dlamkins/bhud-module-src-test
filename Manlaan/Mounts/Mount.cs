using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts
{
	public abstract class Mount
	{
		public string Name { get; private set; }

		public string DisplayName { get; private set; }

		public string ImageFileName { get; private set; }

		public bool IsDisabled { get; private set; }

		public DateTime? QueuedTimestamp { get; internal set; }

		public DateTime? LastUsedTimestamp { get; internal set; }

		public bool IsWaterMount { get; private set; }

		public bool IsWvWMount { get; private set; }

		public SettingEntry<int> OrderSetting { get; private set; }

		public SettingEntry<KeyBinding> KeybindingSetting { get; private set; }

		public CornerIcon CornerIcon { get; private set; }

		public Mount(SettingCollection settingCollection, string name, string displayName, string imageFileName, bool isUnderwaterMount, bool isWvWMount, int defaultOrderSettig)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			Name = name;
			DisplayName = displayName;
			ImageFileName = imageFileName;
			IsWaterMount = isUnderwaterMount;
			IsWvWMount = isWvWMount;
			OrderSetting = settingCollection.DefineSetting<int>("Mount" + name + "Order2", defaultOrderSettig, displayName + " Order", "", (SettingTypeRendererDelegate)null);
			KeybindingSetting = settingCollection.DefineSetting<KeyBinding>("Mount" + name + "Binding", new KeyBinding((Keys)0), displayName + " Binding", "", (SettingTypeRendererDelegate)null);
		}

		public void DoHotKey()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Invalid comparison between Unknown and I4
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Invalid comparison between Unknown and I4
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				QueuedTimestamp = DateTime.UtcNow;
				return;
			}
			LastUsedTimestamp = DateTime.UtcNow;
			if ((int)KeybindingSetting.get_Value().get_ModifierKeys() > 0)
			{
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Press((VirtualKeyShort)18, true);
				}
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Press((VirtualKeyShort)17, true);
				}
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Press((VirtualKeyShort)16, true);
				}
			}
			Keyboard.Press(ToVirtualKey(KeybindingSetting.get_Value().get_PrimaryKey()), true);
			Thread.Sleep(50);
			Keyboard.Release(ToVirtualKey(KeybindingSetting.get_Value().get_PrimaryKey()), true);
			if ((int)KeybindingSetting.get_Value().get_ModifierKeys() > 0)
			{
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Release((VirtualKeyShort)16, true);
				}
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Release((VirtualKeyShort)17, true);
				}
				if (((Enum)KeybindingSetting.get_Value().get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Release((VirtualKeyShort)18, true);
				}
			}
		}

		public void CreateCornerIcon(Texture2D img)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			CornerIcon val = new CornerIcon();
			val.set_IconName(DisplayName);
			val.set_Icon(AsyncTexture2D.op_Implicit(img));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(img));
			val.set_Priority(10);
			CornerIcon = val;
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DoHotKey();
			});
		}

		public void DisposeCornerIcon()
		{
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return (VirtualKeyShort)(short)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}
	}
}
