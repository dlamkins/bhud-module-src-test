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

		protected bool _locationLock;

		protected bool _mouseDragging;

		protected Point _dragStart = Point.get_Zero();

		private Dictionary<Control, Tween> _childTweens = new Dictionary<Control, Tween>();

		private const string TOOLTIP_TEXT = "Drag to move alert container.\nYou can lock it in place by going to the settings panel.";

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

		public bool LocationLock
		{
			get
			{
				return _locationLock;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<bool>(ref _locationLock, value, false, "LocationLock"))
				{
					((Control)this).set_BackgroundColor((Color)(_locationLock ? Color.get_Transparent() : new Color(Color.get_Black(), 0.3f)));
					((Control)this).set_BasicTooltipText(_locationLock ? "" : "Drag to move alert container.\nYou can lock it in place by going to the settings panel.");
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
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Location(args.get_NewValue());
			});
			((Control)this).set_BasicTooltipText(_locationLock ? "" : "Drag to move alert container.\nYou can lock it in place by going to the settings panel.");
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
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			int newWidth = (int)((float)(((Container)this)._children.get_Count() * ((Container)this)._children.get_Item(0).get_Width()) + (float)(((Container)this)._children.get_Count() - 1) * _controlPadding.X + outerPadX * 2f);
			int newHeight = (int)((float)(((Container)this)._children.get_Count() * ((Container)this)._children.get_Item(0).get_Height()) + (float)(((Container)this)._children.get_Count() - 1) * _controlPadding.Y + outerPadY * 2f);
			Point previousSize = ((Control)this).get_Size();
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
			RecalculateLocation();
			if (FlowDirection == AlertFlowDirection.RightToLeft)
			{
				foreach (Control child in ((Container)this)._children)
				{
					child.set_Right(child.get_Right() + (((Control)this).get_Size().X - previousSize.X));
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
					child2.set_Bottom(child2.get_Bottom() + (((Control)this).get_Size().Y - previousSize.Y));
				}
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
			if (((Container)this)._children.get_Count() == 0 || TimersModule.ModuleInstance._hideAlertsSetting.get_Value())
			{
				((Control)this).Hide();
				return;
			}
			((Control)this).Show();
			UpdateSizeToFitChildren();
			ReflowChildLayout(((Container)this)._children.ToArray());
			((Panel)this).RecalculateLayout();
		}

		protected void RecalculateLocation()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			switch (TimersModule.ModuleInstance._alertDisplayOrientationSetting.get_Value())
			{
			case AlertFlowDirection.LeftToRight:
			case AlertFlowDirection.TopToBottom:
				((Control)this).set_Location(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value());
				break;
			case AlertFlowDirection.RightToLeft:
				((Control)this).set_Right(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().X + TimersModule.ModuleInstance._alertContainerSizeSetting.get_Value().X);
				((Control)this).set_Top(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().Y);
				break;
			case AlertFlowDirection.BottomToTop:
				((Control)this).set_Left(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().X);
				((Control)this).set_Bottom(TimersModule.ModuleInstance._alertContainerLocationSetting.get_Value().Y + TimersModule.ModuleInstance._alertContainerSizeSetting.get_Value().Y);
				break;
			}
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (LocationLock || !((Control)this)._visible)
			{
				return (CaptureType)22;
			}
			return ((Container)this).CapturesInput();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!LocationLock)
			{
				_dragStart = ((Control)this).get_RelativeMousePosition();
				_mouseDragging = true;
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		public void HandleLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TimersModule.ModuleInstance != null)
			{
				if (_mouseDragging)
				{
					TimersModule.ModuleInstance._alertContainerLocationSetting.set_Value(((Control)this).get_Location());
					TimersModule.ModuleInstance._alertContainerSizeSetting.set_Value(((Control)this).get_Size());
					_mouseDragging = false;
				}
				ContainerDragged?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (_mouseDragging)
			{
				Point newLocation = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(newLocation);
				_dragStart = ((Control)this).get_RelativeMousePosition();
			}
		}
	}
}
