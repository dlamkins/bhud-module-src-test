using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public class EnumDropdown<TEnum> : ValueControl<TEnum, string, Dropdown> where TEnum : struct, Enum
	{
		private readonly Func<TEnum, string?> _describe;

		private readonly Dictionary<string, TEnum> _descValue = new Dictionary<string, TEnum>();

		public EnumDropdown(Func<TEnum, string?>? describe = null, TEnum start = default(TEnum))
			: base(start)
		{
			_describe = describe ?? ((Func<TEnum, string>)((TEnum v) => Enum.GetName(typeof(TEnum), v)));
			TEnum[] cachedValues = EnumUtil.GetCachedValues<TEnum>();
			foreach (TEnum value in cachedValues)
			{
				string desc = _describe(value);
				if (desc != null)
				{
					_descValue[desc] = value;
					base.Control.get_Items().Add(desc);
				}
			}
			base.Control.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!TryMakeValue(base.Control.get_SelectedItem(), out var value2))
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

		protected override bool TryReflectValue(ref TEnum value)
		{
			string item = _describe(value);
			if (item == null)
			{
				return false;
			}
			base.Control.set_SelectedItem(item);
			return true;
		}
	}
}
