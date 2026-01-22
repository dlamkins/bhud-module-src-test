using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NpcFinder.Controls;
using NpcFinder.Models;
using NpcFinder.Services;
using NpcFinder.Util;

namespace NpcFinder
{
	[Export(typeof(Module))]
	public class NpcFinderModule : Module
	{
		internal static NpcFinderModule ExampleModuleInstance;

		private static readonly bool DEBUG_LOGS = false;

		private ChangelogWindow _changelogWindow;

		private const string CHANGELOG_TEXT = "v1.1.0\r\n- Huge performance improvements\r\n- Much better precision (works for most of the NPCs now)\r\n- Added Suggestions panel\r\n- Added a marker that displays on the corner if it's off-screen\r\n- Stopped it from opening by itself.\r\n- Added changelog window\r\n- Improved NPC title suggestions (prefix + search + scoring)\r\n- Anchors fallback restored when no coordinates are parsed\r\n- Better caching system\r\n- UI improvements\r\n\r\n! Some NPCs may take a bit longer to resolve the position the first time \r\n(due to caching) -> be patient (around max 2-3 minutes)\r\n\r\n** For the next version (v1.2.0) I'm planning to add a feature to search by MAP \r\nand to display all the NPCs on that map ** \r\n\r\n** Also I will try to fix the small offset of the marker when moving the map \r\nin the next version **\r\n";

		private static readonly Logger Logger = Logger.GetLogger<NpcFinderModule>();

		private string _cacheDirPath;

		private string _merchantCacheDirPath;

		private Texture2D _cornerIconTexture;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _contextMenuStrip;

		private CancellationTokenSource _cts;

		private CacheStore _cache;

		private RateLimiter _rate;

		private WikiNpcService _wiki;

		private Gw2MapIndexService _mapIndex;

		private Gw2ApiService _gw2;

		private Gw2MapDetailsService _details;

		private NpcMerchantResolverService _merchantResolver;

		private NpcFinderWindow _npcWindow;

		private BigMapOverlayControl _bigMapOverlay;

		private NpcTarget _currentTarget;

		private int _currentContinentId;

		private int _lastMapId;

		private double _pollMs;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public NpcFinderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ExampleModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		private void ClearCurrentMarker()
		{
			_currentTarget = null;
			BigMapOverlayControl bigMapOverlay = _bigMapOverlay;
			if (bigMapOverlay != null)
			{
				((Control)bigMapOverlay).Invalidate();
			}
			if (DEBUG_LOGS)
			{
				Logger.Warn("[Target] CLEARED (currentTarget=null)");
			}
		}

		private void DeleteAllNpcFinderCache()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(_cacheDirPath))
				{
					Logger.Warn("[Cache] Delete requested but _cacheDirPath is null/empty.");
					return;
				}
				if (!string.Equals(Path.GetFileName(_cacheDirPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)), "NpcFinderCache", StringComparison.OrdinalIgnoreCase))
				{
					Logger.Warn("[Cache] Refusing to delete unexpected folder: " + _cacheDirPath);
					return;
				}
				if (Directory.Exists(_cacheDirPath))
				{
					Directory.Delete(_cacheDirPath, recursive: true);
				}
				Directory.CreateDirectory(_cacheDirPath);
				if (!string.IsNullOrWhiteSpace(_merchantCacheDirPath))
				{
					Directory.CreateDirectory(_merchantCacheDirPath);
				}
				if (DEBUG_LOGS)
				{
					Logger.Warn("[Cache] Deleted and recreated: " + _cacheDirPath + " (merchant=" + (_merchantCacheDirPath ?? "null") + ")");
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Exception [Cache] Delete failed: " + ex);
			}
		}

		private void EnsureChangelogWindow()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (_changelogWindow == null)
			{
				AsyncTexture2D bg = AsyncTexture2D.FromAssetId(155997);
				ChangelogWindow changelogWindow = new ChangelogWindow(bg, "v1.1.0\r\n- Huge performance improvements\r\n- Much better precision (works for most of the NPCs now)\r\n- Added Suggestions panel\r\n- Added a marker that displays on the corner if it's off-screen\r\n- Stopped it from opening by itself.\r\n- Added changelog window\r\n- Improved NPC title suggestions (prefix + search + scoring)\r\n- Anchors fallback restored when no coordinates are parsed\r\n- Better caching system\r\n- UI improvements\r\n\r\n! Some NPCs may take a bit longer to resolve the position the first time \r\n(due to caching) -> be patient (around max 2-3 minutes)\r\n\r\n** For the next version (v1.2.0) I'm planning to add a feature to search by MAP \r\nand to display all the NPCs on that map ** \r\n\r\n** Also I will try to fix the small offset of the marker when moving the map \r\nin the next version **\r\n");
				((Control)changelogWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)changelogWindow).set_Location(new Point(340, 240));
				((WindowBase2)changelogWindow).set_Id("NpcFinderModule_ChangelogWindow");
				((WindowBase2)changelogWindow).set_SavesPosition(true);
				_changelogWindow = changelogWindow;
			}
		}

		protected override async Task LoadAsync()
		{
			MumbleReader.ResetDiscovery();
			_cornerIconTexture = ContentsManager.GetTexture("assets/cornerIconTexture.png");
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			_cts = new CancellationTokenSource();
			string rootDir = null;
			foreach (string d in DirectoriesManager.get_RegisteredDirectories())
			{
				string p = DirectoriesManager.GetFullDirectoryPath(d);
				if (!string.IsNullOrWhiteSpace(p))
				{
					rootDir = p;
					break;
				}
			}
			_cacheDirPath = Path.Combine(rootDir ?? Path.GetTempPath(), "NpcFinderCache");
			_cache = new CacheStore(_cacheDirPath);
			if (DEBUG_LOGS)
			{
				Logger.Info("[Cache] rootDir='" + (rootDir ?? "(null)") + "'");
				Logger.Info("[Cache] cachePath='" + Path.Combine(rootDir ?? Path.GetTempPath(), "NpcFinderCache") + "'");
			}
			_rate = new RateLimiter(250);
			_wiki = new WikiNpcService(_rate, _cache);
			_mapIndex = new Gw2MapIndexService(Gw2ApiManager.get_Gw2ApiClient().get_V2(), _cache);
			_gw2 = new Gw2ApiService(Gw2ApiManager.get_Gw2ApiClient().get_V2(), _cache);
			_details = new Gw2MapDetailsService(_cache);
			_merchantCacheDirPath = Path.Combine(_cacheDirPath, "merchant");
			try
			{
				Directory.CreateDirectory(_merchantCacheDirPath);
			}
			catch
			{
			}
			_merchantResolver = new NpcMerchantResolverService(_wiki, _mapIndex, _gw2, _details, _merchantCacheDirPath);
			NpcFinderModule npcFinderModule = this;
			NpcFinderWindow npcFinderWindow = new NpcFinderWindow(windowBackgroundTexture, _wiki, _mapIndex, _gw2, _merchantResolver, _cts, () => _currentContinentId, delegate(NpcTarget t)
			{
				_currentTarget = t;
				BigMapOverlayControl bigMapOverlay = _bigMapOverlay;
				if (bigMapOverlay != null)
				{
					((Control)bigMapOverlay).Invalidate();
				}
				if (DEBUG_LOGS)
				{
					Logger.Warn("[Target] SET: " + ((t == null) ? "null" : $"{t.MapName} cont={t.TargetContinentId} cx={t.TargetContinentX} cy={t.TargetContinentY}"));
				}
			}, delegate
			{
				ClearCurrentMarker();
			}, delegate
			{
				DeleteAllNpcFinderCache();
			});
			((Control)npcFinderWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)npcFinderWindow).set_Location(new Point(300, 300));
			((WindowBase2)npcFinderWindow).set_Id("NpcFinderModule_NpcFinderWindow");
			((WindowBase2)npcFinderWindow).set_SavesPosition(true);
			npcFinderModule._npcWindow = npcFinderWindow;
			NpcFinderModule npcFinderModule2 = this;
			BigMapOverlayControl bigMapOverlayControl = new BigMapOverlayControl();
			((Control)bigMapOverlayControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)bigMapOverlayControl).set_Location(new Point(0, 0));
			((Control)bigMapOverlayControl).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			((Control)bigMapOverlayControl).set_Visible(true);
			((Control)bigMapOverlayControl).set_ZIndex(int.MaxValue);
			((Control)bigMapOverlayControl).set_ClipsBounds(false);
			bigMapOverlayControl.TargetProvider = () => _currentTarget;
			bigMapOverlayControl.CurrentContinentIdProvider = () => _currentContinentId;
			npcFinderModule2._bigMapOverlay = bigMapOverlayControl;
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_bigMapOverlay).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			});
			CreateCornerIconWithContextMenu();
			await Task.CompletedTask;
		}

		protected override void Update(GameTime gameTime)
		{
			((Module)this).Update(gameTime);
			_pollMs += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_pollMs < 1000.0)
			{
				return;
			}
			_pollMs = 0.0;
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				BigMapOverlayControl bigMapOverlay = _bigMapOverlay;
				if (bigMapOverlay != null)
				{
					((Control)bigMapOverlay).Invalidate();
				}
			}
			MumbleReader.DumpUiOncePerSecond();
			if (!MumbleReader.TryGetMapId(out var mapId) || mapId == _lastMapId)
			{
				return;
			}
			_lastMapId = mapId;
			Task.Run(async delegate
			{
				try
				{
					Gw2MapInfo mi = await _gw2.GetMapInfoAsync(mapId, _cts.Token).ConfigureAwait(continueOnCapturedContext: false);
					_currentContinentId = mi?.ContinentId ?? 0;
				}
				catch
				{
					_currentContinentId = 0;
				}
			});
		}

		protected override void Unload()
		{
			try
			{
				_cts?.Cancel();
			}
			catch
			{
			}
			NpcFinderWindow npcWindow = _npcWindow;
			if (npcWindow != null)
			{
				((Control)npcWindow).Dispose();
			}
			ChangelogWindow changelogWindow = _changelogWindow;
			if (changelogWindow != null)
			{
				((Control)changelogWindow).Dispose();
			}
			BigMapOverlayControl bigMapOverlay = _bigMapOverlay;
			if (bigMapOverlay != null)
			{
				((Control)bigMapOverlay).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			ContextMenuStrip contextMenuStrip = _contextMenuStrip;
			if (contextMenuStrip != null)
			{
				((Control)contextMenuStrip).Dispose();
			}
			Texture2D cornerIconTexture = _cornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			try
			{
				_cts?.Dispose();
			}
			catch
			{
			}
			ExampleModuleInstance = null;
		}

		private void CreateCornerIconWithContextMenu()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			((Control)val).set_BasicTooltipText("NPC Finder");
			val.set_Priority(1645843523);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_npcWindow != null)
				{
					if (!((Control)_npcWindow).get_Visible())
					{
						((Control)_npcWindow).Show();
					}
					else
					{
						((WindowBase2)_npcWindow).ToggleWindow();
					}
				}
			});
			_contextMenuStrip = new ContextMenuStrip();
			((Control)_contextMenuStrip.AddMenuItem("Changelog / Patch notes")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					EnsureChangelogWindow();
					if (((Control)_changelogWindow).get_Parent() == null)
					{
						((Control)_changelogWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
					}
					if (!((Control)_changelogWindow).get_Visible())
					{
						((Control)_changelogWindow).Show();
					}
					else
					{
						((WindowBase2)_changelogWindow).ToggleWindow();
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("[Changelog] Failed to open: " + ex);
				}
			});
			((Control)_cornerIcon).set_Menu(_contextMenuStrip);
		}
	}
}
