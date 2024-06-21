using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class ContainerView : BaseView
	{
		public ContainerView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
		}

		protected override void InternalBuild(Panel parent)
		{
		}

		public void Add(Control control)
		{
			control.set_Parent((Container)(object)base.MainPanel);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
