using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public class EnumDropdown<TEnum> : ValueControl<TEnum, string, Dropdown> where TEnum : struct, Enum
	{
		private readonly Func<TEnum, string> _describe;

		private readonly Dictionary<string, TEnum> _descValue = new Dictionary<string, TEnum>();

		public EnumDropdown(Func<TEnum, string> describe = null)
			: base(initialize: false)
		{
			_describe = describe ?? ((Func<TEnum, string>)((TEnum v) => Enum.GetName(typeof(TEnum), v)));
			TEnum[] cachedValues = EnumUtil.GetCachedValues<TEnum>();
			foreach (TEnum value in cachedValues)
			{
				_descValue[_describe(value)] = value;
			}
			Initialize();
		}

		protected override void Initialize(Dropdown dropdown)
		{
			TEnum[] cachedValues = EnumUtil.GetCachedValues<TEnum>();
			foreach (TEnum value in cachedValues)
			{
				dropdown.get_Items().Add(_describe(value));
			}
			dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!TryMakeValue(dropdown.get_SelectedItem(), out var value2))
				{
					ResetValue();
				}
				else if (!base.Value.Equals(value2))
				{
					CommitValue(value2);
				}
			});
		}

		protected override bool TryMakeValue(string innerValue, out TEnum value)
		{
			return _descValue.TryGetValue(innerValue, out value);
		}

		protected override bool TryReflectValue(ref TEnum value, Dropdown control)
		{
			string item = _describe(value);
			if (item == null)
			{
				return false;
			}
			control.set_SelectedItem(item);
			return true;
		}
	}
}
