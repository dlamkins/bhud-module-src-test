using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 10f));
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent(buildPanel);
			_settingsPanel = val;
			buildPanel.add_ChildAdded((EventHandler<ChildChangedEventArgs>)delegate(object s, ChildChangedEventArgs e)
			{
				Control changedChild = e.get_ChangedChild();
				Scrollbar val2 = (Scrollbar)(object)((changedChild is Scrollbar) ? changedChild : null);
				if (val2 != null)
				{
					((Control)val2).set_ZIndex(int.MaxValue);
				}
			});
			BuildGuildAuthSection();
			_moduleSettings.AuthStatusChanged += OnAuthStatusChanged;
		}

		private void BuildGuildAuthSection()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title("OPUS Guild Authentication");
			((Control)val).set_Width(((Container)_settingsPanel).get_ContentRegion().Width - 50);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(5f, 10f));
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			FlowPanel sectionPanel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(((Container)sectionPanel).get_ContentRegion().Width - 10);
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)sectionPanel);
			Panel checkboxPanel = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text("Enable Guild Authentication");
			((Control)val3).set_BasicTooltipText("When enabled, uses your GW2 API key to verify OPUS guild membership and unlock private tabs.");
			val3.set_Checked(_moduleSettings.EnableGuildAuth);
			((Control)val3).set_Parent((Container)(object)checkboxPanel);
			_enableGuildAuthCheckbox = val3;
			_enableGuildAuthCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnEnableGuildAuthCheckboxChanged);
			Panel val4 = new Panel();
			((Control)val4).set_Width(((Container)sectionPanel).get_ContentRegion().Width - 10);
			((Control)val4).set_Height(30);
			((Control)val4).set_Visible(_moduleSettings.EnableGuildAuth);
			((Control)val4).set_Parent((Container)(object)sectionPanel);
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
			((Control)val6).set_Size(new Point(500, 27));
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
			((Control)val7).set_Parent((Container)(object)sectionPanel);
			_authStatusLabel = val7;
		}

		private void BuildKeybindSection(string title, SettingEntry<KeyBinding>[] keybinds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(title);
			((Control)val).set_Width(((Container)_settingsPanel).get_ContentRegion().Width - 50);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_OuterControlPadding(new Vector2(5f, 10f));
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			FlowPanel sectionPanel = val;
			foreach (SettingEntry<KeyBinding> keybind in keybinds)
			{
				KeybindingAssigner val2 = new KeybindingAssigner(keybind.get_Value());
				val2.set_KeyBindingName(((SettingEntry)keybind).get_DisplayName());
				((Control)val2).set_BasicTooltipText(((SettingEntry)keybind).get_Description());
				val2.set_NameWidth(100);
				((Control)val2).set_Width(250);
				((Control)val2).set_Parent((Container)(object)sectionPanel);
			}
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
