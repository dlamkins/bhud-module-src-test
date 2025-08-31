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
		private enum Pets
		{
			JungleStalker = 1,
			Boar = 2,
			Lynx = 3,
			KrytanDrakehound = 4,
			BrownBear = 5,
			CarrionDevourer = 6,
			SalamanderDrake = 7,
			AlpineWolf = 8,
			SnowLeopard = 9,
			Raven = 10,
			Jaguar = 11,
			MarshDrake = 12,
			BlueMoa = 13,
			WhiteMoa = 14,
			PinkMoa = 0xF,
			BlackMoa = 0x10,
			RedMoa = 17,
			IceDrake = 18,
			RiverDrake = 19,
			Murellow = 20,
			Shark = 21,
			FernHound = 22,
			BlackBear = 23,
			PolarBear = 24,
			Arctodus = 25,
			WhiptailDevourer = 26,
			LashtailDevourer = 27,
			Hyena = 28,
			Wolf = 29,
			Owl = 30,
			Eagle = 0x1F,
			WhiteRaven = 0x20,
			ForestSpider = 33,
			JungleSpider = 34,
			CaveSpider = 35,
			BlackWidowSpider = 36,
			Warthog = 37,
			Siamoth = 38,
			Pig = 39,
			ArmorFish = 40,
			BlueJellyfish = 41,
			RedJellyfish = 42,
			RainbowJellyfish = 43,
			Hawk = 44,
			ReefDrake = 45,
			Smokescale = 46,
			Tiger = 47,
			ElectricWyvern = 48,
			FireWyvern = 51,
			Bristleback = 52,
			Cheetah = 54,
			SandLion = 55,
			Jacaranda = 57,
			RockGazelle = 59,
			FangedIboga = 61,
			WhiteTiger = 0x3F,
			Wallow = 0x40,
			Phoenix = 65,
			SiegeTurtle = 66,
			AetherHunter = 67,
			SkyChakStriker = 68,
			Spinegazer = 69,
			Warclaw = 70,
			JanthiriBee = 71
		}

		private bool _isDisposed;

		private readonly List<Pets> _aquaticPets = new List<Pets>(23)
		{
			Pets.JungleStalker,
			Pets.BrownBear,
			Pets.CarrionDevourer,
			Pets.SalamanderDrake,
			Pets.SnowLeopard,
			Pets.Jaguar,
			Pets.MarshDrake,
			Pets.IceDrake,
			Pets.RiverDrake,
			Pets.Murellow,
			Pets.Shark,
			Pets.BlackBear,
			Pets.PolarBear,
			Pets.Arctodus,
			Pets.WhiptailDevourer,
			Pets.LashtailDevourer,
			Pets.ArmorFish,
			Pets.BlueJellyfish,
			Pets.RedJellyfish,
			Pets.RainbowJellyfish,
			Pets.ReefDrake,
			Pets.Tiger,
			Pets.WhiteTiger
		};

		private readonly List<Pets> _terrestrialPets = new List<Pets>(59)
		{
			Pets.JungleStalker,
			Pets.Boar,
			Pets.Lynx,
			Pets.KrytanDrakehound,
			Pets.BrownBear,
			Pets.CarrionDevourer,
			Pets.SalamanderDrake,
			Pets.AlpineWolf,
			Pets.SnowLeopard,
			Pets.Raven,
			Pets.Jaguar,
			Pets.MarshDrake,
			Pets.BlueMoa,
			Pets.WhiteMoa,
			Pets.PinkMoa,
			Pets.BlackMoa,
			Pets.RedMoa,
			Pets.IceDrake,
			Pets.RiverDrake,
			Pets.Murellow,
			Pets.FernHound,
			Pets.BlackBear,
			Pets.PolarBear,
			Pets.Arctodus,
			Pets.WhiptailDevourer,
			Pets.LashtailDevourer,
			Pets.Hyena,
			Pets.Wolf,
			Pets.Owl,
			Pets.Eagle,
			Pets.WhiteRaven,
			Pets.ForestSpider,
			Pets.JungleSpider,
			Pets.CaveSpider,
			Pets.BlackWidowSpider,
			Pets.Warthog,
			Pets.Siamoth,
			Pets.Pig,
			Pets.Hawk,
			Pets.ReefDrake,
			Pets.Smokescale,
			Pets.Tiger,
			Pets.ElectricWyvern,
			Pets.FireWyvern,
			Pets.Bristleback,
			Pets.Cheetah,
			Pets.SandLion,
			Pets.Jacaranda,
			Pets.RockGazelle,
			Pets.FangedIboga,
			Pets.WhiteTiger,
			Pets.Wallow,
			Pets.Phoenix,
			Pets.SiegeTurtle,
			Pets.AetherHunter,
			Pets.SkyChakStriker,
			Pets.Spinegazer,
			Pets.Warclaw,
			Pets.JanthiriBee
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
			Pets pet_enum = (Pets)pet.Id;
			bool aquatic = _aquaticPets.Contains(pet_enum);
			bool terrestrial = _terrestrialPets.Contains(pet_enum);
			Enviroment = (terrestrial ? Enviroment.Terrestrial : ((Enviroment)0)) | (aquatic ? Enviroment.Aquatic : ((Enviroment)0));
			List<int> petOrder = new List<int>
			{
				13, 14, 15, 16, 17, 5, 20, 23, 24, 25,
				1, 3, 9, 11, 47, 63, 54, 55, 70, 52,
				66, 4, 8, 22, 28, 29, 7, 12, 18, 19,
				45, 6, 26, 27, 10, 30, 31, 32, 44, 65,
				57, 33, 34, 35, 36, 2, 37, 38, 64, 39,
				59, 48, 51, 46, 61, 21, 40, 42, 41, 43,
				67, 68, 69, 71
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
