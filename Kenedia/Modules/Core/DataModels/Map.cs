using System.Runtime.Serialization;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.Core.DataModels
{
	[DataContract]
	public class Map
	{
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
		public LocalizedString Names { get; } = new LocalizedString();


		[DataMember]
		public int Id { get; set; }

		public Map()
		{
		}

		public Map(Map map)
		{
			Id = map.get_Id();
			Name = map.get_Name();
		}

		public Map(Map map, Locale lang)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			Id = map.get_Id();
			Names[lang] = map.get_Name();
		}
	}
}
