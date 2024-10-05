using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class SpecializationChangedEventArgs : EventArgs
	{
		public SpecializationSlotType Slot { get; set; }

		public Specialization? Specialization { get; set; }

		public Specialization? OldSpecialization { get; set; }

		public SpecializationChangedEventArgs(SpecializationSlotType slot, Specialization? specialization, Specialization? oldSpecialization = null)
		{
			Slot = slot;
			Specialization = specialization;
			OldSpecialization = oldSpecialization;
		}
	}
}
