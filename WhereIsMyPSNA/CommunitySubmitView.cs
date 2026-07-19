using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhereIsMyPSNA
{
	internal class CommunitySubmitView : View
	{
		private const int RowHeight = 92;

		private const int RowSpacing = 8;

		private const int HeaderH = 28;

		private const int ButtonWidth = 90;

		private const int FlyoutRowH = 22;

		private const int MaxVisibleFlyoutRows = 10;

		private const float FadeDuration = 0.35f;

		private static readonly RecipeDef[] AllRecipes = (from d in RecipeDefs.ByRecipeSheetId.Values.Distinct()
			orderby d.SheetName
			select d).ToArray();

		private readonly CommunitySubmissionService _submissionService;

		private readonly int _contentWidth;

		private readonly Action<CommunitySubmitView> _registerActive;

		private readonly Action<CommunitySubmitView> _unregisterActive;

		private readonly Panel[] _rowPanels = (Panel[])(object)new Panel[6];

		private readonly LockableTextBox[] _searchBoxes = new LockableTextBox[6];

		private readonly StandardButton[] _actionButtons = (StandardButton[])(object)new StandardButton[6];

		private readonly RecipeDef[] _selectedRecipe = new RecipeDef[6];

		private readonly string[] _npcNames = new string[6];

		private readonly bool[] _isLocked = new bool[6];

		private Label _headerLabel;

		private LoadingSpinner _centerSpinner;

		private Label _statusLabel;

		private bool _isBuilt;

		private float _fadeOpacity;

		private bool _fadeActive;

		private Panel _flyout;

		private Label[] _flyoutRows;

		private RecipeDef[] _currentMatches = Array.Empty<RecipeDef>();

		private int _activeRow = -1;

		private bool _suppressTextChanged;

		public CommunitySubmitView(CommunitySubmissionService submissionService, int contentWidth, Action<CommunitySubmitView> registerActive, Action<CommunitySubmitView> unregisterActive)
			: this()
		{
			_submissionService = submissionService;
			_contentWidth = contentWidth;
			_registerActive = registerActive;
			_unregisterActive = unregisterActive;
		}

		protected override void Build(Container buildPanel)
		{
			if (_submissionService.DisclaimerAcknowledged)
			{
				BuildMainUi(buildPanel);
			}
			else
			{
				BuildDisclaimer(buildPanel);
			}
		}

		private void BuildDisclaimer(Container buildPanel)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text(Strings.Get("Community_Disclaimer"));
			((Control)val).set_Location(new Point(0, 260));
			((Control)val).set_Size(new Point(_contentWidth, 40));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Color.get_White());
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label label = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text(Strings.Get("IUnderstand"));
			((Control)val2).set_Size(new Point(160, 32));
			((Control)val2).set_Location(new Point(_contentWidth / 2 - 80, 310));
			StandardButton button = val2;
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_submissionService.AcknowledgeDisclaimer();
				((Control)label).Dispose();
				((Control)button).Dispose();
				BuildMainUi(buildPanel);
			});
		}

		private void BuildMainUi(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Expected O, but got Unknown
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text(Strings.Get("Community_Header"));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(_contentWidth, 28));
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_LightGray());
			((Control)val).set_Visible(false);
			_headerLabel = val;
			PsnaSchedule.AgentLocation[] locations = PsnaSchedule.GetTodaysLocations();
			int rowsStartY = 36;
			int y = rowsStartY;
			for (int i = 0; i < 6; i++)
			{
				_npcNames[i] = locations[i].Npc;
				Panel[] rowPanels = _rowPanels;
				int num = i;
				Panel val2 = new Panel();
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_Location(new Point(0, y));
				((Control)val2).set_Size(new Point(_contentWidth, 92));
				val2.set_ShowBorder(true);
				val2.set_Title(locations[i].NpcDisplay);
				((Control)val2).set_Visible(false);
				rowPanels[num] = val2;
				Panel row = _rowPanels[i];
				int searchWidth = _contentWidth - 90 - 12 - 12 - 12;
				LockableTextBox[] searchBoxes = _searchBoxes;
				int num2 = i;
				LockableTextBox lockableTextBox = new LockableTextBox();
				((Control)lockableTextBox).set_Parent((Container)(object)row);
				((Control)lockableTextBox).set_Location(new Point(12, 14));
				((Control)lockableTextBox).set_Size(new Point(searchWidth, 28));
				((TextInputBase)lockableTextBox).set_PlaceholderText(Strings.Get("SearchItemPlaceholder"));
				searchBoxes[num2] = lockableTextBox;
				StandardButton[] actionButtons = _actionButtons;
				int num3 = i;
				StandardButton val3 = new StandardButton();
				((Control)val3).set_Parent((Container)(object)row);
				val3.set_Text(Strings.Get("Submit"));
				((Control)val3).set_Location(new Point(12 + searchWidth + 12, 14));
				((Control)val3).set_Size(new Point(90, 28));
				((Control)val3).set_Enabled(false);
				actionButtons[num3] = val3;
				int rowIndex = i;
				((TextInputBase)_searchBoxes[i]).add_TextChanged((EventHandler<EventArgs>)delegate
				{
					OnSearchTextChanged(rowIndex);
				});
				((Control)_actionButtons[i]).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnActionButtonClicked(rowIndex);
				});
				y += 100;
			}
			int rowsTotalHeight = 592;
			int spinnerY = rowsStartY + rowsTotalHeight / 2 - 24;
			LoadingSpinner val4 = new LoadingSpinner();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Size(new Point(48, 48));
			((Control)val4).set_Location(new Point(_contentWidth / 2 - 24, spinnerY));
			((Control)val4).set_Visible(false);
			_centerSpinner = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent(buildPanel);
			val5.set_Text(Strings.Get("Community_LoadingExisting"));
			((Control)val5).set_Location(new Point(0, spinnerY + 48 + 8));
			((Control)val5).set_Size(new Point(_contentWidth, 20));
			val5.set_Font(GameService.Content.get_DefaultFont14());
			val5.set_TextColor(Color.get_LightGray());
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val5).set_Visible(false);
			_statusLabel = val5;
			_isBuilt = true;
			_registerActive?.Invoke(this);
			BuildFlyout();
			((Control)_centerSpinner).set_Visible(true);
			((Control)_statusLabel).set_Visible(true);
			LoadExistingSubmissionsAsync();
		}

		private async Task LoadExistingSubmissionsAsync()
		{
			Dictionary<string, int> existing = await _submissionService.FetchCurrentSubmissionsAsync();
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				if (_isBuilt)
				{
					ApplyExistingSubmissions(existing, animate: true);
				}
			});
		}

		private void ApplyExistingSubmissions(Dictionary<string, int> existing, bool animate)
		{
			((Control)_centerSpinner).set_Visible(false);
			((Control)_statusLabel).set_Visible(false);
			((Control)_headerLabel).set_Visible(true);
			_suppressTextChanged = true;
			for (int i = 0; i < 6; i++)
			{
				((Control)_rowPanels[i]).set_Visible(true);
				if (_submissionService.TryGetShortName(_npcNames[i], out var shortNpc) && existing.TryGetValue(shortNpc, out var itemId) && RecipeDefs.ByRecipeSheetId.TryGetValue(itemId, out var def))
				{
					_isLocked[i] = true;
					((TextInputBase)_searchBoxes[i]).set_Text(def.SheetName);
					((Control)_searchBoxes[i]).set_Enabled(false);
					_searchBoxes[i].Locked = true;
					_actionButtons[i].set_Text(Strings.Get("Edit"));
					((Control)_actionButtons[i]).set_Enabled(true);
				}
				else
				{
					_isLocked[i] = false;
					((Control)_searchBoxes[i]).set_Enabled(true);
					_searchBoxes[i].Locked = false;
					_actionButtons[i].set_Text(Strings.Get("Submit"));
					((Control)_actionButtons[i]).set_Enabled(false);
				}
			}
			_suppressTextChanged = false;
			if (animate)
			{
				StartFadeIn();
			}
			else
			{
				SetUiOpacity(1f);
			}
		}

		private void OnActionButtonClicked(int rowIndex)
		{
			if (_isLocked[rowIndex])
			{
				_isLocked[rowIndex] = false;
				((Control)_searchBoxes[rowIndex]).set_Enabled(true);
				_searchBoxes[rowIndex].Locked = false;
				_selectedRecipe[rowIndex] = null;
				_actionButtons[rowIndex].set_Text(Strings.Get("Submit"));
				((Control)_actionButtons[rowIndex]).set_Enabled(false);
			}
			else
			{
				SubmitRow(rowIndex);
			}
		}

		private void BuildFlyout()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Size(new Point(300, 228));
			val.set_ShowBorder(true);
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 230));
			val.set_CanScroll(true);
			((Control)val).set_Visible(false);
			((Control)val).set_ZIndex(9999);
			_flyout = val;
			_flyoutRows = (Label[])(object)new Label[AllRecipes.Length];
			for (int i = 0; i < AllRecipes.Length; i++)
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_flyout);
				((Control)val2).set_Location(new Point(6, 4 + i * 22));
				((Control)val2).set_Size(new Point(280, 22));
				val2.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val2).set_Visible(false);
				Label label = val2;
				int slotIndex = i;
				((Control)label).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					label.set_TextColor(new Color(255, 228, 181));
				});
				((Control)label).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					label.set_TextColor(RowColor(slotIndex));
				});
				((Control)label).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnResultClicked(slotIndex);
				});
				_flyoutRows[i] = label;
			}
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalMousePressed);
		}

		private void OnGlobalMousePressed(object sender, MouseEventArgs e)
		{
			if (_flyout != null && ((Control)_flyout).get_Visible() && !((Control)_flyout).get_MouseOver() && (_activeRow < 0 || !((Control)_searchBoxes[_activeRow]).get_MouseOver()))
			{
				HideFlyout();
			}
		}

		private void OnSearchTextChanged(int rowIndex)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			if (_suppressTextChanged)
			{
				return;
			}
			_selectedRecipe[rowIndex] = null;
			((Control)_actionButtons[rowIndex]).set_Enabled(false);
			string query = ((TextInputBase)_searchBoxes[rowIndex]).get_Text()?.Trim() ?? "";
			if (query.Length == 0)
			{
				HideFlyout();
				return;
			}
			RecipeDef[] matches = AllRecipes.Where((RecipeDef d) => d.SheetName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 || d.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
			if (matches.Length == 0)
			{
				HideFlyout();
				return;
			}
			_activeRow = rowIndex;
			_currentMatches = matches;
			for (int i = 0; i < _flyoutRows.Length; i++)
			{
				if (i < matches.Length)
				{
					_flyoutRows[i].set_Text(matches[i].SheetName);
					_flyoutRows[i].set_TextColor(RowColor(i));
					((Control)_flyoutRows[i]).set_Visible(true);
				}
				else
				{
					((Control)_flyoutRows[i]).set_Visible(false);
				}
			}
			((Container)_flyout).set_VerticalScrollOffset(0);
			LockableTextBox box = _searchBoxes[rowIndex];
			int flyoutHeight = Math.Min(10, Math.Max(matches.Length, 4)) * 22 + 8;
			((Control)_flyout).set_Size(new Point(((Control)_flyout).get_Size().X, flyoutHeight));
			int screenHeight = ((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			Rectangle absoluteBounds = ((Control)box).get_AbsoluteBounds();
			int num;
			if (((Rectangle)(ref absoluteBounds)).get_Bottom() + flyoutHeight > screenHeight)
			{
				absoluteBounds = ((Control)box).get_AbsoluteBounds();
				num = ((Rectangle)(ref absoluteBounds)).get_Top() - flyoutHeight;
			}
			else
			{
				absoluteBounds = ((Control)box).get_AbsoluteBounds();
				num = ((Rectangle)(ref absoluteBounds)).get_Bottom();
			}
			int flyoutY = num;
			Panel flyout = _flyout;
			absoluteBounds = ((Control)box).get_AbsoluteBounds();
			((Control)flyout).set_Location(new Point(((Rectangle)(ref absoluteBounds)).get_Left(), flyoutY));
			((Control)_flyout).set_Visible(true);
		}

		private void OnResultClicked(int slotIndex)
		{
			if (_activeRow >= 0 && slotIndex < _currentMatches.Length)
			{
				RecipeDef def = _currentMatches[slotIndex];
				int rowIndex = _activeRow;
				_suppressTextChanged = true;
				((TextInputBase)_searchBoxes[rowIndex]).set_Text(def.SheetName);
				_suppressTextChanged = false;
				_selectedRecipe[rowIndex] = def;
				((Control)_actionButtons[rowIndex]).set_Enabled(true);
				HideFlyout();
			}
		}

		private void HideFlyout()
		{
			if (_flyout != null)
			{
				((Control)_flyout).set_Visible(false);
			}
		}

		private Color RowColor(int slotIndex)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (slotIndex >= _currentMatches.Length)
			{
				return Color.get_White();
			}
			return RecipeTooltip.GetRarityColor(_currentMatches[slotIndex].Rarity);
		}

		public void OnWindowHidden()
		{
			HideFlyout();
		}

		private async void SubmitRow(int rowIndex)
		{
			RecipeDef def = _selectedRecipe[rowIndex];
			if (def == null)
			{
				return;
			}
			if (!_submissionService.IsConfigured)
			{
				ScreenNotification.ShowNotification(Strings.Get("Community_NotConfigured"), (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			_isLocked[rowIndex] = true;
			((Control)_searchBoxes[rowIndex]).set_Enabled(false);
			_searchBoxes[rowIndex].Locked = true;
			_actionButtons[rowIndex].set_Text(Strings.Get("Edit"));
			if (!(await _submissionService.SubmitAsync(_npcNames[rowIndex], def.RecipeSheetIds[0])))
			{
				ScreenNotification.ShowNotification(Strings.Get("Community_SubmitFailed"), (NotificationType)0, (Texture2D)null, 4);
			}
		}

		public void Tick(GameTime gameTime)
		{
			if (_isBuilt && _fadeActive)
			{
				_fadeOpacity += (float)(gameTime.get_ElapsedGameTime().TotalSeconds / 0.3499999940395355);
				if (_fadeOpacity >= 1f)
				{
					_fadeOpacity = 1f;
					_fadeActive = false;
				}
				SetUiOpacity(_fadeOpacity);
			}
		}

		private void SetUiOpacity(float opacity)
		{
			for (int i = 0; i < 6; i++)
			{
				((Control)_rowPanels[i]).set_Opacity(opacity);
				((Control)_searchBoxes[i]).set_Opacity(opacity);
				((Control)_actionButtons[i]).set_Opacity(opacity);
			}
		}

		private void StartFadeIn()
		{
			_fadeOpacity = 0f;
			_fadeActive = true;
			SetUiOpacity(0f);
		}

		protected override void Unload()
		{
			_isBuilt = false;
			_unregisterActive?.Invoke(this);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalMousePressed);
			Panel flyout = _flyout;
			if (flyout != null)
			{
				((Control)flyout).Dispose();
			}
		}
	}
}
