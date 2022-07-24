using System;
using System.Globalization;
using AsyncWindowsClipboard;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mumble_Info._Extensions;

namespace Nekres.Mumble_Info
{
	internal class DataPanel : Container
	{
		private readonly Color _grey = new Color(168, 168, 168);

		private readonly Color _orange = new Color(252, 168, 0);

		private readonly Color _red = new Color(252, 84, 84);

		private readonly Color _softRed = new Color(250, 148, 148);

		private readonly Color _lemonGreen = new Color(84, 252, 84);

		private readonly Color _cyan = new Color(84, 252, 252);

		private readonly Color _blue = new Color(0, 168, 252);

		private readonly Color _green = new Color(0, 168, 0);

		private readonly Color _brown = new Color(158, 81, 44);

		private readonly Color _yellow = new Color(252, 252, 84);

		private readonly Color _softYellow = new Color(250, 250, 148);

		private readonly Color _borderColor = Color.get_AntiqueWhite();

		private readonly Color _clickColor = Color.get_AliceBlue();

		private readonly BitmapFont _font;

		private const int _leftMargin = 10;

		private const int _rightMargin = 10;

		private const int _topMargin = 5;

		private const int _borderSize = 1;

		private const string _clipboardMessage = "Copied!";

		private const string _decimalFormat = "0.###";

		private bool _isMousePressed;

		private Rectangle _avatarPositionBounds;

		private bool _mouseOverAvatarPosition;

		private Rectangle _avatarFacingBounds;

		private bool _mouseOverAvatarFacing;

		private Rectangle _mapCoordinatesBounds;

		private bool _mouseOverMapCoordinates;

		private Rectangle _cameraDirectionBounds;

		private bool _mouseOverCameraDirection;

		private Rectangle _cameraPositionBounds;

		private bool _mouseOverCameraPosition;

		private float _memoryUsage => MumbleInfoModule.ModuleInstance.MemoryUsage;

		private float _cpuUsage => MumbleInfoModule.ModuleInstance.CpuUsage;

		private string _cpuName => MumbleInfoModule.ModuleInstance.CpuName;

		private Map _currentMap => MumbleInfoModule.ModuleInstance.CurrentMap;

		private Specialization _currentSpec => MumbleInfoModule.ModuleInstance.CurrentSpec;

		public DataPanel()
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			_font = Control.get_Content().GetFont((FontFace)0, (FontSize)36, (FontStyle)0);
			UpdateLocation(null, null);
			((Control)Control.get_Graphics().get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		protected override void DisposeControl()
		{
			((Control)Control.get_Graphics().get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
			((Container)this).DisposeControl();
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			Point relPos = ((Control)this).get_RelativeMousePosition();
			_mouseOverAvatarFacing = ((Rectangle)(ref _avatarFacingBounds)).Contains(relPos);
			_mouseOverAvatarPosition = ((Rectangle)(ref _avatarPositionBounds)).Contains(relPos);
			_mouseOverMapCoordinates = ((Rectangle)(ref _mapCoordinatesBounds)).Contains(relPos);
			_mouseOverCameraDirection = ((Rectangle)(ref _cameraDirectionBounds)).Contains(relPos);
			_mouseOverCameraPosition = ((Rectangle)(ref _cameraPositionBounds)).Contains(relPos);
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			_isMousePressed = false;
			Vector3 val;
			if (_mouseOverAvatarFacing)
			{
				WindowsClipboardService windowsClipboardService = ClipboardUtil.get_WindowsClipboardService();
				val = GameService.Gw2Mumble.get_PlayerCharacter().get_Forward();
				windowsClipboardService.SetTextAsync(((object)(Vector3)(ref val)).ToString());
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverAvatarPosition)
			{
				WindowsClipboardService windowsClipboardService2 = ClipboardUtil.get_WindowsClipboardService();
				string[] obj = new string[7] { "xpos=\"", null, null, null, null, null, null };
				val = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				obj[1] = val.X.ToString(CultureInfo.InvariantCulture);
				obj[2] = "\" ypos=\"";
				val = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				obj[3] = val.Z.ToString(CultureInfo.InvariantCulture);
				obj[4] = "\" zpos=\"";
				val = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				obj[5] = val.Y.ToString(CultureInfo.InvariantCulture);
				obj[6] = "\"";
				windowsClipboardService2.SetTextAsync(string.Concat(obj));
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverMapCoordinates)
			{
				WindowsClipboardService windowsClipboardService3 = ClipboardUtil.get_WindowsClipboardService();
				string[] obj2 = new string[5] { "xpos=\"", null, null, null, null };
				Coordinates2 playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
				obj2[1] = ((Coordinates2)(ref playerLocationMap)).get_X().ToString(CultureInfo.InvariantCulture);
				obj2[2] = "\" ypos=\"";
				playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
				obj2[3] = ((Coordinates2)(ref playerLocationMap)).get_Y().ToString(CultureInfo.InvariantCulture);
				obj2[4] = "\"";
				windowsClipboardService3.SetTextAsync(string.Concat(obj2));
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverCameraDirection)
			{
				WindowsClipboardService windowsClipboardService4 = ClipboardUtil.get_WindowsClipboardService();
				val = GameService.Gw2Mumble.get_PlayerCamera().get_Forward();
				windowsClipboardService4.SetTextAsync(((object)(Vector3)(ref val)).ToString());
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverCameraPosition)
			{
				WindowsClipboardService windowsClipboardService5 = ClipboardUtil.get_WindowsClipboardService();
				string[] obj3 = new string[7] { "xpos=\"", null, null, null, null, null, null };
				val = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
				obj3[1] = val.X.ToString(CultureInfo.InvariantCulture);
				obj3[2] = "\" ypos=\"";
				val = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
				obj3[3] = val.Z.ToString(CultureInfo.InvariantCulture);
				obj3[4] = "\" zpos=\"";
				val = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
				obj3[5] = val.Y.ToString(CultureInfo.InvariantCulture);
				obj3[6] = "\"";
				windowsClipboardService5.SetTextAsync(string.Concat(obj3));
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			_isMousePressed = true;
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		private void UpdateLocation(object sender, EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(0, 0));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0730: Unknown result type (might be due to invalid IL or missing references)
			//IL_0732: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0811: Unknown result type (might be due to invalid IL or missing references)
			//IL_0814: Unknown result type (might be due to invalid IL or missing references)
			//IL_0835: Unknown result type (might be due to invalid IL or missing references)
			//IL_083a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0849: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_087e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0890: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0924: Unknown result type (might be due to invalid IL or missing references)
			//IL_094b: Unknown result type (might be due to invalid IL or missing references)
			//IL_094e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0979: Unknown result type (might be due to invalid IL or missing references)
			//IL_098e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ddd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ddf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffd: Unknown result type (might be due to invalid IL or missing references)
			//IL_102c: Unknown result type (might be due to invalid IL or missing references)
			//IL_103f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1061: Unknown result type (might be due to invalid IL or missing references)
			//IL_1064: Unknown result type (might be due to invalid IL or missing references)
			//IL_1088: Unknown result type (might be due to invalid IL or missing references)
			//IL_109b: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_111c: Unknown result type (might be due to invalid IL or missing references)
			//IL_111e: Unknown result type (might be due to invalid IL or missing references)
			//IL_114d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1162: Unknown result type (might be due to invalid IL or missing references)
			//IL_1189: Unknown result type (might be due to invalid IL or missing references)
			//IL_118c: Unknown result type (might be due to invalid IL or missing references)
			//IL_11b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1208: Unknown result type (might be due to invalid IL or missing references)
			//IL_121d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1244: Unknown result type (might be due to invalid IL or missing references)
			//IL_1246: Unknown result type (might be due to invalid IL or missing references)
			//IL_127f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1292: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12db: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_1310: Unknown result type (might be due to invalid IL or missing references)
			//IL_1313: Unknown result type (might be due to invalid IL or missing references)
			//IL_1333: Unknown result type (might be due to invalid IL or missing references)
			//IL_1348: Unknown result type (might be due to invalid IL or missing references)
			//IL_136f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1371: Unknown result type (might be due to invalid IL or missing references)
			//IL_1393: Unknown result type (might be due to invalid IL or missing references)
			//IL_13c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1403: Unknown result type (might be due to invalid IL or missing references)
			//IL_1406: Unknown result type (might be due to invalid IL or missing references)
			//IL_1427: Unknown result type (might be due to invalid IL or missing references)
			//IL_142c: Unknown result type (might be due to invalid IL or missing references)
			//IL_143b: Unknown result type (might be due to invalid IL or missing references)
			//IL_144e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1470: Unknown result type (might be due to invalid IL or missing references)
			//IL_1473: Unknown result type (might be due to invalid IL or missing references)
			//IL_1482: Unknown result type (might be due to invalid IL or missing references)
			//IL_1484: Unknown result type (might be due to invalid IL or missing references)
			//IL_1497: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1501: Unknown result type (might be due to invalid IL or missing references)
			//IL_1516: Unknown result type (might be due to invalid IL or missing references)
			//IL_153d: Unknown result type (might be due to invalid IL or missing references)
			//IL_153f: Unknown result type (might be due to invalid IL or missing references)
			//IL_157a: Unknown result type (might be due to invalid IL or missing references)
			//IL_158f: Unknown result type (might be due to invalid IL or missing references)
			//IL_15b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_15b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1613: Unknown result type (might be due to invalid IL or missing references)
			//IL_163a: Unknown result type (might be due to invalid IL or missing references)
			//IL_163d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1666: Unknown result type (might be due to invalid IL or missing references)
			//IL_1687: Unknown result type (might be due to invalid IL or missing references)
			//IL_169a: Unknown result type (might be due to invalid IL or missing references)
			//IL_16bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_16bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_16e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1707: Unknown result type (might be due to invalid IL or missing references)
			//IL_1729: Unknown result type (might be due to invalid IL or missing references)
			//IL_172c: Unknown result type (might be due to invalid IL or missing references)
			//IL_173b: Unknown result type (might be due to invalid IL or missing references)
			//IL_173d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1750: Unknown result type (might be due to invalid IL or missing references)
			//IL_1765: Unknown result type (might be due to invalid IL or missing references)
			//IL_178c: Unknown result type (might be due to invalid IL or missing references)
			//IL_178e: Unknown result type (might be due to invalid IL or missing references)
			//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_17da: Unknown result type (might be due to invalid IL or missing references)
			//IL_1801: Unknown result type (might be due to invalid IL or missing references)
			//IL_1804: Unknown result type (might be due to invalid IL or missing references)
			//IL_1845: Unknown result type (might be due to invalid IL or missing references)
			//IL_185a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1881: Unknown result type (might be due to invalid IL or missing references)
			//IL_1884: Unknown result type (might be due to invalid IL or missing references)
			//IL_18c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_18da: Unknown result type (might be due to invalid IL or missing references)
			//IL_1901: Unknown result type (might be due to invalid IL or missing references)
			//IL_1904: Unknown result type (might be due to invalid IL or missing references)
			//IL_192d: Unknown result type (might be due to invalid IL or missing references)
			//IL_193f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1947: Unknown result type (might be due to invalid IL or missing references)
			//IL_194f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1957: Unknown result type (might be due to invalid IL or missing references)
			//IL_198b: Unknown result type (might be due to invalid IL or missing references)
			//IL_19a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_19f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a34: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a46: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a48: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a97: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a99: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ad0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b50: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b65: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c38: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c57: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1caf: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ceb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ced: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d26: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d62: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d65: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d89: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1de1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1df6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e58: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e94: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e97: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ebb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ece: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ef0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ef3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f13: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f51: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fc9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fef: Unknown result type (might be due to invalid IL or missing references)
			//IL_2002: Unknown result type (might be due to invalid IL or missing references)
			//IL_2024: Unknown result type (might be due to invalid IL or missing references)
			//IL_2027: Unknown result type (might be due to invalid IL or missing references)
			//IL_204b: Unknown result type (might be due to invalid IL or missing references)
			//IL_205e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2080: Unknown result type (might be due to invalid IL or missing references)
			//IL_2083: Unknown result type (might be due to invalid IL or missing references)
			//IL_20a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_20b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_20df: Unknown result type (might be due to invalid IL or missing references)
			//IL_20e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_2103: Unknown result type (might be due to invalid IL or missing references)
			//IL_211a: Unknown result type (might be due to invalid IL or missing references)
			//IL_212f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2156: Unknown result type (might be due to invalid IL or missing references)
			//IL_2159: Unknown result type (might be due to invalid IL or missing references)
			//IL_217d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2190: Unknown result type (might be due to invalid IL or missing references)
			//IL_21b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_21b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_21d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_21ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_2211: Unknown result type (might be due to invalid IL or missing references)
			//IL_2213: Unknown result type (might be due to invalid IL or missing references)
			//IL_224c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2261: Unknown result type (might be due to invalid IL or missing references)
			//IL_2288: Unknown result type (might be due to invalid IL or missing references)
			//IL_228b: Unknown result type (might be due to invalid IL or missing references)
			//IL_22da: Unknown result type (might be due to invalid IL or missing references)
			//IL_22ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_22fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_231e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2321: Unknown result type (might be due to invalid IL or missing references)
			//IL_2343: Unknown result type (might be due to invalid IL or missing references)
			//IL_2358: Unknown result type (might be due to invalid IL or missing references)
			//IL_236d: Unknown result type (might be due to invalid IL or missing references)
			//IL_238e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2390: Unknown result type (might be due to invalid IL or missing references)
			//IL_23b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_23c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_23dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_23fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_2400: Unknown result type (might be due to invalid IL or missing references)
			//IL_243a: Unknown result type (might be due to invalid IL or missing references)
			//IL_244d: Unknown result type (might be due to invalid IL or missing references)
			//IL_245d: Unknown result type (might be due to invalid IL or missing references)
			//IL_247e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2481: Unknown result type (might be due to invalid IL or missing references)
			//IL_24a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_24b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_24cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_24ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_24f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2512: Unknown result type (might be due to invalid IL or missing references)
			//IL_2527: Unknown result type (might be due to invalid IL or missing references)
			//IL_253c: Unknown result type (might be due to invalid IL or missing references)
			//IL_255d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2560: Unknown result type (might be due to invalid IL or missing references)
			//IL_259e: Unknown result type (might be due to invalid IL or missing references)
			//IL_25b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_25c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_25e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_2607: Unknown result type (might be due to invalid IL or missing references)
			//IL_261c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2631: Unknown result type (might be due to invalid IL or missing references)
			//IL_2652: Unknown result type (might be due to invalid IL or missing references)
			//IL_2654: Unknown result type (might be due to invalid IL or missing references)
			//IL_2676: Unknown result type (might be due to invalid IL or missing references)
			//IL_268b: Unknown result type (might be due to invalid IL or missing references)
			//IL_26a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_26c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_26c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2701: Unknown result type (might be due to invalid IL or missing references)
			//IL_2714: Unknown result type (might be due to invalid IL or missing references)
			//IL_2724: Unknown result type (might be due to invalid IL or missing references)
			//IL_2745: Unknown result type (might be due to invalid IL or missing references)
			//IL_2748: Unknown result type (might be due to invalid IL or missing references)
			//IL_276a: Unknown result type (might be due to invalid IL or missing references)
			//IL_277f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2794: Unknown result type (might be due to invalid IL or missing references)
			//IL_27b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_27b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_27d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_27ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_2803: Unknown result type (might be due to invalid IL or missing references)
			//IL_2824: Unknown result type (might be due to invalid IL or missing references)
			//IL_2827: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				int calcTopMargin = 5;
				int calcLeftMargin = 10;
				string text = GameService.Gw2Mumble.get_RawClient().get_Name() + "  ";
				int width = (int)_font.MeasureString(text).Width;
				int height = (int)_font.MeasureString(text).Height;
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _brown, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"({GameService.Gw2Mumble.get_Info().get_BuildId()})/";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"(Mumble Link v{GameService.Gw2Mumble.get_Info().get_Version()})";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 10;
				text = GameService.Gw2Mumble.get_Info().get_ServerAddress() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_Info().get_ServerPort()}  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"- {GameService.Gw2Mumble.get_Info().get_ShardId()}  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"({GameService.Gw2Mumble.get_RawClient().get_Instance()})";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height * 2;
				calcLeftMargin = 10;
				text = "Avatar";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = $"{GameService.Gw2Mumble.get_PlayerCharacter().get_Name()} - {GameService.Gw2Mumble.get_PlayerCharacter().get_Race()}";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softRed, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"  ({GameService.Gw2Mumble.get_PlayerCharacter().get_TeamColorId()})";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Profession";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_PlayerCharacter().get_Profession()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				Specialization currentSpec = _currentSpec;
				if (currentSpec != null && currentSpec.get_Elite() && _currentSpec.get_Id() == GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization())
				{
					calcTopMargin += height;
					calcLeftMargin = 30;
					text = "Elite";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = _currentSpec.get_Name() ?? "";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = $"  ({_currentSpec.get_Id()})";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				Vector3 playerPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				text = "X";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				Rectangle infoBounds = rect;
				calcLeftMargin += width;
				text = "Y";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "Z";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = playerPos.X.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + playerPos.Y.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + playerPos.Z.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _avatarPositionBounds);
				if (_mouseOverAvatarPosition)
				{
					DrawBorder(spriteBatch, _avatarPositionBounds);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				Coordinates3 playerFacing = GameService.Gw2Mumble.get_RawClient().get_AvatarFront();
				text = "Facing";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				infoBounds = rect;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = ((Coordinates3)(ref playerFacing)).get_X().ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + ((Coordinates3)(ref playerFacing)).get_Y().ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + ((Coordinates3)(ref playerFacing)).get_Z().ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _avatarFacingBounds);
				if (_mouseOverAvatarFacing)
				{
					DrawBorder(spriteBatch, _avatarFacingBounds);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = DirectionUtil.IsFacing(playerFacing.SwapYZ()).ToString().SplitAtUpperCase()
					.Trim();
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height * 2;
				calcLeftMargin = 10;
				text = "Map";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (_currentMap != null && _currentMap.get_Id() == GameService.Gw2Mumble.get_CurrentMap().get_Id())
				{
					calcTopMargin += height;
					calcLeftMargin = 30;
					text = _currentMap.get_Name() ?? "";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcTopMargin += height;
					calcLeftMargin = 30;
					text = "Region";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = _currentMap.get_RegionName() ?? "";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcTopMargin += height;
					calcLeftMargin = 30;
					text = "Continent";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcLeftMargin += width;
					text = _currentMap.get_ContinentName() ?? "";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Id";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_CurrentMap().get_Id()}";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Type";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = string.Format("{0} ({1})", GameService.Gw2Mumble.get_CurrentMap().get_Type(), GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() ? "PvP" : "PvE");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				Coordinates2 playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
				text = "X";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				infoBounds = rect;
				calcLeftMargin += width;
				text = "Y";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = ((Coordinates2)(ref playerLocationMap)).get_X().ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + ((Coordinates2)(ref playerLocationMap)).get_Y().ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _mapCoordinatesBounds);
				if (_mouseOverMapCoordinates)
				{
					DrawBorder(spriteBatch, _mapCoordinatesBounds);
				}
				calcTopMargin += height * 2;
				calcLeftMargin = 10;
				text = "Camera";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				Vector3 cameraForward = GameService.Gw2Mumble.get_PlayerCamera().get_Forward();
				text = "Direction";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				infoBounds = rect;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = cameraForward.X.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + cameraForward.Y.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + cameraForward.Z.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _cameraDirectionBounds);
				if (_mouseOverCameraDirection)
				{
					DrawBorder(spriteBatch, _cameraDirectionBounds);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = DirectionUtil.IsFacing(new Coordinates3((double)cameraForward.X, (double)cameraForward.Y, (double)cameraForward.Z)).ToString().SplitAtUpperCase()
					.Trim() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				Vector3 cameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
				text = "Position";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				infoBounds = rect;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = cameraPosition.X.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + cameraPosition.Y.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + cameraPosition.Z.ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _cameraPositionBounds);
				if (_mouseOverCameraPosition)
				{
					DrawBorder(spriteBatch, _cameraPositionBounds);
				}
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Field of View";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_FieldOfView()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Near Plane Render Distance";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_NearPlaneRenderDistance()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Far Plane Render Distance";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_FarPlaneRenderDistance()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height * 2;
				calcLeftMargin = 10;
				text = "User Interface";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Size";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_UI().get_UISize()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Text Input Focused";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcLeftMargin += width;
				text = $"{GameService.Gw2Mumble.get_UI().get_IsTextInputFocused()}";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				calcTopMargin = 5;
				int calcRightMargin = 10;
				if (MumbleInfoModule.ModuleInstance.EnablePerformanceCounters.get_Value())
				{
					text = _memoryUsage.ToString("0.###") + " MB";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = "Memory Usage";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcTopMargin += height;
					calcRightMargin = 10;
					text = $"{Environment.ProcessorCount}x {_cpuName}";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = "CPU";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcTopMargin += height;
					calcRightMargin = 10;
					text = _cpuUsage.ToString("0.###") + "%";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = "CPU Usage";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcTopMargin += height;
					calcRightMargin = 10;
					text = Control.get_Graphics().get_GraphicsDevice().get_Adapter()
						.get_Description() ?? "";
					width = (int)_font.MeasureString(text).Width;
					height = (int)_font.MeasureString(text).Height;
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = ":  ";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
					calcRightMargin += width;
					text = "GPU";
					width = (int)_font.MeasureString(text).Width;
					height = Math.Max(height, (int)_font.MeasureString(text).Height);
					((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		private void DrawBorder(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, bounds.Width + 1, 1), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, 1, bounds.Height + 1), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y + bounds.Height + 1, bounds.Width + 1, 1), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X + bounds.Width + 1, bounds.Y, 1, bounds.Height + 1), _isMousePressed ? _clickColor : _borderColor);
		}
	}
}
