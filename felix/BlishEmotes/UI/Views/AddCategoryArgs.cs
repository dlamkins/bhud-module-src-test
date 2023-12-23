using System.Collections.Generic;

namespace felix.BlishEmotes.UI.Views
{
	public class AddCategoryArgs
	{
		public string Name { get; set; }

		public List<Emote> Emotes { get; set; }

		public string TextureFileName { get; set; }
	}
}
