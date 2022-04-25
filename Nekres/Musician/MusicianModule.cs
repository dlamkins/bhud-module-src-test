using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nekres.Musician.Core.Player;
using Nekres.Musician.UI;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Views;

namespace Nekres.Musician
{
	[Export(typeof(Module))]
	public class MusicianModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(MusicianModule));

		internal static MusicianModule ModuleInstance;

		internal SettingEntry<float> audioVolume;

		internal SettingEntry<bool> stopWhenMoving;

		internal SettingEntry<KeyBinding> keySwapWeapons;

		internal SettingEntry<KeyBinding> keyWeaponSkill1;

		internal SettingEntry<KeyBinding> keyWeaponSkill2;

		internal SettingEntry<KeyBinding> keyWeaponSkill3;

		internal SettingEntry<KeyBinding> keyWeaponSkill4;

		internal SettingEntry<KeyBinding> keyWeaponSkill5;

		internal SettingEntry<KeyBinding> keyHealingSkill;

		internal SettingEntry<KeyBinding> keyUtilitySkill1;

		internal SettingEntry<KeyBinding> keyUtilitySkill2;

		internal SettingEntry<KeyBinding> keyUtilitySkill3;

		internal SettingEntry<KeyBinding> keyEliteSkill;

		internal SettingEntry<string> SheetFilter;

		private CornerIcon _moduleIcon;

		private StandardWindow _moduleWindow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal MusicPlayer MusicPlayer { get; private set; }

		internal MusicSheetService MusicSheetService { get; private set; }

		internal MusicSheetImporter MusicSheetImporter { get; private set; }

		[ImportingConstructor]
		public MusicianModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settingsManager)
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Expected O, but got Unknown
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			audioVolume = settingsManager.DefineSetting<float>("audioVolume", 80f, (Func<string>)(() => "Audio Volume"), (Func<string>)null);
			stopWhenMoving = settingsManager.DefineSetting<bool>("stopWhenMoving", true, (Func<string>)(() => "Stop When Moving"), (Func<string>)(() => "Stops any playback when you start moving."));
			SettingCollection skillKeyBindingsCollection = settingsManager.AddSubCollection("Skills", true, false);
			keySwapWeapons = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keySwapWeapons", new KeyBinding(Keys.OemPipe), (Func<string>)(() => "Swap Weapons"), (Func<string>)null);
			keyWeaponSkill1 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyWeaponSkill1", new KeyBinding(Keys.D1), (Func<string>)(() => "Weapon Skill 1"), (Func<string>)null);
			keyWeaponSkill2 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyWeaponSkill2", new KeyBinding(Keys.D2), (Func<string>)(() => "Weapon Skill 2"), (Func<string>)null);
			keyWeaponSkill3 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyWeaponSkill3", new KeyBinding(Keys.D3), (Func<string>)(() => "Weapon Skill 3"), (Func<string>)null);
			keyWeaponSkill4 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyWeaponSkill4", new KeyBinding(Keys.D4), (Func<string>)(() => "Weapon Skill 4"), (Func<string>)null);
			keyWeaponSkill5 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyWeaponSkill5", new KeyBinding(Keys.D5), (Func<string>)(() => "Weapon Skill 5"), (Func<string>)null);
			keyHealingSkill = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyHealingSkill", new KeyBinding(Keys.D6), (Func<string>)(() => "Healing Skill"), (Func<string>)null);
			keyUtilitySkill1 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyUtilitySkill1", new KeyBinding(Keys.D7), (Func<string>)(() => "Utility Skill 1"), (Func<string>)null);
			keyUtilitySkill2 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyUtilitySkill2", new KeyBinding(Keys.D8), (Func<string>)(() => "Utility Skill 2"), (Func<string>)null);
			keyUtilitySkill3 = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyUtilitySkill3", new KeyBinding(Keys.D9), (Func<string>)(() => "Utility Skill 3"), (Func<string>)null);
			keyEliteSkill = skillKeyBindingsCollection.DefineSetting<KeyBinding>("keyEliteSkill", new KeyBinding(Keys.D0), (Func<string>)(() => "Elite Skill"), (Func<string>)null);
			SettingCollection selfManagedSettings = settingsManager.AddSubCollection("selfManaged", false, false);
			SheetFilter = selfManagedSettings.DefineSetting<string>("sheetFilter", "Title", (Func<string>)null, (Func<string>)null);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			((Control)_moduleIcon).set_Visible(e.get_Value());
			if (!e.get_Value())
			{
				((Control)_moduleWindow).Hide();
			}
		}

		protected override void Initialize()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_moduleIcon = new CornerIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon.png")), ((Module)this).get_Name());
			MusicSheetService = new MusicSheetService(DirectoriesManager.GetFullDirectoryPath("musician"));
			MusicPlayer = new MusicPlayer();
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
		}

		protected override async Task LoadAsync()
		{
			await MusicSheetService.LoadAsync();
			MusicSheetImporter = new MusicSheetImporter(MusicSheetService, GetModuleProgressHandler());
		}

		private void UpdateModuleLoading(string loadingMessage)
		{
			_moduleIcon.set_LoadingMessage(loadingMessage);
		}

		public IProgress<string> GetModuleProgressHandler()
		{
			return new Progress<string>(UpdateModuleLoading);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			Rectangle windowRegion = new Rectangle(40, 26, 423, 724);
			Rectangle contentRegion = new Rectangle(70, 41, 380, 738);
			StandardWindow val = new StandardWindow(ContentsManager.GetTexture("background.png"), windowRegion, contentRegion);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Emblem(ContentsManager.GetTexture("musician_icon.png"));
			((Control)val).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - windowRegion.Width) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - windowRegion.Height) / 2));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id(Guid.NewGuid().ToString());
			((WindowBase2)val).set_Title(((Module)this).get_Name());
			_moduleWindow = val;
			((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			MusicSheetImporter.Init();
			((Module)this).OnModuleLoaded(e);
		}

		private void OnModuleIconClick(object o, MouseEventArgs e)
		{
			_moduleWindow.ToggleWindow((IView)(object)new LibraryView(new LibraryModel(MusicSheetService)));
		}

		protected override void Unload()
		{
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			((Control)_moduleIcon).remove_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			StandardWindow moduleWindow = _moduleWindow;
			if (moduleWindow != null)
			{
				((Control)moduleWindow).Dispose();
			}
			MusicPlayer?.Dispose();
			MusicSheetService?.Dispose();
			MusicSheetImporter?.Dispose();
			ModuleInstance = null;
		}
	}
}
