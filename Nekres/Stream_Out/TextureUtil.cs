using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Nekres.Stream_Out
{
	internal static class TextureUtil
	{
		public static async Task ClearImage(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			using MemoryStream stream = new MemoryStream(File.ReadAllBytes(path));
			using Bitmap bitmap = (Bitmap)Image.FromStream(stream);
			using (Graphics gfx = Graphics.FromImage(bitmap))
			{
				gfx.Clear(Color.Transparent);
				gfx.Flush();
			}
			await bitmap.SaveOnNetworkShare(path, ImageFormat.Png);
		}

		public static async Task SaveToImage(string renderUri, string path)
		{
			await StreamOutModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToByteArrayAsync(renderUri, default(CancellationToken))
				.ContinueWith((Func<Task<byte[]>, Task>)async delegate(Task<byte[]> textureDataResponse)
				{
					if (textureDataResponse.IsFaulted)
					{
						StreamOutModule.Logger.Warn("Request to render service for " + renderUri + " failed.");
					}
					else
					{
						using MemoryStream textureStream = new MemoryStream(textureDataResponse.Result);
						using Bitmap bitmap = new Bitmap(textureStream);
						await bitmap.SaveOnNetworkShare(path, ImageFormat.Png);
					}
				});
		}
	}
}
