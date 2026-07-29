using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Views
{
	internal static class TooltipTextLayout
	{
		[IteratorStateMachine(typeof(_003CWrapLines_003Ed__0))]
		public static IEnumerable<string> WrapLines(string text, float maximumWidth, BitmapFont font)
		{
			return new _003CWrapLines_003Ed__0(-2)
			{
				_003C_003E3__text = text,
				_003C_003E3__maximumWidth = maximumWidth,
				_003C_003E3__font = font
			};
		}
	}
}
