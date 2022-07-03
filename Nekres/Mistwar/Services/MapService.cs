using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.UI.Controls;

namespace Nekres.Mistwar.Services
{
	internal class MapService : IDisposable
	{
		private DirectoriesManager _dir;

		private Gw2ApiManager _api;

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

		public bool IsLoading { get; private set; }

		public string Log { get; private set; }

		public MapService(Gw2ApiManager api, DirectoriesManager dir, WvwService wvw, IProgress<string> loadingIndicator)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			_dir = dir;
			_api = api;
			_wvw = wvw;
			_loadingIndicator = loadingIndicator;
			_mapCache = new Dictionary<int, AsyncTexture2D>();
			MapImage mapImage = new MapImage();
			((Control)mapImage).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)mapImage).set_Size(new Point(0, 0));
			((Control)mapImage).set_Location(new Point(0, 0));
			_mapControl = mapImage;
			((Control)_mapControl).Hide();
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
				Task<Task> task = new Task<Task>(async delegate
				{
					await DownloadMapImage(id);
				});
				task.Start();
				task.Unwrap().Wait();
			}
			IsLoading = false;
			Log = null;
			_loadingIndicator.Report(null);
		}

		private async Task DownloadMapImage(int id)
		{
			if (!_mapCache.TryGetValue(id, out var cacheTex))
			{
				cacheTex = new AsyncTexture2D();
				_mapCache.Add(id, cacheTex);
			}
			await ReloadMap();
			string filePath = string.Format("{0}/{1}.png", _dir.GetFullDirectoryPath("mistwar"), id);
			if (File.Exists(filePath))
			{
				using (MemoryStream fil = new MemoryStream(File.ReadAllBytes(filePath)))
				{
					Texture2D tex = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)fil);
					cacheTex.SwapTexture(tex);
				}
				return;
			}
			await MapUtil.BuildMap(await MapUtil.RequestMap(id), filePath, removeBackground: true, _loadingIndicator).ContinueWith((Func<Task, Task>)async delegate
			{
				using MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(filePath));
				Texture2D tex2 = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
				cacheTex.SwapTexture(tex2);
			});
		}

		public async Task ReloadMap()
		{
			((Control)_mapControl).Hide();
			((Control)_mapControl).set_Enabled(false);
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld() && _mapCache.TryGetValue(GameService.Gw2Mumble.get_CurrentMap().get_Id(), out var tex))
			{
				_mapControl.Texture = tex;
				MapImage mapControl = _mapControl;
				mapControl.WvwObjectives = await _wvw.GetObjectives(GameService.Gw2Mumble.get_CurrentMap().get_Id());
				((Control)_mapControl).set_Enabled(true);
			}
		}

		public void Toggle()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (IsLoading)
			{
				ScreenNotification.ShowNotification("Mistwar is initializing.", (NotificationType)2, (Texture2D)null, 4);
			}
			else if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				_mapControl?.Toggle();
			}
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				((Control)_mapControl).Hide();
				((Control)_mapControl).set_Enabled(false);
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				((Control)_mapControl).Hide();
				((Control)_mapControl).set_Enabled(false);
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
		}
	}
}
