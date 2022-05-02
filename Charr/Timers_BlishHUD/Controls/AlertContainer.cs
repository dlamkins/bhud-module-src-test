using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
				if (((Control)this).SetProperty<bool>(ref _lock, value, false, "Lock"))
				{
					((Control)this).set_BackgroundColor((Color)(_lock ? Color.get_Transparent() : new Color(Color.get_Black(), 0.3f)));
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

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			((Panel)this).OnChildAdded(e);
			OnChildrenChanged(e);
			e.get_ChangedChild().add_Resized((EventHandler<ResizedEventArgs>)ChangedChildOnResized);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			((Panel)this).OnChildRemoved(e);
			OnChildrenChanged(e);
			e.get_ChangedChild().remove_Resized((EventHandler<ResizedEventArgs>)ChangedChildOnResized);
		}

		private void ChangedChildOnResized(object sender, ResizedEventArgs e)
		{
			ReflowChildLayout(((Container)this)._children.ToArray());
		}

		private void OnChildrenChanged(ChildChangedEventArgs e)
		{
			ReflowChildLayout(e.get_ResultingChildren().ToArray());
		}

		public override void RecalculateLayout()
		{
			ReflowChildLayout(((Container)this)._children.ToArray());
			((Panel)this).RecalculateLayout();
		}

		public void FilterChildren<TControl>(Func<TControl, bool> filter) where TControl : Control
		{
			((IEnumerable)((Container)this)._children).Cast<TControl>().ToList().ForEach(delegate(TControl tc)
			{
				((Control)tc).set_Visible(filter(tc));
			});
			ReflowChildLayout(((Container)this)._children.ToArray());
		}

		public void SortChildren<TControl>(Comparison<TControl> comparison) where TControl : Control
		{
			List<TControl> tempChildren = ((IEnumerable)((Container)this)._children).Cast<TControl>().ToList();
			tempChildren.Sort(comparison);
			((IEnumerable<Control>)((Container)this)._children).Select((Func<Control, bool>)((Container)this)._children.Remove);
			((Container)this)._children.AddRange((IEnumerable<Control>)tempChildren);
			ReflowChildLayout(((Container)this)._children.ToArray());
		}

		private void ReflowChildLayoutLeftToRight(IEnumerable<Control> allChildren)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float nextBottom;
			float currentBottom = (nextBottom = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y));
			float lastRight = outerPadX;
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if ((float)child.get_Width() >= (float)((Control)this).get_Width() - lastRight)
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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			float outerPadX = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float nextBottom;
			float currentBottom = (nextBottom = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y));
			float lastLeft = ((_anchor == Point.get_Zero()) ? ((float)((Control)this).get_Width() - outerPadX) : ((float)_anchor.X - outerPadX));
			foreach (Control child in allChildren.Where((Control c) => c.get_Visible()))
			{
				if (outerPadX > lastLeft - (float)child.get_Width())
				{
					currentBottom = nextBottom + _controlPadding.Y;
					lastLeft = (float)((Control)this).get_Width() - outerPadX;
				}
				child.set_Location(new Point((int)(lastLeft - (float)child.get_Width()), (int)currentBottom));
				lastLeft = (float)child.get_Left() - _controlPadding.X;
				nextBottom = Math.Max(nextBottom, child.get_Bottom());
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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			float num = (_padLeftBeforeControl ? _controlPadding.X : _outerControlPadding.X);
			float outerPadY = (_padTopBeforeControl ? _controlPadding.Y : _outerControlPadding.Y);
			float nextRight = num;
			float currentRight = num;
			float lastTop = ((_anchor == Point.get_Zero()) ? ((float)((Control)this).get_Height() - outerPadY) : ((float)_anchor.Y - outerPadY));
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
				lastLeft = (float)(_newLocations[child].X + child.get_Width()) + _controlPadding.X;
			}
			foreach (Control child2 in allChildren)
			{
				if (noAnim)
				{
					child2.set_Location(_newLocations[child2]);
					_animMoves.Clear();
				}
				else
				{
					if (!(_previousLocations[child2] != _newLocations[child2]))
					{
						continue;
					}
					if (child2.get_Location().X > _newLocations[child2].X)
					{
						if (_animMoves.TryGetValue(child2, out var animMove))
						{
							animMove.Cancel();
							_animMoves.Remove(child2);
						}
						animMove = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Control>(child2, (object)new
						{
							Location = _newLocations[child2]
						}, TimersModule.ModuleInstance._alertFadeDelaySetting.get_Value(), 0f, true).Ease((Func<float, float>)Ease.CubeInOut);
						animMove.OnComplete((Action)delegate
						{
							_animMoves.Remove(child2);
						});
						_animMoves.Add(child2, animMove);
					}
					else
					{
						child2.set_Location(_newLocations[child2]);
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
						offset = _newLocations[child2].X - (((Control)this).get_Width() - (int)outerPadX);
					}
					child2.set_Right(_newLocations[child2].X - offset);
				}
				return;
			}
			float lastRight = (float)((Control)this).get_Width() - outerPadX;
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)lastRight, (int)outerPadY));
					_previousLocations.Add(child, child.get_Location());
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)lastRight, (int)outerPadY);
				}
				lastRight = (float)(_newLocations[child].X - child.get_Width()) - _controlPadding.X;
			}
			foreach (Control child3 in allChildren)
			{
				if (noAnim)
				{
					child3.set_Location(new Point(child3.get_Location().X, _newLocations[child3].Y));
					child3.set_Right(_newLocations[child3].X);
					_animMoves.Clear();
					continue;
				}
				if (_animMoves.TryGetValue(child3, out var animMove))
				{
					animMove.Cancel();
					_animMoves.Remove(child3);
				}
				animMove = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Control>(child3, (object)new
				{
					Right = _newLocations[child3].X
				}, TimersModule.ModuleInstance._alertFadeDelaySetting.get_Value(), 0f, true).Ease((Func<float, float>)Ease.CubeInOut);
				animMove.OnComplete((Action)delegate
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					_animMoves.Remove(child3);
					child3.set_Right(_newLocations[child3].X);
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
				lastBottom = (float)(_newLocations[child].Y + child.get_Height()) + _controlPadding.Y;
			}
			foreach (Control child2 in allChildren)
			{
				if (noAnim)
				{
					child2.set_Location(_newLocations[child2]);
					_animMoves.Clear();
				}
				else
				{
					if (!(_previousLocations[child2] != _newLocations[child2]))
					{
						continue;
					}
					if (child2.get_Location().Y > _newLocations[child2].Y)
					{
						if (_animMoves.TryGetValue(child2, out var animMove))
						{
							animMove.Cancel();
							_animMoves.Remove(child2);
						}
						animMove = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Control>(child2, (object)new
						{
							Location = _newLocations[child2]
						}, TimersModule.ModuleInstance._alertFadeDelaySetting.get_Value(), 0f, true).Ease((Func<float, float>)Ease.CubeInOut);
						animMove.OnComplete((Action)delegate
						{
							_animMoves.Remove(child2);
						});
						_animMoves.Add(child2, animMove);
					}
					else
					{
						child2.set_Location(_newLocations[child2]);
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
						offset = _newLocations[child2].Y - (((Control)this).get_Height() - (int)outerPadY);
					}
					child2.set_Bottom(_newLocations[child2].Y - offset);
				}
				return;
			}
			float lastBottom = (float)((Control)this).get_Height() - outerPadY;
			foreach (Control child in allChildren)
			{
				if (!_newLocations.ContainsKey(child))
				{
					_newLocations.Add(child, new Point((int)outerPadX, (int)lastBottom));
					_previousLocations.Add(child, child.get_Location());
				}
				else
				{
					_previousLocations[child] = _newLocations[child];
					_newLocations[child] = new Point((int)outerPadX, (int)lastBottom);
				}
				lastBottom = (float)(_newLocations[child].Y - child.get_Height()) - _controlPadding.Y;
			}
			foreach (Control child3 in allChildren)
			{
				if (noAnim)
				{
					child3.set_Location(new Point(_newLocations[child3].X, child3.get_Location().Y));
					child3.set_Bottom(_newLocations[child3].Y);
					_animMoves.Clear();
					continue;
				}
				if (_animMoves.TryGetValue(child3, out var animMove))
				{
					animMove.Cancel();
					_animMoves.Remove(child3);
				}
				animMove = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<Control>(child3, (object)new
				{
					Bottom = _newLocations[child3].Y
				}, TimersModule.ModuleInstance._alertFadeDelaySetting.get_Value(), 0f, true).Ease((Func<float, float>)Ease.CubeInOut);
				animMove.OnComplete((Action)delegate
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					_animMoves.Remove(child3);
					child3.set_Bottom(_newLocations[child3].Y);
				});
				_animMoves.Add(child3, animMove);
			}
			canChangeSize = true;
		}

		private void ReflowChildLayout(Control[] allChildren)
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

		public AlertContainer()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				Dragging = false;
			});
			_animSizeChange = null;
			((Container)this).add_ChildAdded((EventHandler<ChildChangedEventArgs>)delegate
			{
				canChangeSize = false;
				Tween animSizeChange2 = _animSizeChange;
				if (animSizeChange2 != null)
				{
					animSizeChange2.Cancel();
				}
				UpdateDisplay();
			});
			((Container)this).add_ChildRemoved((EventHandler<ChildChangedEventArgs>)delegate
			{
				canChangeSize = false;
				Tween animSizeChange = _animSizeChange;
				if (animSizeChange != null)
				{
					animSizeChange.Cancel();
				}
				UpdateDisplay();
			});
		}

		protected override CaptureType CapturesInput()
		{
			if (!Lock && ((Control)this).get_Visible())
			{
				return (CaptureType)13;
			}
			return (CaptureType)0;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			MouseOverPanel = false;
			int y = ((Control)this).get_RelativeMousePosition().Y;
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			if (y < ((Rectangle)(ref contentRegion)).get_Bottom())
			{
				MouseOverPanel = true;
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			MouseOverPanel = false;
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (MouseOverPanel)
			{
				Dragging = true;
				DragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			Dragging = false;
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		public int GetChildrenMaxHeight()
		{
			int maxHeight = 0;
			foreach (Control child in ((Container)this).get_Children())
			{
				if (child.get_Height() > maxHeight && child.get_Height() <= TimersModule.ModuleInstance.Resources.MAX_ALERT_HEIGHT)
				{
					maxHeight = child.get_Height();
				}
			}
			return maxHeight;
		}

		public int GetChildrenMaxWidth()
		{
			int maxWidth = 0;
			foreach (Control child in ((Container)this).get_Children())
			{
				if (child.get_Width() > maxWidth && child.get_Width() <= TimersModule.ModuleInstance.Resources.MAX_ALERT_WIDTH)
				{
					maxWidth = child.get_Width();
				}
			}
			return maxWidth;
		}

		public void UpdateDisplay()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected I4, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Expected I4, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Invalid comparison between Unknown and I4
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Invalid comparison between Unknown and I4
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Expected I4, but got Unknown
			if ((int)FlowDirection == 1)
			{
				FlowDirection = (ControlFlowDirection)3;
			}
			else if ((int)FlowDirection == 6)
			{
				FlowDirection = (ControlFlowDirection)7;
			}
			else if ((int)FlowDirection == 0)
			{
				FlowDirection = (ControlFlowDirection)2;
			}
			else if ((int)FlowDirection == 4)
			{
				FlowDirection = (ControlFlowDirection)5;
			}
			ControlFlowDirection flowDirection;
			if (_previousFlowDirection != FlowDirection)
			{
				flowDirection = FlowDirection;
				switch (flowDirection - 2)
				{
				case 0:
				case 1:
					_anchor = Point.get_Zero();
					break;
				case 3:
					_anchor = new Point(((Control)this).get_Right(), ((Control)this).get_Location().Y);
					break;
				case 5:
					_anchor = new Point(((Control)this).get_Location().X, ((Control)this).get_Bottom());
					break;
				}
				SizeChanging = false;
				noAnim = true;
				_newLocations.Clear();
				_previousLocations.Clear();
				_animMoves.Clear();
				ReflowChildLayout(((Container)this).get_Children().ToArray());
			}
			_previousFlowDirection = FlowDirection;
			int maxHeight = GetChildrenMaxHeight();
			int maxWidth = GetChildrenMaxWidth();
			int previousHeight = _newHeight;
			int previousWidth = _newWidth;
			_newHeight = ((Control)this).get_Height();
			_newWidth = ((Control)this).get_Width();
			flowDirection = FlowDirection;
			switch (flowDirection - 2)
			{
			case 1:
			case 5:
				_newHeight = maxHeight * ((Container)this).get_Children().get_Count() + (int)ControlPadding.Y * (((Container)this).get_Children().get_Count() + 1);
				_newWidth = maxWidth + (int)ControlPadding.X * 2;
				break;
			case 0:
			case 3:
				_newHeight = maxHeight + (int)ControlPadding.Y * 2;
				_newWidth = maxWidth * ((Container)this).get_Children().get_Count() + (int)ControlPadding.X * (((Container)this).get_Children().get_Count() + 1);
				break;
			}
			if (_anchor != Point.get_Zero())
			{
				if ((int)FlowDirection == 5)
				{
					((Control)this).set_Right(_anchor.X);
				}
				else if ((int)FlowDirection == 7)
				{
					((Control)this).set_Bottom(_anchor.Y);
				}
			}
			if (!canChangeSize)
			{
				SizeChanging = false;
				ReflowChildLayout(((Container)this).get_Children().ToArray());
			}
			else if (previousWidth != _newWidth || previousHeight != _newHeight)
			{
				Tween animSizeChange = _animSizeChange;
				if (animSizeChange != null)
				{
					animSizeChange.Cancel();
				}
				flowDirection = FlowDirection;
				switch (flowDirection - 2)
				{
				case 1:
				case 5:
					((Control)this).set_Width(_newWidth);
					SizeChanging = false;
					noAnim = false;
					ReflowChildLayout(((Container)this).get_Children().ToArray());
					_animSizeChange = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<AlertContainer>(this, (object)new
					{
						Height = _newHeight
					}, TimersModule.ModuleInstance._alertMoveDelaySetting.get_Value(), (previousHeight < _newHeight) ? 0f : TimersModule.ModuleInstance._alertMoveDelaySetting.get_Value(), true).Ease((Func<float, float>)Ease.CubeInOut);
					break;
				case 0:
				case 3:
					((Control)this).set_Height(_newHeight);
					SizeChanging = false;
					noAnim = false;
					ReflowChildLayout(((Container)this).get_Children().ToArray());
					_animSizeChange = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<AlertContainer>(this, (object)new
					{
						Width = _newWidth
					}, TimersModule.ModuleInstance._alertMoveDelaySetting.get_Value(), (previousWidth < _newWidth) ? 0f : TimersModule.ModuleInstance._alertMoveDelaySetting.get_Value(), true).Ease((Func<float, float>)Ease.CubeInOut);
					break;
				}
				Tween animSizeChange2 = _animSizeChange;
				if (animSizeChange2 != null)
				{
					animSizeChange2.OnBegin((Action)delegate
					{
						if (!canChangeSize)
						{
							_animSizeChange.Cancel();
							_animSizeChange = null;
						}
						SizeChanging = true;
					});
				}
				Tween animSizeChange3 = _animSizeChange;
				if (animSizeChange3 != null)
				{
					animSizeChange3.OnUpdate((Action)delegate
					{
						if (!canChangeSize)
						{
							_animSizeChange.Cancel();
							_animSizeChange = null;
						}
						SizeChanging = true;
						ReflowChildLayout(((Container)this).get_Children().ToArray());
					});
				}
				Tween animSizeChange4 = _animSizeChange;
				if (animSizeChange4 != null)
				{
					animSizeChange4.OnComplete((Action)delegate
					{
						//IL_0008: Unknown result type (might be due to invalid IL or missing references)
						//IL_000e: Invalid comparison between Unknown and I4
						//IL_0024: Unknown result type (might be due to invalid IL or missing references)
						//IL_002a: Invalid comparison between Unknown and I4
						_animSizeChange = null;
						if ((int)FlowDirection == 5)
						{
							((Control)this).set_Right(_anchor.X);
						}
						else if ((int)FlowDirection == 7)
						{
							((Control)this).set_Bottom(_anchor.Y);
						}
						SizeChanging = false;
						ReflowChildLayout(((Container)this).get_Children().ToArray());
						ContainerDragged?.Invoke(this, EventArgs.Empty);
					});
				}
			}
			if (((Container)this).get_Children().get_Count() == 0)
			{
				((Control)this).Hide();
				Tween animSizeChange5 = _animSizeChange;
				if (animSizeChange5 != null)
				{
					animSizeChange5.Cancel();
				}
				_animSizeChange = null;
				((Control)this).set_Height(0);
				((Control)this).set_Width(0);
			}
			else if (AutoShow)
			{
				((Control)this).Show();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Invalid comparison between Unknown and I4
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Invalid comparison between Unknown and I4
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (Dragging)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - DragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				DragStart = Control.get_Input().get_Mouse().get_Position();
				if (_anchor != Point.get_Zero() && ((int)FlowDirection == 5 || (int)FlowDirection == 7))
				{
					_anchor += nOffset;
				}
				ContainerDragged?.Invoke(this, EventArgs.Empty);
			}
			UpdateDisplay();
		}
	}
}
