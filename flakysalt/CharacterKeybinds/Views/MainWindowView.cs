using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Resources;
using flakysalt.CharacterKeybinds.Services;

namespace flakysalt.CharacterKeybinds.Views
{
	public class MainWindowView : View, IDisposable
	{
		private readonly TabbedWindow2 window;

		public CharacterKeybindsTab KeybindsTab { get; private set; }

		public KeybindMigrationTab MigrationTab { get; private set; }

		public Tab SelectedTab => window.get_SelectedTab();

		public event EventHandler<ValueChangedEventArgs<Tab>> TabChanged;

		public event EventHandler<EventArgs> WindowShown;

		public MainWindowView(ContentsManager contentsManager, AsyncTexture2D windowBackgroundTexture, Rectangle windowRegion, Rectangle contentRegion)
			: this()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			TabbedWindow2 val = new TabbedWindow2(windowBackgroundTexture, windowRegion, contentRegion);
			((WindowBase2)val).set_Emblem(contentsManager.GetTexture("images/logo.png"));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title(Loca.moduleName);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("flakysalt_CharacterKeybinds");
			((WindowBase2)val).set_CanClose(true);
			((WindowBase2)val).set_CanResize(true);
			((WindowBase2)val).set_SavesSize(true);
			window = val;
			InitializeTabs(contentsManager);
			((Control)window).add_Shown((EventHandler<EventArgs>)WindowShownEvent);
			window.add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)TabChangedEvent);
			((Control)window).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
			LocaService.Instance.LocaleChanged += delegate
			{
				((WindowBase2)window).set_Title(Loca.moduleName);
			};
			((Control)window).set_Size(new Point(670, 600));
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			int newWidth = MathHelper.Clamp(e.get_CurrentSize().X, 660, e.get_CurrentSize().X);
			int newHeight = MathHelper.Clamp(e.get_CurrentSize().Y, 250, e.get_CurrentSize().Y);
			((Control)window).set_Size(new Point(newWidth, newHeight));
		}

		private void TabChangedEvent(object sender, ValueChangedEventArgs<Tab> e)
		{
			this.TabChanged?.Invoke(sender, e);
		}

		private void WindowShownEvent(object sender, EventArgs e)
		{
			this.WindowShown?.Invoke(sender, e);
		}

		private void InitializeTabs(ContentsManager contentsManager)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			KeybindsTab = new CharacterKeybindsTab();
			MigrationTab = new KeybindMigrationTab();
			AsyncTexture2D obj = AsyncTexture2D.FromAssetId(784346);
			AsyncTexture2D.FromAssetId(157113);
			Tab keybindsTabItem = new Tab(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("images/Character_Keybinds_key_32.png")), (Func<IView>)(() => (IView)(object)KeybindsTab), "Character Keybinds", (int?)null);
			Tab migrationTabItem = new Tab(obj, (Func<IView>)(() => (IView)(object)MigrationTab), Loca.migration, (int?)null);
			CharacterKeybindsTab keybindsTab = KeybindsTab;
			keybindsTab.OnAddButtonClicked = (EventHandler)Delegate.Combine(keybindsTab.OnAddButtonClicked, (EventHandler)delegate
			{
				window.set_SelectedTab(keybindsTabItem);
			});
			window.get_Tabs().Add(keybindsTabItem);
			window.get_Tabs().Add(migrationTabItem);
			if (window.get_Tabs().get_Count() > 0)
			{
				window.set_SelectedTab(keybindsTabItem);
			}
		}

		public void Show()
		{
			((Control)window).Show();
		}

		public void ToggleWindow()
		{
			((WindowBase2)window).ToggleWindow();
		}

		public void Dispose()
		{
			((Control)window).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			window.remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)TabChangedEvent);
			((Control)window).remove_Shown((EventHandler<EventArgs>)WindowShownEvent);
			((Control)window).Dispose();
		}
	}
}
