using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class SettingsView : SettingView<SettingCollection>
	{
		private FlowPanel? _settingFlowPanel;

		private readonly SettingCollection _settings;

		private bool _lockBounds;

		private ViewContainer? _lastSettingContainer;

		private Label? _statusLabel;

		public string Status
		{
			set
			{
				if (_statusLabel != null)
				{
					_statusLabel!.set_Text(value);
				}
			}
		}

		public bool LockBounds
		{
			get
			{
				return _lockBounds;
			}
			set
			{
				if (_lockBounds != value)
				{
					_lockBounds = value;
					UpdateBoundsLocking(_lockBounds);
				}
			}
		}

		public SettingsView(SettingEntry<SettingCollection> setting, int definedWidth = -1)
		{
			_settings = setting.get_Value();
			_lockBounds = true;
			base._002Ector(setting, definedWidth);
		}

		public SettingsView(SettingCollection settings, int definedWidth = -1)
		{
			SettingEntry<SettingCollection> obj = new SettingEntry<SettingCollection>();
			obj.set_Value(settings);
			this._002Ector(obj, definedWidth);
		}

		private void UpdateBoundsLocking(bool locked)
		{
			if (_settingFlowPanel != null)
			{
				((Panel)_settingFlowPanel).set_ShowBorder(!locked);
				((Panel)_settingFlowPanel).set_CanCollapse(!locked);
			}
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_settingFlowPanel = val;
			Label val2 = new Label();
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)_settingFlowPanel);
			val2.set_Text("\n");
			_statusLabel = val2;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)_settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				SettingEntry<Color> colorItem = item as SettingEntry<Color>;
				IView obj;
				if (colorItem == null)
				{
					obj = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width());
				}
				else
				{
					IView val3 = (IView)(object)new ColorSettingView(colorItem, ((Control)_settingFlowPanel).get_Width());
					obj = val3;
				}
				IView view = obj;
				if (view != null)
				{
					ViewContainer val4 = new ViewContainer();
					((Container)val4).set_WidthSizingMode((SizingMode)2);
					((Container)val4).set_HeightSizingMode((SizingMode)1);
					((Control)val4).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val4;
					_lastSettingContainer!.Show(view);
					SettingsView settingsView = view as SettingsView;
					if (settingsView != null)
					{
						settingsView.LockBounds = false;
					}
				}
			}
			UpdateBoundsLocking(_lockBounds);
		}

		protected override void RefreshDisplayName(string displayName)
		{
			if (_settingFlowPanel != null)
			{
				((Panel)_settingFlowPanel).set_Title(displayName);
			}
		}

		protected override void RefreshDescription(string description)
		{
			if (_settingFlowPanel != null)
			{
				((Control)_settingFlowPanel).set_BasicTooltipText(description);
			}
		}

		protected override void RefreshValue(SettingCollection value)
		{
		}
	}
}
