using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
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
	[Export(typeof(Module))]
	public class Module : Module
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

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
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
			TTPanel obj = new TTPanel
			{
				Title = "Blockers Stuff"
			};
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			mainPanel = obj;
			TimerBar timerBar = new TimerBar(0);
			((Control)timerBar).set_Location(new Point(7, 7));
			((Control)timerBar).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width - 14, ((Container)mainPanel).get_ContentRegion().Height / 2 - 7 - 3));
			((Control)timerBar).set_Parent((Container)(object)mainPanel);
			timerBar.MaxValue = 1f;
			timerBar.Value = 1f;
			timerBar.BarText = Translations.TimerBarTextHusks + " (" + Translations.TimerBarTextReady + ")";
			husksBar = timerBar;
			((Control)mainPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				((Control)husksBar).set_Width(((Container)mainPanel).get_ContentRegion().Width - 14);
				((Control)husksBar).set_Height(((Container)mainPanel).get_ContentRegion().Height / 2 - 7 - 3);
				((Control)husksBar).RecalculateLayout();
			});
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
			TimerBar timerBar2 = new TimerBar(1);
			((Control)timerBar2).set_Location(new Point(7, ((Control)husksBar).get_Location().Y + ((Control)husksBar).get_Size().Y + 7));
			((Control)timerBar2).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width - 14, ((Container)mainPanel).get_ContentRegion().Height / 2 - 7 - 3));
			((Control)timerBar2).set_Parent((Container)(object)mainPanel);
			timerBar2.MaxValue = 1f;
			timerBar2.Value = 1f;
			timerBar2.BarText = Translations.TimerBarTextEggs + " (" + Translations.TimerBarTextReady + ")";
			eggsBar = timerBar2;
			((Control)mainPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				((Control)eggsBar).set_Width(((Container)mainPanel).get_ContentRegion().Width - 14);
				((Control)eggsBar).set_Height(((Container)mainPanel).get_ContentRegion().Height / 2 - 7 - 3);
				((Control)eggsBar).set_Location(new Point(7, ((Control)husksBar).get_Location().Y + ((Control)husksBar).get_Size().Y + 7));
				((Control)eggsBar).RecalculateLayout();
			});
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
			Colors = first.Concat((IEnumerable<Color>)(await ((IAllExpandableClient<Color>)(object)Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).AllAsync(default(CancellationToken))));
			SettingsManager.get_ModuleSettings().DefineSetting<Color>("colorPickerSettingTimerBar0", Colors?.First(), (Func<string>)(() => Translations.SettingColorSelectionHusksText), (Func<string>)(() => Translations.SettingColorSelectionHusksTooltipText));
			SettingsManager.get_ModuleSettings().DefineSetting<Color>("colorPickerSettingTimerBarRefilling0", Colors?.First(), (Func<string>)(() => Translations.SettingColorSelectionHusksRefillingText), (Func<string>)(() => Translations.SettingColorSelectionHusksRefillingTooltipText));
			SettingsManager.get_ModuleSettings().DefineSetting<Color>("colorPickerSettingTimerBar1", Colors?.First(), (Func<string>)(() => Translations.SettingColorSelectionEggsText), (Func<string>)(() => Translations.SettingColorSelectionEggsTooltipText));
			SettingsManager.get_ModuleSettings().DefineSetting<Color>("colorPickerSettingTimerBarRefilling1", Colors?.First(), (Func<string>)(() => Translations.SettingColorSelectionEggsRefillingText), (Func<string>)(() => Translations.SettingColorSelectionEggsRefillingTooltipText));
			await _003C_003En__0();
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new TTSettingsCollection(SettingsManager.get_ModuleSettings());
		}

		protected override void Unload()
		{
			Logger.Info("Unloading TT Blockers Stuff");
			TTPanel tTPanel = mainPanel;
			if (tTPanel != null)
			{
				((Control)tTPanel).Dispose();
			}
			TimerBar timerBar = husksBar;
			if (timerBar != null)
			{
				((Control)timerBar).Dispose();
			}
			TimerBar timerBar2 = eggsBar;
			if (timerBar2 != null)
			{
				((Control)timerBar2).Dispose();
			}
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
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() != 73)
			{
				if (mainPanel.TargetVisibility)
				{
					((Control)mainPanel).Hide();
				}
				return;
			}
			Vector3 pos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			Vector2 vec2Pos = default(Vector2);
			((Vector2)(ref vec2Pos))._002Ector(pos.X, pos.Y);
			gatheringSpot = GatheringSpot.FromPosition(vec2Pos);
			if (gatheringSpot == null)
			{
				if (mainPanel.TargetVisibility)
				{
					((Control)mainPanel).Hide();
				}
				return;
			}
			if (!mainPanel.TargetVisibility)
			{
				mainPanel.BlockerIconVisible = false;
				mainPanel.BlockerIconTint = new Color(Color.get_White(), 0f);
				mainPanel.Title = "Blockers Stuff - " + gatheringSpot.Name;
				((Control)mainPanel).Show();
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
				bool isMounted = (int)GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() > 0;
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
					Vector3 rawCharacterPosition = Vector3Extensions.ToXnaVector3(GameService.Gw2Mumble.get_RawClient().get_AvatarPosition());
					Vector2 playerChameraDirection = new Vector2(GameService.Gw2Mumble.get_PlayerCamera().get_Position().X, GameService.Gw2Mumble.get_PlayerCamera().get_Position().Y) - new Vector2(rawCharacterPosition.X, rawCharacterPosition.Y);
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
			husksTimerManager.Update();
			eggsTimerManager.Update();
		}
	}
}
