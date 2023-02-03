using System;
using System.Linq;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public abstract class ValueControl : Container
	{
		private int _spacing;

		protected readonly Label _label;

		public string Label
		{
			get
			{
				return _label.get_Text();
			}
			set
			{
				_label.set_Text(value);
				UpdateLayout();
			}
		}

		public int Spacing
		{
			get
			{
				return _spacing;
			}
			set
			{
				_spacing = value;
				UpdateLayout();
			}
		}

		public int ControlWidth
		{
			get
			{
				return Control.get_Width();
			}
			set
			{
				Control.set_Width(value);
				UpdateLayout();
			}
		}

		public bool ControlEnabled
		{
			get
			{
				return Control.get_Enabled();
			}
			set
			{
				Control.set_Enabled(value);
			}
		}

		public string? AllBasicTooltipText
		{
			set
			{
				Control control = Control;
				Label label = _label;
				string text;
				((Control)this).set_BasicTooltipText(text = value);
				string basicTooltipText;
				((Control)label).set_BasicTooltipText(basicTooltipText = text);
				control.set_BasicTooltipText(basicTooltipText);
			}
		}

		protected Control Control { get; private set; }

		public ValueControl(Control control)
			: this()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			Spacing = 10;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_AutoSizeWidth(true);
			val.set_VerticalAlignment((VerticalAlignment)1);
			_label = val;
			Control = control;
			control.set_Parent((Container)(object)this);
			UpdateLayout();
		}

		public static void AlignLabels(params ValueControl[] controls)
		{
			int width = controls.Max((ValueControl c) => ((Control)c._label).get_Width());
			foreach (ValueControl obj in controls)
			{
				obj._label.set_AutoSizeWidth(false);
				obj._label.set_HorizontalAlignment((HorizontalAlignment)2);
				((Control)obj._label).set_Width(width);
				obj.UpdateLayout();
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (Control != null)
			{
				((Control)_label).set_Height(Control.get_Height());
				Control.set_Left(((Control)_label).get_Right() + Spacing);
				Control.set_Width(((Container)this).get_ContentRegion().Width - Control.get_Left());
				((Container)(object)this).SetContentRegionHeight(Control.get_Bottom());
			}
		}
	}
	public abstract class ValueControl<TValue, TInnerValue, TControl> : ValueControl where TControl : Control, new()
	{
		private TValue _value;

		public TValue Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (TryReflectValue(ref value))
				{
					_value = value;
				}
			}
		}

		protected new TControl Control => (TControl)(object)base.Control;

		protected bool MouseOverControl => ((Control)Control).get_MouseOver();

		public event Action<TValue>? TempValue;

		public event Action? TempClear;

		public event Action<TValue>? ValueCommitted;

		public ValueControl(TValue start)
			: base((Control)(object)new TControl())
		{
			_value = start;
		}

		protected abstract bool TryReflectValue(ref TValue value);

		protected abstract bool TryMakeValue(TInnerValue innerValue, out TValue value);

		protected void ResetValue()
		{
			Value = Value;
		}

		protected void SetTempValue(TValue value, bool reflect)
		{
			if (!reflect || TryReflectValue(ref value))
			{
				this.TempValue?.Invoke(value);
			}
		}

		protected void UnsetTempValue()
		{
			ResetValue();
			this.TempClear?.Invoke();
		}

		protected void CommitValue(TValue value)
		{
			Value = value;
			this.ValueCommitted?.Invoke(value);
		}
	}
}
