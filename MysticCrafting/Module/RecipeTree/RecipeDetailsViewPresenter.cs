using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public void InitializeScrollbar()
		{
			if (base.get_View().Scrollbar == null)
			{
				return;
			}
			((Control)base.get_View().Scrollbar).add_PropertyChanged((PropertyChangedEventHandler)delegate(object sender, PropertyChangedEventArgs args)
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				if (args.PropertyName == "ScrollDistance")
				{
					float num = TargetScrollDistance / (float)(((Control)base.get_View().TreeView).get_Height() + 96 - ((Control)base.get_View().Scrollbar).get_Size().Y);
					if (base.get_View().Scrollbar.get_ScrollDistance() == 0f && num > 0.99f)
					{
						base.get_View().Scrollbar.set_ScrollDistance(0.99f);
						SaveScrollDistance();
					}
				}
			});
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
				base.get_View().Scrollbar.set_ScrollDistance(Math.Min(distance, 1f));
			}
		}
	}
}
