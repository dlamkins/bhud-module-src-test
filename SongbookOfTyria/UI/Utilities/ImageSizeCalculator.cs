using Microsoft.Xna.Framework;

namespace SongbookOfTyria.UI.Utilities
{
	public static class ImageSizeCalculator
	{
		public static Point CalculateAspectRatioSize(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (originalWidth == 0 || originalHeight == 0)
			{
				return new Point(maxWidth, maxHeight);
			}
			float aspectRatio = (float)originalWidth / (float)originalHeight;
			int targetWidth = maxWidth;
			int targetHeight = (int)((float)targetWidth / aspectRatio);
			if (targetHeight > maxHeight)
			{
				targetHeight = maxHeight;
				targetWidth = (int)((float)targetHeight * aspectRatio);
			}
			return new Point(targetWidth, targetHeight);
		}
	}
}
