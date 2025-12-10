using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	public class TraitCollection : IEnumerable<Trait?>, IEnumerable
	{
		public Trait? Adept { get; set; }

		public Trait? Master { get; set; }

		public Trait? GrandMaster { get; set; }

		public Trait? this[TraitTierType slot] => slot switch
		{
			TraitTierType.Adept => Adept, 
			TraitTierType.Master => Master, 
			TraitTierType.GrandMaster => GrandMaster, 
			_ => throw new ArgumentOutOfRangeException("slot", slot, null), 
		};

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__14))]
		public IEnumerator<Trait?> GetEnumerator()
		{
			return new _003CGetEnumerator_003Ed__14(0)
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
