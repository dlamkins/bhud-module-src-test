using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

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
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			_createWebPainter = createWebPainter;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			((FlowPanel)this).set_OuterControlPadding(new Vector2(0f, 10f));
			StandardButton val = new StandardButton();
			val.set_Text("Got It");
			((Control)val).set_Width(100);
			((Control)val).set_Height(30);
			((Control)val).set_Parent((Container)(object)this);
			_acceptBtn = val;
			((Control)_acceptBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Accept();
			});
			Checkbox val2 = new Checkbox();
			val2.set_Text("Don't show again.");
			((Control)val2).set_Parent((Container)(object)this);
			_alwaysHideCheckbox = val2;
			WarningContent warningContent = new WarningContent();
			((Control)warningContent).set_Parent((Container)(object)this);
			_content = warningContent;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			((Control)this).RecalculateLayout();
			WebPeeperModule.Instance.CefService.SetBrowserSize(((Control)this).get_Width(), ((Control)this).get_Height());
		}

		public override void RecalculateLayout()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).RecalculateLayout();
			if (_content != null && _acceptBtn != null && _alwaysHideCheckbox != null)
			{
				((Control)_content).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_acceptBtn).get_Height() - ((Control)_alwaysHideCheckbox).get_Height() - (int)((FlowPanel)this).get_OuterControlPadding().Y - 1);
				((Control)_content).set_Width(((Container)this).get_ContentRegion().Width - 1);
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
