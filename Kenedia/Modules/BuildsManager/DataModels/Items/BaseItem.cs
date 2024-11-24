using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class BaseItem : IDataMember
	{
		private AsyncTexture2D _icon;

		[DataMember]
		public Kenedia.Modules.Core.DataModels.ItemType Type { get; set; }

		[DataMember]
		public TemplateSlotType TemplateSlot { get; set; }

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public byte MappedId { get; set; }

		[DataMember]
		public ItemRarity Rarity { get; set; }

		[DataMember]
		public string Chatlink { get; set; }

		[DataMember]
		public int AssetId { get; set; }

		private AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null)
				{
					return _icon;
				}
				if (AssetId != 0)
				{
					_icon = AsyncTexture2D.FromAssetId(AssetId);
				}
				return _icon;
			}
		}

		[DataMember]
		public LocalizedString Names { get; protected set; } = new LocalizedString();


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
		public LocalizedString DisplayTexts { get; protected set; } = new LocalizedString();


		public string DisplayText
		{
			get
			{
				return DisplayTexts.Text;
			}
			set
			{
				DisplayTexts.Text = value;
			}
		}

		[DataMember]
		public LocalizedString Descriptions { get; protected set; } = new LocalizedString();


		public string Description
		{
			get
			{
				return Descriptions.Text?.InterpretItemDescription();
			}
			set
			{
				Descriptions.Text = value;
			}
		}

		public BaseItem()
		{
		}

		public BaseItem(Item item)
		{
			Rarity = item.Rarity;
			Chatlink = item.ChatLink;
			AssetId = item.Icon.GetAssetIdFromRenderUrl();
			Id = item.Id;
			Name = item.Name;
			Description = item.Description;
			Type = item.Type.ToItemType();
		}

		public BaseItem(Item item, TemplateSlotType templateSlot)
			: this(item)
		{
			TemplateSlot = templateSlot;
		}

		public virtual void Apply(Item item)
		{
			Id = item.Id;
			Name = item.Name;
			Description = item.Description;
			AssetId = item.Icon.GetAssetIdFromRenderUrl();
			Rarity = item.Rarity;
			Chatlink = item.ChatLink;
			Type = item.Type.ToItemType();
			DisplayText = item.Name;
		}

		public virtual void Apply(Gw2Sharp.WebApi.V2.Models.PvpAmulet amulet)
		{
			Id = amulet.Id;
			Name = amulet.Name;
			AssetId = amulet.Icon.GetAssetIdFromRenderUrl();
			DisplayText = amulet.Name;
			Rarity = ItemRarity.Basic;
			Type = Kenedia.Modules.Core.DataModels.ItemType.PvpAmulet;
		}

		public void SetAssetId(int id)
		{
			AssetId = id;
		}
	}
}
