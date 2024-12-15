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
		private readonly IEnumerable<Wing> Wings = new List<Wing>();

		private static RaidSettings Settings => Service.Settings.RaidSettings;

		public RaidPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			Wings = WingFactory.Create(this);
			Service.ApiPollingService!.ApiPollingTrigger += delegate
			{
				Task.Run(async delegate
				{
					List<string> weeklyClears = await GetCurrentClearsService.GetClearsFromApi();
					foreach (Wing wing in Wings)
					{
						foreach (BoxModel encounter in wing.boxes)
						{
							encounter.SetCleared(weeklyClears.Contains(encounter.id));
						}
					}
					((Control)this).Invalidate();
				});
			};
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}

		public void UpdateEncounterLabel(string encounterApiId, string newLabel)
		{
			foreach (Wing wing in Wings)
			{
				if (wing.id == encounterApiId)
				{
					((Label)wing.GroupLabel).set_Text(newLabel);
					break;
				}
				foreach (BoxModel encounter in wing.boxes)
				{
					if (encounter.id == encounterApiId)
					{
						encounter.SetLabel(newLabel);
						return;
					}
				}
			}
		}
	}
}
