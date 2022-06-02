using System;
using Blish_HUD;
using Blish_HUD.Settings;

namespace Ideka.HitboxView
{
	public class SliderEntry : IDisposable
	{
		private readonly SettingEntry<string> _string;

		private readonly int _minValue;

		private readonly int _maxValue;

		private bool _reflecting;

		public SettingEntry<int> Setting { get; }

		public int Value => Setting.get_Value();

		public Action<int> Changed { get; set; }

		public SliderEntry(SettingCollection settings, string key, int defaultValue, int minValue, int maxValue, Func<string> displayNameFunc, Func<string> descriptionFunc, Func<int, int, string> validationErrorFunc)
		{
			SliderEntry sliderEntry = this;
			_minValue = minValue;
			_maxValue = maxValue;
			_string = settings.DefineSetting<string>(key + "Str", "", displayNameFunc, descriptionFunc);
			Setting = settings.DefineSetting<int>(key ?? "", defaultValue, (Func<string>)(() => " "), descriptionFunc);
			SettingComplianceExtensions.SetRange(Setting, _minValue, _maxValue);
			_string.set_Value($"{Setting.get_Value()}");
			SettingComplianceExtensions.SetValidation<string>(_string, (Func<string, SettingValidationResult>)((string str) => sliderEntry.Validate(str, out var _) ? new SettingValidationResult(true, (string)null) : new SettingValidationResult(false, validationErrorFunc?.Invoke(sliderEntry._minValue, sliderEntry._maxValue))));
			_string.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)StringChanged);
			Setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)IntChanged);
		}

		private bool Validate(string str, out int value)
		{
			if (int.TryParse(str, out value) && value > _minValue)
			{
				return value < _maxValue;
			}
			return false;
		}

		private void StringChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (!_reflecting)
			{
				_reflecting = true;
				if (Validate(_string.get_Value(), out var val))
				{
					Setting.set_Value(val);
				}
				else
				{
					_string.set_Value($"{Setting.get_Value()}");
				}
				_reflecting = false;
			}
		}

		private void IntChanged(object sender, ValueChangedEventArgs<int> e)
		{
			Changed?.Invoke(Value);
			if (!_reflecting)
			{
				_reflecting = true;
				_string.set_Value($"{Setting.get_Value()}");
				_reflecting = false;
			}
		}

		public void Dispose()
		{
			_string.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)StringChanged);
			Setting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)IntChanged);
		}
	}
}
