using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Presets
{
	public static class PrimitiveExtensions
	{
		private static bool _requested;

		private static BasicEffect? _effect;

		private static BasicEffect? Effect
		{
			get
			{
				if (_effect == null && !_requested)
				{
					_requested = true;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						//IL_001f: Expected O, but got Unknown
						if (_requested && _effect == null)
						{
							_requested = false;
							_effect = new BasicEffect(graphicsDevice);
						}
					});
				}
				return _effect;
			}
		}

		public static void DrawPrimitive(this SpriteBatch spriteBatch, Primitive primitive, Color color)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			BasicEffect effect = Effect;
			if (effect != null)
			{
				VertexPositionColor[] vertices = ((IEnumerable<Vector3>)primitive.Points).Select((Func<Vector3, VertexPositionColor>)((Vector3 p) => new VertexPositionColor(p, color))).ToArray();
				effect.set_Projection(GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection());
				((Effect)effect).get_CurrentTechnique().get_Passes().get_Item(0)
					.Apply();
				((GraphicsResource)spriteBatch).get_GraphicsDevice().DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, vertices, 0, vertices.Length - 1);
			}
		}
	}
}
