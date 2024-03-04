using Estreya.BlishHUD.Shared.Modules;

namespace Estreya.BlishHUD.EventTable.Models
{
	public class ModuleErrorStateGroup : Estreya.BlishHUD.Shared.Modules.ModuleErrorStateGroup
	{
		public static ModuleErrorStateGroup LOADING_EVENTS = new ModuleErrorStateGroup("loading-events");

		protected ModuleErrorStateGroup(string group)
			: base(group)
		{
		}
	}
}
