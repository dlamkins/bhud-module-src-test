using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Models
{
	public class CraftingProfession
	{
		public CraftingDisciplineType Id { get; set; }

		public int MaxRating { get; set; }

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
	}
}
