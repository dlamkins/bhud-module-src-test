using System;
using System.Linq;
using BhModule.WebPeeper.Window;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal class CefVersionSettingView : EnumSettingView<CefAvailableVersion>
	{
		public static Action UpdateView;

		private StandardButton _downloadBtn;

		private StandardButton _deleteBtn;

		private Label _hint;

		private ProgressBar _progressBar;

		private CefPkgVersion Version => CefService.Versions[((SettingView<CefAvailableVersion>)(object)this).get_Value()];

		private DownloadService DownloadService => WebPeeperModule.Instance.DownloadService;

		public CefVersionSettingView(SettingEntry<CefAvailableVersion> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Expected O, but got Unknown
			base.BuildSetting(buildPanel);
			Control selection = buildPanel.get_Children().get_Item(1);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(100);
			((Control)val).set_Height(25);
			val.set_Text("Delete");
			((Control)val).set_BasicTooltipText("Clear downloaded files.");
			_deleteBtn = val;
			((Control)_deleteBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DownloadService.Delete(Version);
				((Control)_downloadBtn).set_Visible(true);
				((Control)_deleteBtn).set_Visible(false);
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(100);
			((Control)val2).set_Height(25);
			val2.set_Text("Download");
			((Control)val2).set_BasicTooltipText("Download CefSharp packages from Nuget.");
			_downloadBtn = val2;
			((Control)_downloadBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DownloadService.Download(Version);
				((Control)_progressBar).set_Visible(true);
				((Control)_downloadBtn).set_Visible(false);
			});
			ProgressBar progressBar = new ProgressBar(() => DownloadService.ProgressPercentage);
			((Control)progressBar).set_Parent(buildPanel);
			((Control)progressBar).set_Top(2);
			((Control)progressBar).set_Width(100);
			((Control)progressBar).set_Height(23);
			_progressBar = progressBar;
			_progressBar.ProgressUpdated += delegate(object s, ValueChangedEventArgs<float> e)
			{
				if (!(e.get_NewValue() < 1f))
				{
					((Control)_deleteBtn).set_Visible(true);
					((Control)_progressBar).set_Visible(false);
				}
			};
			Label val3 = new Label();
			val3.set_Text("Requires Blish-HUD restart.");
			val3.set_TextColor(Color.get_Yellow());
			((Control)val3).set_Parent(buildPanel);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Top(2);
			_hint = val3;
			selection.add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				((Control)_deleteBtn).set_Left(selection.get_Right() + 5);
				((Control)_downloadBtn).set_Left(selection.get_Right() + 5);
				((Control)_progressBar).set_Left(selection.get_Right() + 5);
				((Control)_hint).set_Left(selection.get_Right() + 5);
			});
			((SettingView<CefAvailableVersion>)(object)this).add_ValueChanged((EventHandler<ValueEventArgs<CefAvailableVersion>>)delegate
			{
				SetChildrenVisibleState();
			});
			SetChildrenVisibleState();
			UpdateView = SetChildrenVisibleState;
		}

		private void SetChildrenVisibleState()
		{
			bool visible = false;
			bool visible2 = false;
			bool visible3 = false;
			bool visible4 = false;
			bool flag = DownloadService.CheckCefLib(Version);
			bool num = DownloadService.DownloadingVersions.Contains(Version);
			bool flag2 = Version == CefService.DefaultVersion;
			bool flag3 = Version == CefService.CurrentVersion;
			bool libLoadStarted = CefService.LibLoadStarted;
			if (num)
			{
				visible = true;
			}
			else if (flag)
			{
				visible3 = !flag2 && !(flag3 && libLoadStarted);
			}
			else
			{
				visible2 = true;
			}
			if (!flag3 && libLoadStarted)
			{
				visible4 = true;
			}
			((Control)_deleteBtn).set_Visible(visible3);
			((Control)_downloadBtn).set_Visible(visible2);
			((Control)_progressBar).set_Visible(visible);
			((Control)_hint).set_Visible(visible4);
			if (((Control)_deleteBtn).get_Visible() || ((Control)_downloadBtn).get_Visible())
			{
				((Control)_hint).set_Left(((Control)_downloadBtn).get_Right() + 10);
			}
			else if (((Control)_progressBar).get_Visible())
			{
				((Control)_hint).set_Left(((Control)_progressBar).get_Right() + 10);
			}
			else
			{
				((Control)_hint).set_Left(((Control)_downloadBtn).get_Left());
			}
		}
	}
}
