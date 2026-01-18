using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using CefHelper;
using CefSharp;
using CefSharp.DevTools;
using CefSharp.OffScreen;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	public class CefService
	{
		private ChromiumWebBrowser _webBrowser;

		private readonly InputMethod _inputMethod;

		public string LastAddressInputText = "";

		public static string CefSharpDllPath = DirectoryUtil.RegisterDirectory(DirectoryUtil.get_CachePath(), "cefsharp/");

		public static string CefSettingFolder = DirectoryUtil.RegisterDirectory(WebPeeperModule.InstanceModuleManager.get_Manifest().get_Name().Replace(" ", "")
			.ToLower());

		private const string _mobileUserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Mobile Safari/537.36";

		private const string _defaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36";

		private string OnContextCreatedScript;

		private string _cefLocalesPath;

		public ChromiumWebBrowser WebBrowser => _webBrowser;

		public InputMethod InputMethod => _inputMethod;

		public CefService()
		{
			SetupCefDllPath();
			SetupCefSharpDllFolder();
			_inputMethod = new InputMethod();
			((Game)WebPeeperModule.BlishHudInstance).add_Exiting((EventHandler<EventArgs>)OnBlishHudExiting);
		}

		public void Load()
		{
			LoadOnContextCreatedScript();
		}

		public void Unload()
		{
			((Game)WebPeeperModule.BlishHudInstance).remove_Exiting((EventHandler<EventArgs>)OnBlishHudExiting);
			AppDomain.CurrentDomain.AssemblyResolve -= CefSharpCoreRuntimeResolver;
			_webBrowser?.Dispose();
			_inputMethod.Dispose();
		}

		private void SetupCefDllPath()
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
				Environment.SetEnvironmentVariable("PATH", (_cefLocalesPath = Path.Combine(Path.GetDirectoryName(GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().MainModule.FileName), "bin64\\cef")) + ";" + Environment.GetEnvironmentVariable("PATH"));
			}
		}

		private void CefSettingInit()
		{
			if (!Cef.IsInitialized)
			{
				ModuleSettings settings = WebPeeperModule.Instance.Settings;
				CefSharpSettings.FocusedNodeChangedEnabled = true;
				CefSettings cefSettings = new CefSettings();
				cefSettings.EnableAudio();
				cefSettings.UserAgent = (settings.IsMobileLayout.get_Value() ? "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Mobile Safari/537.36" : "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36");
				if (!string.IsNullOrEmpty(_cefLocalesPath))
				{
					cefSettings.LocalesDirPath = _cefLocalesPath;
				}
				cefSettings.BrowserSubprocessPath = Path.Combine(CefSharpDllPath, "CefSharp.BrowserSubprocess.exe");
				cefSettings.CachePath = Path.Combine(CefSettingFolder, "CefCache");
				cefSettings.UserDataPath = Path.Combine(CefSettingFolder, "CefUserData");
				cefSettings.CefCommandLineArgs.Add("gpu-preferences");
				if (WebPeeperModule.Instance.Settings.IsCleanMode.get_Value())
				{
					Directory.Delete(cefSettings.CachePath, recursive: true);
					Directory.Delete(cefSettings.UserDataPath, recursive: true);
				}
				cefSettings.PersistSessionCookies = true;
				Default.SetCefSchemeHandler(cefSettings, OnBlishHudSchemeRequested);
				Cef.Initialize(cefSettings);
			}
		}

		private void LoadOnContextCreatedScript()
		{
			using MemoryStream stream = WebPeeperModule.Instance.ContentsManager.GetFileStream("onContextCreated.js") as MemoryStream;
			using TextReader textReader = new StreamReader(stream, Encoding.UTF8);
			OnContextCreatedScript = textReader.ReadToEnd();
		}

		private void ExtractFiles(string[] paths)
		{
			foreach (string text in paths)
			{
				string path = Path.Combine(CefSharpDllPath, text);
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				byte[] fileBytes = WebPeeperModule.InstanceModuleManager.get_DataReader().GetFileBytes(text);
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

		private void SetupCefSharpDllFolder()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CefSharpCoreRuntimeResolver;
			Assembly.Load(WebPeeperModule.InstanceModuleManager.get_DataReader().GetFileBytes("CefSharp.dll"), Array.Empty<byte>());
			ExtractFiles(new string[4] { "CefSharp.BrowserSubprocess.Core.dll", "CefSharp.BrowserSubprocess.exe", "CefSharp.dll", "_\\CefSharp.Core.Runtime.dll" });
		}

		private Assembly CefSharpCoreRuntimeResolver(object sender, ResolveEventArgs args)
		{
			string text = "CefSharp.Core.Runtime";
			if (args.Name.Contains(text))
			{
				return Assembly.LoadFrom(Path.Combine(CefSharpDllPath, "_\\" + text + ".dll"));
			}
			return null;
		}

		public void FocusBlurredElement()
		{
			if (_webBrowser != null && _webBrowser.CanExecuteJavascriptInMainFrame)
			{
				_webBrowser.ExecuteScriptAsync("webPeeper_focusBlurredElement()");
			}
		}

		public async void CloseWebBrowser()
		{
			await WebPeeperModule.Instance.UIService.BrowserWindow.PrepareQuitBrowser();
			if (_webBrowser != null)
			{
				_webBrowser.Dispose();
				_webBrowser = null;
			}
		}

		public void ApplyUserAgent()
		{
			if (_webBrowser != null)
			{
				using DevToolsClient devToolsClient = _webBrowser.GetDevToolsClient();
				string userAgent = (WebPeeperModule.Instance.Settings.IsMobileLayout.get_Value() ? "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Mobile Safari/537.36" : "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36");
				devToolsClient.Emulation.SetUserAgentOverrideAsync(userAgent);
			}
		}

		private void OnBlishHudExiting(object sender, EventArgs e)
		{
			_webBrowser?.Dispose();
		}

		private (Stream, string) OnBlishHudSchemeRequested(IRequest request)
		{
			string text = new Uri(request.Url).AbsolutePath.Remove(0, 1);
			return (WebPeeperModule.Instance.ContentsManager.GetFileStream(text), Cef.GetMimeType(Path.GetExtension(text)));
		}

		private void OnContextCreated(IFrame frame)
		{
			frame.ExecuteJavaScriptAsync(OnContextCreatedScript);
		}

		private void OnFocusedNodeChanged(IDomNode node)
		{
			WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
			{
				if (node == null)
				{
					_inputMethod.Disable();
				}
				else if ((node["contenteditable"] != null && node["contenteditable"] != "false") || node.TagName == "INPUT" || node.TagName == "TEXTAREA")
				{
					_inputMethod.Enable();
				}
				else
				{
					_inputMethod.Disable();
				}
			});
		}

		private void OnMainFrameChanged()
		{
			WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
			{
				_inputMethod.Disable();
			});
		}

		private void OnTitleChanged(object sender, TitleChangedEventArgs e)
		{
			UIService uIService = WebPeeperModule.Instance.UIService;
			if (uIService != null && uIService.BrowserWindow != null)
			{
				((WindowBase2)uIService.BrowserWindow).set_Subtitle(e.Title);
			}
		}

		private void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
		{
			WebPainter.Instance?.SetErrorState(state: false);
		}

		public void OnUrlLoadError(object sender, LoadErrorEventArgs e)
		{
			NavigationBar.Instance?.SetAddressInputText(e.FailedUrl);
			string lastAddressInputText = LastAddressInputText;
			LastAddressInputText = "";
			if (!string.IsNullOrWhiteSpace(lastAddressInputText))
			{
				if (Uri.TryCreate(lastAddressInputText, UriKind.Absolute, out var _))
				{
					WebPainter.Instance?.SetErrorState(state: true);
				}
				else
				{
					_webBrowser.LoadUrlAsync(new Regex("{\\s*text\\s*}").Replace(WebPeeperModule.Instance.Settings.SearchUrl.get_Value(), Uri.EscapeDataString(lastAddressInputText)));
				}
			}
		}

		private void OnFullscreenModeChange(bool isFullscreen)
		{
			if (isFullscreen)
			{
				NavigationBar instance = NavigationBar.Instance;
				if (instance != null)
				{
					((Control)instance).Hide();
				}
			}
			else
			{
				NavigationBar instance2 = NavigationBar.Instance;
				if (instance2 != null)
				{
					((Control)instance2).Show();
				}
			}
		}

		public Task<bool> CreateWebBrowser()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Invalid comparison between Unknown and I4
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			CefSettingInit();
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			if (_webBrowser == null || _webBrowser.IsDisposed)
			{
				BrowserSettings browserSettings = new BrowserSettings(autoDispose: true);
				if (WebPeeperModule.Instance.Settings.IsFollowBhFps.get_Value())
				{
					BrowserSettings browserSettings2 = browserSettings;
					FramerateMethod frameLimiter = GameService.Graphics.get_FrameLimiter();
					int num2 = (browserSettings2.WindowlessFrameRate = (((int)frameLimiter == 1) ? 30 : (((int)frameLimiter != 2) ? 60 : 60)));
				}
				_webBrowser = new ChromiumWebBrowser(WebPeeperModule.Instance.Settings.HomeUrl.get_Value(), browserSettings);
				_webBrowser.TitleChanged += OnTitleChanged;
				_webBrowser.FrameLoadStart += OnFrameLoadStart;
				_webBrowser.LoadError += OnUrlLoadError;
				_webBrowser.BrowserInitialized += delegate
				{
					tcs.TrySetResult(result: true);
				};
				Default.SetBrowserHandlers(_webBrowser, OnContextCreated, OnFocusedNodeChanged, OnMainFrameChanged, OnFullscreenModeChange);
			}
			if (_webBrowser.IsBrowserInitialized)
			{
				tcs.TrySetResult(result: true);
			}
			return tcs.Task;
		}
	}
}
