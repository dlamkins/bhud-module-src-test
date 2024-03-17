using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Items
{
	public class Item
	{
		public const string LAST_SCHEMA_CHANGE = "2022-09-20";

		public int Id { get; }

		public string Name { get; }

		public string? Description { get; set; }

		public ItemType Type { get; set; }

		public ItemRarity Rarity { get; set; }

		public ItemFlag[] Flags { get; set; }

		public string ChatLink { get; set; }

		public string Icon { get; set; }

		public Item(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public static Item FromAPI(Item apiItem)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			Item obj = new Item(apiItem.get_Id(), apiItem.get_Name())
			{
				Description = apiItem.get_Description(),
				Type = (ItemType)((!(apiItem.get_Type()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<ItemType>.op_Implicit(apiItem.get_Type())) : 0),
				Rarity = (ItemRarity)((!(apiItem.get_Rarity()?.get_IsUnknown() ?? true)) ? ((int)ApiEnum<ItemRarity>.op_Implicit(apiItem.get_Rarity())) : 0),
				ChatLink = apiItem.get_ChatLink()
			};
			RenderUrl icon = apiItem.get_Icon();
			obj.Icon = ((RenderUrl)(ref icon)).get_Url()?.AbsoluteUri;
			obj.Flags = (from flag in ((IEnumerable<ApiEnum<ItemFlag>>)apiItem.get_Flags())?.Where((ApiEnum<ItemFlag> flag) => !flag.get_IsUnknown())
				select flag.get_Value()).ToArray();
			return obj;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", Id, Name ?? "Unknown");
		}
	}
}
