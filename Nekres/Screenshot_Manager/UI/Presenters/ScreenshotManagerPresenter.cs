using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Screenshot_Manager.Properties;
using Nekres.Screenshot_Manager.UI.Controls;
using Nekres.Screenshot_Manager.UI.Models;
using Nekres.Screenshot_Manager.UI.Views;

namespace Nekres.Screenshot_Manager.UI.Presenters
{
	public class ScreenshotManagerPresenter : Presenter<ScreenshotManagerView, ScreenshotManagerModel>
	{
		public ScreenshotManagerPresenter(ScreenshotManagerView view, ScreenshotManagerModel model)
			: base(view, model)
		{
			base.get_Model().FileWatcherFactory.FileAdded += OnScreenShotAdded;
			base.get_Model().FileWatcherFactory.FileDeleted += OnScreenShotDeleted;
			base.get_Model().FileWatcherFactory.FileRenamed += OnScreenShotRenamed;
		}

		private void LoadTextures()
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			return base.Load(progress);
		}

		public async void OnScreenShotAdded(object o, ValueEventArgs<string> e)
		{
			AsyncTexture2D texture = new AsyncTexture2D();
			CreateThumbnail(base.get_View().ThumbnailFlowPanel, texture, e.get_Value());
			await LoadTexture(texture, e.get_Value());
		}

		public void OnScreenShotDeleted(object o, ValueEventArgs<string> e)
		{
			if (FindThumbnailByFileName(e.get_Value(), out var ctrl))
			{
				((Container)base.get_View().ThumbnailFlowPanel).RemoveChild((Control)(object)ctrl);
				if (ctrl != null)
				{
					((Control)ctrl).Dispose();
				}
			}
		}

		public void OnScreenShotRenamed(object o, ValueChangedEventArgs<string> e)
		{
			if (FindThumbnailByFileName(e.get_PreviousValue(), out var ctrl))
			{
				ctrl.FileName = e.get_NewValue();
				((TextInputBase)ctrl.NameTextBox).set_Text(Path.GetFileNameWithoutExtension(e.get_NewValue()));
			}
		}

		private bool FindThumbnailByFileName(string fileName, out ResponsiveThumbnail thumbnail)
		{
			thumbnail = ((IEnumerable<Control>)((Container)base.get_View().ThumbnailFlowPanel).get_Children()).Where((Control x) => ((object)x).GetType() == typeof(ResponsiveThumbnail)).Cast<ResponsiveThumbnail>().FirstOrDefault((ResponsiveThumbnail y) => fileName.Equals(y.FileName));
			return thumbnail != null;
		}

		public ThumbnailBase CreateThumbnail(FlowPanel parent, AsyncTexture2D texture, string fileName)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			ResponsiveThumbnail responsiveThumbnail = new ResponsiveThumbnail(texture, fileName);
			((Control)responsiveThumbnail).set_Parent((Container)(object)parent);
			((Control)responsiveThumbnail).set_Size(new Point(((Control)parent).get_Width() / 4 - (int)parent.get_ControlPadding().X, 144));
			responsiveThumbnail.IsFavorite = ScreenshotManagerModule.ModuleInstance.Favorites.get_Value().Any((string x) => x.Equals(Path.GetFileName(fileName)));
			responsiveThumbnail.OnDelete += OnClickDelete;
			responsiveThumbnail.FavoriteChanged += OnFavoriteChanged;
			responsiveThumbnail.OnInspect += OnClickInspect;
			return responsiveThumbnail;
		}

		private void OnClickDelete(object o, EventArgs e)
		{
			ThumbnailBase ctrl = (ThumbnailBase)o;
			if (ScreenshotManagerModule.ModuleInstance.SendToRecycleBin.get_Value())
			{
				DoDelete(ctrl, sendToRecycleBin: true);
				return;
			}
			ConfirmationPrompt.ShowPrompt(delegate(bool confirmed)
			{
				if (confirmed)
				{
					DoDelete(ctrl);
				}
			}, string.Format(Resources.You_are_about_to_permanently_destroy__0__, "“" + Path.GetFileNameWithoutExtension(ctrl.FileName) + "”") + "\n" + Resources.Are_you_sure_, Resources.Yes, Resources.Cancel);
		}

		private async void OnClickInspect(object o, EventArgs e)
		{
			ResponsiveThumbnail ctrl = (ResponsiveThumbnail)o;
			await base.get_Model().FileWatcherFactory.CreateInspectionPanel(ctrl.FileName);
		}

		private async void DoDelete(ThumbnailBase ctrl, bool sendToRecycleBin = false)
		{
			if ((!sendToRecycleBin) ? (!(await FileUtil.DeleteAsync(ctrl.FileName))) : (!(await FileUtil.SendToRecycleBinAsync(ctrl.FileName))))
			{
				ScreenNotification.ShowNotification(string.Format(Resources.Failed_to_delete_image__0__, "“" + Path.GetFileNameWithoutExtension(ctrl.FileName) + "”"), (NotificationType)2, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
				return;
			}
			ScreenshotManagerModule.ModuleInstance.DeleteSfx.Play();
			((Container)base.get_View().ThumbnailFlowPanel).RemoveChild((Control)(object)ctrl);
			((Control)ctrl).Dispose();
			base.get_View().ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>((Comparison<ResponsiveThumbnail>)SortThumbnails);
		}

		private void OnFavoriteChanged(object o, EventArgs e)
		{
			base.get_View().ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>((Comparison<ResponsiveThumbnail>)SortThumbnails);
		}

		public async Task LoadTexture(AsyncTexture2D texture, string fileName)
		{
			await TextureUtil.GetThumbnail(fileName).ContinueWith(delegate(Task<Texture2D> t)
			{
				texture.SwapTexture(t.Result);
			});
		}

		protected override void Unload()
		{
			base.get_Model().FileWatcherFactory.FileAdded -= OnScreenShotAdded;
			base.get_Model().FileWatcherFactory.FileDeleted -= OnScreenShotDeleted;
			base.get_Model().FileWatcherFactory.FileRenamed -= OnScreenShotRenamed;
			base.get_Model().Dispose();
			IEnumerable<string> favorites = from ResponsiveThumbnail x in ((IEnumerable<Control>)((Container)base.get_View().ThumbnailFlowPanel).get_Children()).Where((Control x) => ((object)x).GetType() == typeof(ResponsiveThumbnail))
				where x.IsFavorite
				select x into y
				select Path.GetFileName(y.FileName);
			ScreenshotManagerModule.ModuleInstance.Favorites.set_Value(favorites.ToList());
		}

		public int SortThumbnails(ResponsiveThumbnail x, ResponsiveThumbnail y)
		{
			string fileNameX = Path.GetFileNameWithoutExtension(x.FileName);
			string fileNameY = Path.GetFileNameWithoutExtension(y.FileName);
			((Control)x).set_Visible(fileNameX.Contains(((TextInputBase)base.get_View().SearchBox).get_Text()));
			((Control)y).set_Visible(fileNameY.Contains(((TextInputBase)base.get_View().SearchBox).get_Text()));
			if (!((Control)x).get_Visible() || !((Control)y).get_Visible())
			{
				return string.Compare(fileNameX, fileNameY, StringComparison.InvariantCultureIgnoreCase);
			}
			return y.IsFavorite.CompareTo(x.IsFavorite);
		}
	}
}
