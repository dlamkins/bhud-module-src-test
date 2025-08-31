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
			base.Model.FileWatcherFactory.FileAdded += OnScreenShotAdded;
			base.Model.FileWatcherFactory.FileDeleted += OnScreenShotDeleted;
			base.Model.FileWatcherFactory.FileRenamed += OnScreenShotRenamed;
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
			CreateThumbnail(base.View.ThumbnailFlowPanel, texture, e.Value);
			await LoadTexture(texture, e.Value);
		}

		public void OnScreenShotDeleted(object o, ValueEventArgs<string> e)
		{
			if (FindThumbnailByFileName(e.Value, out var ctrl))
			{
				base.View.ThumbnailFlowPanel.RemoveChild(ctrl);
				ctrl?.Dispose();
			}
		}

		public void OnScreenShotRenamed(object o, ValueChangedEventArgs<string> e)
		{
			if (FindThumbnailByFileName(e.PreviousValue, out var ctrl))
			{
				ctrl.FileName = e.NewValue;
				ctrl.NameTextBox.Text = Path.GetFileNameWithoutExtension(e.NewValue);
			}
		}

		private bool FindThumbnailByFileName(string fileName, out ResponsiveThumbnail thumbnail)
		{
			thumbnail = base.View.ThumbnailFlowPanel.Children.Where((Control x) => x.GetType() == typeof(ResponsiveThumbnail)).Cast<ResponsiveThumbnail>().FirstOrDefault((ResponsiveThumbnail y) => fileName.Equals(y.FileName));
			return thumbnail != null;
		}

		public ThumbnailBase CreateThumbnail(FlowPanel parent, AsyncTexture2D texture, string fileName)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			ResponsiveThumbnail responsiveThumbnail = new ResponsiveThumbnail(texture, fileName);
			responsiveThumbnail.Parent = parent;
			responsiveThumbnail.Size = new Point(parent.Width / 4 - (int)parent.ControlPadding.X, 144);
			responsiveThumbnail.IsFavorite = ScreenshotManagerModule.ModuleInstance.Favorites.Value.Any((string x) => x.Equals(Path.GetFileName(fileName)));
			responsiveThumbnail.OnDelete += OnClickDelete;
			responsiveThumbnail.FavoriteChanged += OnFavoriteChanged;
			responsiveThumbnail.OnInspect += OnClickInspect;
			return responsiveThumbnail;
		}

		private void OnClickDelete(object o, EventArgs e)
		{
			ThumbnailBase ctrl = (ThumbnailBase)o;
			if (ScreenshotManagerModule.ModuleInstance.SendToRecycleBin.Value)
			{
				DoDelete(ctrl, sendToRecycleBin: true);
				return;
			}
			ConfirmationPrompt.ShowPrompt(delegate(bool confirmed)
			{
				if (confirmed)
				{
					DoDelete(ctrl, sendToRecycleBin: false);
				}
			}, string.Format(Resources.You_are_about_to_permanently_destroy__0__, "“" + Path.GetFileNameWithoutExtension(ctrl.FileName) + "”") + "\n" + Resources.Are_you_sure_, Resources.Yes, Resources.Cancel);
		}

		private async void OnClickInspect(object o, EventArgs e)
		{
			ResponsiveThumbnail ctrl = (ResponsiveThumbnail)o;
			await base.Model.FileWatcherFactory.CreateInspectionPanel(ctrl.FileName);
		}

		private async void DoDelete(ThumbnailBase ctrl, bool sendToRecycleBin)
		{
			if (!(await FileUtil.DeleteAsync(ctrl.FileName, sendToRecycleBin)))
			{
				ScreenNotification.ShowNotification(string.Format(Resources.Failed_to_delete_image__0__, "“" + Path.GetFileNameWithoutExtension(ctrl.FileName) + "”"), ScreenNotification.NotificationType.Error);
				GameService.Content.PlaySoundEffectByName("error");
				return;
			}
			ScreenshotManagerModule.ModuleInstance.DeleteSfx.Play(GameService.GameIntegration.Audio.Volume, 0f, 0f);
			base.View.ThumbnailFlowPanel.RemoveChild(ctrl);
			ctrl.Dispose();
			base.View.ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>(SortThumbnails);
		}

		private void OnFavoriteChanged(object o, EventArgs e)
		{
			base.View.ThumbnailFlowPanel.SortChildren<ResponsiveThumbnail>(SortThumbnails);
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
			base.Model.FileWatcherFactory.FileAdded -= OnScreenShotAdded;
			base.Model.FileWatcherFactory.FileDeleted -= OnScreenShotDeleted;
			base.Model.FileWatcherFactory.FileRenamed -= OnScreenShotRenamed;
			base.Model.Dispose();
			IEnumerable<string> favorites = from ResponsiveThumbnail x in base.View.ThumbnailFlowPanel.Children.Where((Control x) => x.GetType() == typeof(ResponsiveThumbnail))
				where x.IsFavorite
				select x into y
				select Path.GetFileName(y.FileName);
			ScreenshotManagerModule.ModuleInstance.Favorites.Value = favorites.ToList();
		}

		public int SortThumbnails(ResponsiveThumbnail x, ResponsiveThumbnail y)
		{
			string fileNameX = Path.GetFileNameWithoutExtension(x.FileName);
			string fileNameY = Path.GetFileNameWithoutExtension(y.FileName);
			x.Visible = fileNameX.Contains(base.View.SearchBox.Text);
			y.Visible = fileNameY.Contains(base.View.SearchBox.Text);
			if (x.IsFavorite && !y.IsFavorite)
			{
				return -1;
			}
			if (!x.IsFavorite && y.IsFavorite)
			{
				return 1;
			}
			if (x.Visible && !y.Visible)
			{
				return -1;
			}
			if (!x.Visible && y.Visible)
			{
				return 1;
			}
			return string.Compare(fileNameX, fileNameY, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
