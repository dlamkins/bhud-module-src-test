using System;
using System.Collections;
using System.Collections.Generic;
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

		public IEnumerator<Trait?> GetEnumerator()
		{
			yield return Adept;
			yield return Master;
			yield return GrandMaster;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
