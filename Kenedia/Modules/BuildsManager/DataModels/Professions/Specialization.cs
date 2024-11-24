using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Specialization : IDisposable
	{
		private bool _isDisposed;

		private AsyncTexture2D _icon;

		private AsyncTexture2D _background;

		private AsyncTexture2D _profession_icon;

		private AsyncTexture2D _profession_icon_big;

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public bool Elite { get; set; }

		[DataMember]
		public ProfessionType Profession { get; set; }

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
		public int IconAssetId { get; set; }

		private AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null)
				{
					return _icon;
				}
				if (IconAssetId != 0)
				{
					_icon = AsyncTexture2D.FromAssetId(IconAssetId);
				}
				return _icon;
			}
		}

		[DataMember]
		public int BackgroundAssetId { get; set; }

		private AsyncTexture2D Background
		{
			get
			{
				if (_background != null)
				{
					return _background;
				}
				if (BackgroundAssetId != 0)
				{
					_background = AsyncTexture2D.FromAssetId(BackgroundAssetId);
				}
				return _background;
			}
		}

		[DataMember]
		public int? ProfessionIconAssetId { get; set; }

		private AsyncTexture2D ProfessionIcon
		{
			get
			{
				if (_profession_icon != null)
				{
					return _profession_icon;
				}
				if ((ProfessionIconAssetId ?? 0) != 0)
				{
					_profession_icon = AsyncTexture2D.FromAssetId(ProfessionIconAssetId.Value);
				}
				return _profession_icon;
			}
		}

		[DataMember]
		public int? ProfessionIconBigAssetId { get; set; }

		private AsyncTexture2D ProfessionIconBig
		{
			get
			{
				if (_profession_icon_big != null)
				{
					return _profession_icon_big;
				}
				if ((ProfessionIconBigAssetId ?? 0) != 0)
				{
					_profession_icon_big = AsyncTexture2D.FromAssetId(ProfessionIconBigAssetId.Value);
				}
				return _profession_icon_big;
			}
		}

		[DataMember]
		public Dictionary<int, Trait> MinorTraits { get; } = new Dictionary<int, Trait>();


		[DataMember]
		public Dictionary<int, Trait> MajorTraits { get; } = new Dictionary<int, Trait>();


		[DataMember]
		public Trait WeaponTrait { get; set; }

		public Specialization()
		{
		}

		public Specialization(Gw2Sharp.WebApi.V2.Models.Specialization specialization)
		{
			Apply(specialization);
		}

		public Specialization(Gw2Sharp.WebApi.V2.Models.Specialization specialization, Dictionary<int, Trait> traits)
			: this(specialization)
		{
			if (!Enum.TryParse<ProfessionType>(specialization.Profession, out var _))
			{
				return;
			}
			int index = 0;
			foreach (int t2 in specialization.MajorTraits)
			{
				if (traits.TryGetValue(t2, out var trait2))
				{
					trait2.Index = index;
					MajorTraits.Add(t2, trait2);
				}
				index++;
			}
			index = 0;
			foreach (int t in specialization.MinorTraits)
			{
				if (traits.TryGetValue(t, out var trait))
				{
					trait.Index = index;
					MinorTraits.Add(t, trait);
				}
				index++;
			}
			if (specialization.WeaponTrait.HasValue && traits.TryGetValue(specialization.WeaponTrait.Value, out var weaponTrait))
			{
				WeaponTrait = weaponTrait;
			}
		}

		public static Specialization FromByte(byte spezializationId, ProfessionType profession, Data data)
		{
			ProfessionDataEntry professions = data.Professions;
			Specialization specialization = default(Specialization);
			if (professions == null || professions[profession]?.Specializations.TryGetValue(spezializationId, out specialization) != true)
			{
				return null;
			}
			return specialization;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_icon = null;
				_background = null;
				_profession_icon = null;
				_profession_icon_big = null;
				WeaponTrait?.Dispose();
				WeaponTrait = null;
				MinorTraits?.Values?.DisposeAll();
				MinorTraits.Clear();
				MajorTraits?.Values?.DisposeAll();
				MajorTraits.Clear();
			}
		}

		public void Apply(Gw2Sharp.WebApi.V2.Models.Specialization specialization)
		{
			if (Enum.TryParse<ProfessionType>(specialization.Profession, out var profession))
			{
				Profession = profession;
			}
			Id = specialization.Id;
			Elite = specialization.Elite;
			Name = specialization.Name;
			IconAssetId = specialization.Icon.GetAssetIdFromRenderUrl();
			BackgroundAssetId = specialization.Background.GetAssetIdFromRenderUrl();
			ProfessionIconAssetId = specialization.ProfessionIcon?.GetAssetIdFromRenderUrl();
			ProfessionIconBigAssetId = specialization.ProfessionIconBig?.GetAssetIdFromRenderUrl();
		}

		public void Apply(Gw2Sharp.WebApi.V2.Models.Specialization specialization, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Trait> traits)
		{
			Gw2Sharp.WebApi.V2.Models.Specialization specialization2 = specialization;
			Apply(specialization2);
			if (!Enum.TryParse<ProfessionType>(specialization2.Profession, out var _))
			{
				return;
			}
			int index = 0;
			foreach (int t2 in specialization2.MajorTraits)
			{
				Trait trait3;
				bool num = MajorTraits.TryGetValue(t2, out trait3);
				if (trait3 == null)
				{
					trait3 = new Trait();
				}
				Gw2Sharp.WebApi.V2.Models.Trait apiTrait2 = traits.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Trait x) => x.Id == t2);
				trait3.Apply(apiTrait2);
				trait3.Index = index;
				if (!num)
				{
					MajorTraits.Add(t2, trait3);
				}
				index++;
			}
			index = 0;
			foreach (int t in specialization2.MinorTraits)
			{
				Trait trait2;
				bool num2 = MinorTraits.TryGetValue(t, out trait2);
				if (trait2 == null)
				{
					trait2 = new Trait();
				}
				Gw2Sharp.WebApi.V2.Models.Trait apiTrait = traits.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Trait x) => x.Id == t);
				trait2.Apply(apiTrait);
				trait2.Index = index;
				if (!num2)
				{
					MinorTraits.Add(t, trait2);
				}
				index++;
			}
			if (!specialization2.WeaponTrait.HasValue)
			{
				return;
			}
			Gw2Sharp.WebApi.V2.Models.Trait trait = traits.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Trait e) => e.Id == specialization2.WeaponTrait.Value);
			if (trait != null)
			{
				if (WeaponTrait == null)
				{
					Trait trait5 = (WeaponTrait = new Trait());
				}
				WeaponTrait?.Apply(trait);
			}
		}
	}
}
