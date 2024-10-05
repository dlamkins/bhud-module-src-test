using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class LegendChangedEventArgs : EventArgs
	{
		public LegendSlotType Slot { get; }

		public Legend? OldValue { get; }

		public Legend? NewValue { get; }

		public LegendChangedEventArgs(LegendSlotType slot, Legend? oldValue, Legend? newValue)
		{
			Slot = slot;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
