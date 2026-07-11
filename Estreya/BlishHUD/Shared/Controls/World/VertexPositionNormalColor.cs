using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VertexPositionNormalColor : IVertexType
	{
		public Vector3 Position;

		public Vector3 Normal;

		public Color Color;

		public static readonly VertexDeclaration VertexDeclaration;

		VertexDeclaration VertexDeclaration => VertexDeclaration;

		static VertexPositionNormalColor()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			VertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[3]
			{
				new VertexElement(0, (VertexElementFormat)2, (VertexElementUsage)0, 0),
				new VertexElement(12, (VertexElementFormat)2, (VertexElementUsage)3, 0),
				new VertexElement(24, (VertexElementFormat)4, (VertexElementUsage)1, 0)
			});
		}

		public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			Normal = normal;
			Color = color;
		}
	}
}
