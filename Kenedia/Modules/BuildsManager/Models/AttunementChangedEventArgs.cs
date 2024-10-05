using System;
using Kenedia.Modules.Core.DataModels;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class AttunementChangedEventArgs : EventArgs
	{
		public AttunementSlotType Slot { get; set; }

		public AttunementType? Attunement { get; set; }

		public AttunementType? OldAttunement { get; set; }

		public AttunementChangedEventArgs(AttunementSlotType slot, AttunementType? attunement, AttunementType? oldAttunement = null)
		{
			Slot = slot;
			Attunement = attunement;
			OldAttunement = oldAttunement;
		}
	}
}
