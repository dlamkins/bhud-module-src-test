using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Frtal.LorebookReader
{
	[Export(typeof(Module))]
	public class LorebookReaderModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<LorebookReaderModule>();

		private SettingEntry<KeyBinding> _readKeybind;

		private SettingEntry<KeyBinding> _stopKeybind;

		private SettingEntry<bool> _showSpeakerButton;

		private SettingEntry<string> _voiceName;

		private SettingEntry<float> _speakingRate;

		private SettingEntry<string> _ocrLanguage;

		private SettingEntry<string> _voiceEngine;

		private SettingEntry<string> _edgeVoice;

		private SettingEntry<bool> _showSubtitles;

		private SettingEntry<float> _subtitleOpacity;

		private SettingEntry<float> _subtitleX;

		private SettingEntry<float> _subtitleY;

		private SettingEntry<int> _subtitleFontSize;

		private SettingEntry<string> _translateMode;

		private SettingEntry<string> _translateTarget;

		private SettingEntry<bool> _conversationCapture;

		private SettingEntry<KeyBinding> _convToggleKeybind;

		private SettingEntry<KeyBinding> _debugDumpKeybind;

		private SettingEntry<string> _dialogZone;

		private SettingEntry<KeyBinding> _calibrateKeybind;

		private SettingEntry<string> _bookZone;

		private SettingEntry<KeyBinding> _bookCalibrateKeybind;

		private SettingEntry<bool> _encRailCollapsed;

		private SettingEntry<KeyBinding> _pauseKeybind;

		private volatile bool _speechPaused;

		private TtsService _tts;

		private EdgeTtsService _edgeTts;

		private LorebookCatalog _catalog;

		private CornerIcon _cornerIcon;

		private StandardWindow _historyWindow;

		private volatile bool _catalogDirty;

		private readonly ConcurrentQueue<Action> _mainThreadQueue = new ConcurrentQueue<Action>();

		private TextRenderer _textRenderer;

		private EncyclopediaView _encyclopediaView;

		private Texture2D _parchmentTexture;

		private int _speakSession;

		private volatile bool _chunkTranslate;

		private string _chunkTranslateTarget = "cs";

		private BookActionButton _speakerButton;

		private BookActionButton _saveButton;

		private BookActionButton _appendButton;

		private SubtitleOverlay _subtitleLabel;

		private volatile bool _subtitleDirty;

		private string _pendingSubtitle;

		private volatile string _cueSource;

		private volatile bool _cueSourceDirty;

		private List<string> _cues;

		private int _cueIndex;

		private double _cueElapsedMs;

		private int[] _cueWordStart;

		private volatile int _currentWordIndex;

		private volatile bool _haveWordEvents;

		private volatile bool _wordSyncValid = true;

		private int _lastSubWidth = -1;

		private int _lastFontSize = -1;

		private int _subWidthCap = -1;

		private bool _readBusy;

		private bool _detectBusy;

		private double _detectTimerMs;

		private volatile bool _bookVisible;

		private Rectangle _bookBox;

		private Rectangle _gw2ClientRect;

		private volatile bool _convVisible;

		private Rectangle _convBox;

		private DialogZoneCalibrator _calibrator;

		private bool _calibNudgeShown;

		private int _dumpBusy;

		private Rectangle _bookCalibBox;

		private readonly Dictionary<string, Texture2D> _xpIconCache = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);

		private bool _unloaded;

		internal SettingEntry<KeyBinding> ReadKeybindSetting => _readKeybind;

		internal SettingEntry<KeyBinding> StopKeybindSetting => _stopKeybind;

		internal SettingEntry<bool> ShowSpeakerButtonSetting => _showSpeakerButton;

		internal SettingEntry<string> VoiceNameSetting => _voiceName;

		internal SettingEntry<float> SpeakingRateSetting => _speakingRate;

		internal SettingEntry<string> OcrLanguageSetting => _ocrLanguage;

		internal SettingEntry<string> VoiceEngineSetting => _voiceEngine;

		internal SettingEntry<string> EdgeVoiceSetting => _edgeVoice;

		internal SettingEntry<bool> ShowSubtitlesSetting => _showSubtitles;

		internal SettingEntry<float> SubtitleOpacitySetting => _subtitleOpacity;

		internal SettingEntry<float> SubtitleXSetting => _subtitleX;

		internal SettingEntry<float> SubtitleYSetting => _subtitleY;

		internal SettingEntry<int> SubtitleFontSizeSetting => _subtitleFontSize;

		internal SettingEntry<string> TranslateModeSetting => _translateMode;

		internal SettingEntry<string> TranslateTargetSetting => _translateTarget;

		internal SettingEntry<bool> ConversationCaptureSetting => _conversationCapture;

		internal SettingEntry<KeyBinding> ConvToggleKeybindSetting => _convToggleKeybind;

		internal SettingEntry<KeyBinding> DebugDumpKeybindSetting => _debugDumpKeybind;

		internal SettingEntry<string> DialogZoneSetting => _dialogZone;

		internal SettingEntry<KeyBinding> CalibrateKeybindSetting => _calibrateKeybind;

		internal SettingEntry<string> BookZoneSetting => _bookZone;

		internal SettingEntry<KeyBinding> BookCalibrateKeybindSetting => _bookCalibrateKeybind;

		internal SettingEntry<bool> EncyclopediaRailCollapsedSetting => _encRailCollapsed;

		internal TextRenderer SharedTextRenderer => _textRenderer;

		internal bool SubtitleEditMode
		{
			get
			{
				if (_subtitleLabel != null)
				{
					return _subtitleLabel.EditMode;
				}
				return false;
			}
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				if (_subtitleLabel != null)
				{
					if (value)
					{
						Point sprite = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
						int width = Math.Max(100, (int)((float)sprite.X * 0.45f));
						_subtitleLabel.BoxWidth = width;
						_lastSubWidth = width;
						_subtitleLabel.EditMode = true;
						int sx = (int)((float)sprite.X * (_subtitleX.get_Value() / 100f)) - width / 2;
						int sy = (int)((float)sprite.Y * (_subtitleY.get_Value() / 100f));
						((Control)_subtitleLabel).set_Location(new Point(Math.Max(0, Math.Min(sx, sprite.X - width)), Math.Max(0, Math.Min(sy, sprite.Y - Math.Max(1, ((Control)_subtitleLabel).get_Size().Y)))));
					}
					else
					{
						_subtitleLabel.EditMode = false;
					}
				}
			}
		}

		internal LorebookCatalog Catalog => _catalog;

		internal string StoryImageDirectory { get; private set; }

		internal bool SpeechPaused => _speechPaused;

		[ImportingConstructor]
		public LorebookReaderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new LorebookSettingsView(this);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Expected O, but got Unknown
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_068b: Expected O, but got Unknown
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Expected O, but got Unknown
			//IL_0799: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e1: Expected O, but got Unknown
			_readKeybind = settings.DefineSetting<KeyBinding>("ReadKeybind", new KeyBinding((ModifierKeys)3, (Keys)82), (Func<string>)(() => "Read lorebook"), (Func<string>)(() => "Reads the currently open lorebook aloud."));
			_stopKeybind = settings.DefineSetting<KeyBinding>("StopKeybind", new KeyBinding((ModifierKeys)3, (Keys)83), (Func<string>)(() => "Stop reading"), (Func<string>)(() => "Stops the current text-to-speech playback."));
			_pauseKeybind = settings.DefineSetting<KeyBinding>("PauseKeybind", new KeyBinding((ModifierKeys)3, (Keys)80), (Func<string>)(() => "Pause / resume reading"), (Func<string>)(() => "Pauses the narration and picks it up where it stopped."));
			_showSpeakerButton = settings.DefineSetting<bool>("ShowSpeakerButton", true, (Func<string>)(() => "Show speaker icon on open books"), (Func<string>)(() => "Displays a clickable speaker icon next to a detected lorebook."));
			_voiceName = settings.DefineSetting<string>("VoiceName", "", (Func<string>)(() => "Voice (part of name)"), (Func<string>)(() => "Leave empty for default. Available voices are listed in the log on module start."));
			_speakingRate = settings.DefineSetting<float>("SpeakingRate", 1f, (Func<string>)(() => "Speaking rate"), (Func<string>)(() => "1.0 = normal speed."));
			SettingComplianceExtensions.SetRange(_speakingRate, 0.5f, 2f);
			_ocrLanguage = settings.DefineSetting<string>("OcrLanguage", "en-US", (Func<string>)(() => "OCR language"), (Func<string>)(() => "Language of your GW2 client: en-US, de-DE, fr-FR or es-ES."));
			_voiceEngine = settings.DefineSetting<string>("VoiceEngine", "windows", (Func<string>)(() => "Voice engine"), (Func<string>)(() => "Windows = offline (private). Edge = online neural voices via a free Microsoft endpoint (sends text to a third party)."));
			_edgeVoice = settings.DefineSetting<string>("EdgeVoice", "en-GB-RyanNeural", (Func<string>)(() => "Edge neural voice"), (Func<string>)(() => "Used when the voice engine is set to Edge."));
			_showSubtitles = settings.DefineSetting<bool>("ShowSubtitles", true, (Func<string>)(() => "Show subtitles"), (Func<string>)(() => "Displays the text being read as an on-screen overlay."));
			_subtitleOpacity = settings.DefineSetting<float>("SubtitleOpacity", 0.9f, (Func<string>)(() => "Subtitle opacity"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_subtitleOpacity, 0.2f, 1f);
			_subtitleX = settings.DefineSetting<float>("SubtitleX", 50f, (Func<string>)(() => "Subtitle horizontal position (%)"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_subtitleX, 0f, 100f);
			_subtitleY = settings.DefineSetting<float>("SubtitleY", 82f, (Func<string>)(() => "Subtitle vertical position (%)"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(_subtitleY, 0f, 100f);
			_subtitleFontSize = settings.DefineSetting<int>("SubtitleFontSize", 24, (Func<string>)(() => "Subtitle size"), (Func<string>)(() => ""));
			_translateMode = settings.DefineSetting<string>("TranslateMode", "off", (Func<string>)(() => "Translation"), (Func<string>)(() => "Off / subtitles only / subtitles + speech. Uses a free online translation endpoint (sends text to a third party)."));
			_translateTarget = settings.DefineSetting<string>("TranslateTarget", "cs", (Func<string>)(() => "Translate to"), (Func<string>)(() => "Target language for translation."));
			_conversationCapture = settings.DefineSetting<bool>("ConversationCapture", false, (Func<string>)(() => "Conversation capture mode"), (Func<string>)(() => "When enabled, also detects NPC dialogue windows (not just lorebooks). Useful for saving story conversations to the encyclopedia."));
			_convToggleKeybind = settings.DefineSetting<KeyBinding>("ConvToggleKeybind", new KeyBinding((ModifierKeys)3, (Keys)67), (Func<string>)(() => "Toggle conversation capture"), (Func<string>)(() => "Quickly turn conversation capture on/off during gameplay."));
			_debugDumpKeybind = settings.DefineSetting<KeyBinding>("DebugDumpKeybind", new KeyBinding((ModifierKeys)3, (Keys)68), (Func<string>)(() => "Save debug capture"), (Func<string>)(() => "Saves the current frame, detector results and OCR output to the lorebook_reader\\debug folder. Attach that folder to bug reports."));
			_dialogZone = settings.DefineSetting<string>("DialogZone", "", (Func<string>)(() => "Calibrated dialogue zone"), (Func<string>)(() => "Internal store of the calibrated dialogue area (pixels + resolution stamp)."));
			_calibrateKeybind = settings.DefineSetting<KeyBinding>("CalibrateKeybind", new KeyBinding((ModifierKeys)3, (Keys)90), (Func<string>)(() => "Calibrate dialogue zone"), (Func<string>)(() => "Opens a draggable frame to mark where dialogue text appears. Do this once per screen resolution."));
			_bookZone = settings.DefineSetting<string>("BookZone", "", (Func<string>)(() => "Lorebook OCR area (calibrated)"), (Func<string>)(() => "Internal store of the calibrated lorebook OCR area."));
			_bookCalibrateKeybind = settings.DefineSetting<KeyBinding>("BookCalibrateKeybind", new KeyBinding((ModifierKeys)3, (Keys)66), (Func<string>)(() => "Calibrate lorebook OCR area"), (Func<string>)(() => "Open a lorebook, then press this to drag a frame over the book text. Fixes text getting cut off. Once per resolution."));
			_encRailCollapsed = settings.DefineSetting<bool>("EncyclopediaRailCollapsed", false, (Func<string>)(() => "Encyclopedia expansion rail collapsed"), (Func<string>)(() => "Internal: the expansion rail shows icons only."));
		}

		protected override async Task LoadAsync()
		{
			_tts = new TtsService();
			_edgeTts = new EdgeTtsService();
			_textRenderer = new TextRenderer(GameService.Graphics.get_GraphicsDeviceManager().get_GraphicsDevice());
			string dir = base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("lorebook_reader");
			_catalog = new LorebookCatalog(dir);
			_catalog.Changed += delegate
			{
				_catalogDirty = true;
			};
			StoryImageDirectory = Path.Combine(dir, "images");
			BookReaderPanel.ImageDirectory = StoryImageDirectory;
			Logger.Info("Available TTS voices: " + string.Join(", ", from v in TtsService.InstalledVoices()
				select v.Name + " [" + v.Lang + "]"));
			await Task.CompletedTask;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Expected O, but got Unknown
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Expected O, but got Unknown
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Expected O, but got Unknown
			_readKeybind.get_Value().set_Enabled(true);
			_readKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnReadActivated);
			_stopKeybind.get_Value().set_Enabled(true);
			_stopKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnStopActivated);
			_pauseKeybind.get_Value().set_Enabled(true);
			_pauseKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnPauseActivated);
			_convToggleKeybind.get_Value().set_Enabled(true);
			_convToggleKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnConvToggleActivated);
			_debugDumpKeybind.get_Value().set_Enabled(true);
			_debugDumpKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnDebugDumpActivated);
			_calibrateKeybind.get_Value().set_Enabled(true);
			_calibrateKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnCalibrateActivated);
			_bookCalibrateKeybind.get_Value().set_Enabled(true);
			_bookCalibrateKeybind.get_Value().add_Activated((EventHandler<EventArgs>)OnBookCalibrateActivated);
			if (!string.IsNullOrEmpty(_catalog?.LoadWarning))
			{
				Logger.Warn("Catalog load warning: " + _catalog.LoadWarning);
				ScreenNotification.ShowNotification("Lorebook Reader: " + _catalog.LoadWarning, (NotificationType)0, (Texture2D)null, 4);
			}
			_speakerButton = MakeBookButton("speaker", "Read this book aloud", delegate
			{
				StartRead();
			});
			_saveButton = MakeBookButton("save", "Save to encyclopedia (don't read)", delegate
			{
				StartSaveOnly();
			});
			_appendButton = MakeBookButton("append", "Append this page to the last saved book", delegate
			{
				StartAppend();
			});
			SubtitleOverlay subtitleOverlay = new SubtitleOverlay(_textRenderer);
			((Control)subtitleOverlay).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			subtitleOverlay.FontSize = _subtitleFontSize.get_Value();
			subtitleOverlay.BoxWidth = 600;
			((Control)subtitleOverlay).set_Visible(false);
			_subtitleLabel = subtitleOverlay;
			_lastFontSize = _subtitleFontSize.get_Value();
			_subWidthCap = (int)_textRenderer.MeasureWidth(new string('n', 42), _lastFontSize) + 16;
			_subtitleLabel.PositionEdited += delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				Point size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				if (size.X > 0 && size.Y > 0)
				{
					float val3 = ((float)((Control)_subtitleLabel).get_Location().X + (float)((Control)_subtitleLabel).get_Width() / 2f) / (float)size.X * 100f;
					float val4 = (float)((Control)_subtitleLabel).get_Location().Y / (float)size.Y * 100f;
					_subtitleX.set_Value(Math.Max(0f, Math.Min(100f, val3)));
					_subtitleY.set_Value(Math.Max(0f, Math.Min(100f, val4)));
				}
			};
			_parchmentTexture = base.ModuleParameters.get_ContentsManager().GetTexture("parchment.png");
			_cornerIcon = new CornerIcon(AsyncTexture2D.op_Implicit(base.ModuleParameters.get_ContentsManager().GetTexture("book.png")), AsyncTexture2D.op_Implicit(base.ModuleParameters.get_ContentsManager().GetTexture("book_hover.png")), "Lorebook Codex");
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_historyWindow).get_Visible())
				{
					_encyclopediaView?.FlushEdits();
					((Control)_historyWindow).Hide();
				}
				else
				{
					ShowEncyclopedia();
				}
			});
			Texture2D winTex = GetRefTexture("window_bg.png");
			if (winTex != null)
			{
				int tw = winTex.get_Width();
				int th = winTex.get_Height();
				StandardWindow val = new StandardWindow(winTex, new Rectangle(0, 0, tw, th), new Rectangle((int)((float)tw * 0.03f), (int)((float)th * 0.078f), (int)((float)tw * 0.94f), (int)((float)th * 0.88f)), new Point(Math.Min(1120, tw), Math.Min(800, th)));
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val).set_Title("Lorebook Codex");
				((WindowBase2)val).set_SavesPosition(true);
				((WindowBase2)val).set_SavesSize(true);
				((WindowBase2)val).set_CanResize(true);
				((WindowBase2)val).set_Id("frtal_lorebook_reader_encyclopedia_v2");
				_historyWindow = val;
			}
			else
			{
				StandardWindow val2 = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605), new Point(1120, 800));
				((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val2).set_Title("Lorebook Codex");
				((WindowBase2)val2).set_SavesPosition(true);
				((WindowBase2)val2).set_CanResize(true);
				((WindowBase2)val2).set_Id("frtal_lorebook_reader_encyclopedia_v2");
				_historyWindow = val2;
			}
			((Control)_historyWindow).add_Hidden((EventHandler<EventArgs>)delegate
			{
				_encyclopediaView?.FlushEdits();
			});
			((Module)this).OnModuleLoaded(e);
		}

		private BookActionButton MakeBookButton(string iconKey, string tooltip, Action onClick)
		{
			Texture2D texture = base.ModuleParameters.get_ContentsManager().GetTexture(iconKey + ".png");
			Texture2D hover = base.ModuleParameters.get_ContentsManager().GetTexture(iconKey + "_hover.png");
			BookActionButton bookActionButton = new BookActionButton(texture, hover, tooltip, onClick);
			((Control)bookActionButton).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			return bookActionButton;
		}

		private void OnReadActivated(object sender, EventArgs e)
		{
			StartRead();
		}

		private void OnStopActivated(object sender, EventArgs e)
		{
			_tts.Stop();
			_edgeTts?.Stop();
			_speechPaused = false;
		}

		private void OnPauseActivated(object sender, EventArgs e)
		{
			TogglePauseSpeaking();
		}

		private void OnDebugDumpActivated(object sender, EventArgs e)
		{
			if (Interlocked.CompareExchange(ref _dumpBusy, 1, 0) == 0)
			{
				Task.Run((Func<Task>)DebugDumpAsync);
			}
		}

		private async Task DebugDumpAsync()
		{
			try
			{
				string root = base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("lorebook_reader");
				string dir = Path.Combine(root, "debug", "dump_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
				Directory.CreateDirectory(dir);
				StringBuilder info = new StringBuilder();
				info.AppendLine("Lorebook Reader debug capture");
				info.AppendLine("Time: " + DateTime.Now.ToString("o"));
				info.AppendLine("Module version: " + (((object)base.ModuleParameters.get_Manifest()).GetType().GetProperty("Version")?.GetValue(base.ModuleParameters.get_Manifest())?.ToString() ?? "?"));
				info.AppendLine("OCR language: " + _ocrLanguage.get_Value());
				info.AppendLine("Conversation capture: " + (_conversationCapture.get_Value() ? "ON" : "OFF"));
				IntPtr hwnd = GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle();
				Rectangle screenRect;
				using (Bitmap screen = ScreenCapture.Grab(hwnd, out screenRect))
				{
					info.AppendLine($"Client area: {screen.Width}x{screen.Height}");
					screen.Save(Path.Combine(dir, "frame.png"), ImageFormat.Png);
					double pSol;
					Rectangle? parch = ParchmentDetector.Find(screen, out pSol);
					info.AppendLine(parch.HasValue ? $"ParchmentDetector: {parch} solidity {pSol:0.000}" : "ParchmentDetector: no hit");
					ConversationHit convHit = ConversationDetector.FindHit(screen);
					object value;
					if (convHit == null)
					{
						value = "ConversationDetector: no hit";
					}
					else
					{
						screenRect = convHit.Panel;
						value = "ConversationDetector: panel " + screenRect.ToString() + $" text {convHit.TextArea}" + $" confidence {convHit.Confidence:0.000}";
					}
					info.AppendLine((string)value);
					if (!string.IsNullOrEmpty(ConversationDetector.LastDiagnostics))
					{
						info.AppendLine("Detector diagnostics:");
						info.AppendLine(ConversationDetector.LastDiagnostics);
					}
					Rectangle? zoneDbg = GetCalibratedZone(screen.Width, screen.Height);
					ConversationHit convUse = convHit;
					if (zoneDbg.HasValue)
					{
						ConversationHit zHit = ConversationDetector.MeasureInZone(screen, zoneDbg.Value);
						info.AppendLine($"Calibrated zone: {zoneDbg.Value}");
						info.AppendLine((zHit != null) ? ($"Zone measure: text {zHit.TextArea} " + $"conf {zHit.Confidence:0.000}") : "Zone measure: no bright text");
						if (!parch.HasValue && zHit != null)
						{
							convUse = zHit;
						}
					}
					else
					{
						info.AppendLine("Calibrated zone: none");
					}
					bool isConversation = !parch.HasValue && convUse != null;
					Rectangle? box = parch ?? convUse?.Panel;
					if (box.HasValue)
					{
						Rectangle? bookCropDbg = (parch.HasValue ? GetCalibratedBookCrop(parch.Value) : null);
						Rectangle inner = (isConversation ? convUse.TextArea : (bookCropDbg ?? ParchmentDetector.InnerCrop(box.Value)));
						inner = Rectangle.Intersect(inner, new Rectangle(0, 0, screen.Width, screen.Height));
						if (!isConversation)
						{
							info.AppendLine("Book crop: " + (bookCropDbg.HasValue ? "calibrated size (auto-positioned)" : "auto InnerCrop"));
						}
						info.AppendLine("Detector used: " + (isConversation ? "conversation" : "parchment"));
						info.AppendLine($"Text crop: {inner}");
						using Bitmap crop = screen.Clone(inner, screen.PixelFormat);
						crop.Save(Path.Combine(dir, "crop.png"), ImageFormat.Png);
						string raw = await OcrService.RecognizeAsync(crop, _ocrLanguage.get_Value(), isConversation);
						File.WriteAllText(Path.Combine(dir, "ocr_raw.txt"), raw ?? "");
						File.WriteAllText(Path.Combine(dir, "ocr_clean.txt"), TextCleaner.CleanForEncyclopedia(raw ?? ""));
					}
					else
					{
						info.AppendLine("No panel detected — frame.png saved for calibration.");
					}
				}
				File.WriteAllText(Path.Combine(dir, "info.txt"), info.ToString());
				ScreenNotification.ShowNotification("Debug capture saved: debug\\" + Path.GetFileName(dir), (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Debug capture saved to " + dir);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Debug capture failed.");
				ScreenNotification.ShowNotification("Debug capture failed: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			finally
			{
				Interlocked.Exchange(ref _dumpBusy, 0);
			}
		}

		private void OnConvToggleActivated(object sender, EventArgs e)
		{
			_conversationCapture.set_Value(!_conversationCapture.get_Value());
			string state = (_conversationCapture.get_Value() ? "ON" : "OFF");
			ScreenNotification.ShowNotification("Conversation capture: " + state, (NotificationType)0, (Texture2D)null, 4);
			if (!_conversationCapture.get_Value())
			{
				_convVisible = false;
			}
			else
			{
				MaybeNudgeCalibration();
			}
		}

		private void MaybeNudgeCalibration()
		{
			if (!_calibNudgeShown && string.IsNullOrEmpty(_dialogZone.get_Value()))
			{
				_calibNudgeShown = true;
				ScreenNotification.ShowNotification("Tip: press Ctrl+Alt+Z to mark where dialogues appear — detection then gets much more reliable.", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		internal void StartCalibration()
		{
			OnCalibrateActivated(null, EventArgs.Empty);
		}

		private void OnCalibrateActivated(object sender, EventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			if (_calibrator == null)
			{
				Point sp = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				Rectangle? zone = GetCalibratedZone(_gw2ClientRect.Width, _gw2ClientRect.Height);
				Rectangle start = default(Rectangle);
				if (zone.HasValue && _gw2ClientRect.Width > 0 && _gw2ClientRect.Height > 0)
				{
					float sx = (float)sp.X / (float)_gw2ClientRect.Width;
					float sy = (float)sp.Y / (float)_gw2ClientRect.Height;
					((Rectangle)(ref start))._002Ector((int)((float)zone.Value.X * sx), (int)((float)zone.Value.Y * sy), (int)((float)zone.Value.Width * sx), (int)((float)zone.Value.Height * sy));
				}
				else
				{
					((Rectangle)(ref start))._002Ector((int)((float)sp.X * 0.35f), (int)((float)sp.Y * 0.06f), (int)((float)sp.X * 0.22f), (int)((float)sp.Y * 0.1f));
				}
				_calibrator = new DialogZoneCalibrator(start, OnCalibrationSaved, CloseCalibrator);
			}
		}

		private void OnCalibrationSaved(Rectangle spriteRect)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Rectangle screenRect;
				int cw;
				int ch;
				using (Bitmap s = ScreenCapture.Grab(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out screenRect))
				{
					cw = s.Width;
					ch = s.Height;
				}
				Point sp = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				float sx = (float)cw / (float)Math.Max(1, sp.X);
				float sy = (float)ch / (float)Math.Max(1, sp.Y);
				int x = Math.Max(0, (int)((float)spriteRect.X * sx));
				int y = Math.Max(0, (int)((float)spriteRect.Y * sy));
				int w = (int)((float)spriteRect.Width * sx);
				int h = (int)((float)spriteRect.Height * sy);
				_dialogZone.set_Value($"{x},{y},{w},{h},{cw},{ch}");
				ScreenNotification.ShowNotification($"Dialogue zone saved ({w}×{h} @ {cw}×{ch}).", (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Dialogue zone calibrated: " + _dialogZone.get_Value());
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Calibration save failed.");
				ScreenNotification.ShowNotification("Calibration save failed: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			CloseCalibrator();
		}

		private void CloseCalibrator()
		{
			DialogZoneCalibrator calibrator = _calibrator;
			if (calibrator != null)
			{
				((Control)calibrator).Dispose();
			}
			_calibrator = null;
		}

		private Rectangle? GetCalibratedZone(int clientW, int clientH)
		{
			string raw = _dialogZone.get_Value();
			if (string.IsNullOrEmpty(raw))
			{
				return null;
			}
			string[] p = raw.Split(',');
			if (p.Length != 6)
			{
				return null;
			}
			if (!int.TryParse(p[0], out var x) || !int.TryParse(p[1], out var y) || !int.TryParse(p[2], out var w) || !int.TryParse(p[3], out var h) || !int.TryParse(p[4], out var rw) || !int.TryParse(p[5], out var rh))
			{
				return null;
			}
			if (w < 16 || h < 16)
			{
				return null;
			}
			if (clientW > 0 && clientH > 0 && (rw != clientW || rh != clientH))
			{
				return null;
			}
			return new Rectangle(x, y, w, h);
		}

		internal void StartBookCalibration()
		{
			OnBookCalibrateActivated(null, EventArgs.Empty);
		}

		private void OnBookCalibrateActivated(object sender, EventArgs e)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			if (_calibrator == null)
			{
				if (!_bookVisible || _gw2ClientRect.Width <= 0)
				{
					ScreenNotification.ShowNotification("Open a lorebook first (wait for the buttons), then calibrate.", (NotificationType)0, (Texture2D)null, 4);
					return;
				}
				_bookCalibBox = _bookBox;
				Point size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				float sx = (float)size.X / (float)_gw2ClientRect.Width;
				float sy = (float)size.Y / (float)_gw2ClientRect.Height;
				Rectangle seed = GetCalibratedBookCrop(_bookBox) ?? ParchmentDetector.InnerCrop(_bookBox);
				Rectangle start = default(Rectangle);
				((Rectangle)(ref start))._002Ector((int)((float)seed.X * sx), (int)((float)seed.Y * sy), (int)((float)seed.Width * sx), (int)((float)seed.Height * sy));
				_calibrator = new DialogZoneCalibrator(start, OnBookZoneSaved, CloseCalibrator);
			}
		}

		private void OnBookZoneSaved(Rectangle spriteRect)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Rectangle b = _bookCalibBox;
				if (b.Width <= 0 || b.Height <= 0)
				{
					ScreenNotification.ShowNotification("Calibration lost the book — open it and try again.", (NotificationType)0, (Texture2D)null, 4);
					CloseCalibrator();
					return;
				}
				Rectangle screenRect;
				int cw;
				int ch;
				using (Bitmap s = ScreenCapture.Grab(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out screenRect))
				{
					cw = s.Width;
					ch = s.Height;
				}
				Point sp = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				float sx = (float)cw / (float)Math.Max(1, sp.X);
				float sy = (float)ch / (float)Math.Max(1, sp.Y);
				double num = (float)spriteRect.X * sx;
				double fy = (float)spriteRect.Y * sy;
				double fw = (float)spriteRect.Width * sx;
				double fh = (float)spriteRect.Height * sy;
				int xBp = (int)Math.Round((num - (double)b.X) * 10000.0 / (double)b.Width);
				int yBp = (int)Math.Round((fy - (double)b.Y) * 10000.0 / (double)b.Height);
				int wBp = (int)Math.Round(fw * 10000.0 / (double)b.Width);
				int hBp = (int)Math.Round(fh * 10000.0 / (double)b.Height);
				_bookZone.set_Value($"{xBp},{yBp},{wBp},{hBp}");
				ScreenNotification.ShowNotification("Lorebook OCR area saved (position stays auto-detected).", (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Lorebook OCR area (rel bp): " + _bookZone.get_Value());
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Book calibration save failed.");
				ScreenNotification.ShowNotification("Book calibration save failed: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			CloseCalibrator();
		}

		private Rectangle? GetCalibratedBookCrop(Rectangle box)
		{
			string raw = _bookZone.get_Value();
			if (string.IsNullOrEmpty(raw))
			{
				return null;
			}
			string[] p = raw.Split(',');
			if (p.Length != 4)
			{
				return null;
			}
			if (!int.TryParse(p[0], out var xBp) || !int.TryParse(p[1], out var yBp) || !int.TryParse(p[2], out var wBp) || !int.TryParse(p[3], out var hBp))
			{
				return null;
			}
			if (wBp < 100 || hBp < 100)
			{
				return null;
			}
			int x = box.X + (int)Math.Round((double)xBp / 10000.0 * (double)box.Width);
			int y = box.Y + (int)Math.Round((double)yBp / 10000.0 * (double)box.Height);
			int w = (int)Math.Round((double)wBp / 10000.0 * (double)box.Width);
			int h = (int)Math.Round((double)hBp / 10000.0 * (double)box.Height);
			return new Rectangle(x, y, w, h);
		}

		private void StartRead()
		{
			if (!_readBusy)
			{
				_readBusy = true;
				Task.Run((Func<Task>)ReadPipelineAsync);
			}
		}

		private void StartSaveOnly()
		{
			if (!_readBusy)
			{
				_readBusy = true;
				Task.Run((Func<Task>)SaveOnlyPipelineAsync);
			}
		}

		private void StartAppend()
		{
			if (!_readBusy)
			{
				_readBusy = true;
				Task.Run((Func<Task>)AppendPipelineAsync);
			}
		}

		private void OnTtsChunk(string chunk)
		{
			if (chunk != null)
			{
				_wordSyncValid = !_chunkTranslate;
			}
			if (chunk != null && _chunkTranslate)
			{
				int session = _speakSession;
				string src = chunk;
				string target = _chunkTranslateTarget;
				Task.Run(async delegate
				{
					string shown = src;
					try
					{
						shown = await TranslationService.TranslateAsync(src, target);
					}
					catch
					{
					}
					if (session == _speakSession)
					{
						_cueSource = shown;
						_cueSourceDirty = true;
					}
				});
			}
			else
			{
				_cueSource = chunk;
				_cueSourceDirty = true;
			}
		}

		private void OnTtsWord(int wordIndex)
		{
			_currentWordIndex = wordIndex;
			_haveWordEvents = true;
		}

		private List<string> BuildSubtitleCues(string text, int lineWidthPx, int fontSize)
		{
			List<string> cues = new List<string>();
			if (string.IsNullOrWhiteSpace(text) || _textRenderer == null)
			{
				return cues;
			}
			text = Regex.Replace(text, "\\s+", " ").Trim();
			string[] array = Regex.Split(text, "(?<=[.!?])\\s+");
			foreach (string obj in array)
			{
				List<string> lines = new List<string>();
				string line = "";
				string[] array2 = obj.Split(' ');
				foreach (string w in array2)
				{
					if (w.Length != 0)
					{
						string cand = ((line.Length == 0) ? w : (line + " " + w));
						if (line.Length == 0 || _textRenderer.MeasureWidth(cand, fontSize) <= (float)lineWidthPx)
						{
							line = cand;
							continue;
						}
						lines.Add(line);
						line = w;
					}
				}
				if (line.Length > 0)
				{
					lines.Add(line);
				}
				List<string> sentCues = new List<string>();
				int idx = lines.Count;
				while (idx > 0)
				{
					int start = Math.Max(0, idx - 2);
					sentCues.Add(string.Join(" ", lines.GetRange(start, idx - start)));
					idx = start;
				}
				sentCues.Reverse();
				cues.AddRange(sentCues);
			}
			return cues;
		}

		private static int[] BuildCueWordStarts(List<string> cues)
		{
			if (cues == null || cues.Count == 0)
			{
				return null;
			}
			int[] starts = new int[cues.Count];
			int acc = 0;
			for (int i = 0; i < cues.Count; i++)
			{
				starts[i] = acc;
				acc += cues[i].Split(' ').Length;
			}
			return starts;
		}

		private async Task<(bool ok, string title, string text)> CaptureBookAsync()
		{
			string title = null;
			IntPtr hwnd = GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle();
			Rectangle screenRect;
			string text;
			using (Bitmap screen = ScreenCapture.Grab(hwnd, out screenRect))
			{
				double solidity;
				Rectangle? box = ParchmentDetector.Find(screen, out solidity);
				bool parchmentDetected = box.HasValue;
				bool isConversation = false;
				Rectangle? convText = null;
				if (box.HasValue)
				{
					Logger.Info($"Parchment {box} solidity {solidity:0.00}");
				}
				else if (_conversationCapture.get_Value())
				{
					Rectangle? zone = GetCalibratedZone(screen.Width, screen.Height);
					ConversationHit hit = (zone.HasValue ? (ConversationDetector.MeasureInZone(screen, zone.Value) ?? new ConversationHit
					{
						Panel = zone.Value,
						TextArea = zone.Value,
						Confidence = 0.0
					}) : ConversationDetector.FindHit(screen));
					if (hit != null)
					{
						box = hit.Panel;
						convText = hit.TextArea;
						_ = hit.Confidence;
						isConversation = true;
						Logger.Info("Conversation " + (zone.HasValue ? "zone" : "auto") + " " + $"panel {hit.Panel} text {hit.TextArea} " + $"conf {hit.Confidence:0.00}");
					}
				}
				if (!box.HasValue)
				{
					box = new Rectangle((int)((double)screen.Width * 0.34), (int)((double)screen.Height * 0.12), (int)((double)screen.Width * 0.32), (int)((double)screen.Height * 0.8));
					Logger.Info("Neither parchment nor conversation detected, using center fallback.");
				}
				Rectangle? bookCrop = ((parchmentDetected && !isConversation) ? GetCalibratedBookCrop(box.Value) : null);
				Rectangle inner = ((!isConversation) ? (bookCrop ?? ParchmentDetector.InnerCrop(box.Value)) : (convText ?? ConversationDetector.TextCrop(box.Value)));
				inner = Rectangle.Intersect(inner, new Rectangle(0, 0, screen.Width, screen.Height));
				using (Bitmap crop = screen.Clone(inner, screen.PixelFormat))
				{
					text = TextCleaner.CleanForEncyclopedia(await OcrService.RecognizeAsync(crop, _ocrLanguage.get_Value(), isConversation));
				}
				title = (isConversation ? TryReadNpcName(screen, box.Value) : TryReadHeader(screen, box.Value));
			}
			if (text.Length < 20)
			{
				ScreenNotification.ShowNotification("Lorebook Reader: no readable text found. Is a book open?", (NotificationType)0, (Texture2D)null, 4);
				return (false, null, null);
			}
			Logger.Info($"OCR ok ({text.Length} chars)" + ((title != null) ? (", title \"" + title + "\"") : "") + ".");
			return (true, title, text);
		}

		private async Task ReadPipelineAsync()
		{
			_ = 1;
			try
			{
				var (ok, title, text) = await CaptureBookAsync();
				if (ok)
				{
					_catalog?.AddCaptured(title, text);
					await SpeakTextAsync(text);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Lorebook read failed.");
				ScreenNotification.ShowNotification("Lorebook Reader: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			finally
			{
				_readBusy = false;
			}
		}

		private async Task SaveOnlyPipelineAsync()
		{
			try
			{
				var (ok, title, text) = await CaptureBookAsync();
				if (ok)
				{
					ScreenNotification.ShowNotification("Saved to encyclopedia: " + ((_catalog?.AddCaptured(title, text))?.DisplayTitle ?? "lorebook"), (NotificationType)0, (Texture2D)null, 4);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Lorebook save failed.");
				ScreenNotification.ShowNotification("Lorebook Reader: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			finally
			{
				_readBusy = false;
			}
		}

		private async Task AppendPipelineAsync()
		{
			try
			{
				var (ok, _, text) = await CaptureBookAsync();
				if (ok)
				{
					LorebookEntry entry = _catalog?.AppendToLatest(text);
					if (entry == null)
					{
						ScreenNotification.ShowNotification("Nothing to append to yet — save a book first.", (NotificationType)0, (Texture2D)null, 4);
					}
					else
					{
						ScreenNotification.ShowNotification("Appended page to: " + entry.DisplayTitle, (NotificationType)0, (Texture2D)null, 4);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Lorebook append failed.");
				ScreenNotification.ShowNotification("Lorebook Reader: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
			finally
			{
				_readBusy = false;
			}
		}

		private async Task SpeakTextAsync(string text)
		{
			Interlocked.Increment(ref _speakSession);
			string mode = _translateMode.get_Value();
			string target = _translateTarget.get_Value();
			string speechLang = _ocrLanguage.get_Value();
			string edgeVoice = _edgeVoice.get_Value();
			if (mode == "full")
			{
				try
				{
					string translated = await TranslationService.TranslateAsync(text, target);
					if (!string.IsNullOrWhiteSpace(translated))
					{
						text = translated;
						speechLang = target;
						edgeVoice = EdgeTtsService.VoiceForLanguage(target) ?? edgeVoice;
					}
				}
				catch (Exception tEx)
				{
					Logger.Warn(tEx, "Translation failed, using original text.");
					ScreenNotification.ShowNotification("Lorebook Reader: translation unavailable — using original.", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			_chunkTranslate = mode == "subtitles";
			_chunkTranslateTarget = target;
			if (_voiceEngine.get_Value() == "edge")
			{
				try
				{
					await _edgeTts.SpeakAsync(text, edgeVoice, _speakingRate.get_Value(), OnTtsChunk, OnTtsWord);
					return;
				}
				catch (Exception edgeEx)
				{
					Logger.Warn(edgeEx, "Edge TTS failed, falling back to offline voice.");
					ScreenNotification.ShowNotification("Lorebook Reader: online voice unavailable — using offline voice.", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			string warning = await _tts.SpeakAsync(text, _voiceName.get_Value(), _speakingRate.get_Value(), speechLang, OnTtsChunk, OnTtsWord);
			if (warning != null)
			{
				ScreenNotification.ShowNotification("Lorebook Reader: " + warning, (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private static string ExpansionIconFile(string expansion)
		{
			if (string.IsNullOrWhiteSpace(expansion))
			{
				return null;
			}
			string text = expansion.Trim().ToLowerInvariant();
			if (text != null)
			{
				switch (text.Length)
				{
				case 13:
					switch (text[0])
					{
					case 'i':
						if (!(text == "icebrood saga"))
						{
							break;
						}
						return "xp_ibs.png";
					case 'j':
						if (!(text == "janthir wilds"))
						{
							break;
						}
						return "xp_jw.png";
					case 's':
						if (!(text == "short stories"))
						{
							break;
						}
						return "xp_shortstories.png";
					}
					break;
				case 4:
					if (!(text == "core"))
					{
						break;
					}
					return "xp_core.png";
				case 15:
					if (!(text == "heart of thorns"))
					{
						break;
					}
					return "xp_hot.png";
				case 12:
					if (!(text == "path of fire"))
					{
						break;
					}
					return "xp_pof.png";
				case 14:
					if (!(text == "end of dragons"))
					{
						break;
					}
					return "xp_eod.png";
				case 22:
					if (!(text == "secrets of the obscure"))
					{
						break;
					}
					return "xp_soto.png";
				case 19:
					if (!(text == "visions of eternity"))
					{
						break;
					}
					return "xp_voe.png";
				}
			}
			return null;
		}

		private Texture2D LoadRefTexture(string file)
		{
			if (file == null)
			{
				return null;
			}
			if (_xpIconCache.TryGetValue(file, out var cached))
			{
				return cached;
			}
			Texture2D tex = null;
			try
			{
				using Stream s = base.ModuleParameters.get_ContentsManager().GetFileStream(file);
				if (s != null)
				{
					tex = TextureUtil.FromStreamPremultiplied(s);
				}
			}
			catch
			{
				tex = null;
			}
			_xpIconCache[file] = tex;
			return tex;
		}

		internal Texture2D GetExpansionIcon(string expansion)
		{
			return LoadRefTexture(ExpansionIconFile(expansion));
		}

		internal Texture2D GetRefTexture(string file)
		{
			return LoadRefTexture(file);
		}

		internal Texture2D GetExpansionStampIcon(string expansion)
		{
			string file = ExpansionIconFile(expansion);
			if (file == null)
			{
				return null;
			}
			return LoadRefTexture(file.Replace(".png", "_big.png")) ?? LoadRefTexture(file);
		}

		private string TryReadHeader(Bitmap screen, Rectangle parchment)
		{
			try
			{
				int hh = (int)((double)parchment.Height * 0.14);
				int hx = Math.Max(0, parchment.X - 10);
				int hy = Math.Max(0, parchment.Y - hh - 6);
				int hw = Math.Min(parchment.Width + 20, screen.Width - hx);
				int hAvail = parchment.Y - 2 - hy;
				if (hAvail < 10 || hw < 20)
				{
					return null;
				}
				Rectangle headerRect = new Rectangle(hx, hy, hw, hAvail);
				using Bitmap headerCrop = screen.Clone(headerRect, screen.PixelFormat);
				string raw = OcrService.RecognizeLineAsync(headerCrop, _ocrLanguage.get_Value()).GetAwaiter().GetResult();
				raw = (raw ?? "").Trim();
				if (raw.Length < 2 || raw.Length > 60)
				{
					return null;
				}
				return raw;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Header OCR failed.");
				return null;
			}
		}

		private string TryReadNpcName(Bitmap screen, Rectangle convBox)
		{
			try
			{
				int labelW = (int)((double)convBox.Width * 0.38);
				int labelH = (int)((double)convBox.Height * 0.45);
				int lx = convBox.Right - labelW;
				int ly = convBox.Y + (int)((double)convBox.Height * 0.15);
				lx = Math.Max(0, Math.Min(lx, screen.Width - labelW));
				ly = Math.Max(0, Math.Min(ly, screen.Height - labelH));
				labelW = Math.Min(labelW, screen.Width - lx);
				labelH = Math.Min(labelH, screen.Height - ly);
				if (labelW < 20 || labelH < 10)
				{
					return null;
				}
				Rectangle labelRect = new Rectangle(lx, ly, labelW, labelH);
				using Bitmap labelCrop = screen.Clone(labelRect, screen.PixelFormat);
				string raw = OcrService.RecognizeLineAsync(labelCrop, _ocrLanguage.get_Value(), invert: true).GetAwaiter().GetResult();
				raw = (raw ?? "").Trim();
				if (raw.Length < 2 || raw.Length > 40)
				{
					return null;
				}
				return raw;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "NPC name OCR failed.");
				return null;
			}
		}

		internal void PlayFromCatalog(LorebookEntry entry)
		{
			if (entry == null || _readBusy)
			{
				return;
			}
			_readBusy = true;
			Task.Run(async delegate
			{
				try
				{
					await SpeakTextAsync(entry.Text);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "History playback failed.");
				}
				finally
				{
					_readBusy = false;
				}
			});
		}

		private void ShowEncyclopedia()
		{
			_encyclopediaView = new EncyclopediaView(this, _parchmentTexture);
			_historyWindow.Show((IView)(object)_encyclopediaView);
		}

		internal void StopSpeaking()
		{
			_tts?.Stop();
			_edgeTts?.Stop();
			_speechPaused = false;
		}

		internal bool TogglePauseSpeaking()
		{
			bool pause = !_speechPaused;
			if (pause)
			{
				_tts?.Pause();
				_edgeTts?.Pause();
			}
			else
			{
				_tts?.Resume();
				_edgeTts?.Resume();
			}
			_speechPaused = pause;
			return pause;
		}

		internal void ExportCatalogDialog()
		{
			try
			{
				string path = Path.Combine(base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("lorebook_reader"), "lorebook_export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json");
				_catalog.ExportToFile(path);
				ScreenNotification.ShowNotification("Exported to lorebook_reader\\" + Path.GetFileName(path), (NotificationType)0, (Texture2D)null, 4);
				Logger.Info("Catalog exported to " + path);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Export failed.");
				ScreenNotification.ShowNotification("Export failed: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
		}

		internal void ImportCatalogDialog()
		{
			try
			{
				string[] files = Directory.GetFiles(base.ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("lorebook_reader"), "lorebook_export*.json");
				if (files.Length == 0)
				{
					ScreenNotification.ShowNotification("No export file found in lorebook_reader folder.", (NotificationType)0, (Texture2D)null, 4);
					return;
				}
				string latest = files.OrderByDescending((string f) => File.GetLastWriteTimeUtc(f)).First();
				int count = _catalog.ImportFromFile(latest);
				ScreenNotification.ShowNotification($"Imported {count} lorebooks from " + Path.GetFileName(latest), (NotificationType)0, (Texture2D)null, 4);
				if (((Control)_historyWindow).get_Visible())
				{
					ShowEncyclopedia();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Import failed.");
				ScreenNotification.ShowNotification("Import failed: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
		}

		internal void RunOnMainThread(Action action)
		{
			if (action != null)
			{
				_mainThreadQueue.Enqueue(action);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0621: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
			Action act;
			while (_mainThreadQueue.TryDequeue(out act))
			{
				try
				{
					act();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Main-thread action failed.");
				}
			}
			_detectTimerMs += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_detectTimerMs >= 1000.0)
			{
				_detectTimerMs = 0.0;
				if (_showSpeakerButton.get_Value() && !_detectBusy && !_readBusy)
				{
					_detectBusy = true;
					Task.Run((Action)DetectForButton);
				}
				else if (!_showSpeakerButton.get_Value())
				{
					_bookVisible = false;
				}
			}
			if (_catalogDirty && _historyWindow != null && ((Control)_historyWindow).get_Visible())
			{
				_catalogDirty = false;
				_encyclopediaView?.RefreshFromCatalog();
			}
			if (_subtitleLabel != null)
			{
				if (_subtitleFontSize.get_Value() != _lastFontSize)
				{
					_lastFontSize = _subtitleFontSize.get_Value();
					_subtitleLabel.FontSize = _lastFontSize;
					_subWidthCap = (int)_textRenderer.MeasureWidth(new string('n', 42), _lastFontSize) + 16;
				}
				if (_subtitleLabel.EditMode)
				{
					((Control)_subtitleLabel).set_Opacity(_subtitleOpacity.get_Value());
				}
				else
				{
					int cueBoxW = Math.Max(100, Math.Min((int)((float)((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X * 0.45f), (_subWidthCap > 0) ? _subWidthCap : int.MaxValue));
					if (_cueSourceDirty)
					{
						_cueSourceDirty = false;
						_cues = (string.IsNullOrEmpty(_cueSource) ? null : BuildSubtitleCues(_cueSource, (int)((float)cueBoxW * 0.9f), _lastFontSize));
						_cueWordStart = BuildCueWordStarts(_cues);
						_cueIndex = 0;
						_cueElapsedMs = 0.0;
						_haveWordEvents = false;
						_currentWordIndex = 0;
						_pendingSubtitle = ((_cues != null && _cues.Count > 0) ? TextCleaner.SanitizeForDisplay(_cues[0]) : null);
						_subtitleDirty = true;
					}
					else if (_cues != null && _cues.Count > 0)
					{
						if (_wordSyncValid && _haveWordEvents && _cueWordStart != null)
						{
							int wi = _currentWordIndex;
							int target;
							for (target = _cueIndex; target + 1 < _cues.Count && wi >= _cueWordStart[target + 1]; target++)
							{
							}
							if (target > _cueIndex)
							{
								_cueIndex = target;
								_pendingSubtitle = TextCleaner.SanitizeForDisplay(_cues[_cueIndex]);
								_subtitleDirty = true;
							}
						}
						else if (_cueIndex < _cues.Count && !_speechPaused)
						{
							_cueElapsedMs += gameTime.get_ElapsedGameTime().TotalMilliseconds;
							double cps = 17.0 * Math.Max(0.5, _speakingRate.get_Value());
							double dur = Math.Max(700.0, Math.Min(7000.0, (double)_cues[_cueIndex].Length / cps * 1000.0));
							if (_cueElapsedMs >= dur && _cueIndex + 1 < _cues.Count)
							{
								_cueIndex++;
								_cueElapsedMs = 0.0;
								_pendingSubtitle = TextCleaner.SanitizeForDisplay(_cues[_cueIndex]);
								_subtitleDirty = true;
							}
						}
					}
					if (_subtitleDirty)
					{
						_subtitleDirty = false;
						_subtitleLabel.SubtitleText = _pendingSubtitle ?? "";
					}
					bool show = _showSubtitles.get_Value() && !string.IsNullOrEmpty(_subtitleLabel.SubtitleText);
					if (show)
					{
						Point sprite2 = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
						int width = Math.Max(100, Math.Min((int)((float)sprite2.X * 0.45f), (_subWidthCap > 0) ? _subWidthCap : int.MaxValue));
						if (width != _lastSubWidth)
						{
							_subtitleLabel.BoxWidth = width;
							_lastSubWidth = width;
						}
						((Control)_subtitleLabel).set_Opacity(_subtitleOpacity.get_Value());
						int sx = (int)((float)sprite2.X * (_subtitleX.get_Value() / 100f)) - width / 2;
						int sy = (int)((float)sprite2.Y * (_subtitleY.get_Value() / 100f));
						((Control)_subtitleLabel).set_Location(new Point(Math.Max(0, Math.Min(sx, sprite2.X - width)), Math.Max(0, Math.Min(sy, sprite2.Y - Math.Max(1, ((Control)_subtitleLabel).get_Size().Y)))));
					}
					((Control)_subtitleLabel).set_Visible(show);
				}
			}
			if (_speakerButton != null)
			{
				bool showButtons = false;
				Rectangle activeBox = default(Rectangle);
				if (_bookVisible && _gw2ClientRect.Width > 0)
				{
					activeBox = _bookBox;
					showButtons = true;
				}
				else if (_convVisible && _gw2ClientRect.Width > 0)
				{
					activeBox = _convBox;
					showButtons = true;
				}
				if (showButtons)
				{
					Point sprite = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
					float scaleX = (float)sprite.X / (float)_gw2ClientRect.Width;
					float scaleY = (float)sprite.Y / (float)_gw2ClientRect.Height;
					int extraOffset = (_convVisible ? ((int)((float)activeBox.Width * scaleX * 0.06f) + 12) : 6);
					int val = (int)((float)activeBox.Right * scaleX) + extraOffset;
					int y = (int)((float)activeBox.Top * scaleY);
					int bx = Math.Min(val, sprite.X - ((Control)_speakerButton).get_Width());
					((Control)_speakerButton).set_Location(new Point(bx, Math.Max(0, y)));
					((Control)_saveButton).set_Location(new Point(bx, Math.Max(0, y + 44)));
					((Control)_appendButton).set_Location(new Point(bx, Math.Max(0, y + 88)));
					((Control)_speakerButton).set_Visible(true);
					((Control)_saveButton).set_Visible(true);
					((Control)_appendButton).set_Visible(true);
				}
				else
				{
					((Control)_speakerButton).set_Visible(false);
					((Control)_saveButton).set_Visible(false);
					((Control)_appendButton).set_Visible(false);
				}
			}
		}

		private void DetectForButton()
		{
			try
			{
				Rectangle screenRect;
				using Bitmap screen = ScreenCapture.Grab(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out screenRect);
				_gw2ClientRect = new Rectangle(0, 0, screen.Width, screen.Height);
				double solidity;
				Rectangle? box = ParchmentDetector.Find(screen, out solidity);
				if (box.HasValue)
				{
					_bookBox = box.Value;
					_bookVisible = true;
					_convVisible = false;
					return;
				}
				_bookVisible = false;
				if (!_conversationCapture.get_Value())
				{
					goto IL_0121;
				}
				Rectangle? zone = GetCalibratedZone(screen.Width, screen.Height);
				if (zone.HasValue)
				{
					ConversationHit zHit = ConversationDetector.MeasureInZone(screen, zone.Value);
					if (zHit != null && zHit.Confidence >= 0.03 && zHit.Confidence <= 0.6)
					{
						_convBox = zHit.Panel;
						_convVisible = true;
					}
					else
					{
						_convVisible = false;
					}
					return;
				}
				Rectangle? conv = ConversationDetector.Find(screen, out solidity);
				if (!conv.HasValue)
				{
					goto IL_0121;
				}
				_convBox = conv.Value;
				_convVisible = true;
				goto end_IL_0017;
				IL_0121:
				_convVisible = false;
				end_IL_0017:;
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Button detection failed.");
				_bookVisible = false;
				_convVisible = false;
			}
			finally
			{
				_detectBusy = false;
			}
		}

		private static void Safe(Action step)
		{
			try
			{
				step();
			}
			catch
			{
			}
		}

		protected override void Unload()
		{
			if (_unloaded)
			{
				return;
			}
			_unloaded = true;
			Safe(delegate
			{
				_encyclopediaView?.FlushEdits();
			});
			Safe(delegate
			{
				_readKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnReadActivated);
			});
			Safe(delegate
			{
				_stopKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnStopActivated);
			});
			Safe(delegate
			{
				_pauseKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnPauseActivated);
			});
			Safe(delegate
			{
				_convToggleKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnConvToggleActivated);
			});
			Safe(delegate
			{
				_debugDumpKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnDebugDumpActivated);
			});
			Safe(delegate
			{
				_calibrateKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnCalibrateActivated);
			});
			Safe(delegate
			{
				_bookCalibrateKeybind.get_Value().remove_Activated((EventHandler<EventArgs>)OnBookCalibrateActivated);
			});
			Safe(delegate
			{
				DialogZoneCalibrator calibrator = _calibrator;
				if (calibrator != null)
				{
					((Control)calibrator).Dispose();
				}
			});
			Safe(delegate
			{
				BookActionButton speakerButton = _speakerButton;
				if (speakerButton != null)
				{
					((Control)speakerButton).Dispose();
				}
			});
			Safe(delegate
			{
				BookActionButton saveButton = _saveButton;
				if (saveButton != null)
				{
					((Control)saveButton).Dispose();
				}
			});
			Safe(delegate
			{
				BookActionButton appendButton = _appendButton;
				if (appendButton != null)
				{
					((Control)appendButton).Dispose();
				}
			});
			Safe(delegate
			{
				SubtitleOverlay subtitleLabel = _subtitleLabel;
				if (subtitleLabel != null)
				{
					((Control)subtitleLabel).Dispose();
				}
			});
			Safe(delegate
			{
				CornerIcon cornerIcon = _cornerIcon;
				if (cornerIcon != null)
				{
					((Control)cornerIcon).Dispose();
				}
			});
			Safe(delegate
			{
				_textRenderer?.Dispose();
			});
			Safe(delegate
			{
				StandardWindow historyWindow = _historyWindow;
				if (historyWindow != null)
				{
					((Control)historyWindow).Dispose();
				}
			});
			Safe(delegate
			{
				_tts?.Dispose();
			});
			Safe(delegate
			{
				_edgeTts?.Dispose();
			});
			Safe(delegate
			{
				foreach (Texture2D value in _xpIconCache.Values)
				{
					if (value != null)
					{
						((GraphicsResource)value).Dispose();
					}
				}
				_xpIconCache.Clear();
			});
			Safe(BookReaderPanel.ClearImageCache);
		}
	}
}
