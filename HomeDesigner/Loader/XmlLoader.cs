using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace HomeDesigner.Loader
{
	public static class XmlLoader
	{
		public static string GetHomesteadFolder()
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "Homesteads");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}

		public static XDocument LoadXml(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Dateipfad darf nicht leer sein.", "filePath");
			}
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Die Datei wurde nicht gefunden: " + filePath);
			}
			return XDocument.Load(filePath);
		}

		public static string GetMapName(XDocument doc)
		{
			return doc.Root?.Attribute("mapName")?.Value ?? "Unbekannte Karte";
		}

		public static string GetMapNameFromPath(string filePath)
		{
			try
			{
				XElement decorations = LoadXml(filePath).Element("Decorations");
				if (decorations != null && decorations.Attribute("mapName") != null)
				{
					return decorations.Attribute("mapName").Value;
				}
			}
			catch
			{
			}
			return "Unbekannte Karte";
		}

		public static XDocument MergeTemplates(IEnumerable<XDocument> templates)
		{
			if (templates == null || !templates.Any())
			{
				throw new ArgumentNullException("templates");
			}
			XElement firstRoot = (templates.FirstOrDefault((XDocument t) => t?.Element("Decorations") != null) ?? throw new InvalidOperationException("Keine gültigen Templates vorhanden.")).Element("Decorations");
			XElement mergedDecorations = new XElement(firstRoot.Name, firstRoot.Attributes());
			string firstMapId = firstRoot.Attribute("mapId")?.Value;
			foreach (XDocument doc in templates)
			{
				if (doc == null)
				{
					continue;
				}
				try
				{
					XElement decorations = doc.Element("Decorations");
					if (decorations == null || decorations.Attribute("mapId")?.Value != firstMapId)
					{
						continue;
					}
					foreach (XElement prop in decorations.Elements("prop"))
					{
						mergedDecorations.Add(new XElement(prop));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Fehler beim Mergen eines Templates: " + ex.Message);
				}
			}
			return new XDocument(mergedDecorations);
		}

		private static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static Quaternion EulerRadiantStringToQuaternion(string rotString)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(rotString))
			{
				return Quaternion.get_Identity();
			}
			string[] parts = rotString.Split(' ');
			if (parts.Length != 3)
			{
				return Quaternion.get_Identity();
			}
			float pitch = 0f;
			float yaw = 0f;
			float roll = 0f;
			float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out pitch);
			float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out yaw);
			float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out roll);
			roll = 0f - roll;
			return Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
		}

		public static void QuaternionToYawPitchRoll(Quaternion q, out float yaw, out float pitch, out float roll)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			q = Quaternion.Normalize(q);
			float t0 = 2f * (q.W * q.Y + q.X * q.Z);
			float t1 = 1f - 2f * (q.Y * q.Y + q.X * q.X);
			yaw = (float)Math.Atan2(t0, t1);
			float t2 = 2f * (q.W * q.X - q.Z * q.Y);
			t2 = Clamp(t2, -1f, 1f);
			pitch = (float)Math.Asin(t2);
			float t3 = 2f * (q.W * q.Z + q.Y * q.X);
			float t4 = 1f - 2f * (q.Z * q.Z + q.X * q.X);
			roll = (float)Math.Atan2(t3, t4);
			roll = 0f - roll;
		}

		public static string QuaternionToEulerRadiantString(Quaternion q)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			QuaternionToYawPitchRoll(q, out var yaw, out var pitch, out var roll);
			return pitch.ToString("F6", CultureInfo.InvariantCulture) + " " + yaw.ToString("F6", CultureInfo.InvariantCulture) + " " + roll.ToString("F6", CultureInfo.InvariantCulture);
		}

		public static float mapToPlayer(float x)
		{
			return 0.0254f * x;
		}

		public static double playerToMap(double x)
		{
			return x / 0.0254;
		}

		public static XDocument SaveBlueprintObjectsToXml(List<BlueprintObject> objects, string mapId)
		{
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			if (objects == null)
			{
				return null;
			}
			string mapName = "unknown";
			if (mapId == "1596")
			{
				mapName = "Comosus Isle";
			}
			else if (mapId == "1558")
			{
				mapName = "Hearth's Glow";
			}
			XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", null), new XElement("Decorations", new XAttribute("version", "1"), new XAttribute("mapId", mapId), new XAttribute("mapName", mapName), new XAttribute("type", "0")));
			XElement root = doc.Root;
			foreach (BlueprintObject obj in objects)
			{
				double posX = playerToMap(obj.Position.X);
				double posY = playerToMap(obj.Position.Y);
				double posZ = playerToMap(obj.Position.Z) * -1.0;
				string posString = posX.ToString("F6", CultureInfo.InvariantCulture) + " " + posY.ToString("F6", CultureInfo.InvariantCulture) + " " + posZ.ToString("F6", CultureInfo.InvariantCulture);
				string rotString = QuaternionToEulerRadiantString(obj.RotationQuaternion);
				string sclString = obj.Scale.ToString("F6", CultureInfo.InvariantCulture);
				if (obj.payloadValue == null)
				{
					root.Add(new XElement("prop", new XAttribute("id", obj.Id), new XAttribute("name", obj.Name), new XAttribute("pos", posString), new XAttribute("rot", rotString), new XAttribute("scl", sclString)));
				}
				else
				{
					root.Add(new XElement("prop", new XAttribute("id", obj.Id), new XAttribute("name", obj.Name), new XAttribute("pos", posString), new XAttribute("rot", rotString), new XAttribute("scl", sclString), new XElement("payload", new XAttribute("pt", obj.payloadPT), new XAttribute("v", obj.payloadV), obj.payloadValue)));
				}
			}
			return doc;
		}

		public static List<BlueprintObject> LoadBlueprintObjectsFromXml(string filePath)
		{
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			List<BlueprintObject> result = new List<BlueprintObject>();
			if (!File.Exists(filePath))
			{
				throw new Exception("File not found");
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filePath);
			foreach (XmlNode node in xmlDocument.SelectNodes("/Decorations/prop") ?? throw new Exception("XML file has the wrong formate"))
			{
				if (node.Attributes == null)
				{
					continue;
				}
				BlueprintObject obj = new BlueprintObject();
				obj.ModelKey = node.Attributes["id"]?.Value ?? "unknown";
				obj.Name = node.Attributes["name"]?.Value;
				if (int.TryParse(node.Attributes["id"].Value, out var intId))
				{
					obj.Id = intId;
					string posStr = node.Attributes["pos"]?.Value;
					if (!string.IsNullOrEmpty(posStr))
					{
						string[] parts = posStr.Split(' ');
						if (parts.Length != 3 || !float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) || !float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y) || !float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
						{
							throw new Exception("XML file has the wrong formate");
						}
						x = mapToPlayer(x);
						y = mapToPlayer(y);
						z = mapToPlayer(z);
						obj.Position = new Vector3(x, y, 0f - z);
					}
					string rotStr = node.Attributes["rot"]?.Value;
					if (!string.IsNullOrEmpty(rotStr))
					{
						obj.RotationQuaternion = EulerRadiantStringToQuaternion(rotStr);
					}
					string sclStr = node.Attributes["scl"]?.Value;
					if (!string.IsNullOrEmpty(sclStr) && float.TryParse(sclStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var scl))
					{
						obj.Scale = scl;
					}
					if (node.FirstChild != null)
					{
						obj.payloadPT = node.FirstChild.Attributes["pt"].Value;
						obj.payloadV = node.FirstChild.Attributes["v"].Value;
						obj.payloadValue = node.FirstChild.InnerText;
					}
					result.Add(obj);
					continue;
				}
				throw new Exception("XML file has the wrong formate");
			}
			return result;
		}

		public static void AddProp(XDocument doc, string id, string name, string pos, string rot, string scl)
		{
			if (doc.Root == null)
			{
				throw new InvalidOperationException("XML-Dokument hat kein Root-Element.");
			}
			XElement newProp = new XElement("prop", new XAttribute("id", id), new XAttribute("name", name), new XAttribute("pos", pos), new XAttribute("rot", rot), new XAttribute("scl", scl));
			doc.Root.Add(newProp);
		}

		public static void RemoveProp(XDocument doc, string id, string name, string pos, string rot, string scl)
		{
			if (doc.Root == null)
			{
				throw new InvalidOperationException("XML-Dokument hat kein Root-Element.");
			}
			doc.Root.Elements("prop").FirstOrDefault((XElement p) => (string)p.Attribute("id") == id && (string)p.Attribute("name") == name && (string)p.Attribute("pos") == pos && (string)p.Attribute("rot") == rot && (string)p.Attribute("scl") == scl)?.Remove();
		}

		public static void SaveXml(XDocument doc, string filePath)
		{
			doc.Save(filePath);
		}

		public static bool PropExists(XDocument doc, string id, string name, string pos, string rot, string scl)
		{
			if (doc.Root == null)
			{
				return false;
			}
			return doc.Root.Elements("prop").Any((XElement p) => (string)p.Attribute("id") == id && (string)p.Attribute("name") == name && (string)p.Attribute("pos") == pos && (string)p.Attribute("rot") == rot && (string)p.Attribute("scl") == scl);
		}
	}
}
