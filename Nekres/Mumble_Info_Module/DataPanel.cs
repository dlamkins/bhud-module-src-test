using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Mumble_Info_Module
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

		private const int _strokeDist = 1;

		private const int _borderSize = 1;

		private const int _clipboardIndent = 4;

		private const char _clipboardIndentChar = ' ';

		private const string _clipboardMessage = "Copied!";

		private const string _decimalFormat = "0.###";

		private bool _isMousePressed;

		private (string, bool) _gameInfo;

		private (string, bool) _avatarInfo;

		private (string, bool) _mapInfo;

		private (string, bool) _cameraInfo;

		private (string, bool) _userInterfaceInfo;

		private (string, bool) _computerInfo;

		private Rectangle _currentFocusBounds;

		private string _currentSingleInfo;

		private float _memoryUsage => MumbleInfoModule.ModuleInstance.MemoryUsage;

		private float _cpuUsage => MumbleInfoModule.ModuleInstance.CpuUsage;

		private string _cpuName => MumbleInfoModule.ModuleInstance.CpuName;

		private bool _captureMouseOnLCtrl => MumbleInfoModule.ModuleInstance.CaptureMouseOnLCtrl.get_Value();

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
			((Control)this).add_Disposed((EventHandler<EventArgs>)OnDisposed);
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMousePressed);
		}

		private void OnLeftMouseButtonReleased(object o, MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_isMousePressed = false;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(_currentFocusBounds))
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_currentSingleInfo);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_gameInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_gameInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_avatarInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_avatarInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_mapInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_mapInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_cameraInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_cameraInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_userInterfaceInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_userInterfaceInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_computerInfo.Item2)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_computerInfo.Item1);
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private void OnMousePressed(object o, MouseEventArgs e)
		{
			_isMousePressed = true;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)((_captureMouseOnLCtrl && PInvoke.IsLControlPressed()) ? 4 : 22);
		}

		private void OnDisposed(object sender, EventArgs e)
		{
			Control.get_Input().get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMousePressed);
		}

		private void UpdateLocation(object sender, EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(0, 0));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Unknown result type (might be due to invalid IL or missing references)
			//IL_067b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_0708: Unknown result type (might be due to invalid IL or missing references)
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0799: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_082e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0856: Unknown result type (might be due to invalid IL or missing references)
			//IL_0858: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0947: Unknown result type (might be due to invalid IL or missing references)
			//IL_094c: Unknown result type (might be due to invalid IL or missing references)
			//IL_095c: Unknown result type (might be due to invalid IL or missing references)
			//IL_096d: Unknown result type (might be due to invalid IL or missing references)
			//IL_096f: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
			//IL_1002: Unknown result type (might be due to invalid IL or missing references)
			//IL_102a: Unknown result type (might be due to invalid IL or missing references)
			//IL_102d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1076: Unknown result type (might be due to invalid IL or missing references)
			//IL_107b: Unknown result type (might be due to invalid IL or missing references)
			//IL_108b: Unknown result type (might be due to invalid IL or missing references)
			//IL_109c: Unknown result type (might be due to invalid IL or missing references)
			//IL_109e: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_10de: Unknown result type (might be due to invalid IL or missing references)
			//IL_1101: Unknown result type (might be due to invalid IL or missing references)
			//IL_1104: Unknown result type (might be due to invalid IL or missing references)
			//IL_1114: Unknown result type (might be due to invalid IL or missing references)
			//IL_1116: Unknown result type (might be due to invalid IL or missing references)
			//IL_1149: Unknown result type (might be due to invalid IL or missing references)
			//IL_115f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1187: Unknown result type (might be due to invalid IL or missing references)
			//IL_1189: Unknown result type (might be due to invalid IL or missing references)
			//IL_11eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1201: Unknown result type (might be due to invalid IL or missing references)
			//IL_1229: Unknown result type (might be due to invalid IL or missing references)
			//IL_122c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1298: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1345: Unknown result type (might be due to invalid IL or missing references)
			//IL_135b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1383: Unknown result type (might be due to invalid IL or missing references)
			//IL_1386: Unknown result type (might be due to invalid IL or missing references)
			//IL_13cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_13e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_13f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_13f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1405: Unknown result type (might be due to invalid IL or missing references)
			//IL_1407: Unknown result type (might be due to invalid IL or missing references)
			//IL_1432: Unknown result type (might be due to invalid IL or missing references)
			//IL_1448: Unknown result type (might be due to invalid IL or missing references)
			//IL_1470: Unknown result type (might be due to invalid IL or missing references)
			//IL_1473: Unknown result type (might be due to invalid IL or missing references)
			//IL_1483: Unknown result type (might be due to invalid IL or missing references)
			//IL_1485: Unknown result type (might be due to invalid IL or missing references)
			//IL_14b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_14b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_14c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_14f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1509: Unknown result type (might be due to invalid IL or missing references)
			//IL_152c: Unknown result type (might be due to invalid IL or missing references)
			//IL_152f: Unknown result type (might be due to invalid IL or missing references)
			//IL_153f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1541: Unknown result type (might be due to invalid IL or missing references)
			//IL_1569: Unknown result type (might be due to invalid IL or missing references)
			//IL_156e: Unknown result type (might be due to invalid IL or missing references)
			//IL_158e: Unknown result type (might be due to invalid IL or missing references)
			//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_1602: Unknown result type (might be due to invalid IL or missing references)
			//IL_162a: Unknown result type (might be due to invalid IL or missing references)
			//IL_162d: Unknown result type (might be due to invalid IL or missing references)
			//IL_163d: Unknown result type (might be due to invalid IL or missing references)
			//IL_163f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1681: Unknown result type (might be due to invalid IL or missing references)
			//IL_1686: Unknown result type (might be due to invalid IL or missing references)
			//IL_1696: Unknown result type (might be due to invalid IL or missing references)
			//IL_16a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_16a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_16d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1706: Unknown result type (might be due to invalid IL or missing references)
			//IL_1708: Unknown result type (might be due to invalid IL or missing references)
			//IL_173b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1751: Unknown result type (might be due to invalid IL or missing references)
			//IL_1779: Unknown result type (might be due to invalid IL or missing references)
			//IL_177b: Unknown result type (might be due to invalid IL or missing references)
			//IL_17d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_180d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1810: Unknown result type (might be due to invalid IL or missing references)
			//IL_1866: Unknown result type (might be due to invalid IL or missing references)
			//IL_186b: Unknown result type (might be due to invalid IL or missing references)
			//IL_187b: Unknown result type (might be due to invalid IL or missing references)
			//IL_188c: Unknown result type (might be due to invalid IL or missing references)
			//IL_188e: Unknown result type (might be due to invalid IL or missing references)
			//IL_18a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_18b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18db: Unknown result type (might be due to invalid IL or missing references)
			//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_18ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1920: Unknown result type (might be due to invalid IL or missing references)
			//IL_1936: Unknown result type (might be due to invalid IL or missing references)
			//IL_195e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1960: Unknown result type (might be due to invalid IL or missing references)
			//IL_19bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_19d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_19f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_19fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aa2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ac5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ac8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ad8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ada: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b23: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bec: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c42: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c47: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c57: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c68: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c91: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cfc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d12: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dd4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dfc: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dff: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e55: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e92: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ea1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eb5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ed8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1edb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f20: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f36: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f61: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fad: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1feb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fed: Unknown result type (might be due to invalid IL or missing references)
			//IL_204f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2065: Unknown result type (might be due to invalid IL or missing references)
			//IL_208d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2090: Unknown result type (might be due to invalid IL or missing references)
			//IL_20fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_2112: Unknown result type (might be due to invalid IL or missing references)
			//IL_213a: Unknown result type (might be due to invalid IL or missing references)
			//IL_213d: Unknown result type (might be due to invalid IL or missing references)
			//IL_218b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2190: Unknown result type (might be due to invalid IL or missing references)
			//IL_21a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_21b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_21b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_21d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_21e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2207: Unknown result type (might be due to invalid IL or missing references)
			//IL_220a: Unknown result type (might be due to invalid IL or missing references)
			//IL_221a: Unknown result type (might be due to invalid IL or missing references)
			//IL_221c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2244: Unknown result type (might be due to invalid IL or missing references)
			//IL_2249: Unknown result type (might be due to invalid IL or missing references)
			//IL_2269: Unknown result type (might be due to invalid IL or missing references)
			//IL_2284: Unknown result type (might be due to invalid IL or missing references)
			//IL_2289: Unknown result type (might be due to invalid IL or missing references)
			//IL_2298: Unknown result type (might be due to invalid IL or missing references)
			//IL_22ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_22cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_22d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_22e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_22e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2317: Unknown result type (might be due to invalid IL or missing references)
			//IL_232d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2355: Unknown result type (might be due to invalid IL or missing references)
			//IL_2357: Unknown result type (might be due to invalid IL or missing references)
			//IL_23b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_23cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_23f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_23f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_245e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2474: Unknown result type (might be due to invalid IL or missing references)
			//IL_249c: Unknown result type (might be due to invalid IL or missing references)
			//IL_249f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2507: Unknown result type (might be due to invalid IL or missing references)
			//IL_251d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2545: Unknown result type (might be due to invalid IL or missing references)
			//IL_2548: Unknown result type (might be due to invalid IL or missing references)
			//IL_2591: Unknown result type (might be due to invalid IL or missing references)
			//IL_2596: Unknown result type (might be due to invalid IL or missing references)
			//IL_25a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_25b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_25b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_25c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_25cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_25d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_25df: Unknown result type (might be due to invalid IL or missing references)
			//IL_2613: Unknown result type (might be due to invalid IL or missing references)
			//IL_2629: Unknown result type (might be due to invalid IL or missing references)
			//IL_2651: Unknown result type (might be due to invalid IL or missing references)
			//IL_2654: Unknown result type (might be due to invalid IL or missing references)
			//IL_2664: Unknown result type (might be due to invalid IL or missing references)
			//IL_2666: Unknown result type (might be due to invalid IL or missing references)
			//IL_26a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_26a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_26b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_26c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_26c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_26e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_26e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_26f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2708: Unknown result type (might be due to invalid IL or missing references)
			//IL_272b: Unknown result type (might be due to invalid IL or missing references)
			//IL_272e: Unknown result type (might be due to invalid IL or missing references)
			//IL_273e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2740: Unknown result type (might be due to invalid IL or missing references)
			//IL_2773: Unknown result type (might be due to invalid IL or missing references)
			//IL_2789: Unknown result type (might be due to invalid IL or missing references)
			//IL_27b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_27b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_2811: Unknown result type (might be due to invalid IL or missing references)
			//IL_2827: Unknown result type (might be due to invalid IL or missing references)
			//IL_284f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2852: Unknown result type (might be due to invalid IL or missing references)
			//IL_28ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_28d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_28f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_28fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2963: Unknown result type (might be due to invalid IL or missing references)
			//IL_2979: Unknown result type (might be due to invalid IL or missing references)
			//IL_29a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_29a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_29f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_29f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a35: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a49: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a81: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ab4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2aca: Unknown result type (might be due to invalid IL or missing references)
			//IL_2af2: Unknown result type (might be due to invalid IL or missing references)
			//IL_2af4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2b59: Unknown result type (might be due to invalid IL or missing references)
			//IL_2b6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2b97: Unknown result type (might be due to invalid IL or missing references)
			//IL_2b9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_2be8: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bed: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bfd: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c10: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c62: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c65: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c75: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c77: Unknown result type (might be due to invalid IL or missing references)
			//IL_2caa: Unknown result type (might be due to invalid IL or missing references)
			//IL_2cc0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ce8: Unknown result type (might be due to invalid IL or missing references)
			//IL_2cea: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d65: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d90: Unknown result type (might be due to invalid IL or missing references)
			//IL_2dde: Unknown result type (might be due to invalid IL or missing references)
			//IL_2de3: Unknown result type (might be due to invalid IL or missing references)
			//IL_2df3: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e04: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e06: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e21: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e35: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e58: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ea0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2eb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ede: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ee0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f45: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f83: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f86: Unknown result type (might be due to invalid IL or missing references)
			//IL_2fd4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2fd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_2fe9: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ffa: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ffc: Unknown result type (might be due to invalid IL or missing references)
			//IL_3019: Unknown result type (might be due to invalid IL or missing references)
			//IL_302d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3050: Unknown result type (might be due to invalid IL or missing references)
			//IL_3053: Unknown result type (might be due to invalid IL or missing references)
			//IL_3063: Unknown result type (might be due to invalid IL or missing references)
			//IL_3065: Unknown result type (might be due to invalid IL or missing references)
			//IL_308d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3092: Unknown result type (might be due to invalid IL or missing references)
			//IL_30b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_30d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_30e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3107: Unknown result type (might be due to invalid IL or missing references)
			//IL_310a: Unknown result type (might be due to invalid IL or missing references)
			//IL_311a: Unknown result type (might be due to invalid IL or missing references)
			//IL_311c: Unknown result type (might be due to invalid IL or missing references)
			//IL_314f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3165: Unknown result type (might be due to invalid IL or missing references)
			//IL_318d: Unknown result type (might be due to invalid IL or missing references)
			//IL_318f: Unknown result type (might be due to invalid IL or missing references)
			//IL_31dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_31f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_320a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3232: Unknown result type (might be due to invalid IL or missing references)
			//IL_3235: Unknown result type (might be due to invalid IL or missing references)
			//IL_3283: Unknown result type (might be due to invalid IL or missing references)
			//IL_3288: Unknown result type (might be due to invalid IL or missing references)
			//IL_3298: Unknown result type (might be due to invalid IL or missing references)
			//IL_32a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_32ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_32c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_32da: Unknown result type (might be due to invalid IL or missing references)
			//IL_32fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3300: Unknown result type (might be due to invalid IL or missing references)
			//IL_3310: Unknown result type (might be due to invalid IL or missing references)
			//IL_3312: Unknown result type (might be due to invalid IL or missing references)
			//IL_3345: Unknown result type (might be due to invalid IL or missing references)
			//IL_335b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3383: Unknown result type (might be due to invalid IL or missing references)
			//IL_3385: Unknown result type (might be due to invalid IL or missing references)
			//IL_33ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_3400: Unknown result type (might be due to invalid IL or missing references)
			//IL_3428: Unknown result type (might be due to invalid IL or missing references)
			//IL_342b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3479: Unknown result type (might be due to invalid IL or missing references)
			//IL_347e: Unknown result type (might be due to invalid IL or missing references)
			//IL_348e: Unknown result type (might be due to invalid IL or missing references)
			//IL_349f: Unknown result type (might be due to invalid IL or missing references)
			//IL_34a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_34d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_34ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_34fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3520: Unknown result type (might be due to invalid IL or missing references)
			//IL_3523: Unknown result type (might be due to invalid IL or missing references)
			//IL_3533: Unknown result type (might be due to invalid IL or missing references)
			//IL_3535: Unknown result type (might be due to invalid IL or missing references)
			//IL_355a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3570: Unknown result type (might be due to invalid IL or missing references)
			//IL_3585: Unknown result type (might be due to invalid IL or missing references)
			//IL_35a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_35aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_3600: Unknown result type (might be due to invalid IL or missing references)
			//IL_3616: Unknown result type (might be due to invalid IL or missing references)
			//IL_362b: Unknown result type (might be due to invalid IL or missing references)
			//IL_364e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3651: Unknown result type (might be due to invalid IL or missing references)
			//IL_36ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_36b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_36d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_36e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_36e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_3716: Unknown result type (might be due to invalid IL or missing references)
			//IL_372a: Unknown result type (might be due to invalid IL or missing references)
			//IL_373a: Unknown result type (might be due to invalid IL or missing references)
			//IL_375d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3760: Unknown result type (might be due to invalid IL or missing references)
			//IL_3770: Unknown result type (might be due to invalid IL or missing references)
			//IL_3772: Unknown result type (might be due to invalid IL or missing references)
			//IL_37a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_37bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_37d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_37f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_37f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_384d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3863: Unknown result type (might be due to invalid IL or missing references)
			//IL_3878: Unknown result type (might be due to invalid IL or missing references)
			//IL_389b: Unknown result type (might be due to invalid IL or missing references)
			//IL_389e: Unknown result type (might be due to invalid IL or missing references)
			//IL_38fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_38ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_391f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3930: Unknown result type (might be due to invalid IL or missing references)
			//IL_3932: Unknown result type (might be due to invalid IL or missing references)
			//IL_396d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3981: Unknown result type (might be due to invalid IL or missing references)
			//IL_3991: Unknown result type (might be due to invalid IL or missing references)
			//IL_39b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_39b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_39c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_39c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_39fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a14: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a29: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3aba: Unknown result type (might be due to invalid IL or missing references)
			//IL_3acf: Unknown result type (might be due to invalid IL or missing references)
			//IL_3af2: Unknown result type (might be due to invalid IL or missing references)
			//IL_3af5: Unknown result type (might be due to invalid IL or missing references)
			//IL_3b51: Unknown result type (might be due to invalid IL or missing references)
			//IL_3b56: Unknown result type (might be due to invalid IL or missing references)
			//IL_3b76: Unknown result type (might be due to invalid IL or missing references)
			//IL_3b87: Unknown result type (might be due to invalid IL or missing references)
			//IL_3b89: Unknown result type (might be due to invalid IL or missing references)
			//IL_3bbd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3bd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3be1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c07: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c17: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c19: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c64: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c79: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d42: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d45: Unknown result type (might be due to invalid IL or missing references)
			//IL_3da1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3da6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3dc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3dd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_3dd9: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() || !GameService.Gw2Mumble.get_IsAvailable() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
			bool togglePressed = PInvoke.IsLControlPressed();
			int calcTopMargin = 5;
			int calcLeftMargin = 10;
			string text = GameService.Gw2Mumble.get_RawClient().get_Name() + "  ";
			int width = (int)_font.MeasureString(text).Width;
			int height = (int)_font.MeasureString(text).Height;
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _brown, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			Rectangle infoBounds = rect;
			_gameInfo.Item1 = text;
			string focusedSingleInfo = text;
			calcLeftMargin += width;
			text = $"({GameService.Gw2Mumble.get_Info().get_BuildId()})/";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_gameInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"(Mumble Link v{GameService.Gw2Mumble.get_Info().get_Version()})";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			ref string item = ref _gameInfo.Item1;
			item = item + text + "\n";
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 10;
			text = GameService.Gw2Mumble.get_Info().get_ServerAddress() ?? "";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_Info().get_ServerPort()}  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"- {GameService.Gw2Mumble.get_Info().get_ShardId()}  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"({GameService.Gw2Mumble.get_RawClient().get_Instance()})";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _grey, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height * 2;
			calcLeftMargin = 10;
			text = "Avatar";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_avatarInfo.Item1 = text + "\n";
			_avatarInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_avatarInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = $"{GameService.Gw2Mumble.get_PlayerCharacter().get_Name()} - {GameService.Gw2Mumble.get_PlayerCharacter().get_Race()}";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softRed, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item2 = ref _avatarInfo.Item1;
			item2 = item2 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = $"  ({GameService.Gw2Mumble.get_PlayerCharacter().get_TeamColorId()})";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item3 = ref _avatarInfo.Item1;
			item3 = item3 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Profession";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item4 = ref _avatarInfo.Item1;
			item4 = item4 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_PlayerCharacter().get_Profession()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item5 = ref _avatarInfo.Item1;
			item5 = item5 + text + "\n";
			focusedSingleInfo += text;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			if (_currentSpec != null && _currentSpec.get_Elite() && _currentSpec.get_Id() == GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization())
			{
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = "Elite";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				infoBounds = rect;
				ref string item6 = ref _avatarInfo.Item1;
				item6 = item6 + new string(' ', 4) + text;
				focusedSingleInfo = text;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				_avatarInfo.Item1 += text;
				focusedSingleInfo += text;
				calcLeftMargin += width;
				text = _currentSpec.get_Name() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				_avatarInfo.Item1 += text;
				focusedSingleInfo += text;
				calcLeftMargin += width;
				text = $"  ({_currentSpec.get_Id()})";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				ref string item7 = ref _avatarInfo.Item1;
				item7 = item7 + text + "\n";
				focusedSingleInfo += text;
				if (Control.get_Input().get_Mouse().get_Position()
					.IsInBounds(infoBounds))
				{
					DrawBorder(spriteBatch, infoBounds);
					_currentSingleInfo = focusedSingleInfo;
					_currentFocusBounds = infoBounds;
				}
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			Vector3 playerPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			text = "X";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item8 = ref _avatarInfo.Item1;
			item8 = item8 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = "Y";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "Z";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = playerPos.X.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + playerPos.Y.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + playerPos.Z.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item9 = ref _avatarInfo.Item1;
			item9 = item9 + text + "\n";
			focusedSingleInfo += text;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			Coordinates3 playerFacing = GameService.Gw2Mumble.get_RawClient().get_AvatarFront();
			text = "Facing";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item10 = ref _avatarInfo.Item1;
			item10 = item10 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = ((Coordinates3)(ref playerFacing)).get_X().ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + ((Coordinates3)(ref playerFacing)).get_Y().ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_avatarInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + ((Coordinates3)(ref playerFacing)).get_Z().ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item11 = ref _avatarInfo.Item1;
			item11 = item11 + text + "\n";
			focusedSingleInfo += text;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = DirectionUtil.IsFacing(playerFacing.SwapYZ()).ToString().SplitAtUpperCase()
				.Trim();
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _softYellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item12 = ref _avatarInfo.Item1;
			item12 = item12 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height * 2;
			calcLeftMargin = 10;
			text = "Map";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_mapInfo.Item1 = text + "\n";
			_mapInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_mapInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
			}
			if (_currentMap != null && _currentMap.get_Id() == GameService.Gw2Mumble.get_CurrentMap().get_Id())
			{
				calcTopMargin += height;
				calcLeftMargin = 30;
				text = _currentMap.get_Name() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				infoBounds = rect;
				ref string item13 = ref _mapInfo.Item1;
				item13 = item13 + new string(' ', 4) + text + "\n";
				focusedSingleInfo = text + "\n";
				calcTopMargin += height;
				calcLeftMargin = 30;
				if (Control.get_Input().get_Mouse().get_Position()
					.IsInBounds(infoBounds))
				{
					DrawBorder(spriteBatch, infoBounds);
					_currentSingleInfo = focusedSingleInfo;
					_currentFocusBounds = infoBounds;
				}
				text = "Region";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				infoBounds = rect;
				ref string item14 = ref _mapInfo.Item1;
				item14 = item14 + new string(' ', 4) + text;
				focusedSingleInfo = text;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				_mapInfo.Item1 += text;
				focusedSingleInfo += text;
				calcLeftMargin += width;
				text = _currentMap.get_RegionName() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				ref string item15 = ref _mapInfo.Item1;
				item15 = item15 + text + "\n";
				focusedSingleInfo = focusedSingleInfo + text + "\n";
				calcTopMargin += height;
				calcLeftMargin = 30;
				if (Control.get_Input().get_Mouse().get_Position()
					.IsInBounds(infoBounds))
				{
					DrawBorder(spriteBatch, infoBounds);
					_currentSingleInfo = focusedSingleInfo;
					_currentFocusBounds = infoBounds;
				}
				text = "Continent";
				width = (int)_font.MeasureString(text).Width;
				height = (int)_font.MeasureString(text).Height;
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				infoBounds = rect;
				ref string item16 = ref _mapInfo.Item1;
				item16 = item16 + new string(' ', 4) + text;
				focusedSingleInfo = text;
				calcLeftMargin += width;
				text = ":  ";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				_mapInfo.Item1 += text;
				focusedSingleInfo += text;
				calcLeftMargin += width;
				text = _currentMap.get_ContinentName() ?? "";
				width = (int)_font.MeasureString(text).Width;
				height = Math.Max(height, (int)_font.MeasureString(text).Height);
				((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
				RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
				ref string item17 = ref _mapInfo.Item1;
				item17 = item17 + text + "\n";
				focusedSingleInfo = focusedSingleInfo + text + "\n";
				if (Control.get_Input().get_Mouse().get_Position()
					.IsInBounds(infoBounds))
				{
					DrawBorder(spriteBatch, infoBounds);
					_currentSingleInfo = focusedSingleInfo;
					_currentFocusBounds = infoBounds;
				}
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Id";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item18 = ref _mapInfo.Item1;
			item18 = item18 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_mapInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_CurrentMap().get_Id()}";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item19 = ref _mapInfo.Item1;
			item19 = item19 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			calcTopMargin += height;
			calcLeftMargin = 30;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			text = "Type";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _blue, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item20 = ref _mapInfo.Item1;
			item20 = item20 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_mapInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = string.Format("{0} ({1})", GameService.Gw2Mumble.get_CurrentMap().get_Type(), GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() ? "PvP" : "PvE");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item21 = ref _mapInfo.Item1;
			item21 = item21 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			calcTopMargin += height;
			calcLeftMargin = 30;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			Coordinates2 playerLocationMap = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
			text = "X";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item22 = ref _mapInfo.Item1;
			item22 = item22 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = "Y";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_mapInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_mapInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = ((Coordinates2)(ref playerLocationMap)).get_X().ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_mapInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + ((Coordinates2)(ref playerLocationMap)).get_X().ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item23 = ref _mapInfo.Item1;
			item23 = item23 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height * 2;
			calcLeftMargin = 10;
			text = "Camera";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_cameraInfo.Item1 = text + "\n";
			_cameraInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_cameraInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			Vector3 cameraForward = GameService.Gw2Mumble.get_PlayerCamera().get_Forward();
			text = "Direction";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item24 = ref _cameraInfo.Item1;
			item24 = item24 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = cameraForward.X.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + cameraForward.Y.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + cameraForward.Z.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item25 = ref _cameraInfo.Item1;
			item25 = item25 + text + "\n";
			focusedSingleInfo += text;
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = DirectionUtil.IsFacing(new Coordinates3((double)cameraForward.X, (double)cameraForward.Y, (double)cameraForward.Z)).ToString().SplitAtUpperCase()
				.Trim() ?? "";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item26 = ref _cameraInfo.Item1;
			item26 = item26 + new string(' ', 4) + text + "\n";
			focusedSingleInfo = text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			Vector3 cameraPosition = GameService.Gw2Mumble.get_PlayerCamera().get_Position();
			text = "Position";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item27 = ref _cameraInfo.Item1;
			item27 = item27 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = cameraPosition.X.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _red, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + cameraPosition.Y.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _lemonGreen, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = "  " + cameraPosition.Z.ToString(togglePressed ? null : "0.###");
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item28 = ref _cameraInfo.Item1;
			item28 = item28 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Field of View";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item29 = ref _cameraInfo.Item1;
			item29 = item29 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_FieldOfView()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item30 = ref _cameraInfo.Item1;
			item30 = item30 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Near Plane Render Distance";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			focusedSingleInfo = text;
			ref string item31 = ref _cameraInfo.Item1;
			item31 = item31 + new string(' ', 4) + text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_NearPlaneRenderDistance()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item32 = ref _cameraInfo.Item1;
			item32 = item32 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Far Plane Render Distance";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _green, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			focusedSingleInfo = text;
			ref string item33 = ref _cameraInfo.Item1;
			item33 = item33 + new string(' ', 4) + text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_cameraInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_PlayerCamera().get_FarPlaneRenderDistance()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item34 = ref _cameraInfo.Item1;
			item34 = item34 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height * 2;
			calcLeftMargin = 10;
			text = "User Interface";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_userInterfaceInfo.Item1 = text + "\n";
			_userInterfaceInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_userInterfaceInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Size";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item35 = ref _userInterfaceInfo.Item1;
			item35 = item35 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_userInterfaceInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_UI().get_UISize()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item36 = ref _userInterfaceInfo.Item1;
			item36 = item36 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcLeftMargin = 30;
			text = "Text Input Focused";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			ref string item37 = ref _userInterfaceInfo.Item1;
			item37 = item37 + new string(' ', 4) + text;
			focusedSingleInfo = text;
			calcLeftMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_userInterfaceInfo.Item1 += text;
			focusedSingleInfo += text;
			calcLeftMargin += width;
			text = $"{GameService.Gw2Mumble.get_UI().get_IsTextInputFocused()}";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(calcLeftMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _yellow, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			ref string item38 = ref _userInterfaceInfo.Item1;
			item38 = item38 + text + "\n";
			focusedSingleInfo = focusedSingleInfo + text + "\n";
			if (Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds))
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin = 5;
			int calcRightMargin = 10;
			text = _memoryUsage.ToString(togglePressed ? null : "0.###") + " MB";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_computerInfo.Item1 = text;
			focusedSingleInfo = text;
			calcRightMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo;
			calcRightMargin += width;
			text = "Memory Usage";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = "\n" + text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo + "\n";
			_computerInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_computerInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcRightMargin = 10;
			text = $"{Environment.ProcessorCount}x {_cpuName}";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text;
			calcRightMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo;
			calcRightMargin += width;
			text = "CPU";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = "\n" + text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo + "\n";
			_computerInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_computerInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcRightMargin = 10;
			text = _cpuUsage.ToString(togglePressed ? null : "0.###") + "%";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text;
			calcRightMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo;
			calcRightMargin += width;
			text = "CPU Usage";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = "\n" + text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo + "\n";
			_computerInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_computerInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
			calcTopMargin += height;
			calcRightMargin = 10;
			text = Control.get_Graphics().get_GraphicsDevice().get_Adapter()
				.get_Description() ?? "";
			width = (int)_font.MeasureString(text).Width;
			height = (int)_font.MeasureString(text).Height;
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _cyan, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			infoBounds = rect;
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text;
			calcRightMargin += width;
			text = ":  ";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo;
			calcRightMargin += width;
			text = "GPU";
			width = (int)_font.MeasureString(text).Width;
			height = Math.Max(height, (int)_font.MeasureString(text).Height);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Size().X - width - calcRightMargin, calcTopMargin, width, height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, rect, _orange, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
			RectangleExtensions.Union(ref rect, ref infoBounds, out infoBounds);
			_computerInfo.Item1 = "\n" + text + _computerInfo.Item1;
			focusedSingleInfo = text + focusedSingleInfo + "\n";
			_computerInfo.Item2 = Control.get_Input().get_Mouse().get_Position()
				.IsInBounds(infoBounds);
			if (_computerInfo.Item2)
			{
				DrawBorder(spriteBatch, infoBounds);
				_currentSingleInfo = focusedSingleInfo;
				_currentFocusBounds = infoBounds;
			}
		}

		private void DrawBorder(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y, 1, bounds.Height), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X, bounds.Y + bounds.Height, bounds.Width, 1), _isMousePressed ? _clickColor : _borderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.X + bounds.Width, bounds.Y, 1, bounds.Height), _isMousePressed ? _clickColor : _borderColor);
		}
	}
}
