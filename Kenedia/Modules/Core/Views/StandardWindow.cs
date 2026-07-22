using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace Kenedia.Modules.Core.Views
{
	public class StandardWindow : Blish_HUD.Controls.StandardWindow
	{
		private Rectangle _subTitleRectangle;

		protected BitmapFont TitleFont = Control.Content.DefaultFont32;

		protected BitmapFont SubTitleFont = Control.Content.DefaultFont18;

		private Rectangle _subEmblemRectangle;

		private Rectangle _mainEmblemRectangle;

		private Rectangle _titleTextRegion;

		private Rectangle _titleRectangle;

		private Rectangle _versionRectangle;

		protected BitmapFont VersionFont = Control.Content.DefaultFont14;

		private readonly List<AnchoredContainer> _attachedContainers = new List<AnchoredContainer>();

		public bool IsActive => WindowBase2.ActiveWindow == this;

		public Version Version
		{
			[CompilerGenerated]
			get
			{
				return _003CVersion_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CVersion_003Ek__BackingField, value, delegate(Version v)
				{
					_003CVersion_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CName_003Ek__BackingField, value, delegate(string v)
				{
					_003CName_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public string SubName
		{
			[CompilerGenerated]
			get
			{
				return _003CSubName_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSubName_003Ek__BackingField, value, delegate(string v)
				{
					_003CSubName_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public AsyncTexture2D MainWindowEmblem { get; set; }

		public AsyncTexture2D SubWindowEmblem { get; set; }

		public Color NameColor { get; set; } = ContentService.Colors.ColonialWhite;


		public Color SubNameColor { get; set; } = Color.White;


		public StandardWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(background, windowRegion, contentRegion)
		{
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_subEmblemRectangle = new Rectangle(21, 6, 64, 64);
			_mainEmblemRectangle = new Rectangle(-43, -58, 128, 128);
			_titleTextRegion = new Rectangle(Math.Max(Math.Max((MainWindowEmblem != null) ? _mainEmblemRectangle.Right : 0, (SubWindowEmblem != null) ? _subEmblemRectangle.Right : 0) - 16, 0), 5, base.Width - Math.Max(_mainEmblemRectangle.Right, _subEmblemRectangle.Right) - 30, 30);
			_versionRectangle = Rectangle.Empty;
			if (Version != null && !string.IsNullOrEmpty($"v. {Version}"))
			{
				RectangleF versionBounds = VersionFont.GetStringRectangle($"v. {Version}");
				_versionRectangle = new Rectangle(_titleTextRegion.Right - (int)versionBounds.Width, _titleTextRegion.Top, (int)versionBounds.Width, _titleTextRegion.Height - 3);
			}
			if (!string.IsNullOrEmpty(Name))
			{
				foreach (BitmapFont font in new List<BitmapFont>(3)
				{
					Control.Content.DefaultFont32,
					Control.Content.DefaultFont18,
					Control.Content.DefaultFont16
				})
				{
					RectangleF titleBounds = font.GetStringRectangle(Name);
					Rectangle titleRectangle = new Rectangle(_titleTextRegion.Left, _titleTextRegion.Top, (int)titleBounds.Width, _titleTextRegion.Height);
					if ((float)_titleTextRegion.Width >= titleBounds.Width + 10f + (float)_versionRectangle.Width)
					{
						_titleRectangle = titleRectangle;
						TitleFont = font;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(SubName))
			{
				Rectangle subTitleRectangle = (_subTitleRectangle = new Rectangle(width: (int)SubTitleFont.GetStringRectangle(SubName).Width, x: _titleRectangle.Right + 25, y: _titleRectangle.Top, height: _titleRectangle.Height));
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (container.ZIndex != ZIndex)
				{
					container.ZIndex = ZIndex;
				}
			}
		}

		public void ShowAttached(AnchoredContainer container = null)
		{
			foreach (AnchoredContainer c in _attachedContainers)
			{
				if (container != c && c.Visible)
				{
					c.Hide();
				}
			}
			container?.Show();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			if (MainWindowEmblem != null)
			{
				spriteBatch.DrawOnCtrl(this, MainWindowEmblem, _mainEmblemRectangle, MainWindowEmblem.Bounds, Color.White, 0f, default(Vector2));
			}
			if (SubWindowEmblem != null)
			{
				spriteBatch.DrawOnCtrl(this, SubWindowEmblem, _subEmblemRectangle, SubWindowEmblem.Bounds, Color.White, 0f, default(Vector2));
			}
			if (_titleRectangle.Width <= _titleTextRegion.Width && !string.IsNullOrEmpty(Name))
			{
				spriteBatch.DrawStringOnCtrl(this, Name, TitleFont, _titleRectangle, NameColor, wrap: false, stroke: true);
			}
			if (_subTitleRectangle.Width <= _titleTextRegion.Width && !string.IsNullOrEmpty(SubName))
			{
				spriteBatch.DrawStringOnCtrl(this, SubName, SubTitleFont, _subTitleRectangle, SubNameColor, wrap: false, stroke: true);
			}
			if (Version != null && _titleTextRegion.Width >= _titleRectangle.Width + 10 + _versionRectangle.Width)
			{
				spriteBatch.DrawStringOnCtrl(this, $"v. {Version}", VersionFont, _versionRectangle, Color.White, wrap: false, stroke: true, 1, HorizontalAlignment.Right, VerticalAlignment.Bottom);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.Children.DisposeAll();
			SubWindowEmblem = null;
			MainWindowEmblem = null;
		}

		protected virtual void AttachContainer(AnchoredContainer container)
		{
			_attachedContainers.Add(container);
		}

		protected virtual void UnAttachContainer(AnchoredContainer container)
		{
			_attachedContainers.Remove(container);
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (container.Parent == Control.Graphics.SpriteScreen)
				{
					container.Hide();
				}
			}
		}
	}
}
