using System;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kenedia.Modules.BuildsManager.DataModels.Converter
{
	public class PowerCoreConverter : JsonConverter<PowerCore>
	{
		public override PowerCore ReadJson(JsonReader reader, Type objectType, PowerCore existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			Kenedia.Modules.Core.DataModels.ItemType type;
			ItemRarity rarity;
			return new PowerCore
			{
				Name = (string)jObject.get_Item("name"),
				Description = (string)jObject.get_Item("description"),
				Type = (Enum.TryParse<Kenedia.Modules.Core.DataModels.ItemType>((string)jObject.get_Item("type"), out type) ? type : Kenedia.Modules.Core.DataModels.ItemType.Unknown),
				Rarity = (Enum.TryParse<ItemRarity>((string)jObject.get_Item("rarity"), out rarity) ? rarity : ItemRarity.Unknown),
				Id = (int)jObject.get_Item("id"),
				AssetId = ((string)jObject.get_Item("icon")).GetAssetIdFromRenderUrl()
			};
		}

		public override void WriteJson(JsonWriter writer, PowerCore value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
