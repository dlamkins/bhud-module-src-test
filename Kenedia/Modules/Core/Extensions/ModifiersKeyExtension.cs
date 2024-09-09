using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ModifiersKeyExtension
	{
		public static Keys GetKey(this ModifierKeys modifier)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected I4, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			return (Keys)((modifier - 1) switch
			{
				1 => 164, 
				0 => 162, 
				3 => 160, 
				_ => 0, 
			});
		}
	}
}
