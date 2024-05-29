using System;

namespace Ideka.CustomCombatText
{
	public static class ChatLinkUtil
	{
		public enum Header : byte
		{
			Skill = 6,
			Trait
		}

		private static string FromBytes(byte[] bytes)
		{
			return "[&" + Convert.ToBase64String(bytes) + "]";
		}

		private static byte[] ValidateId(int id)
		{
			if ((id <= 16777215 && id >= 0) || 1 == 0)
			{
				return BitConverter.GetBytes(id);
			}
			throw new ArgumentException("id must be a valid unsigned 24-bit number.");
		}

		public static string Skill(int id)
		{
			byte[] idBytes = ValidateId(id);
			return FromBytes(new byte[5]
			{
				6,
				idBytes[0],
				idBytes[1],
				idBytes[2],
				0
			});
		}

		public static string Trait(int id)
		{
			byte[] idBytes = ValidateId(id);
			return FromBytes(new byte[5]
			{
				7,
				idBytes[0],
				idBytes[1],
				idBytes[2],
				0
			});
		}
	}
}
