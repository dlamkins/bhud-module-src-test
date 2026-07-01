using System.Collections.Generic;

namespace WhereIsMyPSNA
{
	internal static class RecipeDefs
	{
		public static readonly IReadOnlyDictionary<int, RecipeDef> ByRecipeSheetId;

		public static readonly IReadOnlyDictionary<int, RecipeDef> ByCraftingRecipeId;

		static RecipeDefs()
		{
			RecipeDef[] obj = new RecipeDef[140]
			{
				new RecipeDef
				{
					ItemIds = new int[1] { 41565 },
					RecipeSheetIds = new int[2] { 41577, 81226 },
					CraftingRecipeIds = new int[1] { 7225 },
					Name = "Bowl of Garlic Kale Sautee",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 Expertise\n+70 Condition Damage\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41562 },
					RecipeSheetIds = new int[2] { 41574, 81250 },
					CraftingRecipeIds = new int[1] { 7221 },
					Name = "Bowl of Refugee's Beet Soup",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 toughness\n20% magic find\n+10 experience from kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41569 },
					RecipeSheetIds = new int[2] { 41581, 81252 },
					CraftingRecipeIds = new int[1] { 7230 },
					Name = "Bowl of Sweet and Spicy Butternut Squash Soup",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 Power\n+70 Ferocity\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41568 },
					RecipeSheetIds = new int[2] { 41580, 81214 },
					CraftingRecipeIds = new int[1] { 7229 },
					Name = "Bowl of Zesty Turnip Soup",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 vitality\n20% magic find\n+10 experience from kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41561 },
					RecipeSheetIds = new int[2] { 41573, 81211 },
					CraftingRecipeIds = new int[1] { 7219 },
					Name = "Carrot Soufflé",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+200 Power for 30 Seconds on Kill\n+70 Ferocity\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48921 },
					RecipeSheetIds = new int[1] { 48922 },
					CraftingRecipeIds = new int[1] { 7842 },
					Name = "Bowl of Marjory's Experimental Chili",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+50 Power\n+50 Condition Damage\nGain Might When Using a Heal Skill\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41566 },
					RecipeSheetIds = new int[2] { 41578, 81262 },
					CraftingRecipeIds = new int[1] { 7226 },
					Name = "Mushroom Loaf",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 Vitality\n+70 Power\n+10 Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41564 },
					RecipeSheetIds = new int[2] { 41576, 81247 },
					CraftingRecipeIds = new int[1] { 7223 },
					Name = "Plate of Frostgorge Clams",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 Toughness\n+70 Precision\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41567 },
					RecipeSheetIds = new int[2] { 41579, 81246 },
					CraftingRecipeIds = new int[1] { 7227 },
					Name = "Plate of Spicy Herbed Chicken",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 toughness\n+70 power\n+10 experience from kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43450 },
					RecipeSheetIds = new int[1] { 43482 },
					CraftingRecipeIds = new int[1] { 7232 },
					Name = "Potent Master Maintenance Oil",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Concentration Equal to 3% of Your Precision\nGain Concentration Equal to 6% of Your Healing Power\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43449 },
					RecipeSheetIds = new int[1] { 43484 },
					CraftingRecipeIds = new int[1] { 7231 },
					Name = "Potent Master Tuning Crystal",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Condition Damage Equal to 3% of Your Precision\nGain Condition Damage Equal to 8% of Your Expertise\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43451 },
					RecipeSheetIds = new int[1] { 43483 },
					CraftingRecipeIds = new int[1] { 7233 },
					Name = "Potent Superior Sharpening Stone",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Power Equal to 3% of Your Precision\nGain Power Equal to 6% of Your Ferocity\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 41563 },
					RecipeSheetIds = new int[2] { 41575, 81210 },
					CraftingRecipeIds = new int[1] { 7222 },
					Name = "Spicy Marinated Mushroom",
					Type = "Consumable",
					DetailType = "Food",
					Rarity = "Fine",
					Level = 80,
					Description = "+100 power\n+70 toughness\n+10 experience from kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 15
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48917 },
					RecipeSheetIds = new int[1] { 48920 },
					CraftingRecipeIds = new int[1] { 7839 },
					Name = "Toxic Tuning Crystal",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Condition Damage Equal to 3% of Your Power\nGain Condition Damage Equal to 3% of Your Precision\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48916 },
					RecipeSheetIds = new int[1] { 48919 },
					CraftingRecipeIds = new int[1] { 7841 },
					Name = "Toxic Maintenance Oil",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Concentration Equal to 3% of Your Power\nGain Concentration Equal to 6% of Your Condition Damage\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48915 },
					RecipeSheetIds = new int[1] { 48918 },
					CraftingRecipeIds = new int[1] { 7840 },
					Name = "Toxic Sharpening Stone",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Fine",
					Level = 80,
					Description = "Gain Power Equal to 6% of Your Condition Damage\nGain Power Equal to 8% of Your Expertise\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 33
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67528 },
					RecipeSheetIds = new int[1] { 67966 },
					CraftingRecipeIds = new int[1] { 9900 },
					Name = "Bountiful Maintenance Oil",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain 0.6% Increased Healing to Other Allies for Every 100 Healing Power\nGain 0.8% Increased Healing to Other Allies for Every 100 Concentration\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67531 },
					RecipeSheetIds = new int[1] { 67962 },
					CraftingRecipeIds = new int[1] { 9898 },
					Name = "Bountiful Sharpening Stone",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Power Equal to 6% of Your Healing Power\nGain Power Equal to 8% of Your Concentration\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67522 },
					RecipeSheetIds = new int[1] { 67961 },
					CraftingRecipeIds = new int[1] { 9901 },
					Name = "Bountiful Tuning Crystal",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Condition Damage Equal to 6% of Your Healing Power\nGain Condition Damage Equal to 8% of Your Concentration\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67529 },
					RecipeSheetIds = new int[1] { 67964 },
					CraftingRecipeIds = new int[1] { 9899 },
					Name = "Furious Maintenance Oil",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Concentration Equal to 3% of Your Precision\nGain Healing Power Equal to 3% of Your Precision\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67530 },
					RecipeSheetIds = new int[1] { 67965 },
					CraftingRecipeIds = new int[1] { 9896 },
					Name = "Furious Sharpening Stone",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Power Equal to 3% of Your Precision\nGain Ferocity Equal to 3% of Your Precision\n+10% Experience from Kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 67524 },
					RecipeSheetIds = new int[1] { 67963 },
					CraftingRecipeIds = new int[1] { 9897 },
					Name = "Furious Tuning Crystal",
					Type = "Consumable",
					DetailType = "Utility",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Condition Damage Equal to 3% of Your Precision\nGain Expertise Equal to 3% of Your Precision\n+10% Experience from kills",
					DurationSecs = 1800,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 50018 },
					RecipeSheetIds = new int[1] { 50019 },
					CraftingRecipeIds = new int[1] { 8458 },
					Name = "Maintenance Oil Station",
					Type = "Consumable",
					DetailType = "Generic",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Concentration Equal to 3% of Your Precision\nGain Concentration Equal to 6% of Your Healing Power\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44953 },
					RecipeSheetIds = new int[1] { 44648 },
					CraftingRecipeIds = new int[1] { 7286 },
					Name = "Minor Rune of Exuberance",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Masterwork",
					Level = 60,
					Description = "(1): +10 Vitality\n(2): +14 Healing",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 5
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44959 },
					RecipeSheetIds = new int[1] { 44653 },
					CraftingRecipeIds = new int[1] { 7296 },
					Name = "Minor Rune of Perplexity",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Masterwork",
					Level = 60,
					Description = "(1): +10 Condition Damage\n(2): +4% Confusion Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 5
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44954 },
					RecipeSheetIds = new int[1] { 44650 },
					CraftingRecipeIds = new int[1] { 7299 },
					Name = "Minor Rune of Tormenting",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Masterwork",
					Level = 60,
					Description = "(1): +10 Condition Damage\n(2): +4% Torment Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 5
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44942 },
					RecipeSheetIds = new int[2] { 44660, 44715 },
					CraftingRecipeIds = new int[1] { 7293 },
					Name = "Minor Sigil of Bursting",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Masterwork",
					Level = 0,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 58
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44948 },
					RecipeSheetIds = new int[2] { 44663, 44713 },
					CraftingRecipeIds = new int[1] { 7287 },
					Name = "Minor Sigil of Malice",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Masterwork",
					Level = 0,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 58
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44945 },
					RecipeSheetIds = new int[2] { 44717, 44657 },
					CraftingRecipeIds = new int[1] { 7290 },
					Name = "Minor Sigil of Renewal",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Masterwork",
					Level = 0,
					Description = "Element: ControlDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 58
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 50020 },
					RecipeSheetIds = new int[1] { 50021 },
					CraftingRecipeIds = new int[1] { 8456 },
					Name = "Sharpening Stone Station",
					Type = "Consumable",
					DetailType = "Generic",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Power Equal to 3% of Your Precision\nGain Power Equal to 6% of Your Ferocity\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 50022 },
					RecipeSheetIds = new int[1] { 50023 },
					CraftingRecipeIds = new int[1] { 8457 },
					Name = "Tuning Crystal Station",
					Type = "Consumable",
					DetailType = "Generic",
					Rarity = "Masterwork",
					Level = 80,
					Description = "Gain Condition Damage Equal to 3% of Your Precision\nGain Condition Damage Equal to 8% of Your Expertise\n+10% Experience from Kills",
					DurationSecs = 3600,
					Binding = "None",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 74525 },
					RecipeSheetIds = new int[1] { 73199 },
					CraftingRecipeIds = new int[1] { 11666 },
					Name = "20-Slot Equipment Pact Box",
					Type = "Bag",
					DetailType = "",
					Rarity = "Rare",
					Level = 0,
					Description = "20 slots. If possible, weapons and armor will fill this box before other empty spaces. The contents of this box will not move when inventory is sorted. Items in this box will never appear in a sell-to-vendor list and will not move when inventory is sorted.",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 162
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44952 },
					RecipeSheetIds = new int[1] { 44647 },
					CraftingRecipeIds = new int[1] { 7285 },
					Name = "Major Rune of Exuberance",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Rare",
					Level = 60,
					Description = "(1): +15 Vitality\n(2): +21 Healing\n(3): +30 Vitality\n(4): +39 Precision",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 30
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44958 },
					RecipeSheetIds = new int[1] { 44654 },
					CraftingRecipeIds = new int[1] { 7297 },
					Name = "Major Rune of Perplexity",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Rare",
					Level = 60,
					Description = "(1): +15 Condition Damage\n(2): +6% Confusion Duration\n(3): +30 Condition Damage\n(4): +9% Confusion Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 30
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44955 },
					RecipeSheetIds = new int[1] { 44651 },
					CraftingRecipeIds = new int[1] { 7300 },
					Name = "Major Rune of Tormenting",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Rare",
					Level = 60,
					Description = "(1): +15 Condition Damage\n(2): +6% Torment Duration\n(3): +30 Condition Damage\n(4): +9% Torment Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 30
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44943 },
					RecipeSheetIds = new int[1] { 44716 },
					CraftingRecipeIds = new int[1] { 7294 },
					Name = "Major Sigil of Bursting",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Rare",
					Level = 39,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 108
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44949 },
					RecipeSheetIds = new int[2] { 44714, 44662 },
					CraftingRecipeIds = new int[1] { 7288 },
					Name = "Major Sigil of Malice",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Rare",
					Level = 39,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 108
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44946 },
					RecipeSheetIds = new int[2] { 44718, 44656 },
					CraftingRecipeIds = new int[1] { 7291 },
					Name = "Major Sigil of Renewal",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Rare",
					Level = 39,
					Description = "Element: ControlDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 108
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49782 },
					RecipeSheetIds = new int[1] { 49737 },
					CraftingRecipeIds = new int[1] { 8391 },
					Name = "Watchwork Mechanism",
					Type = "CraftingMaterial",
					DetailType = "",
					Rarity = "Rare",
					Level = 0,
					Description = "Refined from Watchwork Sprockets.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 50
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43779 },
					RecipeSheetIds = new int[1] { 43801 },
					CraftingRecipeIds = new int[1] { 7258 },
					Name = "Box of Celestial Draconic Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 256
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49800 },
					RecipeSheetIds = new int[1] { 49738 },
					CraftingRecipeIds = new int[1] { 8398 },
					Name = "Box of Zealot's Draconic Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 0
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43780 },
					RecipeSheetIds = new int[1] { 43804 },
					CraftingRecipeIds = new int[1] { 7238 },
					Name = "Celestial Draconic Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43781 },
					RecipeSheetIds = new int[1] { 43805 },
					CraftingRecipeIds = new int[1] { 7239 },
					Name = "Celestial Draconic Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+63 Power\n+63 Precision\n+63 Toughness\n+63 Vitality\n+63 Ferocity\n+63 Healing\n+63 Condition Damage\n+63 Concentration\n+63 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43782 },
					RecipeSheetIds = new int[1] { 43806 },
					CraftingRecipeIds = new int[1] { 7240 },
					Name = "Celestial Draconic Gauntlets",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43783 },
					RecipeSheetIds = new int[1] { 43807 },
					CraftingRecipeIds = new int[1] { 7241 },
					Name = "Celestial Draconic Helm",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+28 Power\n+28 Precision\n+28 Toughness\n+28 Vitality\n+28 Ferocity\n+28 Healing\n+28 Condition Damage\n+28 Concentration\n+28 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43784 },
					RecipeSheetIds = new int[1] { 43808 },
					CraftingRecipeIds = new int[1] { 7242 },
					Name = "Celestial Draconic Legs",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+42 Power\n+42 Precision\n+42 Toughness\n+42 Vitality\n+42 Ferocity\n+42 Healing\n+42 Condition Damage\n+42 Concentration\n+42 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43785 },
					RecipeSheetIds = new int[1] { 43809 },
					CraftingRecipeIds = new int[1] { 7243 },
					Name = "Celestial Draconic Pauldrons",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43792 },
					RecipeSheetIds = new int[1] { 43816 },
					CraftingRecipeIds = new int[1] { 7244 },
					Name = "Celestial Emblazoned Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43793 },
					RecipeSheetIds = new int[1] { 43817 },
					CraftingRecipeIds = new int[1] { 7245 },
					Name = "Celestial Emblazoned Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+63 Power\n+63 Precision\n+63 Toughness\n+63 Vitality\n+63 Ferocity\n+63 Healing\n+63 Condition Damage\n+63 Concentration\n+63 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43794 },
					RecipeSheetIds = new int[1] { 43818 },
					CraftingRecipeIds = new int[1] { 7246 },
					Name = "Celestial Emblazoned Gloves",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43795 },
					RecipeSheetIds = new int[1] { 43819 },
					CraftingRecipeIds = new int[1] { 7247 },
					Name = "Celestial Emblazoned Helm",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+28 Power\n+28 Precision\n+28 Toughness\n+28 Vitality\n+28 Ferocity\n+28 Healing\n+28 Condition Damage\n+28 Concentration\n+28 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43796 },
					RecipeSheetIds = new int[1] { 43820 },
					CraftingRecipeIds = new int[1] { 7248 },
					Name = "Celestial Emblazoned Pants",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+42 Power\n+42 Precision\n+42 Toughness\n+42 Vitality\n+42 Ferocity\n+42 Healing\n+42 Condition Damage\n+42 Concentration\n+42 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43797 },
					RecipeSheetIds = new int[1] { 43821 },
					CraftingRecipeIds = new int[1] { 7250 },
					Name = "Celestial Emblazoned Shoulders",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43786 },
					RecipeSheetIds = new int[1] { 43810 },
					CraftingRecipeIds = new int[1] { 7251 },
					Name = "Celestial Exalted Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43787 },
					RecipeSheetIds = new int[1] { 43811 },
					CraftingRecipeIds = new int[1] { 7252 },
					Name = "Celestial Exalted Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+63 Power\n+63 Precision\n+63 Toughness\n+63 Vitality\n+63 Ferocity\n+63 Healing\n+63 Condition Damage\n+63 Concentration\n+63 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43788 },
					RecipeSheetIds = new int[1] { 43812 },
					CraftingRecipeIds = new int[1] { 7253 },
					Name = "Celestial Exalted Gloves",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43791 },
					RecipeSheetIds = new int[1] { 43815 },
					CraftingRecipeIds = new int[1] { 7257 },
					Name = "Celestial Exalted Mantle",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+21 Power\n+21 Precision\n+21 Toughness\n+21 Vitality\n+21 Ferocity\n+21 Healing\n+21 Condition Damage\n+21 Concentration\n+21 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43789 },
					RecipeSheetIds = new int[1] { 43813 },
					CraftingRecipeIds = new int[1] { 7254 },
					Name = "Celestial Exalted Masque",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+28 Power\n+28 Precision\n+28 Toughness\n+28 Vitality\n+28 Ferocity\n+28 Healing\n+28 Condition Damage\n+28 Concentration\n+28 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43790 },
					RecipeSheetIds = new int[1] { 43814 },
					CraftingRecipeIds = new int[1] { 7255 },
					Name = "Celestial Exalted Pants",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+42 Power\n+42 Precision\n+42 Toughness\n+42 Vitality\n+42 Ferocity\n+42 Healing\n+42 Condition Damage\n+42 Concentration\n+42 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43774 },
					RecipeSheetIds = new int[1] { 43798 },
					CraftingRecipeIds = new int[1] { 7236 },
					Name = "Celestial Intricate Gossamer Insignia",
					Type = "CraftingMaterial",
					DetailType = "",
					Rarity = "Exotic",
					Level = 0,
					Description = "Used in the crafting of armor with a bonus to all stats.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 64
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43775 },
					RecipeSheetIds = new int[1] { 43799 },
					CraftingRecipeIds = new int[1] { 7237 },
					Name = "Celestial Orichalcum Imbued Inscription",
					Type = "CraftingMaterial",
					DetailType = "",
					Rarity = "Exotic",
					Level = 80,
					Description = "Used in the crafting of weapons with a bonus to all stats.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43859 },
					RecipeSheetIds = new int[1] { 43837 },
					CraftingRecipeIds = new int[1] { 7274 },
					Name = "Celestial Pearl Bludgeoner",
					Type = "Weapon",
					DetailType = "Mace",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43850 },
					RecipeSheetIds = new int[1] { 43829 },
					CraftingRecipeIds = new int[1] { 7266 },
					Name = "Celestial Pearl Blunderbuss",
					Type = "Weapon",
					DetailType = "Rifle",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43853 },
					RecipeSheetIds = new int[1] { 43831 },
					CraftingRecipeIds = new int[1] { 7268 },
					Name = "Celestial Pearl Brazier",
					Type = "Weapon",
					DetailType = "Torch",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43857 },
					RecipeSheetIds = new int[1] { 43835 },
					CraftingRecipeIds = new int[1] { 7272 },
					Name = "Celestial Pearl Broadsword",
					Type = "Weapon",
					DetailType = "Greatsword",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43856 },
					RecipeSheetIds = new int[1] { 43834 },
					CraftingRecipeIds = new int[1] { 7271 },
					Name = "Celestial Pearl Carver",
					Type = "Weapon",
					DetailType = "Dagger",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43844 },
					RecipeSheetIds = new int[1] { 43822 },
					CraftingRecipeIds = new int[1] { 7259 },
					Name = "Celestial Pearl Conch",
					Type = "Weapon",
					DetailType = "Focus",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43858 },
					RecipeSheetIds = new int[1] { 43836 },
					CraftingRecipeIds = new int[1] { 7273 },
					Name = "Celestial Pearl Crusher",
					Type = "Weapon",
					DetailType = "Hammer",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43849 },
					RecipeSheetIds = new int[1] { 43828 },
					CraftingRecipeIds = new int[1] { 7265 },
					Name = "Celestial Pearl Handcannon",
					Type = "Weapon",
					DetailType = "Pistol",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43861 },
					RecipeSheetIds = new int[1] { 43839 },
					CraftingRecipeIds = new int[1] { 7276 },
					Name = "Celestial Pearl Impaler",
					Type = "Weapon",
					DetailType = "Harpoon",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43851 },
					RecipeSheetIds = new int[1] { 43830 },
					CraftingRecipeIds = new int[1] { 7267 },
					Name = "Celestial Pearl Needler",
					Type = "Weapon",
					DetailType = "ShortBow",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43846 },
					RecipeSheetIds = new int[1] { 43824 },
					CraftingRecipeIds = new int[1] { 7261 },
					Name = "Celestial Pearl Quarterstaff",
					Type = "Weapon",
					DetailType = "Staff",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43855 },
					RecipeSheetIds = new int[1] { 43833 },
					CraftingRecipeIds = new int[1] { 7270 },
					Name = "Celestial Pearl Reaver",
					Type = "Weapon",
					DetailType = "Axe",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43845 },
					RecipeSheetIds = new int[1] { 43823 },
					CraftingRecipeIds = new int[1] { 7260 },
					Name = "Celestial Pearl Rod",
					Type = "Weapon",
					DetailType = "Scepter",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43862 },
					RecipeSheetIds = new int[1] { 43840 },
					CraftingRecipeIds = new int[1] { 7277 },
					Name = "Celestial Pearl Sabre",
					Type = "Weapon",
					DetailType = "Sword",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43860 },
					RecipeSheetIds = new int[1] { 43838 },
					CraftingRecipeIds = new int[1] { 7275 },
					Name = "Celestial Pearl Shell",
					Type = "Weapon",
					DetailType = "Shield",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43854 },
					RecipeSheetIds = new int[1] { 43832 },
					CraftingRecipeIds = new int[1] { 7269 },
					Name = "Celestial Pearl Siren",
					Type = "Weapon",
					DetailType = "Warhorn",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43852 },
					RecipeSheetIds = new int[1] { 43826 },
					CraftingRecipeIds = new int[1] { 7263 },
					Name = "Celestial Pearl Speargun",
					Type = "Weapon",
					DetailType = "Speargun",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43848 },
					RecipeSheetIds = new int[1] { 43827 },
					CraftingRecipeIds = new int[1] { 7264 },
					Name = "Celestial Pearl Stinger",
					Type = "Weapon",
					DetailType = "LongBow",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43847 },
					RecipeSheetIds = new int[1] { 43825 },
					CraftingRecipeIds = new int[1] { 7262 },
					Name = "Celestial Pearl Trident",
					Type = "Weapon",
					DetailType = "Trident",
					Rarity = "Exotic",
					Level = 80,
					Description = "+113 Power\n+113 Precision\n+113 Toughness\n+113 Vitality\n+113 Ferocity\n+113 Healing\n+113 Condition Damage\n+113 Concentration\n+113 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43865 },
					RecipeSheetIds = new int[1] { 43841 },
					CraftingRecipeIds = new int[1] { 7280 },
					Name = "Charged Quartz Orichalcum Amulet",
					Type = "Trinket",
					DetailType = "Amulet",
					Rarity = "Exotic",
					Level = 80,
					Description = "+56 Power\n+56 Precision\n+56 Toughness\n+56 Vitality\n+56 Ferocity\n+56 Healing\n+56 Condition Damage\n+56 Concentration\n+56 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 528
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43864 },
					RecipeSheetIds = new int[1] { 43843 },
					CraftingRecipeIds = new int[1] { 7279 },
					Name = "Charged Quartz Orichalcum Earring",
					Type = "Trinket",
					DetailType = "Accessory",
					Rarity = "Exotic",
					Level = 80,
					Description = "+35 Power\n+35 Precision\n+35 Toughness\n+35 Vitality\n+35 Ferocity\n+35 Healing\n+35 Condition Damage\n+35 Concentration\n+35 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 330
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43863 },
					RecipeSheetIds = new int[1] { 43842 },
					CraftingRecipeIds = new int[1] { 7278 },
					Name = "Charged Quartz Orichalcum Ring",
					Type = "Trinket",
					DetailType = "Ring",
					Rarity = "Exotic",
					Level = 80,
					Description = "+42 Power\n+42 Precision\n+42 Toughness\n+42 Vitality\n+42 Ferocity\n+42 Healing\n+42 Condition Damage\n+42 Concentration\n+42 Expertise",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43866 },
					RecipeSheetIds = new int[1] { 43800 },
					CraftingRecipeIds = new int[1] { 7281 },
					Name = "Exquisite Charged Quartz Jewel",
					Type = "UpgradeComponent",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 80,
					Description = "Double-click to apply to an accessory, amulet, or ring with an unused upgrade slot.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49823 },
					RecipeSheetIds = new int[1] { 49736 },
					CraftingRecipeIds = new int[1] { 8434 },
					Name = "Exquisite Watchwork Sprocket",
					Type = "UpgradeComponent",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 80,
					Description = "Double-click to apply to an accessory, amulet, or ring with an unused upgrade slot.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43778 },
					RecipeSheetIds = new int[1] { 43802 },
					CraftingRecipeIds = new int[1] { 7249 },
					Name = "Satchel of Celestial Emblazoned Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 0
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 43777 },
					RecipeSheetIds = new int[1] { 43803 },
					CraftingRecipeIds = new int[1] { 7256 },
					Name = "Satchel of Celestial Exalted Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 0
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49801 },
					RecipeSheetIds = new int[1] { 49739 },
					CraftingRecipeIds = new int[1] { 8399 },
					Name = "Satchel of Zealot's Emblazoned Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 0
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49802 },
					RecipeSheetIds = new int[1] { 49740 },
					CraftingRecipeIds = new int[1] { 8400 },
					Name = "Satchel of Zealot's Exalted Armor",
					Type = "Container",
					DetailType = "Default",
					Rarity = "Exotic",
					Level = 0,
					Description = "Double-click to unpack a full set of level 80 armor.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 0
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49817 },
					RecipeSheetIds = new int[1] { 49778 },
					CraftingRecipeIds = new int[1] { 8432 },
					Name = "Sprocket Orichalcum Amulet",
					Type = "Trinket",
					DetailType = "Amulet",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 528
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49822 },
					RecipeSheetIds = new int[1] { 49779 },
					CraftingRecipeIds = new int[1] { 8433 },
					Name = "Sprocket Orichalcum Earring",
					Type = "Trinket",
					DetailType = "Accessory",
					Rarity = "Exotic",
					Level = 80,
					Description = "+75 Power\n+53 Precision\n+53 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 330
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49824 },
					RecipeSheetIds = new int[1] { 49780 },
					CraftingRecipeIds = new int[1] { 8435 },
					Name = "Sprocket Orichalcum Ring",
					Type = "Trinket",
					DetailType = "Ring",
					Rarity = "Exotic",
					Level = 80,
					Description = "+90 Power\n+64 Precision\n+64 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48907 },
					RecipeSheetIds = new int[1] { 48910 },
					CraftingRecipeIds = new int[1] { 7833 },
					Name = "Superior Rune of Antitoxin",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Exotic",
					Level = 60,
					Description = "(1): +25 Condition Damage\n(2): -5% Incoming Condition Duration\n(3): +50 Condition Damage\n(4): -10% Incoming Condition Duration\n(5): +100 Condition Damage\n(6): +125 Vitality",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 216
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44951 },
					RecipeSheetIds = new int[1] { 44649 },
					CraftingRecipeIds = new int[1] { 7284 },
					Name = "Superior Rune of Exuberance",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Exotic",
					Level = 60,
					Description = "(1): +25 Vitality\n(2): +35 Healing\n(3): +50 Vitality\n(4): +65 Precision\n(5): +100 Vitality\n(6): +125 Power",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 65
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44957 },
					RecipeSheetIds = new int[1] { 44655 },
					CraftingRecipeIds = new int[1] { 7298 },
					Name = "Superior Rune of Perplexity",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Exotic",
					Level = 60,
					Description = "(1): +25 Condition Damage\n(2): +10% Confusion Duration\n(3): +50 Condition Damage\n(4): +20% Confusion Duration\n(5): +100 Condition Damage\n(6): +20% Confusion Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 65
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44956 },
					RecipeSheetIds = new int[1] { 44652 },
					CraftingRecipeIds = new int[1] { 7301 },
					Name = "Superior Rune of Tormenting",
					Type = "UpgradeComponent",
					DetailType = "Rune",
					Rarity = "Exotic",
					Level = 60,
					Description = "(1): +25 Condition Damage\n(2): +10% Torment Duration\n(3): +50 Condition Damage\n(4): +20% Torment Duration\n(5): +100 Condition Damage\n(6): +20% Torment Duration",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 65
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44944 },
					RecipeSheetIds = new int[1] { 44661 },
					CraftingRecipeIds = new int[1] { 7295 },
					Name = "Superior Sigil of Bursting",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Exotic",
					Level = 60,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 216
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44950 },
					RecipeSheetIds = new int[1] { 44664 },
					CraftingRecipeIds = new int[1] { 7289 },
					Name = "Superior Sigil of Malice",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Exotic",
					Level = 60,
					Description = "Element: EnhancementDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 216
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 44947 },
					RecipeSheetIds = new int[1] { 44658 },
					CraftingRecipeIds = new int[1] { 7292 },
					Name = "Superior Sigil of Renewal",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Exotic",
					Level = 60,
					Description = "Element: ControlDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 216
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 48911 },
					RecipeSheetIds = new int[1] { 48912 },
					CraftingRecipeIds = new int[1] { 7836 },
					Name = "Superior Sigil of Torment",
					Type = "UpgradeComponent",
					DetailType = "Sigil",
					Rarity = "Exotic",
					Level = 60,
					Description = "Element: PainDouble-click to apply to a weapon.",
					DurationSecs = 0,
					Binding = "None",
					VendorValue = 216
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49733 },
					RecipeSheetIds = new int[1] { 49741 },
					CraftingRecipeIds = new int[1] { 8392 },
					Name = "Zealot's Draconic Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49783 },
					RecipeSheetIds = new int[1] { 49742 },
					CraftingRecipeIds = new int[1] { 8393 },
					Name = "Zealot's Draconic Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+134 Power\n+96 Precision\n+96 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49784 },
					RecipeSheetIds = new int[1] { 49743 },
					CraftingRecipeIds = new int[1] { 8394 },
					Name = "Zealot's Draconic Gauntlets",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49785 },
					RecipeSheetIds = new int[1] { 49744 },
					CraftingRecipeIds = new int[1] { 8395 },
					Name = "Zealot's Draconic Helm",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+60 Power\n+43 Precision\n+43 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49786 },
					RecipeSheetIds = new int[1] { 49745 },
					CraftingRecipeIds = new int[1] { 8396 },
					Name = "Zealot's Draconic Legs",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+90 Power\n+64 Precision\n+64 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49787 },
					RecipeSheetIds = new int[1] { 49746 },
					CraftingRecipeIds = new int[1] { 8397 },
					Name = "Zealot's Draconic Pauldrons",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49794 },
					RecipeSheetIds = new int[1] { 49753 },
					CraftingRecipeIds = new int[1] { 8401 },
					Name = "Zealot's Emblazoned Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49795 },
					RecipeSheetIds = new int[1] { 49754 },
					CraftingRecipeIds = new int[1] { 8402 },
					Name = "Zealot's Emblazoned Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+134 Power\n+96 Precision\n+96 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49796 },
					RecipeSheetIds = new int[1] { 49755 },
					CraftingRecipeIds = new int[1] { 8403 },
					Name = "Zealot's Emblazoned Gloves",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49797 },
					RecipeSheetIds = new int[1] { 49756 },
					CraftingRecipeIds = new int[1] { 8404 },
					Name = "Zealot's Emblazoned Helm",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+60 Power\n+43 Precision\n+43 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49798 },
					RecipeSheetIds = new int[1] { 49757 },
					CraftingRecipeIds = new int[1] { 8405 },
					Name = "Zealot's Emblazoned Pants",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+90 Power\n+64 Precision\n+64 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49799 },
					RecipeSheetIds = new int[1] { 49758 },
					CraftingRecipeIds = new int[1] { 8406 },
					Name = "Zealot's Emblazoned Shoulders",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49788 },
					RecipeSheetIds = new int[1] { 49747 },
					CraftingRecipeIds = new int[1] { 8407 },
					Name = "Zealot's Exalted Boots",
					Type = "Armor",
					DetailType = "Boots",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49789 },
					RecipeSheetIds = new int[1] { 49748 },
					CraftingRecipeIds = new int[1] { 8408 },
					Name = "Zealot's Exalted Coat",
					Type = "Armor",
					DetailType = "Coat",
					Rarity = "Exotic",
					Level = 80,
					Description = "+134 Power\n+96 Precision\n+96 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49790 },
					RecipeSheetIds = new int[1] { 49749 },
					CraftingRecipeIds = new int[1] { 8409 },
					Name = "Zealot's Exalted Gloves",
					Type = "Armor",
					DetailType = "Gloves",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49793 },
					RecipeSheetIds = new int[1] { 49752 },
					CraftingRecipeIds = new int[1] { 8412 },
					Name = "Zealot's Exalted Mantle",
					Type = "Armor",
					DetailType = "Shoulders",
					Rarity = "Exotic",
					Level = 80,
					Description = "+45 Power\n+32 Precision\n+32 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49791 },
					RecipeSheetIds = new int[1] { 49750 },
					CraftingRecipeIds = new int[1] { 8410 },
					Name = "Zealot's Exalted Masque",
					Type = "Armor",
					DetailType = "Helm",
					Rarity = "Exotic",
					Level = 80,
					Description = "+60 Power\n+43 Precision\n+43 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49792 },
					RecipeSheetIds = new int[1] { 49751 },
					CraftingRecipeIds = new int[1] { 8411 },
					Name = "Zealot's Exalted Pants",
					Type = "Armor",
					DetailType = "Leggings",
					Rarity = "Exotic",
					Level = 80,
					Description = "+90 Power\n+64 Precision\n+64 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 240
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49866 },
					RecipeSheetIds = new int[1] { 49735 },
					CraftingRecipeIds = new int[1] { 8390 },
					Name = "Zealot's Intricate Gossamer Insignia",
					Type = "CraftingMaterial",
					DetailType = "",
					Rarity = "Exotic",
					Level = 0,
					Description = "Used in the crafting of armor with +Power, +Precision, and +Healing.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 64
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49865 },
					RecipeSheetIds = new int[1] { 49734 },
					CraftingRecipeIds = new int[1] { 8414 },
					Name = "Zealot's Orichalcum Imbued Inscription",
					Type = "CraftingMaterial",
					DetailType = "",
					Rarity = "Exotic",
					Level = 80,
					Description = "Used in the crafting of weapons with +Power, +Precision, and +Healing.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 66
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49818 },
					RecipeSheetIds = new int[1] { 49774 },
					CraftingRecipeIds = new int[1] { 8429 },
					Name = "Zealot's Pearl Bludgeoner",
					Type = "Weapon",
					DetailType = "Mace",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49811 },
					RecipeSheetIds = new int[1] { 49766 },
					CraftingRecipeIds = new int[1] { 8421 },
					Name = "Zealot's Pearl Blunderbuss",
					Type = "Weapon",
					DetailType = "Rifle",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49813 },
					RecipeSheetIds = new int[1] { 49768 },
					CraftingRecipeIds = new int[1] { 8423 },
					Name = "Zealot's Pearl Brazier",
					Type = "Weapon",
					DetailType = "Torch",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49803 },
					RecipeSheetIds = new int[1] { 49772 },
					CraftingRecipeIds = new int[1] { 8427 },
					Name = "Zealot's Pearl Broadsword",
					Type = "Weapon",
					DetailType = "Greatsword",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49816 },
					RecipeSheetIds = new int[1] { 49771 },
					CraftingRecipeIds = new int[1] { 8413 },
					Name = "Zealot's Pearl Carver",
					Type = "Weapon",
					DetailType = "Dagger",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49804 },
					RecipeSheetIds = new int[1] { 49759 },
					CraftingRecipeIds = new int[1] { 8389 },
					Name = "Zealot's Pearl Conch",
					Type = "Weapon",
					DetailType = "Focus",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49806 },
					RecipeSheetIds = new int[1] { 49773 },
					CraftingRecipeIds = new int[1] { 8428 },
					Name = "Zealot's Pearl Crusher",
					Type = "Weapon",
					DetailType = "Hammer",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49810 },
					RecipeSheetIds = new int[1] { 49765 },
					CraftingRecipeIds = new int[1] { 8420 },
					Name = "Zealot's Pearl Handcannon",
					Type = "Weapon",
					DetailType = "Pistol",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49820 },
					RecipeSheetIds = new int[1] { 49776 },
					CraftingRecipeIds = new int[1] { 8431 },
					Name = "Zealot's Pearl Impaler",
					Type = "Weapon",
					DetailType = "Harpoon",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49812 },
					RecipeSheetIds = new int[1] { 49767 },
					CraftingRecipeIds = new int[1] { 8422 },
					Name = "Zealot's Pearl Needler",
					Type = "Weapon",
					DetailType = "ShortBow",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49781 },
					RecipeSheetIds = new int[1] { 49761 },
					CraftingRecipeIds = new int[1] { 8416 },
					Name = "Zealot's Pearl Quarterstaff",
					Type = "Weapon",
					DetailType = "Staff",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49815 },
					RecipeSheetIds = new int[1] { 49770 },
					CraftingRecipeIds = new int[1] { 8425 },
					Name = "Zealot's Pearl Reaver",
					Type = "Weapon",
					DetailType = "Axe",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49805 },
					RecipeSheetIds = new int[1] { 49760 },
					CraftingRecipeIds = new int[1] { 8415 },
					Name = "Zealot's Pearl Rod",
					Type = "Weapon",
					DetailType = "Scepter",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49821 },
					RecipeSheetIds = new int[1] { 49777 },
					CraftingRecipeIds = new int[1] { 8426 },
					Name = "Zealot's Pearl Sabre",
					Type = "Weapon",
					DetailType = "Sword",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49819 },
					RecipeSheetIds = new int[1] { 49775 },
					CraftingRecipeIds = new int[1] { 8430 },
					Name = "Zealot's Pearl Shell",
					Type = "Weapon",
					DetailType = "Shield",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49814 },
					RecipeSheetIds = new int[1] { 49769 },
					CraftingRecipeIds = new int[1] { 8424 },
					Name = "Zealot's Pearl Siren",
					Type = "Weapon",
					DetailType = "Warhorn",
					Rarity = "Exotic",
					Level = 80,
					Description = "+120 Power\n+85 Precision\n+85 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 264
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49808 },
					RecipeSheetIds = new int[1] { 49763 },
					CraftingRecipeIds = new int[1] { 8418 },
					Name = "Zealot's Pearl Speargun",
					Type = "Weapon",
					DetailType = "Speargun",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49809 },
					RecipeSheetIds = new int[1] { 49764 },
					CraftingRecipeIds = new int[1] { 8419 },
					Name = "Zealot's Pearl Stinger",
					Type = "Weapon",
					DetailType = "LongBow",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 49807 },
					RecipeSheetIds = new int[1] { 49762 },
					CraftingRecipeIds = new int[1] { 8417 },
					Name = "Zealot's Pearl Trident",
					Type = "Weapon",
					DetailType = "Trident",
					Rarity = "Exotic",
					Level = 80,
					Description = "+239 Power\n+171 Precision\n+171 Healing",
					DurationSecs = 0,
					Binding = "SoulboundOnUse",
					VendorValue = 396
				},
				new RecipeDef
				{
					ItemIds = new int[1] { 72446 },
					RecipeSheetIds = new int[1] { 75473 },
					CraftingRecipeIds = new int[1] { 9948 },
					Name = "Bough of Melandru",
					Type = "Back",
					DetailType = "",
					Rarity = "Ascended",
					Level = 80,
					Description = "Melandru—the goddess of nature, earth, and growth—can be found in every harvest and every flower.",
					DurationSecs = 0,
					Binding = "AccountBound",
					VendorValue = 330
				}
			};
			Dictionary<int, RecipeDef> sheetLookup = new Dictionary<int, RecipeDef>();
			Dictionary<int, RecipeDef> craftingLookup = new Dictionary<int, RecipeDef>();
			RecipeDef[] array = obj;
			foreach (RecipeDef def in array)
			{
				int[] recipeSheetIds = def.RecipeSheetIds;
				foreach (int id in recipeSheetIds)
				{
					sheetLookup[id] = def;
				}
				recipeSheetIds = def.CraftingRecipeIds;
				foreach (int id2 in recipeSheetIds)
				{
					craftingLookup[id2] = def;
				}
			}
			ByRecipeSheetId = sheetLookup;
			ByCraftingRecipeId = craftingLookup;
		}
	}
}
