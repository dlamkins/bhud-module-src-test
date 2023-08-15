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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.ProofLogix.Core.Services;
using Nekres.ProofLogix.Core.UI;
using Nekres.ProofLogix.Core.UI.Configs;
using Nekres.ProofLogix.Core.UI.Home;
using Nekres.ProofLogix.Core.UI.LookingForOpener;
using Nekres.ProofLogix.Core.UI.SmartPing;
using Nekres.ProofLogix.Core.UI.Table;

namespace Nekres.ProofLogix
{
	[Export(typeof(Module))]
	public class ProofLogix : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<ProofLogix>();

		internal ResourceService Resources;

		internal KpWebApiService KpWebApi;

		internal PartySyncService PartySync;

		internal Gw2WebApiService Gw2WebApi;

		private TabbedWindow2 _window;

		private LockableAxisWindow _table;

		private StandardWindow _registerWindow;

		private StandardWindow _smartPing;

		private CornerIcon _cornerIcon;

		internal AsyncTexture2D Emblem;

		private AsyncTexture2D _icon;

		private AsyncTexture2D _hoverIcon;

		internal SettingEntry<LfoConfig> LfoConfig;

		internal SettingEntry<TableConfig> TableConfig;

		internal SettingEntry<SmartPingConfig> SmartPingConfig;

		internal SettingEntry<KeyBinding> ChatMessageKey;

		private SettingEntry<KeyBinding> _tableKey;

		private SettingEntry<KeyBinding> _smartPingKey;

		internal static ProofLogix Instance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public ProofLogix([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			SettingCollection keyBindings = settings.AddSubCollection("bindings", true, false, (Func<string>)(() => "Key Bindings"));
			_tableKey = keyBindings.DefineSetting<KeyBinding>("table_key", new KeyBinding((ModifierKeys)1, (Keys)75), (Func<string>)(() => "Party Table"), (Func<string>)(() => "Open or close the Party Table dialog."));
			_smartPingKey = keyBindings.DefineSetting<KeyBinding>("smart_ping_key", new KeyBinding((ModifierKeys)1, (Keys)76), (Func<string>)(() => "Smart Ping"), (Func<string>)(() => "Open or close the Smart Ping dialog."));
			ChatMessageKey = keyBindings.DefineSetting<KeyBinding>("chat_message_key", new KeyBinding((Keys)13), (Func<string>)(() => "Chat Message"), (Func<string>)(() => "Give focus to the chat edit box."));
			SettingCollection selfManaged = settings.AddSubCollection("configs", false, false);
			LfoConfig = selfManaged.DefineSetting<LfoConfig>("lfo_config", Nekres.ProofLogix.Core.UI.Configs.LfoConfig.Default, (Func<string>)null, (Func<string>)null);
			TableConfig = selfManaged.DefineSetting<TableConfig>("table_config", Nekres.ProofLogix.Core.UI.Configs.TableConfig.Default, (Func<string>)null, (Func<string>)null);
			SmartPingConfig = selfManaged.DefineSetting<SmartPingConfig>("smart_ping_config", Nekres.ProofLogix.Core.UI.Configs.SmartPingConfig.Default, (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			Resources = new ResourceService();
			KpWebApi = new KpWebApiService();
			PartySync = new PartySyncService();
			Gw2WebApi = new Gw2WebApiService();
		}

		protected override async Task LoadAsync()
		{
			await Resources.LoadAsync();
			await PartySync.InitSquad();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Expected O, but got Unknown
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Expected O, but got Unknown
			GameService.ArcDps.get_Common().Activate();
			Emblem = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("emblem.png"));
			_icon = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("icon.png"));
			_hoverIcon = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("hover_icon.png"));
			CornerIcon val = new CornerIcon(_icon, _hoverIcon, "Kill Proof");
			val.set_MouseInHouse(true);
			val.set_Priority(236278055);
			_cornerIcon = val;
			TabbedWindow2 val2 = new TabbedWindow2(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(100, 36, 839, 605));
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val2).set_Title(((Module)this).get_Name());
			((WindowBase2)val2).set_Subtitle("Account");
			((WindowBase2)val2).set_Emblem(AsyncTexture2D.op_Implicit(Emblem));
			((WindowBase2)val2).set_Id("ProofLogix_KillProof_91702dd39f0340b5bd7883cc566e4f63");
			((WindowBase2)val2).set_CanResize(true);
			((WindowBase2)val2).set_SavesSize(true);
			((WindowBase2)val2).set_SavesPosition(true);
			((Control)val2).set_Width(700);
			((Control)val2).set_Height(600);
			((Control)val2).set_Visible(false);
			_window = val2;
			_window.get_Tabs().Add(new Tab(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(255369), (Func<IView>)(() => (IView)(object)new HomeView()), "Account", (int?)null));
			_window.get_Tabs().Add(new Tab(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156680), (Func<IView>)(() => (IView)((!Resources.HasLoaded()) ? ((object)new LoadingView("Service unavailable…", "Please, try again later.")) : ((object)new LfoView(LfoConfig.get_Value())))), "Looking for Opener", (int?)null));
			_window.add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			LockableAxisWindow lockableAxisWindow = new LockableAxisWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 36, 839, 605));
			((Control)lockableAxisWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)lockableAxisWindow).set_Width(1000);
			((Control)lockableAxisWindow).set_Height(500);
			((WindowBase2)lockableAxisWindow).set_Id("ProofLogix_Table_045b4a5441ac40ea93d98ae2021a8f0c");
			((WindowBase2)lockableAxisWindow).set_Title("Party Table");
			((WindowBase2)lockableAxisWindow).set_Subtitle(GetKeyCombinationString(_tableKey.get_Value()));
			((WindowBase2)lockableAxisWindow).set_CanResize(true);
			((WindowBase2)lockableAxisWindow).set_SavesSize(true);
			((WindowBase2)lockableAxisWindow).set_SavesPosition(true);
			((WindowBase2)lockableAxisWindow).set_CanCloseWithEscape(false);
			((Control)lockableAxisWindow).set_Visible(false);
			((WindowBase2)lockableAxisWindow).set_Emblem(AsyncTexture2D.op_Implicit(Emblem));
			_table = lockableAxisWindow;
			StandardWindow val3 = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 36, 839, 645));
			((Control)val3).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val3).set_Width(500);
			((Control)val3).set_Height(150);
			((WindowBase2)val3).set_Id("ProofLogix_SmartPing_1f4fa9243b014915bfb7af4be545cb7b");
			((WindowBase2)val3).set_Title("Smart Ping");
			((WindowBase2)val3).set_Subtitle(GetKeyCombinationString(_smartPingKey.get_Value()));
			((WindowBase2)val3).set_SavesPosition(true);
			((WindowBase2)val3).set_CanCloseWithEscape(false);
			((Control)val3).set_Visible(false);
			((WindowBase2)val3).set_Emblem(AsyncTexture2D.op_Implicit(Emblem));
			_smartPing = val3;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			_tableKey.get_Value().add_Activated((EventHandler<EventArgs>)OnTableKeyActivated);
			_tableKey.get_Value().add_BindingChanged((EventHandler<EventArgs>)OnTableKeyBindingChanged);
			_tableKey.get_Value().set_Enabled(true);
			_smartPingKey.get_Value().add_Activated((EventHandler<EventArgs>)OnSmartPingKeyActivated);
			_smartPingKey.get_Value().add_BindingChanged((EventHandler<EventArgs>)OnSmartPingKeyBindingChanged);
			_smartPingKey.get_Value().set_Enabled(true);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnSmartPingKeyBindingChanged(object sender, EventArgs e)
		{
			if (_smartPing != null)
			{
				((WindowBase2)_smartPing).set_Subtitle(GetKeyCombinationString(_smartPingKey.get_Value()));
			}
		}

		private void OnSmartPingKeyActivated(object sender, EventArgs e)
		{
			ToggleSmartPing();
		}

		private void OnTableKeyBindingChanged(object sender, EventArgs e)
		{
			if (_table != null)
			{
				((WindowBase2)_table).set_Subtitle(GetKeyCombinationString(_tableKey.get_Value()));
			}
		}

		private void OnTableKeyActivated(object sender, EventArgs e)
		{
			ToggleTable();
		}

		private string GetKeyCombinationString(KeyBinding keyBinding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if ((int)keyBinding.get_ModifierKeys() == 0)
			{
				if ((int)keyBinding.get_PrimaryKey() != 0)
				{
					return $"[{keyBinding.get_PrimaryKey()}]";
				}
				return string.Empty;
			}
			string modifierString = string.Empty;
			if ((keyBinding.get_ModifierKeys() & 1) != 0)
			{
				modifierString += "Ctrl + ";
			}
			if ((keyBinding.get_ModifierKeys() & 2) != 0)
			{
				modifierString += "Alt + ";
			}
			if ((keyBinding.get_ModifierKeys() & 4) != 0)
			{
				modifierString += "Shift + ";
			}
			string text = modifierString;
			Keys primaryKey = keyBinding.get_PrimaryKey();
			return "[" + text + ((object)(Keys)(ref primaryKey)).ToString() + "]";
		}

		public void ToggleRegisterWindow()
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			if (_registerWindow != null)
			{
				((Control)_registerWindow).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)_registerWindow).get_Width()) / 2);
				((Control)_registerWindow).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_registerWindow).get_Height()) / 2);
				((WindowBase2)_registerWindow).BringWindowToFront();
				((Control)_registerWindow).Show();
				return;
			}
			StandardWindow val = new StandardWindow(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 36, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Not Yet Registered");
			((WindowBase2)val).set_Subtitle("Kill Proof");
			((WindowBase2)val).set_CanResize(false);
			((Control)val).set_Width(700);
			((Control)val).set_Height(550);
			((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 700) / 2);
			((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 600) / 2);
			((WindowBase2)val).set_Emblem(Instance.ContentsManager.GetTexture("killproof_icon.png"));
			_registerWindow = val;
			_registerWindow.Show((IView)(object)new RegisterView());
		}

		public void ToggleTable()
		{
			if (!Resources.HasLoaded())
			{
				GameService.Content.PlaySoundEffectByName("error");
			}
			else
			{
				((StandardWindow)_table).ToggleWindow((IView)(object)new TableView(TableConfig.get_Value()));
			}
		}

		public void ToggleSmartPing()
		{
			if (!Resources.HasLoaded())
			{
				GameService.Content.PlaySoundEffectByName("error");
			}
			else if (!PartySync.LocalPlayer.HasKpProfile)
			{
				GameService.Content.PlaySoundEffectByName("error");
				ScreenNotification.ShowNotification("Smart Ping unavailable. Profile not yet loaded.", (NotificationType)2, (Texture2D)null, 4);
			}
			else if (PartySync.LocalPlayer.KpProfile.IsEmpty)
			{
				GameService.Content.PlaySoundEffectByName("error");
				ScreenNotification.ShowNotification("Smart Ping unavailable. Profile has no records.", (NotificationType)2, (Texture2D)null, 4);
			}
			else
			{
				_smartPing.ToggleWindow((IView)(object)new SmartPingView(SmartPingConfig.get_Value()));
			}
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			WindowBase2 wnd = (WindowBase2)((sender is WindowBase2) ? sender : null);
			if (wnd != null)
			{
				wnd.set_Subtitle(e.get_NewValue().get_Name());
			}
		}

		private void OnCornerIconClick(object sender, MouseEventArgs e)
		{
			((WindowBase2)_window).ToggleWindow();
		}

		protected override void Unload()
		{
			if (_smartPingKey != null)
			{
				_smartPingKey.get_Value().set_Enabled(false);
				_smartPingKey.get_Value().remove_BindingChanged((EventHandler<EventArgs>)OnSmartPingKeyBindingChanged);
				_smartPingKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnSmartPingKeyActivated);
			}
			if (_tableKey != null)
			{
				_tableKey.get_Value().set_Enabled(false);
				_tableKey.get_Value().remove_BindingChanged((EventHandler<EventArgs>)OnTableKeyBindingChanged);
				_tableKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnTableKeyActivated);
			}
			if (_window != null)
			{
				_window.remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
				((Control)_window).Dispose();
			}
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
				((Control)_cornerIcon).Dispose();
			}
			StandardWindow registerWindow = _registerWindow;
			if (registerWindow != null)
			{
				((Control)registerWindow).Dispose();
			}
			LockableAxisWindow table = _table;
			if (table != null)
			{
				((Control)table).Dispose();
			}
			AsyncTexture2D hoverIcon = _hoverIcon;
			if (hoverIcon != null)
			{
				hoverIcon.Dispose();
			}
			AsyncTexture2D icon = _icon;
			if (icon != null)
			{
				icon.Dispose();
			}
			AsyncTexture2D emblem = Emblem;
			if (emblem != null)
			{
				emblem.Dispose();
			}
			KpWebApi.Dispose();
			PartySync.Dispose();
			Resources.Dispose();
			Gw2WebApi.Dispose();
			TrackableWindow.Unset();
			Instance = null;
		}
	}
}
