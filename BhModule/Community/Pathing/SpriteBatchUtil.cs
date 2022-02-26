using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BhModule.Community.Pathing
{
	public static class SpriteBatchUtil
	{
		public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF destinationRectangle, Color tint)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 scale = default(Vector2);
			((Vector2)(ref scale))._002Ector(destinationRectangle.Width / (float)texture.get_Width(), destinationRectangle.Height / (float)texture.get_Height());
			spriteBatch.Draw(texture, Point2.op_Implicit(((RectangleF)(ref destinationRectangle)).get_Center() - ((RectangleF)(ref destinationRectangle)).get_Size() / 2f), (Rectangle?)null, tint, 0f, Vector2.get_Zero(), scale, (SpriteEffects)0, 0f);
		}
	}
}
