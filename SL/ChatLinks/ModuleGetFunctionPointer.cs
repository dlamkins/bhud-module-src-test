using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using SQLitePCL;
using Windows.Win32;

namespace SL.ChatLinks
{
	public class ModuleGetFunctionPointer : IGetFunctionPointer
	{
		private readonly ProcessModule _module = module ?? throw new ArgumentNullException("module");

		public ModuleGetFunctionPointer(ProcessModule module)
		{
		}

		public ModuleGetFunctionPointer(string moduleName)
			: this(GetModule(moduleName))
		{
		}

		public static ProcessModule GetModule(string moduleName)
		{
			string moduleName2 = moduleName;
			List<ProcessModule> modules = (from ProcessModule e in Process.GetCurrentProcess().Modules
				where Path.GetFileNameWithoutExtension(e.ModuleName) == moduleName2
				select e).ToList();
			if (modules != null)
			{
				switch (modules.Count)
				{
				case 1:
					return modules[0];
				case 0:
					throw new ArgumentException("Found no modules named '" + moduleName2 + "' in the current process.", "moduleName");
				}
			}
			throw new ArgumentException("Found several modules named '" + moduleName2 + "' in the current process.");
		}

		public IntPtr GetFunctionPointer(string name)
		{
			using SafeProcessHandle handle = new SafeProcessHandle(_module.BaseAddress, ownsHandle: false);
			return PInvoke.GetProcAddress(handle, name);
		}
	}
}
