using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Nekres.FailScreens.Core
{
	internal static class AnimationUtil
	{
		public static int Column(int currentFrame, int framesPerRow)
		{
			return currentFrame % framesPerRow;
		}

		public static int Row(int currentFrame, int framesPerRow)
		{
			return currentFrame / framesPerRow;
		}

		public static int Frame(int totalFrames, int updatesPerFrame)
		{
			return (int)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds * (double)updatesPerFrame) % totalFrames;
		}

		public static Point FramePos(int row, int col, int frameWidth, int frameHeight)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new Point(col * frameWidth, row * frameHeight);
		}

		public static Point Animate(int totalFrames, int framesPerRow, int frameWidth, int frameHeight, int updatesPerFrame)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			int currentFrame = Frame(totalFrames, updatesPerFrame);
			return FramePos(col: Column(currentFrame, framesPerRow), row: Row(currentFrame, framesPerRow), frameWidth: frameWidth, frameHeight: frameHeight);
		}
	}
}
