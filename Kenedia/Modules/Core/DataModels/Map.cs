using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.DataModels
{
	public class Map
	{
		public LocalizedString Names { get; } = new LocalizedString();


		[JsonIgnore]
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

		public int Id { get; set; }

		public MapType Type { get; set; }

		public Map()
		{
		}

		public Map(Gw2Sharp.WebApi.V2.Models.Map map)
		{
			ApplyApiData(map);
		}

		public void ApplyApiData(Gw2Sharp.WebApi.V2.Models.Map map)
		{
			Id = map.Id;
			Type = map.Type;
			Names.Text = map.Name;
		}
	}
}
