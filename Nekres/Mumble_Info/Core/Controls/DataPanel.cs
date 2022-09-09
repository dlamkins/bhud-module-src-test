using System;
using System.Globalization;
using AsyncWindowsClipboard;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Mumble_Info.Core.Controls
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

		private Rectangle _mapHashBounds;

		private bool _mouseOverMapHashBounds;

		private float _memoryUsage => MumbleInfoModule.Instance.MemoryUsage;

		private float _cpuUsage => MumbleInfoModule.Instance.CpuUsage;

		private string _cpuName => MumbleInfoModule.Instance.CpuName;

		private Map _currentMap => MumbleInfoModule.Instance.CurrentMap;

		private Specialization _currentSpec => MumbleInfoModule.Instance.CurrentSpec;

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
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			Point relPos = ((Control)this).get_RelativeMousePosition();
			_mouseOverAvatarFacing = ((Rectangle)(ref _avatarFacingBounds)).Contains(relPos);
			_mouseOverAvatarPosition = ((Rectangle)(ref _avatarPositionBounds)).Contains(relPos);
			_mouseOverMapCoordinates = ((Rectangle)(ref _mapCoordinatesBounds)).Contains(relPos);
			_mouseOverCameraDirection = ((Rectangle)(ref _cameraDirectionBounds)).Contains(relPos);
			_mouseOverCameraPosition = ((Rectangle)(ref _cameraPositionBounds)).Contains(relPos);
			_mouseOverMapHashBounds = ((Rectangle)(ref _mapHashBounds)).Contains(relPos);
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
				windowsClipboardService2.SetTextAsync($"xpos=\"{val.X.ToString(CultureInfo.InvariantCulture)}\" ypos=\"{(MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z : GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y).ToString(CultureInfo.InvariantCulture)}\" zpos=\"{(MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y : GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z).ToString(CultureInfo.InvariantCulture)}\"");
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
				windowsClipboardService5.SetTextAsync($"xpos=\"{val.X.ToString(CultureInfo.InvariantCulture)}\" ypos=\"{(MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z : GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y).ToString(CultureInfo.InvariantCulture)}\" zpos=\"{(MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y : GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z).ToString(CultureInfo.InvariantCulture)}\"");
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mouseOverMapHashBounds)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync($"\"{_currentMap.GetHash()}\": {_currentMap.get_Id()}, // {_currentMap.get_Name()} ({_currentMap.get_Id()})");
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
			//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
			//IL_1021: Unknown result type (might be due to invalid IL or missing references)
			//IL_1024: Unknown result type (might be due to invalid IL or missing references)
			//IL_1048: Unknown result type (might be due to invalid IL or missing references)
			//IL_105b: Unknown result type (might be due to invalid IL or missing references)
			//IL_107d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1080: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_10de: Unknown result type (might be due to invalid IL or missing references)
			//IL_110d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1120: Unknown result type (might be due to invalid IL or missing references)
			//IL_1142: Unknown result type (might be due to invalid IL or missing references)
			//IL_1145: Unknown result type (might be due to invalid IL or missing references)
			//IL_1169: Unknown result type (might be due to invalid IL or missing references)
			//IL_117c: Unknown result type (might be due to invalid IL or missing references)
			//IL_119e: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_122e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1243: Unknown result type (might be due to invalid IL or missing references)
			//IL_126a: Unknown result type (might be due to invalid IL or missing references)
			//IL_126d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1291: Unknown result type (might be due to invalid IL or missing references)
			//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_12da: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1302: Unknown result type (might be due to invalid IL or missing references)
			//IL_1329: Unknown result type (might be due to invalid IL or missing references)
			//IL_132b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1365: Unknown result type (might be due to invalid IL or missing references)
			//IL_137a: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_1421: Unknown result type (might be due to invalid IL or missing references)
			//IL_1424: Unknown result type (might be due to invalid IL or missing references)
			//IL_1444: Unknown result type (might be due to invalid IL or missing references)
			//IL_1459: Unknown result type (might be due to invalid IL or missing references)
			//IL_1480: Unknown result type (might be due to invalid IL or missing references)
			//IL_1482: Unknown result type (might be due to invalid IL or missing references)
			//IL_14a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1514: Unknown result type (might be due to invalid IL or missing references)
			//IL_1517: Unknown result type (might be due to invalid IL or missing references)
			//IL_1538: Unknown result type (might be due to invalid IL or missing references)
			//IL_153d: Unknown result type (might be due to invalid IL or missing references)
			//IL_154c: Unknown result type (might be due to invalid IL or missing references)
			//IL_155f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1581: Unknown result type (might be due to invalid IL or missing references)
			//IL_1584: Unknown result type (might be due to invalid IL or missing references)
			//IL_1593: Unknown result type (might be due to invalid IL or missing references)
			//IL_1595: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_15e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_15e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1612: Unknown result type (might be due to invalid IL or missing references)
			//IL_1627: Unknown result type (might be due to invalid IL or missing references)
			//IL_164e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1650: Unknown result type (might be due to invalid IL or missing references)
			//IL_168b: Unknown result type (might be due to invalid IL or missing references)
			//IL_16a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_16c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_16ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_170f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1724: Unknown result type (might be due to invalid IL or missing references)
			//IL_174b: Unknown result type (might be due to invalid IL or missing references)
			//IL_174e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1777: Unknown result type (might be due to invalid IL or missing references)
			//IL_1798: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_17d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_17f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_17f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1805: Unknown result type (might be due to invalid IL or missing references)
			//IL_1818: Unknown result type (might be due to invalid IL or missing references)
			//IL_183a: Unknown result type (might be due to invalid IL or missing references)
			//IL_183d: Unknown result type (might be due to invalid IL or missing references)
			//IL_184c: Unknown result type (might be due to invalid IL or missing references)
			//IL_184e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1861: Unknown result type (might be due to invalid IL or missing references)
			//IL_1876: Unknown result type (might be due to invalid IL or missing references)
			//IL_189d: Unknown result type (might be due to invalid IL or missing references)
			//IL_189f: Unknown result type (might be due to invalid IL or missing references)
			//IL_18d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1912: Unknown result type (might be due to invalid IL or missing references)
			//IL_1915: Unknown result type (might be due to invalid IL or missing references)
			//IL_1949: Unknown result type (might be due to invalid IL or missing references)
			//IL_1952: Unknown result type (might be due to invalid IL or missing references)
			//IL_1974: Unknown result type (might be due to invalid IL or missing references)
			//IL_1989: Unknown result type (might be due to invalid IL or missing references)
			//IL_19b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_19b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_19e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_19f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a27: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a51: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a94: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ad8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b17: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b38: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b81: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b84: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b93: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b95: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ba8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bbd: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c32: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c59: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c90: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c99: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cf7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cfa: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d37: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d59: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d95: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d98: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1de0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1df3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e15: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e18: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e38: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e76: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ec4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eee: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f12: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f25: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f47: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fa6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fe1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ff6: Unknown result type (might be due to invalid IL or missing references)
			//IL_201d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2020: Unknown result type (might be due to invalid IL or missing references)
			//IL_2044: Unknown result type (might be due to invalid IL or missing references)
			//IL_2057: Unknown result type (might be due to invalid IL or missing references)
			//IL_2079: Unknown result type (might be due to invalid IL or missing references)
			//IL_207c: Unknown result type (might be due to invalid IL or missing references)
			//IL_209c: Unknown result type (might be due to invalid IL or missing references)
			//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_20d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_20da: Unknown result type (might be due to invalid IL or missing references)
			//IL_2113: Unknown result type (might be due to invalid IL or missing references)
			//IL_2128: Unknown result type (might be due to invalid IL or missing references)
			//IL_214f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2152: Unknown result type (might be due to invalid IL or missing references)
			//IL_2178: Unknown result type (might be due to invalid IL or missing references)
			//IL_218b: Unknown result type (might be due to invalid IL or missing references)
			//IL_21ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_21b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_21d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_2209: Unknown result type (might be due to invalid IL or missing references)
			//IL_220c: Unknown result type (might be due to invalid IL or missing references)
			//IL_222c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2241: Unknown result type (might be due to invalid IL or missing references)
			//IL_2268: Unknown result type (might be due to invalid IL or missing references)
			//IL_226a: Unknown result type (might be due to invalid IL or missing references)
			//IL_228c: Unknown result type (might be due to invalid IL or missing references)
			//IL_22a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_22b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_22df: Unknown result type (might be due to invalid IL or missing references)
			//IL_22e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_2306: Unknown result type (might be due to invalid IL or missing references)
			//IL_2319: Unknown result type (might be due to invalid IL or missing references)
			//IL_233b: Unknown result type (might be due to invalid IL or missing references)
			//IL_233e: Unknown result type (might be due to invalid IL or missing references)
			//IL_235e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2373: Unknown result type (might be due to invalid IL or missing references)
			//IL_239a: Unknown result type (might be due to invalid IL or missing references)
			//IL_239c: Unknown result type (might be due to invalid IL or missing references)
			//IL_23d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_23ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_2411: Unknown result type (might be due to invalid IL or missing references)
			//IL_2414: Unknown result type (might be due to invalid IL or missing references)
			//IL_2463: Unknown result type (might be due to invalid IL or missing references)
			//IL_2476: Unknown result type (might be due to invalid IL or missing references)
			//IL_2486: Unknown result type (might be due to invalid IL or missing references)
			//IL_24a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_24aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_24cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_24e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_24f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_2517: Unknown result type (might be due to invalid IL or missing references)
			//IL_2519: Unknown result type (might be due to invalid IL or missing references)
			//IL_253b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2550: Unknown result type (might be due to invalid IL or missing references)
			//IL_2565: Unknown result type (might be due to invalid IL or missing references)
			//IL_2586: Unknown result type (might be due to invalid IL or missing references)
			//IL_2589: Unknown result type (might be due to invalid IL or missing references)
			//IL_25c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_25d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_25e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_2607: Unknown result type (might be due to invalid IL or missing references)
			//IL_260a: Unknown result type (might be due to invalid IL or missing references)
			//IL_262c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2641: Unknown result type (might be due to invalid IL or missing references)
			//IL_2656: Unknown result type (might be due to invalid IL or missing references)
			//IL_2677: Unknown result type (might be due to invalid IL or missing references)
			//IL_2679: Unknown result type (might be due to invalid IL or missing references)
			//IL_269b: Unknown result type (might be due to invalid IL or missing references)
			//IL_26b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_26c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_26e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_26e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_2727: Unknown result type (might be due to invalid IL or missing references)
			//IL_273a: Unknown result type (might be due to invalid IL or missing references)
			//IL_274a: Unknown result type (might be due to invalid IL or missing references)
			//IL_276b: Unknown result type (might be due to invalid IL or missing references)
			//IL_276e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2790: Unknown result type (might be due to invalid IL or missing references)
			//IL_27a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_27ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_27db: Unknown result type (might be due to invalid IL or missing references)
			//IL_27dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_27ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_2814: Unknown result type (might be due to invalid IL or missing references)
			//IL_2829: Unknown result type (might be due to invalid IL or missing references)
			//IL_284a: Unknown result type (might be due to invalid IL or missing references)
			//IL_284d: Unknown result type (might be due to invalid IL or missing references)
			//IL_288a: Unknown result type (might be due to invalid IL or missing references)
			//IL_289d: Unknown result type (might be due to invalid IL or missing references)
			//IL_28ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_28ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_28d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_28f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_2908: Unknown result type (might be due to invalid IL or missing references)
			//IL_291d: Unknown result type (might be due to invalid IL or missing references)
			//IL_293e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2940: Unknown result type (might be due to invalid IL or missing references)
			//IL_2962: Unknown result type (might be due to invalid IL or missing references)
			//IL_2977: Unknown result type (might be due to invalid IL or missing references)
			//IL_298c: Unknown result type (might be due to invalid IL or missing references)
			//IL_29ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_29b0: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() || !GameService.Gw2Mumble.get_IsAvailable() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
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
			text = $" ({GameService.Gw2Mumble.get_PlayerCharacter().get_TeamColorId()})";
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
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? playerPos.Z : playerPos.Y).ToString("0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			calcLeftMargin += width;
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? playerPos.Y : playerPos.Z).ToString("0.###");
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
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? ((Coordinates3)(ref playerFacing)).get_Z() : ((Coordinates3)(ref playerFacing)).get_Y()).ToString("0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			calcLeftMargin += width;
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? ((Coordinates3)(ref playerFacing)).get_Y() : ((Coordinates3)(ref playerFacing)).get_Z()).ToString("0.###");
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
				calcLeftMargin += width;
				text = $" ({GameService.Gw2Mumble.get_CurrentMap().get_Id()})";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
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
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Hash";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				infoBounds = rect;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				calcLeftMargin += width;
				text = _currentMap.GetHash() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
				RectangleExtensions.Union(ref rect, ref infoBounds, out _mapHashBounds);
				if (_mouseOverMapHashBounds)
				{
					DrawBorder(spriteBatch, _mapHashBounds);
				}
			}
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
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? cameraForward.Z : cameraForward.Y).ToString("0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			calcLeftMargin += width;
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? cameraForward.Y : cameraForward.Z).ToString("0.###");
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
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? cameraPosition.Z : cameraPosition.Y).ToString("0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			calcLeftMargin += width;
			text = "  " + (MumbleInfoModule.Instance.SwapYZAxes.get_Value() ? cameraPosition.Y : cameraPosition.Z).ToString("0.###");
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
			if (MumbleInfoModule.Instance.EnablePerformanceCounters.get_Value())
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
