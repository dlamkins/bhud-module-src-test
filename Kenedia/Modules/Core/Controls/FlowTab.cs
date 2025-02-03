using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class FlowTab : PanelTab
	{
		private ControlFlowDirection _flowDirection;

		private Vector2 _outerControlPadding = Vector2.get_Zero();

		private Vector2 _controlPadding = Vector2.get_Zero();

		public ControlFlowDirection FlowDirection
		{
			get
			{
				return _flowDirection;
			}
			set
			{
				SetProperty(ref _flowDirection, value, invalidateLayout: true, "FlowDirection");
			}
		}

		public Vector2 ControlPadding
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _controlPadding;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _controlPadding, value, invalidateLayout: true, "ControlPadding");
			}
		}

		public Vector2 OuterControlPadding
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _outerControlPadding;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetProperty(ref _outerControlPadding, value, invalidateLayout: true, "OuterControlPadding");
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			ReflowChildLayout(_children.ToArray());
		}

		private void ReflowChildLayout(IEnumerable<Control> allChildren)
		{
			IEnumerable<Control> filteredChildren = allChildren.Where((Control c) => c.GetType() != typeof(Scrollbar) && c.Visible);
			switch (_flowDirection)
			{
			case ControlFlowDirection.LeftToRight:
				ReflowChildLayoutLeftToRight(filteredChildren);
				break;
			case ControlFlowDirection.RightToLeft:
				ReflowChildLayoutRightToLeft(filteredChildren);
				break;
			case ControlFlowDirection.TopToBottom:
				ReflowChildLayoutTopToBottom(filteredChildren);
				break;
			case ControlFlowDirection.BottomToTop:
				ReflowChildLayoutBottomToTop(filteredChildren);
				break;
			case ControlFlowDirection.SingleLeftToRight:
				ReflowChildLayoutSingleLeftToRight(filteredChildren);
				break;
			case ControlFlowDirection.SingleRightToLeft:
				ReflowChildLayoutSingleRightToLeft(filteredChildren);
				break;
			case ControlFlowDirection.SingleTopToBottom:
				ReflowChildLayoutSingleTopToBottom(filteredChildren);
				break;
			case ControlFlowDirection.SingleBottomToTop:
				ReflowChildLayoutSingleBottomToTop(filteredChildren);
				break;
			}
		}

		private void ReflowChildLayoutLeftToRight(IEnumerable<Control> allChildren)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float nextBottom;
			float currentBottom = (nextBottom = _outerControlPadding.Y);
			float lastRight = outerPadX;
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if ((float)child.Width >= (float)base.ContentRegion.Width - lastRight)
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastRight = outerPadX;
				}
				child.Location = new Point((int)lastRight, (int)currentBottom);
				lastRight = (float)child.Right + _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.Bottom);
			}
		}

		private void ReflowChildLayoutRightToLeft(IEnumerable<Control> allChildren)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float nextBottom;
			float currentBottom = (nextBottom = _outerControlPadding.Y);
			float lastLeft = (float)base.ContentRegion.Width - outerPadX;
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if (outerPadX > lastLeft - (float)child.Width)
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastLeft = (float)base.ContentRegion.Width - outerPadX;
				}
				child.Location = new Point((int)(lastLeft - (float)child.Width), (int)currentBottom);
				lastLeft = (float)child.Left - _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.Bottom);
			}
		}

		private void ReflowChildLayoutTopToBottom(IEnumerable<Control> allChildren)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			float x = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float nextRight = x;
			float currentRight = x;
			float lastBottom = outerPadY;
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if ((float)child.Height >= (float)base.Height - lastBottom)
				{
					currentRight = nextRight + _controlPadding.X;
					lastBottom = outerPadY;
				}
				child.Location = new Point((int)currentRight, (int)lastBottom);
				lastBottom = (float)child.Bottom + _controlPadding.Y;
				nextRight = Math.Max(nextRight, child.Right);
			}
		}

		private void ReflowChildLayoutBottomToTop(IEnumerable<Control> allChildren)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			float x = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float nextRight = x;
			float currentRight = x;
			float lastTop = (float)base.Height - outerPadY;
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if (outerPadY > lastTop - (float)child.Height)
				{
					currentRight = nextRight + _controlPadding.X;
					lastTop = (float)base.Height - outerPadY;
				}
				child.Location = new Point((int)currentRight, (int)(lastTop - (float)child.Height));
				lastTop = (float)child.Top - _controlPadding.Y;
				nextRight = Math.Max(nextRight, child.Right);
			}
		}

		private void ReflowChildLayoutSingleLeftToRight(IEnumerable<Control> allChildren)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			float x = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float lastLeft = x;
			foreach (Control allChild in allChildren)
			{
				allChild.Location = new Point((int)lastLeft, (int)outerPadY);
				lastLeft = (float)allChild.Right + _controlPadding.X;
			}
		}

		private void ReflowChildLayoutSingleRightToLeft(IEnumerable<Control> allChildren)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float lastLeft = (float)base.ContentRegion.Width - outerPadX;
			foreach (Control child in allChildren)
			{
				child.Location = new Point((int)(lastLeft - (float)child.Width), (int)outerPadY);
				lastLeft = (float)child.Left - _controlPadding.X;
			}
		}

		private void ReflowChildLayoutSingleTopToBottom(IEnumerable<Control> allChildren)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float lastBottom = _outerControlPadding.Y;
			foreach (Control allChild in allChildren)
			{
				allChild.Location = new Point((int)outerPadX, (int)lastBottom);
				lastBottom = (float)allChild.Bottom + _controlPadding.Y;
			}
		}

		private void ReflowChildLayoutSingleBottomToTop(IEnumerable<Control> allChildren)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float lastTop = (float)base.Height - outerPadY;
			foreach (Control child in allChildren)
			{
				child.Location = new Point((int)outerPadX, (int)(lastTop - (float)child.Height));
				lastTop = (float)child.Top - _controlPadding.Y;
			}
		}
	}
}
