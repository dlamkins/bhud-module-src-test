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

		public bool FileChecked;

		public bool FileFetched;

		public bool FileLoaded;

		public string FolderPath;

		private string _fileName;

		public Texture2D Texture;

		public string FileName
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

		public string IconPath => FolderPath + "/" + FileName;
	}
}
