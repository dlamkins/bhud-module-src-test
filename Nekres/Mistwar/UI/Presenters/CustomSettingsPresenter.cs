using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Nekres.Mistwar.UI.Models;
using Nekres.Mistwar.UI.Views;

namespace Nekres.Mistwar.UI.Presenters
{
	public class CustomSettingsPresenter : Presenter<CustomSettingsView, CustomSettingsModel>
	{
		public CustomSettingsPresenter(CustomSettingsView view, CustomSettingsModel model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.get_View().SocialButtonClicked += View_SocialButtonClicked;
			return base.Load(progress);
		}

		protected override void Unload()
		{
			base.get_View().SocialButtonClicked -= View_SocialButtonClicked;
		}

		private void View_SocialButtonClicked(object sender, EventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			BrowserUtil.OpenInDefaultBrowser(((Control)sender).get_BasicTooltipText());
		}
	}
}
