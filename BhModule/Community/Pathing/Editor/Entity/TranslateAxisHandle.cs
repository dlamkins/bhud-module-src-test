using System;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Editor.Entity
{
	public class TranslateAxisHandle : IEntity, IUpdatable, IRenderable3D, IAxisHandle, ICanPick
	{
		private readonly Matrix _axisTransform;

		private readonly VertexBuffer _buffer;

		private readonly BasicEffect _effect;

		private float _mouseIndex;

		private float _mouseOffset;

		private bool _handleActive;

		private Matrix _modelMatrix = Matrix.get_Identity();

		private static readonly int[][] _faceIndexes = new int[142][]
		{
			new int[3] { 2, 48, 1 },
			new int[3] { 1, 48, 25 },
			new int[3] { 1, 25, 24 },
			new int[3] { 24, 25, 26 },
			new int[3] { 24, 26, 27 },
			new int[3] { 48, 2, 47 },
			new int[3] { 47, 2, 3 },
			new int[3] { 47, 3, 46 },
			new int[3] { 46, 3, 4 },
			new int[3] { 46, 4, 45 },
			new int[3] { 45, 4, 5 },
			new int[3] { 45, 5, 44 },
			new int[3] { 44, 5, 6 },
			new int[3] { 44, 6, 43 },
			new int[3] { 43, 6, 7 },
			new int[3] { 43, 7, 8 },
			new int[3] { 43, 8, 42 },
			new int[3] { 42, 8, 9 },
			new int[3] { 42, 9, 41 },
			new int[3] { 41, 9, 10 },
			new int[3] { 41, 10, 40 },
			new int[3] { 40, 10, 11 },
			new int[3] { 40, 11, 39 },
			new int[3] { 39, 11, 12 },
			new int[3] { 39, 12, 38 },
			new int[3] { 38, 12, 13 },
			new int[3] { 38, 13, 37 },
			new int[3] { 37, 13, 36 },
			new int[3] { 36, 13, 14 },
			new int[3] { 36, 14, 35 },
			new int[3] { 35, 14, 15 },
			new int[3] { 35, 15, 34 },
			new int[3] { 34, 15, 16 },
			new int[3] { 34, 16, 33 },
			new int[3] { 33, 16, 17 },
			new int[3] { 33, 17, 32 },
			new int[3] { 32, 17, 18 },
			new int[3] { 32, 18, 31 },
			new int[3] { 31, 18, 19 },
			new int[3] { 31, 19, 20 },
			new int[3] { 31, 20, 30 },
			new int[3] { 30, 20, 21 },
			new int[3] { 30, 21, 29 },
			new int[3] { 29, 21, 22 },
			new int[3] { 29, 22, 28 },
			new int[3] { 28, 22, 23 },
			new int[3] { 28, 23, 27 },
			new int[3] { 27, 23, 24 },
			new int[3] { 24, 49, 1 },
			new int[3] { 22, 49, 23 },
			new int[3] { 20, 49, 21 },
			new int[3] { 18, 49, 19 },
			new int[3] { 16, 49, 17 },
			new int[3] { 14, 49, 15 },
			new int[3] { 12, 49, 13 },
			new int[3] { 10, 49, 11 },
			new int[3] { 8, 49, 9 },
			new int[3] { 6, 49, 7 },
			new int[3] { 4, 49, 5 },
			new int[3] { 2, 49, 3 },
			new int[3] { 13, 49, 14 },
			new int[3] { 21, 49, 22 },
			new int[3] { 5, 49, 6 },
			new int[3] { 15, 49, 16 },
			new int[3] { 17, 49, 18 },
			new int[3] { 19, 49, 20 },
			new int[3] { 23, 49, 24 },
			new int[3] { 1, 49, 2 },
			new int[3] { 3, 49, 4 },
			new int[3] { 7, 49, 8 },
			new int[3] { 9, 49, 10 },
			new int[3] { 11, 49, 12 },
			new int[3] { 48, 73, 25 },
			new int[3] { 25, 73, 50 },
			new int[3] { 25, 50, 26 },
			new int[3] { 26, 50, 51 },
			new int[3] { 26, 51, 27 },
			new int[3] { 27, 51, 52 },
			new int[3] { 27, 52, 28 },
			new int[3] { 28, 52, 53 },
			new int[3] { 28, 53, 29 },
			new int[3] { 29, 53, 54 },
			new int[3] { 29, 54, 30 },
			new int[3] { 30, 54, 55 },
			new int[3] { 30, 55, 31 },
			new int[3] { 31, 55, 56 },
			new int[3] { 31, 56, 32 },
			new int[3] { 32, 56, 57 },
			new int[3] { 32, 57, 33 },
			new int[3] { 33, 57, 58 },
			new int[3] { 33, 58, 34 },
			new int[3] { 34, 58, 59 },
			new int[3] { 34, 59, 35 },
			new int[3] { 35, 59, 60 },
			new int[3] { 35, 60, 36 },
			new int[3] { 36, 60, 61 },
			new int[3] { 36, 61, 37 },
			new int[3] { 37, 61, 62 },
			new int[3] { 37, 62, 38 },
			new int[3] { 38, 62, 63 },
			new int[3] { 38, 63, 39 },
			new int[3] { 39, 63, 64 },
			new int[3] { 39, 64, 40 },
			new int[3] { 40, 64, 65 },
			new int[3] { 40, 65, 41 },
			new int[3] { 41, 65, 66 },
			new int[3] { 41, 66, 42 },
			new int[3] { 42, 66, 67 },
			new int[3] { 42, 67, 43 },
			new int[3] { 43, 67, 68 },
			new int[3] { 43, 68, 44 },
			new int[3] { 44, 68, 69 },
			new int[3] { 44, 69, 45 },
			new int[3] { 45, 69, 70 },
			new int[3] { 45, 70, 46 },
			new int[3] { 46, 70, 71 },
			new int[3] { 46, 71, 47 },
			new int[3] { 47, 71, 72 },
			new int[3] { 47, 72, 48 },
			new int[3] { 48, 72, 73 },
			new int[3] { 73, 63, 50 },
			new int[3] { 50, 63, 62 },
			new int[3] { 50, 62, 51 },
			new int[3] { 51, 62, 61 },
			new int[3] { 51, 61, 52 },
			new int[3] { 52, 61, 60 },
			new int[3] { 52, 60, 53 },
			new int[3] { 53, 60, 59 },
			new int[3] { 53, 59, 54 },
			new int[3] { 54, 59, 58 },
			new int[3] { 54, 58, 55 },
			new int[3] { 55, 58, 57 },
			new int[3] { 55, 57, 56 },
			new int[3] { 63, 73, 64 },
			new int[3] { 64, 73, 72 },
			new int[3] { 64, 72, 65 },
			new int[3] { 65, 72, 71 },
			new int[3] { 65, 71, 66 },
			new int[3] { 66, 71, 70 },
			new int[3] { 66, 70, 67 },
			new int[3] { 67, 70, 69 },
			new int[3] { 67, 69, 68 }
		};

		private static readonly Vector3[] _arrowVerts = (Vector3[])(object)new Vector3[73]
		{
			new Vector3(-1f, 0f, 12f),
			new Vector3(-0.965926f, 0.258819f, 12f),
			new Vector3(-0.866025f, 0.5f, 12f),
			new Vector3(-0.707107f, 0.707107f, 12f),
			new Vector3(-0.5f, 0.866025f, 12f),
			new Vector3(-0.258819f, 0.965926f, 12f),
			new Vector3(0f, 1f, 12f),
			new Vector3(0.258819f, 0.965926f, 12f),
			new Vector3(0.5f, 0.866025f, 12f),
			new Vector3(0.707107f, 0.707107f, 12f),
			new Vector3(0.866025f, 0.5f, 12f),
			new Vector3(0.965926f, 0.258819f, 12f),
			new Vector3(1f, -0f, 12f),
			new Vector3(0.965926f, -0.258819f, 12f),
			new Vector3(0.866025f, -0.5f, 12f),
			new Vector3(0.707107f, -0.707107f, 12f),
			new Vector3(0.5f, -0.866025f, 12f),
			new Vector3(0.258819f, -0.965926f, 12f),
			new Vector3(-0f, -1f, 12f),
			new Vector3(-0.258819f, -0.965926f, 12f),
			new Vector3(-0.5f, -0.866025f, 12f),
			new Vector3(-0.707107f, -0.707107f, 12f),
			new Vector3(-0.866025f, -0.5f, 12f),
			new Vector3(-0.965926f, -0.258819f, 12f),
			new Vector3(-0.5f, -0f, 12f),
			new Vector3(-0.482963f, -0.12941f, 12f),
			new Vector3(-0.433013f, -0.25f, 12f),
			new Vector3(-0.353553f, -0.353553f, 12f),
			new Vector3(-0.25f, -0.433013f, 12f),
			new Vector3(-0.12941f, -0.482963f, 12f),
			new Vector3(0f, -0.5f, 12f),
			new Vector3(0.12941f, -0.482963f, 12f),
			new Vector3(0.25f, -0.433013f, 12f),
			new Vector3(0.353553f, -0.353553f, 12f),
			new Vector3(0.433013f, -0.25f, 12f),
			new Vector3(0.482963f, -0.12941f, 12f),
			new Vector3(0.5f, 0f, 12f),
			new Vector3(0.482963f, 0.12941f, 12f),
			new Vector3(0.433013f, 0.25f, 12f),
			new Vector3(0.353553f, 0.353553f, 12f),
			new Vector3(0.25f, 0.433013f, 12f),
			new Vector3(0.12941f, 0.482963f, 12f),
			new Vector3(0f, 0.5f, 12f),
			new Vector3(-0.12941f, 0.482963f, 12f),
			new Vector3(-0.25f, 0.433013f, 12f),
			new Vector3(-0.353553f, 0.353553f, 12f),
			new Vector3(-0.433013f, 0.25f, 12f),
			new Vector3(-0.482963f, 0.12941f, 12f),
			new Vector3(0f, -0f, 16f),
			new Vector3(-0.5f, -0f, 0f),
			new Vector3(-0.482963f, -0.12941f, 0f),
			new Vector3(-0.433013f, -0.25f, 0f),
			new Vector3(-0.353553f, -0.353553f, 0f),
			new Vector3(-0.25f, -0.433013f, 0f),
			new Vector3(-0.12941f, -0.482963f, 0f),
			new Vector3(0f, -0.5f, 0f),
			new Vector3(0.12941f, -0.482963f, 0f),
			new Vector3(0.25f, -0.433013f, 0f),
			new Vector3(0.353553f, -0.353553f, 0f),
			new Vector3(0.433013f, -0.25f, 0f),
			new Vector3(0.482963f, -0.12941f, 0f),
			new Vector3(0.5f, 0f, 0f),
			new Vector3(0.482963f, 0.12941f, 0f),
			new Vector3(0.433013f, 0.25f, 0f),
			new Vector3(0.353553f, 0.353553f, 0f),
			new Vector3(0.25f, 0.433013f, 0f),
			new Vector3(0.12941f, 0.482963f, 0f),
			new Vector3(0f, 0.5f, 0f),
			new Vector3(-0.12941f, 0.482963f, 0f),
			new Vector3(-0.25f, 0.433013f, 0f),
			new Vector3(-0.353553f, 0.353553f, 0f),
			new Vector3(-0.433013f, 0.25f, 0f),
			new Vector3(-0.482963f, 0.12941f, 0f)
		};

		public Vector3 Origin { get; set; } = Vector3.get_Zero();


		public float DrawOrder => float.MinValue;

		public TranslateAxisHandle(Color axisColor, Matrix axisTransform)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			_axisTransform = axisTransform;
			BasicEffect val = new BasicEffect(GameService.Graphics.get_GraphicsDevice());
			val.set_VertexColorEnabled(true);
			val.set_Alpha(0.4f);
			_effect = val;
			VertexPositionColor[] verts = (VertexPositionColor[])(object)new VertexPositionColor[_faceIndexes.Length * 3];
			for (int i = 0; i < _faceIndexes.Length; i++)
			{
				ref int[] faceDef = ref _faceIndexes[i];
				for (int f = 0; f < 3; f++)
				{
					verts[i * 3 + f] = new VertexPositionColor(_arrowVerts[faceDef[f] - 1], axisColor);
				}
			}
			_buffer = new VertexBuffer(GameService.Graphics.get_GraphicsDevice(), VertexPositionColor.VertexDeclaration, verts.Length, (BufferUsage)1);
			_buffer.SetData<VertexPositionColor>(verts);
		}

		public bool RayIntersects(Ray ray)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			return PickingUtil.IntersectDistance(BoundingBox.CreateFromPoints(_arrowVerts.Select((Vector3 vert) => Vector3.Transform(vert, _modelMatrix))), ray).HasValue;
		}

		public void HandleActivated(Ray ray)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_handleActive = true;
			_mouseIndex = ray.Position.Z;
		}

		public void Update(GameTime gameTime)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (_handleActive)
			{
				_mouseOffset = PickingUtil.CalculateRay(GameService.Input.get_Mouse().get_Position(), GameService.Gw2Mumble.get_PlayerCamera().get_View(), GameService.Gw2Mumble.get_PlayerCamera().get_Projection()).Position.Z;
				return;
			}
			_mouseIndex = 0f;
			_mouseOffset = 0f;
		}

		public void Render(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			graphicsDevice.SetVertexBuffer(_buffer);
			_effect.set_World(_modelMatrix = Matrix.CreateScale(0.08f) * _axisTransform * Matrix.CreateTranslation(0f, 0f, 0f - (_mouseOffset - _mouseIndex)) * Matrix.CreateTranslation(Origin));
			_effect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
			_effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
			Enumerator enumerator = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawPrimitives((PrimitiveType)0, 0, _buffer.get_VertexCount());
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}
	}
}
