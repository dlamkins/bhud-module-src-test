using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Manlaan.Clock.Control;
using Manlaan.Clock.Views;
using Microsoft.Xna.Framework;

namespace Manlaan.Clock
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		public static string[] _fontSizes = new string[12]
		{
			"8", "11", "12", "14", "16", "18", "20", "22", "24", "32",
			"34", "36"
		};

		public static string[] _fontAlign = new string[3] { "Left", "Center", "Right" };

		public static SettingEntry<bool> _settingClockLocal;

		public static SettingEntry<bool> _settingClockTyria;

		public static SettingEntry<bool> _settingClockServer;

		public static SettingEntry<bool> _settingClockDayNight;

		public static SettingEntry<bool> _settingClock24H;

		public static SettingEntry<bool> _settingClockHideLabel;

		public static SettingEntry<string> _settingClockFontSize;

		public static SettingEntry<string> _settingClockLabelAlign;

		public static SettingEntry<string> _settingClockTimeAlign;

		public static SettingEntry<bool> _settingClockDrag;

		public static SettingEntry<Point> _settingClockLoc;

		private DrawClock _clockImg;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			_settingClockLocal = settings.DefineSetting<bool>("ClockLocal", true, "Local", "", (SettingTypeRendererDelegate)null);
			_settingClockTyria = settings.DefineSetting<bool>("ClockTyria", true, "Tyria", "", (SettingTypeRendererDelegate)null);
			_settingClockServer = settings.DefineSetting<bool>("ClockServer", false, "Server", "", (SettingTypeRendererDelegate)null);
			_settingClockDayNight = settings.DefineSetting<bool>("ClockDay", false, "Day/Night", "", (SettingTypeRendererDelegate)null);
			_settingClock24H = settings.DefineSetting<bool>("Clock24H", false, "24 Hour Time", "", (SettingTypeRendererDelegate)null);
			_settingClockHideLabel = settings.DefineSetting<bool>("ClockHideLabel", false, "Hide Labels", "", (SettingTypeRendererDelegate)null);
			_settingClockFontSize = settings.DefineSetting<string>("ClockFont2", "12", "Font Size", "", (SettingTypeRendererDelegate)null);
			_settingClockLabelAlign = settings.DefineSetting<string>("ClockLebelAlign2", "Right", "Label Align", "", (SettingTypeRendererDelegate)null);
			_settingClockTimeAlign = settings.DefineSetting<string>("ClockTimeAlign2", "Right", "Time Align", "", (SettingTypeRendererDelegate)null);
			_settingClockLoc = settings.DefineSetting<Point>("ClockLoc", new Point(100, 100), "Location", "", (SettingTypeRendererDelegate)null);
			_settingClockDrag = settings.DefineSetting<bool>("ClockDrag", false, "Enable Dragging", "", (SettingTypeRendererDelegate)null);
			_settingClockDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockLocal.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockTyria.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockServer.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockDayNight.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClockLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateClockSettings_Location);
			_settingClockLabelAlign.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClockTimeAlign.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClock24H.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockHideLabel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override void Initialize()
		{
			_clockImg = new DrawClock();
			((Control)_clockImg).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			UpdateClockSettings_Show();
			UpdateClockSettings_Font();
			UpdateClockSettings_Location();
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				((Control)_clockImg).Show();
			}
			else
			{
				((Control)_clockImg).Hide();
			}
			_clockImg.LocalTime = DateTime.Now;
			_clockImg.TyriaTime = CalcTyriaTime();
			_clockImg.ServerTime = CalcServerTime();
			_clockImg.DayNightTime = CalcDayNightTime();
		}

		protected override void Unload()
		{
			_settingClockDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockLocal.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockTyria.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockServer.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockDayNight.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockFontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClockLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateClockSettings_Location);
			_settingClockLabelAlign.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClockTimeAlign.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateClockSettings_Font);
			_settingClock24H.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			_settingClockHideLabel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateClockSettings_Show);
			DrawClock clockImg = _clockImg;
			if (clockImg != null)
			{
				((Control)clockImg).Dispose();
			}
		}

		private void UpdateClockSettings_Show(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			_clockImg.ShowLocal = _settingClockLocal.get_Value();
			_clockImg.ShowTyria = _settingClockTyria.get_Value();
			_clockImg.ShowServer = _settingClockServer.get_Value();
			_clockImg.ShowDayNight = _settingClockDayNight.get_Value();
			_clockImg.Show24H = _settingClock24H.get_Value();
			_clockImg.HideLabel = _settingClockHideLabel.get_Value();
			_clockImg.Drag = _settingClockDrag.get_Value();
		}

		private void UpdateClockSettings_Font(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			_clockImg.Font_Size = (FontSize)Enum.Parse(typeof(FontSize), "Size" + _settingClockFontSize.get_Value());
			_clockImg.LabelAlign = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), _settingClockLabelAlign.get_Value());
			_clockImg.TimeAlign = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), _settingClockTimeAlign.get_Value());
		}

		private void UpdateClockSettings_Location(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (_settingClockLoc.get_Value().X < 1)
			{
				_settingClockLoc.set_Value(new Point(1, _settingClockLoc.get_Value().Y));
			}
			if (_settingClockLoc.get_Value().Y < 1)
			{
				_settingClockLoc.set_Value(new Point(_settingClockLoc.get_Value().X, 1));
			}
			((Control)_clockImg).set_Location(_settingClockLoc.get_Value());
		}

		private DateTime CalcServerTime()
		{
			return DateTime.UtcNow;
		}

		private DateTime CalcTyriaTime()
		{
			try
			{
				DateTime UTC = DateTime.UtcNow;
				int utcsec = (utcsec = UTC.Hour * 3600 + UTC.Minute * 60 + UTC.Second);
				int tyriasec = utcsec * 12 - 60;
				tyriasec %= 86400;
				int tyrianhour = tyriasec / 3600;
				tyriasec %= 3600;
				int tyrianmin = tyriasec / 60;
				tyriasec %= 60;
				return new DateTime(2000, 1, 1, tyrianhour, tyrianmin, tyriasec);
			}
			catch
			{
				return new DateTime(2000, 1, 1, 0, 0, 0);
			}
		}

		private string CalcDayNightTime()
		{
			DateTime TyriaTime = CalcTyriaTime();
			int Map = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (Map == 1452 || Map == 1442 || Map == 1438 || Map == 1422 || Map == 1462)
			{
				if (TyriaTime >= new DateTime(2000, 1, 1, 8, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 18, 0, 0))
				{
					return "Day";
				}
				if (TyriaTime >= new DateTime(2000, 1, 1, 18, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 19, 0, 0))
				{
					return "Day+";
				}
				if (TyriaTime >= new DateTime(2000, 1, 1, 19, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 20, 0, 0))
				{
					return "Dusk";
				}
				if (TyriaTime >= new DateTime(2000, 1, 1, 6, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 7, 0, 0))
				{
					return "Night+";
				}
				if (TyriaTime >= new DateTime(2000, 1, 1, 7, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 8, 0, 0))
				{
					return "Dawn";
				}
				return "Night";
			}
			if (TyriaTime >= new DateTime(2000, 1, 1, 6, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 19, 0, 0))
			{
				return "Day";
			}
			if (TyriaTime >= new DateTime(2000, 1, 1, 19, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 20, 0, 0))
			{
				return "Day+";
			}
			if (TyriaTime >= new DateTime(2000, 1, 1, 20, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 21, 0, 0))
			{
				return "Dusk";
			}
			if (TyriaTime >= new DateTime(2000, 1, 1, 4, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 5, 0, 0))
			{
				return "Night+";
			}
			if (TyriaTime >= new DateTime(2000, 1, 1, 5, 0, 0) && TyriaTime < new DateTime(2000, 1, 1, 6, 0, 0))
			{
				return "Dawn";
			}
			return "Night";
		}
	}
}
