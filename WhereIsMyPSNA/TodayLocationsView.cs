using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhereIsMyPSNA
{
	internal class TodayLocationsView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<TodayLocationsView>();

		private const int RowHeight = 102;

		private const int RowSpacing = 0;

		private const int IconSize = 42;

		private const int KarmaIconSize = 24;

		private const int TitleOffset = 26;

		private readonly Panel[] _rowPanels = (Panel[])(object)new Panel[6];

		private readonly Image[] _recipeIcons = (Image[])(object)new Image[6];

		private readonly Label[] _recipeLabels = (Label[])(object)new Label[6];

		private readonly Image[] _karmaIcons = (Image[])(object)new Image[6];

		private readonly Label[] _karmaLabels = (Label[])(object)new Label[6];

		private readonly Label[] _knownLabels = (Label[])(object)new Label[6];

		private readonly StandardButton[] _copyButtons = (StandardButton[])(object)new StandardButton[6];

		private readonly bool[] _slotIsKnown = new bool[6];

		private LoadingSpinner _centerSpinner;

		private Label _statusLabel;

		private RecipeTooltip _tooltip;

		private readonly PsnaDataService _dataService;

		private readonly SettingEntry<bool> _hideKnownNpcs;

		private readonly Action<TodayLocationsView> _registerActive;

		private readonly Action<TodayLocationsView> _unregisterActive;

		private readonly int _contentWidth;

		private readonly int[] _slotItemIds = new int[6];

		private readonly AsyncTexture2D[] _slotCraftedIconTextures = (AsyncTexture2D[])(object)new AsyncTexture2D[6];

		private bool _isBuilt;

		private float _fadeOpacity;

		private bool _fadeActive;

		private const float FadeDuration = 0.35f;

		public TodayLocationsView(PsnaDataService dataService, SettingEntry<bool> hideKnownNpcs, int contentWidth, Action<TodayLocationsView> registerActive, Action<TodayLocationsView> unregisterActive)
			: this()
		{
			_dataService = dataService;
			_hideKnownNpcs = hideKnownNpcs;
			_contentWidth = contentWidth;
			_registerActive = registerActive;
			_unregisterActive = unregisterActive;
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			_tooltip = new RecipeTooltip();
			if (_dataService.CoinTexturesLoaded)
			{
				_tooltip.SetCoinTextures(_dataService.CoinGoldTexture, _dataService.CoinSilverTexture, _dataService.CoinCopperTexture);
			}
			_hideKnownNpcs.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideKnownNpcSettingChanged);
			_dataService.DataUpdated += OnDataUpdated;
			_dataService.StatusChanged += OnStatusChanged;
			return Task.FromResult(result: true);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			BuildRows(buildPanel, PsnaSchedule.GetTodaysLocations());
			int spinnerY = 282;
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(_contentWidth / 2 - 24, spinnerY));
			((Control)val).set_Visible(false);
			_centerSpinner = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("");
			((Control)val2).set_Location(new Point(0, spinnerY + 48 + 8));
			((Control)val2).set_Size(new Point(_contentWidth, 20));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_LightGray());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Visible(false);
			_statusLabel = val2;
			_isBuilt = true;
			_registerActive?.Invoke(this);
			ApplyCurrentState();
		}

		protected override void Unload()
		{
			_isBuilt = false;
			_unregisterActive?.Invoke(this);
			RecipeTooltip tooltip = _tooltip;
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			if (_hideKnownNpcs != null)
			{
				_hideKnownNpcs.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideKnownNpcSettingChanged);
			}
			_dataService.DataUpdated -= OnDataUpdated;
			_dataService.StatusChanged -= OnStatusChanged;
		}

		private void ApplyCurrentState()
		{
			ShowLoadingSpinner();
			_dataService.Fetch();
		}

		private void OnDataUpdated()
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				if (_isBuilt)
				{
					if (_dataService.LastResult != null)
					{
						ApplyResult(_dataService.LastResult, animate: true);
					}
					else if (_dataService.LastFetchFailed)
					{
						ShowFetchFailed();
					}
				}
			});
		}

		private void ShowFetchFailed()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			((Control)_centerSpinner).set_Visible(false);
			((Control)_statusLabel).set_Visible(false);
			for (int i = 0; i < 6; i++)
			{
				((Control)_rowPanels[i]).set_Visible(true);
				_recipeLabels[i].set_Text(Strings.Get("FetchFailed"));
				_recipeLabels[i].set_TextColor(Color.get_White());
				((Control)_recipeLabels[i]).set_Visible(true);
				((Control)_copyButtons[i]).set_Visible(true);
			}
			StartFadeIn();
		}

		private void OnStatusChanged(string text)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				if (_isBuilt)
				{
					_statusLabel.set_Text(text);
				}
			});
		}

		private void BuildRows(Container buildPanel, PsnaSchedule.AgentLocation[] locations)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Expected O, but got Unknown
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Expected O, but got Unknown
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			int y = 0;
			int rowWidth = _contentWidth;
			for (int i = 0; i < locations.Length; i++)
			{
				PsnaSchedule.AgentLocation loc = locations[i];
				Panel[] rowPanels = _rowPanels;
				int num = i;
				Panel val = new Panel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Size(new Point(rowWidth, 102));
				((Control)val).set_Location(new Point(0, y));
				val.set_ShowBorder(true);
				val.set_Title(loc.NpcDisplay);
				rowPanels[num] = val;
				Panel row = _rowPanels[i];
				int leftTextX = 60;
				int rightColW = 200;
				int rightColX = rowWidth - rightColW - 12;
				int leftTextW = rightColX - leftTextX - 8;
				int locationColW = rightColW + 60;
				int locationColX = rightColX - 60;
				int capturedY = y;
				StandardButton[] copyButtons = _copyButtons;
				int num2 = i;
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent(buildPanel);
				val2.set_Text(Strings.Get("Copy"));
				((Control)val2).set_Size(new Point(70, 26));
				((Control)val2).set_Location(new Point(rowWidth - 70 - 10, capturedY + 5));
				((Control)val2).set_Visible(false);
				copyButtons[num2] = val2;
				Image[] recipeIcons = _recipeIcons;
				int num3 = i;
				Image val3 = new Image();
				((Control)val3).set_Parent((Container)(object)row);
				((Control)val3).set_Size(new Point(42, 42));
				((Control)val3).set_Location(new Point(8, 10));
				((Control)val3).set_Visible(false);
				recipeIcons[num3] = val3;
				Label[] recipeLabels = _recipeLabels;
				int num4 = i;
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)row);
				val4.set_Text("");
				((Control)val4).set_Location(new Point(leftTextX, 12));
				((Control)val4).set_Size(new Point(leftTextW, 18));
				val4.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val4).set_Visible(false);
				recipeLabels[num4] = val4;
				Image[] karmaIcons = _karmaIcons;
				int num5 = i;
				Image val5 = new Image();
				((Control)val5).set_Parent((Container)(object)row);
				((Control)val5).set_Size(new Point(24, 24));
				((Control)val5).set_Location(new Point(leftTextX, 32));
				((Control)val5).set_Visible(false);
				karmaIcons[num5] = val5;
				Label[] karmaLabels = _karmaLabels;
				int num6 = i;
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)row);
				val6.set_Text("25,200");
				((Control)val6).set_Location(new Point(leftTextX + 24 + 4, 38));
				((Control)val6).set_Size(new Point(100, 16));
				val6.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val6).set_Visible(false);
				karmaLabels[num6] = val6;
				Label[] knownLabels = _knownLabels;
				int num7 = i;
				Label val7 = new Label();
				((Control)val7).set_Parent(buildPanel);
				val7.set_Text(Strings.Get("AlreadyKnowRecipe"));
				((Control)val7).set_Location(new Point(rowWidth - 70 - 10 - 8 - 240, capturedY + 5));
				((Control)val7).set_Size(new Point(240, 26));
				val7.set_Font(GameService.Content.get_DefaultFont14());
				val7.set_TextColor(new Color(220, 80, 80));
				val7.set_HorizontalAlignment((HorizontalAlignment)2);
				val7.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val7).set_Visible(false);
				knownLabels[num7] = val7;
				PsnaSchedule.AgentLocation capturedLoc = loc;
				((Control)_copyButtons[i]).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (string.IsNullOrEmpty(capturedLoc.ChatCode))
					{
						ScreenNotification.ShowNotification(Strings.Get("NoChatCodeYet"), (NotificationType)0, (Texture2D)null, 4);
					}
					else
					{
						TrySetClipboardText(capturedLoc.ChatCode);
					}
				});
				int slotIndex = i;
				((Control)_recipeIcons[i]).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					Point position2 = GameService.Input.get_Mouse().get_Position();
					ShowTooltipForSlot(slotIndex, position2.X, position2.Y);
				});
				((Control)_recipeIcons[i]).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					Point position = GameService.Input.get_Mouse().get_Position();
					_tooltip.MoveTo(position.X, position.Y);
				});
				((Control)_recipeIcons[i]).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					((Control)_tooltip).set_Visible(false);
				});
				Label val8 = new Label();
				((Control)val8).set_Parent((Container)(object)row);
				val8.set_Text(loc.Location);
				((Control)val8).set_Location(new Point(locationColX, 10));
				((Control)val8).set_Size(new Point(locationColW, 18));
				val8.set_Font(GameService.Content.get_DefaultFont14());
				val8.set_WrapText(false);
				val8.set_HorizontalAlignment((HorizontalAlignment)2);
				Label val9 = new Label();
				((Control)val9).set_Parent((Container)(object)row);
				val9.set_Text(loc.Map);
				((Control)val9).set_Location(new Point(rightColX, 32));
				((Control)val9).set_Size(new Point(rightColW, 16));
				val9.set_Font(GameService.Content.get_DefaultFont12());
				val9.set_TextColor(Color.get_LightGray());
				val9.set_HorizontalAlignment((HorizontalAlignment)2);
				y += 102;
			}
		}

		private void ShowTooltipForSlot(int slotIndex, int mouseX, int mouseY)
		{
			_tooltip.MoveTo(mouseX, mouseY);
			int itemId = _slotItemIds[slotIndex];
			if (itemId > 0 && RecipeDefs.ByRecipeSheetId.TryGetValue(itemId, out var def))
			{
				_tooltip.SetRecipe(def, _slotCraftedIconTextures[slotIndex]);
				((Control)_tooltip).set_Visible(true);
			}
			else
			{
				((Control)_tooltip).set_Visible(false);
			}
		}

		public void OnWindowHidden()
		{
			if (_tooltip != null)
			{
				((Control)_tooltip).set_Visible(false);
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

		private void ShowLoadingSpinner()
		{
			_fadeActive = false;
			((Control)_centerSpinner).set_Visible(true);
			((Control)_statusLabel).set_Visible(true);
			for (int i = 0; i < 6; i++)
			{
				((Control)_rowPanels[i]).set_Visible(false);
				((Control)_recipeIcons[i]).set_Visible(false);
				((Control)_recipeLabels[i]).set_Visible(false);
				((Control)_karmaIcons[i]).set_Visible(false);
				((Control)_karmaLabels[i]).set_Visible(false);
				((Control)_knownLabels[i]).set_Visible(false);
				((Control)_copyButtons[i]).set_Visible(false);
			}
		}

		private void SetUiOpacity(float opacity)
		{
			for (int i = 0; i < 6; i++)
			{
				((Control)_rowPanels[i]).set_Opacity(opacity);
				((Control)_recipeIcons[i]).set_Opacity(opacity);
				((Control)_recipeLabels[i]).set_Opacity(opacity);
				((Control)_karmaIcons[i]).set_Opacity(opacity);
				((Control)_karmaLabels[i]).set_Opacity(opacity);
				((Control)_knownLabels[i]).set_Opacity(opacity);
				((Control)_copyButtons[i]).set_Opacity(opacity);
			}
		}

		private void StartFadeIn()
		{
			_fadeOpacity = 0f;
			_fadeActive = true;
			SetUiOpacity(0f);
		}

		private void ApplyKnownVisibility(int i, bool isKnown)
		{
			bool hide = isKnown && _hideKnownNpcs.get_Value();
			((Control)_rowPanels[i]).set_Visible(!hide);
			((Control)_copyButtons[i]).set_Visible(!hide);
			((Control)_knownLabels[i]).set_Visible(isKnown && !hide);
		}

		private void RelayoutRows()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			int y = 0;
			int rowWidth = _contentWidth;
			for (int i = 0; i < 6; i++)
			{
				if (((Control)_rowPanels[i]).get_Visible())
				{
					((Control)_rowPanels[i]).set_Location(new Point(0, y));
					((Control)_copyButtons[i]).set_Location(new Point(rowWidth - 70 - 10, y + 5));
					((Control)_knownLabels[i]).set_Location(new Point(rowWidth - 70 - 10 - 8 - 240, y + 5));
					y += 102;
				}
			}
		}

		private void OnHideKnownNpcSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (!_isBuilt || _dataService.LastResult == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				if (_slotItemIds[i] > 0)
				{
					ApplyKnownVisibility(i, _slotIsKnown[i]);
				}
			}
			RelayoutRows();
		}

		private void ApplyResult(PsnaDataService.FetchResult result, bool animate)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			((Control)_centerSpinner).set_Visible(false);
			((Control)_statusLabel).set_Visible(false);
			for (int i = 0; i < 6; i++)
			{
				PsnaDataService.SlotData slot = result.Slots[i];
				((Control)_rowPanels[i]).set_Visible(true);
				((Control)_copyButtons[i]).set_Visible(true);
				if (slot.IsNotDetermined)
				{
					_recipeLabels[i].set_Text(Strings.Get("NotDeterminedYet"));
					_recipeLabels[i].set_TextColor(Color.get_White());
					((Control)_recipeLabels[i]).set_Visible(true);
					((Control)_recipeIcons[i]).set_Visible(false);
					((Control)_karmaIcons[i]).set_Visible(false);
					((Control)_karmaLabels[i]).set_Visible(false);
					((Control)_knownLabels[i]).set_Visible(false);
				}
				else
				{
					_slotItemIds[i] = slot.ItemId;
					_slotCraftedIconTextures[i] = slot.CraftedIconTexture;
					RecipeDef def;
					bool hasDef = RecipeDefs.ByRecipeSheetId.TryGetValue(slot.ItemId, out def);
					_recipeLabels[i].set_Text(hasDef ? def.SheetName : "");
					_recipeLabels[i].set_TextColor(hasDef ? RecipeTooltip.GetRarityColor(def.Rarity) : Color.get_White());
					((Control)_recipeLabels[i]).set_Visible(true);
					_recipeIcons[i].set_Texture(slot.IconTexture);
					((Control)_recipeIcons[i]).set_Visible(slot.IconTexture != null);
					_karmaIcons[i].set_Texture(result.KarmaTexture);
					((Control)_karmaIcons[i]).set_Visible(result.KarmaTexture != null);
					((Control)_karmaLabels[i]).set_Visible(true);
					((Control)_knownLabels[i]).set_Visible(false);
				}
			}
			RelayoutRows();
			if (animate)
			{
				StartFadeIn();
			}
			else
			{
				SetUiOpacity(1f);
			}
			ApplyKnownRecipeHighlighting();
			if (_dataService.CoinTexturesLoaded)
			{
				_tooltip.SetCoinTextures(_dataService.CoinGoldTexture, _dataService.CoinSilverTexture, _dataService.CoinCopperTexture);
			}
		}

		private void ApplyKnownRecipeHighlighting()
		{
			HashSet<int> known = _dataService.KnownCraftingRecipeIds;
			for (int i = 0; i < 6; i++)
			{
				int itemId = _slotItemIds[i];
				if (itemId <= 0 || !RecipeDefs.ByRecipeSheetId.TryGetValue(itemId, out var def))
				{
					_slotIsKnown[i] = false;
					((Control)_knownLabels[i]).set_Visible(false);
					continue;
				}
				bool isKnown = known != null && def.CraftingRecipeIds != null && Array.Exists(def.CraftingRecipeIds, (int id) => known.Contains(id));
				_slotIsKnown[i] = isKnown;
				ApplyKnownVisibility(i, isKnown);
			}
			RelayoutRows();
		}

		private static void TrySetClipboardText(string text)
		{
			try
			{
				Clipboard.SetText(text);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to set clipboard.");
			}
		}
	}
}
