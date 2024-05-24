using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
	public class DownloadTextureService : IDisposable
	{
		protected Dictionary<string, AsyncTexture2D> DynamicTextures { get; } = new Dictionary<string, AsyncTexture2D>();


		public Texture2D GetDynamicTexture(string path)
		{
			if (DynamicTextures.ContainsKey(path))
			{
				return AsyncTexture2D.op_Implicit(DynamicTextures[path]);
			}
			Texture2D asyncTex = LoadTexture(path, Textures.get_Pixel());
			DynamicTextures.Add(path, AsyncTexture2D.op_Implicit(asyncTex));
			return asyncTex;
		}

		public bool ValidateTextureCache(string fileName)
		{
			FileInfo configFileInfo = GetFileInfo(fileName);
			if (configFileInfo != null && !configFileInfo.Exists)
			{
				return DownloadFile(Module.STATIC_HOST_URL, fileName);
			}
			return true;
		}

		protected Texture2D LoadTexture(string fileName, Texture2D fallbackTexture)
		{
			FileInfo configFileInfo = GetFileInfo(fileName);
			if (configFileInfo != null && configFileInfo.Exists)
			{
				using (FileStream stream = new FileStream(configFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					return TextureUtil.FromStreamPremultiplied((Stream)stream);
				}
			}
			if (DownloadFile(Module.STATIC_HOST_URL, fileName))
			{
				return LoadTexture(fileName, fallbackTexture);
			}
			return Textures.get_Error();
		}

		private FileInfo GetFileInfo(string fileName)
		{
			return new FileInfo(Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + fileName);
		}

		private bool DownloadFile(string url, string fileName)
		{
			try
			{
				using WebClient webClient = new WebClient();
				string savePath = Service.DirectoriesManager.GetFullDirectoryPath(Module.DIRECTORY_PATH) + "\\" + fileName;
				webClient.DownloadFile(url + "/" + fileName, savePath);
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		public void Dispose()
		{
			DynamicTextures.ToList().ForEach(delegate(KeyValuePair<string, AsyncTexture2D> t)
			{
				t.Value.Dispose();
			});
		}
	}
}
