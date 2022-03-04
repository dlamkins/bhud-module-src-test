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
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageEventsView : View
	{
		private static Point MAIN_PADDING = new Point(20, 20);

		private static readonly Logger Logger = Logger.GetLogger<ManageEventsView>();

		public Panel Panel { get; private set; }

		private IEnumerable<EventCategory> EventCategories { get; set; }

		private List<SettingEntry<bool>> EventSettings { get; set; }

		public ManageEventsView(IEnumerable<EventCategory> categories, List<SettingEntry<bool>> settings)
			: this()
		{
			EventCategories = categories;
			EventSettings = settings;
		}

		private void UpdateToggleButton(GlowButton button)
		{
			button.set_Icon(button.get_Checked() ? EventTableModule.ModuleInstance.ContentsManager.GetIcon("images\\minus.png") : EventTableModule.ModuleInstance.ContentsManager.GetIcon("images\\plus.png"));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Expected O, but got Unknown
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Expected O, but got Unknown
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Expected O, but got Unknown
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Expected O, but got Unknown
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Expected O, but got Unknown
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0633: Expected O, but got Unknown
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_067c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Expected O, but got Unknown
			Panel = new Panel();
			((Control)Panel).set_Parent(buildPanel);
			((Control)Panel).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)Panel).set_Width(buildPanel.get_ContentRegion().Width - MAIN_PADDING.Y * 2);
			((Control)Panel).set_Height(buildPanel.get_ContentRegion().Height - MAIN_PADDING.X);
			Panel.set_CanScroll(true);
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)Panel);
			((Control)val).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val).set_Location(new Point(0, contentRegion.Y));
			((TextInputBase)val).set_PlaceholderText("Search");
			TextBox searchBox = val;
			Panel val2 = new Panel();
			val2.set_Title("Event Categories");
			((Control)val2).set_Parent((Container)(object)Panel);
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			((Control)val2).set_Location(new Point(0, ((Control)searchBox).get_Bottom() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			Panel eventCategoriesPanel = val2;
			((Control)eventCategoriesPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X, contentRegion.Height - ((Control)eventCategoriesPanel).get_Location().Y));
			Menu val3 = new Menu();
			((Control)val3).set_Parent((Container)(object)eventCategoriesPanel);
			Rectangle contentRegion2 = ((Container)eventCategoriesPanel).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
			val3.set_MenuItemHeight(40);
			Menu eventCategories = val3;
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Panel)val4).set_CanScroll(true);
			((Panel)val4).set_ShowBorder(true);
			((Control)val4).set_Parent((Container)(object)Panel);
			((Control)val4).set_Location(new Point(((Control)eventCategoriesPanel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, contentRegion.Y));
			FlowPanel eventPanel = val4;
			((Control)eventPanel).set_Size(new Point(contentRegion.Width - ((Control)eventPanel).get_Left(), contentRegion.Height - 32));
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)((DetailsButton detailsButton) => detailsButton.get_Text().ToLowerInvariant().Contains(((TextInputBase)searchBox).get_Text().ToLowerInvariant())));
			});
			Dictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
			MenuItem allEvents = eventCategories.AddMenuItem("All Events", (Texture2D)null);
			allEvents.Select();
			menus.Add("allEvents", allEvents);
			foreach (EventCategory category2 in from ec in EventCategories
				group ec by ec.Name into ec
				select ec.First())
			{
				menus.Add(category2.Key, eventCategories.AddMenuItem(category2.Name, (Texture2D)null));
			}
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					MenuItem menuItem = (MenuItem)((s is MenuItem) ? s : null);
					eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)delegate(DetailsButton detailsButton)
					{
						IEnumerable<EventCategory> source = EventCategories.Where((EventCategory ec) => ec.Events.Any((Event ev) => ev.Name == detailsButton.get_Text()));
						return menuItem == menus["allEvents"] || source.Any((EventCategory ec) => ec.Name == menuItem.get_Text());
					});
				});
			});
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)Panel);
			((Control)val5).set_Location(new Point(((Control)eventPanel).get_Left(), ((Control)eventPanel).get_Bottom()));
			((Control)val5).set_Size(new Point(((Control)eventPanel).get_Width(), 26));
			Panel buttons = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Check all");
			((Control)val6).set_Parent((Container)(object)buttons);
			((Control)val6).set_Right(((Control)buttons).get_Width());
			((Control)val6).set_Bottom(((Control)buttons).get_Height());
			StandardButton checkAllButton = val6;
			((Control)checkAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					DetailsButton val13 = (DetailsButton)(object)((control is DetailsButton) ? control : null);
					if (((Control)val13).get_Visible())
					{
						Control obj2 = ((IEnumerable<Control>)((Container)val13).get_Children()).Last();
						((GlowButton)((obj2 is GlowButton) ? obj2 : null)).set_Checked(true);
					}
				});
			});
			StandardButton val7 = new StandardButton();
			val7.set_Text("Uncheck all");
			((Control)val7).set_Parent((Container)(object)buttons);
			((Control)val7).set_Right(((Control)checkAllButton).get_Left());
			((Control)val7).set_Bottom(((Control)buttons).get_Height());
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					DetailsButton val12 = (DetailsButton)(object)((control is DetailsButton) ? control : null);
					if (((Control)val12).get_Visible())
					{
						Control obj = ((IEnumerable<Control>)((Container)val12).get_Children()).Last();
						((GlowButton)((obj is GlowButton) ? obj : null)).set_Checked(false);
					}
				});
			});
			foreach (EventCategory category in EventCategories)
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
						group e by e.Name into eg
						select eg.First();
				}
				foreach (Event e2 in enumerable)
				{
					if (e2.Filler)
					{
						continue;
					}
					IEnumerable<SettingEntry<bool>> settings = EventSettings.FindAll((SettingEntry<bool> eventSetting) => ((SettingEntry)eventSetting).get_EntryKey() == e2.Name);
					SettingEntry<bool> setting = settings.First();
					bool enabled = setting.get_Value();
					AsyncTexture2D icon = EventTableModule.ModuleInstance.ContentsManager.GetIcon(e2.Icon);
					DetailsButton val8 = new DetailsButton();
					((Control)val8).set_Parent((Container)(object)eventPanel);
					val8.set_Text(e2.Name);
					val8.set_Icon(icon);
					val8.set_ShowToggleButton(true);
					val8.set_FillColor(Color.get_LightBlue());
					DetailsButton button = val8;
					if (!string.IsNullOrWhiteSpace(e2.Waypoint))
					{
						GlowButton val9 = new GlowButton();
						((Control)val9).set_Parent((Container)(object)button);
						val9.set_ToggleGlow(false);
						((Control)val9).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Waypoint", "Click to Copy", "images\\waypoint.png")));
						val9.set_Icon(EventTableModule.ModuleInstance.ContentsManager.GetIcon("images\\waypoint.png"));
						((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.CopyWaypoint();
						});
					}
					if (!string.IsNullOrWhiteSpace(e2.Wiki))
					{
						GlowButton val10 = new GlowButton();
						((Control)val10).set_Parent((Container)(object)button);
						val10.set_ToggleGlow(false);
						((Control)val10).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Wiki", "Click to Open", "images\\wiki.png")));
						val10.set_Icon(EventTableModule.ModuleInstance.ContentsManager.GetIcon("images\\wiki.png"));
						((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.OpenWiki();
						});
					}
					GlowButton val11 = new GlowButton();
					((Control)val11).set_Parent((Container)(object)button);
					val11.set_Checked(enabled);
					val11.set_ToggleGlow(false);
					GlowButton toggleButton = val11;
					UpdateToggleButton(toggleButton);
					toggleButton.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent eventArgs)
					{
						if (setting != null)
						{
							setting.set_Value(eventArgs.get_Checked());
							toggleButton.set_Checked(setting.get_Value());
							settings.Where((SettingEntry<bool> x) => ((SettingEntry)x).get_EntryKey() != ((SettingEntry)setting).get_EntryKey()).ToList().ForEach(delegate(SettingEntry<bool> x)
							{
								x.set_Value(setting.get_Value());
							});
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
	}
}
