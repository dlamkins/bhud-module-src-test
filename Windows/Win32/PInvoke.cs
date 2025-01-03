using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32.Foundation;

namespace Windows.Win32
{
	[GeneratedCode("Microsoft.Windows.CsWin32", "0.3.106+a37a0b4b70")]
	internal static class PInvoke
	{
		[DllImport("KERNEL32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		internal static extern BOOL FreeLibrary(HMODULE hLibModule);

		internal unsafe static FARPROC GetProcAddress(SafeHandle hModule, string lpProcName)
		{
			bool hModuleAddRef = false;
			try
			{
				fixed (byte* lpProcNameLocal = ((lpProcName != null) ? Encoding.Default.GetBytes(lpProcName) : null))
				{
					if (hModule != null)
					{
						hModule.DangerousAddRef(ref hModuleAddRef);
						HMODULE hModuleLocal = (HMODULE)hModule.DangerousGetHandle();
						return GetProcAddress(hModuleLocal, new PCSTR(lpProcNameLocal));
					}
					throw new ArgumentNullException("hModule");
				}
			}
			finally
			{
				if (hModuleAddRef)
				{
					hModule.DangerousRelease();
				}
			}
		}

		[DllImport("KERNEL32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		internal static extern FARPROC GetProcAddress(HMODULE hModule, PCSTR lpProcName);
	}
}
