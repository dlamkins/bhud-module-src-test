using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhereIsMyPSNA
{
	internal class AllRecipesView : View
	{
		private const int Columns = 2;

		private const int ColumnGap = 8;

		private const int IconSize = 24;

		private const int RowHeight = 28;

		private const int RowGap = 4;

		private const int ItemGap = 4;

		private const int CheckGlyphSize = 32;

		private const int CheckGlyphLeftBleed = 9;

		private const int CheckGlyphDefaultH = 16;

		private static readonly string[] SheetNamePrefixes = new string[4] { "Recipe: ", "Rezept: ", "Receta: ", "Recette : " };

		private static readonly Dictionary<string, int> RarityOrder = new Dictionary<string, int>
		{
			["Junk"] = 0,
			["Basic"] = 1,
			["Fine"] = 2,
			["Masterwork"] = 3,
			["Rare"] = 4,
			["Exotic"] = 5,
			["Ascended"] = 6,
			["Legendary"] = 7
		};

		private static readonly RecipeDef[] AllRecipes = (from d in RecipeDefs.ByRecipeSheetId.Values.Distinct()
			orderby (!RarityOrder.TryGetValue(d.Rarity, out var value)) ? int.MaxValue : value, d.SheetName
			select d).ToArray();

		private readonly PsnaDataService _dataService;

		private readonly int _contentWidth;

		private readonly int _contentHeight;

		private readonly Action<AllRecipesView> _registerActive;

		private readonly Action<AllRecipesView> _unregisterActive;

		private readonly Checkbox[] _checks = (Checkbox[])(object)new Checkbox[AllRecipes.Length];

		private RecipeTooltip _tooltip;

		private bool _isBuilt;

		private static string StripRecipePrefix(string sheetName)
		{
			string[] sheetNamePrefixes = SheetNamePrefixes;
			foreach (string prefix in sheetNamePrefixes)
			{
				if (sheetName.StartsWith(prefix, StringComparison.Ordinal))
				{
					return sheetName.Substring(prefix.Length);
				}
			}
			return sheetName;
		}

		public AllRecipesView(PsnaDataService dataService, int contentWidth, int contentHeight, Action<AllRecipesView> registerActive, Action<AllRecipesView> unregisterActive)
			: this()
		{
			_dataService = dataService;
			_contentWidth = contentWidth;
			_contentHeight = contentHeight;
			_registerActive = registerActive;
			_unregisterActive = unregisterActive;
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			_tooltip = new RecipeTooltip();
			if (_dataService.CoinTexturesLoaded)
			{
				_tooltip.SetCoinTextures(_dataService.CoinGoldTexture, _dataService.CoinSilverTexture, _dataService.CoinCopperTexture);
			}
			_dataService.DataUpdated += OnDataUpdated;
			return Task.FromResult(result: true);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			int count = AllRecipes.Length;
			int rowsPerColumn = (count + 2 - 1) / 2;
			int columnWidth = (_contentWidth - 8) / 2;
			int iconOffset = 36;
			int labelOffset = iconOffset + 24 + 4;
			int labelWidth = columnWidth - labelOffset;
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(_contentWidth, _contentHeight - 10));
			val.set_CanScroll(true);
			Panel scrollPanel = val;
			AsyncTexture2D texture = default(AsyncTexture2D);
			for (int i = 0; i < count; i++)
			{
				RecipeDef def = AllRecipes[i];
				int num = i / rowsPerColumn;
				int row = i % rowsPerColumn;
				int x = num * (columnWidth + 8);
				int y = row * 32;
				Checkbox val2 = new Checkbox();
				((Control)val2).set_Parent((Container)(object)scrollPanel);
				val2.set_Text("");
				((Control)val2).set_Location(new Point(x + 9, y + 14 - 8));
				((Control)val2).set_Enabled(false);
				Checkbox check = val2;
				AsyncTexture2D.TryFromAssetId(def.SheetIconId, ref texture);
				Image val3 = new Image();
				((Control)val3).set_Parent((Container)(object)scrollPanel);
				val3.set_Texture(texture);
				((Control)val3).set_Location(new Point(x + iconOffset, y + 2));
				((Control)val3).set_Size(new Point(24, 24));
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)scrollPanel);
				val4.set_Text(StripRecipePrefix(def.SheetName));
				((Control)val4).set_Location(new Point(x + labelOffset, y));
				((Control)val4).set_Size(new Point(labelWidth, 28));
				val4.set_Font(GameService.Content.get_DefaultFont16());
				val4.set_TextColor(RecipeTooltip.GetRarityColor(def.Rarity));
				val4.set_WrapText(false);
				Label label = val4;
				int index = i;
				((Control)val3).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					ShowTooltip(index);
				});
				((Control)check).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					ShowTooltip(index);
				});
				((Control)label).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					ShowTooltip(index);
				});
				((Control)val3).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
				{
					MoveTooltip();
				});
				((Control)check).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
				{
					MoveTooltip();
				});
				((Control)label).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
				{
					MoveTooltip();
				});
				((Control)val3).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					HideTooltip();
				});
				((Control)check).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					HideTooltip();
				});
				((Control)label).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					HideTooltip();
				});
				_checks[i] = check;
			}
			_isBuilt = true;
			_registerActive?.Invoke(this);
			ApplyKnownState();
			_dataService.Fetch();
		}

		private void ShowTooltip(int index)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Point mouse = GameService.Input.get_Mouse().get_Position();
			_tooltip.MoveTo(mouse.X, mouse.Y);
			RecipeDef def = AllRecipes[index];
			AsyncTexture2D craftedTexture = default(AsyncTexture2D);
			AsyncTexture2D.TryFromAssetId(def.IconId, ref craftedTexture);
			_tooltip.SetRecipe(def, craftedTexture);
			((Control)_tooltip).set_Visible(true);
		}

		private void MoveTooltip()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Point mouse = GameService.Input.get_Mouse().get_Position();
			_tooltip.MoveTo(mouse.X, mouse.Y);
		}

		private void HideTooltip()
		{
			((Control)_tooltip).set_Visible(false);
		}

		private void OnDataUpdated()
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				if (_isBuilt)
				{
					ApplyKnownState();
				}
			});
		}

		private void ApplyKnownState()
		{
			HashSet<int> known = _dataService.KnownCraftingRecipeIds;
			if (known == null)
			{
				return;
			}
			for (int i = 0; i < AllRecipes.Length; i++)
			{
				RecipeDef def = AllRecipes[i];
				_checks[i].set_Checked(def.CraftingRecipeIds != null && Array.Exists(def.CraftingRecipeIds, (int id) => known.Contains(id)));
			}
		}

		public void OnWindowHidden()
		{
			if (_tooltip != null)
			{
				((Control)_tooltip).set_Visible(false);
			}
		}

		protected override void Unload()
		{
			_isBuilt = false;
			_unregisterActive?.Invoke(this);
			_dataService.DataUpdated -= OnDataUpdated;
			RecipeTooltip tooltip = _tooltip;
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
		}
	}
}
