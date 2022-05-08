using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class subtExtension
	{
		public API_ImageStates ImageState;

		public Texture2D Texture;

		public string Path;

		public string FileName;

		public string Url;

		public List<Control> Controls = new List<Control>();

		public List<Texture2D> Textures = new List<Texture2D>();
	}
}
