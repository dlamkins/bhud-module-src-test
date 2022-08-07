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
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
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
				val = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				windowsClipboardService2.SetTextAsync($"xpos=\"{val.X.ToString(CultureInfo.InvariantCulture)}\" ypos=\"{(MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z : GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y).ToString(CultureInfo.InvariantCulture)}\" zpos=\"{(MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y : GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z).ToString(CultureInfo.InvariantCulture)}\"");
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverMapCoordinates)
			{
				WindowsClipboardService windowsClipboardService3 = ClipboardUtil.get_WindowsClipboardService();
				Coordinates2 playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
				string arg = ((Coordinates2)(ref playerLocationMap)).get_X().ToString(CultureInfo.InvariantCulture);
				playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
				windowsClipboardService3.SetTextAsync($"xpos=\"{arg}\" ypos=\"{((Coordinates2)(ref playerLocationMap)).get_Y().ToString(CultureInfo.InvariantCulture)}\"");
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
				val = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
				windowsClipboardService5.SetTextAsync($"xpos=\"{val.X.ToString(CultureInfo.InvariantCulture)}\" ypos=\"{(MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z : GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y).ToString(CultureInfo.InvariantCulture)}\" zpos=\"{(MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y : GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z).ToString(CultureInfo.InvariantCulture)}\"");
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
			//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fea: Unknown result type (might be due to invalid IL or missing references)
			//IL_100c: Unknown result type (might be due to invalid IL or missing references)
			//IL_100f: Unknown result type (might be due to invalid IL or missing references)
			//IL_102f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1044: Unknown result type (might be due to invalid IL or missing references)
			//IL_106b: Unknown result type (might be due to invalid IL or missing references)
			//IL_106d: Unknown result type (might be due to invalid IL or missing references)
			//IL_109c: Unknown result type (might be due to invalid IL or missing references)
			//IL_10af: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_110b: Unknown result type (might be due to invalid IL or missing references)
			//IL_112d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1130: Unknown result type (might be due to invalid IL or missing references)
			//IL_1150: Unknown result type (might be due to invalid IL or missing references)
			//IL_1165: Unknown result type (might be due to invalid IL or missing references)
			//IL_118c: Unknown result type (might be due to invalid IL or missing references)
			//IL_118e: Unknown result type (might be due to invalid IL or missing references)
			//IL_11bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1220: Unknown result type (might be due to invalid IL or missing references)
			//IL_1233: Unknown result type (might be due to invalid IL or missing references)
			//IL_1255: Unknown result type (might be due to invalid IL or missing references)
			//IL_1258: Unknown result type (might be due to invalid IL or missing references)
			//IL_1278: Unknown result type (might be due to invalid IL or missing references)
			//IL_128d: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_1302: Unknown result type (might be due to invalid IL or missing references)
			//IL_1324: Unknown result type (might be due to invalid IL or missing references)
			//IL_1327: Unknown result type (might be due to invalid IL or missing references)
			//IL_134b: Unknown result type (might be due to invalid IL or missing references)
			//IL_135e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1380: Unknown result type (might be due to invalid IL or missing references)
			//IL_1383: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_13b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_13df: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1403: Unknown result type (might be due to invalid IL or missing references)
			//IL_1437: Unknown result type (might be due to invalid IL or missing references)
			//IL_144c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1473: Unknown result type (might be due to invalid IL or missing references)
			//IL_1476: Unknown result type (might be due to invalid IL or missing references)
			//IL_1497: Unknown result type (might be due to invalid IL or missing references)
			//IL_149c: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_14be: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_14f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1507: Unknown result type (might be due to invalid IL or missing references)
			//IL_151c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1543: Unknown result type (might be due to invalid IL or missing references)
			//IL_1546: Unknown result type (might be due to invalid IL or missing references)
			//IL_1571: Unknown result type (might be due to invalid IL or missing references)
			//IL_1586: Unknown result type (might be due to invalid IL or missing references)
			//IL_15ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_15af: Unknown result type (might be due to invalid IL or missing references)
			//IL_15ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_15ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_1626: Unknown result type (might be due to invalid IL or missing references)
			//IL_1629: Unknown result type (might be due to invalid IL or missing references)
			//IL_166e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1683: Unknown result type (might be due to invalid IL or missing references)
			//IL_16aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_16ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_16d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_170a: Unknown result type (might be due to invalid IL or missing references)
			//IL_172c: Unknown result type (might be due to invalid IL or missing references)
			//IL_172f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1750: Unknown result type (might be due to invalid IL or missing references)
			//IL_1755: Unknown result type (might be due to invalid IL or missing references)
			//IL_1764: Unknown result type (might be due to invalid IL or missing references)
			//IL_1777: Unknown result type (might be due to invalid IL or missing references)
			//IL_1799: Unknown result type (might be due to invalid IL or missing references)
			//IL_179c: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_17c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_17d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1835: Unknown result type (might be due to invalid IL or missing references)
			//IL_184a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1871: Unknown result type (might be due to invalid IL or missing references)
			//IL_1874: Unknown result type (might be due to invalid IL or missing references)
			//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_18d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_18e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_190f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1912: Unknown result type (might be due to invalid IL or missing references)
			//IL_1946: Unknown result type (might be due to invalid IL or missing references)
			//IL_194f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1971: Unknown result type (might be due to invalid IL or missing references)
			//IL_1986: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_19b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_19d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_19f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_19fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a73: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a76: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a97: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aab: Unknown result type (might be due to invalid IL or missing references)
			//IL_1abe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ae0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ae3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1af2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1af4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b07: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b43: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b45: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b91: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c56: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c59: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ccd: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cf7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d20: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d52: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d74: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d77: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d97: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dac: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dd3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e23: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e71: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e84: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ea6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ea9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ec9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ede: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f05: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f07: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f40: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f55: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fa3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fd8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ffb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2010: Unknown result type (might be due to invalid IL or missing references)
			//IL_2037: Unknown result type (might be due to invalid IL or missing references)
			//IL_2039: Unknown result type (might be due to invalid IL or missing references)
			//IL_2072: Unknown result type (might be due to invalid IL or missing references)
			//IL_2087: Unknown result type (might be due to invalid IL or missing references)
			//IL_20ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_20d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_20ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_210c: Unknown result type (might be due to invalid IL or missing references)
			//IL_210f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2133: Unknown result type (might be due to invalid IL or missing references)
			//IL_2146: Unknown result type (might be due to invalid IL or missing references)
			//IL_2168: Unknown result type (might be due to invalid IL or missing references)
			//IL_216b: Unknown result type (might be due to invalid IL or missing references)
			//IL_218b: Unknown result type (might be due to invalid IL or missing references)
			//IL_21a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_21c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_21c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_21eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2202: Unknown result type (might be due to invalid IL or missing references)
			//IL_2217: Unknown result type (might be due to invalid IL or missing references)
			//IL_223e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2241: Unknown result type (might be due to invalid IL or missing references)
			//IL_2265: Unknown result type (might be due to invalid IL or missing references)
			//IL_2278: Unknown result type (might be due to invalid IL or missing references)
			//IL_229a: Unknown result type (might be due to invalid IL or missing references)
			//IL_229d: Unknown result type (might be due to invalid IL or missing references)
			//IL_22bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_22d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_22f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_22fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2334: Unknown result type (might be due to invalid IL or missing references)
			//IL_2349: Unknown result type (might be due to invalid IL or missing references)
			//IL_2370: Unknown result type (might be due to invalid IL or missing references)
			//IL_2373: Unknown result type (might be due to invalid IL or missing references)
			//IL_23c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_23d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_23e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_2406: Unknown result type (might be due to invalid IL or missing references)
			//IL_2409: Unknown result type (might be due to invalid IL or missing references)
			//IL_242b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2440: Unknown result type (might be due to invalid IL or missing references)
			//IL_2455: Unknown result type (might be due to invalid IL or missing references)
			//IL_2476: Unknown result type (might be due to invalid IL or missing references)
			//IL_2478: Unknown result type (might be due to invalid IL or missing references)
			//IL_249a: Unknown result type (might be due to invalid IL or missing references)
			//IL_24af: Unknown result type (might be due to invalid IL or missing references)
			//IL_24c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_24e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_24e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_2522: Unknown result type (might be due to invalid IL or missing references)
			//IL_2535: Unknown result type (might be due to invalid IL or missing references)
			//IL_2545: Unknown result type (might be due to invalid IL or missing references)
			//IL_2566: Unknown result type (might be due to invalid IL or missing references)
			//IL_2569: Unknown result type (might be due to invalid IL or missing references)
			//IL_258b: Unknown result type (might be due to invalid IL or missing references)
			//IL_25a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_25b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_25d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_25d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_25fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_260f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2624: Unknown result type (might be due to invalid IL or missing references)
			//IL_2645: Unknown result type (might be due to invalid IL or missing references)
			//IL_2648: Unknown result type (might be due to invalid IL or missing references)
			//IL_2686: Unknown result type (might be due to invalid IL or missing references)
			//IL_2699: Unknown result type (might be due to invalid IL or missing references)
			//IL_26a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_26ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_26cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_26ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_2704: Unknown result type (might be due to invalid IL or missing references)
			//IL_2719: Unknown result type (might be due to invalid IL or missing references)
			//IL_273a: Unknown result type (might be due to invalid IL or missing references)
			//IL_273c: Unknown result type (might be due to invalid IL or missing references)
			//IL_275e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2773: Unknown result type (might be due to invalid IL or missing references)
			//IL_2788: Unknown result type (might be due to invalid IL or missing references)
			//IL_27a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_27ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_27e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_27fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_280c: Unknown result type (might be due to invalid IL or missing references)
			//IL_282d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2830: Unknown result type (might be due to invalid IL or missing references)
			//IL_2852: Unknown result type (might be due to invalid IL or missing references)
			//IL_2867: Unknown result type (might be due to invalid IL or missing references)
			//IL_287c: Unknown result type (might be due to invalid IL or missing references)
			//IL_289d: Unknown result type (might be due to invalid IL or missing references)
			//IL_289f: Unknown result type (might be due to invalid IL or missing references)
			//IL_28c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_28d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_28eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_290c: Unknown result type (might be due to invalid IL or missing references)
			//IL_290f: Unknown result type (might be due to invalid IL or missing references)
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
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? playerPos.Z : playerPos.Y).ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? playerPos.Y : playerPos.Z).ToString("0.###");
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
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? ((Coordinates3)(ref playerFacing)).get_Z() : ((Coordinates3)(ref playerFacing)).get_Y()).ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? ((Coordinates3)(ref playerFacing)).get_Y() : ((Coordinates3)(ref playerFacing)).get_Z()).ToString("0.###");
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
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? cameraForward.Z : cameraForward.Y).ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? cameraForward.Y : cameraForward.Z).ToString("0.###");
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
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? cameraPosition.Z : cameraPosition.Y).ToString("0.###");
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = "  " + (MumbleInfoModule.ModuleInstance.SwapYZAxes.get_Value() ? cameraPosition.Y : cameraPosition.Z).ToString("0.###");
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
