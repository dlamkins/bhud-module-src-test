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

		public Map(Map map)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Id = map.get_Id();
			Name = map.get_Name();
			Type = ApiEnum<MapType>.op_Implicit(map.get_Type());
		}

		public Map(Map map, Locale lang)
			: this(map)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Names[lang] = map.get_Name();
		}
	}
}
