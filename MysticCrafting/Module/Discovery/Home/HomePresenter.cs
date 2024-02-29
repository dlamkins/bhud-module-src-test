using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.Discovery.Home
{
	public class HomePresenter : Presenter<HomeView, HomeViewModel>, IHomePresenter, IPresenter
	{
		public HomePresenter(HomeView view, HomeViewModel model)
			: base(view, model)
		{
		}

		protected override void UpdateView()
		{
		}
	}
}
