using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.Classes
{
	internal class IconLabel : Control
	{
		public bool AutoSize;

		private string _Text;

		private Texture2D _Texture;

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
			}
		}

		public Texture2D Texture
		{
			get
			{
				return _Texture;
			}
			set
			{
				_Texture = value;
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			if (AutoSize)
			{
				float width = GameService.Content.get_DefaultFont14().GetStringRectangle(Text).Width + (float)((Control)this).get_Height() + 6f;
				if (width != (float)((Control)this).get_Width())
				{
					((Control)this).set_Width((int)width);
				}
				float height = GameService.Content.get_DefaultFont14().GetStringRectangle(Text).Height + 6f;
				if (height != (float)((Control)this).get_Height())
				{
					((Control)this).set_Height((int)height);
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			if (Texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Texture, new Rectangle(2, 2, ((Control)this).get_Height() - 4, ((Control)this).get_Height() - 4), (Rectangle?)Texture.get_Bounds(), Color.get_LightGray(), 0f, default(Vector2), (SpriteEffects)0);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, GameService.Content.get_DefaultFont14(), new Rectangle(((Control)this).get_Height() + 2, 2, ((Control)this).get_Width() - (((Control)this).get_Height() + 4), ((Control)this).get_Height() - 4), Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Texture = null;
		}

		public IconLabel()
			: this()
		{
		}
	}
}
