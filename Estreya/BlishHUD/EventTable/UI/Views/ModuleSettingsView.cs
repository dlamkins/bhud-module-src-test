using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ModuleSettingsView : View
	{
		protected override Task<bool> Load(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = buildPanel.get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			FlowPanel parentPanel = val;
			ViewContainer val2 = new ViewContainer();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)parentPanel);
			ViewContainer settingContainer = val2;
			string buttonText = "Open Settings";
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)settingContainer);
			val3.set_Text(buttonText);
			((Control)val3).set_Width((int)EventTableModule.ModuleInstance.Font.MeasureString(buttonText).Width);
			StandardButton button = val3;
			((Control)button).set_Location(new Point(Math.Max(((Control)buildPanel).get_Width() / 2 - ((Control)button).get_Width() / 2, 20), Math.Max(((Control)buildPanel).get_Height() / 2 - ((Control)button).get_Height(), 20)));
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)EventTableModule.ModuleInstance.SettingsWindow).ToggleWindow();
			});
		}

		public ModuleSettingsView()
			: this()
		{
		}
	}
}
