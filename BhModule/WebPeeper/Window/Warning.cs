using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BhModule.WebPeeper.Strings;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;

namespace BhModule.WebPeeper.Window
{
	internal class Warning : FlowPanel
	{
		private static readonly IReadOnlyDictionary<CefAvailableVersion, string> _versions = GetVersions();

		private readonly Dropdown _versionSeletor;

		private readonly WarningContent _content;

		private readonly Checkbox _alwaysHideCheckbox;

		private readonly StandardButton _acceptBtn;

		public static bool IsAccepted { get; private set; } = !WebPeeperModule.Instance.Settings.IsShowWarning.get_Value();


		public event EventHandler<EventArgs> Accepted;

		public Warning()
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(110);
			val.set_SelectedItem(_versions[WebPeeperModule.Instance.Settings.CefVersion.get_Value()]);
			((Control)val).set_BasicTooltipText(((SettingEntry)WebPeeperModule.Instance.Settings.CefVersion).get_Description());
			_versionSeletor = val;
			_versionSeletor.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
			{
				KeyValuePair<CefAvailableVersion, string> keyValuePair = _versions.FirstOrDefault((KeyValuePair<CefAvailableVersion, string> v) => v.Value == e.get_CurrentValue());
				WebPeeperModule.Instance.Settings.CefVersion.set_Value(keyValuePair.Key);
			});
			foreach (KeyValuePair<CefAvailableVersion, string> version in _versions)
			{
				_versionSeletor.get_Items().Add(version.Value);
			}
			WebPeeperModule.Instance.Settings.CefVersion.add_SettingChanged((EventHandler<ValueChangedEventArgs<CefAvailableVersion>>)UpdateVersion);
			WarningContent warningContent = new WarningContent();
			((Control)warningContent).set_Parent((Container)(object)this);
			_content = warningContent;
			Checkbox val2 = new Checkbox();
			val2.set_Text(UIService.Hide_Warning);
			((Control)val2).set_Parent((Container)(object)this);
			_alwaysHideCheckbox = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text(UIService.Accept_Warning);
			((Control)val3).set_Width(100);
			((Control)val3).set_Height(30);
			((Control)val3).set_Parent((Container)(object)this);
			_acceptBtn = val3;
			((Control)_acceptBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Accept();
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			((Control)this).RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			if (_versionSeletor != null && _content != null && _acceptBtn != null && _alwaysHideCheckbox != null)
			{
				((Control)_versionSeletor).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_versionSeletor).get_Width() / 2);
				((Control)_content).set_Left(15);
				((Control)_content).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_versionSeletor).get_Height() - ((Control)_acceptBtn).get_Height() - ((Control)_alwaysHideCheckbox).get_Height() - (int)((FlowPanel)this).get_OuterControlPadding().Y - 1);
				((Control)_content).set_Width(((Container)this).get_ContentRegion().Width - 30);
				((Control)_alwaysHideCheckbox).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_alwaysHideCheckbox).get_Width() / 2);
				((Control)_acceptBtn).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_acceptBtn).get_Width() / 2);
			}
		}

		private void Accept()
		{
			IsAccepted = true;
			this.Accepted?.Invoke(this, EventArgs.Empty);
			WebPeeperModule.Instance.Settings.IsShowWarning.set_Value(!_alwaysHideCheckbox.get_Checked());
			((Control)this).Dispose();
		}

		private void UpdateVersion(object sender, ValueChangedEventArgs<CefAvailableVersion> evt)
		{
			_versionSeletor.set_SelectedItem(_versions[evt.get_NewValue()]);
		}

		private static Dictionary<CefAvailableVersion, string> GetVersions()
		{
			Dictionary<CefAvailableVersion, string> dictionary = new Dictionary<CefAvailableVersion, string>();
			FieldInfo[] fields = typeof(CefAvailableVersion).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				DescriptionAttribute descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault();
				dictionary.Add((CefAvailableVersion)fieldInfo.GetValue(null), descriptionAttribute.Description);
			}
			return dictionary;
		}

		protected override void DisposeControl()
		{
			this.Accepted = null;
			WebPeeperModule.Instance.Settings.CefVersion.remove_SettingChanged((EventHandler<ValueChangedEventArgs<CefAvailableVersion>>)UpdateVersion);
			((FlowPanel)this).DisposeControl();
		}
	}
}
