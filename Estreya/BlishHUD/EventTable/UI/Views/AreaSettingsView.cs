using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class AreaSettingsView : BaseSettingsView
	{
		public class AddAreaEventArgs
		{
			public string Name { get; set; }

			public EventAreaConfiguration AreaConfiguration { get; set; }
		}

		private const int PADDING_X = 20;

		private const int PADDING_Y = 20;

		private readonly Func<IEnumerable<EventAreaConfiguration>> _areaConfigurationFunc;

		private readonly Func<List<EventCategory>> _allEvents;

		private readonly ModuleSettings _moduleSettings;

		private readonly EventState _eventState;

		private IEnumerable<EventAreaConfiguration> _areaConfigurations;

		private Dictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();

		private Panel _areaPanel;

		private StandardWindow _manageEventsWindow;

		private StandardWindow _reorderEventsWindow;

		public event EventHandler<AddAreaEventArgs> AddArea;

		public event EventHandler<EventAreaConfiguration> RemoveArea;

		public AreaSettingsView(Func<IEnumerable<EventAreaConfiguration>> areaConfiguration, Func<List<EventCategory>> allEvents, ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, EventState eventState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, settingEventState, font)
		{
			_areaConfigurationFunc = areaConfiguration;
			_allEvents = allEvents;
			_moduleSettings = moduleSettings;
			_eventState = eventState;
		}

		private void LoadConfigurations()
		{
			_areaConfigurations = _areaConfigurationFunc().ToList();
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			LoadConfigurations();
			Panel newParent = GetPanel(((Control)parent).get_Parent());
			((Control)newParent).set_Location(((Control)parent).get_Location());
			((Control)newParent).set_Size(((Control)parent).get_Size());
			((Container)newParent).set_HeightSizingMode(((Container)parent).get_HeightSizingMode());
			((Container)newParent).set_WidthSizingMode(((Container)parent).get_WidthSizingMode());
			Rectangle bounds = default(Rectangle);
			((Rectangle)(ref bounds))._002Ector(20, 20, ((Container)newParent).get_ContentRegion().Width - 20, ((Container)newParent).get_ContentRegion().Height - 40);
			Panel areaOverviewPanel = GetPanel((Container)(object)newParent);
			areaOverviewPanel.set_ShowBorder(true);
			areaOverviewPanel.set_CanScroll(true);
			((Container)areaOverviewPanel).set_HeightSizingMode((SizingMode)0);
			((Container)areaOverviewPanel).set_WidthSizingMode((SizingMode)0);
			((Control)areaOverviewPanel).set_Location(new Point(bounds.X, bounds.Y));
			((Control)areaOverviewPanel).set_Size(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X - 75, bounds.Height - 26));
			Estreya.BlishHUD.Shared.Controls.Menu menu = new Estreya.BlishHUD.Shared.Controls.Menu();
			((Control)menu).set_Parent((Container)(object)areaOverviewPanel);
			((Container)menu).set_WidthSizingMode((SizingMode)2);
			Estreya.BlishHUD.Shared.Controls.Menu areaOverviewMenu = menu;
			foreach (EventAreaConfiguration areaConfiguration4 in _areaConfigurations)
			{
				string itemName = areaConfiguration4.Name;
				if (!string.IsNullOrWhiteSpace(itemName))
				{
					MenuItem val = new MenuItem(itemName);
					((Control)val).set_Parent((Container)(object)areaOverviewMenu);
					val.set_Text(itemName);
					((Container)val).set_WidthSizingMode((SizingMode)2);
					((Container)val).set_HeightSizingMode((SizingMode)1);
					MenuItem menuItem2 = val;
					_menuItems.Add(itemName, menuItem2);
				}
			}
			int x = ((Control)areaOverviewPanel).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_PanelOffset().X;
			Rectangle areaPanelBounds = new Rectangle(x, bounds.Y, bounds.Width - x, bounds.Height);
			_menuItems.ToList().ForEach(delegate(KeyValuePair<string, MenuItem> menuItem)
			{
				((Control)menuItem.Value).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					EventAreaConfiguration areaConfiguration3 = _areaConfigurations.Where((EventAreaConfiguration areaConfiguration) => areaConfiguration.Name == menuItem.Key).First();
					BuildEditPanel(newParent, areaPanelBounds, menuItem.Value, areaConfiguration3);
				});
			});
			Estreya.BlishHUD.Shared.Controls.Button button = RenderButton(newParent, base.TranslationState.GetTranslation("areaSettingsView-add-btn", "Add"), delegate
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				BuildAddPanel(newParent, areaPanelBounds, areaOverviewMenu);
			});
			((Control)button).set_Location(new Point(((Control)areaOverviewPanel).get_Left(), ((Control)areaOverviewPanel).get_Bottom() + 10));
			((Control)button).set_Width(((Control)areaOverviewPanel).get_Width());
			if (_menuItems.Count > 0)
			{
				KeyValuePair<string, MenuItem> menuItem3 = _menuItems.First();
				EventAreaConfiguration areaConfiguration2 = _areaConfigurations.Where((EventAreaConfiguration areaConfiguration) => areaConfiguration.Name == menuItem3.Key).First();
				BuildEditPanel(newParent, areaPanelBounds, menuItem3.Value, areaConfiguration2);
			}
		}

		private void CreateAreaPanel(Panel parent, Rectangle bounds)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			ClearAreaPanel();
			_areaPanel = GetPanel((Container)(object)parent);
			_areaPanel.set_ShowBorder(true);
			_areaPanel.set_CanScroll(false);
			((Container)_areaPanel).set_HeightSizingMode((SizingMode)0);
			((Container)_areaPanel).set_WidthSizingMode((SizingMode)0);
			((Control)_areaPanel).set_Location(new Point(bounds.X, bounds.Y));
			((Control)_areaPanel).set_Size(new Point(bounds.Width, bounds.Height));
		}

		private void BuildAddPanel(Panel parent, Rectangle bounds, Estreya.BlishHUD.Shared.Controls.Menu menu)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			CreateAreaPanel(parent, bounds);
			Rectangle panelBounds = ((Container)_areaPanel).get_ContentRegion();
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)_areaPanel);
			((Control)val).set_Location(new Point(20, 20));
			((TextInputBase)val).set_PlaceholderText("Area Name");
			TextBox areaName = val;
			string name;
			EventAreaConfiguration configuration2;
			MenuItem menuItem;
			Estreya.BlishHUD.Shared.Controls.Button saveButton = RenderButton(_areaPanel, base.TranslationState.GetTranslation("areaSettingsView-save-btn", "Save"), delegate
			{
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					name = ((TextInputBase)areaName).get_Text();
					if (_areaConfigurations.Any((EventAreaConfiguration configuration) => configuration.Name == name))
					{
						ShowError("Name already used");
					}
					else
					{
						AddAreaEventArgs addAreaEventArgs = new AddAreaEventArgs
						{
							Name = name
						};
						this.AddArea?.Invoke(this, addAreaEventArgs);
						configuration2 = addAreaEventArgs.AreaConfiguration;
						if (configuration2 == null)
						{
							throw new ArgumentNullException("Area configuration could not be created.");
						}
						menuItem = ((Menu)menu).AddMenuItem(name, (Texture2D)null);
						((Control)menuItem).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							//IL_001c: Unknown result type (might be due to invalid IL or missing references)
							BuildEditPanel(parent, bounds, menuItem, configuration2);
						});
						BuildEditPanel(parent, bounds, menuItem, configuration2);
					}
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)saveButton).set_Enabled(false);
			((Control)saveButton).set_Right(((Rectangle)(ref panelBounds)).get_Right() - 20);
			((Control)saveButton).set_Bottom(((Rectangle)(ref panelBounds)).get_Bottom() - 20);
			((TextInputBase)areaName).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
			{
				TextBox val2 = (TextBox)((s is TextBox) ? s : null);
				((Control)saveButton).set_Enabled(!string.IsNullOrWhiteSpace(((TextInputBase)val2).get_Text()));
			});
			Estreya.BlishHUD.Shared.Controls.Button button = RenderButton(_areaPanel, base.TranslationState.GetTranslation("areaSettingsView-cancel-btn", "Cancel"), delegate
			{
				ClearAreaPanel();
			});
			((Control)button).set_Right(((Control)saveButton).get_Left() - 10);
			((Control)button).set_Bottom(((Rectangle)(ref panelBounds)).get_Bottom() - 20);
			((Control)areaName).set_Width(((Rectangle)(ref panelBounds)).get_Right() - 20 - ((Control)areaName).get_Left());
		}

		private void BuildEditPanel(Panel parent, Rectangle bounds, MenuItem menuItem, EventAreaConfiguration areaConfiguration)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			if (areaConfiguration == null)
			{
				throw new ArgumentNullException("areaConfiguration");
			}
			CreateAreaPanel(parent, bounds);
			Rectangle contentRegion = ((Container)_areaPanel).get_ContentRegion();
			Point location = ((Rectangle)(ref contentRegion)).get_Location();
			contentRegion = ((Container)_areaPanel).get_ContentRegion();
			int num = ((Rectangle)(ref contentRegion)).get_Size().X - 50;
			contentRegion = ((Container)_areaPanel).get_ContentRegion();
			Rectangle panelBounds = default(Rectangle);
			((Rectangle)(ref panelBounds))._002Ector(location, new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y));
			Label val = new Label();
			((Control)val).set_Location(new Point(20, 20));
			((Control)val).set_Parent((Container)(object)_areaPanel);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_AutoSizeHeight(true);
			val.set_Text(areaConfiguration.Name);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label areaName = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Left(((Control)areaName).get_Left());
			((Control)val2).set_Top(((Control)areaName).get_Bottom() + 75);
			((Control)val2).set_Parent((Container)(object)_areaPanel);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			FlowPanel settingsPanel = val2;
			((Control)settingsPanel).DoUpdate(GameService.Overlay.get_CurrentGameTime());
			RenderEnabledSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			RenderLocationAndSizeSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			RenderLayoutSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			RenderTextAndColorSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			RenderBehaviourSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			RenderFillerSettings(settingsPanel, areaConfiguration);
			RenderEmptyLine((Panel)(object)settingsPanel);
			((IEnumerable<Control>)((Container)settingsPanel).get_Children()).Last();
			Estreya.BlishHUD.Shared.Controls.Button manageEventsButton = RenderButton(_areaPanel, base.TranslationState.GetTranslation("areaSettingsView-manageEvents-btn", "Manage Events"), delegate
			{
				ManageEvents(areaConfiguration);
			});
			((Control)manageEventsButton).set_Top(((Control)areaName).get_Top());
			((Control)manageEventsButton).set_Left(((Control)settingsPanel).get_Left());
			Estreya.BlishHUD.Shared.Controls.Button button = RenderButton(_areaPanel, base.TranslationState.GetTranslation("areaSettingsView-reorderEvents-btn", "Reorder Events"), delegate
			{
				ReorderEvents(areaConfiguration);
			});
			((Control)button).set_Top(((Control)manageEventsButton).get_Bottom() + 2);
			((Control)button).set_Left(((Control)manageEventsButton).get_Left());
			Estreya.BlishHUD.Shared.Controls.Button removeButton = RenderButtonAsync(_areaPanel, base.TranslationState.GetTranslation("areaSettingsView-remove-btn", "Remove"), async delegate
			{
				ConfirmDialog dialog = new ConfirmDialog("Delete Event Area \"" + areaConfiguration.Name + "\"", "Your are in the process of deleting the event area \"" + areaConfiguration.Name + "\".\nThis action will delete all settings.\n\nContinue?", base.IconState, new ButtonDefinition[2]
				{
					new ButtonDefinition("Yes", DialogResult.Yes),
					new ButtonDefinition("No", DialogResult.No)
				})
				{
					SelectedButtonIndex = 1
				};
				DialogResult num2 = await dialog.ShowDialog();
				((Control)dialog).Dispose();
				if (num2 == DialogResult.Yes)
				{
					this.RemoveArea?.Invoke(this, areaConfiguration);
					((Container)(((Control)menuItem).get_Parent() as Estreya.BlishHUD.Shared.Controls.Menu)).RemoveChild((Control)(object)menuItem);
					_menuItems.Remove(areaConfiguration.Name);
					ClearAreaPanel();
					LoadConfigurations();
				}
			});
			((Control)removeButton).set_Top(((Control)areaName).get_Top());
			((Control)removeButton).set_Right(((Rectangle)(ref panelBounds)).get_Right());
			((Control)areaName).set_Left(((Control)manageEventsButton).get_Right());
			((Control)areaName).set_Width(((Control)removeButton).get_Left() - ((Control)areaName).get_Left());
		}

		private void RenderEnabledSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(false);
			((Panel)val).set_Title("Enabled");
			FlowPanel groupPanel = val;
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.Enabled);
			RenderKeybindingSetting((Panel)(object)groupPanel, areaConfiguration.EnabledKeybinding);
			RenderEnumSetting<DrawInterval>((Panel)(object)groupPanel, areaConfiguration.DrawInterval);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void RenderLocationAndSizeSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Panel)val).set_Title("Location & Size");
			FlowPanel groupPanel = val;
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.Location.X);
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.Location.Y);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.Size.X);
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.EventHeight);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void RenderLayoutSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Panel)val).set_Title("Layout");
			FlowPanel groupPanel = val;
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.DrawBorders);
			RenderEnumSetting<BuildDirection>((Panel)(object)groupPanel, areaConfiguration.BuildDirection);
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.TimeSpan);
			RenderIntSetting((Panel)(object)groupPanel, areaConfiguration.HistorySplit);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void RenderTextAndColorSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Panel)val).set_Title("Text & Color");
			FlowPanel groupPanel = val;
			base.RenderEnumSetting<FontSize>((Panel)(object)groupPanel, areaConfiguration.FontSize);
			RenderColorSetting((Panel)(object)groupPanel, areaConfiguration.TextColor);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.EventTextOpacity);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.DrawShadows);
			RenderColorSetting((Panel)(object)groupPanel, areaConfiguration.ShadowColor);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.ShadowOpacity);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.TimeLineOpacity);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderColorSetting((Panel)(object)groupPanel, areaConfiguration.BackgroundColor);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.Opacity);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.EventBackgroundOpacity);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void RenderBehaviourSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Panel)val).set_Title("Behaviours");
			FlowPanel groupPanel = val;
			RenderEnumSetting<LeftClickAction>((Panel)(object)groupPanel, areaConfiguration.LeftClickAction);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.AcceptWaypointPrompt);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.ShowTooltips);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderEnumSetting<EventCompletedAction>((Panel)(object)groupPanel, areaConfiguration.CompletionAction);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.CompletedEventsBackgroundOpacity);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.CompletedEventsTextOpacity);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.CompletedEventsInvertTextColor);
			RenderButton((Panel)(object)groupPanel, "Reset hidden Events", delegate
			{
				_eventState.Remove(areaConfiguration.Name, EventState.EventStates.Hidden);
			});
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.LimitToCurrentMap);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.AllowUnspecifiedMap);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void RenderFillerSettings(FlowPanel settingsPanel, EventAreaConfiguration areaConfiguration)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)settingsPanel);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Control)settingsPanel).get_Width() - 30);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Panel)val).set_Title("Fillers");
			FlowPanel groupPanel = val;
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.UseFiller);
			RenderColorSetting((Panel)(object)groupPanel, areaConfiguration.FillerTextColor);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.FillerTextOpacity);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderBoolSetting((Panel)(object)groupPanel, areaConfiguration.DrawShadowsForFiller);
			RenderColorSetting((Panel)(object)groupPanel, areaConfiguration.FillerShadowColor);
			RenderFloatSetting((Panel)(object)groupPanel, areaConfiguration.FillerShadowOpacity);
			RenderEmptyLine((Panel)(object)groupPanel, 20);
		}

		private void ReorderEvents(EventAreaConfiguration configuration)
		{
			if (_reorderEventsWindow == null)
			{
				_reorderEventsWindow = WindowUtil.CreateStandardWindow("Reorder Events", ((object)this).GetType(), Guid.Parse("b5cbbd99-f02d-4229-8dda-869b42ac242e"), base.IconState);
			}
			if (((WindowBase2)_reorderEventsWindow).get_CurrentView() != null)
			{
				(((WindowBase2)_reorderEventsWindow).get_CurrentView() as ReorderEventsView).SaveClicked -= new EventHandler<(EventAreaConfiguration, string[])>(ReorderView_SaveClicked);
			}
			ReorderEventsView view = new ReorderEventsView(_allEvents(), configuration.EventOrder.get_Value(), configuration, base.APIManager, base.IconState, base.TranslationState);
			view.SaveClicked += new EventHandler<(EventAreaConfiguration, string[])>(ReorderView_SaveClicked);
			_reorderEventsWindow.Show((IView)(object)view);
		}

		private void ReorderView_SaveClicked(object sender, (EventAreaConfiguration AreaConfiguration, string[] CategoryKeys) e)
		{
			e.AreaConfiguration.EventOrder.set_Value(new List<string>(e.CategoryKeys));
		}

		private void ManageEvents(EventAreaConfiguration configuration)
		{
			if (_manageEventsWindow == null)
			{
				_manageEventsWindow = WindowUtil.CreateStandardWindow("Manage Events", ((object)this).GetType(), Guid.Parse("7dc52c82-67ae-4cfb-9fe3-a16a8b30892c"), base.IconState);
			}
			if (((WindowBase2)_manageEventsWindow).get_CurrentView() != null)
			{
				(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageEventsView).EventChanged -= ManageView_EventChanged;
			}
			ManageEventsView view = new ManageEventsView(_allEvents(), new Dictionary<string, object>
			{
				{ "configuration", configuration },
				{
					"hiddenEventKeys",
					(from x in _eventState.Instances
						where x.AreaName == configuration.Name && x.State == EventState.EventStates.Hidden
						select x.EventKey).ToList()
				}
			}, () => configuration.DisabledEventKeys.get_Value(), _moduleSettings, base.APIManager, base.IconState, base.TranslationState);
			view.EventChanged += ManageView_EventChanged;
			_manageEventsWindow.Show((IView)(object)view);
		}

		private void ManageView_EventChanged(object sender, ManageEventsView.EventChangedArgs e)
		{
			EventAreaConfiguration configuration = e.AdditionalData["configuration"] as EventAreaConfiguration;
			configuration.DisabledEventKeys.set_Value(e.NewState ? new List<string>(from aek in configuration.DisabledEventKeys.get_Value()
				where aek != e.EventSettingKey
				select aek) : new List<string>(configuration.DisabledEventKeys.get_Value()) { e.EventSettingKey });
		}

		private void ClearAreaPanel()
		{
			if (_areaPanel != null)
			{
				((Control)_areaPanel).Hide();
				((Container)_areaPanel).get_Children()?.ToList().ForEach(delegate(Control child)
				{
					child.Dispose();
				});
				((Container)_areaPanel).ClearChildren();
				((Control)_areaPanel).Dispose();
				_areaPanel = null;
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void Unload()
		{
			base.Unload();
			ClearAreaPanel();
			_areaConfigurations = null;
			_menuItems?.Clear();
			StandardWindow manageEventsWindow = _manageEventsWindow;
			if (manageEventsWindow != null)
			{
				((Control)manageEventsWindow).Dispose();
			}
			_manageEventsWindow = null;
			StandardWindow reorderEventsWindow = _reorderEventsWindow;
			if (reorderEventsWindow != null)
			{
				((Control)reorderEventsWindow).Dispose();
			}
			_reorderEventsWindow = null;
		}
	}
}
