using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class FramedMaskedRegion : MaskedRegion
	{
		public Rectangle MaskedRegion;

		public Color BorderColor { get; set; } = ContentService.Colors.ColonialWhite;


		public RectangleDimensions BorderWidth { get; set; } = new RectangleDimensions(2);


		public FramedMaskedRegion()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			base.Parent = Control.Graphics.SpriteScreen;
			ZIndex = int.MaxValue;
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			MaskedRegion = new Rectangle(base.Location.X + BorderWidth.Left, base.Location.Y + BorderWidth.Top, base.Size.X - BorderWidth.Horizontal, base.Size.Y - BorderWidth.Vertical);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
			RecalculateLayout();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			base.Paint(spriteBatch, MaskedRegion);
			Color? borderColor = BorderColor;
			if (borderColor.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, BorderWidth.Top), Rectangle.get_Empty(), borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, BorderWidth.Top / 2), Rectangle.get_Empty(), borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, BorderWidth.Bottom), Rectangle.get_Empty(), borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, BorderWidth.Bottom / 2), Rectangle.get_Empty(), borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), BorderWidth.Left, bounds.Height), Rectangle.get_Empty(), borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), BorderWidth.Left / 2, bounds.Height), Rectangle.get_Empty(), borderColor.Value * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), BorderWidth.Right, bounds.Height), Rectangle.get_Empty(), borderColor.Value * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), BorderWidth.Right / 2, bounds.Height), Rectangle.get_Empty(), borderColor.Value * 0.6f);
			}
		}
	}
}
