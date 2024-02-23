using System;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;

namespace Ideka.CustomCombatText
{
	public class TypeScrollPanel : Panel
	{
		private const int Spacing = 10;

		private ModelTypeScroll? _target;

		private readonly FloatBox _messagePivotXBox;

		private readonly EnumDropdown<ModelTypeScroll.Curve> _curveTypeDropdown;

		private readonly IntBox _scrollSpeedBox;

		private readonly IntBox _maxQueueSizeBox;

		public ModelTypeScroll? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				bool visible;
				((Control)this).set_Enabled(visible = value != null);
				((Control)this).set_Visible(visible);
				_messagePivotXBox.Value = value?.MessagePivotX ?? 0f;
				_curveTypeDropdown.Value = value?.CurveType ?? ModelTypeScroll.Curve.Right;
				_scrollSpeedBox.Value = value?.ScrollSpeed ?? 90;
				_maxQueueSizeBox.Value = value?.MaxQueueSize ?? 3;
				_target = value;
			}
		}

		public TypeScrollPanel()
			: this()
		{
			((Panel)this).set_Title("Scroll Area");
			((Panel)this).set_ShowTint(true);
			FloatBox floatBox = new FloatBox();
			((Control)floatBox).set_Parent((Container)(object)this);
			floatBox.Label = "Message Pivot X";
			floatBox.MinValue = 0f;
			floatBox.MaxValue = 1f;
			floatBox.Scale = 0.001f;
			floatBox.DraggingCommits = true;
			floatBox.AllBasicTooltipText = "Controls message alignment. 0 for left, 1 for right.";
			floatBox.LabelBasicTooltipText = "Click and drag.";
			_messagePivotXBox = floatBox;
			EnumDropdown<ModelTypeScroll.Curve> enumDropdown = new EnumDropdown<ModelTypeScroll.Curve>(new Func<ModelTypeScroll.Curve, string>(DataExtensions.Describe), ModelTypeScroll.Curve.None);
			((Control)enumDropdown).set_Parent((Container)(object)this);
			enumDropdown.Label = "Curve Type";
			_curveTypeDropdown = enumDropdown;
			IntBox intBox = new IntBox();
			((Control)intBox).set_Parent((Container)(object)this);
			intBox.Label = "Scroll Speed";
			intBox.MinValue = 1;
			intBox.DraggingCommits = true;
			intBox.AllBasicTooltipText = "Pixels per second.";
			intBox.LabelBasicTooltipText = "Click and drag.";
			_scrollSpeedBox = intBox;
			IntBox intBox2 = new IntBox();
			((Control)intBox2).set_Parent((Container)(object)this);
			intBox2.Label = "Max Queue Size";
			intBox2.MinValue = 0;
			intBox2.DraggingCommits = true;
			intBox2.AllBasicTooltipText = "Maximum number of messages that can be delayed in appearing while waiting for older messages to scroll and make room. Set to 0 to always show messages immediately.";
			intBox2.LabelBasicTooltipText = "Click and drag.";
			_maxQueueSizeBox = intBox2;
			UpdateLayout();
			_messagePivotXBox.ValueCommitted += delegate(float value)
			{
				ModelTypeScroll target4 = Target;
				if (target4 != null)
				{
					target4.MessagePivotX = value;
				}
			};
			_curveTypeDropdown.ValueCommitted += delegate(ModelTypeScroll.Curve value)
			{
				ModelTypeScroll target3 = Target;
				if (target3 != null)
				{
					target3.CurveType = value;
				}
			};
			_scrollSpeedBox.ValueCommitted += delegate(int value)
			{
				ModelTypeScroll target2 = Target;
				if (target2 != null)
				{
					target2.ScrollSpeed = value;
				}
			};
			_maxQueueSizeBox.ValueCommitted += delegate(int value)
			{
				ModelTypeScroll target = Target;
				if (target != null)
				{
					target.MaxQueueSize = value;
				}
			};
			Target = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_messagePivotXBox != null)
			{
				FloatBox messagePivotXBox = _messagePivotXBox;
				int top;
				((Control)_messagePivotXBox).set_Left(top = 10);
				((Control)messagePivotXBox).set_Top(top);
				((Control)(object)_messagePivotXBox).ArrangeTopDown(10, (Control)_curveTypeDropdown, (Control)_scrollSpeedBox, (Control)_maxQueueSizeBox);
				((Control)(object)_messagePivotXBox).WidthFillRight(10);
				((Control)(object)_curveTypeDropdown).WidthFillRight(10);
				((Control)(object)_scrollSpeedBox).WidthFillRight(10);
				((Control)(object)_maxQueueSizeBox).WidthFillRight(10);
				ValueControl.AlignLabels(_messagePivotXBox, _curveTypeDropdown, _scrollSpeedBox, _maxQueueSizeBox);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_maxQueueSizeBox, 10);
			}
		}
	}
}
