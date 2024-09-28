using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace FarmingTracker
{
	public class PlaceholderTabView : View
	{
		private readonly string _featureText;

		private readonly bool _onlyShowFeatureText;

		private HintLabel? _hintLabel;

		public PlaceholderTabView(string featureText, bool onlyShowFeatureText = false)
			: this()
		{
			_featureText = featureText;
			_onlyShowFeatureText = onlyShowFeatureText;
		}

		protected override void Unload()
		{
			HintLabel? hintLabel = _hintLabel;
			if (hintLabel != null)
			{
				((Control)hintLabel).Dispose();
			}
			_hintLabel = null;
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			string text = (_onlyShowFeatureText ? _featureText : (_featureText + " not yet implemented. May come with a future release!"));
			_hintLabel = new HintLabel(buildPanel, text);
		}
	}
}
