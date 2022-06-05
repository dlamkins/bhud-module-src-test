using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class GW2API
	{
		public class BaseObject
		{
			public int? Id;

			public string Name;
		}

		public class LegendaryItem
		{
			public class iType
			{
				public bool IsUnknown;

				public int Value;

				public string RawValue;
			}

			public class iDetails
			{
				public List<int> StatChoices;

				public iType Type;
			}

			public class iIcon
			{
				public string Url;
			}

			public class iRarity
			{
				public bool isSet;

				public bool IsUnknown;

				public int Value;

				public string RawValue;
			}

			public string Name;

			public iIcon Icon;

			public string ChatLink;

			public int Id;

			public iDetails Details;

			public iType Type;

			public iRarity Rarity;

			public ItemRarity __Rarity;

			public Texture2D Texture;

			public ItemRarity _Rarity
			{
				get
				{
					if (Rarity.isSet)
					{
						return __Rarity;
					}
					if (Rarity != null)
					{
						switch (Rarity.Value)
						{
						case 0:
							__Rarity = ItemRarity.Unknown;
							break;
						case 1:
							__Rarity = ItemRarity.Junk;
							break;
						case 2:
							__Rarity = ItemRarity.Basic;
							break;
						case 3:
							__Rarity = ItemRarity.Fine;
							break;
						case 4:
							__Rarity = ItemRarity.Masterwork;
							break;
						case 5:
							__Rarity = ItemRarity.Rare;
							break;
						case 6:
							__Rarity = ItemRarity.Exotic;
							break;
						case 7:
							__Rarity = ItemRarity.Ascended;
							break;
						case 8:
							__Rarity = ItemRarity.Legendary;
							break;
						}
					}
					Rarity.isSet = true;
					return __Rarity;
				}
			}
		}

		public class Type
		{
			public string Value;

			public string RawValue;
		}

		public class intType
		{
			public int Value;

			public string RawValue;
		}

		public class Slot
		{
			public int Value;

			public string RawValue;
		}

		public class Rarity
		{
			public string Value;

			public string RawValue;
		}

		public class Fact
		{
			public double? Percent;

			public string Text;

			public string Description;

			public string Status;

			public Icon Icon;

			public Type Type;

			public int? RequiresTrait;

			public int? Overrides;

			public int? ApplyCount;

			public int? Duration;
		}

		public class Flag
		{
			public string Value;

			public string RawValue;
		}

		public class intFlag
		{
			public int Value;

			public string RawValue;
		}

		public class Attribute
		{
			public int Value;

			public string RawValue;
		}

		public class WeightClass
		{
			public int Value;

			public string RawValue;
		}

		public class Stat
		{
			public Attribute Attribute;

			public string Value;

			public double Multiplier;
		}

		public class Stats : BaseObject
		{
			public List<Stat> Attributes;
		}

		public class Details
		{
			public Type Type;

			public List<Flag> Flags;

			public List<string> Bonuses;

			public List<int> StatChoices;

			public double? AttributeAdjustment;

			public string Description;
		}

		public class intDetails
		{
			public intType Type;

			public List<Flag> Flags;

			public List<string> Bonuses;

			public List<int> StatChoices;

			public double? AttributeAdjustment;

			public string Description;

			public WeightClass WeightClass;
		}

		public class Icon
		{
			public string Path;

			public string Url;
		}

		public class ProfessionSkill
		{
			public int Id;
		}

		public class ProfessionWeapon
		{
			public int Specialization;

			public List<intFlag> Flags;
		}

		public class Profession : BaseObject
		{
			public new string Id;

			public IReadOnlyDictionary<int, int> SkillsByPalette;

			public List<ProfessionSkill> Skills;

			public List<Flag> Flags;

			public List<int> Specializations;

			public Icon IconBig;

			public Icon Icon;

			public int Code;

			public IReadOnlyDictionary<string, ProfessionWeapon> Weapons;
		}

		public class Item : BaseObject
		{
			public string Description;

			public intDetails Details;

			public Icon Icon;

			public List<Flag> Flags;

			public string ChatLink;

			public Rarity Rarity;

			public intType Type;
		}

		public class Skill : BaseObject
		{
			public int? Specialization;

			public string Description;

			public Slot Slot;

			public Type Type;

			public List<string> Categories;

			public Icon Icon;

			public int PaletteID;

			public List<Flag> Flags;

			public string ChatLink;
		}

		public class Trait : BaseObject
		{
			public string Description;

			public int? Specialization;

			public int? Tier;

			public int? Order;

			public Slot Slot;

			public List<Fact> Facts;

			public List<Fact> TraitedFacts;

			public List<string> Categories;

			public Icon Icon;

			public List<Flag> Flags;

			public string ChatLink;
		}

		public class Specialization : BaseObject
		{
			public string Profession;

			public bool Elite;

			public List<int> MinorTraits;

			public List<int> MajorTraits;

			public int? WeaponTrait;

			public Icon Icon;

			public Icon Background;

			public Icon ProfessionIconBig;

			public Icon ProfessionIcon;

			public List<Trait> Traits = new List<Trait>();
		}

		public static string BasePath;
	}
}
