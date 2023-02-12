using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids
{
	public class RaidPanel : GridPanel
	{
		private static RaidSettings Settings => Service.Settings.RaidSettings;

		public RaidPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			RaidPanel raidPanel = this;
			WeeklyWings weeklyWings = WingRotationService.GetWeeklyWings();
			IEnumerable<Wing> wings = WingFactory.Create(this, weeklyWings);
			Service.ApiPollingService!.ApiPollingTrigger += delegate
			{
				Task.Run(async delegate
				{
					List<string> weeklyClears = await GetCurrentClearsService.GetClearsFromApi();
					foreach (Wing item in wings)
					{
						foreach (BoxModel encounter in item.boxes)
						{
							encounter.SetCleared(weeklyClears.Contains(encounter.id));
						}
					}
					((Control)raidPanel).Invalidate();
				});
			};
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}
	}
}
