using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Trait : IDisposable
	{
		private bool _isDisposed;

		[DataMember]
		public List<int> Skills = new List<int>();

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int Index { get; set; }

		[DataMember]
		public int Specialization { get; set; }

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
		public string ChatLink { get; set; }

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
				if (IconAssetId != 0)
				{
					_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconAssetId);
				}
				return _003CIcon_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIcon_003Ek__BackingField = value;
			}
		}

		[DataMember]
		public TraitSlot Type { get; set; }

		[DataMember]
		public TraitTierType Tier { get; set; }

		[DataMember]
		public int Order { get; set; }

		public List<TraitFact> Facts { get; private set; }

		public List<TraitFact> TraitedFacts { get; private set; }

		public Trait()
		{
		}

		public Trait(Gw2Sharp.WebApi.V2.Models.Trait trait)
		{
			Apply(trait);
		}

		public void SetLiveAPI(Gw2Sharp.WebApi.V2.Models.Trait trait)
		{
			Facts = trait.Facts?.ToList();
			TraitedFacts = trait.TraitedFacts?.ToList();
		}

		internal static Trait FromByte(byte order, Specialization specialization, TraitTierType tier)
		{
			if (order != 0)
			{
				return specialization?.MajorTraits.Where<KeyValuePair<int, Trait>>((KeyValuePair<int, Trait> e) => e.Value.Tier == tier)?.ToList()?.Find((KeyValuePair<int, Trait> e) => e.Value.Order == order - 1).Value;
			}
			return null;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Icon = null;
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Trait trait)
		{
			Id = trait.Id;
			Name = trait.Name;
			Description = trait.Description;
			Specialization = trait.Specialization;
			IconAssetId = trait.Icon.GetAssetIdFromRenderUrl();
			Tier = (TraitTierType)trait.Tier;
			Order = trait.Order;
			Type = trait.Slot!.Value;
			ChatLink = trait.CreateChatLink();
			if (trait.Skills == null)
			{
				return;
			}
			foreach (TraitSkill s in trait.Skills!)
			{
				Skills.Add(s.Id);
			}
		}
	}
}
