using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Controls
{
	public class IconButton : StandardButton
	{
		private const int IconSize = 16;

		private Color _tint;

		private Texture2D _iconTexture;

		public Color Tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tint;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				if (!(_tint == value))
				{
					_tint = value;
					((Control)this).Invalidate();
				}
			}
		}

		public Texture2D IconTexture
		{
			get
			{
				return _iconTexture;
			}
			set
			{
				if (_iconTexture != value)
				{
					_iconTexture = value;
					((Control)this).Invalidate();
				}
			}
		}

		public IconButton(Texture2D icon, Color tint)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_iconTexture = icon;
			_tint = tint;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			((StandardButton)this).Paint(spriteBatch, bounds);
			if (_iconTexture != null)
			{
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(((Control)this)._size.X / 2 - 8, ((Control)this)._size.Y / 2 - 8, 16, 16);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _iconTexture, iconBounds, _tint);
			}
		}
	}
}
