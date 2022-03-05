using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts
{
	public abstract class Mount
	{
		private readonly Helper _helper;

		public string Name { get; private set; }

		public string DisplayName { get; private set; }

		public string ImageFileName { get; private set; }

		public DateTime? QueuedTimestamp { get; internal set; }

		public DateTime? LastUsedTimestamp { get; internal set; }

		public bool IsWaterMount { get; private set; }

		public bool IsWvWMount { get; private set; }

		public SettingEntry<int> OrderSetting { get; private set; }

		public SettingEntry<KeyBinding> KeybindingSetting { get; private set; }

		public CornerIcon CornerIcon { get; private set; }

		public bool IsAvailable
		{
			get
			{
				if (OrderSetting.get_Value() != 0)
				{
					return IsKeybindSet;
				}
				return false;
			}
		}

		public bool IsKeybindSet
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Invalid comparison between Unknown and I4
				if ((int)KeybindingSetting.get_Value().get_ModifierKeys() == 0)
				{
					return (int)KeybindingSetting.get_Value().get_PrimaryKey() > 0;
				}
				return true;
			}
		}

		public Mount(SettingCollection settingCollection, Helper helper, string name, string displayName, string imageFileName, bool isUnderwaterMount, bool isWvWMount, int defaultOrderSetting)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			_helper = helper;
			Name = name;
			DisplayName = displayName;
			ImageFileName = imageFileName;
			IsWaterMount = isUnderwaterMount;
			IsWvWMount = isWvWMount;
			OrderSetting = settingCollection.DefineSetting<int>("Mount" + name + "Order2", defaultOrderSetting, (Func<string>)(() => displayName + " Order"), (Func<string>)(() => ""));
			KeybindingSetting = settingCollection.DefineSetting<KeyBinding>("Mount" + name + "Binding", new KeyBinding((Keys)0), (Func<string>)(() => displayName + " Binding"), (Func<string>)(() => ""));
		}

		public async Task DoHotKey()
		{
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				QueuedTimestamp = DateTime.UtcNow;
				return;
			}
			if ((int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() == 0)
			{
				LastUsedTimestamp = DateTime.UtcNow;
			}
			await _helper.TriggerKeybind(KeybindingSetting);
		}

		public void CreateCornerIcon(Texture2D img)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
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
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DoHotKey();
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
	}
}
