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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _flowDirection;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<ControlFlowDirection>(ref _flowDirection, value, true, "FlowDirection");
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
				((Control)this).SetProperty<Vector2>(ref _controlPadding, value, true, "ControlPadding");
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
				((Control)this).SetProperty<Vector2>(ref _outerControlPadding, value, true, "OuterControlPadding");
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			ReflowChildLayout(((Container)this)._children.ToArray());
		}

		private void ReflowChildLayout(IEnumerable<Control> allChildren)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected I4, but got Unknown
			IEnumerable<Control> filteredChildren = allChildren.Where((Control c) => ((object)c).GetType() != typeof(Scrollbar) && c.get_Visible());
			ControlFlowDirection flowDirection = _flowDirection;
			switch ((int)flowDirection)
			{
			case 0:
				ReflowChildLayoutLeftToRight(filteredChildren);
				break;
			case 4:
				ReflowChildLayoutRightToLeft(filteredChildren);
				break;
			case 1:
				ReflowChildLayoutTopToBottom(filteredChildren);
				break;
			case 6:
				ReflowChildLayoutBottomToTop(filteredChildren);
				break;
			case 2:
				ReflowChildLayoutSingleLeftToRight(filteredChildren);
				break;
			case 5:
				ReflowChildLayoutSingleRightToLeft(filteredChildren);
				break;
			case 3:
				ReflowChildLayoutSingleTopToBottom(filteredChildren);
				break;
			case 7:
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
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if ((float)child.get_Width() >= (float)((Container)this).get_ContentRegion().Width - lastRight)
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastRight = outerPadX;
				}
				child.set_Location(new Point((int)lastRight, (int)currentBottom));
				lastRight = (float)child.get_Right() + _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.get_Bottom());
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
			float lastLeft = (float)((Container)this).get_ContentRegion().Width - outerPadX;
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if (outerPadX > lastLeft - (float)child.get_Width())
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastLeft = (float)((Container)this).get_ContentRegion().Width - outerPadX;
				}
				child.set_Location(new Point((int)(lastLeft - (float)child.get_Width()), (int)currentBottom));
				lastLeft = (float)child.get_Left() - _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.get_Bottom());
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
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if ((float)child.get_Height() >= (float)((Control)this).get_Height() - lastBottom)
				{
					currentRight = nextRight + _controlPadding.X;
					lastBottom = outerPadY;
				}
				child.set_Location(new Point((int)currentRight, (int)lastBottom));
				lastBottom = (float)child.get_Bottom() + _controlPadding.Y;
				nextRight = Math.Max(nextRight, child.get_Right());
			}
		}

		private void ReflowChildLayoutBottomToTop(IEnumerable<Control> allChildren)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			float x = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float nextRight = x;
			float currentRight = x;
			float lastTop = (float)((Control)this).get_Height() - outerPadY;
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if (outerPadY > lastTop - (float)child.get_Height())
				{
					currentRight = nextRight + _controlPadding.X;
					lastTop = (float)((Control)this).get_Height() - outerPadY;
				}
				child.set_Location(new Point((int)currentRight, (int)(lastTop - (float)child.get_Height())));
				lastTop = (float)child.get_Top() - _controlPadding.Y;
				nextRight = Math.Max(nextRight, child.get_Right());
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
				allChild.set_Location(new Point((int)lastLeft, (int)outerPadY));
				lastLeft = (float)allChild.get_Right() + _controlPadding.X;
			}
		}

		private void ReflowChildLayoutSingleRightToLeft(IEnumerable<Control> allChildren)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float lastLeft = (float)((Container)this).get_ContentRegion().Width - outerPadX;
			foreach (Control child in allChildren)
			{
				child.set_Location(new Point((int)(lastLeft - (float)child.get_Width()), (int)outerPadY));
				lastLeft = (float)child.get_Left() - _controlPadding.X;
			}
		}

		private void ReflowChildLayoutSingleTopToBottom(IEnumerable<Control> allChildren)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float lastBottom = _outerControlPadding.Y;
			foreach (Control allChild in allChildren)
			{
				allChild.set_Location(new Point((int)outerPadX, (int)lastBottom));
				lastBottom = (float)allChild.get_Bottom() + _controlPadding.Y;
			}
		}

		private void ReflowChildLayoutSingleBottomToTop(IEnumerable<Control> allChildren)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = _outerControlPadding.X;
			float outerPadY = _outerControlPadding.Y;
			float lastTop = (float)((Control)this).get_Height() - outerPadY;
			foreach (Control child in allChildren)
			{
				child.set_Location(new Point((int)outerPadX, (int)(lastTop - (float)child.get_Height())));
				lastTop = (float)child.get_Top() - _controlPadding.Y;
			}
		}
	}
}
