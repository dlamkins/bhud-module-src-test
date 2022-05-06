using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageEventsView : View
	{
		private static Point MAIN_PADDING = new Point(20, 20);

		private static readonly Logger Logger = Logger.GetLogger<ManageEventsView>();

		public Panel Panel { get; private set; }

		private void UpdateToggleButton(GlowButton button)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				button.set_Icon(AsyncTexture2D.op_Implicit(button.get_Checked() ? EventTableModule.ModuleInstance.IconState.GetIcon("images\\minus.png") : EventTableModule.ModuleInstance.IconState.GetIcon("images\\plus.png")));
			});
		}

		protected override void Build(Container buildPanel)
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
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Expected O, but got Unknown
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Expected O, but got Unknown
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Expected O, but got Unknown
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Expected O, but got Unknown
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0661: Unknown result type (might be due to invalid IL or missing references)
			//IL_066d: Expected O, but got Unknown
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ca: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width - MAIN_PADDING.X);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height - MAIN_PADDING.Y);
			val.set_CanScroll(true);
			Panel = val;
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)Panel);
			((Control)val2).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val2).set_Location(new Point(0, contentRegion.Y));
			((TextInputBase)val2).set_PlaceholderText(Strings.ManageEventsView_SearchBox_Placeholder);
			TextBox searchBox = val2;
			Panel val3 = new Panel();
			val3.set_Title(Strings.ManageEventsView_EventCategories_Title);
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
			Menu eventCategories = val4;
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val5).set_CanScroll(true);
			((Panel)val5).set_ShowBorder(true);
			((Control)val5).set_Location(new Point(((Control)eventCategoriesPanel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, contentRegion.Y));
			((Control)val5).set_Parent((Container)(object)Panel);
			FlowPanel eventPanel = val5;
			((Control)eventPanel).set_Size(new Point(contentRegion.Width - ((Control)eventPanel).get_Left() - MAIN_PADDING.X, contentRegion.Height - 32));
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				eventPanel.FilterChildren<EventDetailsButton>((Func<EventDetailsButton, bool>)((EventDetailsButton detailsButton) => ((DetailsButton)detailsButton).get_Text().ToLowerInvariant().Contains(((TextInputBase)searchBox).get_Text().ToLowerInvariant())));
			});
			Dictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
			MenuItem allEvents = eventCategories.AddMenuItem(Strings.ManageEventsView_AllEvents, (Texture2D)null);
			allEvents.Select();
			menus.Add("allEvents", allEvents);
			foreach (EventCategory category2 in from ec in EventTableModule.ModuleInstance.EventCategories
				group ec by ec.Key into ec
				select ec.First())
			{
				menus.Add(category2.Key, eventCategories.AddMenuItem(category2.Name, (Texture2D)null));
			}
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					MenuItem menuItem = (MenuItem)((s is MenuItem) ? s : null);
					EventCategory category3 = EventTableModule.ModuleInstance.EventCategories.Where((EventCategory ec) => ec.Name == menuItem.get_Text()).FirstOrDefault();
					eventPanel.FilterChildren<EventDetailsButton>((Func<EventDetailsButton, bool>)((EventDetailsButton detailsButton) => menuItem == menus["allEvents"] || category3.Events.Any((Event ev) => ev.EventCategory.Key == detailsButton.Event.EventCategory.Key && ev.Key == detailsButton.Event.Key)));
				});
			});
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)Panel);
			((Control)val6).set_Location(new Point(((Control)eventPanel).get_Left(), ((Control)eventPanel).get_Bottom()));
			((Control)val6).set_Size(new Point(((Control)eventPanel).get_Width(), 26));
			Panel buttons = val6;
			StandardButton val7 = new StandardButton();
			val7.set_Text(Strings.ManageEventsView_CheckAll);
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
					if (((Control)eventDetailsButton2).get_Visible())
					{
						Control obj3 = ((IEnumerable<Control>)((Container)eventDetailsButton2).get_Children()).Last();
						((GlowButton)((obj3 is GlowButton) ? obj3 : null)).set_Checked(true);
					}
				});
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text(Strings.ManageEventsView_UncheckAll);
			((Control)val8).set_Parent((Container)(object)buttons);
			((Control)val8).set_Right(((Control)checkAllButton).get_Left());
			((Control)val8).set_Bottom(((Control)buttons).get_Height());
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					EventDetailsButton eventDetailsButton = control as EventDetailsButton;
					if (((Control)eventDetailsButton).get_Visible())
					{
						Control obj2 = ((IEnumerable<Control>)((Container)eventDetailsButton).get_Children()).Last();
						((GlowButton)((obj2 is GlowButton) ? obj2 : null)).set_Checked(false);
					}
				});
			});
			foreach (EventCategory category in EventTableModule.ModuleInstance.EventCategories)
			{
				IEnumerable<Event> enumerable;
				if (!category.ShowCombined)
				{
					IEnumerable<Event> events = category.Events;
					enumerable = events;
				}
				else
				{
					enumerable = from e in category.Events
						group e by e.Key into eg
						select eg.First();
				}
				foreach (Event e2 in enumerable)
				{
					if (e2.Filler)
					{
						continue;
					}
					IEnumerable<SettingEntry<bool>> settings = EventTableModule.ModuleInstance.ModuleSettings.AllEvents.FindAll((SettingEntry<bool> eventSetting) => ((SettingEntry)eventSetting).get_EntryKey().ToLowerInvariant() == e2.SettingKey.ToLowerInvariant());
					SettingEntry<bool> setting = settings.First();
					bool enabled = setting.get_Value();
					EventDetailsButton obj = new EventDetailsButton
					{
						Event = e2
					};
					((Control)obj).set_Parent((Container)(object)eventPanel);
					((DetailsButton)obj).set_Text(e2.Name);
					((DetailsButton)obj).set_ShowToggleButton(true);
					((DetailsButton)obj).set_FillColor(Color.get_LightBlue());
					EventDetailsButton button = obj;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						((DetailsButton)button).set_Icon(AsyncTexture2D.op_Implicit(EventTableModule.ModuleInstance.IconState.GetIcon(e2.Icon)));
					});
					if (!string.IsNullOrWhiteSpace(e2.Waypoint))
					{
						GlowButton val9 = new GlowButton();
						((Control)val9).set_Parent((Container)(object)button);
						val9.set_ToggleGlow(false);
						GlowButton waypointButton = val9;
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							//IL_001a: Unknown result type (might be due to invalid IL or missing references)
							//IL_0024: Expected O, but got Unknown
							((Control)waypointButton).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView(Strings.ManageEventsView_Waypoint_Title, Strings.ManageEventsView_Waypoint_Description, "images\\waypoint.png")));
							waypointButton.set_Icon(AsyncTexture2D.op_Implicit(EventTableModule.ModuleInstance.IconState.GetIcon("images\\waypoint.png")));
						});
						((Control)waypointButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.CopyWaypoint();
						});
					}
					if (!string.IsNullOrWhiteSpace(e2.Wiki))
					{
						GlowButton val10 = new GlowButton();
						((Control)val10).set_Parent((Container)(object)button);
						val10.set_ToggleGlow(false);
						GlowButton wikiButton = val10;
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							//IL_001a: Unknown result type (might be due to invalid IL or missing references)
							//IL_0024: Expected O, but got Unknown
							((Control)wikiButton).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView(Strings.ManageEventsView_Wiki_Title, Strings.ManageEventsView_Wiki_Description, "images\\wiki.png")));
							wikiButton.set_Icon(AsyncTexture2D.op_Implicit(EventTableModule.ModuleInstance.IconState.GetIcon("images\\wiki.png")));
						});
						((Control)wikiButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.OpenWiki();
						});
					}
					GlowButton val11 = new GlowButton();
					((Control)val11).set_Parent((Container)(object)button);
					val11.set_ToggleGlow(false);
					GlowButton editButton = val11;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0024: Expected O, but got Unknown
						((Control)editButton).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Edit", "Edit Description", "156684")));
						editButton.set_Icon(AsyncTexture2D.op_Implicit(EventTableModule.ModuleInstance.IconState.GetIcon("156684", checkRenderAPI: false)));
					});
					((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						e2.Edit();
					});
					GlowButton val12 = new GlowButton();
					((Control)val12).set_Parent((Container)(object)button);
					val12.set_Checked(enabled);
					val12.set_ToggleGlow(false);
					GlowButton toggleButton = val12;
					UpdateToggleButton(toggleButton);
					toggleButton.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent eventArgs)
					{
						if (setting != null)
						{
							setting.set_Value(eventArgs.get_Checked());
							toggleButton.set_Checked(setting.get_Value());
							UpdateToggleButton(toggleButton);
						}
					});
					((Control)toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						toggleButton.set_Checked(!toggleButton.get_Checked());
					});
				}
			}
		}

		public ManageEventsView()
			: this()
		{
		}
	}
}
