using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace roguishpanda.AB_Bauble_Farm
{
	public class StaticEventSettingsTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<MainWindowModule>();

		private MainWindowModule _BaubleFarmModule;

		private AsyncTexture2D _NoTexture;

		private Panel[] _staticEventsPanels;

		private TextBox[] _staticEventTextbox;

		private Panel _staticPackagePanel;

		private int _CurrentEventSelected;

		private Panel _SettingsControlPanel;

		private TextBox _textNewEvent;

		private Label _CreateEventAlert;

		private StandardButton _buttonRestartModule;

		private Label _CurrentEventLabel;

		private Panel _staticEventsTitlePanel;

		private Label _staticEventsTitleLabel;

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

		private Panel _staticSettingsPanel;

		private SettingCollection _MainSettings;

		private List<StaticDetailData> _eventNotes;

		private List<StaticDetailData> _eventNotesReload;

		private List<PackageData> _PackageData;

		private int StaticRowNum;

		private StandardButton _buttonSaveEvents;

		private StandardButton _buttonReloadEvents;

		private string _CurrentPackage;

		private Label[] _WaypointsLabel;

		private TextBox[] _WaypointsTextbox;

		private Label[] _NotesLabel;

		private MultilineTextBox[] _NotesTextbox;

		public readonly JsonSerializerOptions _jsonOptions;

		protected override void Build(Container buildPanel)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Expected O, but got Unknown
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Expected O, but got Unknown
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Expected O, but got Unknown
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Expected O, but got Unknown
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Expected O, but got Unknown
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Expected O, but got Unknown
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Expected O, but got Unknown
			_BaubleFarmModule = MainWindowModule.ModuleInstance;
			_MainSettings = _BaubleFarmModule._settings;
			_eventNotes = new List<StaticDetailData>(_BaubleFarmModule._staticEvents);
			_eventNotesReload = new List<StaticDetailData>(_BaubleFarmModule._staticEvents);
			_PackageData = new List<PackageData>(_BaubleFarmModule._PackageData);
			StaticRowNum = _BaubleFarmModule.StaticRowNum;
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
			_staticSettingsPanel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_staticSettingsPanel);
			((Control)val2).set_Size(new Point(300, 400));
			((Control)val2).set_Location(new Point(100, 100));
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			_staticPackagePanel = val2;
			Label val3 = new Label();
			val3.set_Text("Add Event:");
			((Control)val3).set_Size(new Point(200, 30));
			((Control)val3).set_Location(new Point(100, 510));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Parent((Container)(object)_staticSettingsPanel);
			TextBox val4 = new TextBox();
			((Control)val4).set_Size(new Point(300, 40));
			((Control)val4).set_Location(new Point(100, 540));
			((Control)val4).set_Parent((Container)(object)_staticSettingsPanel);
			_textNewEvent = val4;
			Image val5 = new Image();
			val5.set_Texture(_addTexture);
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Location(new Point(405, 540));
			((Control)val5).set_Parent((Container)(object)_staticSettingsPanel);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)CreateEvent_Click);
			StandardButton val6 = new StandardButton();
			val6.set_Text("Save");
			((Control)val6).set_Size(new Point(140, 40));
			((Control)val6).set_Location(new Point(530, 550));
			((Control)val6).set_Visible(false);
			((Control)val6).set_Parent((Container)(object)_staticSettingsPanel);
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
			((Control)val7).set_Parent((Container)(object)_staticSettingsPanel);
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
			((Control)val8).set_Parent((Container)(object)_staticSettingsPanel);
			_CreateEventAlert = val8;
			StandardButton val9 = new StandardButton();
			val9.set_Text("Restart Module");
			((Control)val9).set_Size(new Point(200, 40));
			((Control)val9).set_Location(new Point(530, 550));
			((Control)val9).set_Visible(false);
			((Control)val9).set_Parent((Container)(object)_staticSettingsPanel);
			_buttonRestartModule = val9;
			((Control)_buttonRestartModule).add_Click((EventHandler<MouseEventArgs>)RestartModule_Click);
			Label val10 = new Label();
			((Control)val10).set_Size(new Point(300, 40));
			((Control)val10).set_Location(new Point(420, 60));
			val10.set_Font(GameService.Content.get_DefaultFont32());
			val10.set_TextColor(Color.get_LimeGreen());
			((Control)val10).set_Parent((Container)(object)_staticSettingsPanel);
			_CurrentEventLabel = val10;
			AsyncTexture2D TitleTexture = AsyncTexture2D.FromAssetId(1234872);
			Panel val11 = new Panel();
			((Control)val11).set_Parent((Container)(object)_staticSettingsPanel);
			((Control)val11).set_Size(new Point(290, 40));
			((Control)val11).set_Location(new Point(102, 60));
			val11.set_BackgroundTexture(TitleTexture);
			_staticEventsTitlePanel = val11;
			Label val12 = new Label();
			val12.set_Text("Static Events");
			((Control)val12).set_Size(new Point(300, 40));
			((Control)val12).set_Location(new Point(10, 0));
			val12.set_Font(GameService.Content.get_DefaultFont16());
			val12.set_TextColor(Color.get_White());
			((Control)val12).set_Parent((Container)(object)_staticEventsTitlePanel);
			_staticEventsTitleLabel = val12;
			_staticEventsPanels = (Panel[])(object)new Panel[StaticRowNum];
			_staticEventTextbox = (TextBox[])(object)new TextBox[StaticRowNum];
			_cancelButton = (Image[])(object)new Image[StaticRowNum];
			_upArrowButton = (Image[])(object)new Image[StaticRowNum];
			_downArrowButton = (Image[])(object)new Image[StaticRowNum];
			LoadEventTable(StaticRowNum);
			if (StaticRowNum != 0)
			{
				StaticSettings_Click(_staticEventsPanels[0], null);
			}
		}

		private void CurrentEvent_TextChanged(int Index)
		{
			try
			{
				string NewDescription = ((TextInputBase)_staticEventTextbox[Index]).get_Text();
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
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				List<StaticDetailData> originalNotesData = _eventNotes;
				int NewID = 1;
				if (_eventNotes.Count > 0)
				{
					NewID = originalNotesData.Max((StaticDetailData note) => note.ID) + 1;
				}
				if (((TextInputBase)_textNewEvent).get_Text().Length < 4)
				{
					_CreateEventAlert.set_Text("* 4 characters mininimum required to create new event");
					((Control)_CreateEventAlert).set_Visible(true);
					_CreateEventAlert.set_TextColor(Color.get_Red());
					return;
				}
				_CreateEventAlert.set_Text("Event has been added! Click save to confirm changes!");
				((Control)_CreateEventAlert).set_Visible(true);
				_CreateEventAlert.set_TextColor(Color.get_LimeGreen());
				StaticDetailData notesData = new StaticDetailData
				{
					ID = NewID,
					Description = ((TextInputBase)_textNewEvent).get_Text(),
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
				for (int i = 0; i < StaticRowNum; i++)
				{
					((Control)_staticEventsPanels[i]).Dispose();
					((Control)_staticEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				StaticRowNum = _eventNotes.Count();
				_staticEventsPanels = (Panel[])(object)new Panel[StaticRowNum];
				_staticEventTextbox = (TextBox[])(object)new TextBox[StaticRowNum];
				_cancelButton = (Image[])(object)new Image[StaticRowNum];
				_upArrowButton = (Image[])(object)new Image[StaticRowNum];
				_downArrowButton = (Image[])(object)new Image[StaticRowNum];
				LoadEventTable(StaticRowNum);
				StaticSettings_Click(_staticEventsPanels[StaticRowNum - 1], null);
				((TextInputBase)_textNewEvent).set_Text("");
				((Control)_buttonSaveEvents).set_Visible(true);
				((Control)_buttonReloadEvents).set_Visible(true);
				((Control)_buttonRestartModule).set_Visible(false);
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
				package.StaticDetailData = _eventNotes;
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
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string Description = _eventNotes[Index].Description;
				int ID = _eventNotes[Index].ID;
				_eventNotes.RemoveAll((StaticDetailData note) => note.Description == Description && note.ID == ID);
				_eventNotes = _eventNotes.Select((StaticDetailData note, int index) => new StaticDetailData
				{
					ID = index + 1,
					Description = note.Description,
					WaypointData = note.WaypointData,
					NotesData = note.NotesData
				}).ToList();
				if (_eventNotes.Count <= 0)
				{
					((Control)_staticEventsPanels[0]).Dispose();
					((Control)_staticEventTextbox[0]).Dispose();
					((Control)_cancelButton[0]).Dispose();
					((Control)_upArrowButton[0]).Dispose();
					((Control)_downArrowButton[0]).Dispose();
					((Control)_SettingsControlPanel).Dispose();
					((Control)_CurrentEventLabel).set_Visible(false);
					((Control)_buttonSaveEvents).set_Visible(true);
					((Control)_buttonReloadEvents).set_Visible(true);
					((Control)_buttonRestartModule).set_Visible(false);
					return;
				}
				_eventNotes.Max((StaticDetailData note) => note.ID);
				for (int i = 0; i < StaticRowNum; i++)
				{
					((Control)_staticEventsPanels[i]).Dispose();
					((Control)_staticEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				StaticRowNum = _eventNotes.Count();
				_staticEventsPanels = (Panel[])(object)new Panel[StaticRowNum];
				_staticEventTextbox = (TextBox[])(object)new TextBox[StaticRowNum];
				_cancelButton = (Image[])(object)new Image[StaticRowNum];
				_upArrowButton = (Image[])(object)new Image[StaticRowNum];
				_downArrowButton = (Image[])(object)new Image[StaticRowNum];
				LoadEventTable(StaticRowNum);
				StaticSettings_Click(_staticEventsPanels[0], null);
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
				_ = _eventNotes[Index].ID;
				StaticDetailData temp = _eventNotes[Index];
				_eventNotes[Index] = _eventNotes[Index + Direction];
				_eventNotes[Index + Direction] = temp;
				for (int i = 0; i < StaticRowNum; i++)
				{
					((Control)_staticEventsPanels[i]).Dispose();
					((Control)_staticEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				StaticRowNum = _eventNotes.Count();
				_staticEventsPanels = (Panel[])(object)new Panel[StaticRowNum];
				_staticEventTextbox = (TextBox[])(object)new TextBox[StaticRowNum];
				_cancelButton = (Image[])(object)new Image[StaticRowNum];
				_upArrowButton = (Image[])(object)new Image[StaticRowNum];
				_downArrowButton = (Image[])(object)new Image[StaticRowNum];
				LoadEventTable(StaticRowNum);
				StaticSettings_Click(_staticEventsPanels[Index + Direction], null);
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
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_eventNotes = new List<StaticDetailData>(_eventNotesReload);
				for (int i = 0; i < StaticRowNum; i++)
				{
					((Control)_staticEventsPanels[i]).Dispose();
					((Control)_staticEventTextbox[i]).Dispose();
					((Control)_cancelButton[i]).Dispose();
					((Control)_upArrowButton[i]).Dispose();
					((Control)_downArrowButton[i]).Dispose();
				}
				StaticRowNum = _eventNotes.Count();
				_staticEventsPanels = (Panel[])(object)new Panel[StaticRowNum];
				_staticEventTextbox = (TextBox[])(object)new TextBox[StaticRowNum];
				_cancelButton = (Image[])(object)new Image[StaticRowNum];
				_upArrowButton = (Image[])(object)new Image[StaticRowNum];
				_downArrowButton = (Image[])(object)new Image[StaticRowNum];
				LoadEventTable(StaticRowNum);
				StaticSettings_Click(_staticEventsPanels[0], null);
				((Control)_CreateEventAlert).set_Visible(true);
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
				List<StaticDetailData> eventNotes = _eventNotes;
				for (int i = 0; i < TotalEvents; i++)
				{
					int Index = i;
					Panel[] staticEventsPanels = _staticEventsPanels;
					int num = i;
					Panel val = new Panel();
					((Control)val).set_Parent((Container)(object)_staticPackagePanel);
					((Control)val).set_Size(new Point(300, 40));
					((Control)val).set_Location(new Point(0, i * 40));
					staticEventsPanels[num] = val;
					((Control)_staticEventsPanels[i]).add_Click((EventHandler<MouseEventArgs>)StaticSettings_Click);
					if (i % 2 == 0)
					{
						((Control)_staticEventsPanels[i]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.5f));
					}
					else
					{
						((Control)_staticEventsPanels[i]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.2f));
					}
					TextBox[] staticEventTextbox = _staticEventTextbox;
					int num2 = i;
					TextBox val2 = new TextBox();
					((TextInputBase)val2).set_Text(eventNotes[i].Description);
					((Control)val2).set_Size(new Point(200, 30));
					((Control)val2).set_Location(new Point(30, 5));
					val2.set_HorizontalAlignment((HorizontalAlignment)0);
					((TextInputBase)val2).set_Font(GameService.Content.get_DefaultFont16());
					val2.set_HideBackground(true);
					((TextInputBase)val2).set_ForeColor(Color.get_LimeGreen());
					((Control)val2).set_Parent((Container)(object)_staticEventsPanels[i]);
					staticEventTextbox[num2] = val2;
					((TextInputBase)_staticEventTextbox[i]).add_TextChanged((EventHandler<EventArgs>)delegate
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
					((Control)val3).set_Parent((Container)(object)_staticEventsPanels[i]);
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
					((Control)val4).set_Parent((Container)(object)_staticEventsPanels[i]);
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
					((Control)val5).set_Parent((Container)(object)_staticEventsPanels[i]);
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

		private void StaticSettings_Click(object sender, MouseEventArgs e)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Expected O, but got Unknown
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Expected O, but got Unknown
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Expected O, but got Unknown
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Expected O, but got Unknown
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Expected O, but got Unknown
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				object[] staticEventsPanels = _staticEventsPanels;
				int senderIndex = Array.IndexOf(staticEventsPanels, sender);
				List<StaticDetailData> eventNotes = _eventNotes;
				if (_SettingsControlPanel != null)
				{
					((Control)_SettingsControlPanel).Dispose();
				}
				_CurrentEventLabel.set_Text(_eventNotes[senderIndex].Description);
				_CurrentEventSelected = senderIndex;
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_staticSettingsPanel);
				((Control)val).set_Location(new Point(410, 110));
				((Control)val).set_Size(new Point(600, 400));
				val.set_CanScroll(true);
				_SettingsControlPanel = val;
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
					Label val2 = new Label();
					val2.set_Text("Waypoint:");
					((Control)val2).set_Size(new Point(100, 40));
					((Control)val2).set_Location(new Point(0, 0));
					val2.set_HorizontalAlignment((HorizontalAlignment)2);
					val2.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val2).set_Parent((Container)(object)_SettingsControlPanel);
					waypointsLabel[num] = val2;
					TextBox[] waypointsTextbox = _WaypointsTextbox;
					int num2 = y;
					TextBox val3 = new TextBox();
					((Control)val3).set_Size(new Point(350, 40));
					((Control)val3).set_Location(new Point(110, 0));
					((TextInputBase)val3).set_Font(GameService.Content.get_DefaultFont16());
					((Control)val3).set_Parent((Container)(object)_SettingsControlPanel);
					waypointsTextbox[num2] = val3;
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
					Label val4 = new Label();
					val4.set_Text("Note #" + (z + 1) + ":");
					((Control)val4).set_Size(new Point(100, 40));
					((Control)val4).set_Location(new Point(0, 50 + currentControlCount * 90));
					val4.set_HorizontalAlignment((HorizontalAlignment)2);
					val4.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val4).set_Parent((Container)(object)_SettingsControlPanel);
					notesLabel[num3] = val4;
					MultilineTextBox[] notesTextbox = _NotesTextbox;
					int num4 = z;
					MultilineTextBox val5 = new MultilineTextBox();
					((Control)val5).set_Size(new Point(450, 80));
					((Control)val5).set_Location(new Point(110, 50 + currentControlCount * 90));
					((TextInputBase)val5).set_Font(GameService.Content.get_DefaultFont16());
					((Control)val5).set_Parent((Container)(object)_SettingsControlPanel);
					notesTextbox[num4] = val5;
					Image[] broadcastImage = _broadcastImage;
					int num5 = z;
					Image val6 = new Image();
					val6.set_Texture(_broadcastTexture);
					((Control)val6).set_Size(new Point(32, 32));
					((Control)val6).set_Location(new Point(50, 80 + currentControlCount * 90));
					((Control)val6).set_Parent((Container)(object)_SettingsControlPanel);
					broadcastImage[num5] = val6;
					Checkbox[] broadcastCheckbox = _broadcastCheckbox;
					int num6 = z;
					Checkbox val7 = new Checkbox();
					((Control)val7).set_Size(new Point(32, 32));
					((Control)val7).set_Location(new Point(80, 80 + currentControlCount * 90));
					((Control)val7).set_Parent((Container)(object)_SettingsControlPanel);
					broadcastCheckbox[num6] = val7;
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
				for (int j = 0; j < StaticRowNum; j++)
				{
					if (j % 2 == 0)
					{
						((Control)_staticEventsPanels[j]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.5f));
					}
					else
					{
						((Control)_staticEventsPanels[j]).set_BackgroundColor(new Color(0f, 0f, 0f, 0.2f));
					}
				}
				((Control)_staticEventsPanels[senderIndex]).set_BackgroundColor(new Color(0f, 0f, 0f, 1f));
				for (int i = 0; i < StaticRowNum; i++)
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
				if (senderIndex + 1 != StaticRowNum)
				{
					((Control)_downArrowButton[senderIndex]).set_Visible(true);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load event details when clicking event panel: " + ex.Message);
			}
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

		public StaticEventSettingsTabView()
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
