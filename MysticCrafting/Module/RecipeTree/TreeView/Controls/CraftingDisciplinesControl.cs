using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class CraftingDisciplinesControl : Control
	{
		public int DisciplineCount { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.get_DefaultFont16();


		public AsyncTexture2D Icon { get; set; } = ServiceContainer.TextureRepository.Textures.CraftingIcon;


		public CraftingDisciplinesControl(Container parent)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			if (DisciplineCount != 0)
			{
				string text = "400";
				int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), RectangleExtension.OffsetBy(new Rectangle(0, 0, 20, 20), 1, 1), Color.get_Black(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), new Rectangle(0, 0, 20, 20), Color.get_LightYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Icon), new Rectangle(textWidth, 0, IconSize.X, IconSize.Y), Color.get_LightYellow());
			}
		}
	}
}
