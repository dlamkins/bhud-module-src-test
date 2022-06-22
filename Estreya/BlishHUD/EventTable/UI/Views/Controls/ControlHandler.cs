using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	public class ControlHandler
	{
		private static List<ControlProvider> Provider { get; } = new List<ControlProvider>
		{
			new TextBoxProvider(),
			new IntTrackBarProvider(),
			new IntTextBoxProvider(),
			new FloatTrackBarProvider(),
			new FloatTextBoxProvider(),
			new CheckboxProvider(),
			new TimeSpanProvider(),
			new KeybindingProvider()
		};


		private static void Register<T, TOverrideType>(ControlProvider<T, TOverrideType> controlProvider)
		{
			lock (Provider)
			{
				if (Provider.Any((ControlProvider p) => (p as ControlProvider<T, TOverrideType>)?.Types.Intersect(controlProvider.Types).Any() ?? false))
				{
					throw new ArgumentException($"Control Types \"{controlProvider.Types}\" already registered.");
				}
				Provider.Add(controlProvider);
			}
		}

		public static Control CreateFromSetting<T>(SettingEntry<T> settingEntry, Func<SettingEntry<T>, T, bool> validationFunction, int width, int heigth, int x, int y)
		{
			return CreateFromChangedTypeSetting(settingEntry, validationFunction, width, heigth, x, y);
		}

		public static Control CreateFromChangedTypeSetting<T, TOverride>(SettingEntry<T> settingEntry, Func<SettingEntry<T>, TOverride, bool> validationFunction, int width, int heigth, int x, int y)
		{
			List<ControlProvider> providers = Provider.Where((ControlProvider p) => p is ControlProvider<T, TOverride>).ToList();
			if (providers.Count == 0)
			{
				SettingEntry<T> obj2 = settingEntry;
				if (obj2 != null && ((SettingEntry)obj2).get_SettingType().IsEnum)
				{
					Register((ControlProvider<T, T>)Activator.CreateInstance(typeof(EnumProvider<>).MakeGenericType(typeof(T))));
					return CreateFromChangedTypeSetting(settingEntry, validationFunction, width, heigth, x, y);
				}
				SettingEntry<T> obj3 = settingEntry;
				if (((obj3 != null) ? ((SettingEntry)obj3).get_SettingType().Name : null) == typeof(List<T>).Name)
				{
					Register((ControlProvider<T, TOverride>)Activator.CreateInstance(typeof(ListViewProvider<>).MakeGenericType(typeof(TOverride))));
					return CreateFromChangedTypeSetting(settingEntry, validationFunction, width, heigth, x, y);
				}
				throw new NotSupportedException($"Control Type \"{((SettingEntry)settingEntry).get_SettingType()}\" is not supported.");
			}
			return (providers.First() as ControlProvider<T, TOverride>).CreateControl(new BoxedValue<T>(settingEntry.get_Value(), delegate(T val)
			{
				settingEntry.set_Value(val);
			}), (T obj) => !((SettingEntry)(object)settingEntry).IsDisabled(), (TOverride val) => validationFunction?.Invoke(settingEntry, val) ?? true, settingEntry.GetRange<T>(), width, heigth, x, y);
		}

		public static Control CreateFromProperty<TObject, TProperty>(TObject obj, Expression<Func<TObject, TProperty>> expression, Func<TObject, bool> isEnabled, Func<TProperty, bool> validationFunction, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			return CreateFromPropertyWithChangedTypeValidation(obj, expression, isEnabled, validationFunction, range, width, heigth, x, y);
		}

		public static Control CreateFromPropertyWithChangedTypeValidation<TObject, TProperty, TOverrideType>(TObject obj, Expression<Func<TObject, TProperty>> expression, Func<TObject, bool> isEnabled, Func<TOverrideType, bool> validationFunction, (float Min, float Max)? range, int width, int heigth, int x, int y)
		{
			List<ControlProvider> providers = Provider.Where((ControlProvider p) => (p as ControlProvider<TProperty, TOverrideType>)?.Types.Contains(typeof(TProperty)) ?? false).ToList();
			if (providers.Count == 0)
			{
				if (typeof(TProperty).IsEnum)
				{
					Register((ControlProvider<TProperty, TOverrideType>)Activator.CreateInstance(typeof(EnumProvider<>).MakeGenericType(typeof(TProperty))));
					return CreateFromPropertyWithChangedTypeValidation(obj, expression, isEnabled, validationFunction, range, width, heigth, x, y);
				}
				if (typeof(TProperty).Name == typeof(List<>).Name)
				{
					Register((ControlProvider<TProperty, TOverrideType>)Activator.CreateInstance(typeof(ListViewProvider<>).MakeGenericType(typeof(TOverrideType).GenericTypeArguments[0])));
					return CreateFromPropertyWithChangedTypeValidation(obj, expression, isEnabled, validationFunction, range, width, heigth, x, y);
				}
				throw new NotSupportedException($"Control Type \"{typeof(TProperty)}\" is not supported.");
			}
			Control ctrl = (providers.First() as ControlProvider<TProperty, TOverrideType>).CreateControl(new BoxedValue<TProperty>(expression.Compile()(obj), delegate(TProperty val)
			{
				MemberExpression memberExpression2 = expression.Body as MemberExpression;
				if (memberExpression2 != null)
				{
					PropertyInfo propertyInfo = memberExpression2.Member as PropertyInfo;
					if (propertyInfo != null)
					{
						propertyInfo.SetValue(obj, val, null);
					}
				}
			}), (TProperty val) => isEnabled?.Invoke(obj) ?? true, validationFunction, range, width, heigth, x, y);
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression != null)
			{
				PropertyInfo property = memberExpression.Member as PropertyInfo;
				if ((object)property != null)
				{
					ctrl.set_BasicTooltipText(property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name);
				}
			}
			return ctrl;
		}

		public static Control Create<T>(int width, int heigth, int x, int y)
		{
			List<ControlProvider> providers = Provider.Where((ControlProvider p) => (p as ControlProvider<T, T>)?.Types.Contains(typeof(T)) ?? false).ToList();
			if (providers.Count == 0)
			{
				if (typeof(T).IsEnum)
				{
					Register((ControlProvider<T, T>)Activator.CreateInstance(typeof(EnumProvider<>).MakeGenericType(typeof(T))));
					return Create<T>(width, heigth, x, y);
				}
				if (typeof(T).Name == typeof(List<>).Name)
				{
					Register((ControlProvider<T, T>)Activator.CreateInstance(typeof(ListViewProvider<>).MakeGenericType(typeof(T).GenericTypeArguments[0])));
					return Create<T>(width, heigth, x, y);
				}
				throw new NotSupportedException($"Control Type \"{typeof(T)}\" is not supported.");
			}
			return (providers.First() as ControlProvider<T, T>).CreateControl(null, null, null, null, width, heigth, x, y);
		}
	}
}
