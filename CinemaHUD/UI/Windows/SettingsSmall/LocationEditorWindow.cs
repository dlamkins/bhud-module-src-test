using System;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule;
using CinemaModule.Models;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace CinemaHUD.UI.Windows.SettingsSmall
{
	public class LocationEditorWindow : SmallWindow
	{
		private class SavedLocationExport
		{
			public string Name { get; set; }

			public WorldPosition3D Position { get; set; }

			public float ScreenWidth { get; set; }
		}

		private const float MoveStep = 0.5f;

		private const float RotateStep = 5f;

		private const float DefaultScreenDistanceFromPlayer = 10f;

		private const float DefaultScreenHeightAbovePlayer = 3f;

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private SavedLocation _location;

		private TextBox _nameTextBox;

		private Label _positionLabel;

		private TrackBar _widthTrackBar;

		private Label _widthValueLabel;

		public LocationEditorWindow(CinemaUserSettings settings, CinemaController controller)
			: base("Edit Location")
		{
			_settings = settings;
			_controller = controller;
		}

		protected override void BuildContent()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Expected O, but got Unknown
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			val.set_ControlPadding(new Vector2(0f, 8f));
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel panel = val;
			Label val2 = new Label();
			val2.set_Text("Location Name");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Parent((Container)(object)panel);
			TextBox val3 = new TextBox();
			((Control)val3).set_Width(280);
			((TextInputBase)val3).set_PlaceholderText("Enter a name for this location");
			((Control)val3).set_Parent((Container)(object)panel);
			_nameTextBox = val3;
			((TextInputBase)_nameTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ApplyNameChange();
			});
			Label val4 = new Label();
			val4.set_Text("Position");
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Parent((Container)(object)panel);
			Label val5 = new Label();
			val5.set_Text("Not set");
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			val5.set_TextColor(Color.get_LightGray());
			((Control)val5).set_Parent((Container)(object)panel);
			_positionLabel = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Set to My Current Position");
			((Control)val6).set_Width(200);
			((Control)val6).set_Parent((Container)(object)panel);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetPositionFromPlayer();
			});
			BuildMovementControls((Container)(object)panel);
			BuildRotationControls((Container)(object)panel);
			Label val7 = new Label();
			val7.set_Text("Screen Width");
			val7.set_AutoSizeHeight(true);
			val7.set_AutoSizeWidth(true);
			val7.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val7).set_Parent((Container)(object)panel);
			FlowPanel val8 = new FlowPanel();
			val8.set_FlowDirection((ControlFlowDirection)0);
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			val8.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val8).set_Parent((Container)(object)panel);
			FlowPanel widthRow = val8;
			TrackBar val9 = new TrackBar();
			val9.set_MinValue(4f);
			val9.set_MaxValue(50f);
			val9.set_Value(10f);
			((Control)val9).set_Width(200);
			((Control)val9).set_Parent((Container)(object)widthRow);
			_widthTrackBar = val9;
			Label val10 = new Label();
			val10.set_Text("10m");
			val10.set_AutoSizeHeight(true);
			val10.set_AutoSizeWidth(true);
			((Control)val10).set_Parent((Container)(object)widthRow);
			_widthValueLabel = val10;
			_widthTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_widthValueLabel.set_Text($"{_widthTrackBar.get_Value():F0}m");
				ApplyWidthChange();
			});
			StandardButton val11 = new StandardButton();
			val11.set_Text("Export Location");
			((Control)val11).set_Width(200);
			((Control)val11).set_Parent((Container)(object)panel);
			((Control)val11).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopyLocationToClipboard();
			});
		}

		private void BuildMovementControls(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("Move Position");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val2).set_Parent(parent);
			FlowPanel horizontalPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("West");
			((Control)val3).set_Width(70);
			((Control)val3).set_Parent((Container)(object)horizontalPanel);
			StandardButton westBtn = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("East");
			((Control)val4).set_Width(70);
			((Control)val4).set_Parent((Container)(object)horizontalPanel);
			StandardButton eastBtn = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("North");
			((Control)val5).set_Width(65);
			((Control)val5).set_Parent((Container)(object)horizontalPanel);
			StandardButton northBtn = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("South");
			((Control)val6).set_Width(65);
			((Control)val6).set_Parent((Container)(object)horizontalPanel);
			((Control)westBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(-0.5f, 0f, 0f);
			});
			((Control)eastBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(0.5f, 0f, 0f);
			});
			((Control)northBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(0f, 0.5f, 0f);
			});
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(0f, -0.5f, 0f);
			});
			FlowPanel val7 = new FlowPanel();
			val7.set_FlowDirection((ControlFlowDirection)0);
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			val7.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val7).set_Parent(parent);
			FlowPanel verticalPanel = val7;
			StandardButton val8 = new StandardButton();
			val8.set_Text("Up");
			((Control)val8).set_Width(70);
			((Control)val8).set_Parent((Container)(object)verticalPanel);
			StandardButton upBtn = val8;
			StandardButton val9 = new StandardButton();
			val9.set_Text("Down");
			((Control)val9).set_Width(70);
			((Control)val9).set_Parent((Container)(object)verticalPanel);
			((Control)upBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(0f, 0f, 0.5f);
			});
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MovePosition(0f, 0f, -0.5f);
			});
		}

		private void BuildRotationControls(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("Rotate Screen");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val2).set_Parent(parent);
			FlowPanel rotationPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Pan West");
			((Control)val3).set_Width(70);
			((Control)val3).set_Parent((Container)(object)rotationPanel);
			StandardButton panWestBtn = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Pan East");
			((Control)val4).set_Width(70);
			((Control)val4).set_Parent((Container)(object)rotationPanel);
			StandardButton panEastBtn = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Tilt Up");
			((Control)val5).set_Width(65);
			((Control)val5).set_Parent((Container)(object)rotationPanel);
			StandardButton tiltUpBtn = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Tilt Down");
			((Control)val6).set_Width(65);
			((Control)val6).set_Parent((Container)(object)rotationPanel);
			((Control)panWestBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RotatePosition(-5f, 0f);
			});
			((Control)panEastBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RotatePosition(5f, 0f);
			});
			((Control)tiltUpBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RotatePosition(0f, 5f);
			});
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RotatePosition(0f, -5f);
			});
			FlowPanel val7 = new FlowPanel();
			val7.set_FlowDirection((ControlFlowDirection)0);
			((Container)val7).set_WidthSizingMode((SizingMode)2);
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			val7.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val7).set_Parent(parent);
			FlowPanel extraPanel = val7;
			StandardButton val8 = new StandardButton();
			val8.set_Text("Reset Rotation");
			((Control)val8).set_Width(110);
			((Control)val8).set_Parent((Container)(object)extraPanel);
			StandardButton resetRotationBtn = val8;
			StandardButton val9 = new StandardButton();
			val9.set_Text("Face Me");
			((Control)val9).set_Width(80);
			((Control)val9).set_Parent((Container)(object)extraPanel);
			((Control)resetRotationBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ResetRotation();
			});
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				FacePlayer();
			});
		}

		public void CreateNew()
		{
			WorldPosition3D position = CreatePositionFromPlayer();
			_location = _settings.AddSavedLocation("New Location", position, _settings.WorldScreenWidth);
			_controller.SelectSavedLocation(_location.Id);
			((WindowBase2)this).set_Title("New Location");
			LoadLocationIntoUI();
			((Control)this).Show();
		}

		private WorldPosition3D CreatePositionFromPlayer()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService mumble = GameService.Gw2Mumble;
			if (mumble == null || !mumble.get_IsAvailable())
			{
				return new WorldPosition3D(0f, 0f, 0f, 0f, 0f, 0);
			}
			Vector3 position = mumble.get_PlayerCharacter().get_Position();
			Vector3 playerForward = mumble.get_PlayerCharacter().get_Forward();
			int mapId = mumble.get_CurrentMap().get_Id();
			Vector3 screenPos = position + playerForward * 10f + new Vector3(0f, 0f, 3f);
			Vector3 toPlayer = position - screenPos;
			float yaw = MathHelper.ToDegrees((float)Math.Atan2(toPlayer.X, toPlayer.Y));
			yaw = WorldPosition3D.NormalizeYaw(yaw);
			return new WorldPosition3D(screenPos.X, screenPos.Y, screenPos.Z, yaw, 0f, mapId);
		}

		public void Edit(SavedLocation location)
		{
			_location = location;
			if (_settings.SelectedSavedLocationId != location.Id)
			{
				_controller.SelectSavedLocation(location.Id);
			}
			((WindowBase2)this).set_Title("Edit: " + location.Name);
			LoadLocationIntoUI();
			((Control)this).Show();
		}

		private void LoadLocationIntoUI()
		{
			((TextInputBase)_nameTextBox).set_Text(_location?.Name ?? "");
			_widthTrackBar.set_Value(_location?.ScreenWidth ?? 10f);
			_widthValueLabel.set_Text($"{_widthTrackBar.get_Value():F0}m");
			UpdatePositionLabel();
		}

		private void UpdatePositionLabel()
		{
			if (_location?.Position != null && _location.Position.MapId != 0)
			{
				WorldPosition3D pos = _location.Position;
				_positionLabel.set_Text($"X: {pos.X:F1} Y: {pos.Y:F1} Z: {pos.Z:F1} | Yaw: {pos.Yaw:F0}° Tilt: {pos.Pitch:F0}° | Map: {pos.MapId}");
			}
			else
			{
				_positionLabel.set_Text("Not set - Go in-game and click 'Set to My Current Position'");
			}
		}

		private void ApplyNameChange()
		{
			if (_location != null)
			{
				_location.Name = (string.IsNullOrWhiteSpace(((TextInputBase)_nameTextBox).get_Text()) ? "Unnamed Location" : ((TextInputBase)_nameTextBox).get_Text());
				_settings.UpdateSavedLocation(_location);
			}
		}

		private void ApplyWidthChange()
		{
			if (_location != null)
			{
				_location.ScreenWidth = _widthTrackBar.get_Value();
				_settings.UpdateSavedLocation(_location);
				_settings.WorldScreenWidth = _widthTrackBar.get_Value();
			}
		}

		private void ApplyPositionChange()
		{
			if (_location != null)
			{
				_settings.UpdateSavedLocation(_location);
				if (_location.Position != null)
				{
					_settings.WorldPosition = new WorldPosition3D(_location.Position.X, _location.Position.Y, _location.Position.Z, _location.Position.Yaw, _location.Position.Pitch, _location.Position.MapId);
				}
			}
		}

		private void SetPositionFromPlayer()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			if (_location != null)
			{
				Gw2MumbleService mumble = GameService.Gw2Mumble;
				if (mumble != null && mumble.get_IsAvailable())
				{
					Vector3 position = mumble.get_PlayerCharacter().get_Position();
					Vector3 playerForward = mumble.get_PlayerCharacter().get_Forward();
					int mapId = mumble.get_CurrentMap().get_Id();
					Vector3 screenPos = position + playerForward * 10f + new Vector3(0f, 0f, 3f);
					Vector3 toPlayer = position - screenPos;
					float yaw = MathHelper.ToDegrees((float)Math.Atan2(toPlayer.X, toPlayer.Y));
					yaw = WorldPosition3D.NormalizeYaw(yaw);
					_location.Position = new WorldPosition3D(screenPos.X, screenPos.Y, screenPos.Z, yaw, 0f, mapId);
					ApplyPositionChange();
					UpdatePositionLabel();
				}
			}
		}

		private void MovePosition(float deltaX, float deltaY, float deltaZ)
		{
			if (_location?.Position != null && _location.Position.MapId != 0)
			{
				_location.Position.X += deltaX;
				_location.Position.Y += deltaY;
				_location.Position.Z += deltaZ;
				ApplyPositionChange();
				UpdatePositionLabel();
			}
		}

		private void RotatePosition(float deltaYaw, float deltaPitch)
		{
			if (_location?.Position != null && _location.Position.MapId != 0)
			{
				_location.Position.Yaw = WorldPosition3D.NormalizeYaw(_location.Position.Yaw + deltaYaw);
				_location.Position.Pitch = MathHelper.Clamp(_location.Position.Pitch + deltaPitch, -89f, 89f);
				ApplyPositionChange();
				UpdatePositionLabel();
			}
		}

		private void ResetRotation()
		{
			if (_location?.Position != null)
			{
				_location.Position.Yaw = 0f;
				_location.Position.Pitch = 0f;
				ApplyPositionChange();
				UpdatePositionLabel();
			}
		}

		private void FacePlayer()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if (_location?.Position != null && _location.Position.MapId != 0)
			{
				Gw2MumbleService mumble = GameService.Gw2Mumble;
				if (mumble != null && mumble.get_IsAvailable())
				{
					Vector3 position = mumble.get_PlayerCharacter().get_Position();
					Vector3 screenPos = _location.Position.ToVector3();
					Vector3 toPlayer = position - screenPos;
					float yaw = MathHelper.ToDegrees((float)Math.Atan2(toPlayer.X, toPlayer.Y));
					_location.Position.Yaw = WorldPosition3D.NormalizeYaw(yaw);
					_location.Position.Pitch = 0f;
					ApplyPositionChange();
					UpdatePositionLabel();
				}
			}
		}

		private void CopyLocationToClipboard()
		{
			if (_location != null)
			{
				try
				{
					Clipboard.SetText(JsonConvert.SerializeObject((object)new SavedLocationExport
					{
						Name = _location.Name,
						Position = _location.Position,
						ScreenWidth = _location.ScreenWidth
					}, (Formatting)0));
					Logger.GetLogger(((object)this).GetType()).Info("Copied location '" + _location.Name + "' to clipboard");
				}
				catch (Exception ex)
				{
					Logger.GetLogger(((object)this).GetType()).Warn("Failed to copy location to clipboard: " + ex.Message);
				}
			}
		}
	}
}
