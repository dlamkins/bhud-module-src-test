using System.ComponentModel;

namespace felix.BlishEmotes.Services
{
	public static class TexturesExtensions
	{
		public static string ToFileName(this Textures val)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
			if (attributes.Length == 0)
			{
				return string.Empty;
			}
			return attributes[0].Description;
		}
	}
}
