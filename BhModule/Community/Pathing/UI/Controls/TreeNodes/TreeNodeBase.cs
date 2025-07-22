using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BhModule.Community.Pathing.UI.Controls.TreeView;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls.TreeNodes
{
	public abstract class TreeNodeBase : Container
	{
		public bool DevMode;

		private AsyncTexture2D _textureArrow = AsyncTexture2D.FromAssetId(155909);

		public Color BackgroundOpaqueColor = Color.get_Black();

		public float BackgroundOpacity = 0.3f;

		protected float ArrowOpacity = 1f;

		public Color HighlightColor = Color.get_LightYellow();

		public BhModule.Community.Pathing.UI.Controls.TreeView.TreeView TreeView { get; protected set; }

		protected int NodeDepth { get; set; }

		public bool Expanded { get; set; }

		public bool Expandable { get; set; } = true;


		public bool Clickable { get; set; }

		public bool ShowBackground { get; set; } = true;


		public bool MouseOverItemDetails { get; set; }

		public int PaddingLeft { get; set; } = 14;


		public int PanelHeight { get; set; }

		public Rectangle PanelRectangle => new Rectangle(2, 2, ((Control)this).get_Width(), PanelHeight - 4);

		public float ArrowRotation { get; set; }

		public bool Highlighted { get; set; }

		public string Name { get; set; }

		public IList<TreeNodeBase> ChildBaseNodes { get; } = new List<TreeNodeBase>();


		private Tween SlideAnimation { get; set; }

		public event EventHandler<MouseEventArgs> OnPanelClick;

		protected TreeNodeBase()
			: this()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)OnPropertyChanged);
		}

		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Parent", StringComparison.InvariantCultureIgnoreCase))
			{
				OnParentChanged();
			}
		}

		protected virtual void OnParentChanged()
		{
			if (((Control)this).get_Parent() == null)
			{
				return;
			}
			Container parent = ((Control)this).get_Parent();
			BhModule.Community.Pathing.UI.Controls.TreeView.TreeView treeView = parent as BhModule.Community.Pathing.UI.Controls.TreeView.TreeView;
			if (treeView == null)
			{
				TreeNodeBase parentNode = parent as TreeNodeBase;
				if (parentNode != null)
				{
					TreeView = parentNode.TreeView;
					((Control)this).set_Visible(parentNode.Expanded);
					NodeDepth = parentNode.NodeDepth + 1;
				}
			}
			else
			{
				TreeView = treeView;
				((Control)this).set_Visible(true);
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			TreeNodeBase newChild = e.get_ChangedChild() as TreeNodeBase;
			if (newChild != null)
			{
				TreeView?.AddNode(newChild);
				ChildBaseNodes?.Add(newChild);
				ReflowChildLayout(ChildBaseNodes);
				((Container)this).OnChildAdded(e);
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			TreeNodeBase removedChild = e.get_ChangedChild() as TreeNodeBase;
			if (removedChild != null)
			{
				if (TreeView != null)
				{
					TreeView.RemoveNode(removedChild);
				}
				ChildBaseNodes.Remove(removedChild);
			}
			ReflowChildLayout(ChildBaseNodes);
			((Container)this).OnChildRemoved(e);
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			if (!MouseOverItemDetails)
			{
				ContextMenuStrip menu = ((Control)this).get_Menu();
				if (menu != null && ((Control)menu).get_Visible())
				{
					((Control)((Control)this).get_Menu()).Hide();
				}
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			MouseOverItemDetails = ((Control)this).get_RelativeMousePosition().Y <= PanelHeight;
			if ((MouseOverItemDetails && ChildBaseNodes.Count > 0 && Expandable) || Clickable)
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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			int lastBottom = PanelHeight;
			foreach (TreeNodeBase containerChild in containerChildren)
			{
				((Control)containerChild).set_Location(new Point(PaddingLeft, lastBottom));
				((Control)containerChild).set_Width(((Control)this).get_Width() - PaddingLeft);
				lastBottom = ((Control)containerChild).get_Bottom();
			}
			return lastBottom + 5;
		}

		public void ShowChildren()
		{
			foreach (TreeNodeBase childBaseNode in ChildBaseNodes)
			{
				((Control)childBaseNode).Show();
			}
		}

		public void HideChildren()
		{
			foreach (TreeNodeBase childBaseNode in ChildBaseNodes)
			{
				((Control)childBaseNode).Hide();
			}
		}

		public void Toggle()
		{
			if (Expanded)
			{
				Collapse();
			}
			else
			{
				Expand();
			}
		}

		public void Expand()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (!Expanded)
			{
				Expanded = true;
				ShowChildren();
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				Animate(((Rectangle)(ref contentRegion)).get_Bottom());
				((Control)this).RecalculateLayout();
			}
		}

		public void Collapse()
		{
			if (Expanded)
			{
				Expanded = false;
				HideChildren();
				Animate(PanelHeight);
				((Control)this).RecalculateLayout();
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
			BhModule.Community.Pathing.UI.Controls.TreeView.TreeView list = ((Control)this).get_Parent() as BhModule.Community.Pathing.UI.Controls.TreeView.TreeView;
			if (list != null)
			{
				((Control)list).RecalculateLayout();
			}
		}

		public void UpdateContentRegion()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			List<TreeNodeBase> nodes = ChildBaseNodes.Where((TreeNodeBase n) => ((Control)n).get_Visible()).ToList();
			int bottomChild = ReflowChildLayout(nodes);
			((Container)this).set_ContentRegion(nodes.Any() ? new Rectangle(0, 0, ((Control)this).get_Width(), bottomChild) : new Rectangle(0, 0, ((Control)this).get_Width(), PanelHeight));
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

		public void ClearChildNodes()
		{
			if (ChildBaseNodes.Count > 0)
			{
				Queue<Control> controlsQueue = new Queue<Control>((IEnumerable<Control>)ChildBaseNodes);
				while (controlsQueue.Count > 0)
				{
					Control obj = controlsQueue.Dequeue();
					obj.set_Parent((Container)null);
					obj.Dispose();
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (MouseOverItemDetails)
			{
				if ((int)e.get_EventType() == 514 && MouseOverItemDetails && ChildBaseNodes.Count > 0 && Expandable)
				{
					this.OnPanelClick?.Invoke(this, e);
					Toggle();
					Control.get_Content().PlaySoundEffectByName($"tab-swap-{RandomUtil.GetRandom(1, 5)}");
				}
				((Control)this).OnClick(e);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (ShowBackground || Highlighted)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), PanelRectangle, BackgroundOpaqueColor * BackgroundOpacity);
			}
			if (Highlighted)
			{
				DrawFrame(spriteBatch);
			}
			if (Expandable)
			{
				DrawArrow(spriteBatch);
			}
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawArrow(SpriteBatch spriteBatch)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (ChildBaseNodes.Any())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureArrow), new Rectangle(15, PanelHeight / 2, 32, 32), (Rectangle?)null, Color.get_White() * ArrowOpacity, ArrowRotation, new Vector2(8f, 16f), (SpriteEffects)0);
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
			Color lineColor = HighlightColor * 0.5f;
			int lineSize = 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.X, 0, PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.X, PanelHeight - lineSize, PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, lineSize, PanelRectangle.Height + lineSize * 2), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(PanelRectangle.Width - lineSize, 0, lineSize, PanelHeight), lineColor);
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_PropertyChanged((PropertyChangedEventHandler)OnPropertyChanged);
			SlideAnimation = null;
			TreeView = null;
			this.OnPanelClick = null;
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			ContextMenuStrip menu = ((Control)this).get_Menu();
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
