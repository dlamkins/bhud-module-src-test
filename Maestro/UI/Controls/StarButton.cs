using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Controls
{
	public class StarButton : Control
	{
		private const int TEXTURE_SIZE = 16;

		private const int CONTROL_SIZE = 20;

		private bool _isFavorite;

		public bool IsFavorite
		{
			get
			{
				return _isFavorite;
			}
			set
			{
				_isFavorite = value;
				((Control)this).set_BasicTooltipText(value ? "Remove from Favorites" : "Add to Favorites");
			}
		}

		public StarButton()
			: this()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			base._size = new Point(20, 20);
			((Control)this).set_BasicTooltipText("Add to Favorites");
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = (_isFavorite ? MaestroIcons.StarFilled : MaestroIcons.StarOutline);
			Color color = (_isFavorite ? MaestroTheme.AmberGold : (((Control)this).get_MouseOver() ? MaestroTheme.MutedCream : MaestroTheme.MediumGray));
			int offset = 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, new Rectangle(offset, offset, 16, 16), color);
		}
	}
}
