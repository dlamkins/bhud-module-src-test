using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.State
{
	public class CategoryStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<CategoryStates>();

		private const string STATE_FILE = "categories.txt";

		private const double INTERVAL_SAVESTATE = 5000.0;

		private const double INTERVAL_UPDATEINACTIVECATEGORIES = 100.0;

		private HashSet<string> _inactiveCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private readonly SafeList<PathingCategory> _rawInactiveCategories = new SafeList<PathingCategory>();

		private double _lastSaveState;

		private double _lastInactiveCategoriesCalculation;

		private bool _stateDirty;

		private bool _calculationDirty;

		public CategoryStates(IRootPackState packState)
			: base(packState)
		{
		}

		private async Task LoadState()
		{
			string categoryStatesPath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), "categories.txt");
			if (!File.Exists(categoryStatesPath))
			{
				return;
			}
			string[] recordedCategories = Array.Empty<string>();
			try
			{
				recordedCategories = await FileUtil.ReadLinesAsync(categoryStatesPath);
			}
			catch (Exception e)
			{
				Logger.Error(e, "Failed to read categories.txt (" + categoryStatesPath + ").");
			}
			_rawInactiveCategories.Clear();
			PathingCategory rootCategory = _rootPackState.RootCategory;
			if (rootCategory != null)
			{
				string[] array = recordedCategories;
				foreach (string categoryNamespace in array)
				{
					_rawInactiveCategories.Add(rootCategory.GetOrAddCategoryFromNamespace(categoryNamespace));
				}
				_calculationDirty = true;
			}
		}

		private async Task SaveState(GameTime gameTime)
		{
			if (!_stateDirty)
			{
				return;
			}
			Logger.Debug("Saving CategoryStates state.");
			PathingCategory[] inactiveCategories = _rawInactiveCategories.ToArray();
			string categoryStatesPath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), "categories.txt");
			try
			{
				await FileUtil.WriteLinesAsync(categoryStatesPath, inactiveCategories.Select((PathingCategory c) => c.Namespace));
			}
			catch (Exception e)
			{
				Logger.Error(e, "Failed to write categories.txt (" + categoryStatesPath + ").");
			}
			_stateDirty = false;
		}

		private void CalculateOptimizedCategoryStates(GameTime gameTime)
		{
			if (_calculationDirty)
			{
				PathingCategory[] array = _rawInactiveCategories.ToArray();
				HashSet<string> preCalcInactiveCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				PathingCategory[] array2 = array;
				foreach (PathingCategory inactiveCategory in array2)
				{
					AddAllSubCategories(preCalcInactiveCategories, inactiveCategory);
				}
				_inactiveCategories = preCalcInactiveCategories;
				_calculationDirty = false;
			}
		}

		private void AddAllSubCategories(HashSet<string> categories, PathingCategory currentCategory)
		{
			categories.Add(currentCategory.Namespace);
			foreach (PathingCategory subCategory in currentCategory)
			{
				AddAllSubCategories(categories, subCategory);
			}
		}

		protected override async Task<bool> Initialize()
		{
			await LoadState();
			return true;
		}

		public override async Task Reload()
		{
			_inactiveCategories.Clear();
			_rawInactiveCategories.Clear();
			await LoadState();
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateWithCadence(CalculateOptimizedCategoryStates, gameTime, 100.0, ref _lastInactiveCategoriesCalculation);
			UpdateCadenceUtil.UpdateAsyncWithCadence(SaveState, gameTime, 5000.0, ref _lastSaveState);
		}

		public override async Task Unload()
		{
			await SaveState(null);
		}

		public bool GetNamespaceInactive(string categoryNamespace)
		{
			return _inactiveCategories.Contains(categoryNamespace);
		}

		public bool GetCategoryInactive(PathingCategory category)
		{
			return _rawInactiveCategories.Contains(category);
		}

		public void SetInactive(PathingCategory category, bool isInactive)
		{
			_rawInactiveCategories.Remove(category);
			if (isInactive)
			{
				_rawInactiveCategories.Add(category);
			}
			_stateDirty = true;
			_calculationDirty = true;
		}

		public void SetInactive(string categoryNamespace, bool isInactive)
		{
			SetInactive(_rootPackState.RootCategory.GetOrAddCategoryFromNamespace(categoryNamespace), isInactive);
		}
	}
}
