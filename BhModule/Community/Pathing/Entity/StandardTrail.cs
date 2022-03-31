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
using BhModule.Community.Pathing.Utility.ColorThief;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Entities;
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

		private const string ATTR_FADENEAR = "fadenear";

		private const string ATTR_FADEFAR = "fadefar";

		private const string ATTR_MINIMAPVISIBILITY = "minimapvisibility";

		private const string ATTR_MAPVISIBILITY = "mapvisibility";

		private const string ATTR_INGAMEVISIBILITY = "ingamevisibility";

		private const string ATTR_TEXTURE = "texture";

		private Texture2D _texture;

		private const string ATTR_TRAILSCALE = "trailscale";

		private const string ATTR_ISWALL = "iswall";

		private const string ATTR_TRIGGERRANGE = "triggerrange";

		private const float DEFAULT_TRAILRESOLUTION = 30f;

		private const string ATTR_RESETLENGTH = "resetlength";

		private static readonly Logger Logger = Logger.GetLogger<StandardTrail>();

		private Vector3[][] _sectionPoints;

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

		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				_texture = value;
				if (_texture == null)
				{
					return;
				}
				if (Texture != null && TrailSampleColor == Color.get_White())
				{
					List<QuantizedColor> palette = ColorThief.GetPalette(Texture);
					palette.Sort((QuantizedColor color, QuantizedColor color2) => color2.Population.CompareTo(color.Population));
					Color? dominantColor = palette.FirstOrDefault()?.Color;
					if (dominantColor.HasValue)
					{
						TrailSampleColor = dominantColor.Value;
					}
				}
				FadeIn();
			}
		}

		public Color TrailSampleColor { get; set; } = Color.get_White();


		public float TrailScale { get; set; }

		public bool IsWall { get; set; }

		public override float TriggerRange { get; set; }

		public float ResetLength { get; set; }

		public override float DrawOrder => float.MaxValue;

		public override RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, (double X, double Y) offsets, double scale, float opacity)
		{
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.Map) || Texture == null)
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
					Vector2 thisPoint = GetScaledLocation(trailSection[i].X, trailSection[i].Y, scale, offsets);
					Vector2 nextPoint = GetScaledLocation(trailSection[i + 1].X, trailSection[i + 1].Y, scale, offsets);
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

		private VertexBuffer PostProcessTrailSection(IEnumerable<Vector3> points)
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
			return BuildTrailSection(pointsArr, distance);
		}

		private VertexBuffer BuildTrailSection(IEnumerable<Vector3> points, float distance)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			Vector3[] pointsArr = (points as Vector3[]) ?? points.ToArray();
			VertexPositionColorTexture[] verts = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[pointsArr.Length * 2];
			float pastDistance = distance;
			Vector3 curPoint = pointsArr[0];
			Vector3 offset = Vector3.get_Zero();
			for (int i = 0; i < pointsArr.Length - 1; i++)
			{
				Vector3 nextPoint = pointsArr[i + 1];
				offset = Vector3.Cross(nextPoint - curPoint, IsWall ? Vector3.get_Up() : Vector3.get_Forward());
				((Vector3)(ref offset)).Normalize();
				Vector3 leftPoint = curPoint + offset * 0.508f * TrailScale;
				Vector3 rightPoint = curPoint + offset * -0.508f * TrailScale;
				verts[i * 2 + 1] = new VertexPositionColorTexture(leftPoint, Color.get_White(), new Vector2(0f, pastDistance / 1.016f - 1f));
				verts[i * 2] = new VertexPositionColorTexture(rightPoint, Color.get_White(), new Vector2(1f, pastDistance / 1.016f - 1f));
				pastDistance -= Vector3.Distance(curPoint, nextPoint);
				curPoint = nextPoint;
			}
			Vector3 fleftPoint = curPoint + offset * 0.508f;
			Vector3 frightPoint = curPoint + offset * -0.508f;
			verts[pointsArr.Length * 2 - 1] = new VertexPositionColorTexture(fleftPoint, Color.get_White(), new Vector2(0f, pastDistance / 1.016f - 1f));
			verts[pointsArr.Length * 2 - 2] = new VertexPositionColorTexture(frightPoint, Color.get_White(), new Vector2(1f, pastDistance / 1.016f - 1f));
			VertexBuffer val = new VertexBuffer(GameService.Graphics.get_GraphicsDevice(), VertexPositionColorTexture.VertexDeclaration, verts.Length, (BufferUsage)1);
			val.SetData<VertexPositionColorTexture>(verts);
			return val;
		}

		private void BuildBuffers(ITrail trail)
		{
			List<VertexBuffer> buffers = new List<VertexBuffer>();
			foreach (ITrailSection section in trail.TrailSections)
			{
				VertexBuffer processedBuffer = PostProcessTrailSection(section.TrailPoints.Select((Func<Vector3, Vector3>)((Vector3 v) => new Vector3(v.X, v.Y, v.Z))));
				if (processedBuffer != null)
				{
					buffers.Add(processedBuffer);
				}
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
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			if (IsFiltered(EntityRenderTarget.World) || Texture == null || ((GraphicsResource)_texture).get_IsDisposed() || _sectionBuffers.Length == 0 || !InGameVisibility)
			{
				return;
			}
			graphicsDevice.set_RasterizerState(CullDirection);
			_packState.SharedTrailEffect.SetEntityState(Texture, Math.Min(AnimationSpeed, _packState.UserConfiguration.PackMaxTrailAnimationSpeed.get_Value()), Math.Min(FadeNear, _packState.UserConfiguration.PackMaxViewDistance.get_Value() - (FadeFar - FadeNear)), Math.Min(FadeFar, _packState.UserConfiguration.PackMaxViewDistance.get_Value()), GetOpacity(), 0.25f, CanFade && _packState.UserConfiguration.PackFadeTrailsAroundCharacter.get_Value(), base.DebugRender ? Color.get_Red() : Tint);
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
				attribute.GetValueAsTextureAsync(resourceManager).ContinueWith(delegate(Task<Texture2D> textureTaskResult)
				{
					if (!textureTaskResult.IsFaulted && textureTaskResult.Result != null)
					{
						Texture = textureTaskResult.Result;
					}
					else
					{
						Logger.Warn("Trail failed to load texture '{trailTexture}'", new object[1] { attribute });
					}
				});
			}
			else
			{
				Texture = AsyncTexture2D.op_Implicit(_packState.UserResourceStates.Textures.DefaultTrailTexture);
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
			if (collection.TryGetSubset("festival", out var attributes8))
			{
				AddBehavior(FestivalFilter.BuildFromAttributes(attributes8));
			}
			if (collection.TryGetSubset("mount", out var attributes7))
			{
				AddBehavior(MountFilter.BuildFromAttributes(attributes7));
			}
			if (collection.TryGetSubset("profession", out var attributes6))
			{
				AddBehavior(ProfessionFilter.BuildFromAttributes(attributes6));
			}
			if (collection.TryGetSubset("race", out var attributes5))
			{
				AddBehavior(RaceFilter.BuildFromAttributes(attributes5));
			}
			if (collection.TryGetSubset("specialization", out var attributes4))
			{
				AddBehavior(SpecializationFilter.BuildFromAttributes(attributes4));
			}
			if (collection.TryGetSubset("maptype", out var attributes3))
			{
				AddBehavior(MapTypeFilter.BuildFromAttributes(attributes3));
			}
			if (collection.TryGetSubset("schedule", out var attributes2))
			{
				AddBehavior(ScheduleFilter.BuildFromAttributes(attributes2));
			}
			if (collection.TryGetSubset("achievement", out var attributes))
			{
				AddBehavior(AchievementFilter.BuildFromAttributes(attributes, _packState));
			}
		}

		private IEnumerable<Vector3> PostProcessing_DouglasPeucker(IEnumerable<Vector3> points, float error = 0.2f)
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

		private IEnumerable<Vector3> PostProcessing_HermiteCurve(IEnumerable<Vector3> points, float resolution = 0.15f, float tension = 0.5f, bool smartSampling = true, float curvatureLowerBound = 0.05f, float curvatureUpperBound = 2f, uint upsampleCount = 10u)
		{
			tension = MathHelper.Clamp(tension, 0f, 1f);
			Vector3[] pointsArr = points.ToArray();
			Vector3 val;
			Vector3 prevPoint = (val = pointsArr[0]);
			yield return val;
			_003C_003Ec__DisplayClass94_0 CS_0024_003C_003E8__locals0 = default(_003C_003Ec__DisplayClass94_0);
			for (int j = 0; j < pointsArr.Length - 1; j++)
			{
				Vector3 p0 = pointsArr[j];
				Vector3 p1 = pointsArr[j + 1];
				Vector3 m0;
				if (j > 0)
				{
					m0 = tension * (p1 - pointsArr[j - 1]);
				}
				else
				{
					m0 = p1 - p0;
				}
				Vector3 m1;
				if (j < pointsArr.Length - 2)
				{
					m1 = tension * (pointsArr[j + 2] - p0);
				}
				else
				{
					m1 = p1 - p0;
				}
				uint numPoints = (uint)(SplineLength() / resolution);
				float kappa = 0f;
				for (int i = 0; i < numPoints; i++)
				{
					float t2 = (float)i * (1f / (float)numPoints);
					if (smartSampling)
					{
						kappa = GetCurvature(t2);
					}
					Vector3 sampledPoint = H00(t2) * p0 + H10(t2) * m0 + H01(t2) * p1 + H11(t2) * m1;
					if (smartSampling && kappa < curvatureLowerBound)
					{
						val = prevPoint - sampledPoint;
						if (((Vector3)(ref val)).Length() < 10f)
						{
							continue;
						}
					}
					prevPoint = sampledPoint;
					yield return sampledPoint;
					if (smartSampling && kappa > curvatureUpperBound)
					{
						float t3 = (float)(i + 1) * (1f / (float)numPoints);
						float delta = 1f / (float)upsampleCount;
						for (float k = delta; k < 1f; k += delta)
						{
							float dt = (t3 - t2) * k;
							yield return H00(t2 + dt) * p0 + H10(t2 + dt) * m0 + H01(t2 + dt) * p1 + H11(t2 + dt) * m1;
						}
					}
				}
				float GetCurvature(float t0)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0033: Unknown result type (might be due to invalid IL or missing references)
					//IL_0038: Unknown result type (might be due to invalid IL or missing references)
					//IL_0044: Unknown result type (might be due to invalid IL or missing references)
					//IL_0049: Unknown result type (might be due to invalid IL or missing references)
					//IL_004e: Unknown result type (might be due to invalid IL or missing references)
					//IL_005a: Unknown result type (might be due to invalid IL or missing references)
					//IL_005f: Unknown result type (might be due to invalid IL or missing references)
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0070: Unknown result type (might be due to invalid IL or missing references)
					//IL_0075: Unknown result type (might be due to invalid IL or missing references)
					//IL_0081: Unknown result type (might be due to invalid IL or missing references)
					//IL_0086: Unknown result type (might be due to invalid IL or missing references)
					//IL_008b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0097: Unknown result type (might be due to invalid IL or missing references)
					//IL_009c: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
					//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0102: Unknown result type (might be due to invalid IL or missing references)
					//IL_0107: Unknown result type (might be due to invalid IL or missing references)
					Vector3 val2 = Vector3.Cross(H00dt(t0) * p0 + H10dt(t0) * m0 + H01dt(t0) * p1 + H11dt(t0) * m1, H00dt2(t0) * p0 + H10dt2(t0) * m0 + H01dt2(t0) * p1 + H11dt2(t0) * m1);
					double num = ((Vector3)(ref val2)).Length();
					val2 = H00dt(t0) * p0 + H10dt(t0) * m0 + H01dt(t0) * p1 + H11dt(t0) * m1;
					return (float)(num / Math.Pow(((Vector3)(ref val2)).Length(), 3.0));
				}
				float SplineLength()
				{
					//IL_0003: Unknown result type (might be due to invalid IL or missing references)
					//IL_0008: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0045: Unknown result type (might be due to invalid IL or missing references)
					//IL_004a: Unknown result type (might be due to invalid IL or missing references)
					//IL_004f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0054: Unknown result type (might be due to invalid IL or missing references)
					//IL_0061: Unknown result type (might be due to invalid IL or missing references)
					//IL_0067: Unknown result type (might be due to invalid IL or missing references)
					//IL_006c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0071: Unknown result type (might be due to invalid IL or missing references)
					//IL_007c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0082: Unknown result type (might be due to invalid IL or missing references)
					//IL_0087: Unknown result type (might be due to invalid IL or missing references)
					//IL_008c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0091: Unknown result type (might be due to invalid IL or missing references)
					//IL_0096: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
					//IL_0119: Unknown result type (might be due to invalid IL or missing references)
					//IL_011e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0129: Unknown result type (might be due to invalid IL or missing references)
					//IL_0138: Unknown result type (might be due to invalid IL or missing references)
					//IL_013d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0146: Unknown result type (might be due to invalid IL or missing references)
					Vector3 c1 = 6f * (p1 - p0) - 4f * m0 - 2f * m1;
					Vector3 c2 = 6f * (p0 - p1) + 3f * (m1 + m0);
					List<Vector2> obj = new List<Vector2>
					{
						new Vector2(0f, 128f / 225f),
						new Vector2(-0.5384693f, 0.47862867f),
						new Vector2(0.5384693f, 0.47862867f),
						new Vector2(-0.90617985f, 0.23692688f),
						new Vector2(0.90617985f, 0.23692688f)
					};
					float length = 0f;
					foreach (Vector2 coeff in obj)
					{
						float t4 = 0.5f * (1f + coeff.X);
						float num2 = length;
						Vector3 val3 = Derivative(t4);
						length = num2 + ((Vector3)(ref val3)).Length() * coeff.Y;
					}
					return 0.5f * length;
					Vector3 Derivative(float t)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						//IL_0008: Unknown result type (might be due to invalid IL or missing references)
						//IL_000f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0014: Unknown result type (might be due to invalid IL or missing references)
						//IL_0019: Unknown result type (might be due to invalid IL or missing references)
						//IL_001e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0023: Unknown result type (might be due to invalid IL or missing references)
						return (Vector3)CS_0024_003C_003E8__locals0 + t * (c1 + t * c2);
					}
				}
			}
			yield return pointsArr.Last();
			static float H00(float t)
			{
				return (1f + 2f * t) * (float)Math.Pow(1f - t, 2.0);
			}
			static float H00dt(float t)
			{
				return 6f * t * t - 6f * t;
			}
			static float H00dt2(float t)
			{
				return 12f * t - 6f;
			}
			static float H01(float t)
			{
				return (float)Math.Pow(t, 2.0) * (3f - 2f * t);
			}
			static float H01dt(float t)
			{
				return -6f * t * t + 6f * t;
			}
			static float H01dt2(float t)
			{
				return -12f * t + 6f;
			}
			static float H10(float t)
			{
				return t * (float)Math.Pow(1f - t, 2.0);
			}
			static float H10dt(float t)
			{
				return 3f * t * t - 4f * t + 1f;
			}
			static float H10dt2(float t)
			{
				return 6f * t - 4f;
			}
			static float H11(float t)
			{
				return (float)Math.Pow(t, 2.0) * (t - 1f);
			}
			static float H11dt(float t)
			{
				return 3f * t * t - 2f * t;
			}
			static float H11dt2(float t)
			{
				return 6f * t - 2f;
			}
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

		private IEnumerable<Vector3> PostProcessing_SetTrailResolution(IEnumerable<Vector3> points, float resolution = 30f)
		{
			Vector3[] pointsArr = (points as Vector3[]) ?? points.ToArray();
			if (pointsArr.Length < 1)
			{
				yield break;
			}
			Vector3 val;
			Vector3 prevPoint = (val = pointsArr[0]);
			yield return val;
			for (int i = 1; i < pointsArr.Length; i++)
			{
				Vector3 curPoint = pointsArr[i];
				float dist = Vector3.Distance(prevPoint, curPoint);
				float s = dist / resolution;
				float inc = 1f / s;
				for (float v = inc; v < s - inc; v += inc)
				{
					yield return Vector3.Lerp(prevPoint, curPoint, v / s);
				}
				prevPoint = curPoint;
				yield return curPoint;
			}
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
			Initialize(trail);
		}

		private void Populate(AttributeCollection collection, TextureResourceManager resourceManager)
		{
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
			List<Vector3[]> trailSections = new List<Vector3[]>(trail.TrailSections.Count());
			foreach (ITrailSection trailSection in trail.TrailSections)
			{
				trailSections.Add(PostProcessing_DouglasPeucker(trailSection.TrailPoints.Select((Func<Vector3, Vector3>)((Vector3 v) => new Vector3(v.X, v.Y, v.Z))), _packState.UserResourceStates.Static.MapTrailDouglasPeuckerError).ToArray());
			}
			_sectionPoints = trailSections.ToArray();
			Populate(trail.GetAggregatedAttributes(), TextureResourceManager.GetTextureResourceManager(trail.ResourceManager));
			BuildBuffers(trail);
			FadeIn();
		}
	}
}
