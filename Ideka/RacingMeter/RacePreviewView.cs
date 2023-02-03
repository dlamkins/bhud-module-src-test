using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RacePreviewView : RaceDrawer
	{
		private class PreviewBounds : MapBounds
		{
			private readonly RacePreviewView _view;

			public PreviewBounds(RacePreviewView view)
			{
				_view = view;
			}

			public override Vector2 FromWorld(int mapId, Vector3 worldMeters)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				MapData mapData = RacingModule.MapData;
				Vector2 mapCenter = _view.MapCenter;
				float scale = _view.Scale;
				Matrix identity = Matrix.get_Identity();
				Point center = base.Center;
				return mapData.WorldToScreenMap(mapId, worldMeters, mapCenter, scale, identity, ((Point)(ref center)).ToVector2());
			}

			public override Vector2 FromMap(Vector2 mapCoords)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				Vector2 mapCenter = _view.MapCenter;
				float scale = _view.Scale;
				Matrix identity = Matrix.get_Identity();
				Point center = base.Center;
				return MapData.MapToScreenMap(mapCoords, mapCenter, scale, identity, ((Point)(ref center)).ToVector2());
			}
		}

		private const int StartScale = -3;

		private const float MoveAnimationTime = 0.5f;

		private const float ZoomAnimationTime = 0.2f;

		private FullRace? _fullRace;

		public FullGhost? _fullGhost;

		private Vector2 _mapCenter;

		private float _scale;

		private float _scaleLevel;

		private Tween? _moveAnimation;

		private Tween? _zoomAnimation;

		private readonly HashSet<Map> _maps;

		private readonly PreviewBounds _bounds;

		private Vector2? _dragStart;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				_fullRace = value;
				Race race = _fullRace?.Race;
				if (race == null)
				{
					return;
				}
				Map map = RacingModule.MapData.GetMap(race.MapId);
				if (map != null)
				{
					IEnumerable<Vector2> mapPoints = race.RacePoints.Select((RacePoint p) => map.WorldMetersToMap(p.Position));
					Vector2 center = mapPoints.Aggregate((Vector2 a, Vector2 b) => a + b) / (float)mapPoints.Count();
					Tween? moveAnimation = _moveAnimation;
					if (moveAnimation != null)
					{
						moveAnimation!.Pause();
					}
					_moveAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RacePreviewView>(this, (object)new
					{
						MapCenterX = center.X,
						MapCenterY = center.Y
					}, 0.5f, 0f, true).Ease((Func<float, float>)EaseOut);
					ScaleLevel = -3f;
				}
			}
		}

		public override Race? Race => FullRace?.Race;

		public FullGhost? FullGhost
		{
			get
			{
				return _fullGhost;
			}
			set
			{
				_fullGhost = value;
			}
		}

		public Ghost? Ghost => FullGhost?.Ghost;

		public Vector2 MapCenter
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _mapCenter;
			}
			private set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_mapCenter = value;
			}
		}

		public float MapCenterX
		{
			get
			{
				return _mapCenter.X;
			}
			set
			{
				_mapCenter.X = value;
			}
		}

		public float MapCenterY
		{
			get
			{
				return _mapCenter.Y;
			}
			set
			{
				_mapCenter.Y = value;
			}
		}

		public float Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				if (value != _scale)
				{
					Point val = ((Control)this).get_Size();
					Vector2 size = ((Point)(ref val)).ToVector2();
					Point position = Control.get_Input().get_Mouse().get_Position();
					Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
					val = position - ((Rectangle)(ref absoluteBounds)).get_Center();
					Vector2 mousePos = ((Point)(ref val)).ToVector2();
					Vector2 val2 = size / _scale - size / value;
					Vector2 ratio = mousePos / size;
					Vector2 delta = val2 * ratio;
					MapCenter += delta;
					Vector2? dragStart2 = _dragStart;
					if (dragStart2.HasValue)
					{
						Vector2 dragStart = dragStart2.GetValueOrDefault();
						_dragStart = dragStart - delta;
					}
					_scale = value;
				}
			}
		}

		public float ScaleLevel
		{
			get
			{
				return _scaleLevel;
			}
			set
			{
				if (value != _scaleLevel)
				{
					_scaleLevel = value;
					Tween? zoomAnimation = _zoomAnimation;
					if (zoomAnimation != null)
					{
						zoomAnimation!.Pause();
					}
					_zoomAnimation = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RacePreviewView>(this, (object)new
					{
						Scale = (float)Math.Pow(2.0, _scaleLevel)
					}, 0.2f, 0f, true).Ease((Func<float, float>)EaseOut);
				}
			}
		}

		public float GhostProgress { get; set; }

		private static float EaseOut(float x)
		{
			return (float)(1.0 - MathUtils.Cubed(1f - x));
		}

		public RacePreviewView()
		{
			_maps = new HashSet<Map>();
			_bounds = new PreviewBounds(this);
			Control.get_Input().get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)MouseMoved);
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)LeftMouseButtonReleased);
		}

		public void RepopulateMaps(IEnumerable<int> mapIds)
		{
			_maps.Clear();
			foreach (int mapId in new HashSet<int>(mapIds))
			{
				Map map = RacingModule.MapData.GetMap(mapId);
				if (map != null)
				{
					_maps.Add(map);
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)12;
		}

		private bool HandleHidden()
		{
			bool num = !((Control)(object)this).IsVisible();
			if (num && _dragStart.HasValue)
			{
				DragEnd();
			}
			return num;
		}

		private void DragStart()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver() && !HandleHidden())
			{
				Tween? moveAnimation = _moveAnimation;
				if (moveAnimation != null)
				{
					moveAnimation!.Pause();
				}
				Point position = Control.get_Input().get_Mouse().get_Position();
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				Point val = position - ((Rectangle)(ref absoluteBounds)).get_Center();
				Vector2 mousePos = ((Point)(ref val)).ToVector2();
				_dragStart = mousePos / Scale;
			}
		}

		private void Drag()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			Vector2? dragStart = _dragStart;
			if (dragStart.HasValue)
			{
				Vector2 start = dragStart.GetValueOrDefault();
				if (!HandleHidden())
				{
					Point position = Control.get_Input().get_Mouse().get_Position();
					Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
					Point val = position - ((Rectangle)(ref absoluteBounds)).get_Center();
					Vector2 mousePos = ((Point)(ref val)).ToVector2();
					MapCenter -= mousePos / Scale - start;
					_dragStart = mousePos / Scale;
				}
			}
		}

		private void DragEnd()
		{
			_dragStart = null;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonPressed(e);
			DragStart();
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseWheelScrolled(e);
			float scaleLevel = ScaleLevel;
			MouseState state = Control.get_Input().get_Mouse().get_State();
			ScaleLevel = scaleLevel + (float)Math.Sign(((MouseState)(ref state)).get_ScrollWheelValue());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle _)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			_bounds.Rectangle = ((Control)this).get_AbsoluteBounds();
			foreach (Map data in _maps)
			{
				ShapeExtensions.DrawRectangle(spriteBatch, RectangleF.op_Implicit(_bounds.FromMap(data.get_ContinentRect())), Color.get_Black(), 1f, 0f);
			}
			if (Race != null)
			{
				DrawMapLine(spriteBatch, _bounds);
				Ghost ghost = Ghost;
				if (ghost != null)
				{
					DrawGhost(spriteBatch, _bounds, ghost.SnapshotAt(GhostProgress));
				}
			}
		}

		private void MouseMoved(object sender, MouseEventArgs e)
		{
			Drag();
		}

		private void LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			DragEnd();
		}

		protected override void DisposeControl()
		{
			Control.get_Input().get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)MouseMoved);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)LeftMouseButtonReleased);
			((Control)this).DisposeControl();
		}
	}
}
