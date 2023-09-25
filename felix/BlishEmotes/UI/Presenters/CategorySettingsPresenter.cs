using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using felix.BlishEmotes.Exceptions;
using felix.BlishEmotes.UI.Views;

namespace felix.BlishEmotes.UI.Presenters
{
	internal class CategorySettingsPresenter : Presenter<CategorySettingsView, (CategoriesManager, EmotesManager)>
	{
		private static readonly Logger Logger = Logger.GetLogger<CategorySettingsPresenter>();

		public CategorySettingsPresenter(CategorySettingsView view, (CategoriesManager, EmotesManager) model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.View.AddCategory += View_AddCategoryClicked;
			base.View.UpdateCategory += View_UpdateCategoryClicked;
			base.View.DeleteCategory += View_DeleteCategoryClicked;
			base.View.ReorderCategories += View_ReorderCategories;
			base.View.Categories = base.Model.Item1.GetAll();
			base.View.Emotes = base.Model.Item2.GetAll();
			return base.Load(progress);
		}

		public bool IsEmoteInCategory(Guid categoryId, Emote emote)
		{
			try
			{
				return base.Model.Item1.IsEmoteInCategory(categoryId, emote);
			}
			catch (NotFoundException)
			{
				return false;
			}
		}

		private void View_ReorderCategories(object sender, List<Category> e)
		{
			base.Model.Item1.ReorderCategories(e);
		}

		private void View_DeleteCategoryClicked(object sender, Category e)
		{
			base.Model.Item1.DeleteCategory(e);
			base.View.Categories = base.Model.Item1.GetAll();
			base.View.Rebuild();
		}

		private void View_UpdateCategoryClicked(object sender, Category e)
		{
			try
			{
				Category updatedCategory = base.Model.Item1.UpdateCategory(e);
				base.View.Categories = base.Model.Item1.GetAll();
				base.View.Rebuild(updatedCategory);
			}
			catch (UniqueViolationException)
			{
				Logger.Error("Failed to update category " + e.Name + " - Name already in use.");
			}
			catch (NotFoundException)
			{
				Logger.Error("Failed to update category " + e.Name + " - Not found.");
			}
		}

		private void View_AddCategoryClicked(object sender, AddCategoryArgs e)
		{
			try
			{
				Category newCategory = base.Model.Item1.CreateCategory(e.Name, e.Emotes);
				base.View.Categories = base.Model.Item1.GetAll();
				base.View.Rebuild(newCategory);
			}
			catch (UniqueViolationException)
			{
				Logger.Error("Failed to update category " + e.Name + " - Name already in use.");
			}
		}

		protected override void Unload()
		{
			base.View.AddCategory -= View_AddCategoryClicked;
			base.View.UpdateCategory -= View_UpdateCategoryClicked;
			base.View.DeleteCategory -= View_DeleteCategoryClicked;
			base.View.ReorderCategories -= View_ReorderCategories;
		}
	}
}
