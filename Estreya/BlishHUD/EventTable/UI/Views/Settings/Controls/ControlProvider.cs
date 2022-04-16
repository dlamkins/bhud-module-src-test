using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls
{
	public abstract class ControlProvider
	{
		private static readonly Logger Logger = Logger.GetLogger<ControlProvider>();

		protected static List<ControlProvider> Provider { get; } = new List<ControlProvider>
		{
			new CheckboxProvider(),
			new TextBoxProvider(),
			new FloatTrackBarProvider(),
			new IntTrackBarProvider(),
			new KeybindingProvider()
		};


		protected static void Register<T>(ControlProvider<T> controlProvider)
		{
			lock (Provider)
			{
				if (Provider.Any(delegate(ControlProvider p)
				{
					ControlProvider<T> controlProvider2 = p as ControlProvider<T>;
					return controlProvider2 != null && controlProvider2.Type == controlProvider.Type;
				}))
				{
					throw new ArgumentException($"Control Type \"{controlProvider.Type}\" already registered.");
				}
				Provider.Add(controlProvider);
			}
		}

		public static Control Create<T>(SettingEntry<T> settingEntry, Func<SettingEntry<T>, T, bool> validationFunction, int width, int heigth, int x, int y)
		{
			List<ControlProvider> providers = Provider.Where(delegate(ControlProvider p)
			{
				ControlProvider<T> controlProvider = p as ControlProvider<T>;
				return controlProvider != null && controlProvider.Type == ((SettingEntry)settingEntry).get_SettingType();
			}).ToList();
			if (providers.Count == 0)
			{
				SettingEntry<T> obj = settingEntry;
				if (obj != null && ((SettingEntry)obj).get_SettingType().IsEnum)
				{
					Register((ControlProvider<T>)Activator.CreateInstance(typeof(EnumProvider<>).MakeGenericType(typeof(T))));
					return Create(settingEntry, validationFunction, width, heigth, x, y);
				}
				throw new NotSupportedException($"Control Type \"{((SettingEntry)settingEntry).get_SettingType()}\" is not supported.");
			}
			return (providers.First() as ControlProvider<T>).CreateControl(settingEntry, validationFunction, width, heigth, x, y);
		}

		public static Control Create<T>(int width, int heigth, int x, int y)
		{
			List<ControlProvider> providers = Provider.Where(delegate(ControlProvider p)
			{
				ControlProvider<T> controlProvider = p as ControlProvider<T>;
				return controlProvider != null && controlProvider.Type == typeof(T);
			}).ToList();
			if (providers.Count == 0)
			{
				if (typeof(T).IsEnum)
				{
					Register((ControlProvider<T>)Activator.CreateInstance(typeof(EnumProvider<>).MakeGenericType(typeof(T))));
					return Create<T>(width, heigth, x, y);
				}
				throw new NotSupportedException($"Control Type \"{typeof(T)}\" is not supported.");
			}
			return (providers.First() as ControlProvider<T>).CreateControl(null, null, width, heigth, x, y);
		}
	}
	public abstract class ControlProvider<T> : ControlProvider
	{
		public Type Type { get; }

		internal ControlProvider()
		{
			Type = typeof(T);
		}

		internal abstract Control CreateControl(SettingEntry<T> settingEntry, Func<SettingEntry<T>, T, bool> validationFunction, int width, int heigth, int x, int y);
	}
}
