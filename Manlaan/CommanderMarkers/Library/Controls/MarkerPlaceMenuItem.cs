using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class MarkerPlaceMenuItem : ContextMenuStripItem
	{
		private MarkerSet _markerSet;

		public MarkerPlaceMenuItem(MarkerSet markerSet)
			: this("Place " + markerSet.name)
		{
			_markerSet = markerSet;
			((Control)this).set_BasicTooltipText(markerSet.description);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((ContextMenuStripItem)this).OnClick(e);
			Service.MapWatch.PlaceMarkers(_markerSet);
			Service.MapWatch.RemovePreviewMarkerSet();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((ContextMenuStripItem)this).OnMouseEntered(e);
			Service.MapWatch.PreviewMarkerSet(_markerSet);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((ContextMenuStripItem)this).OnMouseLeft(e);
			Service.MapWatch.RemovePreviewMarkerSet();
		}
	}
}
