using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.HitboxView
{
	public class Quad
	{
		public VertexPositionNormalTexture[] Vertices { get; }

		public int[] Indexes { get; }

		public Vector3 Origin { get; }

		public Vector3 Normal { get; }

		public Vector3 Up { get; }

		public Vector3 Left { get; }

		public Vector3 UpperLeft { get; }

		public Vector3 UpperRight { get; }

		public Vector3 LowerLeft { get; }

		public Vector3 LowerRight { get; }

		public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			Vertices = (VertexPositionNormalTexture[])(object)new VertexPositionNormalTexture[4];
			Indexes = new int[6];
			Origin = origin;
			Normal = normal;
			Up = up;
			Left = Vector3.Cross(normal, Up);
			Vector3 uppercenter = Up * height / 2f + origin;
			UpperLeft = uppercenter + Left * width / 2f;
			UpperRight = uppercenter - Left * width / 2f;
			LowerLeft = UpperLeft - Up * height;
			LowerRight = UpperRight - Up * height;
			FillVertices();
		}

		private void FillVertices()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			Vector2 textureUpperLeft = default(Vector2);
			((Vector2)(ref textureUpperLeft))._002Ector(0f, 0f);
			Vector2 textureUpperRight = default(Vector2);
			((Vector2)(ref textureUpperRight))._002Ector(1f, 0f);
			Vector2 textureLowerLeft = default(Vector2);
			((Vector2)(ref textureLowerLeft))._002Ector(0f, 1f);
			Vector2 textureLowerRight = default(Vector2);
			((Vector2)(ref textureLowerRight))._002Ector(1f, 1f);
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vertices[i].Normal = Normal;
			}
			Vertices[0].Position = LowerLeft;
			Vertices[0].TextureCoordinate = textureLowerLeft;
			Vertices[1].Position = UpperLeft;
			Vertices[1].TextureCoordinate = textureUpperLeft;
			Vertices[2].Position = LowerRight;
			Vertices[2].TextureCoordinate = textureLowerRight;
			Vertices[3].Position = UpperRight;
			Vertices[3].TextureCoordinate = textureUpperRight;
			Indexes[0] = 0;
			Indexes[1] = 1;
			Indexes[2] = 2;
			Indexes[3] = 2;
			Indexes[4] = 1;
			Indexes[5] = 3;
		}
	}
}
