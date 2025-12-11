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
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			Id = skill.get_Id();
			Name = skill.get_Name();
			AssetId = skill.get_Icon()?.GetAssetIdFromRenderUrl();
			Professions = skill.get_Professions().ToList();
			Slot = skill.get_Slot()?.ToEnum();
		}
	}
}
