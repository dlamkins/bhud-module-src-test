using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using NAudio.Wave;
using Nekres.ChatMacros.Core.Services.Data;
using Nekres.ChatMacros.Core.Services.Speech;
using Nekres.ChatMacros.Core.UI.Configs;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services
{
	internal class SpeechService : IDisposable
	{
		private sealed class SpeechRecognizerDisplay : Control
		{
			private string _text = string.Empty;

			private DateTime _textExpiresAt;

			private readonly BitmapFont _font;

			private DateTime _lastTextCursorBlink;

			private DateTime _lastEllipsisBlink;

			private bool _inputDetected;

			private Color _redShift;

			private SpeechService _speech;

			public string Text
			{
				get
				{
					return _text;
				}
				set
				{
					((Control)this).SetProperty<string>(ref _text, value, false, "Text");
				}
			}

			public DateTime TextExpiresAt
			{
				get
				{
					return _textExpiresAt;
				}
				set
				{
					((Control)this).SetProperty<DateTime>(ref _textExpiresAt, value, false, "TextExpiresAt");
				}
			}

			private string ListeningText => Resources.Listening;

			private string NoInput => Resources.No_input_is_being_detected__Verify_your_settings_;

			public SpeechRecognizerDisplay(SpeechService speech)
				: this()
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				_speech = speech;
				_redShift = new Color(255, 57, 57);
				_font = ChatMacros.Instance.ContentsManager.GetBitmapFont("fonts/Lato-Regular.ttf", 60);
				((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				Rectangle contentRegion = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion();
				((Control)this).set_Size(((Rectangle)(ref contentRegion)).get_Size());
				((Control)this).set_ZIndex(2147483600);
				_speech.InputDetected += OnInputDetected;
				((Control)this).get_Parent().add_ContentResized((EventHandler<RegionChangedEventArgs>)OnParentResized);
			}

			private void OnInputDetected(object sender, ValueEventArgs<bool> e)
			{
				_inputDetected = e.get_Value();
			}

			protected override void OnShown(EventArgs e)
			{
				_text = string.Empty;
				((Control)this).OnShown(e);
			}

			private void OnParentResized(object sender, RegionChangedEventArgs e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				Rectangle currentRegion = e.get_CurrentRegion();
				((Control)this).set_Size(((Rectangle)(ref currentRegion)).get_Size());
			}

			protected override void DisposeControl()
			{
				if (((Control)this).get_Parent() != null)
				{
					((Control)this).get_Parent().remove_ContentResized((EventHandler<RegionChangedEventArgs>)OnParentResized);
				}
				_speech.InputDetected -= OnInputDetected;
				_font?.Dispose();
				((Control)this).DisposeControl();
			}

			protected override CaptureType CapturesInput()
			{
				return (CaptureType)1;
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_020d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0213: Unknown result type (might be due to invalid IL or missing references)
				//IL_0219: Unknown result type (might be due to invalid IL or missing references)
				//IL_022e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				Size2 listenSize = ((BitmapFont)_font).MeasureString(ListeningText);
				Rectangle listenBounds = default(Rectangle);
				((Rectangle)(ref listenBounds))._002Ector(bounds.X, bounds.Y - (int)Math.Round(listenSize.Height), bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ListeningText, (BitmapFont)(object)_font, listenBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
				DrawEllipsisCursor(spriteBatch, listenBounds, ListeningText, _font, ref _lastEllipsisBlink, Color.get_White(), stroke: true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
				if (!_inputDetected)
				{
					Size2 inputDetectedSize = ((BitmapFont)_font).MeasureString(NoInput);
					Rectangle inputDetectedBounds = default(Rectangle);
					((Rectangle)(ref inputDetectedBounds))._002Ector(bounds.X, listenBounds.Y - (int)Math.Round(inputDetectedSize.Height), bounds.Width, bounds.Height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, NoInput, (BitmapFont)(object)_font, inputDetectedBounds, Color.get_White(), false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
				}
				if (!string.IsNullOrWhiteSpace(_text))
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, (BitmapFont)(object)_font, bounds, Color.get_White(), false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
					DrawTextCursor(spriteBatch, _text, _font, bounds, ref _lastTextCursorBlink, Color.get_White(), stroke: true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
					if (DateTime.UtcNow < _textExpiresAt)
					{
						TimeSpan expiresIn = DateTime.UtcNow.Subtract(_textExpiresAt);
						string expireText = expiresIn.ToString((expiresIn.TotalSeconds > -1.0) ? "\\.ff" : ((expiresIn.TotalMinutes > -1.0) ? "ss\\.ff" : "m\\:ss")).TrimStart('0');
						Color expireColor = Color.Lerp(Color.get_White(), _redShift, 1f + (float)expiresIn.TotalMilliseconds / 1500f);
						Size2 textSize = ((BitmapFont)_font).MeasureString(_text);
						Rectangle expireBounds = default(Rectangle);
						((Rectangle)(ref expireBounds))._002Ector(bounds.X + (int)Math.Round(textSize.Width) + 4 + 1, bounds.Y, bounds.Width, bounds.Height);
						SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, expireText, (BitmapFont)(object)_font, expireBounds, expireColor, false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)1);
					}
				}
			}

			private void DrawTextCursor(SpriteBatch spriteBatch, string text, BitmapFont font, Rectangle bounds, ref DateTime lastTextCursorBlink, Color color, bool stroke = false, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1, string cursor = "_", int intervalMs = 250)
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Expected I4, but got Unknown
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Expected I4, but got Unknown
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_013d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_014f: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0175: Unknown result type (might be due to invalid IL or missing references)
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				double elapsedMilliseconds = DateTime.UtcNow.Subtract(lastTextCursorBlink).TotalMilliseconds;
				if (!(elapsedMilliseconds < (double)intervalMs))
				{
					if (elapsedMilliseconds >= (double)(intervalMs * 2))
					{
						lastTextCursorBlink = DateTime.UtcNow;
					}
					Size2 textSize = ((BitmapFont)font).MeasureString(text);
					Rectangle textBounds = default(Rectangle);
					((Rectangle)(ref textBounds))._002Ector((int)horizontalAlignment switch
					{
						0 => ((Rectangle)(ref bounds)).get_Left(), 
						1 => bounds.X + (bounds.Width - (int)Math.Round(textSize.Width)) / 2, 
						2 => ((Rectangle)(ref bounds)).get_Right() - (int)Math.Round(textSize.Width), 
						_ => ((Rectangle)(ref bounds)).get_Left(), 
					}, (int)verticalAlignment switch
					{
						0 => ((Rectangle)(ref bounds)).get_Top(), 
						1 => bounds.Y + (bounds.Height - (int)Math.Round(textSize.Height)) / 2, 
						2 => ((Rectangle)(ref bounds)).get_Bottom() - (int)Math.Round(textSize.Height), 
						_ => ((Rectangle)(ref bounds)).get_Top(), 
					}, (int)Math.Round(textSize.Width), (int)Math.Round(textSize.Height));
					Size2 cursorSize = ((BitmapFont)font).MeasureString(cursor);
					Rectangle cursorBounds = default(Rectangle);
					((Rectangle)(ref cursorBounds))._002Ector(((Rectangle)(ref textBounds)).get_Right(), textBounds.Y, (int)Math.Round(cursorSize.Width), (int)Math.Round(cursorSize.Height));
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, cursor, (BitmapFont)(object)font, cursorBounds, color, false, stroke, strokeDistance, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}

			private void DrawEllipsisCursor(SpriteBatch spriteBatch, Rectangle bounds, string text, BitmapFont font, ref DateTime lastTextCursorBlink, Color color, bool stroke = false, int strokeDistance = 1, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
			{
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				double totalMilliseconds = DateTime.UtcNow.Subtract(lastTextCursorBlink).TotalMilliseconds;
				int cycleDuration = 500;
				int count = (int)(totalMilliseconds % (double)cycleDuration / (double)((float)cycleDuration / 3f)) + 1;
				string ellipsis = new string('.', (count > 3) ? 3 : count);
				DrawTextCursor(spriteBatch, text, font, bounds, ref lastTextCursorBlink, color, stroke, strokeDistance, horizontalAlignment, verticalAlignment, ellipsis, 500);
			}
		}

		public const int SAMPLE_RATE = 16000;

		public const int CHANNELS = 1;

		public const int PARTIAL_RESULT_EXPIRE_MS = 1500;

		private Stream _audioStream;

		private Stream _secondaryAudioStream;

		private WaveInEvent _audioSource;

		private bool _isRecording;

		private ISpeechRecognitionProvider _recognizer;

		private SpeechRecognizerDisplay _display;

		private DateTime _lastSpeechDetected;

		private DateTime _partialResultExpiresAt;

		public IEnumerable<(int DeviceNumber, string ProductName, int Channels, Guid DeviceNameGuid, Guid ProductNameGuid, Guid ManufacturerGuid)> InputDevices
		{
			get
			{
				int deviceNumber = 0;
				while (deviceNumber < WaveInEvent.DeviceCount)
				{
					WaveInCapabilities device = WaveInEvent.GetCapabilities(deviceNumber);
					yield return (deviceNumber, device.ProductName, device.Channels, device.NameGuid, device.ProductGuid, device.ManufacturerGuid);
					int num = deviceNumber + 1;
					deviceNumber = num;
				}
			}
		}

		public event EventHandler<ValueEventArgs<bool>> InputDetected;

		public event EventHandler<ValueEventArgs<Stream>> SecondaryVoiceStreamChanged;

		public event EventHandler<ValueEventArgs<Stream>> VoiceStreamChanged;

		public event EventHandler<EventArgs> StartRecording;

		public event EventHandler<EventArgs> StopRecording;

		public SpeechService()
		{
			_recognizer = new WindowsSpeech(this);
			StartRecognizer();
			ChatMacros.Instance.InputConfig.add_SettingChanged((EventHandler<ValueChangedEventArgs<InputConfig>>)OnInputConfigChanged);
			_recognizer.PartialResult += OnPartialResultReceived;
			_recognizer.FinalResult += OnFinalResultReceived;
			_recognizer.SpeechDetected += OnSpeechDetected;
		}

		public void UpdateGrammar()
		{
			ChatMacros.Instance.Macro.UpdateMacros();
			_recognizer.ChangeGrammar(freeDictation: false, BaseMacro.GetCommands(ChatMacros.Instance.Macro.ActiveMacros));
		}

		private void StartRecognizer()
		{
			SpeechRecognizerDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
			SpeechRecognizerDisplay speechRecognizerDisplay = new SpeechRecognizerDisplay(this);
			((Control)speechRecognizerDisplay).set_Visible(false);
			_display = speechRecognizerDisplay;
			_recognizer.Reset(ChatMacros.Instance.InputConfig.get_Value().VoiceLanguage.Culture(), ChatMacros.Instance.InputConfig.get_Value().SecondaryVoiceLanguage.Culture(), freeDictation: false, BaseMacro.GetCommands(ChatMacros.Instance.Macro.ActiveMacros));
			ChangeDevice(ChatMacros.Instance.InputConfig.get_Value().InputDevice);
		}

		private void OnSpeechDetected(object sender, EventArgs e)
		{
			if (DateTime.UtcNow.Subtract(_lastSpeechDetected).TotalSeconds > 2.0)
			{
				_lastSpeechDetected = DateTime.UtcNow;
				this.InputDetected?.Invoke(this, new ValueEventArgs<bool>(true));
			}
		}

		private void OnPartialResultReceived(object sender, ValueEventArgs<string> e)
		{
			_partialResultExpiresAt = DateTime.UtcNow.AddMilliseconds(1500.0);
			_display.Text = e.get_Value();
			_display.TextExpiresAt = _partialResultExpiresAt;
		}

		private async void OnFinalResultReceived(object sender, ValueEventArgs<string> e)
		{
			BaseMacro macro = FastenshteinUtil.FindClosestMatchBy(e.get_Value(), ChatMacros.Instance.Macro.ActiveMacros, (BaseMacro m) => m.VoiceCommands);
			await ChatMacros.Instance.Macro.Trigger(macro);
		}

		private void OnInputConfigChanged(object sender, ValueChangedEventArgs<InputConfig> e)
		{
			if (e.get_NewValue() != null)
			{
				StartRecognizer();
			}
		}

		public void Update(GameTime gameTime)
		{
			if (ChatMacros.Instance.InputConfig.get_Value().PushToTalk != null)
			{
				if (Gw2Util.IsInGame() && ChatMacros.Instance.InputConfig.get_Value().PushToTalk.get_IsTriggering())
				{
					Start();
				}
				else
				{
					Stop();
				}
			}
			if (DateTime.UtcNow.Subtract(_lastSpeechDetected).TotalSeconds > 30.0)
			{
				_lastSpeechDetected = DateTime.UtcNow;
				this.InputDetected?.Invoke(this, new ValueEventArgs<bool>(false));
			}
			if (DateTime.UtcNow > _partialResultExpiresAt)
			{
				_display.Text = string.Empty;
				_recognizer.DiscardResult();
			}
		}

		private void ChangeDevice(Guid productNameGuid)
		{
			(int, string, int, Guid, Guid, Guid) device2 = InputDevices.FirstOrDefault(((int DeviceNumber, string ProductName, int Channels, Guid DeviceNameGuid, Guid ProductNameGuid, Guid ManufacturerGuid) device) => device.ProductNameGuid.Equals(productNameGuid));
			(int, string, int, Guid, Guid, Guid) tuple = device2;
			if (tuple.Item1 != 0 || !(tuple.Item2 == (string)null) || tuple.Item3 != 0 || !(tuple.Item4 == default(Guid)) || !(tuple.Item5 == default(Guid)) || !(tuple.Item6 == default(Guid)))
			{
				_audioSource?.Dispose();
				_audioSource = new WaveInEvent
				{
					DeviceNumber = device2.Item1,
					WaveFormat = new WaveFormat(16000, 1)
				};
				_audioStream?.Dispose();
				_audioStream = new SpeechStream(8192);
				_secondaryAudioStream?.Dispose();
				_secondaryAudioStream = new SpeechStream(8192);
				this.InputDetected?.Invoke(this, new ValueEventArgs<bool>(false));
				_audioSource.DataAvailable += OnDataAvailable;
				_audioSource.StartRecording();
				this.VoiceStreamChanged?.Invoke(this, new ValueEventArgs<Stream>(_audioStream));
				this.SecondaryVoiceStreamChanged?.Invoke(this, new ValueEventArgs<Stream>(_secondaryAudioStream));
			}
		}

		public void Stop()
		{
			if (_isRecording)
			{
				_isRecording = false;
				((Control)_display).Hide();
				this.StopRecording?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Start()
		{
			if (!_isRecording)
			{
				_isRecording = true;
				((Control)_display).Show();
				this.StartRecording?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnDataAvailable(object sender, WaveInEventArgs e)
		{
			_audioStream.Write(e.Buffer, 0, e.BytesRecorded);
			_secondaryAudioStream.Write(e.Buffer, 0, e.BytesRecorded);
		}

		public void Dispose()
		{
			_recognizer.PartialResult -= OnPartialResultReceived;
			_recognizer.FinalResult -= OnFinalResultReceived;
			_recognizer.SpeechDetected -= OnSpeechDetected;
			_recognizer?.Dispose();
			_audioSource?.Dispose();
			_audioStream?.Dispose();
			_secondaryAudioStream?.Dispose();
		}
	}
}
