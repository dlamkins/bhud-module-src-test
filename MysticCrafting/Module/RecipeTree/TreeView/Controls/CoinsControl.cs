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
	public class CoinsControl : Control
	{
		private long _unitPrice;

		public long Gold;

		public long Silver;

		public long Copper;

		public long UnitPrice
		{
			get
			{
				return _unitPrice;
			}
			set
			{
				_unitPrice = value;
				Gold = value / 10000;
				Silver = value % 10000 / 100;
				Copper = value % 100;
				((Control)this).set_Width(CalculateWidth());
			}
		}

		public string SuffixText { get; set; }

		public Point IconSize { get; set; } = new Point(20, 20);


		public BitmapFont TextFont { get; set; } = GameService.Content.get_DefaultFont16();


		public CoinsControl(Container parent)
			: this()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
		}

		public int CalculateWidth()
		{
			int width = 0;
			if (Gold != 0L)
			{
				width += CalculateWidth(Gold);
			}
			if (Gold != 0L || Silver != 0L)
			{
				width += CalculateWidth(Silver);
			}
			if (Gold != 0L || Silver != 0L || Copper != 0L)
			{
				width += CalculateWidth(Copper);
			}
			return width;
		}

		public int CalculateWidth(long coin)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			string text = coin.ToString("N0");
			return (int)Math.Ceiling(TextFont.MeasureString(text).Width) + IconSize.X + 5;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			if (UnitPrice != 0L)
			{
				Rectangle position = default(Rectangle);
				((Rectangle)(ref position))._002Ector(0, bounds.Y, ((Control)this).get_Width(), ((Control)this).get_Height());
				if (Gold != 0L)
				{
					position.X += PaintCoin(Gold, ServiceContainer.TextureRepository.Textures.CoinGold, new Color(210, 177, 66), spriteBatch, position);
				}
				if (Gold != 0L || Silver != 0L)
				{
					position.X += PaintCoin(Silver, ServiceContainer.TextureRepository.Textures.CoinSilver, new Color(184, 184, 184), spriteBatch, position);
				}
				if (Gold != 0L || Silver != 0L || Copper != 0L)
				{
					position.X += PaintCoin(Copper, ServiceContainer.TextureRepository.Textures.CoinCopper, new Color(187, 102, 34), spriteBatch, position);
				}
				((Control)this).set_Size(new Point(position.X, ((Control)this).get_Size().Y));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, SuffixText, GameService.Content.get_DefaultFont16(), new Rectangle(position.X, position.Y, 50, 20), Color.get_White() * 0.7f, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		private int PaintCoin(long amount, AsyncTexture2D texture, Color color, SpriteBatch spriteBatch, Rectangle position)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			string text = amount.ToString("N0");
			int textWidth = (int)Math.Ceiling(TextFont.MeasureString(text).Width);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), RectangleExtension.OffsetBy(new Rectangle(position.X, position.Y, textWidth, 20), 1, 1), Color.get_Black(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), new Rectangle(position.X, position.Y, textWidth, 20), color, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(texture), new Rectangle(position.X + textWidth, position.Y + 3, IconSize.X, IconSize.Y));
			return textWidth + IconSize.X + 2;
		}
	}
}
