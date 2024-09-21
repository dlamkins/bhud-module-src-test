using System;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree
{
	public class RecipeTableHeaderItemPresenter : Presenter<RecipeTableHeaderItemView, IApiService>, IRecipeTableHeaderItemPresenter, IPresenter
	{
		public RecipeTableHeaderItemPresenter(RecipeTableHeaderItemView view, IApiService model)
			: base(view, model)
		{
		}

		protected override void UpdateView()
		{
			base.get_Model().LoadingStarted += LoadingStarted;
			base.get_Model().LoadingFinished += LoadingFinished;
			if (base.get_Model().Loading)
			{
				base.get_View().Loading = true;
			}
		}

		protected override void Unload()
		{
			base.get_Model().LoadingStarted -= LoadingStarted;
			base.get_Model().LoadingFinished -= LoadingFinished;
			base.Unload();
		}

		public string GetTooltipText()
		{
			return $"{Common.LastLoaded}: {base.get_Model().LastLoaded:t}";
		}

		private void LoadingStarted(object sender, EventArgs e)
		{
			base.get_View().Loading = true;
		}

		private void LoadingFinished(object sender, EventArgs e)
		{
			base.get_View().Loading = false;
			base.get_View().LabelTooltip = GetTooltipText();
			base.get_View().Failed = base.get_Model().LastLoadFailed();
		}
	}
}
