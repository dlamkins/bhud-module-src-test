using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Oberyn.AnglerAssociate.Data;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public static class FishCatalog
	{
		public static IEnumerable<Fish> All => FishDataTyria.All.Concat(FishDataOrr.All).Concat(FishDataMaguumaJungle.All).Concat(FishDataCantha.All)
			.Concat(FishDataCastora.All)
			.Concat(FishDataCrystalDesert.All)
			.Concat(FishDataGlobal.All)
			.Concat(FishDataHornOfMaguuma.All)
			.Concat(FishDataJanthir.All);

		[IteratorStateMachine(typeof(_003CGetCatchableNow_003Ed__2))]
		public static IEnumerable<Fish> GetCatchableNow()
		{
			return new _003CGetCatchableNow_003Ed__2(-2);
		}
	}
}
