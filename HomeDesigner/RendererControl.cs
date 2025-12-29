using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using HomeDesigner.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomeDesigner
{
	public class RendererControl : Control
	{
		public enum GizmoMode
		{
			Translate,
			Rotate,
			Scale
		}

		public enum RotationSpace
		{
			World,
			Local
		}

		public enum TransformMode
		{
			Translate,
			Rotate,
			Scale
		}

		public enum SelectionMode
		{
			None,
			RectangleStart,
			RectangleEnd,
			RectangleHeight,
			PolygonPoints,
			PolygonHeight
		}

		private readonly BlueprintRenderer _blueprintRenderer;

		public int internalObjectId = 1;

		public int historyPosition;

		public GizmoMode gizmoMode;

		private Vector3 pivotObject = Vector3.get_Zero();

		private Quaternion pivotRotation = Quaternion.get_Identity();

		public RotationSpace _rotationSpace;

		public TransformMode currentMode;

		private bool _gizmoActive;

		private BlueprintObject activeGizmo;

		private Dictionary<BlueprintObject, Vector3> _startPositions = new Dictionary<BlueprintObject, Vector3>();

		private bool _dragInitialized;

		private Vector3 _lastProjectedPoint;

		private Dictionary<BlueprintObject, Quaternion> _startRotations = new Dictionary<BlueprintObject, Quaternion>();

		private Vector3 _rotationStartVec;

		private float _snapAccum;

		private const float SnapStepSize = (float)Math.PI / 4f;

		private const float SnapThreshold = 0.7f;

		private bool _prevShift;

		public SelectionMode _selectionMode;

		private Vector3 _rectStart;

		private Vector3 _rectEnd;

		private float _areaHeight;

		private float _dragStartMouseY;

		private List<Vector3> _polygonPoints = new List<Vector3>();

		private bool _isSelectingPolygon;

		private BasicEffect _selectionEffect;

		public float planeZ;

		public bool multiLassoSelect;

		public List<BlueprintObject> Objects { get; } = new List<BlueprintObject>();


		public List<BlueprintObject> SelectedObjects { get; } = new List<BlueprintObject>();


		public List<BlueprintObject> BackupObjects { get; } = new List<BlueprintObject>();


		public Dictionary<int, List<BlueprintObject>> HistoryList { get; } = new Dictionary<int, List<BlueprintObject>>();


		public List<BlueprintObject> TranslateGizmos { get; } = new List<BlueprintObject>();


		public List<BlueprintObject> RotateGizmos { get; } = new List<BlueprintObject>();


		public List<BlueprintObject> ScaleGizmos { get; } = new List<BlueprintObject>();


		public List<BlueprintObject> CopiedObjects { get; } = new List<BlueprintObject>();


		public bool IsSelectingPolygon => _isSelectingPolygon;

		public RendererControl(BlueprintRenderer renderer)
			: this()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			_blueprintRenderer = renderer;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Visible(true);
			((Control)this).set_ZIndex(-1000);
			if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1596)
			{
				planeZ = 1f;
			}
			else if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1558)
			{
				planeZ = 15f;
			}
			else
			{
				planeZ = 0f;
			}
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
				((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			});
			BasicEffect val = new BasicEffect(_blueprintRenderer.GraphicsDevice);
			val.set_VertexColorEnabled(true);
			_selectionEffect = val;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public BlueprintRenderer getBlueprintrenderer()
		{
			return _blueprintRenderer;
		}

		public void updateWorld()
		{
			_blueprintRenderer.PrecomputeWorlds(Objects);
		}

		public void updateGizmos()
		{
			_blueprintRenderer.PrecomputeGizmoWorlds(TranslateGizmos);
			_blueprintRenderer.PrecomputeGizmoWorlds(RotateGizmos);
			_blueprintRenderer.PrecomputeGizmoWorlds(ScaleGizmos);
		}

		public void AddObject(BlueprintObject obj)
		{
			Objects.Add(obj);
			_blueprintRenderer.PrecomputeWorlds(Objects);
		}

		public void RemoveObject(BlueprintObject obj)
		{
			Objects.Remove(obj);
			_blueprintRenderer.PrecomputeWorlds(Objects);
		}

		public void AddTranslateGizmos(BlueprintObject obj)
		{
			TranslateGizmos.Add(obj);
		}

		public void AddRotateGizmos(BlueprintObject obj)
		{
			RotateGizmos.Add(obj);
		}

		public void AddScaleGizmos(BlueprintObject obj)
		{
			ScaleGizmos.Add(obj);
		}

		public void ClearSelection()
		{
			foreach (BlueprintObject @object in Objects)
			{
				@object.Selected = false;
			}
			SelectedObjects.Clear();
			clearPivotObject();
			clearPivotRotation();
			updateBackupObjects();
		}

		public void SelectObject(BlueprintObject obj, bool multiSelect = false)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!multiSelect)
			{
				ClearSelection();
			}
			obj.Selected = true;
			SelectedObjects.Add(obj);
			if (_rotationSpace == RotationSpace.Local)
			{
				pivotRotation = obj.RotationQuaternion;
			}
			else
			{
				pivotRotation = Quaternion.get_Identity();
			}
			updateGizmos();
			updateBackupObjects();
		}

		private void updateBackupObjects()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			BackupObjects.Clear();
			foreach (BlueprintObject obj in SelectedObjects)
			{
				BackupObjects.Add(new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = obj.Position,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					CachedWorld = obj.CachedWorld,
					InternalId = obj.InternalId,
					IsOriginal = false
				});
			}
		}

		public void loadHistory()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			Objects.Clear();
			foreach (BlueprintObject obj in HistoryList[historyPosition])
			{
				Objects.Add(new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = obj.Position,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					CachedWorld = obj.CachedWorld,
					InternalId = obj.InternalId,
					IsOriginal = obj.IsOriginal,
					Selected = obj.Selected,
					BoundingBox = obj.BoundingBox
				});
			}
		}

		public void resetHistory()
		{
			HistoryList.Clear();
			historyPosition = 0;
		}

		public void updateHistoryList()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			List<BlueprintObject> histList = new List<BlueprintObject>();
			foreach (BlueprintObject obj in Objects)
			{
				histList.Add(new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = obj.Position,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					CachedWorld = obj.CachedWorld,
					InternalId = obj.InternalId,
					IsOriginal = obj.IsOriginal,
					Selected = obj.Selected,
					BoundingBox = obj.BoundingBox
				});
			}
			foreach (int key in HistoryList.Keys.Where((int k) => k > historyPosition).ToList())
			{
				HistoryList.Remove(key);
			}
			historyPosition++;
			HistoryList.Add(historyPosition, histList);
		}

		public Vector3 getPivotObject()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return pivotObject;
		}

		public void clearPivotObject()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			pivotObject = Vector3.get_Zero();
		}

		public void setPivotRotation(Quaternion rot)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			pivotRotation = rot;
		}

		public Quaternion getPivotRotation()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return pivotRotation;
		}

		public void clearPivotRotation()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			pivotRotation = Quaternion.get_Identity();
		}

		public void updateWorldPivot()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (!_blueprintRenderer._modelPivots.TryGetValue(SelectedObjects[0].ModelKey, out var pivotLocal))
			{
				pivotLocal = Vector3.get_Zero();
			}
			pivotObject = Vector3.Transform(pivotLocal, SelectedObjects[0].CachedWorld);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice graphicsDevice = _blueprintRenderer.GraphicsDevice;
			graphicsDevice.set_DepthStencilState(DepthStencilState.Default);
			graphicsDevice.set_RasterizerState(RasterizerState.CullNone);
			Matrix view = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Matrix projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			_blueprintRenderer.Draw(view, projection, Objects);
			if (SelectedObjects.Count > 0)
			{
				updateWorldPivot();
				switch (gizmoMode)
				{
				case GizmoMode.Translate:
					paintGizmo(view, projection, TranslateGizmos);
					break;
				case GizmoMode.Rotate:
					paintGizmo(view, projection, RotateGizmos);
					break;
				case GizmoMode.Scale:
					paintGizmo(view, projection, ScaleGizmos);
					break;
				}
				if (BackupObjects.Count > 0)
				{
					_blueprintRenderer.Draw(view, projection, BackupObjects);
				}
			}
			if (_selectionEffect == null)
			{
				InitializeSelectionEffect(_blueprintRenderer.GraphicsDevice);
			}
			if (_selectionMode == SelectionMode.RectangleStart || _selectionMode == SelectionMode.RectangleEnd || _selectionMode == SelectionMode.RectangleHeight)
			{
				Point mouse = GameService.Input.get_Mouse().get_Position();
				spriteBatch.Draw(_blueprintRenderer.contentManager.GetTexture("Icons/Mouse_Rectangle.png"), new Rectangle(mouse.X + 32, mouse.Y + 32, 32, 32), (Rectangle?)null, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0, 0f);
			}
			else if (_selectionMode == SelectionMode.PolygonPoints || _selectionMode == SelectionMode.PolygonHeight)
			{
				Point mouse2 = GameService.Input.get_Mouse().get_Position();
				spriteBatch.Draw(_blueprintRenderer.contentManager.GetTexture("Icons/Mouse_Lasso.png"), new Rectangle(mouse2.X + 32, mouse2.Y + 32, 32, 32), (Rectangle?)null, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0, 0f);
			}
			if (_selectionMode == SelectionMode.RectangleEnd)
			{
				Ray ray = CreateRayFromMouse();
				if (RaycastGround(ray, out var current))
				{
					DrawSelectionRectangle(_rectStart, current, Color.get_LimeGreen());
				}
			}
			else if (_selectionMode == SelectionMode.RectangleHeight)
			{
				setAreaHeight();
				DrawCuboidPreview(_rectStart, _rectEnd, _areaHeight);
			}
			else if (_selectionMode == SelectionMode.PolygonPoints)
			{
				if (_polygonPoints.Count > 0)
				{
					DrawPolygon(_polygonPoints);
				}
			}
			else if (_selectionMode == SelectionMode.PolygonHeight)
			{
				setAreaHeight();
				DrawPolygonPrismPreview(_polygonPoints, _areaHeight);
			}
		}

		private void paintGizmo(Matrix view, Matrix projection, List<BlueprintObject> gizmolist)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			foreach (BlueprintObject item in gizmolist)
			{
				item.Position = pivotObject;
				item.RotationQuaternion = pivotRotation;
			}
			_blueprintRenderer.PrecomputeGizmoWorlds(gizmolist);
			_blueprintRenderer.DrawGizmo(view, projection, gizmolist, activeGizmo);
		}

		private void OnLeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (Control.get_ActiveControl() != this)
			{
				return;
			}
			bool num = GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)162);
			bool altDown = GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)164);
			if (!num)
			{
				if (altDown)
				{
					RaycastSelect(e, multi: true);
				}
				else
				{
					RaycastSelect(e);
				}
			}
		}

		private void RaycastSelect(MouseEventArgs e, bool multi = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Ray ray = CreateRayFromMouse();
			bool isGizmo = false;
			BlueprintObject closest = null;
			GetClosestObject(ray, out closest, out isGizmo);
			if (closest != null && !isGizmo)
			{
				if (multi)
				{
					if (closest.Selected)
					{
						SelectedObjects.Remove(closest);
						updateBackupObjects();
						closest.Selected = false;
					}
					else
					{
						SelectObject(closest, multiSelect: true);
					}
				}
				else if (closest.Selected)
				{
					ClearSelection();
				}
				else
				{
					SelectObject(closest);
				}
			}
			else if (closest != null && isGizmo)
			{
				_gizmoActive = true;
				activeGizmo = closest;
				switch (currentMode)
				{
				case TransformMode.Translate:
					GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)GizmoTranslate);
					break;
				case TransformMode.Rotate:
					GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)GizmoRotate);
					break;
				case TransformMode.Scale:
					GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)GizmoScale);
					break;
				}
				GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			}
		}

		private BlueprintObject GetClosestObject(Ray ray, out BlueprintObject closest, out bool isGizmo)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			closest = null;
			isGizmo = false;
			float minDist = float.MaxValue;
			if (SelectedObjects.Count > 0)
			{
				switch (gizmoMode)
				{
				case GizmoMode.Translate:
					foreach (BlueprintObject obj2 in TranslateGizmos)
					{
						float? dist2 = ((Ray)(ref ray)).Intersects(obj2.BoundingBox);
						if (dist2.HasValue && dist2.Value < minDist)
						{
							closest = obj2;
							minDist = dist2.Value;
						}
					}
					break;
				case GizmoMode.Rotate:
					foreach (BlueprintObject obj3 in RotateGizmos)
					{
						float? dist3 = ((Ray)(ref ray)).Intersects(obj3.BoundingBox);
						if (dist3.HasValue && dist3.Value < minDist)
						{
							closest = obj3;
							minDist = dist3.Value;
						}
					}
					break;
				case GizmoMode.Scale:
					foreach (BlueprintObject obj4 in ScaleGizmos)
					{
						float? dist4 = ((Ray)(ref ray)).Intersects(obj4.BoundingBox);
						if (dist4.HasValue && dist4.Value < minDist)
						{
							closest = obj4;
							minDist = dist4.Value;
						}
					}
					break;
				}
				if (closest != null)
				{
					isGizmo = true;
					return closest;
				}
			}
			foreach (BlueprintObject obj in Objects)
			{
				float? dist = ((Ray)(ref ray)).Intersects(obj.BoundingBox);
				if (dist.HasValue && dist.Value < minDist)
				{
					closest = obj;
					minDist = dist.Value;
				}
			}
			isGizmo = false;
			return closest;
		}

		private Ray CreateRayFromMouse()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Point position = GameService.Input.get_Mouse().get_Position();
			Viewport vp = _blueprintRenderer.GraphicsDevice.get_Viewport();
			float x = (float)position.X / (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width() * (float)((Viewport)(ref vp)).get_Width();
			float y = (float)position.Y / (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height() * (float)((Viewport)(ref vp)).get_Height();
			Vector2 mousePos = default(Vector2);
			((Vector2)(ref mousePos))._002Ector(x, y);
			Vector3 near = ((Viewport)(ref vp)).Unproject(new Vector3(mousePos, 0f), GameService.Gw2Mumble.get_PlayerCamera().get_Projection(), GameService.Gw2Mumble.get_PlayerCamera().get_View(), Matrix.get_Identity());
			Vector3 dir = Vector3.Normalize(((Viewport)(ref vp)).Unproject(new Vector3(mousePos, 1f), GameService.Gw2Mumble.get_PlayerCamera().get_Projection(), GameService.Gw2Mumble.get_PlayerCamera().get_View(), Matrix.get_Identity()) - near);
			return new Ray(near, dir);
		}

		private void GizmoTranslate(object sender, MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			if (!_gizmoActive || activeGizmo == null)
			{
				return;
			}
			Ray ray = CreateRayFromMouse();
			Vector3 gizmoOrigin = SelectedObjects.First().Position;
			Vector3 axisDir = Vector3.get_UnitX();
			if (activeGizmo.ModelKey.Contains("Y"))
			{
				axisDir = Vector3.get_UnitY();
			}
			if (activeGizmo.ModelKey.Contains("Z"))
			{
				axisDir = Vector3.get_UnitZ();
			}
			if (_rotationSpace == RotationSpace.Local)
			{
				axisDir = Vector3.Transform(axisDir, getPivotRotation());
			}
			Vector3 projectedPoint = ClosestPointBetweenRayAndLine(ray, gizmoOrigin, axisDir);
			if (!_dragInitialized)
			{
				_lastProjectedPoint = projectedPoint;
				_dragInitialized = true;
				return;
			}
			Vector3 val = projectedPoint - _lastProjectedPoint;
			_lastProjectedPoint = projectedPoint;
			float movement = Vector3.Dot(val, axisDir);
			Vector3 offset = axisDir * movement;
			foreach (BlueprintObject selectedObject in SelectedObjects)
			{
				selectedObject.Position += offset;
			}
			_blueprintRenderer.PrecomputeWorlds(SelectedObjects);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
		}

		private Vector3 ClosestPointBetweenRayAndLine(Ray ray, Vector3 lineOrigin, Vector3 lineDir)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			((Vector3)(ref lineDir)).Normalize();
			Vector3 diff = ray.Position - lineOrigin;
			float a = Vector3.Dot(ray.Direction, ray.Direction);
			float b = Vector3.Dot(ray.Direction, lineDir);
			float c = Vector3.Dot(lineDir, lineDir);
			float d = Vector3.Dot(ray.Direction, diff);
			float e = Vector3.Dot(lineDir, diff);
			float denom = a * c - b * b;
			if (Math.Abs(denom) < 1E-06f)
			{
				return lineOrigin;
			}
			float t = (b * e - c * d) / denom;
			float s = (a * e - b * d) / denom;
			_ = ray.Position + ray.Direction * t;
			return lineOrigin + lineDir * s;
		}

		private void GizmoRotate(object sender, MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			if (!_gizmoActive || activeGizmo == null)
			{
				return;
			}
			Ray ray = CreateRayFromMouse();
			Vector3 pivot = pivotObject;
			Vector3 axisDir = Vector3.get_UnitX();
			if (activeGizmo.ModelKey.Contains("Y"))
			{
				axisDir = Vector3.get_UnitY();
			}
			if (activeGizmo.ModelKey.Contains("Z"))
			{
				axisDir = Vector3.get_UnitZ();
			}
			if (_rotationSpace == RotationSpace.Local)
			{
				axisDir = Vector3.Transform(axisDir, getPivotRotation());
			}
			Plane rotationPlane = default(Plane);
			((Plane)(ref rotationPlane))._002Ector(axisDir, 0f - Vector3.Dot(axisDir, pivot));
			if (!RayIntersectsPlane(ray, rotationPlane, out var hitPoint))
			{
				return;
			}
			Vector3 dir = Vector3.Normalize(hitPoint - pivot);
			KeyboardState state = GameService.Input.get_Keyboard().get_State();
			bool shift = ((KeyboardState)(ref state)).IsKeyDown((Keys)160);
			if (!_dragInitialized)
			{
				_rotationStartVec = dir;
				_snapAccum = 0f;
				_prevShift = shift;
				_dragInitialized = true;
				return;
			}
			if (shift && !_prevShift)
			{
				_rotationStartVec = dir;
				_snapAccum = 0f;
				foreach (BlueprintObject obj4 in SelectedObjects)
				{
					_startRotations[obj4] = obj4.RotationQuaternion;
					_startPositions[obj4] = obj4.Position;
				}
			}
			if (!shift && _prevShift)
			{
				_rotationStartVec = dir;
				_snapAccum = 0f;
				foreach (BlueprintObject obj3 in SelectedObjects)
				{
					_startRotations[obj3] = obj3.RotationQuaternion;
					_startPositions[obj3] = obj3.Position;
				}
			}
			_prevShift = shift;
			float angle = (float)Math.Acos(MathHelper.Clamp(Vector3.Dot(_rotationStartVec, dir), -1f, 1f));
			float sign = Math.Sign(Vector3.Dot(Vector3.Cross(_rotationStartVec, dir), axisDir));
			angle *= sign;
			angle *= 1.5f;
			float finalAngle = angle;
			if (shift)
			{
				_snapAccum += angle;
				if (!(Math.Abs(_snapAccum) >= 0.7f))
				{
					_rotationStartVec = dir;
					return;
				}
				_ = _snapAccum / 0.7f;
				finalAngle = (float)Math.Sign(_snapAccum) * ((float)Math.PI / 4f);
				_snapAccum = 0f;
				_rotationStartVec = dir;
				foreach (BlueprintObject obj2 in SelectedObjects)
				{
					_startRotations[obj2] = obj2.RotationQuaternion;
					_startPositions[obj2] = obj2.Position;
				}
			}
			else
			{
				_snapAccum = 0f;
			}
			Quaternion deltaRot = Quaternion.CreateFromAxisAngle(axisDir, finalAngle);
			foreach (BlueprintObject obj in SelectedObjects)
			{
				if (!_startPositions.ContainsKey(obj))
				{
					_startPositions[obj] = obj.Position;
				}
				if (!_startRotations.ContainsKey(obj))
				{
					_startRotations[obj] = obj.RotationQuaternion;
				}
				Vector3 rotatedOffset = Vector3.Transform(_startPositions[obj] - pivot, deltaRot);
				obj.Position = pivot + rotatedOffset;
				obj.RotationQuaternion = deltaRot * _startRotations[obj];
			}
			_blueprintRenderer.PrecomputeWorlds(SelectedObjects);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
		}

		private bool RayIntersectsPlane(Ray ray, Plane plane, out Vector3 intersection)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			float denom = Vector3.Dot(plane.Normal, ray.Direction);
			if (Math.Abs(denom) < 1E-06f)
			{
				intersection = Vector3.get_Zero();
				return false;
			}
			float t = (0f - (Vector3.Dot(plane.Normal, ray.Position) + plane.D)) / denom;
			intersection = ray.Position + ray.Direction * t;
			return t >= 0f;
		}

		private void GizmoScale(object sender, MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			if (!_gizmoActive)
			{
				return;
			}
			Ray ray = CreateRayFromMouse();
			Vector3 gizmoOrigin = SelectedObjects.First().Position;
			Vector3 axisDir = Vector3.get_UnitX();
			if (activeGizmo.ModelKey.Contains("Y"))
			{
				axisDir = Vector3.get_UnitY();
			}
			if (activeGizmo.ModelKey.Contains("Z"))
			{
				axisDir = Vector3.get_UnitZ();
			}
			Vector3 projectedPoint = ClosestPointBetweenRayAndLine(ray, gizmoOrigin, axisDir);
			if (!_dragInitialized)
			{
				_lastProjectedPoint = projectedPoint;
				_dragInitialized = true;
				return;
			}
			Vector3 val = projectedPoint - _lastProjectedPoint;
			_lastProjectedPoint = projectedPoint;
			float movement = Vector3.Dot(val, axisDir);
			float scaleFactor = 1f + movement * 1.2f;
			scaleFactor = MathHelper.Clamp(scaleFactor, 0.5f, 1.5f);
			foreach (BlueprintObject selectedObject in SelectedObjects)
			{
				float newScale = selectedObject.Scale * scaleFactor;
				newScale = (selectedObject.Scale = MathHelper.Clamp(newScale, 0.1f, 2f));
			}
			_blueprintRenderer.PrecomputeWorlds(SelectedObjects);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
		}

		private void OnGizmoConfirm(object sender, MouseEventArgs e)
		{
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			if (!_gizmoActive)
			{
				return;
			}
			_gizmoActive = false;
			_dragInitialized = false;
			activeGizmo = null;
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoTranslate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoRotate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoScale);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
			_startRotations.Clear();
			_startPositions.Clear();
			foreach (BlueprintObject obj in SelectedObjects)
			{
				_startRotations[obj] = obj.RotationQuaternion;
				_startPositions[obj] = obj.Position;
			}
			if (_rotationSpace == RotationSpace.World)
			{
				setPivotRotation(Quaternion.get_Identity());
			}
			else
			{
				setPivotRotation(SelectedObjects[0].RotationQuaternion);
			}
			updateGizmos();
			updateHistoryList();
			updateBackupObjects();
		}

		private void OnGizmoKeyPress(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Invalid comparison between Unknown and I4
			if ((int)e.get_Key() == 84)
			{
				if (!_gizmoActive)
				{
					return;
				}
				_gizmoActive = false;
				_dragInitialized = false;
				activeGizmo = null;
				GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoTranslate);
				GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoRotate);
				GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoScale);
				GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
				GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
				GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
				_startRotations.Clear();
				_startPositions.Clear();
				if (_rotationSpace == RotationSpace.World)
				{
					setPivotRotation(Quaternion.get_Identity());
					updateGizmos();
				}
				Dictionary<int, BlueprintObject> map = SelectedObjects.ToDictionary((BlueprintObject o) => o.InternalId);
				foreach (BlueprintObject backup in BackupObjects)
				{
					if (map.TryGetValue(backup.InternalId, out var original))
					{
						original.ModelKey = backup.ModelKey;
						original.Id = backup.Id;
						original.Name = backup.Name;
						original.Position = backup.Position;
						original.Rotation = backup.Rotation;
						original.RotationQuaternion = backup.RotationQuaternion;
						original.Scale = backup.Scale;
						original.CachedWorld = backup.CachedWorld;
						original.InternalId = backup.InternalId;
						original.IsOriginal = true;
					}
				}
			}
			else if ((int)e.get_Key() == 55)
			{
				placeCopy();
			}
		}

		public void placeCopy()
		{
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			if (!_gizmoActive)
			{
				return;
			}
			_gizmoActive = false;
			_dragInitialized = false;
			activeGizmo = null;
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoTranslate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoRotate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoScale);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
			_startRotations.Clear();
			_startPositions.Clear();
			if (_rotationSpace == RotationSpace.World)
			{
				setPivotRotation(Quaternion.get_Identity());
			}
			foreach (BlueprintObject obj in BackupObjects)
			{
				Objects.Add(new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = obj.Position,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					CachedWorld = obj.CachedWorld,
					InternalId = internalObjectId,
					IsOriginal = true,
					Selected = obj.Selected,
					BoundingBox = obj.BoundingBox
				});
				internalObjectId++;
			}
			ClearSelection();
			updateGizmos();
			updateHistoryList();
			updateBackupObjects();
		}

		public void InitializeSelectionEffect(GraphicsDevice device)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			BasicEffect val = new BasicEffect(device);
			val.set_VertexColorEnabled(true);
			_selectionEffect = val;
		}

		public void StartRectangleSelection()
		{
			ScreenNotification.ShowNotification("Rectangle selection started", (NotificationType)0, (Texture2D)null, 4);
			_selectionMode = SelectionMode.RectangleStart;
			_polygonPoints.Clear();
			_isSelectingPolygon = false;
		}

		public void PolygonSelection()
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (_selectionMode == SelectionMode.None)
			{
				_polygonPoints.Clear();
				_selectionMode = SelectionMode.PolygonPoints;
				ScreenNotification.ShowNotification("Lasso selection started", (NotificationType)0, (Texture2D)null, 4);
			}
			else if (_selectionMode == SelectionMode.PolygonPoints)
			{
				if (_polygonPoints == null || _polygonPoints.Count < 3)
				{
					ScreenNotification.ShowNotification("Too few points for a selection", (NotificationType)0, (Texture2D)null, 4);
					_polygonPoints.Clear();
					_selectionMode = SelectionMode.None;
				}
				else
				{
					_dragStartMouseY = GameService.Input.get_Mouse().get_Position().Y;
					_areaHeight = 0f;
					_selectionMode = SelectionMode.PolygonHeight;
				}
			}
		}

		public void CancelSelection()
		{
			_selectionMode = SelectionMode.None;
			_isSelectingPolygon = false;
			_polygonPoints.Clear();
		}

		public void OnSelectionClick(DesignerView view)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			if (_selectionMode == SelectionMode.None || GameService.Input.get_Mouse().get_ActiveControl() != this)
			{
				return;
			}
			Ray ray = CreateRayFromMouse();
			if (!RaycastGround(ray, out var _))
			{
				return;
			}
			if (_selectionMode == SelectionMode.RectangleStart)
			{
				if (RaycastGround(ray, out _rectStart))
				{
					_selectionMode = SelectionMode.RectangleEnd;
				}
			}
			else if (_selectionMode == SelectionMode.RectangleEnd)
			{
				if (RaycastGround(ray, out _rectEnd))
				{
					_areaHeight = 0f;
					_dragStartMouseY = GameService.Input.get_Mouse().get_Position().Y;
					_selectionMode = SelectionMode.RectangleHeight;
				}
			}
			else if (_selectionMode == SelectionMode.RectangleHeight)
			{
				SelectObjectsInCuboid();
				_selectionMode = SelectionMode.None;
			}
			else if (_selectionMode == SelectionMode.PolygonPoints)
			{
				if (RaycastGround(ray, out var corner))
				{
					_polygonPoints.Add(corner);
				}
			}
			else if (_selectionMode == SelectionMode.PolygonHeight)
			{
				SelectObjectsInPolygon();
				_selectionMode = SelectionMode.None;
			}
		}

		private bool RaycastGround(Ray ray, out Vector3 hit)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			if (Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				hit = Vector3.get_Zero();
				return false;
			}
			float t = (planeZ - ray.Position.Z) / ray.Direction.Z;
			if (t <= 0f)
			{
				hit = Vector3.get_Zero();
				return false;
			}
			hit = ray.Position + ray.Direction * t;
			return true;
		}

		private void setAreaHeight()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			float dy = _dragStartMouseY - (float)GameService.Input.get_Mouse().get_Position().Y;
			float scale = 0.05f;
			_areaHeight = dy * scale;
		}

		private void DrawCuboidPreview(Vector3 start, Vector3 end, float height)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Expected O, but got Unknown
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice gd = _blueprintRenderer.GraphicsDevice;
			Matrix view = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Matrix projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			float x1 = Math.Min(start.X, end.X);
			float x2 = Math.Max(start.X, end.X);
			float y1 = Math.Min(start.Y, end.Y);
			float y2 = Math.Max(start.Y, end.Y);
			float baseZ = start.Z;
			float topZ = baseZ + height;
			Vector3 b1 = default(Vector3);
			((Vector3)(ref b1))._002Ector(x1, y1, baseZ);
			Vector3 b2 = default(Vector3);
			((Vector3)(ref b2))._002Ector(x2, y1, baseZ);
			Vector3 b3 = default(Vector3);
			((Vector3)(ref b3))._002Ector(x2, y2, baseZ);
			Vector3 b4 = default(Vector3);
			((Vector3)(ref b4))._002Ector(x1, y2, baseZ);
			Vector3 t1 = default(Vector3);
			((Vector3)(ref t1))._002Ector(x1, y1, topZ);
			Vector3 t2 = default(Vector3);
			((Vector3)(ref t2))._002Ector(x2, y1, topZ);
			Vector3 t3 = default(Vector3);
			((Vector3)(ref t3))._002Ector(x2, y2, topZ);
			Vector3 t4 = default(Vector3);
			((Vector3)(ref t4))._002Ector(x1, y2, topZ);
			Color fill = default(Color);
			((Color)(ref fill))._002Ector(0.2f, 0.6f, 1f, 0.25f);
			List<VertexPositionColor> tris = new List<VertexPositionColor>();
			AddQuad(tris, t1, t2, t3, t4, fill);
			AddQuad(tris, b1, b2, t2, t1, fill);
			AddQuad(tris, b2, b3, t3, t2, fill);
			AddQuad(tris, b3, b4, t4, t3, fill);
			AddQuad(tris, b4, b1, t1, t4, fill);
			Color border = Color.get_LimeGreen();
			VertexPositionColor[] borderVerts = (VertexPositionColor[])(object)new VertexPositionColor[18]
			{
				new VertexPositionColor(b1, border),
				new VertexPositionColor(b2, border),
				new VertexPositionColor(b3, border),
				new VertexPositionColor(b4, border),
				new VertexPositionColor(b1, border),
				new VertexPositionColor(b1, border),
				new VertexPositionColor(t1, border),
				new VertexPositionColor(b2, border),
				new VertexPositionColor(t2, border),
				new VertexPositionColor(b3, border),
				new VertexPositionColor(t3, border),
				new VertexPositionColor(b4, border),
				new VertexPositionColor(t4, border),
				new VertexPositionColor(t1, border),
				new VertexPositionColor(t2, border),
				new VertexPositionColor(t3, border),
				new VertexPositionColor(t4, border),
				new VertexPositionColor(t1, border)
			};
			if (_selectionEffect == null)
			{
				BasicEffect val = new BasicEffect(gd);
				val.set_VertexColorEnabled(true);
				_selectionEffect = val;
			}
			_selectionEffect.set_World(Matrix.get_Identity());
			_selectionEffect.set_View(view);
			_selectionEffect.set_Projection(projection);
			RasterizerState oldRasterizer = gd.get_RasterizerState();
			RasterizerState val2 = new RasterizerState();
			val2.set_CullMode((CullMode)0);
			gd.set_RasterizerState(val2);
			Enumerator enumerator = ((Effect)_selectionEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, tris.ToArray(), 0, tris.Count / 3);
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, borderVerts, 0, borderVerts.Length - 1);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
			gd.set_RasterizerState(oldRasterizer);
		}

		private void AddQuad(List<VertexPositionColor> v, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color c)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			v.Add(new VertexPositionColor(v1, c));
			v.Add(new VertexPositionColor(v2, c));
			v.Add(new VertexPositionColor(v3, c));
			v.Add(new VertexPositionColor(v1, c));
			v.Add(new VertexPositionColor(v3, c));
			v.Add(new VertexPositionColor(v4, c));
		}

		private void SelectObjectsInCuboid()
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			float minX = Math.Min(_rectStart.X, _rectEnd.X);
			float maxX = Math.Max(_rectStart.X, _rectEnd.X);
			float minZ = Math.Min(_rectStart.Y, _rectEnd.Y);
			float maxZ = Math.Max(_rectStart.Y, _rectEnd.Y);
			float minY = Math.Min(planeZ, planeZ + _areaHeight);
			float maxY = Math.Max(planeZ, planeZ + _areaHeight);
			if (!multiLassoSelect)
			{
				ClearSelection();
			}
			foreach (BlueprintObject obj in Objects)
			{
				if (!multiLassoSelect || !obj.Selected)
				{
					Vector3 p = obj.Position;
					if (p.X >= minX && p.X <= maxX && p.Y >= minZ && p.Y <= maxZ && p.Z >= minY && p.Z <= maxY)
					{
						SelectObject(obj, multiSelect: true);
					}
				}
			}
			ScreenNotification.ShowNotification($"{SelectedObjects.Count} Objects selected", (NotificationType)0, (Texture2D)null, 4);
		}

		private void SelectObjectsInPolygon()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			if (_polygonPoints == null || _polygonPoints.Count < 3)
			{
				return;
			}
			List<Vector2> poly2 = ((IEnumerable<Vector3>)_polygonPoints).Select((Func<Vector3, Vector2>)((Vector3 p) => new Vector2(p.X, p.Y))).ToList();
			float z = _polygonPoints[0].Z;
			float topZ = z + _areaHeight;
			float minZ = Math.Min(z, topZ);
			float maxZ = Math.Max(z, topZ);
			if (!multiLassoSelect)
			{
				ClearSelection();
			}
			Vector2 pos2 = default(Vector2);
			foreach (BlueprintObject obj in Objects)
			{
				if (multiLassoSelect && obj.Selected)
				{
					continue;
				}
				Vector3 p2 = obj.Position;
				if (!(p2.Z < minZ) && !(p2.Z > maxZ))
				{
					((Vector2)(ref pos2))._002Ector(obj.Position.X, obj.Position.Y);
					if (PointInPolygon(pos2, poly2))
					{
						SelectObject(obj, multiSelect: true);
					}
				}
			}
			ScreenNotification.ShowNotification($"{SelectedObjects.Count} Objects selected", (NotificationType)0, (Texture2D)null, 4);
			_isSelectingPolygon = false;
			_selectionMode = SelectionMode.None;
			_polygonPoints.Clear();
		}

		private bool PointInPolygon(Vector2 pt, List<Vector2> poly)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			bool inside = false;
			int i = 0;
			int j = poly.Count - 1;
			while (i < poly.Count)
			{
				Vector2 pi = poly[i];
				Vector2 pj = poly[j];
				if (pi.Y > pt.Y != pj.Y > pt.Y && pt.X < (pj.X - pi.X) * (pt.Y - pi.Y) / (pj.Y - pi.Y + float.Epsilon) + pi.X)
				{
					inside = !inside;
				}
				j = i++;
			}
			return inside;
		}

		private void DrawSelectionRectangle(Vector3 start, Vector3 end, Color borderColor)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice gd = _blueprintRenderer.GraphicsDevice;
			RasterizerState oldRasterizer = gd.get_RasterizerState();
			RasterizerState val = new RasterizerState();
			val.set_CullMode((CullMode)0);
			gd.set_RasterizerState(val);
			Matrix view = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Matrix projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			float z = start.Z;
			float x1 = Math.Min(start.X, end.X);
			float x2 = Math.Max(start.X, end.X);
			float y1 = Math.Min(start.Y, end.Y);
			float y2 = Math.Max(start.Y, end.Y);
			Vector3 v1 = default(Vector3);
			((Vector3)(ref v1))._002Ector(x1, y1, z);
			Vector3 v2 = default(Vector3);
			((Vector3)(ref v2))._002Ector(x2, y1, z);
			Vector3 v3 = default(Vector3);
			((Vector3)(ref v3))._002Ector(x2, y2, z);
			Vector3 v4 = default(Vector3);
			((Vector3)(ref v4))._002Ector(x1, y2, z);
			Color fillColor = default(Color);
			((Color)(ref fillColor))._002Ector(0.2f, 0.6f, 1f, 0.25f);
			VertexPositionColor[] fillVerts = (VertexPositionColor[])(object)new VertexPositionColor[6]
			{
				new VertexPositionColor(v1, fillColor),
				new VertexPositionColor(v2, fillColor),
				new VertexPositionColor(v3, fillColor),
				new VertexPositionColor(v1, fillColor),
				new VertexPositionColor(v3, fillColor),
				new VertexPositionColor(v4, fillColor)
			};
			VertexPositionColor[] borderVerts = (VertexPositionColor[])(object)new VertexPositionColor[5]
			{
				new VertexPositionColor(v1, borderColor),
				new VertexPositionColor(v2, borderColor),
				new VertexPositionColor(v3, borderColor),
				new VertexPositionColor(v4, borderColor),
				new VertexPositionColor(v1, borderColor)
			};
			if (_selectionEffect == null)
			{
				BasicEffect val2 = new BasicEffect(gd);
				val2.set_VertexColorEnabled(true);
				_selectionEffect = val2;
			}
			_selectionEffect.set_World(Matrix.get_Identity());
			_selectionEffect.set_View(view);
			_selectionEffect.set_Projection(projection);
			Enumerator enumerator = ((Effect)_selectionEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, fillVerts, 0, 2);
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, borderVerts, 0, borderVerts.Length - 1);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
			gd.set_RasterizerState(oldRasterizer);
		}

		private void DrawPolygon(List<Vector3> points)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			if (points.Count == 0)
			{
				return;
			}
			foreach (Vector3 p in points)
			{
				DrawSelectionMarker(p + Vector3.get_Up() * 0.2f, 0.2f, Color.get_Yellow());
			}
			if (points.Count < 2)
			{
				return;
			}
			GraphicsDevice gd = _blueprintRenderer.GraphicsDevice;
			Matrix view = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Matrix proj = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			List<VertexPositionColor> verts = new List<VertexPositionColor>();
			for (int i = 0; i < points.Count; i++)
			{
				verts.Add(new VertexPositionColor(points[i] + Vector3.get_Up() * 0.05f, Color.get_Cyan()));
			}
			if (points.Count > 1)
			{
				verts.Add(new VertexPositionColor(points[0] + Vector3.get_Up() * 0.05f, Color.get_Cyan()));
			}
			_selectionEffect.set_World(Matrix.get_Identity());
			_selectionEffect.set_View(view);
			_selectionEffect.set_Projection(proj);
			Enumerator enumerator2 = ((Effect)_selectionEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator2)).MoveNext())
				{
					((Enumerator)(ref enumerator2)).get_Current().Apply();
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, verts.ToArray(), 0, verts.Count - 1);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator2)).Dispose();
			}
		}

		private void DrawSelectionMarker(Vector3 pos, float size, Color color)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice gd = _blueprintRenderer.GraphicsDevice;
			VertexPositionColor[] verts = (VertexPositionColor[])(object)new VertexPositionColor[4]
			{
				new VertexPositionColor(pos + new Vector3(0f - size, 0f, 0f), color),
				new VertexPositionColor(pos + new Vector3(size, 0f, 0f), color),
				new VertexPositionColor(pos + new Vector3(0f, 0f, 0f - size), color),
				new VertexPositionColor(pos + new Vector3(0f, 0f, size), color)
			};
			Enumerator enumerator = ((Effect)_selectionEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, verts, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void DrawPolygonPrismPreview(List<Vector3> points, float height)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Expected O, but got Unknown
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Expected O, but got Unknown
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			if (points == null || points.Count < 3)
			{
				return;
			}
			GraphicsDevice gd = _blueprintRenderer.GraphicsDevice;
			Matrix view = GameService.Gw2Mumble.get_PlayerCamera().get_View();
			Matrix projection = GameService.Gw2Mumble.get_PlayerCamera().get_Projection();
			float baseZ = points[0].Z;
			float topZ = baseZ + height;
			Color fill = default(Color);
			((Color)(ref fill))._002Ector(0.2f, 0.6f, 1f, 0.25f);
			Color border = Color.get_LimeGreen();
			List<Vector3> bottom = new List<Vector3>();
			List<Vector3> top = new List<Vector3>();
			for (int i2 = 0; i2 < points.Count; i2++)
			{
				bottom.Add(new Vector3(points[i2].X, points[i2].Y, baseZ));
				top.Add(new Vector3(points[i2].X, points[i2].Y, topZ));
			}
			List<VertexPositionColor> tris = new List<VertexPositionColor>();
			Vector3 topCenter = Vector3.get_Zero();
			for (int n = 0; n < top.Count; n++)
			{
				topCenter += top[n];
			}
			topCenter /= (float)top.Count;
			for (int m = 0; m < top.Count; m++)
			{
				int next = (m + 1) % top.Count;
				tris.Add(new VertexPositionColor(topCenter, fill));
				tris.Add(new VertexPositionColor(top[m], fill));
				tris.Add(new VertexPositionColor(top[next], fill));
			}
			for (int l = 0; l < bottom.Count; l++)
			{
				int next2 = (l + 1) % bottom.Count;
				AddQuad(tris, bottom[l], bottom[next2], top[next2], top[l], fill);
			}
			List<VertexPositionColor> borderVerts = new List<VertexPositionColor>();
			for (int k = 0; k < bottom.Count; k++)
			{
				borderVerts.Add(new VertexPositionColor(bottom[k], border));
			}
			borderVerts.Add(new VertexPositionColor(bottom[0], border));
			for (int j = 0; j < bottom.Count; j++)
			{
				borderVerts.Add(new VertexPositionColor(bottom[j], border));
				borderVerts.Add(new VertexPositionColor(top[j], border));
			}
			for (int i = 0; i < top.Count; i++)
			{
				borderVerts.Add(new VertexPositionColor(top[i], border));
			}
			borderVerts.Add(new VertexPositionColor(top[0], border));
			if (_selectionEffect == null)
			{
				BasicEffect val = new BasicEffect(gd);
				val.set_VertexColorEnabled(true);
				_selectionEffect = val;
			}
			_selectionEffect.set_World(Matrix.get_Identity());
			_selectionEffect.set_View(view);
			_selectionEffect.set_Projection(projection);
			RasterizerState oldRasterizer = gd.get_RasterizerState();
			RasterizerState val2 = new RasterizerState();
			val2.set_CullMode((CullMode)0);
			gd.set_RasterizerState(val2);
			Enumerator enumerator = ((Effect)_selectionEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, tris.ToArray(), 0, tris.Count / 3);
					gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, borderVerts.ToArray(), 0, borderVerts.Count - 1);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
			gd.set_RasterizerState(oldRasterizer);
		}

		public void unload()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoTranslate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoRotate);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)GizmoScale);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGizmoConfirm);
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnGizmoKeyPress);
		}
	}
}
