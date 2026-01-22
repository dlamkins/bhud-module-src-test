using System;
using BhModule.WebPeeper.Strings;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace BhModule.WebPeeper.Window
{
	public class Warning : FlowPanel
	{
		private readonly WarningContent _content;

		private readonly Checkbox _alwaysHideCheckbox;

		private readonly StandardButton _acceptBtn;

		private readonly Action _createWebPainter;

		public static bool Accepted = !WebPeeperModule.Instance.Settings.IsShowWarning.get_Value();

		public Warning(Action createWebPainter)
			: this()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			_createWebPainter = createWebPainter;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			WarningContent warningContent = new WarningContent();
			((Control)warningContent).set_Parent((Container)(object)this);
			_content = warningContent;
			Checkbox val = new Checkbox();
			val.set_Text(BhModule.WebPeeper.Strings.UIService.Hide_Warning);
			((Control)val).set_Parent((Container)(object)this);
			_alwaysHideCheckbox = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text(BhModule.WebPeeper.Strings.UIService.Accept_Warning);
			((Control)val2).set_Width(100);
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)this);
			_acceptBtn = val2;
			((Control)_acceptBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Accept();
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			((Control)this).RecalculateLayout();
			WebPeeperModule.Instance.CefService.SetBrowserSize(((Control)this).get_Width(), ((Control)this).get_Height());
		}

		public override void RecalculateLayout()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			if (_content != null && _acceptBtn != null && _alwaysHideCheckbox != null)
			{
				((Control)_content).set_Left(15);
				((Control)_content).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_acceptBtn).get_Height() - ((Control)_alwaysHideCheckbox).get_Height() - (int)((FlowPanel)this).get_OuterControlPadding().Y - 1);
				((Control)_content).set_Width(((Container)this).get_ContentRegion().Width - 30);
				((Control)_acceptBtn).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_acceptBtn).get_Width() / 2);
				((Control)_alwaysHideCheckbox).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_alwaysHideCheckbox).get_Width() / 2);
			}
		}

		private void Accept()
		{
			((Control)this).set_Parent((Container)null);
			Accepted = true;
			_createWebPainter();
			WebPeeperModule.Instance.Settings.IsShowWarning.set_Value(!_alwaysHideCheckbox.get_Checked());
			((Control)this).Dispose();
		}
	}
}
