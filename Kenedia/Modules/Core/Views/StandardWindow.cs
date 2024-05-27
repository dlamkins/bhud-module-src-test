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
	public class StandardWindow : StandardWindow
	{
		private Rectangle _subTitleRectangle;

		protected BitmapFont TitleFont = Control.get_Content().get_DefaultFont32();

		protected BitmapFont SubTitleFont = Control.get_Content().get_DefaultFont18();

		private Rectangle _subEmblemRectangle;

		private Rectangle _mainEmblemRectangle;

		private Rectangle _titleTextRegion;

		private Rectangle _titleRectangle;

		private Rectangle _versionRectangle;

		private string _name;

		protected BitmapFont VersionFont = Control.get_Content().get_DefaultFont14();

		private Version _version;

		private string _subName;

		private readonly List<AnchoredContainer> _attachedContainers = new List<AnchoredContainer>();

		public bool IsActive => WindowBase2.get_ActiveWindow() == this;

		public Version Version
		{
			get
			{
				return _version;
			}
			set
			{
				Common.SetProperty(ref _version, value, ((Control)this).RecalculateLayout);
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
				Common.SetProperty(ref _name, value, ((Control)this).RecalculateLayout);
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
				Common.SetProperty(ref _subName, value, ((Control)this).RecalculateLayout);
			}
		}

		public AsyncTexture2D MainWindowEmblem { get; set; }

		public AsyncTexture2D SubWindowEmblem { get; set; }

		public Color NameColor { get; set; } = Colors.ColonialWhite;


		public Color SubNameColor { get; set; } = Color.get_White();


		public StandardWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
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
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).RecalculateLayout();
			_subEmblemRectangle = new Rectangle(21, 6, 64, 64);
			_mainEmblemRectangle = new Rectangle(-43, -58, 128, 128);
			_titleTextRegion = new Rectangle(Math.Max(Math.Max((MainWindowEmblem != null) ? ((Rectangle)(ref _mainEmblemRectangle)).get_Right() : 0, (SubWindowEmblem != null) ? ((Rectangle)(ref _subEmblemRectangle)).get_Right() : 0) - 16, 0), 5, ((Control)this).get_Width() - Math.Max(((Rectangle)(ref _mainEmblemRectangle)).get_Right(), ((Rectangle)(ref _subEmblemRectangle)).get_Right()) - 30, 30);
			_versionRectangle = Rectangle.get_Empty();
			if (Version != null && !string.IsNullOrEmpty($"v. {Version}"))
			{
				RectangleF versionBounds = VersionFont.GetStringRectangle($"v. {Version}");
				_versionRectangle = new Rectangle(((Rectangle)(ref _titleTextRegion)).get_Right() - (int)versionBounds.Width, ((Rectangle)(ref _titleTextRegion)).get_Top(), (int)versionBounds.Width, _titleTextRegion.Height - 3);
			}
			if (!string.IsNullOrEmpty(Name))
			{
				Rectangle titleRectangle = default(Rectangle);
				foreach (BitmapFont font in new List<BitmapFont>
				{
					Control.get_Content().get_DefaultFont32(),
					Control.get_Content().get_DefaultFont18(),
					Control.get_Content().get_DefaultFont16()
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
			((WindowBase2)this).UpdateContainer(gameTime);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (((Control)container).get_ZIndex() != ((Control)this).get_ZIndex())
				{
					((Control)container).set_ZIndex(((Control)this).get_ZIndex());
				}
			}
		}

		public void ShowAttached(AnchoredContainer container = null)
		{
			foreach (AnchoredContainer c in _attachedContainers)
			{
				if (container != c && ((Control)c).get_Visible())
				{
					((Control)c).Hide();
				}
			}
			if (container != null)
			{
				((Control)container).Show();
			}
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
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			if (MainWindowEmblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(MainWindowEmblem), _mainEmblemRectangle, (Rectangle?)MainWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (SubWindowEmblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(SubWindowEmblem), _subEmblemRectangle, (Rectangle?)SubWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (_titleRectangle.Width <= _titleTextRegion.Width && !string.IsNullOrEmpty(Name))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Name, TitleFont, _titleRectangle, NameColor, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (_subTitleRectangle.Width <= _titleTextRegion.Width && !string.IsNullOrEmpty(SubName))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, SubName, SubTitleFont, _subTitleRectangle, SubNameColor, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (Version != null && _titleTextRegion.Width >= _titleRectangle.Width + 10 + _versionRectangle.Width)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"v. {Version}", VersionFont, _versionRectangle, Color.get_White(), false, true, 1, (HorizontalAlignment)2, (VerticalAlignment)2);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			((WindowBase2)this).DisposeControl();
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
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
			((Control)this).OnHidden(e);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (((Control)container).get_Parent() == Control.get_Graphics().get_SpriteScreen())
				{
					((Control)container).Hide();
				}
			}
		}
	}
}
