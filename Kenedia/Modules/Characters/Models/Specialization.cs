using System;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Models
{
	public class Specialization
	{
		public int Id { get; set; }

		public ProfessionType Profession { get; set; }

		public LocalizedString Names { get; set; } = new LocalizedString();


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

		public int IconAssetId { get; set; }

		public int IconBigAssetId { get; set; }

		[JsonIgnore]
		public AsyncTexture2D Icon
		{
			get
			{
				if (_003CIcon_003Ek__BackingField == null && IconAssetId != 0)
				{
					_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconAssetId);
				}
				return _003CIcon_003Ek__BackingField;
			}
		}

		[JsonIgnore]
		public AsyncTexture2D IconBig
		{
			get
			{
				if (_003CIconBig_003Ek__BackingField == null && IconAssetId != 0)
				{
					_003CIconBig_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconBigAssetId);
				}
				return _003CIconBig_003Ek__BackingField;
			}
		}

		public void ApplyApiData(Gw2Sharp.WebApi.V2.Models.Specialization specialization)
		{
			Id = specialization.Id;
			Profession = (Enum.TryParse<ProfessionType>(specialization.Profession, out var professionType) ? professionType : ((ProfessionType)0));
			IconAssetId = specialization.ProfessionIcon.GetAssetIdFromRenderUrl();
			IconBigAssetId = specialization.ProfessionIconBig.GetAssetIdFromRenderUrl();
			Name = specialization.Name;
		}
	}
}
