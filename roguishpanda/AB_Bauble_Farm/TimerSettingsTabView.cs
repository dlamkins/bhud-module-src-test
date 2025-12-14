using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.Json;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace roguishpanda.AB_Bauble_Farm
{
	public class TimerSettingsTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<MainWindowModule>();

		private MainWindowModule _BaubleFarmModule;

		private AsyncTexture2D _NoTexture;

		private Panel[] _timerEventsPanels;

		private TextBox[] _timerEventTextbox;

		private Panel _timerPackagePanel;

		private int _CurrentEventSelected;

		private Panel _SettingsControlPanel;

		private TextBox _textNewEvent;

		private Label _CreateEventAlert;

		private StandardButton _buttonRestartModule;

		private Label _MinutesLabelDisplay;

		private Label _SecondsLabelDisplay;

		private Label _CurrentEventLabel;

		private Panel _timerEventsTitlePanel;

		private Label _timerEventsTitleLabel;

		private Image[] _cancelButton;

		private Image[] _upArrowButton;

		private Image[] _downArrowButton;

		private Image[] _broadcastImage;

		private Checkbox[] _broadcastCheckbox;

		private AsyncTexture2D _cancelTexture;

		private AsyncTexture2D _addTexture;

		private AsyncTexture2D _broadcastTexture;

		private Texture2D _upArrowTexture;

		private Texture2D _downArrowTexture;

		private Panel _timerSettingsPanel;

		private SettingEntry<KeyBinding> _timerKeybind;

		private SettingEntry<int> _timerMinutesDefault;

		private SettingEntry<int> _timerSecondsDefault;

		private ViewContainer _settingsViewContainer;

		private SettingCollection _MainSettings;

		private List<TimerDetailData> _eventNotes;

		private List<PackageData> _PackageData;

		private int TimerRowNum;

		private StandardButton _buttonSaveEvents;

		private StandardButton _buttonReloadEvents;

		private string _CurrentPackage;

		private Label[] _WaypointsLabel;

		private TextBox[] _WaypointsTextbox;

		private Label[] _NotesLabel;

		private MultilineTextBox[] _NotesTextbox;

		private Label _TTSLabel;

		private TextBox _TTSTextbox;

		private Checkbox _TTSCheckbox;

		private StandardButton _TTSTest;

		private SettingCollection _Settings;

		private SettingEntry<int> _TTSVolumeSettingEntry;

		private SettingEntry<int> _TTSSpeedSettingEntry;

		public readonly JsonSerializerOptions _jsonOptions;

		protected override void Build(Container buildPanel)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Expected O, but got Unknown
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Expected O, but got Unknown
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Expected O, but got Unknown
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Expected O, but got Unknown
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Expected O, but got Unknown
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Expected O, but got Unknown
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Expected O, but got Unknown
			_BaubleFarmModule = MainWindowModule.ModuleInstance;
			_MainSettings = _BaubleFarmModule._settings;
			_eventNotes = new List<TimerDetailData>(_BaubleFarmModule._timerEvents);
			_PackageData = new List<PackageData>(_BaubleFarmModule._PackageData);
			TimerRowNum = _BaubleFarmModule.TimerRowNum;
			_CurrentPackage = _BaubleFarmModule._CurrentPackage;
			_NoTexture = new AsyncTexture2D();
			_cancelTexture = AsyncTexture2D.FromAssetId(2175782);
			_addTexture = AsyncTexture2D.FromAssetId(155911);
			_broadcastTexture = AsyncTexture2D.FromAssetId(1234950);
			_upArrowTexture = _BaubleFarmModule.ContentsManager.GetTexture("png\\517181.png");
			_downArrowTexture = _BaubleFarmModule.ContentsManager.GetTexture("png\\517181-180.png");
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			int num = ((Rectangle)(ref contentRegion)).get_Size().X + 500;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y + 400));
			contentRegion = buildPanel.get_ContentRegion();
			int x = ((Rectangle)(ref contentRegion)).get_Location().X;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Location(new Point(x, ((Rectangle)(ref contentRegion)).get_Location().Y - 35));
			val.set_CanScroll(true);
			val.set_BackgroundTexture(MainWindowModule.ModuleInstance._asyncTimertexture);
			_timerSettingsPanel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_timerSettingsPanel);
			((Control)val2).set_Size(new Point(300, 400));
			((Control)val2).set_Location(new Point(100, 100));
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			_timerPackagePanel = val2;
			Label val3 = new Label();
			val3.set_Text("Add Timer:");
			((Control)val3).set_Size(new Point(200, 30));
			((Control)val3).set_Location(new Point(100, 510));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Parent((Container)(object)_timerSettingsPanel);
			TextBox val4 = new TextBox();
			((Control)val4).set_Size(new Point(300, 40));
			((Control)val4).set_Location(new Point(100, 540));
			((Control)val4).set_Parent((Container)(object)_timerSettingsPanel);
			_textNewEvent = val4;
			Image val5 = new Image();
			val5.set_Texture(_addTexture);
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Location(new Point(405, 540));
			((Control)val5).set_Parent((Container)(object)_timerSettingsPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)CreateEvent_Click);
			StandardButton val6 = new StandardButton();
			val6.set_Text("Save");
			((Control)val6).set_Size(new Point(140, 40));
			((Control)val6).set_Location(new Point(530, 550));
			((Control)val6).set_Visible(false);
			((Control)val6).set_Parent((Container)(object)_timerSettingsPanel);
			_buttonSaveEvents = val6;
			((Control)_buttonSaveEvents).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CreateEventJson();
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Reload");
			((Control)val7).set_Size(new Point(140, 40));
			((Control)val7).set_Location(new Point(680, 550));
			((Control)val7).set_Visible(false);
			((Control)val7).set_Parent((Container)(object)_timerSettingsPanel);
			_buttonReloadEvents = val7;
			((Control)_buttonReloadEvents).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ReloadEvents();
			});
			Label val8 = new Label();
			((Control)val8).set_Size(new Point(400, 40));
			((Control)val8).set_Location(new Point(530, 590));
			val8.set_Font(GameService.Content.get_DefaultFont16());
			val8.set_TextColor(Color.get_Red());
			((Control)val8).set_Visible(false);
			((Control)val8).set_Parent((Container)(object)_timerSettingsPanel);
			_CreateEventAlert = val8;
			StandardButton val9 = new StandardButton();
			val9.set_Text("Restart Module");
			((Control)val9).set_Size(new Point(200, 40));
			((Control)val9).set_Location(new Point(530, 550));
			((Control)val9).set_Visible(false);
			((Control)val9).set_Parent((Container)(object)_timerSettingsPanel);
			_buttonRestartModule = val9;
			((Control)_buttonRestartModule).add_Click((EventHandler<MouseEventArgs>)RestartModule_Click);
			Label val10 = new Label();
			((Control)val10).set_Size(new Point(300, 40));
			((Control)val10).set_Location(new Point(420, 60));
			val10.set_Font(GameService.Content.get_DefaultFont32());
			val10.set_TextColor(Color.get_LimeGreen());
			((Control)val10).set_Parent((Container)(object)_timerSettingsPanel);
			_CurrentEventLabel = val10;
			AsyncTexture2D TitleTexture = AsyncTexture2D.FromAssetId(1234872);
			Panel val11 = new Panel();
			((Control)val11).set_Parent((Container)(object)_timerSettingsPanel);
			((Control)val11).set_Size(new Point(290, 40));
			((Control)val11).set_Location(new Point(102, 60));
			val11.set_BackgroundTexture(TitleTexture);
			_timerEventsTitlePanel = val11;
			Label val12 = new Label();
			val12.set_Text("Timers");
			((Control)val12).set_Size(new Point(300, 40));
			((Control)val12).set_Location(new Point(10, 0));
			val12.set_Font(GameService.Content.get_DefaultFont16());
			val12.set_TextColor(Color.get_White());
			((Control)val12).set_Parent((Container)(object)_timerEventsTitlePanel);
			_timerEventsTitleLabel = val12;
			_timerEventsPanels = (Panel[])(object)new Panel[TimerRowNum];
			_timerEventTextbox = (TextBox[])(object)new TextBox[TimerRowNum];
			_cancelButton = (Image[])(object)new Image[TimerRowNum];
			_upArrowButton = (Image[])(object)new Image[TimerRowNum];
			_downArrowButton = (Image[])(object)new Image[TimerRowNum];
			LoadEventTable(TimerRowNum);
			if (TimerRowNum != 0)
			{
				TimerSettings_Click(_timerEventsPanels[0], null);
			}
		}

		private void CurrentEvent_TextChanged(int Index)
		{
			try
			{
				string NewDescription = ((TextInputBase)_timerEventTextbox[Index]).get_Text();
				_CurrentEventLabel.set_Text(NewDescription);
				_eventNotes[Index].Description = NewDescription;
				((Control)_buttonSaveEvents).set_Visible(true);
				((Control)_buttonReloadEvents).set_Visible(true);
				((Control)_buttonRestartModule).set_Visible(false);
				((Control)_CreateEventAlert).set_Visible(false);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to rename event: " + ex.Message);
			}
		}

		private void RestartModule_Click(object sender, MouseEventArgs e)
		{
			_BaubleFarmModule.Restart();
			((Control)_buttonRestartModule).set_Visible(false);
			((Control)_CreateEventAlert).set_Visible(false);
		}

		private void CreateEvent_Click(object sender, MouseEventArgs e)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				List<TimerDetailData> originalNotesData = _eventNotes;
				int NewID = 1;
				if (_eventNotes.Count > 0)
				{
					NewID = originalNotesData.Max((TimerDetailData note) => note.ID) + 1;
				}
				if (((TextInputBase)_textNewEvent).get_Text().Length < 4)
				{
					_CreateEventAlert.set_Text("* 4 characters mininimum required to create new event");
					((Control)_CreateEventAlert).set_Visible(true);
					_CreateEventAlert.set_TextColor(Color.get_Red());
					return;
				}
				foreach (TimerDetailData item in originalNotesData)
				{
					if (item.Description == ((TextInputBase)_textNewEvent).get_Text())
					{
						_CreateEventAlert.set_Text("* This event already exists");
						((Control)_CreateEventAlert).set_Visible(true);
						_CreateEventAlert.set_TextColor(Color.get_Red());
						return;
					}
				}
				_CreateEventAlert.set_Text("Event has been added! Click save to confirm changes!");
				((Control)_CreateEventAlert).set_Visible(true);
				_CreateEventAlert.set_TextColor(Color.get_LimeGreen());
				TimerDetailData notesData = new TimerDetailData
				{
					ID = NewID,
					Description = ((TextInputBase)_textNewEvent).get_Text(),
					Minutes = 8.0,
					Seconds = 30.0,
					TTSText = "",
					TTSActive = false,
					WaypointData = new List<NotesData>
					{
						new NotesData
						{
							Type = "",
							Notes = "",
							Broadcast = false
						}
					},
					NotesData = new List<NotesData>
					{
						new NotesData
						{
							Type = "",
							Notes = "",
							Broadcast = false
						},
						new NotesData
						{
							Type = "",
							Notes = "",
							Broadcast = false
						},
						new NotesData
						{
							Type = "",
							Notes = "",
							Broadcast = false
						},
						new NotesData
						{
							Type = "",
							Notes = "",
							Broadcast = false
						}
					}
				};
				_eventNotes.Add(notesData);
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_timerEventsPanels[i]).Dispose();
					((Control)_timerEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				if (_settingsViewContainer != null)
				{
					_settingsViewContainer.Clear();
					((Control)_settingsViewContainer).Dispose();
				}
				TimerRowNum = _eventNotes.Count();
				_timerEventsPanels = (Panel[])(object)new Panel[TimerRowNum];
				_timerEventTextbox = (TextBox[])(object)new TextBox[TimerRowNum];
				_cancelButton = (Image[])(object)new Image[TimerRowNum];
				_upArrowButton = (Image[])(object)new Image[TimerRowNum];
				_downArrowButton = (Image[])(object)new Image[TimerRowNum];
				LoadEventTable(TimerRowNum);
				TimerSettings_Click(_timerEventsPanels[TimerRowNum - 1], null);
				((TextInputBase)_textNewEvent).set_Text("");
				((Control)_buttonSaveEvents).set_Visible(true);
				((Control)_buttonReloadEvents).set_Visible(true);
				((Control)_buttonRestartModule).set_Visible(false);
				((Control)_MinutesLabelDisplay).set_Visible(true);
				((Control)_SecondsLabelDisplay).set_Visible(true);
				((Control)_CurrentEventLabel).set_Visible(true);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to created event: " + ex.Message);
			}
		}

		public void ReplacePackage(List<PackageData> packageList, PackageData newPackage)
		{
			for (int i = 0; i < packageList.Count; i++)
			{
				if (packageList[i].PackageName == newPackage.PackageName)
				{
					packageList[i] = newPackage;
					return;
				}
			}
			packageList.Add(newPackage);
		}

		private void CreateEventJson()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			PackageData package = _PackageData.FirstOrDefault((PackageData p) => p.PackageName == _CurrentPackage);
			if (package != null)
			{
				package.TimerDetailData = _eventNotes;
				ReplacePackage(_PackageData, package);
				string jsonFilePath = Path.Combine(_BaubleFarmModule.DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Package_Defaults.json");
				try
				{
					string jsonContent = JsonSerializer.Serialize<List<PackageData>>(_PackageData, _jsonOptions);
					File.WriteAllText(jsonFilePath, jsonContent);
					_CreateEventAlert.set_Text("Events have been saved!");
					((Control)_CreateEventAlert).set_Visible(true);
					((Control)_buttonReloadEvents).set_Visible(false);
					_CreateEventAlert.set_TextColor(Color.get_LimeGreen());
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to save JSON file: " + ex.Message);
				}
				_BaubleFarmModule.Restart();
				return;
			}
			throw new ArgumentException("No PackageData found with PackageName: " + _CurrentPackage);
		}

		private void CancelEvent_Click(int Index)
		{
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string Description = _eventNotes[Index].Description;
				int ID = _eventNotes[Index].ID;
				_eventNotes.RemoveAll((TimerDetailData note) => note.Description == Description && note.ID == ID);
				_eventNotes = _eventNotes.Select((TimerDetailData note, int index) => new TimerDetailData
				{
					ID = index + 1,
					Description = note.Description,
					Minutes = note.Minutes,
					Seconds = note.Seconds,
					TTSText = note.TTSText,
					TTSActive = note.TTSActive,
					WaypointData = note.WaypointData,
					NotesData = note.NotesData
				}).ToList();
				if (_eventNotes.Count <= 0)
				{
					((Control)_timerEventsPanels[0]).Dispose();
					((Control)_timerEventTextbox[0]).Dispose();
					((Control)_cancelButton[0]).Dispose();
					((Control)_upArrowButton[0]).Dispose();
					((Control)_downArrowButton[0]).Dispose();
					_settingsViewContainer.Clear();
					((Control)_settingsViewContainer).Dispose();
					((Control)_SettingsControlPanel).Dispose();
					((Control)_MinutesLabelDisplay).set_Visible(false);
					((Control)_SecondsLabelDisplay).set_Visible(false);
					((Control)_CurrentEventLabel).set_Visible(false);
					((Control)_buttonSaveEvents).set_Visible(true);
					((Control)_buttonReloadEvents).set_Visible(true);
					((Control)_buttonRestartModule).set_Visible(false);
					return;
				}
				_eventNotes.Max((TimerDetailData note) => note.ID);
				_MainSettings.AddSubCollection(_CurrentPackage + "_PackageInfo", false).AddSubCollection("TimerInfo_" + ID, false).UndefineSetting("TimerInfo_" + ID);
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_timerEventsPanels[i]).Dispose();
					((Control)_timerEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				_settingsViewContainer.Clear();
				((Control)_settingsViewContainer).Dispose();
				TimerRowNum = _eventNotes.Count();
				_timerEventsPanels = (Panel[])(object)new Panel[TimerRowNum];
				_timerEventTextbox = (TextBox[])(object)new TextBox[TimerRowNum];
				_cancelButton = (Image[])(object)new Image[TimerRowNum];
				_upArrowButton = (Image[])(object)new Image[TimerRowNum];
				_downArrowButton = (Image[])(object)new Image[TimerRowNum];
				LoadEventTable(TimerRowNum);
				TimerSettings_Click(_timerEventsPanels[0], null);
				((Control)_buttonSaveEvents).set_Visible(true);
				((Control)_buttonReloadEvents).set_Visible(true);
				((Control)_buttonRestartModule).set_Visible(false);
				((Control)_CreateEventAlert).set_Visible(true);
				_CreateEventAlert.set_Text("Event was deleted!");
				_CreateEventAlert.set_TextColor(Color.get_LimeGreen());
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to remove event: " + ex.Message);
			}
		}

		private void MoveEvent_Click(int Index, int Direction)
		{
			try
			{
				_ = _eventNotes[Index].Description;
				int ID = _eventNotes[Index].ID;
				TimerDetailData temp = _eventNotes[Index];
				_eventNotes[Index] = _eventNotes[Index + Direction];
				_eventNotes[Index + Direction] = temp;
				_MainSettings.AddSubCollection(_CurrentPackage + "_PackageInfo", false).AddSubCollection("TimerInfo_" + ID, false).UndefineSetting("TimerInfo_" + ID);
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_timerEventsPanels[i]).Dispose();
					((Control)_timerEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				_settingsViewContainer.Clear();
				((Control)_settingsViewContainer).Dispose();
				TimerRowNum = _eventNotes.Count();
				_timerEventsPanels = (Panel[])(object)new Panel[TimerRowNum];
				_timerEventTextbox = (TextBox[])(object)new TextBox[TimerRowNum];
				_cancelButton = (Image[])(object)new Image[TimerRowNum];
				_upArrowButton = (Image[])(object)new Image[TimerRowNum];
				_downArrowButton = (Image[])(object)new Image[TimerRowNum];
				LoadEventTable(TimerRowNum);
				TimerSettings_Click(_timerEventsPanels[Index + Direction], null);
				((Control)_buttonSaveEvents).set_Visible(true);
				((Control)_buttonReloadEvents).set_Visible(true);
				((Control)_buttonRestartModule).set_Visible(false);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to move event: " + ex.Message);
			}
		}

		private void ReloadEvents()
		{
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_eventNotes = new List<TimerDetailData>(_BaubleFarmModule._timerEvents);
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_timerEventsPanels[i]).Dispose();
					((Control)_timerEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				_settingsViewContainer.Clear();
				((Control)_settingsViewContainer).Dispose();
				TimerRowNum = _eventNotes.Count();
				_timerEventsPanels = (Panel[])(object)new Panel[TimerRowNum];
				_timerEventTextbox = (TextBox[])(object)new TextBox[TimerRowNum];
				_cancelButton = (Image[])(object)new Image[TimerRowNum];
				_upArrowButton = (Image[])(object)new Image[TimerRowNum];
				_downArrowButton = (Image[])(object)new Image[TimerRowNum];
				LoadEventTable(TimerRowNum);
				TimerSettings_Click(_timerEventsPanels[0], null);
				((Control)_buttonSaveEvents).set_Visible(false);
				((Control)_buttonReloadEvents).set_Visible(false);
				((Control)_CreateEventAlert).set_Visible(true);
				((Control)_MinutesLabelDisplay).set_Visible(true);
				((Control)_SecondsLabelDisplay).set_Visible(true);
				((Control)_CurrentEventLabel).set_Visible(true);
				_CreateEventAlert.set_Text("Events have reloaded!");
				_CreateEventAlert.set_TextColor(Color.get_LimeGreen());
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to reload events: " + ex.Message);
			}
		}

		public void LoadEventTable(int TotalEvents)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Expected O, but got Unknown
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Expected O, but got Unknown
			try
			{
				List<TimerDetailData> eventNotes = _eventNotes;
				for (int i = 0; i < TotalEvents; i++)
				{
					int Index = i;
					Panel[] timerEventsPanels = _timerEventsPanels;
					int num = i;
					Panel val = new Panel();
					((Control)val).set_Parent((Container)(object)_timerPackagePanel);
					((Control)val).set_Size(new Point(300, 40));
					((Control)val).set_Location(new Point(0, i * 40));
					timerEventsPanels[num] = val;
					((Control)_timerEventsPanels[i]).add_Click((EventHandler<MouseEventArgs>)TimerSettings_Click);
					if (i % 2 == 0)
					{
						((Control)_timerEventsPanels[i]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.5f));
					}
					else
					{
						((Control)_timerEventsPanels[i]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.2f));
					}
					TextBox[] timerEventTextbox = _timerEventTextbox;
					int num2 = i;
					TextBox val2 = new TextBox();
					((TextInputBase)val2).set_Text(eventNotes[i].Description);
					((Control)val2).set_Size(new Point(200, 30));
					((Control)val2).set_Location(new Point(30, 5));
					val2.set_HorizontalAlignment((HorizontalAlignment)0);
					((TextInputBase)val2).set_Font(GameService.Content.get_DefaultFont16());
					val2.set_HideBackground(true);
					((TextInputBase)val2).set_ForeColor(Color.get_LimeGreen());
					((Control)val2).set_Parent((Container)(object)_timerEventsPanels[i]);
					timerEventTextbox[num2] = val2;
					((TextInputBase)_timerEventTextbox[i]).add_TextChanged((EventHandler<EventArgs>)delegate
					{
						CurrentEvent_TextChanged(Index);
					});
					Image[] cancelButton = _cancelButton;
					int num3 = i;
					Image val3 = new Image();
					val3.set_Texture(_cancelTexture);
					((Control)val3).set_Size(new Point(16, 16));
					((Control)val3).set_Location(new Point(10, 10));
					((Control)val3).set_Visible(false);
					((Control)val3).set_Parent((Container)(object)_timerEventsPanels[i]);
					cancelButton[num3] = val3;
					((Control)_cancelButton[i]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						CancelEvent_Click(Index);
					});
					Image[] upArrowButton = _upArrowButton;
					int num4 = i;
					Image val4 = new Image();
					val4.set_Texture(AsyncTexture2D.op_Implicit(_upArrowTexture));
					((Control)val4).set_Size(new Point(20, 20));
					((Control)val4).set_Location(new Point(240, 4));
					((Control)val4).set_Visible(false);
					((Control)val4).set_Parent((Container)(object)_timerEventsPanels[i]);
					upArrowButton[num4] = val4;
					((Control)_upArrowButton[i]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						MoveEvent_Click(Index, -1);
					});
					Image[] downArrowButton = _downArrowButton;
					int num5 = i;
					Image val5 = new Image();
					val5.set_Texture(AsyncTexture2D.op_Implicit(_downArrowTexture));
					((Control)val5).set_Size(new Point(20, 20));
					((Control)val5).set_Location(new Point(240, 20));
					((Control)val5).set_Visible(false);
					((Control)val5).set_Parent((Container)(object)_timerEventsPanels[i]);
					downArrowButton[num5] = val5;
					((Control)_downArrowButton[i]).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						MoveEvent_Click(Index, 1);
					});
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load events: " + ex.Message);
			}
		}

		private void TimerSettings_Click(object sender, MouseEventArgs e)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Expected O, but got Unknown
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Expected O, but got Unknown
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Expected O, but got Unknown
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Expected O, but got Unknown
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Expected O, but got Unknown
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Expected O, but got Unknown
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Expected O, but got Unknown
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_069d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c2: Expected O, but got Unknown
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Expected O, but got Unknown
			//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0806: Unknown result type (might be due to invalid IL or missing references)
			//IL_0810: Unknown result type (might be due to invalid IL or missing references)
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_0822: Unknown result type (might be due to invalid IL or missing references)
			//IL_0832: Unknown result type (might be due to invalid IL or missing references)
			//IL_0843: Expected O, but got Unknown
			//IL_0844: Unknown result type (might be due to invalid IL or missing references)
			//IL_0849: Unknown result type (might be due to invalid IL or missing references)
			//IL_084e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0858: Unknown result type (might be due to invalid IL or missing references)
			//IL_0860: Unknown result type (might be due to invalid IL or missing references)
			//IL_086a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Expected O, but got Unknown
			//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0905: Unknown result type (might be due to invalid IL or missing references)
			//IL_090a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0914: Unknown result type (might be due to invalid IL or missing references)
			//IL_091f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0929: Unknown result type (might be due to invalid IL or missing references)
			//IL_0930: Unknown result type (might be due to invalid IL or missing references)
			//IL_0941: Expected O, but got Unknown
			//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				object[] timerEventsPanels = _timerEventsPanels;
				int senderIndex = Array.IndexOf(timerEventsPanels, sender);
				List<TimerDetailData> eventNotes = _eventNotes;
				if (_settingsViewContainer != null)
				{
					_settingsViewContainer.Clear();
					((Control)_settingsViewContainer).Dispose();
				}
				if (_SettingsControlPanel != null)
				{
					((Control)_SettingsControlPanel).Dispose();
				}
				SettingCollection TimerCollector = _MainSettings.AddSubCollection(_CurrentPackage + "_PackageInfo", false).AddSubCollection("TimerInfo_" + _eventNotes[senderIndex].ID, false);
				_CurrentEventSelected = senderIndex;
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_timerSettingsPanel);
				((Control)val).set_Location(new Point(410, 110));
				((Control)val).set_Size(new Point(600, 400));
				val.set_CanScroll(true);
				_SettingsControlPanel = val;
				ViewContainer val2 = new ViewContainer();
				((Control)val2).set_Parent((Container)(object)_SettingsControlPanel);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Size(new Point(500, 100));
				_settingsViewContainer = val2;
				SettingsView settingsView = new SettingsView(TimerCollector, -1);
				_settingsViewContainer.Show((IView)(object)settingsView);
				_CurrentEventLabel.set_Text(_eventNotes[senderIndex].Description);
				Label val3 = new Label();
				((Control)val3).set_Size(new Point(100, 40));
				((Control)val3).set_Location(new Point(480, 20));
				val3.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val3).set_Parent((Container)(object)_SettingsControlPanel);
				_MinutesLabelDisplay = val3;
				Label val4 = new Label();
				((Control)val4).set_Size(new Point(100, 40));
				((Control)val4).set_Location(new Point(480, 40));
				val4.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val4).set_Parent((Container)(object)_SettingsControlPanel);
				_SecondsLabelDisplay = val4;
				_timerKeybind = new SettingEntry<KeyBinding>();
				_timerKeybind = TimerCollector.DefineSetting<KeyBinding>("Keybind", new KeyBinding((Keys)0), (Func<string>)(() => "Keybind"), (Func<string>)(() => "Keybind is used to control start/stop for timer"));
				_timerMinutesDefault = TimerCollector.DefineSetting<int>("TimerMinutes", Convert.ToInt32(_eventNotes[senderIndex].Minutes), (Func<string>)(() => "Timer (minutes)"), (Func<string>)(() => "Use to control minutes on the timer"));
				SettingComplianceExtensions.SetRange(_timerMinutesDefault, 0, 59);
				_MinutesLabelDisplay.set_Text(_timerMinutesDefault.get_Value() + " Minutes");
				_timerMinutesDefault.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
				{
					LoadTimeCustomized(senderIndex);
				});
				_timerSecondsDefault = TimerCollector.DefineSetting<int>("TimerSeconds", Convert.ToInt32(_eventNotes[senderIndex].Seconds), (Func<string>)(() => "Timer (seconds)"), (Func<string>)(() => "Use to control seconds on the timer"));
				SettingComplianceExtensions.SetRange(_timerSecondsDefault, 0, 59);
				_SecondsLabelDisplay.set_Text(_timerSecondsDefault.get_Value() + " Seconds");
				_timerSecondsDefault.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
				{
					LoadTimeCustomized(senderIndex);
				});
				_ = _eventNotes[senderIndex].WaypointData.Count;
				_ = _eventNotes[senderIndex].NotesData.Count;
				_WaypointsLabel = (Label[])(object)new Label[1];
				_WaypointsTextbox = (TextBox[])(object)new TextBox[1];
				_NotesLabel = (Label[])(object)new Label[4];
				_NotesTextbox = (MultilineTextBox[])(object)new MultilineTextBox[4];
				_broadcastImage = (Image[])(object)new Image[4];
				_broadcastCheckbox = (Checkbox[])(object)new Checkbox[4];
				int currentControlCount = 0;
				for (int y = 0; y < 1; y++)
				{
					Label[] waypointsLabel = _WaypointsLabel;
					int num = y;
					Label val5 = new Label();
					val5.set_Text("Waypoint:");
					((Control)val5).set_Size(new Point(100, 40));
					((Control)val5).set_Location(new Point(0, 90));
					val5.set_HorizontalAlignment((HorizontalAlignment)2);
					val5.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val5).set_Parent((Container)(object)_SettingsControlPanel);
					waypointsLabel[num] = val5;
					TextBox[] waypointsTextbox = _WaypointsTextbox;
					int num2 = y;
					TextBox val6 = new TextBox();
					((Control)val6).set_Size(new Point(350, 40));
					((Control)val6).set_Location(new Point(110, 90));
					((TextInputBase)val6).set_Font(GameService.Content.get_DefaultFont16());
					((Control)val6).set_Parent((Container)(object)_SettingsControlPanel);
					waypointsTextbox[num2] = val6;
					((TextInputBase)_WaypointsTextbox[y]).add_TextChanged((EventHandler<EventArgs>)_WaypointsTextbox_TextChanged);
					if (eventNotes[senderIndex].WaypointData.Count > y)
					{
						((TextInputBase)_WaypointsTextbox[y]).set_Text(eventNotes[senderIndex].WaypointData[y].Notes);
					}
				}
				for (int z = 0; z < 4; z++)
				{
					Label[] notesLabel = _NotesLabel;
					int num3 = z;
					Label val7 = new Label();
					val7.set_Text("Note #" + (z + 1) + ":");
					((Control)val7).set_Size(new Point(100, 40));
					((Control)val7).set_Location(new Point(0, 140 + currentControlCount * 90));
					val7.set_HorizontalAlignment((HorizontalAlignment)2);
					val7.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val7).set_Parent((Container)(object)_SettingsControlPanel);
					notesLabel[num3] = val7;
					MultilineTextBox[] notesTextbox = _NotesTextbox;
					int num4 = z;
					MultilineTextBox val8 = new MultilineTextBox();
					((Control)val8).set_Size(new Point(450, 80));
					((Control)val8).set_Location(new Point(110, 140 + currentControlCount * 90));
					((TextInputBase)val8).set_Font(GameService.Content.get_DefaultFont16());
					((Control)val8).set_Parent((Container)(object)_SettingsControlPanel);
					notesTextbox[num4] = val8;
					Image[] broadcastImage = _broadcastImage;
					int num5 = z;
					Image val9 = new Image();
					val9.set_Texture(_broadcastTexture);
					((Control)val9).set_Size(new Point(32, 32));
					((Control)val9).set_Location(new Point(50, 170 + currentControlCount * 90));
					((Control)val9).set_Parent((Container)(object)_SettingsControlPanel);
					broadcastImage[num5] = val9;
					Checkbox[] broadcastCheckbox = _broadcastCheckbox;
					int num6 = z;
					Checkbox val10 = new Checkbox();
					((Control)val10).set_Size(new Point(32, 32));
					((Control)val10).set_Location(new Point(80, 170 + currentControlCount * 90));
					((Control)val10).set_Parent((Container)(object)_SettingsControlPanel);
					broadcastCheckbox[num6] = val10;
					((TextInputBase)_NotesTextbox[z]).add_TextChanged((EventHandler<EventArgs>)_NotesTextbox_TextChanged);
					_broadcastCheckbox[z].add_CheckedChanged((EventHandler<CheckChangedEvent>)_broadcastCheckbox_CheckedChanged);
					if (eventNotes[senderIndex].NotesData.Count > z)
					{
						((TextInputBase)_NotesTextbox[z]).set_Text(WrapText(eventNotes[senderIndex].NotesData[z].Notes, 60));
						if (eventNotes[senderIndex].NotesData[z].Broadcast)
						{
							_broadcastCheckbox[z].set_Checked(true);
						}
						_broadcastCheckbox[z].add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
						{
							((Control)_buttonSaveEvents).set_Visible(true);
						});
					}
					currentControlCount++;
				}
				Label val11 = new Label();
				val11.set_Text("TTS:");
				((Control)val11).set_Size(new Point(70, 40));
				((Control)val11).set_Location(new Point(0, 500));
				val11.set_HorizontalAlignment((HorizontalAlignment)2);
				val11.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val11).set_Parent((Container)(object)_SettingsControlPanel);
				_TTSLabel = val11;
				TextBox val12 = new TextBox();
				((Control)val12).set_Size(new Point(350, 40));
				((Control)val12).set_Location(new Point(110, 500));
				((TextInputBase)val12).set_Font(GameService.Content.get_DefaultFont16());
				((Control)val12).set_Parent((Container)(object)_SettingsControlPanel);
				_TTSTextbox = val12;
				Checkbox val13 = new Checkbox();
				((Control)val13).set_Size(new Point(32, 32));
				((Control)val13).set_Location(new Point(80, 504));
				val13.set_Checked(eventNotes[senderIndex].TTSActive);
				((Control)val13).set_Parent((Container)(object)_SettingsControlPanel);
				_TTSCheckbox = val13;
				if (eventNotes[senderIndex].TTSText != null)
				{
					((TextInputBase)_TTSTextbox).set_Text(eventNotes[senderIndex].TTSText.ToString());
				}
				((TextInputBase)_TTSTextbox).add_TextChanged((EventHandler<EventArgs>)_TTSTextbox_TextChanged);
				_TTSCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)_TTSCheckbox_CheckedChanged);
				StandardButton val14 = new StandardButton();
				val14.set_Text("Test");
				((Control)val14).set_Size(new Point(60, 40));
				((Control)val14).set_Location(new Point(470, 500));
				((Control)val14).set_Visible(true);
				((Control)val14).set_Parent((Container)(object)_SettingsControlPanel);
				_TTSTest = val14;
				_Settings = _BaubleFarmModule._settings;
				SettingCollection obj = _Settings.AddSubCollection("MainSettings", false);
				_TTSSpeedSettingEntry = null;
				int TTSSpeed = 0;
				obj.TryGetSetting<int>("TTSSpeedDefaultTimer", ref _TTSSpeedSettingEntry);
				if (_TTSSpeedSettingEntry != null)
				{
					TTSSpeed = _TTSSpeedSettingEntry.get_Value();
				}
				_TTSVolumeSettingEntry = null;
				int TTSVolume = 100;
				obj.TryGetSetting<int>("TTSVolumeDefaultTimer", ref _TTSVolumeSettingEntry);
				if (_TTSVolumeSettingEntry != null)
				{
					TTSVolume = _TTSVolumeSettingEntry.get_Value();
				}
				((Control)_TTSTest).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
					speechSynthesizer.Rate = TTSSpeed;
					speechSynthesizer.Volume = TTSVolume;
					speechSynthesizer.SpeakAsync(((TextInputBase)_TTSTextbox).get_Text().ToString());
				});
				for (int j = 0; j < TimerRowNum; j++)
				{
					if (j % 2 == 0)
					{
						((Control)_timerEventsPanels[j]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.5f));
					}
					else
					{
						((Control)_timerEventsPanels[j]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.2f));
					}
				}
				((Control)_timerEventsPanels[senderIndex]).set_BackgroundColor(new Color(0f, 0f, 0f, 1f));
				for (int i = 0; i < TimerRowNum; i++)
				{
					((Control)_cancelButton[i]).set_Visible(false);
					((Control)_upArrowButton[i]).set_Visible(false);
					((Control)_downArrowButton[i]).set_Visible(false);
				}
				((Control)_cancelButton[senderIndex]).set_Visible(true);
				if (senderIndex != 0)
				{
					((Control)_upArrowButton[senderIndex]).set_Visible(true);
				}
				if (senderIndex + 1 != TimerRowNum)
				{
					((Control)_downArrowButton[senderIndex]).set_Visible(true);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load event details when clicking event panel: " + ex.Message);
			}
		}

		private void _TTSCheckbox_CheckedChanged(object sender, CheckChangedEvent e)
		{
			_eventNotes[_CurrentEventSelected].TTSActive = _TTSCheckbox.get_Checked();
			((Control)_buttonSaveEvents).set_Visible(true);
		}

		private void _TTSTextbox_TextChanged(object sender, EventArgs e)
		{
			_eventNotes[_CurrentEventSelected].TTSText = ((TextInputBase)_TTSTextbox).get_Text().ToString();
			((Control)_buttonSaveEvents).set_Visible(true);
		}

		private void _WaypointsTextbox_TextChanged(object sender, EventArgs e)
		{
			object[] waypointsTextbox = _WaypointsTextbox;
			int senderIndex = Array.IndexOf(waypointsTextbox, sender);
			_eventNotes[_CurrentEventSelected].WaypointData[senderIndex].Notes = ((TextInputBase)_WaypointsTextbox[senderIndex]).get_Text();
			((Control)_buttonSaveEvents).set_Visible(true);
		}

		private void _NotesTextbox_TextChanged(object sender, EventArgs e)
		{
			object[] notesTextbox = _NotesTextbox;
			int senderIndex = Array.IndexOf(notesTextbox, sender);
			_eventNotes[_CurrentEventSelected].NotesData[senderIndex].Notes = ((TextInputBase)_NotesTextbox[senderIndex]).get_Text().Replace("\n", " ");
			((Control)_buttonSaveEvents).set_Visible(true);
		}

		private void _broadcastCheckbox_CheckedChanged(object sender, CheckChangedEvent e)
		{
			object[] broadcastCheckbox = _broadcastCheckbox;
			int senderIndex = Array.IndexOf(broadcastCheckbox, sender);
			_eventNotes[_CurrentEventSelected].NotesData[senderIndex].Broadcast = _broadcastCheckbox[senderIndex].get_Checked();
			((Control)_buttonSaveEvents).set_Visible(true);
		}

		private string WrapText(string text, int maxWidth)
		{
			string[] array = text.Split(' ');
			List<string> lines = new List<string>();
			StringBuilder currentLine = new StringBuilder();
			string[] array2 = array;
			foreach (string word in array2)
			{
				if (currentLine.Length + word.Length + 1 > maxWidth)
				{
					lines.Add(currentLine.ToString().Trim());
					currentLine.Clear();
				}
				currentLine.Append(word + " ");
			}
			if (currentLine.Length > 0)
			{
				lines.Add(currentLine.ToString().Trim());
			}
			return string.Join("\n", lines);
		}

		private void LoadTimeCustomized(int Index)
		{
			TimeSpan Minutes = TimeSpan.FromMinutes(_timerMinutesDefault.get_Value());
			TimeSpan Seconds = TimeSpan.FromSeconds(_timerSecondsDefault.get_Value());
			_MinutesLabelDisplay.set_Text(_timerMinutesDefault.get_Value() + " Minutes");
			_SecondsLabelDisplay.set_Text(_timerSecondsDefault.get_Value() + " Seconds");
			if (Index < _BaubleFarmModule._timerDurationDefaults.Count())
			{
				_BaubleFarmModule._timerDurationDefaults[Index] = Minutes + Seconds;
				_BaubleFarmModule._timerLabels[Index].set_Text(_BaubleFarmModule._timerDurationDefaults[Index].ToString("mm\\:ss"));
			}
			if (Index < _eventNotes.Count)
			{
				_eventNotes[Index].Minutes = Minutes.TotalMinutes;
				_eventNotes[Index].Seconds = Seconds.TotalSeconds;
			}
			((TextInputBase)_textNewEvent).set_Text("");
			((Control)_buttonSaveEvents).set_Visible(true);
			((Control)_buttonRestartModule).set_Visible(false);
		}

		public TimerSettingsTabView()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			JsonSerializerOptions val = new JsonSerializerOptions();
			val.set_WriteIndented(true);
			_jsonOptions = val;
			((View)this)._002Ector();
		}
	}
}
