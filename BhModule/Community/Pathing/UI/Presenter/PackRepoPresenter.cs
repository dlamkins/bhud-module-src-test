using System;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.MarkerPackRepo;
using BhModule.Community.Pathing.UI.Controls;
using BhModule.Community.Pathing.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace BhModule.Community.Pathing.UI.Presenter
{
	public class PackRepoPresenter : Presenter<PackRepoView, BhModule.Community.Pathing.MarkerPackRepo.MarkerPackRepo>
	{
		public PackRepoPresenter(PackRepoView view, BhModule.Community.Pathing.MarkerPackRepo.MarkerPackRepo model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void UpdateView()
		{
			((Container)base.get_View().RepoFlowPanel).ClearChildren();
			foreach (MarkerPackPkg item in base.get_Model().MarkerPackages.OrderByDescending((MarkerPackPkg markerPkg) => markerPkg.LastUpdate))
			{
				MarkerPackHero markerPackHero = new MarkerPackHero(item);
				((Control)markerPackHero).set_Parent((Container)(object)base.get_View().RepoFlowPanel);
				((Control)markerPackHero).set_Width(((Control)base.get_View().RepoFlowPanel).get_Width() - 60);
			}
		}
	}
}
