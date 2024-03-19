namespace DanceDanceRotationModule.Model
{
	public static class OrientationExtensions
	{
		public static bool IsVertical(NotesOrientation orientation)
		{
			if ((uint)orientation > 1u && (uint)(orientation - 2) <= 2u)
			{
				return true;
			}
			return false;
		}
	}
}
