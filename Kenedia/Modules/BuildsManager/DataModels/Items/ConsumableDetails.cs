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
		private AsyncTexture2D _icon;

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
				if (_icon != null)
				{
					return _icon;
				}
				_icon = AsyncTexture2D.FromAssetId(AssetId);
				return _icon;
			}
		}
	}
}
