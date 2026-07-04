using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace WhereIsMyPSNA
{
	internal class LockableTextBox : TextBox
	{
		public bool Locked { get; set; }

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (!Locked)
			{
				((TextInputBase)this).OnLeftMouseButtonPressed(e);
			}
		}

		public LockableTextBox()
			: this()
		{
		}
	}
}
