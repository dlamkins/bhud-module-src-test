using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.QoL.Classes;
using Kenedia.Modules.QoL.Strings;
using Kenedia.Modules.QoL.SubModules.ItemDesctruction.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.QoL.SubModules
{
	public class ItemDestruction : SubModule
	{
		public SettingEntry<KeyBinding> Cancel_Key;

		private LoadingSpinner LoadingSpinner;

		private CursorSpinner CursorIcon;

		private DeleteIndicator DeleteIndicator;

		private ButtonState MouseState;

		private Point MousePos = Point.get_Zero();

		private Point ItemPos = Point.get_Zero();

		private string _Instruction = common.ClickItem;

		private bool DeleteRunning;

		private bool DeletePrepared;

		public string Instruction
		{
			get
			{
				return _Instruction;
			}
			set
			{
				_Instruction = value;
				CursorIcon.Instruction = value;
			}
		}

		private void Cancel_Key_Activated(object sender, EventArgs e)
		{
			DeletePrepared = false;
		}

		public async void Paste()
		{
			await Task.Run(delegate
			{
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)65, true);
				Thread.Sleep(5);
				Keyboard.Stroke((VirtualKeyShort)86, true);
				Thread.Sleep(5);
				Keyboard.Release((VirtualKeyShort)162, true);
				DeletePrepared = false;
			});
		}

		public async void Copy()
		{
			DeleteRunning = true;
			await Task.Run(delegate
			{
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)65, true);
				Thread.Sleep(5);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Press((VirtualKeyShort)162, true);
				Keyboard.Stroke((VirtualKeyShort)67, true);
				Thread.Sleep(5);
				Keyboard.Release((VirtualKeyShort)162, true);
				Keyboard.Release((VirtualKeyShort)160, true);
				Thread.Sleep(5);
				Keyboard.Stroke((VirtualKeyShort)8, true);
				Keyboard.Stroke((VirtualKeyShort)13, true);
			});
			await Task.Run(delegate
			{
				string text = ClipboardUtil.get_WindowsClipboardService().GetTextAsync()?.Result;
				text = ((text.Length > 3) ? text.Substring(1, text.Length - 2) : "");
				if (text.Length > 0)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				}
				DeletePrepared = true;
			});
			DeleteRunning = false;
		}

		public ItemDestruction()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Name = "Item Desctruction";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("ItemDestruction", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("ItemDestruction", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("ItemDestruction", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("ItemDestruction", _Icons.ModuleIcon_Active_HoveredWhite);
			Initialize();
			LoadData();
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((ModifierKeys)1, (Keys)46), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			SettingCollection internal_settings = settings.AddSubCollection(Name + " Internal Settings", false, false);
			Cancel_Key = internal_settings.DefineSetting<KeyBinding>(Name + "Cancel_Key", new KeyBinding((Keys)27), (Func<string>)null, (Func<string>)null);
			Cancel_Key.get_Value().set_Enabled(true);
			Cancel_Key.get_Value().add_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
		}

		private void ToggleModule_Key_Activated(object sender, EventArgs e)
		{
			ToggleModule();
		}

		public override void ToggleModule()
		{
			base.ToggleModule();
			((Control)CursorIcon).set_Visible(Active);
		}

		public override void Initialize()
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			base.Initialize();
			CursorSpinner obj = new CursorSpinner
			{
				Name = Name
			};
			((Control)obj).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			obj.Background = QoL.ModuleInstance.TextureManager.getBackground(_Backgrounds.Tooltip);
			((Control)obj).set_Visible(false);
			obj.Instruction = common.ClickItem;
			CursorIcon = obj;
			string[] obj2 = new string[2]
			{
				common.ClickItem,
				common.ThrowItem
			};
			BitmapFont Font = GameService.Content.get_DefaultFont14();
			int width = 0;
			string[] array = obj2;
			foreach (string s in array)
			{
				width = Math.Max((int)Font.MeasureString(s).Width, width);
			}
			((Control)CursorIcon).set_Size(new Point(50 + width + 5, 50));
			DeleteIndicator deleteIndicator = new DeleteIndicator();
			((Control)deleteIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)deleteIndicator).set_Size(new Point(32, 32));
			((Control)deleteIndicator).set_Visible(false);
			deleteIndicator.Texture = QoL.ModuleInstance.TextureManager.getControl(_Controls.Delete);
			((Control)deleteIndicator).set_ClipsBounds(false);
			DeleteIndicator = deleteIndicator;
		}

		public override void LoadData()
		{
			Loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Invalid comparison between Unknown and I4
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Invalid comparison between Unknown and I4
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() || DeleteRunning)
			{
				return;
			}
			MouseState mouse = Mouse.GetState();
			KeyboardState keyboard = Keyboard.GetState();
			bool num = (int)MouseState == 1 && (int)((MouseState)(ref mouse)).get_LeftButton() == 0;
			if ((int)((MouseState)(ref mouse)).get_LeftButton() == 1 && (int)MouseState == 0)
			{
				if (ItemPos.Distance2D(((MouseState)(ref mouse)).get_Position()) < 50)
				{
					MousePos = ((MousePos == Point.get_Zero()) ? ((MouseState)(ref mouse)).get_Position() : MousePos);
				}
				else
				{
					DeletePrepared = false;
				}
			}
			if (num)
			{
				((Control)DeleteIndicator).set_Visible(false);
				if (((KeyboardState)(ref keyboard)).IsKeyDown((Keys)160))
				{
					ItemPos = ((MouseState)(ref mouse)).get_Position();
					((Control)DeleteIndicator).set_Visible(true);
					Instruction = common.ThrowItem;
					Copy();
				}
				else if (DeletePrepared)
				{
					if (MousePos.Distance2D(((MouseState)(ref mouse)).get_Position()) > 100)
					{
						Paste();
					}
					DeletePrepared = false;
					((Control)DeleteIndicator).set_Visible(false);
				}
				else
				{
					MousePos = Point.get_Zero();
					Instruction = common.ClickItem;
				}
			}
			MouseState = ((MouseState)(ref mouse)).get_LeftButton();
		}

		public override void UpdateLanguage(object sender, EventArgs e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateLanguage(sender, e);
			Instruction = common.ClickItem;
			string[] obj = new string[2]
			{
				common.ClickItem,
				common.ThrowItem
			};
			BitmapFont Font = GameService.Content.get_DefaultFont14();
			int width = 0;
			string[] array = obj;
			foreach (string s in array)
			{
				width = Math.Max((int)Font.MeasureString(s).Width, width);
			}
			((Control)CursorIcon).set_Size(new Point(50 + width + 5, 50));
		}

		public override void Dispose()
		{
			LoadingSpinner loadingSpinner = LoadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			CursorSpinner cursorIcon = CursorIcon;
			if (cursorIcon != null)
			{
				((Control)cursorIcon).Dispose();
			}
			DeleteIndicator deleteIndicator = DeleteIndicator;
			if (deleteIndicator != null)
			{
				((Control)deleteIndicator).Dispose();
			}
			Cancel_Key.get_Value().set_Enabled(false);
			Cancel_Key.get_Value().remove_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().set_Enabled(false);
			ToggleModule_Key.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			base.Dispose();
		}
	}
}
