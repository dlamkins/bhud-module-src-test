using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MysticCrafting.Module.Strings
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Recipe
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("MysticCrafting.Module.Strings.Recipe", typeof(Recipe).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string Account => ResourceManager.GetString("Account", resourceCulture);

		internal static string AllRecipes => ResourceManager.GetString("AllRecipes", resourceCulture);

		internal static string CountInBank => ResourceManager.GetString("CountInBank", resourceCulture);

		internal static string CountInInventoryCharacter => ResourceManager.GetString("CountInInventoryCharacter", resourceCulture);

		internal static string CountInMaterialStorage => ResourceManager.GetString("CountInMaterialStorage", resourceCulture);

		internal static string CountInSharedInventory => ResourceManager.GetString("CountInSharedInventory", resourceCulture);

		internal static string CurrentRecipe => ResourceManager.GetString("CurrentRecipe", resourceCulture);

		internal static string Ingredients => ResourceManager.GetString("Ingredients", resourceCulture);

		internal static string MoreRequired => ResourceManager.GetString("MoreRequired", resourceCulture);

		internal static string MysticForge => ResourceManager.GetString("MysticForge", resourceCulture);

		internal static string PriceEach => ResourceManager.GetString("PriceEach", resourceCulture);

		internal static string PricePerNumber => ResourceManager.GetString("PricePerNumber", resourceCulture);

		internal static string RecipeRequiredLevel => ResourceManager.GetString("RecipeRequiredLevel", resourceCulture);

		internal static string RecipeSource => ResourceManager.GetString("RecipeSource", resourceCulture);

		internal static string RecipeSourceAutomatic => ResourceManager.GetString("RecipeSourceAutomatic", resourceCulture);

		internal static string RecipeSourceOther => ResourceManager.GetString("RecipeSourceOther", resourceCulture);

		internal static string RecipeSourceRecipeSheet => ResourceManager.GetString("RecipeSourceRecipeSheet", resourceCulture);

		internal static string Requirement => ResourceManager.GetString("Requirement", resourceCulture);

		internal static string SoldByVendors => ResourceManager.GetString("SoldByVendors", resourceCulture);

		internal static string TradingPost => ResourceManager.GetString("TradingPost", resourceCulture);

		internal static string TradingPostBuy => ResourceManager.GetString("TradingPostBuy", resourceCulture);

		internal static string TradingPostSell => ResourceManager.GetString("TradingPostSell", resourceCulture);

		internal static string Unavailable => ResourceManager.GetString("Unavailable", resourceCulture);

		internal static string VendorsMore => ResourceManager.GetString("VendorsMore", resourceCulture);

		internal static string WalletLabel => ResourceManager.GetString("WalletLabel", resourceCulture);

		internal Recipe()
		{
		}
	}
}
