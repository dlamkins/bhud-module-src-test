using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.Refinement
{
	public class CustomTableMetal
	{
		private static FlowPanel _tablePanel;

		private static FlowPanel _nameTimer;

		private static FlowPanel _name;

		private static FlowPanel _def;

		private static FlowPanel _eff1;

		private static FlowPanel _eff2;

		private static FlowPanel _defQty;

		private static FlowPanel _defBuy;

		private static FlowPanel _defSell;

		private static FlowPanel _eff1Qty;

		private static FlowPanel _eff1Buy;

		private static FlowPanel _eff1Sell;

		private static FlowPanel _eff2Qty;

		private static FlowPanel _eff2Buy;

		private static FlowPanel _eff2Sell;

		private static Texture2D _copper = DecorModule.DecorModuleInstance.CopperCoin;

		private static Texture2D _silver = DecorModule.DecorModuleInstance.SilverCoin;

		private static Texture2D _effiency = DecorModule.DecorModuleInstance.Efficiency;

		private static Texture2D _arrowUp = DecorModule.DecorModuleInstance.ArrowUp;

		private static Texture2D _arrowDown = DecorModule.DecorModuleInstance.ArrowDown;

		private static Texture2D _arrowNeutral = DecorModule.DecorModuleInstance.ArrowNeutral;

		private static Dictionary<Item, Tooltip> itemTooltips = new Dictionary<Item, Tooltip>();

		private static Timer _updateTimer;

		private static int secondsCounter = 0;

		private static string _activeColumn = "Name";

		private static bool _isAscending = true;

		private static Dictionary<string, List<Item>> _currentItemsByType = new Dictionary<string, List<Item>>();

		private static Dictionary<string, Dictionary<string, List<Item>>> _itemsByCategoryByType = new Dictionary<string, Dictionary<string, List<Item>>>();

		public static async Task Initialize(FlowPanel tablePanel, string type)
		{
			if (!_itemsByCategoryByType.ContainsKey(type))
			{
				Dictionary<string, Dictionary<string, List<Item>>> itemsByCategoryByType = _itemsByCategoryByType;
				itemsByCategoryByType[type] = await ItemFetcher.FetchItemsAsync(type);
				_currentItemsByType[type] = null;
			}
			_tablePanel = tablePanel;
			((Control)_tablePanel).set_Location(new Point(20, 0));
			((Panel)_tablePanel).set_ShowBorder(true);
			((Control)_tablePanel).set_Width(1050);
			_tablePanel.set_ControlPadding(new Vector2(2f, 0f));
			_name = await CreateHeader("Name", 255, 565, (Item item) => item.Name, type);
			((Panel)_name).set_Icon(AsyncTexture2D.op_Implicit(_arrowDown));
			_def = await CreateHeader("Default", 255, 600, null, type);
			_eff1 = await CreateHeader("Trade Efficiency (1x)", 255, 600, null, type);
			((Panel)_eff1).set_Icon(AsyncTexture2D.op_Implicit(_effiency));
			_eff2 = await CreateHeader("Trade Efficiency (2x)", 255, 600, null, type);
			((Panel)_eff2).set_Icon(AsyncTexture2D.op_Implicit(_effiency));
			_defQty = CreateInnerHeader("Qty", 68, (Panel)(object)_def, (Item item) => item.DefaultQty, type);
			_defBuy = CreateInnerHeader("Buy", 93, (Panel)(object)_def, (Item item) => item.DefaultBuy, type);
			_defSell = CreateInnerHeader("Sell", 93, (Panel)(object)_def, (Item item) => item.DefaultSell, type);
			_eff1Qty = CreateInnerHeader("Qty", 68, (Panel)(object)_eff1, (Item item) => item.TradeEfficiency1Qty, type);
			_eff1Buy = CreateInnerHeader("Buy", 93, (Panel)(object)_eff1, (Item item) => item.TradeEfficiency1Buy, type);
			_eff1Sell = CreateInnerHeader("Sell", 93, (Panel)(object)_eff1, (Item item) => item.TradeEfficiency1Sell, type);
			_eff2Qty = CreateInnerHeader("Qty", 68, (Panel)(object)_eff2, (Item item) => item.TradeEfficiency2Qty, type);
			_eff2Buy = CreateInnerHeader("Buy", 93, (Panel)(object)_eff2, (Item item) => item.TradeEfficiency2Buy, type);
			_eff2Sell = CreateInnerHeader("Sell", 93, (Panel)(object)_eff2, (Item item) => item.TradeEfficiency2Sell, type);
			await PopulateTable(type);
		}

		private static async Task<FlowPanel> CreateHeader(string text, int xSize, int ySize, Func<Item, IComparable> sortKeySelector = null, string type = null)
		{
			if (text == "Name")
			{
				await InitializeNameTimer((Panel)(object)_tablePanel, type);
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)((text == "Name") ? _nameTimer : _tablePanel));
			((Panel)val).set_Title(text);
			((Control)val).set_Size(new Point(xSize, ySize));
			FlowPanel headerPanel = val;
			if (sortKeySelector != null)
			{
				((Control)headerPanel).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ResetHeaderIcons();
					if (_activeColumn == text)
					{
						_isAscending = !_isAscending;
					}
					else
					{
						_activeColumn = text;
						_isAscending = true;
					}
					((Panel)headerPanel).set_Icon(AsyncTexture2D.op_Implicit(_isAscending ? _arrowDown : _arrowUp));
					SortAndPopulate(type, sortKeySelector);
				});
			}
			return headerPanel;
		}

		private static FlowPanel CreateInnerHeader(string text, int xSize, Panel flowPanel, Func<Item, IComparable> sortKeySelector = null, string type = null)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)flowPanel);
			((Panel)val).set_Title(text);
			((Control)val).set_Width(xSize);
			((Control)val).set_Location(new Point(10, 0));
			((Panel)val).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			FlowPanel headerPanel = val;
			if (sortKeySelector != null)
			{
				((Control)headerPanel).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ResetHeaderIcons();
					if (_activeColumn == text)
					{
						_isAscending = !_isAscending;
					}
					else
					{
						_activeColumn = text;
						_isAscending = true;
					}
					((Panel)headerPanel).set_Icon(AsyncTexture2D.op_Implicit((!(_activeColumn == text)) ? null : (_isAscending ? _arrowDown : _arrowUp)));
					SortAndPopulate(type, delegate(Item item)
					{
						try
						{
							if (text == "Buy" || text == "Sell")
							{
								return int.Parse(sortKeySelector(item).ToString());
							}
						}
						catch
						{
							return 0;
						}
						return sortKeySelector(item);
					});
				});
			}
			return headerPanel;
		}

		private static async Task SortAndPopulate(string type, Func<Item, IComparable> sortKeySelector)
		{
			List<Item> sortedItems = (from item in _currentItemsByType[type]
				orderby (!_isAscending) ? null : sortKeySelector(item), _isAscending ? null : sortKeySelector(item) descending
				select item).ToList();
			if (!sortedItems.SequenceEqual(_currentItemsByType[type]))
			{
				_currentItemsByType[type] = sortedItems;
				await PopulateTable(type);
			}
		}

		public static async Task PopulateTable(string type)
		{
			ClearColumnContent();
			if (_currentItemsByType[type] == null)
			{
				_currentItemsByType[type] = (from item in _itemsByCategoryByType[type].SelectMany((KeyValuePair<string, List<Item>> category) => category.Value)
					orderby (!_isAscending) ? null : item.Name, _isAscending ? null : item.Name descending
					select item).ToList();
			}
			int itemCount = 0;
			foreach (Item item2 in _currentItemsByType[type])
			{
				itemCount++;
				Color rowBackgroundColor = ((itemCount % 2 == 0) ? new Color(0, 0, 0, 175) : new Color(0, 0, 0, 75));
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)_name);
				val.set_FlowDirection((ControlFlowDirection)1);
				((Control)val).set_Size(new Point(255, 30));
				((Control)val).set_Padding(new Thickness(10f, 10f));
				FlowPanel val2 = val;
				((Control)val2).set_Tooltip(await CustomTooltip(item2));
				FlowPanel nameFlowPanel = val;
				Texture2D texture = await LeftSideSection.GetOrCreateTextureAsync(item2.Name, item2.Icon);
				if (texture != null)
				{
					Image val3 = new Image(AsyncTexture2D.op_Implicit(texture));
					((Control)val3).set_Parent((Container)(object)nameFlowPanel);
					((Control)val3).set_Size(new Point(30, 30));
					((Control)val3).set_Location(new Point(0, 0));
					((Control)val3).set_Tooltip(((Control)nameFlowPanel).get_Tooltip());
				}
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)nameFlowPanel);
				val4.set_Text(" " + item2.Name);
				((Control)val4).set_Size(new Point(255, 30));
				val4.set_TextColor(Color.get_White());
				val4.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val4).set_BackgroundColor(rowBackgroundColor);
				((Control)val4).set_Padding(new Thickness(10f, 10f));
				((Control)val4).set_Tooltip(((Control)nameFlowPanel).get_Tooltip());
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)_defQty);
				val5.set_Text("    " + item2.DefaultQty);
				((Control)val5).set_Size(new Point(68, 30));
				val5.set_TextColor(Color.get_White());
				val5.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val5).set_BackgroundColor(rowBackgroundColor);
				((Control)val5).set_Tooltip(((Control)nameFlowPanel).get_Tooltip());
				await CreateCurrencyDisplay(_defBuy, item2, item2.DefaultBuy, rowBackgroundColor);
				await CreateCurrencyDisplay(_defSell, item2, item2.DefaultSell, rowBackgroundColor);
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)_eff1Qty);
				val6.set_Text("    " + item2.TradeEfficiency1Qty);
				((Control)val6).set_Size(new Point(68, 30));
				val6.set_TextColor(Color.get_White());
				val6.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val6).set_BackgroundColor(rowBackgroundColor);
				((Control)val6).set_Tooltip(((Control)nameFlowPanel).get_Tooltip());
				await CreateCurrencyDisplay(_eff1Buy, item2, item2.TradeEfficiency1Buy, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff1Sell, item2, item2.TradeEfficiency1Sell, rowBackgroundColor);
				Label val7 = new Label();
				((Control)val7).set_Parent((Container)(object)_eff2Qty);
				val7.set_Text("    " + item2.TradeEfficiency2Qty);
				((Control)val7).set_Size(new Point(68, 30));
				val7.set_TextColor(Color.get_White());
				val7.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val7).set_BackgroundColor(rowBackgroundColor);
				((Control)val7).set_Tooltip(((Control)nameFlowPanel).get_Tooltip());
				await CreateCurrencyDisplay(_eff2Buy, item2, item2.TradeEfficiency2Buy, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff2Sell, item2, item2.TradeEfficiency2Sell, rowBackgroundColor);
			}
			int padding = 125;
			int totalHeight = 30 * itemCount + padding;
			((Control)_name).set_Size(new Point(((Control)_name).get_Size().X, totalHeight - 35));
			((Control)_def).set_Size(new Point(((Control)_def).get_Size().X, totalHeight));
			((Control)_eff1).set_Size(new Point(((Control)_eff1).get_Size().X, totalHeight));
			((Control)_eff2).set_Size(new Point(((Control)_eff2).get_Size().X, totalHeight));
			UpdateInnerPanelHeights();
		}

		private static async Task<FlowPanel> CreateCurrencyDisplay(FlowPanel parent, Item item, string value, Color backgroundColor)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Size(new Point(98, 30));
			((Control)val).set_BackgroundColor(backgroundColor);
			FlowPanel val2 = val;
			((Control)val2).set_Tooltip(await CustomTooltip(item));
			FlowPanel flowPanel = val;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)flowPanel);
			val3.set_Text((value.Length < 3) ? string.Empty : ((value.Length == 3) ? ("  " + value.Substring(value.Length - 3, 1)) : (" " + value.Substring(value.Length - 4, 2))));
			((Control)val3).set_Size((value.Length > 2) ? new Point(25, 30) : new Point(48, 30));
			val3.set_TextColor(Color.get_White());
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Tooltip(((Control)flowPanel).get_Tooltip());
			Image val4 = new Image(AsyncTexture2D.op_Implicit(_silver));
			((Control)val4).set_Parent((Container)(object)((value.Length > 2) ? flowPanel : null));
			((Control)val4).set_Tooltip(((Control)flowPanel).get_Tooltip());
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)flowPanel);
			val5.set_Text((value.Length >= 2) ? value.Substring(value.Length - 2) : value);
			((Control)val5).set_Size(new Point(22, 30));
			val5.set_TextColor(Color.get_White());
			val5.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val5).set_Tooltip(((Control)flowPanel).get_Tooltip());
			Image val6 = new Image(AsyncTexture2D.op_Implicit(_copper));
			((Control)val6).set_Parent((Container)(object)flowPanel);
			((Control)val6).set_Tooltip(((Control)flowPanel).get_Tooltip());
			return flowPanel;
		}

		private static void UpdateInnerPanelHeights()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			((Control)_nameTimer).set_Size(new Point(((Control)_name).get_Size().X, ((Control)_name).get_Size().Y - 15));
			((Control)_name).set_Location(new Point(0, 35));
			((Control)_defQty).set_Size(new Point(((Control)_defQty).get_Size().X, ((Control)_def).get_Size().Y - 86));
			((Control)_defBuy).set_Size(new Point(((Control)_defBuy).get_Size().X, ((Control)_def).get_Size().Y - 86));
			((Control)_defSell).set_Size(new Point(((Control)_defSell).get_Size().X, ((Control)_def).get_Size().Y - 86));
			((Control)_eff1Qty).set_Size(new Point(((Control)_eff1Qty).get_Size().X, ((Control)_eff1).get_Size().Y - 86));
			((Control)_eff1Buy).set_Size(new Point(((Control)_eff1Buy).get_Size().X, ((Control)_eff1).get_Size().Y - 86));
			((Control)_eff1Sell).set_Size(new Point(((Control)_eff1Sell).get_Size().X, ((Control)_eff1).get_Size().Y - 86));
			((Control)_eff2Qty).set_Size(new Point(((Control)_eff2Qty).get_Size().X, ((Control)_eff2).get_Size().Y - 86));
			((Control)_eff2Buy).set_Size(new Point(((Control)_eff2Buy).get_Size().X, ((Control)_eff2).get_Size().Y - 86));
			((Control)_eff2Sell).set_Size(new Point(((Control)_eff2Sell).get_Size().X, ((Control)_eff2).get_Size().Y - 86));
		}

		private static void ClearColumnContent()
		{
			((Container)_name).get_Children().Clear();
			((Container)_defQty).get_Children().Clear();
			((Container)_defBuy).get_Children().Clear();
			((Container)_defSell).get_Children().Clear();
			((Container)_eff1Qty).get_Children().Clear();
			((Container)_eff1Buy).get_Children().Clear();
			((Container)_eff1Sell).get_Children().Clear();
			((Container)_eff2Qty).get_Children().Clear();
			((Container)_eff2Buy).get_Children().Clear();
			((Container)_eff2Sell).get_Children().Clear();
		}

		private static void ResetHeaderIcons()
		{
			((Panel)_name).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_defQty).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_defBuy).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_defSell).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff1Qty).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff1Buy).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff1Sell).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff2Qty).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff2Buy).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
			((Panel)_eff2Sell).set_Icon(AsyncTexture2D.op_Implicit(_arrowNeutral));
		}

		private static async Task<Tooltip> CustomTooltip(Item item)
		{
			if (itemTooltips.ContainsKey(item))
			{
				return itemTooltips[item];
			}
			Tooltip customTooltip = new Tooltip();
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)customTooltip);
			Image val2 = val;
			val2.set_Texture(AsyncTexture2D.op_Implicit(LeftSideSection.CreateIconTexture(await DecorModule.DecorModuleInstance.Client.GetByteArrayAsync(item.Icon))));
			((Control)val).set_Size(new Point(30, 30));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)customTooltip);
			val3.set_Text(item.Name);
			val3.set_TextColor(Color.get_White());
			val3.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val3).set_Location(new Point(35, 3));
			val3.set_AutoSizeWidth(true);
			itemTooltips[item] = customTooltip;
			return customTooltip;
		}

		private static async Task RefreshPrices(string type)
		{
			List<Item> items = _currentItemsByType[type];
			items = await ItemFetcher.UpdateItemPrices(items);
			_currentItemsByType[type] = items;
			await UpdatePriceColumns(type);
		}

		private static async Task UpdatePriceColumns(string type)
		{
			((Container)_defBuy).get_Children().Clear();
			((Container)_defSell).get_Children().Clear();
			((Container)_eff1Buy).get_Children().Clear();
			((Container)_eff1Sell).get_Children().Clear();
			((Container)_eff2Buy).get_Children().Clear();
			((Container)_eff2Sell).get_Children().Clear();
			foreach (Item item in _currentItemsByType[type])
			{
				Color rowBackgroundColor = ((_currentItemsByType[type].IndexOf(item) % 2 != 0) ? new Color(0, 0, 0, 175) : new Color(0, 0, 0, 75));
				await CreateCurrencyDisplay(_defBuy, item, item.DefaultBuy, rowBackgroundColor);
				await CreateCurrencyDisplay(_defSell, item, item.DefaultSell, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff1Buy, item, item.TradeEfficiency1Buy, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff1Sell, item, item.TradeEfficiency1Sell, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff2Buy, item, item.TradeEfficiency2Buy, rowBackgroundColor);
				await CreateCurrencyDisplay(_eff2Sell, item, item.TradeEfficiency2Sell, rowBackgroundColor);
			}
		}

		private static async Task InitializeNameTimer(Panel parentPanel, string type)
		{
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parentPanel);
			val.set_FlowDirection((ControlFlowDirection)1);
			((Control)val).set_Padding(new Thickness(10f, 10f));
			_nameTimer = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_nameTimer);
			val2.set_Text("      Prices will update in 30 s");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			((Control)val2).set_Size(new Point(255, 30));
			val2.set_ShowShadow(true);
			val2.set_ShadowColor(new Color(0, 0, 0, 255));
			Label updateTimerLabel = val2;
			int secondsCounter = 60;
			Timer timer = new Timer(1000.0);
			timer.Elapsed += delegate
			{
				if (secondsCounter > 0)
				{
					secondsCounter--;
					updateTimerLabel.set_Text($"      Prices will update in {secondsCounter} s");
				}
			};
			timer.AutoReset = true;
			timer.Enabled = true;
			Timer timer2 = new Timer(61000.0);
			timer2.Elapsed += async delegate
			{
				secondsCounter = 60;
				await RefreshPrices(type);
				updateTimerLabel.set_Text($"      Prices will update in {secondsCounter} s");
			};
			timer2.AutoReset = true;
			timer2.Enabled = true;
		}
	}
}
