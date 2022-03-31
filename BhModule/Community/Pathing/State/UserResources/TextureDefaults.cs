using System;
using System.IO;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class TextureDefaults
	{
		private static readonly Logger Logger = Logger.GetLogger<TextureDefaults>();

		private const string TRAIL_NAME = "defaultTrail.png";

		private const string MARKER_NAME = "defaultMarker.png";

		public AsyncTexture2D DefaultTrailTexture { get; set; }

		public AsyncTexture2D DefaultMarkerTexture { get; set; }

		public TextureDefaults()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			DefaultTrailTexture = new AsyncTexture2D(Textures.get_Error());
			DefaultMarkerTexture = new AsyncTexture2D(Textures.get_Error());
		}

		public async Task Load(string userResourceDir)
		{
			string trailOut = Path.Combine(userResourceDir, "defaultTrail.png");
			string markerOut = Path.Combine(userResourceDir, "defaultMarker.png");
			if (!File.Exists(trailOut))
			{
				try
				{
					Stream trailStream = PathingModule.Instance.ContentsManager.GetFileStream("png\\defaults\\trail.png");
					await FileUtil.WriteStreamAsync(trailOut, trailStream);
				}
				catch (Exception ex2)
				{
					Logger.Warn(ex2, "Failed to write default trail to path {filePath}.", new object[1] { trailOut });
				}
			}
			if (!File.Exists(markerOut))
			{
				try
				{
					Stream markerStream = PathingModule.Instance.ContentsManager.GetFileStream("png\\defaults\\marker.png");
					await FileUtil.WriteStreamAsync(markerOut, markerStream);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to write default trail to path {filePath}.", new object[1] { markerOut });
				}
			}
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				FileStream fileStream = File.Open(trailOut, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				FileStream fileStream2 = File.Open(markerOut, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				DefaultTrailTexture.SwapTexture(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)fileStream));
				DefaultMarkerTexture.SwapTexture(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)fileStream2));
			});
		}
	}
}
