using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomeDesigner
{
	public class BlueprintRenderer : IDisposable
	{
		private Ray? _debugRay;

		private float _debugRayLength = 200f;

		public readonly ContentsManager contentManager;

		private Dictionary<string, ObjLoader> _models = new Dictionary<string, ObjLoader>();

		private Dictionary<string, ObjLoader> _gizmoModels = new Dictionary<string, ObjLoader>();

		public Dictionary<string, Vector3> _modelPivots = new Dictionary<string, Vector3>();

		public DecorationLUT decorationLut = new DecorationLUT();

		public Dictionary<int, AsyncTexture2D> decoIconDict = new Dictionary<int, AsyncTexture2D>();

		public Dictionary<int, string> decoCategories = new Dictionary<int, string>();

		private BasicEffect _effect;

		public int renderDistance = 1000;

		public int gizmoSize = 5;

		private Dictionary<string, List<Matrix>> _precomputedWorlds = new Dictionary<string, List<Matrix>>();

		public GraphicsDevice GraphicsDevice { get; }

		public BlueprintRenderer(GraphicsDevice graphicsDevice, ContentsManager contentManager)
		{
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException("graphicsDevice");
			this.contentManager = contentManager ?? throw new ArgumentNullException("contentManager");
			BasicEffect val = new BasicEffect(GraphicsDevice);
			val.set_VertexColorEnabled(false);
			val.set_LightingEnabled(true);
			_effect = val;
			_effect.get_DirectionalLight0().set_Enabled(true);
			_effect.get_DirectionalLight0().set_DiffuseColor(Vector3.get_One());
			_effect.get_DirectionalLight0().set_Direction(Vector3.Normalize(new Vector3(0.3f, 0.5f, 0.1f)));
			_effect.get_DirectionalLight1().set_Enabled(true);
			_effect.get_DirectionalLight1().set_DiffuseColor(new Vector3(0.7f, 0.7f, 0.7f));
			_effect.get_DirectionalLight1().set_Direction(Vector3.Normalize(new Vector3(-0.3f, -0.4f, -0.1f)));
			_effect.get_DirectionalLight2().set_Enabled(true);
			_effect.get_DirectionalLight2().set_DiffuseColor(new Vector3(0.6f, 0.6f, 0.6f));
			_effect.get_DirectionalLight2().set_Direction(Vector3.Normalize(new Vector3(0f, -0.5f, -0.4f)));
			_effect.set_AmbientLightColor(new Vector3(0.5f, 0.5f, 0.5f));
		}

		public void LoadModel(string key, string path, Vector3 pivot)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (contentManager.GetFileStream(path) == null)
			{
				path = "models/placeholder.obj";
			}
			using Stream stream = contentManager.GetFileStream(path);
			ObjLoader loader = new ObjLoader(GraphicsDevice);
			loader.Load(stream);
			_models[key] = loader;
			_modelPivots[key] = pivot;
			BoundingBox bb = (loader.ModelBoundingBox = BoundingBox.CreateFromPoints(loader.Vertices, 0, -1));
		}

		public void LoadGizmoModel(string key, string path)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			using Stream stream = contentManager.GetFileStream(path);
			ObjLoader loader = new ObjLoader(GraphicsDevice);
			loader.Load(stream);
			_gizmoModels[key] = loader;
			BoundingBox bb = (loader.ModelBoundingBox = BoundingBox.CreateFromPoints(loader.Vertices, 0, -1));
		}

		public IEnumerable<string> GetModelKeys()
		{
			return _models.Keys;
		}

		public void PrecomputeWorlds(List<BlueprintObject> objects)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			float adjustScale = 0.05f;
			foreach (BlueprintObject obj in objects)
			{
				if (_models.TryGetValue(obj.ModelKey, out var loader))
				{
					Vector3 pivot = _modelPivots[obj.ModelKey];
					Matrix rotationMatrix = Matrix.CreateFromQuaternion(obj.RotationQuaternion);
					obj.BoundingBox = TransformBoundingBox(transform: obj.CachedWorld = Matrix.CreateScale(obj.Scale * adjustScale) * Matrix.CreateTranslation(-pivot) * rotationMatrix * Matrix.CreateTranslation(pivot + obj.Position), box: loader.ModelBoundingBox);
				}
			}
		}

		public void PrecomputeGizmoWorlds(List<BlueprintObject> gizmos)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			foreach (BlueprintObject gizmo in gizmos)
			{
				if (_gizmoModels.TryGetValue(gizmo.ModelKey, out var loader))
				{
					float num = Vector3.Distance(GameService.Gw2Mumble.get_PlayerCamera().get_Position(), gizmo.Position);
					float baseScale = (float)gizmoSize * 0.001f;
					float scale = num * baseScale;
					if (scale < 0.001f)
					{
						scale = 0.001f;
					}
					else if (scale > 100f)
					{
						scale = 100f;
					}
					Vector3 pivot = Vector3.get_Zero();
					Matrix rotationMatrix = Matrix.CreateFromQuaternion(gizmo.RotationQuaternion);
					gizmo.BoundingBox = TransformBoundingBox(transform: gizmo.CachedWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(-pivot) * rotationMatrix * Matrix.CreateTranslation(pivot + gizmo.Position), box: loader.ModelBoundingBox);
				}
			}
		}

		private BoundingBox TransformBoundingBox(BoundingBox box, Matrix transform)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] corners = ((BoundingBox)(ref box)).GetCorners();
			Vector3.Transform(corners, ref transform, corners);
			return BoundingBox.CreateFromPoints(corners, 0, -1);
		}

		public void Draw(Matrix view, Matrix projection, List<BlueprintObject> _objects)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice.set_BlendState(BlendState.AlphaBlend);
			GraphicsDevice graphicsDevice = GraphicsDevice;
			DepthStencilState val = new DepthStencilState();
			val.set_DepthBufferWriteEnable(false);
			val.set_DepthBufferFunction((CompareFunction)3);
			graphicsDevice.set_DepthStencilState(val);
			Vector3 playerPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			foreach (BlueprintObject obj in _objects)
			{
				if (!_models.TryGetValue(obj.ModelKey, out var loader) || Vector3.Distance(playerPos, obj.Position) > (float)renderDistance)
				{
					continue;
				}
				_effect.set_World(obj.CachedWorld);
				_effect.set_View(view);
				_effect.set_Projection(projection);
				if (obj.Selected)
				{
					_effect.set_DiffuseColor(new Vector3(0.6f, 0.6f, 0.1f));
					_effect.set_Alpha(1f);
				}
				else if (!obj.IsOriginal)
				{
					_effect.set_DiffuseColor(new Vector3(0.6f, 0.6f, 0.1f));
					_effect.set_Alpha(0.5f);
				}
				else
				{
					_effect.set_DiffuseColor(new Vector3(0.15f, 0.55f, 1f));
					_effect.set_Alpha(1f);
				}
				GraphicsDevice.SetVertexBuffer(loader.VertexBuffer);
				GraphicsDevice.set_Indices(loader.IndexBuffer);
				GraphicsDevice.set_DepthStencilState(DepthStencilState.Default);
				GraphicsDevice.set_RasterizerState(RasterizerState.CullNone);
				Enumerator enumerator2 = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						((Enumerator)(ref enumerator2)).get_Current().Apply();
						GraphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, loader.PrimitiveCount);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)).Dispose();
				}
			}
		}

		public void SetDebugRay(Ray ray, float length = 200f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_debugRay = ray;
			_debugRayLength = length;
		}

		private void DrawDebugRay(Ray ray, Matrix view, Matrix projection, float length)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Expected O, but got Unknown
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice gd = GraphicsDevice;
			if (ray.Direction == Vector3.get_Zero())
			{
				return;
			}
			Vector3 dir = Vector3.Normalize(ray.Direction);
			Vector3 start = ray.Position;
			Vector3 end = start + dir * length;
			VertexPositionColor[] lineVerts = (VertexPositionColor[])(object)new VertexPositionColor[2]
			{
				new VertexPositionColor(start, Color.get_Red()),
				new VertexPositionColor(end, Color.get_Red())
			};
			Vector3 up = Vector3.get_Up();
			if (Math.Abs(Vector3.Dot(dir, up)) > 0.99f)
			{
				up = Vector3.get_Right();
			}
			Vector3 axis1 = Vector3.Cross(dir, up);
			if (axis1 != Vector3.get_Zero())
			{
				((Vector3)(ref axis1)).Normalize();
			}
			Vector3 axis2 = Vector3.Cross(dir, axis1);
			if (axis2 != Vector3.get_Zero())
			{
				((Vector3)(ref axis2)).Normalize();
			}
			float headLength = MathHelper.Min(length * 0.05f, 2f);
			float headWidth = headLength * 0.8f;
			Vector3 p1 = end - dir * headLength + axis1 * headWidth;
			Vector3 p2 = end - dir * headLength - axis1 * headWidth;
			Vector3 p3 = end - dir * headLength + axis2 * headWidth;
			Vector3 p4 = end - dir * headLength - axis2 * headWidth;
			VertexPositionColor[] arrowVerts = (VertexPositionColor[])(object)new VertexPositionColor[8]
			{
				new VertexPositionColor(end, Color.get_Yellow()),
				new VertexPositionColor(p1, Color.get_Yellow()),
				new VertexPositionColor(end, Color.get_Yellow()),
				new VertexPositionColor(p2, Color.get_Yellow()),
				new VertexPositionColor(end, Color.get_Yellow()),
				new VertexPositionColor(p3, Color.get_Yellow()),
				new VertexPositionColor(end, Color.get_Yellow()),
				new VertexPositionColor(p4, Color.get_Yellow())
			};
			float crossSize = MathHelper.Min(length * 0.01f, 0.5f);
			List<VertexPositionColor> crossVerts = new List<VertexPositionColor>();
			crossVerts.Add(new VertexPositionColor(start - Vector3.get_Right() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(start + Vector3.get_Right() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(start - Vector3.get_Up() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(start + Vector3.get_Up() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(start - Vector3.get_Forward() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(start + Vector3.get_Forward() * crossSize, Color.get_Lime()));
			crossVerts.Add(new VertexPositionColor(end - Vector3.get_Right() * crossSize, Color.get_Cyan()));
			crossVerts.Add(new VertexPositionColor(end + Vector3.get_Right() * crossSize, Color.get_Cyan()));
			crossVerts.Add(new VertexPositionColor(end - Vector3.get_Up() * crossSize, Color.get_Cyan()));
			crossVerts.Add(new VertexPositionColor(end + Vector3.get_Up() * crossSize, Color.get_Cyan()));
			crossVerts.Add(new VertexPositionColor(end - Vector3.get_Forward() * crossSize, Color.get_Cyan()));
			crossVerts.Add(new VertexPositionColor(end + Vector3.get_Forward() * crossSize, Color.get_Cyan()));
			BasicEffect val = new BasicEffect(gd);
			val.set_VertexColorEnabled(true);
			val.set_World(Matrix.get_Identity());
			val.set_View(view);
			val.set_Projection(projection);
			BasicEffect fx = val;
			try
			{
				Enumerator enumerator = ((Effect)fx).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, lineVerts, 0, 1);
						gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, arrowVerts, 0, arrowVerts.Length / 2);
						gd.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, crossVerts.ToArray(), 0, crossVerts.Count / 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
				}
			}
			finally
			{
				((IDisposable)fx)?.Dispose();
			}
		}

		public void DrawGizmo(Matrix view, Matrix projection, List<BlueprintObject> gizmoObjects, BlueprintObject activeGizmo)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			if (gizmoObjects == null || gizmoObjects.Count == 0)
			{
				return;
			}
			GraphicsDevice gd = GraphicsDevice;
			gd.set_RasterizerState(RasterizerState.CullNone);
			gd.set_BlendState(BlendState.AlphaBlend);
			gd.set_DepthStencilState(DepthStencilState.None);
			foreach (BlueprintObject gizmo in gizmoObjects)
			{
				if (!_gizmoModels.TryGetValue(gizmo.ModelKey, out var loader))
				{
					continue;
				}
				_effect.set_World(gizmo.CachedWorld);
				_effect.set_View(view);
				_effect.set_Projection(projection);
				bool num = activeGizmo != null && gizmo.ModelKey == activeGizmo.ModelKey;
				if (gizmo.ModelKey.Contains("X"))
				{
					_effect.set_DiffuseColor(new Vector3(1f, 0f, 0f));
				}
				else if (gizmo.ModelKey.Contains("Y"))
				{
					_effect.set_DiffuseColor(new Vector3(0f, 1f, 0f));
				}
				else if (gizmo.ModelKey.Contains("Z"))
				{
					_effect.set_DiffuseColor(new Vector3(0f, 0.2f, 1f));
				}
				else
				{
					_effect.set_DiffuseColor(new Vector3(1f, 1f, 1f));
				}
				if (num)
				{
					_effect.set_Alpha(1f);
					float pulse = 0.8f + 0.2f * (float)Math.Sin(GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalSeconds * 6.0);
					BasicEffect effect = _effect;
					effect.set_DiffuseColor(effect.get_DiffuseColor() * pulse);
				}
				else
				{
					_effect.set_Alpha((activeGizmo != null) ? 0.15f : 0.4f);
					BasicEffect effect2 = _effect;
					effect2.set_DiffuseColor(effect2.get_DiffuseColor() * 0.8f);
				}
				gd.SetVertexBuffer(loader.VertexBuffer);
				gd.set_Indices(loader.IndexBuffer);
				Enumerator enumerator2 = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						((Enumerator)(ref enumerator2)).get_Current().Apply();
						gd.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, loader.PrimitiveCount);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)).Dispose();
				}
			}
			gd.set_DepthStencilState(DepthStencilState.Default);
		}

		public void Dispose()
		{
			BasicEffect effect = _effect;
			if (effect != null)
			{
				((GraphicsResource)effect).Dispose();
			}
			foreach (ObjLoader value2 in _models.Values)
			{
				VertexBuffer vertexBuffer = value2.VertexBuffer;
				if (vertexBuffer != null)
				{
					((GraphicsResource)vertexBuffer).Dispose();
				}
				IndexBuffer indexBuffer = value2.IndexBuffer;
				if (indexBuffer != null)
				{
					((GraphicsResource)indexBuffer).Dispose();
				}
			}
			_models.Clear();
			decorationLut = null;
			foreach (KeyValuePair<int, AsyncTexture2D> item in decoIconDict)
			{
				AsyncTexture2D value = item.Value;
				if (value != null)
				{
					value.Dispose();
				}
			}
			decoIconDict.Clear();
			_models.Clear();
			_gizmoModels.Clear();
		}
	}
}
