using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Debug;
using Microsoft.Xna.Framework.Graphics;

namespace Tortle.PlayerMarker.Util
{
	public static class TextureUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TextureUtil));

		public static async Task<Texture2D> FromPathPremultipliedAsync(string filePath)
		{
			return await Task.Run((Func<Texture2D>)delegate
			{
				try
				{
					using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
					return TextureUtil.FromStreamPremultiplied((Stream)fileStream);
				}
				catch (UnauthorizedAccessException ex)
				{
					Logger.Error((Exception)ex, "Unable to access {file}", new object[1] { filePath });
					Contingency.NotifyFileSaveAccessDenied(filePath, "Unable to access file", false);
					return Textures.get_Error();
				}
				catch (Exception ex2)
				{
					Logger.Error(ex2, "Unable to create texture from {file}", new object[1] { filePath });
					return Textures.get_Error();
				}
			});
		}
	}
}
