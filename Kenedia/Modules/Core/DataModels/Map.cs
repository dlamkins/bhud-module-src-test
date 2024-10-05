using System.Runtime.Serialization;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.Core.DataModels
{
	[DataContract]
	public class Map
	{
		[DataMember]
		public LocalizedString Names { get; } = new LocalizedString();


		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public MapType Type { get; set; }

		public Map()
		{
		}

		public Map(Gw2Sharp.WebApi.V2.Models.Map map)
		{
			Id = map.Id;
			Name = map.Name;
			Type = map.Type;
		}

		public Map(Gw2Sharp.WebApi.V2.Models.Map map, Locale lang)
			: this(map)
		{
			Names[lang] = map.Name;
		}
	}
}
