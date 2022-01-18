using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Controls;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Reader;

namespace BhModule.Community.Pathing
{
	public class PackInitiator : IUpdatable
	{
		private static readonly Logger Logger = Logger.GetLogger<PackInitiator>();

		private readonly string _watchPath;

		private readonly ModuleSettings _moduleSettings;

		private readonly IProgress<string> _loadingIndicator;

		private readonly IRootPackState _packState;

		private readonly SafeList<Pack> _packs = new SafeList<Pack>();

		private SharedPackCollection _sharedPackCollection;

		private readonly PackReaderSettings _packReaderSettings;

		private bool _isLoading;

		private int _lastMap = -1;

		public PackInitiator(string watchPath, ModuleSettings moduleSettings, IProgress<string> loadingIndicator)
		{
			_watchPath = watchPath;
			_moduleSettings = moduleSettings;
			_loadingIndicator = loadingIndicator;
			_packReaderSettings = new PackReaderSettings();
			_packReaderSettings.VenderPrefixes.Add("bh-");
			_packState = new SharedPackState(moduleSettings);
		}

		public IEnumerable<ContextMenuStripItem> GetPackMenuItems()
		{
			bool isAnyMarkers = !_isLoading && _sharedPackCollection != null && _sharedPackCollection.Categories != null && _sharedPackCollection.Categories.Any((PathingCategory category) => !string.IsNullOrWhiteSpace(category.DisplayName));
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text("All Markers");
			val.set_CanCheck(true);
			val.set_Checked(_moduleSettings.GlobalPathablesEnabled.get_Value());
			val.set_Submenu((ContextMenuStrip)(object)(isAnyMarkers ? new CategoryContextMenuStrip(_packState, _sharedPackCollection.Categories, forceShowAll: false) : null));
			ContextMenuStripItem allMarkers = val;
			allMarkers.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				_moduleSettings.GlobalPathablesEnabled.set_Value(e.get_Checked());
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem();
			val2.set_Text("Reload Markers");
			((Control)val2).set_Enabled(!_isLoading && _packState.CurrentMapId > 0);
			ContextMenuStripItem reloadMarkers = val2;
			((Control)reloadMarkers).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_packState.CurrentMapId >= 0)
				{
					_lastMap = -1;
					LoadMapFromEachPackInBackground(_packState.CurrentMapId);
				}
			});
			ContextMenuStripItem val3 = new ContextMenuStripItem();
			val3.set_Text("Unload Markers");
			((Control)val3).set_Enabled(!_isLoading && _packState.CurrentMapId > 0);
			ContextMenuStripItem unloadMarkers = val3;
			((Control)unloadMarkers).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (_packState.CurrentMapId >= 0)
				{
					await UnloadStateAndCollection();
				}
			});
			yield return allMarkers;
			yield return reloadMarkers;
			yield return unloadMarkers;
		}

		public async Task Init()
		{
			await _packState.Load();
			await LoadAllPacks();
		}

		private async Task LoadUnpackedPackFiles(string unpackedDir)
		{
			Pack newPack = Pack.FromDirectoryMarkerPack(unpackedDir);
			_packs.Add(newPack);
		}

		private async Task LoadPackedPackFiles(IEnumerable<string> zipPackFiles)
		{
			foreach (Pack newPack in zipPackFiles.Select(Pack.FromArchivedMarkerPack))
			{
				_packs.Add(newPack);
			}
		}

		private async Task UnloadStateAndCollection()
		{
			_sharedPackCollection?.Unload();
			await _packState.Unload();
		}

		private async Task LoadAllPacks()
		{
			foreach (string markerDir in (_packState.UserResourceStates.Advanced.MarkerLoadPaths ?? Array.Empty<string>()).Concat(new string[1] { _watchPath }))
			{
				await LoadPackedPackFiles(Directory.GetFiles(markerDir, "*.zip", SearchOption.AllDirectories));
				await LoadPackedPackFiles(Directory.GetFiles(markerDir, "*.taco", SearchOption.AllDirectories));
				await LoadUnpackedPackFiles(markerDir);
			}
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() != 0)
			{
				PackInitiator packInitiator = this;
				int mapId = (_packState.CurrentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id());
				packInitiator.LoadMapFromEachPackInBackground(mapId);
			}
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
		}

		private async Task PrepareState(int mapId)
		{
			await UnloadStateAndCollection();
			_sharedPackCollection = new SharedPackCollection();
		}

		private void LoadMapFromEachPackInBackground(int mapId)
		{
			lock (this)
			{
				if (mapId == _lastMap)
				{
					return;
				}
				_lastMap = mapId;
			}
			Thread thread = new Thread((ThreadStart)async delegate
			{
				await LoadMapFromEachPack(mapId);
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private async Task LoadMapFromEachPack(int mapId)
		{
			_isLoading = true;
			Stopwatch loadTimer = Stopwatch.StartNew();
			_loadingIndicator.Report("Loading marker packs...");
			await PrepareState(mapId);
			Pack lastPack = null;
			try
			{
				Pack[] array = _packs.ToArray();
				foreach (Pack pack2 in array)
				{
					lastPack = pack2;
					_loadingIndicator.Report("Loading " + pack2.Name + "...");
					await pack2.LoadMapAsync(mapId, _sharedPackCollection, _packReaderSettings);
				}
			}
			catch (FileNotFoundException e2)
			{
				Logger.Warn("Pack file '{packPath}' failed to load because it could not be found.", new object[1] { e2.FileName });
				if (lastPack != null)
				{
					_packs.Remove(lastPack);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Loading pack failed.");
				if (lastPack != null)
				{
					_packs.Remove(lastPack);
				}
			}
			_loadingIndicator.Report("Finalizing marker collection...");
			await _packState.LoadPackCollection(_sharedPackCollection);
			Pack[] array2 = _packs.ToArray();
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].ReleaseLocks();
			}
			_loadingIndicator.Report("");
			_isLoading = false;
			loadTimer.Stop();
			Logger.Info(string.Format("Finished loading packs {0} in {1} ms for map {2}.", string.Join(", ", _packs.Select((Pack pack) => pack.Name)), loadTimer.ElapsedMilliseconds, mapId));
		}

		private void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			if (e.get_Value() != _packState.CurrentMapId)
			{
				_packState.CurrentMapId = e.get_Value();
				LoadMapFromEachPackInBackground(e.get_Value());
			}
		}

		public void Update(GameTime gameTime)
		{
			((IUpdatable)_packState).Update(gameTime);
		}

		public void Unload()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			_packState.Unload();
		}
	}
}
