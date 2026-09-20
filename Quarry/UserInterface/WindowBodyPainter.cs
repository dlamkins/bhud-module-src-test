using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quarry.UserInterface
{
	public static class WindowBodyPainter
	{
		private const int TopFadeHeight = 46;

		private static AsyncTexture2D leftAccent;

		private static AsyncTexture2D LeftAccent => leftAccent ?? (leftAccent = AsyncTexture2D.FromAssetId(605025));

		public static void PaintBody(SpriteBatch spriteBatch, Control ctrl, Rectangle contentRegion, bool showLeftAccent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), contentRegion, UiStyle.WindowBody);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(contentRegion.X, contentRegion.Y, contentRegion.Width, 1), UiStyle.Accent * 0.25f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(contentRegion.X, contentRegion.Y, 1, contentRegion.Height), UiStyle.CardBorder);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref contentRegion)).get_Right() - 1, contentRegion.Y, 1, contentRegion.Height), UiStyle.CardBorder);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(contentRegion.X, ((Rectangle)(ref contentRegion)).get_Bottom() - 1, contentRegion.Width, 1), UiStyle.CardBorder);
			Texture2D fade = GameService.Content.GetTexture("fade-down-46");
			if (fade != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, fade, new Rectangle(contentRegion.X, contentRegion.Y, contentRegion.Width, 46));
			}
			if (showLeftAccent && LeftAccent != null && LeftAccent.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit(LeftAccent), new Rectangle(contentRegion.X, contentRegion.Y, LeftAccent.get_Texture().get_Width(), contentRegion.Height));
			}
		}
	}
}
