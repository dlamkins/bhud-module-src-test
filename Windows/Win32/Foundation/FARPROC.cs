using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation
{
	[GeneratedCode("Microsoft.Windows.CsWin32", "0.3.106+a37a0b4b70")]
	internal struct FARPROC
	{
		internal IntPtr Value;

		internal static FARPROC Null => default(FARPROC);

		internal bool IsNull => Value == (IntPtr)0;

		internal FARPROC(IntPtr value)
		{
			Value = value;
		}

		public static implicit operator IntPtr(FARPROC value)
		{
			return value.Value;
		}

		public static explicit operator FARPROC(IntPtr value)
		{
			return new FARPROC(value);
		}

		public static bool operator ==(FARPROC left, FARPROC right)
		{
			return left.Value == right.Value;
		}

		public static bool operator !=(FARPROC left, FARPROC right)
		{
			return !(left == right);
		}

		public bool Equals(FARPROC other)
		{
			return Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj is FARPROC)
			{
				FARPROC other = (FARPROC)obj;
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

		internal TDelegate CreateDelegate<TDelegate>() where TDelegate : Delegate
		{
			return Marshal.GetDelegateForFunctionPointer<TDelegate>(Value);
		}
	}
}
