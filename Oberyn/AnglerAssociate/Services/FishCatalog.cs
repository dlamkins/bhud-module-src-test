using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Oberyn.AnglerAssociate.Data;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public static class FishCatalog
	{
		public static IEnumerable<Fish> All => TyriaFishData.All.Concat(CanthaFishData.All).Concat(CastoraFishData.All).Concat(CrystalDesertFishData.All)
			.Concat(GlobalFishData.All)
			.Concat(HornOfMaguumaFishData.All)
			.Concat(JanthirFishData.All);

		[IteratorStateMachine(typeof(_003CGetCatchableNow_003Ed__2))]
		public static IEnumerable<Fish> GetCatchableNow()
		{
			return new _003CGetCatchableNow_003Ed__2(-2);
		}
	}
}
