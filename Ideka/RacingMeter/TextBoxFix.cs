using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.RacingMeter
{
	public class TextBoxFix : TextBox
	{
		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (((Control)this).get_Enabled())
			{
				((TextInputBase)this).OnLeftMouseButtonPressed(e);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (((Control)this).get_Enabled())
			{
				((TextInputBase)this).OnClick(e);
			}
		}

		public TextBoxFix()
			: this()
		{
		}
	}
}
