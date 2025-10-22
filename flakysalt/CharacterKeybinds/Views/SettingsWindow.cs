using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

		private MainWindowView moduleWindowView;

		private AutoClickerView troubleshootWindow;

		private FlowPanel _settingFlowPanel;

		private ViewContainer _lastSettingContainer;

		private StandardButton reportBugButton;

		private StandardButton fairMacroUseButton;

		private StandardButton characterKeybindSettinsButton;

		private StandardButton openTroubleshootWindowButton;

		private StandardButton faqButton;

		public SettingsWindow(CharacterKeybindsSettings model, MainWindowView moduleWindowView, AutoClickerView autoclickView)
			: this()
		{
			this.model = model;
			this.moduleWindowView = moduleWindowView;
			troubleshootWindow = autoclickView;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Expected O, but got Unknown
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Expected O, but got Unknown
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Expected O, but got Unknown
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Expected O, but got Unknown
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Expected O, but got Unknown
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
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Width(((Control)_settingFlowPanel).get_Width());
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Container)val4).set_AutoSizePadding(new Point(0, 15));
			((Control)val4).set_Parent((Container)(object)_settingFlowPanel);
			((Control)val4).set_Visible(!Directory.Exists(model.gw2KeybindsFolder.get_Value()));
			FlowPanel topFlowPanel = val4;
			((SettingEntry)model.gw2KeybindsFolder).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				((Control)topFlowPanel).set_Visible(!Directory.Exists(model.gw2KeybindsFolder.get_Value()));
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)topFlowPanel);
			((Control)val5).set_Width(((Control)topFlowPanel).get_Width());
			val5.set_Text("This Path is not valid! Please change it to where GW2 is storing its keybinds.");
			val5.set_TextColor(Color.get_OrangeRed());
			val5.set_AutoSizeHeight(true);
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)model.settingsCollection).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val6 = new ViewContainer();
					((Container)val6).set_WidthSizingMode((SizingMode)1);
					((Container)val6).set_HeightSizingMode((SizingMode)1);
					((Control)val6).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val6;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Width(((Control)_settingFlowPanel).get_Width());
			val7.set_FlowDirection((ControlFlowDirection)0);
			val7.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			((Container)val7).set_AutoSizePadding(new Point(0, 15));
			((Control)val7).set_Parent((Container)(object)_settingFlowPanel);
			FlowPanel bottombuttonFlowPanel = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val8).set_Size(new Point(200, 30));
			val8.set_Text("Help / FAQ");
			faqButton = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val9).set_Size(new Point(200, 30));
			val9.set_Text("Report a Bug");
			reportBugButton = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val10).set_Left(10);
			((Control)val10).set_Size(new Point(200, 30));
			val10.set_Text("Arenanet Macro Policy");
			fairMacroUseButton = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_settingFlowPanel);
			((Control)val11).set_Size(new Point(200, 30));
			val11.set_Text("Troubleshoot");
			openTroubleshootWindowButton = val11;
			((Control)faqButton).add_Click((EventHandler<MouseEventArgs>)FaqButton_Click);
			((Control)reportBugButton).add_Click((EventHandler<MouseEventArgs>)ReportBugButton_Click);
			((Control)fairMacroUseButton).add_Click((EventHandler<MouseEventArgs>)FairMacroUseButton_Click);
			((Control)characterKeybindSettinsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				moduleWindowView.Show();
			});
			((Control)openTroubleshootWindowButton).add_Click((EventHandler<MouseEventArgs>)OpenTroubleshootWindowButton_Click);
		}

		private void FaqButton_Click(object sender, MouseEventArgs e)
		{
			Process.Start("https://blishhud.com/modules/?module=flakysalt.CharacterKeybinds");
		}

		private void OpenTroubleshootWindowButton_Click(object sender, MouseEventArgs e)
		{
			AutoClickerView autoClickerView = troubleshootWindow;
			if (autoClickerView != null)
			{
				((Control)autoClickerView.WindowView).Show();
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
