using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.SelfHosting;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Controls.Input;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models.BlishHudAPI;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Chat;
using Estreya.BlishHUD.Shared.Models.GameIntegration.Guild;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.GameIntegration;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SemVer;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class SelfHostingEventsView : BaseView
	{
		public class EventChangedArgs
		{
			public bool OldState { get; set; }

			public bool NewState { get; set; }

			public Dictionary<string, object> AdditionalData { get; set; }

			public string EventSettingKey { get; set; }
		}

		private class AddCategoryDropdown
		{
			public string Key { get; set; }

			public string Name { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		private class AddZoneDropdown
		{
			public string CategoryKey { get; set; }

			public string Key { get; set; }

			public string Name { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		private class AddEventDropdown
		{
			public string CategoryKey { get; set; }

			public string ZoneKey { get; set; }

			public string Key { get; set; }

			public string Name { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		private static readonly Point MAIN_PADDING = new Point(20, 20);

		private readonly ModuleSettings _moduleSettings;

		private readonly SelfHostingEventService _selfHostingEventService;

		private readonly AccountService _accountService;

		private readonly ChatService _chatService;

		private static TimeSpan _progressCheckInterval = TimeSpan.FromSeconds(1.0);

		private double _lastProgressCheck = _progressCheckInterval.TotalMilliseconds;

		private FlowPanel _activeEventsGroup;

		private TimeSpan _maxHostingDuration;

		private List<SelfHostingCategoryDefinition> _definitions;

		public Panel Panel { get; private set; }

		public SelfHostingEventsView(ModuleSettings moduleSettings, SelfHostingEventService selfHostingEventService, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, AccountService accountService, ChatService chatService)
			: base(apiManager, iconService, translationService)
		{
			_moduleSettings = moduleSettings;
			_selfHostingEventService = selfHostingEventService;
			_accountService = accountService;
			_chatService = chatService;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - MAIN_PADDING.X * 2);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - MAIN_PADDING.Y * 2);
			val.set_CanScroll(true);
			Panel = val;
			BuildEntriesPanel();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdateUtil.Update(CheckAndUpdateProgress, gameTime, _progressCheckInterval.TotalMilliseconds, ref _lastProgressCheck);
		}

		private void CheckAndUpdateProgress()
		{
			FlowPanel panel = _activeEventsGroup;
			if (panel == null)
			{
				return;
			}
			foreach (DataDetailsButton<SelfHostingEventEntry> child in ((Container)panel).GetChildrenOfType<DataDetailsButton<SelfHostingEventEntry>>().ToList())
			{
				if (child != null)
				{
					SelfHostingEventEntry entry = child.Data;
					((DetailsButton)child).set_CurrentFill((int)MathHelper.Scale((DateTimeOffset.UtcNow - entry.StartTime.ToUniversalTime()).TotalMinutes, 0.0, entry.Duration, 0.0, ((DetailsButton)child).get_MaxFill()));
				}
			}
		}

		private void BuildEntriesPanel()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Expected O, but got Unknown
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Expected O, but got Unknown
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Expected O, but got Unknown
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			ClearPanel();
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			List<SelfHostingCategoryDefinition> categories = _definitions ?? new List<SelfHostingCategoryDefinition>();
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)Panel);
			((Control)val).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val).set_Location(new Point(0, contentRegion.Y));
			((TextInputBase)val).set_PlaceholderText("Search...");
			TextBox searchBox = val;
			Panel val2 = new Panel();
			val2.set_Title("Categories");
			((Control)val2).set_Parent((Container)(object)Panel);
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			((Control)val2).set_Location(new Point(0, ((Control)searchBox).get_Bottom() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			Panel categoriesPanel = val2;
			((Control)categoriesPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, contentRegion.Height - ((Control)categoriesPanel).get_Location().Y - 78));
			Estreya.BlishHUD.Shared.Controls.Menu menu = new Estreya.BlishHUD.Shared.Controls.Menu();
			((Control)menu).set_Parent((Container)(object)categoriesPanel);
			Rectangle contentRegion2 = ((Container)categoriesPanel).get_ContentRegion();
			((Control)menu).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
			((Menu)menu).set_MenuItemHeight(40);
			Estreya.BlishHUD.Shared.Controls.Menu categoryMenu = menu;
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val3).set_CanScroll(true);
			((Panel)val3).set_ShowBorder(true);
			((Control)val3).set_Location(new Point(((Control)categoriesPanel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, contentRegion.Y));
			((Control)val3).set_Parent((Container)(object)Panel);
			val3.set_ControlPadding(new Vector2(5f, 5f));
			val3.set_OuterControlPadding(new Vector2(5f, 5f));
			_activeEventsGroup = val3;
			((Control)_activeEventsGroup).set_Size(new Point(contentRegion.Width - ((Control)_activeEventsGroup).get_Location().X - MAIN_PADDING.X, contentRegion.Height));
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_activeEventsGroup.FilterChildren<DataDetailsButton<SelfHostingEventEntry>>((Func<DataDetailsButton<SelfHostingEventEntry>, bool>)((DataDetailsButton<SelfHostingEventEntry> detailsButton) => ((DetailsButton)detailsButton).get_Text().ToLowerInvariant().Contains(((TextInputBase)searchBox).get_Text().ToLowerInvariant())));
			});
			Dictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
			MenuItem allEvents = ((Menu)categoryMenu).AddMenuItem("All Events", (Texture2D)null);
			allEvents.Select();
			menus.Add("allEvents", allEvents);
			MenuItem divider1MenuItem = ((Menu)categoryMenu).AddMenuItem("-------------------------------------", (Texture2D)null);
			((Control)divider1MenuItem).set_Enabled(false);
			menus.Add("divider1MenuItem", divider1MenuItem);
			switch (_moduleSettings.MenuEventSortMode.get_Value())
			{
			case MenuEventSortMode.Alphabetical:
				categories = categories.OrderBy((SelfHostingCategoryDefinition c) => c.Key).ToList();
				break;
			case MenuEventSortMode.AlphabeticalDesc:
				categories = categories.OrderByDescending((SelfHostingCategoryDefinition c) => c.Key).ToList();
				break;
			}
			foreach (SelfHostingCategoryDefinition category in categories)
			{
				MenuItem categoryItem = ((Menu)categoryMenu).AddMenuItem(category.Name, (Texture2D)null);
				foreach (SelfHostingZoneDefinition zone in category.Zones)
				{
					MenuItem val4 = new MenuItem(zone.Name);
					((Control)val4).set_Parent((Container)(object)categoryItem);
					MenuItem zoneItem = val4;
					menus.Add(category.Key + "-" + zone.Key, zoneItem);
				}
				menus.Add(category.Key, categoryItem);
			}
			MenuItem menuItem;
			string categoryKey;
			string zoneKey;
			SelfHostingCategoryDefinition category2;
			SelfHostingZoneDefinition zone2;
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					try
					{
						menuItem = (MenuItem)((s is MenuItem) ? s : null);
						if (menuItem != null)
						{
							string[] array = menuItemPair.Key.Split('-');
							categoryKey = ((array.Length >= 1) ? array[0] : null);
							zoneKey = ((array.Length >= 2) ? array[1] : null);
							category2 = categories.Find((SelfHostingCategoryDefinition ec) => ec.Key == categoryKey);
							zone2 = category2?.Zones.Find((SelfHostingZoneDefinition z) => z.Key == zoneKey);
							_activeEventsGroup.FilterChildren<DataDetailsButton<SelfHostingEventEntry>>((Func<DataDetailsButton<SelfHostingEventEntry>, bool>)delegate(DataDetailsButton<SelfHostingEventEntry> detailsButton)
							{
								if (menuItem == menus["allEvents"])
								{
									return true;
								}
								if (category2 == null)
								{
									return true;
								}
								SelfHostingEventEntry entry = detailsButton.Data;
								if (entry.CategoryKey != category2.Key)
								{
									return false;
								}
								return (zone2 == null || !(zone2.Key != entry.ZoneKey)) && (category2.Zones.Find((SelfHostingZoneDefinition z) => z.Key == entry.ZoneKey)?.Events.Any((SelfHostingEventDefinition e) => e.Key == entry.EventKey) ?? false);
							});
						}
					}
					catch (Exception ex3)
					{
						ShowError("Failed to filter events:\n" + ex3.Message);
					}
				});
			});
			StandardButton val5 = new StandardButton();
			val5.set_Text("Add");
			((Control)val5).set_Parent((Container)(object)Panel);
			((Control)val5).set_Location(new Point(((Control)categoriesPanel).get_Left(), ((Control)categoriesPanel).get_Bottom()));
			((Control)val5).set_Size(new Point(((Control)categoriesPanel).get_Width(), 26));
			StandardButton addButton = val5;
			((Control)addButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BuildAddPanel();
			});
			StandardButton val6 = new StandardButton();
			val6.set_Text("Remove");
			((Control)val6).set_Parent((Container)(object)Panel);
			((Control)val6).set_Location(new Point(((Control)addButton).get_Left(), ((Control)addButton).get_Bottom()));
			((Control)val6).set_Size(new Point(((Control)categoriesPanel).get_Width(), 26));
			StandardButton removeButton = val6;
			((Control)removeButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				try
				{
					await _selfHostingEventService.DeleteEntry();
					ShowInfo("Deleted your currently hosted entry.");
					UpdateActiveEventsGroup();
				}
				catch (Exception ex2)
				{
					ShowError("Could not delete currently hosted entry: " + ex2.Message);
				}
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Reload");
			((Control)val7).set_Parent((Container)(object)Panel);
			((Control)val7).set_Location(new Point(((Control)removeButton).get_Left(), ((Control)removeButton).get_Bottom()));
			((Control)val7).set_Size(new Point(((Control)categoriesPanel).get_Width(), 26));
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					RefreshEvents();
					ShowInfo("Reloaded");
				}
				catch (Exception ex)
				{
					ShowError("Could not reload: " + ex.Message);
				}
			});
			UpdateActiveEventsGroup();
		}

		private void BuildAddPanel()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Expected O, but got Unknown
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Expected O, but got Unknown
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Expected O, but got Unknown
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Expected O, but got Unknown
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			ClearPanel();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)Panel);
			((Control)val).set_Width(((Container)Panel).get_ContentRegion().Width);
			((Control)val).set_Height(((Container)Panel).get_ContentRegion().Height);
			Panel addPanel = val;
			List<AddCategoryDropdown> parsedCategories = _definitions?.Select((SelfHostingCategoryDefinition c) => new AddCategoryDropdown
			{
				Key = c.Key,
				Name = c.Name
			}).ToList() ?? new List<AddCategoryDropdown>();
			Dropdown<AddZoneDropdown> zoneDropdown = null;
			Dropdown<AddEventDropdown> eventDropdown = null;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)addPanel);
			val2.set_FlowDirection((ControlFlowDirection)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width((int)((float)((Container)addPanel).get_ContentRegion().Width * 0.75f));
			((Control)val2).set_Top(100);
			FlowPanel categoryGroup = val2;
			Label categoryLabel = RenderLabel((Panel)(object)categoryGroup, "Category:").TitleLabel;
			categoryLabel.set_AutoSizeWidth(false);
			((Control)categoryLabel).set_Width(125);
			Point categoryDropdownLocation = default(Point);
			((Point)(ref categoryDropdownLocation))._002Ector(((Control)categoryLabel).get_Right() + 20, 0);
			Dropdown<AddCategoryDropdown> categoryDropdown = RenderDropdown((Panel)(object)categoryGroup, categoryDropdownLocation, ((Container)categoryGroup).get_ContentRegion().Width - categoryDropdownLocation.X, parsedCategories.ToArray(), null, null, async delegate(AddCategoryDropdown oldVal, AddCategoryDropdown newVal)
			{
				if (zoneDropdown != null && newVal != null)
				{
					List<SelfHostingZoneDefinition> obj2 = await _selfHostingEventService.GetCategoryZones(newVal.Key);
					zoneDropdown.Items.Clear();
					foreach (SelfHostingZoneDefinition z in obj2)
					{
						zoneDropdown.Items.Add(new AddZoneDropdown
						{
							CategoryKey = newVal.Key,
							Key = z.Key,
							Name = z.Name
						});
					}
					zoneDropdown.SelectedItem = null;
				}
				return true;
			});
			categoryDropdown.PanelHeight = 500;
			categoryDropdown.PreselectOnItemsChange = false;
			((Control)categoryGroup).RecalculateLayout();
			((Control)categoryGroup).Update(GameService.Overlay.get_CurrentGameTime());
			((Control)categoryGroup).set_Left(((Container)Panel).get_ContentRegion().Width / 2 - ((Control)categoryGroup).get_Width() / 2);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)addPanel);
			val3.set_FlowDirection((ControlFlowDirection)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Control)categoryGroup).get_Width());
			((Control)val3).set_Top(((Control)categoryGroup).get_Bottom() + 10);
			FlowPanel zoneGroup = val3;
			Label zoneLabel = RenderLabel((Panel)(object)zoneGroup, "Zone:").TitleLabel;
			zoneLabel.set_AutoSizeWidth(false);
			((Control)zoneLabel).set_Width(((Control)categoryLabel).get_Width());
			Point zoneDropdownLocation = default(Point);
			((Point)(ref zoneDropdownLocation))._002Ector(((Control)zoneLabel).get_Right() + 20, 0);
			zoneDropdown = RenderDropdown((Panel)(object)zoneGroup, zoneDropdownLocation, ((Container)zoneGroup).get_ContentRegion().Width - zoneDropdownLocation.X, new AddZoneDropdown[0], null, null, async delegate(AddZoneDropdown oldVal, AddZoneDropdown newVal)
			{
				if (eventDropdown != null && newVal != null)
				{
					List<SelfHostingEventDefinition> source = await _selfHostingEventService.GetCategoryZoneEvents(newVal.CategoryKey, newVal.Key);
					eventDropdown.Items.Clear();
					foreach (SelfHostingEventDefinition e2 in source.OrderBy((SelfHostingEventDefinition e) => e.Name))
					{
						eventDropdown.Items.Add(new AddEventDropdown
						{
							CategoryKey = newVal.CategoryKey,
							ZoneKey = newVal.Key,
							Key = e2.Key,
							Name = e2.Name
						});
					}
					eventDropdown.SelectedItem = null;
				}
				return true;
			});
			zoneDropdown.PanelHeight = 500;
			zoneDropdown.PreselectOnItemsChange = false;
			((Control)zoneGroup).RecalculateLayout();
			((Control)zoneGroup).Update(GameService.Overlay.get_CurrentGameTime());
			((Control)zoneGroup).set_Left(((Container)Panel).get_ContentRegion().Width / 2 - ((Control)zoneGroup).get_Width() / 2);
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)addPanel);
			val4.set_FlowDirection((ControlFlowDirection)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Width(((Control)zoneGroup).get_Width());
			((Control)val4).set_Top(((Control)zoneGroup).get_Bottom() + 10);
			FlowPanel eventGroup = val4;
			Label eventLabel = RenderLabel((Panel)(object)eventGroup, "Event:").TitleLabel;
			eventLabel.set_AutoSizeWidth(false);
			((Control)eventLabel).set_Width(((Control)categoryLabel).get_Width());
			Point eventDropdownLocation = default(Point);
			((Point)(ref eventDropdownLocation))._002Ector(((Control)eventLabel).get_Right() + 20, 0);
			eventDropdown = RenderDropdown((Panel)(object)eventGroup, eventDropdownLocation, ((Container)eventGroup).get_ContentRegion().Width - eventDropdownLocation.X, new AddEventDropdown[0], null);
			eventDropdown.PanelHeight = 500;
			eventDropdown.PreselectOnItemsChange = false;
			((Control)eventGroup).RecalculateLayout();
			((Control)eventGroup).Update(GameService.Overlay.get_CurrentGameTime());
			((Control)eventGroup).set_Left(((Container)Panel).get_ContentRegion().Width / 2 - ((Control)eventGroup).get_Width() / 2);
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)addPanel);
			val5.set_FlowDirection((ControlFlowDirection)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Width(((Control)eventGroup).get_Width());
			((Control)val5).set_Top(((Control)eventGroup).get_Bottom() + 10);
			FlowPanel durationGroup = val5;
			Label durationLabel = RenderLabel((Panel)(object)durationGroup, "Duration (min):").TitleLabel;
			durationLabel.set_AutoSizeWidth(false);
			((Control)durationLabel).set_Width(((Control)categoryLabel).get_Width());
			Point durationDropdownLocation = default(Point);
			((Point)(ref durationDropdownLocation))._002Ector(((Control)durationLabel).get_Right() + 20, 0);
			int maxHostingMinutes = (int)_maxHostingDuration.TotalMinutes;
			Dropdown<int> durationDropdown = RenderDropdown((Panel)(object)durationGroup, durationDropdownLocation, ((Container)durationGroup).get_ContentRegion().Width - durationDropdownLocation.X, Enumerable.Range(1, maxHostingMinutes).ToArray(), Math.Min(5, maxHostingMinutes));
			durationDropdown.PanelHeight = 500;
			((Control)durationGroup).RecalculateLayout();
			((Control)durationGroup).Update(GameService.Overlay.get_CurrentGameTime());
			((Control)durationGroup).set_Left(((Container)Panel).get_ContentRegion().Width / 2 - ((Control)durationGroup).get_Width() / 2);
			FormattedLabel obj = new FormattedLabelBuilder().SetWidth(((Control)durationGroup).get_Width()).AutoSizeHeight().Wrap()
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("The start time will be the current time you click on \"Add\".", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build();
			((Control)obj).set_Parent((Container)(object)addPanel);
			((Control)obj).set_Left(((Control)durationGroup).get_Left());
			((Control)obj).set_Top(((Control)durationGroup).get_Bottom() + 20);
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)addPanel);
			Rectangle contentRegion = ((Container)addPanel).get_ContentRegion();
			((Control)val6).set_Top(((Rectangle)(ref contentRegion)).get_Bottom() - 36);
			val6.set_FlowDirection((ControlFlowDirection)2);
			((Container)val6).set_WidthSizingMode((SizingMode)1);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			FlowPanel buttonGroup = val6;
			RenderButtonAsync((Panel)(object)buttonGroup, "Add", async delegate
			{
				try
				{
					await _selfHostingEventService.AddEntry(categoryDropdown.SelectedItem?.Key, zoneDropdown.SelectedItem?.Key, eventDropdown.SelectedItem?.Key, DateTimeOffset.UtcNow, durationDropdown.SelectedItem);
					ShowInfo("Added successfully.");
					BuildEntriesPanel();
				}
				catch (FlurlHttpException ex3)
				{
					APIError error = await ex3.GetResponseJsonAsync<APIError>();
					ShowError("Could not add entry: " + error.Message);
				}
				catch (Exception ex2)
				{
					ShowError("Could not add entry: " + ex2.Message);
				}
			});
			RenderButton((Panel)(object)buttonGroup, "Cancel", delegate
			{
				BuildEntriesPanel();
			});
			((Control)RenderButtonAsync((Panel)(object)buttonGroup, "Remove current", async delegate
			{
				try
				{
					await _selfHostingEventService.DeleteEntry();
					ShowInfo("Deleted your currently hosted entry.");
				}
				catch (Exception)
				{
					ShowError("Could not delete currently hosted entry.");
				}
			})).set_Enabled(_selfHostingEventService.HasSelfHostingEntry());
			((Control)buttonGroup).RecalculateLayout();
			((Control)buttonGroup).Update(GameService.Overlay.get_CurrentGameTime());
			((Control)buttonGroup).set_Left(((Container)addPanel).get_ContentRegion().Width / 2 - ((Control)buttonGroup).get_Width() / 2);
		}

		private void ClearPanel()
		{
			((Container)Panel).ClearChildren();
		}

		private void RefreshEvents()
		{
			UpdateActiveEventsGroup();
		}

		private void UpdateActiveEventsGroup()
		{
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Expected O, but got Unknown
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Expected O, but got Unknown
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Expected O, but got Unknown
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			((Container)_activeEventsGroup).ClearChildren();
			Account account = _accountService.Account;
			string currentAccountName = ((account != null) ? account.get_Name() : null);
			foreach (SelfHostingEventEntry ev in _selfHostingEventService.Events.ToArray().ToList())
			{
				bool isMyHosting = ev.AccountName == currentAccountName;
				TimeSpan runningDuration = DateTimeOffset.UtcNow - ev.StartTime;
				SelfHostingCategoryDefinition obj = _definitions?.FirstOrDefault((SelfHostingCategoryDefinition c) => c.Key == ev.CategoryKey);
				string categoryName = obj?.Name ?? ev.CategoryKey;
				SelfHostingZoneDefinition zoneDef = obj?.Zones?.FirstOrDefault((SelfHostingZoneDefinition z) => z.Key == ev.ZoneKey);
				string zoneName = zoneDef?.Name ?? ev.ZoneKey;
				SelfHostingEventDefinition eventDef = zoneDef?.Events?.FirstOrDefault((SelfHostingEventDefinition e) => e.Key == ev.EventKey);
				string eventName = eventDef?.Name ?? ev.EventKey;
				DateTimeOffset startTimeLocal = ev.StartTime.ToLocalTime();
				string startTimeString = ((startTimeLocal.Date == DateTimeOffset.Now.Date) ? startTimeLocal.ToString("t") : startTimeLocal.ToString("g"));
				DataDetailsButton<SelfHostingEventEntry> dataDetailsButton = new DataDetailsButton<SelfHostingEventEntry>();
				((Control)dataDetailsButton).set_Parent((Container)(object)_activeEventsGroup);
				dataDetailsButton.Data = ev;
				((DetailsButton)dataDetailsButton).set_Text($"{categoryName} - {zoneName} - {eventName}\n\nHosted by: {ev.AccountName}\nStarting: {startTimeString}\nDuration: {ev.Duration} minutes");
				((DetailsButton)dataDetailsButton).set_Icon(base.IconService.GetIcon("42681.png"));
				((DetailsButton)dataDetailsButton).set_ShowToggleButton(true);
				((Control)dataDetailsButton).set_Height(175);
				((Control)dataDetailsButton).set_Width(500);
				((DetailsButton)dataDetailsButton).set_IconSize((DetailsIconSize)0);
				((Control)dataDetailsButton).set_BackgroundColor(isMyHosting ? (Color.get_LightGreen() * 0.25f) : Color.get_Transparent());
				DataDetailsButton<SelfHostingEventEntry> detailsButton = dataDetailsButton;
				if (Program.get_OverlayVersion() >= new Version(1, 2, 0, (string)null, (string)null))
				{
					((DetailsButton)detailsButton).set_MaxFill(100);
					((DetailsButton)detailsButton).set_ShowVignette(true);
					((DetailsButton)detailsButton).set_CurrentFill((int)MathHelper.Scale(runningDuration.TotalMinutes, 0.0, ev.Duration, 0.0, ((DetailsButton)detailsButton).get_MaxFill()));
					((DetailsButton)detailsButton).set_ShowFillFraction(true);
				}
				GlowButton val = new GlowButton();
				((Control)val).set_Parent((Container)(object)detailsButton);
				((Control)val).set_BasicTooltipText((!isMyHosting) ? "Ask for invite" : "You can't ask yourself. Or can you?");
				val.set_Icon(base.IconService.GetIcon("156386.png"));
				((Control)val).set_Enabled(!isMyHosting);
				GlowButton askForInviteButton = val;
				if (isMyHosting)
				{
					askForInviteButton.set_GlowColor(Color.get_Transparent());
				}
				((Control)askForInviteButton).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					_logger.Debug("Asking for invite for event: " + ev.CategoryKey + " - " + ev.EventKey + " | Hosted by: " + ev.AccountName);
					try
					{
						await _chatService.ChangeChannel(ChatChannel.Squad);
						await _chatService.ChangeChannel(ChatChannel.Private, GuildNumber.Guild_1, ev.AccountName);
						await _chatService.Send("Hey, I would like to join your event! Can you invite me to a party/squad?");
					}
					catch (Exception ex3)
					{
						ShowError("Could not send chat message.");
						_logger.Warn(ex3, "Could not send chat message.");
					}
				});
				bool sameInstance = ev.InstanceIP == GameService.Gw2Mumble.get_Info().get_ServerAddress();
				if (!isMyHosting)
				{
					if (sameInstance)
					{
						GlowButton val2 = new GlowButton();
						((Control)val2).set_Parent((Container)(object)detailsButton);
						((Control)val2).set_BasicTooltipText("You are on the same instance as the host.");
						val2.set_Icon(base.IconService.GetIcon("155023.png"));
					}
					else
					{
						GlowButton val3 = new GlowButton();
						((Control)val3).set_Parent((Container)(object)detailsButton);
						((Control)val3).set_BasicTooltipText("You are NOT on the same instance as the host.");
						val3.set_Icon(base.IconService.GetIcon("155018.png"));
					}
				}
				if (!string.IsNullOrWhiteSpace(eventDef?.WikiUrl))
				{
					GlowButton val4 = new GlowButton();
					((Control)val4).set_Parent((Container)(object)detailsButton);
					((Control)val4).set_BasicTooltipText("Open wiki page for this event.");
					val4.set_Icon(base.IconService.GetIcon("102353.png"));
					((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						try
						{
							Process.Start(eventDef.WikiUrl);
						}
						catch (Exception ex2)
						{
							ShowError(ex2.Message);
						}
					});
				}
				GlowButton val5 = new GlowButton();
				((Control)val5).set_Parent((Container)(object)detailsButton);
				((Control)val5).set_BasicTooltipText((!isMyHosting) ? "Report this host." : "Why would you report yourself?");
				val5.set_Icon(base.IconService.GetIcon("851256.png"));
				((Control)val5).set_Enabled(!isMyHosting);
				GlowButton reportButton = val5;
				if (isMyHosting)
				{
					reportButton.set_GlowColor(Color.get_Transparent());
				}
				((Control)reportButton).add_Click((EventHandler<MouseEventArgs>)async delegate(object s, MouseEventArgs e)
				{
					DataDetailsButton<SelfHostingEventEntry> detailsButton2 = ((Control)((s is GlowButton) ? s : null)).get_Parent() as DataDetailsButton<SelfHostingEventEntry>;
					SelfHostingEventEntry detailsButtonEvent = detailsButton2.Data;
					try
					{
						Dropdown<SelfHostingReportType> dropdownReportType = new Dropdown<SelfHostingReportType>
						{
							PanelHeight = 200
						};
						SelfHostingReportType[] array = (SelfHostingReportType[])Enum.GetValues(typeof(SelfHostingReportType));
						foreach (SelfHostingReportType item in array)
						{
							dropdownReportType.Items.Add(item);
						}
						dropdownReportType.SelectedItem = SelfHostingReportType.Spam;
						InputDialog<Dropdown<SelfHostingReportType>> inputDialogReportType = new InputDialog<Dropdown<SelfHostingReportType>>(dropdownReportType, "Reporting " + detailsButtonEvent.AccountName + " - Select Type", "Please selected the matching offence for the report", base.IconService);
						if (await inputDialogReportType.ShowDialog() == DialogResult.OK)
						{
							SelfHostingReportType reportType = (SelfHostingReportType)inputDialogReportType.Input;
							TextBox val6 = new TextBox();
							((TextInputBase)val6).set_PlaceholderText("Reason");
							InputDialog<TextBox> inputDialogReportReason = new InputDialog<TextBox>(val6, "Reporting " + detailsButtonEvent.AccountName + " - Select Reason", "Please provide a reason for the report.", base.IconService);
							if (await inputDialogReportReason.ShowDialog() == DialogResult.OK)
							{
								string reportReason = (string)inputDialogReportReason.Input;
								await _selfHostingEventService.ReportHost(detailsButtonEvent.AccountName, reportType, reportReason);
							}
						}
					}
					catch (FlurlHttpException fhex)
					{
						APIError content = await fhex.GetResponseJsonAsync<APIError>();
						_logger.Warn((Exception)fhex, "Could not report host: " + content.Message);
						ShowError("Could not report host: " + content.Message);
					}
					catch (Exception ex)
					{
						ShowError("Could not report host.");
						_logger.Warn(ex, "Could not report host.");
					}
				});
			}
			FlowPanel activeEventsGroup = _activeEventsGroup;
			((Control)activeEventsGroup).set_Height(((Control)activeEventsGroup).get_Height() - 1);
			FlowPanel activeEventsGroup2 = _activeEventsGroup;
			((Control)activeEventsGroup2).set_Height(((Control)activeEventsGroup2).get_Height() + 1);
			SortActiveEvents();
		}

		private void SortActiveEvents()
		{
			_activeEventsGroup.SortChildren<DataDetailsButton<SelfHostingEventEntry>>((Comparison<DataDetailsButton<SelfHostingEventEntry>>)delegate(DataDetailsButton<SelfHostingEventEntry> a, DataDetailsButton<SelfHostingEventEntry> b)
			{
				Account account = _accountService.Account;
				string text = ((account != null) ? account.get_Name() : null);
				if (a.Data.AccountName == b.Data.AccountName)
				{
					return 0;
				}
				if (a.Data.AccountName == text && b.Data.AccountName != text)
				{
					return -1;
				}
				return (a.Data.AccountName != text && b.Data.AccountName == text) ? 1 : 0;
			});
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			await TryLoadingCategories();
			await TryLoadingMaxHostingDuration();
			return true;
		}

		private async Task TryLoadingCategories()
		{
			try
			{
				_definitions = await _selfHostingEventService.GetDefinitions();
			}
			catch (Exception ex)
			{
				_logger.Warn(ex, "Failed to load definitions.");
			}
		}

		private async Task TryLoadingMaxHostingDuration()
		{
			try
			{
				_maxHostingDuration = TimeSpan.FromSeconds(await _selfHostingEventService.GetMaxHostingDuration());
			}
			catch (Exception ex)
			{
				_logger.Warn(ex, "Failed to load max hosting duration.");
			}
		}
	}
}
