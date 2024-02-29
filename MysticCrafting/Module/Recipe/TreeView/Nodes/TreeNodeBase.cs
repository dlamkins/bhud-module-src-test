using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public abstract class TreeNodeBase : Container
	{
		public Color BackgroundOpaqueColor = Color.Black;

		public Color FrameColor = Color.Black;

		private string _fullPath;

		internal ContextMenuStrip MenuStrip;

		protected int MenuDepth { get; set; }

		public bool Expanded { get; set; }

		public bool Expandable { get; set; } = true;


		public bool ShowBackground { get; set; } = true;


		public bool ShowFrame { get; set; } = true;


		public virtual bool LoadingChildren { get; set; }

		public bool MouseOverItemDetails { get; set; }

		public int PaddingLeft { get; set; } = 25;


		public int PanelHeight { get; set; }

		public int PanelExtensionHeight { get; set; }

		internal int PanelCollapsedHeight
		{
			get
			{
				if (!Expanded)
				{
					return PanelHeight;
				}
				return PanelHeight - PanelExtensionHeight;
			}
		}

		internal int PanelExpandedHeight => PanelHeight + PanelExtensionHeight;

		public Rectangle PanelRectangle => new Rectangle(2, 2, base.Width, PanelCollapsedHeight - 4);

		public float ArrowRotation { get; set; }

		public IEnumerable<TreeNodeBase> Nodes => _children.OfType<TreeNodeBase>();

		public abstract string PathName { get; }

		public virtual string FullPath => _fullPath ?? (_fullPath = GetFullPath());

		private Tween SlideAnimation { get; set; }

		public event EventHandler<MouseEventArgs> OnPanelClick;

		public virtual string GetFullPath(string separator = "/")
		{
			TreeNodeBase parent = base.Parent as TreeNodeBase;
			if (parent != null)
			{
				return parent.FullPath + separator + PathName;
			}
			return PathName;
		}

		public TreeNodeBase()
		{
		}

		public void ClearChildNodes()
		{
			while (Nodes.Count() > 0)
			{
				Nodes.FirstOrDefault().Parent = null;
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			TreeNodeBase newChild = e.ChangedChild as TreeNodeBase;
			if (newChild != null)
			{
				newChild.PanelHeight = PanelCollapsedHeight;
				newChild.MenuDepth = MenuDepth + 1;
				ReflowChildLayout(Nodes);
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			int height = (Expanded ? (PanelHeight - PanelExtensionHeight) : PanelHeight);
			MouseOverItemDetails = base.RelativeMousePosition.Y <= height;
			if (MouseOverItemDetails && Nodes.Any() && Expandable)
			{
				base.EffectBehind?.Enable();
			}
			else
			{
				base.EffectBehind?.Disable();
			}
			base.OnMouseMoved(e);
		}

		private int ReflowChildLayout(IEnumerable<TreeNodeBase> containerChildren)
		{
			int lastBottom = PanelHeight + 5;
			foreach (TreeNodeBase item in containerChildren.Where((TreeNodeBase c) => c.Visible))
			{
				item.Location = new Point(PaddingLeft, lastBottom);
				lastBottom = item.Bottom + 5;
			}
			return lastBottom + 5;
		}

		public void Toggle()
		{
			if (Expanded)
			{
				Expanded = false;
				PanelHeight -= PanelExtensionHeight;
				Animate(PanelHeight);
				RecalculateParentLayout();
			}
			else
			{
				Expanded = true;
				PanelHeight += PanelExtensionHeight;
				Animate(base.ContentRegion.Bottom + PanelExtensionHeight);
				RecalculateParentLayout();
			}
		}

		private void Animate(int newHeight)
		{
			SlideAnimation?.CancelAndComplete();
			float rotation = (Expanded ? ((float)Math.PI / 2f) : 0f);
			base.Height = newHeight;
			SlideAnimation = Control.Animation.Tweener.Tween(this, new
			{
				ArrowRotation = rotation
			}, 0.3f).Ease(Ease.QuadOut);
		}

		public override void RecalculateLayout()
		{
			if (base.EffectBehind != null)
			{
				base.EffectBehind.Size = new Vector2(PanelRectangle.Width, PanelRectangle.Height);
				base.EffectBehind.Location = new Vector2(PanelRectangle.X, PanelRectangle.Y);
			}
			UpdateContentRegion();
			RecalculateParentLayout();
		}

		public void RecalculateParentLayout()
		{
			TreeNodeBase parentContainer = base.Parent as TreeNodeBase;
			if (parentContainer != null && parentContainer.Expanded)
			{
				parentContainer.RecalculateLayout();
			}
			(base.Parent as TreeView)?.RecalculateLayout();
		}

		private void UpdateContentRegion()
		{
			int bottomChild = ReflowChildLayout(Nodes);
			base.ContentRegion = (Nodes.Any() ? new Rectangle(0, 0, base.Width, bottomChild) : new Rectangle(0, 0, base.Width, PanelHeight));
			base.Height = (Expanded ? base.ContentRegion.Bottom : PanelHeight);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			ItemTabsContainer tabsControl = base.Children.OfType<ItemTabsContainer>()?.FirstOrDefault();
			if ((tabsControl == null || base.RelativeMousePosition.Y <= tabsControl.Top || base.RelativeMousePosition.Y >= tabsControl.Bottom || base.RelativeMousePosition.X <= tabsControl.Left || base.RelativeMousePosition.X >= tabsControl.Right) && e.EventType == MouseEventType.LeftMouseButtonReleased && MouseOverItemDetails && Nodes.Any() && Expandable)
			{
				this.OnPanelClick?.Invoke(this, e);
				Toggle();
				ServiceContainer.AudioService.PlayMenuClick();
			}
		}

		protected override void OnRightMouseButtonReleased(MouseEventArgs e)
		{
			if (MouseOverItemDetails)
			{
				MenuStrip?.Show(e.MousePosition);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (ShowBackground)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, PanelRectangle, BackgroundOpaqueColor * 0.2f);
			}
			if (Expandable)
			{
				DrawArrow(spriteBatch);
			}
			if (ShowFrame)
			{
				DrawFrame(spriteBatch);
			}
			DrawPanelExtension(spriteBatch);
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawPanelExtension(SpriteBatch spriteBatch)
		{
			if (Expanded)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, PanelCollapsedHeight, base.Width, PanelExtensionHeight), Color.Black * 0.05f);
			}
		}

		private void DrawArrow(SpriteBatch spriteBatch)
		{
			if (Nodes.Any())
			{
				spriteBatch.DrawOnCtrl(this, ServiceContainer.TextureRepository.Textures.TextureArrow, new Rectangle(15, PanelCollapsedHeight / 2, 32, 32), null, Color.White, ArrowRotation, new Vector2(8f, 16f));
			}
		}

		private void DrawFrame(SpriteBatch spriteBatch)
		{
			Color lineColor = FrameColor * 0.5f;
			int lineSize = 2;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(PanelRectangle.X, 0, PanelRectangle.Width, lineSize), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(PanelRectangle.X, PanelCollapsedHeight - lineSize, PanelRectangle.Width, lineSize), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, lineSize, PanelRectangle.Height + lineSize * 2), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(PanelRectangle.Width - lineSize, 0, lineSize, PanelCollapsedHeight), lineColor);
		}
	}
}
