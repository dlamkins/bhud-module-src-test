using System;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Windows.Win32.Foundation
{
	[DebuggerDisplay("{Value}")]
	[GeneratedCode("Microsoft.Windows.CsWin32", "0.3.106+a37a0b4b70")]
	internal readonly struct HINSTANCE : IEquatable<HINSTANCE>
	{
		internal readonly IntPtr Value;

		internal static HINSTANCE Null => default(HINSTANCE);

		internal bool IsNull => Value == (IntPtr)0;

		internal HINSTANCE(IntPtr value)
		{
			Value = value;
		}

		public static implicit operator IntPtr(HINSTANCE value)
		{
			return value.Value;
		}

		public static explicit operator HINSTANCE(IntPtr value)
		{
			return new HINSTANCE(value);
		}

		public static bool operator ==(HINSTANCE left, HINSTANCE right)
		{
			return left.Value == right.Value;
		}

		public static bool operator !=(HINSTANCE left, HINSTANCE right)
		{
			return !(left == right);
		}

		public bool Equals(HINSTANCE other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj is HINSTANCE)
			{
				HINSTANCE other = (HINSTANCE)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return $"0x{Value:x}";
		}

		public static implicit operator HMODULE(HINSTANCE value)
		{
			return new HMODULE(value.Value);
		}
	}
}
