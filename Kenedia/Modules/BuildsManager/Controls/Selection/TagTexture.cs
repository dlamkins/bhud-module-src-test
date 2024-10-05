using Blish_HUD.Content;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class TagTexture : DetailedTexture
	{
		public TemplateTag Tag { get; internal set; }

		public TagTexture(AsyncTexture2D texture)
			: base(texture)
		{
		}
	}
}
