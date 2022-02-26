using System;
using System.Security.Cryptography;
using System.Text;

namespace BhModule.Community.Pathing.Utility
{
	public static class GuidExtensions
	{
		public unsafe static Guid Xor(this Guid a, Guid b)
		{
			long* ap = (long*)(&a);
			long* bp = (long*)(&b);
			*ap ^= *bp;
			ap[1] ^= bp[1];
			return *(Guid*)ap;
		}

		public static Guid ToGuid(this string value)
		{
			return new Guid(MD5.Create().ComputeHash(Encoding.Default.GetBytes(value)));
		}

		public static string ToBase64String(this Guid guid)
		{
			return Convert.ToBase64String(guid.ToByteArray()).Substring(0, 22) + "==";
		}
	}
}
