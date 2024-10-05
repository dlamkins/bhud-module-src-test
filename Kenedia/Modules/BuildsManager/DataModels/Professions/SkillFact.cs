using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class SkillFact
	{
		private AsyncTexture2D _icon;

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

		public AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null)
				{
					return _icon;
				}
				_icon = AsyncTexture2D.FromAssetId(IconAssetId);
				return _icon;
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
