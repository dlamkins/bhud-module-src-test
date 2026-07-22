using System;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kenedia.Modules.BuildsManager.Utility
{
	public class TemplateConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public TemplateFactory TemplateFactory { get; }

		public TemplateConverter(TemplateFactory templateFactory)
		{
			TemplateFactory = templateFactory;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Template);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			UniqueObservableCollection<string> tags = new UniqueObservableCollection<string>();
			try
			{
				if (jo.TryGetValue("Tags", StringComparison.OrdinalIgnoreCase, out var tagToken) && tagToken != null)
				{
					tags = tagToken.ToObject<UniqueObservableCollection<string>>(serializer);
				}
			}
			catch
			{
				tags = new UniqueObservableCollection<string>();
			}
			string name = (string?)jo["Name"];
			string buildCode = (string?)jo["BuildCode"];
			string gearCode = (string?)jo["GearCode"];
			string description = (string?)jo["Description"];
			int? race = (int?)jo["Race"];
			int? profession = (int?)jo["Profession"];
			int elitespecId = ((int?)jo["EliteSpecializationId"]).GetValueOrDefault();
			string lastModified = (string?)jo["LastModified"];
			return TemplateFactory.CreateTemplate(name, buildCode, gearCode, description, tags, (Races)race.GetValueOrDefault(-1), (ProfessionType)profession.GetValueOrDefault(1), elitespecId, lastModified);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
