using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.MaestroCreator
{
	public abstract class BaseChip : Panel
	{
		protected const int BORDER_THICKNESS = 2;

		private bool _isSelected;

		public int Index { get; set; }

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				((Control)this).Invalidate();
			}
		}

		public event EventHandler RemoveClicked;

		public event EventHandler<MouseEventArgs> ChipClicked;

		protected void FireChipClicked(MouseEventArgs e)
		{
			this.ChipClicked?.Invoke(this, e);
		}

		protected void FireRemoveClicked()
		{
			this.RemoveClicked?.Invoke(this, EventArgs.Empty);
		}

		protected void DrawBorder(SpriteBatch spriteBatch, Rectangle bounds, Color borderColor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, bounds.Width, 2), borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, bounds.Height - 2, bounds.Width, 2), borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(0, 0, 2, bounds.Height), borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(bounds.Width - 2, 0, 2, bounds.Height), borderColor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_isSelected)
			{
				DrawBorder(spriteBatch, bounds, MaestroTheme.AmberGold);
			}
		}

		protected BaseChip()
			: this()
		{
		}
	}
}
