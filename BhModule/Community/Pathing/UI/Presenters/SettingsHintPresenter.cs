using System;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Views;
using Blish_HUD.Graphics.UI;

namespace BhModule.Community.Pathing.UI.Presenters
{
	public class SettingsHintPresenter : Presenter<SettingsHintView, (Action, Action, PackInitiator)>
	{
		public SettingsHintPresenter(SettingsHintView view, (Action OpenSettings, Action OpenMarkerPacks, PackInitiator packInitiator) model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.get_View().OpenSettingsClicked += View_OpenSettingsClicked;
			base.get_View().OpenMarkerPacksClicked += View_OpenMarkerPacksClicked;
			return base.Load(progress);
		}

		private void View_OpenMarkerPacksClicked(object sender, EventArgs e)
		{
			base.get_Model().Item2();
		}

		private void View_OpenSettingsClicked(object sender, EventArgs e)
		{
			base.get_Model().Item1();
		}

		protected override void Unload()
		{
			base.get_View().OpenSettingsClicked -= View_OpenSettingsClicked;
			base.get_View().OpenMarkerPacksClicked -= View_OpenMarkerPacksClicked;
		}
	}
}
