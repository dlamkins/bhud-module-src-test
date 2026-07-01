using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace WhereIsMyPSNA
{
	public class PsnaWindow : StandardWindow
	{
		private class SlotData
		{
			public bool IsNotDetermined;

			public int ItemId;

			public string ItemName;

			public Texture2D IconTexture;

			public Texture2D CraftedIconTexture;
		}

		private class FetchCache
		{
			public DateTime Timestamp;

			public SlotData[] Slots;

			public Texture2D KarmaTexture;
		}

		private class Gw2Item
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("icon")]
			public string Icon { get; set; }
		}

		private class Gw2Currency
		{
			[JsonProperty("icon")]
			public string Icon { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<PsnaWindow>();

		private static readonly HttpClient Http = new HttpClient();

		private const string SheetCsvUrl = "https://docs.google.com/spreadsheets/d/14Jf0-RAcva1w-vx71vK1YvRIcGi4sQfWcrZHyhwAzwQ/export?format=csv&gid=0";

		private const string Gw2ItemsUrl = "https://api.guildwars2.com/v2/items?ids=";

		private const string Gw2KarmaCurrency = "https://api.guildwars2.com/v2/currencies/2";

		private const int RowHeight = 102;

		private const int RowSpacing = 0;

		private const int RowPadding = 20;

		private const int IconSize = 42;

		private const int KarmaIconSize = 24;

		private const int TitleOffset = 26;

		private static readonly int[] ScheduleToSheetCol = new int[6] { 0, 1, 3, 2, 4, 5 };

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

		private readonly RecipeTooltip _tooltip;

		private readonly Gw2ApiManager _apiManager;

		private readonly SettingEntry<bool> _hideKnownNpcs;

		private readonly int[] _slotItemIds = new int[6];

		private readonly Texture2D[] _slotCraftedIconTextures = (Texture2D[])(object)new Texture2D[6];

		private bool _coinTexturesLoaded;

		private int _hoveredSlot = -1;

		private CancellationTokenSource _spinnerCts;

		private volatile HashSet<int> _knownCraftingRecipeIds;

		private float _fadeOpacity;

		private bool _fadeActive;

		private const float FadeDuration = 0.35f;

		private const string CoinGoldUrl = "https://render.guildwars2.com/file/090A980A96D39FD36FBB004903644C6DBEFB1FFB/156904.png";

		private const string CoinSilverUrl = "https://render.guildwars2.com/file/E5A2197D78ECE4AE0349C8B3710D033D22DB0DA6/156907.png";

		private const string CoinCopperUrl = "https://render.guildwars2.com/file/6CF8F96A3299CFC75D5CC90617C3C70331A1EF0E/156902.png";

		private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5.0);

		private FetchCache _cache;

		public PsnaWindow(ContentsManager contentsManager, Gw2ApiManager apiManager, SettingEntry<bool> hideKnownNpcs)
			: this(contentsManager.GetTexture("window_bg.png"), new Rectangle(40, 26, 913, 691), new Rectangle(70, 40, 839, 636))
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Expected O, but got Unknown
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Where Is My PSNA");
			((WindowBase2)this).set_Subtitle("Daily Locations");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("window_emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("PsnaWindow_com.odizinne.whereismypsna_38d37290-b5f9-447d-97ea-45b0b50e5f56");
			_apiManager = apiManager;
			_hideKnownNpcs = hideKnownNpcs;
			_tooltip = new RecipeTooltip();
			_hideKnownNpcs.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideKnownNpcSettingChanged);
			BuildRows(PsnaSchedule.GetTodaysLocations());
			int spinnerY = 282;
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(((Container)this).get_ContentRegion().Width / 2 - 24, spinnerY));
			((Control)val).set_Visible(false);
			_centerSpinner = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("");
			((Control)val2).set_Location(new Point(0, spinnerY + 48 + 8));
			((Control)val2).set_Size(new Point(((Container)this).get_ContentRegion().Width, 20));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_LightGray());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Visible(false);
			_statusLabel = val2;
		}

		private void BuildRows(PsnaSchedule.AgentLocation[] locations)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Expected O, but got Unknown
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Expected O, but got Unknown
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			int y = 0;
			int rowWidth = ((Container)this).get_ContentRegion().Width;
			for (int i = 0; i < locations.Length; i++)
			{
				PsnaSchedule.AgentLocation loc = locations[i];
				Panel[] rowPanels = _rowPanels;
				int num = i;
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(new Point(rowWidth, 102));
				((Control)val).set_Location(new Point(0, y));
				val.set_ShowBorder(true);
				val.set_Title(loc.Npc);
				rowPanels[num] = val;
				Panel row = _rowPanels[i];
				int leftTextX = 60;
				int rightColW = 200;
				int rightColX = rowWidth - rightColW - 12;
				int leftTextW = rightColX - leftTextX - 8;
				int capturedY = y;
				StandardButton[] copyButtons = _copyButtons;
				int num2 = i;
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text("Copy");
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
				((Control)val7).set_Parent((Container)(object)this);
				val7.set_Text("You already know this recipe");
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
						ScreenNotification.ShowNotification("No chat code for this location yet", (NotificationType)0, (Texture2D)null, 4);
					}
					else
					{
						TrySetClipboardText(capturedLoc.ChatCode);
					}
				});
				Label val8 = new Label();
				((Control)val8).set_Parent((Container)(object)row);
				val8.set_Text(loc.Location);
				((Control)val8).set_Location(new Point(rightColX, 10));
				((Control)val8).set_Size(new Point(rightColW, 18));
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

		public void ToggleWindow()
		{
			((WindowBase2)this).ToggleWindow();
			if (((Control)this).get_Visible())
			{
				if (_cache != null && DateTime.UtcNow - _cache.Timestamp < CacheExpiry)
				{
					ApplyCache(_cache);
					RefreshKnownLabelsAsync();
					return;
				}
				ShowLoadingSpinner();
				_spinnerCts?.Cancel();
				_spinnerCts = new CancellationTokenSource();
				Task accountTask = FetchAccountRecipesAsync();
				Task.Run(() => FetchAllAsync(_spinnerCts.Token, accountTask));
			}
			else
			{
				((Control)_tooltip).set_Visible(false);
			}
		}

		private void ShowLoadingSpinner()
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
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
			});
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

		private void SetStatus(string text)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				_statusLabel.set_Text(text);
			});
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
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			int y = 0;
			int rowWidth = ((Container)this).get_ContentRegion().Width;
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
			if (!((Control)this).get_Visible() || _cache == null)
			{
				return;
			}
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				for (int i = 0; i < 6; i++)
				{
					if (_slotItemIds[i] > 0)
					{
						ApplyKnownVisibility(i, _slotIsKnown[i]);
					}
				}
				RelayoutRows();
			});
		}

		private void ApplyCache(FetchCache cache)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				((Control)_centerSpinner).set_Visible(false);
				((Control)_statusLabel).set_Visible(false);
				for (int i = 0; i < 6; i++)
				{
					SlotData slotData = cache.Slots[i];
					((Control)_rowPanels[i]).set_Visible(true);
					((Control)_copyButtons[i]).set_Visible(true);
					if (slotData.IsNotDetermined)
					{
						_recipeLabels[i].set_Text("Not determined yet");
						((Control)_recipeLabels[i]).set_Visible(true);
						((Control)_recipeIcons[i]).set_Visible(false);
						((Control)_karmaIcons[i]).set_Visible(false);
						((Control)_karmaLabels[i]).set_Visible(false);
						((Control)_knownLabels[i]).set_Visible(false);
					}
					else
					{
						_slotItemIds[i] = slotData.ItemId;
						_slotCraftedIconTextures[i] = slotData.CraftedIconTexture;
						_recipeLabels[i].set_Text(slotData.ItemName);
						((Control)_recipeLabels[i]).set_Visible(true);
						_recipeIcons[i].set_Texture(AsyncTexture2D.op_Implicit(slotData.IconTexture));
						((Control)_recipeIcons[i]).set_Visible(slotData.IconTexture != null);
						_karmaIcons[i].set_Texture(AsyncTexture2D.op_Implicit(cache.KarmaTexture));
						((Control)_karmaIcons[i]).set_Visible(cache.KarmaTexture != null);
						((Control)_karmaLabels[i]).set_Visible(true);
						((Control)_knownLabels[i]).set_Visible(false);
					}
				}
				RelayoutRows();
				StartFadeIn();
			});
		}

		private async Task FetchAccountRecipesAsync()
		{
			if (!_apiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[1] { (TokenPermission)1 }))
			{
				Logger.Warn("Skipping account recipes fetch: missing Account permission.");
				return;
			}
			try
			{
				_knownCraftingRecipeIds = new HashSet<int>((IEnumerable<int>)(await ((IBlobClient<IApiV2ObjectList<int>>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Recipes()).GetAsync(default(CancellationToken))));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch account recipes.");
			}
		}

		private async Task RefreshKnownLabelsAsync()
		{
			await FetchAccountRecipesAsync();
			HashSet<int> known = _knownCraftingRecipeIds;
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				for (int i = 0; i < 6; i++)
				{
					int num = _slotItemIds[i];
					if (num <= 0 || !RecipeDefs.ByRecipeSheetId.TryGetValue(num, out var value))
					{
						_slotIsKnown[i] = false;
						((Control)_knownLabels[i]).set_Visible(false);
					}
					else
					{
						bool flag = known != null && value.CraftingRecipeIds != null && Array.Exists(value.CraftingRecipeIds, (int id) => known.Contains(id));
						_slotIsKnown[i] = flag;
						ApplyKnownVisibility(i, flag);
					}
				}
				RelayoutRows();
			});
		}

		private async Task FetchAllAsync(CancellationToken ct, Task accountRecipesTask)
		{
			_ = 5;
			try
			{
				if (!_coinTexturesLoaded)
				{
					Texture2D[] coins = await Task.WhenAll<Texture2D>(FetchTextureAsync("https://render.guildwars2.com/file/090A980A96D39FD36FBB004903644C6DBEFB1FFB/156904.png"), FetchTextureAsync("https://render.guildwars2.com/file/E5A2197D78ECE4AE0349C8B3710D033D22DB0DA6/156907.png"), FetchTextureAsync("https://render.guildwars2.com/file/6CF8F96A3299CFC75D5CC90617C3C70331A1EF0E/156902.png"));
					_coinTexturesLoaded = true;
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						_tooltip.SetCoinTextures(coins[0], coins[1], coins[2]);
					});
				}
				SetStatus("Fetching schedule...");
				Task<Texture2D> karmaTask = FetchKarmaIconAsync();
				Task<(int[] itemIds, bool[] todayFlags)> sheetTask = FetchSheetAsync();
				await Task.WhenAll(karmaTask, sheetTask);
				if (ct.IsCancellationRequested)
				{
					return;
				}
				Texture2D karmaTexture = karmaTask.Result;
				(int[], bool[]) result = sheetTask.Result;
				int[] itemIds = result.Item1;
				bool[] todayFlags = result.Item2;
				int[] validIds = (from x in itemIds.Select((int id, int col) => new { id, col })
					where x.id > 0 && todayFlags[x.col]
					select x.id).Distinct().ToArray();
				SetStatus("Fetching recipe data...");
				Dictionary<int, Gw2Item> dictionary = ((validIds.Length == 0) ? new Dictionary<int, Gw2Item>() : (await FetchItemsAsync(validIds)));
				Dictionary<int, Gw2Item> items = dictionary;
				int[] craftedIdPerSlot = new int[6];
				for (int k = 0; k < 6; k++)
				{
					int sheetCol = ScheduleToSheetCol[k];
					if (todayFlags[sheetCol] && itemIds[sheetCol] > 0 && RecipeDefs.ByRecipeSheetId.TryGetValue(itemIds[sheetCol], out var def) && def.ItemIds.Length != 0)
					{
						craftedIdPerSlot[k] = def.ItemIds[0];
					}
				}
				int[] distinctCraftedIds = craftedIdPerSlot.Where((int id) => id > 0).Distinct().ToArray();
				dictionary = ((distinctCraftedIds.Length == 0) ? new Dictionary<int, Gw2Item>() : (await FetchItemsAsync(distinctCraftedIds)));
				Dictionary<int, Gw2Item> craftedItems = dictionary;
				SlotData[] slotResults = new SlotData[6];
				Task<Texture2D>[] iconTasks = new Task<Texture2D>[6];
				Task<Texture2D>[] craftedIconTasks = new Task<Texture2D>[6];
				for (int j = 0; j < 6; j++)
				{
					int sheetCol2 = ScheduleToSheetCol[j];
					slotResults[j] = new SlotData();
					if (!todayFlags[sheetCol2] || itemIds[sheetCol2] <= 0 || !items.TryGetValue(itemIds[sheetCol2], out var item))
					{
						slotResults[j].IsNotDetermined = true;
						iconTasks[j] = Task.FromResult<Texture2D>(null);
						craftedIconTasks[j] = Task.FromResult<Texture2D>(null);
					}
					else
					{
						slotResults[j].ItemId = itemIds[sheetCol2];
						slotResults[j].ItemName = item.Name;
						iconTasks[j] = (string.IsNullOrEmpty(item.Icon) ? Task.FromResult<Texture2D>(null) : FetchTextureAsync(item.Icon));
						craftedIconTasks[j] = ((craftedIdPerSlot[j] > 0 && craftedItems.TryGetValue(craftedIdPerSlot[j], out var craftedItem) && !string.IsNullOrEmpty(craftedItem.Icon)) ? FetchTextureAsync(craftedItem.Icon) : Task.FromResult<Texture2D>(null));
					}
				}
				SetStatus("Fetching icons...");
				await Task.WhenAll(iconTasks.Concat(craftedIconTasks).ToArray());
				if (ct.IsCancellationRequested)
				{
					return;
				}
				for (int i = 0; i < 6; i++)
				{
					if (!slotResults[i].IsNotDetermined)
					{
						slotResults[i].IconTexture = iconTasks[i].Result;
						slotResults[i].CraftedIconTexture = craftedIconTasks[i].Result;
					}
				}
				SetStatus("Checking known recipes...");
				await accountRecipesTask;
				if (ct.IsCancellationRequested)
				{
					return;
				}
				HashSet<int> known = _knownCraftingRecipeIds;
				if (slotResults.Any((SlotData s) => !s.IsNotDetermined))
				{
					_cache = new FetchCache
					{
						Timestamp = DateTime.UtcNow,
						Slots = slotResults,
						KarmaTexture = karmaTexture
					};
				}
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					((Control)_centerSpinner).set_Visible(false);
					((Control)_statusLabel).set_Visible(false);
					for (int m = 0; m < 6; m++)
					{
						SlotData slotData = slotResults[m];
						((Control)_rowPanels[m]).set_Visible(true);
						((Control)_copyButtons[m]).set_Visible(true);
						if (slotData.IsNotDetermined)
						{
							_recipeLabels[m].set_Text("Not determined yet");
							((Control)_recipeLabels[m]).set_Visible(true);
						}
						else
						{
							_slotItemIds[m] = slotData.ItemId;
							_slotCraftedIconTextures[m] = slotData.CraftedIconTexture;
							_recipeLabels[m].set_Text(slotData.ItemName);
							((Control)_recipeLabels[m]).set_Visible(true);
							if (slotData.IconTexture != null)
							{
								_recipeIcons[m].set_Texture(AsyncTexture2D.op_Implicit(slotData.IconTexture));
								((Control)_recipeIcons[m]).set_Visible(true);
							}
							_karmaIcons[m].set_Texture(AsyncTexture2D.op_Implicit(karmaTexture));
							((Control)_karmaLabels[m]).set_Visible(true);
							((Control)_karmaIcons[m]).set_Visible(karmaTexture != null);
							if (RecipeDefs.ByRecipeSheetId.TryGetValue(slotData.ItemId, out var value))
							{
								bool flag = known != null && value.CraftingRecipeIds != null && Array.Exists(value.CraftingRecipeIds, (int id) => known.Contains(id));
								_slotIsKnown[m] = flag;
								ApplyKnownVisibility(m, flag);
							}
						}
					}
					RelayoutRows();
					StartFadeIn();
				});
			}
			catch (Exception ex) when (!ct.IsCancellationRequested)
			{
				Logger.Warn(ex, "Failed to fetch PSNA data.");
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					((Control)_centerSpinner).set_Visible(false);
					((Control)_statusLabel).set_Visible(false);
					for (int l = 0; l < 6; l++)
					{
						((Control)_rowPanels[l]).set_Visible(true);
						_recipeLabels[l].set_Text("Fetch failed");
						((Control)_recipeLabels[l]).set_Visible(true);
						((Control)_copyButtons[l]).set_Visible(true);
					}
					StartFadeIn();
				});
			}
		}

		private async Task<(int[] itemIds, bool[] todayFlags)> FetchSheetAsync()
		{
			string[] lines = (await Http.GetStringAsync("https://docs.google.com/spreadsheets/d/14Jf0-RAcva1w-vx71vK1YvRIcGi4sQfWcrZHyhwAzwQ/export?format=csv&gid=0")).Split('\n');
			string[] idLine = lines[1].Split(',');
			string[] dateLine = lines[2].Split(',');
			string currentDate = ((dateLine.Length > 6) ? dateLine[6].Trim().Trim('"') : "");
			int[] itemIds = new int[6];
			bool[] todayFlags = new bool[6];
			for (int col = 0; col < 6; col++)
			{
				if (col < idLine.Length && int.TryParse(idLine[col].Trim().Trim('"'), out var id))
				{
					itemIds[col] = id;
				}
				string colDate = ((col < dateLine.Length) ? dateLine[col].Trim().Trim('"') : "");
				todayFlags[col] = !string.IsNullOrEmpty(currentDate) && colDate == currentDate;
			}
			return (itemIds, todayFlags);
		}

		private async Task<Dictionary<int, Gw2Item>> FetchItemsAsync(int[] ids)
		{
			return JsonConvert.DeserializeObject<List<Gw2Item>>(await Http.GetStringAsync("https://api.guildwars2.com/v2/items?ids=" + string.Join(",", ids))).ToDictionary((Gw2Item x) => x.Id);
		}

		private async Task<Texture2D> FetchKarmaIconAsync()
		{
			_ = 1;
			try
			{
				Gw2Currency currency = JsonConvert.DeserializeObject<Gw2Currency>(await Http.GetStringAsync("https://api.guildwars2.com/v2/currencies/2"));
				if (!string.IsNullOrEmpty(currency?.Icon))
				{
					return await FetchTextureAsync(currency.Icon);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch karma icon.");
			}
			return null;
		}

		private async Task<Texture2D> FetchTextureAsync(string url)
		{
			_ = 1;
			try
			{
				byte[] bytes = await Http.GetByteArrayAsync(url);
				TaskCompletionSource<Texture2D> tcs = new TaskCompletionSource<Texture2D>();
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice gd)
				{
					try
					{
						using MemoryStream memoryStream = new MemoryStream(bytes);
						tcs.SetResult(Texture2D.FromStream(gd, (Stream)memoryStream));
					}
					catch (Exception exception)
					{
						tcs.SetException(exception);
					}
				});
				return await tcs.Task;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to fetch texture: " + url);
				return null;
			}
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

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).UpdateContainer(gameTime);
			if (!((Control)this).get_Visible())
			{
				return;
			}
			if (_fadeActive)
			{
				_fadeOpacity += (float)(gameTime.get_ElapsedGameTime().TotalSeconds / 0.3499999940395355);
				if (_fadeOpacity >= 1f)
				{
					_fadeOpacity = 1f;
					_fadeActive = false;
				}
				SetUiOpacity(_fadeOpacity);
			}
			Point mouse = GameService.Input.get_Mouse().get_Position();
			int hitSlot = -1;
			for (int i = 0; i < 6; i++)
			{
				if (((Control)_recipeIcons[i]).get_Visible())
				{
					Rectangle absoluteBounds = ((Control)_recipeIcons[i]).get_AbsoluteBounds();
					if (((Rectangle)(ref absoluteBounds)).Contains(mouse.X, mouse.Y))
					{
						hitSlot = i;
						break;
					}
				}
			}
			if (hitSlot >= 0)
			{
				_tooltip.MoveTo(mouse.X, mouse.Y);
				if (hitSlot != _hoveredSlot)
				{
					_hoveredSlot = hitSlot;
					int itemId = _slotItemIds[hitSlot];
					if (itemId > 0 && RecipeDefs.ByRecipeSheetId.TryGetValue(itemId, out var def))
					{
						_tooltip.SetRecipe(def, _slotCraftedIconTextures[hitSlot]);
						((Control)_tooltip).set_Visible(true);
					}
					else
					{
						((Control)_tooltip).set_Visible(false);
					}
				}
			}
			else
			{
				_hoveredSlot = -1;
				((Control)_tooltip).set_Visible(false);
			}
		}

		protected override void DisposeControl()
		{
			_spinnerCts?.Cancel();
			_spinnerCts?.Dispose();
			RecipeTooltip tooltip = _tooltip;
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			if (_hideKnownNpcs != null)
			{
				_hideKnownNpcs.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideKnownNpcSettingChanged);
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
