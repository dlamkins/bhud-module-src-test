using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public struct VertexPositionNormal : IVertexType
	{
		public Vector3 Position;

		public Vector3 Normal;

		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[2]
		{
			new VertexElement(0, (VertexElementFormat)2, (VertexElementUsage)0, 0),
			new VertexElement(12, (VertexElementFormat)2, (VertexElementUsage)3, 0)
		});

		VertexDeclaration VertexDeclaration => VertexDeclaration;

		public VertexPositionNormal(Vector3 position, Vector3 normal)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			Normal = normal;
		}
	}
}
