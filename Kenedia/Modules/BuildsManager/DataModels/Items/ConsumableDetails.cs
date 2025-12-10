using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class ConsumableDetails
	{
		[DataMember]
		public ItemConsumableType Type { get; set; }

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
				return Descriptions.Text.InterpretItemDescription();
			}
			set
			{
				Descriptions.Text = value;
			}
		}

		[DataMember]
		public int? DurationMs { get; set; }

		[DataMember]
		public int AssetId { get; protected set; }

		public AsyncTexture2D Icon
		{
			get
			{
				if (_003CIcon_003Ek__BackingField != null)
				{
					return _003CIcon_003Ek__BackingField;
				}
				_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(AssetId);
				return _003CIcon_003Ek__BackingField;
			}
		}
	}
}
