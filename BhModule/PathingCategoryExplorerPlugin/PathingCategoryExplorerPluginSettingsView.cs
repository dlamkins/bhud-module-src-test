using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace BhModule.PathingCategoryExplorerPlugin
{
	public class PathingCategoryExplorerPluginSettingsView : View
	{
		public static Action UpadateForgeIntervalTitle;

		public static Action DisposeRootFlowPanel;

		private FlowPanel rootFlowPanel;

		private readonly SettingCollection settings;

		public PathingCategoryExplorerPluginSettingsView(SettingCollection settings)
		{
			this.settings = settings;
			((View)this)._002Ector();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			DisposeRootFlowPanel?.Invoke();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			rootFlowPanel = val;
			DisposeRootFlowPanel = delegate
			{
				DisposeRootFlowPanel = null;
				((Control)rootFlowPanel).Dispose();
			};
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView val2;
				if ((val2 = SettingView.FromType(item, ((Control)rootFlowPanel).get_Width())) != null)
				{
					ViewContainer val3 = new ViewContainer();
					((Container)val3).set_WidthSizingMode((SizingMode)2);
					((Container)val3).set_HeightSizingMode((SizingMode)1);
					((Control)val3).set_Parent((Container)(object)rootFlowPanel);
					val3.Show(val2);
				}
			}
		}
	}
}
