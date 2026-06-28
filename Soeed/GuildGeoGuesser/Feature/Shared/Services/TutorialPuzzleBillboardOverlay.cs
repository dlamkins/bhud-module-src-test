using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public sealed class TutorialPuzzleBillboardOverlay : Control, IDisposable
	{
		private const float RingPulseSpeed = 3f;

		private readonly Location _puzzleLocation;

		private readonly AsyncTexture2D _icon;

		private readonly TutorialBillboard _billboard;

		private double _animationTime;

		public TutorialPuzzleBillboardOverlay(Location puzzleLocation)
			: this()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			_puzzleLocation = puzzleLocation;
			_icon = Service.Textures.DatAsset(157122);
			_billboard = new TutorialBillboard(_icon, puzzleLocation.AvatarPosition, Vector2.get_One());
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_ZIndex(2147483637);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_animationTime += gameTime.get_ElapsedGameTime().TotalSeconds;
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Visible(true);
		}

		private bool ShouldRender()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() != _puzzleLocation.MapId)
			{
				return false;
			}
			return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRender() && TutorialWorldProjection.TryProject(_puzzleLocation.AvatarPosition, out var projection))
			{
				float pulse = (float)(Math.Sin(_animationTime * 3.0) * 0.5 + 0.5);
				float distance = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), _puzzleLocation.AvatarPosition);
				if (projection.OnScreen && projection.InFront)
				{
					DrawWorldMarker(pulse, distance);
					return;
				}
				TutorialMarkerDraw.DrawPulsingMarker(spriteBatch, _icon, projection.EdgeScreen, pulse, Color.get_Red());
				TutorialMarkerDraw.DrawOffScreenPointer(spriteBatch, projection.EdgeScreen, projection.ArrowAngleRadians, pulse, Color.get_Red());
			}
		}

		private void DrawWorldMarker(float pulse, float distanceMeters)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				GraphicsDevice graphicsDevice = ((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice();
				Vector3 worldCenter = _puzzleLocation.AvatarPosition;
				_billboard.SetPosition(worldCenter);
				_billboard.SetWorldSize(TutorialWorldPulsingMarker.GetInnerRingDiameter(pulse));
				_billboard.SetDistanceOpacity(distanceMeters);
				_billboard.HandleRebuild(graphicsDevice);
				_billboard.Draw(graphicsDevice);
				TutorialWorldPulsingMarker.DrawRings(graphicsDevice, worldCenter, pulse, Color.get_LightGreen(), distanceMeters);
			}
			finally
			{
				((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
			}
		}

		public void Dispose()
		{
			((Control)this).DisposeControl();
		}

		protected override void DisposeControl()
		{
			((Control)this).set_Parent((Container)null);
			((Control)this).DisposeControl();
		}
	}
}
