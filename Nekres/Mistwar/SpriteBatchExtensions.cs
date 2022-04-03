using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Triangulation;

namespace Nekres.Mistwar
{
	public static class SpriteBatchExtensions
	{
		public static void DrawPolygonFill(this SpriteBatch spriteBatch, Vector2 position, Vector2[] vertices, Color color, bool outline = true)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			if (vertices.Length == 2)
			{
				ShapeExtensions.DrawPolygon(spriteBatch, position, (IReadOnlyList<Vector2>)vertices, color, 1f, 0f);
				return;
			}
			Color colorFill = color * (outline ? 0.5f : 1f);
			Vector2[] outVertices = default(Vector2[]);
			int[] outIndices = default(int[]);
			Triangulator.Triangulate(vertices, (WindingOrder)1, ref outVertices, ref outIndices);
			List<VertexPositionColor> triangleVertices = new List<VertexPositionColor>();
			for (int i = 0; i < outIndices.Length - 2; i += 3)
			{
				triangleVertices.Add(GetVertexPositionColor(new Vector2(outVertices[outIndices[i]].X + position.X, outVertices[outIndices[i]].Y + position.Y), colorFill));
				triangleVertices.Add(GetVertexPositionColor(new Vector2(outVertices[outIndices[i + 1]].X + position.X, outVertices[outIndices[i + 1]].Y + position.Y), colorFill));
				triangleVertices.Add(GetVertexPositionColor(new Vector2(outVertices[outIndices[i + 2]].X + position.X, outVertices[outIndices[i + 2]].Y + position.Y), colorFill));
			}
			((GraphicsResource)spriteBatch).get_GraphicsDevice().DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, triangleVertices.ToArray(), 0, triangleVertices.Count / 3);
			if (outline)
			{
				ShapeExtensions.DrawPolygon(spriteBatch, position, (IReadOnlyList<Vector2>)vertices, color, 1f, 0f);
			}
		}

		private static VertexPositionColor GetVertexPositionColor(Vector2 vertex, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new VertexPositionColor(new Vector3(vertex, -0.1f), color);
		}
	}
}
