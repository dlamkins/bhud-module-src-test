using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RaceEditor : RaceDrawerWorld
	{
		public class RawPoint
		{
			public string Type { get; set; } = "";


			public double X { get; set; }

			public double Y { get; set; }

			public double Z { get; set; }

			public Vector3 Position => new Vector3((float)X, (float)Y, (float)Z);
		}

		private bool _disposed;

		private BasicEffect? _effect;

		private readonly EditState _state;

		private readonly CommandList _commands;

		private readonly Dictionary<string, RawPoint> _rawPoints = new Dictionary<string, RawPoint>();

		private (RacePoint? point, RacePoint preview) _pointPreview = (null, new RacePoint());

		public FullRace FullRace
		{
			get
			{
				return _state.FullRace;
			}
			set
			{
				_state.FullRace = value;
			}
		}

		public override Race Race => FullRace.Race;

		public MeasurerRealtime Measurer { get; }

		public bool Draw300InGuide { get; set; }

		public RacePoint? Selected => _state.Selected;

		public bool CanUndo => _commands.CanUndo;

		public bool CanRedo => _commands.CanRedo;

		public bool IsDirty => _commands.IsDirty;

		public event Action<FullRace> RaceLoaded
		{
			add
			{
				_state.RaceLoaded += value;
			}
			remove
			{
				_state.RaceLoaded -= value;
			}
		}

		public event Action<FullRace> RaceModified
		{
			add
			{
				_state.RaceModified += value;
			}
			remove
			{
				_state.RaceModified -= value;
			}
		}

		public event Action<RacePoint> PointSelected
		{
			add
			{
				_state.PointSelected += value;
			}
			remove
			{
				_state.PointSelected -= value;
			}
		}

		public event Action<RacePoint> PointInserted
		{
			add
			{
				_state.PointInserted += value;
			}
			remove
			{
				_state.PointInserted -= value;
			}
		}

		public event Action<RacePoint> PointRemoved
		{
			add
			{
				_state.PointRemoved += value;
			}
			remove
			{
				_state.PointRemoved -= value;
			}
		}

		public event Action<RacePoint, bool> PointSwapped
		{
			add
			{
				_state.PointSwapped += value;
			}
			remove
			{
				_state.PointSwapped -= value;
			}
		}

		public event Action<RacePoint> PointModified
		{
			add
			{
				_state.PointModified += value;
			}
			remove
			{
				_state.PointModified -= value;
			}
		}

		public event Action<bool, bool> CanUndoRedoChanged
		{
			add
			{
				_commands.CanUndoRedoChanged += value;
			}
			remove
			{
				_commands.CanUndoRedoChanged -= value;
			}
		}

		public event Action<bool> IsDirtyChanged
		{
			add
			{
				_commands.IsDirtyChanged += value;
			}
			remove
			{
				_commands.IsDirtyChanged -= value;
			}
		}

		public event Action<int>? TestRequested;

		public RaceEditor(MeasurerRealtime measurer, FullRace? race)
		{
			Measurer = measurer;
			_state = new EditState(race);
			_commands = new CommandList(_state);
			RaceLoaded += delegate
			{
				_commands.Clear();
			};
			Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Expected O, but got Unknown
				if (!_disposed && _effect == null)
				{
					_effect = new BasicEffect(graphicsDevice);
				}
			});
		}

		private bool FixPoint(out RacePoint result)
		{
			return FixPoint(null, out result);
		}

		private bool FixPoint(RacePoint? point, out RacePoint result)
		{
			result = point ?? Selected;
			return result != null;
		}

		public void Test(RacePoint? testPoint)
		{
			if (FixPoint(testPoint, out var target))
			{
				int index = Race.RacePoints.IndexOf(target);
				(int, RacePoint) p2 = Race.RacePoints.Enumerate().Reverse().FirstOrDefault(((int index, RacePoint item) p) => p.item.IsCheckpoint && p.index <= index);
				int i = Race.Checkpoints.ToList().IndexOf(p2.Item2);
				Test((i >= 0) ? i : 0);
			}
		}

		public void Test(int testCheckpoint = 0)
		{
			this.TestRequested?.Invoke(testCheckpoint);
		}

		public void DiscardChanges()
		{
			FullRace original = LocalData.GetRaceFromDisk(FullRace.Meta.Id);
			if (original != null)
			{
				FullRace = original;
			}
		}

		public void DeleteRace()
		{
			FullRace toDelete = FullRace;
			FullRace = DataExtensions.NewRace();
			RacingModule.LocalData.DeleteRace(toDelete);
		}

		public void Undo()
		{
			_commands.Undo();
		}

		public void Redo()
		{
			_commands.Redo();
		}

		public void Save()
		{
			RacingModule.LocalData.SaveRace(FullRace);
			_commands.Saved();
			ScreenNotification.ShowNotification(Strings.NotifyRaceSaved, (NotificationType)0, (Texture2D)null, 4);
		}

		public string? Describe(RacePoint? point)
		{
			if (point == null)
			{
				return null;
			}
			int i = Race.RacePoints.IndexOf(point);
			if (i < 0)
			{
				return null;
			}
			return Strings.PointDescription.Format(i + 1, point!.Type.Describe());
		}

		public void RenameRace(string name)
		{
			_commands.Run(new RenameRaceCommand(name));
		}

		public void SetRaceType(RaceType type)
		{
			_commands.Run(new SetRaceTypeCommand(type));
		}

		public void SetRaceMap(int mapId)
		{
			_commands.Run(new SetRaceMapCommand(mapId));
		}

		public void Select(RacePoint? point)
		{
			_commands.Run(new SelectPointCommand(point));
		}

		public void SelectNearest()
		{
			Select(Race.RacePoints.MinBy((RacePoint c) => Vector3.DistanceSquared(c.Position, Measurer.Pos.Meters)));
		}

		public void SelectPrevious(RacePoint? point = null)
		{
			if (point == null)
			{
				point = Selected;
			}
			int i = _state.PointIndexOf(point);
			Select((i < 0 || point == null) ? Race.RacePoints.Last() : Race.RacePoints.Skip(i - 1).First());
		}

		public void SelectNext(RacePoint? point = null)
		{
			if (point == null)
			{
				point = Selected;
			}
			int i = _state.PointIndexOf(point);
			Select((i < 0 || point == null) ? Race.RacePoints.First() : Race.RacePoints.Skip(Math.Min(i + 1, Race.RacePoints.Count - 1)).First());
		}

		public void InsertCheckpoint(bool before)
		{
			InsertCheckpoint(null, before);
		}

		public void InsertCheckpoint(RacePoint? existing, bool before)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (existing == null)
			{
				existing = Selected;
			}
			int i = _state.PointIndexOf(existing);
			i = ((i < 0) ? ((!before) ? Race.RacePoints.Count : 0) : (before ? i : (i + 1)));
			_commands.Run(new InsertPointCommand(i, new RacePoint
			{
				Position = Measurer.Pos.Meters,
				Radius = (Selected?.Radius ?? 15f),
				Type = (Selected?.Type ?? RacePointType.Checkpoint)
			}));
		}

		public void RemoveCheckpoint(RacePoint? point = null)
		{
			if (FixPoint(point, out var @fixed))
			{
				_commands.Run(new RemovePointCommand(@fixed));
			}
		}

		public void SwapCheckpoint(bool previous)
		{
			SwapCheckpoint(null, previous);
		}

		public void SwapCheckpoint(RacePoint? point, bool previous)
		{
			if (FixPoint(point, out var target))
			{
				_commands.Run(new SwapPointCommand(target, previous));
			}
		}

		public void MovePoint(Vector3 position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			MovePoint(null, position);
		}

		public void MovePoint(float? x = null, float? y = null, float? z = null)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (FixPoint(out var point))
			{
				MovePoint(point, new Vector3(x ?? point.Position.X, y ?? point.Position.Y, z ?? point.Position.Z));
			}
		}

		public void MovePoint(RacePoint? point, Vector3 position)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_pointPreview.point = null;
			if (FixPoint(point, out var target))
			{
				_commands.Run(new MovePointCommand(target, position));
			}
		}

		public void SnapPoint()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (FixPoint(out var point) && _rawPoints.Any())
			{
				RawPoint closest = _rawPoints.Values.MinBy((RawPoint p) => Vector3.DistanceSquared(p.Position, point.Position));
				_commands.Run(new MovePointCommand(point, closest.Position));
			}
		}

		public void ResizePoint(float radius)
		{
			ResizePoint(null, radius);
		}

		public void ResizePoint(RacePoint? point, float radius)
		{
			_pointPreview.point = null;
			if (FixPoint(point, out var target))
			{
				_commands.Run(new ResizePointCommand(target, radius));
			}
		}

		public void SetPointType(RacePointType type)
		{
			SetPointType(null, type);
		}

		public void SetPointType(RacePoint? point, RacePointType type)
		{
			if (FixPoint(point, out var target))
			{
				_commands.Run(new SetPointTypeCommand(target, type));
			}
		}

		public void MovePointPreview(Vector3 position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			MovePointPreview(null, position);
		}

		public void MovePointPreview(RacePoint? point, Vector3 position)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (FixPoint(point, out var target))
			{
				_pointPreview.point = target;
				_pointPreview.preview.Position = position;
				_pointPreview.preview.Radius = target.Radius;
			}
		}

		public void ResizePointPreview()
		{
			ResizePointPreview(null);
		}

		public void ResizePointPreview(float radius)
		{
			ResizePointPreview(null, radius);
		}

		public void ResizePointPreview(RacePoint? point)
		{
			if (FixPoint(point, out var target))
			{
				ResizePointPreview(target, target.Radius);
			}
		}

		public void ResizePointPreview(RacePoint? point, float radius)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (FixPoint(point, out var target))
			{
				_pointPreview.point = target;
				_pointPreview.preview.Position = target.Position;
				_pointPreview.preview.Radius = radius;
			}
		}

		protected override void DrawRaceToWorld(SpriteBatch spriteBatch)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch2 = spriteBatch;
			BasicEffect effect = _effect;
			if (effect != null)
			{
				effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection());
				int length = Race.RoadPoints.Count();
				if (length >= 2)
				{
					((Effect)effect).get_CurrentTechnique().get_Passes().get_Item(0)
						.Apply();
					VertexPosition[] vertices = (VertexPosition[])(object)new VertexPosition[length];
					foreach (var (i, p) in Race.RoadPoints.Enumerate())
					{
						vertices[i] = new VertexPosition(p.Position);
					}
					((GraphicsResource)spriteBatch2).get_GraphicsDevice().DrawUserPrimitives<VertexPosition>((PrimitiveType)3, vertices, 0, length - 1);
				}
			}
			foreach (RacePoint point2 in Race.RacePoints)
			{
				drawPoint(point2);
			}
			foreach (RawPoint value in _rawPoints.Values)
			{
				SpriteBatch spriteBatch3 = spriteBatch2;
				Vector3 position = value.Position;
				DrawRacePoint(spriteBatch3, position, 1f, (Color)(value.Type switch
				{
					"checkpoint" => Color.get_White(), 
					"flag" => Color.get_Yellow(), 
					"boost" => Color.get_Blue(), 
					"obstacle" => Color.get_Red(), 
					_ => Color.get_Black(), 
				}), flat: true);
			}
			void drawPoint(RacePoint point)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				bool selected = point == Selected;
				Color color = (Color)(point.Type switch
				{
					RacePointType.Guide => RaceDrawer.GuidePointColor, 
					RacePointType.Reset => RaceDrawer.ResetColor, 
					_ => point.Collides(Measurer.Pos.Meters) ? RaceDrawer.TouchedCheckpointColor : RaceDrawer.CheckpointColor, 
				});
				RacePoint preview = ((_pointPreview.point == point) ? _pointPreview.preview : point);
				if (!selected)
				{
					((Color)(ref color)).set_A((byte)64);
				}
				DrawRacePoint(spriteBatch2, preview, color, point.Type == RacePointType.Guide);
				if (selected && point.IsCheckpoint && Draw300InGuide)
				{
					DrawRacePoint(spriteBatch2, preview.Position, 7.62f, color);
				}
			}
		}

		protected override void DrawRaceToMap(SpriteBatch spriteBatch, IMapBounds map)
		{
			SpriteBatch spriteBatch2 = spriteBatch;
			IMapBounds map2 = map;
			DrawMapLine(spriteBatch2, map2);
			foreach (var (point2, j) in Race.RacePoints.Select((RacePoint c, int i) => (c, i)))
			{
				drawPoint(point2, $"{j + 1}");
			}
			void drawPoint(RacePoint point, string text)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				bool selected = point == Selected;
				Color color = (Color)(point.Type switch
				{
					RacePointType.Guide => RaceDrawer.GuidePointColor, 
					RacePointType.Reset => RaceDrawer.ResetColor, 
					_ => RaceDrawer.CheckpointColor, 
				});
				RacePoint preview = ((_pointPreview.point == point) ? _pointPreview.preview : point);
				if (selected)
				{
					ShapeExtensions.DrawCircle(spriteBatch2, map2.FromWorld(Race.MapId, preview.Position), 20f, 4, color, 2f, 0f);
				}
				else
				{
					DrawMapRacePoint(spriteBatch2, map2, preview, color);
				}
				DrawText(spriteBatch2, map2.FromWorld(Race.MapId, preview.Position), Control.get_Content().get_DefaultFont14(), Color.get_White(), text);
			}
		}

		protected override void DisposeControl()
		{
			_disposed = true;
			BasicEffect? effect = _effect;
			if (effect != null)
			{
				((GraphicsResource)effect).Dispose();
			}
			base.DisposeControl();
		}
	}
}
