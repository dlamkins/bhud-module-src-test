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
		private readonly BitmapFont _titleFont = GameService.Content.get_DefaultFont32();

		private Rectangle _subEmblemRectangle;

		private Rectangle _mainEmblemRectangle;

		private Rectangle _titleRectangle;

		private string _name;

		private readonly List<AnchoredContainer> _attachedContainers = new List<AnchoredContainer>();

		public bool IsActive => WindowBase2.get_ActiveWindow() == this;

		public Version Version { get; set; }

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

		public AsyncTexture2D MainWindowEmblem { get; set; }

		public AsyncTexture2D SubWindowEmblem { get; set; }

		public StandardWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)


		public override void RecalculateLayout()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).RecalculateLayout();
			_subEmblemRectangle = new Rectangle(21, 6, 64, 64);
			_mainEmblemRectangle = new Rectangle(-43, -58, 128, 128);
			if (!string.IsNullOrEmpty(Name))
			{
				RectangleF titleBounds = _titleFont.GetStringRectangle(Name);
				_titleRectangle = new Rectangle(80, 2, (int)titleBounds.Width, Math.Max(30, (int)titleBounds.Height));
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
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			if (MainWindowEmblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(MainWindowEmblem), _mainEmblemRectangle, (Rectangle?)MainWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (SubWindowEmblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(SubWindowEmblem), _subEmblemRectangle, (Rectangle?)SubWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			if (_titleRectangle.Width < bounds.Width - (_subEmblemRectangle.Width - 20) && !string.IsNullOrEmpty(Name))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Name, _titleFont, _titleRectangle, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)2);
			}
			if (Version != (Version)null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"v. {Version}", Control.get_Content().get_DefaultFont16(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 150, ((Rectangle)(ref bounds)).get_Top() + 10, 100, 30), Color.get_White(), false, true, 1, (HorizontalAlignment)2, (VerticalAlignment)0);
			}
		}

		protected override void DisposeControl()
		{
			((WindowBase2)this).DisposeControl();
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
			AsyncTexture2D subWindowEmblem = SubWindowEmblem;
			if (subWindowEmblem != null)
			{
				subWindowEmblem.Dispose();
			}
			SubWindowEmblem = null;
			AsyncTexture2D mainWindowEmblem = MainWindowEmblem;
			if (mainWindowEmblem != null)
			{
				mainWindowEmblem.Dispose();
			}
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
