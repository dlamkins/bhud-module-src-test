using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ModuleManagerPlus.UI
{
	internal class CardPanel : FlowPanel
	{
		private readonly AsyncTexture2D _backgroundTexture;

		public CardPanel(TextureLoader textureLoader)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(1050, 1050));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(50f, 50f));
			((FlowPanel)this).set_ControlPadding(new Vector2(50f, 50f));
			((Panel)this).set_CanScroll(true);
			((Container)this).set_HeightSizingMode((SizingMode)0);
			((Container)this).set_WidthSizingMode((SizingMode)0);
			((Control)this).set_ClipsBounds(true);
			_backgroundTexture = AsyncTexture2D.op_Implicit(textureLoader.LoadTextureFromRef("textures/1909321.png"));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (_backgroundTexture.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_backgroundTexture), bounds, Color.get_White());
			}
		}
	}
}
