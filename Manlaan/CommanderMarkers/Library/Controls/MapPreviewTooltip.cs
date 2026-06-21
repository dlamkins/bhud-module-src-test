using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Manlaan.CommanderMarkers.Library.Models;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public static class MapPreviewTooltip
	{
		public static void Apply(Control control, MapPreviewTarget target)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			if (!string.IsNullOrWhiteSpace(target.CommunitySetId) || !string.IsNullOrWhiteSpace(target.Label))
			{
				DetailsButton detailsButton = (DetailsButton)(object)((control is DetailsButton) ? control : null);
				if (detailsButton != null)
				{
					detailsButton.set_HighlightType((DetailsHighlightType)2);
				}
				control.set_BasicTooltipText((string)null);
				control.set_Tooltip(new Tooltip((ITooltipView)(object)new MapPreviewTooltipView(target)));
			}
		}
	}
}
