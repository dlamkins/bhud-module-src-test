using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.MouseCursor.Controls;
using Manlaan.MouseCursor.Models;
using Manlaan.MouseCursor.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.MouseCursor
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public enum ClipMode
		{
			Never,
			Always
		}

		public enum ShowMode
		{
			Never,
			Always,
			Dragging,
			NotDragging
		}

		internal static Module ModuleInstance;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		public static SettingCollection _settingsHidden;

		public static SettingEntry<int> _settingMouseCursorSize;

		public static SettingEntry<float> _settingMouseCursorOpacity;

		public static SettingEntry<string> _settingMouseCursorImage;

		public static SettingEntry<string> _settingMouseCursorColor;

		public static SettingEntry<bool> _settingMouseCursorCameraDrag;

		public static SettingEntry<bool> _settingMouseCursorAboveBlish;

		public static SettingEntry<ShowMode> _settingMouseCursorShow;

		public static SettingEntry<ClipMode> _settingMouseCursorClip;

		public static SettingEntry<ShowMode> _settingMouseCursorShowCombat;

		public static SettingEntry<ClipMode> _settingMouseCursorClipCombat;

		public static SettingEntry<bool> _settingMouseCursorFreezeCursor;

		public static SettingEntry<float> _settingMouseCursorFreezeCursorPeriod;

		public static SettingEntry<bool> _settingMouseCursorLogDebug;

		public static List<MouseFile> _mouseFiles = new List<MouseFile>();

		public static List<Color> _colors = new List<Color>();

		private DrawMouseCursor _mouseImg;

		private TimeSpan _freezeStart;

		private Microsoft.Xna.Framework.Point _freezeStartPoint;

		private bool _freezeCursor;

		private bool _shouldClip;

		private bool _inActionCam;

		private bool _inActionCamChanged;

		private bool _camDragged;

		private bool _camDraggedChanged;

		private bool _cursorVis = true;

		private bool _cursorVisChanged;

		private double _cursorVel;

		private bool _cursorVelChanged;

		private MouseState _lastMouseState;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingMouseCursorImage = settings.DefineSetting<string>("MouseCursorImage", "Circle Cyan.png", (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorColor = settings.DefineSetting<string>("MouseCursorColor", "White0", (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorSize = settings.DefineSetting<int>("MouseCursorSize", 70, (Func<string>)(() => "Size"), (Func<string>)null);
			_settingMouseCursorOpacity = settings.DefineSetting<float>("MouseCursorOpacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)null);
			_settingMouseCursorCameraDrag = settings.DefineSetting<bool>("MouseCursorCameraDrag", false, (Func<string>)(() => "Show When Camera Dragging"), (Func<string>)(() => "Shows the cursor when you move the camera."));
			_settingMouseCursorAboveBlish = settings.DefineSetting<bool>("MouseCursorAboveBlish", false, (Func<string>)(() => "Show Above Blish Windows"), (Func<string>)null);
			_settingMouseCursorShow = settings.DefineSetting<ShowMode>("MouseCursorShow", ShowMode.Never, (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorShowCombat = settings.DefineSetting<ShowMode>("MouseCursorShowCombat", ShowMode.Never, (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorClip = settings.DefineSetting<ClipMode>("MouseCursorClip", ClipMode.Never, (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorClipCombat = settings.DefineSetting<ClipMode>("MouseCursorClipCombat", ClipMode.Never, (Func<string>)(() => ""), (Func<string>)null);
			_settingMouseCursorFreezeCursor = settings.DefineSetting<bool>("MouseCursorCenterAfterDrag", false, (Func<string>)(() => "Freeze Cursor After Dragging"), (Func<string>)null);
			_settingMouseCursorFreezeCursorPeriod = settings.DefineSetting<float>("MouseCursorFreezePeriod", 2f, (Func<string>)(() => ""), (Func<string>)(() => $"{_settingMouseCursorFreezeCursorPeriod.get_Value():0} ms"));
			_settingMouseCursorLogDebug = settings.DefineSetting<bool>("MouseCursorLogDebug", false, (Func<string>)(() => "Log Debug Messages"), (Func<string>)null);
			_settingMouseCursorImage.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseCursorSettingsCursorImageNColor);
			_settingMouseCursorColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseCursorSettingsCursorImageNColor);
			_settingMouseCursorSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateMouseCursorSettingsCursorSize);
			_settingMouseCursorOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateMouseCursorSettingsOpacity);
			_settingMouseCursorAboveBlish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseCursorSettingsAboveBlish);
			SettingComplianceExtensions.SetRange(_settingMouseCursorSize, 0, 300);
			SettingComplianceExtensions.SetRange(_settingMouseCursorOpacity, 0f, 1f);
			SettingComplianceExtensions.SetRange(_settingMouseCursorFreezeCursorPeriod, 1f, 500f);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override void Initialize()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_005d: Expected O, but got Unknown
			_mouseFiles = new List<MouseFile>();
			_colors = new List<Color>();
			foreach (KeyValuePair<string, int[]> color in MouseColors.Colors)
			{
				List<Color> colors = _colors;
				Color val = new Color();
				val.set_Name(color.Key);
				ColorMaterial val2 = new ColorMaterial();
				val2.set_Rgb((IReadOnlyList<int>)color.Value);
				val.set_Cloth(val2);
				colors.Add(val);
			}
			_mouseImg = new DrawMouseCursor();
			((Control)_mouseImg).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		protected override async Task LoadAsync()
		{
			await _003C_003En__0();
			string[] array = new string[23]
			{
				"Circle Blue.png", "Circle Cyan.png", "Circle Green.png", "Circle Magenta.png", "Circle Red.png", "Circle Yellow.png", "Arrow 1.png", "Arrow 2.png", "Arrow 3.png", "Circle 1.png",
				"Circle 2.png", "Circle 3.png", "Circle 4.png", "Circle 5.png", "Circle 6.png", "Circle 7.png", "Circle 8.png", "Cross 1.png", "Cross 2.png", "Cross 3.png",
				"Cross 4.png", "Cross 5.png", "mouse.psd"
			};
			foreach (string file in array)
			{
				ExtractFile(file);
			}
			string dailiesDirectory = DirectoriesManager.GetFullDirectoryPath("mousecursor");
			array = Directory.GetFiles(dailiesDirectory, ".");
			foreach (string file2 in array)
			{
				if (file2.ToLower().Contains(".png"))
				{
					_mouseFiles.Add(new MouseFile
					{
						File = file2,
						Name = file2.Substring(dailiesDirectory.Length + 1)
					});
				}
			}
			_mouseFiles.Sort(delegate(MouseFile x, MouseFile y)
			{
				if (x.Name == null && y.Name == null)
				{
					return 0;
				}
				if (x.Name == null)
				{
					return -1;
				}
				return (y.Name == null) ? 1 : x.Name.CompareTo(y.Name);
			});
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			UpdateMouseCursorSettingsCursorSize();
			UpdateMouseCursorSettingsOpacity();
			UpdateMouseCursorSettingsAboveBlish();
			UpdateMouseCursorSettingsCursorImageNColor();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			LogDebug("==============================================================================");
			UpdateCursorState(gameTime);
			UpdateCursorClipping();
			UpdateCursorFreeze(gameTime);
			UpdateCursorImg();
			_lastMouseState = Mouse.GetState();
			LogDebug("======================================END=====================================");
		}

		private void UpdateCursorState(GameTime gt)
		{
			bool cursorVis = GameService.Input.get_Mouse().get_CursorIsVisible();
			_cursorVisChanged = _cursorVis != cursorVis;
			_cursorVis = cursorVis;
			bool camDragged = !cursorVis && !_inActionCam && (Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed);
			_camDraggedChanged = _camDragged != camDragged;
			_camDragged = camDragged;
			bool inActionCam = !cursorVis && !_camDragged && WinApi.GetClientRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle()).GetValueOrDefault().Contains(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
			_inActionCamChanged = _inActionCam != inActionCam;
			_inActionCam = inActionCam;
			double cursorVel = (double)(Mouse.GetState().Position.ToVector2() - _lastMouseState.Position.ToVector2()).Length() / gt.ElapsedGameTime.TotalSeconds;
			_cursorVelChanged = cursorVel - _cursorVel < 1E-09;
			_cursorVel = cursorVel;
			LogDebug($"_cursorVisChanged         {_cursorVisChanged}");
			LogDebug($"_cursorVis                {_cursorVis}");
			LogDebug($"_camDraggedChanged        {_camDraggedChanged}");
			LogDebug($"_camDragged               {_camDragged}");
			LogDebug($"_inActionCamChanged       {_inActionCamChanged}");
			LogDebug($"_inActionCam              {_inActionCam}");
			LogDebug($"WForms.Cursor.Clip        {Cursor.Clip}");
			LogDebug($"clientToScr               {WinApi.ClientToScreen(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle())}");
			LogDebug($"clientRect                {WinApi.GetClientRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle())}");
			LogDebug($"clientWindowRect          {GameService.Graphics.get_WindowWidth()};{GameService.Graphics.get_WindowHeight()}");
		}

		private void UpdateCursorImg()
		{
			((Control)_mouseImg).set_Visible(GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !_inActionCam);
			((Control)_mouseImg).set_Visible(((Control)_mouseImg).get_Visible() && ((!GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && (_settingMouseCursorShow.get_Value() == ShowMode.Always || (_settingMouseCursorShow.get_Value() == ShowMode.Dragging && _camDragged) || (_settingMouseCursorShow.get_Value() == ShowMode.NotDragging && !_camDragged))) || (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && (_settingMouseCursorShowCombat.get_Value() == ShowMode.Always || (_settingMouseCursorShowCombat.get_Value() == ShowMode.Dragging && _camDragged) || (_settingMouseCursorShowCombat.get_Value() == ShowMode.NotDragging && !_camDragged)))));
			WinApi.GetClientRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
			if (_cursorVis)
			{
				((Control)_mouseImg).set_Location(new Microsoft.Xna.Framework.Point(Clamp(GameService.Input.get_Mouse().get_Position().X - _settingMouseCursorSize.get_Value() / 2, -_settingMouseCursorSize.get_Value() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X - _settingMouseCursorSize.get_Value() / 2), Clamp(GameService.Input.get_Mouse().get_Position().Y - _settingMouseCursorSize.get_Value() / 2, -_settingMouseCursorSize.get_Value() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y - _settingMouseCursorSize.get_Value() / 2)));
			}
			LogDebug($"Mouse.GetState().Position  {Mouse.GetState().Position}");
			LogDebug($"Input.Mouse.Position       {GameService.Input.get_Mouse().get_Position()}");
			LogDebug($"_mouseImg.Location         {((Control)_mouseImg).get_Location()}");
		}

		private void UpdateCursorFreeze(GameTime gameTime)
		{
			_freezeCursor = (((_camDraggedChanged && !_camDragged && !_inActionCam) || (_inActionCamChanged && !_inActionCam)) ? _settingMouseCursorFreezeCursor.get_Value() : _freezeCursor);
			_freezeStart = (((_camDraggedChanged && !_camDragged && !_inActionCam) || (_inActionCamChanged && !_inActionCam)) ? gameTime.TotalGameTime : _freezeStart);
			_freezeStartPoint = (((_camDraggedChanged && _camDragged && !_inActionCamChanged) || (_inActionCamChanged && _inActionCam && !_camDraggedChanged)) ? new Microsoft.Xna.Framework.Point(Mouse.GetState().Position.X, Mouse.GetState().Position.Y) : _freezeStartPoint);
			LogDebug($"_freezeCursor              {_freezeCursor}");
			LogDebug($"updateFreezeStartPoint     {(_camDraggedChanged && _camDragged && !_inActionCamChanged) || (_inActionCamChanged && _inActionCam && !_camDraggedChanged)}");
			LogDebug($"_freezeStartPoint          {_freezeStartPoint}");
			LogDebug($"_settingMouseCursorSize    {_settingMouseCursorSize.get_Value()}");
			if (_freezeCursor)
			{
				double frozenFor = gameTime.TotalGameTime.Subtract(_freezeStart).TotalMilliseconds;
				if (frozenFor > (double)_settingMouseCursorFreezeCursorPeriod.get_Value() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || !GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
				{
					_freezeCursor = false;
				}
				System.Drawing.Point? clientToScr = WinApi.ClientToScreen(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
				System.Drawing.Rectangle? clientRect = WinApi.GetClientRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
				Cursor.Position = (_freezeCursor ? Cursor.Clip.Location : Cursor.Position);
				Cursor.Clip = new System.Drawing.Rectangle(_freezeCursor ? (clientToScr.GetValueOrDefault().X + _freezeStartPoint.X) : (_shouldClip ? clientToScr.GetValueOrDefault().X : 0), _freezeCursor ? (clientToScr.GetValueOrDefault().Y + _freezeStartPoint.Y) : (_shouldClip ? clientToScr.GetValueOrDefault().Y : 0), _freezeCursor ? 1 : (_shouldClip ? clientRect.GetValueOrDefault().Width : 0), _freezeCursor ? 1 : (_shouldClip ? clientRect.GetValueOrDefault().Height : 0));
				LogDebug($"   CurrentFreezeTime       {frozenFor}");
				LogDebug($"   _freezeCursor           {_freezeCursor}");
				LogDebug($"   WForms.Cursor.Clip      {Cursor.Clip}");
			}
		}

		private void UpdateCursorClipping()
		{
			System.Drawing.Rectangle? clientRect = WinApi.GetClientRect(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
			System.Drawing.Point? clientToScr = WinApi.ClientToScreen(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
			bool shouldClip = !_freezeCursor && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && (_inActionCam || _camDragged || (!GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && _settingMouseCursorClip.get_Value() == ClipMode.Always) || (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && _settingMouseCursorClipCombat.get_Value() == ClipMode.Always));
			clientRect.GetValueOrDefault().Contains(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
			bool num = _shouldClip != shouldClip;
			_shouldClip = shouldClip;
			if (num || (_cursorVisChanged && _cursorVis))
			{
				LogDebug($"    Clip? {_shouldClip}");
				Cursor.Clip = new System.Drawing.Rectangle(_shouldClip ? clientToScr.GetValueOrDefault().X : 0, _shouldClip ? clientToScr.GetValueOrDefault().Y : 0, _shouldClip ? clientRect.GetValueOrDefault().Width : 0, _shouldClip ? clientRect.GetValueOrDefault().Height : 0);
				LogDebug($"    WForms.Cursor.Clip {Cursor.Clip}");
			}
			LogDebug($"End Setting cursor clip to {Cursor.Clip}");
		}

		protected override void Unload()
		{
			_settingMouseCursorImage.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseCursorSettingsCursorImageNColor);
			_settingMouseCursorColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseCursorSettingsCursorImageNColor);
			_settingMouseCursorSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateMouseCursorSettingsCursorSize);
			_settingMouseCursorOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateMouseCursorSettingsOpacity);
			_settingMouseCursorAboveBlish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseCursorSettingsAboveBlish);
			Cursor.Clip = default(System.Drawing.Rectangle);
			DrawMouseCursor mouseImg = _mouseImg;
			if (mouseImg != null)
			{
				((Control)mouseImg).Dispose();
			}
			_mouseFiles = null;
			_colors = null;
			ModuleInstance = null;
		}

		private static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			return value;
		}

		private void UpdateMouseCursorSettingsCursorSize(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			((Control)_mouseImg).set_Size(new Microsoft.Xna.Framework.Point(_settingMouseCursorSize.get_Value(), _settingMouseCursorSize.get_Value()));
		}

		private void UpdateMouseCursorSettingsOpacity(object sender = null, ValueChangedEventArgs<float> e = null)
		{
			((Control)_mouseImg).set_Opacity(_settingMouseCursorOpacity.get_Value());
		}

		private void UpdateMouseCursorSettingsCursorImageNColor(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			MouseFile mouseFile = _mouseFiles.Find((MouseFile x) => x.Name.Equals(_settingMouseCursorImage.get_Value()));
			if (mouseFile == null || string.IsNullOrEmpty(mouseFile.File) || !File.Exists(mouseFile.File))
			{
				_mouseImg.Texture = ContentsManager.GetTexture("Circle Cyan.png");
			}
			else
			{
				GraphicsDeviceContext gd = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					_mouseImg.Texture = PremultiplyTexture(mouseFile.File, ((GraphicsDeviceContext)(ref gd)).get_GraphicsDevice());
				}
				finally
				{
					((GraphicsDeviceContext)(ref gd)).Dispose();
				}
			}
			_mouseImg.Tint = ToRGB(_colors.Find((Color x) => x.get_Name().Equals(_settingMouseCursorColor.get_Value())));
		}

		private void UpdateMouseCursorSettingsAboveBlish(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_mouseImg.AboveBlish = _settingMouseCursorAboveBlish.get_Value();
		}

		private void ExtractFile(string filePath)
		{
			string fullPath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("mousecursor"), filePath);
			using Stream fs = ContentsManager.GetFileStream(filePath);
			fs.Position = 0L;
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int)fs.Length);
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			File.WriteAllBytes(fullPath, buffer);
		}

		private Texture2D PremultiplyTexture(string FilePath, GraphicsDevice device)
		{
			try
			{
				FileStream titleStream = File.OpenRead(FilePath);
				Texture2D texture = Texture2D.FromStream(device, titleStream);
				titleStream.Close();
				Microsoft.Xna.Framework.Color[] buffer = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];
				texture.GetData(buffer);
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
				}
				texture.SetData(buffer);
				return texture;
			}
			catch
			{
				return ContentsManager.GetTexture("Circle Cyan.png");
			}
		}

		private Microsoft.Xna.Framework.Color ToRGB(Color color)
		{
			if (color == null)
			{
				return new Microsoft.Xna.Framework.Color(255, 255, 255);
			}
			return new Microsoft.Xna.Framework.Color(color.get_Cloth().get_Rgb()[0], color.get_Cloth().get_Rgb()[1], color.get_Cloth().get_Rgb()[2]);
		}

		private void LogDebug(string msg)
		{
			if (_settingMouseCursorLogDebug.get_Value())
			{
				Logger.Debug(msg);
			}
		}

		private void LogDebug(string msg, params object[] args)
		{
			if (_settingMouseCursorLogDebug.get_Value())
			{
				Logger.Debug(msg, args);
			}
		}

		private void LogDebug(Exception ex, string msg)
		{
			if (_settingMouseCursorLogDebug.get_Value())
			{
				Logger.Debug(ex, msg);
			}
		}

		private void LogDebug(Exception ex, string msg, params object[] args)
		{
			if (_settingMouseCursorLogDebug.get_Value())
			{
				Logger.Debug(ex, msg, args);
			}
		}
	}
}
