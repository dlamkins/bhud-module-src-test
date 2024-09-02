using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarmingTracker
{
	public class ClickThroughImage : Image
	{
		public ClickThroughImage(Texture2D texture, Point location, Container parent)
			: this(AsyncTexture2D.op_Implicit(texture))
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(location);
			((Control)this).set_Parent(parent);
			SetOpacity(isTransparent: true);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void SetOpacity(bool isTransparent)
		{
			((Control)this).set_Opacity(isTransparent ? 0.1f : 0.8f);
		}

		public void SetLeft(int width)
		{
			((Control)this).set_Left(width - 70);
		}
	}
}
