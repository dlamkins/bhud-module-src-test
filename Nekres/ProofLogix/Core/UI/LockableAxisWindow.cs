using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI
{
	internal class LockableAxisWindow : StandardWindow
	{
		public LockableAxisWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public LockableAxisWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public LockableAxisWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		public LockableAxisWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Point clamp = ((WindowBase2)this).HandleWindowResize(newSize);
			return new Point(((Control)this)._size.X, clamp.Y);
		}
	}
}
