using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Resources;

namespace flakysalt.CharacterKeybinds.Views
{
	public class KeybindMigrationTab : View
	{
		private FlowPanel mainFlowPanel;

		private Checkbox confirmationCheckbox;

		private StandardButton startMigrtionButton;

		private StandardButton deleteOldDataButton;

		private Label resultLabel;

		private Label explinationLabel;

		public EventHandler OnMigrateClicked;

		public EventHandler OnDeleteClicked;

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			((Container)val).set_WidthSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Parent(buildPanel);
			mainFlowPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)mainFlowPanel);
			((Control)val2).set_Width(((Control)mainFlowPanel).get_Width());
			val2.set_Text(Loca.migration);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)mainFlowPanel);
			((Control)val3).set_Width(((Control)mainFlowPanel).get_Width());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			val3.set_Text("This tab allows you to migrate your keybindings from the old version of the module to the new version.\nSome specialization might no be translated perfectly so please check after the process if your keybinds look fine.\n\nDont worry, you will not lose your old keybind data in this process. You can delete it afterwards if you want to.\n(This will probably be the only time you have to do this!)");
			explinationLabel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)mainFlowPanel);
			((Control)val4).set_Width(((Control)mainFlowPanel).get_Width());
			val4.set_Text("Start Migration");
			((Control)val4).set_BasicTooltipText("Starts the migration process from the old data to the new data format.");
			startMigrtionButton = val4;
			Checkbox val5 = new Checkbox();
			((Control)val5).set_Padding(new Thickness(50f, 0f, 0f, 0f));
			((Control)val5).set_Parent((Container)(object)mainFlowPanel);
			((Control)val5).set_Width(((Control)mainFlowPanel).get_Width());
			val5.set_Text("I understand deleting old data is irreversible.");
			val5.set_Checked(false);
			confirmationCheckbox = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)mainFlowPanel);
			((Control)val6).set_Width(((Control)mainFlowPanel).get_Width());
			val6.set_Text("Delete Old Keybind Data");
			((Control)val6).set_BasicTooltipText("Deletes the old keybind data from the previous version of the module.");
			((Control)val6).set_Enabled(confirmationCheckbox.get_Checked());
			deleteOldDataButton = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)mainFlowPanel);
			((Control)val7).set_Width(((Control)mainFlowPanel).get_Width());
			val7.set_Text("");
			val7.set_AutoSizeHeight(true);
			val7.set_AutoSizeWidth(true);
			resultLabel = val7;
			confirmationCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				((Control)deleteOldDataButton).set_Enabled(confirmationCheckbox.get_Checked());
			});
			((Control)startMigrtionButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnMigrateClicked?.Invoke(sender, (EventArgs)(object)args);
			});
			((Control)deleteOldDataButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnDeleteClicked?.Invoke(sender, (EventArgs)(object)args);
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		public void SetDeletionText()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			resultLabel.set_Text("Old Data Deleted Successfully.");
			resultLabel.set_TextColor(Color.get_LimeGreen());
		}

		public void SetMigrationResult(List<string> result)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			resultLabel.set_Text("");
			if (result.Count == 0)
			{
				resultLabel.set_Text("Migration completed successfully with no issues.");
				resultLabel.set_TextColor(Color.get_LimeGreen());
				return;
			}
			resultLabel.set_TextColor(Color.get_OrangeRed());
			resultLabel.set_Text("Problems found during migration. Please check the following specialcations manually:\n");
			foreach (string VARIABLE in result)
			{
				Label obj = resultLabel;
				obj.set_Text(obj.get_Text() + VARIABLE + "\n");
			}
		}

		public KeybindMigrationTab()
			: this()
		{
		}
	}
}
