using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Playback;
using Maestro.Services.Practice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Practice
{
	public class PracticeWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 520;

			public const int WindowHeight = 720;

			public const int ContentWidth = 490;

			public const int HudHeight = 64;

			public const int LoopBarHeight = 40;

			public const int BottomPadding = 60;
		}

		private static Texture2D _backgroundTexture;

		private readonly Song _song;

		private readonly PracticeSettings _settings;

		private readonly PracticeInputListener _inputListener;

		private readonly PracticeHud _hud;

		private readonly NoteHighway _highway;

		private readonly SectionLoopBar _loopBar;

		private readonly Panel _confirmationOverlay;

		private readonly Label _confirmationLabel;

		private readonly Label _octaveNoteLabel;

		private readonly StandardButton _readyButton;

		private readonly Panel _resultOverlay;

		private readonly Label _resultTitleLabel;

		private readonly Label _resultScoreLabel;

		private readonly Label _resultComboLabel;

		private readonly Label _resultBreakdownLabel;

		private readonly StandardButton _resultRestartButton;

		private readonly StandardButton _resultCloseButton;

		private bool _isWaitingForConfirmation;

		private bool _isClosed;

		private readonly bool _isConstructed;

		public PracticeSession Session { get; }

		public Song Song { get; }

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(520, 720));
		}

		public PracticeWindow(Song song, KeyboardService keyboardService, PracticeSettings settings)
			: this(GetBackground(), new Rectangle(0, 0, 520, 720), new Rectangle(15, 20, 490, 720))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Expected O, but got Unknown
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Expected O, but got Unknown
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Expected O, but got Unknown
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Expected O, but got Unknown
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Expected O, but got Unknown
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Expected O, but got Unknown
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Expected O, but got Unknown
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Expected O, but got Unknown
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			//IL_062c: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Expected O, but got Unknown
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Expected O, but got Unknown
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Expected O, but got Unknown
			if (song == null)
			{
				throw new ArgumentNullException("song");
			}
			if (keyboardService == null)
			{
				throw new ArgumentNullException("keyboardService");
			}
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			_song = song;
			Song = song;
			_settings = settings;
			((WindowBase2)this).set_Title("Practice");
			((WindowBase2)this).set_Subtitle(song.Name + " - " + song.Artist);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("PracticeWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			NoteTimeline timeline = NoteTimeline.Build(song.Notes);
			KeyboardServiceKeySender sender = new KeyboardServiceKeySender(keyboardService);
			InstrumentInfo instrumentInfo = InstrumentCatalog.Get(song.Instrument);
			Session = new PracticeSession(timeline, sender, autoOctave: true, settings.CountdownLengthMs.get_Value(), instrumentInfo.MinOctave, instrumentInfo.MaxOctave);
			Session.Clock.Speed = settings.LastUsedSpeed.get_Value();
			_inputListener = new PracticeInputListener(keyboardService, Module.Instance.Settings);
			_inputListener.Attach(Session);
			PracticeHud practiceHud = new PracticeHud(Session, settings);
			((Control)practiceHud).set_Parent((Container)(object)this);
			((Control)practiceHud).set_Location(new Point(0, 0));
			((Control)practiceHud).set_Width(490);
			((Control)practiceHud).set_Height(64);
			_hud = practiceHud;
			_hud.PauseRequested += OnPauseRequested;
			_hud.RestartRequested += OnRestartRequested;
			_hud.CloseRequested += OnCloseRequested;
			NoteHighway noteHighway = new NoteHighway(Session, settings.LookaheadSeconds.get_Value(), Module.Instance.Settings);
			((Control)noteHighway).set_Parent((Container)(object)this);
			((Control)noteHighway).set_Location(new Point(0, 64));
			((Control)noteHighway).set_Width(490);
			((Control)noteHighway).set_Height(556);
			_highway = noteHighway;
			SectionLoopBar sectionLoopBar = new SectionLoopBar(Session);
			((Control)sectionLoopBar).set_Parent((Container)(object)this);
			((Control)sectionLoopBar).set_Location(new Point(0, 620));
			((Control)sectionLoopBar).set_Width(490);
			((Control)sectionLoopBar).set_Height(40);
			_loopBar = sectionLoopBar;
			_highway.OnAfterTick = OnHighwayAfterTick;
			Session.Clock.Pause();
			_isWaitingForConfirmation = true;
			int overlayTop = 64;
			int overlayHeight = 556;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, overlayTop));
			((Control)val).set_Size(new Point(490, overlayHeight));
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 220));
			((Control)val).set_ZIndex(100);
			((Control)val).set_Visible(true);
			_confirmationOverlay = val;
			int blockHeight = 174;
			int blockTop = overlayHeight / 2 - blockHeight / 2;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_confirmationOverlay);
			val2.set_Text($"Equip your {_song.Instrument} and click Ready");
			((Control)val2).set_Location(new Point(0, blockTop));
			((Control)val2).set_Size(new Point(490, 26));
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_TextColor(MaestroTheme.CreamWhite);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)0);
			_confirmationLabel = val2;
			int noteTop = blockTop + 26 + 14;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_confirmationOverlay);
			val3.set_Text("Octaves are switched automatically for now.\nAn upcoming release will let you handle them yourself.\nTiles marked # are sharp notes (a piano's black keys).\nPlay them like in game: Alt + skill slots 1-5 (C# D# F# G# A#).");
			((Control)val3).set_Location(new Point(0, noteTop));
			((Control)val3).set_Size(new Point(490, 100));
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(MaestroTheme.MutedCream);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_VerticalAlignment((VerticalAlignment)0);
			_octaveNoteLabel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_confirmationOverlay);
			val4.set_Text("Ready");
			((Control)val4).set_Location(new Point(195, noteTop + 100 + 4));
			((Control)val4).set_Size(new Point(100, 30));
			_readyButton = val4;
			((Control)_readyButton).add_Click((EventHandler<MouseEventArgs>)OnReadyClicked);
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(0, overlayTop));
			((Control)val5).set_Size(new Point(490, overlayHeight));
			((Control)val5).set_BackgroundColor(new Color(0, 0, 0, 220));
			((Control)val5).set_ZIndex(100);
			((Control)val5).set_Visible(false);
			_resultOverlay = val5;
			int centerY = overlayHeight / 2;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_resultOverlay);
			val6.set_Text("SONG COMPLETE");
			((Control)val6).set_Location(new Point(0, centerY - 110));
			((Control)val6).set_Size(new Point(490, 30));
			val6.set_Font(GameService.Content.get_DefaultFont18());
			val6.set_TextColor(MaestroTheme.AmberGold);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			_resultTitleLabel = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_resultOverlay);
			val7.set_Text("SCORE 0");
			((Control)val7).set_Location(new Point(0, centerY - 70));
			((Control)val7).set_Size(new Point(490, 42));
			val7.set_Font(GameService.Content.get_DefaultFont32() ?? GameService.Content.get_DefaultFont18());
			val7.set_TextColor(MaestroTheme.CreamWhite);
			val7.set_HorizontalAlignment((HorizontalAlignment)1);
			_resultScoreLabel = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)_resultOverlay);
			val8.set_Text("MAX COMBO x0");
			((Control)val8).set_Location(new Point(0, centerY - 20));
			((Control)val8).set_Size(new Point(490, 26));
			val8.set_Font(GameService.Content.get_DefaultFont16());
			val8.set_TextColor(MaestroTheme.CreamWhite);
			val8.set_HorizontalAlignment((HorizontalAlignment)1);
			_resultComboLabel = val8;
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)_resultOverlay);
			val9.set_Text("");
			((Control)val9).set_Location(new Point(0, centerY + 12));
			((Control)val9).set_Size(new Point(490, 24));
			val9.set_Font(GameService.Content.get_DefaultFont14());
			val9.set_TextColor(MaestroTheme.MutedCream);
			val9.set_HorizontalAlignment((HorizontalAlignment)1);
			_resultBreakdownLabel = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)_resultOverlay);
			val10.set_Text("Restart");
			((Control)val10).set_Location(new Point(137, centerY + 52));
			((Control)val10).set_Size(new Point(100, 30));
			_resultRestartButton = val10;
			((Control)_resultRestartButton).add_Click((EventHandler<MouseEventArgs>)OnResultRestartClicked);
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_resultOverlay);
			val11.set_Text("Close");
			((Control)val11).set_Location(new Point(253, centerY + 52));
			((Control)val11).set_Size(new Point(100, 30));
			_resultCloseButton = val11;
			((Control)_resultCloseButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Hide();
			});
			Session.OnCompleted += OnSessionCompleted;
			_isConstructed = true;
		}

		private void OnSessionCompleted(PracticeResult result)
		{
			int score = result.PerfectCount * 100 + result.GoodCount * 50;
			_resultScoreLabel.set_Text($"SCORE {score}");
			_resultComboLabel.set_Text($"MAX COMBO x{result.MaxCombo}");
			_resultBreakdownLabel.set_Text($"Perfect {result.PerfectCount}   Good {result.GoodCount}   Miss {result.MissCount}   Wrong {result.WrongCount}");
			((Control)_resultOverlay).set_Visible(true);
		}

		private void OnResultRestartClicked(object sender, MouseEventArgs e)
		{
			((Control)_resultOverlay).set_Visible(false);
			OnRestartRequested();
		}

		private void OnReadyClicked(object sender, MouseEventArgs e)
		{
			if (_isWaitingForConfirmation)
			{
				_isWaitingForConfirmation = false;
				((Control)_confirmationOverlay).set_Visible(false);
				ResetOctaveForInstrument();
				Session.Clock.Resume();
			}
		}

		private void ResetOctaveForInstrument()
		{
			InstrumentInfo info = InstrumentCatalog.Get(_song.Instrument);
			for (int j = 0; j < 5; j++)
			{
				Module.Instance.PlayOctaveChange(up: false);
				Thread.Sleep(150);
			}
			for (int i = 0; i < -info.MinOctave; i++)
			{
				Module.Instance.PlayOctaveChange(up: true);
				Thread.Sleep(150);
			}
		}

		private void OnHighwayAfterTick()
		{
			_loopBar.ApplyLoopIfNeeded();
			_hud.RefreshStats();
		}

		private void OnPauseRequested()
		{
			if (!_isWaitingForConfirmation)
			{
				if (Session.Clock.IsPaused)
				{
					Session.Clock.Resume();
				}
				else
				{
					Session.Clock.Pause();
				}
			}
		}

		private void OnRestartRequested()
		{
			if (!_isWaitingForConfirmation)
			{
				((Control)_resultOverlay).set_Visible(false);
				Session.Clock.Pause();
				Session.Restart(_settings.CountdownLengthMs.get_Value());
				ResetOctaveForInstrument();
				Session.Clock.Resume();
			}
		}

		private void OnCloseRequested()
		{
			((Control)this).Hide();
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			if (_isConstructed && !_isClosed)
			{
				_isClosed = true;
				((Control)this).Dispose();
			}
		}

		protected override void DisposeControl()
		{
			_isClosed = true;
			if (_hud != null)
			{
				_hud.PauseRequested -= OnPauseRequested;
				_hud.RestartRequested -= OnRestartRequested;
				_hud.CloseRequested -= OnCloseRequested;
			}
			if (_highway != null)
			{
				_highway.OnAfterTick = null;
			}
			if (_readyButton != null)
			{
				((Control)_readyButton).remove_Click((EventHandler<MouseEventArgs>)OnReadyClicked);
			}
			if (_resultRestartButton != null)
			{
				((Control)_resultRestartButton).remove_Click((EventHandler<MouseEventArgs>)OnResultRestartClicked);
			}
			if (Session != null)
			{
				Session.OnCompleted -= OnSessionCompleted;
			}
			_inputListener?.Detach();
			((WindowBase2)this).DisposeControl();
		}
	}
}
