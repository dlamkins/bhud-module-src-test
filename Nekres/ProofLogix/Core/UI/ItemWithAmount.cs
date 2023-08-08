using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.ProofLogix.Core.UI
{
	internal class ItemWithAmount : Image
	{
		private int _amount;

		private Color _borderColor;

		private BitmapFont _font = GameService.Content.get_DefaultFont16();

		private readonly Color _amountColor = new Color(255, 247, 169);

		public int Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _amount, value, false, "Amount");
			}
		}

		public Color BorderColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _borderColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _borderColor, value, false, "BorderColor");
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref _font, value, false, "Font");
			}
		}

		public ItemWithAmount(AsyncTexture2D icon)
			: this(icon)
		{
		}//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			((Image)this).Paint(spriteBatch, bounds);
			spriteBatch.DrawRectangleOnCtrl((Control)(object)this, bounds, 2, BorderColor);
			if (Amount > 1)
			{
				string text = Amount.ToString();
				Rectangle dest = default(Rectangle);
				((Rectangle)(ref dest))._002Ector(-6, 2, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, dest, _amountColor, false, true, 2, (HorizontalAlignment)2, (VerticalAlignment)0);
			}
		}
	}
}
