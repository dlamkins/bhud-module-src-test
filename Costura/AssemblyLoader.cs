using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Costura
{
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		private static object nullCacheLock = new object();

		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		private static string tempBasePath;

		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		private static List<string> preload32List = new List<string>();

		private static List<string> preload64List = new List<string>();

		private static Dictionary<string, string> checksums = new Dictionary<string, string>();

		private static int isAttached;

		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return "";
			}
			return culture.Name;
		}

		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		private static Assembly ReadFromDiskCache(string tempBasePath, AssemblyName requestedAssemblyName)
		{
			string name = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				name = requestedAssemblyName.CultureInfo.Name + "." + name;
			}
			string bittyness = ((IntPtr.Size == 8) ? "64" : "32");
			string assemblyTempFilePath = Path.Combine(tempBasePath, name + ".dll");
			if (File.Exists(assemblyTempFilePath))
			{
				return Assembly.LoadFile(assemblyTempFilePath);
			}
			assemblyTempFilePath = Path.ChangeExtension(assemblyTempFilePath, "exe");
			if (File.Exists(assemblyTempFilePath))
			{
				return Assembly.LoadFile(assemblyTempFilePath);
			}
			assemblyTempFilePath = Path.Combine(Path.Combine(tempBasePath, bittyness), name + ".dll");
			if (File.Exists(assemblyTempFilePath))
			{
				return Assembly.LoadFile(assemblyTempFilePath);
			}
			assemblyTempFilePath = Path.ChangeExtension(assemblyTempFilePath, "exe");
			if (File.Exists(assemblyTempFilePath))
			{
				return Assembly.LoadFile(assemblyTempFilePath);
			}
			return null;
		}

		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream stream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using DeflateStream source = new DeflateStream(stream, CompressionMode.Decompress);
					MemoryStream memoryStream = new MemoryStream();
					CopyTo(source, memoryStream);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			if (resourceNames.TryGetValue(name, out var value))
			{
				return LoadStream(value);
			}
			return null;
		}

		private static byte[] ReadStream(Stream stream)
		{
			byte[] data = new byte[stream.Length];
			stream.Read(data, 0, data.Length);
			return data;
		}

		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string name = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				name = requestedAssemblyName.CultureInfo.Name + "." + name;
			}
			byte[] assemblyData;
			using (Stream stream = LoadStream(assemblyNames, name))
			{
				if (stream == null)
				{
					return null;
				}
				assemblyData = ReadStream(stream);
			}
			using (Stream stream2 = LoadStream(symbolNames, name))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = ReadStream(stream2);
					return Assembly.Load(assemblyData, rawSymbolStore);
				}
			}
			return Assembly.Load(assemblyData);
		}

		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(e.Name))
				{
					return null;
				}
			}
			AssemblyName requestedAssemblyName = new AssemblyName(e.Name);
			Assembly assembly = ReadExistingAssembly(requestedAssemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromDiskCache(tempBasePath, requestedAssemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, requestedAssemblyName);
			if ((object)assembly == null)
			{
				lock (nullCacheLock)
				{
					nullCache[e.Name] = true;
				}
				if ((requestedAssemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
				{
					assembly = Assembly.Load(requestedAssemblyName);
				}
			}
			return assembly;
		}

		static AssemblyLoader()
		{
			checksums.Add("costura64.e_sqlite3.dll", "912134509E96EC5049AC7A3316785CD8214CCF05");
			assemblyNames.Add("asyncclipboardservice", "costura.asyncclipboardservice.dll.compressed");
			assemblyNames.Add("blish hud", "costura.blish hud.exe.compressed");
			assemblyNames.Add("costura", "costura.costura.dll.compressed");
			assemblyNames.Add("de.blish hud.resources", "costura.de.blish hud.resources.dll.compressed");
			assemblyNames.Add("entityframework", "costura.entityframework.dll.compressed");
			assemblyNames.Add("entityframework.sqlserver", "costura.entityframework.sqlserver.dll.compressed");
			assemblyNames.Add("es.blish hud.resources", "costura.es.blish hud.resources.dll.compressed");
			assemblyNames.Add("fr.blish hud.resources", "costura.fr.blish hud.resources.dll.compressed");
			assemblyNames.Add("gw2sharp", "costura.gw2sharp.dll.compressed");
			assemblyNames.Add("jsonflatfiledatastore", "costura.jsonflatfiledatastore.dll.compressed");
			assemblyNames.Add("microsoft.bcl.asyncinterfaces", "costura.microsoft.bcl.asyncinterfaces.dll.compressed");
			assemblyNames.Add("monogame.extended", "costura.monogame.extended.dll.compressed");
			assemblyNames.Add("monogame.framework", "costura.monogame.framework.dll.compressed");
			assemblyNames.Add("naudio.core", "costura.naudio.core.dll.compressed");
			assemblyNames.Add("naudio.wasapi", "costura.naudio.wasapi.dll.compressed");
			assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			assemblyNames.Add("semver", "costura.semver.dll.compressed");
			assemblyNames.Add("sharpdx.direct2d1", "costura.sharpdx.direct2d1.dll.compressed");
			assemblyNames.Add("sharpdx.direct3d11", "costura.sharpdx.direct3d11.dll.compressed");
			assemblyNames.Add("sharpdx.direct3d9", "costura.sharpdx.direct3d9.dll.compressed");
			assemblyNames.Add("sharpdx", "costura.sharpdx.dll.compressed");
			assemblyNames.Add("sharpdx.dxgi", "costura.sharpdx.dxgi.dll.compressed");
			assemblyNames.Add("sharpdx.mathematics", "costura.sharpdx.mathematics.dll.compressed");
			assemblyNames.Add("sharpdx.mediafoundation", "costura.sharpdx.mediafoundation.dll.compressed");
			assemblyNames.Add("sharpdx.xaudio2", "costura.sharpdx.xaudio2.dll.compressed");
			assemblyNames.Add("sharpdx.xinput", "costura.sharpdx.xinput.dll.compressed");
			assemblyNames.Add("sqlitenetextensions", "costura.sqlitenetextensions.dll.compressed");
			assemblyNames.Add("sqlitenetextensionsasync", "costura.sqlitenetextensionsasync.dll.compressed");
			assemblyNames.Add("sqlitepclraw.batteries_green", "costura.sqlitepclraw.batteries_green.dll.compressed");
			assemblyNames.Add("sqlitepclraw.provider.dynamic_cdecl", "costura.sqlitepclraw.provider.dynamic_cdecl.dll.compressed");
			assemblyNames.Add("sqlitepclraw.provider.e_sqlite3", "costura.sqlitepclraw.provider.e_sqlite3.dll.compressed");
			assemblyNames.Add("system.buffers", "costura.system.buffers.dll.compressed");
			assemblyNames.Add("system.data.sqlite", "costura.system.data.sqlite.dll.compressed");
			assemblyNames.Add("system.data.sqlite.ef6", "costura.system.data.sqlite.ef6.dll.compressed");
			assemblyNames.Add("system.data.sqlite.linq", "costura.system.data.sqlite.linq.dll.compressed");
			assemblyNames.Add("system.diagnostics.diagnosticsource", "costura.system.diagnostics.diagnosticsource.dll.compressed");
			assemblyNames.Add("system.memory", "costura.system.memory.dll.compressed");
			assemblyNames.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
			assemblyNames.Add("system.resources.extensions", "costura.system.resources.extensions.dll.compressed");
			assemblyNames.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
			assemblyNames.Add("system.servicemodel.primitives", "costura.system.servicemodel.primitives.dll.compressed");
			assemblyNames.Add("system.text.encodings.web", "costura.system.text.encodings.web.dll.compressed");
			assemblyNames.Add("system.text.json", "costura.system.text.json.dll.compressed");
			assemblyNames.Add("system.threading.tasks.extensions", "costura.system.threading.tasks.extensions.dll.compressed");
			assemblyNames.Add("system.valuetuple", "costura.system.valuetuple.dll.compressed");
			preload64List.Add("costura64.e_sqlite3.dll");
		}

		private static void CreateDirectory(string tempBasePath)
		{
			if (!Directory.Exists(tempBasePath))
			{
				Directory.CreateDirectory(tempBasePath);
			}
		}

		private static string ResourceNameToPath(string lib)
		{
			string bittyness = ((IntPtr.Size == 8) ? "64" : "32");
			string name = lib;
			if (lib.StartsWith("costura" + bittyness + "."))
			{
				name = Path.Combine(bittyness, lib.Substring(10));
			}
			else if (lib.StartsWith("costura."))
			{
				name = lib.Substring(8);
			}
			if (name.EndsWith(".compressed"))
			{
				name = name.Substring(0, name.Length - 11);
			}
			return name;
		}

		private static string CalculateChecksum(string filename)
		{
			using FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			using BufferedStream inputStream = new BufferedStream(stream);
			using SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = sHA1CryptoServiceProvider.ComputeHash(inputStream);
			StringBuilder stringBuilder = new StringBuilder(2 * array.Length);
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.AppendFormat("{0:X2}", b);
			}
			return stringBuilder.ToString();
		}

		[DllImport("kernel32.dll")]
		private static extern uint SetErrorMode(uint uMode);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

		private static void InternalPreloadUnmanagedLibraries(string tempBasePath, IList<string> libs, Dictionary<string, string> checksums)
		{
			foreach (string lib in libs)
			{
				string name = ResourceNameToPath(lib);
				string text = Path.Combine(tempBasePath, name);
				if (File.Exists(text))
				{
					string text2 = CalculateChecksum(text);
					if (text2 != checksums[lib])
					{
						File.Delete(text);
					}
				}
				if (File.Exists(text))
				{
					continue;
				}
				using Stream source = LoadStream(lib);
				using FileStream destination = File.OpenWrite(text);
				CopyTo(source, destination);
			}
			uint errorModes = 32771u;
			uint originalErrorMode = SetErrorMode(errorModes);
			foreach (string lib2 in libs)
			{
				string name = ResourceNameToPath(lib2);
				if (name.EndsWith(".dll"))
				{
					string lpFileName = Path.Combine(tempBasePath, name);
					LoadLibraryEx(lpFileName, IntPtr.Zero, 8u);
				}
			}
			SetErrorMode(originalErrorMode);
		}

		private static void PreloadUnmanagedLibraries(string hash, string tempBasePath, List<string> libs, Dictionary<string, string> checksums)
		{
			string mutexId = "Costura" + hash;
			using Mutex mutex = new Mutex(initiallyOwned: false, mutexId);
			bool flag = false;
			try
			{
				try
				{
					flag = mutex.WaitOne(60000, exitContext: false);
					if (!flag)
					{
						throw new TimeoutException("Timeout waiting for exclusive access");
					}
				}
				catch (AbandonedMutexException)
				{
					flag = true;
				}
				string path = ((IntPtr.Size == 8) ? "64" : "32");
				CreateDirectory(Path.Combine(tempBasePath, path));
				InternalPreloadUnmanagedLibraries(tempBasePath, libs, checksums);
			}
			finally
			{
				if (flag)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		public static void Attach()
		{
			if (Interlocked.Exchange(ref isAttached, 1) == 1)
			{
				return;
			}
			AppDomain currentDomain = AppDomain.CurrentDomain;
			object setupInformation = currentDomain.GetType()?.GetProperty("SetupInformation")?.GetValue(currentDomain);
			PropertyInfo targetFrameworkNameProperty = setupInformation?.GetType()?.GetProperty("TargetFrameworkName");
			if ((object)targetFrameworkNameProperty != null && targetFrameworkNameProperty.GetValue(setupInformation) == null)
			{
				string text = ((TargetFrameworkAttribute)(Assembly.GetCallingAssembly()?.GetCustomAttribute(typeof(TargetFrameworkAttribute))))?.FrameworkName;
				if (text != null)
				{
					currentDomain.SetData("TargetFrameworkName", text);
				}
			}
			string md5Hash = "2474F2701C62F55A8A06738EF996235A";
			string prefixPath = Path.Combine(Path.GetTempPath(), "Costura");
			tempBasePath = Path.Combine(prefixPath, md5Hash);
			List<string> unmanagedAssemblies = ((IntPtr.Size == 8) ? preload64List : preload32List);
			PreloadUnmanagedLibraries(md5Hash, tempBasePath, unmanagedAssemblies, checksums);
			currentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				lock (nullCacheLock)
				{
					if (nullCache.ContainsKey(e.Name))
					{
						return null;
					}
				}
				AssemblyName ResolveAssemblyrequestedAssemblyName = new AssemblyName(e.Name);
				Assembly ResolveAssemblyassembly = ReadExistingAssembly(ResolveAssemblyrequestedAssemblyName);
				if ((object)ResolveAssemblyassembly != null)
				{
					return ResolveAssemblyassembly;
				}
				ResolveAssemblyassembly = ReadFromDiskCache(tempBasePath, ResolveAssemblyrequestedAssemblyName);
				if ((object)ResolveAssemblyassembly != null)
				{
					return ResolveAssemblyassembly;
				}
				ResolveAssemblyassembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, ResolveAssemblyrequestedAssemblyName);
				if ((object)ResolveAssemblyassembly == null)
				{
					lock (nullCacheLock)
					{
						nullCache[e.Name] = true;
					}
					if ((ResolveAssemblyrequestedAssemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
					{
						ResolveAssemblyassembly = Assembly.Load(ResolveAssemblyrequestedAssemblyName);
					}
				}
				return ResolveAssemblyassembly;
			};
		}
	}
}
