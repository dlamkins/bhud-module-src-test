using System;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kenedia.Modules.BuildsManager.DataModels.Converter
{
	public class RelicConverter : JsonConverter<Relic>
	{
		public override Relic ReadJson(JsonReader reader, Type objectType, Relic existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			Kenedia.Modules.Core.DataModels.ItemType type;
			ItemRarity rarity;
			return new Relic
			{
				Name = (string?)jObject["name"],
				Description = (string?)jObject["description"],
				Type = (Enum.TryParse<Kenedia.Modules.Core.DataModels.ItemType>((string?)jObject["type"], out type) ? type : Kenedia.Modules.Core.DataModels.ItemType.Unknown),
				Rarity = (Enum.TryParse<ItemRarity>((string?)jObject["rarity"], out rarity) ? rarity : ItemRarity.Unknown),
				Id = (int)jObject["id"],
				AssetId = ((string?)jObject["icon"]).GetAssetIdFromRenderUrl()
			};
		}

		public override void WriteJson(JsonWriter writer, Relic value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
