using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.UI.Extensions;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls.TreeView
{
	public class CustomFlowPanel : FlowPanel
	{
		private float _targetScrollDistance;

		private float _scrollTarget;

		public Scrollbar Scrollbar
		{
			get
			{
				Container parent = ((Control)this).get_Parent();
				if (parent == null)
				{
					return null;
				}
				return ((IEnumerable)parent.get_Children()).OfType<Scrollbar>().FirstOrDefault((Scrollbar s) => s.get_AssociatedContainer() == this);
			}
		}

		public float ScrollDistance
		{
			get
			{
				Scrollbar scrollbar = Scrollbar;
				if (scrollbar == null)
				{
					return 0f;
				}
				return scrollbar.get_ScrollDistance();
			}
			set
			{
				if (((Panel)this).get_CanScroll() && Scrollbar != null)
				{
					Scrollbar.set_ScrollDistance(MathHelper.Clamp(value, 0f, 1f));
				}
			}
		}

		public void SetTargetScrollDistance(float distance)
		{
			_scrollTarget = MathHelper.Clamp(distance, 0f, 1f);
		}

		public void SaveScrollDistance(int height)
		{
			float scrollbarDistance = Scrollbar.get_ScrollDistance();
			if (Scrollbar != null && !float.IsNaN(scrollbarDistance))
			{
				_targetScrollDistance = scrollbarDistance * (float)(height - ((Control)Scrollbar).get_Height());
			}
		}

		public void UpdateScrollDistance()
		{
			UpdateScrollDistance(_targetScrollDistance);
		}

		public void UpdateScrollDistance(float target)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if (Scrollbar != null && !float.IsNaN(target))
			{
				List<Control> visibleChildren = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control c) => c.get_Visible()).ToList();
				float scrollRange = (visibleChildren.Any() ? visibleChildren.Max((Control c) => c.get_Bottom()) : 0) + (int)((FlowPanel)this).get_ControlPadding().Y - ((Control)Scrollbar).get_Height();
				if (scrollRange <= 0f)
				{
					Scrollbar.set_ScrollDistance(0f);
					return;
				}
				float distance = target / scrollRange;
				Scrollbar.set_ScrollDistance(MathHelper.Clamp(distance, 0f, 1f));
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			((FlowPanel)this).OnChildAdded(e);
			e.get_ChangedChild().add_Resized((EventHandler<ResizedEventArgs>)ChangedChild_Resized);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			((FlowPanel)this).OnChildRemoved(e);
			e.get_ChangedChild().remove_Resized((EventHandler<ResizedEventArgs>)ChangedChild_Resized);
		}

		private void ChangedChild_Resized(object sender, ResizedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_PreviousSize().Y != e.get_CurrentSize().Y)
			{
				int previousBottomSize = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control c) => c != sender).Sum((Control c) => c.get_Height()) + e.get_PreviousSize().Y - (int)((FlowPanel)this).get_ControlPadding().Y;
				if (((Panel)this).get_CanScroll())
				{
					SaveScrollDistance(previousBottomSize);
				}
				UpdateScrollDistance();
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_targetScrollDistance > 0f)
			{
				UpdateScrollDistance(_targetScrollDistance);
				_targetScrollDistance = 0f;
			}
			if (_scrollTarget > 0f)
			{
				ScrollDistance = _scrollTarget;
				_scrollTarget = 0f;
			}
		}

		public void ScrollToChild(Control child, int paddingTop)
		{
			int childPosition = ((Container)(object)this).ContainsChildPosition(child);
			if (childPosition != -1)
			{
				ScrollToChild(Math.Max(0, childPosition - paddingTop));
			}
		}

		public void ScrollToChild(int childYPosition)
		{
			if (childYPosition < 0)
			{
				return;
			}
			Container parent = ((Control)this).get_Parent();
			Scrollbar scrollbar = ((parent != null) ? ((IEnumerable)parent.get_Children()).OfType<Scrollbar>().FirstOrDefault((Scrollbar s) => s.get_AssociatedContainer() == this) : null);
			if (scrollbar == null)
			{
				return;
			}
			if (childYPosition == 0)
			{
				scrollbar.set_ScrollDistance(0f);
				return;
			}
			List<Control> visibleChildren = ((IEnumerable<Control>)((Container)this).get_Children()).Where((Control c) => c.get_Visible()).ToList();
			if (visibleChildren.Any())
			{
				float scrollRange = (float)visibleChildren.Max((Control c) => c.get_Bottom()) - (float)((Control)scrollbar).get_Height();
				if (scrollRange <= 0f)
				{
					scrollbar.set_ScrollDistance(0f);
					SetTargetScrollDistance(0f);
				}
				else
				{
					float scrollPosition = MathHelper.Clamp((float)childYPosition / scrollRange, 0f, 1f);
					scrollbar.set_ScrollDistance(scrollPosition);
					SetTargetScrollDistance(scrollPosition);
				}
			}
		}

		protected override void DisposeControl()
		{
			foreach (Control child in ((Container)this).get_Children())
			{
				child.remove_Resized((EventHandler<ResizedEventArgs>)ChangedChild_Resized);
			}
			((FlowPanel)this).DisposeControl();
		}

		public CustomFlowPanel()
			: this()
		{
		}
	}
}
