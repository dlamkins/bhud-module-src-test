using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using GatheringTools.Services;
using GatheringTools.ToolSearch.Model;
using GatheringTools.ToolSearch.Services;
using Microsoft.Xna.Framework;

namespace GatheringTools.ToolSearch.Controls
{
	public class ToolSearchStandardWindow : StandardWindow
	{
		private readonly TextureService _textureService;

		private readonly List<GatheringTool> _allGatheringTools;

		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Logger _logger;

		private readonly Label _infoLabel;

		private readonly FlowPanel _toolLocationsFlowPanel;

		private readonly Checkbox _showOnlyUnlimitedToolsCheckbox;

		private readonly Checkbox _showBankCheckbox;

		private readonly Checkbox _showSharedInventoryCheckbox;

		private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

		private readonly LoadingSpinnerContainer _loadingSpinnerContainer;

		private const int MAX_CONTENT_WIDTH = 210;

		private const int SPACING_BETWEEN_EQUIPPED_AND_INVENTORY_TOOLS = 5;

		private const string API_KEY_ERROR_MESSAGE = "Error: API key problem.\nPossible Reasons:\n- After starting GW2 you have to log into a character once for Blish to know which API key to use.\n- Blish needs a few more seconds to give an API token to the module. You may have to reopen window to update.\n- API key is missing in Blish. Add API key to Blish.\n- API key exists but is missing permissions. Add API key with necessary permissions to Blish.\n- API is down or has issues or something else went wrong. Check Blish log file.";

		public ToolSearchStandardWindow(TextureService textureService, SettingService settingService, List<GatheringTool> allGatheringTools, Gw2ApiManager gw2ApiManager, Logger logger)
			: this(textureService.WindowBackgroundTexture, new Rectangle(10, 30, 235, 610), new Rectangle(30, 30, 230, 600))
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Expected O, but got Unknown
			ToolSearchStandardWindow toolSearchStandardWindow = this;
			_textureService = textureService;
			_allGatheringTools = allGatheringTools;
			_gw2ApiManager = gw2ApiManager;
			_logger = logger;
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel rootFlowPanel = val;
			Checkbox val2 = new Checkbox();
			val2.set_Text("Only unlimited tools");
			val2.set_Checked(settingService.ShowOnlyUnlimitedToolsSetting.get_Value());
			((Control)val2).set_BasicTooltipText("Show only unlimited tools");
			((Control)val2).set_Parent((Container)(object)rootFlowPanel);
			_showOnlyUnlimitedToolsCheckbox = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text("Bank");
			val3.set_Checked(settingService.ShowBankToolsSetting.get_Value());
			((Control)val3).set_BasicTooltipText("Show gathering tools in bank");
			((Control)val3).set_Parent((Container)(object)rootFlowPanel);
			_showBankCheckbox = val3;
			Checkbox val4 = new Checkbox();
			val4.set_Text("Shared inventory slots");
			val4.set_Checked(settingService.ShowSharedInventoryToolsSetting.get_Value());
			((Control)val4).set_BasicTooltipText("Show gathering tools in shared inventory slots");
			((Control)val4).set_Parent((Container)(object)rootFlowPanel);
			_showSharedInventoryCheckbox = val4;
			Label val5 = new Label();
			val5.set_ShowShadow(true);
			((Control)val5).set_Size(new Point(210, 0));
			val5.set_AutoSizeHeight(true);
			val5.set_WrapText(true);
			((Control)val5).set_Parent((Container)(object)rootFlowPanel);
			_infoLabel = val5;
			LoadingSpinnerContainer loadingSpinnerContainer = new LoadingSpinnerContainer();
			((Container)loadingSpinnerContainer).set_WidthSizingMode((SizingMode)1);
			((Container)loadingSpinnerContainer).set_HeightSizingMode((SizingMode)1);
			((Control)loadingSpinnerContainer).set_Parent((Container)(object)rootFlowPanel);
			_loadingSpinnerContainer = loadingSpinnerContainer;
			FlowPanel val6 = new FlowPanel();
			val6.set_ControlPadding(new Vector2(0f, 5f));
			((Control)val6).set_Size(new Point(210, 500));
			val6.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val6).set_CanScroll(true);
			((Control)val6).set_Parent((Container)(object)rootFlowPanel);
			_toolLocationsFlowPanel = val6;
			_showOnlyUnlimitedToolsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate(object s, CheckChangedEvent e)
			{
				settingService.ShowOnlyUnlimitedToolsSetting.set_Value(e.get_Checked());
				await toolSearchStandardWindow.ShowWindowAndUpdateToolsInUi();
			});
			_showBankCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate(object s, CheckChangedEvent e)
			{
				settingService.ShowBankToolsSetting.set_Value(e.get_Checked());
				await toolSearchStandardWindow.ShowWindowAndUpdateToolsInUi();
			});
			_showSharedInventoryCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate(object s, CheckChangedEvent e)
			{
				settingService.ShowSharedInventoryToolsSetting.set_Value(e.get_Checked());
				await toolSearchStandardWindow.ShowWindowAndUpdateToolsInUi();
			});
		}

		public async Task ToggleVisibility()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
			}
			else
			{
				await ShowWindowAndUpdateToolsInUi();
			}
		}

		private async Task ShowWindowAndUpdateToolsInUi()
		{
			((Control)this).Show();
			if (_semaphoreSlim.Wait(0))
			{
				try
				{
					await UpdateToolsInUiFromApi();
				}
				finally
				{
					_semaphoreSlim.Release();
				}
			}
		}

		private async Task UpdateToolsInUiFromApi()
		{
			((Container)_toolLocationsFlowPanel).ClearChildren();
			_infoLabel.set_Text("Getting API data...");
			((Control)_loadingSpinnerContainer).Show();
			var (accountTools, flag) = await FindGatheringToolsService.GetToolsFromApi(_allGatheringTools, _gw2ApiManager, _logger);
			_infoLabel.set_Text(string.Empty);
			((Control)_loadingSpinnerContainer).Hide();
			if (flag)
			{
				_infoLabel.set_Text("Error: API key problem.\nPossible Reasons:\n- After starting GW2 you have to log into a character once for Blish to know which API key to use.\n- Blish needs a few more seconds to give an API token to the module. You may have to reopen window to update.\n- API key is missing in Blish. Add API key to Blish.\n- API key exists but is missing permissions. Add API key with necessary permissions to Blish.\n- API is down or has issues or something else went wrong. Check Blish log file.");
				return;
			}
			FilterGatheringToolsService.FilterTools(accountTools, _showOnlyUnlimitedToolsCheckbox.get_Checked(), _showBankCheckbox.get_Checked(), _showSharedInventoryCheckbox.get_Checked());
			if (accountTools.HasTools())
			{
				ShowToolsInUi(accountTools, _toolLocationsFlowPanel, _textureService, _logger);
			}
			else
			{
				_infoLabel.set_Text("No tools found with current search filter or no character has tools equipped!");
			}
		}

		private static void ShowToolsInUi(AccountTools accountTools, FlowPanel toolLocationsFlowPanel, TextureService textureService, Logger logger)
		{
			if (accountTools.BankGatheringTools.Any())
			{
				HeaderWithToolsFlowPanel headerWithToolsFlowPanel = new HeaderWithToolsFlowPanel("Bank", textureService.BankTexture, accountTools.BankGatheringTools, logger);
				((Container)headerWithToolsFlowPanel).set_WidthSizingMode((SizingMode)1);
				((Container)headerWithToolsFlowPanel).set_HeightSizingMode((SizingMode)1);
				((Panel)headerWithToolsFlowPanel).set_ShowBorder(true);
				((Control)headerWithToolsFlowPanel).set_Parent((Container)(object)toolLocationsFlowPanel);
			}
			if (accountTools.SharedInventoryGatheringTools.Any())
			{
				HeaderWithToolsFlowPanel headerWithToolsFlowPanel2 = new HeaderWithToolsFlowPanel("Shared inventory", textureService.SharedInventoryTexture, accountTools.SharedInventoryGatheringTools, logger);
				((Container)headerWithToolsFlowPanel2).set_WidthSizingMode((SizingMode)1);
				((Container)headerWithToolsFlowPanel2).set_HeightSizingMode((SizingMode)1);
				((Panel)headerWithToolsFlowPanel2).set_ShowBorder(true);
				((Control)headerWithToolsFlowPanel2).set_Parent((Container)(object)toolLocationsFlowPanel);
			}
			foreach (CharacterTools character in accountTools.Characters)
			{
				if (character.HasTools())
				{
					ShowCharacterTools(character, toolLocationsFlowPanel, textureService, logger);
				}
			}
		}

		private static void ShowCharacterTools(CharacterTools character, FlowPanel rootFlowPanel, TextureService textureService, Logger logger)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(character.CharacterName);
			((Control)val).set_BasicTooltipText(character.CharacterName);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Control)val).set_Parent((Container)(object)rootFlowPanel);
			FlowPanel characterFlowPanel = val;
			if (character.EquippedGatheringTools.Any())
			{
				HeaderWithToolsFlowPanel headerWithToolsFlowPanel = new HeaderWithToolsFlowPanel("Equipped tools", textureService.EquipmentTexture, character.EquippedGatheringTools, logger);
				((Container)headerWithToolsFlowPanel).set_WidthSizingMode((SizingMode)1);
				((Container)headerWithToolsFlowPanel).set_HeightSizingMode((SizingMode)1);
				((Panel)headerWithToolsFlowPanel).set_ShowBorder(false);
				((Control)headerWithToolsFlowPanel).set_Parent((Container)(object)characterFlowPanel);
			}
			if (character.InventoryGatheringTools.Any())
			{
				HeaderWithToolsFlowPanel headerWithToolsFlowPanel2 = new HeaderWithToolsFlowPanel("Inventory", textureService.CharacterInventoryTexture, character.InventoryGatheringTools, logger);
				((Container)headerWithToolsFlowPanel2).set_WidthSizingMode((SizingMode)1);
				((Container)headerWithToolsFlowPanel2).set_HeightSizingMode((SizingMode)1);
				((Panel)headerWithToolsFlowPanel2).set_ShowBorder(false);
				((Control)headerWithToolsFlowPanel2).set_Parent((Container)(object)characterFlowPanel);
			}
		}
	}
}
