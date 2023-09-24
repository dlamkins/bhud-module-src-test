using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace Manlaan.CommanderMarkers.CornerIcon
{
	public class CornerIconContextMenu : ContextMenuStrip
	{
		public CornerIconContextMenu(Func<IEnumerable<ContextMenuStripItem>> getItemsDelegate)
			: this(getItemsDelegate)
		{
		}

		protected override void OnHidden(EventArgs e)
		{
			((ContextMenuStrip)this).OnHidden(e);
			Service.MapWatch.RemovePreviewMarkerSet();
		}
	}
}
