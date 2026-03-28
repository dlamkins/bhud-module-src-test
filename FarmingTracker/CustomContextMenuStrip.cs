using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarmingTracker
{
	public class CustomContextMenuStrip : Container
	{
		private const int BORDER_PADDING = 2;

		private const int ITEM_WIDTH = 135;

		private const int ITEM_HEIGHT = 22;

		private const int ITEM_VERTICALMARGIN = 6;

		private const int CONTROL_WIDTH = 139;

		private const int CLICK_DEBOUNCE = 100;

		private static readonly List<WeakReference<CustomContextMenuStrip>> _contextMenuStrips;

		private static readonly Texture2D _textureMenuEdge;

		private static double _lastOpenTime;

		private (Point Position, int DownOffset, int UpOffset) _targetOffset;

		static CustomContextMenuStrip()
		{
			_contextMenuStrips = new List<WeakReference<CustomContextMenuStrip>>();
			_textureMenuEdge = Control.get_Content().GetTexture("scrollbar-track");
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)HandleMouseButtonPressed);
			Control.get_Input().get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)HandleMouseButtonPressed);
		}

		private static void RegisterContextMenuStrip(CustomContextMenuStrip contextMenuStrip)
		{
			lock (_contextMenuStrips)
			{
				_contextMenuStrips.Add(new WeakReference<CustomContextMenuStrip>(contextMenuStrip));
			}
		}

		private static void HandleMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _lastOpenTime < 100.0)
			{
				return;
			}
			lock (_contextMenuStrips)
			{
				WeakReference<CustomContextMenuStrip>[] allMenuStrips = _contextMenuStrips.ToArray();
				if (Control.get_Input().get_Mouse().get_ActiveControl() is CustomContextMenuStrip)
				{
					return;
				}
				WeakReference<CustomContextMenuStrip>[] array = allMenuStrips;
				foreach (WeakReference<CustomContextMenuStrip> cmsRef in array)
				{
					if (!cmsRef.TryGetTarget(out var cms))
					{
						_contextMenuStrips.Remove(cmsRef);
					}
					else if (((Control)cms).get_Visible() && !((Control)cms).get_MouseOver())
					{
						((Control)cms).Hide();
					}
				}
			}
		}

		public CustomContextMenuStrip()
			: this()
		{
			((Control)this).set_Visible(false);
			((Control)this).set_Width(139);
			((Control)this).set_ZIndex(2147483583);
			RegisterContextMenuStrip(this);
		}

		protected override void OnShown(EventArgs e)
		{
			_lastOpenTime = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			if (base._children.get_IsEmpty())
			{
				((Control)this).set_Visible(false);
			}
			else
			{
				((Control)this).OnShown(e);
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).set_Parent((Container)null);
			((Control)this).OnHidden(e);
		}

		private int GetVerticalOffset(int yStart, int downOffset = 0, int upOffset = 0)
		{
			int yUnderDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Bottom() - (yStart + ((Control)this)._size.Y);
			int yAboveDef = ((Control)Control.get_Graphics().get_SpriteScreen()).get_Top() + (yStart - ((Control)this)._size.Y);
			if (yUnderDef <= 0 && yUnderDef <= yAboveDef)
			{
				return yStart - ((Control)this)._size.Y + downOffset;
			}
			return yStart + upOffset;
		}

		private void SetPositionFromOffset((Point Position, int DownOffset, int UpOffset) offset)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(offset.Position.X, GetVerticalOffset(offset.Position.Y, offset.DownOffset, offset.UpOffset)));
		}

		public void Show(Point position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			SetPositionFromOffset(_targetOffset = (position, 0, 0));
			((Control)this).Show();
		}

		public void Show(Control activeControl)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			CustomContextMenuStripItem parentMenu = activeControl as CustomContextMenuStripItem;
			Rectangle absoluteBounds;
			if (parentMenu != null)
			{
				absoluteBounds = ((Control)parentMenu).get_AbsoluteBounds();
				int num = ((Rectangle)(ref absoluteBounds)).get_Right() - 3;
				absoluteBounds = ((Control)parentMenu).get_AbsoluteBounds();
				SetPositionFromOffset(_targetOffset = (new Point(num, ((Rectangle)(ref absoluteBounds)).get_Top()), 19, 0));
				((Control)this).set_ZIndex(((Control)((Control)parentMenu).get_Parent()).get_ZIndex() + 1);
			}
			else
			{
				absoluteBounds = activeControl.get_AbsoluteBounds();
				SetPositionFromOffset(_targetOffset = (((Rectangle)(ref absoluteBounds)).get_Location(), 0, activeControl.get_Height()));
			}
			((Control)this).Show();
		}

		public override void Hide()
		{
			((Control)this).set_Visible(false);
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			((Container)this).OnChildAdded(e);
			OnChildMembershipChanged(e);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			((Container)this).OnChildRemoved(e);
			OnChildMembershipChanged(e);
		}

		private void OnChildMembershipChanged(ChildChangedEventArgs e)
		{
			if (e.get_Added())
			{
				CustomContextMenuStripItem newChild = e.get_ChangedChild() as CustomContextMenuStripItem;
				if (newChild == null)
				{
					((CancelEventArgs)(object)e).Cancel = true;
					return;
				}
				((Control)newChild).set_Height(22);
				((Control)newChild).add_Resized((EventHandler<ResizedEventArgs>)ChildOnResized);
			}
			else
			{
				e.get_ChangedChild().remove_Resized((EventHandler<ResizedEventArgs>)ChildOnResized);
			}
			if (((Control)this).get_Visible())
			{
				SetPositionFromOffset(_targetOffset);
			}
			((Control)this).Invalidate();
		}

		private void ChildOnResized(object sender, ResizedEventArgs e)
		{
			((Control)this).Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (base._children.get_IsEmpty())
			{
				return;
			}
			int maxChildWidth = 139;
			int lastChildBottom = -4;
			foreach (Control item in ((IEnumerable<Control>)base._children).Where((Control c) => c.get_Visible()))
			{
				maxChildWidth = Math.Max(item.get_Width(), maxChildWidth);
				item.set_Location(new Point(2, lastChildBottom + 6));
				lastChildBottom = item.get_Bottom();
			}
			((Control)this)._size = new Point(maxChildWidth + 4, lastChildBottom + 2);
			foreach (Control child in ((Container)this).get_Children())
			{
				child.set_Width(maxChildWidth);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, 2, ((Control)this)._size.X - 4, ((Control)this)._size.Y - 4), Color.FromNonPremultiplied(33, 32, 33, 255));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureMenuEdge, new Rectangle(0, 1, _textureMenuEdge.get_Width(), ((Control)this)._size.Y - 2), (Rectangle?)new Rectangle(0, 1, _textureMenuEdge.get_Width(), ((Control)this)._size.Y - 2), Color.get_White() * 0.8f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureMenuEdge, new Rectangle(1, 2, _textureMenuEdge.get_Width(), ((Control)this)._size.X - 2), (Rectangle?)new Rectangle(1, 2, _textureMenuEdge.get_Width(), ((Control)this)._size.X - 2), Color.get_White() * 0.8f, -(float)Math.PI / 2f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureMenuEdge, new Rectangle(1, ((Control)this)._size.Y, _textureMenuEdge.get_Width(), ((Control)this)._size.X - 2), (Rectangle?)new Rectangle(1, 2, _textureMenuEdge.get_Width(), ((Control)this)._size.X - 2), Color.get_White() * 0.8f, -(float)Math.PI / 2f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureMenuEdge, new Rectangle(((Control)this)._size.X - _textureMenuEdge.get_Width(), 1, _textureMenuEdge.get_Width(), ((Control)this)._size.Y - 2), (Rectangle?)new Rectangle(0, 1, _textureMenuEdge.get_Width(), ((Control)this)._size.Y - 2), Color.get_White() * 0.8f);
		}
	}
}
