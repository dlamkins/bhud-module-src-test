using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Behavior.Filter;
using BhModule.Community.Pathing.Behavior.Modifier;
using BhModule.Community.Pathing.Content;
using BhModule.Community.Pathing.Editor.TypeConverters;
using BhModule.Community.Pathing.Editor.TypeEditors;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Entity
{
	public class StandardMarker : PathingEntity, ICanPick, IHasMapInfo
	{
		private const double ABOVEBELOWINDICATOR_THRESHOLD = 2.0;

		private const float VERTICALOFFSET_THRESHOLD = 30f;

		private static readonly Texture2D _aboveTexture;

		private static readonly Texture2D _belowTexture;

		private static DynamicVertexBuffer _sharedVertexBuffer;

		private static readonly Vector4[] _screenVerts;

		private static readonly Vector3[] _faceVerts;

		private Matrix _modelMatrix = Matrix.get_Identity();

		private const string ATTR_ALPHA = "alpha";

		private const string ATTR_CANFADE = "canfade";

		private const string ATTR_FADECENTER = "fadecenter";

		private const string ATTR_COLOR = "color";

		private const string ATTR_TINT = "tint";

		private const string ATTR_CULL = "cull";

		private const string ATTR_FADENEAR = "fadenear";

		private const string ATTR_FADEFAR = "fadefar";

		private const string ATTR_GUID = "guid";

		private const string ATTR_HEIGHTOFFSET = "heightoffset";

		private const string ATTR_ICONFILE = "iconfile";

		private Texture2D _texture;

		private const string ATTR_ICONSIZE = "iconsize";

		private const string ATTR_INVERTBEHAVIOR = "invertbehavior";

		private const string ATTR_MAPDISPLAYSIZE = "mapdisplaysize";

		private const string ATTR_SCALEONMAPWITHZOOM = "scaleonmapwithzoom";

		private const string ATTR_MINIMAPVISIBILITY = "minimapvisibility";

		private const string ATTR_MAPVISIBILITY = "mapvisibility";

		private const string ATTR_INGAMEVISIBILITY = "ingamevisibility";

		private const string ATTR_MINSIZE = "minsize";

		private const string ATTR_MAXSIZE = "maxsize";

		private const string ATTR_ROTATE = "rotate";

		private const string ATTR_ROTATEX = "rotate-x";

		private const string ATTR_ROTATEY = "rotate-y";

		private const string ATTR_ROTATEZ = "rotate-z";

		private const string ATTR_TEXT = "text";

		private const string ATTR_TITLE = "title";

		private const string ATTR_TITLECOLOR = "title-color";

		private const string ATTR_TIP = "tip";

		private const string ATTR_NAME = "tip-name";

		private const string ATTR_DESCRIPTION = "tip-description";

		private const string ATTR_XPOS = "xpos";

		private const string ATTR_YPOS = "ypos";

		private const string ATTR_ZPOS = "zpos";

		private const string ATTR_TRIGGERRANGE = "triggerrange";

		private bool _focused;

		private const string ATTR_RESETLENGTH = "resetlength";

		private const string ATTR_AUTOTRIGGER = "autotrigger";

		private static readonly Logger Logger;

		[Description("Specifies the opacity of a marker or trail where 1 is opaque and 0 is fully transparent. Values outside of this range will be clamped to this range.")]
		[Category("Appearance")]
		public float Alpha { get; set; }

		public bool CanFade { get; set; } = true;


		[Description("Tints the marker or trail with the color provided. Powerful when reusing existing marker icons or trail textures to make them differ in color.")]
		[Category("Appearance")]
		[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(ColorConverter))]
		public Color Tint { get; set; }

		[Description("By default markers and trails are rendered without culling meaning that both sides are rendered at all times. Alternative culling settings allow you to disable culling for one side or the other. For example, a trail can be made to be visible from only below.")]
		[Category("Appearance")]
		public RasterizerState CullDirection { get; set; } = RasterizerState.CullNone;


		public float FadeNear { get; set; }

		public float FadeFar { get; set; }

		[Description("A unique identifier used to track the state of certain behaviors between launch sessions.")]
		[Category("Behavior")]
		public Guid Guid { get; set; }

		[Description("Renders the marker the specified amount higher than the actual position.")]
		[Category("Appearance")]
		public float HeightOffset { get; set; }

		[JsonIgnore]
		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				if (_texture != null)
				{
					FadeIn();
				}
			}
		}

		public float Size { get; set; }

		public bool InvertBehavior { get; set; }

		public float MapDisplaySize { get; set; }

		public bool ScaleOnMapWithZoom { get; set; }

		public bool MiniMapVisibility { get; set; }

		public bool MapVisibility { get; set; }

		public bool InGameVisibility { get; set; }

		public float MinSize { get; set; }

		public float MaxSize { get; set; } = float.MaxValue;


		[DisplayName("Rotate")]
		[Description("Allows you to statically rotate a marker instead of it automatically facing the player.")]
		[Category("Appearance")]
		public Vector3? RotationXyz { get; set; }

		[DisplayName("Title")]
		[Category("Appearance")]
		public string BillboardText { get; set; }

		[DisplayName("Title-Color")]
		[Category("Appearance")]
		[Editor(typeof(ColorEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(ColorConverter))]
		public Color BillboardTextColor { get; set; }

		[DisplayName("Tip-Name")]
		[Category("Appearance")]
		public string TipName { get; set; }

		[DisplayName("Tip-Description")]
		[Category("Appearance")]
		public string TipDescription { get; set; }

		[Description("The primary attributes used to determine where to position a marker.")]
		[Category("Appearance")]
		[Editor(typeof(Vector3Editor), typeof(UITypeEditor))]
		public Vector3 Position { get; set; }

		[Description("This attribute is used by multiple other attributes to define a distance from the marker in which those attributes will activate their functionality or behavior.")]
		[Category("Behavior")]
		public override float TriggerRange { get; set; }

		[Description("The focused state indicates if the player is within the trigger range which may activate a behavior.")]
		[Category("State Debug")]
		public bool Focused
		{
			get
			{
				return _focused;
			}
			private set
			{
				if (_focused == value)
				{
					return;
				}
				_focused = value;
				foreach (IBehavior behavior in base.Behaviors)
				{
					ICanFocus focusable = behavior as ICanFocus;
					if (focusable != null)
					{
						if (_focused)
						{
							focusable.Focus();
						}
						else
						{
							focusable.Unfocus();
						}
					}
				}
			}
		}

		[Description("When using behavior 4 (reappear after timer) this value defines, in seconds, the duration until the marker is reset after being activated.")]
		[Category("Behavior")]
		public float ResetLength { get; set; }

		[Description("If enabled, attributes and behaviors which would normally require an interaction to activate will instead activate automatically when within TriggerRange.")]
		[Category("Behavior")]
		public bool AutoTrigger { get; set; }

		public override float DrawOrder => Vector3.DistanceSquared(Position, GameService.Gw2Mumble.get_PlayerCamera().get_Position());

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, (double X, double Y) offsets, double scale, float opacity)
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.Map) || Texture == null)
			{
				return null;
			}
			bool isMapOpen = GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			MapVisibilityLevel mapMarkerVisibilityLevel = _packState.UserConfiguration.MapMarkerVisibilityLevel.get_Value();
			bool allowedOnMap = MapVisibility && mapMarkerVisibilityLevel != MapVisibilityLevel.Never;
			if (isMapOpen && !allowedOnMap && mapMarkerVisibilityLevel != MapVisibilityLevel.Always)
			{
				return null;
			}
			MapVisibilityLevel miniMapMarkerVisibilityLevel = _packState.UserConfiguration.MiniMapMarkerVisibilityLevel.get_Value();
			bool allowedOnMiniMap = MiniMapVisibility && miniMapMarkerVisibilityLevel != MapVisibilityLevel.Never;
			if (!isMapOpen && !allowedOnMiniMap && miniMapMarkerVisibilityLevel != MapVisibilityLevel.Always)
			{
				return null;
			}
			if (!ScaleOnMapWithZoom)
			{
				scale = 1.0;
			}
			Vector2 location = GetScaledLocation(Position.X, Position.Y, scale, offsets);
			if (!((Rectangle)(ref bounds)).Contains(location))
			{
				return null;
			}
			float drawScale = (float)(1.0 / scale);
			RectangleF drawRect = default(RectangleF);
			((RectangleF)(ref drawRect))._002Ector(Point2.op_Implicit(location - new Vector2(MapDisplaySize / 2f * drawScale, MapDisplaySize / 2f * drawScale)), Size2.op_Implicit(new Vector2(MapDisplaySize * drawScale, MapDisplaySize * drawScale)));
			spriteBatch.Draw(Texture, drawRect, Tint * opacity);
			if (_packState.UserConfiguration.MapShowAboveBelowIndicators.get_Value() && scale < 2.0)
			{
				float diff = Position.Z - GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
				if (Math.Abs(diff) > 30f)
				{
					RectangleF indicatorPosition = default(RectangleF);
					((RectangleF)(ref indicatorPosition))._002Ector(((RectangleF)(ref drawRect)).get_Right() - (float)_aboveTexture.get_Width() * drawScale, ((RectangleF)(ref drawRect)).get_Top(), (float)_aboveTexture.get_Width() * drawScale, (float)_aboveTexture.get_Height() * drawScale);
					spriteBatch.Draw((diff > 0f) ? _aboveTexture : _belowTexture, indicatorPosition, Color.get_White() * opacity);
				}
			}
			return drawRect;
		}

		static StandardMarker()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			_aboveTexture = PathingModule.Instance.ContentsManager.GetTexture("png\\1130638.png");
			_belowTexture = PathingModule.Instance.ContentsManager.GetTexture("png\\1130639.png");
			_screenVerts = (Vector4[])(object)new Vector4[4];
			_faceVerts = (Vector3[])(object)new Vector3[4]
			{
				new Vector3(-0.5f, -0.5f, 0f),
				new Vector3(0.5f, -0.5f, 0f),
				new Vector3(-0.5f, 0.5f, 0f),
				new Vector3(0.5f, 0.5f, 0f)
			};
			Logger = Logger.GetLogger<StandardMarker>();
			CreateSharedVertexBuffer();
		}

		private static void CreateSharedVertexBuffer()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			_sharedVertexBuffer = new DynamicVertexBuffer(GameService.Graphics.get_GraphicsDevice(), typeof(VertexPositionTexture), 4, (BufferUsage)1);
			VertexPositionTexture[] verts = (VertexPositionTexture[])(object)new VertexPositionTexture[_faceVerts.Length];
			for (int i = 0; i < _faceVerts.Length; i++)
			{
				ref Vector3 vert = ref _faceVerts[i];
				verts[i] = new VertexPositionTexture(vert, new Vector2((float)((vert.X < 0f) ? 1 : 0), (float)((vert.Y < 0f) ? 1 : 0)));
			}
			((VertexBuffer)_sharedVertexBuffer).SetData<VertexPositionTexture>(verts);
		}

		private float GetOpacity()
		{
			float fade = 1f - MathHelper.Clamp((base.DistanceToPlayer - WorldUtil.GameToWorldCoord(FadeNear)) / (WorldUtil.GameToWorldCoord(FadeFar) - WorldUtil.GameToWorldCoord(FadeNear)), 0f, 1f);
			return Alpha * fade * _packState.UserConfiguration.PackMaxOpacityOverride.get_Value() * base.AnimatedFadeOpacity * ((!_packState.UserConfiguration.PackFadePathablesDuringCombat.get_Value()) ? 1f : (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() ? 0.5f : 1f));
		}

		public bool RayIntersects(Ray ray)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return PickingUtil.IntersectDistance(BoundingBox.CreateFromPoints(_faceVerts.Select((Vector3 vert) => Vector3.Transform(vert, _modelMatrix))), ray).HasValue;
		}

		public override void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.World) || _texture == null || ((GraphicsResource)_texture).get_IsDisposed() || !InGameVisibility)
			{
				return;
			}
			float maxRender = Math.Min(FadeFar, _packState.UserConfiguration.PackMaxViewDistance.get_Value());
			if (base.DistanceToPlayer > maxRender)
			{
				return;
			}
			float minRender = Math.Min(FadeNear, _packState.UserConfiguration.PackMaxViewDistance.get_Value() - (FadeFar - FadeNear));
			graphicsDevice.set_RasterizerState(CullDirection);
			Matrix modelMatrix = Matrix.CreateScale(Size * 2f, Size * 2f, 1f);
			Vector3 position = Position + new Vector3(0f, 0f, HeightOffset);
			if (!RotationXyz.HasValue)
			{
				modelMatrix *= Matrix.CreateBillboard(position, new Vector3(camera.get_Position().X, camera.get_Position().Y, camera.get_Position().Z), new Vector3(0f, 0f, 1f), (Vector3?)camera.get_Forward());
				Matrix transformMatrix = Matrix.Multiply(Matrix.Multiply(modelMatrix, _packState.SharedMarkerEffect.View), _packState.SharedMarkerEffect.Projection);
				for (int i = 0; i < _faceVerts.Length; i++)
				{
					_screenVerts[i] = Vector4.Transform(_faceVerts[i], transformMatrix);
					ref Vector4 reference = ref _screenVerts[i];
					reference /= _screenVerts[i].W;
				}
				float num = BoundingRectangle.CreateFrom((IReadOnlyList<Point2>)((IEnumerable<Vector4>)_screenVerts).Select((Func<Vector4, Point2>)((Vector4 s) => new Point2(s.X, s.Y))).ToArray()).HalfExtents.Y * 2f;
				Viewport viewport = ((GraphicsResource)_packState.SharedMarkerEffect).get_GraphicsDevice().get_Viewport();
				float pixelSizeY = num * (float)((Viewport)(ref viewport)).get_Height();
				float limitY = MathHelper.Clamp(pixelSizeY, MinSize * 4f, MaxSize * 4f);
				modelMatrix *= Matrix.CreateTranslation(-position) * Matrix.CreateScale(limitY / pixelSizeY) * Matrix.CreateTranslation(position);
			}
			else
			{
				modelMatrix *= Matrix.CreateRotationX(RotationXyz.Value.X) * Matrix.CreateRotationY(RotationXyz.Value.Y) * Matrix.CreateRotationZ(RotationXyz.Value.Z) * Matrix.CreateTranslation(position);
			}
			_packState.SharedMarkerEffect.SetEntityState(modelMatrix, Texture, GetOpacity(), minRender, maxRender, CanFade && _packState.UserConfiguration.PackFadeMarkersBetweenCharacterAndCamera.get_Value(), Tint, base.DebugRender);
			_modelMatrix = modelMatrix;
			graphicsDevice.SetVertexBuffer((VertexBuffer)(object)_sharedVertexBuffer);
			Enumerator enumerator = ((Effect)_packState.SharedMarkerEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Alpha(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			Alpha = _packState.UserResourceStates.Population.MarkerPopulationDefaults.Alpha;
			if (collection.TryPopAttribute("alpha", out var attribute))
			{
				Alpha = MathHelper.Clamp(attribute.GetValueAsFloat(Alpha), 0f, 1f);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_CanFade(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			CanFade = _packState.UserResourceStates.Population.MarkerPopulationDefaults.CanFade;
			if (collection.TryPopAttribute("fadecenter", out var attribute2))
			{
				CanFade = attribute2.GetValueAsBool();
			}
			if (collection.TryPopAttribute("canfade", out var attribute))
			{
				CanFade = attribute.GetValueAsBool();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Tint(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			Tint = _packState.UserResourceStates.Population.MarkerPopulationDefaults.Tint;
			if (collection.TryPopAttribute("color", out var attribute2))
			{
				Tint = attribute2.GetValueAsColor(Tint);
			}
			if (collection.TryPopAttribute("tint", out var attribute))
			{
				Tint = attribute.GetValueAsColor(Tint);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Cull(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			CullDirection cullDirection = _packState.UserResourceStates.Population.MarkerPopulationDefaults.Cull;
			if (collection.TryPopAttribute("cull", out var attribute))
			{
				cullDirection = attribute.GetValueAsEnum<CullDirection>();
			}
			CullDirection = (RasterizerState)(cullDirection switch
			{
				BhModule.Community.Pathing.Entity.CullDirection.None => RasterizerState.CullNone, 
				BhModule.Community.Pathing.Entity.CullDirection.Clockwise => RasterizerState.CullClockwise, 
				BhModule.Community.Pathing.Entity.CullDirection.CounterClockwise => RasterizerState.CullCounterClockwise, 
				_ => CullDirection, 
			});
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_FadeNearAndFar(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			FadeNear = _packState.UserResourceStates.Population.MarkerPopulationDefaults.FadeNear;
			FadeFar = _packState.UserResourceStates.Population.MarkerPopulationDefaults.FadeFar;
			if (collection.TryPopAttribute("fadenear", out var attribute2))
			{
				FadeNear = attribute2.GetValueAsFloat(FadeNear);
			}
			if (collection.TryPopAttribute("fadefar", out var attribute))
			{
				FadeFar = attribute.GetValueAsFloat(FadeFar);
			}
			if (FadeNear < 0f || FadeFar < 0f)
			{
				FadeNear = float.MaxValue;
				FadeFar = float.MaxValue;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Guid(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			Guid = _packState.UserResourceStates.Population.MarkerPopulationDefaults.Guid;
			if (collection.TryPopAttribute("guid", out var attribute))
			{
				Guid = attribute.GetValueAsGuid();
			}
			if (Guid == Guid.Empty)
			{
				Guid = Guid.NewGuid();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_HeightOffset(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			HeightOffset = _packState.UserResourceStates.Population.MarkerPopulationDefaults.HeightOffset;
			if (collection.TryPopAttribute("heightoffset", out var attribute))
			{
				HeightOffset = attribute.GetValueAsFloat(HeightOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_IconFile(TmfLib.Prototype.AttributeCollection collection, TextureResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("iconfile", out var attribute))
			{
				attribute.GetValueAsTextureAsync(resourceManager).ContinueWith(delegate(Task<Texture2D> textureTaskResult)
				{
					if (!textureTaskResult.IsFaulted && textureTaskResult.Result != null)
					{
						Texture = textureTaskResult.Result;
					}
				});
			}
			else
			{
				Texture = Textures.get_Error();
				Logger.Warn("Marker '" + Guid.ToBase64String() + "' is missing 'iconfile' attribute.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_IconSize(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			Size = _packState.UserResourceStates.Population.MarkerPopulationDefaults.IconSize;
			if (collection.TryPopAttribute("iconsize", out var attribute))
			{
				Size = attribute.GetValueAsFloat(attribute.GetValueAsFloat(_packState.UserResourceStates.Population.MarkerPopulationDefaults.IconSize / 2f) * 2f);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_InvertBehavior(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("invertbehavior", out var attribute))
			{
				InvertBehavior = attribute.GetValueAsBool();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_MapScaling(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			MapDisplaySize = _packState.UserResourceStates.Population.MarkerPopulationDefaults.MapDisplaySize;
			ScaleOnMapWithZoom = _packState.UserResourceStates.Population.MarkerPopulationDefaults.ScaleOnMapWithZoom;
			if (collection.TryPopAttribute("mapdisplaysize", out var attribute))
			{
				MapDisplaySize = attribute.GetValueAsFloat(MapDisplaySize);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_MapVisibility(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			MiniMapVisibility = _packState.UserResourceStates.Population.MarkerPopulationDefaults.MiniMapVisibility;
			MapVisibility = _packState.UserResourceStates.Population.MarkerPopulationDefaults.MapVisibility;
			InGameVisibility = _packState.UserResourceStates.Population.MarkerPopulationDefaults.InGameVisibility;
			if (collection.TryPopAttribute("minimapvisibility", out var attribute3))
			{
				MiniMapVisibility = attribute3.GetValueAsBool();
			}
			if (collection.TryPopAttribute("mapvisibility", out var attribute2))
			{
				MapVisibility = attribute2.GetValueAsBool();
			}
			if (collection.TryPopAttribute("ingamevisibility", out var attribute))
			{
				InGameVisibility = attribute.GetValueAsBool();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_MinMaxSize(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			MinSize = _packState.UserResourceStates.Population.MarkerPopulationDefaults.MinSize;
			MaxSize = _packState.UserResourceStates.Population.MarkerPopulationDefaults.MaxSize;
			if (collection.TryPopAttribute("minsize", out var attribute2))
			{
				MinSize = attribute2.GetValueAsFloat(MinSize);
			}
			if (collection.TryPopAttribute("maxsize", out var attribute))
			{
				MaxSize = attribute.GetValueAsFloat(MaxSize);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Rotation(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			float? rotationX = null;
			float? rotationY = null;
			float? rotationZ = null;
			if (collection.TryPopAttribute("rotate-x", out var attribute4))
			{
				rotationX = MathHelper.ToRadians(attribute4.GetValueAsFloat());
			}
			if (collection.TryPopAttribute("rotate-y", out var attribute3))
			{
				rotationY = MathHelper.ToRadians(attribute3.GetValueAsFloat());
			}
			if (collection.TryPopAttribute("rotate-z", out var attribute2))
			{
				rotationZ = MathHelper.ToRadians(attribute2.GetValueAsFloat());
			}
			if (collection.TryPopAttribute("rotate", out var attribute))
			{
				float[] rotations = attribute.GetValueAsFloats().Select((Func<float, float>)MathHelper.ToRadians).ToArray();
				if (rotations.Length != 0)
				{
					rotationX = rotations[0];
				}
				if (rotations.Length > 1)
				{
					rotationY = rotations[1];
				}
				if (rotations.Length > 2)
				{
					rotationZ = rotations[2];
				}
			}
			if (rotationX.HasValue || rotationY.HasValue || rotationZ.HasValue)
			{
				RotationXyz = new Vector3(rotationX.GetValueOrDefault(), rotationY.GetValueOrDefault(), rotationZ.GetValueOrDefault());
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Title(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			BillboardTextColor = _packState.UserResourceStates.Population.MarkerPopulationDefaults.TitleColor;
			if (collection.TryPopAttribute("text", out var attribute3))
			{
				BillboardText = attribute3.GetValueAsString();
			}
			if (collection.TryPopAttribute("title", out var attribute2))
			{
				BillboardText = attribute2.GetValueAsString();
			}
			if (collection.TryPopAttribute("title-color", out var attribute))
			{
				BillboardTextColor = attribute.GetValueAsColor(BillboardTextColor);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Tip(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (base.Category != null)
			{
				TipName = base.Category.DisplayName;
			}
			if (collection.TryPopAttribute("tip-name", out var attribute2))
			{
				TipName = attribute2.GetValueAsString();
			}
			if (collection.TryPopAttribute("tip-description", out var attribute))
			{
				TipDescription = attribute.GetValueAsString();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Position(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			float positionX = 0f;
			float positionY = 0f;
			float positionZ = 0f;
			if (collection.TryPopAttribute("xpos", out var attribute3))
			{
				positionX = attribute3.GetValueAsFloat();
			}
			if (collection.TryPopAttribute("ypos", out var attribute2))
			{
				positionY = attribute2.GetValueAsFloat();
			}
			if (collection.TryPopAttribute("zpos", out var attribute))
			{
				positionZ = attribute.GetValueAsFloat();
			}
			Position = new Vector3(positionX, positionZ, positionY);
		}

		private void AddBehavior(IBehavior behavior)
		{
			if (behavior != null)
			{
				base.Behaviors.Add(behavior);
			}
		}

		public override void HandleBehavior()
		{
			if (base.DistanceToPlayer <= TriggerRange)
			{
				Focus();
			}
			else
			{
				Unfocus();
			}
		}

		private void Populate_Behaviors(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryGetSubset("festival", out var attributes15))
			{
				AddBehavior(FestivalFilter.BuildFromAttributes(attributes15));
			}
			if (collection.TryGetSubset("mount", out var attributes14))
			{
				AddBehavior(MountFilter.BuildFromAttributes(attributes14));
			}
			if (collection.TryGetSubset("profession", out var attributes13))
			{
				AddBehavior(ProfessionFilter.BuildFromAttributes(attributes13));
			}
			if (collection.TryGetSubset("race", out var attributes12))
			{
				AddBehavior(RaceFilter.BuildFromAttributes(attributes12));
			}
			if (collection.TryGetSubset("specialization", out var attributes11))
			{
				AddBehavior(SpecializationFilter.BuildFromAttributes(attributes11));
			}
			if (collection.TryGetSubset("maptype", out var attributes10))
			{
				AddBehavior(MapTypeFilter.BuildFromAttributes(attributes10));
			}
			if (collection.TryGetSubset("schedule", out var attributes9))
			{
				AddBehavior(ScheduleFilter.BuildFromAttributes(attributes9));
			}
			if (collection.TryGetSubset("behavior", out var attributes8))
			{
				AddBehavior(StandardBehaviorFilter.BuildFromAttributes(attributes8, _packState, this));
			}
			if (collection.TryGetSubset("achievement", out var attributes7))
			{
				AddBehavior(AchievementFilter.BuildFromAttributes(attributes7, _packState));
			}
			if (collection.TryGetSubset("info", out var attributes6))
			{
				AddBehavior(InfoModifier.BuildFromAttributes(attributes6, this, _packState));
			}
			if (collection.TryGetSubset("bounce", out var attributes5))
			{
				AddBehavior(BounceModifier.BuildFromAttributes(attributes5, this, _packState));
			}
			if (collection.TryGetSubset("copy", out var attributes4))
			{
				AddBehavior(CopyModifier.BuildFromAttributes(attributes4, this, _packState));
			}
			if (collection.TryGetSubset("toggle", out var attributes3))
			{
				AddBehavior(ToggleModifier.BuildFromAttributes(attributes3, this, _packState));
			}
			if (collection.TryGetSubset("show", out var attributes2))
			{
				AddBehavior(ShowHideModifier.BuildFromAttributes(attributes2, this, _packState));
			}
			if (collection.TryGetSubset("hide", out var attributes))
			{
				AddBehavior(ShowHideModifier.BuildFromAttributes(attributes, this, _packState));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Triggers(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			TriggerRange = _packState.UserResourceStates.Population.MarkerPopulationDefaults.TriggerRange;
			if (collection.TryPopAttribute("triggerrange", out var attribute))
			{
				TriggerRange = attribute.GetValueAsFloat(_packState.UserResourceStates.Population.MarkerPopulationDefaults.TriggerRange);
			}
		}

		public override void Focus()
		{
			if (!Focused)
			{
				Focused = true;
				if (AutoTrigger)
				{
					Interact(autoTriggered: true);
				}
			}
		}

		public override void Unfocus()
		{
			Focused = false;
		}

		public override void Interact(bool autoTriggered)
		{
			foreach (IBehavior behavior in base.Behaviors)
			{
				(behavior as ICanInteract)?.Interact(autoTriggered);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_TacOMisc(TmfLib.Prototype.AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("resetlength", out var attribute2))
			{
				ResetLength = attribute2.GetValueAsFloat();
			}
			if (collection.TryPopAttribute("autotrigger", out var attribute))
			{
				AutoTrigger = attribute.GetValueAsBool();
			}
		}

		public StandardMarker(IPackState packState, IPointOfInterest pointOfInterest)
			: base(packState, pointOfInterest)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Populate(pointOfInterest.GetAggregatedAttributes(), TextureResourceManager.GetTextureResourceManager(pointOfInterest.ResourceManager));
		}

		private void Populate(TmfLib.Prototype.AttributeCollection collection, TextureResourceManager resourceManager)
		{
			Populate_Guid(collection, resourceManager);
			Populate_Position(collection, resourceManager);
			Populate_Triggers(collection, resourceManager);
			Populate_MinMaxSize(collection, resourceManager);
			Populate_IconSize(collection, resourceManager);
			Populate_IconFile(collection, resourceManager);
			Populate_Tint(collection, resourceManager);
			Populate_Rotation(collection, resourceManager);
			Populate_HeightOffset(collection, resourceManager);
			Populate_Alpha(collection, resourceManager);
			Populate_FadeNearAndFar(collection, resourceManager);
			Populate_Cull(collection, resourceManager);
			Populate_MapScaling(collection, resourceManager);
			Populate_MapVisibility(collection, resourceManager);
			Populate_CanFade(collection, resourceManager);
			Populate_Tip(collection, resourceManager);
			Populate_InvertBehavior(collection, resourceManager);
			Populate_TacOMisc(collection, resourceManager);
			Populate_Behaviors(collection, resourceManager);
		}

		public override void Update(GameTime gameTime)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.DistanceToPlayer = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), Position);
			base.Update(gameTime);
		}
	}
}
