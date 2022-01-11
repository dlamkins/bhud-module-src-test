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
		private static readonly Logger Logger = Logger.GetLogger<ManageEventsView>();

		public FlowPanel FlowPanel { get; private set; }

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
			button.set_Icon(button.get_Checked() ? EventTableModule.ModuleInstance.ContentsManager.GetRenderIcon("images\\minus.png") : EventTableModule.ModuleInstance.ContentsManager.GetRenderIcon("images\\plus.png"));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Expected O, but got Unknown
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Expected O, but got Unknown
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Expected O, but got Unknown
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053a: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Expected O, but got Unknown
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Expected O, but got Unknown
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)1);
			((Control)val).set_Top(0);
			((Panel)val).set_CanScroll(true);
			val.set_OuterControlPadding(new Vector2((float)((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, (float)((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			((Control)val).set_Parent(buildPanel);
			FlowPanel = val;
			Rectangle contentRegion = ((Container)FlowPanel).get_ContentRegion();
			Panel eventCategoriesPanel = new Panel();
			eventCategoriesPanel.set_Title("Event Categories");
			((Control)eventCategoriesPanel).set_Parent((Container)(object)FlowPanel);
			eventCategoriesPanel.set_CanScroll(true);
			eventCategoriesPanel.set_ShowBorder(true);
			((Control)eventCategoriesPanel).set_Size(((DesignStandard)(ref Panel.MenuStandard)).get_Size() - new Point(0, ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			((Control)eventCategoriesPanel).set_Location(new Point(0, contentRegion.Y));
			Menu eventCategories = new Menu();
			((Control)eventCategories).set_Parent((Container)(object)eventCategoriesPanel);
			Rectangle contentRegion2 = ((Container)eventCategoriesPanel).get_ContentRegion();
			((Control)eventCategories).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
			eventCategories.set_MenuItemHeight(40);
			FlowPanel eventPanel = new FlowPanel();
			eventPanel.set_FlowDirection((ControlFlowDirection)0);
			((Panel)eventPanel).set_CanScroll(true);
			((Panel)eventPanel).set_ShowBorder(true);
			((Control)eventPanel).set_Parent((Container)(object)FlowPanel);
			((Control)eventPanel).set_Location(new Point(0, contentRegion.Y));
			((Control)eventPanel).set_Size(new Point(contentRegion.Width - (((Control)eventCategoriesPanel).get_Location().X + ((Control)eventCategoriesPanel).get_Width()) - (int)FlowPanel.get_OuterControlPadding().X, contentRegion.Height - (int)FlowPanel.get_OuterControlPadding().Y - 32));
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
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)FlowPanel);
			((Control)val2).set_Size(new Point(contentRegion.Width - (((Control)eventCategoriesPanel).get_Location().X + ((Control)eventCategoriesPanel).get_Width()) - (int)FlowPanel.get_OuterControlPadding().X, 26));
			Panel buttons = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Check all");
			((Control)val3).set_Parent((Container)(object)buttons);
			((Control)val3).set_Right(((Control)buttons).get_Width());
			((Control)val3).set_Bottom(((Control)buttons).get_Height());
			StandardButton checkAllButton = val3;
			((Control)checkAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					DetailsButton val10 = (DetailsButton)(object)((control is DetailsButton) ? control : null);
					if (((Control)val10).get_Visible())
					{
						Control obj2 = ((IEnumerable<Control>)((Container)val10).get_Children()).Last();
						((GlowButton)((obj2 is GlowButton) ? obj2 : null)).set_Checked(true);
					}
				});
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("Uncheck all");
			((Control)val4).set_Parent((Container)(object)buttons);
			((Control)val4).set_Right(((Control)checkAllButton).get_Left());
			((Control)val4).set_Bottom(((Control)buttons).get_Height());
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Container)eventPanel).get_Children().ToList().ForEach(delegate(Control control)
				{
					menus["allEvents"].get_Selected();
					DetailsButton val9 = (DetailsButton)(object)((control is DetailsButton) ? control : null);
					if (((Control)val9).get_Visible())
					{
						Control obj = ((IEnumerable<Control>)((Container)val9).get_Children()).Last();
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
					AsyncTexture2D icon = EventTableModule.ModuleInstance.ContentsManager.GetRenderIcon(e2.Icon);
					DetailsButton val5 = new DetailsButton();
					((Control)val5).set_Parent((Container)(object)eventPanel);
					val5.set_Text(e2.Name);
					val5.set_Icon(icon);
					val5.set_ShowToggleButton(true);
					val5.set_FillColor(Color.get_LightBlue());
					DetailsButton button = val5;
					if (!string.IsNullOrWhiteSpace(e2.Waypoint))
					{
						GlowButton val6 = new GlowButton();
						((Control)val6).set_Parent((Container)(object)button);
						val6.set_ToggleGlow(false);
						((Control)val6).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Waypoint", "Click to Copy", "images\\waypoint.png")));
						val6.set_Icon(EventTableModule.ModuleInstance.ContentsManager.GetRenderIcon("images\\waypoint.png"));
						((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.CopyWaypoint();
						});
					}
					if (!string.IsNullOrWhiteSpace(e2.Wiki))
					{
						GlowButton val7 = new GlowButton();
						((Control)val7).set_Parent((Container)(object)button);
						val7.set_ToggleGlow(false);
						((Control)val7).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Wiki", "Click to Open", "images\\wiki.png")));
						val7.set_Icon(EventTableModule.ModuleInstance.ContentsManager.GetRenderIcon("images\\wiki.png"));
						((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							e2.OpenWiki();
						});
					}
					GlowButton val8 = new GlowButton();
					((Control)val8).set_Parent((Container)(object)button);
					val8.set_Checked(enabled);
					val8.set_ToggleGlow(false);
					GlowButton toggleButton = val8;
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
