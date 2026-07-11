using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics;
using Estreya.BlishHUD.Shared.Controls.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Rendering.Blender
{
	public class BlenderRenderEntity : WorldEntity
	{
		private readonly BasicEffect _effect;

		private Dictionary<string, ObjModel> _models = new Dictionary<string, ObjModel>();

		public new float DrawOrder { get; }

		public BlenderRenderEntity(Vector3 position)
			: base(position, 1f)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				BasicEffect val = new BasicEffect(((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice());
				val.set_VertexColorEnabled(true);
				val.set_LightingEnabled(true);
				val.set_TextureEnabled(false);
				_effect = val;
				_effect.get_DirectionalLight0().set_Enabled(true);
				_effect.get_DirectionalLight0().set_DiffuseColor(Vector3.get_One());
				_effect.get_DirectionalLight0().set_Direction(Vector3.Normalize(new Vector3(0.3f, 0.5f, 0.1f)));
				_effect.get_DirectionalLight0().set_SpecularColor(Vector3.get_Zero());
				_effect.get_DirectionalLight1().set_Enabled(true);
				_effect.get_DirectionalLight1().set_DiffuseColor(new Vector3(0.7f, 0.7f, 0.7f));
				_effect.get_DirectionalLight1().set_Direction(Vector3.Normalize(new Vector3(-0.3f, -0.4f, -0.1f)));
				_effect.get_DirectionalLight2().set_Enabled(true);
				_effect.get_DirectionalLight2().set_DiffuseColor(new Vector3(0.6f, 0.6f, 0.6f));
				_effect.get_DirectionalLight2().set_Direction(Vector3.Normalize(new Vector3(0f, -0.5f, -0.4f)));
				_effect.set_AmbientLightColor(new Vector3(0.5f, 0.5f, 0.5f));
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public void LoadModel(string key, string path)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}
			using FileStream stream = new FileStream(path, FileMode.Open);
			Func<string, FileStream> fileProvider = (string fileName) => new FileStream(Path.Combine(Path.GetDirectoryName(path), fileName), FileMode.Open);
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				ObjModel model = new ObjLoader().Load(stream, ((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), fileProvider);
				_models[key] = model;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		public void LoadModel(string key, Stream fileStream, Func<string, Stream> fileProvider)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDeviceContext ctx = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				ObjModel model = new ObjLoader().Load(fileStream, ((GraphicsDeviceContext)(ref ctx)).get_GraphicsDevice(), fileProvider);
				_models[key] = model;
			}
			finally
			{
				((GraphicsDeviceContext)(ref ctx)).Dispose();
			}
		}

		private BoundingBox TransformBoundingBox(BoundingBox box, Matrix transform)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] corners = ((BoundingBox)(ref box)).GetCorners();
			Vector3.Transform(corners, ref transform, corners);
			return BoundingBox.CreateFromPoints(corners, 0, -1);
		}

		public new void Update(GameTime gameTime)
		{
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			graphicsDevice.set_DepthStencilState(DepthStencilState.Default);
			graphicsDevice.set_BlendState(BlendState.Opaque);
			graphicsDevice.get_SamplerStates().set_Item(0, SamplerState.LinearWrap);
			graphicsDevice.set_RasterizerState(RasterizerState.CullNone);
			Vector3 playerPos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			foreach (ObjModel model in _models.Values)
			{
				Vector3.Distance(playerPos, base.Position);
				Matrix worldMatrix = GetMatrix(graphicsDevice, world, camera);
				_effect.set_World(worldMatrix);
				_effect.set_View(GameService.Gw2Mumble.get_PlayerCamera().get_View());
				_effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_Projection());
				graphicsDevice.SetVertexBuffer(model.VertexBuffer);
				graphicsDevice.set_Indices(model.IndexBuffer);
				foreach (ObjModelPart part in model.Parts)
				{
					if (part.Texture != null)
					{
						_effect.set_TextureEnabled(true);
						_effect.set_Texture(part.Texture);
						_effect.set_VertexColorEnabled(false);
					}
					else
					{
						_effect.set_TextureEnabled(false);
						_effect.set_VertexColorEnabled(true);
					}
					_effect.set_DiffuseColor(Vector3.get_One());
					_effect.set_EmissiveColor(part.EmissiveColor);
					Enumerator enumerator3 = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
					try
					{
						while (((Enumerator)(ref enumerator3)).MoveNext())
						{
							((Enumerator)(ref enumerator3)).get_Current().Apply();
							graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, part.IndexOffset, part.PrimitiveCount);
						}
					}
					finally
					{
						((IDisposable)(Enumerator)(ref enumerator3)).Dispose();
					}
				}
			}
		}

		public override bool IsPlayerInside(bool includeZAxis = true)
		{
			return false;
		}
	}
}
