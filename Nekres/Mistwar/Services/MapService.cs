using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;
using Nekres.Mistwar.UI.Controls;

namespace Nekres.Mistwar.Services
{
	internal class MapService : IDisposable
	{
		private float _opacity;

		private float _colorIntensity;

		private readonly IProgress<string> _loadingIndicator;

		private Dictionary<int, AsyncTexture2D> _mapCache;

		private DirectoriesManager _dir;

		private WvwService _wvw;

		private MapImage _mapControl;

		private StandardWindow _window;

		private const int PADDING_RIGHT = 5;

		public float Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				_opacity = value;
				_mapControl?.SetOpacity(value);
			}
		}

		public float ColorIntensity
		{
			get
			{
				return _colorIntensity;
			}
			set
			{
				_colorIntensity = value;
				_mapControl?.SetColorIntensity(value);
			}
		}

		public bool IsLoading { get; private set; }

		public bool IsReady { get; private set; }

		public MapService(DirectoriesManager dir, WvwService wvw, IProgress<string> loadingIndicator)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			_dir = dir;
			_wvw = wvw;
			_loadingIndicator = loadingIndicator;
			_mapCache = new Dictionary<int, AsyncTexture2D>();
			StandardWindow val = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title(string.Empty);
			((WindowBase2)val).set_Emblem(MistwarModule.ModuleInstance.CornerTex);
			((WindowBase2)val).set_Subtitle(((Module)MistwarModule.ModuleInstance).get_Name());
			((WindowBase2)val).set_Id("Mistwar_Map_86a367fa-61ba-4bab-ae3b-fb08b407214a");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_SavesSize(true);
			((WindowBase2)val).set_CanResize(true);
			((Control)val).set_Width(800);
			((Control)val).set_Height(800);
			((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 800) / 2);
			((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 800) / 2);
			_window = val;
			MapImage mapImage = new MapImage();
			((Control)mapImage).set_Parent((Container)(object)_window);
			((Control)mapImage).set_Width(((Container)_window).get_ContentRegion().Width - 5);
			((Control)mapImage).set_Height(((Container)_window).get_ContentRegion().Height - 5);
			((Control)mapImage).set_Left(0);
			((Control)mapImage).set_Top(0);
			_mapControl = mapImage;
			((Container)_window).add_ContentResized((EventHandler<RegionChangedEventArgs>)OnWindowResized);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		private void OnWindowResized(object sender, RegionChangedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			MapImage mapControl = _mapControl;
			Rectangle currentRegion = e.get_CurrentRegion();
			int num = ((Rectangle)(ref currentRegion)).get_Size().X - 5;
			currentRegion = e.get_CurrentRegion();
			((Control)mapControl).set_Size(new Point(num, ((Rectangle)(ref currentRegion)).get_Size().Y - 5));
		}

		public void DownloadMaps(int[] mapIds)
		{
			if (!IsReady && !IsLoading && !mapIds.IsNullOrEmpty())
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					LoadMapsInBackground(mapIds);
				});
				thread.IsBackground = true;
				thread.Start();
			}
		}

		private void LoadMapsInBackground(int[] mapIds)
		{
			IsLoading = true;
			foreach (int id in mapIds)
			{
				DownloadMapImage(id).Wait();
			}
			_loadingIndicator.Report(null);
			IsLoading = false;
		}

		private async Task DownloadMapImage(int id)
		{
			AsyncTexture2D tex;
			lock (_mapCache)
			{
				if (!_mapCache.TryGetValue(id, out tex))
				{
					tex = new AsyncTexture2D();
					_mapCache.Add(id, tex);
				}
			}
			string filePath = Path.Combine(_dir.GetFullDirectoryPath("mistwar"), $"{id}.png");
			IsReady = LoadFromCache(filePath, tex);
			if (IsReady)
			{
				await ReloadMap();
				return;
			}
			Map map = await MapUtil.GetMap(id);
			if (map != null)
			{
				await MapUtil.BuildMap(map, filePath, removeBackground: true, _loadingIndicator);
				IsReady = LoadFromCache(filePath, tex);
				if (IsReady)
				{
					await ReloadMap();
				}
			}
		}

		private bool LoadFromCache(string filePath, AsyncTexture2D cacheTex, int retries = 2, int delayMs = 2000)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using MemoryStream fil = new MemoryStream(File.ReadAllBytes(filePath));
				GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					Texture2D tex = Texture2D.FromStream(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), (Stream)fil);
					cacheTex.SwapTexture(tex);
					return true;
				}
				finally
				{
					((GraphicsDeviceContext)(ref gdc)).Dispose();
				}
			}
			catch (Exception e)
			{
				if (retries > 0)
				{
					MistwarModule.Logger.Warn(e, $"Failed to load map images from disk. Retrying in {delayMs / 1000} second(s) (remaining retries: {retries}).");
					Task.Delay(delayMs).Wait();
					LoadFromCache(filePath, cacheTex, retries - 1, delayMs);
				}
				MistwarModule.Logger.Warn(e, "After multiple attempts '" + filePath + "' could not be loaded.");
				return false;
			}
		}

		public async Task ReloadMap()
		{
			if (!GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
			{
				return;
			}
			AsyncTexture2D tex;
			lock (_mapCache)
			{
				if (!_mapCache.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out tex) || tex == null)
				{
					return;
				}
			}
			_mapControl.Texture.SwapTexture(AsyncTexture2D.op_Implicit(tex));
			ContinentFloorRegionMap map = await GetMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			_mapControl.Map = map;
			((WindowBase2)_window).set_Title(((map != null) ? map.get_Name() : null) ?? string.Empty);
			List<WvwObjectiveEntity> wvwObjectives = await _wvw.GetObjectives(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			if (!wvwObjectives.IsNullOrEmpty())
			{
				_mapControl.WvwObjectives = wvwObjectives;
				MistwarModule.ModuleInstance?.MarkerService?.ReloadMarkers(wvwObjectives);
			}
		}

		private async Task<ContinentFloorRegionMap> GetMap(int mapId)
		{
			Map obj = await MapUtil.GetMap(mapId);
			return await MapUtil.GetMapExpanded(obj, obj.get_DefaultFloor());
		}

		public void Toggle(bool forceHide = false)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (forceHide)
			{
				((Control)_window).Hide();
			}
			else if (!IsReady)
			{
				ScreenNotification.ShowNotification("(" + ((Module)MistwarModule.ModuleInstance).get_Name() + ") Map images are being prepared...", (NotificationType)2, (Texture2D)null, 4);
			}
			else if (GameUtil.IsAvailable() && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
			{
				((WindowBase2)_window).ToggleWindow();
			}
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Toggle(forceHide: true);
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Toggle(forceHide: true);
			}
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			await ReloadMap();
		}

		public void Dispose()
		{
			((Container)_window).remove_ContentResized((EventHandler<RegionChangedEventArgs>)OnWindowResized);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			((Control)_window).Dispose();
			foreach (AsyncTexture2D value in _mapCache.Values)
			{
				if (value != null)
				{
					value.Dispose();
				}
			}
		}
	}
}
