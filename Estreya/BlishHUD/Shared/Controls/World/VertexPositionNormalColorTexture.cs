using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VertexPositionNormalColorTexture : IVertexType
	{
		public Vector3 Position;

		public Vector3 Normal;

		public Color Color;

		public Vector2 TextureCoordinate;

		public static readonly VertexDeclaration VertexDeclaration;

		VertexDeclaration VertexDeclaration => VertexDeclaration;

		static VertexPositionNormalColorTexture()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			VertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[4]
			{
				new VertexElement(0, (VertexElementFormat)2, (VertexElementUsage)0, 0),
				new VertexElement(12, (VertexElementFormat)2, (VertexElementUsage)3, 0),
				new VertexElement(24, (VertexElementFormat)4, (VertexElementUsage)1, 0),
				new VertexElement(28, (VertexElementFormat)1, (VertexElementUsage)2, 0)
			});
		}

		public VertexPositionNormalColorTexture(Vector3 pos, Vector3 norm, Color col, Vector2 tex)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Position = pos;
			Normal = norm;
			Color = col;
			TextureCoordinate = tex;
		}
	}
}
