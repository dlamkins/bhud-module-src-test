using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class PetChangedEventArgs : EventArgs
	{
		public PetSlotType Slot { get; set; }

		public Pet? Pet { get; set; }

		public Pet? OldPet { get; set; }

		public PetChangedEventArgs(PetSlotType slot, Pet? pet, Pet? oldPet = null)
		{
			Slot = slot;
			Pet = pet;
			OldPet = oldPet;
		}
	}
}
