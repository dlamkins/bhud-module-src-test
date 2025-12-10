using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class SkillFact
	{
		[DataMember]
		public int? RequiresTrait { get; set; }

		[DataMember]
		public int? Overrides { get; set; }

		[DataMember]
		public SkillFactType Type { get; }

		[DataMember]
		public LocalizedString Texts { get; protected set; } = new LocalizedString();


		public string Text
		{
			get
			{
				return Texts.Text;
			}
			set
			{
				Texts.Text = value;
			}
		}

		[DataMember]
		public int IconAssetId { get; set; }

		private AsyncTexture2D Icon
		{
			get
			{
				if (_003CIcon_003Ek__BackingField != null)
				{
					return _003CIcon_003Ek__BackingField;
				}
				_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconAssetId);
				return _003CIcon_003Ek__BackingField;
			}
		}

		public SkillFact()
		{
		}

		public SkillFact(Gw2Sharp.WebApi.V2.Models.SkillFact fact)
		{
			Text = fact.Text;
			IconAssetId = fact.Icon.GetAssetIdFromRenderUrl();
			RequiresTrait = fact.RequiresTrait;
			Type = fact.Type.Value;
			Overrides = fact.Overrides;
		}
	}
}
