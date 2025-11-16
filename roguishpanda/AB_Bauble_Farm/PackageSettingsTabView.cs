using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace roguishpanda.AB_Bauble_Farm
{
	public class PackageSettingsTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<MainWindowModule>();

		private MainWindowModule _BaubleFarmModule;

		private List<PackageData> _PackageData;

		private List<PackageData> _CommunityPackageData;

		private Panel _timerEventsTitlePanel;

		private Label _timerEventsTitleLabel;

		private Label _PackageLabelDisplay;

		private Label _PackageCreateLabel;

		private TextBox _PackageCreateTextBox;

		private StandardButton _ButtonCreate;

		private TextBox _PackageRenameTextBox;

		private Label _DefaultPackageLabel;

		private SettingEntry<string> _PackageSettingEntry;

		private SettingCollection _Settings;

		private StandardButton _ButtonSaveRename;

		private StandardButton _ButtonLoadRename;

		private Label _PackageLoadPersonalLabel;

		private Dropdown _PackageLoadPersonalDropdown;

		private StandardButton _ButtonLoadPersonal;

		private Label _PackageLoadCommunityLabel;

		private Dropdown _PackageLoadCommunityDropdown;

		private Label _PackageLoadPackageAlert;

		private StandardButton _ButtonDeletePersonal;

		private StandardButton _ButtonLoadCommunity;

		private StandardButton _ButtonCopyClipboard;

		private StandardButton _ButtonImportClipboard;

		public readonly JsonSerializerOptions _jsonOptions;

		protected override void Build(Container buildPanel)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Expected O, but got Unknown
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Expected O, but got Unknown
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Expected O, but got Unknown
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Expected O, but got Unknown
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Expected O, but got Unknown
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Expected O, but got Unknown
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Expected O, but got Unknown
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Expected O, but got Unknown
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Expected O, but got Unknown
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Expected O, but got Unknown
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Expected O, but got Unknown
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_0665: Expected O, but got Unknown
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0673: Unknown result type (might be due to invalid IL or missing references)
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a5: Expected O, but got Unknown
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Expected O, but got Unknown
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_070a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0715: Unknown result type (might be due to invalid IL or missing references)
			//IL_071a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Unknown result type (might be due to invalid IL or missing references)
			//IL_074c: Expected O, but got Unknown
			_BaubleFarmModule = MainWindowModule.ModuleInstance;
			_PackageData = new List<PackageData>(_BaubleFarmModule._PackageData);
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			int num = ((Rectangle)(ref contentRegion)).get_Size().X + 200;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y + 300));
			contentRegion = buildPanel.get_ContentRegion();
			int x = ((Rectangle)(ref contentRegion)).get_Location().X;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Location(new Point(x, ((Rectangle)(ref contentRegion)).get_Location().Y - 35));
			val.set_BackgroundTexture(MainWindowModule.ModuleInstance._asyncTimertexture);
			Panel listSettingsPanel = val;
			AsyncTexture2D TitleTexture = AsyncTexture2D.FromAssetId(1234872);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)listSettingsPanel);
			((Control)val2).set_Size(new Point(700, 40));
			((Control)val2).set_Location(new Point(102, 60));
			val2.set_BackgroundTexture(TitleTexture);
			_timerEventsTitlePanel = val2;
			Label val3 = new Label();
			val3.set_Text("Package Settings");
			((Control)val3).set_Size(new Point(300, 40));
			((Control)val3).set_Location(new Point(10, 0));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Parent((Container)(object)_timerEventsTitlePanel);
			_timerEventsTitleLabel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Copy to Clipboard");
			((Control)val4).set_Size(new Point(140, 30));
			((Control)val4).set_Location(new Point(110, 110));
			((Control)val4).set_Parent((Container)(object)listSettingsPanel);
			_ButtonCopyClipboard = val4;
			((Control)_ButtonCopyClipboard).add_Click((EventHandler<MouseEventArgs>)_ButtonCopyClipboard_Click);
			StandardButton val5 = new StandardButton();
			val5.set_Text("Import from Clipboard");
			((Control)val5).set_Size(new Point(160, 30));
			((Control)val5).set_Location(new Point(270, 110));
			((Control)val5).set_Parent((Container)(object)listSettingsPanel);
			_ButtonImportClipboard = val5;
			((Control)_ButtonImportClipboard).add_Click((EventHandler<MouseEventArgs>)_ButtonImportClipboard_Click);
			Label val6 = new Label();
			((Control)val6).set_Size(new Point(500, 40));
			((Control)val6).set_Location(new Point(440, 105));
			val6.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val6).set_Visible(false);
			((Control)val6).set_Parent((Container)(object)listSettingsPanel);
			_PackageLoadPackageAlert = val6;
			Label val7 = new Label();
			val7.set_Text("Current Package:");
			((Control)val7).set_Size(new Point(160, 40));
			((Control)val7).set_Location(new Point(110, 150));
			val7.set_Font(GameService.Content.get_DefaultFont16());
			val7.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val7).set_Visible(true);
			((Control)val7).set_Parent((Container)(object)listSettingsPanel);
			_PackageLabelDisplay = val7;
			Label val8 = new Label();
			((Control)val8).set_Size(new Point(300, 40));
			((Control)val8).set_Location(new Point(290, 150));
			val8.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val8).set_Visible(true);
			((Control)val8).set_Parent((Container)(object)listSettingsPanel);
			_DefaultPackageLabel = val8;
			TextBox val9 = new TextBox();
			((Control)val9).set_Size(new Point(300, 30));
			((Control)val9).set_Location(new Point(280, 150));
			((TextInputBase)val9).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val9).set_Visible(false);
			((Control)val9).set_Parent((Container)(object)listSettingsPanel);
			_PackageRenameTextBox = val9;
			StandardButton val10 = new StandardButton();
			val10.set_Text("Rename");
			((Control)val10).set_Size(new Point(80, 30));
			((Control)val10).set_Location(new Point(610, 150));
			((Control)val10).set_Visible(true);
			((Control)val10).set_Parent((Container)(object)listSettingsPanel);
			_ButtonLoadRename = val10;
			((Control)_ButtonLoadRename).add_Click((EventHandler<MouseEventArgs>)_ButtonLoadRename_Click);
			StandardButton val11 = new StandardButton();
			val11.set_Text("Save");
			((Control)val11).set_Size(new Point(80, 30));
			((Control)val11).set_Location(new Point(610, 150));
			((Control)val11).set_Visible(false);
			((Control)val11).set_Parent((Container)(object)listSettingsPanel);
			_ButtonSaveRename = val11;
			((Control)_ButtonSaveRename).add_Click((EventHandler<MouseEventArgs>)_ButtonSaveRename_Click);
			Label val12 = new Label();
			val12.set_Text("Create Package:");
			((Control)val12).set_Size(new Point(160, 40));
			((Control)val12).set_Location(new Point(110, 200));
			val12.set_Font(GameService.Content.get_DefaultFont16());
			val12.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val12).set_Visible(true);
			((Control)val12).set_Parent((Container)(object)listSettingsPanel);
			_PackageCreateLabel = val12;
			TextBox val13 = new TextBox();
			((Control)val13).set_Size(new Point(300, 30));
			((Control)val13).set_Location(new Point(280, 200));
			((TextInputBase)val13).set_Font(GameService.Content.get_DefaultFont16());
			((Control)val13).set_Visible(true);
			((Control)val13).set_Parent((Container)(object)listSettingsPanel);
			_PackageCreateTextBox = val13;
			StandardButton val14 = new StandardButton();
			val14.set_Text("Create");
			((Control)val14).set_Size(new Point(80, 30));
			((Control)val14).set_Location(new Point(610, 200));
			((Control)val14).set_Visible(true);
			((Control)val14).set_Parent((Container)(object)listSettingsPanel);
			_ButtonCreate = val14;
			((Control)_ButtonCreate).add_Click((EventHandler<MouseEventArgs>)_ButtonCreate_Click);
			Label val15 = new Label();
			val15.set_Text("Community Packages:");
			((Control)val15).set_Size(new Point(160, 40));
			((Control)val15).set_Location(new Point(110, 250));
			val15.set_Font(GameService.Content.get_DefaultFont16());
			val15.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val15).set_Parent((Container)(object)listSettingsPanel);
			_PackageLoadCommunityLabel = val15;
			Dropdown val16 = new Dropdown();
			((Control)val16).set_Size(new Point(310, 40));
			((Control)val16).set_Location(new Point(280, 250));
			((Control)val16).set_Parent((Container)(object)listSettingsPanel);
			_PackageLoadCommunityDropdown = val16;
			StandardButton val17 = new StandardButton();
			val17.set_Text("Import");
			((Control)val17).set_Size(new Point(80, 30));
			((Control)val17).set_Location(new Point(610, 250));
			((Control)val17).set_Parent((Container)(object)listSettingsPanel);
			_ButtonLoadCommunity = val17;
			((Control)_ButtonLoadCommunity).add_Click((EventHandler<MouseEventArgs>)_ButtonLoadCommunity_Click);
			LoadCommunityPackageDropdownOptions();
			Label val18 = new Label();
			val18.set_Text("Personal Packages:");
			((Control)val18).set_Size(new Point(160, 40));
			((Control)val18).set_Location(new Point(110, 300));
			val18.set_Font(GameService.Content.get_DefaultFont16());
			val18.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val18).set_Visible(false);
			((Control)val18).set_Parent((Container)(object)listSettingsPanel);
			_PackageLoadPersonalLabel = val18;
			Dropdown val19 = new Dropdown();
			((Control)val19).set_Size(new Point(310, 40));
			((Control)val19).set_Location(new Point(280, 300));
			((Control)val19).set_Visible(false);
			((Control)val19).set_Parent((Container)(object)listSettingsPanel);
			_PackageLoadPersonalDropdown = val19;
			StandardButton val20 = new StandardButton();
			val20.set_Text("Load");
			((Control)val20).set_Size(new Point(80, 30));
			((Control)val20).set_Location(new Point(610, 300));
			((Control)val20).set_Visible(false);
			((Control)val20).set_Parent((Container)(object)listSettingsPanel);
			_ButtonLoadPersonal = val20;
			((Control)_ButtonLoadPersonal).add_Click((EventHandler<MouseEventArgs>)_ButtonLoadPersonal_Click);
			StandardButton val21 = new StandardButton();
			val21.set_Text("Delete");
			((Control)val21).set_Size(new Point(80, 30));
			((Control)val21).set_Location(new Point(700, 300));
			((Control)val21).set_Visible(false);
			((Control)val21).set_Parent((Container)(object)listSettingsPanel);
			_ButtonDeletePersonal = val21;
			((Control)_ButtonDeletePersonal).add_Click((EventHandler<MouseEventArgs>)_ButtonDeletePersonal_Click);
			LoadPersonalPackageDropdownOptions();
			_ = _BaubleFarmModule._PackageSettingsCollection;
			_Settings = _BaubleFarmModule._settings;
			SettingCollection PackageSettings = _Settings.AddSubCollection("PackageSettings", false);
			if (PackageSettings != null)
			{
				_PackageSettingEntry = null;
				PackageSettings.TryGetSetting<string>("CurrentPackageSelection", ref _PackageSettingEntry);
				if (_PackageSettingEntry != null)
				{
					_DefaultPackageLabel.set_Text(_PackageSettingEntry.get_Value().ToString());
				}
			}
			((Control)_ButtonSaveRename).set_Visible(false);
		}

		public static void CopyToClipboard(string text)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				Clipboard.SetText(text);
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		private void _ButtonCopyClipboard_Click(object sender, MouseEventArgs e)
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			string PackageName = _DefaultPackageLabel.get_Text().ToString();
			int index = _PackageData.FindIndex((PackageData p) => p.PackageName == PackageName);
			if (index >= 0)
			{
				CopyToClipboard(JsonSerializer.Serialize<PackageData>(_PackageData[index], _jsonOptions).ToString());
				_PackageLoadPackageAlert.set_Text("The package has been copied to your clipboard!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
			}
			else
			{
				_PackageLoadPackageAlert.set_Text("* The package could not be copied to your clipboard!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
			}
		}

		public static string GetClipboardText()
		{
			if (Clipboard.ContainsText())
			{
				return Clipboard.GetText();
			}
			return string.Empty;
		}

		private void _ButtonImportClipboard_Click(object sender, MouseEventArgs e)
		{
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			string ClipboardText = GetClipboardText();
			try
			{
				PackageData PackageData = JsonSerializer.Deserialize<PackageData>(ClipboardText, _jsonOptions);
				if (PackageData != null)
				{
					foreach (PackageData packageDatum in _PackageData)
					{
						if (packageDatum.PackageName == PackageData.PackageName)
						{
							_PackageLoadPackageAlert.set_Text("* This package name already exists within personal packages!");
							_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
							return;
						}
					}
					_PackageData.Add(PackageData);
					SavePackageJsonUpdate();
					LoadPersonalPackageDropdownOptions();
					_PackageLoadPackageAlert.set_Text("The package was imported from your clipboard to personal packages!");
					_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
				}
				else
				{
					_PackageLoadPackageAlert.set_Text("* The package could not be imported from your clipboard!");
					_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				}
			}
			catch (JsonException val)
			{
				JsonException ex2 = val;
				_PackageLoadPackageAlert.set_Text("* The package could not be imported from your clipboard!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				Logger.Warn("Deserialization failed: " + ((Exception)(object)ex2).Message);
			}
			catch (Exception ex)
			{
				_PackageLoadPackageAlert.set_Text("* The package could not be imported from your clipboard!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				Logger.Warn("Unexpected error: " + ex.Message);
			}
		}

		private void _ButtonDeletePersonal_Click(object sender, MouseEventArgs e)
		{
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_PackageLoadPackageAlert).set_Visible(false);
			((Control)_ButtonSaveRename).set_Visible(false);
			((Control)_ButtonLoadRename).set_Visible(true);
			_PackageData = _PackageData.Where((PackageData pd) => pd.PackageName != _PackageLoadPersonalDropdown.get_SelectedItem()).ToList();
			SavePackageJsonUpdate();
			LoadPersonalPackageDropdownOptions();
		}

		private void _ButtonLoadPersonal_Click(object sender, MouseEventArgs e)
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_ButtonSaveRename).set_Visible(false);
			((Control)_ButtonLoadRename).set_Visible(true);
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			_PackageSettingEntry.set_Value(_PackageLoadPersonalDropdown.get_SelectedItem().ToString());
			_DefaultPackageLabel.set_Text(_PackageLoadPersonalDropdown.get_SelectedItem().ToString());
			LoadPersonalPackageDropdownOptions();
			_BaubleFarmModule.Restart();
			_PackageLoadPackageAlert.set_Text("The personal package has been loaded!");
			_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
		}

		private void _ButtonLoadCommunity_Click(object sender, MouseEventArgs e)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			((Control)_ButtonSaveRename).set_Visible(false);
			((Control)_ButtonLoadRename).set_Visible(true);
			string CommunityPackageName = _PackageLoadCommunityDropdown.get_SelectedItem().ToString();
			foreach (PackageData packageDatum in _PackageData)
			{
				if (packageDatum.PackageName == CommunityPackageName)
				{
					_PackageLoadPackageAlert.set_Text("* This package name already exists within personal packages!");
					_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
					return;
				}
			}
			int index = _CommunityPackageData.FindIndex((PackageData p) => p.PackageName == CommunityPackageName);
			if (index >= 0)
			{
				PackageData CommunityPackageData = _CommunityPackageData[index];
				_PackageData.Add(CommunityPackageData);
				SavePackageJsonUpdate();
				_PackageSettingEntry.set_Value(_PackageLoadCommunityDropdown.get_SelectedItem().ToString());
				_DefaultPackageLabel.set_Text(_PackageLoadCommunityDropdown.get_SelectedItem().ToString());
				LoadPersonalPackageDropdownOptions();
				_BaubleFarmModule.Restart();
				_PackageLoadPackageAlert.set_Text("This community package has been loaded into personal packages!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
			}
			else
			{
				_PackageLoadPackageAlert.set_Text("* This community package could not be loaded into personal packages!");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
			}
		}

		private void _ButtonCreate_Click(object sender, MouseEventArgs e)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			((Control)_ButtonSaveRename).set_Visible(false);
			((Control)_ButtonLoadRename).set_Visible(true);
			if (((TextInputBase)_PackageCreateTextBox).get_Text().Length < 4)
			{
				_PackageLoadPackageAlert.set_Text("* 4 characters mininimum required to create new package");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				return;
			}
			if (((TextInputBase)_PackageCreateTextBox).get_Text().Length > 28)
			{
				_PackageLoadPackageAlert.set_Text("* 28 characters maximum allowed for new package names");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				return;
			}
			foreach (PackageData packageDatum in _PackageData)
			{
				if (packageDatum.PackageName == ((TextInputBase)_PackageCreateTextBox).get_Text())
				{
					_PackageLoadPackageAlert.set_Text("* This package name already exists");
					_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
					return;
				}
			}
			((TextInputBase)_PackageRenameTextBox).set_Text("");
			_PackageLoadPackageAlert.set_Text("Package has been created!");
			_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
			PackageData newPackage = new PackageData
			{
				PackageName = ((TextInputBase)_PackageCreateTextBox).get_Text(),
				StaticDetailData = new List<StaticDetailData>(),
				TimerDetailData = new List<TimerDetailData>()
			};
			_PackageData.Add(newPackage);
			SavePackageJsonUpdate();
			_PackageSettingEntry.set_Value(((TextInputBase)_PackageCreateTextBox).get_Text().ToString());
			_DefaultPackageLabel.set_Text(((TextInputBase)_PackageCreateTextBox).get_Text().ToString());
			LoadPersonalPackageDropdownOptions();
			_BaubleFarmModule.Restart();
		}

		private void _ButtonLoadRename_Click(object sender, MouseEventArgs e)
		{
			((Control)_DefaultPackageLabel).set_Visible(false);
			((Control)_PackageRenameTextBox).set_Visible(true);
			((Control)_ButtonLoadRename).set_Visible(false);
			((Control)_ButtonSaveRename).set_Visible(true);
			((Control)_PackageLoadPackageAlert).set_Visible(false);
			((TextInputBase)_PackageRenameTextBox).set_Text(_DefaultPackageLabel.get_Text());
		}

		private async Task LoadCommunityPackageDropdownOptions()
		{
			try
			{
				_PackageLoadCommunityDropdown.get_Items().Clear();
				_CommunityPackageData = new List<PackageData>();
				string jsonFilePath = "Defaults\\Community_Packages.json";
				Stream json = _BaubleFarmModule.ContentsManager.GetFileStream(jsonFilePath);
				using (StreamReader reader = new StreamReader(json))
				{
					_CommunityPackageData = JsonSerializer.Deserialize<List<PackageData>>(await reader.ReadToEndAsync(), _jsonOptions);
				}
				for (int i = 0; i < _CommunityPackageData.Count; i++)
				{
					string PackageName = _CommunityPackageData[i].PackageName;
					_PackageLoadCommunityDropdown.get_Items().Add(PackageName);
				}
				if (_PackageLoadCommunityDropdown.get_Items().Count > 0)
				{
					_PackageLoadCommunityDropdown.set_SelectedItem(_PackageLoadCommunityDropdown.get_Items()[0]);
					((Control)_PackageLoadCommunityLabel).set_Visible(true);
					((Control)_PackageLoadCommunityDropdown).set_Visible(true);
					((Control)_ButtonLoadCommunity).set_Visible(true);
				}
				else
				{
					((Control)_PackageLoadPersonalLabel).set_Visible(false);
					((Control)_PackageLoadPersonalDropdown).set_Visible(false);
					((Control)_ButtonLoadPersonal).set_Visible(false);
					((Control)_ButtonDeletePersonal).set_Visible(false);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load packagedropdowns: " + ex.Message);
			}
		}

		private async Task LoadPersonalPackageDropdownOptions()
		{
			try
			{
				_PackageLoadPersonalDropdown.get_Items().Clear();
				_PackageData = new List<PackageData>();
				string jsonFilePath = Path.Combine(_BaubleFarmModule.DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Package_Defaults.json");
				using (StreamReader reader = new StreamReader(jsonFilePath))
				{
					_PackageData = JsonSerializer.Deserialize<List<PackageData>>(await reader.ReadToEndAsync(), _jsonOptions);
				}
				for (int i = 0; i < _PackageData.Count; i++)
				{
					string PackageName = _PackageData[i].PackageName;
					if (PackageName != _DefaultPackageLabel.get_Text())
					{
						_PackageLoadPersonalDropdown.get_Items().Add(PackageName);
					}
				}
				if (_PackageLoadPersonalDropdown.get_Items().Count > 0)
				{
					_PackageLoadPersonalDropdown.set_SelectedItem(_PackageLoadPersonalDropdown.get_Items()[0]);
					((Control)_PackageLoadPersonalLabel).set_Visible(true);
					((Control)_PackageLoadPersonalDropdown).set_Visible(true);
					((Control)_ButtonLoadPersonal).set_Visible(true);
					((Control)_ButtonDeletePersonal).set_Visible(true);
				}
				else
				{
					((Control)_PackageLoadPersonalLabel).set_Visible(false);
					((Control)_PackageLoadPersonalDropdown).set_Visible(false);
					((Control)_ButtonLoadPersonal).set_Visible(false);
					((Control)_ButtonDeletePersonal).set_Visible(false);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load packagedropdowns: " + ex.Message);
			}
		}

		private void _ButtonSaveRename_Click(object sender, MouseEventArgs e)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			if (((TextInputBase)_PackageRenameTextBox).get_Text().Length < 4)
			{
				_PackageLoadPackageAlert.set_Text("* 4 characters mininimum required to create new package");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				return;
			}
			if (((TextInputBase)_PackageRenameTextBox).get_Text().Length > 28)
			{
				_PackageLoadPackageAlert.set_Text("* 28 characters maximum allowed for new package names");
				_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
				return;
			}
			foreach (PackageData packageDatum in _PackageData)
			{
				if (packageDatum.PackageName == ((TextInputBase)_PackageRenameTextBox).get_Text() && _DefaultPackageLabel.get_Text() != ((TextInputBase)_PackageRenameTextBox).get_Text())
				{
					_PackageLoadPackageAlert.set_Text("* This package name already exists");
					((Control)_PackageLoadPackageAlert).set_Visible(true);
					_PackageLoadPackageAlert.set_TextColor(Color.get_Red());
					return;
				}
			}
			_PackageLoadPackageAlert.set_Text("Package has been renamed!");
			((Control)_PackageLoadPackageAlert).set_Visible(true);
			_PackageLoadPackageAlert.set_TextColor(Color.get_LimeGreen());
			((Control)_ButtonSaveRename).set_Visible(false);
			((Control)_DefaultPackageLabel).set_Visible(true);
			((Control)_PackageRenameTextBox).set_Visible(false);
			((Control)_ButtonLoadRename).set_Visible(true);
			((Control)_ButtonSaveRename).set_Visible(false);
			PackageData package = _PackageData.FirstOrDefault((PackageData p) => p.PackageName == _DefaultPackageLabel.get_Text());
			if (package != null)
			{
				package.PackageName = ((TextInputBase)_PackageRenameTextBox).get_Text();
				_PackageSettingEntry.set_Value(((TextInputBase)_PackageRenameTextBox).get_Text());
				_DefaultPackageLabel.set_Text(((TextInputBase)_PackageRenameTextBox).get_Text());
			}
			SavePackageJsonUpdate();
			LoadPersonalPackageDropdownOptions();
			_BaubleFarmModule.Restart();
		}

		private void SavePackageJsonUpdate()
		{
			string jsonFilePath = Path.Combine(_BaubleFarmModule.DirectoriesManager.GetFullDirectoryPath("Shiny_Baubles"), "Package_Defaults.json");
			try
			{
				string jsonContent = JsonSerializer.Serialize<List<PackageData>>(_PackageData, _jsonOptions);
				File.WriteAllText(jsonFilePath, jsonContent);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to save JSON file: " + ex.Message);
			}
		}

		public PackageSettingsTabView()
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
