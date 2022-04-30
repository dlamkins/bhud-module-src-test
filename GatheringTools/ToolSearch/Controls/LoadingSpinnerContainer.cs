using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace GatheringTools.ToolSearch.Controls
{
	public class LoadingSpinnerContainer : Container
	{
		public LoadingSpinnerContainer()
			: this()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Location(new Point(60, 0));
			((Control)val).set_Parent((Container)(object)this);
		}
	}
}
