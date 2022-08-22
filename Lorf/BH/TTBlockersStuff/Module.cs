using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Lorf.BH.TTBlockersStuff.Settings;
using Lorf.BH.TTBlockersStuff.UI;
using Microsoft.Xna.Framework;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private const int PROGRESS_BAR_MARGIN = 7;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		public static Module Instance;

		private TTPanel mainPanel;

		private TimerBar husksBar;

		private TimerBar eggsBar;

		private GatheringSpot gatheringSpot;

		public IEnumerable<Gw2Sharp.WebApi.V2.Models.Color> Colors;

		private TimerManager husksTimerManager;

		private TimerManager eggsTimerManager;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			Instance = this;
		}

		protected override void Initialize()
		{
			Logger.Info("Initializing TT Blockers Stuff");
			mainPanel = new TTPanel
			{
				Title = "Blockers Stuff",
				Parent = GameService.Graphics.SpriteScreen
			};
			husksBar = new TimerBar(0)
			{
				Location = new Point(7, 7),
				Size = new Point(mainPanel.ContentRegion.Width - 14, mainPanel.ContentRegion.Height / 2 - 7 - 3),
				Parent = mainPanel,
				MaxValue = 80f,
				Value = 80f,
				BarText = Translations.TimerBarTextHusks + " (" + Translations.TimerBarTextReady + ")"
			};
			mainPanel.Resized += delegate
			{
				husksBar.Width = mainPanel.ContentRegion.Width - 14;
				husksBar.Height = mainPanel.ContentRegion.Height / 2 - 7 - 3;
				husksBar.RecalculateLayout();
			};
			husksTimerManager = new TimerManager
			{
				Name = Translations.TimerBarTextHusks,
				TimerBar = husksBar
			};
			husksBar.InternalClick += delegate
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				husksTimerManager.Activate(gatheringSpot.HuskTime);
			};
			eggsBar = new TimerBar(1)
			{
				Location = new Point(7, husksBar.Location.Y + husksBar.Size.Y + 7),
				Size = new Point(mainPanel.ContentRegion.Width - 14, mainPanel.ContentRegion.Height / 2 - 7 - 3),
				Parent = mainPanel,
				MaxValue = 40f,
				Value = 40f,
				BarText = Translations.TimerBarTextEggs + " (" + Translations.TimerBarTextReady + ")"
			};
			mainPanel.Resized += delegate
			{
				eggsBar.Width = mainPanel.ContentRegion.Width - 14;
				eggsBar.Height = mainPanel.ContentRegion.Height / 2 - 7 - 3;
				eggsBar.Location = new Point(7, husksBar.Location.Y + husksBar.Size.Y + 7);
				eggsBar.RecalculateLayout();
			};
			eggsTimerManager = new TimerManager
			{
				Name = Translations.TimerBarTextEggs,
				TimerBar = eggsBar
			};
			eggsBar.InternalClick += delegate
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				eggsTimerManager.Activate(40);
			};
		}

		protected override async Task LoadAsync()
		{
			IEnumerable<Gw2Sharp.WebApi.V2.Models.Color> first = new List<Gw2Sharp.WebApi.V2.Models.Color>
			{
				new Gw2Sharp.WebApi.V2.Models.Color
				{
					Name = "Default",
					Cloth = new ColorMaterial
					{
						Rgb = new List<int> { 237, 121, 38 }
					}
				}
			};
			Colors = first.Concat(await Instance.Gw2ApiManager.Gw2ApiClient.V2.Colors.AllAsync());
			SettingsManager.ModuleSettings.DefineSetting("colorPickerSettingTimerBar0", Colors?.First(), () => Translations.SettingColorSelectionHusksText, () => Translations.SettingColorSelectionHusksTooltipText);
			SettingsManager.ModuleSettings.DefineSetting("colorPickerSettingTimerBar1", Colors?.First(), () => Translations.SettingColorSelectionEggsText, () => Translations.SettingColorSelectionEggsTooltipText);
			await base.LoadAsync();
		}

		public override IView GetSettingsView()
		{
			return new TTSettingsCollection(SettingsManager.ModuleSettings);
		}

		protected override void Unload()
		{
			Logger.Info("Unloading TT Blockers Stuff");
			mainPanel?.Dispose();
			husksBar?.Dispose();
			eggsBar?.Dispose();
			Instance = null;
		}

		protected override void Update(GameTime gameTime)
		{
			if (GameService.Gw2Mumble.CurrentMap.Id != 73)
			{
				if (mainPanel.TargetVisibility)
				{
					mainPanel.Hide();
				}
				return;
			}
			Vector3 pos = GameService.Gw2Mumble.PlayerCharacter.Position;
			Vector2 vec2Pos = new Vector2(pos.X, pos.Y);
			gatheringSpot = GatheringSpot.FromPosition(vec2Pos);
			if (gatheringSpot == null)
			{
				if (mainPanel.TargetVisibility)
				{
					mainPanel.Hide();
				}
				return;
			}
			if (!mainPanel.TargetVisibility)
			{
				eggsTimerManager.Reset();
				husksTimerManager.Reset();
				mainPanel.BlockerIconVisible = false;
				mainPanel.BlockerIconTint = new Microsoft.Xna.Framework.Color(Microsoft.Xna.Framework.Color.White, 0f);
				mainPanel.Title = "Blockers Stuff - " + gatheringSpot.Name;
				mainPanel.Show();
			}
			bool isInMajorBlockRange = Vector2.Distance(gatheringSpot.Position, vec2Pos) < 10f;
			if (mainPanel.BlockerIconVisible != isInMajorBlockRange)
			{
				mainPanel.BlockerIconVisible = isInMajorBlockRange;
			}
			if (isInMajorBlockRange)
			{
				bool isInMiddleBlockRange = Vector2.Distance(gatheringSpot.Position, vec2Pos) < 1f;
				bool isInBlockRange = Vector2.Distance(gatheringSpot.Position, vec2Pos) < 0.35f;
				bool isMounted = GameService.Gw2Mumble.PlayerCharacter.CurrentMount != Gw2Sharp.Models.MountType.None;
				if (mainPanel.IsMounted != isMounted)
				{
					mainPanel.IsMounted = isMounted;
				}
				if (mainPanel.IsBlocking != isInBlockRange)
				{
					mainPanel.IsBlocking = isInBlockRange;
				}
				if (isInBlockRange)
				{
					if (mainPanel.BlockerIconRotation != 0f)
					{
						mainPanel.BlockerIconRotation = 0f;
					}
					mainPanel.BlockerIconTint = Microsoft.Xna.Framework.Color.LawnGreen;
				}
				else
				{
					Vector2 playerChameraDirection = new Vector2(GameService.Gw2Mumble.PlayerCamera.Position.X, GameService.Gw2Mumble.PlayerCamera.Position.Y) - new Vector2(GameService.Gw2Mumble.PlayerCharacter.Position.X, GameService.Gw2Mumble.PlayerCharacter.Position.Y);
					Vector2 targetDirection = gatheringSpot.Position - new Vector2(GameService.Gw2Mumble.PlayerCharacter.Position.X, GameService.Gw2Mumble.PlayerCharacter.Position.Y);
					double sin = playerChameraDirection.X * targetDirection.Y - targetDirection.X * playerChameraDirection.Y;
					float angle = MathHelper.ToRadians((float)(Math.Atan2(playerChameraDirection.X * targetDirection.X + playerChameraDirection.Y * targetDirection.Y, sin) * (180.0 / Math.PI)));
					mainPanel.BlockerIconRotation = angle;
					if (isInMiddleBlockRange)
					{
						mainPanel.BlockerIconTint = new Microsoft.Xna.Framework.Color((Vector2.Distance(gatheringSpot.Position, vec2Pos) - 0.35f) / 0.65f + 0.486f, (Vector2.Distance(gatheringSpot.Position, vec2Pos) - 0.35f) / 0.65f + 0.988f, 0f, 1f);
					}
					else if (isInMajorBlockRange)
					{
						mainPanel.BlockerIconTint = new Microsoft.Xna.Framework.Color(1f, 1f - (Vector2.Distance(gatheringSpot.Position, vec2Pos) - 1f) / 9f, 0f, 1f);
					}
				}
			}
			husksTimerManager.Update();
			eggsTimerManager.Update();
		}
	}
}
