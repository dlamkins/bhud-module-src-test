using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageEventsView : BaseView
	{
		public class EventChangedArgs
		{
			public bool OldState { get; set; }

			public bool NewState { get; set; }

			public Dictionary<string, object> AdditionalData { get; set; }

			public string EventSettingKey { get; set; }
		}

		public struct CustomActionDefinition
		{
			public string Name { get; set; }

			public string Tooltip { get; set; }

			public string Icon { get; set; }

			public Action<Estreya.BlishHUD.EventTable.Models.Event> Action { get; set; }
		}

		private static readonly Point MAIN_PADDING = new Point(20, 20);

		private static readonly Logger Logger = Logger.GetLogger<ManageEventsView>();

		private readonly Dictionary<string, object> _additionalData;

		private readonly Func<List<string>> _getDisabledEventKeys;

		private readonly ModuleSettings _moduleSettings;

		private readonly List<EventCategory> allEvents;

		public Panel Panel { get; private set; }

		public event EventHandler<EventChangedArgs> EventChanged;

		public ManageEventsView(List<EventCategory> allEvents, Dictionary<string, object> additionalData, Func<List<string>> getDisabledEventKeys, ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			this.allEvents = allEvents;
			_additionalData = additionalData ?? new Dictionary<string, object>();
			_getDisabledEventKeys = getDisabledEventKeys;
			_moduleSettings = moduleSettings;
		}

		private void UpdateToggleButton(GlowButton button)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				button.set_Icon(button.get_Checked() ? base.IconService.GetIcon("784259.png") : base.IconService.GetIcon("784261.png"));
			});
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Expected O, but got Unknown
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Expected O, but got Unknown
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_065a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_067e: Expected O, but got Unknown
			//IL_067e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0704: Expected O, but got Unknown
			//IL_0704: Unknown result type (might be due to invalid IL or missing references)
			//IL_076a: Unknown result type (might be due to invalid IL or missing references)
			//IL_076f: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_0799: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_082c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0843: Unknown result type (might be due to invalid IL or missing references)
			//IL_084a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0878: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e0: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - MAIN_PADDING.X);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - MAIN_PADDING.Y);
			val.set_CanScroll(true);
			Panel = val;
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			List<EventCategory> eventCategories = this.allEvents;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)Panel);
			((Control)val2).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val2).set_Location(new Point(0, contentRegion.Y));
			((TextInputBase)val2).set_PlaceholderText("Search...");
			TextBox searchBox = val2;
			Panel val3 = new Panel();
			val3.set_Title("Event Categories");
			((Control)val3).set_Parent((Container)(object)Panel);
			val3.set_CanScroll(true);
			val3.set_ShowBorder(true);
			((Control)val3).set_Location(new Point(0, ((Control)searchBox).get_Bottom() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			Panel eventCategoriesPanel = val3;
			((Control)eventCategoriesPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, contentRegion.Height - ((Control)eventCategoriesPanel).get_Location().Y));
			Menu val4 = new Menu();
			((Control)val4).set_Parent((Container)(object)eventCategoriesPanel);
			Rectangle contentRegion2 = ((Container)eventCategoriesPanel).get_ContentRegion();
			((Control)val4).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
			val4.set_MenuItemHeight(40);
			Menu eventCategoryMenu = val4;
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val5).set_CanScroll(true);
			((Panel)val5).set_ShowBorder(true);
			((Control)val5).set_Location(new Point(((Control)eventCategoriesPanel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, contentRegion.Y));
			((Control)val5).set_Parent((Container)(object)Panel);
			FlowPanel eventPanel = val5;
			((Control)eventPanel).set_Size(new Point(contentRegion.Width - ((Control)eventPanel).get_Location().X - MAIN_PADDING.X, contentRegion.Height - 32));
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				eventPanel.FilterChildren<EventDetailsButton>((Func<EventDetailsButton, bool>)((EventDetailsButton detailsButton) => ((DetailsButton)detailsButton).get_Text().ToLowerInvariant().Contains(((TextInputBase)searchBox).get_Text().ToLowerInvariant())));
			});
			Dictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
			MenuItem allEvents = eventCategoryMenu.AddMenuItem("All Events", (Texture2D)null);
			allEvents.Select();
			menus.Add("allEvents", allEvents);
			MenuItem enabledEvents = eventCategoryMenu.AddMenuItem("Enabled Events", (Texture2D)null);
			menus.Add("enabledEvents", enabledEvents);
			MenuItem divider1MenuItem = eventCategoryMenu.AddMenuItem("-------------------------------------", (Texture2D)null);
			((Control)divider1MenuItem).set_Enabled(false);
			menus.Add("divider1MenuItem", divider1MenuItem);
			IEnumerable<EventCategory> categoryList = from ec in eventCategories
				group ec by ec.Key into ec
				select ec.First();
			switch (_moduleSettings.MenuEventSortMode.get_Value())
			{
			case MenuEventSortMode.Alphabetical:
				categoryList = categoryList.OrderBy((EventCategory c) => c.Name);
				break;
			case MenuEventSortMode.AlphabeticalDesc:
				categoryList = categoryList.OrderByDescending((EventCategory c) => c.Name);
				break;
			}
			foreach (EventCategory category2 in categoryList)
			{
				menus.Add(category2.Key, eventCategoryMenu.AddMenuItem(category2.Name, (Texture2D)null));
			}
			EventCategory category3;
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					try
					{
						MenuItem menuItem = (MenuItem)((s is MenuItem) ? s : null);
						if (menuItem != null)
						{
							category3 = eventCategories.Where((EventCategory ec) => ec.Name == menuItem.get_Text()).FirstOrDefault();
							eventPanel.FilterChildren<EventDetailsButton>((Func<EventDetailsButton, bool>)delegate(EventDetailsButton detailsButton)
							{
								if (menuItem == menus["allEvents"])
								{
									return true;
								}
								return (menuItem == menus["enabledEvents"]) ? (!_getDisabledEventKeys().Contains(detailsButton.Event.SettingKey)) : category3.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.SettingKey.Split('_')[0] == detailsButton.Event.SettingKey.Split('_')[0] && ev.Key == detailsButton.Event.Key);
							});
						}
					}
					catch (Exception ex)
					{
						ShowError("Failed to filter events:\n" + ex.Message);
					}
				});
			});
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)Panel);
			((Control)val6).set_Location(new Point(((Control)eventPanel).get_Left(), ((Control)eventPanel).get_Bottom()));
			((Control)val6).set_Size(new Point(((Control)eventPanel).get_Width(), 26));
			Panel buttons = val6;
			StandardButton val7 = new StandardButton();
			val7.set_Text("Check all");
			((Control)val7).set_Parent((Container)(object)buttons);
			((Control)val7).set_Right(((Control)buttons).get_Width());
			((Control)val7).set_Bottom(((Control)buttons).get_Height());
			StandardButton checkAllButton = val7;
			((Control)checkAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					EventDetailsButton eventDetailsButton2 = control as EventDetailsButton;
					if (eventDetailsButton2 != null && ((Control)eventDetailsButton2).get_Visible())
					{
						Control obj3 = ((IEnumerable<Control>)((Container)eventDetailsButton2).get_Children()).Last();
						GlowButton val15 = (GlowButton)(object)((obj3 is GlowButton) ? obj3 : null);
						if (val15 != null)
						{
							val15.set_Checked(true);
						}
					}
				});
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text("Uncheck all");
			((Control)val8).set_Parent((Container)(object)buttons);
			((Control)val8).set_Right(((Control)checkAllButton).get_Left());
			((Control)val8).set_Bottom(((Control)buttons).get_Height());
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					EventDetailsButton eventDetailsButton = control as EventDetailsButton;
					if (eventDetailsButton != null && ((Control)eventDetailsButton).get_Visible())
					{
						Control obj2 = ((IEnumerable<Control>)((Container)eventDetailsButton).get_Children()).Last();
						GlowButton val14 = (GlowButton)(object)((obj2 is GlowButton) ? obj2 : null);
						if (val14 != null)
						{
							val14.set_Checked(false);
						}
					}
				});
			});
			foreach (EventCategory category in eventCategories)
			{
				IEnumerable<Estreya.BlishHUD.EventTable.Models.Event> enumerable;
				if (!category.ShowCombined)
				{
					IEnumerable<Estreya.BlishHUD.EventTable.Models.Event> events = category.Events;
					enumerable = events;
				}
				else
				{
					enumerable = from e in category.Events
						group e by e.Key into eg
						select eg.First();
				}
				foreach (Estreya.BlishHUD.EventTable.Models.Event e2 in enumerable)
				{
					if (e2.Filler)
					{
						continue;
					}
					bool enabled = !_getDisabledEventKeys().Contains(e2.SettingKey);
					EventDetailsButton obj = new EventDetailsButton
					{
						Event = e2
					};
					((Control)obj).set_Parent((Container)(object)eventPanel);
					((DetailsButton)obj).set_Text(e2.Name);
					((DetailsButton)obj).set_Icon(base.IconService.GetIcon(e2.Icon));
					((DetailsButton)obj).set_ShowToggleButton(true);
					((DetailsButton)obj).set_FillColor(Color.get_LightBlue());
					EventDetailsButton button = obj;
					if (!string.IsNullOrWhiteSpace(e2.Waypoint))
					{
						AsyncTexture2D icon2 = base.IconService.GetIcon("102348.png");
						GlowButton val9 = new GlowButton();
						((Control)val9).set_Parent((Container)(object)button);
						val9.set_ToggleGlow(false);
						((Control)val9).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Waypoint", "Click to copy waypoint!", icon2, base.TranslationService)));
						val9.set_Icon(icon2);
						((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							ClipboardUtil.get_WindowsClipboardService().SetTextAsync(e2.Waypoint).ContinueWith(delegate(Task<bool> clipboardTask)
							{
								//IL_0013: Unknown result type (might be due to invalid IL or missing references)
								//IL_0033: Unknown result type (might be due to invalid IL or missing references)
								string message = "Copied!";
								NotificationType type = (NotificationType)0;
								if (clipboardTask.IsFaulted)
								{
									message = clipboardTask.Exception.Message;
									type = (NotificationType)2;
								}
								GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
								{
									//IL_0007: Unknown result type (might be due to invalid IL or missing references)
									ScreenNotification.ShowNotification(message, type, (Texture2D)null, 4);
								});
							});
						});
					}
					if (!string.IsNullOrWhiteSpace(e2.Wiki))
					{
						AsyncTexture2D icon = base.IconService.GetIcon("102353.png");
						GlowButton val10 = new GlowButton();
						((Control)val10).set_Parent((Container)(object)button);
						val10.set_ToggleGlow(false);
						((Control)val10).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Wiki", "Click to open wiki!", icon, base.TranslationService)));
						val10.set_Icon(icon);
						((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							Process.Start(e2.Wiki);
						});
					}
					if (_additionalData.ContainsKey("hiddenEventKeys"))
					{
						List<string> hiddenEventKeys = _additionalData["hiddenEventKeys"] as List<string>;
						if (hiddenEventKeys != null && hiddenEventKeys.Contains(e2.SettingKey))
						{
							GlowButton val11 = new GlowButton();
							((Control)val11).set_Parent((Container)(object)button);
							val11.set_ToggleGlow(false);
							val11.set_Icon(base.IconService.GetIcon("155018.png"));
							((Control)val11).set_BasicTooltipText("This event is currently hidden due to dynamic states.");
							((Control)val11).set_Enabled(false);
						}
					}
					if (_additionalData.ContainsKey("customActions"))
					{
						List<CustomActionDefinition> customActions = _additionalData["customActions"] as List<CustomActionDefinition>;
						if (customActions != null)
						{
							foreach (CustomActionDefinition customAction in customActions)
							{
								if (!string.IsNullOrWhiteSpace(customAction.Name) && customAction.Action != null)
								{
									GlowButton val12 = new GlowButton();
									((Control)val12).set_Parent((Container)(object)button);
									val12.set_ToggleGlow(false);
									val12.set_Icon((customAction.Icon != null) ? base.IconService.GetIcon(customAction.Icon) : null);
									((Control)val12).set_BasicTooltipText(customAction.Tooltip);
									((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
									{
										customAction.Action?.Invoke(e2);
									});
								}
							}
						}
					}
					GlowButton val13 = new GlowButton();
					((Control)val13).set_Parent((Container)(object)button);
					val13.set_Checked(enabled);
					val13.set_ToggleGlow(false);
					GlowButton toggleButton = val13;
					UpdateToggleButton(toggleButton);
					toggleButton.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent eventArgs)
					{
						this.EventChanged?.Invoke(this, new EventChangedArgs
						{
							OldState = !eventArgs.get_Checked(),
							NewState = eventArgs.get_Checked(),
							EventSettingKey = button.Event.SettingKey,
							AdditionalData = _additionalData
						});
						toggleButton.set_Checked(eventArgs.get_Checked());
						UpdateToggleButton(toggleButton);
					});
					((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						toggleButton.set_Checked(!toggleButton.get_Checked());
					});
				}
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
