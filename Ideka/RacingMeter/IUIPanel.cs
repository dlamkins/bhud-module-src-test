using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public interface IUIPanel
	{
		Panel Panel { get; }

		Texture2D Icon { get; }

		string Caption { get; }
	}
}
