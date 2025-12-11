using Blish_HUD.Content;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Characters.Models
{
	public class Race
	{
		public Races Id { get; set; }

		public LocalizedString Names { get; set; } = new LocalizedString();


		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				if (_003CIcon_003Ek__BackingField == null)
				{
					_003CIcon_003Ek__BackingField = TexturesService.GetTextureFromRef("textures\\races\\" + Id.ToString().ToLower() + ".png");
				}
				return _003CIcon_003Ek__BackingField;
			}
		}
	}
}
