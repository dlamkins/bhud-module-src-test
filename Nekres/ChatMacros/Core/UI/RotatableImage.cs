using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ChatMacros.Core.UI
{
	internal class RotatableImage : Image
	{
		private float _rotation;

		public float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _rotation, value, false, "Rotation");
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (((Image)this).get_Texture() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Image)this).get_Texture()), bounds, (Rectangle?)((Image)this).get_SourceRectangle(), ((Image)this).get_Tint(), Rotation, Vector2.get_Zero(), ((Image)this).get_SpriteEffects());
			}
		}

		public RotatableImage()
			: this()
		{
		}
	}
}
