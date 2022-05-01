using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Universal_Search_Module.Controls.SearchResultItems;
using Universal_Search_Module.Services.SearchHandler;

namespace Universal_Search_Module.Controls
{
	public class SearchWindow : WindowBase2
	{
		private const int WINDOW_WIDTH = 512;

		private const int WINDOW_HEIGHT = 178;

		private const int TITLEBAR_HEIGHT = 32;

		private const int DROPDOWN_WIDTH = 100;

		private readonly IDictionary<string, SearchHandler> _searchHandlers;

		private static Texture2D _textureWindowBackground;

		private List<SearchResultItem> _results;

		private SearchHandler _selectedSearchHandler;

		private TextBox _searchbox;

		private LoadingSpinner _spinner;

		private Label _noneLabel;

		private Dropdown _searchHandlerSelect;

		private Task _delayTask;

		private CancellationTokenSource _delayCancellationToken;

		private readonly SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1, 1);

		static SearchWindow()
		{
			_textureWindowBackground = UniversalSearchModule.ModuleInstance.ContentsManager.GetTexture("textures\\156390.png");
		}

		public SearchWindow(IEnumerable<SearchHandler> searchHandlers)
			: this()
		{
			_searchHandlers = searchHandlers.ToDictionary((SearchHandler x) => x.Name, (SearchHandler y) => y);
			_selectedSearchHandler = _searchHandlers.First().Value;
			BuildWindow();
			BuildContents();
		}

		private void BuildWindow()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Chat Code Searchs");
			((WindowBase2)this).ConstructWindow(_textureWindowBackground, new Rectangle(0, 0, 512, 178), new Rectangle(0, 32, 512, 146));
		}

		private void BuildContents()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Size(new Point(100, ((DesignStandard)(ref Dropdown.Standard)).get_Size().Y));
			val.set_SelectedItem(_selectedSearchHandler.Name);
			((Control)val).set_Parent((Container)(object)this);
			_searchHandlerSelect = val;
			foreach (KeyValuePair<string, SearchHandler> searchHandler in _searchHandlers)
			{
				_searchHandlerSelect.get_Items().Add(searchHandler.Key);
			}
			_searchHandlerSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)SearchHandlerSelectValueChanged);
			TextBox val2 = new TextBox();
			((Control)val2).set_Location(new Point(100, 0));
			((Control)val2).set_Size(new Point(((Control)this)._size.X - 100, ((DesignStandard)(ref TextBox.Standard)).get_Size().Y));
			((TextInputBase)val2).set_PlaceholderText("Search");
			((Control)val2).set_Parent((Container)(object)this);
			_searchbox = val2;
			LoadingSpinner val3 = new LoadingSpinner();
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val3).set_Location(((Rectangle)(ref contentRegion)).get_Size() / new Point(2) - new Point(32, 32));
			((Control)val3).set_Visible(false);
			((Control)val3).set_Parent((Container)(object)this);
			_spinner = val3;
			Label val4 = new Label();
			contentRegion = ((Container)this).get_ContentRegion();
			((Control)val4).set_Size(((Rectangle)(ref contentRegion)).get_Size() - new Point(0, ((DesignStandard)(ref TextBox.Standard)).get_Size().Y * 2));
			((Control)val4).set_Location(new Point(0, ((DesignStandard)(ref TextBox.Standard)).get_Size().Y));
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val4).set_Visible(false);
			val4.set_Text("No Results");
			((Control)val4).set_Parent((Container)(object)this);
			_noneLabel = val4;
			_results = new List<SearchResultItem>(3);
			((TextInputBase)_searchbox).add_TextChanged((EventHandler<EventArgs>)SearchboxOnTextChanged);
		}

		private void SearchHandlerSelectValueChanged(object sender, ValueChangedEventArgs e)
		{
			_selectedSearchHandler = _searchHandlers[e.get_CurrentValue()];
			Search();
		}

		private void AddSearchResultItems(IEnumerable<SearchResultItem> items)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int lastResultBottom = ((Control)_searchbox).get_Bottom();
			foreach (SearchResultItem searchItem in items)
			{
				((Control)searchItem).set_Width(((Control)this)._size.X - 4);
				((Control)searchItem).set_Parent((Container)(object)this);
				((Control)searchItem).set_Location(new Point(2, lastResultBottom + 3));
				lastResultBottom = ((Control)searchItem).get_Bottom();
				_results.Add(searchItem);
			}
		}

		private bool HandlePrefix(string searchText)
		{
			if (searchText.Length > 1 && searchText.Length <= 2 && searchText.EndsWith(" "))
			{
				searchText = searchText.Replace(" ", string.Empty);
				foreach (KeyValuePair<string, SearchHandler> possibleSearchHandler in _searchHandlers)
				{
					if (possibleSearchHandler.Value.Prefix.Equals(searchText, StringComparison.OrdinalIgnoreCase))
					{
						_searchHandlerSelect.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)SearchHandlerSelectValueChanged);
						_searchHandlerSelect.set_SelectedItem(possibleSearchHandler.Value.Name);
						_selectedSearchHandler = possibleSearchHandler.Value;
						_searchHandlerSelect.add_ValueChanged((EventHandler<ValueChangedEventArgs>)SearchHandlerSelectValueChanged);
						((TextInputBase)_searchbox).set_Text(string.Empty);
						return false;
					}
				}
			}
			return true;
		}

		private async Task SearchAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await _searchSemaphore.WaitAsync(cancellationToken);
			try
			{
				_results.ForEach(delegate(SearchResultItem r)
				{
					((Control)r).Dispose();
				});
				_results.Clear();
				cancellationToken.ThrowIfCancellationRequested();
				string searchText = ((TextInputBase)_searchbox).get_Text();
				if (!HandlePrefix(searchText) || searchText.Length <= 2)
				{
					((Control)_noneLabel).Show();
					return;
				}
				((Control)_noneLabel).Hide();
				((Control)_spinner).Show();
				AddSearchResultItems(_selectedSearchHandler.Search(searchText));
				((Control)_spinner).Hide();
				if (!_results.Any())
				{
					((Control)_noneLabel).Show();
				}
			}
			finally
			{
				_searchSemaphore.Release();
			}
		}

		private void SearchboxOnTextChanged(object sender, EventArgs e)
		{
			Search();
		}

		private void Search()
		{
			try
			{
				if (HandlePrefix(((TextInputBase)_searchbox).get_Text()))
				{
					if (_delayTask != null)
					{
						_delayCancellationToken.Cancel();
						_delayTask = null;
						_delayCancellationToken = null;
					}
					_delayCancellationToken = new CancellationTokenSource();
					_delayTask = new Task(async delegate
					{
						await DelaySeach(_delayCancellationToken.Token);
					}, _delayCancellationToken.Token);
					_delayTask.Start();
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async Task DelaySeach(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				await Task.Delay(300, cancellationToken);
				await SearchAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureWindowBackground, bounds);
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
