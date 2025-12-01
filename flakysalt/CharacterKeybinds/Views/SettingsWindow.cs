using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data.Tutorial;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Resources;

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

		private StandardButton tutorialButton;

		private Action onCloseAction;

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
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Expected O, but got Unknown
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Expected O, but got Unknown
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Expected O, but got Unknown
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Expected O, but got Unknown
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Expected O, but got Unknown
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Expected O, but got Unknown
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
			val3.set_Text(SettingsLoca.tutorialButton);
			tutorialButton = val3;
			onCloseAction = (Action)Delegate.Combine(onCloseAction, (Action)delegate
			{
				model.experiencedFtue.set_Value(true);
			});
			((Control)tutorialButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				TutorialView.Instance.Show(new SetupTutorial());
				((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)topButtonPanel);
			((Control)val4).set_Left(10);
			((Control)val4).set_Size(new Point(200, 50));
			val4.set_Text(SettingsLoca.keybindSettingsButton);
			characterKeybindSettinsButton = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Width(((Control)_settingFlowPanel).get_Width());
			val5.set_FlowDirection((ControlFlowDirection)0);
			val5.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Container)val5).set_AutoSizePadding(new Point(0, 5));
			((Control)val5).set_Parent((Container)(object)_settingFlowPanel);
			FlowPanel topFlowPanel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)topFlowPanel);
			((Control)val6).set_Width(((Control)topFlowPanel).get_Width());
			val6.set_Text(Directory.Exists(model.gw2KeybindsFolder.get_Value()) ? SettingsLoca.keybindsDirectoryValid : SettingsLoca.keybindsDirectoryInvalid);
			val6.set_TextColor(Directory.Exists(model.gw2KeybindsFolder.get_Value()) ? Color.get_GreenYellow() : Color.get_OrangeRed());
			val6.set_AutoSizeHeight(true);
			Label folderLabel = val6;
			((SettingEntry)model.gw2KeybindsFolder).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				bool flag = Directory.Exists(model.gw2KeybindsFolder.get_Value());
				folderLabel.set_Text(flag ? SettingsLoca.keybindsDirectoryValid : SettingsLoca.keybindsDirectoryInvalid);
				folderLabel.set_TextColor(flag ? Color.get_GreenYellow() : Color.get_OrangeRed());
			});
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)model.settingsCollection).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val7 = new ViewContainer();
					((Container)val7).set_WidthSizingMode((SizingMode)1);
					((Container)val7).set_HeightSizingMode((SizingMode)1);
					((Control)val7).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val7;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Width(((Control)_settingFlowPanel).get_Width());
			val8.set_FlowDirection((ControlFlowDirection)0);
			val8.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			((Container)val8).set_AutoSizePadding(new Point(0, 15));
			((Control)val8).set_Parent((Container)(object)_settingFlowPanel);
			FlowPanel bottombuttonFlowPanel = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val9).set_Size(new Point(200, 30));
			val9.set_Text(SettingsLoca.helpFaqButton);
			faqButton = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val10).set_Size(new Point(200, 30));
			val10.set_Text(SettingsLoca.reportBugButton);
			reportBugButton = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)bottombuttonFlowPanel);
			((Control)val11).set_Left(10);
			((Control)val11).set_Size(new Point(200, 30));
			val11.set_Text(SettingsLoca.anetMacroPolicyButton);
			fairMacroUseButton = val11;
			StandardButton val12 = new StandardButton();
			((Control)val12).set_Parent((Container)(object)_settingFlowPanel);
			((Control)val12).set_Size(new Point(200, 30));
			val12.set_Text(SettingsLoca.troubleshootButton);
			openTroubleshootWindowButton = val12;
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
