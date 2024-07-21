using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;

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
			List<IRecurringService> _recurringServices = new List<IRecurringService>
			{
				ServiceContainer.TradingPostService,
				ServiceContainer.PlayerItemService,
				ServiceContainer.PlayerUnlocksService,
				ServiceContainer.WalletService
			};
			if (_recurringServices.Any((IRecurringService s) => !s.Loaded))
			{
				foreach (IRecurringService item in _recurringServices)
				{
					item.LoadingFinished += delegate
					{
						if (!_recurringServices.Any((IRecurringService s) => s.Loading) && !base.get_View().RecipeDetailsLoaded)
						{
							base.get_View().LoadRecipeDetails();
						}
					};
				}
				base.get_View().BuildLoadingPanel(parent, _recurringServices);
			}
			else
			{
				base.get_View().LoadRecipeDetails();
			}
		}

		public void SaveScrollDistance()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (base.get_View().Scrollbar != null && !float.IsNaN(base.get_View().Scrollbar.get_ScrollDistance()))
			{
				TargetScrollDistance = base.get_View().Scrollbar.get_ScrollDistance() * (float)(((Control)base.get_View().TreeView).get_Height() - ((Control)base.get_View().Scrollbar).get_Size().Y);
			}
		}

		public void UpdateScrollDistance()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (base.get_View().Scrollbar != null && !float.IsNaN(TargetScrollDistance))
			{
				float distance = TargetScrollDistance / (float)(((Control)base.get_View().TreeView).get_Height() - ((Control)base.get_View().Scrollbar).get_Size().Y);
				base.get_View().Scrollbar.set_ScrollDistance(distance);
			}
		}
	}
}
