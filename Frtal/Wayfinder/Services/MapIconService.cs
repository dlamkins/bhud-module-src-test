using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Frtal.Wayfinder.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder.Services
{
	public class MapIconService
	{
		private static readonly Logger Logger = Logger.GetLogger<MapIconService>();

		private readonly Gw2ApiManager _api;

		private readonly Dictionary<TargetKind, AsyncTexture2D> _icons = new Dictionary<TargetKind, AsyncTexture2D>();

		private readonly Dictionary<TargetKind, Texture2D> _mono = new Dictionary<TargetKind, Texture2D>();

		private static readonly Dictionary<TargetKind, string> FileIds = new Dictionary<TargetKind, string>
		{
			{
				TargetKind.Waypoint,
				"map_waypoint"
			},
			{
				TargetKind.PointOfInterest,
				"map_poi"
			},
			{
				TargetKind.Vista,
				"map_vista"
			},
			{
				TargetKind.Heart,
				"map_heart_full"
			},
			{
				TargetKind.SkillPoint,
				"map_heropoint"
			}
		};

		public MapIconService(Gw2ApiManager api)
		{
			_api = api;
		}

		public AsyncTexture2D For(TargetKind kind)
		{
			if (!_icons.TryGetValue(kind, out var tex))
			{
				return null;
			}
			return tex;
		}

		public Texture2D TextureFor(TargetKind kind, bool monochrome)
		{
			if (monochrome && _mono.TryGetValue(kind, out var mono))
			{
				return mono;
			}
			AsyncTexture2D obj = For(kind);
			if (obj == null)
			{
				return null;
			}
			return obj.get_Texture();
		}

		public void EnsureMonochrome()
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			if (_mono.Count >= _icons.Count)
			{
				return;
			}
			foreach (KeyValuePair<TargetKind, AsyncTexture2D> kv in _icons)
			{
				if (_mono.ContainsKey(kv.Key))
				{
					continue;
				}
				AsyncTexture2D value = kv.Value;
				Texture2D src = ((value != null) ? value.get_Texture() : null);
				if (src == null || src.get_Width() <= 0 || src.get_Height() <= 0)
				{
					continue;
				}
				try
				{
					Color[] data = (Color[])(object)new Color[src.get_Width() * src.get_Height()];
					src.GetData<Color>(data);
					for (int i = 0; i < data.Length; i++)
					{
						Color c = data[i];
						byte j = (byte)((((Color)(ref c)).get_R() * 299 + ((Color)(ref c)).get_G() * 587 + ((Color)(ref c)).get_B() * 114) / 1000);
						data[i] = new Color(j, j, j, ((Color)(ref c)).get_A());
					}
					GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
					Texture2D tex;
					try
					{
						tex = new Texture2D(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), src.get_Width(), src.get_Height(), false, (SurfaceFormat)0);
					}
					finally
					{
						((GraphicsDeviceContext)(ref ctx)).Dispose();
					}
					tex.SetData<Color>(data);
					_mono[kv.Key] = tex;
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, $"Failed to build the monochrome icon for {kv.Key}.");
				}
			}
		}

		public async Task LoadAsync()
		{
			foreach (KeyValuePair<TargetKind, string> kv in FileIds)
			{
				try
				{
					File file = await ((IBulkExpandableClient<File, string>)(object)_api.get_Gw2ApiClient().get_V2().get_Files()).GetAsync(kv.Value, default(CancellationToken));
					if (file != null)
					{
						file.get_Icon();
						_icons[kv.Key] = GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(file.get_Icon()));
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load icon '" + kv.Value + "'.");
				}
			}
			Logger.Info($"Loaded {_icons.Count} map icons.");
		}

		public void Dispose()
		{
			foreach (Texture2D value in _mono.Values)
			{
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			_mono.Clear();
		}
	}
}
