using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageDynamicEventsSettingsView : BaseView
	{
		private static readonly Logger Logger = Logger.GetLogger<ManageDynamicEventsSettingsView>();

		private static readonly Point MAIN_PADDING = new Point(20, 20);

		private readonly DynamicEventService _dynamicEventService;

		private readonly Func<List<string>> _getDisabledEventGuids;

		private readonly ModuleSettings _moduleSettings;

		private readonly List<Map> _maps = new List<Map>();

		private StandardWindow _editEventWindow;

		public Panel Panel { get; private set; }

		public event EventHandler<ManageEventsView.EventChangedArgs> EventChanged;

		public ManageDynamicEventsSettingsView(DynamicEventService dynamicEventService, Func<List<string>> getDisabledEventGuids, ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_dynamicEventService = dynamicEventService;
			_getDisabledEventGuids = getDisabledEventGuids;
			_moduleSettings = moduleSettings;
			_dynamicEventService.CustomEventsUpdated += DynamicEventService_CustomEventsUpdated;
		}

		private Task DynamicEventService_CustomEventsUpdated(object sender)
		{
			((Container)base.MainPanel).ClearChildren();
			InternalBuild(base.MainPanel);
			return Task.CompletedTask;
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
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Expected O, but got Unknown
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Expected O, but got Unknown
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05af: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - MAIN_PADDING.X);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - MAIN_PADDING.Y);
			val.set_CanScroll(true);
			Panel = val;
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			IEnumerable<Map> maps = _maps.Where((Map m) => _dynamicEventService.Events?.Any((DynamicEvent de) => de.MapId == m.get_Id()) ?? false);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)Panel);
			((Control)val2).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val2).set_Location(new Point(0, contentRegion.Y));
			((TextInputBase)val2).set_PlaceholderText("Search...");
			TextBox searchBox = val2;
			Panel val3 = new Panel();
			val3.set_Title("Maps");
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
				eventPanel.FilterChildren<DataDetailsButton<DynamicEvent>>((Func<DataDetailsButton<DynamicEvent>, bool>)((DataDetailsButton<DynamicEvent> detailsButton) => ((DetailsButton)detailsButton).get_Text().ToLowerInvariant().Contains(((TextInputBase)searchBox).get_Text().ToLowerInvariant())));
			});
			Dictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();
			MenuItem allEvents = val4.AddMenuItem("Current Map", (Texture2D)null);
			allEvents.Select();
			menus.Add("allEvents", allEvents);
			menus.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItemPair)
			{
				((Control)menuItemPair.Value).add_Click((EventHandler<MouseEventArgs>)delegate(object s, MouseEventArgs e)
				{
					MenuItem menuItem = (MenuItem)((s is MenuItem) ? s : null);
					if (menuItem != null)
					{
						Map map3 = maps.Where((Map map) => map.get_Name() == menuItem.get_Text()).FirstOrDefault();
						eventPanel.FilterChildren<DataDetailsButton<DynamicEvent>>((Func<DataDetailsButton<DynamicEvent>, bool>)((DataDetailsButton<DynamicEvent> detailsButton) => menuItem == menus["allEvents"] || detailsButton.Data.MapId == map3.get_Id()));
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
					DataDetailsButton<DynamicEvent> dataDetailsButton2 = control as DataDetailsButton<DynamicEvent>;
					if (dataDetailsButton2 != null && ((Control)dataDetailsButton2).get_Visible())
					{
						Control obj4 = ((IEnumerable<Control>)((Container)dataDetailsButton2).get_Children()).Last();
						GlowButton val13 = (GlowButton)(object)((obj4 is GlowButton) ? obj4 : null);
						if (val13 != null)
						{
							val13.set_Checked(true);
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
					DataDetailsButton<DynamicEvent> dataDetailsButton = control as DataDetailsButton<DynamicEvent>;
					if (dataDetailsButton != null && ((Control)dataDetailsButton).get_Visible())
					{
						Control obj3 = ((IEnumerable<Control>)((Container)dataDetailsButton).get_Children()).Last();
						GlowButton val12 = (GlowButton)(object)((obj3 is GlowButton) ? obj3 : null);
						if (val12 != null)
						{
							val12.set_Checked(false);
						}
					}
				});
			});
			List<DynamicEvent> eventList = _dynamicEventService.Events.ToList();
			foreach (Map map2 in maps.Where((Map m) => m.get_Id() == GameService.Gw2Mumble.get_CurrentMap().get_Id()))
			{
				foreach (DynamicEvent e2 in from e in eventList
					where e.MapId == map2.get_Id()
					orderby e.IsCustom descending
					select e)
				{
					bool enabled = !_getDisabledEventGuids().Contains(e2.ID);
					DataDetailsButton<DynamicEvent> obj = new DataDetailsButton<DynamicEvent>
					{
						Data = e2
					};
					((Control)obj).set_Parent((Container)(object)eventPanel);
					((DetailsButton)obj).set_Text(e2.Name);
					((DetailsButton)obj).set_ShowToggleButton(true);
					((DetailsButton)obj).set_FillColor(Color.get_LightBlue());
					DataDetailsButton<DynamicEvent> button = obj;
					DynamicEvent.DynamicEventIcon icon = e2.Icon;
					int num;
					if (icon == null)
					{
						num = 0;
					}
					else
					{
						_ = icon.FileID;
						num = 1;
					}
					if (num != 0)
					{
						GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							((DetailsButton)button).set_Icon(base.IconService.GetIcon($"{e2.Icon.FileID}.png"));
						});
					}
					GlowButton val9 = new GlowButton();
					((Control)val9).set_Parent((Container)(object)button);
					val9.set_ToggleGlow(false);
					val9.set_Icon(base.IconService.GetIcon("156706.png"));
					((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						if (_editEventWindow == null)
						{
							_editEventWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Edit Dynamic Event", ((object)this).GetType(), Guid.Parse("5e20b5b0-d0a8-4e36-b65b-3bfc9c971d3d"), base.IconService);
						}
						if (_editEventWindow.CurrentView != null)
						{
							EditDynamicEventView obj2 = _editEventWindow.CurrentView as EditDynamicEventView;
							obj2.SaveClicked -= EditEventView_SaveClicked;
							obj2.RemoveClicked -= EditEventView_RemoveClicked;
							obj2.CloseRequested -= EditEventView_CloseRequested;
						}
						EditDynamicEventView editDynamicEventView = new EditDynamicEventView(e2.CopyWithJson(new JsonSerializerSettings()), base.APIManager, base.IconService, base.TranslationService);
						editDynamicEventView.SaveClicked += EditEventView_SaveClicked;
						editDynamicEventView.RemoveClicked += EditEventView_RemoveClicked;
						editDynamicEventView.CloseRequested += EditEventView_CloseRequested;
						_editEventWindow.Show((IView)(object)editDynamicEventView);
					});
					if (e2.IsCustom)
					{
						GlowButton val10 = new GlowButton();
						((Control)val10).set_Parent((Container)(object)button);
						val10.set_ToggleGlow(false);
						val10.set_Icon(base.IconService.GetIcon("440023.png"));
						((Control)val10).set_BasicTooltipText("This event is customized!");
					}
					GlowButton val11 = new GlowButton();
					((Control)val11).set_Parent((Container)(object)button);
					val11.set_Checked(enabled);
					val11.set_ToggleGlow(false);
					GlowButton toggleButton = val11;
					UpdateToggleButton(toggleButton);
					toggleButton.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent eventArgs)
					{
						this.EventChanged?.Invoke(this, new ManageEventsView.EventChangedArgs
						{
							OldState = !eventArgs.get_Checked(),
							NewState = eventArgs.get_Checked(),
							EventSettingKey = button.Data.ID
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

		private void EditEventView_CloseRequested(object sender, EventArgs e)
		{
			((Control)_editEventWindow).Hide();
		}

		private async Task EditEventView_RemoveClicked(object sender, DynamicEvent e)
		{
			await _dynamicEventService.RemoveCustomEvent(e.ID);
			await _dynamicEventService.NotifyCustomEventsUpdated();
		}

		private async Task EditEventView_SaveClicked(object sender, DynamicEvent e)
		{
			await _dynamicEventService.AddCustomEvent(e);
			await _dynamicEventService.NotifyCustomEventsUpdated();
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			try
			{
				IApiV2ObjectList<Map> maps = await ((IAllExpandableClient<Map>)(object)base.APIManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken));
				_maps.AddRange((IEnumerable<Map>)maps);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to add maps:");
				return false;
			}
		}

		protected override void Unload()
		{
			base.Unload();
			_dynamicEventService.CustomEventsUpdated -= DynamicEventService_CustomEventsUpdated;
			StandardWindow editEventWindow = _editEventWindow;
			if (editEventWindow != null)
			{
				((Control)editEventWindow).Dispose();
			}
			_editEventWindow = null;
		}
	}
}