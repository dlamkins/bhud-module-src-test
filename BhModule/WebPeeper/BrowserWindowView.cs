using System;
using System.Threading.Tasks;
using BhModule.WebPeeper.Window;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal class BrowserWindowView : View
	{
		private Container _window;

		private Control _windowContent;

		protected override void Build(Container window)
		{
			_window = window;
			_window.add_ContentResized((EventHandler<RegionChangedEventArgs>)OnWindowResize);
			if (CefService.Outdated)
			{
				ShowOutdatedWarning();
			}
			else
			{
				ShowDownloadProgress();
			}
		}

		private void OnWindowResize(object sender, RegionChangedEventArgs evt)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (_windowContent != null)
			{
				Control windowContent = _windowContent;
				Rectangle currentRegion = evt.get_CurrentRegion();
				windowContent.set_Size(((Rectangle)(ref currentRegion)).get_Size());
			}
		}

		protected override void Unload()
		{
			_window.remove_ContentResized((EventHandler<RegionChangedEventArgs>)OnWindowResize);
			Control windowContent = _windowContent;
			if (windowContent != null)
			{
				windowContent.Dispose();
			}
		}

		private void ShowOutdatedWarning()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (Warning.IsAccepted)
			{
				ShowDownloadProgress();
				return;
			}
			WebPeeperModule.Instance.DownloadService.Download(CefService.CurrentVersion);
			Warning warning = new Warning();
			((Control)warning).set_Parent(_window);
			Rectangle contentRegion = _window.get_ContentRegion();
			((Control)warning).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			Warning warning2 = warning;
			warning2.Accepted += delegate
			{
				ShowDownloadProgress();
			};
			_windowContent = (Control)(object)warning2;
		}

		private void ShowDownloadProgress()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			DownloadService downloadService = WebPeeperModule.Instance.DownloadService;
			bool flag = downloadService.CheckCefLib(CefService.CurrentVersion);
			if (!flag)
			{
				downloadService.Download(CefService.CurrentVersion);
			}
			if (downloadService.Downloading || !flag)
			{
				ProgressBar obj = new ProgressBar(() => downloadService.ProgressPercentage)
				{
					Text = "Downloading CEF..."
				};
				((Control)obj).set_Parent(_window);
				Rectangle contentRegion = _window.get_ContentRegion();
				((Control)obj).set_Size(((Rectangle)(ref contentRegion)).get_Size());
				obj.BarSize = new Point(150, 30);
				ProgressBar progressBar = obj;
				progressBar.ProgressUpdated += delegate(object s, ValueChangedEventArgs<float> e)
				{
					if (e.get_NewValue() >= 1f && ((Control)_window).get_Visible())
					{
						ShowBrowser();
					}
				};
				_windowContent = (Control)(object)progressBar;
			}
			else
			{
				ShowBrowser();
			}
		}

		private void ShowBrowser()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Control windowContent = _windowContent;
			if (windowContent != null)
			{
				windowContent.Dispose();
			}
			WaitingCefSetup waitingCefSetup = new WaitingCefSetup();
			((Control)waitingCefSetup).set_Parent(_window);
			Rectangle contentRegion = _window.get_ContentRegion();
			((Control)waitingCefSetup).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			_windowContent = (Control)(object)waitingCefSetup;
			WebPeeperModule.Instance.CefService.StartBrowsing().ContinueWith(delegate
			{
				WebPeeperModule.BlishHudInstance.get_Form().SafeInvoke(delegate
				{
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					Control windowContent2 = _windowContent;
					if (windowContent2 != null)
					{
						windowContent2.Dispose();
					}
					Rectangle contentRegion2 = _window.get_ContentRegion();
					WindowContent windowContent3 = new WindowContent(((Rectangle)(ref contentRegion2)).get_Size());
					((Control)windowContent3).set_Parent(_window);
					_windowContent = (Control)(object)windowContent3;
				});
			}, TaskContinuationOptions.OnlyOnRanToCompletion);
		}

		public BrowserWindowView()
			: this()
		{
		}
	}
}
