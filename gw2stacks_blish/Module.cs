using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Flurl.Http;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
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

		private TabbedWindow2 gw2stacks_root;

		private CornerIcon icon;

		private LoadingSpinner loadingSpinner;

		private double loadingIntervalTicks;

		private double cooldownIntervalTicks;

		private bool isOnCooldown;

		private bool validData;

		private bool running;

		private bool fatalError;

		private bool hasLut;

		private SettingEntry<bool> includeConsumableSetting;

		private SettingEntry<bool> localJson;

		private Dictionary<int, AsyncTexture2D> itemTextures;

		private Model model;

		private Gw2Api api;

		private Dictionary<string, List<ItemForDisplay>> adviceDictionary;

		private AdviceTabView adviceView;

		private Dictionary<Tab, string> tabNameMapping;

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
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.stackAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.stackAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.vendorAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.vendorAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.rareSalvageAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.rareSalvageAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.craftLuckAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.craftLuckAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.deletableAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.deletableAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.salvageAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.salvageAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.consumableAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.consumableAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.gobblerAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.gobblerAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.karmaAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.karmaAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.craftingAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.craftingAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.lwsAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.lwsAdvice]
				},
				{
					new Tab(GameService.Content.GetTexture("155052"), () => adviceView, Magic.adviceTypeNameMapping[Magic.AdviceType.miscAdvice]),
					Magic.adviceTypeNameMapping[Magic.AdviceType.miscAdvice]
				}
			};
		}

		private async Task load_LUT()
		{
			_ = 7;
			try
			{
				bool local = localJson.Value;
				string path = DirectoriesManager.GetFullDirectoryPath("gw2stacks");
				if (path == null)
				{
					path = DirectoryUtil.RegisterDirectory("gw2stacks");
				}
				else
				{
					DirectoryReader dir = new DirectoryReader(path);
					if (!dir.FileExists("LUT.json") || !dir.FileExists("localeItemLUT.json") || !dir.FileExists("chineseLocal.json") || !dir.FileExists("englishLocal.json") || !dir.FileExists("germanLocal.json") || !dir.FileExists("koreanLocal.json") || !dir.FileExists("spanishLocal.json") || !dir.FileExists("frenchLocal.json"))
					{
						local = false;
					}
				}
				if (local)
				{
					Logger.Info("Loading local LUT");
					Magic.jsonLut = JsonConvert.DeserializeObject<LUT>(System.IO.File.ReadAllText(path + "/LUT.json"));
					Magic.localeItemNamesLut = JsonConvert.DeserializeObject<localeLut>(System.IO.File.ReadAllText(path + "/localeItemLUT.json"));
					Magic.englishToChinese = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/chineseLocal.json"));
					Magic.englishToEnglish = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/englishLocal.json"));
					Magic.englishToGerman = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/germanLocal.json"));
					Magic.englishToKorean = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/koreanLocal.json"));
					Magic.englishToSpanish = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/spanishLocal.json"));
					Magic.englishToFrench = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(path + "/frenchLocal.json"));
				}
				else
				{
					Logger.Info("Loading remote LUT");
					Magic.jsonLut = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/LUT.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<LUT>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.localeItemNamesLut = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/localeItemLUT.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<localeLut>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToChinese = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/chineseLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToEnglish = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/englishLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToGerman = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/germanLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToKorean = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/koreanLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToSpanish = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/spanishLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
					Magic.englishToFrench = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/frenchLocal.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<Dictionary<string, string>>(default(CancellationToken), (HttpCompletionOption)0);
				}
				hasLut = true;
				Logger.Info("Lut successfully parsed");
			}
			catch (Exception e_)
			{
				fatalError = true;
				hasLut = false;
				Logger.Fatal("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			includeConsumableSetting = settings.DefineSetting("includeConsumables", defaultValue: true, () => " include consumables", () => "toggle to include food and utility");
			localJson = settings.DefineSetting("localLut", defaultValue: false, () => "use a local item json", () => "will only have an effect if a LUT exists inside the gw2stacks folder");
		}

		private void create_window()
		{
			gw2stacks_root = new TabbedWindow2(AsyncTexture2D.FromAssetId(155997), new Microsoft.Xna.Framework.Rectangle(24, 30, 565, 630), new Microsoft.Xna.Framework.Rectangle(82, 30, 467, 600));
			gw2stacks_root.Parent = GameService.Graphics.SpriteScreen;
			adviceView = new AdviceTabView();
			gw2stacks_root.Tabs.Clear();
			create_name_tab_mapping();
			foreach (Tab tab in tabNameMapping.Keys)
			{
				gw2stacks_root.Tabs.Add(tab);
			}
			gw2stacks_root.TabChanged += on_tab_change;
		}

		private void create_values()
		{
			itemTextures = new Dictionary<int, AsyncTexture2D>();
			adviceDictionary = new Dictionary<string, List<ItemForDisplay>>();
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
			model = new Model(Logger);
			api = new Gw2Api(Gw2ApiManager);
			icon.Show();
		}

		private void update_tab_locale()
		{
			Magic.set_locale(GameService.Overlay.UserLocale.Value);
			foreach (Tab tab in gw2stacks_root.Tabs)
			{
				tab.Name = Magic.get_current_translated_string(tabNameMapping[tab]);
			}
		}

		private void update_advice()
		{
			adviceDictionary = new Dictionary<string, List<ItemForDisplay>>();
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
		}

		private void validate_api()
		{
			try
			{
				if (Gw2ApiManager.HasSubtoken)
				{
					if (Gw2ApiManager.HasPermissions(new List<TokenPermission>
					{
						TokenPermission.Account,
						TokenPermission.Characters,
						TokenPermission.Inventories
					}))
					{
						fatalError = false;
						Logger.Info("Api validated successfully");
						icon.Show();
					}
					else
					{
						fatalError = true;
						Logger.Fatal("Missing Permissions");
					}
				}
				else
				{
					fatalError = true;
					Logger.Fatal("No subtoken supplied");
				}
			}
			catch (InvalidAccessTokenException e_4)
			{
				fatalError = true;
				Logger.Fatal("Invalid access token: " + e_4.Message);
			}
			catch (MissingScopesException e_3)
			{
				fatalError = true;
				Logger.Fatal("Missing scopes: " + e_3.Message);
			}
			catch (RequestException e_2)
			{
				fatalError = true;
				Logger.Fatal("Request exception: " + e_2.Message);
			}
			catch (Exception e_)
			{
				fatalError = true;
				Logger.Fatal("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
			}
		}

		private async Task start_api_update()
		{
			if (!running && hasLut)
			{
				icon.Enabled = false;
				validData = false;
				gw2stacks_root.Hide();
				loadingSpinner.Location = icon.Location;
				Logger.Info("starting setup");
				model.includeConsumables = includeConsumableSetting.Value;
				loadingSpinner.Show();
				if (!isOnCooldown)
				{
					model?.reset_state();
					await (model?.setup(api));
				}
				else
				{
					Logger.Info("on cooldown");
				}
				running = true;
			}
		}

		private async Task on_click()
		{
			validate_api();
			if (!fatalError)
			{
				try
				{
					await start_api_update();
				}
				catch (Exception e_)
				{
					fatalError = true;
					Logger.Fatal("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
				}
			}
		}

		private void update_views(string tabName_)
		{
			gw2stacks_root.Title = tabName_;
			adviceView.update(adviceDictionary[tabName_], tabName_, itemTextures);
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
					Logger.Fatal("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
				}
			}
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			await load_LUT();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			try
			{
				create_window();
				create_values();
			}
			catch (Exception e_2)
			{
				fatalError = true;
				Logger.Fatal("Unexpected exception: " + e_2.Message + " @" + e_2.StackTrace);
			}
			validate_api();
			Gw2ApiManager.SubtokenUpdated += delegate
			{
				validate_api();
			};
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
					Logger.Info("Cooldown over");
				}
				cooldownIntervalTicks = 0.0;
			}
			if (!(loadingIntervalTicks > 100.0))
			{
				return;
			}
			if (!validData && running && !fatalError && model.validData)
			{
				Logger.Info("task finished");
				running = false;
				try
				{
					model.includeConsumables = includeConsumableSetting.Value;
					update_tab_locale();
					update_advice();
					validData = true;
					update_views(gw2stacks_root.SelectedTab.Name);
					loadingSpinner.Hide();
					icon.Enabled = true;
					gw2stacks_root.Show();
					if (!isOnCooldown)
					{
						isOnCooldown = true;
						cooldownIntervalTicks = 0.0;
					}
				}
				catch (Exception e_)
				{
					fatalError = true;
					Logger.Fatal("Unexpected exception: " + e_.Message + " @" + e_.StackTrace);
				}
			}
			if (fatalError)
			{
				gw2stacks_root?.Hide();
				icon?.Hide();
				loadingSpinner?.Hide();
			}
			loadingIntervalTicks = 0.0;
		}

		protected override void Unload()
		{
			gw2stacks_root?.Dispose();
			icon?.Dispose();
			loadingSpinner?.Dispose();
		}
	}
}
