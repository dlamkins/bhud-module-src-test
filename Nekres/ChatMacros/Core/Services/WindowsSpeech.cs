using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ChatMacros.Core.Services.Speech;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services
{
	internal class WindowsSpeech : ISpeechRecognitionProvider, IDisposable
	{
		private SpeechRecognitionEngine _recognizer;

		private SpeechRecognitionEngine _secondaryLanguageRecognizer;

		private CultureInfo _voiceCulture;

		private CultureInfo _secondaryVoiceCulture;

		private Grammar _grammar;

		private bool _isListening;

		private int _processing;

		private (float, string) _lastResult;

		private bool _lockResult;

		private AudioSignalProblem _lastAudioSignalProblem;

		private readonly SpeechService _speech;

		public event EventHandler<EventArgs> SpeechDetected;

		public event EventHandler<ValueEventArgs<string>> FinalResult;

		public event EventHandler<ValueEventArgs<string>> PartialResult;

		public WindowsSpeech(SpeechService speech)
		{
			_speech = speech;
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
		}

		public void DiscardResult()
		{
			if (!_lockResult)
			{
				_lastResult = default((float, string));
				_lastAudioSignalProblem = AudioSignalProblem.None;
			}
		}

		private void OnVoiceStreamChanged(object sender, ValueEventArgs<Stream> e)
		{
			ChangeDevice(_recognizer, e.get_Value());
		}

		private void OnSecondaryVoiceStreamChanged(object sender, ValueEventArgs<Stream> e)
		{
			ChangeDevice(_secondaryLanguageRecognizer, e.get_Value());
		}

		private void OnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			_lastResult = default((float, string));
			_lastAudioSignalProblem = AudioSignalProblem.None;
			this.PartialResult?.Invoke(this, new ValueEventArgs<string>(string.Empty));
		}

		private void ChangeDevice(SpeechRecognitionEngine recognizer, Stream stream)
		{
			if (recognizer != null)
			{
				recognizer.RecognizeAsyncCancel();
				recognizer.SetInputToAudioStream(stream, new SpeechAudioFormatInfo(16000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
				recognizer.RecognizeAsync(RecognizeMode.Multiple);
			}
		}

		public void Reset(CultureInfo lang, CultureInfo secondaryLanguage, bool freeDictation, params string[] grammar)
		{
			Disable();
			if (grammar == null || grammar.Length >= 1)
			{
				ChangeModel(lang, secondaryLanguage);
				if (CreateRecognizers())
				{
					ChangeGrammar(freeDictation, grammar);
					RegisterDelegates(_recognizer);
					RegisterDelegates(_secondaryLanguageRecognizer);
					_speech.StartRecording += OnStartRecording;
					_speech.StopRecording += OnStopRecording;
					_speech.VoiceStreamChanged += OnVoiceStreamChanged;
					_speech.SecondaryVoiceStreamChanged += OnSecondaryVoiceStreamChanged;
				}
			}
		}

		private void Disable()
		{
			_speech.StartRecording -= OnStartRecording;
			_speech.StopRecording -= OnStopRecording;
			_speech.VoiceStreamChanged -= OnVoiceStreamChanged;
			_speech.SecondaryVoiceStreamChanged -= OnSecondaryVoiceStreamChanged;
			_recognizer?.Dispose();
			_recognizer = null;
			_secondaryLanguageRecognizer?.Dispose();
			_secondaryLanguageRecognizer = null;
		}

		private void RegisterDelegates(SpeechRecognitionEngine recognizer)
		{
			if (recognizer != null)
			{
				recognizer.SpeechHypothesized += OnSpeechRecorded;
				recognizer.SpeechRecognized += OnSpeechRecorded;
				recognizer.SpeechRecognitionRejected += OnSpeechRecorded;
				recognizer.AudioSignalProblemOccurred += OnAudioSignalProblemOccurred;
				recognizer.SpeechDetected += OnSpeechDetected;
			}
		}

		private void OnSpeechDetected(object sender, SpeechDetectedEventArgs e)
		{
			this.SpeechDetected?.Invoke(this, EventArgs.Empty);
		}

		private bool CreateRecognizers()
		{
			int recognizers = 0;
			try
			{
				_recognizer = new SpeechRecognitionEngine(_voiceCulture);
				_recognizer.MaxAlternates = 1;
				recognizers++;
			}
			catch (Exception e2)
			{
				ScreenNotification.ShowNotification(string.Format(Resources.Speech_recognition_for__0__is_not_installed_, "'" + _voiceCulture.DisplayName + "'"), (NotificationType)2, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
				ChatMacros.Logger.Warn(e2, "Speech recognition for '" + _voiceCulture.EnglishName + "' is not installed on the system.");
			}
			if (_voiceCulture.Equals(_secondaryVoiceCulture))
			{
				return recognizers > 0;
			}
			try
			{
				_secondaryLanguageRecognizer = new SpeechRecognitionEngine(_secondaryVoiceCulture);
				_secondaryLanguageRecognizer.MaxAlternates = 1;
				recognizers++;
			}
			catch (Exception e)
			{
				ScreenNotification.ShowNotification(string.Format(Resources.Speech_recognition_for__0__is_not_installed_, "'" + _secondaryVoiceCulture.DisplayName + "'"), (NotificationType)2, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
				ChatMacros.Logger.Warn(e, "Speech recognition for '" + _secondaryVoiceCulture.EnglishName + "' is not installed on the system.");
			}
			return recognizers > 0;
		}

		public static bool TestVoiceLanguage(CultureInfo culture)
		{
			try
			{
				using (new SpeechRecognitionEngine(culture))
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void ChangeModel(CultureInfo lang, CultureInfo secondaryLang)
		{
			_voiceCulture = lang;
			_secondaryVoiceCulture = secondaryLang;
		}

		private void ChangeGrammar(SpeechRecognitionEngine recognizer, bool freeDictation, params string[] grammar)
		{
			if (recognizer != null && (grammar == null || grammar.Length >= 1))
			{
				if (recognizer.Grammars?.Any() ?? false)
				{
					recognizer.UnloadAllGrammars();
				}
				if (freeDictation)
				{
					_grammar = new DictationGrammar();
				}
				else
				{
					_grammar = new Grammar(new GrammarBuilder(new Choices(grammar))
					{
						Culture = recognizer.RecognizerInfo.Culture
					})
					{
						Name = Guid.NewGuid().ToString("n")
					};
				}
				recognizer.LoadGrammar(_grammar);
			}
		}

		public void ChangeGrammar(bool freeDictation, params string[] grammar)
		{
			ChangeGrammar(_recognizer, freeDictation, grammar);
			ChangeGrammar(_secondaryLanguageRecognizer, freeDictation, grammar);
		}

		private void OnSpeechRecorded(object sender, RecognitionEventArgs e)
		{
			Interlocked.Increment(ref _processing);
			ProcessSpeech(e.Result?.Alternates);
			Interlocked.Decrement(ref _processing);
		}

		private void ProcessSpeech(IEnumerable<RecognizedPhrase> phrases)
		{
			RecognizedWordUnit best = (from phrase in phrases?.SelectMany((RecognizedPhrase phrase) => phrase.Words)
				orderby phrase.Confidence descending
				select phrase).FirstOrDefault();
			ProcessWord(best);
		}

		private void ProcessWord(RecognizedWordUnit word)
		{
			if (word == null || word.Confidence < 0.3f)
			{
				return;
			}
			if (string.Equals(_lastResult.Item2, word.Text, StringComparison.InvariantCultureIgnoreCase))
			{
				if (_lastResult.Item1 < word.Confidence)
				{
					_lastResult = (word.Confidence, word.Text);
				}
			}
			else
			{
				_lastResult = (word.Confidence, word.Text);
				this.PartialResult?.Invoke(this, new ValueEventArgs<string>(_lastResult.Item2));
			}
		}

		private void OnAudioSignalProblemOccurred(object sender, AudioSignalProblemOccurredEventArgs e)
		{
			_lastAudioSignalProblem = e.AudioSignalProblem;
		}

		private void InvokeResultAvailable()
		{
			string bestResult = _lastResult.Item2;
			_lastResult = default((float, string));
			if (!string.IsNullOrEmpty(bestResult) && _isListening)
			{
				this.FinalResult?.Invoke(this, new ValueEventArgs<string>(bestResult));
			}
			else if (_lastAudioSignalProblem != 0)
			{
				ScreenNotification.ShowNotification("Audio signal problem: " + _lastAudioSignalProblem.ToString().SplitCamelCase(), (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async void OnStopRecording(object sender, EventArgs e)
		{
			_lockResult = true;
			do
			{
				await Task.Delay(650);
			}
			while (Interlocked.CompareExchange(ref _processing, 0, 0) > 0);
			InvokeResultAvailable();
			_isListening = false;
			_lockResult = false;
		}

		private void OnStartRecording(object sender, EventArgs e)
		{
			_lastAudioSignalProblem = AudioSignalProblem.None;
			_isListening = true;
		}

		public void Dispose()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			Disable();
		}
	}
}
