using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blish_HUD.Extended
{
	internal static class StringExtensions
	{
		public static string ToSHA1Hash(this string input, bool lowerCase = true)
		{
			using SHA1Managed sha1 = new SHA1Managed();
			return string.Concat(from b in sha1.ComputeHash(Encoding.UTF8.GetBytes(input))
				select b.ToString(lowerCase ? "x2" : "X2"));
		}
	}
}
