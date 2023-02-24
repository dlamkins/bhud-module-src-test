using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Debug;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Tortle.PlayerMarker.Util
{
	public static class TextureUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TextureUtil));

		public static Texture2D FromPathPremultiplied(string filePath)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using FileStream fs = File.OpenRead(filePath);
				GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					return TextureUtil.FromStreamPremultiplied(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), (Stream)fs);
				}
				finally
				{
					((GraphicsDeviceContext)(ref ctx)).Dispose();
				}
			}
			catch (UnauthorizedAccessException e2)
			{
				Logger.Error((Exception)e2, "Unable to access {file}", new object[1] { filePath });
				Contingency.NotifyFileSaveAccessDenied(filePath, "Unable to access file", false);
				return Textures.get_Error();
			}
			catch (Exception e)
			{
				Logger.Error(e, "Unable to create texture from {file}", new object[1] { filePath });
				return Textures.get_Error();
			}
		}
	}
}
