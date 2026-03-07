using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	internal class Magic
	{
		public enum AdviceType
		{
			stackAdvice,
			vendorAdvice,
			rareSalvageAdvice,
			craftLuckAdvice,
			deletableAdvice,
			salvageAdvice,
			consumableAdvice,
			gobblerAdvice,
			karmaAdvice,
			craftingAdvice,
			lwsAdvice,
			miscAdvice
		}

		public enum StorageType
		{
			materialStorage,
			bankStorage,
			sharedStorage
		}

		public enum SalvageKit
		{
			CrudeSalvageKit = 23038,
			BasicSalvageKit = 23040,
			CopperSalvageKit = 44602,
			FineSalvageKit = 23041,
			JourneySalvageKit = 23042,
			MasterSalvageKit = 23043,
			MysticSalvageKit = 23045,
			SilverSalvageKit = 67027,
			RuneSalvageKit = 89409,
			AscendedSalvageTool = 75284,
			AscendedSalvageKit = 73481,
			AscendedSalvageKit5 = 79105
		}

		private static Locale currentLocale = Locale.English;

		public static int ectoId = 19721;

		public static double salvagePrice = 0.10496;

		public static double ectoChance = 0.875;

		public static double tax = 0.85;

		public static LUT jsonLut = null;

		public static localeLut localeItemNamesLut = null;

		public static Logger log = null;

		public static Translation translation = new Translation();

		public static MagicLists magicLists = new MagicLists();

		public static ItemInfo unknown = new ItemInfo
		{
			Id = 0,
			Name = "Item unknown to LUT",
			IconId = 63369,
			Rarity = 0,
			Description = null,
			Type = 0,
			isFoodOrUtility = false,
			Flags = new List<int>(),
			Level = 0
		};

		public static Item silkBag = new Item(9566, isCharacterBound_: false, isAccountBound_: false, delayedCreate: true);

		public static Item borealTrunk = new Item(92292, isCharacterBound_: false, isAccountBound_: false, delayedCreate: true);

		private static Dictionary<int, string> luckNameMapping = new Dictionary<int, string>
		{
			{ 45175, "Essence of Luck (fine)" },
			{ 45176, "Essence of Luck (masterwork)" },
			{ 45177, "Essence of Luck (rare)" },
			{ 45178, "Essence of Luck (exotic)" },
			{ 45179, "Essence of Luck (legendary)" }
		};

		public static Dictionary<int, string> gameplayConsumables = new Dictionary<int, string>
		{
			{ 78758, "Trade to get bounty for bandit leader." },
			{ 78886, "Have in inventory while defeating a bandit leader to spawn the Legendary Bandit Executioner" },
			{ 84335, "Use during a treasure hunt meta in Desert Highlands to spawn chests" },
			{ 67826, "Use in the Silverwastes after a meta completes to spawn chests. Make sure you have required keys." },
			{ 67979, "Open a greater nightmare pod in the Silverwastes after completing meta." },
			{ 67818, "Use during breach event in Silverwastes." },
			{ 67780, "Open Tarnished chest in Silverwastes." },
			{ 87517, "Open krait Sunken Chests to progress a Master Diver achievement." },
			{ 48716, "Open chests in the Aetherpath of the Twilight Arbor dungeon." },
			{ 78782, "Complete this bounty." },
			{ 78754, "Complete this bounty." },
			{ 78786, "Complete this bounty." },
			{ 78784, "Complete this bounty." },
			{ 78781, "Complete this bounty." },
			{ 78883, "Complete this bounty." },
			{ 78859, "Complete this bounty." },
			{ 78988, "Complete this bounty." },
			{ 78867, "Complete this bounty." },
			{ 78954, "Complete this bounty." },
			{ 71627, "Complete events in the Verdant Brink." },
			{ 75024, "Complete events in the Auric Basin." },
			{ 71207, "Complete events in the Tangled Depths." },
			{ 87630, "Contribute Spare Parts to kick off meta event in the Domain of Kourna." },
			{ 93407, "Use in the Drizzlewood Coast to spawn chests. Make sure you have required keys." },
			{ 93371, "Use to unlock achievements (and play in Drizzlewood Coast)" },
			{ 93817, "Use to unlock achievements (and play  Drizzlewood Coast)}, or just delete/tp when you are done." },
			{ 93842, "Use to unlock achievements (and play  Drizzlewood Coast)}, or just delete/tp when you are done." },
			{ 93799, "Use to unlock achievements (and play  Drizzlewood Coast)}, or just delete/tp when you are done." }
		};

		private static Dictionary<CraftingDisciplineType, string> disciplineNameMapping = new Dictionary<CraftingDisciplineType, string>
		{
			{
				CraftingDisciplineType.Scribe,
				"Scribe"
			},
			{
				CraftingDisciplineType.Tailor,
				"Tailor"
			},
			{
				CraftingDisciplineType.Leatherworker,
				"Leatherworker"
			},
			{
				CraftingDisciplineType.Weaponsmith,
				"Weaponsmith"
			},
			{
				CraftingDisciplineType.Armorsmith,
				"Armorsmith"
			},
			{
				CraftingDisciplineType.Artificer,
				"Artificer"
			},
			{
				CraftingDisciplineType.Chef,
				"Chef"
			},
			{
				CraftingDisciplineType.Jeweler,
				"Jeweler"
			},
			{
				CraftingDisciplineType.Huntsman,
				"Huntsman"
			},
			{
				CraftingDisciplineType.Unknown,
				"Unknown"
			}
		};

		public static Dictionary<AdviceType, string> adviceTypeNameMapping = new Dictionary<AdviceType, string>
		{
			{
				AdviceType.stackAdvice,
				"Stack advice"
			},
			{
				AdviceType.vendorAdvice,
				"Vendor advice"
			},
			{
				AdviceType.rareSalvageAdvice,
				"Rare salvage advice"
			},
			{
				AdviceType.craftLuckAdvice,
				"Craftable luck advice"
			},
			{
				AdviceType.deletableAdvice,
				"Deletable advice"
			},
			{
				AdviceType.salvageAdvice,
				"Salvagable  advice"
			},
			{
				AdviceType.consumableAdvice,
				"Consumable  advice"
			},
			{
				AdviceType.gobblerAdvice,
				"Gobbler  advice"
			},
			{
				AdviceType.karmaAdvice,
				"Karma consumable  advice"
			},
			{
				AdviceType.craftingAdvice,
				"Crafting advice"
			},
			{
				AdviceType.lwsAdvice,
				"Living world advice"
			},
			{
				AdviceType.miscAdvice,
				"Miscellaneous  advice"
			}
		};

		public static Dictionary<StorageType, string> storageTypeNameMapping = new Dictionary<StorageType, string>
		{
			{
				StorageType.materialStorage,
				"Material Storage"
			},
			{
				StorageType.bankStorage,
				"Bank Storage"
			},
			{
				StorageType.sharedStorage,
				"Shared Storage"
			}
		};

		public static Dictionary<int, CraftingMiscAdvice> craftingMiscAdvices = new Dictionary<int, CraftingMiscAdvice>
		{
			{
				9251,
				new CraftingMiscAdvice(new Dictionary<int, int>
				{
					{ 20008, 1 },
					{ 8439, 1 },
					{ 19997, 1 },
					{ 49871, 1 },
					{ 48951, 1 },
					{ 37214, 1 },
					{ 19999, 1 },
					{ 49872, 1 },
					{ 20006, 1 },
					{ 8446, 1 }
				}, "Craft: ", 9251)
			},
			{
				38050,
				new CraftingMiscAdvice(new Dictionary<int, int>
				{
					{ 20016, 1 },
					{ 20010, 1 },
					{ 20015, 1 },
					{ 20013, 1 }
				}, "Craft: ", 38050)
			}
		};

		public static List<int> luckIds = new List<int> { 45175, 45176, 45177 };

		private static List<ItemType> nonStackableTypes = new List<ItemType>
		{
			ItemType.Armor,
			ItemType.Back,
			ItemType.Gathering,
			ItemType.Tool,
			ItemType.Trinket,
			ItemType.Weapon,
			ItemType.Bag,
			ItemType.Container
		};

		private static List<ItemType> salvagableEquipment = new List<ItemType>
		{
			ItemType.Armor,
			ItemType.Back,
			ItemType.Trinket,
			ItemType.Weapon
		};

		private static List<ApiEnum<RecipeType>> pertinentRecipeTypes = new List<ApiEnum<RecipeType>>
		{
			(ApiEnum<RecipeType>)RecipeType.Refinement,
			(ApiEnum<RecipeType>)RecipeType.RefinementEctoplasm,
			(ApiEnum<RecipeType>)RecipeType.RefinementObsidian,
			(ApiEnum<RecipeType>)RecipeType.IngredientCooking
		};

		public static List<Gobbler> gobblers = new List<Gobbler>
		{
			new Gobbler(77093, 46731, 250, "Herta"),
			new Gobbler(66999, 46731, 50, "Mawdrey"),
			new Gobbler(69887, 46733, 50, "Princess"),
			new Gobbler(68369, 46735, 50, "Star of Gratitude"),
			new Gobbler(81780, new List<int> { 46733, 46731, 46735 }, 25, "Gleam of Sentience"),
			new Gobbler(101771, new List<int> { 46733, 46731, 46735 }, 25, "Portable Wizard's Tower Exchange"),
			new Gobbler(83305, 83103, 25, "Spearmarshal's Plea")
		};

		public static List<MiscAdvice> miscAdvices = new List<MiscAdvice>
		{
			new MiscAdvice(43773, 25, "Transform Quartz Crystals into a Charged Quartz Crystal at a place of power."),
			new MiscAdvice(66608, 100, "Sift through silky sand."),
			new MiscAdvice(48717, 4, "Craft 'Completed Aetherkey'."),
			new MiscAdvice(93472, 1, "Consume to get War Supplies"),
			new MiscAdvice(93649, 1, "Consume to get War Supplies"),
			new MiscAdvice(93455, 1, "Consume to get War Supplies"),
			new MiscAdvice(68531, 1, "Consume to get Mordrem parts which can be exchanged for map currency"),
			new MiscAdvice(39752, 250, "Convert to Bauble Bubble"),
			new MiscAdvice(36041, 1000, "Convert to Candy Corn Cob"),
			new MiscAdvice(43319, 1000, "Convert to Jorbreaker")
		};

		public static List<int> lws3Id = new List<int> { 79280, 79469, 79899, 80332, 81127, 81706 };

		public static List<int> lws4Id = new List<int> { 86069, 86977, 87645, 88955, 89537, 90783 };

		public static List<int> ibsId = new List<int> { 92272 };

		public static List<int> wizardGobblers = new List<int>
		{
			66999, 69887, 68369, 67270, 72606, 81512, 79995, 77093, 73718, 79197,
			79558, 80144, 81120, 81780, 83305, 88660, 80672, 86360, 49501, 84440,
			78177, 101771
		};

		public static List<int> wizardScrolls = new List<int>
		{
			76630, 71577, 76827, 73024, 76293, 70968, 76065, 95026, 98007, 79073,
			79456, 79905, 80238, 81060, 81673, 79744, 83305, 85884, 86981, 87624,
			88934, 88765, 89621, 90336, 92694, 91975, 92411, 93343, 97009, 100939,
			20030, 78657, 103998, 92108
		};

		public static List<ItemWeaponType> singularWeaponTypes = new List<ItemWeaponType>
		{
			ItemWeaponType.Rifle,
			ItemWeaponType.Greatsword,
			ItemWeaponType.Hammer,
			ItemWeaponType.LongBow,
			ItemWeaponType.ShortBow,
			ItemWeaponType.Speargun,
			ItemWeaponType.Staff,
			ItemWeaponType.Trident,
			ItemWeaponType.Harpoon,
			ItemWeaponType.Focus,
			ItemWeaponType.Scepter,
			ItemWeaponType.Torch,
			ItemWeaponType.Shield,
			ItemWeaponType.Warhorn
		};

		public static void set_locale(Locale newLocale_)
		{
			currentLocale = newLocale_;
		}

		public static bool is_luck_essence(int id_)
		{
			return luckNameMapping.ContainsKey(id_);
		}

		public static bool is_non_stackable_type(ItemType type_)
		{
			return nonStackableTypes.Contains(type_);
		}

		public static bool is_salvagable_equipment(ItemType type_)
		{
			return salvagableEquipment.Contains(type_);
		}

		public static bool is_pertinent_recipe(RecipeType recipe_)
		{
			return pertinentRecipeTypes.Contains((ApiEnum<RecipeType>)recipe_);
		}

		public static bool is_gameplay_consumable(int id_)
		{
			return gameplayConsumables.ContainsKey(id_);
		}

		public static bool is_karma_item(int id_)
		{
			return magicLists.karmaIds.Contains(id_);
		}

		public static int levenshtein_distance(ReadOnlySpan<char> left_, ReadOnlySpan<char> right_)
		{
			if (left_.Length == 0)
			{
				return right_.Length;
			}
			if (right_.Length == 0)
			{
				return left_.Length;
			}
			if (left_[0] == right_[0])
			{
				return levenshtein_distance(left_.Slice(1), right_.Slice(1));
			}
			int a = levenshtein_distance(left_.Slice(1), right_);
			int b = levenshtein_distance(left_, right_.Slice(1));
			int c = levenshtein_distance(left_.Slice(1), right_.Slice(1));
			return 1 + Math.Min(a, Math.Min(b, c));
		}

		public static bool string_similar(string left_, string right_)
		{
			string[] splitLeft = left_.Split(' ');
			string[] splitRight = right_.Split(' ');
			if (splitLeft.Count() != splitRight.Count())
			{
				return false;
			}
			int result = 0;
			foreach (int sub in splitLeft.Zip(splitRight, (string leftSub, string rightSub) => levenshtein_distance(new ReadOnlySpan<char>(leftSub.ToCharArray()), new ReadOnlySpan<char>(rightSub.ToCharArray()))))
			{
				result += sub;
			}
			if (result <= 5)
			{
				return true;
			}
			return false;
		}

		public static bool is_lws3_item(int id_)
		{
			return lws3Id.Contains(id_);
		}

		public static bool is_lws4_item(int id_)
		{
			return lws4Id.Contains(id_);
		}

		public static bool is_ibs_item(int id_)
		{
			return ibsId.Contains(id_);
		}

		public static string get_local_storage_name(string storage_)
		{
			if (storageTypeNameMapping.ContainsValue(storage_))
			{
				return get_current_translated_string(storage_);
			}
			return storage_;
		}

		public static string get_local_storage(StorageType storage_)
		{
			return get_current_translated_string(storageTypeNameMapping[storage_]);
		}

		public static string get_local_advice(AdviceType advice_)
		{
			return get_current_translated_string(adviceTypeNameMapping[advice_]);
		}

		public static string get_local_discipline(CraftingDisciplineType discipline_)
		{
			return get_current_translated_string(disciplineNameMapping[discipline_]);
		}

		public static string get_local_consumable_id_name(int id_)
		{
			if (is_gameplay_consumable(id_))
			{
				return get_current_translated_string(gameplayConsumables[id_]);
			}
			return "invalid id requested: consumable localisation";
		}

		public static string get_local_name(int id_)
		{
			if (is_luck_essence(id_))
			{
				return get_current_translated_string(luckNameMapping[id_]);
			}
			if (!localeItemNamesLut.english.ContainsKey(id_))
			{
				if (id_ == 97873)
				{
					return "Amber Runestone";
				}
				return "invalid id: " + id_;
			}
			return currentLocale switch
			{
				Locale.English => localeItemNamesLut.english[id_], 
				Locale.Spanish => localeItemNamesLut.spanish[id_], 
				Locale.German => localeItemNamesLut.german[id_], 
				Locale.French => localeItemNamesLut.french[id_], 
				Locale.Korean => localeItemNamesLut.korean[id_], 
				Locale.Chinese => localeItemNamesLut.chinese[id_], 
				_ => localeItemNamesLut.english[id_], 
			};
		}

		public static int id_from_Render_URI(string uri_)
		{
			if (uri_ == null || uri_ == "")
			{
				return 63369;
			}
			return Convert.ToInt32(uri_.Split('/').Last().Split('.')[0]);
		}

		private static string handle_missing_translation(string tooltip_, string message_)
		{
			log.Warn(message_ + ": " + tooltip_);
			return message_;
		}

		public static string get_current_translated_string(string tooltip_)
		{
			if (tooltip_ == null)
			{
				return null;
			}
			if (tooltip_ == "" || tooltip_ == " ")
			{
				return tooltip_;
			}
			int index = translation.english.FindIndex((string item) => item.Contains(tooltip_));
			if (index == -1)
			{
				log.Warn("index of >" + tooltip_ + "< is -1");
				return currentLocale switch
				{
					Locale.English => handle_missing_translation(tooltip_, "no english local"), 
					Locale.Spanish => handle_missing_translation(tooltip_, "no spanish local"), 
					Locale.German => handle_missing_translation(tooltip_, "no german local"), 
					Locale.French => handle_missing_translation(tooltip_, "no french local"), 
					Locale.Korean => handle_missing_translation(tooltip_, "no korean local"), 
					Locale.Chinese => handle_missing_translation(tooltip_, "no chinese local"), 
					_ => handle_missing_translation(tooltip_, "no english local"), 
				};
			}
			return currentLocale switch
			{
				Locale.English => translation.english[index], 
				Locale.Spanish => translation.spanish[index], 
				Locale.German => translation.german[index], 
				Locale.French => translation.french[index], 
				Locale.Korean => translation.korean[index], 
				Locale.Chinese => translation.chinese[index], 
				_ => translation.english[index], 
			};
		}
	}
}
