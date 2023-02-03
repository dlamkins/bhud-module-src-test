using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ideka.RacingMeter
{
	public class IntBox : ValueTextBox<int>
	{
		private int _minValue = int.MinValue;

		private int _maxValue = int.MaxValue;

		private readonly KeyBinding _dragCancel;

		private readonly EscBlockWindow _escBlock;

		private Point? _dragStart;

		private Vector2 _dragAmount = Vector2.get_Zero();

		private static bool HoldingAlt
		{
			get
			{
				if (!Control.get_Input().get_Keyboard().get_KeysDown()
					.Contains((Keys)164))
				{
					return Control.get_Input().get_Keyboard().get_KeysDown()
						.Contains((Keys)165);
				}
				return true;
			}
		}

		private static bool HoldingShift
		{
			get
			{
				if (!Control.get_Input().get_Keyboard().get_KeysDown()
					.Contains((Keys)160))
				{
					return Control.get_Input().get_Keyboard().get_KeysDown()
						.Contains((Keys)161);
				}
				return true;
			}
		}

		public int MinValue
		{
			get
			{
				return _minValue;
			}
			set
			{
				if (_minValue != value)
				{
					_minValue = Math.Min(value, MaxValue);
					if (base.Value < MinValue)
					{
						CommitValue(MinValue);
					}
				}
			}
		}

		public int MaxValue
		{
			get
			{
				return _maxValue;
			}
			set
			{
				if (_maxValue != value)
				{
					_maxValue = Math.Max(value, MinValue);
					if (base.Value > MaxValue)
					{
						CommitValue(MaxValue);
					}
				}
			}
		}

		public float XScale { get; set; } = 1f;


		public float YScale { get; set; } = -1f;


		public float Scale { get; set; } = 0.1f;


		public IntBox()
			: base(0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			_escBlock = new EscBlockWindow((Container)(object)this);
			base.Spacing = 10;
			_dragCancel = new KeyBinding((Keys)27);
			KeyBinding dragCancel = _dragCancel;
			bool enabled;
			_dragCancel.set_BlockSequenceFromGw2(enabled = false);
			dragCancel.set_Enabled(enabled);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)LeftMouseButtonPressed);
			Control.get_Input().get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)MouseMoved);
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)LeftMouseButtonReleased);
			_dragCancel.add_Activated((EventHandler<EventArgs>)DragCancel);
		}

		protected override bool TryMakeText(ref int value, out string text)
		{
			value = Math.Max(Math.Min(value, MaxValue), MinValue);
			text = $"{value}";
			return true;
		}

		protected override bool TryMakeValue(string innerValue, out int value)
		{
			if (!int.TryParse(innerValue, out value))
			{
				return false;
			}
			value = Math.Max(Math.Min(value, MaxValue), MinValue);
			return true;
		}

		private int DragValue()
		{
			return (int)Math.Round((_dragAmount.X * XScale + _dragAmount.Y * YScale) * Scale);
		}

		private void DragStart()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			_escBlock.BlockStart();
			_dragStart = Control.get_Input().get_Mouse().get_PositionRaw();
			_dragAmount = Vector2.get_Zero();
			KeyBinding dragCancel = _dragCancel;
			bool enabled;
			_dragCancel.set_BlockSequenceFromGw2(enabled = true);
			dragCancel.set_Enabled(enabled);
		}

		private void DragEnd(bool commit)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			_escBlock.BlockEnd();
			_dragStart = null;
			KeyBinding dragCancel = _dragCancel;
			bool enabled;
			_dragCancel.set_BlockSequenceFromGw2(enabled = false);
			dragCancel.set_Enabled(enabled);
			UnsetTempValue();
			if (commit)
			{
				CommitValue(base.Value + DragValue());
			}
			_dragAmount = Vector2.get_Zero();
		}

		private bool HandleHidden()
		{
			bool num = !((Control)(object)this).IsVisible();
			if (num && _dragStart.HasValue)
			{
				DragEnd(commit: false);
			}
			return num;
		}

		private void LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!HandleHidden())
			{
				if (_dragStart.HasValue)
				{
					DragEnd(commit: false);
				}
				if (((Control)this).get_MouseOver() && !base.MouseOverControl)
				{
					DragStart();
				}
			}
		}

		private void MouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			Point? dragStart = _dragStart;
			if (!dragStart.HasValue)
			{
				return;
			}
			Point start = dragStart.GetValueOrDefault();
			if (!HandleHidden())
			{
				Vector2 dragAmount = _dragAmount;
				Point val = Control.get_Input().get_Mouse().get_PositionRaw() - start;
				_dragAmount = dragAmount + ((Point)(ref val)).ToVector2() * (HoldingAlt ? 0.1f : 1f) * (float)((!HoldingShift) ? 1 : 10);
				if (base.Value + DragValue() < MinValue)
				{
					_dragAmount = new Vector2((float)(MinValue - base.Value) / Scale / XScale, 0f);
				}
				else if (base.Value + DragValue() > MaxValue)
				{
					_dragAmount = new Vector2((float)(MaxValue - base.Value) / Scale / XScale, 0f);
				}
				Mouse.SetPosition(start.X, start.Y);
				SetTempValue(base.Value + DragValue(), reflect: true);
			}
		}

		private void LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			if (!HandleHidden() && _dragStart.HasValue)
			{
				DragEnd(commit: true);
			}
		}

		private void DragCancel(object sender, EventArgs e)
		{
			if (!HandleHidden() && _dragStart.HasValue)
			{
				DragEnd(commit: false);
			}
		}

		protected override void DisposeControl()
		{
			if (_dragStart.HasValue)
			{
				DragEnd(commit: false);
			}
			KeyBinding dragCancel = _dragCancel;
			bool enabled;
			_dragCancel.set_BlockSequenceFromGw2(enabled = false);
			dragCancel.set_Enabled(enabled);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)LeftMouseButtonPressed);
			Control.get_Input().get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)MouseMoved);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)LeftMouseButtonReleased);
			_dragCancel.remove_Activated((EventHandler<EventArgs>)DragCancel);
			EscBlockWindow escBlock = _escBlock;
			if (escBlock != null)
			{
				((Control)escBlock).Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
