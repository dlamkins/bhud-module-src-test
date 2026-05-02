using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class EmbeddedResourceLoader
	{
		public static Texture2D LoadTexture(string fileName)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			Assembly assembly = typeof(EmbeddedResourceLoader).GetTypeInfo().Assembly;
			string resourceName = assembly.GetManifestResourceNames().FirstOrDefault((string r) => r.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
			if (resourceName == null)
			{
				throw new InvalidOperationException("Embedded resource '" + fileName + "' not found.");
			}
			using Stream stream = assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
			{
				throw new InvalidOperationException("Failed to open stream for '" + fileName + "'.");
			}
			GraphicsDeviceContext gdx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D texture = Texture2D.FromStream(((GraphicsDeviceContext)(ref gdx)).get_GraphicsDevice(), stream);
				Color[] pixels = (Color[])(object)new Color[texture.get_Width() * texture.get_Height()];
				texture.GetData<Color>(pixels);
				for (int i = 0; i < pixels.Length; i++)
				{
					Color c = pixels[i];
					float alpha = (float)(int)((Color)(ref c)).get_A() / 255f;
					pixels[i] = new Color((byte)((float)(int)((Color)(ref c)).get_R() * alpha), (byte)((float)(int)((Color)(ref c)).get_G() * alpha), (byte)((float)(int)((Color)(ref c)).get_B() * alpha), ((Color)(ref c)).get_A());
				}
				texture.SetData<Color>(pixels);
				return texture;
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdx)).Dispose();
			}
		}

		public static SoundEffect LoadSound(string fileName)
		{
			Assembly assembly = typeof(EmbeddedResourceLoader).GetTypeInfo().Assembly;
			string resourceName = assembly.GetManifestResourceNames().FirstOrDefault((string r) => r.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
			if (resourceName == null)
			{
				throw new InvalidOperationException("Embedded resource '" + fileName + "' not found.");
			}
			using Stream stream = assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
			{
				throw new InvalidOperationException("Failed to open stream for '" + fileName + "'.");
			}
			return SoundEffect.FromStream(stream);
		}
	}
}
