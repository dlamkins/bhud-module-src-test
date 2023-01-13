using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
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

		public IEnumerable<Color> Colors;

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
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
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
				MaxValue = 1f,
				Value = 1f,
				BarText = Translations.TimerBarTextHusks + " (" + Translations.TimerBarTextReady + ")"
			};
			mainPanel.Resized += delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
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
				MaxValue = 1f,
				Value = 1f,
				BarText = Translations.TimerBarTextEggs + " (" + Translations.TimerBarTextReady + ")"
			};
			mainPanel.Resized += delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
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
			List<Color> list = new List<Color>();
			Color val = new Color();
			val.set_Name("Default");
			ColorMaterial val2 = new ColorMaterial();
			val2.set_Rgb((IReadOnlyList<int>)new List<int> { 237, 121, 38 });
			val.set_Cloth(val2);
			list.Add(val);
			IEnumerable<Color> first = list;
			Colors = first.Concat((IEnumerable<Color>)(await ((IAllExpandableClient<Color>)(object)Instance.Gw2ApiManager.Gw2ApiClient.get_V2().get_Colors()).AllAsync(default(CancellationToken))));
			SettingsManager.ModuleSettings.DefineSetting<Color>("colorPickerSettingTimerBar0", Colors?.First(), () => Translations.SettingColorSelectionHusksText, () => Translations.SettingColorSelectionHusksTooltipText);
			SettingsManager.ModuleSettings.DefineSetting<Color>("colorPickerSettingTimerBarRefilling0", Colors?.First(), () => Translations.SettingColorSelectionHusksRefillingText, () => Translations.SettingColorSelectionHusksRefillingTooltipText);
			SettingsManager.ModuleSettings.DefineSetting<Color>("colorPickerSettingTimerBar1", Colors?.First(), () => Translations.SettingColorSelectionEggsText, () => Translations.SettingColorSelectionEggsTooltipText);
			SettingsManager.ModuleSettings.DefineSetting<Color>("colorPickerSettingTimerBarRefilling1", Colors?.First(), () => Translations.SettingColorSelectionEggsRefillingText, () => Translations.SettingColorSelectionEggsRefillingTooltipText);
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
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Invalid comparison between Unknown and I4
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.CurrentMap.Id != 73)
			{
				if (mainPanel.TargetVisibility)
				{
					mainPanel.Hide();
				}
				return;
			}
			Vector3 pos = GameService.Gw2Mumble.PlayerCharacter.Position;
			Vector2 vec2Pos = default(Vector2);
			((Vector2)(ref vec2Pos))._002Ector(pos.X, pos.Y);
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
				mainPanel.BlockerIconVisible = false;
				mainPanel.BlockerIconTint = new Color(Color.get_White(), 0f);
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
				bool isMounted = (int)GameService.Gw2Mumble.PlayerCharacter.CurrentMount > 0;
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
					mainPanel.BlockerIconTint = Color.get_LawnGreen();
				}
				else
				{
					Vector3 rawCharacterPosition = GameService.Gw2Mumble.RawClient.get_AvatarPosition().ToXnaVector3();
					Vector2 playerChameraDirection = new Vector2(GameService.Gw2Mumble.PlayerCamera.Position.X, GameService.Gw2Mumble.PlayerCamera.Position.Y) - new Vector2(rawCharacterPosition.X, rawCharacterPosition.Y);
					Vector2 targetDirection = gatheringSpot.Position - new Vector2(rawCharacterPosition.X, rawCharacterPosition.Y);
					double sin = playerChameraDirection.X * targetDirection.Y - targetDirection.X * playerChameraDirection.Y;
					float angle = MathHelper.ToRadians((float)(Math.Atan2(playerChameraDirection.X * targetDirection.X + playerChameraDirection.Y * targetDirection.Y, sin) * (180.0 / Math.PI)));
					mainPanel.BlockerIconRotation = angle;
					if (isInMiddleBlockRange)
					{
						mainPanel.BlockerIconTint = new Color((Vector2.Distance(gatheringSpot.Position, vec2Pos) - 0.35f) / 0.65f + 0.486f, (Vector2.Distance(gatheringSpot.Position, vec2Pos) - 0.35f) / 0.65f + 0.988f, 0f, 1f);
					}
					else if (isInMajorBlockRange)
					{
						mainPanel.BlockerIconTint = new Color(1f, 1f - (Vector2.Distance(gatheringSpot.Position, vec2Pos) - 1f) / 9f, 0f, 1f);
					}
				}
			}
			husksTimerManager.Update(gameTime);
			eggsTimerManager.Update(gameTime);
		}
	}
}
