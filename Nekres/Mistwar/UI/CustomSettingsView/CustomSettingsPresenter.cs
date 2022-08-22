using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Nekres.Mistwar.UI.CustomSettingsView
{
	public class CustomSettingsPresenter : Presenter<CustomSettingsView, CustomSettingsModel>
	{
		public CustomSettingsPresenter(CustomSettingsView view, CustomSettingsModel model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.get_View().BrowserButtonClick += View_BrowserButtonClicked;
			return base.Load(progress);
		}

		protected override void Unload()
		{
			base.get_View().BrowserButtonClick -= View_BrowserButtonClicked;
		}

		private void View_BrowserButtonClicked(object o, EventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
			BrowserUtil.OpenInDefaultBrowser(((Control)o).get_BasicTooltipText());
		}
	}
}