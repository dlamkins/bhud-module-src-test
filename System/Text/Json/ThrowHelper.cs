using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json
{
	internal static class ThrowHelper
	{
		public const string ExceptionSourceValueToRethrowAsJsonException = "System.Text.Json.Rethrowable";

		[MethodImpl(MethodImplOptions.NoInlining)]
		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowOutOfMemoryException_BufferMaximumSizeExceeded(uint capacity)
		{
			throw new OutOfMemoryException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.BufferMaximumSizeExceeded, capacity));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentNullException(string parameterName)
		{
			throw new ArgumentNullException(parameterName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentOutOfRangeException_MaxDepthMustBePositive(string parameterName)
		{
			throw GetArgumentOutOfRangeException(parameterName, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MaxDepthMustBePositive);
		}

		private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(string parameterName, string message)
		{
			return new ArgumentOutOfRangeException(parameterName, message);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentOutOfRangeException_CommentEnumMustBeInRange(string parameterName)
		{
			throw GetArgumentOutOfRangeException(parameterName, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CommentHandlingMustBeValid);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentOutOfRangeException_ArrayIndexNegative(string paramName)
		{
			throw new ArgumentOutOfRangeException(paramName, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ArrayIndexNegative);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentOutOfRangeException_JsonConverterFactory_TypeNotSupported(Type typeToConvert)
		{
			throw new ArgumentOutOfRangeException("typeToConvert", _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerConverterFactoryInvalidArgument, typeToConvert.FullName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_ArrayTooSmall(string paramName)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ArrayTooSmall, paramName);
		}

		private static ArgumentException GetArgumentException(string message)
		{
			return new ArgumentException(message);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException(string message)
		{
			throw GetArgumentException(message);
		}

		public static InvalidOperationException GetInvalidOperationException_CallFlushFirst(int _buffered)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CallFlushToAvoidDataLoss, _buffered));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_DestinationTooShort()
		{
			throw GetArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DestinationTooShort);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_PropertyNameTooLarge(int tokenLength)
		{
			throw GetArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, tokenLength));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_ValueTooLarge(long tokenLength)
		{
			throw GetArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ValueTooLarge, tokenLength));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_ValueNotSupported()
		{
			throw GetArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SpecialNumberValuesNotSupported);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NeedLargerSpan()
		{
			throw GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FailedToGetLargerSpan);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowPropertyNameTooLargeArgumentException(int length)
		{
			throw GetArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, length));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
		{
			if (propertyName.Length > 166666666)
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ValueTooLarge, value.Length));
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
		{
			if (propertyName.Length > 166666666)
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ValueTooLarge, value.Length));
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
		{
			if (propertyName.Length > 166666666)
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ValueTooLarge, value.Length));
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
		{
			if (propertyName.Length > 166666666)
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ValueTooLarge, value.Length));
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationOrArgumentException(ReadOnlySpan<byte> propertyName, int currentDepth, int maxDepth)
		{
			currentDepth &= 0x7FFFFFFF;
			if (currentDepth >= maxDepth)
			{
				ThrowInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DepthTooLarge, currentDepth, maxDepth));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException(int currentDepth, int maxDepth)
		{
			currentDepth &= 0x7FFFFFFF;
			ThrowInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DepthTooLarge, currentDepth, maxDepth));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException(string message)
		{
			throw GetInvalidOperationException(message);
		}

		private static InvalidOperationException GetInvalidOperationException(string message)
		{
			return new InvalidOperationException(message)
			{
				Source = "System.Text.Json.Rethrowable"
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_DepthNonZeroOrEmptyJson(int currentDepth)
		{
			throw GetInvalidOperationException(currentDepth);
		}

		private static InvalidOperationException GetInvalidOperationException(int currentDepth)
		{
			currentDepth &= 0x7FFFFFFF;
			if (currentDepth != 0)
			{
				return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ZeroDepthAtEnd, currentDepth));
			}
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.EmptyJsonIsInvalid);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationOrArgumentException(ReadOnlySpan<char> propertyName, int currentDepth, int maxDepth)
		{
			currentDepth &= 0x7FFFFFFF;
			if (currentDepth >= maxDepth)
			{
				ThrowInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DepthTooLarge, currentDepth, maxDepth));
			}
			else
			{
				ThrowArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.PropertyNameTooLarge, propertyName.Length));
			}
		}

		public static InvalidOperationException GetInvalidOperationException_ExpectedArray(JsonTokenType tokenType)
		{
			return GetInvalidOperationException("array", tokenType);
		}

		public static InvalidOperationException GetInvalidOperationException_ExpectedObject(JsonTokenType tokenType)
		{
			return GetInvalidOperationException("object", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedNumber(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("number", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedBoolean(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("boolean", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedString(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("string", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedPropertyName(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("propertyName", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedStringComparison(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException(tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedComment(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("comment", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_CannotSkipOnPartial()
		{
			throw GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotSkip);
		}

		private static InvalidOperationException GetInvalidOperationException(string message, JsonTokenType tokenType)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidCast, tokenType, message));
		}

		private static InvalidOperationException GetInvalidOperationException(JsonTokenType tokenType)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidComparison, tokenType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		internal static void ThrowJsonElementWrongTypeException(JsonTokenType expectedType, JsonTokenType actualType)
		{
			throw GetJsonElementWrongTypeException(expectedType.ToValueKind(), actualType.ToValueKind());
		}

		internal static InvalidOperationException GetJsonElementWrongTypeException(JsonValueKind expectedType, JsonValueKind actualType)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonElementHasWrongType, expectedType, actualType));
		}

		internal static InvalidOperationException GetJsonElementWrongTypeException(string expectedTypeName, JsonValueKind actualType)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonElementHasWrongType, expectedTypeName, actualType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonReaderException(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte = 0, ReadOnlySpan<byte> bytes = default(ReadOnlySpan<byte>))
		{
			throw GetJsonReaderException(ref json, resource, nextByte, bytes);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static JsonException GetJsonReaderException(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte, ReadOnlySpan<byte> bytes)
		{
			string resourceString = GetResourceString(ref json, resource, nextByte, JsonHelpers.Utf8GetString(bytes));
			long lineNumber = json.CurrentState._lineNumber;
			long bytePositionInLine = json.CurrentState._bytePositionInLine;
			resourceString += $" LineNumber: {lineNumber} | BytePositionInLine: {bytePositionInLine}.";
			return new JsonReaderException(resourceString, lineNumber, bytePositionInLine);
		}

		private static bool IsPrintable(byte value)
		{
			if (value >= 32)
			{
				return value < 127;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetPrintableString(byte value)
		{
			if (!IsPrintable(value))
			{
				return $"0x{value:X2}";
			}
			char c = (char)value;
			return c.ToString();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string GetResourceString(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte, string characters)
		{
			string printableString = GetPrintableString(nextByte);
			string result = "";
			switch (resource)
			{
			case ExceptionResource.ArrayDepthTooLarge:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ArrayDepthTooLarge, json.CurrentState.Options.MaxDepth);
				break;
			case ExceptionResource.MismatchedObjectArray:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MismatchedObjectArray, printableString);
				break;
			case ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.TrailingCommaNotAllowedBeforeArrayEnd;
				break;
			case ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.TrailingCommaNotAllowedBeforeObjectEnd;
				break;
			case ExceptionResource.EndOfStringNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.EndOfStringNotFound;
				break;
			case ExceptionResource.RequiredDigitNotFoundAfterSign:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.RequiredDigitNotFoundAfterSign, printableString);
				break;
			case ExceptionResource.RequiredDigitNotFoundAfterDecimal:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.RequiredDigitNotFoundAfterDecimal, printableString);
				break;
			case ExceptionResource.RequiredDigitNotFoundEndOfData:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.RequiredDigitNotFoundEndOfData;
				break;
			case ExceptionResource.ExpectedEndAfterSingleJson:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedEndAfterSingleJson, printableString);
				break;
			case ExceptionResource.ExpectedEndOfDigitNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedEndOfDigitNotFound, printableString);
				break;
			case ExceptionResource.ExpectedNextDigitEValueNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedNextDigitEValueNotFound, printableString);
				break;
			case ExceptionResource.ExpectedSeparatorAfterPropertyNameNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedSeparatorAfterPropertyNameNotFound, printableString);
				break;
			case ExceptionResource.ExpectedStartOfPropertyNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedStartOfPropertyNotFound, printableString);
				break;
			case ExceptionResource.ExpectedStartOfPropertyOrValueNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedStartOfPropertyOrValueNotFound;
				break;
			case ExceptionResource.ExpectedStartOfPropertyOrValueAfterComment:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedStartOfPropertyOrValueAfterComment, printableString);
				break;
			case ExceptionResource.ExpectedStartOfValueNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedStartOfValueNotFound, printableString);
				break;
			case ExceptionResource.ExpectedValueAfterPropertyNameNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedValueAfterPropertyNameNotFound;
				break;
			case ExceptionResource.FoundInvalidCharacter:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FoundInvalidCharacter, printableString);
				break;
			case ExceptionResource.InvalidEndOfJsonNonPrimitive:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidEndOfJsonNonPrimitive, json.TokenType);
				break;
			case ExceptionResource.ObjectDepthTooLarge:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectDepthTooLarge, json.CurrentState.Options.MaxDepth);
				break;
			case ExceptionResource.ExpectedFalse:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedFalse, characters);
				break;
			case ExceptionResource.ExpectedNull:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedNull, characters);
				break;
			case ExceptionResource.ExpectedTrue:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedTrue, characters);
				break;
			case ExceptionResource.InvalidCharacterWithinString:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidCharacterWithinString, printableString);
				break;
			case ExceptionResource.InvalidCharacterAfterEscapeWithinString:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidCharacterAfterEscapeWithinString, printableString);
				break;
			case ExceptionResource.InvalidHexCharacterWithinString:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidHexCharacterWithinString, printableString);
				break;
			case ExceptionResource.EndOfCommentNotFound:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.EndOfCommentNotFound;
				break;
			case ExceptionResource.ZeroDepthAtEnd:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ZeroDepthAtEnd);
				break;
			case ExceptionResource.ExpectedJsonTokens:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedJsonTokens;
				break;
			case ExceptionResource.NotEnoughData:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NotEnoughData;
				break;
			case ExceptionResource.ExpectedOneCompleteToken:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExpectedOneCompleteToken;
				break;
			case ExceptionResource.InvalidCharacterAtStartOfComment:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidCharacterAtStartOfComment, printableString);
				break;
			case ExceptionResource.UnexpectedEndOfDataWhileReadingComment:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.UnexpectedEndOfDataWhileReadingComment);
				break;
			case ExceptionResource.UnexpectedEndOfLineSeparator:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.UnexpectedEndOfLineSeparator);
				break;
			case ExceptionResource.InvalidLeadingZeroInNumber:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidLeadingZeroInNumber, printableString);
				break;
			}
			return result;
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
		{
			throw GetInvalidOperationException(resource, currentDepth, maxDepth, token, tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_InvalidCommentValue()
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWriteCommentWithEmbeddedDelimiter);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_InvalidUTF8(ReadOnlySpan<byte> value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = Math.Min(value.Length, 10);
			for (int i = 0; i < num; i++)
			{
				byte b = value[i];
				if (IsPrintable(b))
				{
					stringBuilder.Append((char)b);
				}
				else
				{
					stringBuilder.Append($"0x{b:X2}");
				}
			}
			if (num < value.Length)
			{
				stringBuilder.Append("...");
			}
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotEncodeInvalidUTF8, stringBuilder));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_InvalidUTF16(int charAsInt)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotEncodeInvalidUTF16, $"0x{charAsInt:X2}"));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ReadInvalidUTF16(int charAsInt)
		{
			throw GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotReadInvalidUTF16, $"0x{charAsInt:X2}"));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ReadIncompleteUTF16()
		{
			throw GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotReadIncompleteUTF16);
		}

		public static InvalidOperationException GetInvalidOperationException_ReadInvalidUTF8(DecoderFallbackException innerException = null)
		{
			return GetInvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotTranscodeInvalidUtf8, innerException);
		}

		public static ArgumentException GetArgumentException_ReadInvalidUTF16(EncoderFallbackException innerException)
		{
			return new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotTranscodeInvalidUtf16, innerException);
		}

		public static InvalidOperationException GetInvalidOperationException(string message, Exception innerException)
		{
			InvalidOperationException ex = new InvalidOperationException(message, innerException);
			ex.Source = "System.Text.Json.Rethrowable";
			return ex;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException GetInvalidOperationException(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
		{
			string resourceString = GetResourceString(resource, currentDepth, maxDepth, token, tokenType);
			InvalidOperationException invalidOperationException = GetInvalidOperationException(resourceString);
			invalidOperationException.Source = "System.Text.Json.Rethrowable";
			return invalidOperationException;
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowOutOfMemoryException(uint capacity)
		{
			throw new OutOfMemoryException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.BufferMaximumSizeExceeded, capacity));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string GetResourceString(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
		{
			string result = "";
			switch (resource)
			{
			case ExceptionResource.MismatchedObjectArray:
				result = ((tokenType == JsonTokenType.PropertyName) ? _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWriteEndAfterProperty, (char)token) : _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MismatchedObjectArray, (char)token));
				break;
			case ExceptionResource.DepthTooLarge:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DepthTooLarge, currentDepth & 0x7FFFFFFF, maxDepth);
				break;
			case ExceptionResource.CannotStartObjectArrayWithoutProperty:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotStartObjectArrayWithoutProperty, tokenType);
				break;
			case ExceptionResource.CannotStartObjectArrayAfterPrimitiveOrClose:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotStartObjectArrayAfterPrimitiveOrClose, tokenType);
				break;
			case ExceptionResource.CannotWriteValueWithinObject:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWriteValueWithinObject, tokenType);
				break;
			case ExceptionResource.CannotWritePropertyWithinArray:
				result = ((tokenType == JsonTokenType.PropertyName) ? _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWritePropertyAfterProperty) : _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWritePropertyWithinArray, tokenType));
				break;
			case ExceptionResource.CannotWriteValueAfterPrimitiveOrClose:
				result = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotWriteValueAfterPrimitiveOrClose, tokenType);
				break;
			}
			return result;
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowFormatException()
		{
			throw new FormatException
			{
				Source = "System.Text.Json.Rethrowable"
			};
		}

		public static void ThrowFormatException(NumericType numericType)
		{
			string message = "";
			switch (numericType)
			{
			case NumericType.Byte:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatByte;
				break;
			case NumericType.SByte:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatSByte;
				break;
			case NumericType.Int16:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatInt16;
				break;
			case NumericType.Int32:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatInt32;
				break;
			case NumericType.Int64:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatInt64;
				break;
			case NumericType.Int128:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatInt128;
				break;
			case NumericType.UInt16:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatUInt16;
				break;
			case NumericType.UInt32:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatUInt32;
				break;
			case NumericType.UInt64:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatUInt64;
				break;
			case NumericType.UInt128:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatUInt128;
				break;
			case NumericType.Half:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatHalf;
				break;
			case NumericType.Single:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatSingle;
				break;
			case NumericType.Double:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatDouble;
				break;
			case NumericType.Decimal:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.FormatDecimal;
				break;
			}
			throw new FormatException(message)
			{
				Source = "System.Text.Json.Rethrowable"
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowFormatException(DataType dataType)
		{
			string message = "";
			switch (dataType)
			{
			case DataType.Boolean:
			case DataType.DateOnly:
			case DataType.DateTime:
			case DataType.DateTimeOffset:
			case DataType.TimeOnly:
			case DataType.TimeSpan:
			case DataType.Guid:
			case DataType.Version:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.UnsupportedFormat, dataType);
				break;
			case DataType.Base64String:
				message = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotDecodeInvalidBase64;
				break;
			}
			throw new FormatException(message)
			{
				Source = "System.Text.Json.Rethrowable"
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExpectedChar(JsonTokenType tokenType)
		{
			throw GetInvalidOperationException("char", tokenType);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowObjectDisposedException_Utf8JsonWriter()
		{
			throw new ObjectDisposedException("Utf8JsonWriter");
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowObjectDisposedException_JsonDocument()
		{
			throw new ObjectDisposedException("JsonDocument");
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_NodeValueNotAllowed(string paramName)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeValueNotAllowed, paramName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_DuplicateKey(string paramName, string propertyName)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeDuplicateKey, propertyName), paramName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeAlreadyHasParent()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeAlreadyHasParent);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeCycleDetected()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeCycleDetected);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeElementCannotBeObjectOrArray()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeElementCannotBeObjectOrArray);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_CollectionIsReadOnly()
		{
			throw GetNotSupportedException_CollectionIsReadOnly();
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeWrongType(string typeName)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeWrongType, typeName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeParentWrongType(string typeName)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeParentWrongType, typeName));
		}

		public static NotSupportedException GetNotSupportedException_CollectionIsReadOnly()
		{
			return new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CollectionIsReadOnly);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_DeserializeWrongType(Type type, object value)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeWrongType, type, value.GetType()));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_SerializerDoesNotSupportComments(string paramName)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonSerializerDoesNotSupportComments, paramName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_SerializationNotSupported(Type propertyType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationNotSupportedType, propertyType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_TypeRequiresAsyncSerialization(Type propertyType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.TypeRequiresAsyncSerialization, propertyType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_DictionaryKeyTypeNotSupported(Type keyType, JsonConverter converter)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DictionaryKeyTypeNotSupported, keyType, converter.GetType()));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_DeserializeUnableToConvertValue(Type propertyType)
		{
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToConvertValue, propertyType))
			{
				AppendPathInformation = true
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidCastException_DeserializeUnableToAssignValue(Type typeOfValue, Type declaredType)
		{
			throw new InvalidCastException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToAssignValue, typeOfValue, declaredType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_DeserializeUnableToAssignNull(Type declaredType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToAssignNull, declaredType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPopulateNotSupportedByConverter(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPopulateNotSupportedByConverter, propertyInfo.Name, propertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPropertyMustHaveAGetter(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyMustHaveAGetter, propertyInfo.Name, propertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPropertyValueTypeMustHaveASetter(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyValueTypeMustHaveASetter, propertyInfo.Name, propertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPropertyCannotAllowPolymorphicDeserialization(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyCannotAllowPolymorphicDeserialization, propertyInfo.Name, propertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPropertyCannotAllowReadOnlyMember(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyCannotAllowReadOnlyMember, propertyInfo.Name, propertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ObjectCreationHandlingPropertyCannotAllowReferenceHandling()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyCannotAllowReferenceHandling);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_ObjectCreationHandlingPropertyDoesNotSupportParameterizedConstructors()
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectCreationHandlingPropertyDoesNotSupportParameterizedConstructors);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_SerializationConverterRead(JsonConverter converter)
		{
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationConverterRead, converter))
			{
				AppendPathInformation = true
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_SerializationConverterWrite(JsonConverter converter)
		{
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationConverterWrite, converter))
			{
				AppendPathInformation = true
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_SerializerCycleDetected(int maxDepth)
		{
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerCycleDetected, maxDepth))
			{
				AppendPathInformation = true
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException(string message = null)
		{
			throw new JsonException(message)
			{
				AppendPathInformation = true
			};
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_CannotSerializeInvalidType(string paramName, Type typeToConvert, Type declaringType, string propertyName)
		{
			if (declaringType == null)
			{
				throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotSerializeInvalidType, typeToConvert), paramName);
			}
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotSerializeInvalidMember, typeToConvert, propertyName, declaringType), paramName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_CannotSerializeInvalidType(Type typeToConvert, Type declaringType, MemberInfo memberInfo)
		{
			if (declaringType == null)
			{
				throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotSerializeInvalidType, typeToConvert));
			}
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotSerializeInvalidMember, typeToConvert, memberInfo.Name, declaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationConverterNotCompatible(Type converterType, Type type)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationConverterNotCompatible, converterType, type));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ResolverTypeNotCompatible(Type requestedType, Type actualType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ResolverTypeNotCompatible, actualType, requestedType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ResolverTypeInfoOptionsNotCompatible()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ResolverTypeInfoOptionsNotCompatible);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonSerializerOptionsNoTypeInfoResolverSpecified()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonSerializerOptionsNoTypeInfoResolverSpecified);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonSerializerIsReflectionDisabled()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonSerializerIsReflectionDisabled);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationConverterOnAttributeInvalid(Type classType, MemberInfo memberInfo)
		{
			string text = classType.ToString();
			if (memberInfo != null)
			{
				text = text + "." + memberInfo.Name;
			}
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationConverterOnAttributeInvalid, text));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationConverterOnAttributeNotCompatible(Type classTypeAttributeIsOn, MemberInfo memberInfo, Type typeToConvert)
		{
			string text = classTypeAttributeIsOn.ToString();
			if (memberInfo != null)
			{
				text = text + "." + memberInfo.Name;
			}
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationConverterOnAttributeNotCompatible, text, typeToConvert));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializerOptionsReadOnly(JsonSerializerContext context)
		{
			string message = ((context == null) ? _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerOptionsReadOnly : _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerContextOptionsReadOnly);
			throw new InvalidOperationException(message);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_DefaultTypeInfoResolverImmutable()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DefaultTypeInfoResolverImmutable);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_TypeInfoResolverChainImmutable()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.TypeInfoResolverChainImmutable);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_TypeInfoImmutable()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.TypeInfoImmutable);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidChainedResolver()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerOptions_InvalidChainedResolver);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializerPropertyNameConflict(Type type, string propertyName)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerPropertyNameConflict, type, propertyName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializerPropertyNameNull(JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerPropertyNameNull, jsonPropertyInfo.DeclaringType, jsonPropertyInfo.MemberName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonPropertyRequiredAndNotDeserializable(JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonPropertyRequiredAndNotDeserializable, jsonPropertyInfo.Name, jsonPropertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonPropertyRequiredAndExtensionData(JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonPropertyRequiredAndExtensionData, jsonPropertyInfo.Name, jsonPropertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_JsonRequiredPropertyMissing(JsonTypeInfo parent, BitArray requiredPropertiesSet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (KeyValuePair<string, JsonPropertyInfo> item in parent.PropertyCache!.List)
			{
				JsonPropertyInfo value = item.Value;
				if (value.IsRequired && !requiredPropertiesSet[value.RequiredPropertyIndex])
				{
					if (!flag)
					{
						stringBuilder.Append(CultureInfo.CurrentUICulture.TextInfo.ListSeparator);
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(value.Name);
					flag = false;
					if (stringBuilder.Length >= 50)
					{
						break;
					}
				}
			}
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonRequiredPropertiesMissing, parent.Type, stringBuilder.ToString()));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NamingPolicyReturnNull(JsonNamingPolicy namingPolicy)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NamingPolicyReturnNull, namingPolicy));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializerConverterFactoryReturnsNull(Type converterType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerConverterFactoryReturnsNull, converterType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializerConverterFactoryReturnsJsonConverterFactorty(Type converterType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializerConverterFactoryReturnsJsonConverterFactory, converterType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_MultiplePropertiesBindToConstructorParameters(Type parentType, string parameterName, string firstMatchName, string secondMatchName)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MultipleMembersBindWithConstructorParameter, firstMatchName, secondMatchName, parentType, parameterName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ConstructorParameterIncompleteBinding(Type parentType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ConstructorParamIncompleteBinding, parentType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExtensionDataCannotBindToCtorParam(string propertyName, JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExtensionDataCannotBindToCtorParam, propertyName, jsonPropertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonIncludeOnInaccessibleProperty(string memberName, Type declaringType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonIncludeOnInaccessibleProperty, memberName, declaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_IgnoreConditionOnValueTypeInvalid(string clrPropertyName, Type propertyDeclaringType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.IgnoreConditionOnValueTypeInvalid, clrPropertyName, propertyDeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NumberHandlingOnPropertyInvalid(JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NumberHandlingOnPropertyInvalid, jsonPropertyInfo.MemberName, jsonPropertyInfo.DeclaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ConverterCanConvertMultipleTypes(Type runtimePropertyType, JsonConverter jsonConverter)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ConverterCanConvertMultipleTypes, jsonConverter.GetType(), jsonConverter.Type, runtimePropertyType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_ObjectWithParameterizedCtorRefMetadataNotSupported(ReadOnlySpan<byte> propertyName, ref Utf8JsonReader reader, [ScopedRef] ref ReadStack state)
		{
			JsonTypeInfo topJsonTypeInfoWithParameterizedConstructor = state.GetTopJsonTypeInfoWithParameterizedConstructor();
			state.Current.JsonPropertyName = propertyName.ToArray();
			NotSupportedException ex = new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ObjectWithParameterizedCtorRefMetadataNotSupported, topJsonTypeInfoWithParameterizedConstructor.Type));
			ThrowNotSupportedException(ref state, in reader, ex);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonTypeInfoOperationNotPossibleForKind(JsonTypeInfoKind kind)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidJsonTypeInfoOperationForKind, kind));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_CreateObjectConverterNotCompatible(Type type)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CreateObjectConverterNotCompatible, type));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ReThrowWithPath([ScopedRef] ref ReadStack state, JsonReaderException ex)
		{
			string text = state.JsonPath();
			string message = ex.Message;
			int num = message.LastIndexOf(" LineNumber: ", StringComparison.Ordinal);
			message = ((num < 0) ? (message + " Path: " + text + ".") : (message.Substring(0, num) + " Path: " + text + " |" + message.Substring(num)));
			throw new JsonException(message, text, ex.LineNumber, ex.BytePositionInLine, ex);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ReThrowWithPath([ScopedRef] ref ReadStack state, in Utf8JsonReader reader, Exception ex)
		{
			JsonException ex2 = new JsonException(null, ex);
			AddJsonExceptionInformation(ref state, in reader, ex2);
			throw ex2;
		}

		public static void AddJsonExceptionInformation([ScopedRef] ref ReadStack state, in Utf8JsonReader reader, JsonException ex)
		{
			long lineNumber = reader.CurrentState._lineNumber;
			ex.LineNumber = lineNumber;
			long bytePositionInLine = reader.CurrentState._bytePositionInLine;
			ex.BytePositionInLine = bytePositionInLine;
			string arg = (ex.Path = state.JsonPath());
			string text2 = ex._message;
			if (string.IsNullOrEmpty(text2))
			{
				Type p = state.Current.JsonPropertyInfo?.PropertyType ?? state.Current.JsonTypeInfo.Type;
				text2 = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToConvertValue, p);
				ex.AppendPathInformation = true;
			}
			if (ex.AppendPathInformation)
			{
				text2 += $" Path: {arg} | LineNumber: {lineNumber} | BytePositionInLine: {bytePositionInLine}.";
				ex.SetMessage(text2);
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ReThrowWithPath(ref WriteStack state, Exception ex)
		{
			JsonException ex2 = new JsonException(null, ex);
			AddJsonExceptionInformation(ref state, ex2);
			throw ex2;
		}

		public static void AddJsonExceptionInformation(ref WriteStack state, JsonException ex)
		{
			string text2 = (ex.Path = state.PropertyPath());
			string text3 = ex._message;
			if (string.IsNullOrEmpty(text3))
			{
				text3 = _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializeUnableToSerialize);
				ex.AppendPathInformation = true;
			}
			if (ex.AppendPathInformation)
			{
				text3 = text3 + " Path: " + text2 + ".";
				ex.SetMessage(text3);
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationDuplicateAttribute(Type attribute, MemberInfo memberInfo)
		{
			Type type = memberInfo as Type;
			string p = (((object)type != null) ? type.ToString() : $"{memberInfo.DeclaringType}.{memberInfo.Name}");
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationDuplicateAttribute, attribute, p));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationDuplicateTypeAttribute(Type classType, Type attribute)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationDuplicateTypeAttribute, classType, attribute));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationDuplicateTypeAttribute<TAttribute>(Type classType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationDuplicateTypeAttribute, classType, typeof(TAttribute)));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ExtensionDataConflictsWithUnmappedMemberHandling(Type classType, JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ExtensionDataConflictsWithUnmappedMemberHandling, classType, jsonPropertyInfo.MemberName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_SerializationDataExtensionPropertyInvalid(JsonPropertyInfo jsonPropertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationDataExtensionPropertyInvalid, jsonPropertyInfo.PropertyType, jsonPropertyInfo.MemberName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NodeJsonObjectCustomConverterNotAllowedOnExtensionProperty()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NodeJsonObjectCustomConverterNotAllowedOnExtensionProperty);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException([ScopedRef] ref ReadStack state, in Utf8JsonReader reader, NotSupportedException ex)
		{
			string text = ex.Message;
			Type type = state.Current.JsonPropertyInfo?.PropertyType ?? state.Current.JsonTypeInfo.Type;
			if (!text.Contains(type.ToString()))
			{
				if (text.Length > 0)
				{
					text += " ";
				}
				text += _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationNotSupportedParentType, type);
			}
			long lineNumber = reader.CurrentState._lineNumber;
			long bytePositionInLine = reader.CurrentState._bytePositionInLine;
			text += $" Path: {state.JsonPath()} | LineNumber: {lineNumber} | BytePositionInLine: {bytePositionInLine}.";
			throw new NotSupportedException(text, ex);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException(ref WriteStack state, NotSupportedException ex)
		{
			string text = ex.Message;
			Type type = state.Current.JsonPropertyInfo?.PropertyType ?? state.Current.JsonTypeInfo.Type;
			if (!text.Contains(type.ToString()))
			{
				if (text.Length > 0)
				{
					text += " ";
				}
				text += _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.SerializationNotSupportedParentType, type);
			}
			text = text + " Path: " + state.PropertyPath() + ".";
			throw new NotSupportedException(text, ex);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_DeserializeNoConstructor(Type type, ref Utf8JsonReader reader, [ScopedRef] ref ReadStack state)
		{
			string message = ((!type.IsInterface) ? _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeNoConstructor, "JsonConstructorAttribute", type) : _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializePolymorphicInterface, type));
			ThrowNotSupportedException(ref state, in reader, new NotSupportedException(message));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_CannotPopulateCollection(Type type, ref Utf8JsonReader reader, [ScopedRef] ref ReadStack state)
		{
			ThrowNotSupportedException(ref state, in reader, new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.CannotPopulateCollection, type)));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataValuesInvalidToken(JsonTokenType tokenType)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataInvalidTokenAfterValues, tokenType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataReferenceNotFound(string id)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataReferenceNotFound, id));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataValueWasNotString(JsonTokenType tokenType)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataValueWasNotString, tokenType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataValueWasNotString(JsonValueKind valueKind)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataValueWasNotString, valueKind));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties(ReadOnlySpan<byte> propertyName, [ScopedRef] ref ReadStack state)
		{
			state.Current.JsonPropertyName = propertyName.ToArray();
			ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties();
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataUnexpectedProperty(ReadOnlySpan<byte> propertyName, [ScopedRef] ref ReadStack state)
		{
			state.Current.JsonPropertyName = propertyName.ToArray();
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataUnexpectedProperty));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_UnmappedJsonProperty(Type type, string unmappedPropertyName)
		{
			throw new JsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.UnmappedJsonProperty, unmappedPropertyName, type));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties()
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataReferenceCannotContainOtherProperties);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataIdIsNotFirstProperty(ReadOnlySpan<byte> propertyName, [ScopedRef] ref ReadStack state)
		{
			state.Current.JsonPropertyName = propertyName.ToArray();
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataIdIsNotFirstProperty);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataStandaloneValuesProperty([ScopedRef] ref ReadStack state, ReadOnlySpan<byte> propertyName)
		{
			state.Current.JsonPropertyName = propertyName.ToArray();
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataStandaloneValuesProperty);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataInvalidPropertyWithLeadingDollarSign(ReadOnlySpan<byte> propertyName, [ScopedRef] ref ReadStack state, in Utf8JsonReader reader)
		{
			if (state.Current.IsProcessingDictionary())
			{
				state.Current.JsonPropertyNameAsString = reader.GetString();
			}
			else
			{
				state.Current.JsonPropertyName = propertyName.ToArray();
			}
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataInvalidPropertyWithLeadingDollarSign);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataDuplicateIdFound(string id)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataDuplicateIdFound, id));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataDuplicateTypeProperty()
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataDuplicateTypeProperty);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataInvalidReferenceToValueType(Type propertyType)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataInvalidReferenceToValueType, propertyType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataInvalidPropertyInArrayMetadata([ScopedRef] ref ReadStack state, Type propertyType, in Utf8JsonReader reader)
		{
			ref ReadStackFrame current = ref state.Current;
			byte[] jsonPropertyName;
			if (!reader.HasValueSequence)
			{
				jsonPropertyName = reader.ValueSpan.ToArray();
			}
			else
			{
				ReadOnlySequence<byte> sequence = reader.ValueSequence;
				jsonPropertyName = BuffersExtensions.ToArray(in sequence);
			}
			current.JsonPropertyName = jsonPropertyName;
			string @string = reader.GetString();
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataPreservedArrayFailed, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataInvalidPropertyInArrayMetadata, @string), _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToConvertValue, propertyType)));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataPreservedArrayValuesNotFound([ScopedRef] ref ReadStack state, Type propertyType)
		{
			state.Current.JsonPropertyName = null;
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataPreservedArrayFailed, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataStandaloneValuesProperty, _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.DeserializeUnableToConvertValue, propertyType)));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_MetadataCannotParsePreservedObjectIntoImmutable(Type propertyType)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataCannotParsePreservedObjectToImmutable, propertyType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_MetadataReferenceOfTypeCannotBeAssignedToType(string referenceId, Type currentType, Type typeToConvert)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MetadataReferenceOfTypeCannotBeAssignedToType, referenceId, currentType, typeToConvert));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_JsonPropertyInfoIsBoundToDifferentJsonTypeInfo(JsonPropertyInfo propertyInfo)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonPropertyInfoBoundToDifferentParent, propertyInfo.Name, propertyInfo.ParentTypeInfo!.Type.FullName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		internal static void ThrowUnexpectedMetadataException(ReadOnlySpan<byte> propertyName, ref Utf8JsonReader reader, [ScopedRef] ref ReadStack state)
		{
			if (JsonSerializer.GetMetadataPropertyName(propertyName, state.Current.BaseJsonTypeInfo.PolymorphicTypeResolver) != 0)
			{
				ThrowJsonException_MetadataUnexpectedProperty(propertyName, ref state);
			}
			else
			{
				ThrowJsonException_MetadataInvalidPropertyWithLeadingDollarSign(propertyName, ref state, in reader);
			}
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_NoMetadataForType(Type type, IJsonTypeInfoResolver resolver)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NoMetadataForType, type, resolver?.ToString() ?? "<null>"));
		}

		public static NotSupportedException GetNotSupportedException_AmbiguousMetadataForType(Type type, Type match1, Type match2)
		{
			return new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.AmbiguousMetadataForType, type, match1, match2));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_ConstructorContainsNullParameterNames(Type declaringType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.ConstructorContainsNullParameterNames, declaringType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoMetadataForType(Type type, IJsonTypeInfoResolver resolver)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NoMetadataForType, type, resolver?.ToString() ?? "<null>"));
		}

		public static Exception GetInvalidOperationException_NoMetadataForTypeProperties(IJsonTypeInfoResolver resolver, Type type)
		{
			return new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.NoMetadataForTypeProperties, resolver?.ToString() ?? "<null>", type));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoMetadataForTypeProperties(IJsonTypeInfoResolver resolver, Type type)
		{
			throw GetInvalidOperationException_NoMetadataForTypeProperties(resolver, type);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowMissingMemberException_MissingFSharpCoreMember(string missingFsharpCoreMember)
		{
			throw new MissingMemberException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.MissingFSharpCoreMember, missingFsharpCoreMember));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_BaseConverterDoesNotSupportMetadata(Type derivedType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_DerivedConverterDoesNotSupportMetadata, derivedType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_DerivedConverterDoesNotSupportMetadata(Type derivedType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_DerivedConverterDoesNotSupportMetadata, derivedType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_RuntimeTypeNotSupported(Type baseType, Type runtimeType)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_RuntimeTypeNotSupported, runtimeType, baseType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowNotSupportedException_RuntimeTypeDiamondAmbiguity(Type baseType, Type runtimeType, Type derivedType1, Type derivedType2)
		{
			throw new NotSupportedException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_RuntimeTypeDiamondAmbiguity, runtimeType, derivedType1, derivedType2, baseType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_TypeDoesNotSupportPolymorphism(Type baseType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_TypeDoesNotSupportPolymorphism, baseType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_DerivedTypeNotSupported(Type baseType, Type derivedType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_DerivedTypeIsNotSupported, derivedType, baseType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_DerivedTypeIsAlreadySpecified(Type baseType, Type derivedType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_DerivedTypeIsAlreadySpecified, baseType, derivedType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_TypeDicriminatorIdIsAlreadySpecified(Type baseType, object typeDiscriminator)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_TypeDicriminatorIdIsAlreadySpecified, baseType, typeDiscriminator));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidCustomTypeDiscriminatorPropertyName()
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_InvalidCustomTypeDiscriminatorPropertyName);
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_PolymorphicTypeConfigurationDoesNotSpecifyDerivedTypes(Type baseType)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_ConfigurationDoesNotSpecifyDerivedTypes, baseType));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidEnumTypeWithSpecialChar(Type enumType, string enumName)
		{
			throw new InvalidOperationException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.InvalidEnumTypeWithSpecialChar, enumType.Name, enumName));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowJsonException_UnrecognizedTypeDiscriminator(object typeDiscriminator)
		{
			ThrowJsonException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Format(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.Polymorphism_UnrecognizedTypeDiscriminator, typeDiscriminator));
		}

		[_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003EDoesNotReturn]
		public static void ThrowArgumentException_JsonPolymorphismOptionsAssociatedWithDifferentJsonTypeInfo(string parameterName)
		{
			throw new ArgumentException(_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonPolymorphismOptionsAssociatedWithDifferentJsonTypeInfo, parameterName);
		}
	}
}
