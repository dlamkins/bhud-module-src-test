using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class ScreenDraw : Control
	{
		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).set_Size(((Control)((Control)this).get_Parent()).get_Size());
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			Viewport viewport = ((GraphicsResource)spriteBatch).get_GraphicsDevice().get_Viewport();
			Vector3 screenPosition = ((Viewport)(ref viewport)).Project(new Vector3(WorldUtil.GameToWorldCoord(GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X), WorldUtil.GameToWorldCoord(GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y), WorldUtil.GameToWorldCoord(GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z)), GameService.Gw2Mumble.get_PlayerCamera().get_Projection(), GameService.Gw2Mumble.get_PlayerCamera().get_View(), GameService.Gw2Mumble.get_PlayerCamera().get_PlayerView());
			new Vector2(screenPosition.X, screenPosition.Y);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle((int)screenPosition.X, (int)screenPosition.Y, 50, 40), Color.get_LightBlue());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Hello!", GameService.Content.get_DefaultFont18(), new Rectangle((int)screenPosition.X, (int)screenPosition.Y, 50, 40), Color.get_Magenta(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public ScreenDraw()
			: this()
		{
		}
	}
}
