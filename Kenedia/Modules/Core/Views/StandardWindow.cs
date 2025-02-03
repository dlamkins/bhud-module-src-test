using System;
using System.Collections.Generic;
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

		private string _name;

		protected BitmapFont VersionFont = Control.Content.DefaultFont14;

		private Version _version;

		private string _subName;

		private readonly List<AnchoredContainer> _attachedContainers = new List<AnchoredContainer>();

		public bool IsActive => WindowBase2.ActiveWindow == this;

		public Version Version
		{
			get
			{
				return _version;
			}
			set
			{
				Common.SetProperty(ref _version, value, new Action(RecalculateLayout));
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Common.SetProperty(ref _name, value, new Action(RecalculateLayout));
			}
		}

		public string SubName
		{
			get
			{
				return _subName;
			}
			set
			{
				Common.SetProperty(ref _subName, value, new Action(RecalculateLayout));
			}
		}

		public AsyncTexture2D MainWindowEmblem { get; set; }

		public AsyncTexture2D SubWindowEmblem { get; set; }

		public Color NameColor { get; set; } = ContentService.Colors.ColonialWhite;


		public Color SubNameColor { get; set; } = Color.get_White();


		public StandardWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(background, windowRegion, contentRegion)
		{
		}//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)


		public override void RecalculateLayout()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_subEmblemRectangle = new Rectangle(21, 6, 64, 64);
			_mainEmblemRectangle = new Rectangle(-43, -58, 128, 128);
			_titleTextRegion = new Rectangle(Math.Max(Math.Max((MainWindowEmblem != null) ? ((Rectangle)(ref _mainEmblemRectangle)).get_Right() : 0, (SubWindowEmblem != null) ? ((Rectangle)(ref _subEmblemRectangle)).get_Right() : 0) - 16, 0), 5, base.Width - Math.Max(((Rectangle)(ref _mainEmblemRectangle)).get_Right(), ((Rectangle)(ref _subEmblemRectangle)).get_Right()) - 30, 30);
			_versionRectangle = Rectangle.get_Empty();
			if (Version != null && !string.IsNullOrEmpty($"v. {Version}"))
			{
				RectangleF versionBounds = VersionFont.GetStringRectangle($"v. {Version}");
				_versionRectangle = new Rectangle(((Rectangle)(ref _titleTextRegion)).get_Right() - (int)versionBounds.Width, ((Rectangle)(ref _titleTextRegion)).get_Top(), (int)versionBounds.Width, _titleTextRegion.Height - 3);
			}
			if (!string.IsNullOrEmpty(Name))
			{
				Rectangle titleRectangle = default(Rectangle);
				foreach (BitmapFont font in new List<BitmapFont>(3)
				{
					Control.Content.DefaultFont32,
					Control.Content.DefaultFont18,
					Control.Content.DefaultFont16
				})
				{
					RectangleF titleBounds2 = font.GetStringRectangle(Name);
					((Rectangle)(ref titleRectangle))._002Ector(((Rectangle)(ref _titleTextRegion)).get_Left(), ((Rectangle)(ref _titleTextRegion)).get_Top(), (int)titleBounds2.Width, _titleTextRegion.Height);
					if ((float)_titleTextRegion.Width >= titleBounds2.Width + 10f + (float)_versionRectangle.Width)
					{
						_titleRectangle = titleRectangle;
						TitleFont = font;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(SubName))
			{
				RectangleF titleBounds = SubTitleFont.GetStringRectangle(SubName);
				Rectangle subTitleRectangle = default(Rectangle);
				((Rectangle)(ref subTitleRectangle))._002Ector(((Rectangle)(ref _titleRectangle)).get_Right() + 25, ((Rectangle)(ref _titleRectangle)).get_Top(), (int)titleBounds.Width, _titleRectangle.Height);
				_subTitleRectangle = subTitleRectangle;
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (MainWindowEmblem != null)
			{
				spriteBatch.DrawOnCtrl(this, MainWindowEmblem, _mainEmblemRectangle, MainWindowEmblem.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (SubWindowEmblem != null)
			{
				spriteBatch.DrawOnCtrl(this, SubWindowEmblem, _subEmblemRectangle, SubWindowEmblem.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
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
				spriteBatch.DrawStringOnCtrl(this, $"v. {Version}", VersionFont, _versionRectangle, Color.get_White(), wrap: false, stroke: true, 1, HorizontalAlignment.Right, VerticalAlignment.Bottom);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
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
