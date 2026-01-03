using System;
using System.Threading.Tasks;
using Blish_HUD;
using LoreBridge.Models;
using LoreBridge.Translation;
using LoreBridge.Translation.Language;
using LoreBridge.Translation.Translators;
using Microsoft.Xna.Framework;

namespace LoreBridge.Services
{
	public class TranslationService : Service
	{
		private Settings _settings;

		private ITranslator _translator;

		private TranslatorConfig _translatorConfig;

		public override void Load(Settings settings)
		{
			_settings = settings;
			CreateTranslator((Translators)_settings.TranslationTranslator.get_Value());
			_settings.TranslationLanguage.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTranslationLanguageChanged);
			_settings.TranslationTranslator.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTranslationTranslatorChanged);
			_settings.TranslationLibreTranslateUrl.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnLibreTranslateUrlChanged);
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Unload()
		{
			_settings.TranslationLanguage.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTranslationLanguageChanged);
			_settings.TranslationTranslator.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTranslationTranslatorChanged);
			_settings.TranslationLibreTranslateUrl.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnLibreTranslateUrlChanged);
		}

		public async Task<string> TranslateAsync(string text)
		{
			string translation = await _translator.TranslateAsync(text).ConfigureAwait(continueOnCapturedContext: false);
			return (!string.IsNullOrWhiteSpace(translation)) ? translation : "";
		}

		private void CreateTranslator(Translators translator)
		{
			if (_translatorConfig == null)
			{
				_translatorConfig = new TranslatorConfig();
			}
			_translatorConfig.TargetLang = LanguagesInfo.GetByLanguage(_settings.TranslationLanguage.get_Value());
			if (_settings.TranslationTranslator.get_Value() == 4)
			{
				_translatorConfig.ApiUrl = _settings.TranslationLibreTranslateUrl.get_Value();
			}
			_translator?.Dispose();
			_translator = translator switch
			{
				Translators.Google => new Google(_translatorConfig), 
				Translators.Google2 => new Google2(_translatorConfig), 
				Translators.Yandex => new Yandex(_translatorConfig), 
				Translators.LibreTranslate => new LibreTranslate(_translatorConfig), 
				_ => null, 
			};
		}

		private void OnTranslationLanguageChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_translatorConfig.TargetLang = LanguagesInfo.GetByLanguage(e.get_NewValue());
		}

		private void OnTranslationTranslatorChanged(object sender, ValueChangedEventArgs<int> e)
		{
			CreateTranslator((Translators)e.get_NewValue());
		}

		private void OnLibreTranslateUrlChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (_settings.TranslationTranslator.get_Value() == 4)
			{
				_translatorConfig.ApiUrl = e.get_NewValue();
			}
		}
	}
}
