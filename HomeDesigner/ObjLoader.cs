using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomeDesigner
{
	public class ObjLoader
	{
		private readonly GraphicsDevice _gd;

		public VertexBuffer VertexBuffer { get; private set; }

		public IndexBuffer IndexBuffer { get; private set; }

		public int PrimitiveCount { get; private set; }

		public BoundingBox ModelBoundingBox { get; set; }

		public Vector3[] Vertices { get; private set; }

		public int[] Indices { get; private set; }

		public ObjLoader(GraphicsDevice graphicsDevice)
		{
			_gd = graphicsDevice ?? throw new ArgumentNullException("graphicsDevice");
		}

		public void Load(Stream objStream)
		{
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Expected O, but got Unknown
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Expected O, but got Unknown
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Expected O, but got Unknown
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> positions = new List<Vector3>();
			List<Vector2> texcoords = new List<Vector2>();
			List<Vector3> normals = new List<Vector3>();
			List<VertexPositionNormalTexture> vertexList = new List<VertexPositionNormalTexture>();
			List<int> indexList = new List<int>();
			Dictionary<string, int> vertexCache = new Dictionary<string, int>();
			CultureInfo inv = CultureInfo.InvariantCulture;
			using (StreamReader reader = new StreamReader(objStream))
			{
				string line;
				VertexPositionNormalTexture v2 = default(VertexPositionNormalTexture);
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (line.Length == 0 || line.StartsWith("#"))
					{
						continue;
					}
					string[] parts = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 0)
					{
						continue;
					}
					string text = parts[0];
					if (text == null)
					{
						continue;
					}
					switch (text.Length)
					{
					case 1:
						switch (text[0])
						{
						case 'v':
							if (parts.Length >= 4)
							{
								positions.Add(new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv)));
							}
							break;
						case 'f':
						{
							if (parts.Length < 4)
							{
								break;
							}
							for (int j = 1; j < parts.Length - 2; j++)
							{
								string[] tri = new string[3]
								{
									parts[1],
									parts[j + 1],
									parts[j + 2]
								};
								Vector3 faceNormal = Vector3.get_Zero();
								if (normals.Count == 0)
								{
									int a = ResolveIndex(tri[0].Split('/')[0], positions.Count);
									int b = ResolveIndex(tri[1].Split('/')[0], positions.Count);
									int c = ResolveIndex(tri[2].Split('/')[0], positions.Count);
									if (a >= 0 && b >= 0 && c >= 0)
									{
										Vector3 pA = positions[a];
										Vector3 val = positions[b];
										Vector3 pC = positions[c];
										faceNormal = Vector3.Cross(val - pA, pC - pA);
										if (faceNormal != Vector3.get_Zero())
										{
											((Vector3)(ref faceNormal)).Normalize();
										}
									}
									else
									{
										faceNormal = Vector3.get_Up();
									}
								}
								string[] array = tri;
								for (int k = 0; k < array.Length; k++)
								{
									string[] fields = array[k].Split('/');
									int vIdx = ((fields.Length >= 1) ? ResolveIndex(fields[0], positions.Count) : (-1));
									int vtIdx = ((fields.Length >= 2) ? ResolveIndex(fields[1], texcoords.Count) : (-1));
									int vnIdx = ((fields.Length >= 3) ? ResolveIndex(fields[2], normals.Count) : (-1));
									string key = $"{vIdx}/{vtIdx}/{vnIdx}";
									if (!vertexCache.TryGetValue(key, out var vertIndex))
									{
										Vector3 pos = ((vIdx >= 0 && vIdx < positions.Count) ? positions[vIdx] : Vector3.get_Zero());
										Vector2 tex = ((vtIdx >= 0 && vtIdx < texcoords.Count) ? texcoords[vtIdx] : Vector2.get_Zero());
										Vector3 norm = ((vnIdx >= 0 && vnIdx < normals.Count) ? normals[vnIdx] : faceNormal);
										if (norm != Vector3.get_Zero())
										{
											((Vector3)(ref norm)).Normalize();
										}
										else
										{
											norm = Vector3.get_Up();
										}
										((VertexPositionNormalTexture)(ref v2))._002Ector(pos, norm, tex);
										vertIndex = vertexList.Count;
										vertexList.Add(v2);
										vertexCache.Add(key, vertIndex);
									}
									indexList.Add(vertIndex);
								}
							}
							break;
						}
						}
						break;
					case 2:
						switch (text[1])
						{
						case 't':
							if (text == "vt" && parts.Length >= 3)
							{
								texcoords.Add(new Vector2(float.Parse(parts[1], inv), 1f - float.Parse(parts[2], inv)));
							}
							break;
						case 'n':
							if (text == "vn" && parts.Length >= 4)
							{
								normals.Add(new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv)));
							}
							break;
						}
						break;
					case 6:
						switch (text[0])
						{
						case 'u':
							if (text == "usemtl")
							{
							}
							break;
						case 'm':
							if (text == "mtllib")
							{
							}
							break;
						}
						break;
					}
				}
			}
			if (vertexList.Count == 0 || indexList.Count == 0)
			{
				throw new InvalidDataException("OBJ enthält keine gültigen Vertices oder Faces.");
			}
			VertexBuffer = new VertexBuffer(_gd, typeof(VertexPositionNormalTexture), vertexList.Count, (BufferUsage)1);
			VertexBuffer.SetData<VertexPositionNormalTexture>(vertexList.ToArray());
			Vertices = vertexList.Select((VertexPositionNormalTexture v) => v.Position).ToArray();
			Indices = indexList.ToArray();
			if (vertexList.Count < 65536)
			{
				ushort[] idx16 = new ushort[indexList.Count];
				for (int i = 0; i < indexList.Count; i++)
				{
					idx16[i] = (ushort)indexList[i];
				}
				IndexBuffer = new IndexBuffer(_gd, (IndexElementSize)0, idx16.Length, (BufferUsage)1);
				IndexBuffer.SetData<ushort>(idx16);
			}
			else
			{
				IndexBuffer = new IndexBuffer(_gd, (IndexElementSize)1, Indices.Length, (BufferUsage)1);
				IndexBuffer.SetData<int>(Indices);
			}
			PrimitiveCount = indexList.Count / 3;
			ModelBoundingBox = BoundingBox.CreateFromPoints(Vertices, 0, -1);
		}

		private int ResolveIndex(string token, int count)
		{
			if (string.IsNullOrEmpty(token))
			{
				return -1;
			}
			if (!int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idx))
			{
				return -1;
			}
			if (idx < 0)
			{
				return count + idx;
			}
			return idx - 1;
		}
	}
}
