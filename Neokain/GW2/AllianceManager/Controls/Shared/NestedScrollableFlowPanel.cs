using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class NestedScrollableFlowPanel : FlowPanel
	{
		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			Control activeControl = GameService.Input.get_Mouse().get_ActiveControl();
			if (!IsMouseOverNestedScrollable(activeControl))
			{
				((Control)this).OnMouseWheelScrolled(e);
			}
		}

		private bool IsMouseOverNestedScrollable(Control activeControl)
		{
			Control current = activeControl;
			while (current != null && current != this)
			{
				Panel panel = (Panel)(object)((current is Panel) ? current : null);
				if (panel != null && panel.get_CanScroll())
				{
					return true;
				}
				current = (Control)(object)current.get_Parent();
			}
			return false;
		}

		public NestedScrollableFlowPanel()
			: this()
		{
		}
	}
}
