using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using CefHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class CefService
	{
		private static readonly Dictionary<CefAvailableVersion, CefPkgVersion> _versions = new Dictionary<CefAvailableVersion, CefPkgVersion>
		{
			{
				CefAvailableVersion.v103,
				new CefPkgVersion("103.0.90", "103.0.9")
			},
			{
				CefAvailableVersion.v144,
				new CefPkgVersion("144.0.120", "144.0.12")
			}
		};

		private static string _cefFolder = Path.Combine(CefSharpVersionsFolder, $"{CurrentVersion}");

		private static string _cefSharpFolder = Path.Combine(CefSharpVersionsFolder, $"{CurrentVersion}");

		private static string _cefSharpBhmPath = Path.Combine("cef", $"{CurrentVersion}");

		private static readonly Dictionary<string, AssemblyLoadType> _pendingResolveDlls = new Dictionary<string, AssemblyLoadType>();

		private bool _eventHandlersBound;

		private static readonly CefPkgVersion _suggestionVersion = _versions[CefAvailableVersion.v144];

		public static readonly CefPkgVersion DefaultVersion = _versions[CefAvailableVersion.v103];

		private static WebPeeperModule Module => WebPeeperModule.Instance;

		private static ModuleSettings Settings => Module.Settings;

		public static CefPkgVersion CurrentVersion { get; private set; } = _versions[Settings.CefVersion.get_Value()];


		private static string CefSharpVersionsFolder => DirectoryUtil.RegisterDirectory(Module.DataFolder, "CefVersions");

		private static string CefCacheFolder => DirectoryUtil.RegisterDirectory(Module.DataFolder, "CefCache");

		public static bool LibLoadStarted { get; private set; } = false;


		public static IReadOnlyDictionary<CefAvailableVersion, CefPkgVersion> Versions => _versions;

		private static bool IsDefaultVersion => CurrentVersion == DefaultVersion;

		public static bool Outdated => CurrentVersion < _suggestionVersion;

		public static event EventHandler LibLoadStart;

		public void Load()
		{
			CleanOldData();
			ExtractFiles();
			Module.DownloadService.Download(CurrentVersion);
		}

		public void Unload()
		{
			CefService.LibLoadStart = null;
			((Game)WebPeeperModule.BlishHudInstance).remove_Exiting((EventHandler<EventArgs>)OnBlishHudExiting);
			AppDomain.CurrentDomain.AssemblyResolve -= CefSharpLibResolver;
			if (LibLoadStarted)
			{
				OnBlishHudExiting(this, EventArgs.Empty);
			}
		}

		public void ApplySettingVersion()
		{
			CefPkgVersion currentVersion = _versions[Settings.CefVersion.get_Value()];
			if (!LibLoadStarted)
			{
				CurrentVersion = currentVersion;
			}
		}

		public string GetCefSharpFolder(CefPkgVersion version)
		{
			return Path.Combine(CefSharpVersionsFolder, version.ToString());
		}

		private void CleanOldData()
		{
			WebPeeperModule.Logger.Debug("CefService.CleanOldData: cleaning WebPeeper old version data.");
			try
			{
				string path = Path.Combine(DirectoryUtil.get_CachePath(), "cefsharp");
				if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
			}
			catch (Exception ex)
			{
				WebPeeperModule.Logger.Error(ex.Message);
			}
			try
			{
				string path2 = Path.Combine(Module.DataFolder, "CefUserData");
				if (Directory.Exists(path2))
				{
					Directory.Delete(path2, recursive: true);
				}
			}
			catch (Exception ex2)
			{
				WebPeeperModule.Logger.Error(ex2.Message);
			}
			try
			{
				CefPkgVersion version = _versions[Settings.CefErrorVersion.get_Value()];
				Settings.CefErrorVersion.set_Value(CefAvailableVersion.v103);
				Module.DownloadService.Delete(version);
			}
			catch (Exception ex3)
			{
				WebPeeperModule.Logger.Error(ex3.Message);
			}
		}

		private void ClearCefCache()
		{
			try
			{
				Directory.Delete(CefCacheFolder, recursive: true);
			}
			catch
			{
			}
		}

		private void SetupCefDll()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
			{
				setLibCefDllFolder(this, EventArgs.Empty);
			}
			else
			{
				GameService.GameIntegration.get_Gw2Instance().add_Gw2Started((EventHandler<EventArgs>)setLibCefDllFolder);
			}
			void setLibCefDllFolder(object s, EventArgs e)
			{
				GameService.GameIntegration.get_Gw2Instance().remove_Gw2Started((EventHandler<EventArgs>)setLibCefDllFolder);
				if (IsDefaultVersion)
				{
					_cefFolder = Path.Combine(Path.GetDirectoryName(GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().MainModule.FileName), "bin64\\cef");
				}
				else
				{
					_cefFolder = ChangePathTail(_cefFolder, $"{CurrentVersion}");
				}
				Environment.SetEnvironmentVariable("PATH", _cefFolder + ";" + Environment.GetEnvironmentVariable("PATH"));
				WebPeeperModule.Logger.Debug($"CefService.SetupCefDll: cef {CurrentVersion} path {_cefFolder}");
			}
		}

		private void BindEventHandlers()
		{
			if (!_eventHandlersBound)
			{
				WebPeeperModule.Logger.Debug("CefService.BindEventHandlers: binding cefHelper event.");
				_eventHandlersBound = true;
				SetContextCreatedScript();
				((Game)WebPeeperModule.BlishHudInstance).add_Exiting((EventHandler<EventArgs>)OnBlishHudExiting);
				Browser.add_BlishHudSchemeRequested((Func<string, Stream>)OnBlishHudSchemeRequested);
				Browser.add_FocusedChanged((Action<bool>)OnFocusedChanged);
				Browser.add_TitleChanged((Action<string>)OnTitleChanged);
			}
		}

		private void SetContextCreatedScript()
		{
			using MemoryStream stream = Module.ContentsManager.GetFileStream("onContextCreated.js") as MemoryStream;
			using TextReader textReader = new StreamReader(stream, Encoding.UTF8);
			Browser.ContextCreatedScript = textReader.ReadToEnd();
		}

		private void ExtractFiles()
		{
			WebPeeperModule.Logger.Debug("CefService.ExtractFiles: extracting CefSharp default version.");
			string[] array = new string[4] { "CefSharp.dll", "CefSharp.BrowserSubprocess.Core.dll", "CefSharp.BrowserSubprocess.exe", "CefSharp.Core.Runtime.dll" }.Select((string f) => Path.Combine(ChangePathTail(_cefSharpBhmPath, $"{DefaultVersion}"), f)).ToArray();
			string text = ChangePathTail(_cefSharpFolder, $"{DefaultVersion}");
			Directory.CreateDirectory(text);
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				string path = Path.Combine(text, Path.GetFileName(text2));
				byte[] fileBytes = WebPeeperModule.InstanceModuleManager.get_DataReader().GetFileBytes(text2);
				try
				{
					using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096);
					fileStream.Write(fileBytes, 0, fileBytes.Length);
				}
				catch
				{
				}
			}
		}

		private void SetupCefSharpDll()
		{
			_pendingResolveDlls.Add("CefHelper", AssemblyLoadType.Bytes);
			if (IsDefaultVersion)
			{
				_pendingResolveDlls.Add("CefSharp", AssemblyLoadType.Path);
				_pendingResolveDlls.Add("CefSharp.OffScreen", AssemblyLoadType.Bytes);
				_pendingResolveDlls.Add("CefSharp.Core", AssemblyLoadType.Bytes);
				_pendingResolveDlls.Add("CefSharp.Core.Runtime", AssemblyLoadType.Path);
			}
			else
			{
				_pendingResolveDlls.Add("CefSharp", AssemblyLoadType.Path);
				_pendingResolveDlls.Add("CefSharp.OffScreen", AssemblyLoadType.Path);
				_pendingResolveDlls.Add("CefSharp.Core", AssemblyLoadType.Path);
				_pendingResolveDlls.Add("CefSharp.Core.Runtime", AssemblyLoadType.Path);
			}
			_cefSharpFolder = ChangePathTail(_cefSharpFolder, $"{CurrentVersion}");
			_cefSharpBhmPath = ChangePathTail(_cefSharpBhmPath, $"{CurrentVersion}");
			AppDomain.CurrentDomain.AssemblyResolve += CefSharpLibResolver;
			WebPeeperModule.Logger.Debug($"CefService.SetupCefSharpDll: cefsharp {CurrentVersion} path {_cefSharpFolder}");
			WebPeeperModule.Logger.Debug($"CefService.SetupCefSharpDll: cefsharp {CurrentVersion} path .bhm\\{_cefSharpBhmPath}");
		}

		private Assembly CefSharpLibResolver(object sender, ResolveEventArgs args)
		{
			string name = new AssemblyName(args.Name).Name;
			if (!_pendingResolveDlls.TryGetValue(name, out var value))
			{
				WebPeeperModule.Logger.Debug("CefService.CefSharpLibResolver: not in pending, skip load");
				return null;
			}
			_pendingResolveDlls.Remove(name);
			name += ".dll";
			switch (value)
			{
			case AssemblyLoadType.Bytes:
			{
				string text2 = Path.Combine(_cefSharpBhmPath, name);
				byte[] fileBytes = WebPeeperModule.InstanceModuleManager.get_DataReader().GetFileBytes(text2);
				WebPeeperModule.Logger.Debug("CefService.CefSharpLibResolver: load .bhm\\" + text2);
				return Assembly.Load(fileBytes);
			}
			case AssemblyLoadType.Path:
			{
				string text = Path.Combine(_cefSharpFolder, name);
				WebPeeperModule.Logger.Debug("CefService.CefSharpLibResolver: load " + text);
				return Assembly.LoadFrom(text);
			}
			default:
				return null;
			}
		}

		public Task<Texture2D> GetScreenshot()
		{
			return Browser.GetScreenshot().ContinueWith(delegate(Task<byte[]> t)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				int count = t.Result.Length;
				GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					using MemoryStream memoryStream = new MemoryStream();
					memoryStream.Write(t.Result, 0, count);
					return Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
				}
				finally
				{
					((GraphicsDeviceContext)(ref val)).Dispose();
				}
			});
		}

		public async void Search(string text)
		{
			HttpClient client = new HttpClient();
			try
			{
				if (!Uri.TryCreate(text, UriKind.Absolute, out var _))
				{
					UriBuilder uriBuilder = new UriBuilder(text);
					using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(3.0));
					HttpResponseMessage val = await client.GetAsync(uriBuilder.Uri, (HttpCompletionOption)1, cts.Token);
					try
					{
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				Browser.LoadUrlAsync(text);
			}
			catch
			{
				Browser.LoadUrlAsync(new Regex("{\\s*text\\s*}").Replace(Settings.SearchUrl.get_Value(), Uri.EscapeDataString(text)));
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}

		public void ApplyFrameRate()
		{
			Browser.SetFrameRate(Settings.GetFrameRate());
		}

		public void ApplyUserAgent()
		{
			Browser.SetMobileUserAgent(Settings.IsMobileLayout.get_Value());
		}

		private void OnBlishHudExiting(object sender, EventArgs e)
		{
			Browser.Dispose();
		}

		private Stream OnBlishHudSchemeRequested(string filePath)
		{
			return Module.ContentsManager.GetFileStream(filePath);
		}

		private void OnFocusedChanged(bool focused)
		{
			WebPeeperModule.Logger.Debug($"CefService.OnFocusedChanged: focused={focused}");
			WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
			{
				if (focused)
				{
					Module.ImeService.Enable();
				}
				else
				{
					Module.ImeService.Disable();
				}
			});
		}

		private void OnTitleChanged(string title)
		{
			UiService uiService = Module.UiService;
			if (uiService != null && uiService.BrowserWindow != null)
			{
				((WindowBase2)uiService.BrowserWindow).set_Subtitle(title);
			}
		}

		private string ChangePathTail(string path, string directoryName)
		{
			return Path.Combine(Path.GetDirectoryName(path) ?? "", directoryName);
		}

		private void SetupLib()
		{
			if (!LibLoadStarted)
			{
				WebPeeperModule.Logger.Debug("CefService.SetupLib");
				LibLoadStarted = true;
				CefVersionSettingView.UpdateView?.Invoke();
				CefService.LibLoadStart?.Invoke(this, EventArgs.Empty);
				if (Settings.IsCleanMode.get_Value())
				{
					ClearCefCache();
				}
				SetupCefDll();
				SetupCefSharpDll();
			}
		}

		private Task CreateWebBrowser()
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			try
			{
				Browser.CefSettingInit(IsDefaultVersion ? _cefFolder : Path.Combine(_cefFolder, "locales"), CefCacheFolder, _cefSharpFolder);
				Browser.Create(Settings.HomeUrl.get_Value(), Settings.GetFrameRate(), Settings.IsMobileLayout.get_Value()).ContinueWith(delegate(Task<bool> t)
				{
					if (t.Status == TaskStatus.RanToCompletion)
					{
						tcs.TrySetResult(result: true);
					}
					else
					{
						WebPeeperModule.Logger.Error(t.Exception?.Message);
						tcs.TrySetException(t.Exception);
					}
				});
				Task.Delay(TimeSpan.FromSeconds(5.0)).ContinueWith((Task t) => tcs.TrySetCanceled());
			}
			catch (Exception ex)
			{
				WebPeeperModule.Logger.Error(ex.Message);
				Settings.RedownloadCef();
				tcs.TrySetException(ex);
			}
			return tcs.Task;
		}

		public async void CloseWebBrowser()
		{
			BrowserWindow browserWindow = Module.UiService?.BrowserWindow;
			if (browserWindow != null)
			{
				await browserWindow.PrepareQuitBrowser();
			}
			Browser.Close();
		}

		public Task StartBrowsing()
		{
			WebPeeperModule.Logger.Debug("CefService.StartBrowsing");
			return Task.Run(async delegate
			{
				SetupLib();
				BindEventHandlers();
				await CreateWebBrowser();
			});
		}
	}
}
