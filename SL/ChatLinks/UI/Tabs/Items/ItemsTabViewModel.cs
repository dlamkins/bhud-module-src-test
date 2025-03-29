using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using GuildWars2.Items;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI.Tabs.Items.Collections;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ItemsTabViewModel : ViewModel, IDisposable
	{
		public delegate ItemsTabViewModel Factory();

		[CompilerGenerated]
		private IStringLocalizer<ItemsTabView> _003Clocalizer_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IOptionsMonitor<ChatLinkOptions> _003Coptions_003EP;

		[CompilerGenerated]
		private ItemSearch _003Csearch_003EP;

		[CompilerGenerated]
		private ItemsListViewModel.Factory _003CitemsListViewModelFactory_003EP;

		[CompilerGenerated]
		private ChatLinkEditorViewModel.Factory _003CchatLinkEditorViewModelFactory_003EP;

		private ContentArea _area;

		private Item? _selectedItem;

		private ObservableCollection<ItemCategoryMenuItem> _menuItems;

		private string _selectedCategory;

		private string _searchText;

		private bool _searching;

		private int _searchNumber;

		private int _resultTotal;

		private string _resultText;

		public ContentArea Area
		{
			get
			{
				return _area;
			}
			private set
			{
				if (SetField(ref _area, value, "Area"))
				{
					OnPropertyChanged("ContentIcon");
					OnPropertyChanged("ContentTitle");
				}
			}
		}

		public Item? SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				SetField(ref _selectedItem, value, "SelectedItem");
			}
		}

		public ObservableCollection<ItemsListViewModel> SearchResults { get; }

		public string SearchPlaceholder => (string)_003Clocalizer_003EP["Search placeholder"];

		public ObservableCollection<ItemCategoryMenuItem> MenuItems
		{
			get
			{
				return _menuItems;
			}
			private set
			{
				SetField(ref _menuItems, value, "MenuItems");
			}
		}

		public string SelectedCategory
		{
			get
			{
				return _selectedCategory;
			}
			set
			{
				SetField(ref _selectedCategory, value, "SelectedCategory");
			}
		}

		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				SetField(ref _searchText, value, "SearchText");
			}
		}

		public bool Searching
		{
			get
			{
				return _searching;
			}
			set
			{
				SetField(ref _searching, value, "Searching");
			}
		}

		public int ResultTotal
		{
			get
			{
				return _resultTotal;
			}
			private set
			{
				SetField(ref _resultTotal, value, "ResultTotal");
			}
		}

		public string ResultText
		{
			get
			{
				return _resultText;
			}
			set
			{
				SetField(ref _resultText, value, "ResultText");
			}
		}

		public AsyncTexture2D? ContentIcon => _area.GetIcon();

		public string ContentTitle => _area.GetTitle();

		public RelayCommand BackCommand => new RelayCommand(delegate
		{
			if ((object)SelectedItem != null)
			{
				SelectedItem = null;
				Area = Area.Back();
			}
			else if (!string.IsNullOrWhiteSpace(SearchText))
			{
				SearchText = "";
				Area = Area.Back();
			}
			else if (!string.IsNullOrWhiteSpace(SelectedCategory))
			{
				SelectedCategory = "recently_added";
				Area = Area.Back();
			}
		});

		public AsyncRelayCommand ShowRecentCommand => new AsyncRelayCommand(async delegate
		{
			SelectedItem = null;
			SearchText = "";
			SelectedCategory = "";
			Area = new RecentlyAddedContentArea();
			await NewItems(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		});

		public AsyncRelayCommand<ItemsFilter> ShowCategoryCommand => new AsyncRelayCommand<ItemsFilter>(async delegate(ItemsFilter filter)
		{
			SelectedItem = null;
			SearchText = filter.Text ?? "";
			SelectedCategory = filter.Category ?? "";
			Area = new CategoryContentArea(filter.Label ?? "");
			await FilterItems(filter, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		});

		public RelayCommand<Item> SelectItemCommand => new RelayCommand<Item>(delegate(Item item)
		{
			SelectedItem = item;
			Area = Area.SelectItem();
		});

		public AsyncRelayCommand SearchCommand => new AsyncRelayCommand(async delegate
		{
			await Task.Run((Func<Task>)OnSearch).ConfigureAwait(continueOnCapturedContext: false);
		});

		public ItemsTabViewModel(IStringLocalizer<ItemsTabView> localizer, IEventAggregator eventAggregator, IOptionsMonitor<ChatLinkOptions> options, ItemSearch search, ItemsListViewModel.Factory itemsListViewModelFactory, ChatLinkEditorViewModel.Factory chatLinkEditorViewModelFactory)
		{
			_003Clocalizer_003EP = localizer;
			_003CeventAggregator_003EP = eventAggregator;
			_003Coptions_003EP = options;
			_003Csearch_003EP = search;
			_003CitemsListViewModelFactory_003EP = itemsListViewModelFactory;
			_003CchatLinkEditorViewModelFactory_003EP = chatLinkEditorViewModelFactory;
			_area = new RecentlyAddedContentArea();
			SearchResults = new ObservableCollection<ItemsListViewModel>();
			_menuItems = new ObservableCollection<ItemCategoryMenuItem>();
			_selectedCategory = "recently_added";
			_searchText = "";
			_resultText = "";
			base._002Ector();
		}

		private List<ItemCategoryMenuItem> GetCategories()
		{
			return new List<ItemCategoryMenuItem>(19)
			{
				new ItemCategoryMenuItem
				{
					Id = "recently_added",
					Label = (string)_003Clocalizer_003EP["Recently Added"]
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Armor"],
					Subcategories = new List<ItemCategoryMenuItem>(8)
					{
						new ItemCategoryMenuItem
						{
							Id = "armor",
							Label = (string)_003Clocalizer_003EP["All Armor"]
						},
						new ItemCategoryMenuItem
						{
							Id = "chest",
							Label = (string)_003Clocalizer_003EP["Chest"]
						},
						new ItemCategoryMenuItem
						{
							Id = "leggings",
							Label = (string)_003Clocalizer_003EP["Leggings"]
						},
						new ItemCategoryMenuItem
						{
							Id = "gloves",
							Label = (string)_003Clocalizer_003EP["Gloves"]
						},
						new ItemCategoryMenuItem
						{
							Id = "helm",
							Label = (string)_003Clocalizer_003EP["Headgear"]
						},
						new ItemCategoryMenuItem
						{
							Id = "helm_aquatic",
							Label = (string)_003Clocalizer_003EP["Aquatic Headgear"]
						},
						new ItemCategoryMenuItem
						{
							Id = "boots",
							Label = (string)_003Clocalizer_003EP["Boots"]
						},
						new ItemCategoryMenuItem
						{
							Id = "shoulders",
							Label = (string)_003Clocalizer_003EP["Shoulders"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Weapons"],
					Subcategories = new List<ItemCategoryMenuItem>(24)
					{
						new ItemCategoryMenuItem
						{
							Id = "weapon",
							Label = (string)_003Clocalizer_003EP["All Weapons"]
						},
						new ItemCategoryMenuItem
						{
							Id = "axe",
							Label = (string)_003Clocalizer_003EP["Axes"]
						},
						new ItemCategoryMenuItem
						{
							Id = "dagger",
							Label = (string)_003Clocalizer_003EP["Daggers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "focus",
							Label = (string)_003Clocalizer_003EP["Foci"]
						},
						new ItemCategoryMenuItem
						{
							Id = "greatsword",
							Label = (string)_003Clocalizer_003EP["Greatswords"]
						},
						new ItemCategoryMenuItem
						{
							Id = "hammer",
							Label = (string)_003Clocalizer_003EP["Hammers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "longbow",
							Label = (string)_003Clocalizer_003EP["Longbows"]
						},
						new ItemCategoryMenuItem
						{
							Id = "sword",
							Label = (string)_003Clocalizer_003EP["Swords"]
						},
						new ItemCategoryMenuItem
						{
							Id = "shortbow",
							Label = (string)_003Clocalizer_003EP["Short Bows"]
						},
						new ItemCategoryMenuItem
						{
							Id = "mace",
							Label = (string)_003Clocalizer_003EP["Maces"]
						},
						new ItemCategoryMenuItem
						{
							Id = "pistol",
							Label = (string)_003Clocalizer_003EP["Pistols"]
						},
						new ItemCategoryMenuItem
						{
							Id = "rifle",
							Label = (string)_003Clocalizer_003EP["Rifles"]
						},
						new ItemCategoryMenuItem
						{
							Id = "scepter",
							Label = (string)_003Clocalizer_003EP["Scepters"]
						},
						new ItemCategoryMenuItem
						{
							Id = "staff",
							Label = (string)_003Clocalizer_003EP["Staffs"]
						},
						new ItemCategoryMenuItem
						{
							Id = "torch",
							Label = (string)_003Clocalizer_003EP["Torches"]
						},
						new ItemCategoryMenuItem
						{
							Id = "warhorn",
							Label = (string)_003Clocalizer_003EP["Warhorns"]
						},
						new ItemCategoryMenuItem
						{
							Id = "shield",
							Label = (string)_003Clocalizer_003EP["Shields"]
						},
						new ItemCategoryMenuItem
						{
							Id = "spear",
							Label = (string)_003Clocalizer_003EP["Spears"]
						},
						new ItemCategoryMenuItem
						{
							Id = "harpoon_gun",
							Label = (string)_003Clocalizer_003EP["Harpoon Guns"]
						},
						new ItemCategoryMenuItem
						{
							Id = "trident",
							Label = (string)_003Clocalizer_003EP["Tridents"]
						},
						new ItemCategoryMenuItem
						{
							Id = "toy",
							Label = (string)_003Clocalizer_003EP["Toys"]
						},
						new ItemCategoryMenuItem
						{
							Id = "toy_two_handed",
							Label = (string)_003Clocalizer_003EP["Toys (Two-Handed)"]
						},
						new ItemCategoryMenuItem
						{
							Id = "small_bundle",
							Label = (string)_003Clocalizer_003EP["Small Bundles"]
						},
						new ItemCategoryMenuItem
						{
							Id = "large_bundle",
							Label = (string)_003Clocalizer_003EP["Large Bundles"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Upgrade Components"],
					Subcategories = new List<ItemCategoryMenuItem>(10)
					{
						new ItemCategoryMenuItem
						{
							Id = "upgrade_component",
							Label = (string)_003Clocalizer_003EP["All Upgrade Components"]
						},
						new ItemCategoryMenuItem
						{
							Id = "infusion",
							Label = (string)_003Clocalizer_003EP["Infusions"]
						},
						new ItemCategoryMenuItem
						{
							Id = "enrichment",
							Label = (string)_003Clocalizer_003EP["Enrichments"]
						},
						new ItemCategoryMenuItem
						{
							Id = "glyph",
							Label = (string)_003Clocalizer_003EP["Glyphs"]
						},
						new ItemCategoryMenuItem
						{
							Id = "rune",
							Label = (string)_003Clocalizer_003EP["Runes"]
						},
						new ItemCategoryMenuItem
						{
							Id = "rune_pvp",
							Label = (string)_003Clocalizer_003EP["Runes (PvP)"]
						},
						new ItemCategoryMenuItem
						{
							Id = "sigil",
							Label = (string)_003Clocalizer_003EP["Sigils"]
						},
						new ItemCategoryMenuItem
						{
							Id = "sigil_pvp",
							Label = (string)_003Clocalizer_003EP["Sigils (PvP)"]
						},
						new ItemCategoryMenuItem
						{
							Id = "jewel",
							Label = (string)_003Clocalizer_003EP["Jewels"]
						},
						new ItemCategoryMenuItem
						{
							Id = "universal_upgrade",
							Label = (string)_003Clocalizer_003EP["Universal Upgrades"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Trinkets"],
					Subcategories = new List<ItemCategoryMenuItem>(4)
					{
						new ItemCategoryMenuItem
						{
							Id = "trinket",
							Label = (string)_003Clocalizer_003EP["All Trinkets"]
						},
						new ItemCategoryMenuItem
						{
							Id = "accessory",
							Label = (string)_003Clocalizer_003EP["Accessories"]
						},
						new ItemCategoryMenuItem
						{
							Id = "amulet",
							Label = (string)_003Clocalizer_003EP["Amulets"]
						},
						new ItemCategoryMenuItem
						{
							Id = "ring",
							Label = (string)_003Clocalizer_003EP["Rings"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Id = "back",
					Label = (string)_003Clocalizer_003EP["Back Items"]
				},
				new ItemCategoryMenuItem
				{
					Id = "relic",
					Label = (string)_003Clocalizer_003EP["Relics"]
				},
				new ItemCategoryMenuItem
				{
					Id = "power_core",
					Label = (string)_003Clocalizer_003EP["Power Cores"]
				},
				new ItemCategoryMenuItem
				{
					Id = "jade_tech_module",
					Label = (string)_003Clocalizer_003EP["Jade Tech Modules"]
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Gathering Tools"],
					Subcategories = new List<ItemCategoryMenuItem>(6)
					{
						new ItemCategoryMenuItem
						{
							Id = "gathering_tool",
							Label = (string)_003Clocalizer_003EP["All Gathering Tools"]
						},
						new ItemCategoryMenuItem
						{
							Id = "harvesting_sickle",
							Label = (string)_003Clocalizer_003EP["Harvesting Sickles"]
						},
						new ItemCategoryMenuItem
						{
							Id = "logging_axe",
							Label = (string)_003Clocalizer_003EP["Logging Axes"]
						},
						new ItemCategoryMenuItem
						{
							Id = "mining_pick",
							Label = (string)_003Clocalizer_003EP["Mining Picks"]
						},
						new ItemCategoryMenuItem
						{
							Id = "bait",
							Label = (string)_003Clocalizer_003EP["Bait"]
						},
						new ItemCategoryMenuItem
						{
							Id = "lure",
							Label = (string)_003Clocalizer_003EP["Lures"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Consumables"],
					Subcategories = new List<ItemCategoryMenuItem>(24)
					{
						new ItemCategoryMenuItem
						{
							Id = "consumable",
							Label = (string)_003Clocalizer_003EP["All Consumables"]
						},
						new ItemCategoryMenuItem
						{
							Id = "food",
							Label = (string)_003Clocalizer_003EP["Food"]
						},
						new ItemCategoryMenuItem
						{
							Id = "utility",
							Label = (string)_003Clocalizer_003EP["Utilities"]
						},
						new ItemCategoryMenuItem
						{
							Id = "booze",
							Label = (string)_003Clocalizer_003EP["Booze"]
						},
						new ItemCategoryMenuItem
						{
							Id = "transmutation",
							Label = (string)_003Clocalizer_003EP["Transmutations"]
						},
						new ItemCategoryMenuItem
						{
							Id = "upgrade_extractor",
							Label = (string)_003Clocalizer_003EP["Upgrade Extractors"]
						},
						new ItemCategoryMenuItem
						{
							Id = "mount_license",
							Label = (string)_003Clocalizer_003EP["Mount Licenses"]
						},
						new ItemCategoryMenuItem
						{
							Id = "unlocker",
							Label = (string)_003Clocalizer_003EP["Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "mount_skin_unlocker",
							Label = (string)_003Clocalizer_003EP["Mount Skin Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "outfit_unlocker",
							Label = (string)_003Clocalizer_003EP["Outfit Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "glider_skin_unlocker",
							Label = (string)_003Clocalizer_003EP["Glider Skin Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "jade_bot_skin_unlocker",
							Label = (string)_003Clocalizer_003EP["Jade Bot Skin Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "miniature_unlocker",
							Label = (string)_003Clocalizer_003EP["Miniature Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "mist_champion_skin_unlocker",
							Label = (string)_003Clocalizer_003EP["Mist Champion Skin Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "dye",
							Label = (string)_003Clocalizer_003EP["Dyes"]
						},
						new ItemCategoryMenuItem
						{
							Id = "recipe_sheet",
							Label = (string)_003Clocalizer_003EP["Recipe Sheets"]
						},
						new ItemCategoryMenuItem
						{
							Id = "expansions",
							Label = (string)_003Clocalizer_003EP["Expansions"]
						},
						new ItemCategoryMenuItem
						{
							Id = "content_unlocker",
							Label = (string)_003Clocalizer_003EP["Content Unlockers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "random_unlocker",
							Label = (string)_003Clocalizer_003EP["Random Unlocker"]
						},
						new ItemCategoryMenuItem
						{
							Id = "appearance_changer",
							Label = (string)_003Clocalizer_003EP["Appearance Changers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "contract_npc",
							Label = (string)_003Clocalizer_003EP["Contract NPCs"]
						},
						new ItemCategoryMenuItem
						{
							Id = "teleport_to_friend",
							Label = (string)_003Clocalizer_003EP["Teleport to Friend"]
						},
						new ItemCategoryMenuItem
						{
							Id = "halloween_consumable",
							Label = (string)_003Clocalizer_003EP["Halloween Consumables"]
						},
						new ItemCategoryMenuItem
						{
							Id = "generic_consumable",
							Label = (string)_003Clocalizer_003EP["Generic Consumables"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Id = "currency",
					Label = (string)_003Clocalizer_003EP["Currencies"]
				},
				new ItemCategoryMenuItem
				{
					Id = "service",
					Label = (string)_003Clocalizer_003EP["Services"]
				},
				new ItemCategoryMenuItem
				{
					Label = (string)_003Clocalizer_003EP["Containers"],
					Subcategories = new List<ItemCategoryMenuItem>(5)
					{
						new ItemCategoryMenuItem
						{
							Id = "container",
							Label = (string)_003Clocalizer_003EP["All Containers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "default_container",
							Label = (string)_003Clocalizer_003EP["Normal Containers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "gift_box",
							Label = (string)_003Clocalizer_003EP["Gift Boxes"]
						},
						new ItemCategoryMenuItem
						{
							Id = "immediate_container",
							Label = (string)_003Clocalizer_003EP["Immediate Containers"]
						},
						new ItemCategoryMenuItem
						{
							Id = "black_lion_chest",
							Label = (string)_003Clocalizer_003EP["Black Lion Chests"]
						}
					}
				},
				new ItemCategoryMenuItem
				{
					Id = "crafting_material",
					Label = (string)_003Clocalizer_003EP["Crafting Materials"]
				},
				new ItemCategoryMenuItem
				{
					Id = "gizmo",
					Label = (string)_003Clocalizer_003EP["Gizmos"]
				},
				new ItemCategoryMenuItem
				{
					Id = "miniature",
					Label = (string)_003Clocalizer_003EP["Miniatures"]
				},
				new ItemCategoryMenuItem
				{
					Id = "salvage_tool",
					Label = (string)_003Clocalizer_003EP["Salvage Tools"]
				},
				new ItemCategoryMenuItem
				{
					Id = "trophy",
					Label = (string)_003Clocalizer_003EP["Trophies"]
				}
			};
		}

		public ChatLinkEditorViewModel CreateChatLinkEditorViewModel(Item item)
		{
			return _003CchatLinkEditorViewModelFactory_003EP(item);
		}

		public Task Load()
		{
			MenuItems = new ObservableCollection<ItemCategoryMenuItem>(GetCategories());
			_003CeventAggregator_003EP.Subscribe(new Func<LocaleChanged, Task>(OnLocaleChanged));
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseDownloaded, Task>(OnDatabaseDownloaded));
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseSeeded, Task>(OnDatabaseSeeded));
			return Task.CompletedTask;
		}

		private async Task OnLocaleChanged(LocaleChanged args)
		{
			OnPropertyChanged("SearchPlaceholder");
			MenuItems = new ObservableCollection<ItemCategoryMenuItem>(GetCategories());
			await Task.Run((Func<Task>)OnSearch).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task OnDatabaseDownloaded(DatabaseDownloaded downloaded)
		{
			await Task.Run((Func<Task>)OnSearch).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task OnDatabaseSeeded(DatabaseSeeded args)
		{
			if (args.Updated["items"] > 0)
			{
				await Task.Run((Func<Task>)OnSearch).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task OnSearch()
		{
			SelectedItem = null;
			string query = SearchText.Trim();
			int length = query.Length;
			if (length >= 3)
			{
				await FilterItems(new ItemsFilter
				{
					Category = SelectedCategory,
					Text = query
				}, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			else if (length == 0)
			{
				await FilterItems(new ItemsFilter
				{
					Category = SelectedCategory
				}, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			}
			Area = Area.Search(query);
		}

		private async Task NewItems(CancellationToken cancellationToken)
		{
			Searching = true;
			try
			{
				SearchResults.Clear();
				int maxResults = _003Coptions_003EP.CurrentValue.MaxResultCount;
				await foreach (Item item in _003Csearch_003EP.NewItems(maxResults).WithCancellation(cancellationToken))
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						ItemsListViewModel vm = _003CitemsListViewModelFactory_003EP(item, item.Id == SelectedItem?.Id);
						SearchResults.Add(vm);
						continue;
					}
					break;
				}
				ResultTotal = await _003Csearch_003EP.CountItems().ConfigureAwait(continueOnCapturedContext: false);
				ResultText = (string)((ResultTotal <= maxResults) ? _003Clocalizer_003EP["Total results", new object[1] { ResultTotal }] : _003Clocalizer_003EP["Partial results", new object[2] { maxResults, ResultTotal }]);
			}
			finally
			{
				Searching = false;
			}
		}

		private async Task FilterItems(ItemsFilter filter, CancellationToken cancellationToken)
		{
			int searchNumber = Interlocked.Increment(ref _searchNumber);
			Searching = true;
			try
			{
				SearchResults.Clear();
				int maxResults = _003Coptions_003EP.CurrentValue.MaxResultCount;
				ResultContext context = new ResultContext();
				await foreach (Item item in _003Csearch_003EP.FilterItems(filter, maxResults, context, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					if (!cancellationToken.IsCancellationRequested && searchNumber == _searchNumber)
					{
						ItemsListViewModel vm = _003CitemsListViewModelFactory_003EP(item, item.Id == SelectedItem?.Id);
						SearchResults.Add(vm);
						continue;
					}
					break;
				}
				ResultTotal = context.ResultTotal;
				ResultText = (string)((ResultTotal <= maxResults) ? _003Clocalizer_003EP["Total results", new object[1] { ResultTotal }] : _003Clocalizer_003EP["Partial results", new object[2] { maxResults, ResultTotal }]);
			}
			finally
			{
				Searching = false;
			}
		}

		public void Dispose()
		{
			_003CeventAggregator_003EP.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, Task>(OnLocaleChanged));
			_003CeventAggregator_003EP.Unsubscribe<DatabaseDownloaded>(new Func<DatabaseDownloaded, Task>(OnDatabaseDownloaded));
			_003CeventAggregator_003EP.Unsubscribe<DatabaseSeeded>(new Func<DatabaseSeeded, Task>(OnDatabaseSeeded));
		}
	}
}
