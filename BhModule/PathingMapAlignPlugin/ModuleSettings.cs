using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Settings;

namespace BhModule.PathingMapAlignPlugin
{
	public class ModuleSettings
	{
		public SettingEntry<int> Width { get; private set; }

		public SettingEntry<int> Height { get; private set; }

		public SettingEntry<int> X { get; private set; }

		public SettingEntry<int> Y { get; private set; }

		public SettingEntry<float> Scale { get; private set; }

		public ModuleSettings(SettingCollection settings)
		{
			int num = -300;
			int num2 = 300;
			Width = settings.DefineSetting<int>("Width", 0, (Func<string>)delegate
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(17, 1);
				defaultInterpolatedStringHandler5.AppendLiteral("Mini Map Width <");
				defaultInterpolatedStringHandler5.AppendFormatted(Width.get_Value());
				defaultInterpolatedStringHandler5.AppendLiteral(">");
				return defaultInterpolatedStringHandler5.ToStringAndClear();
			}, (Func<string>)(() => "This will change the X position of the minimap."));
			SettingComplianceExtensions.SetRange(Width, num, num2);
			Width.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Height = settings.DefineSetting<int>("Height", 0, (Func<string>)delegate
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Mini Map Height <");
				defaultInterpolatedStringHandler4.AppendFormatted(Height.get_Value());
				defaultInterpolatedStringHandler4.AppendLiteral(">");
				return defaultInterpolatedStringHandler4.ToStringAndClear();
			}, (Func<string>)(() => "This will change the Y position of the minimap."));
			SettingComplianceExtensions.SetRange(Height, num, num2);
			Height.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			X = settings.DefineSetting<int>("X", 0, (Func<string>)delegate
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(13, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("Mini Map X <");
				defaultInterpolatedStringHandler3.AppendFormatted(X.get_Value());
				defaultInterpolatedStringHandler3.AppendLiteral(">");
				return defaultInterpolatedStringHandler3.ToStringAndClear();
			}, (Func<string>)(() => "Mini map left position offset."));
			SettingComplianceExtensions.SetRange(X, num, num2);
			X.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Y = settings.DefineSetting<int>("Y", 0, (Func<string>)delegate
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(13, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Mini Map Y <");
				defaultInterpolatedStringHandler2.AppendFormatted(Y.get_Value());
				defaultInterpolatedStringHandler2.AppendLiteral(">");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}, (Func<string>)(() => "Mini map top position offset."));
			SettingComplianceExtensions.SetRange(Y, num, num2);
			Y.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Scale = settings.DefineSetting<float>("Scale", 1f, (Func<string>)delegate
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Marker Scale <");
				defaultInterpolatedStringHandler.AppendFormatted(Math.Round(Scale.get_Value(), 2));
				defaultInterpolatedStringHandler.AppendLiteral(">");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}, (Func<string>)(() => "Keep adjusting until the far side markers are aligned."));
			SettingComplianceExtensions.SetRange(Scale, 1f, 3f);
			Scale.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSettingChanged);
		}

		private void OnSettingChanged(object target, EventArgs e)
		{
			PathingMapAlignPluginSettingsView.UpdateTitles?.Invoke();
		}

		public void Reset()
		{
			X.set_Value(0);
			Y.set_Value(0);
			Width.set_Value(0);
			Height.set_Value(0);
			Scale.set_Value(1f);
		}

		public void Unload()
		{
			PathingMapAlignPluginSettingsView.DisposeRootFlowPanel?.Invoke();
			X.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Y.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Width.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Height.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSettingChanged);
			Scale.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnSettingChanged);
		}
	}
}
