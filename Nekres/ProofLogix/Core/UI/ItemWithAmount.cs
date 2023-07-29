using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.ProofLogix.Core.UI
{
	internal class ItemWithAmount : Image
	{
		private int _amount;

		private BitmapFont _font = GameService.Content.get_DefaultFont14();

		private readonly Color _amountColor = new Color(255, 237, 159);

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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			((Image)this).Paint(spriteBatch, bounds);
			string text = Amount.ToString();
			Rectangle dest = default(Rectangle);
			((Rectangle)(ref dest))._002Ector(-4, 0, bounds.Width, bounds.Height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, dest, _amountColor, false, true, 2, (HorizontalAlignment)2, (VerticalAlignment)0);
		}
	}
}
