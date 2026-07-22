using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class Tag : Blish_HUD.Controls.FlowPanel, IFontControl
	{
		private readonly Label _text;

		private readonly ImageButton _delete;

		private readonly ImageButton _dummy;

		private Color _disabledColor = new Color(156, 156, 156);

		private Texture2D _disabledBackground;

		private bool _active;

		public int TagPanelIndex { get; set; }

		public Point DesiredSize => new Point((int)Font.MeasureString(Text).Width + 30, Math.Max(20, Font.LineHeight + 4) + 5);

		public Action OnDeleteAction { get; set; }

		public Action OnClickAction { get; set; }

		public BitmapFont Font
		{
			get
			{
				return _text.Font;
			}
			set
			{
				if (value != null && _text.Font != value)
				{
					_dummy.Size = new Point(value.LineHeight, value.LineHeight);
					_delete.Size = new Point(value.LineHeight, value.LineHeight);
					_text.Font = value;
					_text.Height = Math.Max(20, value.LineHeight + 4);
					base.Height = Math.Max(20, value.LineHeight + 4) + 5;
				}
			}
		}

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				if (_active != value)
				{
					_active = value;
					this.ActiveChanged?.Invoke(this, null);
				}
			}
		}

		public bool CanInteract { get; set; } = true;


		public AsyncTexture2D Background
		{
			[CompilerGenerated]
			get
			{
				return _003CBackground_003Ek__BackingField;
			}
			set
			{
				_003CBackground_003Ek__BackingField = value;
				if (value != null)
				{
					CreateDisabledBackground(null, null);
					_003CBackground_003Ek__BackingField.TextureSwapped += CreateDisabledBackground;
				}
			}
		}

		public bool ShowDelete
		{
			get
			{
				return _delete.Visible;
			}
			set
			{
				if (_delete != null)
				{
					_delete.Visible = value;
					_dummy.Visible = !value;
					Invalidate();
				}
			}
		}

		public string Text
		{
			get
			{
				return _text?.Text;
			}
			set
			{
				if (_text != null)
				{
					_text.Text = value;
					_text.Width = (int)Font.MeasureString(value).Width + 4;
					base.Width = (int)Font.MeasureString(value).Width + _delete.Width + (int)base.OuterControlPadding.X + base.AutoSizePadding.X + (int)base.ControlPadding.X;
				}
			}
		}

		public event EventHandler Deleted;

		public event EventHandler ActiveChanged;

		public Tag()
		{
			Background = AsyncTexture2D.FromAssetId(1620622);
			base.FlowDirection = ControlFlowDirection.SingleLeftToRight;
			base.OuterControlPadding = new Vector2(3f, 3f);
			base.ControlPadding = new Vector2(4f, 0f);
			base.AutoSizePadding = new Point(5, 0);
			_delete = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(156012),
				HoveredTexture = AsyncTexture2D.FromAssetId(156011),
				TextureRectangle = new Rectangle(4, 4, 24, 24),
				Size = new Point(20, 20),
				BasicTooltipText = string.Format(strings_common.DeleteX, strings_common.Tag)
			};
			_delete.Click += Delete_Click;
			_dummy = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(156025),
				TextureRectangle = new Rectangle(44, 48, 43, 46),
				Size = new Point(20, 20),
				Visible = false
			};
			_text = new Label
			{
				Parent = this,
				Height = Math.Max(20, Control.Content.DefaultFont14.LineHeight + 4),
				VerticalAlignment = VerticalAlignment.Middle,
				Text = "Tag"
			};
			base.Height = Math.Max(20, Font.LineHeight + 4) + 5;
			base.Width = (int)Font.MeasureString("Tag").Width + _delete.Width + (int)base.OuterControlPadding.X + base.AutoSizePadding.X + (int)base.ControlPadding.X;
			_text.Width = (int)Font.MeasureString("Tag").Width + 4;
		}

		public void SetActive(bool active)
		{
			_active = active;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Background != null)
			{
				AsyncTexture2D texture = (Active ? Background : ((_disabledBackground != null) ? ((AsyncTexture2D)_disabledBackground) : Background));
				spriteBatch.DrawOnCtrl(this, texture, bounds, bounds, Active ? (Color.White * 0.98f) : (_disabledColor * 0.8f));
			}
			Color color = Color.Black;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 2), Rectangle.Empty, color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 1), Rectangle.Empty, color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 2, bounds.Width, 2), Rectangle.Empty, color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 1, bounds.Width, 1), Rectangle.Empty, color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, 2, bounds.Height), Rectangle.Empty, color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, 1, bounds.Height), Rectangle.Empty, color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 2, bounds.Top, 2, bounds.Height), Rectangle.Empty, color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 1, bounds.Top, 1, bounds.Height), Rectangle.Empty, color * 0.6f);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!_delete.MouseOver)
			{
				base.OnClick(e);
				if (CanInteract)
				{
					Active = !Active;
				}
				OnClickAction?.Invoke();
			}
		}

		private void Delete_Click(object sender, MouseEventArgs e)
		{
			this.Deleted?.Invoke(this, EventArgs.Empty);
			OnDeleteAction?.Invoke();
			Dispose();
		}

		private void CreateDisabledBackground(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_disabledBackground = Background.Texture.ToGrayScaledPalettable();
			Background.TextureSwapped -= CreateDisabledBackground;
		}
	}
}
