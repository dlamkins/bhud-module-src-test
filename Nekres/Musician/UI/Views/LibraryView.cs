using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Nekres.Musician.Controls;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Controls;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Presenters;

namespace Nekres.Musician.UI.Views
{
	internal class LibraryView : View<LibraryPresenter>
	{
		private const int TOP_MARGIN = 0;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const int LEFT_MARGIN = 8;

		private string _activeFilter;

		private IEnumerable<MusicSheetModel> _initialSheets;

		public FlowPanel MelodyFlowPanel { get; private set; }

		internal event EventHandler<ValueEventArgs<string>> OnSelectedSortChanged;

		internal event EventHandler<EventArgs> OnImportFromClipboardClick;

		public LibraryView(LibraryModel model)
		{
			base.WithPresenter(new LibraryPresenter(this, model));
			_activeFilter = MusicianModule.ModuleInstance.SheetFilter.get_Value();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			return await Task.Run(async delegate
			{
				progress.Report(MusicianModule.ModuleInstance.MusicSheetImporter.Log);
				IEnumerable<MusicSheetModel> initialSheets = await MusicianModule.ModuleInstance.MusicSheetService.GetAll();
				_initialSheets = initialSheets;
				return !MusicianModule.ModuleInstance.MusicSheetImporter.IsLoading;
			});
		}

		protected override void Unload()
		{
			base.Unload();
		}

		protected override async void Build(Container buildPanel)
		{
			TextBox val = new TextBox();
			((Control)val).set_Parent(buildPanel);
			((TextInputBase)val).set_MaxLength(256);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(150, 32));
			((TextInputBase)val).set_PlaceholderText("Search...");
			((TextInputBase)val).add_TextChanged((EventHandler<EventArgs>)OnSearchFilterChanged);
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(buildPanel.get_ContentRegion().Width - 5 - 150, 0));
			((Control)val2).set_Width(150);
			Dropdown ddSortMethod = val2;
			ddSortMethod.get_Items().Add(((Presenter<LibraryView, LibraryModel>)base.get_Presenter()).get_Model().DD_TITLE);
			ddSortMethod.get_Items().Add(((Presenter<LibraryView, LibraryModel>)base.get_Presenter()).get_Model().DD_ARTIST);
			ddSortMethod.get_Items().Add(((Presenter<LibraryView, LibraryModel>)base.get_Presenter()).get_Model().DD_USER);
			ddSortMethod.get_Items().Add("------------------");
			string[] names = Enum.GetNames(typeof(Instrument));
			foreach (string instrument in names)
			{
				ddSortMethod.get_Items().Add(instrument);
			}
			ddSortMethod.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnSortChanged);
			OnSortChanged(ddSortMethod, new ValueChangedEventArgs(string.Empty, ddSortMethod.get_SelectedItem()));
			LibraryView libraryView = this;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(0, ((Control)ddSortMethod).get_Bottom() + 10));
			((Control)val3).set_Size(new Point(buildPanel.get_ContentRegion().Width - 10, buildPanel.get_ContentRegion().Height - 150));
			val3.set_FlowDirection((ControlFlowDirection)0);
			val3.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val3).set_CanCollapse(false);
			((Panel)val3).set_CanScroll(true);
			((Panel)val3).set_Collapsed(false);
			((Panel)val3).set_ShowTint(true);
			((Panel)val3).set_ShowBorder(true);
			libraryView.MelodyFlowPanel = val3;
			foreach (MusicSheetModel sheet in _initialSheets)
			{
				CreateSheetButton(sheet);
			}
			ClipboardButton clipboardButton = new ClipboardButton();
			((Control)clipboardButton).set_Parent(buildPanel);
			((Control)clipboardButton).set_Size(new Point(42, 42));
			((Control)clipboardButton).set_Location(new Point((buildPanel.get_ContentRegion().Width - 42) / 2, ((Control)MelodyFlowPanel).get_Bottom() + 10));
			((Control)clipboardButton).set_BasicTooltipText("Import XML from Clipboard");
			((Control)clipboardButton).add_Click((EventHandler<MouseEventArgs>)OnImportFromClipboardBtnClick);
			ddSortMethod.set_SelectedItem(_activeFilter);
		}

		public void CreateSheetButton(MusicSheetModel model)
		{
			SheetButton sheetButton = new SheetButton(model);
			((Control)sheetButton).set_Parent((Container)(object)MelodyFlowPanel);
			sheetButton.OnPreviewClick += OnPreviewClick;
			sheetButton.OnEmulateClick += OnEmulateClick;
			sheetButton.OnDelete += OnDeleteClick;
		}

		private void OnSortChanged(object o, ValueChangedEventArgs e)
		{
			MusicianModule.ModuleInstance.SheetFilter.set_Value(e.get_CurrentValue());
			this.OnSelectedSortChanged?.Invoke(o, new ValueEventArgs<string>(e.get_CurrentValue()));
		}

		private async void OnPreviewClick(object o, ValueEventArgs<bool> e)
		{
			SheetButton sheetBtn = (SheetButton)o;
			if (e.get_Value())
			{
				MusicSheetModel sheet = await MusicianModule.ModuleInstance.MusicSheetService.GetById(sheetBtn.Id);
				await MusicianModule.ModuleInstance.MusicPlayer.PlayPreview(MusicSheet.FromModel(sheet));
			}
			else
			{
				MusicianModule.ModuleInstance.MusicPlayer.Stop();
			}
		}

		private async void OnEmulateClick(object o, EventArgs e)
		{
			SheetButton sheetBtn = (SheetButton)o;
			MusicSheetModel sheet = await MusicianModule.ModuleInstance.MusicSheetService.GetById(sheetBtn.Id);
			MusicianModule.ModuleInstance.MusicPlayer.PlayEmulate(MusicSheet.FromModel(sheet));
		}

		private async void OnDeleteClick(object o, ValueEventArgs<Guid> e)
		{
			await MusicianModule.ModuleInstance.MusicSheetService.Delete(e.get_Value());
		}

		private void OnImportFromClipboardBtnClick(object o, MouseEventArgs e)
		{
			this.OnImportFromClipboardClick?.Invoke(this, EventArgs.Empty);
		}

		private void OnSearchFilterChanged(object o, EventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			string text = ((TextInputBase)(TextBox)o).get_Text();
			text = (string.IsNullOrEmpty(text) ? text : text.ToLowerInvariant());
			MelodyFlowPanel.SortChildren<SheetButton>((Comparison<SheetButton>)delegate(SheetButton x, SheetButton y)
			{
				((Control)x).set_Visible(string.IsNullOrEmpty(text) || (((Panel)x).get_Title() + " - " + x.Artist).ToLowerInvariant().Contains(text) || x.User.ToLowerInvariant().Contains(text));
				((Control)y).set_Visible(string.IsNullOrEmpty(text) || (((Panel)y).get_Title() + " - " + y.Artist).ToLowerInvariant().Contains(text) || y.User.ToLowerInvariant().Contains(text));
				if (!((Control)x).get_Visible() || !((Control)y).get_Visible())
				{
					return 0;
				}
				if (MusicianModule.ModuleInstance.SheetFilter.get_Value().Equals(((Presenter<LibraryView, LibraryModel>)base.get_Presenter()).get_Model().DD_ARTIST))
				{
					return string.Compare(x.Artist, y.Artist, StringComparison.InvariantCultureIgnoreCase);
				}
				return MusicianModule.ModuleInstance.SheetFilter.get_Value().Equals(((Presenter<LibraryView, LibraryModel>)base.get_Presenter()).get_Model().DD_TITLE) ? string.Compare(((Panel)x).get_Title(), ((Panel)y).get_Title(), StringComparison.InvariantCultureIgnoreCase) : string.Compare(x.User, y.User, StringComparison.InvariantCultureIgnoreCase);
			});
		}
	}
}
