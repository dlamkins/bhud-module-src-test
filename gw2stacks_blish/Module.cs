using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Flurl.Http;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using gw2stacks_blish.data;
using gw2stacks_blish.reader;
using views;

namespace gw2stacks_blish
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private TabbedWindow2 gw2stacksWindow;

		private TabbedWindow2 ignoredItemsWindow;

		private StandardWindow characterBasedWindow;

		private CornerIcon icon;

		private LoadingSpinner loadingSpinner;

		private double loadingIntervalTicks;

		private double cooldownIntervalTicks;

		private bool isOnCooldown;

		private bool validData;

		private bool running;

		private bool fatalError;

		private bool isApiAvailable = true;

		private bool hasLut;

		private bool ignoreItemsFlag;

		private SettingEntry<bool> includeConsumableSetting;

		private SettingEntry<bool> localJson;

		private SettingEntry<bool> ignoreItemsFeature;

		private SettingEntry<string> displayType;

		private SettingEntry<bool> showBag;

		private SettingEntry<bool> itemShortcut;

		private Dictionary<int, AsyncTexture2D> itemTextures = new Dictionary<int, AsyncTexture2D>();

		private gw2stacks_blish.data.Model model;

		private Gw2Api api;

		private Dictionary<string, List<ItemForDisplay>> adviceDictionary = new Dictionary<string, List<ItemForDisplay>>();

		private Dictionary<string, List<ItemForDisplay>> fullCharacterInventories = new Dictionary<string, List<ItemForDisplay>>();

		private Dictionary<string, List<BagForDisplay>> characterBags = new Dictionary<string, List<BagForDisplay>>();

		private List<ItemForDisplay> combinedAdvice = new List<ItemForDisplay>();

		private AdviceTabView adviceView;

		private IgnoredView ignoredView;

		private FullCharacterView fullCharacterView;

		private FullCharacterBagView fullCharacterBagView;

		private ItemView itemView;

		private Dictionary<Tab, string> tabNameMapping;

		private Tab ignoredItemsTab;

		private List<int> ignoredItemList = new List<int>();

		private List<int> excludedItemIds = new List<int>();

		private Texture2D emptyTexture;

		private Texture2D border;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		private void create_name_tab_mapping()
		{
			tabNameMapping = new Dictionary<Tab, string>
			{
				{
					new Tab(AsyncTexture2D.FromAssetId(358447), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.stackAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.stackAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(255379), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.vendorAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.vendorAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(157091), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.rareSalvageAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.rareSalvageAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(536054), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.craftLuckAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.craftLuckAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(102597), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.deletableAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.deletableAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(156660), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.salvageAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.salvageAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(157123), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.consumableAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.consumableAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(156658), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.gobblerAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.gobblerAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(1494404), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.karmaAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.karmaAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(156685), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.craftingAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.craftingAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(156722), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.lwsAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.lwsAdvice]
				},
				{
					new Tab(AsyncTexture2D.FromAssetId(157099), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.miscAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.miscAdvice]
				}
			};
		}

		private async Task load_model()
		{
			running = false;
			if (!running && hasLut)
			{
				icon.Enabled = false;
				validData = false;
				hide_windows();
				loadingSpinner.Location = icon.Location;
				Logger.Debug("starting setup");
				model.includeConsumables = includeConsumableSetting.Value;
				loadingSpinner.Show();
				string path = DirectoriesManager.GetFullDirectoryPath("gw2stacks");
				if (path == null)
				{
					path = DirectoryUtil.RegisterDirectory("gw2stacks");
				}
				if (new DirectoryReader(path).FileExists("modelBackup.json"))
				{
					string input = System.IO.File.ReadAllText(path + "/modelBackup.json");
					model = JsonConvert.DeserializeObject<gw2stacks_blish.data.Model>(input);
				}
				else
				{
					fatalError = true;
					hasLut = false;
					handle_error("API error and error reading local model backup");
					Logger.Warn("Unexpected exception: Error reading local model backup @load_model");
				}
				running = true;
			}
		}

		private async Task save_model()
		{
			string path = DirectoriesManager.GetFullDirectoryPath("gw2stacks");
			if (path == null)
			{
				path = DirectoryUtil.RegisterDirectory("gw2stacks");
			}
			string output = JsonConvert.SerializeObject(model);
			System.IO.File.WriteAllText(path + "/modelBackup.json", output);
		}

		private async Task load_LUT()
		{
			_ = 3;
			try
			{
				bool local = localJson.Value;
				string path = DirectoriesManager.GetFullDirectoryPath("gw2stacks");
				if (path == null)
				{
					path = DirectoryUtil.RegisterDirectory("gw2stacks");
				}
				DirectoryReader dir = new DirectoryReader(path);
				if (!dir.FileExists("LUT.json") || !dir.FileExists("localeItemLUT.json") || !dir.FileExists("translation.json"))
				{
					local = false;
				}
				if (new DirectoryReader(path).FileExists("ignoredItemsList.json"))
				{
					string input = System.IO.File.ReadAllText(path + "/ignoredItemsList.json");
					ignoredItemList = JsonConvert.DeserializeObject<List<int>>(input);
				}
				if (dir.FileExists("version.json"))
				{
					int hostedVersion = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/version.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<int>(default(CancellationToken), (HttpCompletionOption)0);
					int localVersion = JsonConvert.DeserializeObject<int>(System.IO.File.ReadAllText(path + "/version.json"));
					if (hostedVersion > localVersion)
					{
						local = false;
						string output2 = JsonConvert.SerializeObject(hostedVersion);
						System.IO.File.WriteAllText(path + "/version.json", output2);
					}
				}
				else
				{
					local = false;
				}
				if (local)
				{
					Logger.Debug("Loading local LUT");
					string value = System.IO.File.ReadAllText(path + "/LUT.json");
					Magic.jsonLut = JsonConvert.DeserializeObject<LUT>(value);
					Magic.localeItemNamesLut = JsonConvert.DeserializeObject<localeLut>(System.IO.File.ReadAllText(path + "/localeItemLUT.json"));
					System.IO.File.ReadAllText(path + "/translation.json");
					Magic.translation = JsonConvert.DeserializeObject<Translation>(value);
				}
				else
				{
					Logger.Debug("Loading remote LUT");
					Magic.jsonLut = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/LUT.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<LUT>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.localeItemNamesLut = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/localeItemLUT.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<localeLut>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.translation = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/translation.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Translation>(default(CancellationToken), (HttpCompletionOption)0);
					string output = JsonConvert.SerializeObject(Magic.jsonLut);
					System.IO.File.WriteAllText(path + "/LUT.json", output);
					output = JsonConvert.SerializeObject(Magic.localeItemNamesLut);
					System.IO.File.WriteAllText(path + "/localeItemLUT.json", output);
					output = JsonConvert.SerializeObject(Magic.translation);
					System.IO.File.WriteAllText(path + "/translation.json", output);
				}
				hasLut = true;
				Logger.Debug("Lut successfully parsed");
			}
			catch (Exception e_)
			{
				fatalError = true;
				hasLut = false;
				handle_error("Error when creating LUTs");
				Logger.Warn("Unexpected exception: can't create LUT @" + e_.StackTrace);
			}
		}

		private async Task load_textures()
		{
			border = ContentsManager.GetTexture("Textures\\MasterworkBorder.png");
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			includeConsumableSetting = settings.DefineSetting("includeConsumables", defaultValue: true, () => " include consumables", () => "toggle to include food and utility");
			localJson = settings.DefineSetting("localLut", defaultValue: false, () => "use a local item json", () => "will only have an effect if a LUT exists inside the gw2stacks folder");
			ignoreItemsFeature = settings.DefineSetting("ignoreItems", defaultValue: false, () => "blacklist", () => "enable the blacklist feature for item advice");
			displayType = settings.DefineSetting("UI version", "0", () => "", () => "Choose the UI version\n0 for classic gw2stacks\n1 for character based advice\n2 for item specific advice");
			showBag = settings.DefineSetting("Show bags", defaultValue: false, () => "", () => "Toggle showing bags in the inventory recreation");
			itemShortcut = settings.DefineSetting("Enable advice shortcuts", defaultValue: false, () => "", () => "Enable a shift+lclick shortcut for item advice (only works in mode 2)");
			displayType.SettingChanged += delegate
			{
				if (validData)
				{
					hide_windows();
				}
			};
			itemShortcut.SettingChanged += delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (!e.NewValue)
				{
					GameService.Input.Mouse.LeftMouseButtonPressed -= on_mouse_alt_click;
				}
			};
		}

		private void create_window()
		{
			gw2stacksWindow = new TabbedWindow2(AsyncTexture2D.FromAssetId(155997), new Microsoft.Xna.Framework.Rectangle(24, 30, 565, 630), new Microsoft.Xna.Framework.Rectangle(82, 30, 467, 600));
			gw2stacksWindow.Location = new Point(GameService.Graphics.SpriteScreen.Width / 4, GameService.Graphics.SpriteScreen.Height / 4);
			gw2stacksWindow.Hidden += delegate
			{
				ignoredItemsWindow?.Hide();
			};
			ignoredItemsWindow = new TabbedWindow2(AsyncTexture2D.FromAssetId(155997), new Microsoft.Xna.Framework.Rectangle(24, 30, 565, 630), new Microsoft.Xna.Framework.Rectangle(82, 30, 467, 600));
			ignoredItemsWindow.Location = new Point(GameService.Graphics.SpriteScreen.Width / 4 * 2, GameService.Graphics.SpriteScreen.Height / 4);
			ignoredView = new IgnoredView();
			ignoredItemsTab = new Tab(GameService.Content.GetTexture("155052"), () => ignoredView, Magic.adviceTypeNameMapping[Magic.AdviceType.stackAdvice]);
			ignoredItemsWindow.Tabs.Add(ignoredItemsTab);
			ignoredItemsWindow.Parent = GameService.Graphics.SpriteScreen;
			ignoredView.set_values(itemTextures, refresh_views, excludedItemIds);
			gw2stacksWindow.Parent = GameService.Graphics.SpriteScreen;
			adviceView = new AdviceTabView();
			adviceView.set_values(itemTextures, refresh_views, excludedItemIds);
			gw2stacksWindow.Tabs.Clear();
			create_name_tab_mapping();
			foreach (Tab tab in tabNameMapping.Keys)
			{
				gw2stacksWindow.Tabs.Add(tab);
			}
			gw2stacksWindow.TabChanged += on_tab_change;
			characterBasedWindow = new StandardWindow(AsyncTexture2D.FromAssetId(155985), new Microsoft.Xna.Framework.Rectangle(40, 26, 913, 691), new Microsoft.Xna.Framework.Rectangle(70, 71, 839, 605));
			characterBasedWindow.Hide();
			characterBasedWindow.Parent = GameService.Graphics.SpriteScreen;
			characterBasedWindow.Title = "Gw2stacks";
			characterBasedWindow.CanResize = true;
			GameService.Gw2Mumble.PlayerCharacter.NameChanged += on_character_change;
			itemView = new ItemView();
			itemView.set_values(itemTextures, combinedAdvice);
			fullCharacterView = new FullCharacterView();
			fullCharacterView.set_values(itemTextures);
			fullCharacterBagView = new FullCharacterBagView();
			fullCharacterBagView.set_values(itemTextures);
			showBag.SettingChanged += delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.NewValue)
				{
					characterBasedWindow.Show(fullCharacterBagView);
				}
				else
				{
					characterBasedWindow.Show(fullCharacterView);
				}
			};
		}

		private async void on_mouse_alt_click(object s_ = null, MouseEventArgs e_ = null)
		{
			if (GameService.Input.Keyboard.KeysDown.Contains(Keys.LeftShift))
			{
				Logger.Warn("registered click combo");
				_ = GameService.Input.Mouse.Position;
				await Task.Delay(50);
				Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.LSHIFT, sendToSystem: true);
				await Task.Delay(50);
				Blish_HUD.Controls.Intern.Keyboard.Press(VirtualKeyShort.LCONTROL, sendToSystem: true);
				Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.KEY_A, sendToSystem: true);
				await Task.Delay(50);
				Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.KEY_C, sendToSystem: true);
				await Task.Delay(50);
				Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.LCONTROL, sendToSystem: true);
				await Task.Delay(50);
				Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.BACK, sendToSystem: true);
				Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RETURN, sendToSystem: true);
				string entry = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
				Logger.Debug("Found entry: " + entry);
				itemView.set_search_string(entry);
				show_windows();
			}
		}

		private void create_values()
		{
			icon = new CornerIcon(AsyncTexture2D.FromAssetId(155052), "gw2stacks");
			icon.Parent = GameService.Graphics.SpriteScreen;
			icon.Click += async delegate
			{
				await on_click();
			};
			loadingSpinner = new LoadingSpinner
			{
				Size = new Point(48, 48)
			};
			loadingSpinner.Parent = GameService.Graphics.SpriteScreen;
			loadingSpinner.Hide();
			loadingSpinner.Enabled = false;
			model = new gw2stacks_blish.data.Model(Logger);
			api = new Gw2Api(Gw2ApiManager);
			icon.Show();
		}

		public void handle_error(string error_)
		{
			if (icon != null)
			{
				if (fatalError)
				{
					icon.Enabled = false;
				}
				CornerIcon cornerIcon = icon;
				cornerIcon.BasicTooltipText = cornerIcon.BasicTooltipText + "\n" + error_;
			}
		}

		private void update_tab_locale()
		{
			Logger.Debug("started tab locale update");
			Magic.set_locale(GameService.Overlay.UserLocale.Value);
			foreach (Tab tab in gw2stacksWindow.Tabs)
			{
				tab.Name = Magic.get_current_translated_string(tabNameMapping[tab]);
			}
			ignoredItemsTab.Name = Magic.get_current_translated_string("Ignored Items");
			Logger.Debug("ended tab locale update");
		}

		private void refresh_views(int id_, bool mainWindow_)
		{
			if (ignoredItemList.Contains(id_))
			{
				ignoredItemList.Remove(id_);
			}
			else if (mainWindow_)
			{
				ignoredItemList.Add(id_);
			}
			update_excluded();
			ignoredView.refresh();
			adviceView.refresh();
		}

		private void update_advice()
		{
			Logger.Debug("started advice update");
			adviceDictionary.Clear();
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.stackAdvice), model.get_stacks_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.vendorAdvice), model.get_vendor_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.rareSalvageAdvice), model.get_rare_salvage_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.craftLuckAdvice), model.get_craft_luck_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.deletableAdvice), model.get_just_delete_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.salvageAdvice), model.get_just_salvage_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.consumableAdvice), model.get_play_to_consume_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.gobblerAdvice), model.get_gobbler_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.karmaAdvice), model.get_karma_consumables_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.craftingAdvice), model.get_crafting_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.lwsAdvice), model.get_living_world_advice());
			adviceDictionary.Add(Magic.get_local_advice(Magic.AdviceType.miscAdvice), model.get_misc_advice());
			List<ItemForDisplay> fullAdvice = new List<ItemForDisplay>();
			foreach (List<ItemForDisplay> item2 in adviceDictionary.Values)
			{
				fullAdvice.AddRange(item2);
			}
			combinedAdvice.Clear();
			foreach (KeyValuePair<int, gw2stacks_blish.data.Item> entry in model?.items)
			{
				IEnumerable<ItemForDisplay> applicable2 = fullAdvice.Where((ItemForDisplay itemForDisplay) => itemForDisplay.applicable_to_id(entry.Key));
				combinedAdvice.Add(new CombinedItemForDisplay(entry.Value, applicable2.ToList()));
			}
			fullCharacterInventories.Clear();
			foreach (KeyValuePair<string, List<int?>> entry3 in model?.characterInventory)
			{
				List<ItemForDisplay> list = new List<ItemForDisplay>();
				foreach (int? id in entry3.Value)
				{
					if (!id.HasValue)
					{
						list.Add(new EmptyItemForDisplay());
						continue;
					}
					IEnumerable<ItemForDisplay> applicable = combinedAdvice.Where((ItemForDisplay item) => item.applicable_to_id(id.Value));
					if (applicable.Count() != 1)
					{
						Logger.Warn("Internal error while assigning combined advice to inventory: invalid count of " + applicable.Count());
					}
					list.Add(applicable.First());
				}
				fullCharacterInventories.Add(entry3.Key, list);
				Logger.Debug("Character: " + entry3.Key + " has " + list.Count + " items");
			}
			characterBags.Clear();
			foreach (KeyValuePair<string, List<InventoryBagSlot>> entry2 in model?.inventoryBags)
			{
				List<BagForDisplay> bagsForDisplay = new List<BagForDisplay>();
				foreach (InventoryBagSlot bag in entry2.Value)
				{
					Logger.Debug("Found bag of id " + bag.get_id() + " on character " + entry2.Key);
					if (bag.get_id() == 0)
					{
						bagsForDisplay.Add(new BagForDisplay(null, 0));
					}
					else
					{
						bagsForDisplay.Add(new BagForDisplay(bag.get_id(), bag.get_size()));
					}
				}
				characterBags.Add(entry2.Key, bagsForDisplay);
				Logger.Debug("Character: " + entry2.Key + " has " + bagsForDisplay.Count + " bag slots");
			}
			Logger.Debug("ended advice update");
		}

		public void show_windows()
		{
			int mode = 0;
			GameService.Input.Mouse.LeftMouseButtonPressed -= on_mouse_alt_click;
			try
			{
				mode = Convert.ToInt32(displayType.Value);
			}
			catch (Exception)
			{
				Logger.Warn("Invalid mode selection. Defaulting to mode 0");
				mode = 0;
			}
			switch (mode)
			{
			case 0:
				gw2stacksWindow.Show();
				if (ignoreItemsFlag)
				{
					ignoredItemsWindow.Show();
				}
				break;
			case 1:
				if (showBag.Value)
				{
					characterBasedWindow.Show(fullCharacterBagView);
				}
				else
				{
					characterBasedWindow.Show(fullCharacterView);
				}
				break;
			case 2:
				characterBasedWindow.Show(itemView);
				if (itemShortcut.Value)
				{
					GameService.Input.Mouse.LeftMouseButtonPressed += on_mouse_alt_click;
				}
				break;
			default:
				gw2stacksWindow.Show();
				if (ignoreItemsFlag)
				{
					ignoredItemsWindow.Show();
				}
				break;
			}
		}

		public void hide_windows()
		{
			characterBasedWindow?.Hide();
			ignoredItemsWindow?.Hide();
			gw2stacksWindow?.Hide();
		}

		private void validate_api(object s = null, ValueEventArgs<IEnumerable<TokenPermission>> e = null)
		{
			try
			{
				if (Gw2ApiManager.HasSubtoken)
				{
					if (Gw2ApiManager.HasPermissions(new List<TokenPermission>
					{
						TokenPermission.Account,
						TokenPermission.Characters,
						TokenPermission.Inventories,
						TokenPermission.Unlocks
					}))
					{
						fatalError = false;
						isApiAvailable = true;
						icon.Enabled = true;
						icon.BasicTooltipText = null;
						Logger.Debug("Api validated successfully");
						icon.Show();
					}
					else
					{
						isApiAvailable = false;
						handle_error("Missing API permissions");
						Logger.Warn("Missing Permissions");
					}
				}
				else
				{
					isApiAvailable = false;
					handle_error("No subtoken supplied");
					Logger.Warn("No subtoken supplied");
				}
			}
			catch (InvalidAccessTokenException e_4)
			{
				isApiAvailable = false;
				handle_error("Invalid access token");
				Logger.Warn("Invalid access token: " + e_4.Message);
			}
			catch (MissingScopesException e_3)
			{
				isApiAvailable = false;
				handle_error("Missing API scopes");
				Logger.Warn("Missing scopes: " + e_3.Message);
			}
			catch (RequestException e_2)
			{
				isApiAvailable = false;
				handle_error("API request exception");
				Logger.Warn("Request exception: " + e_2.Message);
			}
			catch (Exception e_)
			{
				isApiAvailable = false;
				handle_error("Unexpected API exception");
				Logger.Warn("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
			}
			if (!isApiAvailable)
			{
				string path = DirectoriesManager.GetFullDirectoryPath("gw2stacks");
				if (path == null)
				{
					path = DirectoryUtil.RegisterDirectory("gw2stacks");
				}
				if (!new DirectoryReader(path).FileExists("modelBackup.json"))
				{
					fatalError = true;
					Logger.Warn("model backup not found");
				}
			}
		}

		private async Task start_api_update()
		{
			if (!running && hasLut)
			{
				icon.Enabled = false;
				validData = false;
				hide_windows();
				loadingSpinner.Location = icon.Location;
				Logger.Debug("starting setup");
				model.includeConsumables = includeConsumableSetting.Value;
				loadingSpinner.Show();
				if (!isOnCooldown)
				{
					model?.reset_state();
					await (model?.setup(api));
				}
				else
				{
					Logger.Debug("on cooldown");
				}
				running = true;
			}
		}

		private async Task on_click()
		{
			Logger.Debug("Started on_click");
			validate_api();
			if (!fatalError)
			{
				try
				{
					await start_api_update();
					await save_model();
				}
				catch (RequestException requestE_)
				{
					Logger.Warn("Unexpected exception: GW2 API request exception @" + requestE_.StackTrace);
					Logger.Warn("Attempting to load local API backup");
					isApiAvailable = false;
				}
				catch (Exception e_2)
				{
					if (e_2.Source != "Gw2Sharp")
					{
						fatalError = true;
					}
					else
					{
						isApiAvailable = false;
					}
					handle_error("Unexpected backend exception");
					Logger.Warn("Unexpected exception: " + e_2.Message + " @" + e_2.StackTrace);
				}
			}
			if (!isApiAvailable && !fatalError)
			{
				try
				{
					await load_model();
				}
				catch (Exception e_)
				{
					fatalError = true;
					hasLut = false;
					handle_error("API error and error reading local model backup");
					Logger.Warn("Unexpected exception: Error reading local model backup @" + e_.StackTrace);
				}
			}
		}

		private void update_views(string tabName_)
		{
			Logger.Debug("started view update");
			gw2stacksWindow.Title = tabName_;
			adviceView.update(adviceDictionary[tabName_], tabName_, ignoreItemsFlag);
			ignoredItemsWindow.Title = Magic.get_current_translated_string("Ignored Items");
			ignoredView.update(Magic.get_current_translated_string("Ignored Items"));
			itemView.update();
			Logger.Debug("update views name found " + GameService.Gw2Mumble.PlayerCharacter.Name);
			if (fullCharacterInventories.ContainsKey(GameService.Gw2Mumble.PlayerCharacter.Name))
			{
				fullCharacterView.update(fullCharacterInventories[GameService.Gw2Mumble.PlayerCharacter.Name], GameService.Gw2Mumble.PlayerCharacter.Name, characterBags[GameService.Gw2Mumble.PlayerCharacter.Name]);
				fullCharacterBagView.update(fullCharacterInventories[GameService.Gw2Mumble.PlayerCharacter.Name], GameService.Gw2Mumble.PlayerCharacter.Name, characterBags[GameService.Gw2Mumble.PlayerCharacter.Name]);
				Logger.Debug("ended view update");
			}
		}

		private void on_character_change(object sender_, ValueEventArgs<string> e_)
		{
			string newName = e_.Value ?? "invalid name";
			if (fullCharacterInventories.ContainsKey(newName))
			{
				fullCharacterView.update(fullCharacterInventories[newName], newName, characterBags[newName]);
				fullCharacterBagView.update(fullCharacterInventories[newName], newName, characterBags[newName]);
			}
		}

		private void on_tab_change(object sender_, ValueChangedEventArgs<Tab> event_)
		{
			if (validData)
			{
				try
				{
					string tabName = event_.NewValue.Name;
					update_views(tabName);
				}
				catch (Exception e_)
				{
					fatalError = true;
					handle_error("Unexpected UI exception (tab change)");
					Logger.Warn("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
				}
			}
		}

		private void update_excluded()
		{
			if (hasLut)
			{
				excludedItemIds.Clear();
				if (ignoreItemsFeature.Value)
				{
					excludedItemIds.AddRange(ignoredItemList);
				}
			}
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			await load_LUT();
			await load_textures();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			try
			{
				create_window();
				create_values();
				Magic.log = Logger;
				if (!hasLut)
				{
					handle_error("Error when creating LUTs");
				}
			}
			catch (Exception e_)
			{
				fatalError = true;
				handle_error("Unexpected error while creating UI");
				Logger.Warn("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
			}
			validate_api();
			Gw2ApiManager.SubtokenUpdated += validate_api;
		}

		protected override void Update(GameTime gameTime)
		{
			loadingIntervalTicks += gameTime.ElapsedGameTime.Milliseconds;
			cooldownIntervalTicks += gameTime.ElapsedGameTime.Milliseconds;
			if (cooldownIntervalTicks > 300000.0)
			{
				if (isOnCooldown)
				{
					isOnCooldown = false;
					Logger.Debug("Cooldown over");
				}
				cooldownIntervalTicks = 0.0;
			}
			if (!(loadingIntervalTicks > 100.0))
			{
				return;
			}
			if (!validData && running && !fatalError && model.validData)
			{
				Logger.Debug("task finished");
				running = false;
				try
				{
					model.includeConsumables = includeConsumableSetting.Value;
					update_tab_locale();
					update_advice();
					update_excluded();
					ignoreItemsFlag = ignoreItemsFeature.Value;
					validData = true;
					update_views(gw2stacksWindow.SelectedTab.Name);
					loadingSpinner.Hide();
					icon.Enabled = true;
					show_windows();
					if (!isOnCooldown)
					{
						isOnCooldown = true;
						cooldownIntervalTicks = 0.0;
					}
				}
				catch (Exception e_)
				{
					fatalError = true;
					handle_error("Unexpected error updating UI");
					Logger.Warn("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
				}
			}
			if (fatalError)
			{
				hide_windows();
				loadingSpinner?.Hide();
			}
			loadingIntervalTicks = 0.0;
		}

		protected override void Unload()
		{
			gw2stacksWindow?.Dispose();
			ignoredItemsWindow?.Hide();
			characterBasedWindow?.Dispose();
			icon?.Dispose();
			loadingSpinner?.Dispose();
			try
			{
				string text = DirectoryUtil.RegisterDirectory("gw2stacks");
				System.IO.File.WriteAllText(contents: JsonConvert.SerializeObject(ignoredItemList), path: text + "/ignoredItemsList.json");
			}
			catch (Exception e_)
			{
				Logger.Warn("Unexpected exception: can't save ignored items @" + e_.StackTrace);
			}
			Gw2ApiManager.SubtokenUpdated -= validate_api;
			GameService.Input.Mouse.LeftMouseButtonPressed -= on_mouse_alt_click;
			GameService.Gw2Mumble.PlayerCharacter.NameChanged -= on_character_change;
		}
	}
}
