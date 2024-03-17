using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class Menu : Menu
	{
		public override void RecalculateLayout()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			((Menu)this).RecalculateLayout();
			int lastBottom = 0;
			foreach (Control item in ((IEnumerable<Control>)((Container)this)._children).Where((Control c) => c.get_Visible()))
			{
				item.set_Location(new Point(0, lastBottom));
				item.set_Width(((Control)this).get_Width());
				lastBottom = item.get_Bottom();
			}
		}

		public Menu()
			: this()
		{
		}
	}
}
