using System;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Events;
using BhModule.Community.Pathing.UI.Views;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;

namespace BhModule.Community.Pathing.UI.Presenter
{
	public class CategoryTreePresenter : Presenter<CategoryTreeView, PackInitiator>
	{
		private readonly PathingModule _module;

		private bool _packEventsInitialized;

		private bool _updatingView;

		private static readonly Logger _logger = Logger.GetLogger<CategoryTreePresenter>();

		public CategoryTreePresenter(CategoryTreeView view, PathingModule module)
			: base(view, module.PackInitiator)
		{
			_module = module;
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			if (!((Module)_module).get_Loaded())
			{
				((Module)_module).add_ModuleLoaded((EventHandler<EventArgs>)_module_ModuleLoaded);
			}
			else
			{
				Initialize();
			}
			return Task.FromResult(result: true);
		}

		protected override void UpdateView()
		{
			if (_updatingView || base.get_View().TreeView == null)
			{
				return;
			}
			base.get_View().TreeView.ClearChildNodes();
			if (_module.PackInitiator == null || _module.PackInitiator.IsLoading || !base.get_View().ValidateMarkerPacksState())
			{
				return;
			}
			base.get_View().TreeView.SetPackInitiator(_module.PackInitiator);
			try
			{
				_updatingView = true;
				base.get_View().TreeView.LoadNodes();
				if (base.get_View().TargetCategory != null)
				{
					base.get_View().TreeView.NavigateToPath(base.get_View().TargetCategory.GetPath());
					base.get_View().TargetCategory = null;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Failed to update view.");
			}
			finally
			{
				_updatingView = false;
			}
			if (!_packEventsInitialized)
			{
				InitalizePackEvents();
			}
		}

		private void _module_ModuleLoaded(object sender, EventArgs e)
		{
			Initialize();
		}

		private void Initialize()
		{
			base.get_View().TreeView.SetPackInitiator(_module.PackInitiator);
			InitalizePackEvents();
			if (!_module.PackInitiator.IsLoading)
			{
				base.get_View().SetLoading(loading: true);
				ThreadUtil.RunOnMainThread(((Presenter<CategoryTreeView, PackInitiator>)this).UpdateView);
				base.get_View().SetLoading(loading: false);
			}
		}

		private void InitalizePackEvents()
		{
			_module.PackInitiator.LoadMapFromEachPackStarted += PackInitiatorOnLoadMapFromEachPackStarted;
			_module.PackInitiator.LoadMapFromEachPackFinished += PackInitiatorOnLoadMapFromEachPackFinished;
			_module.PackInitiator.PackState.CategoryStates.CategoryInactiveChanged += CategoryStatesOnCategoryInactiveChanged;
			_module.PackInitiator.PackState.CategoryStates.CategoryStatesOptimized += CategoryStatesOnCategoryStatesOptimized;
			_packEventsInitialized = true;
		}

		private void CategoryStatesOnCategoryStatesOptimized(object sender, EventArgs e)
		{
			base.get_View().TreeView?.UpdateSearchResultsCheckState(_module.PackInitiator.PackState);
		}

		private void CategoryStatesOnCategoryInactiveChanged(object sender, PathingCategoryEventArgs e)
		{
			base.get_View().TreeView?.UpdateCheckedState(e.Category, e.Active);
		}

		private void PackInitiatorOnLoadMapFromEachPackStarted(object sender, EventArgs e)
		{
			base.get_View().SetLoading(loading: true);
			base.get_View().TreeView.ClearChildNodes();
		}

		private void PackInitiatorOnLoadMapFromEachPackFinished(object sender, EventArgs e)
		{
			ThreadUtil.RunOnMainThread(((Presenter<CategoryTreeView, PackInitiator>)this).UpdateView);
			base.get_View().SetLoading(loading: false);
		}

		protected override void Unload()
		{
			if (_module != null)
			{
				((Module)_module).remove_ModuleLoaded((EventHandler<EventArgs>)_module_ModuleLoaded);
				if (_module.PackInitiator != null)
				{
					_module.PackInitiator.LoadMapFromEachPackStarted -= PackInitiatorOnLoadMapFromEachPackStarted;
					_module.PackInitiator.LoadMapFromEachPackFinished -= PackInitiatorOnLoadMapFromEachPackFinished;
					_module.PackInitiator.PackState.CategoryStates.CategoryInactiveChanged -= CategoryStatesOnCategoryInactiveChanged;
				}
			}
			_packEventsInitialized = false;
			base.Unload();
		}
	}
}
