using System;
using System.ComponentModel;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
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
				base.Text = val.ToString();
				SetProperty(ref _value, val, invalidateLayout: false, "Value");
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
				SetProperty(ref _maxValue, value, invalidateLayout: false, "MaxValue");
				MaxValueChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
			}
		}

		public int MinValue { get; set; } = int.MinValue;


		public event EventHandler<EventArgs> AfterTextChanged;

		public NumberBox()
		{
			base.HorizontalAlignment = HorizontalAlignment.Center;
			MaxValue = int.MaxValue;
			Value = 0;
			base.TextChanged += OnTextChanged;
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
			if (string.IsNullOrEmpty(base.Text))
			{
				_value = 1;
				return;
			}
			if (!int.TryParse(base.Text, out var value))
			{
				base.Text = Value.ToString();
				return;
			}
			Value = Math.Min(Math.Max(value, MinValue), MaxValue);
			this.AfterTextChanged?.Invoke(sender, e);
		}

		protected override void DisposeControl()
		{
			base.TextChanged -= OnTextChanged;
			base.Tooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
