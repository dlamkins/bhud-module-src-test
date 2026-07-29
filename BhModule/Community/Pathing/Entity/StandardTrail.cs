using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Behavior.Filter;
using BhModule.Community.Pathing.Content;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Entity
{
	public class StandardTrail : PathingEntity, ICanPick
	{
		private const float TRAIL_WIDTH = 0.508f;

		private VertexBuffer[] _sectionBuffers;

		private const string ATTR_ALPHA = "alpha";

		private const string ATTR_ANIMATIONSPEED = "animspeed";

		private const string ATTR_CANFADE = "canfade";

		private const string ATTR_COLOR = "color";

		private const string ATTR_TINT = "tint";

		private const string ATTR_CULL = "cull";

		private const string ATTR_EDITTAG = "edittag";

		private const string ATTR_FADENEAR = "fadenear";

		private const string ATTR_FADEFAR = "fadefar";

		private const string ATTR_GUID = "guid";

		private const string ATTR_MINIMAPVISIBILITY = "minimapvisibility";

		private const string ATTR_MAPVISIBILITY = "mapvisibility";

		private const string ATTR_INGAMEVISIBILITY = "ingamevisibility";

		private const string ATTR_TEXTURE = "texture";

		private AsyncTexture2D _texture;

		private const string ATTR_TRAILSCALE = "trailscale";

		private const string ATTR_ISWALL = "iswall";

		private const string ATTR_TRIGGERRANGE = "triggerrange";

		private const float DEFAULT_TRAILRESOLUTION = 30f;

		private const string ATTR_RESETLENGTH = "resetlength";

		private static readonly Logger Logger = Logger.GetLogger<StandardTrail>();

		internal Vector3[][] _sectionPoints;

		public float Alpha { get; set; }

		public float AnimationSpeed { get; set; }

		public bool CanFade { get; set; } = true;


		public Color Tint { get; set; }

		public RasterizerState CullDirection { get; set; } = RasterizerState.CullNone;


		public float FadeNear { get; set; }

		public float FadeFar { get; set; }

		public bool MiniMapVisibility { get; set; }

		public bool MapVisibility { get; set; }

		public bool InGameVisibility { get; set; }

		public AsyncTexture2D Texture
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

		public Color TrailSampleColor { get; set; } = Color.get_White();


		public float TrailScale { get; set; }

		public bool IsWall { get; set; }

		public override float TriggerRange { get; set; }

		public float ResetLength { get; set; }

		public override float DrawOrder => float.MaxValue;

		public TextureResourceManager TextureResourceManager { get; }

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity)
		{
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.Map) || Texture == null || _sectionPoints == null)
			{
				return null;
			}
			bool isMapOpen = GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			MapVisibilityLevel mapTrailVisibilityLevel = _packState.UserConfiguration.MapTrailVisibilityLevel.get_Value();
			bool allowedOnMap = MapVisibility && mapTrailVisibilityLevel != MapVisibilityLevel.Never;
			if (isMapOpen && !allowedOnMap && mapTrailVisibilityLevel != MapVisibilityLevel.Always)
			{
				return null;
			}
			MapVisibilityLevel miniMapTrailVisibilityLevel = _packState.UserConfiguration.MiniMapTrailVisibilityLevel.get_Value();
			bool allowedOnMiniMap = MiniMapVisibility && miniMapTrailVisibilityLevel != MapVisibilityLevel.Never;
			if (!isMapOpen && !allowedOnMiniMap && miniMapTrailVisibilityLevel != MapVisibilityLevel.Always)
			{
				return null;
			}
			bool lastPointInBounds = false;
			Vector3[][] sectionPoints = _sectionPoints;
			foreach (Vector3[] trailSection in sectionPoints)
			{
				for (int i = 0; i < trailSection.Length - 1; i++)
				{
					Vector2 thisPoint = GetScaledLocation(trailSection[i].X, trailSection[i].Y, scale, offsetX, offsetY);
					Vector2 nextPoint = GetScaledLocation(trailSection[i + 1].X, trailSection[i + 1].Y, scale, offsetX, offsetY);
					bool inBounds = false;
					if (lastPointInBounds | (inBounds = ((Rectangle)(ref bounds)).Contains(nextPoint)))
					{
						float drawOpacity = opacity;
						if (_packState.UserConfiguration.MapFadeVerticallyDistantTrailSegments.get_Value())
						{
							float averageVert = (trailSection[i].Z + trailSection[i + 1].Z) / 2f;
							drawOpacity *= MathHelper.Clamp(1f - Math.Abs(averageVert - GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z) * 0.005f, 0.15f, 1f);
						}
						float distance = Vector2.Distance(thisPoint, nextPoint);
						float angle = (float)Math.Atan2(nextPoint.Y - thisPoint.Y, nextPoint.X - thisPoint.X);
						DrawLine(spriteBatch, thisPoint, angle, distance, TrailSampleColor * drawOpacity, _packState.UserConfiguration.MapTrailWidth.get_Value());
					}
					lastPointInBounds = inBounds;
				}
			}
			return null;
		}

		private void DrawLine(SpriteBatch spriteBatch, Vector2 position, float angle, float distance, Color color, float thickness)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(Textures.get_Pixel(), position, (Rectangle?)null, color, angle, Vector2.get_Zero(), new Vector2(distance, thickness), (SpriteEffects)0, 0f);
		}

		private VertexBuffer PostProcessTrailSection(GraphicsDevice graphicsDevice, IEnumerable<Vector3> points)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			points = PostProcessing_SetTrailResolution(points, 20f);
			Vector3[] pointsArr = (points as Vector3[]) ?? points.ToArray();
			float distance = 0f;
			for (int i = 0; i < pointsArr.Length - 1; i++)
			{
				distance += Vector3.Distance(pointsArr[i], pointsArr[i + 1]);
			}
			if (!(distance > 0f))
			{
				return null;
			}
			return BuildTrailSection(graphicsDevice, pointsArr, distance);
		}

		private VertexBuffer BuildTrailSection(GraphicsDevice graphicsDevice, IEnumerable<Vector3> points, float distance)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			Vector3[] pointsArr = (points as Vector3[]) ?? points.ToArray();
			VertexPositionColorTexture[] verts = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[pointsArr.Length * 2];
			float pastDistance = distance;
			Vector3 curPoint = pointsArr[0];
			Vector3 offset = Vector3.get_Zero();
			Vector3 lastOffset = Vector3.get_Zero();
			float flipOver = 1f;
			float normalOffset = 0.508f * TrailScale;
			Vector3 modDistance = Vector3.get_Zero();
			for (int i = 0; i < pointsArr.Length - 1; i++)
			{
				Vector3 nextPoint = pointsArr[i + 1];
				Vector3 pathDirection = nextPoint - curPoint;
				offset = Vector3.Cross(pathDirection, IsWall ? Vector3.Cross(pathDirection, Vector3.get_Forward()) : Vector3.get_Forward());
				((Vector3)(ref offset)).Normalize();
				if (lastOffset != Vector3.get_Zero() && Vector3.Dot(offset, lastOffset) < 0f)
				{
					flipOver *= -1f;
				}
				modDistance = offset * normalOffset * flipOver;
				verts[i * 2 + 1] = new VertexPositionColorTexture(curPoint + modDistance, Color.get_White(), new Vector2(0f, pastDistance / 1.016f - 1f));
				verts[i * 2] = new VertexPositionColorTexture(curPoint - modDistance, Color.get_White(), new Vector2(1f, pastDistance / 1.016f - 1f));
				pastDistance -= Vector3.Distance(curPoint, nextPoint);
				lastOffset = offset;
				curPoint = nextPoint;
			}
			Vector3 fleftPoint = curPoint + modDistance;
			Vector3 frightPoint = curPoint - modDistance;
			verts[pointsArr.Length * 2 - 1] = new VertexPositionColorTexture(fleftPoint, Color.get_White(), new Vector2(0f, pastDistance / 1.016f - 1f));
			verts[pointsArr.Length * 2 - 2] = new VertexPositionColorTexture(frightPoint, Color.get_White(), new Vector2(1f, pastDistance / 1.016f - 1f));
			VertexBuffer val = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, verts.Length, (BufferUsage)1);
			val.SetData<VertexPositionColorTexture>(verts);
			return val;
		}

		private void BuildBuffers(ITrail trail)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			List<VertexBuffer> buffers = new List<VertexBuffer>();
			GraphicsDeviceContext gdctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				foreach (ITrailSection section in trail.TrailSections)
				{
					VertexBuffer processedBuffer = PostProcessTrailSection(((GraphicsDeviceContext)(ref gdctx)).get_GraphicsDevice(), section.TrailPoints.Select((Func<Vector3, Vector3>)((Vector3 v) => new Vector3(v.X, v.Y, v.Z))));
					if (processedBuffer != null)
					{
						buffers.Add(processedBuffer);
					}
				}
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdctx)).Dispose();
			}
			_sectionBuffers = buffers.ToArray();
		}

		internal void BuildBuffers(Vector3[] trail)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			List<VertexBuffer> buffers = new List<VertexBuffer>();
			GraphicsDeviceContext gdctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				VertexBuffer processedBuffer = PostProcessTrailSection(((GraphicsDeviceContext)(ref gdctx)).get_GraphicsDevice(), ((IEnumerable<Vector3>)trail).Select((Func<Vector3, Vector3>)((Vector3 v) => new Vector3(v.X, v.Y, v.Z))));
				if (processedBuffer != null)
				{
					buffers.Add(processedBuffer);
				}
			}
			finally
			{
				((GraphicsDeviceContext)(ref gdctx)).Dispose();
			}
			_sectionBuffers = buffers.ToArray();
		}

		private float GetOpacity()
		{
			return Alpha * _packState.UserConfiguration.PackMaxOpacityOverride.get_Value() * base.AnimatedFadeOpacity * ((!_packState.UserConfiguration.PackFadePathablesDuringCombat.get_Value()) ? 1f : (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() ? 0.5f : 1f));
		}

		public bool RayIntersects(Ray ray)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			for (int s = 0; s < _sectionPoints.Length; s++)
			{
				ref Vector3[] section = ref _sectionPoints[s];
				for (int i = 0; i < _sectionPoints[s].Length; i++)
				{
					ref Vector3 point = ref section[i];
					if (PickingUtil.IntersectDistance(BoundingSphere.CreateFromPoints((IEnumerable<Vector3>)(object)new Vector3[2]
					{
						point,
						point + Vector3.get_One()
					}), ray).HasValue)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.World))
			{
				return;
			}
			AsyncTexture2D texture = _texture;
			if (texture == null || !texture.get_HasTexture() || ((GraphicsResource)_texture.get_Texture()).get_IsDisposed() || _sectionBuffers.Length == 0 || !InGameVisibility)
			{
				return;
			}
			graphicsDevice.set_RasterizerState(CullDirection);
			_packState.SharedTrailEffect.SetEntityState(AsyncTexture2D.op_Implicit(Texture), Math.Min(AnimationSpeed, _packState.UserConfiguration.PackMaxTrailAnimationSpeed.get_Value()), Math.Min(FadeNear, _packState.UserConfiguration.PackMaxViewDistance.get_Value() - (FadeFar - FadeNear)), Math.Min(FadeFar, _packState.UserConfiguration.PackMaxViewDistance.get_Value()), GetOpacity(), _packState.UserResourceStates.Advanced.CharacterTrailFadeMultiplier, CanFade && _packState.UserConfiguration.PackFadeTrailsAroundCharacter.get_Value(), Tint);
			for (int i = 0; i < _sectionBuffers.Length; i++)
			{
				ref VertexBuffer vertexBuffer = ref _sectionBuffers[i];
				graphicsDevice.SetVertexBuffer(vertexBuffer);
				Enumerator enumerator = ((Effect)_packState.SharedTrailEffect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						graphicsDevice.DrawPrimitives((PrimitiveType)1, 0, vertexBuffer.get_VertexCount() - 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Alpha(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			Alpha = _packState.UserResourceStates.Population.TrailPopulationDefaults.Alpha;
			if (collection.TryPopAttribute("alpha", out var attribute))
			{
				Alpha = MathHelper.Clamp(attribute.GetValueAsFloat(Alpha), 0f, 1f);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_AnimationSpeed(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			AnimationSpeed = _packState.UserResourceStates.Population.TrailPopulationDefaults.AnimSpeed;
			if (collection.TryPopAttribute("animspeed", out var attribute))
			{
				AnimationSpeed = attribute.GetValueAsFloat(AnimationSpeed);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_CanFade(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			CanFade = _packState.UserResourceStates.Population.MarkerPopulationDefaults.CanFade;
			if (collection.TryPopAttribute("canfade", out var attribute))
			{
				CanFade = attribute.GetValueAsBool();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Tint(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			Tint = _packState.UserResourceStates.Population.TrailPopulationDefaults.Tint;
			if (collection.TryPopAttribute("color", out var attribute2))
			{
				Tint = attribute2.GetValueAsColor(Tint);
			}
			if (collection.TryPopAttribute("tint", out var attribute))
			{
				Tint = attribute.GetValueAsColor(Tint);
			}
			if (Tint != _packState.UserResourceStates.Population.TrailPopulationDefaults.Tint)
			{
				TrailSampleColor = Tint;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Cull(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			CullDirection cullDirection = _packState.UserResourceStates.Population.TrailPopulationDefaults.Cull;
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
		private void Populate_EditTag(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("edittag", out var attribute))
			{
				base.EditTag = attribute.GetValueAsInt();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_FadeNearAndFar(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			FadeNear = _packState.UserResourceStates.Population.TrailPopulationDefaults.FadeNear;
			FadeFar = _packState.UserResourceStates.Population.TrailPopulationDefaults.FadeFar;
			if (collection.TryPopAttribute("fadenear", out var attribute2))
			{
				FadeNear = attribute2.GetValueAsFloat(FadeNear);
			}
			if (collection.TryPopAttribute("fadefar", out var attribute))
			{
				FadeFar = attribute.GetValueAsFloat(FadeFar);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Guid(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			base.Guid = _packState.UserResourceStates.Population.MarkerPopulationDefaults.Guid;
			if (collection.TryPopAttribute("guid", out var attribute))
			{
				base.Guid = attribute.GetValueAsGuid();
			}
			if (base.Guid == Guid.Empty)
			{
				base.Guid = Guid.NewGuid();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_MapVisibility(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			MiniMapVisibility = _packState.UserResourceStates.Population.TrailPopulationDefaults.MiniMapVisibility;
			MapVisibility = _packState.UserResourceStates.Population.TrailPopulationDefaults.MapVisibility;
			InGameVisibility = _packState.UserResourceStates.Population.TrailPopulationDefaults.InGameVisibility;
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
		private void Populate_Texture(AttributeCollection collection, TextureResourceManager resourceManager)
		{
			if (collection.TryGetAttribute("texture", out var attribute))
			{
				attribute.GetValueAsTextureAsync(resourceManager).ContinueWith(delegate(Task<(Texture2D Texture, Color Sample)> textureTaskResult)
				{
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					//IL_003b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Unknown result type (might be due to invalid IL or missing references)
					if (!textureTaskResult.IsFaulted && textureTaskResult.Result.Texture != null)
					{
						Texture = AsyncTexture2D.op_Implicit(textureTaskResult.Result.Texture);
						if (TrailSampleColor == Color.get_White())
						{
							TrailSampleColor = textureTaskResult.Result.Sample;
						}
					}
					else
					{
						Texture = _packState.UserResourceStates.Textures.DefaultTrailTexture;
						Logger.Warn("Trail failed to load texture '{trailTexture}'", new object[1] { attribute });
					}
				});
			}
			else
			{
				Texture = _packState.UserResourceStates.Textures.DefaultTrailTexture;
				Logger.Warn("Trail is missing 'texture' attribute.");
			}
		}

		private void Populate_TrailScale(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			TrailScale = _packState.UserResourceStates.Population.TrailPopulationDefaults.TrailScale;
			if (collection.TryPopAttribute("trailscale", out var attribute))
			{
				TrailScale = attribute.GetValueAsFloat(TrailScale);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_IsWall(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("iswall", out var attribute))
			{
				IsWall = attribute.GetValueAsBool();
			}
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
		}

		private void Populate_Behaviors(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryGetSubset("festival", out var attributes10))
			{
				AddBehavior(FestivalFilter.BuildFromAttributes(attributes10));
			}
			if (collection.TryGetSubset("mount", out var attributes9))
			{
				AddBehavior(MountFilter.BuildFromAttributes(attributes9));
			}
			if (collection.TryGetSubset("profession", out var attributes8))
			{
				AddBehavior(ProfessionFilter.BuildFromAttributes(attributes8));
			}
			if (collection.TryGetSubset("race", out var attributes7))
			{
				AddBehavior(RaceFilter.BuildFromAttributes(attributes7));
			}
			if (collection.TryGetSubset("specialization", out var attributes6))
			{
				AddBehavior(SpecializationFilter.BuildFromAttributes(attributes6));
			}
			if (collection.TryGetSubset("maptype", out var attributes5))
			{
				AddBehavior(MapTypeFilter.BuildFromAttributes(attributes5));
			}
			if (collection.TryGetSubset("schedule", out var attributes4))
			{
				AddBehavior(ScheduleFilter.BuildFromAttributes(attributes4));
			}
			if (collection.TryGetSubset("raid", out var attributes3))
			{
				AddBehavior(RaidFilter.BuildFromAttributes(attributes3, this, _packState));
			}
			if (collection.TryGetSubset("achievement", out var attributes2))
			{
				AddBehavior(AchievementFilter.BuildFromAttributes(attributes2, this, _packState));
			}
			if (_packState.UserConfiguration.ScriptsEnabled.get_Value() && collection.TryGetSubset("script", out var attributes))
			{
				AddBehavior(Script.BuildFromAttributes(attributes, this, _packState));
			}
		}

		internal IEnumerable<Vector3> PostProcessing_DouglasPeucker(IEnumerable<Vector3> points, float error = 0.2f)
		{
			Vector3[] vectors = points.ToArray();
			if (vectors.Length < 3)
			{
				return vectors;
			}
			ConcurrentBag<int> keep = new ConcurrentBag<int>
			{
				0,
				vectors.Length - 1
			};
			Recursive(0, vectors.Length - 1);
			List<int> list = keep.ToList();
			list.Sort();
			return list.Select((int i) => vectors[i]).ToList();
			void Recursive(int first, int last)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				if (last - first + 1 >= 3)
				{
					Vector3 vFirst = vectors[first];
					Vector3 lastToFirst = vectors[last] - vFirst;
					float length = ((Vector3)(ref lastToFirst)).Length();
					float maxDist = error;
					int split = 0;
					for (int j = first + 1; j < last; j++)
					{
						Vector3 v = vectors[j];
						Vector3 val = Vector3.Cross(vFirst - v, lastToFirst);
						float dist = ((Vector3)(ref val)).Length() / length;
						if (!(dist < maxDist))
						{
							maxDist = dist;
							split = j;
						}
					}
					if (split != 0)
					{
						keep.Add(split);
						Task[] array = new Task[2]
						{
							Task.Run(delegate
							{
								Recursive(first, split);
							}),
							Task.Run(delegate
							{
								Recursive(split, last);
							})
						};
						for (int k = 0; k < array.Length; k++)
						{
							array[k].Wait();
						}
					}
				}
			}
		}

		[IteratorStateMachine(typeof(_003CPostProcessing_HermiteCurve_003Ed__99))]
		private IEnumerable<Vector3> PostProcessing_HermiteCurve(IEnumerable<Vector3> points, float resolution = 0.15f, float tension = 0.5f, bool smartSampling = true, float curvatureLowerBound = 0.05f, float curvatureUpperBound = 2f, uint upsampleCount = 10u)
		{
			return new _003CPostProcessing_HermiteCurve_003Ed__99(-2)
			{
				_003C_003E3__points = points,
				_003C_003E3__resolution = resolution,
				_003C_003E3__tension = tension,
				_003C_003E3__smartSampling = smartSampling,
				_003C_003E3__curvatureLowerBound = curvatureLowerBound,
				_003C_003E3__curvatureUpperBound = curvatureUpperBound,
				_003C_003E3__upsampleCount = upsampleCount
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_Triggers(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			TriggerRange = _packState.UserResourceStates.Population.MarkerPopulationDefaults.TriggerRange;
			if (collection.TryPopAttribute("triggerrange", out var attribute))
			{
				TriggerRange = attribute.GetValueAsFloat(_packState.UserResourceStates.Population.MarkerPopulationDefaults.TriggerRange);
			}
		}

		[IteratorStateMachine(typeof(_003CPostProcessing_SetTrailResolution_003Ed__107))]
		private IEnumerable<Vector3> PostProcessing_SetTrailResolution(IEnumerable<Vector3> points, float resolution = 30f)
		{
			return new _003CPostProcessing_SetTrailResolution_003Ed__107(-2)
			{
				_003C_003E3__points = points,
				_003C_003E3__resolution = resolution
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Populate_TacOMisc(AttributeCollection collection, IPackResourceManager resourceManager)
		{
			if (collection.TryPopAttribute("resetlength", out var attribute))
			{
				ResetLength = attribute.GetValueAsFloat();
			}
		}

		public StandardTrail(IPackState packState, ITrail trail)
			: base(packState, trail)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			TextureResourceManager = TextureResourceManager.GetTextureResourceManager(trail.ResourceManager);
			Initialize(trail);
		}

		private void Populate(AttributeCollection collection, TextureResourceManager resourceManager)
		{
			Populate_Guid(collection, resourceManager);
			Populate_Alpha(collection, resourceManager);
			Populate_AnimationSpeed(collection, resourceManager);
			Populate_Tint(collection, resourceManager);
			Populate_TrailScale(collection, resourceManager);
			Populate_FadeNearAndFar(collection, resourceManager);
			Populate_Texture(collection, resourceManager);
			Populate_Cull(collection, resourceManager);
			Populate_MapVisibility(collection, resourceManager);
			Populate_CanFade(collection, resourceManager);
			Populate_IsWall(collection, resourceManager);
			Populate_Behaviors(collection, resourceManager);
		}

		private void Initialize(ITrail trail)
		{
			Populate(trail.GetAggregatedAttributes(), TextureResourceManager.GetTextureResourceManager(trail.ResourceManager));
			if (trail.TrailSections != null)
			{
				List<Vector3[]> trailSections = new List<Vector3[]>(trail.TrailSections.Count());
				foreach (ITrailSection trailSection in trail.TrailSections)
				{
					trailSections.Add(PostProcessing_DouglasPeucker(trailSection.TrailPoints.Select((Func<Vector3, Vector3>)((Vector3 v) => new Vector3(v.X, v.Y, v.Z))), _packState.UserResourceStates.Advanced.MapTrailDouglasPeuckerError).ToArray());
				}
				_sectionPoints = trailSections.ToArray();
				BuildBuffers(trail);
			}
			else
			{
				_sectionBuffers = Array.Empty<VertexBuffer>();
			}
			FadeIn();
		}
	}
}
