using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
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
		internal static Module ModuleInstance;

		public static SettingCollection _settingsHidden;

		public static SettingEntry<int> _settingMouseCursorSize;

		public static SettingEntry<float> _settingMouseCursorOpacity;

		public static SettingEntry<string> _settingMouseCursorImage;

		public static SettingEntry<string> _settingMouseCursorColor;

		public static SettingEntry<bool> _settingMouseCursorCameraDrag;

		public static SettingEntry<bool> _settingMouseCursorAboveBlish;

		public static SettingEntry<bool> _settingMouseCursorOnlyCombat;

		private DrawMouseCursor _mouseImg;

		private Point _mousePos = new Point(0, 0);

		private bool _inActionCam;

		public static List<MouseFile> _mouseFiles = new List<MouseFile>();

		public static List<Color> _colors = new List<Color>();

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
			_settingMouseCursorImage = settings.DefineSetting<string>("MouseCursorImage", "Circle Cyan.png", (Func<string>)null, (Func<string>)null);
			_settingMouseCursorColor = settings.DefineSetting<string>("MouseCursorColor", "White0", (Func<string>)null, (Func<string>)null);
			_settingMouseCursorSize = settings.DefineSetting<int>("MouseCursorSize", 70, (Func<string>)(() => "Size"), (Func<string>)(() => ""));
			_settingMouseCursorOpacity = settings.DefineSetting<float>("MouseCursorOpacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)(() => ""));
			_settingMouseCursorCameraDrag = settings.DefineSetting<bool>("MouseCursorCameraDrag", false, (Func<string>)(() => "Show When Camera Dragging"), (Func<string>)(() => "Shows the cursor when you move the camera."));
			_settingMouseCursorAboveBlish = settings.DefineSetting<bool>("MouseCursorAboveBlish", false, (Func<string>)(() => "Show Above Blish Windows"), (Func<string>)(() => ""));
			_settingMouseCursorOnlyCombat = settings.DefineSetting<bool>("MouseCursorOnlyCombat", false, (Func<string>)(() => "Only Show During Combat"), (Func<string>)(() => ""));
			_settingMouseCursorImage.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseSettings_string);
			_settingMouseCursorColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseSettings_string);
			_settingMouseCursorSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateMouseSettings_int);
			_settingMouseCursorOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateMouseSettings_float);
			_settingMouseCursorCameraDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			_settingMouseCursorAboveBlish.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			_settingMouseCursorOnlyCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			SettingComplianceExtensions.SetRange(_settingMouseCursorSize, 0, 300);
			SettingComplianceExtensions.SetRange(_settingMouseCursorOpacity, 0f, 1f);
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
			UpdateMouseSettings_int();
			UpdateMouseSettings_float();
			UpdateMouseSettings_bool();
			UpdateMouseSettings_string();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			bool lrBtnPressed = GameService.Input.get_Mouse().get_CameraDragging() || GameService.Input.get_Mouse().get_State().LeftButton == ButtonState.Pressed;
			bool camIsDragged = !GameService.Input.get_Mouse().get_CursorIsVisible() || lrBtnPressed;
			if (!_inActionCam && !GameService.Input.get_Mouse().get_CursorIsVisible() && !lrBtnPressed)
			{
				_inActionCam = true;
			}
			if (GameService.Input.get_Mouse().get_CursorIsVisible())
			{
				_inActionCam = false;
			}
			GameService.Debug.get_OverlayTexts().TryAdd("RightButton.Pressed ", (Func<GameTime, string>)((GameTime _) => "RightButton     " + ((GameService.Input.get_Mouse().get_State().RightButton == ButtonState.Pressed) ? "Yes" : "No")));
			GameService.Debug.get_OverlayTexts().TryAdd("LeftButton.Pressed  ", (Func<GameTime, string>)((GameTime _) => "LeftButton      " + ((GameService.Input.get_Mouse().get_State().LeftButton == ButtonState.Pressed) ? "Yes" : "No")));
			GameService.Debug.get_OverlayTexts().TryAdd("CursorIsVisible     ", (Func<GameTime, string>)((GameTime _) => "CursorIsVisible " + (GameService.Input.get_Mouse().get_CursorIsVisible() ? "Yes" : "No")));
			GameService.Debug.get_OverlayTexts().TryAdd("CameraDragging      ", (Func<GameTime, string>)((GameTime _) => "CameraDragging  " + (GameService.Input.get_Mouse().get_CameraDragging() ? "Yes" : "No")));
			GameService.Debug.get_OverlayTexts().TryAdd("InActionCam         ", (Func<GameTime, string>)((GameTime _) => "InActionCam     " + (_inActionCam ? "Yes" : "No")));
			((Control)_mouseImg).set_Visible(GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() && !_inActionCam);
			((Control)_mouseImg).set_Visible(((Control)_mouseImg).get_Visible() && (!_settingMouseCursorOnlyCombat.get_Value() || GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat()));
			((Control)_mouseImg).set_Visible(((Control)_mouseImg).get_Visible() && (_settingMouseCursorCameraDrag.get_Value() || !camIsDragged));
			if (GameService.Input.get_Mouse().get_CursorIsVisible())
			{
				_mousePos.X = GameService.Input.get_Mouse().get_Position().X;
				_mousePos.Y = GameService.Input.get_Mouse().get_Position().Y;
			}
			((Control)_mouseImg).set_Location(new Point(_mousePos.X - _settingMouseCursorSize.get_Value() / 2, _mousePos.Y - _settingMouseCursorSize.get_Value() / 2));
		}

		protected override void Unload()
		{
			_settingMouseCursorImage.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseSettings_string);
			_settingMouseCursorColor.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateMouseSettings_string);
			_settingMouseCursorSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)UpdateMouseSettings_int);
			_settingMouseCursorOpacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateMouseSettings_float);
			_settingMouseCursorCameraDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			_settingMouseCursorAboveBlish.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			_settingMouseCursorOnlyCombat.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateMouseSettings_bool);
			DrawMouseCursor mouseImg = _mouseImg;
			if (mouseImg != null)
			{
				((Control)mouseImg).Dispose();
			}
			_mouseFiles = null;
			_colors = null;
			ModuleInstance = null;
		}

		private void UpdateMouseSettings_int(object sender = null, ValueChangedEventArgs<int> e = null)
		{
			((Control)_mouseImg).set_Size(new Point(_settingMouseCursorSize.get_Value(), _settingMouseCursorSize.get_Value()));
		}

		private void UpdateMouseSettings_float(object sender = null, ValueChangedEventArgs<float> e = null)
		{
			((Control)_mouseImg).set_Opacity(_settingMouseCursorOpacity.get_Value());
		}

		private void UpdateMouseSettings_string(object sender = null, ValueChangedEventArgs<string> e = null)
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

		private void UpdateMouseSettings_bool(object sender = null, ValueChangedEventArgs<bool> e = null)
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
				Color[] buffer = new Color[texture.Width * texture.Height];
				texture.GetData(buffer);
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
				}
				texture.SetData(buffer);
				return texture;
			}
			catch
			{
				return ContentsManager.GetTexture("Circle Cyan.png");
			}
		}

		private Color ToRGB(Color color)
		{
			if (color == null)
			{
				return new Color(255, 255, 255);
			}
			return new Color(color.get_Cloth().get_Rgb()[0], color.get_Cloth().get_Rgb()[1], color.get_Cloth().get_Rgb()[2]);
		}
	}
}
