using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SQLitePCL;

namespace MysticCrafting.Module
{
	public class ModuleGetFunctionPointer : IGetFunctionPointer
	{
		private readonly ProcessModule _module;

		public static ProcessModule GetModule(string moduleName)
		{
			List<ProcessModule> list = (from ProcessModule e in Process.GetCurrentProcess().Modules
				where Path.GetFileNameWithoutExtension(e.ModuleName) == moduleName
				select e).ToList();
			if (list.Count == 0)
			{
				throw new ArgumentException("Found no modules named '" + moduleName + "' in the current process.", "moduleName");
			}
			if (list.Count > 1)
			{
				throw new ArgumentException("Found several modules named '" + moduleName + "' in the current process.", "moduleName");
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
			if (!NativeLibrary.TryGetExport(_module.BaseAddress, name, out var address))
			{
				return IntPtr.Zero;
			}
			return address;
		}
	}
}
