using System;
using System.Threading.Tasks;
using Gw2Sharp.Models;

namespace Blish_HUD.Extended
{
	public static class PositionUtil
	{
		private const double JumpHeight = 1.622;

		private const double JumpHeightError = 0.3244;

		private const double MinJumpHeight = 1.2976;

		private const double MaxJumpHeight = 1.9464000000000001;

		private const double MaxHeightChange = 0.10000000149011612;

		private const double MaxWiggleDistanceSqr = 0.005;

		private const double MaxYSlide = 0.004;

		private static readonly TimeSpan JumpTime = TimeSpan.FromSeconds(0.65);

		private static readonly TimeSpan JumpTimeError = TimeSpan.FromSeconds(0.25);

		private static readonly TimeSpan MinJumpTime = JumpTime - JumpTimeError;

		private static readonly TimeSpan MaxJumpTime = JumpTime + JumpTimeError;

		private static readonly TimeSpan MaxStillTime = TimeSpan.FromSeconds(0.1);

		public static void RequestGroundPosition(Action<bool, Coordinates3> callback)
		{
			Task.Run(delegate
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0139: Unknown result type (might be due to invalid IL or missing references)
				//IL_019c: Unknown result type (might be due to invalid IL or missing references)
				DateTime utcNow = DateTime.UtcNow;
				DateTime utcNow2 = DateTime.UtcNow;
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				Coordinates3 avatarPosition = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition();
				Coordinates3 avatarPosition2 = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition();
				while (((Coordinates3)(ref avatarPosition2)).get_X() == ((Coordinates3)(ref avatarPosition)).get_X() && ((Coordinates3)(ref avatarPosition2)).get_Z() == ((Coordinates3)(ref avatarPosition)).get_Z() && ((Coordinates3)(ref avatarPosition2)).get_Y() >= ((Coordinates3)(ref avatarPosition)).get_Y())
				{
					Coordinates3 a = avatarPosition2;
					avatarPosition2 = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition();
					num = Math.Max(num, Math.Pow(((Coordinates3)(ref avatarPosition2)).get_X() - ((Coordinates3)(ref avatarPosition)).get_X(), 2.0) + Math.Pow(((Coordinates3)(ref avatarPosition2)).get_Z() - ((Coordinates3)(ref avatarPosition)).get_Z(), 2.0));
					num2 = Math.Max(num2, ((Coordinates3)(ref avatarPosition2)).get_Y() - ((Coordinates3)(ref avatarPosition)).get_Y());
					num3 = Math.Min(num2, ((Coordinates3)(ref avatarPosition2)).get_Y() - ((Coordinates3)(ref avatarPosition)).get_Y());
					if (num > 0.005 || num2 > 1.9464000000000001 || num3 < -0.10000000149011612)
					{
						callback(arg1: false, avatarPosition2);
						break;
					}
					if (!CloseEnough(a, avatarPosition2))
					{
						utcNow2 = DateTime.UtcNow;
					}
					TimeSpan timeSpan = utcNow2 - utcNow;
					if (timeSpan > MaxJumpTime)
					{
						callback(arg1: false, avatarPosition2);
						break;
					}
					if (DateTime.UtcNow - utcNow2 >= MaxStillTime)
					{
						double num4 = Math.Abs(((Coordinates3)(ref avatarPosition2)).get_Y() - ((Coordinates3)(ref avatarPosition)).get_Y());
						if (timeSpan < MinJumpTime && num2 >= 1.2976 && num4 <= 0.10000000149011612)
						{
							callback(arg1: true, avatarPosition2);
							break;
						}
					}
				}
			});
		}

		private static bool CloseEnough(Coordinates3 a, Coordinates3 b)
		{
			if (((Coordinates3)(ref a)).get_X() == ((Coordinates3)(ref b)).get_X() && ((Coordinates3)(ref a)).get_Z() == ((Coordinates3)(ref b)).get_Z())
			{
				return Math.Abs(((Coordinates3)(ref a)).get_Y() - ((Coordinates3)(ref b)).get_Y()) < 0.004;
			}
			return false;
		}
	}
}
