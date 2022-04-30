using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using GatheringTools.Services;
using GatheringTools.ToolSearch.Services;
using Microsoft.Xna.Framework;

namespace GatheringTools.LogoutOverlay
{
	public class ReminderContainer : Container
	{
		private const float RELATIVE_Y_OFFSET_FROM_SCREEN_CENTER = 0.005f;

		private readonly Image _reminderBackgroundImage;

		private readonly Label _reminderTextLabel;

		private readonly Image _tool1Image;

		private readonly Image _tool2Image;

		private readonly Image _tool3Image;

		public ReminderContainer(TextureService textureService, SettingService settingService)
			: this()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Image val = new Image(AsyncTexture2D.op_Implicit(textureService.ReminderBackgroundTexture));
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(((Control)this).get_Size());
			_reminderBackgroundImage = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(textureService.Tool1Texture));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_ClipsBounds(false);
			_tool1Image = val2;
			Image val3 = new Image(AsyncTexture2D.op_Implicit(textureService.Tool2Texture));
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_ClipsBounds(false);
			_tool2Image = val3;
			Image val4 = new Image(AsyncTexture2D.op_Implicit(textureService.Tool3Texture));
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_ClipsBounds(false);
			_tool3Image = val4;
			Label val5 = new Label();
			val5.set_TextColor(Color.get_Red());
			val5.set_ShowShadow(true);
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_ClipsBounds(false);
			((Control)val5).set_Parent((Container)(object)this);
			_reminderTextLabel = val5;
			UpdateReminderText(settingService.ReminderTextSetting.get_Value());
			UpdateReminderTextFontSize(settingService.ReminderTextFontSizeIndexSetting.get_Value());
			UpdateIconSize(settingService.ReminderIconSizeSetting.get_Value());
			UpdateContainerSizeAndMoveAboveLogoutDialog(settingService.ReminderWindowSizeSetting.get_Value());
			settingService.ReminderTextFontSizeIndexSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				UpdateReminderTextFontSize(e.get_NewValue());
			});
			settingService.ReminderTextSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				UpdateReminderText(e.get_NewValue());
			});
			settingService.ReminderWindowSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				UpdateContainerSizeAndMoveAboveLogoutDialog(e.get_NewValue());
			});
			settingService.ReminderIconSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object s, ValueChangedEventArgs<int> e)
			{
				UpdateIconSize(e.get_NewValue());
			});
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
		}

		private void OnSpriteScreenResized(object sender, ResizedEventArgs e)
		{
			MoveAboveLogoutDialog();
		}

		private void MoveAboveLogoutDialog()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Point logoutDialogTextCenter = GetLogoutDialogTextCenter(((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y);
			Point containerCenterToTopLeftCornerOffset = default(Point);
			((Point)(ref containerCenterToTopLeftCornerOffset))._002Ector(((Control)this).get_Size().X / 2, ((Control)this).get_Size().Y / 2);
			((Control)this).set_Location(logoutDialogTextCenter - containerCenterToTopLeftCornerOffset);
		}

		private void UpdateReminderText(string reminderText)
		{
			_reminderTextLabel.set_Text(reminderText);
			UpdateChildLocations();
		}

		private void UpdateContainerSizeAndMoveAboveLogoutDialog(int size)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(670 * (5 + size) / 40, 75 * (5 + size) / 40));
			((Control)_reminderBackgroundImage).set_Size(((Control)this).get_Size());
			UpdateChildLocations();
			MoveAboveLogoutDialog();
		}

		private void UpdateReminderTextFontSize(int fontSizeIndex)
		{
			_reminderTextLabel.set_Font(FontService.Fonts[fontSizeIndex]);
			UpdateChildLocations();
		}

		private void UpdateIconSize(int iconSize)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Point size = default(Point);
			((Point)(ref size))._002Ector(iconSize, iconSize);
			((Control)_tool1Image).set_Size(size);
			((Control)_tool2Image).set_Size(size);
			((Control)_tool3Image).set_Size(size);
			UpdateChildLocations();
		}

		private void UpdateChildLocations()
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			int labelAndToolWidth = ((Control)_reminderTextLabel).get_Width() + 3 * ((Control)_tool1Image).get_Width();
			int labelLocationOffsetX = (((Control)this).get_Width() - labelAndToolWidth) / 2;
			int labelLocationOffsetY = (((Control)this).get_Height() - ((Control)_reminderTextLabel).get_Height()) / 2;
			int toolOffsetY = (((Control)this).get_Height() - ((Control)_tool1Image).get_Height()) / 2;
			int tool1OffsetX = labelLocationOffsetX + ((Control)_reminderTextLabel).get_Width();
			int tool2OffsetX = tool1OffsetX + ((Control)_tool1Image).get_Width();
			int tool3OffsetX = tool1OffsetX + 2 * ((Control)_tool1Image).get_Width();
			((Control)_reminderTextLabel).set_Location(new Point(labelLocationOffsetX, labelLocationOffsetY));
			((Control)_tool1Image).set_Location(new Point(tool1OffsetX, toolOffsetY));
			((Control)_tool2Image).set_Location(new Point(tool2OffsetX, toolOffsetY));
			((Control)_tool3Image).set_Location(new Point(tool3OffsetX, toolOffsetY));
		}

		private static Point GetLogoutDialogTextCenter(int screenWidth, int screenHeight)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)((float)screenWidth * 0.5f);
			int y = (int)((float)screenHeight * 0.505f);
			return new Point(num, y);
		}

		protected override void DisposeControl()
		{
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			((Container)this).DisposeControl();
		}
	}
}
