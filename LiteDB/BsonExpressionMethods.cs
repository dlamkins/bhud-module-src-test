using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiteDB
{
	internal class BsonExpressionMethods
	{
		private static readonly Random _random = new Random();

		public static BsonValue COUNT(IEnumerable<BsonValue> values)
		{
			return values.Count();
		}

		public static BsonValue MIN(IEnumerable<BsonValue> values)
		{
			BsonValue min = BsonValue.MaxValue;
			foreach (BsonValue value in values)
			{
				if (value.CompareTo(min) <= 0)
				{
					min = value;
				}
			}
			if (!(min == BsonValue.MaxValue))
			{
				return min;
			}
			return BsonValue.MinValue;
		}

		public static BsonValue MAX(IEnumerable<BsonValue> values)
		{
			BsonValue max = BsonValue.MinValue;
			foreach (BsonValue value in values)
			{
				if (value.CompareTo(max) >= 0)
				{
					max = value;
				}
			}
			if (!(max == BsonValue.MinValue))
			{
				return max;
			}
			return BsonValue.MaxValue;
		}

		public static BsonValue FIRST(IEnumerable<BsonValue> values)
		{
			return values.FirstOrDefault();
		}

		public static BsonValue LAST(IEnumerable<BsonValue> values)
		{
			return values.LastOrDefault();
		}

		public static BsonValue AVG(IEnumerable<BsonValue> values)
		{
			BsonValue sum = new BsonValue(0);
			int count = 0;
			foreach (BsonValue value in values.Where((BsonValue x) => x.IsNumber))
			{
				sum += value;
				count++;
			}
			if (count > 0)
			{
				return sum / count;
			}
			return 0;
		}

		public static BsonValue SUM(IEnumerable<BsonValue> values)
		{
			BsonValue sum = new BsonValue(0);
			foreach (BsonValue value in values.Where((BsonValue x) => x.IsNumber))
			{
				sum += value;
			}
			return sum;
		}

		public static BsonValue ANY(IEnumerable<BsonValue> values)
		{
			return values.Any();
		}

		public static BsonValue MINVALUE()
		{
			return BsonValue.MinValue;
		}

		[Volatile]
		public static BsonValue OBJECTID()
		{
			return ObjectId.NewObjectId();
		}

		[Volatile]
		public static BsonValue GUID()
		{
			return Guid.NewGuid();
		}

		[Volatile]
		public static BsonValue NOW()
		{
			return DateTime.Now;
		}

		[Volatile]
		public static BsonValue NOW_UTC()
		{
			return DateTime.UtcNow;
		}

		[Volatile]
		public static BsonValue TODAY()
		{
			return DateTime.Today;
		}

		public static BsonValue MAXVALUE()
		{
			return BsonValue.MaxValue;
		}

		public static BsonValue INT32(BsonValue value)
		{
			if (value.IsNumber)
			{
				return value.AsInt32;
			}
			if (value.IsString && int.TryParse(value.AsString, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue INT64(BsonValue value)
		{
			if (value.IsNumber)
			{
				return value.AsInt64;
			}
			if (value.IsString && long.TryParse(value.AsString, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DOUBLE(Collation collation, BsonValue value)
		{
			if (value.IsNumber)
			{
				return value.AsDouble;
			}
			if (value.IsString && double.TryParse(value.AsString, NumberStyles.Any, collation.Culture.NumberFormat, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DOUBLE(BsonValue value, BsonValue culture)
		{
			if (value.IsNumber)
			{
				return value.AsDouble;
			}
			if (value.IsString && culture.IsString)
			{
				CultureInfo c = new CultureInfo(culture.AsString);
				if (double.TryParse(value.AsString, NumberStyles.Any, c.NumberFormat, out var val))
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue DECIMAL(Collation collation, BsonValue value)
		{
			if (value.IsNumber)
			{
				return value.AsDecimal;
			}
			if (value.IsString && decimal.TryParse(value.AsString, NumberStyles.Any, collation.Culture.NumberFormat, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DECIMAL(BsonValue value, BsonValue culture)
		{
			if (value.IsNumber)
			{
				return value.AsDecimal;
			}
			if (value.IsString && culture.IsString)
			{
				CultureInfo c = new CultureInfo(culture.AsString);
				if (decimal.TryParse(value.AsString, NumberStyles.Any, c.NumberFormat, out var val))
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue STRING(BsonValue value)
		{
			return value.IsNull ? "" : (value.IsString ? value.AsString : value.ToString());
		}

		public static BsonValue ARRAY(IEnumerable<BsonValue> values)
		{
			return new BsonArray(values);
		}

		public static BsonValue BINARY(BsonValue value)
		{
			if (value.IsBinary)
			{
				return value;
			}
			if (value.IsString)
			{
				byte[] data = null;
				bool isBase64 = false;
				try
				{
					data = Convert.FromBase64String(value.AsString);
					isBase64 = true;
				}
				catch (FormatException)
				{
				}
				if (isBase64)
				{
					return data;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue OBJECTID(BsonValue value)
		{
			if (value.IsObjectId)
			{
				return value.AsObjectId;
			}
			if (value.IsString)
			{
				ObjectId val = null;
				bool isObjectId = false;
				try
				{
					val = new ObjectId(value.AsString);
					isObjectId = true;
				}
				catch
				{
				}
				if (isObjectId)
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue GUID(BsonValue value)
		{
			if (value.IsGuid)
			{
				return value.AsGuid;
			}
			if (value.IsString)
			{
				Guid val = Guid.Empty;
				bool isGuid = false;
				try
				{
					val = new Guid(value.AsString);
					isGuid = true;
				}
				catch
				{
				}
				if (isGuid)
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue BOOLEAN(BsonValue value)
		{
			if (value.IsBoolean)
			{
				return value.AsBoolean;
			}
			bool val = false;
			bool isBool = false;
			try
			{
				val = Convert.ToBoolean(value.AsString);
				isBool = true;
			}
			catch
			{
			}
			if (isBool)
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME(Collation collation, BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime;
			}
			if (value.IsString && DateTime.TryParse(value.AsString, collation.Culture.DateTimeFormat, DateTimeStyles.None, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME(BsonValue value, BsonValue culture)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime;
			}
			if (value.IsString && culture.IsString)
			{
				CultureInfo c = new CultureInfo(culture.AsString);
				if (DateTime.TryParse(value.AsString, c.DateTimeFormat, DateTimeStyles.None, out var val))
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME_UTC(Collation collation, BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime;
			}
			if (value.IsString && DateTime.TryParse(value.AsString, collation.Culture.DateTimeFormat, DateTimeStyles.AssumeUniversal, out var val))
			{
				return val;
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME_UTC(BsonValue value, BsonValue culture)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime;
			}
			if (value.IsString && culture.IsString)
			{
				CultureInfo c = new CultureInfo(culture.AsString);
				if (DateTime.TryParse(value.AsString, c.DateTimeFormat, DateTimeStyles.AssumeUniversal, out var val))
				{
					return val;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME(BsonValue year, BsonValue month, BsonValue day)
		{
			if (year.IsNumber && month.IsNumber && day.IsNumber)
			{
				return new DateTime(year.AsInt32, month.AsInt32, day.AsInt32);
			}
			return BsonValue.Null;
		}

		public static BsonValue DATETIME_UTC(BsonValue year, BsonValue month, BsonValue day)
		{
			if (year.IsNumber && month.IsNumber && day.IsNumber)
			{
				return new DateTime(year.AsInt32, month.AsInt32, day.AsInt32, 0, 0, 0, DateTimeKind.Utc);
			}
			return BsonValue.Null;
		}

		public static BsonValue IS_MINVALUE(BsonValue value)
		{
			return value.IsMinValue;
		}

		public static BsonValue IS_NULL(BsonValue value)
		{
			return value.IsNull;
		}

		public static BsonValue IS_INT32(BsonValue value)
		{
			return value.IsInt32;
		}

		public static BsonValue IS_INT64(BsonValue value)
		{
			return value.IsInt64;
		}

		public static BsonValue IS_DOUBLE(BsonValue value)
		{
			return value.IsDouble;
		}

		public static BsonValue IS_DECIMAL(BsonValue value)
		{
			return value.IsDecimal;
		}

		public static BsonValue IS_NUMBER(BsonValue value)
		{
			return value.IsNumber;
		}

		public static BsonValue IS_STRING(BsonValue value)
		{
			return value.IsString;
		}

		public static BsonValue IS_DOCUMENT(BsonValue value)
		{
			return value.IsDocument;
		}

		public static BsonValue IS_ARRAY(BsonValue value)
		{
			return value.IsArray;
		}

		public static BsonValue IS_BINARY(BsonValue value)
		{
			return value.IsBinary;
		}

		public static BsonValue IS_OBJECTID(BsonValue value)
		{
			return value.IsObjectId;
		}

		public static BsonValue IS_GUID(BsonValue value)
		{
			return value.IsGuid;
		}

		public static BsonValue IS_BOOLEAN(BsonValue value)
		{
			return value.IsBoolean;
		}

		public static BsonValue IS_DATETIME(BsonValue value)
		{
			return value.IsDateTime;
		}

		public static BsonValue IS_MAXVALUE(BsonValue value)
		{
			return value.IsMaxValue;
		}

		public static BsonValue INT(BsonValue value)
		{
			return INT32(value);
		}

		public static BsonValue LONG(BsonValue value)
		{
			return INT64(value);
		}

		public static BsonValue BOOL(BsonValue value)
		{
			return BOOLEAN(value);
		}

		public static BsonValue DATE(Collation collation, BsonValue value)
		{
			return DATETIME(collation, value);
		}

		public static BsonValue DATE(BsonValue values, BsonValue culture)
		{
			return DATETIME(values, culture);
		}

		public static BsonValue DATE_UTC(Collation collation, BsonValue value)
		{
			return DATETIME_UTC(collation, value);
		}

		public static BsonValue DATE_UTC(BsonValue values, BsonValue culture)
		{
			return DATETIME_UTC(values, culture);
		}

		public static BsonValue DATE(BsonValue year, BsonValue month, BsonValue day)
		{
			return DATETIME(year, month, day);
		}

		public static BsonValue DATE_UTC(BsonValue year, BsonValue month, BsonValue day)
		{
			return DATETIME_UTC(year, month, day);
		}

		public static BsonValue IS_INT(BsonValue value)
		{
			return IS_INT32(value);
		}

		public static BsonValue IS_LONG(BsonValue value)
		{
			return IS_INT64(value);
		}

		public static BsonValue IS_BOOL(BsonValue value)
		{
			return IS_BOOLEAN(value);
		}

		public static BsonValue IS_DATE(BsonValue value)
		{
			return IS_DATETIME(value);
		}

		public static BsonValue YEAR(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Year;
			}
			return BsonValue.Null;
		}

		public static BsonValue MONTH(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Month;
			}
			return BsonValue.Null;
		}

		public static BsonValue DAY(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Day;
			}
			return BsonValue.Null;
		}

		public static BsonValue HOUR(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Hour;
			}
			return BsonValue.Null;
		}

		public static BsonValue MINUTE(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Minute;
			}
			return BsonValue.Null;
		}

		public static BsonValue SECOND(BsonValue value)
		{
			if (value.IsDateTime)
			{
				return value.AsDateTime.Second;
			}
			return BsonValue.Null;
		}

		public static BsonValue DATEADD(BsonValue dateInterval, BsonValue number, BsonValue value)
		{
			if (dateInterval.IsString && number.IsNumber && value.IsDateTime)
			{
				string datePart = dateInterval.AsString;
				int numb = number.AsInt32;
				DateTime date = value.AsDateTime;
				switch ((datePart == "M") ? "month" : datePart.ToLower())
				{
				case "y":
				case "year":
					return date.AddYears(numb);
				case "month":
					return date.AddMonths(numb);
				case "d":
				case "day":
					return date.AddDays(numb);
				case "h":
				case "hour":
					return date.AddHours(numb);
				case "m":
				case "minute":
					return date.AddMinutes(numb);
				case "s":
				case "second":
					return date.AddSeconds(numb);
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue DATEDIFF(BsonValue dateInterval, BsonValue starts, BsonValue ends)
		{
			if (dateInterval.IsString && starts.IsDateTime && ends.IsDateTime)
			{
				string datePart = dateInterval.AsString;
				DateTime start = starts.AsDateTime;
				DateTime end = ends.AsDateTime;
				switch ((datePart == "M") ? "month" : datePart.ToLower())
				{
				case "y":
				case "year":
					return start.YearDifference(end);
				case "month":
					return start.MonthDifference(end);
				case "d":
				case "day":
					return Convert.ToInt32(Math.Truncate(end.Subtract(start).TotalDays));
				case "h":
				case "hour":
					return Convert.ToInt32(Math.Truncate(end.Subtract(start).TotalHours));
				case "m":
				case "minute":
					return Convert.ToInt32(Math.Truncate(end.Subtract(start).TotalMinutes));
				case "s":
				case "second":
					return Convert.ToInt32(Math.Truncate(end.Subtract(start).TotalSeconds));
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue TO_LOCAL(BsonValue date)
		{
			if (date.IsDateTime)
			{
				return date.AsDateTime.ToLocalTime();
			}
			return BsonValue.Null;
		}

		public static BsonValue TO_UTC(BsonValue date)
		{
			if (date.IsDateTime)
			{
				return date.AsDateTime.ToUniversalTime();
			}
			return BsonValue.Null;
		}

		public static BsonValue ABS(BsonValue value)
		{
			return value.Type switch
			{
				BsonType.Int32 => Math.Abs(value.AsInt32), 
				BsonType.Int64 => Math.Abs(value.AsInt64), 
				BsonType.Double => Math.Abs(value.AsDouble), 
				BsonType.Decimal => Math.Abs(value.AsDecimal), 
				_ => BsonValue.Null, 
			};
		}

		public static BsonValue ROUND(BsonValue value, BsonValue digits)
		{
			if (digits.IsNumber)
			{
				switch (value.Type)
				{
				case BsonType.Int32:
					return value.AsInt32;
				case BsonType.Int64:
					return value.AsInt64;
				case BsonType.Double:
					return Math.Round(value.AsDouble, digits.AsInt32);
				case BsonType.Decimal:
					return Math.Round(value.AsDecimal, digits.AsInt32);
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue POW(BsonValue x, BsonValue y)
		{
			if (x.IsNumber && y.IsNumber)
			{
				return Math.Pow(x.AsDouble, y.AsDouble);
			}
			return BsonValue.Null;
		}

		public static BsonValue JSON(BsonValue json)
		{
			if (json.IsString)
			{
				BsonValue value = null;
				bool isJson = false;
				try
				{
					value = JsonSerializer.Deserialize(json.AsString);
					isJson = true;
				}
				catch (LiteException ex) when (ex.ErrorCode == 203)
				{
				}
				if (isJson)
				{
					return value;
				}
			}
			return BsonValue.Null;
		}

		public static BsonValue EXTEND(BsonValue source, BsonValue extend)
		{
			if (source.IsDocument && extend.IsDocument)
			{
				BsonDocument newDoc = new BsonDocument();
				source.AsDocument.CopyTo(newDoc);
				extend.AsDocument.CopyTo(newDoc);
				newDoc.RawId = source.AsDocument.RawId;
				return newDoc;
			}
			if (source.IsDocument)
			{
				return source;
			}
			if (extend.IsDocument)
			{
				return extend;
			}
			return new BsonDocument();
		}

		public static IEnumerable<BsonValue> ITEMS(BsonValue array)
		{
			if (array.IsArray)
			{
				foreach (BsonValue item in array.AsArray)
				{
					yield return item;
				}
			}
			else if (array.IsBinary)
			{
				byte[] asBinary = array.AsBinary;
				foreach (byte value in asBinary)
				{
					yield return value;
				}
			}
			else
			{
				yield return array;
			}
		}

		public static IEnumerable<BsonValue> CONCAT(IEnumerable<BsonValue> first, IEnumerable<BsonValue> second)
		{
			return first.Concat(second);
		}

		public static IEnumerable<BsonValue> KEYS(BsonValue document)
		{
			if (!document.IsDocument)
			{
				yield break;
			}
			foreach (string key in document.AsDocument.Keys)
			{
				yield return key;
			}
		}

		public static IEnumerable<BsonValue> VALUES(BsonValue document)
		{
			if (!document.IsDocument)
			{
				yield break;
			}
			foreach (BsonValue value in document.AsDocument.Values)
			{
				yield return value;
			}
		}

		public static BsonValue OID_CREATIONTIME(BsonValue objectId)
		{
			if (objectId.IsObjectId)
			{
				return objectId.AsObjectId.CreationTime;
			}
			return BsonValue.Null;
		}

		public static BsonValue IIF(BsonValue test, BsonValue ifTrue, BsonValue ifFalse)
		{
			throw new NotImplementedException();
		}

		public static BsonValue COALESCE(BsonValue left, BsonValue right)
		{
			if (!left.IsNull)
			{
				return left;
			}
			return right;
		}

		public static BsonValue LENGTH(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.Length;
			}
			if (value.IsBinary)
			{
				return value.AsBinary.Length;
			}
			if (value.IsArray)
			{
				return value.AsArray.Count;
			}
			if (value.IsDocument)
			{
				return value.AsDocument.Keys.Count;
			}
			if (value.IsNull)
			{
				return 0;
			}
			return BsonValue.Null;
		}

		public static IEnumerable<BsonValue> TOP(IEnumerable<BsonValue> values, BsonValue num)
		{
			if (num.IsInt32 || num.IsInt64)
			{
				int numInt = num.AsInt32;
				if (numInt > 0)
				{
					return values.Take(numInt);
				}
			}
			return Enumerable.Empty<BsonValue>();
		}

		public static IEnumerable<BsonValue> UNION(IEnumerable<BsonValue> left, IEnumerable<BsonValue> right)
		{
			return left.Union(right);
		}

		public static IEnumerable<BsonValue> EXCEPT(IEnumerable<BsonValue> left, IEnumerable<BsonValue> right)
		{
			return left.Except(right);
		}

		public static IEnumerable<BsonValue> DISTINCT(IEnumerable<BsonValue> items)
		{
			return items.Distinct();
		}

		[Volatile]
		public static BsonValue RANDOM()
		{
			return _random.Next();
		}

		[Volatile]
		public static BsonValue RANDOM(BsonValue min, BsonValue max)
		{
			if (min.IsNumber && max.IsNumber)
			{
				return _random.Next(min.AsInt32, max.AsInt32);
			}
			return BsonValue.Null;
		}

		public static BsonValue LOWER(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.ToLowerInvariant();
			}
			return BsonValue.Null;
		}

		public static BsonValue UPPER(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.ToUpperInvariant();
			}
			return BsonValue.Null;
		}

		public static BsonValue LTRIM(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.TrimStart();
			}
			return BsonValue.Null;
		}

		public static BsonValue RTRIM(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.TrimEnd();
			}
			return BsonValue.Null;
		}

		public static BsonValue TRIM(BsonValue value)
		{
			if (value.IsString)
			{
				return value.AsString.Trim();
			}
			return BsonValue.Null;
		}

		public static BsonValue INDEXOF(BsonValue value, BsonValue search)
		{
			if (value.IsString && search.IsString)
			{
				return value.AsString.IndexOf(search.AsString);
			}
			return BsonValue.Null;
		}

		public static BsonValue INDEXOF(BsonValue value, BsonValue search, BsonValue startIndex)
		{
			if (value.IsString && search.IsString && startIndex.IsNumber)
			{
				return value.AsString.IndexOf(search.AsString, startIndex.AsInt32);
			}
			return BsonValue.Null;
		}

		public static BsonValue SUBSTRING(BsonValue value, BsonValue startIndex)
		{
			if (value.IsString && startIndex.IsNumber)
			{
				return value.AsString.Substring(startIndex.AsInt32);
			}
			return BsonValue.Null;
		}

		public static BsonValue SUBSTRING(BsonValue value, BsonValue startIndex, BsonValue length)
		{
			if (value.IsString && startIndex.IsNumber && length.IsNumber)
			{
				return value.AsString.Substring(startIndex.AsInt32, length.AsInt32);
			}
			return BsonValue.Null;
		}

		public static BsonValue REPLACE(BsonValue value, BsonValue oldValue, BsonValue newValue)
		{
			if (value.IsString && oldValue.IsString && newValue.IsString)
			{
				return value.AsString.Replace(oldValue.AsString, newValue.AsString);
			}
			return BsonValue.Null;
		}

		public static BsonValue LPAD(BsonValue value, BsonValue totalWidth, BsonValue paddingChar)
		{
			if (value.IsString && totalWidth.IsNumber && paddingChar.IsString)
			{
				string c = paddingChar.AsString;
				if (string.IsNullOrEmpty(c))
				{
					throw new ArgumentOutOfRangeException("paddingChar");
				}
				return value.AsString.PadLeft(totalWidth.AsInt32, c[0]);
			}
			return BsonValue.Null;
		}

		public static BsonValue RPAD(BsonValue value, BsonValue totalWidth, BsonValue paddingChar)
		{
			if (value.IsString && totalWidth.IsNumber && paddingChar.IsString)
			{
				string c = paddingChar.AsString;
				if (string.IsNullOrEmpty(c))
				{
					throw new ArgumentOutOfRangeException("paddingChar");
				}
				return value.AsString.PadRight(totalWidth.AsInt32, c[0]);
			}
			return BsonValue.Null;
		}

		public static IEnumerable<BsonValue> SPLIT(BsonValue value, BsonValue separator)
		{
			if (value.IsString && separator.IsString)
			{
				string[] values = value.AsString.Split(new string[1] { separator.AsString }, StringSplitOptions.RemoveEmptyEntries);
				string[] array = values;
				foreach (string str in array)
				{
					yield return str;
				}
			}
		}

		public static IEnumerable<BsonValue> SPLIT(BsonValue value, BsonValue pattern, BsonValue useRegex)
		{
			if (useRegex.IsBoolean && useRegex.AsBoolean)
			{
				if (value.IsString && pattern.IsString)
				{
					string[] values = Regex.Split(value.AsString, pattern.AsString, RegexOptions.Compiled);
					string[] array = values;
					foreach (string str in array)
					{
						yield return str;
					}
				}
				yield break;
			}
			foreach (BsonValue item in SPLIT(value, pattern))
			{
				yield return item;
			}
		}

		public static BsonValue FORMAT(BsonValue value, BsonValue format)
		{
			if (format.IsString)
			{
				return string.Format("{0:" + format.AsString + "}", value.RawValue);
			}
			return BsonValue.Null;
		}

		public static BsonValue JOIN(IEnumerable<BsonValue> values)
		{
			return JOIN(values, "");
		}

		public static BsonValue JOIN(IEnumerable<BsonValue> values, BsonValue separator)
		{
			if (separator.IsString)
			{
				return string.Join(separator.AsString, values.Select((BsonValue x) => STRING(x).AsString).ToArray());
			}
			return BsonValue.Null;
		}

		public static BsonValue IS_MATCH(BsonValue value, BsonValue pattern)
		{
			if (!value.IsString || !pattern.IsString)
			{
				return false;
			}
			return Regex.IsMatch(value.AsString, pattern.AsString);
		}

		public static BsonValue MATCH(BsonValue value, BsonValue pattern, BsonValue group)
		{
			if (!value.IsString || !pattern.IsString)
			{
				return null;
			}
			Match match = Regex.Match(value.AsString, pattern.AsString);
			if (!match.Success)
			{
				return BsonValue.Null;
			}
			if (group.IsNumber)
			{
				return match.Groups[group.AsInt32].Value;
			}
			if (group.IsString)
			{
				return match.Groups[group.AsString].Value;
			}
			return BsonValue.Null;
		}
	}
}
