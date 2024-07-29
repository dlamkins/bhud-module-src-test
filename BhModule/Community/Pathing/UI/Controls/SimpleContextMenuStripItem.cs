using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace BhModule.Community.Pathing.UI.Controls
{
	internal class SimpleContextMenuStripItem : ContextMenuStripItem
	{
		private Action _onClick;

		private Action<bool> _onCheck;

		public SimpleContextMenuStripItem(string text, Action onClick)
			: this()
		{
			((ContextMenuStripItem)this).set_Text(text);
			_onClick = onClick;
		}

		public SimpleContextMenuStripItem(string text, Action<bool> onCheck, bool isChecked)
			: this()
		{
			((ContextMenuStripItem)this).set_Text(text);
			((ContextMenuStripItem)this).set_CanCheck(true);
			((ContextMenuStripItem)this).set_Checked(isChecked);
			_onCheck = onCheck;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((ContextMenuStripItem)this).OnClick(e);
			if (_onClick != null)
			{
				_onClick();
			}
		}

		protected override void OnCheckedChanged(CheckChangedEvent e)
		{
			((ContextMenuStripItem)this).OnCheckedChanged(e);
			if (_onCheck != null)
			{
				_onCheck(e.get_Checked());
			}
		}
	}
}
