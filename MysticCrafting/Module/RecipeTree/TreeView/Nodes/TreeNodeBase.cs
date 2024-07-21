using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public abstract class TreeNodeBase : Container
	{
		public Color BackgroundOpaqueColor = Color.get_Black();

		public float BackgroundOpacity = 0.2f;

		public Color FrameColor = Color.get_Black();

		private string _fullPath;

		internal ContextMenuStrip MenuStrip;

		public TreeView TreeView { get; protected set; }

		public int NodeIndex { get; set; }

		protected int MenuDepth { get; set; }

		public bool Expanded { get; set; }

		public bool Expandable { get; set; } = true;


		public bool ShowBackground { get; set; } = true;


		public bool ShowFrame { get; set; } = true;


		public bool LoadingChildren { get; set; }

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

		public Rectangle PanelRectangle => new Rectangle(2, 2, ((Control)this).get_Width(), PanelCollapsedHeight - 4);

		public float ArrowRotation { get; set; }

		public IEnumerable<TreeNodeBase> ChildNodes => ((IEnumerable)base._children).OfType<TreeNodeBase>();

		public abstract string PathName { get; }

		public virtual string FullPath => _fullPath ?? (_fullPath = GetFullPath());

		private Tween SlideAnimation { get; set; }

		public event EventHandler<MouseEventArgs> OnPanelClick;

		public virtual string GetFullPath(string separator = "/")
		{
			TreeNodeBase parent = ((Control)this).get_Parent() as TreeNodeBase;
			if (parent != null)
			{
				return parent.FullPath + separator + PathName;
			}
			return PathName;
		}

		protected TreeNodeBase(Container parent)
			: this()
		{
		}//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)


		public void ClearChildNodes()
		{
			while (ChildNodes.Count() > 0)
			{
				((Control)ChildNodes.FirstOrDefault()).set_Parent((Container)null);
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			TreeNodeBase newChild = e.get_ChangedChild() as TreeNodeBase;
			if (newChild != null)
			{
				int maxChildIndex = (ChildNodes.Any() ? ChildNodes.Max((TreeNodeBase n) => n.NodeIndex) : 0);
				newChild.NodeIndex = ((maxChildIndex == 0) ? (++NodeIndex) : maxChildIndex++);
				newChild.PanelHeight = PanelCollapsedHeight;
				newChild.MenuDepth = MenuDepth + 1;
				ReflowChildLayout(ChildNodes);
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			int height = (Expanded ? (PanelHeight - PanelExtensionHeight) : PanelHeight);
			MouseOverItemDetails = ((Control)this).get_RelativeMousePosition().Y <= height;
			if (MouseOverItemDetails && ChildNodes.Any() && Expandable)
			{
				ControlEffect effectBehind = ((Control)this).get_EffectBehind();
				if (effectBehind != null)
				{
					effectBehind.Enable();
				}
			}
			else
			{
				ControlEffect effectBehind2 = ((Control)this).get_EffectBehind();
				if (effectBehind2 != null)
				{
					effectBehind2.Disable();
				}
			}
			((Control)this).OnMouseMoved(e);
		}

		private int ReflowChildLayout(IEnumerable<TreeNodeBase> containerChildren)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			int lastBottom = PanelHeight + 3;
			foreach (TreeNodeBase item in containerChildren.Where((TreeNodeBase c) => ((Control)c).get_Visible()))
			{
				((Control)item).set_Location(new Point(PaddingLeft, lastBottom));
				lastBottom = ((Control)item).get_Bottom() + 3;
			}
			return lastBottom + 5;
		}

		public void Toggle()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
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
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				Animate(((Rectangle)(ref contentRegion)).get_Bottom() + PanelExtensionHeight);
				RecalculateParentLayout();
			}
		}

		private void Animate(int newHeight)
		{
			Tween slideAnimation = SlideAnimation;
			if (slideAnimation != null)
			{
				slideAnimation.CancelAndComplete();
			}
			float rotation = (Expanded ? ((float)Math.PI / 2f) : 0f);
			((Control)this).set_Height(newHeight);
			SlideAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<TreeNodeBase>(this, (object)new
			{
				ArrowRotation = rotation
			}, 0.3f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
		}

		public override void RecalculateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_EffectBehind() != null)
			{
				((Control)this).get_EffectBehind().set_Size(new Vector2((float)PanelRectangle.Width, (float)PanelRectangle.Height));
				((Control)this).get_EffectBehind().set_Location(new Vector2((float)PanelRectangle.X, (float)PanelRectangle.Y));
			}
			UpdateContentRegion();
			RecalculateParentLayout();
		}

		public void RecalculateParentLayout()
		{
			TreeNodeBase parentContainer = ((Control)this).get_Parent() as TreeNodeBase;
			if (parentContainer != null && parentContainer.Expanded)
			{
				((Control)parentContainer).RecalculateLayout();
			}
			TreeView list = ((Control)this).get_Parent() as TreeView;
			if (list != null)
			{
				((Control)list).RecalculateLayout();
			}
		}

		private void UpdateContentRegion()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			int bottomChild = ReflowChildLayout(ChildNodes);
			((Container)this).set_ContentRegion(ChildNodes.Any() ? new Rectangle(0, 0, ((Control)this).get_Width(), bottomChild) : new Rectangle(0, 0, ((Control)this).get_Width(), PanelHeight));
			int height;
			if (!Expanded)
			{
				height = PanelHeight;
			}
			else
			{
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				height = ((Rectangle)(ref contentRegion)).get_Bottom();
			}
			((Control)this).set_Height(height);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Invalid comparison between Unknown and I4
			ItemTabsContainer tabsControl = ((IEnumerable)((Container)this).get_Children()).OfType<ItemTabsContainer>()?.FirstOrDefault();
			if ((tabsControl == null || ((Control)this).get_RelativeMousePosition().Y <= ((Control)tabsControl).get_Top() || ((Control)this).get_RelativeMousePosition().Y >= ((Control)tabsControl).get_Bottom() || ((Control)this).get_RelativeMousePosition().X <= ((Control)tabsControl).get_Left() || ((Control)this).get_RelativeMousePosition().X >= ((Control)tabsControl).get_Right()) && (int)e.get_EventType() == 514 && MouseOverItemDetails && ChildNodes.Any() && Expandable)
			{
				this.OnPanelClick?.Invoke(this, e);
				Toggle();
				ServiceContainer.AudioService.PlayMenuClick();
			}
		}

		protected override void OnRightMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (MouseOverItemDetails)
			{
				ContextMenuStrip menuStrip = MenuStrip;
				if (menuStrip != null)
				{
					menuStrip.Show(e.get_MousePosition());
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (ShowBackground)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), PanelRectangle, BackgroundOpaqueColor * BackgroundOpacity);
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
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawPanelExtension(SpriteBatch spriteBatch)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (Expanded)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, PanelCollapsedHeight, ((Control)this).get_Width(), PanelExtensionHeight), Color.get_Black() * 0.05f);
			}
		}

		private void DrawArrow(SpriteBatch spriteBatch)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (ChildNodes.Any())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.Textures.TextureArrow), new Rectangle(15, PanelCollapsedHeight / 2, 32, 32), (Rectangle?)null, Color.get_White(), ArrowRotation, new Vector2(8f, 16f), (SpriteEffects)0);
			}
		}

		private void DrawFrame(SpriteBatch spriteBatch)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			Color lineColor = FrameColor * 0.5f;
			int lineSize = 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.X, 0, PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.X, PanelCollapsedHeight - lineSize, PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, lineSize, PanelRectangle.Height + lineSize * 2), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.Width - lineSize, 0, lineSize, PanelCollapsedHeight), lineColor);
		}
	}
}
