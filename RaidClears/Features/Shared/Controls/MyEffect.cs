using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Controls
{
	public class MyEffect : ControlEffect
	{
		private readonly Texture2D texture;

		private readonly Rectangle boundChange;

		public Color Tint { get; set; } = Color.get_Transparent();


		public MyEffect(Control assignedControl)
			: this(assignedControl)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			texture = AsyncTexture2D.op_Implicit(Service.Textures!.GetRandomGridBoxMask());
			boundChange = new Rectangle(Service.Random.Next(0, 3), Service.Random.Next(0, 2), Service.Random.Next(-3, 0), Service.Random.Next(-2, 0));
		}

		protected override SpriteBatchParameters GetSpriteBatchParameters()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			return new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
		}

		public override void PaintEffect(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (Service.Settings.OrganicGridBoxBackgrounds.get_Value())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ((ControlEffect)this).get_AssignedControl(), texture, RectangleExtension.Add(bounds, boundChange), Tint);
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ((ControlEffect)this).get_AssignedControl(), Textures.get_Pixel(), bounds, Tint);
			}
		}
	}
}
