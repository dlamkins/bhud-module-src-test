using System;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;

namespace Ideka.CustomCombatText
{
	public class TypeTopPanel : Panel
	{
		private const int Spacing = 10;

		private ModelTypeTop? _target;

		private readonly FloatBox _messagePivotXBox;

		private readonly FloatBox _messageTimeoutBox;

		private readonly BoolBox _animateOnHitBox;

		public ModelTypeTop? Target
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
				_messageTimeoutBox.Value = (float)(value?.MessageTimeout.TotalSeconds ?? 3.0);
				_animateOnHitBox.Value = value?.AnimateOnHit ?? true;
				_target = value;
			}
		}

		public TypeTopPanel()
			: this()
		{
			((Panel)this).set_Title("Top Area");
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
			FloatBox floatBox2 = new FloatBox();
			((Control)floatBox2).set_Parent((Container)(object)this);
			floatBox2.Label = "Message Timeout";
			floatBox2.MinValue = 1f;
			floatBox2.Scale = 0.01f;
			floatBox2.DraggingCommits = true;
			floatBox2.AllBasicTooltipText = "Seconds to wait before messages disappear.";
			floatBox2.LabelBasicTooltipText = "Click and drag.";
			_messageTimeoutBox = floatBox2;
			BoolBox boolBox = new BoolBox();
			((Control)boolBox).set_Parent((Container)(object)this);
			boolBox.Label = "Animate on Hit";
			((Control)boolBox).set_BasicTooltipText("Messages perform a small animation when appearing or merging.");
			_animateOnHitBox = boolBox;
			UpdateLayout();
			_messagePivotXBox.ValueCommitted += delegate(float value)
			{
				ModelTypeTop target3 = Target;
				if (target3 != null)
				{
					target3.MessagePivotX = value;
				}
			};
			_messageTimeoutBox.ValueCommitted += delegate(float value)
			{
				ModelTypeTop target2 = Target;
				if (target2 != null)
				{
					target2.MessageTimeout = TimeSpan.FromSeconds(value);
				}
			};
			_animateOnHitBox.ValueCommitted += delegate(bool value)
			{
				ModelTypeTop target = Target;
				if (target != null)
				{
					target.AnimateOnHit = value;
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
				((Control)(object)_messagePivotXBox).ArrangeTopDown(10, (Control)_messageTimeoutBox, (Control)_animateOnHitBox);
				((Control)(object)_messagePivotXBox).WidthFillRight(10);
				((Control)(object)_messageTimeoutBox).WidthFillRight(10);
				((Control)(object)_animateOnHitBox).WidthFillRight(10);
				ValueControl.AlignLabels(_messagePivotXBox, _messageTimeoutBox, _animateOnHitBox);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_animateOnHitBox, 10);
			}
		}
	}
}
