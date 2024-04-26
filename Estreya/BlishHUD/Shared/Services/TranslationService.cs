using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Services
{
	public class TranslationService : ManagedService
	{
		private static readonly List<string> _locales = new List<string> { "en", "de", "es", "fr" };

		private readonly string _rootUrl;

		private IFlurlClient _flurlClient;

		private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _translations;

		public TranslationService(ServiceConfiguration configuration, IFlurlClient flurlClient, string rootUrl)
			: base(configuration)
		{
			_flurlClient = flurlClient;
			_rootUrl = rootUrl;
		}

		protected override Task Initialize()
		{
			_translations = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
			return Task.CompletedTask;
		}

		protected override Task Clear()
		{
			_translations?.Clear();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_translations?.Clear();
			_translations = null;
			_flurlClient = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override async Task Load()
		{
			await Task.WhenAll(_locales.Select(LoadLocale));
		}

		private async Task LoadLocale(string locale)
		{
			try
			{
				string obj = await _flurlClient.Request(_rootUrl, "translation." + locale + ".properties").WithTimeout(TimeSpan.FromSeconds(5.0)).GetStringAsync(default(CancellationToken), (HttpCompletionOption)0);
				ConcurrentDictionary<string, string> localeTranslations = new ConcurrentDictionary<string, string>();
				string[] array = obj.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					string[] lineParts = array[i].Trim('\n', '\r').Split('=');
					if (lineParts.Length >= 2)
					{
						string key = lineParts[0];
						string value = string.Join("=", lineParts.Skip(1));
						if (!localeTranslations.TryAdd(key, value))
						{
							Logger.Warn(key + " for locale " + locale + " already added.");
						}
					}
				}
				_translations.TryAdd(locale, localeTranslations);
				Logger.Debug($"Loaded {localeTranslations.Count} translations for locale {locale}");
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to load translations for locale " + locale + ":");
			}
		}

		public string GetTranslation(string key, string defaultValue = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				return defaultValue;
			}
			string result = default(string);
			if (!(GetTranslationsForLocale(Thread.CurrentThread.CurrentUICulture)?.TryGetValue(key, out result) ?? false))
			{
				return defaultValue;
			}
			return result;
		}

		private ConcurrentDictionary<string, string> GetTranslationsForLocale(CultureInfo locale)
		{
			CultureInfo tempLocale = locale;
			while (tempLocale != null && tempLocale.LCID != 127)
			{
				if (_translations.TryGetValue(tempLocale.Name, out var translations))
				{
					return translations;
				}
				tempLocale = tempLocale.Parent;
			}
			return null;
		}
	}
}