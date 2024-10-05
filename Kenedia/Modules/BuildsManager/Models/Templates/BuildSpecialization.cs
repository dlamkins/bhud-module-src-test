using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	public class BuildSpecialization
	{
		public SpecializationSlotType SpecializationSlot { get; }

		public Specialization Specialization { get; set; }

		public TraitCollection Traits { get; } = new TraitCollection();


		public BuildSpecialization(SpecializationSlotType slot)
		{
			SpecializationSlot = slot;
		}

		public byte GetSpecializationByte()
		{
			return (byte)((Specialization != null) ? ((uint)Specialization.Id) : 0u);
		}

		public byte GetTraitByte(TraitTierType traitSlot)
		{
			if (Traits[traitSlot] != null)
			{
				int? order = Traits[traitSlot]?.Order;
				return (byte)((!order.HasValue) ? new int?(0) : (order + 1)).Value;
			}
			return 0;
		}
	}
}
