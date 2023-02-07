using System.Collections.Generic;
using System.Runtime.Serialization;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Kenedia.Modules.Characters.Models
{
	[DataContract]
	public class Map
	{
		public string Name
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Expected I4, but got Unknown
				Locale value = GameService.Overlay.get_UserLocale().get_Value();
				return (value - 1) switch
				{
					1 => Names.De, 
					2 => Names.Fr, 
					0 => Names.Es, 
					_ => Names.En, 
				};
			}
			set
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Expected I4, but got Unknown
				Locale value2 = GameService.Overlay.get_UserLocale().get_Value();
				switch (value2 - 1)
				{
				case 1:
					Names.De = value;
					break;
				case 2:
					Names.Fr = value;
					break;
				case 0:
					Names.Es = value;
					break;
				default:
					Names.En = value;
					break;
				}
			}
		}

		[DataMember]
		public LocalizedString Names { get; set; } = new LocalizedString();


		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int APIId { get; set; }

		[DataMember]
		public IReadOnlyList<int> Floors { get; set; }

		[DataMember]
		public int DefaultFloor { get; set; }

		[DataMember]
		public int ContinentId { get; set; }
	}
}
