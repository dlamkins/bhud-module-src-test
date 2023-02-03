using System;

namespace Ideka.RacingMeter
{
	public class StringBox : ValueTextBox<string>
	{
		private readonly Func<string, string> _validator;

		protected override bool TryMakeValue(string innerValue, out string value)
		{
			value = _validator(innerValue);
			return value != null;
		}

		protected override bool TryMakeText(ref string value, out string text)
		{
			value = (text = _validator(value));
			return value != null;
		}

		public StringBox(Func<string, string>? validator = null)
			: base("")
		{
			_validator = validator ?? ((Func<string, string>)((string s) => s));
		}
	}
}
