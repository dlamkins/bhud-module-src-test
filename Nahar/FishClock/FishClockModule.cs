using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nahar.FishClock
{
	[Export(typeof(Module))]
	public class FishClockModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<FishClockModule>();

		private Label _fishingTimeLabel;

		private SettingEntry<FishingMapSelection> _selectedMapSetting;

		private SettingEntry<int> _labelPositionX;

		private SettingEntry<int> _labelPositionY;

		private bool _isDragging;

		private Point _dragOffset = Point.Zero;

		private double _runningTime;

		[ImportingConstructor]
		public FishClockModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_selectedMapSetting = settings.DefineSetting("SelectedFishingMapEnum", FishingMapSelection.Seitung_Province_Cantha, () => "Map to Monitor", () => "Choose which map you want to monitor the fishing time zone for.");
			_labelPositionX = settings.DefineSetting("LabelPosX", 300);
			_labelPositionY = settings.DefineSetting("LabelPosY", 200);
		}

		protected override void Initialize()
		{
		}

		protected override Task LoadAsync()
		{
			int savedX = _labelPositionX.Value;
			int savedY = _labelPositionY.Value;
			_fishingTimeLabel = new Label
			{
				Text = "Loading Fishing Clock...",
				Size = new Point(450, 40),
				Location = new Point(savedX, savedY),
				Font = GameService.Content.DefaultFont18,
				TextColor = Color.White,
				ShowShadow = true,
				Parent = GameService.Graphics.SpriteScreen
			};
			_fishingTimeLabel.LeftMouseButtonPressed += OnLabelPressed;
			_fishingTimeLabel.LeftMouseButtonReleased += OnLabelReleased;
			return Task.CompletedTask;
		}

		private void OnLabelPressed(object sender, MouseEventArgs e)
		{
			if (GameService.Input.Keyboard.ActiveModifiers.HasFlag(ModifierKeys.Ctrl))
			{
				_isDragging = true;
				_dragOffset = GameService.Input.Mouse.Position - _fishingTimeLabel.Location;
			}
		}

		private void OnLabelReleased(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				_isDragging = false;
				_labelPositionX.Value = _fishingTimeLabel.Location.X;
				_labelPositionY.Value = _fishingTimeLabel.Location.Y;
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (_fishingTimeLabel == null)
			{
				return;
			}
			if (_isDragging)
			{
				if (!GameService.Input.Keyboard.ActiveModifiers.HasFlag(ModifierKeys.Ctrl))
				{
					_isDragging = false;
					_labelPositionX.Value = _fishingTimeLabel.Location.X;
					_labelPositionY.Value = _fishingTimeLabel.Location.Y;
				}
				else
				{
					_fishingTimeLabel.Location = GameService.Input.Mouse.Position - _dragOffset;
				}
			}
			_runningTime += gameTime.ElapsedGameTime.TotalMilliseconds;
			if (_runningTime < 1000.0 && !_isDragging)
			{
				return;
			}
			_runningTime = 0.0;
			FishingMapSelection mapaSelecionado = _selectedMapSetting.Value;
			bool isCantha = false;
			string nomeAmigavel = "";
			switch (mapaSelecionado)
			{
			case FishingMapSelection.Seitung_Province_Cantha:
				nomeAmigavel = "Seitung Province (Cantha)";
				isCantha = true;
				break;
			case FishingMapSelection.New_Kaineng_City_Cantha:
				nomeAmigavel = "New Kaineng City (Cantha)";
				isCantha = true;
				break;
			case FishingMapSelection.The_Echovald_Wilds_Cantha:
				nomeAmigavel = "The Echovald Wilds (Cantha)";
				isCantha = true;
				break;
			case FishingMapSelection.Dragon_End_Cantha:
				nomeAmigavel = "Dragon's End (Cantha)";
				isCantha = true;
				break;
			case FishingMapSelection.Gyala_Delve_Cantha:
				nomeAmigavel = "Gyala Delve (Cantha)";
				isCantha = true;
				break;
			case FishingMapSelection.Kourna_Tyria:
				nomeAmigavel = "Domain of Kourna (Tyria)";
				isCantha = false;
				break;
			case FishingMapSelection.Core_Tyria:
				nomeAmigavel = "Core Tyria";
				isCantha = false;
				break;
			case FishingMapSelection.Orr_Tyria:
				nomeAmigavel = "Ruins of Orr (Tyria)";
				isCantha = false;
				break;
			}
			DateTime agoraUtc = DateTime.UtcNow;
			double currentInGameMinute = ((double)(agoraUtc.Hour % 2 * 60 + agoraUtc.Minute) + (double)agoraUtc.Second / 60.0) * 12.0 % 1440.0;
			string estadoAtual = "Unknown";
			double minutosReaisRestantes = 0.0;
			Color corDoTexto = Color.White;
			if (isCantha)
			{
				if (currentInGameMinute >= 420.0 && currentInGameMinute < 480.0)
				{
					estadoAtual = "DAWN \ud83c\udf05";
					minutosReaisRestantes = (480.0 - currentInGameMinute) / 12.0;
					corDoTexto = Color.Orange;
				}
				else if (currentInGameMinute >= 480.0 && currentInGameMinute < 1140.0)
				{
					estadoAtual = "DAY ☀\ufe0f";
					minutosReaisRestantes = (1140.0 - currentInGameMinute) / 12.0;
					corDoTexto = Color.LightYellow;
				}
				else if (currentInGameMinute >= 1140.0 && currentInGameMinute < 1200.0)
				{
					estadoAtual = "DUSK \ud83c\udf07";
					minutosReaisRestantes = (1200.0 - currentInGameMinute) / 12.0;
					corDoTexto = Color.OrangeRed;
				}
				else
				{
					estadoAtual = "NIGHT \ud83c\udf19";
					minutosReaisRestantes = ((currentInGameMinute >= 1200.0) ? (1440.0 - currentInGameMinute + 420.0) : (420.0 - currentInGameMinute)) / 12.0;
					corDoTexto = Color.SkyBlue;
				}
			}
			else if (currentInGameMinute >= 300.0 && currentInGameMinute < 360.0)
			{
				estadoAtual = "DAWN \ud83c\udf05";
				minutosReaisRestantes = (360.0 - currentInGameMinute) / 12.0;
				corDoTexto = Color.Orange;
			}
			else if (currentInGameMinute >= 360.0 && currentInGameMinute < 1200.0)
			{
				estadoAtual = "DAY ☀\ufe0f";
				minutosReaisRestantes = (1200.0 - currentInGameMinute) / 12.0;
				corDoTexto = Color.LightYellow;
			}
			else if (currentInGameMinute >= 1200.0 && currentInGameMinute < 1260.0)
			{
				estadoAtual = "DUSK \ud83c\udf07";
				minutosReaisRestantes = (1260.0 - currentInGameMinute) / 12.0;
				corDoTexto = Color.OrangeRed;
			}
			else
			{
				estadoAtual = "NIGHT \ud83c\udf19";
				minutosReaisRestantes = ((currentInGameMinute >= 1260.0) ? (1440.0 - currentInGameMinute + 300.0) : (300.0 - currentInGameMinute)) / 12.0;
				corDoTexto = Color.SkyBlue;
			}
			_fishingTimeLabel.Text = "[" + nomeAmigavel + "] " + estadoAtual + " - Left: " + TimeSpan.FromMinutes(minutosReaisRestantes).ToString("mm\\:ss");
			_fishingTimeLabel.TextColor = corDoTexto;
		}

		protected override void Unload()
		{
			if (_fishingTimeLabel != null)
			{
				_fishingTimeLabel.LeftMouseButtonPressed -= OnLabelPressed;
				_fishingTimeLabel.LeftMouseButtonReleased -= OnLabelReleased;
				_fishingTimeLabel.Dispose();
				_fishingTimeLabel = null;
			}
		}
	}
}
