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

		public Map(Map map)
		{
			ApplyApiData(map);
		}

		public void ApplyApiData(Map map)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Id = map.get_Id();
			Type = ApiEnum<MapType>.op_Implicit(map.get_Type());
			Names.Text = map.get_Name();
		}
	}
}
