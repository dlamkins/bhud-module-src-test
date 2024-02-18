using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Recipe
{
	public class RecipeDetailsViewPresenter : Presenter<RecipeDetailsView, MysticItem>, IRecipeDetailsViewPresenter, IPresenter
	{
		private float TargetScrollDistance { get; set; }

		public RecipeDetailsViewPresenter(RecipeDetailsView view, MysticItem model)
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
				ServiceContainer.WalletService
			};
			if (_recurringServices.Any((IRecurringService s) => !s.Loaded))
			{
				foreach (IRecurringService item in _recurringServices)
				{
					item.LoadingFinished += delegate
					{
						if (!_recurringServices.Any((IRecurringService s) => s.Loading) && !base.View.RecipeDetailsLoaded)
						{
							base.View.BuildRecipeDetails();
						}
					};
				}
				base.View.BuildLoadingPanel(parent, _recurringServices);
			}
			else
			{
				base.View.BuildRecipeDetails();
			}
		}

		public void SaveScrollDistance()
		{
			if (base.View.Scrollbar != null)
			{
				TargetScrollDistance = base.View.Scrollbar.ScrollDistance * (float)(base.View.RecipeItemList.Height - base.View.Scrollbar.Size.Y);
			}
		}

		public void UpdateScrollDistance()
		{
			if (base.View.Scrollbar != null)
			{
				float distance = TargetScrollDistance / (float)(base.View.RecipeItemList.Height - base.View.Scrollbar.Size.Y);
				base.View.Scrollbar.ScrollDistance = distance;
			}
		}
	}
}
