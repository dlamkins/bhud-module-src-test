using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Mumble.Models;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class UIOverlay
	{
		public static float GetScale()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected I4, but got Unknown
			float num = Math.Min((float)Math.Min(GameService.Graphics.get_WindowWidth(), 1024) / 1024f, (float)Math.Min(GameService.Graphics.get_WindowHeight(), 768) / 768f);
			UiSize uISize = GameService.Gw2Mumble.get_UI().get_UISize();
			return num * (int)uISize switch
			{
				0 => 0.9f, 
				2 => 1.1f, 
				3 => 1.21f, 
				_ => 1f, 
			};
		}

		public static Vector2 GetHealthCenter()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(0.5005f * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds().Width, (1f - 65.5f * GetScale() / (float)GameService.Graphics.get_WindowHeight()) * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds().Height);
		}

		public static float GetHealthRadius()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			return 51f * GetScale() / (float)GameService.Graphics.get_WindowHeight() * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds().Height;
		}

		public static float GetEnduranceThickness()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			return 7f * GetScale() / (float)GameService.Graphics.get_WindowHeight() * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds().Height;
		}

		public static (Vector2, Vector2) GetEndurancePoints(double x)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			double radians = (MathUtils.Lerp(-62.99999999999999, 62.99999999999999, x) - 90.0) * (Math.PI / 180.0);
			Vector2 cossin = default(Vector2);
			((Vector2)(ref cossin))._002Ector((float)Math.Cos(radians), (float)Math.Sin(radians));
			Vector2 val = GetHealthCenter() + cossin * GetHealthRadius();
			Vector2 end = val + cossin * GetEnduranceThickness();
			return (val, end);
		}
	}
}
