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
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					//IL_0062: Unknown result type (might be due to invalid IL or missing references)
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0074: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0086: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0098: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
					if (Rarity.isSet)
					{
						return __Rarity;
					}
					if (Rarity != null)
					{
						switch (Rarity.Value)
						{
						case 0:
							__Rarity = (ItemRarity)0;
							break;
						case 1:
							__Rarity = (ItemRarity)1;
							break;
						case 2:
							__Rarity = (ItemRarity)2;
							break;
						case 3:
							__Rarity = (ItemRarity)3;
							break;
						case 4:
							__Rarity = (ItemRarity)4;
							break;
						case 5:
							__Rarity = (ItemRarity)5;
							break;
						case 6:
							__Rarity = (ItemRarity)6;
							break;
						case 7:
							__Rarity = (ItemRarity)7;
							break;
						case 8:
							__Rarity = (ItemRarity)8;
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
