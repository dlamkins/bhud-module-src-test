using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.RecipeTree
{
	public class RecipeDetailsViewPresenter : Presenter<RecipeDetailsView, int>, IRecipeDetailsViewPresenter, IPresenter
	{
		private float TargetScrollDistance { get; set; }

		public RecipeDetailsViewPresenter(RecipeDetailsView view, int model)
			: base(view, model)
		{
		}

		protected override void UpdateView()
		{
		}

		public void HandleServiceLoading(Container parent)
		{
			new List<IApiService>
			{
				ServiceContainer.TradingPostService,
				ServiceContainer.PlayerItemService,
				ServiceContainer.PlayerUnlocksService,
				ServiceContainer.WalletService
			};
			base.get_View().LoadRecipeDetails();
		}

		public void SaveScrollDistance()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (base.get_View().Scrollbar != null && !float.IsNaN(base.get_View().Scrollbar.get_ScrollDistance()))
			{
				TargetScrollDistance = base.get_View().Scrollbar.get_ScrollDistance() * (float)(((Control)base.get_View().TreeView).get_Height() + 96 - ((Control)base.get_View().Scrollbar).get_Size().Y);
			}
		}

		public void UpdateScrollDistance()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (base.get_View().Scrollbar != null && !float.IsNaN(TargetScrollDistance))
			{
				float distance = TargetScrollDistance / (float)(((Control)base.get_View().TreeView).get_Height() + 96 - ((Control)base.get_View().Scrollbar).get_Size().Y);
				base.get_View().Scrollbar.set_ScrollDistance(distance);
			}
		}
	}
}
