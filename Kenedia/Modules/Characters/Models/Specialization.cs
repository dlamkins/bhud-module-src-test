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

		public void ApplyApiData(Specialization specialization)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Id = specialization.get_Id();
			Profession = (ProfessionType)(Enum.TryParse<ProfessionType>(specialization.get_Profession(), out ProfessionType professionType) ? ((int)professionType) : 0);
			IconAssetId = specialization.get_ProfessionIcon().GetAssetIdFromRenderUrl();
			IconBigAssetId = specialization.get_ProfessionIconBig().GetAssetIdFromRenderUrl();
			Name = specialization.get_Name();
		}
	}
}
