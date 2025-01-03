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

		private static List<string> preloadWinX86List = new List<string>();

		private static List<string> preloadWinX64List = new List<string>();

		private static List<string> preloadWinArm64List = new List<string>();

		private static Dictionary<string, string> checksums = new Dictionary<string, string>();

		private static int isAttached;

		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return string.Empty;
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

		private static string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
		{
			string name = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				name = (CultureToString(requestedAssemblyName.CultureInfo) + "." + name).ToLowerInvariant();
			}
			return name;
		}

		private static string GetPlatformName()
		{
			string bittyness = ((IntPtr.Size == 8) ? "64" : "86");
			return "win-x" + bittyness;
		}

		private static Assembly ReadFromDiskCache(string tempBasePath, AssemblyName requestedAssemblyName)
		{
			string name = GetAssemblyResourceName(requestedAssemblyName);
			string platformName = GetPlatformName();
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
			assemblyTempFilePath = Path.Combine(Path.Combine(tempBasePath, platformName), name + ".dll");
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
			string name = GetAssemblyResourceName(requestedAssemblyName);
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
			string assemblyNameAsString = e.Name;
			AssemblyName assemblyName = new AssemblyName(assemblyNameAsString);
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(assemblyNameAsString))
				{
					return null;
				}
			}
			AssemblyName requestedAssemblyName = new AssemblyName(assemblyNameAsString);
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
					nullCache[assemblyNameAsString] = true;
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
			checksums.Add("costura_win_x86.sliekens.e_sqlite3.dll", "1D70002B499DA92F9371D5B819080A06C19CCB6F");
			checksums.Add("costura_win_x64.sliekens.e_sqlite3.dll", "426841DA35A580D9B280D2FD9EAF546859DA9CF7");
			preloadWinX64List.Add("costura_win_x64.sliekens.e_sqlite3.dll");
			preloadWinX86List.Add("costura_win_x86.sliekens.e_sqlite3.dll");
		}

		private static List<string> GetUnmanagedAssemblies()
		{
			if (IntPtr.Size != 8)
			{
				return preloadWinX86List;
			}
			return preloadWinX64List;
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
			string platformName = GetPlatformName();
			string name = lib;
			string platformPrefix = ("costura-" + platformName + ".").Replace("-", "_");
			string costuraPrefix = "costura.";
			if (lib.StartsWith(platformPrefix))
			{
				name = Path.Combine(platformName, lib.Substring(platformPrefix.Length));
			}
			else if (lib.StartsWith(costuraPrefix))
			{
				name = lib.Substring(costuraPrefix.Length);
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
			using SHA1 sHA = SHA1.Create();
			byte[] array = sHA.ComputeHash(inputStream);
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
				string platformName = GetPlatformName();
				string text = Path.Combine(tempBasePath, platformName);
				CreateDirectory(text);
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

		public static void Attach(bool subscribe)
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
			string md5Hash = "98593D73B80173137531834E29B70F87";
			string prefixPath = Path.Combine(Path.GetTempPath(), "Costura");
			tempBasePath = Path.Combine(prefixPath, md5Hash);
			List<string> unmanagedAssemblies = GetUnmanagedAssemblies();
			PreloadUnmanagedLibraries(md5Hash, tempBasePath, unmanagedAssemblies, checksums);
			if (!subscribe)
			{
				return;
			}
			currentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				string ResolveAssemblyassemblyNameAsString = e.Name;
				AssemblyName ResolveAssemblyassemblyName = new AssemblyName(ResolveAssemblyassemblyNameAsString);
				lock (nullCacheLock)
				{
					if (nullCache.ContainsKey(ResolveAssemblyassemblyNameAsString))
					{
						return null;
					}
				}
				AssemblyName ResolveAssemblyrequestedAssemblyName = new AssemblyName(ResolveAssemblyassemblyNameAsString);
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
						nullCache[ResolveAssemblyassemblyNameAsString] = true;
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
