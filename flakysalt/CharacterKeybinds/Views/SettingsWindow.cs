using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Model;

namespace flakysalt.CharacterKeybinds.Views
{
	public class SettingsWindow : View
	{
		private CharacterKeybindsSettings model;

		private CharacterKeybindsTab characterKeybindWindow;

		private Autoclicker troubleshootWindow;

		private FlowPanel _settingFlowPanel;

		private ViewContainer _lastSettingContainer;

		private StandardButton reportBugButton;

		private StandardButton fairMacroUseButton;

		private StandardButton characterKeybindSettinsButton;

		private StandardButton openTroubleshootWindowButton;

		private StandardButton faqButton;

		public SettingsWindow(CharacterKeybindsSettings model, CharacterKeybindsTab assignmentWindow, Autoclicker autoclickView)
			: this()
		{
			this.model = model;
			characterKeybindWindow = assignmentWindow;
			troubleshootWindow = autoclickView;
		}

		protected override void Build(Container buildPanel)
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
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Expected O, but got Unknown
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
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Width(((Control)_settingFlowPanel).get_Width());
			val2.set_FlowDirection((ControlFlowDirection)0);
			val2.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Container)val2).set_AutoSizePadding(new Point(0, 15));
			((Control)val2).set_Parent((Container)(object)_settingFlowPanel);
			FlowPanel topButtonPanel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)topButtonPanel);
			((Control)val3).set_Left(10);
			((Control)val3).set_Size(new Point(200, 50));
			val3.set_Text("Keybind Settings");
			characterKeybindSettinsButton = val3;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)model.settingsCollection).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val4 = new ViewContainer();
					((Container)val4).set_WidthSizingMode((SizingMode)1);
					((Container)val4).set_HeightSizingMode((SizingMode)1);
					((Control)val4).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val4;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Width(((Control)_settingFlowPanel).get_Width());
			val5.set_FlowDirection((ControlFlowDirection)0);
			val5.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Container)val5).set_AutoSizePadding(new Point(0, 15));
			((Control)val5).set_Parent((Container)(object)_settingFlowPanel);
			FlowPanel bottombuttonFlowPanel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val6).set_Size(new Point(200, 30));
			val6.set_Text("Help / FAQ");
			faqButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val7).set_Size(new Point(200, 30));
			val7.set_Text("Report a Bug");
			reportBugButton = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val8).set_Left(10);
			((Control)val8).set_Size(new Point(200, 30));
			val8.set_Text("Arenanet Macro Policy");
			fairMacroUseButton = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_settingFlowPanel);
			((Control)val9).set_Size(new Point(200, 30));
			val9.set_Text("Troubleshoot");
			openTroubleshootWindowButton = val9;
			((Control)faqButton).add_Click((EventHandler<MouseEventArgs>)FaqButton_Click);
			((Control)reportBugButton).add_Click((EventHandler<MouseEventArgs>)ReportBugButton_Click);
			((Control)fairMacroUseButton).add_Click((EventHandler<MouseEventArgs>)FairMacroUseButton_Click);
			((Control)characterKeybindSettinsButton).add_Click((EventHandler<MouseEventArgs>)OpenCharacterKeybindsSettingButton_Click);
			((Control)openTroubleshootWindowButton).add_Click((EventHandler<MouseEventArgs>)OpenTroubleshootWindowButton_Click);
		}

		private void FaqButton_Click(object sender, MouseEventArgs e)
		{
			Process.Start("https://blishhud.com/modules/?module=flakysalt.CharacterKeybinds");
		}

		private void OpenCharacterKeybindsSettingButton_Click(object sender, MouseEventArgs e)
		{
			characterKeybindWindow?.Show();
		}

		private void OpenTroubleshootWindowButton_Click(object sender, MouseEventArgs e)
		{
			Autoclicker autoclicker = troubleshootWindow;
			if (autoclicker != null)
			{
				((Control)autoclicker.WindowView).Show();
			}
		}

		private void ReportBugButton_Click(object sender, MouseEventArgs e)
		{
			Process.Start("https://github.com/flakysalt/Blish-HUD-CharacterKeybinds/issues");
		}

		private void FairMacroUseButton_Click(object sender, MouseEventArgs e)
		{
			Process.Start("https://help.guildwars2.com/hc/en-us/articles/360013762153-Policy-Macros-and-Macro-Use");
		}

		protected override void Unload()
		{
			((Control)reportBugButton).remove_Click((EventHandler<MouseEventArgs>)ReportBugButton_Click);
			((Control)fairMacroUseButton).remove_Click((EventHandler<MouseEventArgs>)FairMacroUseButton_Click);
			((View<IPresenter>)this).Unload();
		}
	}
}
