using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.Core.DataModels
{
	[DataContract]
	public class BaseSkill
	{
		private AsyncTexture2D _icon;

		[DataMember]
		public SkillSlot? Slot { get; set; }

		[DataMember]
		public int Id { get; set; }

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
		public LocalizedString Descriptions { get; protected set; } = new LocalizedString();


		public string Description
		{
			get
			{
				return Descriptions.Text;
			}
			set
			{
				Descriptions.Text = value;
			}
		}

		[DataMember]
		public List<string> Professions { get; set; } = new List<string>();


		[DataMember]
		public int? AssetId { get; set; }

		public AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null || !AssetId.HasValue)
				{
					return _icon;
				}
				_icon = AsyncTexture2D.FromAssetId(AssetId.Value);
				return _icon;
			}
		}

		public BaseSkill()
		{
		}

		public BaseSkill(Skill skill)
		{
			Id = skill.Id;
			Name = skill.Name;
			AssetId = skill.Icon?.GetAssetIdFromRenderUrl();
			Professions = skill.Professions.ToList();
			Slot = skill.Slot?.ToEnum();
		}
	}
}
