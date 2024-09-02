using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace FarmingTracker
{
	public class PlaceholderTabView : View
	{
		private readonly string _featureText;

		private readonly bool _onlyShowFeatureText;

		public PlaceholderTabView(string featureText, bool onlyShowFeatureText = false)
			: this()
		{
			_featureText = featureText;
			_onlyShowFeatureText = onlyShowFeatureText;
		}

		protected override void Build(Container buildPanel)
		{
			string text = (_onlyShowFeatureText ? _featureText : (_featureText + " not yet implemented. May come with a future release!"));
			new HintLabel(buildPanel, text);
		}
	}
}
