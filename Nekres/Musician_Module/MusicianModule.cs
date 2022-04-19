using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician_Module.Controls;
using Nekres.Musician_Module.Controls.Instrument;
using Nekres.Musician_Module.Notation.Persistance;
using Nekres.Musician_Module.Player;
using Nekres.Musician_Module.UI.Models;
using Nekres.Musician_Module.UI.Views;

namespace Nekres.Musician_Module
{
	[Export(typeof(Module))]
	public class MusicianModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(MusicianModule));

		internal static MusicianModule ModuleInstance;

		private Texture2D ICON;

		private const int TOP_MARGIN = 0;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const int LEFT_MARGIN = 8;

		private const string DD_TITLE = "Title";

		private const string DD_ARTIST = "Artist";

		private const string DD_USER = "User";

		private const string DD_HARP = "Harp";

		private const string DD_FLUTE = "Flute";

		private const string DD_LUTE = "Lute";

		private const string DD_HORN = "Horn";

		private const string DD_BASS = "Bass";

		private const string DD_BELL = "Bell";

		private const string DD_BELL2 = "Bell2";

		private WindowTab _musicianTab;

		private HealthPoolButton _stopButton;

		private List<SheetButton> _displayedSheets;

		private readonly string[] Instruments = new string[7] { "Harp", "Flute", "Lute", "Horn", "Bell", "Bell2", "Bass" };

		private XmlMusicSheetReader _xmlParser;

		private List<RawMusicSheet> _rawMusicSheets;

		private SettingEntry<bool> settingBackgroundPlayback;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal Conveyor Conveyor { get; private set; }

		internal MusicPlayer MusicPlayer { get; private set; }

		[ImportingConstructor]
		public MusicianModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settingsManager)
		{
			settingBackgroundPlayback = settingsManager.DefineSetting<bool>("backgroundPlayback", false, "No background playback", "Stop key emulation when GW2 is in the background", (SettingTypeRendererDelegate)null);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override void Initialize()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			ICON = ICON ?? ContentsManager.GetTexture("musician_icon.png");
			Conveyor conveyor = new Conveyor();
			((Control)conveyor).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)conveyor).set_Visible(false);
			Conveyor = conveyor;
			_xmlParser = new XmlMusicSheetReader();
			_displayedSheets = new List<SheetButton>();
			HealthPoolButton val = new HealthPoolButton();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_Text("Stop Playback");
			((Control)val).set_ZIndex(-1);
			((Control)val).set_Visible(false);
			_stopButton = val;
			((Control)_stopButton).add_Click((EventHandler<MouseEventArgs>)StopPlayback);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2LostFocus((EventHandler<EventArgs>)OnGw2LostFocus);
		}

		protected override async Task LoadAsync()
		{
			await Task.Run(() => _rawMusicSheets = _xmlParser.LoadDirectory(DirectoriesManager.GetFullDirectoryPath("musician")));
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_musicianTab = GameService.Overlay.get_BlishHudWindow().AddTab("Musician", AsyncTexture2D.op_Implicit(ICON), BuildHomePanel((WindowBase)(object)GameService.Overlay.get_BlishHudWindow()), 0);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnGw2LostFocus(object sender, EventArgs e)
		{
			if (settingBackgroundPlayback.get_Value())
			{
				StopPlayback(null, null);
			}
		}

		protected override void Unload()
		{
			MusicPlayerFactory.Dispose();
			HealthPoolButton stopButton = _stopButton;
			if (stopButton != null)
			{
				((Control)stopButton).Dispose();
			}
			Conveyor conveyor = Conveyor;
			if (conveyor != null)
			{
				((Control)conveyor).Dispose();
			}
			StopPlayback(null, null);
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_musicianTab);
			ModuleInstance = null;
		}

		private void StopPlayback(object o, MouseEventArgs e)
		{
			if (Conveyor != null)
			{
				((Control)Conveyor).set_Visible(false);
			}
			if (_stopButton != null)
			{
				((Control)_stopButton).set_Visible(false);
			}
			MusicPlayer?.Dispose();
			foreach (SheetButton displayedSheet in _displayedSheets)
			{
				displayedSheet.IsPreviewing = false;
			}
		}

		private Panel BuildHomePanel(WindowBase wndw)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Container)wndw).get_ContentRegion().Size);
			Panel hPanel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Location(new Point(((Control)hPanel).get_Width() - 630, 50));
			((Control)val2).set_Size(new Point(630, ((Control)hPanel).get_Size().Y - 50 - 10));
			((Control)val2).set_Parent((Container)(object)hPanel);
			val2.set_CanScroll(true);
			Panel contentPanel = val2;
			Panel val3 = new Panel();
			val3.set_ShowBorder(true);
			((Control)val3).set_Size(new Point(((Control)hPanel).get_Width() - ((Control)contentPanel).get_Width() - 10, ((Control)contentPanel).get_Height() + 10));
			((Control)val3).set_Location(new Point(8, 20));
			((Control)val3).set_Parent((Container)(object)hPanel);
			Panel menuSection = val3;
			Menu val4 = new Menu();
			((Control)val4).set_Size(((Container)menuSection).get_ContentRegion().Size);
			val4.set_MenuItemHeight(40);
			((Control)val4).set_Parent((Container)(object)menuSection);
			Panel lPanel = BuildLibraryPanel(wndw);
			((Control)val4.AddMenuItem("Library", (Texture2D)null)).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				wndw.Navigate(lPanel, true);
			});
			return hPanel;
		}

		private Panel BuildLibraryPanel(WindowBase wndw)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Container)wndw).get_ContentRegion().Size);
			Panel lPanel = val;
			BackButton val2 = new BackButton(wndw);
			val2.set_Text("Musician");
			val2.set_NavTitle("Library");
			((Control)val2).set_Parent((Container)(object)lPanel);
			((Control)val2).set_Location(new Point(20, 20));
			BackButton backButton = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Location(new Point(0, 10 + ((Control)backButton).get_Bottom()));
			((Control)val3).set_Size(new Point(((Control)lPanel).get_Width(), ((Control)lPanel).get_Size().Y - 50 - 10));
			((Control)val3).set_Parent((Container)(object)lPanel);
			val3.set_ShowTint(true);
			val3.set_ShowBorder(true);
			val3.set_CanScroll(true);
			Panel melodyPanel = val3;
			foreach (RawMusicSheet sheet in _rawMusicSheets)
			{
				SheetButton sheetButton = new SheetButton();
				((Control)sheetButton).set_Parent((Container)(object)melodyPanel);
				((DetailsButton)sheetButton).set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("instruments\\" + sheet.Instrument.ToLowerInvariant() + ".png")));
				((DetailsButton)sheetButton).set_IconSize((DetailsIconSize)0);
				sheetButton.Artist = sheet.Artist;
				((Panel)sheetButton).set_Title(sheet.Title);
				sheetButton.User = sheet.User;
				sheetButton.MusicSheet = sheet;
				SheetButton melody = sheetButton;
				_displayedSheets.Add(melody);
				((Control)melody).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					if (melody.MouseOverPlay)
					{
						ScreenNotification.ShowNotification("Practice mode (Synthesia) is not yet implemented.", (NotificationType)0, (Texture2D)null, 4);
					}
					if (melody.MouseOverEmulate)
					{
						StopPlayback(null, null);
						((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
						MusicPlayer = MusicPlayerFactory.Create(melody.MusicSheet, InstrumentMode.Emulate);
						MusicPlayer.Worker.Start();
						((Control)_stopButton).set_Visible(true);
					}
					if (melody.MouseOverPreview)
					{
						ScreenNotification.ShowNotification("Preview is not yet implemented.", (NotificationType)0, (Texture2D)null, 4);
					}
				});
			}
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)lPanel);
			((Control)val4).set_Visible(((Control)melodyPanel).get_Visible());
			((Control)val4).set_Location(new Point(((Control)lPanel).get_Right() - 150 - 10, 5));
			((Control)val4).set_Width(150);
			Dropdown ddSortMethod = val4;
			ddSortMethod.get_Items().Add("Title");
			ddSortMethod.get_Items().Add("Artist");
			ddSortMethod.get_Items().Add("User");
			ddSortMethod.get_Items().Add("------------------");
			ddSortMethod.get_Items().Add("Harp");
			ddSortMethod.get_Items().Add("Flute");
			ddSortMethod.get_Items().Add("Lute");
			ddSortMethod.get_Items().Add("Horn");
			ddSortMethod.get_Items().Add("Bass");
			ddSortMethod.get_Items().Add("Bell");
			ddSortMethod.get_Items().Add("Bell2");
			ddSortMethod.add_ValueChanged((EventHandler<ValueChangedEventArgs>)UpdateSort);
			ddSortMethod.set_SelectedItem("Title");
			UpdateSort(ddSortMethod, EventArgs.Empty);
			((Control)backButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				wndw.NavigateHome();
			});
			return lPanel;
		}

		private Panel BuildComposerPanel(WindowBase wndw)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Expected O, but got Unknown
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Expected O, but got Unknown
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Expected O, but got Unknown
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Expected O, but got Unknown
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Expected O, but got Unknown
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Expected O, but got Unknown
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Container)wndw).get_ContentRegion().Size);
			Panel cPanel = val;
			BackButton val2 = new BackButton(wndw);
			val2.set_Text("Musician");
			val2.set_NavTitle("Composer");
			((Control)val2).set_Parent((Container)(object)cPanel);
			((Control)val2).set_Location(new Point(20, 20));
			BackButton backButton = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Location(new Point(28, 10 + ((Control)backButton).get_Bottom()));
			((Control)val3).set_Size(new Point(((Control)cPanel).get_Size().X - 50 - 8, ((Control)cPanel).get_Size().Y - 50 - 10));
			((Control)val3).set_Parent((Container)(object)cPanel);
			val3.set_CanScroll(false);
			Panel composerPanel = val3;
			TextBox val4 = new TextBox();
			((Control)val4).set_Size(new Point(150, 20));
			((Control)val4).set_Location(new Point(0, 20));
			((TextInputBase)val4).set_PlaceholderText("Title");
			((Control)val4).set_Parent((Container)(object)composerPanel);
			TextBox titleTextBox = val4;
			Label val5 = new Label();
			((Control)val5).set_Size(new Point(20, 20));
			((Control)val5).set_Location(new Point(((Control)titleTextBox).get_Left() + ((Control)titleTextBox).get_Width() + 20, ((Control)titleTextBox).get_Top()));
			val5.set_Text(" - ");
			((Control)val5).set_Parent((Container)(object)composerPanel);
			Label titleArtistLabel = val5;
			TextBox val6 = new TextBox();
			((Control)val6).set_Size(new Point(150, 20));
			((Control)val6).set_Location(new Point(((Control)titleArtistLabel).get_Left() + ((Control)titleArtistLabel).get_Width() + 20, ((Control)titleArtistLabel).get_Top()));
			((TextInputBase)val6).set_PlaceholderText("Artist");
			((Control)val6).set_Parent((Container)(object)composerPanel);
			Label val7 = new Label();
			((Control)val7).set_Size(new Point(150, 20));
			((Control)val7).set_Location(new Point(0, ((Control)titleTextBox).get_Top() + 20 + 10));
			val7.set_Text("Created by");
			((Control)val7).set_Parent((Container)(object)composerPanel);
			Label userLabel = val7;
			TextBox val8 = new TextBox();
			((Control)val8).set_Size(new Point(150, 20));
			((Control)val8).set_Location(new Point(((Control)titleArtistLabel).get_Left() + ((Control)titleArtistLabel).get_Width() + 20, ((Control)userLabel).get_Top()));
			((TextInputBase)val8).set_PlaceholderText("User.####");
			((Control)val8).set_Parent((Container)(object)composerPanel);
			TextBox userTextBox = val8;
			Dropdown val9 = new Dropdown();
			((Control)val9).set_Parent((Container)(object)composerPanel);
			((Control)val9).set_Location(new Point(0, ((Control)userTextBox).get_Top() + 20 + 10));
			((Control)val9).set_Width(150);
			Dropdown ddInstrumentSelection = val9;
			string[] instruments = Instruments;
			foreach (string item in instruments)
			{
				ddInstrumentSelection.get_Items().Add(item);
			}
			Dropdown val10 = new Dropdown();
			((Control)val10).set_Parent((Container)(object)composerPanel);
			((Control)val10).set_Location(new Point(((Control)titleArtistLabel).get_Left() + ((Control)titleArtistLabel).get_Width() + 20, ((Control)ddInstrumentSelection).get_Top()));
			((Control)val10).set_Width(150);
			val10.get_Items().Add("Favor Notes");
			val10.get_Items().Add("Favor Chords");
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)composerPanel);
			((Control)val11).set_Location(new Point(0, ((Control)ddInstrumentSelection).get_Top() + 22 + 10));
			((Control)val11).set_Size(new Point(150, 20));
			val11.set_Text("Beats per minute:");
			Label tempoLabel = val11;
			CounterBox val12 = new CounterBox();
			((Control)val12).set_Parent((Container)(object)composerPanel);
			((Control)val12).set_Location(new Point(((Control)titleArtistLabel).get_Left() + ((Control)titleArtistLabel).get_Width() + 20, ((Control)tempoLabel).get_Top()));
			val12.set_ValueWidth(50);
			val12.set_MaxValue(200);
			val12.set_MinValue(40);
			val12.set_Numerator(5);
			val12.set_Value(90);
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)composerPanel);
			((Control)val13).set_Location(new Point(0, ((Control)tempoLabel).get_Top() + 22 + 10));
			((Control)val13).set_Size(new Point(150, 20));
			val13.set_Text("Notes per beat:");
			Label meterLabel = val13;
			CounterBox val14 = new CounterBox();
			((Control)val14).set_Parent((Container)(object)composerPanel);
			((Control)val14).set_Location(new Point(((Control)titleArtistLabel).get_Left() + ((Control)titleArtistLabel).get_Width() + 20, ((Control)meterLabel).get_Top()));
			val14.set_ValueWidth(50);
			val14.set_MaxValue(16);
			val14.set_MinValue(1);
			val14.set_Prefix("1\\");
			val14.set_Exponential(true);
			val14.set_Value(1);
			CounterBox meterCounterBox = val14;
			Label val15 = new Label();
			((Control)val15).set_Size(new Point(((Control)composerPanel).get_Width(), ((Control)composerPanel).get_Height() - 300));
			((Control)val15).set_Location(new Point(0, ((Control)meterCounterBox).get_Top() + 22 + 10));
			((Control)val15).set_Parent((Container)(object)composerPanel);
			Label notationTextBox = val15;
			StandardButton val16 = new StandardButton();
			val16.set_Text("Save");
			((Control)val16).set_Location(new Point(((Control)composerPanel).get_Width() - 128 - 5, ((Control)notationTextBox).get_Bottom() + 5));
			((Control)val16).set_Width(128);
			((Control)val16).set_Height(26);
			((Control)val16).set_Parent((Container)(object)composerPanel);
			((Control)val16).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
			});
			((Control)backButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				wndw.NavigateHome();
			});
			return cPanel;
		}

		private void UpdateSort(object sender, EventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			switch (((Dropdown)sender).get_SelectedItem())
			{
			case "Title":
				_displayedSheets.Sort((SheetButton e1, SheetButton e2) => ((Panel)e1).get_Title().CompareTo(((Panel)e2).get_Title()));
				foreach (SheetButton displayedSheet in _displayedSheets)
				{
					((Control)displayedSheet).set_Visible(true);
				}
				break;
			case "Artist":
				_displayedSheets.Sort((SheetButton e1, SheetButton e2) => e1.Artist.CompareTo(e2.Artist));
				foreach (SheetButton displayedSheet2 in _displayedSheets)
				{
					((Control)displayedSheet2).set_Visible(true);
				}
				break;
			case "User":
				_displayedSheets.Sort((SheetButton e1, SheetButton e2) => e1.User.CompareTo(e2.User));
				foreach (SheetButton displayedSheet3 in _displayedSheets)
				{
					((Control)displayedSheet3).set_Visible(true);
				}
				break;
			case "Harp":
				_displayedSheets.Sort((SheetButton e1, SheetButton e2) => e1.MusicSheet.Instrument.CompareTo(e2.MusicSheet.Instrument));
				foreach (SheetButton displayedSheet4 in _displayedSheets)
				{
					((Control)displayedSheet4).set_Visible(string.Equals(displayedSheet4.MusicSheet.Instrument, "Harp", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Flute":
				foreach (SheetButton displayedSheet5 in _displayedSheets)
				{
					((Control)displayedSheet5).set_Visible(string.Equals(displayedSheet5.MusicSheet.Instrument, "Flute", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Lute":
				foreach (SheetButton displayedSheet6 in _displayedSheets)
				{
					((Control)displayedSheet6).set_Visible(string.Equals(displayedSheet6.MusicSheet.Instrument, "Lute", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Horn":
				foreach (SheetButton displayedSheet7 in _displayedSheets)
				{
					((Control)displayedSheet7).set_Visible(string.Equals(displayedSheet7.MusicSheet.Instrument, "Horn", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Bass":
				foreach (SheetButton displayedSheet8 in _displayedSheets)
				{
					((Control)displayedSheet8).set_Visible(string.Equals(displayedSheet8.MusicSheet.Instrument, "Bass", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Bell":
				foreach (SheetButton displayedSheet9 in _displayedSheets)
				{
					((Control)displayedSheet9).set_Visible(string.Equals(displayedSheet9.MusicSheet.Instrument, "Bell", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			case "Bell2":
				foreach (SheetButton displayedSheet10 in _displayedSheets)
				{
					((Control)displayedSheet10).set_Visible(string.Equals(displayedSheet10.MusicSheet.Instrument, "Bell2", StringComparison.InvariantCultureIgnoreCase));
				}
				break;
			}
			RepositionMel();
		}

		private void RepositionMel()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int pos = 0;
			foreach (SheetButton displayedSheet in _displayedSheets)
			{
				int x = pos % 3;
				int y = pos / 3;
				((Control)displayedSheet).set_Location(new Point(x * 335, y * 108));
				((Container)(Panel)((Control)displayedSheet).get_Parent()).set_VerticalScrollOffset(0);
				((Control)((Control)displayedSheet).get_Parent()).Invalidate();
				if (((Control)displayedSheet).get_Visible())
				{
					pos++;
				}
			}
		}
	}
}
