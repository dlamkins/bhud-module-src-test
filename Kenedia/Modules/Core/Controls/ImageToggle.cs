using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class ImageToggle : Control
	{
		private readonly AsyncTexture2D _exTexture = AsyncTexture2D.FromAssetId(784262);

		private bool _clicked;

		private readonly Action<bool> _onChanged;

		private Rectangle _xTextureRectangle;

		private Rectangle _xDrawRectangle;

		private bool _checked;

		private Func<string> _setLocalizedTooltip;

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				((Control)this).set_BasicTooltipText(value?.Invoke());
			}
		}

		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D HoveredTexture { get; set; }

		public AsyncTexture2D ActiveTexture { get; set; }

		public AsyncTexture2D ClickedTexture { get; set; }

		public Rectangle TextureRectangle { get; set; }

		public Rectangle SizeRectangle { get; set; }

		public bool ShowX { get; set; }

		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				_checked = value;
				OnCheckedChanged();
			}
		}

		public event EventHandler<CheckChangedEvent> CheckedChanged;

		public ImageToggle()
			: this()
		{
		}

		public ImageToggle(Action<bool> onChanged)
			: this()
		{
			_onChanged = onChanged;
		}

		private void OnCheckedChanged()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent(_checked));
		}

		private AsyncTexture2D GetTexture()
		{
			if (!_clicked || ClickedTexture == null)
			{
				if (!Checked || ActiveTexture == null)
				{
					if (!((Control)this).get_MouseOver() || HoveredTexture == null)
					{
						return Texture;
					}
					return HoveredTexture;
				}
				return ActiveTexture;
			}
			return ClickedTexture;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = GetTexture();
			if (texture != null)
			{
				_clicked = _clicked && ((Control)this).get_MouseOver();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(texture), (SizeRectangle != Rectangle.get_Empty()) ? SizeRectangle : bounds, (Rectangle?)((TextureRectangle == Rectangle.get_Empty()) ? texture.get_Bounds() : TextureRectangle), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (ShowX && !Checked)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_exTexture), _xDrawRectangle, (Rectangle?)_xTextureRectangle, Color.get_White());
			}
		}

		public override void RecalculateLayout()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int size = Math.Min(((Control)this).get_Width() / 2, ((Control)this).get_Height() / 2);
			_xDrawRectangle = new Rectangle(((Control)this).get_Width() - size, ((Control)this).get_Height() - size, size, size);
			_xTextureRectangle = new Rectangle(4, 4, 28, 28);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			((Control)this).OnClick(e);
			Checked = !Checked;
			_onChanged?.Invoke(Checked);
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent(Checked));
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonPressed(e);
			_clicked = true;
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			_clicked = false;
		}
	}
}
