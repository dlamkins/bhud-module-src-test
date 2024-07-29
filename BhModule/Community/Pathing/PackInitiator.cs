using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Controls;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
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

		private readonly PathingModule _module;

		private readonly IProgress<string> _loadingIndicator;

		private readonly IRootPackState _packState;

		private readonly SafeList<PackWrapper> _packs = new SafeList<PackWrapper>();

		private SharedPackCollection _sharedPackCollection;

		private readonly PackReaderSettings _packReaderSettings;

		private SettingCollection _packToggleSettings;

		private int _lastMap = -1;

		public bool IsLoading { get; private set; }

		public IRootPackState PackState => _packState;

		public PackInitiator(string watchPath, PathingModule module, IProgress<string> loadingIndicator)
		{
			_watchPath = watchPath;
			_module = module;
			_loadingIndicator = loadingIndicator;
			_packToggleSettings = _module.SettingsManager.get_ModuleSettings().AddSubCollection("PackToggleSettings", false);
			_packReaderSettings = new PackReaderSettings();
			_packReaderSettings.VenderPrefixes.Add("bh-");
			_packState = new SharedPackState(_module);
		}

		public void ReloadPacks()
		{
			if (_packState.CurrentMapId >= 0 && !IsLoading)
			{
				_lastMap = -1;
				LoadMapFromEachPackInBackground(_packState.CurrentMapId);
			}
		}

		private IEnumerable<ContextMenuStripItem> GetPackLoadSettings(PackWrapper pack)
		{
			yield return (ContextMenuStripItem)(object)new SimpleContextMenuStripItem("Always Load", delegate(bool e)
			{
				pack.AlwaysLoad.set_Value(e);
			}, pack.AlwaysLoad.get_Value());
			if (pack.IsLoaded)
			{
				ContextMenuStripItem val = new ContextMenuStripItem();
				val.set_Text($"Loaded in {pack.LoadTime} milliseconds");
				((Control)val).set_Enabled(false);
				yield return val;
			}
			else
			{
				yield return (ContextMenuStripItem)(object)new SimpleContextMenuStripItem("Load Marker Pack", delegate
				{
					pack.ForceLoad = true;
					ReloadPacks();
				});
			}
			SimpleContextMenuStripItem simpleContextMenuStripItem = new SimpleContextMenuStripItem("Delete Pack", delegate
			{
				if (pack.Package != null)
				{
					PackHandlingUtil.DeletePack(_module, pack.Package);
				}
			});
			((Control)simpleContextMenuStripItem).set_Enabled(pack.Package != null);
			yield return (ContextMenuStripItem)(object)simpleContextMenuStripItem;
		}

		private IEnumerable<ContextMenuStripItem> GetPackToggles()
		{
			PackWrapper[] packs = _packs.ToArray();
			foreach (PackWrapper pack in packs.OrderBy((PackWrapper p) => p.Package?.Name ?? p.Pack.Name))
			{
				if (!(pack.Pack.Name == "markers"))
				{
					string packName = pack.Package?.Name ?? pack.Pack.Name;
					ContextMenuStripItem val = new ContextMenuStripItem();
					val.set_Text(packName);
					val.set_Submenu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => GetPackLoadSettings(pack))));
					yield return val;
				}
			}
			yield return (ContextMenuStripItem)(object)new ContextMenuStripDivider();
			SimpleContextMenuStripItem simpleContextMenuStripItem = new SimpleContextMenuStripItem("Reload Marker Packs", ReloadPacks);
			((Control)simpleContextMenuStripItem).set_Enabled(!IsLoading && _packState.CurrentMapId > 0);
			((Control)simpleContextMenuStripItem).set_BasicTooltipText(((int)_module.Settings.KeyBindReloadMarkerPacks.get_Value().get_PrimaryKey() != 0) ? ("Keybind: " + _module.Settings.KeyBindReloadMarkerPacks.get_Value().GetBindingDisplayText()) : null);
			yield return (ContextMenuStripItem)(object)simpleContextMenuStripItem;
			SimpleContextMenuStripItem simpleContextMenuStripItem2 = new SimpleContextMenuStripItem("Unload Marker Packs", async delegate
			{
				if (_packState.CurrentMapId >= 0)
				{
					await UnloadStateAndCollection();
				}
			});
			((Control)simpleContextMenuStripItem2).set_Enabled(!IsLoading && _packState.CurrentMapId > 0);
			yield return (ContextMenuStripItem)(object)simpleContextMenuStripItem2;
			yield return (ContextMenuStripItem)(object)new SimpleContextMenuStripItem("Download Marker Packs", delegate
			{
				_module.SettingsWindow.set_SelectedTab(_module.MarkerRepoTab);
				((Control)_module.SettingsWindow).Show();
			});
		}

		public IEnumerable<ContextMenuStripItem> GetPackMenuItems()
		{
			bool isAnyMarkers = !IsLoading && _sharedPackCollection != null && _sharedPackCollection.Categories != null && _sharedPackCollection.Categories.Any((PathingCategory category) => !string.IsNullOrWhiteSpace(category.DisplayName));
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text("All Markers");
			val.set_CanCheck(true);
			val.set_Checked(_module.Settings.GlobalPathablesEnabled.get_Value());
			val.set_Submenu((ContextMenuStrip)(object)(isAnyMarkers ? new CategoryContextMenuStrip(_packState, _sharedPackCollection.Categories, forceShowAll: false) : null));
			ContextMenuStripItem allMarkers = val;
			allMarkers.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				_module.Settings.GlobalPathablesEnabled.set_Value(e.get_Checked());
			});
			ContextMenuStripItem val2 = new ContextMenuStripItem();
			val2.set_Text("Manage Marker Packs");
			val2.set_Submenu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetPackToggles));
			ContextMenuStripItem packs = val2;
			if (_module.Settings.ScriptsEnabled.get_Value() && _module.ScriptEngine.Global != null && _module.ScriptEngine.Global.Menu.Menus.Any())
			{
				yield return _module.ScriptEngine.Global.Menu.BuildMenu();
			}
			yield return allMarkers;
			yield return packs;
		}

		public async Task Init()
		{
			await _packState.Load();
			await LoadAllPacks();
		}

		private PackWrapper GetPackWrapper(Pack pack)
		{
			SettingEntry<bool> alwaysLoad = _packToggleSettings.DefineSetting<bool>(pack.Name + "_AlwaysLoad", true, (Func<string>)null, (Func<string>)null);
			return new PackWrapper(_module, pack, alwaysLoad);
		}

		public async Task LoadUnpackedPackFiles(string unpackedDir)
		{
			try
			{
				Pack newPack = Pack.FromDirectoryMarkerPack(unpackedDir);
				_packs.Add(GetPackWrapper(newPack));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Unpacked markers failed to load.");
			}
		}

		public async Task LoadPackedPackFiles(IEnumerable<string> zipPackFiles)
		{
			foreach (string packArchive in zipPackFiles)
			{
				try
				{
					Pack newPack = Pack.FromArchivedMarkerPack(packArchive);
					_packs.Add(GetPackWrapper(newPack));
				}
				catch (InvalidDataException)
				{
					Logger.Warn("Pack " + packArchive + " appears to be corrupt.  Please remove it and download it again.");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Pack " + packArchive + " failed to load.");
				}
			}
		}

		public async Task LoadPack(Pack pack)
		{
			if (pack != null)
			{
				_packs.Add(GetPackWrapper(pack));
			}
		}

		public void UnloadPackByName(string packName)
		{
			PackWrapper[] array = _packs.ToArray();
			foreach (PackWrapper pack in array)
			{
				if (string.Equals(packName, pack.Pack.Name, StringComparison.OrdinalIgnoreCase))
				{
					_packs.Remove(pack);
					break;
				}
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
			IsLoading = true;
			Stopwatch loadTimer = Stopwatch.StartNew();
			_packState.Module.ScriptEngine.Reset();
			List<(Pack Pack, long LoadDuration)> packTimings = new List<(Pack, long)>();
			_loadingIndicator.Report("Loading marker packs...");
			await PrepareState(mapId);
			PackWrapper[] packs = _packs.ToArray();
			PackWrapper[] array = packs;
			foreach (PackWrapper pack in array)
			{
				pack.IsLoaded = false;
				if (pack.AlwaysLoad.get_Value() || pack.ForceLoad)
				{
					try
					{
						Stopwatch packTimer = Stopwatch.StartNew();
						_loadingIndicator.Report("Loading " + pack.Pack.Name + "...");
						await pack.Pack.LoadMapAsync(mapId, _sharedPackCollection, _packReaderSettings);
						pack.IsLoaded = true;
						pack.LoadTime = packTimer.ElapsedMilliseconds;
						packTimings.Add((pack.Pack, pack.LoadTime));
					}
					catch (FileNotFoundException e2)
					{
						Logger.Warn("Pack file '{packPath}' failed to load because it could not be found.", new object[1] { e2.FileName });
						_packs.Remove(pack);
					}
					catch (Exception e)
					{
						Logger.Warn(e, "Loading pack '" + pack.Pack.Name + "' failed.");
					}
				}
			}
			_loadingIndicator.Report("Finalizing marker collection...");
			try
			{
				await _packState.LoadPackCollection(_sharedPackCollection);
			}
			catch (Exception e3)
			{
				Logger.Warn(e3, "Finalizing packs failed.");
				_packState?.Unload();
			}
			if (_packState.Module.Settings.ScriptsEnabled.get_Value())
			{
				_loadingIndicator.Report("Loading scripts...");
				array = packs;
				foreach (PackWrapper pack in array)
				{
					if (pack.AlwaysLoad.get_Value() || pack.ForceLoad)
					{
						Stopwatch packTimer = Stopwatch.StartNew();
						await _packState.Module.ScriptEngine.LoadScript("pack.lua", pack.Pack.ResourceManager, pack.Pack.Name);
						pack.LoadTime += packTimer.ElapsedMilliseconds;
					}
				}
			}
			PackWrapper[] array2 = packs;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Pack.ReleaseLocks();
			}
			_loadingIndicator.Report(null);
			IsLoading = false;
			Logger.Info(string.Format("Finished loading packs {0} in {1}ms for map {2}.", string.Join(", ", packTimings.Select(((Pack Pack, long LoadDuration) p) => string.Format("{0}{1}[{2}ms]", p.Pack.ManifestedPack ? "+" : "-", p.Pack.Name, p.LoadDuration))), loadTimer.ElapsedMilliseconds, mapId));
		}

		private void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			if (e.get_Value() == _packState.CurrentMapId)
			{
				return;
			}
			_packState.CurrentMapId = e.get_Value();
			foreach (PackWrapper pack in _packs)
			{
				pack.ForceLoad = false;
			}
			LoadMapFromEachPackInBackground(e.get_Value());
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
