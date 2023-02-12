using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Dungeons.Models;
using RaidClears.Features.Dungeons.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons
{
	public class DungeonPanel : GridPanel
	{
		private IEnumerable<Dungeon> _dungeons;

		private static DungeonSettings Settings => Service.Settings.DungeonSettings;

		public DungeonPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			DungeonPanel dungeonPanel = this;
			DungeonsClearsService dungeonClearsService = new DungeonsClearsService();
			_dungeons = DungeonFactory.Create(this);
			Service.ApiPollingService!.ApiPollingTrigger += delegate
			{
				Task.Run(async delegate
				{
					List<string> weeklyClears = await dungeonClearsService.GetClearsFromApi();
					List<string> freqPaths = await dungeonClearsService.GetFrequenterPaths();
					foreach (Dungeon dungeon in dungeonPanel._dungeons)
					{
						foreach (Path encounter in dungeon.boxes.OfType<Path>())
						{
							encounter.SetCleared(weeklyClears.Contains(encounter.id));
							encounter.SetFrequenter(freqPaths.Contains(encounter.id));
							if (dungeon.index == 8 && encounter.id.Equals("freq"))
							{
								encounter.SetFrequenter(freqStatus: true);
								((Label)encounter.Box).set_Text($"{freqPaths.Count()}/8");
								encounter.ApplyTextColor();
							}
						}
					}
					((Control)dungeonPanel).Invalidate();
				});
			};
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}
	}
}
