using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class ImageToggle : Control, ICheckable
	{
		private readonly AsyncTexture2D _exTexture = AsyncTexture2D.FromAssetId(784262);

		private bool _clicked;

		private Rectangle _xTextureRectangle;

		private Rectangle _xDrawRectangle;

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D HoveredTexture { get; set; }

		public AsyncTexture2D ActiveTexture { get; set; }

		public AsyncTexture2D ClickedTexture { get; set; }

		public Rectangle TextureRectangle { get; set; }

		public Rectangle SizeRectangle { get; set; }

		public Color ImageColor { get; set; } = Color.White;


		public Color? ActiveColor { get; set; } = Color.White;


		public bool ShowX { get; set; }

		public bool Checked
		{
			[CompilerGenerated]
			get
			{
				return _003CChecked_003Ek__BackingField;
			}
			set
			{
				_003CChecked_003Ek__BackingField = value;
				OnCheckedChanged();
			}
		}

		public Action<bool> OnCheckChanged
		{
			[CompilerGenerated]
			get
			{
				return _003COnCheckChanged_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003COnCheckChanged_003Ek__BackingField, value);
			}
		}

		public event EventHandler<CheckChangedEvent> CheckedChanged;

		public ImageToggle()
		{
		}

		public ImageToggle(Action<bool> onChanged)
			: this()
		{
			OnCheckChanged = onChanged;
		}

		private void OnCheckedChanged()
		{
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent(Checked));
		}

		private AsyncTexture2D GetTexture()
		{
			if (!_clicked || ClickedTexture == null)
			{
				if (!Checked || ActiveTexture == null)
				{
					if (!base.MouseOver || HoveredTexture == null)
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
			AsyncTexture2D texture = GetTexture();
			if (texture != null)
			{
				_clicked = _clicked && base.MouseOver;
				spriteBatch.DrawOnCtrl(this, texture, (SizeRectangle != Rectangle.Empty) ? SizeRectangle : bounds, (TextureRectangle == Rectangle.Empty) ? texture.Bounds : TextureRectangle, (!Checked) ? ImageColor : (ActiveColor ?? ImageColor), 0f, default(Vector2));
			}
			if (ShowX && !Checked)
			{
				spriteBatch.DrawOnCtrl(this, _exTexture, _xDrawRectangle, _xTextureRectangle, Color.White);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int size = Math.Min(base.Width / 2, base.Height / 2);
			_xDrawRectangle = new Rectangle(base.Width - size, base.Height - size, size, size);
			_xTextureRectangle = new Rectangle(4, 4, 28, 28);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Checked = !Checked;
			OnCheckChanged?.Invoke(Checked);
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent(Checked));
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			_clicked = true;
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			_clicked = false;
		}
	}
}
