using System;

namespace Ideka.RacingMeter
{
	public class StringBox : ValueTextBox<string>
	{
		private readonly Func<string, string> _validator;

		protected override bool TryMakeValue(string innerValue, out string value)
		{
			value = ((_validator != null) ? _validator(innerValue) : innerValue);
			return value != null;
		}

		protected override bool TryMakeText(ref string value, out string text)
		{
			value = (text = ((_validator != null) ? _validator(value) : value));
			return value != null;
		}

		public StringBox(Func<string, string> validator = null)
		{
			_validator = validator;
		}
	}
}
