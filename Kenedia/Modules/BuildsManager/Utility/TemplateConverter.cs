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
			: this()
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
			UniqueObservableCollection<string> tags;
			try
			{
				tags = jo.get_Item("Tags").ToObject<UniqueObservableCollection<string>>(serializer);
			}
			catch
			{
				tags = new UniqueObservableCollection<string>();
			}
			string name = (string)jo.get_Item("Name");
			string buildCode = (string)jo.get_Item("BuildCode");
			string gearCode = (string)jo.get_Item("GearCode");
			string description = (string)jo.get_Item("Description");
			int? race = (int?)jo.get_Item("Race");
			int? profession = (int?)jo.get_Item("Profession");
			int? elitespecId = (int?)jo.get_Item("EliteSpecializationId");
			string lastModified = (string)jo.get_Item("LastModified");
			return TemplateFactory.CreateTemplate(name, buildCode, gearCode, description, tags, (Races)race.GetValueOrDefault(-1), (ProfessionType)profession.GetValueOrDefault(1), elitespecId.GetValueOrDefault(), lastModified);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
