using System;
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

		[Obsolete("Use OuterControlPadding instead.")]
		public bool PadLeftBeforeControl
		{
			get
			{
				return _padLeftBeforeControl;
			}
			set
			{
				SetProperty(ref _padLeftBeforeControl, value, invalidateLayout: true, "PadLeftBeforeControl");
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
				SetProperty(ref _padTopBeforeControl, value, invalidateLayout: true, "PadTopBeforeControl");
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
				SetProperty(ref _flowDirection, value, invalidateLayout: true, "FlowDirection");
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
				if (SetProperty(ref _locationLock, value, invalidateLayout: false, "LocationLock"))
				{
					base.BackgroundColor = (Color)(_locationLock ? Color.get_Transparent() : new Color(Color.get_Black(), 0.3f));
					base.BasicTooltipText = (_locationLock ? "" : "Drag to move alert container.\nYou can lock it in place by going to the settings panel.");
				}
			}
		}

		public AlertContainer()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			GameService.Input.Mouse.LeftMouseButtonReleased += delegate(object sender, MouseEventArgs args)
			{
				HandleLeftMouseButtonReleased(args);
			};
			TimersModule.ModuleInstance._alertContainerLocationSetting.SettingChanged += delegate(object sender, ValueChangedEventArgs<Point> args)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				base.Location = args.NewValue;
			};
			TimersModule.ModuleInstance._hideAlertsSetting.SettingChanged += delegate
			{
				if (!TimersModule.ModuleInstance._hideAlertsSetting.Value && _children.Count > 0)
				{
					Show();
				}
				else
				{
					Hide();
				}
			};
			base.BasicTooltipText = (_locationLock ? "" : "Drag to move alert container.\nYou can lock it in place by going to the settings panel.");
			base.Visible = false;
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			OnChildrenChanged(e.ResultingChildren);
			base.OnChildAdded(e);
			e.ChangedChild.Resized += ChangedChildOnResized;
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			e.ChangedChild.Top = (int)outerPadY;
			e.ChangedChild.Left = (int)outerPadX;
			switch (FlowDirection)
			{
			case AlertFlowDirection.LeftToRight:
				e.ChangedChild.Left = base.Width + e.ChangedChild.Width;
				break;
			case AlertFlowDirection.RightToLeft:
				e.ChangedChild.Right = -e.ChangedChild.Width;
				break;
			case AlertFlowDirection.TopToBottom:
				e.ChangedChild.Top = base.Height + e.ChangedChild.Height;
				break;
			case AlertFlowDirection.BottomToTop:
				e.ChangedChild.Bottom = -e.ChangedChild.Height;
				break;
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			OnChildrenChanged(e.ResultingChildren);
			base.OnChildRemoved(e);
			e.ChangedChild.Resized -= ChangedChildOnResized;
		}

		private void ChangedChildOnResized(object sender, ResizedEventArgs e)
		{
			OnChildrenChanged(_children.ToArray());
		}

		private void OnChildrenChanged(IEnumerable<Control> resultingChildren)
		{
			RecalculateLayout();
		}

		public void FilterChildren<TControl>(Func<TControl, bool> filter) where TControl : Control
		{
			_children.Cast<TControl>().ToList().ForEach(delegate(TControl tc)
			{
				tc.Visible = filter(tc);
			});
			Invalidate();
		}

		public void SortChildren<TControl>(Comparison<TControl> comparison) where TControl : Control
		{
			List<TControl> tempChildren = _children.Cast<TControl>().ToList();
			tempChildren.Sort(comparison);
			_children = new ControlCollection<Control>(tempChildren);
			Invalidate();
		}

		private void UpdateSizeToFitChildren()
		{
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			if (_children.Count == 0)
			{
				return;
			}
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			int newWidth = (int)((float)(_children.Count * _children[0].Width) + (float)(_children.Count - 1) * _controlPadding.X + outerPadX * 2f);
			int newHeight = (int)((float)(_children.Count * _children[0].Height) + (float)(_children.Count - 1) * _controlPadding.Y + outerPadY * 2f);
			Point previousSize = base.Size;
			if (FlowDirection == AlertFlowDirection.LeftToRight || FlowDirection == AlertFlowDirection.RightToLeft)
			{
				base.Width = newWidth;
				base.Height = (int)((float)_children[0].Height + outerPadY * 2f);
			}
			else if (FlowDirection == AlertFlowDirection.TopToBottom || FlowDirection == AlertFlowDirection.BottomToTop)
			{
				base.Width = (int)((float)_children[0].Width + outerPadX * 2f);
				base.Height = newHeight;
			}
			switch (TimersModule.ModuleInstance._alertDisplayOrientationSetting.Value)
			{
			case AlertFlowDirection.LeftToRight:
			case AlertFlowDirection.TopToBottom:
				base.Location = TimersModule.ModuleInstance._alertContainerLocationSetting.Value;
				break;
			case AlertFlowDirection.RightToLeft:
				base.Right = TimersModule.ModuleInstance._alertContainerLocationSetting.Value.X + TimersModule.ModuleInstance._alertContainerSizeSetting.Value.X;
				base.Top = TimersModule.ModuleInstance._alertContainerLocationSetting.Value.Y;
				break;
			case AlertFlowDirection.BottomToTop:
				base.Left = TimersModule.ModuleInstance._alertContainerLocationSetting.Value.X;
				base.Bottom = TimersModule.ModuleInstance._alertContainerLocationSetting.Value.Y + TimersModule.ModuleInstance._alertContainerSizeSetting.Value.Y;
				break;
			}
			if (FlowDirection == AlertFlowDirection.RightToLeft)
			{
				foreach (Control child in _children)
				{
					child.Right += base.Size.X - previousSize.X;
				}
			}
			else
			{
				if (FlowDirection != AlertFlowDirection.BottomToTop)
				{
					return;
				}
				foreach (Control child2 in _children)
				{
					child2.Bottom += base.Size.Y - previousSize.Y;
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
			float startLocationX = ((FlowDirection != AlertFlowDirection.RightToLeft) ? outerPadX : ((float)base.Width - outerPadX));
			float num = ((FlowDirection != AlertFlowDirection.BottomToTop) ? outerPadY : ((float)base.Height - outerPadY));
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
					child.Top = (int)nextLocationY;
					break;
				case AlertFlowDirection.RightToLeft:
					tweenValue = new
					{
						Right = (int)nextLocationX
					};
					child.Top = (int)nextLocationY;
					break;
				case AlertFlowDirection.TopToBottom:
					tweenValue = new
					{
						Top = (int)nextLocationY
					};
					child.Left = (int)nextLocationX;
					break;
				case AlertFlowDirection.BottomToTop:
					tweenValue = new
					{
						Bottom = (int)nextLocationY
					};
					child.Left = (int)nextLocationX;
					break;
				}
				childTween = Control.Animation.Tweener.Tween(child, tweenValue, TimersModule.ModuleInstance._alertMoveDelaySetting.Value);
				childTween.OnComplete(delegate
				{
					_childTweens.Remove(child);
				});
				_childTweens.Add(child, childTween);
				nextLocationX += ((float)child.Width + _controlPadding.X) * (float)directionX;
				nextLocationY += ((float)child.Height + _controlPadding.Y) * (float)directionY;
			}
		}

		public override void RecalculateLayout()
		{
			if (_children.Count == 0 || TimersModule.ModuleInstance._hideAlertsSetting.Value)
			{
				Hide();
			}
			else
			{
				Show();
			}
			UpdateSizeToFitChildren();
			ReflowChildLayout(_children.ToArray());
			base.RecalculateLayout();
		}

		protected override CaptureType CapturesInput()
		{
			if (LocationLock || !_visible)
			{
				return CaptureType.DoNotBlock;
			}
			return base.CapturesInput();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!LocationLock)
			{
				_dragStart = base.RelativeMousePosition;
				_mouseDragging = true;
			}
			base.OnLeftMouseButtonPressed(e);
		}

		private void HandleLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TimersModule.ModuleInstance != null)
			{
				if (_mouseDragging)
				{
					TimersModule.ModuleInstance._alertContainerLocationSetting.Value = base.Location;
					TimersModule.ModuleInstance._alertContainerSizeSetting.Value = base.Size;
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
				Point newLocation = (base.Location = Control.Input.Mouse.Position - _dragStart);
				_dragStart = base.RelativeMousePosition;
			}
		}
	}
}
