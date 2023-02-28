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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			Id = skill.get_Id();
			Name = skill.get_Name();
			AssetId = skill.get_Icon()?.GetAssetIdFromRenderUrl();
			Professions = skill.get_Professions().ToList();
			Slot = skill.get_Slot()?.ToEnum();
		}
	}
}
