using Blish_HUD.Input;

namespace Manlaan.Mounts.Views
{
	public static class KeyBindingExtensions
	{
		public static bool EqualsKeyBinding(this KeyBinding firstKeyBinding, KeyBinding secondKeyBinding)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (firstKeyBinding == null || secondKeyBinding == null)
			{
				return false;
			}
			if (firstKeyBinding.get_PrimaryKey() == secondKeyBinding.get_PrimaryKey())
			{
				return firstKeyBinding.get_ModifierKeys() == secondKeyBinding.get_ModifierKeys();
			}
			return false;
		}
	}
}
