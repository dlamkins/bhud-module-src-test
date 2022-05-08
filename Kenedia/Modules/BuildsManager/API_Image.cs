using System.Collections.Generic;
using System.Text.RegularExpressions;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class API_Image
	{
		public List<Control> connectedControls = new List<Control>();

		public string Url;

		public bool fileChecked;

		public bool fileFetched;

		public bool fileLoaded;

		public string folderPath;

		private string _iconPath;

		private string _fileName;

		public Texture2D _Texture;

		public Texture2D Texture;

		public string fileName
		{
			get
			{
				if (_fileName != null)
				{
					return _fileName;
				}
				_fileName = Regex.Match(Url, "[0-9]*.png").ToString();
				return _fileName;
			}
		}

		public string iconPath => folderPath + "/" + fileName;

		private bool fetchImage()
		{
			return false;
		}
	}
}
