using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Extensions
{
	public static class SpriteBatchExtensions
	{
		public static SpriteBatchParameters Clone(this SpriteBatchParameters sbp)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_SortMode(sbp.get_SortMode());
			val.set_BlendState(sbp.get_BlendState());
			val.set_SamplerState(sbp.get_SamplerState());
			val.set_DepthStencilState(sbp.get_DepthStencilState());
			val.set_RasterizerState(sbp.get_RasterizerState());
			val.set_Effect(sbp.get_Effect());
			val.set_TransformMatrix(sbp.get_TransformMatrix());
			return val;
		}

		public static void DrawFrame(this SpriteBatch spriteBatch, Control ctrl, Rectangle _selectorBounds, Color borderColor, int width = 1)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + width, ((Rectangle)(ref _selectorBounds)).get_Top(), _selectorBounds.Width - width * 2, width), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left() + width, ((Rectangle)(ref _selectorBounds)).get_Bottom() - width, _selectorBounds.Width - width * 2, width), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Left(), ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref _selectorBounds)).get_Right() - width, ((Rectangle)(ref _selectorBounds)).get_Top(), width, _selectorBounds.Height), (Rectangle?)Rectangle.get_Empty(), borderColor * 0.8f);
		}
	}
}
