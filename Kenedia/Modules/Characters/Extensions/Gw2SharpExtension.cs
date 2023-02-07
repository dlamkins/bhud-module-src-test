using System.Collections.Generic;
using Gw2Sharp.Models;
using Kenedia.Modules.Characters.Enums;
using Kenedia.Modules.Characters.Services;

namespace Kenedia.Modules.Characters.Extensions
{
	public static class Gw2SharpExtension
	{
		public static Data.CraftingProfession GetData(this int key, Dictionary<int, Data.CraftingProfession> data)
		{
			data.TryGetValue(key, out var value);
			return value;
		}

		public static Data.Profession GetData(this ProfessionType key, Dictionary<ProfessionType, Data.Profession> data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			data.TryGetValue(key, out var value);
			return value;
		}

		public static Data.Specialization GetData(this SpecializationType key, Dictionary<SpecializationType, Data.Specialization> data)
		{
			data.TryGetValue(key, out var value);
			return value;
		}

		public static Data.Race GetData(this RaceType key, Dictionary<RaceType, Data.Race> data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			data.TryGetValue(key, out var value);
			return value;
		}
	}
}
