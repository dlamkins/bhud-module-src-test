using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class ReMenu : Menu
	{
		public override void RecalculateLayout()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			((Menu)this).RecalculateLayout();
			int lastBottom = 0;
			foreach (Control item in ((IEnumerable<Control>)((Container)this)._children).Where((Control c) => c.get_Visible()))
			{
				item.set_Location(new Point(0, lastBottom));
				item.set_Width(((Control)this).get_AbsoluteBounds().Width);
				lastBottom = item.get_Bottom();
			}
		}

		public ReMenu()
			: this()
		{
		}
	}
}
