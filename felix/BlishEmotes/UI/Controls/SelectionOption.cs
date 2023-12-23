using Microsoft.Xna.Framework.Graphics;

namespace felix.BlishEmotes.UI.Controls
{
	public class SelectionOption
	{
		public Texture2D Texture { get; protected set; }

		public string TextureRef { get; protected set; }

		public SelectionOption(Texture2D texture, string textureRef)
		{
			Texture = texture;
			TextureRef = textureRef;
		}
	}
}
