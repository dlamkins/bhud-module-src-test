using System;
using System.Threading.Tasks;
using Blish_HUD.Graphics.UI;
using felix.BlishEmotes.UI.Views;

namespace felix.BlishEmotes.UI.Presenters
{
	internal class DummySettingsPresenter : Presenter<DummySettingsView, Action>
	{
		public DummySettingsPresenter(DummySettingsView view, Action model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.View.OpenSettingsClicked += View_OpenSettingsClicked;
			return base.Load(progress);
		}

		private void View_OpenSettingsClicked(object sender, EventArgs e)
		{
			base.Model();
		}

		protected override void Unload()
		{
			base.View.OpenSettingsClicked -= View_OpenSettingsClicked;
		}
	}
}
