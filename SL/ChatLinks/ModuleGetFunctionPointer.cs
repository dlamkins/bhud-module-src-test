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
		private readonly ProcessModule _module;

		public static ProcessModule GetModule(string moduleName)
		{
			string moduleName2 = moduleName;
			List<ProcessModule> list = (from ProcessModule e in Process.GetCurrentProcess().Modules
				where Path.GetFileNameWithoutExtension(e.ModuleName) == moduleName2
				select e).ToList();
			if (list.Count == 0)
			{
				throw new ArgumentException("Found no modules named '" + moduleName2 + "' in the current process.", "moduleName");
			}
			if (list.Count > 1)
			{
				throw new ArgumentException("Found several modules named '" + moduleName2 + "' in the current process.", "moduleName");
			}
			return list[0];
		}

		public ModuleGetFunctionPointer(string moduleName)
			: this(GetModule(moduleName))
		{
		}

		public ModuleGetFunctionPointer(ProcessModule module)
		{
			_module = module ?? throw new ArgumentNullException("module");
		}

		public IntPtr GetFunctionPointer(string name)
		{
			using SafeProcessHandle handle = new SafeProcessHandle(_module.BaseAddress, ownsHandle: false);
			return PInvoke.GetProcAddress(handle, name);
		}
	}
}
