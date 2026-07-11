using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Estreya.BlishHUD.Shared.Controls.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Rendering.Blender
{
	public class ObjLoader
	{
		private class MaterialDef
		{
			public Color DiffuseColor = Color.get_White();

			public Vector3 EmissiveColor = Vector3.get_Zero();

			public string TextureFilename;
		}

		public ObjModel Load(Stream objStream, GraphicsDevice graphicsDevice, Func<string, Stream> fileStreamProvider)
		{
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Expected O, but got Unknown
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Expected O, but got Unknown
			List<Vector3> positions = new List<Vector3>();
			List<Vector2> texcoords = new List<Vector2>();
			List<Vector3> normals = new List<Vector3>();
			List<VertexPositionNormalColorTexture> vertexList = new List<VertexPositionNormalColorTexture>();
			Dictionary<string, int> vertexCache = new Dictionary<string, int>();
			Dictionary<string, List<int>> indicesByMaterial = new Dictionary<string, List<int>>();
			Dictionary<string, MaterialDef> materials = new Dictionary<string, MaterialDef>();
			string currentMaterial = "Default";
			indicesByMaterial[currentMaterial] = new List<int>();
			materials[currentMaterial] = new MaterialDef();
			CultureInfo inv = CultureInfo.InvariantCulture;
			using (StreamReader reader = new StreamReader(objStream))
			{
				string line;
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
					switch (parts[0])
					{
					case "mtllib":
					{
						if (fileStreamProvider == null || parts.Length <= 1)
						{
							break;
						}
						using (Stream mtlStream = fileStreamProvider(parts[1]))
						{
							if (mtlStream != null)
							{
								LoadMtlLib(mtlStream, materials);
							}
						}
						break;
					}
					case "usemtl":
						if (parts.Length > 1)
						{
							currentMaterial = parts[1];
							if (!indicesByMaterial.ContainsKey(currentMaterial))
							{
								indicesByMaterial[currentMaterial] = new List<int>();
							}
							if (!materials.ContainsKey(currentMaterial))
							{
								materials[currentMaterial] = new MaterialDef();
							}
						}
						break;
					case "v":
						if (parts.Length >= 4)
						{
							positions.Add(new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv)));
						}
						break;
					case "vt":
						if (parts.Length >= 3)
						{
							texcoords.Add(new Vector2(float.Parse(parts[1], inv), 1f - float.Parse(parts[2], inv)));
						}
						break;
					case "vn":
						if (parts.Length >= 4)
						{
							normals.Add(new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv)));
						}
						break;
					case "f":
					{
						if (parts.Length < 4)
						{
							break;
						}
						for (int i = 1; i < parts.Length - 2; i++)
						{
							string[] obj = new string[3]
							{
								parts[1],
								parts[i + 1],
								parts[i + 2]
							};
							Vector3 faceNormal = Vector3.get_Up();
							if (normals.Count == 0)
							{
								_ = positions.Count;
								_ = 3;
							}
							string[] array = obj;
							for (int j = 0; j < array.Length; j++)
							{
								string[] fields = array[j].Split('/');
								int vIdx = ((fields.Length >= 1) ? ResolveIndex(fields[0], positions.Count) : (-1));
								int vtIdx = ((fields.Length >= 2) ? ResolveIndex(fields[1], texcoords.Count) : (-1));
								int vnIdx = ((fields.Length >= 3) ? ResolveIndex(fields[2], normals.Count) : (-1));
								Color col = materials[currentMaterial].DiffuseColor;
								string key = $"{vIdx}/{vtIdx}/{vnIdx}/{((Color)(ref col)).get_PackedValue()}";
								if (!vertexCache.TryGetValue(key, out var vertIndex))
								{
									Vector3 pos = ((vIdx >= 0 && vIdx < positions.Count) ? positions[vIdx] : Vector3.get_Zero());
									Vector2 tex = ((vtIdx >= 0 && vtIdx < texcoords.Count) ? texcoords[vtIdx] : Vector2.get_Zero());
									Vector3 norm = ((vnIdx >= 0 && vnIdx < normals.Count) ? normals[vnIdx] : faceNormal);
									if (norm == Vector3.get_Zero())
									{
										norm = Vector3.get_Up();
									}
									VertexPositionNormalColorTexture v2 = new VertexPositionNormalColorTexture(pos, norm, col, tex);
									vertIndex = vertexList.Count;
									vertexList.Add(v2);
									vertexCache.Add(key, vertIndex);
								}
								indicesByMaterial[currentMaterial].Add(vertIndex);
							}
						}
						break;
					}
					}
				}
			}
			ObjModel model = new ObjModel();
			model.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalColorTexture), vertexList.Count, (BufferUsage)1);
			model.VertexBuffer.SetData<VertexPositionNormalColorTexture>(vertexList.ToArray());
			model.Bounds = BoundingBox.CreateFromPoints(vertexList.Select((VertexPositionNormalColorTexture v) => v.Position));
			List<int> finalIndices = new List<int>();
			foreach (KeyValuePair<string, List<int>> kvp in indicesByMaterial)
			{
				string matName = kvp.Key;
				List<int> idxList = kvp.Value;
				if (idxList.Count == 0)
				{
					continue;
				}
				ObjModelPart part = new ObjModelPart();
				part.MaterialName = matName;
				part.IndexOffset = finalIndices.Count;
				part.PrimitiveCount = idxList.Count / 3;
				part.DiffuseColor = materials[matName].DiffuseColor;
				part.EmissiveColor = materials[matName].EmissiveColor;
				string texName = materials[matName].TextureFilename;
				if (!string.IsNullOrEmpty(texName) && fileStreamProvider != null)
				{
					try
					{
						using Stream texStream = fileStreamProvider(texName);
						if (texStream != null)
						{
							part.Texture = Texture2D.FromStream(graphicsDevice, texStream);
						}
					}
					catch
					{
					}
				}
				finalIndices.AddRange(idxList);
				model.Parts.Add(part);
			}
			if (finalIndices.Count > 0)
			{
				model.IndexBuffer = new IndexBuffer(graphicsDevice, (IndexElementSize)1, finalIndices.Count, (BufferUsage)1);
				model.IndexBuffer.SetData<int>(finalIndices.ToArray());
			}
			return model;
		}

		private void LoadMtlLib(Stream mtlStream, Dictionary<string, MaterialDef> materials)
		{
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			CultureInfo inv = CultureInfo.InvariantCulture;
			MaterialDef currentMat = null;
			using StreamReader reader = new StreamReader(mtlStream);
			string line;
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
				switch (parts[0].ToLower())
				{
				case "newmtl":
				{
					currentMat = new MaterialDef();
					string name = string.Join(" ", parts.Skip(1));
					materials[name] = currentMat;
					break;
				}
				case "kd":
					if (currentMat != null && parts.Length >= 4)
					{
						currentMat.DiffuseColor = new Color(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv));
					}
					break;
				case "ke":
					if (currentMat != null && parts.Length >= 4)
					{
						currentMat.EmissiveColor = new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv));
					}
					break;
				case "em":
					if (currentMat != null && parts.Length >= 4)
					{
						currentMat.EmissiveColor = new Vector3(float.Parse(parts[1], inv), float.Parse(parts[2], inv), float.Parse(parts[3], inv));
					}
					break;
				case "map_kd":
					if (currentMat != null && parts.Length >= 2)
					{
						currentMat.TextureFilename = string.Join(" ", parts.Skip(1));
					}
					break;
				}
			}
		}

		private int ResolveIndex(string token, int count)
		{
			if (int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idx))
			{
				if (idx >= 0)
				{
					return idx - 1;
				}
				return count + idx;
			}
			return -1;
		}
	}
}
