using System;
using System.Collections.Generic;
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
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageEventsView : BaseView
	{
		private static Point MAIN_PADDING = new Point(20, 20);

		private static readonly Logger Logger = Logger.GetLogger<ManageEventsView>();

		private readonly List<EventCategory> allEvents;

		private readonly Dictionary<string, object> _additionalData;

		private readonly Func<List<string>> _getDisabledEventKeys;

		public Panel Panel { get; private set; }

		public event EventHandler<EventChangedArgs> EventChanged;

		public ManageEventsView(List<EventCategory> allEvents, Dictionary<string, object> additionalData, Func<List<string>> getDisabledEventKeys, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, font)
		{
			this.allEvents = allEvents;
			_additionalData = additionalData ?? new Dictionary<string, object>();
			_getDisabledEventKeys = getDisabledEventKeys;
		}

		private void UpdateToggleButton(GlowButton button)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				button.set_Icon(button.get_Checked() ? base.IconState.GetIcon("784259.png") : base.IconState.GetIcon("784261.png"));
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
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Expected O, but got Unknown
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Expected O, but got Unknown
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Expected O, but got Unknown
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Expected O, but got Unknown
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0682: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Expected O, but got Unknown
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
			foreach (EventCategory category2 in from ec in eventCategories
				group ec by ec.Key into ec
				select ec.First())
			{
				menus.Add(category2.Key, eventCategoryMenu.AddMenuItem(category2.Name, (Texture2D)null));
			}
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					MenuItem menuItem = (MenuItem)((s is MenuItem) ? s : null);
					if (menuItem != null)
					{
						EventCategory category3 = eventCategories.Where((EventCategory ec) => ec.Name == menuItem.get_Text()).FirstOrDefault();
						eventPanel.FilterChildren<EventDetailsButton>((Func<EventDetailsButton, bool>)((EventDetailsButton detailsButton) => menuItem == menus["allEvents"] || category3.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.SettingKey.Split('_')[0] == detailsButton.Event.SettingKey.Split('_')[0] && ev.Key == detailsButton.Event.Key)));
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
						GlowButton val14 = (GlowButton)(object)((obj3 is GlowButton) ? obj3 : null);
						if (val14 != null)
						{
							val14.set_Checked(true);
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
						GlowButton val13 = (GlowButton)(object)((obj2 is GlowButton) ? obj2 : null);
						if (val13 != null)
						{
							val13.set_Checked(false);
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
					((DetailsButton)obj).set_ShowToggleButton(true);
					((DetailsButton)obj).set_FillColor(Color.get_LightBlue());
					EventDetailsButton button = obj;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						((DetailsButton)button).set_Icon(base.IconState.GetIcon(e2.Icon));
					});
					if (!string.IsNullOrWhiteSpace(e2.Waypoint))
					{
						GlowButton val9 = new GlowButton();
						((Control)val9).set_Parent((Container)(object)button);
						val9.set_ToggleGlow(false);
						GlowButton waypointButton = val9;
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							//IL_004d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0057: Expected O, but got Unknown
							AsyncTexture2D icon2 = base.IconState.GetIcon("102348.png");
							((Control)waypointButton).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Waypoint", "Click to copy waypoint!", icon2, base.TranslationState)));
							waypointButton.set_Icon(icon2);
						});
						((Control)waypointButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
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
							//IL_004d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0057: Expected O, but got Unknown
							AsyncTexture2D icon = base.IconState.GetIcon("102353.png");
							((Control)wikiButton).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Wiki", "Click to open wiki!", icon, base.TranslationState)));
							wikiButton.set_Icon(icon);
						});
						((Control)wikiButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
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
							val11.set_Icon(base.IconState.GetIcon("155018.png"));
							((Control)val11).set_BasicTooltipText("This event is currently hidden due to dynamic states.");
							((Control)val11).set_Enabled(false);
						}
					}
					GlowButton val12 = new GlowButton();
					((Control)val12).set_Parent((Container)(object)button);
					val12.set_Checked(enabled);
					val12.set_ToggleGlow(false);
					GlowButton toggleButton = val12;
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
