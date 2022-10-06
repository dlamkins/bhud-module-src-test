using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.RacingMeter
{
	public class RaceEditor : RaceHolder
	{
		private BasicEffect _effect;

		private readonly CommandList _commands;

		private readonly EditState _state;

		private (RacePoint point, RacePoint preview) _pointPreview = (null, new RacePoint());

		public override FullRace FullRace
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

		public bool Draw300InGuide { get; set; }

		public RacePoint Selected => _state.Selected;

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

		public event Action LocalRacesChanged;

		public RaceEditor(CommandList commands)
		{
			_commands = commands;
			_state = commands.State;
			RaceLoaded += delegate
			{
				_commands.Clear();
			};
			RacingModule.MetaPanel.PanelOverride = new EditorPanel(this);
			Control.get_Graphics().QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected O, but got Unknown
				if (_effect == null)
				{
					_effect = new BasicEffect(graphicsDevice);
				}
			});
		}

		public void Test(RacePoint testPoint)
		{
			if (IsReady(ref testPoint))
			{
				int index = base.Race.RacePoints.IndexOf(testPoint);
				(int, RacePoint) p2 = base.Race.RacePoints.Enumerate().Reverse().FirstOrDefault(((int index, RacePoint item) p) => p.item.IsCheckpoint && p.index <= index);
				int i = base.Race.Checkpoints.ToList().IndexOf(p2.Item2);
				Test((i >= 0) ? i : 0);
			}
		}

		public void Test(int testCheckpoint = 0)
		{
			if (base.Race != null)
			{
				RacingModule.MetaPanel.PanelOverride = new TestPanel();
				RacingModule.Racer.Run(testCheckpoint);
			}
		}

		public void DiscardChanges()
		{
			if (FullRace != null)
			{
				RacingModule.Racer.FullRace = DataInterface.GetLocalRace(FullRace.Meta.Id);
				this.LocalRacesChanged?.Invoke();
			}
		}

		public void DeleteRace()
		{
			if (FullRace != null)
			{
				DataInterface.DeleteRace(FullRace);
				RacingModule.Racer.FullRace = null;
				this.LocalRacesChanged?.Invoke();
			}
		}

		private void Undo(object sender, EventArgs e)
		{
			Undo();
		}

		private void Redo(object sender, EventArgs e)
		{
			Redo();
		}

		private void Save(object sender, EventArgs e)
		{
			Save();
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
			if (FullRace != null)
			{
				DataInterface.SaveRace(FullRace);
				_commands.Saved();
				ScreenNotification.ShowNotification(Strings.NotifyRaceSaved, (NotificationType)0, (Texture2D)null, 4);
				this.LocalRacesChanged?.Invoke();
			}
		}

		public string Describe(RacePoint point)
		{
			if (point == null || base.Race == null)
			{
				return null;
			}
			int i = base.Race.RacePoints.IndexOf(point);
			if (i < 0)
			{
				return null;
			}
			return Strings.PointDescription.Format(i + 1, point.Type.Describe());
		}

		private bool IsReady(out RacePoint point)
		{
			point = null;
			return IsReady(ref point);
		}

		private bool IsReady(ref RacePoint point, bool nullPointOk = false)
		{
			if (base.Race != null)
			{
				return (point ?? (point = Selected)) != null || nullPointOk;
			}
			return false;
		}

		public void RenameRace(string name)
		{
			if (base.Race != null)
			{
				_commands.Run(new RenameRaceCommand(name));
			}
		}

		public void SetRaceType(RaceType type)
		{
			if (base.Race != null)
			{
				_commands.Run(new SetRaceTypeCommand(type));
			}
		}

		public void SetRaceMap(int mapId)
		{
			if (base.Race != null)
			{
				_commands.Run(new SetRaceMapCommand(mapId));
			}
		}

		public void Select(RacePoint point)
		{
			_commands.Run(new SelectPointCommand(point));
		}

		public void SelectNearest()
		{
			Select(base.Race?.RacePoints.MinBy((RacePoint c) => Vector3.DistanceSquared(c.Position, RacingModule.Measurer.Pos.Meters)));
		}

		public void SelectPrevious(RacePoint point = null)
		{
			if (IsReady(ref point, nullPointOk: true))
			{
				int i = _state.PointIndexOf(point);
				Select((i < 0 || point == null) ? base.Race.RacePoints.Last() : base.Race.RacePoints.Skip(i - 1).First());
			}
		}

		public void SelectNext(RacePoint point = null)
		{
			if (IsReady(ref point, nullPointOk: true))
			{
				int i = _state.PointIndexOf(point);
				Select((i < 0 || point == null) ? base.Race.RacePoints.First() : base.Race.RacePoints.Skip(Math.Min(i + 1, base.Race.RacePoints.Count - 1)).First());
			}
		}

		public void InsertCheckpoint(bool before)
		{
			InsertCheckpoint(null, before);
		}

		public void InsertCheckpoint(RacePoint existing, bool before)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (IsReady(ref existing, nullPointOk: true))
			{
				int i = _state.PointIndexOf(existing);
				i = ((i < 0) ? ((!before) ? base.Race.RacePoints.Count : 0) : (before ? i : (i + 1)));
				_commands.Run(new InsertPointCommand(i, new RacePoint
				{
					Position = RacingModule.Measurer.Pos.Meters,
					Radius = (Selected?.Radius ?? 15f),
					Type = (Selected?.Type ?? RacePointType.Checkpoint)
				}));
			}
		}

		public void RemoveCheckpoint(RacePoint point = null)
		{
			if (IsReady(ref point))
			{
				_commands.Run(new RemovePointCommand(point));
			}
		}

		public void SwapCheckpoint(bool previous)
		{
			SwapCheckpoint(null, previous);
		}

		public void SwapCheckpoint(RacePoint point, bool previous)
		{
			if (IsReady(ref point))
			{
				_commands.Run(new SwapPointCommand(point, previous));
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
			if (IsReady(out var point))
			{
				MovePoint(point, new Vector3(x ?? point.Position.X, y ?? point.Position.Y, z ?? point.Position.Z));
			}
		}

		public void MovePoint(RacePoint point, Vector3 position)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_pointPreview.point = null;
			if (IsReady(ref point))
			{
				_commands.Run(new MovePointCommand(point, position));
			}
		}

		public void ResizePoint(float radius)
		{
			ResizePoint(null, radius);
		}

		public void ResizePoint(RacePoint point, float radius)
		{
			_pointPreview.point = null;
			if (IsReady(ref point))
			{
				_commands.Run(new ResizePointCommand(point, radius));
			}
		}

		public void SetPointType(RacePointType type)
		{
			SetPointType(null, type);
		}

		public void SetPointType(RacePoint point, RacePointType type)
		{
			if (IsReady(ref point))
			{
				_commands.Run(new SetPointTypeCommand(point, type));
			}
		}

		public void MovePointPreview(Vector3 position)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			MovePointPreview(null, position);
		}

		public void MovePointPreview(RacePoint point, Vector3 position)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (IsReady(ref point))
			{
				_pointPreview.point = point;
				_pointPreview.preview.Position = position;
				_pointPreview.preview.Radius = point.Radius;
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

		public void ResizePointPreview(RacePoint point)
		{
			if (IsReady(ref point))
			{
				ResizePointPreview(point, point.Radius);
			}
		}

		public void ResizePointPreview(RacePoint point, float radius)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (IsReady(ref point))
			{
				_pointPreview.point = point;
				_pointPreview.preview.Position = point.Position;
				_pointPreview.preview.Radius = radius;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			if (!CanDraw())
			{
				return;
			}
			if (_effect != null)
			{
				_effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection());
				int length = base.Race.RoadPoints.Count();
				if (length >= 2)
				{
					((Effect)_effect).get_CurrentTechnique().get_Passes().get_Item(0)
						.Apply();
					VertexPosition[] vertices = (VertexPosition[])(object)new VertexPosition[length];
					foreach (var (i, p) in base.Race.RoadPoints.Enumerate())
					{
						vertices[i] = new VertexPosition(p.Position);
					}
					((GraphicsResource)spriteBatch).get_GraphicsDevice().DrawUserPrimitives<VertexPosition>((PrimitiveType)3, vertices, 0, length - 1);
				}
			}
			foreach (RacePoint point2 in base.Race.RacePoints)
			{
				drawPoint(point2);
			}
			void drawPoint(RacePoint point)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				bool selected = point == Selected;
				Color color = (Color)(point.Type switch
				{
					RacePointType.Guide => RaceHolder.GuidePointColor, 
					RacePointType.Reset => RaceHolder.ResetColor, 
					_ => point.Collides(RacingModule.Measurer.Pos.Meters) ? RaceHolder.TouchedCheckpointColor : RaceHolder.CheckpointColor, 
				});
				RacePoint preview = ((_pointPreview.point == point) ? _pointPreview.preview : point);
				if (!selected)
				{
					((Color)(ref color)).set_A((byte)64);
				}
				DrawRacePoint(spriteBatch, preview, color, point.Type == RacePointType.Guide);
				if (selected && point.IsCheckpoint && Draw300InGuide)
				{
					DrawRacePoint(spriteBatch, preview.Position, 7.62f, color);
				}
			}
		}

		public override void DrawToMap(SpriteBatch spriteBatch, MapBounds map)
		{
			if (base.Race == null)
			{
				return;
			}
			DrawMapLine(spriteBatch, map);
			foreach (var (point2, j) in base.Race.RacePoints.Select((RacePoint c, int i) => (c, i)))
			{
				drawPoint(point2, $"{j + 1}");
			}
			void drawPoint(RacePoint point, string text = null)
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
					RacePointType.Guide => RaceHolder.GuidePointColor, 
					RacePointType.Reset => RaceHolder.ResetColor, 
					_ => RaceHolder.CheckpointColor, 
				});
				RacePoint preview = ((_pointPreview.point == point) ? _pointPreview.preview : point);
				if (selected)
				{
					ShapeExtensions.DrawCircle(spriteBatch, map.FromWorld(base.Race.MapId, preview.Position), 20f, 4, color, 2f, 0f);
				}
				else
				{
					DrawMapRacePoint(spriteBatch, map, preview, color);
				}
				DrawText(spriteBatch, map.FromWorld(base.Race.MapId, preview.Position), Control.get_Content().get_DefaultFont14(), Color.get_White(), text);
			}
		}

		protected override void DisposeControl()
		{
			if (RacingModule.MetaPanel.PanelOverride is EditorPanel)
			{
				RacingModule.MetaPanel.PanelOverride = null;
			}
			base.DisposeControl();
		}
	}
}
