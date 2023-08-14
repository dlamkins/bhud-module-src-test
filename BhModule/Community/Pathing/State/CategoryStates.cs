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

		private const string INVERTEDSTATE_FILE = "invcategories.txt";

		private const double INTERVAL_SAVESTATE = 5000.0;

		private const double INTERVAL_UPDATEINACTIVECATEGORIES = 100.0;

		private HashSet<string> _inactiveCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private readonly SafeList<PathingCategory> _rawInactiveCategories = new SafeList<PathingCategory>();

		private readonly SafeList<PathingCategory> _rawInvertedCategories = new SafeList<PathingCategory>();

		private double _lastSaveState;

		private double _lastInactiveCategoriesCalculation;

		private bool _stateDirty;

		private bool _calculationDirty;

		public CategoryStates(IRootPackState packState)
			: base(packState)
		{
		}

		private async Task LoadCategoryState(string stateFileName, SafeList<PathingCategory> rawCategoriesList, PathingCategory rootCategory)
		{
			string categoryStatePath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), stateFileName);
			if (File.Exists(categoryStatePath))
			{
				string[] recordedCategories = Array.Empty<string>();
				try
				{
					recordedCategories = await FileUtil.ReadLinesAsync(categoryStatePath);
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to read categories.txt (" + categoryStatePath + ").");
				}
				rawCategoriesList.Clear();
				string[] array = recordedCategories;
				foreach (string categoryNamespace in array)
				{
					rawCategoriesList.Add(rootCategory.GetOrAddCategoryFromNamespace(categoryNamespace));
				}
			}
		}

		private void CleanTwinStates(SafeList<PathingCategory> categories, SafeList<PathingCategory> invertedCategories)
		{
			foreach (PathingCategory twin in categories.ToArray().Intersect(invertedCategories.ToArray()))
			{
				categories.Remove(twin);
				invertedCategories.Remove(twin);
			}
		}

		private async Task LoadStates()
		{
			PathingCategory rootCategory = _rootPackState.RootCategory;
			if (rootCategory != null)
			{
				Logger.Debug("Loading CategoryStates state.");
				await LoadCategoryState("categories.txt", _rawInactiveCategories, rootCategory);
				await LoadCategoryState("invcategories.txt", _rawInvertedCategories, rootCategory);
				CleanTwinStates(_rawInactiveCategories, _rawInvertedCategories);
				_calculationDirty = true;
			}
		}

		private async Task SaveCategoryState(string stateFileName, SafeList<PathingCategory> rawCategoriesList)
		{
			PathingCategory[] toggledCategories = rawCategoriesList.ToArray();
			string categoryStatePath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), stateFileName);
			try
			{
				await FileUtil.WriteLinesAsync(categoryStatePath, toggledCategories.Select((PathingCategory c) => c.Namespace));
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to write " + stateFileName + " (" + categoryStatePath + ").");
			}
		}

		private async Task SaveStates(GameTime gameTime)
		{
			if (_stateDirty)
			{
				Logger.Debug("Saving CategoryStates state.");
				await SaveCategoryState("categories.txt", _rawInactiveCategories);
				await SaveCategoryState("invcategories.txt", _rawInvertedCategories);
				_stateDirty = false;
			}
		}

		private void AddAllSubCategories(HashSet<string> categories, PathingCategory topCategory)
		{
			Queue<PathingCategory> remainingCategories = new Queue<PathingCategory>(topCategory);
			while (remainingCategories.Count > 0)
			{
				PathingCategory category = remainingCategories.Dequeue();
				categories.Add(category.Namespace);
				foreach (PathingCategory subCategory in category)
				{
					remainingCategories.Enqueue(subCategory);
				}
			}
		}

		private void CalculateOptimizedCategoryStates(GameTime gameTime)
		{
			if (!_calculationDirty || _rootPackState.RootCategory == null)
			{
				return;
			}
			PathingCategory[] inactiveCategories = _rawInactiveCategories.ToArray();
			PathingCategory[] activeInvertedCategories = _rawInvertedCategories.ToArray();
			Queue<PathingCategory> remainingCategories = new Queue<PathingCategory>();
			remainingCategories.Enqueue(_rootPackState.RootCategory);
			HashSet<string> preCalcInactiveCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			while (remainingCategories.Count > 0)
			{
				PathingCategory category = remainingCategories.Dequeue();
				if (inactiveCategories.Contains(category) || (!category.DefaultToggle && !activeInvertedCategories.Contains(category)))
				{
					preCalcInactiveCategories.Add(category.Namespace);
					AddAllSubCategories(preCalcInactiveCategories, category);
					continue;
				}
				foreach (PathingCategory subCategory in category)
				{
					remainingCategories.Enqueue(subCategory);
				}
			}
			_inactiveCategories = preCalcInactiveCategories;
			_calculationDirty = false;
		}

		protected override async Task<bool> Initialize()
		{
			await LoadStates();
			return true;
		}

		public override async Task Reload()
		{
			_inactiveCategories.Clear();
			_rawInactiveCategories.Clear();
			_rawInvertedCategories.Clear();
			await LoadStates();
		}

		public override void Update(GameTime gameTime)
		{
			UpdateCadenceUtil.UpdateWithCadence(CalculateOptimizedCategoryStates, gameTime, 100.0, ref _lastInactiveCategoriesCalculation);
			UpdateCadenceUtil.UpdateAsyncWithCadence(SaveStates, gameTime, 5000.0, ref _lastSaveState);
		}

		public override async Task Unload()
		{
			await SaveStates(null);
		}

		public bool GetNamespaceInactive(string categoryNamespace)
		{
			return _inactiveCategories.Contains(categoryNamespace);
		}

		private bool GetCategoryInactive(PathingCategory category, SafeList<PathingCategory> rawCategoriesList)
		{
			return rawCategoriesList.Contains(category);
		}

		public bool GetCategoryInactive(PathingCategory category)
		{
			if (category.DefaultToggle)
			{
				return GetCategoryInactive(category, _rawInactiveCategories);
			}
			return !GetCategoryInactive(category, _rawInvertedCategories);
		}

		private void SetInactive(PathingCategory category, bool isInactive, SafeList<PathingCategory> rawCategoriesList)
		{
			rawCategoriesList.Remove(category);
			if (isInactive)
			{
				rawCategoriesList.Add(category);
			}
			_stateDirty = true;
			_calculationDirty = true;
		}

		public void SetInactive(PathingCategory category, bool isInactive)
		{
			if (category.DefaultToggle)
			{
				SetInactive(category, isInactive, _rawInactiveCategories);
			}
			else
			{
				SetInactive(category, !isInactive, _rawInvertedCategories);
			}
		}

		public void SetInactive(string categoryNamespace, bool isInactive)
		{
			SetInactive(_rootPackState.RootCategory.GetOrAddCategoryFromNamespace(categoryNamespace), isInactive);
		}
	}
}
