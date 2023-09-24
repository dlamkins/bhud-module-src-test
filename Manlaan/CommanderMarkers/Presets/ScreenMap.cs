using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.CommanderMarkers.Presets
{
	public class ScreenMap : Control
	{
		public static class Data
		{
			private const double BlishScale = 1.1148272017837235;

			private static int _lastTick;

			private static Vector2 _mapCenter;

			private static Matrix _mapRotation;

			private static float _scale;

			private static Rectangle _screenBounds;

			private static Vector2 _boundsCenter;

			public static Vector2 MapCenter => UpdateAndReturn(ref _mapCenter);

			public static Matrix MapRotation => UpdateAndReturn(ref _mapRotation);

			public static float Scale => UpdateAndReturn(ref _scale);

			public static Rectangle ScreenBounds => UpdateAndReturn(ref _screenBounds);

			public static Vector2 BoundsCenter => UpdateAndReturn(ref _boundsCenter);

			private static T UpdateAndReturn<T>(ref T value)
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				if (GameService.Gw2Mumble.get_Tick() != _lastTick)
				{
					_lastTick = GameService.Gw2Mumble.get_Tick();
					_mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
					_mapRotation = Matrix.CreateRotationZ((GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen()) ? ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()) : 0f);
					_screenBounds = MumbleUtils.GetMapBounds();
					_scale = (float)(1.1148272017837235 / GameService.Gw2Mumble.get_UI().get_MapScale());
					Point val = ((Rectangle)(ref _screenBounds)).get_Location();
					Vector2 val2 = ((Point)(ref val)).ToVector2();
					val = ((Rectangle)(ref _screenBounds)).get_Size();
					_boundsCenter = val2 + ((Point)(ref val)).ToVector2() / 2f;
				}
				return value;
			}
		}

		private readonly List<IMapEntity> _entities = new List<IMapEntity>();

		private readonly MapData _mapData;

		private readonly ScreenMapBounds _mapBounds;

		private bool _previewActive;

		private BitmapFont _bitmapFont = GameService.Content.get_DefaultFont32();

		public ScreenMap(MapData mapData)
			: this()
		{
			_mapData = mapData;
			_mapBounds = new ScreenMapBounds(_mapData);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void AddEntity(IMapEntity entity)
		{
			_entities.Add(entity);
		}

		public void RemoveEntity(IMapEntity entity)
		{
			_entities.Remove(entity);
		}

		public void ClearEntities()
		{
			_entities.Clear();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			Rectangle screenBounds = Data.ScreenBounds;
			((Control)this).set_Location(((Rectangle)(ref screenBounds)).get_Location());
			screenBounds = Data.ScreenBounds;
			((Control)this).set_Size(((Rectangle)(ref screenBounds)).get_Size());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || _mapData.Current == null || GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() || (Service.Settings.AutoMarker_OnlyWhenCommander.get_Value() && !GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander() && !Service.LtMode.get_Value()))
			{
				return;
			}
			Vector3 playerPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			((Rectangle)(ref bounds)).set_Location(((Control)this).get_Location());
			_mapBounds.Rectangle = bounds;
			bool promptDrawn = !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			foreach (IMapEntity entity in _entities)
			{
				entity.DrawToMap(spriteBatch, _mapBounds, (Control)(object)this, playerPosition);
				if (!promptDrawn && entity.DistanceFrom(playerPosition) < 15f)
				{
					promptDrawn = true;
					DrawPrompt(spriteBatch, entity);
				}
			}
			if (promptDrawn && !_previewActive && GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				Service.MapWatch.PreviewClosestMarkerSet();
				_previewActive = true;
			}
			if (_previewActive && (!promptDrawn || !GameService.Gw2Mumble.get_UI().get_IsMapOpen()))
			{
				Service.MapWatch.RemovePreviewMarkerSet();
				_previewActive = false;
			}
		}

		protected void DrawPrompt(SpriteBatch spriteBatch, IMapEntity marker)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			string interactKey = Service.Settings._settingInteractKeyBinding.get_Value().GetBindingDisplayText();
			Rectangle _promptRectangle = default(Rectangle);
			((Rectangle)(ref _promptRectangle))._002Ector(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 150, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 120, 300, 120);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Press '" + interactKey + "' to place markers\n" + marker.GetMarkerText(), _bitmapFont, _promptRectangle, Color.get_Black(), false, true, 3, (HorizontalAlignment)1, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Press '" + interactKey + "' to place markers\n" + marker.GetMarkerText(), _bitmapFont, _promptRectangle, Color.get_Orange(), false, (HorizontalAlignment)1, (VerticalAlignment)0);
		}
	}
}
