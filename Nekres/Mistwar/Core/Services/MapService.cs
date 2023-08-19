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
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Core.UI.Controls;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Core.Services
{
	internal class MapService : IDisposable
	{
		private float _opacity;

		private float _colorIntensity;

		private readonly IProgress<string> _loadingIndicator;

		private Dictionary<int, AsyncTexture2D> _mapCache;

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

		public bool IsAvailable { get; private set; }

		public MapService(IProgress<string> loadingIndicator)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			IsLoading = true;
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
			if (!mapIds.IsNullOrEmpty())
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
			string filePath = Path.Combine(MistwarModule.ModuleInstance.DirectoriesManager.GetFullDirectoryPath("mistwar"), $"{id}.png");
			if (LoadFromCache(filePath, tex))
			{
				await ReloadMap();
				return;
			}
			Map map = await MistwarModule.ModuleInstance.Resources.GetMap(id);
			if (map != null)
			{
				await MapUtil.BuildMap(map, filePath, removeBackground: true, _loadingIndicator);
				if (LoadFromCache(filePath, tex))
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
				IsAvailable = false;
				Toggle(forceHide: true);
				return;
			}
			AsyncTexture2D tex;
			lock (_mapCache)
			{
				if (!_mapCache.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out tex) || tex == null)
				{
					IsAvailable = false;
					Toggle(forceHide: true);
					return;
				}
			}
			_mapControl.Texture.SwapTexture(AsyncTexture2D.op_Implicit(tex));
			ContinentFloorRegionMap map = await GetMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			if (map == null)
			{
				IsAvailable = false;
				Toggle(forceHide: true);
				return;
			}
			_mapControl.Map = map;
			((WindowBase2)_window).set_Title(map.get_Name());
			List<WvwObjectiveEntity> wvwObjectives = await MistwarModule.ModuleInstance.WvW.GetObjectives(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			if (wvwObjectives.IsNullOrEmpty())
			{
				IsAvailable = false;
				Toggle(forceHide: true);
			}
			else
			{
				_mapControl.WvwObjectives = wvwObjectives;
				MistwarModule.ModuleInstance?.Markers?.ReloadMarkers(wvwObjectives);
				IsAvailable = true;
			}
		}

		private async Task<ContinentFloorRegionMap> GetMap(int mapId)
		{
			Map map = await MistwarModule.ModuleInstance.Resources.GetMap(mapId);
			if (map == null)
			{
				return null;
			}
			return await MistwarModule.ModuleInstance.Resources.GetMapExpanded(map, map.get_DefaultFloor());
		}

		public void Toggle(bool forceHide = false)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (forceHide)
			{
				((Control)_window).Hide();
			}
			else if (IsLoading || !IsAvailable)
			{
				ScreenNotification.ShowNotification(((Module)MistwarModule.ModuleInstance).get_Name() + " unavailable. Map not loaded.", (NotificationType)2, (Texture2D)null, 4);
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
