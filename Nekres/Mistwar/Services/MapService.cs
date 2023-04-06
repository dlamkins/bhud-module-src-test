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
		private DirectoriesManager _dir;

		private WvwService _wvw;

		private MapImage _mapControl;

		private float _opacity;

		private float _colorIntensity;

		private readonly IProgress<string> _loadingIndicator;

		private Dictionary<int, AsyncTexture2D> _mapCache;

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

		public bool IsVisible
		{
			get
			{
				MapImage mapControl = _mapControl;
				if (mapControl == null)
				{
					return false;
				}
				return ((Control)mapControl).get_Visible();
			}
		}

		public bool IsLoading { get; private set; }

		public MapService(DirectoriesManager dir, WvwService wvw, IProgress<string> loadingIndicator)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			_dir = dir;
			_wvw = wvw;
			_loadingIndicator = loadingIndicator;
			_mapCache = new Dictionary<int, AsyncTexture2D>();
			MapImage mapImage = new MapImage();
			((Control)mapImage).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)mapImage).set_Size(new Point(0, 0));
			((Control)mapImage).set_Location(new Point(0, 0));
			((Control)mapImage).set_Visible(false);
			_mapControl = mapImage;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
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
			IsLoading = true;
			foreach (int id in mapIds)
			{
				DownloadMapImage(id).Wait();
			}
			IsLoading = false;
			_loadingIndicator.Report(null);
		}

		private async Task DownloadMapImage(int id)
		{
			if (!_mapCache.TryGetValue(id, out var cacheTex))
			{
				cacheTex = new AsyncTexture2D();
				_mapCache.Add(id, cacheTex);
			}
			string filePath = string.Format("{0}/{1}.png", _dir.GetFullDirectoryPath("mistwar"), id);
			if (LoadFromCache(filePath, cacheTex))
			{
				await ReloadMap();
				return;
			}
			Map map = await MapUtil.GetMap(id);
			if (map != null)
			{
				await MapUtil.BuildMap(map, filePath, removeBackground: true, _loadingIndicator);
				if (LoadFromCache(filePath, cacheTex))
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
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() && _mapCache.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out var tex) && tex != null)
			{
				_mapControl.Texture.SwapTexture(AsyncTexture2D.op_Implicit(tex));
				MapImage mapControl = _mapControl;
				mapControl.Map = await GetMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
				List<WvwObjectiveEntity> wvwObjectives = await _wvw.GetObjectives(GameService.Gw2Mumble.get_CurrentMap().get_Id());
				if (!wvwObjectives.IsNullOrEmpty())
				{
					_mapControl.WvwObjectives = wvwObjectives;
					MistwarModule.ModuleInstance?.MarkerService?.ReloadMarkers(wvwObjectives);
				}
			}
		}

		private async Task<ContinentFloorRegionMap> GetMap(int mapId)
		{
			Map obj = await MapUtil.GetMap(mapId);
			return await MapUtil.GetMapExpanded(obj, obj.get_DefaultFloor());
		}

		public void Toggle(bool forceHide = false, bool silent = false)
		{
			if (IsLoading)
			{
				ScreenNotification.ShowNotification("(" + ((Module)MistwarModule.ModuleInstance).get_Name() + ") Map images are being prepared...", (NotificationType)2, (Texture2D)null, 4);
			}
			else
			{
				_mapControl?.Toggle(forceHide, silent);
			}
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Toggle(forceHide: true, silent: true);
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Toggle(forceHide: true, silent: true);
			}
		}

		private async void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			await ReloadMap();
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			MapImage mapControl = _mapControl;
			if (mapControl != null)
			{
				((Control)mapControl).Dispose();
			}
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
