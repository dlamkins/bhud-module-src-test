using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Events;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.State
{
	public class CategoryStates : ManagedState
	{
		private static readonly Logger Logger = Logger.GetLogger<CategoryStates>();

		private const string NEW_STATE_FILE = "category_preferences.txt";

		private const string OLD_STATE_FILE = "categories.txt";

		private const string OLD_INVERTED_FILE = "invcategories.txt";

		private const double INTERVAL_SAVESTATE = 5000.0;

		private const double INTERVAL_UPDATEINACTIVECATEGORIES = 100.0;

		private readonly ConcurrentDictionary<string, bool> _explicitStates = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

		private HashSet<string> _evaluatedInactiveCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private double _lastSaveState;

		private double _lastInactiveCategoriesCalculation;

		private bool _stateDirty;

		private bool _calculationDirty;

		public event EventHandler<PathingCategoryEventArgs> CategoryInactiveChanged;

		public event EventHandler<EventArgs> CategoryStatesOptimized;

		public event EventHandler<PathingCategoryEventArgs> TriggerOpenCategoryView;

		public CategoryStates(IRootPackState packState)
			: base(packState)
		{
		}

		protected override async Task<bool> Initialize()
		{
			await LoadStates();
			return true;
		}

		public override async Task Reload()
		{
			await SaveStates(null);
			_explicitStates.Clear();
			_evaluatedInactiveCategories.Clear();
			await LoadStates();
		}

		private async Task LoadStates()
		{
			string dataDir = DataDirUtil.GetSafeDataDir("states");
			string newStatePath = Path.Combine(dataDir, "category_preferences.txt");
			Logger.Debug("Loading CategoryStates state.");
			if (!File.Exists(newStatePath))
			{
				await MigrateOldStates(dataDir);
			}
			else
			{
				await LoadUnifiedState(newStatePath);
			}
			_calculationDirty = true;
		}

		private async Task LoadUnifiedState(string filePath)
		{
			try
			{
				string[] array = await FileUtil.ReadLinesAsync(filePath);
				foreach (string line in array)
				{
					if (!string.IsNullOrWhiteSpace(line) && line.Length >= 2)
					{
						bool explicitActive = line[0] == '+';
						string categoryNamespace = line.Substring(1);
						_explicitStates[categoryNamespace] = explicitActive;
					}
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to read unified category states from " + filePath + ".");
			}
		}

		private async Task MigrateOldStates(string dataDir)
		{
			string oldStatePath = Path.Combine(dataDir, "categories.txt");
			string oldInvertedPath = Path.Combine(dataDir, "invcategories.txt");
			if (File.Exists(oldStatePath))
			{
				try
				{
					string[] array = await FileUtil.ReadLinesAsync(oldStatePath);
					foreach (string ns2 in array)
					{
						_explicitStates[ns2] = false;
					}
				}
				catch (Exception e2)
				{
					Logger.Warn(e2, "Failed to migrate legacy standard categories.");
				}
			}
			if (File.Exists(oldInvertedPath))
			{
				try
				{
					string[] array = await FileUtil.ReadLinesAsync(oldInvertedPath);
					foreach (string ns in array)
					{
						_explicitStates[ns] = true;
					}
				}
				catch (Exception e)
				{
					Logger.Warn(e, "Failed to migrate legacy inverted categories.");
				}
			}
			if (_explicitStates.Count > 0)
			{
				_stateDirty = true;
				Logger.Info($"Successfully migrated {_explicitStates.Count} legacy category states to the unified layout.");
			}
		}

		private async Task SaveStates(GameTime gameTime)
		{
			if (!_stateDirty)
			{
				return;
			}
			Logger.Debug("Saving CategoryStates preferences.");
			string newStatePath = Path.Combine(DataDirUtil.GetSafeDataDir("states"), "category_preferences.txt");
			try
			{
				IEnumerable<string> lines = _explicitStates.Select((KeyValuePair<string, bool> kvp) => (kvp.Value ? "+" : "-") + kvp.Key);
				await FileUtil.WriteLinesAsync(newStatePath, lines);
				_stateDirty = false;
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to write unified category states to " + newStatePath + ".");
			}
		}

		private void CalculateOptimizedCategoryStates(GameTime gameTime)
		{
			if (!_calculationDirty || _rootPackState.RootCategory == null)
			{
				return;
			}
			HashSet<string> preCalcInactive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			Queue<PathingCategory> remainingCategories = new Queue<PathingCategory>();
			remainingCategories.Enqueue(_rootPackState.RootCategory);
			while (remainingCategories.Count > 0)
			{
				PathingCategory category = remainingCategories.Dequeue();
				if (!(_explicitStates.TryGetValue(category.Namespace, out var explicitActive) ? explicitActive : category.DefaultToggle))
				{
					preCalcInactive.Add(category.Namespace);
					AddAllSubCategories(preCalcInactive, category);
					continue;
				}
				foreach (PathingCategory subCategory in category)
				{
					remainingCategories.Enqueue(subCategory);
				}
			}
			_evaluatedInactiveCategories = preCalcInactive;
			this.CategoryStatesOptimized?.Invoke(this, EventArgs.Empty);
			_calculationDirty = false;
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
			return _evaluatedInactiveCategories.Contains(categoryNamespace);
		}

		public bool GetRawNamespaceInactive(string categoryNamespace)
		{
			if (_explicitStates.TryGetValue(categoryNamespace, out var active))
			{
				return !active;
			}
			return false;
		}

		public bool GetCategoryInactive(PathingCategory category)
		{
			if (_explicitStates.TryGetValue(category.Namespace, out var explicitActive))
			{
				return !explicitActive;
			}
			return !category.DefaultToggle;
		}

		public void SetInactive(PathingCategory category, bool isInactive)
		{
			bool targetActiveState = !isInactive;
			if (category.DefaultToggle == targetActiveState)
			{
				_explicitStates.TryRemove(category.Namespace, out var _);
			}
			else
			{
				_explicitStates[category.Namespace] = targetActiveState;
			}
			this.CategoryInactiveChanged?.Invoke(this, new PathingCategoryEventArgs(category)
			{
				Active = targetActiveState
			});
			_stateDirty = true;
			_calculationDirty = true;
		}

		public void SetInactive(string categoryNamespace, bool isInactive)
		{
			if (_rootPackState?.RootCategory != null && _rootPackState.RootCategory.TryGetCategoryFromNamespace(categoryNamespace, out var liveCategory))
			{
				SetInactive(liveCategory, isInactive);
				return;
			}
			_explicitStates[categoryNamespace] = !isInactive;
			_stateDirty = true;
			_calculationDirty = true;
		}

		public void TriggerOpenCategory(PathingCategory category)
		{
			this.TriggerOpenCategoryView?.Invoke(this, new PathingCategoryEventArgs(category));
		}
	}
}
