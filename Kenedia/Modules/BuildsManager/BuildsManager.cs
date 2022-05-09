using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	[Export(typeof(Module))]
	public class BuildsManager : Module
	{
		internal static BuildsManager ModuleInstance;

		public static readonly Logger Logger = Logger.GetLogger<BuildsManager>();

		public const uint WM_COMMAND = 273u;

		public const uint WM_PASTE = 770u;

		public static TextureManager TextureManager;

		public static iPaths Paths;

		public static iData Data;

		public iTicks Ticks = new iTicks();

		public static List<int> ArmoryItems = new List<int>();

		public SettingEntry<bool> PasteOnCopy;

		public SettingEntry<bool> ShowCornerIcon;

		public SettingEntry<KeyBinding> ReloadKey;

		public SettingEntry<KeyBinding> ToggleWindow;

		public SettingEntry<int> GameVersion;

		public SettingEntry<string> ModuleVersion;

		public List<Template> Templates = new List<Template>();

		private Template _Selected_Template;

		public Window_MainWindow MainWindow;

		public LoadingSpinner loadingSpinner;

		public ProgressBar downloadBar;

		private CornerIcon cornerIcon;

		private static bool _DataLoaded;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public Template Selected_Template
		{
			get
			{
				return _Selected_Template;
			}
			set
			{
				if (_Selected_Template != null)
				{
					_Selected_Template.Edit -= OnSelected_Template_Edit;
				}
				_Selected_Template = value;
				if (value != null)
				{
					OnSelected_Template_Changed();
					_Selected_Template.Edit += OnSelected_Template_Edit;
				}
			}
		}

		public static bool DataLoaded
		{
			get
			{
				return _DataLoaded;
			}
			set
			{
				_DataLoaded = value;
				if (value)
				{
					ModuleInstance.OnDataLoaded();
				}
			}
		}

		public event EventHandler Selected_Template_Redraw;

		public event EventHandler Selected_Template_Edit;

		public event EventHandler Selected_Template_Changed;

		public event EventHandler Template_Deleted;

		public event EventHandler Templates_Loaded;

		public event EventHandler DataLoaded_Event;

		[ImportingConstructor]
		public BuildsManager([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		[DllImport("user32")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		public void OnSelected_Template_Redraw(object sender, EventArgs e)
		{
			this.Selected_Template_Redraw?.Invoke(this, EventArgs.Empty);
		}

		public void OnSelected_Template_Edit(object sender, EventArgs e)
		{
			this.Selected_Template_Edit?.Invoke(this, EventArgs.Empty);
		}

		public void OnSelected_Template_Changed()
		{
			this.Selected_Template_Changed?.Invoke(this, EventArgs.Empty);
		}

		public void OnTemplate_Deleted()
		{
			Selected_Template = new Template();
			LoadTemplates();
			this.Template_Deleted?.Invoke(this, EventArgs.Empty);
		}

		public void OnTemplates_Loaded()
		{
			LoadTemplates();
			this.Templates_Loaded?.Invoke(this, EventArgs.Empty);
		}

		private void OnDataLoaded()
		{
			this.DataLoaded_Event?.Invoke(this, EventArgs.Empty);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			ReloadKey = settings.DefineSetting<KeyBinding>("ReloadKey", new KeyBinding((Keys)0), (Func<string>)(() => "Reload Button"), (Func<string>)(() => ""));
			ToggleWindow = settings.DefineSetting<KeyBinding>("ToggleWindow", new KeyBinding((ModifierKeys)1, (Keys)66), (Func<string>)(() => "Toggle Window"), (Func<string>)(() => "Show / Hide the UI"));
			PasteOnCopy = settings.DefineSetting<bool>("PasteOnCopy", false, (Func<string>)(() => "Paste Stat/Upgrade Name"), (Func<string>)(() => "Paste Stat/Upgrade Name after copying it."));
			ShowCornerIcon = settings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show Corner Icon"), (Func<string>)(() => "Show / Hide the Corner Icon of this module."));
			SettingCollection internal_settings = settings.AddSubCollection("Internal Settings", false);
			GameVersion = internal_settings.DefineSetting<int>("GameVersion", 0, (Func<string>)null, (Func<string>)null);
			ModuleVersion = internal_settings.DefineSetting<string>("ModuleVersion", "0.0.0", (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			Logger.Info("Starting Builds Manager v." + (object)((Module)this).get_Version().BaseVersion());
			Paths = new iPaths(DirectoriesManager.GetFullDirectoryPath("builds-manager"));
			ArmoryItems.AddRange(new int[45]
			{
				80248, 80131, 80190, 80111, 80356, 80399, 80296, 80145, 80578, 80161,
				80252, 80281, 80384, 80435, 80254, 80205, 80277, 80557, 91505, 91536,
				81908, 91048, 91234, 93105, 95380, 74155, 30698, 30699, 30686, 30696,
				30697, 30695, 30684, 30702, 30687, 30690, 30685, 30701, 30691, 30688,
				30692, 30694, 30693, 30700, 30689
			});
			ReloadKey.get_Value().set_Enabled(true);
			ReloadKey.get_Value().add_Activated((EventHandler<EventArgs>)ReloadKey_Activated);
			ToggleWindow.get_Value().set_Enabled(true);
			ToggleWindow.get_Value().add_Activated((EventHandler<EventArgs>)ToggleWindow_Activated);
			DataLoaded = false;
		}

		private void ToggleWindow_Activated(object sender, EventArgs e)
		{
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((WindowBase2)mainWindow).ToggleWindow();
			}
		}

		private void ReloadKey_Activated(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification("Rebuilding the UI", (NotificationType)1, (Texture2D)null, 4);
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			CreateUI();
			((WindowBase2)MainWindow).ToggleWindow();
		}

		protected override async Task LoadAsync()
		{
		}

		public void LoadTemplates()
		{
			string currentTemplate = _Selected_Template?.Name;
			Templates = new List<Template>();
			List<string> list = Directory.GetFiles(Paths.builds, "*.json", SearchOption.AllDirectories).ToList();
			list.Sort((string a, string b) => a.CompareTo(b));
			foreach (string item in list)
			{
				Template template = new Template(item);
				Templates.Add(template);
				if (template.Name == currentTemplate)
				{
					_Selected_Template = template;
				}
			}
			_Selected_Template?.SetChanged();
			OnSelected_Template_Changed();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			TextureManager = new TextureManager(ContentsManager, DirectoriesManager);
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template)));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(ShowCornerIcon.get_Value());
			cornerIcon = val;
			((Control)cornerIcon).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template_White)));
			});
			((Control)cornerIcon).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template)));
			});
			((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (MainWindow != null)
				{
					((WindowBase2)MainWindow).ToggleWindow();
				}
			});
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(((Control)cornerIcon).get_Location().X - ((Control)cornerIcon).get_Width(), ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5));
			((Control)val2).set_Size(((Control)cornerIcon).get_Size());
			((Control)val2).set_Visible(false);
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			loadingSpinner = val2;
			ProgressBar progressBar = new ProgressBar();
			((Control)progressBar).set_Location(new Point(((Control)cornerIcon).get_Location().X, ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5 + 3));
			((Control)progressBar).set_Size(new Point(150, ((Control)cornerIcon).get_Height() - 6));
			((Control)progressBar).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			progressBar.Progress = 0.66;
			((Control)progressBar).set_Visible(false);
			downloadBar = progressBar;
			((Control)cornerIcon).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				((Control)loadingSpinner).set_Location(new Point(((Control)cornerIcon).get_Location().X - ((Control)cornerIcon).get_Width(), ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5));
				((Control)downloadBar).set_Location(new Point(((Control)cornerIcon).get_Location().X, ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5 + 3));
			});
			ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			((Module)this).OnModuleLoaded(e);
			DataLoaded_Event += delegate
			{
				CreateUI();
			};
			LoadData();
		}

		private void ShowCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (cornerIcon != null)
			{
				((Control)cornerIcon).set_Visible(e.get_NewValue());
			}
		}

		protected override void Update(GameTime gameTime)
		{
			Ticks.global += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (Ticks.global > 250.0)
			{
				Ticks.global -= 250.0;
			}
		}

		protected override void Unload()
		{
			((Control)MainWindow).Dispose();
			Data = null;
			TextureManager = null;
			((Control)cornerIcon).Dispose();
			ModuleInstance = null;
		}

		public static string getCultureString()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected I4, but got Unknown
			string culture = "en-EN";
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (value - 1) switch
			{
				2 => "fr-FR", 
				0 => "es-ES", 
				1 => "de-DE", 
				_ => "en-EN", 
			};
		}

		public async Task Fetch_APIData(bool force = false)
		{
			if (GameVersion.get_Value() == GameService.Gw2Mumble.get_Info().get_Version() && !(ModuleVersion.get_Value() != ((object)((Module)this).get_Version().BaseVersion()).ToString()) && !force)
			{
				return;
			}
			List<APIDownload_Image> downloadList = new List<APIDownload_Image>();
			string culture = getCultureString();
			double total2 = 0.0;
			double completed2 = 0.0;
			double progress = 0.0;
			List<int> _runes = JsonConvert.DeserializeObject<List<int>>(new StreamReader(ContentsManager.GetFileStream("data\\runes.json")).ReadToEnd());
			List<int> _sigils = JsonConvert.DeserializeObject<List<int>>(new StreamReader(ContentsManager.GetFileStream("data\\sigils.json")).ReadToEnd());
			Logger.Debug("Fetching all required Data from the API!");
			((Control)loadingSpinner).set_Visible(true);
			((Control)downloadBar).set_Visible(true);
			downloadBar.Progress = progress;
			downloadBar.Text = $"{completed2} / 9";
			IReadOnlyList<Item> sigils = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)_sigils, default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Sigils"));
			IReadOnlyList<Item> runes = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)_runes, default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Runes"));
			IReadOnlyList<Item> armory_items = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)ArmoryItems, default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Armory"));
			IApiV2ObjectList<Profession> professions = await ((IAllExpandableClient<Profession>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).AllAsync(default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Professions"));
			IApiV2ObjectList<Specialization> specs = await ((IAllExpandableClient<Specialization>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).AllAsync(default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Specs"));
			IApiV2ObjectList<Trait> traits = await ((IAllExpandableClient<Trait>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Traits()).AllAsync(default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Traits"));
			List<int> Skill_Ids = new List<int>();
			foreach (Profession profession2 in (IEnumerable<Profession>)professions)
			{
				Logger.Debug($"Checking {profession2.get_Name()} Skills");
				foreach (ProfessionSkill skill4 in profession2.get_Skills())
				{
					if (!Skill_Ids.Contains(skill4.get_Id()))
					{
						Skill_Ids.Add(skill4.get_Id());
					}
				}
			}
			Logger.Debug($"Fetching a total of {Skill_Ids.Count} Skills");
			IReadOnlyList<Skill> skills = await ((IBulkExpandableClient<Skill, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Skills()).ManyAsync((IEnumerable<int>)Skill_Ids, default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Skills"));
			IApiV2ObjectList<Itemstat> stats = await ((IAllExpandableClient<Itemstat>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Itemstats()).AllAsync(default(CancellationToken));
			completed2 += 1.0;
			downloadBar.Progress = completed2 / 9.0;
			downloadBar.Text = $"{completed2} / 9";
			Logger.Debug(string.Format("Fetched {0}", "Itemstats"));
			List<API.RuneItem> Runes = new List<API.RuneItem>();
			RenderUrl val;
			foreach (ItemUpgradeComponent item5 in runes)
			{
				ItemUpgradeComponent rune = item5;
				if (rune == null)
				{
					continue;
				}
				((Item)rune).get_Icon();
				val = ((Item)rune).get_Icon();
				if (((RenderUrl)(ref val)).get_Url() != null)
				{
					API.RuneItem obj = new API.RuneItem
					{
						Name = ((Item)rune).get_Name(),
						Id = ((Item)rune).get_Id(),
						ChatLink = ((Item)rune).get_ChatLink()
					};
					API.Icon icon = new API.Icon();
					val = ((Item)rune).get_Icon();
					icon.Url = ((RenderUrl)(ref val)).get_Url().ToString();
					icon.Path = Paths.rune_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)rune).get_Icon()), "[0-9]*.png");
					obj.Icon = icon;
					obj.Bonuses = rune.get_Details().get_Bonuses().ToList();
					API.RuneItem temp4 = obj;
					Runes.Add(temp4);
					if (!File.Exists(Paths.BasePath + temp4.Icon.Path))
					{
						downloadList.Add(new APIDownload_Image
						{
							display_text = $"Downloading Item Icon '{((Item)rune).get_Name()}'",
							url = temp4.Icon.Url,
							path = Paths.BasePath + temp4.Icon.Path
						});
					}
					total2 += 1.0;
				}
			}
			File.WriteAllText(Paths.runes + "runes [" + culture + "].json", JsonConvert.SerializeObject((object)Runes.ToArray()));
			List<API.SigilItem> Sigils = new List<API.SigilItem>();
			foreach (ItemUpgradeComponent item6 in sigils)
			{
				ItemUpgradeComponent sigil = item6;
				if (sigil == null)
				{
					continue;
				}
				((Item)sigil).get_Icon();
				val = ((Item)sigil).get_Icon();
				if (((RenderUrl)(ref val)).get_Url() != null)
				{
					API.SigilItem obj2 = new API.SigilItem
					{
						Name = ((Item)sigil).get_Name(),
						Id = ((Item)sigil).get_Id(),
						ChatLink = ((Item)sigil).get_ChatLink()
					};
					API.Icon icon2 = new API.Icon();
					val = ((Item)sigil).get_Icon();
					icon2.Url = ((RenderUrl)(ref val)).get_Url().ToString();
					icon2.Path = Paths.sigil_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)sigil).get_Icon()), "[0-9]*.png");
					obj2.Icon = icon2;
					obj2.Description = sigil.get_Details().get_InfixUpgrade().get_Buff()
						.get_Description();
					API.SigilItem temp5 = obj2;
					Sigils.Add(temp5);
					if (!File.Exists(Paths.BasePath + temp5.Icon.Path))
					{
						downloadList.Add(new APIDownload_Image
						{
							display_text = $"Downloading Item Icon '{((Item)sigil).get_Name()}'",
							url = temp5.Icon.Url,
							path = Paths.BasePath + temp5.Icon.Path
						});
					}
					((ApiV2BaseObject)sigil).set_HttpResponseInfo((ApiV2HttpResponseInfo)null);
					total2 += 1.0;
				}
			}
			File.WriteAllText(Paths.sigils + "sigils [" + culture + "].json", JsonConvert.SerializeObject((object)Sigils.ToArray()));
			List<API.Stat> Stats = new List<API.Stat>();
			foreach (Itemstat stat in (IEnumerable<Itemstat>)stats)
			{
				if (stat == null || Enum.GetName(typeof(_EquipmentStats), stat.get_Id()) == null)
				{
					continue;
				}
				API.Stat temp6 = new API.Stat
				{
					Name = stat.get_Name(),
					Id = stat.get_Id(),
					Icon = new API.Icon
					{
						Path = "textures\\stat icons\\" + stat.get_Id() + ".png"
					}
				};
				foreach (ItemstatAttribute attribute in stat.get_Attributes())
				{
					temp6.Attributes.Add(new API.StatAttribute
					{
						Id = (int)attribute.get_Attribute().get_Value(),
						Name = API.UniformAttributeName(attribute.get_Attribute().get_RawValue()),
						Multiplier = attribute.get_Multiplier(),
						Value = attribute.get_Value(),
						Icon = new API.Icon
						{
							Path = "textures\\stats\\" + (int)attribute.get_Attribute().get_Value() + ".png"
						}
					});
				}
				Stats.Add(temp6);
			}
			File.WriteAllText(Paths.stats + "stats [" + culture + "].json", JsonConvert.SerializeObject((object)Stats.ToArray()));
			List<API.ArmorItem> Armors = new List<API.ArmorItem>();
			List<API.WeaponItem> Weapons = new List<API.WeaponItem>();
			List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();
			foreach (Item i in armory_items)
			{
				if (i == null)
				{
					continue;
				}
				i.get_Icon();
				val = i.get_Icon();
				if (!(((RenderUrl)(ref val)).get_Url() != null))
				{
					continue;
				}
				if (i.get_Type().get_RawValue() == "Armor")
				{
					ItemArmor item = (ItemArmor)i;
					if (item != null)
					{
						API.ArmorItem obj3 = new API.ArmorItem
						{
							Name = ((Item)item).get_Name(),
							Id = ((Item)item).get_Id(),
							ChatLink = ((Item)item).get_ChatLink()
						};
						API.Icon icon3 = new API.Icon();
						val = ((Item)item).get_Icon();
						icon3.Url = ((RenderUrl)(ref val)).get_Url().ToString();
						icon3.Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)item).get_Icon()), "[0-9]*.png");
						obj3.Icon = icon3;
						obj3.AttributeAdjustment = item.get_Details().get_AttributeAdjustment();
						API.ArmorItem temp7 = obj3;
						Enum.TryParse<API.armorSlot>(item.get_Details().get_Type().get_RawValue(), out temp7.Slot);
						Enum.TryParse<API.armorWeight>(item.get_Details().get_WeightClass().get_RawValue(), out temp7.ArmorWeight);
						Armors.Add(temp7);
					}
				}
				if (i.get_Type().get_RawValue() == "Weapon")
				{
					ItemWeapon item2 = (ItemWeapon)i;
					if (item2 != null)
					{
						API.WeaponItem obj4 = new API.WeaponItem
						{
							Name = ((Item)item2).get_Name(),
							Id = ((Item)item2).get_Id(),
							ChatLink = ((Item)item2).get_ChatLink()
						};
						API.Icon icon4 = new API.Icon();
						val = ((Item)item2).get_Icon();
						icon4.Url = ((RenderUrl)(ref val)).get_Url().ToString();
						icon4.Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)item2).get_Icon()), "[0-9]*.png");
						obj4.Icon = icon4;
						obj4.AttributeAdjustment = item2.get_Details().get_AttributeAdjustment();
						API.WeaponItem temp9 = obj4;
						Enum.TryParse<API.weaponSlot>(item2.get_Details().get_Type().get_RawValue(), out temp9.Slot);
						Enum.TryParse<API.weaponType>(item2.get_Details().get_Type().get_RawValue(), out temp9.WeaponType);
						Weapons.Add(temp9);
					}
				}
				if (i.get_Type().get_RawValue() == "Trinket")
				{
					ItemTrinket item4 = (ItemTrinket)i;
					if (item4 != null)
					{
						API.TrinketItem obj5 = new API.TrinketItem
						{
							Name = ((Item)item4).get_Name(),
							Id = ((Item)item4).get_Id(),
							ChatLink = ((Item)item4).get_ChatLink()
						};
						API.Icon icon5 = new API.Icon();
						val = ((Item)item4).get_Icon();
						icon5.Url = ((RenderUrl)(ref val)).get_Url().ToString();
						icon5.Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)item4).get_Icon()), "[0-9]*.png");
						obj5.Icon = icon5;
						obj5.AttributeAdjustment = item4.get_Details().get_AttributeAdjustment();
						API.TrinketItem temp8 = obj5;
						Enum.TryParse<API.trinketType>(item4.get_Details().get_Type().get_RawValue(), out temp8.TrinketType);
						Trinkets.Add(temp8);
					}
				}
				if (i.get_Type().get_RawValue() == "Back")
				{
					ItemBack item3 = (ItemBack)i;
					if (item3 != null)
					{
						API.TrinketItem obj6 = new API.TrinketItem
						{
							Name = ((Item)item3).get_Name(),
							Id = ((Item)item3).get_Id(),
							ChatLink = ((Item)item3).get_ChatLink()
						};
						API.Icon icon6 = new API.Icon();
						val = ((Item)item3).get_Icon();
						icon6.Url = ((RenderUrl)(ref val)).get_Url().ToString();
						icon6.Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(((Item)item3).get_Icon()), "[0-9]*.png");
						obj6.Icon = icon6;
						obj6.TrinketType = API.trinketType.Back;
						obj6.AttributeAdjustment = item3.get_Details().get_AttributeAdjustment();
						Trinkets.Add(obj6);
					}
				}
				if (!File.Exists(Paths.armory_icons + Regex.Match(RenderUrl.op_Implicit(i.get_Icon()), "[0-9]*.png")))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Item Icon '{i.get_Name()}'",
						url = RenderUrl.op_Implicit(i.get_Icon()),
						path = Paths.armory_icons + Regex.Match(RenderUrl.op_Implicit(i.get_Icon()), "[0-9]*.png")
					});
				}
			}
			File.WriteAllText(Paths.armory + "armors [" + culture + "].json", JsonConvert.SerializeObject((object)Armors.ToArray()));
			File.WriteAllText(Paths.armory + "weapons [" + culture + "].json", JsonConvert.SerializeObject((object)Weapons.ToArray()));
			File.WriteAllText(Paths.armory + "trinkets [" + culture + "].json", JsonConvert.SerializeObject((object)Trinkets.ToArray()));
			Logger.Debug("Preparing Traits ....");
			List<API.Trait> Traits = new List<API.Trait>();
			foreach (Trait trait3 in (IEnumerable<Trait>)traits)
			{
				if (trait3 != null)
				{
					trait3.get_Icon();
					val = trait3.get_Icon();
					if (((RenderUrl)(ref val)).get_Url() != null)
					{
						API.Trait obj7 = new API.Trait
						{
							Name = trait3.get_Name(),
							Description = trait3.get_Description(),
							Id = trait3.get_Id()
						};
						API.Icon icon7 = new API.Icon();
						val = trait3.get_Icon();
						icon7.Url = ((RenderUrl)(ref val)).get_Url().ToString();
						icon7.Path = Paths.traits_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(trait3.get_Icon()), "[0-9]*.png");
						obj7.Icon = icon7;
						obj7.Specialization = trait3.get_Specialization();
						obj7.Tier = trait3.get_Tier();
						obj7.Order = trait3.get_Order();
						obj7.Type = (API.traitType)Enum.Parse(typeof(API.traitType), trait3.get_Slot().get_RawValue(), ignoreCase: true);
						Traits.Add(obj7);
					}
				}
			}
			Logger.Debug("Preparing Specializations ....");
			List<API.Specialization> Specializations = new List<API.Specialization>();
			foreach (Specialization spec in (IEnumerable<Specialization>)specs)
			{
				if (spec == null)
				{
					continue;
				}
				spec.get_Icon();
				val = spec.get_Icon();
				if (!(((RenderUrl)(ref val)).get_Url() != null))
				{
					continue;
				}
				API.Specialization obj8 = new API.Specialization
				{
					Name = spec.get_Name(),
					Id = spec.get_Id()
				};
				API.Icon icon8 = new API.Icon();
				val = spec.get_Icon();
				icon8.Url = ((RenderUrl)(ref val)).get_Url().ToString();
				icon8.Path = Paths.spec_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(spec.get_Icon()), "[0-9]*.png");
				obj8.Icon = icon8;
				API.Icon icon9 = new API.Icon();
				val = spec.get_Background();
				icon9.Url = ((RenderUrl)(ref val)).get_Url().ToString();
				icon9.Path = Paths.spec_backgrounds.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(spec.get_Background()), "[0-9]*.png");
				obj8.Background = icon9;
				object obj9;
				if (!spec.get_ProfessionIconBig().HasValue)
				{
					obj9 = null;
				}
				else
				{
					obj9 = new API.Icon
					{
						Url = spec.get_ProfessionIconBig().ToString()
					};
					object obj10 = obj9;
					string text = Paths.spec_icons.Replace(Paths.BasePath, "");
					RenderUrl? professionIconBig = spec.get_ProfessionIconBig();
					((API.Icon)obj10).Path = text + Regex.Match(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
				}
				obj8.ProfessionIconBig = (API.Icon)obj9;
				object obj11;
				if (!spec.get_ProfessionIcon().HasValue)
				{
					obj11 = null;
				}
				else
				{
					obj11 = new API.Icon
					{
						Url = spec.get_ProfessionIcon().ToString()
					};
					object obj12 = obj11;
					string text2 = Paths.spec_icons.Replace(Paths.BasePath, "");
					RenderUrl? professionIconBig = spec.get_ProfessionIcon();
					((API.Icon)obj12).Path = text2 + Regex.Match(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
				}
				obj8.ProfessionIcon = (API.Icon)obj11;
				obj8.Profession = spec.get_Profession();
				obj8.Elite = spec.get_Elite();
				API.Specialization temp3 = obj8;
				temp3.WeaponTrait = Traits.Find((API.Trait e) => e.Id == spec.get_WeaponTrait());
				if (temp3.WeaponTrait != null && !File.Exists(Paths.BasePath + temp3.WeaponTrait.Icon.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Trait Icon '{temp3.WeaponTrait.Name}'",
						url = temp3.WeaponTrait.Icon.Url,
						path = Paths.BasePath + temp3.WeaponTrait.Icon.Path
					});
				}
				foreach (int id3 in spec.get_MinorTraits())
				{
					API.Trait trait2 = Traits.Find((API.Trait e) => e.Id == id3);
					if (trait2 != null)
					{
						temp3.MinorTraits.Add(trait2);
						if (!File.Exists(Paths.BasePath + trait2.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Trait Icon '{trait2.Name}'",
								url = trait2.Icon.Url,
								path = Paths.BasePath + trait2.Icon.Path
							});
						}
					}
				}
				foreach (int id2 in spec.get_MajorTraits())
				{
					API.Trait trait = Traits.Find((API.Trait e) => e.Id == id2);
					if (trait != null)
					{
						temp3.MajorTraits.Add(trait);
						if (!File.Exists(Paths.BasePath + trait.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Trait Icon '{trait.Name}'",
								url = trait.Icon.Url,
								path = Paths.BasePath + trait.Icon.Path
							});
						}
					}
				}
				Specializations.Add(temp3);
			}
			Logger.Debug("Preparing Skills ....");
			List<API.Skill> Skills = new List<API.Skill>();
			foreach (Skill skill3 in skills)
			{
				if (skill3 == null)
				{
					continue;
				}
				skill3.get_Icon();
				val = skill3.get_Icon();
				if (!(((RenderUrl)(ref val)).get_Url() != null) || skill3.get_Professions().Count != 1)
				{
					continue;
				}
				API.Skill obj13 = new API.Skill
				{
					Name = skill3.get_Name(),
					Id = skill3.get_Id()
				};
				API.Icon icon10 = new API.Icon();
				val = skill3.get_Icon();
				icon10.Url = ((RenderUrl)(ref val)).get_Url().ToString();
				icon10.Path = Paths.skill_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(skill3.get_Icon()), "[0-9]*.png");
				obj13.Icon = icon10;
				obj13.ChatLink = skill3.get_ChatLink();
				obj13.Description = skill3.get_Description();
				obj13.Specialization = (skill3.get_Specialization().HasValue ? skill3.get_Specialization().Value : 0);
				obj13.Flags = (from e in skill3.get_Flags().ToList()
					select e.get_RawValue()).ToList();
				obj13.Categories = new List<string>();
				API.Skill temp2 = obj13;
				if (skill3.get_Categories() != null)
				{
					foreach (string category in skill3.get_Categories())
					{
						temp2.Categories.Add(category);
					}
				}
				Enum.TryParse<API.skillSlot>(skill3.get_Slot().get_RawValue(), out temp2.Slot);
				Skills.Add(temp2);
			}
			Logger.Debug("Preparing Professions ....");
			List<API.Profession> Professions = new List<API.Profession>();
			foreach (Profession profession in (IEnumerable<Profession>)professions)
			{
				if (profession == null)
				{
					continue;
				}
				profession.get_Icon();
				val = profession.get_Icon();
				if (!(((RenderUrl)(ref val)).get_Url() != null))
				{
					continue;
				}
				API.Profession obj14 = new API.Profession
				{
					Name = profession.get_Name(),
					Id = profession.get_Id()
				};
				API.Icon icon11 = new API.Icon();
				val = profession.get_Icon();
				icon11.Url = ((RenderUrl)(ref val)).get_Url().ToString();
				icon11.Path = Paths.profession_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(profession.get_Icon()), "[0-9]*.png");
				obj14.Icon = icon11;
				API.Icon icon12 = new API.Icon();
				val = profession.get_IconBig();
				icon12.Url = ((RenderUrl)(ref val)).get_Url().ToString();
				icon12.Path = Paths.profession_icons.Replace(Paths.BasePath, "") + Regex.Match(RenderUrl.op_Implicit(profession.get_IconBig()), "[0-9]*.png");
				obj14.IconBig = icon12;
				API.Profession temp = obj14;
				Logger.Debug("Adding Specs ....");
				foreach (int id in profession.get_Specializations())
				{
					API.Specialization spec2 = Specializations.Find((API.Specialization e) => e.Id == id);
					if (spec2 != null)
					{
						temp.Specializations.Add(spec2);
						if (!File.Exists(Paths.BasePath + spec2.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Specialization Icon '{spec2.Name}'",
								url = spec2.Icon.Url,
								path = Paths.BasePath + spec2.Icon.Path
							});
						}
						if (!File.Exists(Paths.BasePath + spec2.Background.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Background '{spec2.Name}'",
								url = spec2.Background.Url,
								path = Paths.BasePath + spec2.Background.Path
							});
						}
						if (spec2.ProfessionIcon != null && !File.Exists(Paths.BasePath + spec2.ProfessionIcon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading ProfessionIcon '{spec2.Name}'",
								url = spec2.ProfessionIcon.Url,
								path = Paths.BasePath + spec2.ProfessionIcon.Path
							});
						}
						if (spec2.ProfessionIconBig != null && !File.Exists(Paths.BasePath + spec2.ProfessionIconBig.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading ProfessionIconBig '{spec2.Name}'",
								url = spec2.ProfessionIconBig.Url,
								path = Paths.BasePath + spec2.ProfessionIconBig.Path
							});
						}
					}
				}
				Logger.Debug("Adding Weapons ....");
				foreach (KeyValuePair<string, ProfessionWeapon> weapon in profession.get_Weapons())
				{
					Enum.TryParse<API.weaponType>(weapon.Key, out var weaponType);
					temp.Weapons.Add(new API.ProfessionWeapon
					{
						Weapon = weaponType,
						Specialization = weapon.Value.get_Specialization(),
						Wielded = ((IEnumerable<ApiEnum<ProfessionWeaponFlag>>)weapon.Value.get_Flags()).Select((ApiEnum<ProfessionWeaponFlag> e) => (API.weaponHand)Enum.Parse(typeof(API.weaponHand), e.get_RawValue())).ToList()
					});
				}
				Logger.Debug("Adding Skills ....");
				List<iData.SkillID_Pair> SkillID_Pairs = JsonConvert.DeserializeObject<List<iData.SkillID_Pair>>(new StreamReader(ContentsManager.GetFileStream("data\\skillpalettes.json")).ReadToEnd());
				if (profession.get_Id() == "Revenant")
				{
					foreach (ProfessionSkill iSkill in profession.get_Skills())
					{
						API.Skill skill2 = Skills.Find((API.Skill e) => e.Id == iSkill.get_Id());
						iData.SkillID_Pair paletteID = SkillID_Pairs.Find((iData.SkillID_Pair e) => e.ID == iSkill.get_Id());
						if (skill2 != null && paletteID != null)
						{
							skill2.PaletteId = paletteID.PaletteID;
							temp.Skills.Add(skill2);
							if (!File.Exists(Paths.BasePath + skill2.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill2.Name}'",
									url = skill2.Icon.Url,
									path = Paths.BasePath + skill2.Icon.Path
								});
							}
						}
					}
				}
				else
				{
					foreach (KeyValuePair<int, int> skillIDs in profession.get_SkillsByPalette())
					{
						API.Skill skill = Skills.Find((API.Skill e) => e.Id == skillIDs.Value);
						if (skill != null)
						{
							skill.PaletteId = skillIDs.Key;
							temp.Skills.Add(skill);
							if (!File.Exists(Paths.BasePath + skill.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill.Name}'",
									url = skill.Icon.Url,
									path = Paths.BasePath + skill.Icon.Path
								});
							}
						}
					}
				}
				if (!File.Exists(Paths.BasePath + temp.Icon.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Profession Icon '{temp.Name}'",
						url = temp.Icon.Url,
						path = Paths.BasePath + temp.Icon.Path
					});
				}
				if (!File.Exists(Paths.BasePath + temp.IconBig.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Profession Icon '{temp.Name}'",
						url = temp.IconBig.Url,
						path = Paths.BasePath + temp.IconBig.Path
					});
				}
				Professions.Add(temp);
			}
			Logger.Debug("Saving Professions ....");
			File.WriteAllText(Paths.professions + "professions [" + culture + "].json", JsonConvert.SerializeObject((object)Professions.ToArray()));
			downloadBar.Progress = 0.0;
			total2 = downloadList.Count;
			completed2 = 0.0;
			Logger.Debug("All required Images queued. Downloading now ....");
			foreach (APIDownload_Image image in downloadList)
			{
				Logger.Debug("Downloading: '{0}' from url '{1}' to '{2}'", new object[3] { image.display_text, image.url, image.path });
				FileStream stream = new FileStream(image.path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
				await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToStreamAsync((Stream)stream, image.url, default(CancellationToken));
				stream.Close();
				completed2 += 1.0;
				progress = completed2 / total2;
				downloadBar.Progress = progress;
				downloadBar.Text = string.Format("{0} / {1} ({2})", completed2, downloadList.Count, Math.Round(progress * 100.0, 2) + "%");
				((Control)downloadBar).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._Label).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._BackgroundTexture).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._FilledTexture).set_BasicTooltipText(image.display_text);
			}
			((Control)loadingSpinner).set_Visible(false);
			((Control)downloadBar).set_Visible(false);
			GameVersion.set_Value(GameService.Gw2Mumble.get_Info().get_Version());
			ModuleVersion.set_Value(((object)((Module)this).get_Version().BaseVersion()).ToString());
		}

		private async Task LoadData()
		{
			await Fetch_APIData();
			Data = new iData(ContentsManager, DirectoriesManager);
		}

		private void CreateUI()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			LoadTemplates();
			Selected_Template = new Template();
			int Height = 670;
			int Width = 915;
			Window_MainWindow window_MainWindow = new Window_MainWindow(TextureManager.getBackground(_Backgrounds.MainWindow), new Rectangle(30, 30, Width, Height + 30), new Rectangle(30, 15, Width - 3, Height + 25));
			((Control)window_MainWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)window_MainWindow).set_Title("Builds Manager");
			((WindowBase2)window_MainWindow).set_Emblem(TextureManager._Emblems[0]);
			((WindowBase2)window_MainWindow).set_Subtitle("v." + ((object)((Module)this).get_Version().BaseVersion()).ToString());
			((WindowBase2)window_MainWindow).set_SavesPosition(true);
			((WindowBase2)window_MainWindow).set_Id("BuildsManager New");
			MainWindow = window_MainWindow;
			Selected_Template = Selected_Template;
		}
	}
}
