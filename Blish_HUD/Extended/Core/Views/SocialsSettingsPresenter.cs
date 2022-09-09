using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Blish_HUD.Extended.Core.Views
{
	public class SocialsSettingsPresenter : Presenter<SocialsSettingsView, SocialsSettingsModel>
	{
		public SocialsSettingsPresenter(SocialsSettingsView view, SocialsSettingsModel model)
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

		private async void View_BrowserButtonClicked(object o, EventArgs e)
		{
			((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
			await BrowserUtil.Open(((Control)o).get_BasicTooltipText());
		}
	}
}
