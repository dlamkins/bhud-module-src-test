using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace flakysalt.CharacterKeybinds.Util
{
	internal static class CharacterKeybindFileUtil
	{
		public static void MoveAllXmlFiles(string sourcePath, string destinationPath)
		{
			string[] files = Directory.GetFiles(sourcePath, "*.xml");
			if (!Directory.Exists(destinationPath))
			{
				Directory.CreateDirectory(destinationPath);
			}
			string[] array = files;
			foreach (string obj in array)
			{
				string fileName = Path.GetFileName(obj);
				string destPath = Path.Combine(destinationPath, fileName);
				File.Move(obj, destPath);
			}
		}

		public static List<string> GetKeybindFiles(string path)
		{
			string[] xmlFiles = Directory.GetFiles(path, "*.xml");
			for (int i = 0; i < xmlFiles.Length; i++)
			{
				xmlFiles[i] = Path.GetFileNameWithoutExtension(xmlFiles[i]);
			}
			return xmlFiles.ToList();
		}
	}
}
