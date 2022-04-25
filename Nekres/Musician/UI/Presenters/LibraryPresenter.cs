using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician.Controls;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Views;

namespace Nekres.Musician.UI.Presenters
{
	internal class LibraryPresenter : Presenter<LibraryView, LibraryModel>
	{
		public LibraryPresenter(LibraryView view, LibraryModel model)
			: base(view, model)
		{
			view.OnSelectedSortChanged += View_SelectedSortChanged;
			view.OnImportFromClipboardClick += View_ImportFromClipboardClicked;
			model.MusicSheetService.OnSheetUpdated += OnSheetUpdated;
		}

		protected override void Unload()
		{
			base.get_View().OnSelectedSortChanged -= View_SelectedSortChanged;
			base.get_View().OnImportFromClipboardClick -= View_ImportFromClipboardClicked;
			base.get_Model().MusicSheetService.OnSheetUpdated -= OnSheetUpdated;
			base.Unload();
		}

		private void OnSheetUpdated(object o, ValueEventArgs<MusicSheetModel> e)
		{
			if (!TryGetSheetButtonById(e.get_Value().Id, out var button))
			{
				base.get_View().CreateSheetButton(e.get_Value());
				return;
			}
			button.Artist = e.get_Value().Artist;
			((Panel)button).set_Title(e.get_Value().Title);
			button.User = e.get_Value().User;
		}

		private bool TryGetSheetButtonById(Guid id, out SheetButton button)
		{
			button = ((IEnumerable<Control>)((Container)base.get_View().MelodyFlowPanel).get_Children()).Where((Control x) => ((object)x).GetType() == typeof(SheetButton)).Cast<SheetButton>().FirstOrDefault((SheetButton y) => y.Id.Equals(id));
			if (button == null)
			{
				return false;
			}
			return true;
		}

		private void View_SelectedSortChanged(object o, ValueEventArgs<string> e)
		{
			FlowPanel melodyFlowPanel = base.get_View().MelodyFlowPanel;
			if (melodyFlowPanel == null)
			{
				return;
			}
			melodyFlowPanel.SortChildren<SheetButton>((Comparison<SheetButton>)delegate(SheetButton x, SheetButton y)
			{
				Instrument result;
				bool flag = Enum.TryParse<Instrument>(e.get_Value(), ignoreCase: true, out result);
				((Control)x).set_Visible(!flag || x.Instrument.ToString().Equals(e.get_Value(), StringComparison.InvariantCultureIgnoreCase));
				((Control)y).set_Visible(!flag || y.Instrument.ToString().Equals(e.get_Value(), StringComparison.InvariantCultureIgnoreCase));
				if (!((Control)x).get_Visible() || !((Control)y).get_Visible())
				{
					return 0;
				}
				if (base.get_Model().DD_TITLE.Equals(e.get_Value()))
				{
					return string.Compare(((Panel)x).get_Title(), ((Panel)y).get_Title(), StringComparison.InvariantCulture);
				}
				if (base.get_Model().DD_ARTIST.Equals(e.get_Value()))
				{
					return string.Compare(x.Artist, y.Artist, StringComparison.InvariantCulture);
				}
				return base.get_Model().DD_USER.Equals(e.get_Value()) ? string.Compare(x.User, y.User, StringComparison.InvariantCulture) : 0;
			});
		}

		private async void View_ImportFromClipboardClicked(object o, EventArgs e)
		{
			if (!MusicSheet.TryParseXml(await ClipboardUtil.get_WindowsClipboardService().GetTextAsync(), out var sheet))
			{
				GameService.Content.PlaySoundEffectByName("error");
				ScreenNotification.ShowNotification("Your clipboard does not contain a valid music sheet.", (NotificationType)2, (Texture2D)null, 4);
			}
			else
			{
				await MusicianModule.ModuleInstance.MusicSheetService.AddOrUpdate(sheet);
				base.get_View().CreateSheetButton(sheet.ToModel());
			}
		}
	}
}
