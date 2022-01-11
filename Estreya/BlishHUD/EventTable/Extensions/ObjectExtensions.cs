using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class ObjectExtensions
	{
		public static T DeepCopy<T>(this T item)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			binaryFormatter.Serialize(stream, item);
			stream.Seek(0L, SeekOrigin.Begin);
			T result = (T)binaryFormatter.Deserialize(stream);
			stream.Close();
			return result;
		}
	}
}
