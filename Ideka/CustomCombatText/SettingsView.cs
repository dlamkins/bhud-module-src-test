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
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
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
					IView val2 = (IView)(object)new ColorSettingView(colorItem, ((Control)_settingFlowPanel).get_Width());
					obj = val2;
				}
				IView view = obj;
				if (view != null)
				{
					ViewContainer val3 = new ViewContainer();
					((Container)val3).set_WidthSizingMode((SizingMode)2);
					((Container)val3).set_HeightSizingMode((SizingMode)1);
					((Control)val3).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val3;
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
