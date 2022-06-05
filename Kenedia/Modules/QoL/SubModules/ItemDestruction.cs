using System;
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
		private enum State
		{
			Ready,
			Copying,
			Copied,
			Dragging,
			ReadyToPaste,
			Pasting,
			Pasted,
			Done
		}

		public SettingEntry<KeyBinding> Cancel_Key;

		private LoadingSpinner LoadingSpinner;

		private CursorSpinner CursorIcon;

		private DeleteIndicator DeleteIndicator;

		private State ModuleState;

		private Point MousePos = Point.get_Zero();

		private Point ItemPos = Point.get_Zero();

		private string _Instruction = common.ClickItem;

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
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			base.DefineSettings(settings);
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((ModifierKeys)1, (Keys)46), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			SettingCollection internal_settings = settings.AddSubCollection(Name + " Internal Settings", false, false);
			Cancel_Key = internal_settings.DefineSetting<KeyBinding>(Name + "Cancel_Key", new KeyBinding((Keys)27), (Func<string>)null, (Func<string>)null);
			Cancel_Key.get_Value().set_Enabled(true);
			Cancel_Key.get_Value().add_Activated((EventHandler<EventArgs>)Cancel_Key_Activated);
			ToggleModule_Key.get_Value().set_Enabled(true);
			ToggleModule_Key.get_Value().add_Activated((EventHandler<EventArgs>)ToggleModule_Key_Activated);
			Enabled = settings.DefineSetting<bool>(Name + "Enabled", true, (Func<string>)(() => $"Enable {Name}"), (Func<string>)null);
			ShowOnBar = settings.DefineSetting<bool>(Name + "ShowOnBar", true, (Func<string>)(() => string.Format("Show Icon", Name)), (Func<string>)null);
		}

		private void ToggleModule_Key_Activated(object sender, EventArgs e)
		{
			ToggleModule();
		}

		public override void ToggleModule()
		{
			base.ToggleModule();
			if (Loaded)
			{
				((Control)CursorIcon).set_Visible(base.Active);
				DeleteIndicator deleteIndicator = DeleteIndicator;
				if (deleteIndicator != null)
				{
					((Control)deleteIndicator).Hide();
				}
			}
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
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonReleased);
			LoadData();
		}

		private async void Mouse_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			if (base.Active && ModuleState == State.Dragging)
			{
				MouseState mouse = Mouse.GetState();
				if (MousePos.Distance2D(((MouseState)(ref mouse)).get_Position()) > 15)
				{
					await Paste();
				}
				else
				{
					ModuleState = State.Done;
				}
			}
		}

		private async void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (base.Active)
			{
				MouseState mouse = Mouse.GetState();
				KeyboardState keyboard = Keyboard.GetState();
				((Control)DeleteIndicator).set_Visible(false);
				if (ModuleState != State.Copying && ModuleState != State.Pasting && ((KeyboardState)(ref keyboard)).IsKeyDown((Keys)160))
				{
					ItemPos = ((MouseState)(ref mouse)).get_Position();
					Instruction = common.ThrowItem;
					await Copy();
				}
				if (ItemPos.Distance2D(((MouseState)(ref mouse)).get_Position()) > 100)
				{
					ModuleState = State.Done;
				}
				else if (ModuleState == State.ReadyToPaste)
				{
					ModuleState = State.Dragging;
				}
				mouse = default(MouseState);
			}
		}

		public override void LoadData()
		{
			Loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (ModuleState == State.Copied)
			{
				ModuleState = State.ReadyToPaste;
			}
			else if (ModuleState == State.Done || ModuleState == State.Pasted)
			{
				MousePos = Point.get_Zero();
				((Control)DeleteIndicator).set_Visible(false);
				Instruction = common.ClickItem;
				ModuleState = State.Ready;
				((Control)DeleteIndicator).set_Visible(false);
			}
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
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonPressed);
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)Mouse_LeftMouseButtonReleased);
			base.Dispose();
		}

		private void Cancel_Key_Activated(object sender, EventArgs e)
		{
			ModuleState = State.Done;
		}

		public async Task Paste()
		{
			ModuleState = State.Pasting;
			await Task.Delay(25);
			Keyboard.Press((VirtualKeyShort)162, true);
			Keyboard.Stroke((VirtualKeyShort)86, true);
			await Task.Delay(5);
			Keyboard.Release((VirtualKeyShort)162, true);
			ModuleState = State.Pasted;
		}

		public async Task Copy()
		{
			ModuleState = State.Copying;
			await Task.Delay(25);
			Keyboard.Release((VirtualKeyShort)160, true);
			await Task.Delay(5);
			Keyboard.Press((VirtualKeyShort)162, true);
			Keyboard.Stroke((VirtualKeyShort)65, true);
			await Task.Delay(5);
			Keyboard.Release((VirtualKeyShort)162, true);
			Keyboard.Press((VirtualKeyShort)162, true);
			Keyboard.Stroke((VirtualKeyShort)67, true);
			await Task.Delay(5);
			Keyboard.Release((VirtualKeyShort)162, true);
			Keyboard.Stroke((VirtualKeyShort)8, true);
			Keyboard.Stroke((VirtualKeyShort)13, true);
			string text = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync();
			if (text != null && text.Length > 0)
			{
				text = (text.StartsWith("[") ? text.Substring(1, text.Length - 1) : text);
				text = (text.EndsWith("]") ? text.Substring(0, text.Length - 1) : text);
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
			}
			ModuleState = State.Copied;
			((Control)DeleteIndicator).set_Visible(true);
		}
	}
}
