using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Primitives
{
	internal static class ThrowHelper
	{
		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(GetArgumentName(argument));
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(GetArgumentName(argument));
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException(GetResourceText(resource));
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new InvalidOperationException(GetResourceText(resource));
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		internal static void ThrowInvalidOperationException(ExceptionResource resource, params object[] args)
		{
			string message = string.Format(GetResourceText(resource), args);
			throw new InvalidOperationException(message);
		}

		internal static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(GetArgumentName(argument));
		}

		internal static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(GetArgumentName(argument));
		}

		internal static ArgumentException GetArgumentException(ExceptionResource resource)
		{
			return new ArgumentException(GetResourceText(resource));
		}

		private static string GetResourceText(ExceptionResource resource)
		{
			return resource switch
			{
				ExceptionResource.Argument_InvalidOffsetLength => _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ESR.Argument_InvalidOffsetLength, 
				ExceptionResource.Argument_InvalidOffsetLengthStringSegment => _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ESR.Argument_InvalidOffsetLengthStringSegment, 
				ExceptionResource.Capacity_CannotChangeAfterWriteStarted => _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ESR.Capacity_CannotChangeAfterWriteStarted, 
				ExceptionResource.Capacity_NotEnough => _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ESR.Capacity_NotEnough, 
				ExceptionResource.Capacity_NotUsedEntirely => _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ESR.Capacity_NotUsedEntirely, 
				_ => "", 
			};
		}

		private static string GetArgumentName(ExceptionArgument argument)
		{
			return argument.ToString();
		}
	}
}
