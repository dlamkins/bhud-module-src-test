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
		protected bool _lock;

		protected bool Dragging;

		protected Point DragStart = Point.get_Zero();

		protected bool MouseOverPanel;

		private int _newHeight;

		private int _newWidth;

		private Point _anchor = Point.get_Zero();

		private Tween _animSizeChange;

		public EventHandler<EventArgs> ContainerDragged;

		protected Vector2 _controlPadding = Vector2.get_Zero();

		protected Vector2 _outerControlPadding = Vector2.get_Zero();

		protected bool _padLeftBeforeControl;

		protected bool _padTopBeforeControl;

		protected ControlFlowDirection _flowDirection;

		private Dictionary<Control, Tween> _animMoves = new Dictionary<Control, Tween>();

		private Dictionary<Control, Point> _newLocations = new Dictionary<Control, Point>();

		private Dictionary<Control, Point> _previousLocations = new Dictionary<Control, Point>();

		private bool canChangeSize;

		private ControlFlowDirection _previousFlowDirection;

		private bool noAnim;

		private bool SizeChanging;

		public bool AutoShow;

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
				if (SetProperty(ref _lock, value, invalidateLayout: false, "Lock"))
				{
					base.BackgroundColor = (Color)(_lock ? Color.get_Transparent() : new Color(Color.get_Black(), 0.3f));
				}
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

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			OnChildrenChanged(e);
			e.ChangedChild.Resized += ChangedChildOnResized;
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			base.OnChildRemoved(e);
			OnChildrenChanged(e);
			e.ChangedChild.Resized -= ChangedChildOnResized;
		}

		private void ChangedChildOnResized(object sender, ResizedEventArgs e)
		{
			ReflowChildLayout(_children.ToArray());
		}

		private void OnChildrenChanged(ChildChangedEventArgs e)
		{
			ReflowChildLayout(e.ResultingChildren.ToArray());
		}

		public override void RecalculateLayout()
		{
			ReflowChildLayout(_children.ToArray());
			base.RecalculateLayout();
		}

		public void FilterChildren<TControl>(Func<TControl, bool> filter) where TControl : Control
		{
			_children.Cast<TControl>().ToList().ForEach(delegate(TControl tc)
			{
				tc.Visible = filter(tc);
			});
			ReflowChildLayout(_children.ToArray());
		}

		public void SortChildren<TControl>(Comparison<TControl> comparison) where TControl : Control
		{
			List<TControl> tempChildren = _children.Cast<TControl>().ToList();
			tempChildren.Sort(comparison);
			_children.Select(_children.Remove);
			_children.AddRange(tempChildren);
			ReflowChildLayout(_children.ToArray());
		}

		private void ReflowChildLayoutLeftToRight(IEnumerable<Control> allChildren)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float nextBottom;
			float currentBottom = (nextBottom = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y));
			float lastRight = outerPadX;
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if ((float)child.Width >= (float)base.Width - lastRight)
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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float nextBottom;
			float currentBottom = (nextBottom = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y));
			float lastLeft = ((_anchor == Point.get_Zero()) ? ((float)base.Width - outerPadX) : ((float)_anchor.X - outerPadX));
			foreach (Control child in allChildren.Where((Control c) => c.Visible))
			{
				if (outerPadX > lastLeft - (float)child.Width)
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastLeft = (float)base.Width - outerPadX;
				}
				child.Location = new Point((int)(lastLeft - (float)child.Width), (int)currentBottom);
				lastLeft = (float)child.Left - _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.Bottom);
			}
		}

		private void ReflowChildLayoutTopToBottom(IEnumerable<Control> allChildren)
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			float nextRight = outerPadX;
			float currentRight = outerPadX;
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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			float num = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			float nextRight = num;
			float currentRight = num;
			float lastTop = ((_anchor == Point.get_Zero()) ? ((float)base.Height - outerPadY) : ((float)_anchor.Y - outerPadY));
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
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			float num = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			float lastLeft = num;
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)lastLeft, (int)outerPadY));
					_previousLocations.Add(child, Point.get_Zero());
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)lastLeft, (int)outerPadY);
				}
				lastLeft = (float)(_newLocations[child].X + child.Width) + _controlPadding.X;
			}
			foreach (Control child2 in allChildren)
			{
				if (noAnim)
				{
					child2.Location = _newLocations[child2];
					_animMoves.Clear();
				}
				else
				{
					if (!(_previousLocations[child2] != _newLocations[child2]))
					{
						continue;
					}
					if (child2.Location.X > _newLocations[child2].X)
					{
						if (_animMoves.TryGetValue(child2, out var animMove))
						{
							animMove.Cancel();
							_animMoves.Remove(child2);
						}
						animMove = Control.Animation.Tweener.Tween(child2, new
						{
							Location = _newLocations[child2]
						}, TimersModule.ModuleInstance._alertFadeDelaySetting.Value).Ease(Ease.CubeInOut);
						animMove.OnComplete(delegate
						{
							_animMoves.Remove(child2);
						});
						_animMoves.Add(child2, animMove);
					}
					else
					{
						child2.Location = _newLocations[child2];
					}
				}
			}
			canChangeSize = true;
		}

		private void ReflowChildLayoutSingleRightToLeft(IEnumerable<Control> allChildren)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			if (SizeChanging)
			{
				int offset = 0;
				foreach (Control child2 in allChildren)
				{
					if (child2 == allChildren.First())
					{
						offset = _newLocations[child2].X - (base.Width - (int)outerPadX);
					}
					child2.Right = _newLocations[child2].X - offset;
				}
				return;
			}
			float lastRight = (float)base.Width - outerPadX;
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)lastRight, (int)outerPadY));
					_previousLocations.Add(child, child.Location);
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)lastRight, (int)outerPadY);
				}
				lastRight = (float)(_newLocations[child].X - child.Width) - _controlPadding.X;
			}
			foreach (Control child3 in allChildren)
			{
				if (noAnim)
				{
					child3.Location = new Point(child3.Location.X, _newLocations[child3].Y);
					child3.Right = _newLocations[child3].X;
					_animMoves.Clear();
					continue;
				}
				if (_animMoves.TryGetValue(child3, out var animMove))
				{
					animMove.Cancel();
					_animMoves.Remove(child3);
				}
				animMove = Control.Animation.Tweener.Tween(child3, new
				{
					Right = _newLocations[child3].X
				}, TimersModule.ModuleInstance._alertFadeDelaySetting.Value).Ease(Ease.CubeInOut);
				animMove.OnComplete(delegate
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					_animMoves.Remove(child3);
					child3.Right = _newLocations[child3].X;
				});
				_animMoves.Add(child3, animMove);
			}
			canChangeSize = true;
		}

		private void ReflowChildLayoutSingleTopToBottom(IEnumerable<Control> allChildren)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float lastBottom = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)outerPadX, (int)lastBottom));
					_previousLocations.Add(child, Point.get_Zero());
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)outerPadX, (int)lastBottom);
				}
				lastBottom = (float)(_newLocations[child].Y + child.Height) + _controlPadding.Y;
			}
			foreach (Control child2 in allChildren)
			{
				if (noAnim)
				{
					child2.Location = _newLocations[child2];
					_animMoves.Clear();
				}
				else
				{
					if (!(_previousLocations[child2] != _newLocations[child2]))
					{
						continue;
					}
					if (child2.Location.Y > _newLocations[child2].Y)
					{
						if (_animMoves.TryGetValue(child2, out var animMove))
						{
							animMove.Cancel();
							_animMoves.Remove(child2);
						}
						animMove = Control.Animation.Tweener.Tween(child2, new
						{
							Location = _newLocations[child2]
						}, TimersModule.ModuleInstance._alertFadeDelaySetting.Value).Ease(Ease.CubeInOut);
						animMove.OnComplete(delegate
						{
							_animMoves.Remove(child2);
						});
						_animMoves.Add(child2, animMove);
					}
					else
					{
						child2.Location = _newLocations[child2];
					}
				}
			}
			canChangeSize = true;
		}

		private void ReflowChildLayoutSingleBottomToTop(IEnumerable<Control> allChildren)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			if (SizeChanging)
			{
				int offset = 0;
				foreach (Control child2 in allChildren)
				{
					if (child2 == allChildren.First())
					{
						offset = _newLocations[child2].Y - (base.Height - (int)outerPadY);
					}
					child2.Bottom = _newLocations[child2].Y - offset;
				}
				return;
			}
			float lastBottom = (float)base.Height - outerPadY;
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)outerPadX, (int)lastBottom));
					_previousLocations.Add(child, child.Location);
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)outerPadX, (int)lastBottom);
				}
				lastBottom = (float)(_newLocations[child].Y - child.Height) - _controlPadding.Y;
			}
			foreach (Control child3 in allChildren)
			{
				if (noAnim)
				{
					child3.Location = new Point(_newLocations[child3].X, child3.Location.Y);
					child3.Bottom = _newLocations[child3].Y;
					_animMoves.Clear();
					continue;
				}
				if (_animMoves.TryGetValue(child3, out var animMove))
				{
					animMove.Cancel();
					_animMoves.Remove(child3);
				}
				animMove = Control.Animation.Tweener.Tween(child3, new
				{
					Bottom = _newLocations[child3].Y
				}, TimersModule.ModuleInstance._alertFadeDelaySetting.Value).Ease(Ease.CubeInOut);
				animMove.OnComplete(delegate
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					_animMoves.Remove(child3);
					child3.Bottom = _newLocations[child3].Y;
				});
				_animMoves.Add(child3, animMove);
			}
			canChangeSize = true;
		}

		private void ReflowChildLayout(Control[] allChildren)
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

		public AlertContainer()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			GameService.Input.Mouse.LeftMouseButtonReleased += delegate(object sender, MouseEventArgs args)
			{
				HandleLeftMouseButtonReleased(args);
			};
			_animSizeChange = null;
			base.ChildAdded += delegate
			{
				canChangeSize = false;
				_animSizeChange?.Cancel();
				UpdateDisplay();
			};
			base.ChildRemoved += delegate
			{
				canChangeSize = false;
				_animSizeChange?.Cancel();
				UpdateDisplay();
			};
		}

		protected override CaptureType CapturesInput()
		{
			if (Lock || !base.Visible)
			{
				return CaptureType.DoNotBlock;
			}
			return base.CapturesInput();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!Lock)
			{
				DragStart = base.RelativeMousePosition;
				Dragging = true;
			}
			base.OnLeftMouseButtonPressed(e);
		}

		public void HandleLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			Dragging = false;
			switch (FlowDirection)
			{
			case ControlFlowDirection.SingleLeftToRight:
			case ControlFlowDirection.SingleTopToBottom:
			{
				Point newLoc = base.Location;
				newLoc.X = Math.Max(base.Location.X, 0);
				newLoc.X = Math.Min(newLoc.X, GameService.Graphics.SpriteScreen.Width - base.Width / 2);
				newLoc.Y = Math.Max(base.Location.Y, 0);
				newLoc.Y = Math.Min(newLoc.Y, GameService.Graphics.SpriteScreen.Height - base.Height / 2);
				base.Location = newLoc;
				break;
			}
			case ControlFlowDirection.SingleRightToLeft:
			case ControlFlowDirection.SingleBottomToTop:
			{
				Point newAnchor = _anchor;
				newAnchor.X = Math.Max(_anchor.X, base.Width / 2);
				newAnchor.X = Math.Min(newAnchor.X, GameService.Graphics.SpriteScreen.Width);
				newAnchor.Y = Math.Max(_anchor.Y, base.Height / 2);
				newAnchor.Y = Math.Min(newAnchor.Y, GameService.Graphics.SpriteScreen.Height);
				_anchor = newAnchor;
				break;
			}
			}
			ContainerDragged?.Invoke(this, EventArgs.Empty);
		}

		public int GetChildrenMaxHeight()
		{
			int maxHeight = 0;
			foreach (Control child in base.Children)
			{
				if (child.Height > maxHeight && child.Height <= TimersModule.ModuleInstance.Resources.MAX_ALERT_HEIGHT)
				{
					maxHeight = child.Height;
				}
			}
			return maxHeight;
		}

		public int GetChildrenMaxWidth()
		{
			int maxWidth = 0;
			foreach (Control child in base.Children)
			{
				if (child.Width > maxWidth && child.Width <= TimersModule.ModuleInstance.Resources.MAX_ALERT_WIDTH)
				{
					maxWidth = child.Width;
				}
			}
			return maxWidth;
		}

		public void UpdateDisplay()
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			if (FlowDirection == ControlFlowDirection.TopToBottom)
			{
				FlowDirection = ControlFlowDirection.SingleTopToBottom;
			}
			else if (FlowDirection == ControlFlowDirection.BottomToTop)
			{
				FlowDirection = ControlFlowDirection.SingleBottomToTop;
			}
			else if (FlowDirection == ControlFlowDirection.LeftToRight)
			{
				FlowDirection = ControlFlowDirection.SingleLeftToRight;
			}
			else if (FlowDirection == ControlFlowDirection.RightToLeft)
			{
				FlowDirection = ControlFlowDirection.SingleRightToLeft;
			}
			if (_previousFlowDirection != FlowDirection)
			{
				switch (FlowDirection)
				{
				case ControlFlowDirection.SingleLeftToRight:
				case ControlFlowDirection.SingleTopToBottom:
					_anchor = Point.get_Zero();
					break;
				case ControlFlowDirection.SingleRightToLeft:
					_anchor = new Point(base.Right, base.Location.Y);
					break;
				case ControlFlowDirection.SingleBottomToTop:
					_anchor = new Point(base.Location.X, base.Bottom);
					break;
				}
				SizeChanging = false;
				noAnim = true;
				_newLocations.Clear();
				_previousLocations.Clear();
				_animMoves.Clear();
				ReflowChildLayout(base.Children.ToArray());
			}
			_previousFlowDirection = FlowDirection;
			int maxHeight = GetChildrenMaxHeight();
			int maxWidth = GetChildrenMaxWidth();
			int previousHeight = _newHeight;
			int previousWidth = _newWidth;
			_newHeight = base.Height;
			_newWidth = base.Width;
			switch (FlowDirection)
			{
			case ControlFlowDirection.SingleTopToBottom:
			case ControlFlowDirection.SingleBottomToTop:
				_newHeight = maxHeight * base.Children.Count + (int)ControlPadding.Y * (base.Children.Count + 1);
				_newWidth = maxWidth + (int)ControlPadding.X * 2;
				break;
			case ControlFlowDirection.SingleLeftToRight:
			case ControlFlowDirection.SingleRightToLeft:
				_newHeight = maxHeight + (int)ControlPadding.Y * 2;
				_newWidth = maxWidth * base.Children.Count + (int)ControlPadding.X * (base.Children.Count + 1);
				break;
			}
			if (_anchor != Point.get_Zero())
			{
				if (FlowDirection == ControlFlowDirection.SingleRightToLeft)
				{
					base.Right = _anchor.X;
				}
				else if (FlowDirection == ControlFlowDirection.SingleBottomToTop)
				{
					base.Bottom = _anchor.Y;
				}
			}
			if (!canChangeSize)
			{
				SizeChanging = false;
				ReflowChildLayout(base.Children.ToArray());
			}
			else if (previousWidth != _newWidth || previousHeight != _newHeight)
			{
				_animSizeChange?.Cancel();
				switch (FlowDirection)
				{
				case ControlFlowDirection.SingleTopToBottom:
				case ControlFlowDirection.SingleBottomToTop:
					base.Width = _newWidth;
					SizeChanging = false;
					noAnim = false;
					ReflowChildLayout(base.Children.ToArray());
					_animSizeChange = Control.Animation.Tweener.Tween(this, new
					{
						Height = _newHeight
					}, TimersModule.ModuleInstance._alertMoveDelaySetting.Value, (previousHeight < _newHeight) ? 0f : TimersModule.ModuleInstance._alertMoveDelaySetting.Value).Ease(Ease.CubeInOut);
					break;
				case ControlFlowDirection.SingleLeftToRight:
				case ControlFlowDirection.SingleRightToLeft:
					base.Height = _newHeight;
					SizeChanging = false;
					noAnim = false;
					ReflowChildLayout(base.Children.ToArray());
					_animSizeChange = Control.Animation.Tweener.Tween(this, new
					{
						Width = _newWidth
					}, TimersModule.ModuleInstance._alertMoveDelaySetting.Value, (previousWidth < _newWidth) ? 0f : TimersModule.ModuleInstance._alertMoveDelaySetting.Value).Ease(Ease.CubeInOut);
					break;
				}
				_animSizeChange?.OnBegin(delegate
				{
					if (!canChangeSize)
					{
						_animSizeChange.Cancel();
						_animSizeChange = null;
					}
					SizeChanging = true;
				});
				_animSizeChange?.OnUpdate(delegate
				{
					if (!canChangeSize)
					{
						_animSizeChange.Cancel();
						_animSizeChange = null;
					}
					SizeChanging = true;
					ReflowChildLayout(base.Children.ToArray());
				});
				_animSizeChange?.OnComplete(delegate
				{
					_animSizeChange = null;
					if (FlowDirection == ControlFlowDirection.SingleRightToLeft)
					{
						base.Right = _anchor.X;
					}
					else if (FlowDirection == ControlFlowDirection.SingleBottomToTop)
					{
						base.Bottom = _anchor.Y;
					}
					SizeChanging = false;
					ReflowChildLayout(base.Children.ToArray());
					ContainerDragged?.Invoke(this, EventArgs.Empty);
				});
			}
			if (base.Children.Count == 0)
			{
				Hide();
				_animSizeChange?.Cancel();
				_animSizeChange = null;
				base.Height = 0;
				base.Width = 0;
			}
			else if (AutoShow)
			{
				Show();
			}
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
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (Dragging)
			{
				Point newLocation = Control.Input.Mouse.Position - DragStart;
				Point offset = newLocation - base.Location;
				base.Location = newLocation;
				DragStart = base.RelativeMousePosition;
				if (_anchor != Point.get_Zero() && (FlowDirection == ControlFlowDirection.SingleRightToLeft || FlowDirection == ControlFlowDirection.SingleBottomToTop))
				{
					_anchor += offset;
				}
			}
			UpdateDisplay();
		}
	}
}
