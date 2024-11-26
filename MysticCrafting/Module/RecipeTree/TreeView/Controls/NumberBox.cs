using System;
using System.ComponentModel;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class NumberBox : TextBox
	{
		private int _value;

		public EventHandler<PropertyChangedEventArgs> MaxValueChanged;

		private int _maxValue;

		private Point? _dragStartPoint;

		private int? _dragStartValue;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				int val = Clamp(value, MinValue, MaxValue);
				((TextInputBase)this).set_Text(val.ToString());
				((Control)this).SetProperty<int>(ref _value, val, false, "Value");
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
				((Control)this).SetProperty<int>(ref _maxValue, value, false, "MaxValue");
				MaxValueChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
			}
		}

		public int MinValue { get; set; } = int.MinValue;


		public event EventHandler<EventArgs> AfterTextChanged;

		public NumberBox()
			: this()
		{
			((TextBox)this).set_HorizontalAlignment((HorizontalAlignment)1);
			MaxValue = int.MaxValue;
			Value = 0;
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)OnTextChanged);
		}

		private int Clamp(int val, int min, int max)
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			if (val.CompareTo(max) > 0)
			{
				return max;
			}
			return val;
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(((TextInputBase)this).get_Text()))
			{
				_value = 1;
				return;
			}
			if (!int.TryParse(((TextInputBase)this).get_Text(), out var value))
			{
				((TextInputBase)this).set_Text(Value.ToString());
				return;
			}
			Value = Math.Min(Math.Max(value, MinValue), MaxValue);
			this.AfterTextChanged?.Invoke(sender, e);
		}

		protected override void DisposeControl()
		{
			((TextInputBase)this).remove_TextChanged((EventHandler<EventArgs>)OnTextChanged);
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			MaxValueChanged = null;
			this.AfterTextChanged = null;
			((TextInputBase)this).DisposeControl();
		}
	}
}
