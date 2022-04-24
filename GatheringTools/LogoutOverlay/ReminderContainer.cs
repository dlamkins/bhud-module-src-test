using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.LogoutOverlay
{
	public class ReminderContainer : Container
	{
		private const float RELATIVE_Y_OFFSET_FROM_SCREEN_CENTER = 0.005f;

		private readonly Image _reminderBackgroundImage;

		private readonly Texture2D _reminderBackgroundTexture;

		private readonly Texture2D _tool1Texture;

		private readonly Texture2D _tool2Texture;

		private readonly Texture2D _tool3Texture;

		private readonly Label _reminderTextLabel;

		private readonly Image _tool1Image;

		private readonly Image _tool2Image;

		private readonly Image _tool3Image;

		public ReminderContainer(ContentsManager contentsManager)
			: this()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_reminderBackgroundTexture = contentsManager.GetTexture("logoutDialogTextArea.png");
			_tool1Texture = contentsManager.GetTexture("1998933.png");
			_tool2Texture = contentsManager.GetTexture("1998934.png");
			_tool3Texture = contentsManager.GetTexture("1998935.png");
			Image val = new Image(AsyncTexture2D.op_Implicit(_reminderBackgroundTexture));
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(((Control)this).get_Size());
			_reminderBackgroundImage = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(_tool1Texture));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_ClipsBounds(false);
			_tool1Image = val2;
			Image val3 = new Image(AsyncTexture2D.op_Implicit(_tool2Texture));
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_ClipsBounds(false);
			_tool2Image = val3;
			Image val4 = new Image(AsyncTexture2D.op_Implicit(_tool3Texture));
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
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)_reminderBackgroundImage).set_Size(((Control)this).get_Size());
			});
		}

		protected override void DisposeControl()
		{
			Texture2D tool1Texture = _tool1Texture;
			if (tool1Texture != null)
			{
				((GraphicsResource)tool1Texture).Dispose();
			}
			Texture2D tool3Texture = _tool3Texture;
			if (tool3Texture != null)
			{
				((GraphicsResource)tool3Texture).Dispose();
			}
			Texture2D tool2Texture = _tool2Texture;
			if (tool2Texture != null)
			{
				((GraphicsResource)tool2Texture).Dispose();
			}
			Texture2D reminderBackgroundTexture = _reminderBackgroundTexture;
			if (reminderBackgroundTexture != null)
			{
				((GraphicsResource)reminderBackgroundTexture).Dispose();
			}
			((Container)this).DisposeControl();
		}

		public void MoveAboveLogoutDialog()
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

		public void UpdateReminderText(string reminderText)
		{
			_reminderTextLabel.set_Text(reminderText);
			UpdateChildLocations();
		}

		public void UpdateContainerSizeAndMoveAboveLogoutDialog(int size)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(670 * (5 + size) / 40, 75 * (5 + size) / 40));
			UpdateChildLocations();
			MoveAboveLogoutDialog();
		}

		public void UpdateReminderTextFontSize(int fontSizeIndex)
		{
			_reminderTextLabel.set_Font(FontService.Fonts[fontSizeIndex]);
			UpdateChildLocations();
		}

		public void UpdateIconSize(int iconSize)
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

		public static Point GetLogoutDialogTextCenter(int screenWidth, int screenHeight)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)((float)screenWidth * 0.5f);
			int y = (int)((float)screenHeight * 0.505f);
			return new Point(num, y);
		}
	}
}
