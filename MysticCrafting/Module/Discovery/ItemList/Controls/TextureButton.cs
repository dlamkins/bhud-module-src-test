using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Discovery.ItemList.Controls
{
	public class TextureButton : Control
	{
		public bool HasActiveState = true;

		public bool Active { get; set; }

		public AsyncTexture2D Texture { get; set; }

		protected override void OnClick(MouseEventArgs e)
		{
			Active = !Active;
			((Control)this).OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			Color tint = (HasActiveState ? (Color.get_White() * 0.3f) : Color.get_LightYellow());
			if (Active)
			{
				tint = Color.get_LightYellow();
			}
			if (((Control)this).get_MouseOver())
			{
				tint = Color.get_LightYellow() * 0.6f;
			}
			Texture2D obj = AsyncTexture2D.op_Implicit(Texture);
			Thickness padding = ((Control)this).get_Padding();
			int num = (int)((Thickness)(ref padding)).get_Left();
			padding = ((Control)this).get_Padding();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, new Rectangle(num, (int)((Thickness)(ref padding)).get_Top(), ((Control)this).get_Size().X, ((Control)this).get_Size().Y), tint);
		}

		public TextureButton()
			: this()
		{
		}
	}
}
