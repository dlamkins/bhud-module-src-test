using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ModifiersKeyExtension
	{
		public static Keys GetKey(this ModifierKeys modifier)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			return (Keys)(modifier switch
			{
				ModifierKeys.Alt => 164, 
				ModifierKeys.Ctrl => 162, 
				ModifierKeys.Shift => 160, 
				_ => 0, 
			});
		}
	}
}
