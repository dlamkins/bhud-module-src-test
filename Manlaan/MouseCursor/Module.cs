using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
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
		private static readonly Logger Logger = Logger.GetLogger<Module>();

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
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingMouseCursorImage = settings.DefineSetting<string>("MouseCursorImage", "Circle Cyan.png", (Func<string>)null, (Func<string>)null);
			_settingMouseCursorColor = settings.DefineSetting<string>("MouseCursorColor", "White0", (Func<string>)null, (Func<string>)null);
			_settingMouseCursorSize = settings.DefineSetting<int>("MouseCursorSize", 70, "Size", "", (SettingTypeRendererDelegate)null);
			_settingMouseCursorOpacity = settings.DefineSetting<float>("MouseCursorOpacity", 1f, "Opacity", "", (SettingTypeRendererDelegate)null);
			_settingMouseCursorCameraDrag = settings.DefineSetting<bool>("MouseCursorCameraDrag", false, "Show When Camera Dragging", "Shows the cursor when you move the camera.", (SettingTypeRendererDelegate)null);
			_settingMouseCursorAboveBlish = settings.DefineSetting<bool>("MouseCursorAboveBlish", false, "Show Above Blish Windows", "", (SettingTypeRendererDelegate)null);
			_settingMouseCursorOnlyCombat = settings.DefineSetting<bool>("MouseCursorOnlyCombat", false, "Only Show During Combat", "", (SettingTypeRendererDelegate)null);
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
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0063: Expected O, but got Unknown
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
			GameService.Input.get_Mouse().add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateMousePos);
		}

		protected override async Task LoadAsync()
		{
			string[] mousefiles = new string[23]
			{
				"Circle Blue.png", "Circle Cyan.png", "Circle Green.png", "Circle Magenta.png", "Circle Red.png", "Circle Yellow.png", "Arrow 1.png", "Arrow 2.png", "Arrow 3.png", "Circle 1.png",
				"Circle 2.png", "Circle 3.png", "Circle 4.png", "Circle 5.png", "Circle 6.png", "Circle 7.png", "Circle 8.png", "Cross 1.png", "Cross 2.png", "Cross 3.png",
				"Cross 4.png", "Cross 5.png", "mouse.psd"
			};
			string[] array = mousefiles;
			foreach (string file in array)
			{
				ExtractFile(file);
			}
			string dailiesDirectory = DirectoriesManager.GetFullDirectoryPath("mousecursor");
			string[] files = Directory.GetFiles(dailiesDirectory, ".");
			foreach (string file2 in files)
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
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Invalid comparison between Unknown and I4
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Invalid comparison between Unknown and I4
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			((Control)_mouseImg).set_Visible(_settingMouseCursorCameraDrag.get_Value() || !GameService.Input.get_Mouse().get_CameraDragging());
			if (((Control)_mouseImg).get_Visible() && _settingMouseCursorOnlyCombat.get_Value() && !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				((Control)_mouseImg).set_Visible(false);
			}
			MouseState state = GameService.Input.get_Mouse().get_State();
			if ((int)((MouseState)(ref state)).get_RightButton() != 1 && ((Control)_mouseImg).get_Visible())
			{
				int x2 = GameService.Input.get_Mouse().get_Position().X - _settingMouseCursorSize.get_Value() / 2;
				int y2 = GameService.Input.get_Mouse().get_Position().Y - _settingMouseCursorSize.get_Value() / 2;
				((Control)_mouseImg).set_Location(new Point(x2, y2));
				return;
			}
			state = GameService.Input.get_Mouse().get_State();
			if ((int)((MouseState)(ref state)).get_RightButton() == 1 && ((Control)_mouseImg).get_Visible())
			{
				int x = _mousePos.X - _settingMouseCursorSize.get_Value() / 2;
				int y = _mousePos.Y - _settingMouseCursorSize.get_Value() / 2;
				((Control)_mouseImg).set_Location(new Point(x, y));
			}
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
			GameService.Input.get_Mouse().remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateMousePos);
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
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((Control)_mouseImg).set_Size(new Point(_settingMouseCursorSize.get_Value(), _settingMouseCursorSize.get_Value()));
		}

		private void UpdateMouseSettings_float(object sender = null, ValueChangedEventArgs<float> e = null)
		{
			((Control)_mouseImg).set_Opacity(_settingMouseCursorOpacity.get_Value());
		}

		private void UpdateMouseSettings_string(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			MouseFile mouseFile = _mouseFiles.Find((MouseFile x) => x.Name.Equals(_settingMouseCursorImage.get_Value()));
			if (mouseFile == null || string.IsNullOrEmpty(mouseFile.File) || !File.Exists(mouseFile.File))
			{
				_mouseImg.Texture = ContentsManager.GetTexture("Circle Cyan.png");
			}
			else
			{
				_mouseImg.Texture = PremultiplyTexture(mouseFile.File, GameService.Graphics.get_GraphicsDevice());
			}
			_mouseImg.Tint = ToRGB(_colors.Find((Color x) => x.get_Name().Equals(_settingMouseCursorColor.get_Value())));
		}

		private void UpdateMouseSettings_bool(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_mouseImg.AboveBlish = _settingMouseCursorAboveBlish.get_Value();
		}

		private void UpdateMousePos(object sender, MouseEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_mousePos = GameService.Input.get_Mouse().get_Position();
		}

		private void ExtractFile(string filePath)
		{
			string fullPath = Path.Combine(DirectoriesManager.GetFullDirectoryPath("mousecursor"), filePath);
			using Stream fs = ContentsManager.GetFileStream(filePath);
			fs.Position = 0L;
			byte[] buffer = new byte[fs.Length];
			int content = fs.Read(buffer, 0, (int)fs.Length);
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			File.WriteAllBytes(fullPath, buffer);
		}

		private Texture2D PremultiplyTexture(string FilePath, GraphicsDevice device)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture;
			try
			{
				FileStream titleStream = File.OpenRead(FilePath);
				texture = Texture2D.FromStream(device, (Stream)titleStream);
				titleStream.Close();
				Color[] buffer = (Color[])(object)new Color[texture.get_Width() * texture.get_Height()];
				texture.GetData<Color>(buffer);
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Color.FromNonPremultiplied((int)((Color)(ref buffer[i])).get_R(), (int)((Color)(ref buffer[i])).get_G(), (int)((Color)(ref buffer[i])).get_B(), (int)((Color)(ref buffer[i])).get_A());
				}
				texture.SetData<Color>(buffer);
			}
			catch
			{
				texture = ContentsManager.GetTexture("Circle Cyan.png");
			}
			return texture;
		}

		private Color ToRGB(Color color)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			if (color == null)
			{
				return new Color(255, 255, 255);
			}
			return new Color(color.get_Cloth().get_Rgb()[0], color.get_Cloth().get_Rgb()[1], color.get_Cloth().get_Rgb()[2]);
		}
	}
}
