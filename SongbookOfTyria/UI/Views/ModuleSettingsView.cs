using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Settings;

namespace SongbookOfTyria.UI.Views
{
	public class ModuleSettingsView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleSettingsView>();

		private static readonly Color InfoColor = Color.get_White();

		private static readonly Color SuccessColor = new Color(100, 200, 100);

		private static readonly Color WarningColor = new Color(255, 200, 100);

		private static readonly Color ErrorColor = new Color(255, 100, 100);

		private readonly ModuleSettings _moduleSettings;

		private FlowPanel _settingsPanel;

		private StandardButton _refreshButton;

		private Label _cacheStatusLabel;

		private Label _authStatusLabel;

		private Checkbox _enableGuildAuthCheckbox;

		private TextBox _apiKeyTextBox;

		private Panel _apiKeyPanel;

		public ModuleSettingsView(ModuleSettings moduleSettings)
			: this()
		{
			_moduleSettings = moduleSettings ?? throw new ArgumentNullException("moduleSettings");
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 10f));
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			((Control)val).set_Parent(buildPanel);
			_settingsPanel = val;
			BuildRefreshSection();
			BuildSettingsEntries();
			BuildAuthStatusSection();
			_moduleSettings.CacheStatusChanged += OnCacheStatusChanged;
			_moduleSettings.AuthStatusChanged += OnAuthStatusChanged;
		}

		private void BuildRefreshSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Width(((Container)_settingsPanel).get_ContentRegion().Width - 20);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			FlowPanel refreshPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Refresh Data");
			((Control)val2).set_Width(120);
			((Control)val2).set_Height(26);
			((Control)val2).set_Parent((Container)(object)refreshPanel);
			_refreshButton = val2;
			Label val3 = new Label();
			val3.set_Text("");
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Height(26);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_TextColor(InfoColor);
			((Control)val3).set_Parent((Container)(object)refreshPanel);
			_cacheStatusLabel = val3;
			((Control)_refreshButton).add_Click((EventHandler<MouseEventArgs>)OnRefreshButtonClick);
		}

		private void BuildSettingsEntries()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Width(((Container)_settingsPanel).get_ContentRegion().Width - 20);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			FlowPanel settingsContainer = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(((Container)settingsContainer).get_ContentRegion().Width);
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)settingsContainer);
			Panel checkboxPanel = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text("Enable OPUS Guild Authentication");
			((Control)val3).set_BasicTooltipText("When enabled, uses your GW2 API key to verify OPUS guild membership and unlock private tabs.");
			val3.set_Checked(_moduleSettings.EnableGuildAuth);
			((Control)val3).set_Parent((Container)(object)checkboxPanel);
			_enableGuildAuthCheckbox = val3;
			_enableGuildAuthCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnEnableGuildAuthCheckboxChanged);
			Panel val4 = new Panel();
			((Control)val4).set_Width(((Container)settingsContainer).get_ContentRegion().Width);
			((Control)val4).set_Height(30);
			((Control)val4).set_Visible(_moduleSettings.EnableGuildAuth);
			((Control)val4).set_Parent((Container)(object)settingsContainer);
			_apiKeyPanel = val4;
			Label val5 = new Label();
			val5.set_Text("GW2 API Key");
			((Control)val5).set_BasicTooltipText("Enter your GW2 API key with 'account' permission. Get one at https://account.arena.net/applications");
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Height(27);
			((Control)val5).set_Location(new Point(0, 0));
			((Control)val5).set_Parent((Container)(object)_apiKeyPanel);
			TextBox val6 = new TextBox();
			((TextInputBase)val6).set_Text(_moduleSettings.Gw2ApiKey);
			((Control)val6).set_BasicTooltipText("Enter your GW2 API key with 'account' permission. Get one at https://account.arena.net/applications");
			((Control)val6).set_Size(new Point(700, 27));
			((Control)val6).set_Location(new Point(100, 0));
			((Control)val6).set_Parent((Container)(object)_apiKeyPanel);
			_apiKeyTextBox = val6;
			((TextInputBase)_apiKeyTextBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnApiKeyTextBoxFocusChanged);
			Label val7 = new Label();
			val7.set_Text("");
			val7.set_AutoSizeWidth(true);
			((Control)val7).set_Height(26);
			val7.set_TextColor(InfoColor);
			((Control)val7).set_Visible(_moduleSettings.EnableGuildAuth);
			((Control)val7).set_Parent((Container)(object)settingsContainer);
			_authStatusLabel = val7;
		}

		private void BuildAuthStatusSection()
		{
		}

		private async void OnRefreshButtonClick(object sender, MouseEventArgs e)
		{
			if (!((Control)_refreshButton).get_Enabled())
			{
				return;
			}
			((Control)_refreshButton).set_Enabled(false);
			_refreshButton.set_Text("Refreshing...");
			UpdateCacheStatus("Refreshing data...", StatusType.Info);
			try
			{
				await _moduleSettings.RefreshDataAsync();
				UpdateCacheStatus("Data refreshed successfully.", StatusType.Success);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to refresh data: " + ex.Message);
				UpdateCacheStatus("Failed to refresh data.", StatusType.Error);
			}
			finally
			{
				((Control)_refreshButton).set_Enabled(true);
				_refreshButton.set_Text("Refresh Data");
			}
		}

		private void OnCacheStatusChanged(object sender, StatusChangedEventArgs e)
		{
			UpdateCacheStatus(e.Message, e.Type);
		}

		private void OnAuthStatusChanged(object sender, StatusChangedEventArgs e)
		{
			UpdateAuthStatus(e.Message, e.Type);
		}

		private void OnEnableGuildAuthCheckboxChanged(object sender, CheckChangedEvent e)
		{
			_moduleSettings.SetEnableGuildAuth(e.get_Checked());
			UpdateGuildAuthControlsVisibility(e.get_Checked());
		}

		private void UpdateGuildAuthControlsVisibility(bool visible)
		{
			if (_apiKeyPanel != null)
			{
				((Control)_apiKeyPanel).set_Visible(visible);
			}
			if (_authStatusLabel != null)
			{
				((Control)_authStatusLabel).set_Visible(visible);
			}
		}

		private void OnApiKeyTextBoxFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				_moduleSettings.SetGw2ApiKey(((TextInputBase)_apiKeyTextBox).get_Text());
			}
		}

		private void UpdateCacheStatus(string message, StatusType type)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (_cacheStatusLabel != null)
			{
				_cacheStatusLabel.set_Text(message);
				_cacheStatusLabel.set_TextColor(GetColorForStatus(type));
			}
		}

		private void UpdateAuthStatus(string message, StatusType type)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (_authStatusLabel != null)
			{
				_authStatusLabel.set_Text(message);
				_authStatusLabel.set_TextColor(GetColorForStatus(type));
			}
		}

		private Color GetColorForStatus(StatusType type)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(type switch
			{
				StatusType.Success => SuccessColor, 
				StatusType.Warning => WarningColor, 
				StatusType.Error => ErrorColor, 
				_ => InfoColor, 
			});
		}

		protected override void Unload()
		{
			if (_refreshButton != null)
			{
				((Control)_refreshButton).remove_Click((EventHandler<MouseEventArgs>)OnRefreshButtonClick);
			}
			if (_enableGuildAuthCheckbox != null)
			{
				_enableGuildAuthCheckbox.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnEnableGuildAuthCheckboxChanged);
			}
			if (_apiKeyTextBox != null)
			{
				((TextInputBase)_apiKeyTextBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnApiKeyTextBoxFocusChanged);
			}
			if (_moduleSettings != null)
			{
				_moduleSettings.CacheStatusChanged -= OnCacheStatusChanged;
				_moduleSettings.AuthStatusChanged -= OnAuthStatusChanged;
			}
			FlowPanel settingsPanel = _settingsPanel;
			if (settingsPanel != null)
			{
				((Control)settingsPanel).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
