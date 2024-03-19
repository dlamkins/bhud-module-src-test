using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LiteDB
{
	public class JsonWriter
	{
		private static readonly IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

		private readonly TextWriter _writer;

		private int _indent;

		private string _spacer = "";

		public int Indent { get; set; } = 4;


		public bool Pretty { get; set; }

		public JsonWriter(TextWriter writer)
		{
			_writer = writer;
		}

		public void Serialize(BsonValue value)
		{
			_indent = 0;
			_spacer = (Pretty ? " " : "");
			WriteValue(value ?? BsonValue.Null);
		}

		private void WriteValue(BsonValue value)
		{
			switch (value.Type)
			{
			case BsonType.Null:
				_writer.Write("null");
				break;
			case BsonType.Array:
				WriteArray(value.AsArray);
				break;
			case BsonType.Document:
				WriteObject(value.AsDocument);
				break;
			case BsonType.Boolean:
				_writer.Write(value.AsBoolean.ToString().ToLower());
				break;
			case BsonType.String:
				WriteString(value.AsString);
				break;
			case BsonType.Int32:
				_writer.Write(value.AsInt32.ToString(_numberFormat));
				break;
			case BsonType.Double:
			{
				double d = value.AsDouble;
				if (double.IsNaN(d) || double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d))
				{
					_writer.Write("null");
				}
				else
				{
					_writer.Write(value.AsDouble.ToString("0.0########", _numberFormat));
				}
				break;
			}
			case BsonType.Binary:
			{
				byte[] bytes = value.AsBinary;
				WriteExtendDataType("$binary", Convert.ToBase64String(bytes, 0, bytes.Length));
				break;
			}
			case BsonType.ObjectId:
				WriteExtendDataType("$oid", value.AsObjectId.ToString());
				break;
			case BsonType.Guid:
				WriteExtendDataType("$guid", value.AsGuid.ToString());
				break;
			case BsonType.DateTime:
				WriteExtendDataType("$date", value.AsDateTime.ToUniversalTime().ToString("o"));
				break;
			case BsonType.Int64:
				WriteExtendDataType("$numberLong", value.AsInt64.ToString(_numberFormat));
				break;
			case BsonType.Decimal:
				WriteExtendDataType("$numberDecimal", value.AsDecimal.ToString(_numberFormat));
				break;
			case BsonType.MinValue:
				WriteExtendDataType("$minValue", "1");
				break;
			case BsonType.MaxValue:
				WriteExtendDataType("$maxValue", "1");
				break;
			}
		}

		private void WriteObject(BsonDocument obj)
		{
			int length = obj.Keys.Count();
			bool hasData = length > 0;
			WriteStartBlock("{", hasData);
			int index = 0;
			foreach (KeyValuePair<string, BsonValue> el in obj.GetElements())
			{
				WriteKeyValue(el.Key, el.Value, index++ < length - 1);
			}
			WriteEndBlock("}", hasData);
		}

		private void WriteArray(BsonArray arr)
		{
			bool hasData = arr.Count > 0;
			WriteStartBlock("[", hasData);
			for (int i = 0; i < arr.Count; i++)
			{
				BsonValue item = arr[i];
				if (Pretty && item != null && (!item.IsDocument || !item.AsDocument.Keys.Any()) && (!item.IsArray || item.AsArray.Count <= 0))
				{
					WriteIndent();
				}
				WriteValue(item ?? BsonValue.Null);
				if (i < arr.Count - 1)
				{
					_writer.Write(',');
				}
				WriteNewLine();
			}
			WriteEndBlock("]", hasData);
		}

		private void WriteString(string s)
		{
			_writer.Write('"');
			int i = s.Length;
			for (int index = 0; index < i; index++)
			{
				char c = s[index];
				switch (c)
				{
				case '"':
					_writer.Write("\\\"");
					continue;
				case '\\':
					_writer.Write("\\\\");
					continue;
				case '\b':
					_writer.Write("\\b");
					continue;
				case '\f':
					_writer.Write("\\f");
					continue;
				case '\n':
					_writer.Write("\\n");
					continue;
				case '\r':
					_writer.Write("\\r");
					continue;
				case '\t':
					_writer.Write("\\t");
					continue;
				}
				switch (CharUnicodeInfo.GetUnicodeCategory(c))
				{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
				case UnicodeCategory.OtherLetter:
				case UnicodeCategory.DecimalDigitNumber:
				case UnicodeCategory.LetterNumber:
				case UnicodeCategory.OtherNumber:
				case UnicodeCategory.SpaceSeparator:
				case UnicodeCategory.ConnectorPunctuation:
				case UnicodeCategory.DashPunctuation:
				case UnicodeCategory.OpenPunctuation:
				case UnicodeCategory.ClosePunctuation:
				case UnicodeCategory.InitialQuotePunctuation:
				case UnicodeCategory.FinalQuotePunctuation:
				case UnicodeCategory.OtherPunctuation:
				case UnicodeCategory.MathSymbol:
				case UnicodeCategory.CurrencySymbol:
				case UnicodeCategory.ModifierSymbol:
				case UnicodeCategory.OtherSymbol:
					_writer.Write(c);
					continue;
				}
				_writer.Write("\\u");
				TextWriter writer = _writer;
				int num = c;
				writer.Write(num.ToString("x04"));
			}
			_writer.Write('"');
		}

		private void WriteExtendDataType(string type, string value)
		{
			_writer.Write("{\"");
			_writer.Write(type);
			_writer.Write("\":");
			_writer.Write(_spacer);
			_writer.Write("\"");
			_writer.Write(value);
			_writer.Write("\"}");
		}

		private void WriteKeyValue(string key, BsonValue value, bool comma)
		{
			WriteIndent();
			_writer.Write('"');
			_writer.Write(key);
			_writer.Write("\":");
			if (Pretty)
			{
				_writer.Write(' ');
				if (value != null && ((value.IsDocument && value.AsDocument.Keys.Any()) || (value.IsArray && value.AsArray.Count > 0)))
				{
					WriteNewLine();
				}
			}
			WriteValue(value ?? BsonValue.Null);
			if (comma)
			{
				_writer.Write(',');
			}
			WriteNewLine();
		}

		private void WriteStartBlock(string str, bool hasData)
		{
			if (hasData)
			{
				WriteIndent();
				_writer.Write(str);
				WriteNewLine();
				_indent++;
			}
			else
			{
				_writer.Write(str);
			}
		}

		private void WriteEndBlock(string str, bool hasData)
		{
			if (hasData)
			{
				_indent--;
				WriteIndent();
				_writer.Write(str);
			}
			else
			{
				_writer.Write(str);
			}
		}

		private void WriteNewLine()
		{
			if (Pretty)
			{
				_writer.WriteLine();
			}
		}

		private void WriteIndent()
		{
			if (Pretty)
			{
				_writer.Write("".PadRight(_indent * Indent, ' '));
			}
		}
	}
}
