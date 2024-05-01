using Blish_HUD.Content;
using Blish_HUD.Controls;

namespace Ideka.CustomCombatText
{
	public interface IUIPanel
	{
		Panel Panel { get; }

		AsyncTexture2D Icon { get; }

		string Caption { get; }
	}
}
