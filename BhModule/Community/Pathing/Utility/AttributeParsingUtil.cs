using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Content;
using Blish_HUD;
using Cronos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Utility
{
	public static class AttributeParsingUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(AttributeParsingUtil));

		private const char ATTRIBUTEVALUE_DELIMITER = ',';

		private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

		private static IEnumerable<string> SplitAttributeValue(this IAttribute attribute)
		{
			return attribute.Value.Split(',');
		}

		private static T InternalGetValueAsEnum<T>(string attributeValue) where T : Enum
		{
			if (!EnumUtil.TryParseCacheEnum<T>(attributeValue, out var value))
			{
				return default(T);
			}
			return value;
		}

		private static int InternalGetValueAsInt(string attributeValue, int @default = 0)
		{
			if (!int.TryParse(attributeValue, NumberStyles.Any, _invariantCulture, out var value))
			{
				return @default;
			}
			return value;
		}

		private static float InternalGetValueAsFloat(string attributeValue, float @default = 0f)
		{
			if (!float.TryParse(attributeValue, NumberStyles.Any, _invariantCulture, out var value))
			{
				return @default;
			}
			return value;
		}

		internal static Guid InternalGetValueAsGuid(string attributeValue)
		{
			byte[] rawGuid = null;
			try
			{
				if (attributeValue.Length % 4 == 0 && attributeValue.EndsWith("=="))
				{
					rawGuid = Convert.FromBase64String(attributeValue);
				}
				else
				{
					using MD5 md5 = MD5.Create();
					rawGuid = md5.ComputeHash(Encoding.UTF8.GetBytes(attributeValue));
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to parse value " + attributeValue + " as a GUID.");
			}
			if (rawGuid == null || rawGuid.Length != 16)
			{
				return default(Guid);
			}
			return new Guid(rawGuid);
		}

		private static bool InternalGetValueAsBool(string attributeValue)
		{
			if (InternalGetValueAsInt(attributeValue) <= 0)
			{
				return string.Equals(attributeValue, "true", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		public static string GetValueAsString(this IAttribute attribute)
		{
			return attribute.Value;
		}

		public static IEnumerable<string> GetValueAsStrings(this IAttribute attribute)
		{
			return from val in attribute.SplitAttributeValue()
				select val.Trim();
		}

		public static int GetValueAsInt(this IAttribute attribute, int @default = 0)
		{
			return InternalGetValueAsInt(attribute.Value, @default);
		}

		public static float GetValueAsFloat(this IAttribute attribute, float @default = 0f)
		{
			return InternalGetValueAsFloat(attribute.Value, @default);
		}

		public static bool GetValueAsBool(this IAttribute attribute)
		{
			return InternalGetValueAsBool(attribute.Value);
		}

		public static Guid GetValueAsGuid(this IAttribute attribute)
		{
			return InternalGetValueAsGuid(attribute.Value);
		}

		public static IEnumerable<int> GetValueAsInts(this IAttribute attribute)
		{
			return from attr in attribute.SplitAttributeValue()
				select InternalGetValueAsInt(attr);
		}

		public static IEnumerable<float> GetValueAsFloats(this IAttribute attribute)
		{
			return from attr in attribute.SplitAttributeValue()
				select InternalGetValueAsFloat(attr);
		}

		public static IEnumerable<Guid> GetValueAsGuids(this IAttribute attribute)
		{
			return attribute.SplitAttributeValue().Select(InternalGetValueAsGuid);
		}

		public static IEnumerable<bool> GetValueAsBools(this IAttribute attribute)
		{
			return attribute.SplitAttributeValue().Select(InternalGetValueAsBool);
		}

		public static async Task<(Texture2D Texture, Color Sample)> GetValueAsTextureAsync(this IAttribute attribute, TextureResourceManager resourceManager)
		{
			return await resourceManager.LoadTextureAsync(attribute.GetValueAsString());
		}

		public static Color GetValueAsColor(this IAttribute attribute, Color @default = default(Color))
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Color color = default(Color);
			if (!ColorUtil.TryParseHex(attribute.GetValueAsString().ToLowerInvariant(), ref color))
			{
				return @default;
			}
			return color;
		}

		public static CronExpression GetValueAsCronExpression(this IAttribute attribute)
		{
			try
			{
				return (attribute.GetValueAsString().Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length > 5) ? CronExpression.Parse(attribute.GetValueAsString(), CronFormat.IncludeSeconds) : CronExpression.Parse(attribute.GetValueAsString());
			}
			catch (CronFormatException ex)
			{
				Logger.Warn((Exception)ex, "Failed to parse value {attributeValue} as a cron expression.", new object[1] { attribute.GetValueAsString() });
			}
			return null;
		}

		public static T GetValueAsEnum<T>(this IAttribute attribute) where T : Enum
		{
			return InternalGetValueAsEnum<T>(attribute.Value);
		}

		public static IEnumerable<T> GetValueAsEnums<T>(this IAttribute attribute) where T : Enum
		{
			return attribute.SplitAttributeValue().Select(InternalGetValueAsEnum<T>);
		}
	}
}