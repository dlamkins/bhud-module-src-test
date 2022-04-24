using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.ToolSearch
{
	public class ToolSearchStandardWindow : StandardWindow
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Logger _logger;

		private readonly Label _infoLabel;

		private readonly FlowPanel _charactersFlowPanel;

		private readonly LoadingSpinner _loadingSpinner;

		private readonly Checkbox _showOnlyUnlimitedToolsCheckbox;

		private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

		private bool _apiAccessFailed;

		private const string API_KEY_ERROR_MESSAGE = "Error: API key problem.\nPossible Reasons:\n- After starting GW2 you have to log into a character once for Blish to know which API key to use.\n- Blish needs a few more seconds to give an API token to the module. You may have to reopen window to update.\n- API key is missing in Blish. Add API key to Blish.\n- API key exists but is missing permissions. Add API key with necessary permissions to Blish.\n- API is down or has issues or something else went wrong. Check Blish log file.";

		public ToolSearchStandardWindow(SettingEntry<bool> showOnlyUnlimitedToolsSetting, Texture2D windowBackgroundTexture, Gw2ApiManager gw2ApiManager, Logger logger)
			: this(windowBackgroundTexture, new Rectangle(10, 30, 235, 610), new Rectangle(30, 30, 230, 600))
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			ToolSearchStandardWindow toolSearchStandardWindow = this;
			_gw2ApiManager = gw2ApiManager;
			_logger = logger;
			Label val = new Label();
			((Control)val).set_Location(new Point(0, 30));
			val.set_TextColor(Color.get_White());
			val.set_ShowShadow(true);
			((Control)val).set_Size(new Point(200, 0));
			val.set_AutoSizeHeight(true);
			((Control)val).set_ClipsBounds(false);
			val.set_WrapText(true);
			((Control)val).set_Parent((Container)(object)this);
			_infoLabel = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(60, 60));
			((Control)val2).set_Parent((Container)(object)this);
			_loadingSpinner = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text("Only unlimited tools");
			val3.set_Checked(showOnlyUnlimitedToolsSetting.get_Value());
			((Control)val3).set_BasicTooltipText("Show only unlimited tools");
			((Control)val3).set_Parent((Container)(object)this);
			_showOnlyUnlimitedToolsCheckbox = val3;
			_showOnlyUnlimitedToolsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate(object s, CheckChangedEvent e)
			{
				showOnlyUnlimitedToolsSetting.set_Value(e.get_Checked());
				await toolSearchStandardWindow.ShowWindowAndUpdateToolsInUi();
			});
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Location(new Point(0, 30));
			((Control)val4).set_Size(new Point(200, 500));
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val4).set_CanScroll(true);
			((Control)val4).set_Parent((Container)(object)this);
			_charactersFlowPanel = val4;
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
			((Container)_charactersFlowPanel).ClearChildren();
			_infoLabel.set_Text("Getting API data...");
			((Control)_loadingSpinner).Show();
			List<CharacterAndTools> charactersAndTools = await GetToolsFromApi();
			_infoLabel.set_Text(string.Empty);
			((Control)_loadingSpinner).Hide();
			if (_apiAccessFailed)
			{
				_infoLabel.set_Text("Error: API key problem.\nPossible Reasons:\n- After starting GW2 you have to log into a character once for Blish to know which API key to use.\n- Blish needs a few more seconds to give an API token to the module. You may have to reopen window to update.\n- API key is missing in Blish. Add API key to Blish.\n- API key exists but is missing permissions. Add API key with necessary permissions to Blish.\n- API is down or has issues or something else went wrong. Check Blish log file.");
				return;
			}
			List<CharacterAndTools> filteredCharactersAndTools = FilterCharacters(charactersAndTools, _showOnlyUnlimitedToolsCheckbox.get_Checked());
			if (filteredCharactersAndTools.Any())
			{
				ShowToolsInUi(filteredCharactersAndTools);
			}
			else
			{
				_infoLabel.set_Text("No tools found with current search filter or no character has tools equipped!");
			}
		}

		private async Task<List<CharacterAndTools>> GetToolsFromApi()
		{
			_apiAccessFailed = false;
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_apiAccessFailed = true;
				return new List<CharacterAndTools>();
			}
			try
			{
				return await GatheringToolsService.GetCharactersAndTools(_gw2ApiManager);
			}
			catch (Exception e)
			{
				_apiAccessFailed = true;
				_logger.Error("Could not get gathering tools from API", new object[1] { e });
				return new List<CharacterAndTools>();
			}
		}

		private static List<CharacterAndTools> FilterCharacters(List<CharacterAndTools> charactersAndTools, bool showOnlyUnlimitedTools)
		{
			List<CharacterAndTools> filteredCharactersAndTools = charactersAndTools.Where((CharacterAndTools c) => c.HasTools()).ToList();
			if (showOnlyUnlimitedTools)
			{
				filteredCharactersAndTools = filteredCharactersAndTools.Where((CharacterAndTools c) => c.HasUnlimitedTools()).ToList();
			}
			return filteredCharactersAndTools;
		}

		private void ShowToolsInUi(List<CharacterAndTools> charactersAndTools)
		{
			foreach (CharacterAndTools charactersAndTool in charactersAndTools)
			{
				CharacterAndToolsFlowPanel characterAndToolsFlowPanel = new CharacterAndToolsFlowPanel(charactersAndTool, _showOnlyUnlimitedToolsCheckbox.get_Checked(), _logger);
				((FlowPanel)characterAndToolsFlowPanel).set_FlowDirection((ControlFlowDirection)3);
				((Container)characterAndToolsFlowPanel).set_WidthSizingMode((SizingMode)1);
				((Container)characterAndToolsFlowPanel).set_HeightSizingMode((SizingMode)1);
				((Panel)characterAndToolsFlowPanel).set_ShowBorder(true);
				((Control)characterAndToolsFlowPanel).set_Parent((Container)(object)_charactersFlowPanel);
			}
		}
	}
}
