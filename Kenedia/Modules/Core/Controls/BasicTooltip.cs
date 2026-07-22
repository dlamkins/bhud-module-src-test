using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class BasicTooltip : Control
	{
		public Rectangle TextureRectangle { get; set; } = new Rectangle(40, 25, 250, 250);


		public AsyncTexture2D Background { get; set; } = AsyncTexture2D.FromAssetId(156003);


		public BitmapFont Font
		{
			[CompilerGenerated]
			get
			{
				return _003CFont_003Ek__BackingField;
			}
			set
			{
				_003CFont_003Ek__BackingField = value;
				UpdateLayout();
			}
		}

		public string Text
		{
			[CompilerGenerated]
			get
			{
				return _003CText_003Ek__BackingField;
			}
			set
			{
				_003CText_003Ek__BackingField = value;
				if (value == null)
				{
					Hide();
				}
				UpdateLayout();
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
			base.Location = new Point(Control.Input.Mouse.Position.X, Control.Input.Mouse.Position.Y + 25);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Font != null && Text != null)
			{
				spriteBatch.DrawOnCtrl(this, Background, bounds, (TextureRectangle != Rectangle.Empty) ? TextureRectangle : Background.Bounds, Color.White, 0f, default(Vector2));
				spriteBatch.DrawStringOnCtrl(this, Text, Font, bounds.Add(new Rectangle(5, 5, -10, -10)), Color.White, wrap: false, HorizontalAlignment.Center, VerticalAlignment.Top);
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
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			base.Location = new Point(Control.Input.Mouse.Position.X, Control.Input.Mouse.Position.Y + 25);
		}

		private void UpdateLayout()
		{
			if (Font != null && Text != null)
			{
				Size2 sSize = Font.MeasureString(Text);
				base.Size = new Point(10 + (int)sSize.Width, 10 + (int)sSize.Height);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Background = null;
		}

		public BasicTooltip()
		{
			_003CFont_003Ek__BackingField = GameService.Content.DefaultFont14;
			base._002Ector();
		}
	}
}
