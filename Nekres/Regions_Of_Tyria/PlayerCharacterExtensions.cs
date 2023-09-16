using System;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;

namespace Nekres.Regions_Of_Tyria
{
	public static class PlayerCharacterExtensions
	{
		private static Vector3 _prevPosition;

		private static float _prevSpeed;

		private static DateTime _prevUpdate = DateTime.UtcNow;

		public static float GetSpeed(this PlayerCharacter player, GameTime gameTime)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (DateTime.UtcNow.Subtract(_prevUpdate).TotalMilliseconds < 40.0)
			{
				return _prevSpeed;
			}
			_prevUpdate = DateTime.UtcNow;
			Vector3 currentPosition = player.get_Position();
			float num = Vector3.Distance(currentPosition, _prevPosition) / (float)gameTime.get_ElapsedGameTime().TotalSeconds;
			_prevPosition = currentPosition;
			_prevSpeed = num;
			return num;
		}
	}
}
