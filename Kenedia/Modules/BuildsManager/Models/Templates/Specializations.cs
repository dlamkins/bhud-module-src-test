using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	public class Specializations : IEnumerable<BuildSpecialization>, IEnumerable
	{
		public BuildSpecialization Specialization1 { get; } = new BuildSpecialization(SpecializationSlotType.Line_1);


		public BuildSpecialization Specialization2 { get; } = new BuildSpecialization(SpecializationSlotType.Line_2);


		public BuildSpecialization Specialization3 { get; } = new BuildSpecialization(SpecializationSlotType.Line_3);


		public BuildSpecialization this[SpecializationSlotType slot] => slot switch
		{
			SpecializationSlotType.Line_1 => Specialization1, 
			SpecializationSlotType.Line_2 => Specialization2, 
			SpecializationSlotType.Line_3 => Specialization3, 
			_ => throw new ArgumentOutOfRangeException("slot", slot, null), 
		};

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__11))]
		public IEnumerator<BuildSpecialization> GetEnumerator()
		{
			return new _003CGetEnumerator_003Ed__11(0)
			{
				_003C_003E4__this = this
			};
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
