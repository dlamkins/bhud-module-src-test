using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.Core.DataModels
{
	[DataContract]
	public class BaseSkill
	{
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
				if (_003CIcon_003Ek__BackingField != null || !AssetId.HasValue)
				{
					return _003CIcon_003Ek__BackingField;
				}
				_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(AssetId.Value);
				return _003CIcon_003Ek__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				_003CIcon_003Ek__BackingField = value;
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
