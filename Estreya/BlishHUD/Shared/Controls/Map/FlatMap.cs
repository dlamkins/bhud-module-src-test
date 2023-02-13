using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.Shared.Utils;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls.Map
{
	public class FlatMap : Control
	{
		private const int MAPWIDTH_MAX = 362;

		private const int MAPHEIGHT_MAX = 338;

		private const int MAPWIDTH_MIN = 170;

		private const int MAPHEIGHT_MIN = 170;

		private const int MAPOFFSET_MIN = 19;

		private double _lastMapViewChanged;

		private AsyncLock _entityLock = new AsyncLock();

		private MapEntity _activeEntity;

		private List<MapEntity> _entities = new List<MapEntity>();

		public FlatMap()
			: this()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			((Control)this).set_ZIndex(-214748364);
			((Control)this).set_Location(new Point(1));
			((Control)this).set_SpriteBatchParameters(new SpriteBatchParameters((SpriteSortMode)0, BlendState.Opaque, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null));
			UpdateBounds();
		}

		public void AddEntity(MapEntity mapEntity)
		{
			using (_entityLock.Lock())
			{
				_entities.Add(mapEntity);
				mapEntity.Disposed += MapEntity_Disposed;
			}
		}

		private void MapEntity_Disposed(object sender, EventArgs e)
		{
			RemoveEntity(sender as MapEntity);
		}

		public void RemoveEntity(MapEntity mapEntity)
		{
			using (_entityLock.Lock())
			{
				_entities.Remove(mapEntity);
			}
		}

		public void ClearEntities()
		{
			using (_entityLock.Lock())
			{
				_entities.Clear();
			}
		}

		private int GetOffset(float curr, float max, float min, float val)
		{
			return (int)Math.Round((curr - min) / (max - min) * (val - 19f) + 19f, 0);
		}

		private void UpdateBounds()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			Size compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			if (((Size)(ref compassSize)).get_Width() < 1)
			{
				return;
			}
			compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			if (((Size)(ref compassSize)).get_Height() < 1)
			{
				return;
			}
			Point newSize = default(Point);
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				((Control)this).set_Location(Point.get_Zero());
				newSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int offsetWidth = GetOffset(((Size)(ref compassSize)).get_Width(), 362f, 170f, 40f);
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int offsetHeight = GetOffset(((Size)(ref compassSize)).get_Height(), 338f, 170f, 40f);
				if (GameService.Gw2Mumble.get_UI().get_IsCompassTopRight())
				{
					int width = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					((Control)this).set_Location(new Point(width - ((Size)(ref compassSize)).get_Width() - offsetWidth + 1, 1));
				}
				else
				{
					int width2 = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					int num = width2 - ((Size)(ref compassSize)).get_Width() - offsetWidth;
					int height = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Height;
					compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
					((Control)this).set_Location(new Point(num, height - ((Size)(ref compassSize)).get_Height() - offsetHeight - 40));
				}
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				int num2 = ((Size)(ref compassSize)).get_Width() + offsetWidth;
				compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
				((Point)(ref newSize))._002Ector(num2, ((Size)(ref compassSize)).get_Height() + offsetHeight);
			}
			((Control)this).set_Size(newSize);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			UpdateBounds();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
			((Rectangle)(ref bounds))._002Ector(((Control)this).get_Location(), ((Rectangle)(ref bounds)).get_Size());
			double scale = GameService.Gw2Mumble.get_UI().get_MapScale() * 0.897;
			double offsetX = (double)bounds.X + (double)bounds.Width / 2.0;
			double offsetY = (double)bounds.Y + (double)bounds.Height / 2.0;
			float opacity = MathHelper.Clamp((float)(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds - _lastMapViewChanged) / 0.65f, 0f, 1f);
			_activeEntity = null;
			if (_entityLock.IsFree())
			{
				using (_entityLock.Lock())
				{
					foreach (MapEntity entity in _entities)
					{
						RectangleF? hint = entity.RenderToMiniMap(spriteBatch, bounds, offsetX, offsetY, scale, opacity);
						if (hint.HasValue)
						{
							RectangleF value = hint.Value;
							if (((RectangleF)(ref value)).Contains(Point2.op_Implicit(GameService.Input.get_Mouse().get_Position())))
							{
								_activeEntity = entity;
							}
						}
					}
				}
			}
			UpdateTooltip();
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (_activeEntity == null)
			{
				return null;
			}
			return ((Control)this).TriggerMouseInput(mouseEventType, ms);
		}

		private void UpdateTooltip()
		{
			((Control)this).set_BasicTooltipText(_activeEntity?.TooltipText);
		}

		protected override void DisposeControl()
		{
			for (int i = _entities.Count - 1; i >= 0; i--)
			{
				_entities[i]?.Dispose();
			}
			_entities?.Clear();
			_entities = null;
		}
	}
}
