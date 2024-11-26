using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class ImageButton : Control
	{
		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D HoverTexture { get; set; }

		public AsyncTexture2D Icon { get; set; }

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			Thickness padding;
			if (((Control)this).get_MouseOver() && HoverTexture != null)
			{
				Texture2D obj = AsyncTexture2D.op_Implicit(HoverTexture);
				padding = ((Control)this).get_Padding();
				int num = (int)((Thickness)(ref padding)).get_Left();
				padding = ((Control)this).get_Padding();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, new Rectangle(num, (int)((Thickness)(ref padding)).get_Top(), ((Control)this).get_Size().X, ((Control)this).get_Size().Y), Color.get_White());
			}
			else if (Texture != null)
			{
				Texture2D obj2 = AsyncTexture2D.op_Implicit(Texture);
				padding = ((Control)this).get_Padding();
				int num2 = (int)((Thickness)(ref padding)).get_Left();
				padding = ((Control)this).get_Padding();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj2, new Rectangle(num2, (int)((Thickness)(ref padding)).get_Top(), ((Control)this).get_Size().X, ((Control)this).get_Size().Y), Color.get_White());
			}
		}

		protected override void DisposeControl()
		{
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			((Control)this).DisposeControl();
		}

		public ImageButton()
			: this()
		{
		}
	}
}
