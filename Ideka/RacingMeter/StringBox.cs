using System;
using System.Linq;

namespace Ideka.RacingMeter
{
	public class StringBox : ValueTextBox<string>
	{
		private bool _hideValue;

		private readonly Func<string, string> _validator;

		public bool HideValue
		{
			get
			{
				return _hideValue;
			}
			set
			{
				_hideValue = value;
				string prev = base.Value;
				TryReflectValue(ref prev);
			}
		}

		protected override bool TryMakeValue(string innerValue, out string value)
		{
			value = _validator(innerValue);
			return value != null;
		}

		protected override bool TryMakeText(ref string value, out string text)
		{
			value = _validator(value);
			text = (HideValue ? string.Join("", value.Select((char _) => '*')) : value);
			return value != null;
		}

		public StringBox(Func<string, string>? validator = null)
			: base("")
		{
			_validator = validator ?? ((Func<string, string>)((string s) => s));
		}
	}
}
