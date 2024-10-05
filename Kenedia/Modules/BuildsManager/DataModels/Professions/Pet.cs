using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Pet : IDisposable, IBaseApiData, IDataMember
	{
		private bool _isDisposed;

		private readonly List<int> _aquaticPets = new List<int>(23)
		{
			1, 5, 6, 7, 9, 11, 12, 18, 19, 20,
			21, 23, 24, 25, 26, 27, 40, 41, 42, 43,
			45, 47, 63
		};

		private readonly List<int> _terrestrialPets = new List<int>(54)
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
			22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
			32, 33, 34, 35, 36, 37, 38, 39, 44, 45,
			46, 47, 48, 51, 52, 54, 55, 57, 59, 61,
			63, 64, 65, 66
		};

		private AsyncTexture2D _icon;

		private AsyncTexture2D _selectedIcon;

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
		public int IconAssetId { get; set; }

		public AsyncTexture2D Icon
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

		public AsyncTexture2D SelectedIcon
		{
			get
			{
				if (_selectedIcon != null)
				{
					return _selectedIcon;
				}
				int assetId = ((IconAssetId == 52565) ? 1769874 : (IconAssetId + 1));
				_selectedIcon = AsyncTexture2D.FromAssetId(assetId);
				return _selectedIcon;
			}
		}

		[DataMember]
		public Dictionary<int, Skill> Skills { get; set; } = new Dictionary<int, Skill>();


		[DataMember]
		public Enviroment Enviroment { get; set; }

		[DataMember]
		public int Order { get; set; }

		public Pet()
		{
		}

		public Pet(Gw2Sharp.WebApi.V2.Models.Pet pet)
		{
			Apply(pet);
		}

		public Pet(Gw2Sharp.WebApi.V2.Models.Pet pet, List<Skill> skills)
			: this(pet)
		{
			foreach (PetSkill petSkill in pet.Skills)
			{
				Skill skill = skills.Find((Skill e) => e.Id == petSkill.Id);
				if (skill != null)
				{
					Skills.Add(petSkill.Id, skill);
				}
			}
			ApplyLanguage(pet, skills);
		}

		public static Pet FromByte(byte id)
		{
			Pet pet = default(Pet);
			if (!(BuildsManager.Data.Pets?.TryGetValue(id, out pet) ?? false))
			{
				return null;
			}
			return pet;
		}

		public void ApplyLanguage(Gw2Sharp.WebApi.V2.Models.Pet pet)
		{
			Name = pet.Name;
			Description = pet.Description;
		}

		public void ApplyLanguage(Gw2Sharp.WebApi.V2.Models.Pet pet, List<Skill> skills)
		{
			ApplyLanguage(pet);
			foreach (KeyValuePair<int, Skill> petSkill in Skills)
			{
				Skill skill = skills.Find((Skill e) => e.Id == petSkill.Value.Id);
				if (skill != null)
				{
					petSkill.Value.Name = skill.Name;
					petSkill.Value.Description = skill.Description;
				}
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_icon = null;
				_selectedIcon = null;
				Skills?.Values?.DisposeAll();
				Skills?.Clear();
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Pet pet)
		{
			Id = pet.Id;
			IconAssetId = pet.Icon.GetAssetIdFromRenderUrl();
			bool aquatic = _aquaticPets.Contains(pet.Id);
			bool terrestrial = _terrestrialPets.Contains(pet.Id);
			Enviroment = (terrestrial ? Enviroment.Terrestrial : ((Enviroment)0)) | (aquatic ? Enviroment.Aquatic : ((Enviroment)0));
			List<int> petOrder = new List<int>
			{
				13, 14, 15, 16, 17, 5, 20, 23, 24, 25,
				1, 3, 9, 11, 47, 63, 54, 55, 52, 66,
				4, 8, 22, 28, 29, 7, 12, 18, 19, 45,
				6, 26, 27, 10, 30, 31, 32, 44, 65, 57,
				33, 34, 35, 36, 2, 37, 38, 64, 39, 59,
				48, 51, 46, 61, 21, 40, 42, 41, 43
			};
			Order = petOrder.IndexOf(pet.Id);
			ApplyLanguage(pet);
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Pet pet, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> skills)
		{
			Apply(pet);
			foreach (PetSkill petSkill in pet.Skills)
			{
				Gw2Sharp.WebApi.V2.Models.Skill skill = skills.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id == petSkill.Id);
				if (skill != null)
				{
					Skills.Add(petSkill.Id, new Skill(skill));
				}
			}
		}
	}
}
