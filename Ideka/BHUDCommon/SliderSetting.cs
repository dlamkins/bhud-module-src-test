using System;
using Blish_HUD;
using Blish_HUD.Settings;

namespace Ideka.BHUDCommon
{
	public class SliderSetting : GenericSetting<int>
	{
		private readonly SettingEntry<string> _string;

		private readonly int _minValue;

		private readonly int _maxValue;

		private bool _reflecting;

		public SliderSetting(SettingCollection settings, string key, int defaultValue, int minValue, int maxValue, Func<string> displayNameFunc, Func<string> descriptionFunc, Func<int, int, string> validationErrorFunc)
		{
			SliderSetting sliderSetting = this;
			_minValue = minValue;
			_maxValue = maxValue;
			_string = settings.DefineSetting<string>(key + "Str", "", displayNameFunc, descriptionFunc);
			SettingEntry<int> setting = settings.DefineSetting<int>(key ?? "", defaultValue, (Func<string>)(() => " "), descriptionFunc);
			SettingComplianceExtensions.SetRange(setting, _minValue, _maxValue);
			_string.set_Value($"{setting.get_Value()}");
			SettingComplianceExtensions.SetValidation<string>(_string, (Func<string, SettingValidationResult>)((string str) => sliderSetting.Validate(str, out var _) ? new SettingValidationResult(true, (string)null) : new SettingValidationResult(false, validationErrorFunc?.Invoke(sliderSetting._minValue, sliderSetting._maxValue))));
			_string.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)StringChanged);
			Initialize(setting);
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
					base.Setting.set_Value(val);
				}
				else
				{
					_string.set_Value($"{base.Setting.get_Value()}");
				}
				_reflecting = false;
			}
		}

		protected override void SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			base.SettingChanged(sender, e);
			if (!_reflecting)
			{
				_reflecting = true;
				_string.set_Value($"{base.Setting.get_Value()}");
				_reflecting = false;
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			_string.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)StringChanged);
		}
	}
}
