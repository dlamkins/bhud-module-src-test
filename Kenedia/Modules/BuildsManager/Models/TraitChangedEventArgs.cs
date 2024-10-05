using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TraitChangedEventArgs : EventArgs
	{
		public SpecializationSlotType SpecSlot { get; set; }

		public TraitTierType Slot { get; set; }

		public Trait? Trait { get; set; }

		public Trait? OldTrait { get; set; }

		public TraitChangedEventArgs(SpecializationSlotType specSlot, TraitTierType slot, Trait? trait, Trait? oldTrait = null)
		{
			SpecSlot = specSlot;
			Slot = slot;
			Trait = trait;
			OldTrait = oldTrait;
		}
	}
}
