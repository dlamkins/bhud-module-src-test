using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Rendering.Blender
{
	public class ObjModel
	{
		public VertexBuffer VertexBuffer;

		public IndexBuffer IndexBuffer;

		public List<ObjModelPart> Parts = new List<ObjModelPart>();

		public BoundingBox Bounds;
	}
}
