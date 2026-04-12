using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.MaestroCreator
{
	public abstract class BaseChip : Panel
	{
		protected const int BORDER_THICKNESS = 2;

		protected Color _currentColor;

		private bool _isSelected;

		private bool _isPlaying;

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

		public bool IsPlaying
		{
			get
			{
				return _isPlaying;
			}
			set
			{
				_isPlaying = value;
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

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (_isPlaying || _isSelected)
			{
				MaestroTheme.DrawRoundedRect(spriteBatch, (Control)(object)this, bounds, MaestroTheme.AmberGold);
				Rectangle inset = default(Rectangle);
				((Rectangle)(ref inset))._002Ector(2, 2, bounds.Width - 4, bounds.Height - 4);
				MaestroTheme.DrawRoundedRect(spriteBatch, (Control)(object)this, inset, _currentColor);
			}
			else
			{
				MaestroTheme.DrawRoundedRect(spriteBatch, (Control)(object)this, bounds, _currentColor);
			}
		}

		protected BaseChip()
			: this()
		{
		}
	}
}
