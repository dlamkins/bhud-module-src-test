using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Settings;

namespace RaidClears.Dungeons.Services
{
	public class DungeonCornerIconService : IDisposable
	{
		private readonly Texture2D _cornerIconTexture;

		private readonly Texture2D _cornerIconHoverTexture;

		private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;

		private readonly SettingEntry<bool> _dungeonEnabledSetting;

		private readonly EventHandler<MouseEventArgs> _cornerIconClickEventHandler;

		private readonly string _tooltip;

		private CornerIcon _cornerIcon;

		public DungeonCornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting, SettingEntry<bool> dungeonsEnabledSetting, string tooltip, EventHandler<MouseEventArgs> cornerIconClickEventHandler, TextureService textureService)
		{
			_tooltip = tooltip;
			_cornerIconIsVisibleSetting = cornerIconIsVisibleSetting;
			_dungeonEnabledSetting = dungeonsEnabledSetting;
			_cornerIconClickEventHandler = cornerIconClickEventHandler;
			_cornerIconTexture = textureService.DungeonsCornerIconTexture;
			_cornerIconHoverTexture = textureService.DungeonsCornerIconHoverTexture;
			_cornerIconIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			_dungeonEnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			if (dungeonsEnabledSetting.get_Value() && cornerIconIsVisibleSetting.get_Value())
			{
				CreateCornerIcon();
			}
		}

		public void Dispose()
		{
			_cornerIconIsVisibleSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			_dungeonEnabledSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			if (_cornerIcon != null)
			{
				RemoveCornerIcon();
			}
		}

		private void CreateCornerIcon()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			RemoveCornerIcon();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
			((Control)val).set_BasicTooltipText(_tooltip);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click(_cornerIconClickEventHandler);
		}

		private void RemoveCornerIcon()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click(_cornerIconClickEventHandler);
				((Control)_cornerIcon).Dispose();
				_cornerIcon = null;
			}
		}

		private void OnCornerIconIsVisibleSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (_dungeonEnabledSetting.get_Value() && _cornerIconIsVisibleSetting.get_Value())
			{
				CreateCornerIcon();
			}
			else
			{
				RemoveCornerIcon();
			}
		}
	}
}
