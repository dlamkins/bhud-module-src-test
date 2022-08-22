using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;

namespace Charr.Timers_BlishHUD.Controls
{
	public class AlertContainer : Panel
	{
		public EventHandler<EventArgs> ContainerDragged;

		protected Vector2 _controlPadding = Vector2.get_Zero();

		protected Vector2 _outerControlPadding = Vector2.get_Zero();

		protected bool _padLeftBeforeControl;

		protected bool _padTopBeforeControl;

		protected AlertFlowDirection _flowDirection = AlertFlowDirection.TopToBottom;

		protected bool _lock;

		protected bool _mouseDragging;

		protected Point _dragStart = Point.get_Zero();

		private Dictionary<Control, Tween> _childTweens = new Dictionary<Control, Tween>();

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

		[Obsolete("Use OuterControlPadding instead.")]
		public bool PadLeftBeforeControl
		{
			get
			{
				return _padLeftBeforeControl;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _padLeftBeforeControl, value, true, "PadLeftBeforeControl");
			}
		}

		[Obsolete("Use OuterControlPadding instead.")]
		public bool PadTopBeforeControl
		{
			get
			{
				return _padTopBeforeControl;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _padTopBeforeControl, value, true, "PadTopBeforeControl");
			}
		}

		public AlertFlowDirection FlowDirection
		{
			get
			{
				return _flowDirection;
			}
			set
			{
				((Control)this).SetProperty<AlertFlowDirection>(ref _flowDirection, value, true, "FlowDirection");
			}
		}

		public bool Lock
		{
			get
			{
				return _lock;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<bool>(ref _lock, value, false, "Lock"))
				{
					((Control)this).set_BackgroundColor((Color)(_lock ? Color.get_Transparent() : new Color(Color.get_Black(), 0.3f)));
				}
			}
		}

		public AlertContainer()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				HandleLeftMouseButtonReleased(args);
			});
			TimersModule.ModuleInstance._alertContainerLocationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)delegate(object sender, ValueChangedEventArgs<Point> args)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).get_Location() != args.get_NewValue())
				{
					((Control)this).set_Location(args.get_NewValue());
				}
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate(object sender, ResizedEventArgs args)
			{
				HandleResized(args);
			});
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			OnChildrenChanged(e.get_ResultingChildren());
			((Panel)this).OnChildAdded(e);
			e.get_ChangedChild().add_Resized((EventHandler<ResizedEventArgs>)ChangedChildOnResized);
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			e.get_ChangedChild().set_Top((int)outerPadY);
			e.get_ChangedChild().set_Left((int)outerPadX);
			switch (FlowDirection)
			{
			case AlertFlowDirection.LeftToRight:
				e.get_ChangedChild().set_Left(((Control)this).get_Width() + e.get_ChangedChild().get_Width());
				break;
			case AlertFlowDirection.RightToLeft:
				e.get_ChangedChild().set_Right(-e.get_ChangedChild().get_Width());
				break;
			case AlertFlowDirection.TopToBottom:
				e.get_ChangedChild().set_Top(((Control)this).get_Height() + e.get_ChangedChild().get_Height());
				break;
			case AlertFlowDirection.BottomToTop:
				e.get_ChangedChild().set_Bottom(-e.get_ChangedChild().get_Height());
				break;
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			OnChildrenChanged(e.get_ResultingChildren());
			((Panel)this).OnChildRemoved(e);
			e.get_ChangedChild().remove_Resized((EventHandler<ResizedEventArgs>)ChangedChildOnResized);
		}

		private void ChangedChildOnResized(object sender, ResizedEventArgs e)
		{
			OnChildrenChanged(((Container)this)._children.ToArray());
		}

		private void OnChildrenChanged(IEnumerable<Control> resultingChildren)
		{
			ReflowChildLayout(resultingChildren.ToArray());
		}

		public void FilterChildren<TControl>(Func<TControl, bool> filter) where TControl : Control
		{
			((IEnumerable)((Container)this)._children).Cast<TControl>().ToList().ForEach(delegate(TControl tc)
			{
				((Control)tc).set_Visible(filter(tc));
			});
			((Control)this).Invalidate();
		}

		public void SortChildren<TControl>(Comparison<TControl> comparison) where TControl : Control
		{
			List<TControl> tempChildren = ((IEnumerable)((Container)this)._children).Cast<TControl>().ToList();
			tempChildren.Sort(comparison);
			((Container)this)._children = new ControlCollection<Control>((IEnumerable<Control>)tempChildren);
			((Control)this).Invalidate();
		}

		private void UpdateSizeToFitChildren()
		{
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			int newWidth = (int)((float)(((Container)this)._children.get_Count() * ((Container)this)._children.get_Item(0).get_Width()) + (float)(((Container)this)._children.get_Count() - 1) * _controlPadding.X + outerPadX * 2f);
			int newHeight = (int)((float)(((Container)this)._children.get_Count() * ((Container)this)._children.get_Item(0).get_Height()) + (float)(((Container)this)._children.get_Count() - 1) * _controlPadding.Y + outerPadY * 2f);
			if (FlowDirection == AlertFlowDirection.LeftToRight || FlowDirection == AlertFlowDirection.RightToLeft)
			{
				((Control)this).set_Width(newWidth);
				((Control)this).set_Height((int)((float)((Container)this)._children.get_Item(0).get_Height() + outerPadY * 2f));
			}
			else if (FlowDirection == AlertFlowDirection.TopToBottom || FlowDirection == AlertFlowDirection.BottomToTop)
			{
				((Control)this).set_Width((int)((float)((Container)this)._children.get_Item(0).get_Width() + outerPadX * 2f));
				((Control)this).set_Height(newHeight);
			}
		}

		private void ReflowChildLayout(Control[] allChildren)
		{
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			int directionX = 0;
			int directionY = 0;
			if (FlowDirection == AlertFlowDirection.LeftToRight || FlowDirection == AlertFlowDirection.RightToLeft)
			{
				directionX = ((FlowDirection == AlertFlowDirection.LeftToRight) ? 1 : (-1));
			}
			if (FlowDirection == AlertFlowDirection.TopToBottom || FlowDirection == AlertFlowDirection.BottomToTop)
			{
				directionY = ((FlowDirection == AlertFlowDirection.TopToBottom) ? 1 : (-1));
			}
			float startLocationX = ((FlowDirection != AlertFlowDirection.RightToLeft) ? outerPadX : ((float)((Control)this).get_Width() - outerPadX));
			float num = ((FlowDirection != AlertFlowDirection.BottomToTop) ? outerPadY : ((float)((Control)this).get_Height() - outerPadY));
			float nextLocationX = startLocationX;
			float nextLocationY = num;
			foreach (Control child in allChildren)
			{
				if (_childTweens.TryGetValue(child, out var childTween))
				{
					childTween.Cancel();
					_childTweens.Remove(child);
				}
				object tweenValue = new { };
				switch (FlowDirection)
				{
				case AlertFlowDirection.LeftToRight:
					tweenValue = new
					{
						Left = (int)nextLocationX
					};
					child.set_Top((int)nextLocationY);
					break;
				case AlertFlowDirection.RightToLeft:
					tweenValue = new
					{
						Right = (int)nextLocationX
					};
					child.set_Top((int)nextLocationY);
					break;
				case AlertFlowDirection.TopToBottom:
					tweenValue = new
					{
						Top = (int)nextLocationY
					};
					child.set_Left((int)nextLocationX);
					break;
				case AlertFlowDirection.BottomToTop:
					tweenValue = new
					{
						Bottom = (int)nextLocationY
					};
					child.set_Left((int)nextLocationX);
					break;
				}
				childTween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Control>(child, tweenValue, TimersModule.ModuleInstance._alertMoveDelaySetting.get_Value(), 0f, true);
				childTween.OnComplete((Action)delegate
				{
					_childTweens.Remove(child);
				});
				_childTweens.Add(child, childTween);
				nextLocationX += ((float)child.get_Width() + _controlPadding.X) * (float)directionX;
				nextLocationY += ((float)child.get_Height() + _controlPadding.Y) * (float)directionY;
			}
		}

		public override void RecalculateLayout()
		{
			if (((Container)this)._children.get_Count() != 0)
			{
				UpdateSizeToFitChildren();
				ReflowChildLayout(((Container)this)._children.ToArray());
				((Panel)this).RecalculateLayout();
			}
		}

		protected void HandleResized(ResizedEventArgs args)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			switch (TimersModule.ModuleInstance._alertDisplayOrientationSetting.get_Value())
			{
			case AlertFlowDirection.LeftToRight:
			case AlertFlowDirection.TopToBottom:
				((Control)this).set_Left(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().X);
				((Control)this).set_Top(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().Y);
				break;
			case AlertFlowDirection.RightToLeft:
				((Control)this).set_Right(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().X);
				((Control)this).set_Top(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().Y);
				break;
			case AlertFlowDirection.BottomToTop:
				((Control)this).set_Left(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().X);
				((Control)this).set_Bottom(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().Y);
				break;
			}
			if (FlowDirection == AlertFlowDirection.RightToLeft)
			{
				foreach (Control child in ((Container)this)._children)
				{
					child.set_Right(child.get_Right() + (args.get_CurrentSize().X - args.get_PreviousSize().X));
				}
			}
			else
			{
				if (FlowDirection != AlertFlowDirection.BottomToTop)
				{
					return;
				}
				foreach (Control child2 in ((Container)this)._children)
				{
					child2.set_Bottom(child2.get_Bottom() + (args.get_CurrentSize().Y - args.get_PreviousSize().Y));
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (Lock || !((Control)this).get_Visible())
			{
				return (CaptureType)22;
			}
			return ((Container)this).CapturesInput();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!Lock)
			{
				_dragStart = ((Control)this).get_RelativeMousePosition();
				_mouseDragging = true;
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		public void HandleLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			if (TimersModule.ModuleInstance == null)
			{
				return;
			}
			if (_mouseDragging)
			{
				switch (TimersModule.ModuleInstance._alertDisplayOrientationSetting.get_Value())
				{
				case AlertFlowDirection.LeftToRight:
				case AlertFlowDirection.TopToBottom:
					TimersModule.ModuleInstance._alertContainerLocationSetting.set_Value(((Control)this).get_Location());
					break;
				case AlertFlowDirection.RightToLeft:
					TimersModule.ModuleInstance._alertContainerLocationSetting.set_Value(new Point(((Control)this).get_Right(), ((Control)this).get_Top()));
					break;
				case AlertFlowDirection.BottomToTop:
					TimersModule.ModuleInstance._alertContainerLocationSetting.set_Value(new Point(((Control)this).get_Left(), ((Control)this).get_Bottom()));
					break;
				}
				_mouseDragging = false;
			}
			ContainerDragged?.Invoke(this, EventArgs.Empty);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (_mouseDragging)
			{
				Point newLocation = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				_ = newLocation - ((Control)this).get_Location();
				((Control)this).set_Location(newLocation);
				_dragStart = ((Control)this).get_RelativeMousePosition();
			}
		}
	}
}
