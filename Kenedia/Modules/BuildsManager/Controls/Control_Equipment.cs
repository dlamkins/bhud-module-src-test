using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class Control_Equipment : Control
	{
		public double Scale;

		private readonly Texture2D _RuneTexture;

		private readonly List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();

		private List<API.ArmorItem> Armors = new List<API.ArmorItem>();

		private readonly List<API.WeaponItem> Weapons = new List<API.WeaponItem>();

		private readonly List<Texture2D> WeaponSlots = new List<Texture2D>();

		private readonly List<Texture2D> AquaticWeaponSlots = new List<Texture2D>();

		private readonly List<SelectionPopUp.SelectionEntry> Stats_Selection = new List<SelectionPopUp.SelectionEntry>();

		private readonly List<SelectionPopUp.SelectionEntry> Sigils_Selection = new List<SelectionPopUp.SelectionEntry>();

		private readonly List<SelectionPopUp.SelectionEntry> Runes_Selection = new List<SelectionPopUp.SelectionEntry>();

		private readonly List<SelectionPopUp.SelectionEntry> Weapons_Selection = new List<SelectionPopUp.SelectionEntry>();

		private List<string> Instructions = new List<string>();

		private string _Profession;

		public CustomTooltip CustomTooltip;

		public SelectionPopUp SelectionPopUp;

		public EventHandler Changed;

		public Template Template => BuildsManager.s_moduleInstance.Selected_Template;

		public Control_Equipment(Container parent)
			: this()
		{
			((Control)this).set_Parent(parent);
			_RuneTexture = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getEquipTexture(EquipmentTextures.Rune), 37, 37, 54, 54);
			Trinkets = new List<API.TrinketItem>();
			foreach (API.TrinketItem item5 in BuildsManager.s_moduleInstance.Data.Trinkets)
			{
				Trinkets.Add(item5);
			}
			WeaponSlots = new List<Texture2D>
			{
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[6],
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[7],
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[6],
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[7]
			};
			AquaticWeaponSlots = new List<Texture2D>
			{
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[9],
				BuildsManager.s_moduleInstance.TextureManager._EquipSlotTextures[9]
			};
			Weapons = new List<API.WeaponItem>();
			foreach (API.WeaponItem weapon in BuildsManager.s_moduleInstance.Data.Weapons)
			{
				Weapons.Add(weapon);
			}
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnClick);
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnRightClick);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			CustomTooltip = customTooltip;
			SelectionPopUp = new SelectionPopUp((Container)(object)GameService.Graphics.get_SpriteScreen())
			{
				CustomTooltip = CustomTooltip
			};
			SelectionPopUp.Changed += SelectionPopUp_Changed;
			foreach (API.RuneItem item4 in BuildsManager.s_moduleInstance.Data.Runes)
			{
				Runes_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item4,
					Texture = item4.Icon._AsyncTexture,
					Header = item4.Name,
					Content = item4.Bonuses
				});
			}
			foreach (API.SigilItem item3 in BuildsManager.s_moduleInstance.Data.Sigils)
			{
				Sigils_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item3,
					Texture = item3.Icon._AsyncTexture,
					Header = item3.Name,
					Content = new List<string> { item3.Description }
				});
			}
			foreach (API.WeaponItem item2 in Weapons)
			{
				Weapons_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item2,
					Texture = item2.Icon._AsyncTexture,
					Header = item2.WeaponType.getLocalName(),
					Content = new List<string> { string.Empty }
				});
			}
			foreach (API.Stat item in BuildsManager.s_moduleInstance.Data.Stats)
			{
				Stats_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item,
					Texture = item.Icon._AsyncTexture,
					Header = item.Name,
					Content = item.Attributes.Select((API.StatAttribute e) => "+ " + e.Name).ToList(),
					ContentTextures = item.Attributes.Select((API.StatAttribute e) => e.Icon._AsyncTexture).ToList()
				});
			}
			Instructions = common.GearTab_Tips.Split('\n').ToList();
			BuildsManager.s_moduleInstance.LanguageChanged += ModuleInstance_LanguageChanged;
			ProfessionChanged();
			UpdateLayout();
			BuildsManager.s_moduleInstance.Selected_Template_Changed += ModuleInstance_Selected_Template_Changed;
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			CustomTooltip customTooltip = CustomTooltip;
			if (customTooltip != null)
			{
				((Control)customTooltip).Dispose();
			}
			SelectionPopUp selectionPopUp = SelectionPopUp;
			if (selectionPopUp != null)
			{
				((Control)selectionPopUp).Dispose();
			}
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnClick);
			((Control)this).remove_RightMouseButtonPressed((EventHandler<MouseEventArgs>)OnRightClick);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
			SelectionPopUp.Changed -= SelectionPopUp_Changed;
			Runes_Selection.DisposeAll();
			Sigils_Selection.DisposeAll();
			Weapons.DisposeAll();
			Weapons_Selection.DisposeAll();
			Stats_Selection.DisposeAll();
			BuildsManager.s_moduleInstance.LanguageChanged -= ModuleInstance_LanguageChanged;
			BuildsManager.s_moduleInstance.Selected_Template_Changed -= ModuleInstance_Selected_Template_Changed;
		}

		protected override void OnShown(EventArgs e)
		{
			UpdateLayout();
		}

		private void SelectionPopUp_Changed(object sender, EventArgs e)
		{
			OnChanged();
		}

		private void ModuleInstance_LanguageChanged(object sender, EventArgs e)
		{
			Instructions = common.GearTab_Tips.Split('\n').ToList();
			foreach (SelectionPopUp.SelectionEntry item2 in Weapons_Selection)
			{
				API.WeaponItem item = (API.WeaponItem)item2.Object;
				item2.Header = item.WeaponType.getLocalName();
			}
			foreach (SelectionPopUp.SelectionEntry item3 in Stats_Selection)
			{
				API.Stat stat3 = (API.Stat)item3.Object;
				item3.Header = stat3.Name;
			}
			foreach (SelectionPopUp.SelectionEntry item4 in Runes_Selection)
			{
				API.RuneItem stat2 = (API.RuneItem)item4.Object;
				item4.Header = stat2.Name;
			}
			foreach (SelectionPopUp.SelectionEntry item5 in Sigils_Selection)
			{
				API.SigilItem stat = (API.SigilItem)item5.Object;
				item5.Header = stat.Name;
			}
		}

		private void ModuleInstance_Selected_Template_Changed(object sender, EventArgs e)
		{
			UpdateLayout();
		}

		private void OnChanged()
		{
			BuildsManager.s_moduleInstance.Selected_Template.SetChanged();
		}

		private void OnGlobalClick(object sender, MouseEventArgs m)
		{
			if (!((Control)this).get_MouseOver() && !((Control)SelectionPopUp).get_MouseOver())
			{
				((Control)SelectionPopUp).Hide();
			}
		}

		private void SetClipboard(string text)
		{
			if (text != string.Empty && text != null)
			{
				try
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
				}
				catch (ArgumentException)
				{
					ScreenNotification.ShowNotification("Failed to set the clipboard text!", (NotificationType)2, (Texture2D)null, 4);
				}
				catch
				{
				}
			}
		}

		private void OnRightClick(object sender, MouseEventArgs mouse)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			if (DateTime.Now.Subtract(SelectionPopUp.LastClick).TotalMilliseconds < 250.0)
			{
				return;
			}
			((Control)SelectionPopUp).Hide();
			if (((Enum)Control.get_Input().get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)2))
			{
				foreach (Weapon_TemplateItem item6 in Template.Gear.Weapons)
				{
					if (item6.Hovered)
					{
						bool canSelect = true;
						if (item6.Slot == EquipmentSlots.Weapon1_OffHand)
						{
							canSelect = Template.Gear.Weapons[0].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) != 1;
						}
						if (item6.Slot == EquipmentSlots.Weapon2_OffHand)
						{
							canSelect = Template.Gear.Weapons[2].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[2].WeaponType.ToString()) != 1;
						}
						if (canSelect)
						{
							((Control)SelectionPopUp).Show();
							((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
								.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item6.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
								.Y - ((Control)this).get_RelativeMousePosition().Y + item6.Bounds.Y - 1));
							SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Weapons;
							SelectionPopUp.List = Weapons_Selection;
							SelectionPopUp.Slot = item6.Slot;
							SelectionPopUp.SelectionTarget = item6;
						}
					}
				}
				foreach (AquaticWeapon_TemplateItem item5 in Template.Gear.AquaticWeapons)
				{
					if (item5.Hovered)
					{
						((Control)SelectionPopUp).Show();
						((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
							.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item5.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
							.Y - ((Control)this).get_RelativeMousePosition().Y + item5.Bounds.Y - 1));
						SelectionPopUp.SelectionType = SelectionPopUp.selectionType.AquaticWeapons;
						SelectionPopUp.List = Weapons_Selection;
						SelectionPopUp.Slot = item5.Slot;
						SelectionPopUp.SelectionTarget = item5;
					}
				}
				return;
			}
			string text = string.Empty;
			foreach (Weapon_TemplateItem item4 in Template.Gear.Weapons)
			{
				if (item4.Hovered && item4.Stat != null)
				{
					SetClipboard(item4.Stat.Name);
					text = item4.Stat.Name;
				}
				if (((Rectangle)(ref item4.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()) && item4.Sigil != null)
				{
					SetClipboard(item4.Sigil.Name);
					text = item4.Sigil.Name;
				}
			}
			foreach (AquaticWeapon_TemplateItem item3 in Template.Gear.AquaticWeapons)
			{
				if (item3.Hovered && item3.Stat != null)
				{
					SetClipboard(item3.Stat.Name);
					text = item3.Stat.Name;
				}
				if (item3.Sigils == null)
				{
					continue;
				}
				for (int i = 0; i < item3.Sigils.Count; i++)
				{
					if (item3.Sigils[i] != null)
					{
						Rectangle val = item3.SigilsBounds[i];
						if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
						{
							SetClipboard(item3.Sigils[i].Name);
							text = item3.Sigils[i].Name;
						}
					}
				}
			}
			foreach (Armor_TemplateItem item2 in Template.Gear.Armor)
			{
				if (item2.Hovered && item2.Stat != null)
				{
					SetClipboard(item2.Stat.Name);
					text = item2.Stat.Name;
				}
				if (((Rectangle)(ref item2.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()) && item2.Rune != null)
				{
					SetClipboard(item2.Rune.Name);
					text = item2.Rune.Name;
				}
			}
			foreach (TemplateItem item in Template.Gear.Trinkets)
			{
				if (item.Hovered && item.Stat != null)
				{
					SetClipboard(item.Stat.Name);
					text = item.Stat.Name;
				}
			}
			if (BuildsManager.s_moduleInstance.PasteOnCopy.get_Value())
			{
				Paste(text);
			}
		}

		public async void Paste(string text)
		{
			byte[] prevClipboardContent = await ClipboardUtil.get_WindowsClipboardService().GetAsUnicodeBytesAsync();
			await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text).ContinueWith(delegate(Task<bool> clipboardResult)
			{
				if (clipboardResult.IsFaulted)
				{
					BuildsManager.Logger.Warn((Exception)clipboardResult.Exception, "Failed to set clipboard text to {text}!", new object[1] { text });
				}
				else
				{
					Task.Run(delegate
					{
						Keyboard.Press((VirtualKeyShort)162, true);
						Keyboard.Stroke((VirtualKeyShort)65, true);
						Thread.Sleep(50);
						Keyboard.Stroke((VirtualKeyShort)86, true);
						Thread.Sleep(50);
						Keyboard.Release((VirtualKeyShort)162, true);
					}).ContinueWith(delegate(Task result)
					{
						if (result.IsFaulted)
						{
							BuildsManager.Logger.Warn((Exception)result.Exception, "Failed to paste {text}", new object[1] { text });
						}
						else if (prevClipboardContent != null)
						{
							ClipboardUtil.get_WindowsClipboardService().SetUnicodeBytesAsync(prevClipboardContent);
						}
					});
				}
			});
		}

		private void OnClick(object sender, MouseEventArgs m)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_0768: Unknown result type (might be due to invalid IL or missing references)
			//IL_076d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			if (DateTime.Now.Subtract(SelectionPopUp.LastClick).TotalMilliseconds < 250.0)
			{
				return;
			}
			((Control)SelectionPopUp).Hide();
			foreach (TemplateItem item4 in Template.Gear.Trinkets)
			{
				if (item4.Hovered)
				{
					((Control)SelectionPopUp).Show();
					((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item4.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item4.Bounds.Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Stats;
					SelectionPopUp.SelectionTarget = item4;
					SelectionPopUp.List = Stats_Selection;
				}
			}
			foreach (Armor_TemplateItem item3 in Template.Gear.Armor)
			{
				if (item3.Hovered)
				{
					((Control)SelectionPopUp).Show();
					((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item3.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item3.Bounds.Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Stats;
					SelectionPopUp.SelectionTarget = item3;
					SelectionPopUp.List = Stats_Selection;
				}
				if (((Rectangle)(ref item3.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					((Control)SelectionPopUp).Show();
					((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item3.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item3.Bounds.Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Runes;
					SelectionPopUp.SelectionTarget = item3;
					SelectionPopUp.List = Runes_Selection;
				}
			}
			foreach (Weapon_TemplateItem item2 in Template.Gear.Weapons)
			{
				if (item2.Hovered)
				{
					bool canSelect = true;
					if (item2.Slot == EquipmentSlots.Weapon1_OffHand)
					{
						canSelect = Template.Gear.Weapons[0].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) != 1;
					}
					if (item2.Slot == EquipmentSlots.Weapon2_OffHand)
					{
						canSelect = Template.Gear.Weapons[2].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[2].WeaponType.ToString()) != 1;
					}
					if (canSelect)
					{
						((Control)SelectionPopUp).Show();
						((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
							.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item2.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
							.Y - ((Control)this).get_RelativeMousePosition().Y + item2.Bounds.Y - 1));
						SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Stats;
						SelectionPopUp.SelectionTarget = item2;
						SelectionPopUp.List = Stats_Selection;
					}
				}
				if (((Rectangle)(ref item2.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					((Control)SelectionPopUp).Show();
					((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item2.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item2.Bounds.Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Sigils;
					SelectionPopUp.SelectionTarget = item2;
					SelectionPopUp.List = Sigils_Selection;
				}
			}
			foreach (AquaticWeapon_TemplateItem item in Template.Gear.AquaticWeapons)
			{
				if (item.Hovered)
				{
					((Control)SelectionPopUp).Show();
					((Control)SelectionPopUp).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X + ((Rectangle)(ref item.Bounds)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item.Bounds.Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.Stats;
					SelectionPopUp.SelectionTarget = item;
					SelectionPopUp.List = Stats_Selection;
				}
				Rectangle val = item.SigilsBounds[0];
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					((Control)SelectionPopUp).Show();
					SelectionPopUp selectionPopUp = SelectionPopUp;
					int num = Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X;
					val = item.SigilsBounds[1];
					((Control)selectionPopUp).set_Location(new Point(num + ((Rectangle)(ref val)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item.SigilsBounds[0].Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.AquaticSigils;
					SelectionPopUp.UpgradeIndex = 0;
					SelectionPopUp.SelectionTarget = item;
					SelectionPopUp.List = Sigils_Selection;
				}
				val = item.SigilsBounds[1];
				if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					((Control)SelectionPopUp).Show();
					SelectionPopUp selectionPopUp2 = SelectionPopUp;
					int num2 = Control.get_Input().get_Mouse().get_Position()
						.X - ((Control)this).get_RelativeMousePosition().X;
					val = item.SigilsBounds[1];
					((Control)selectionPopUp2).set_Location(new Point(num2 + ((Rectangle)(ref val)).get_Right() + 3, Control.get_Input().get_Mouse().get_Position()
						.Y - ((Control)this).get_RelativeMousePosition().Y + item.SigilsBounds[1].Y - 1));
					SelectionPopUp.SelectionType = SelectionPopUp.selectionType.AquaticSigils;
					SelectionPopUp.UpgradeIndex = 1;
					SelectionPopUp.SelectionTarget = item;
					SelectionPopUp.List = Sigils_Selection;
				}
			}
		}

		private void UpdateTemplate()
		{
		}

		private void ProfessionChanged()
		{
			string id = "Unkown";
			if (Template.Build.Profession != null)
			{
				_Profession = Template.Build.Profession.Id;
				id = Template.Build.Profession.Id;
			}
			API.armorWeight armorWeight2;
			switch (id)
			{
			case "Elementalist":
			case "Necromancer":
			case "Mesmer":
				armorWeight2 = API.armorWeight.Light;
				break;
			case "Ranger":
			case "Thief":
			case "Engineer":
				armorWeight2 = API.armorWeight.Medium;
				break;
			default:
				armorWeight2 = API.armorWeight.Heavy;
				break;
			}
			API.armorWeight armorWeight = armorWeight2;
			Armors = new List<API.ArmorItem>
			{
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem()
			};
			foreach (API.ArmorItem armor in BuildsManager.s_moduleInstance.Data.Armors)
			{
				if (armor.ArmorWeight == armorWeight)
				{
					Armors[(int)armor.Slot] = armor;
				}
			}
		}

		public void UpdateLayout()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).get_RelativeMousePosition();
			int offset = 1;
			int size = 48;
			int statSize = (int)((double)size / 1.5);
			if (((Control)CustomTooltip).get_Visible())
			{
				((Control)CustomTooltip).set_Visible(false);
			}
			int i = 0;
			foreach (TemplateItem item4 in Template.Gear.Trinkets)
			{
				if (item4 != null)
				{
					item4.Bounds = new Rectangle(offset, 5 + i * (size + 6), size, size);
					item4.UpgradeBounds = new Rectangle(offset + size + 8, 5 + i * (size + 6), size, size);
					item4.StatBounds = new Rectangle(offset + (size - statSize), 5 + i * (size + 6) + (size - statSize), statSize, statSize);
				}
				i++;
			}
			i = 0;
			offset += 90;
			foreach (Armor_TemplateItem item3 in Template.Gear.Armor)
			{
				if (item3 != null)
				{
					item3.Bounds = new Rectangle(offset, 5 + i * (size + 6), size, size);
					item3.UpgradeBounds = new Rectangle(offset + size + 8, 5 + i * (size + 6), size, size);
					item3.StatBounds = new Rectangle(offset + (size - statSize), 5 + i * (size + 6) + (size - statSize), statSize, statSize);
				}
				i++;
			}
			i = 0;
			offset += 150;
			foreach (Weapon_TemplateItem item2 in Template.Gear.Weapons)
			{
				if (item2 != null)
				{
					item2.Bounds = new Rectangle(offset, 5 + i * (size + 6), size, size);
					item2.UpgradeBounds = new Rectangle(offset + size + 8, 5 + i * (size + 6), size, size);
					item2.StatBounds = new Rectangle(offset + (size - statSize), 5 + i * (size + 6) + (size - statSize), statSize, statSize);
				}
				if (i == 1)
				{
					i++;
				}
				i++;
			}
			i = 0;
			offset += 150;
			foreach (AquaticWeapon_TemplateItem item in Template.Gear.AquaticWeapons)
			{
				if (item != null)
				{
					item.Bounds = new Rectangle(offset, 5 + i * (size + 6), size, size);
					item.UpgradeBounds = new Rectangle(offset + size + 8, 5 + i * (size + 6), size, size);
					for (int j = 0; j < 2; j++)
					{
						item.SigilsBounds[j] = new Rectangle(item.UpgradeBounds.X, item.UpgradeBounds.Y + 1 + item.UpgradeBounds.Height / 2 * j, item.UpgradeBounds.Width / 2 - 2, item.UpgradeBounds.Height / 2 - 2);
					}
					item.StatBounds = new Rectangle(offset + (size - statSize), 5 + i * (size + 6) + (size - statSize), statSize, statSize);
				}
				if (i == 0)
				{
					i += 2;
				}
				i++;
			}
		}

		private void UpdateStates()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0889: Unknown result type (might be due to invalid IL or missing references)
			Point mPos = ((Control)this).get_RelativeMousePosition();
			int offset = 1;
			_ = 48.0 / 1.5;
			if (((Control)CustomTooltip).get_Visible())
			{
				((Control)CustomTooltip).set_Visible(false);
			}
			int i = 0;
			foreach (TemplateItem item4 in Template.Gear.Trinkets)
			{
				if (item4 != null)
				{
					item4.Hovered = ((Rectangle)(ref item4.Bounds)).Contains(mPos);
					if (item4.Hovered && item4.Stat != null && (!((Control)SelectionPopUp).get_Visible() || !((Control)SelectionPopUp).get_MouseOver()))
					{
						((Control)CustomTooltip).set_Visible(true);
						if (CustomTooltip.CurrentObject != item4)
						{
							CustomTooltip.CurrentObject = item4;
							CustomTooltip.Header = item4.Stat.Name;
							CustomTooltip.TooltipContent = new List<string>();
							foreach (API.StatAttribute attribute4 in item4.Stat.Attributes)
							{
								CustomTooltip.TooltipContent.Add("+ " + ((double)attribute4.Value + Math.Round(attribute4.Multiplier * Trinkets[i].AttributeAdjustment)) + " " + attribute4.Name);
							}
						}
					}
				}
				i++;
			}
			i = 0;
			offset += 90;
			foreach (Armor_TemplateItem item3 in Template.Gear.Armor)
			{
				if (item3 != null)
				{
					item3.Hovered = ((Rectangle)(ref item3.Bounds)).Contains(mPos);
					if (!((Control)SelectionPopUp).get_Visible() || !((Control)SelectionPopUp).get_MouseOver())
					{
						if (((Rectangle)(ref item3.UpgradeBounds)).Contains(mPos) && item3.Rune != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item3.Rune)
							{
								CustomTooltip.CurrentObject = item3.Rune;
								CustomTooltip.Header = item3.Rune.Name;
								CustomTooltip.TooltipContent = item3.Rune.Bonuses;
							}
						}
						else if (item3.Hovered && item3.Stat != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item3)
							{
								CustomTooltip.CurrentObject = item3;
								CustomTooltip.Header = item3.Stat.Name;
								CustomTooltip.TooltipContent = new List<string>();
								foreach (API.StatAttribute attribute3 in item3.Stat.Attributes)
								{
									CustomTooltip.TooltipContent.Add("+ " + Math.Round(attribute3.Multiplier * Armors[i].AttributeAdjustment) + " " + attribute3.Name);
								}
							}
						}
					}
				}
				i++;
			}
			i = 0;
			offset += 150;
			foreach (Weapon_TemplateItem item2 in Template.Gear.Weapons)
			{
				if (item2 != null)
				{
					item2.Hovered = ((Rectangle)(ref item2.Bounds)).Contains(mPos);
					if (!((Control)SelectionPopUp).get_Visible() || !((Control)SelectionPopUp).get_MouseOver())
					{
						if (((Rectangle)(ref item2.UpgradeBounds)).Contains(mPos) && item2.Sigil != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item2.Sigil)
							{
								CustomTooltip.CurrentObject = item2.Sigil;
								CustomTooltip.Header = item2.Sigil.Name;
								CustomTooltip.TooltipContent = new List<string> { item2.Sigil.Description };
							}
						}
						else if (item2.Hovered && item2.Stat != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item2)
							{
								CustomTooltip.CurrentObject = item2;
								CustomTooltip.Header = item2.Stat.Name;
								CustomTooltip.TooltipContent = new List<string>();
								bool twoHanded = false;
								if (item2.Slot == EquipmentSlots.Weapon1_MainHand || item2.Slot == EquipmentSlots.Weapon1_OffHand)
								{
									twoHanded = Template.Gear.Weapons[0].WeaponType != API.weaponType.Unkown && (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) == 1;
								}
								if (item2.Slot == EquipmentSlots.Weapon2_MainHand || item2.Slot == EquipmentSlots.Weapon2_OffHand)
								{
									twoHanded = Template.Gear.Weapons[2].WeaponType != API.weaponType.Unkown && (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[2].WeaponType.ToString()) == 1;
								}
								API.WeaponItem weapon2 = (twoHanded ? Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Greatsword) : Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Axe));
								if (weapon2 != null)
								{
									foreach (API.StatAttribute attribute2 in item2.Stat.Attributes)
									{
										CustomTooltip.TooltipContent.Add("+ " + Math.Round(attribute2.Multiplier * weapon2.AttributeAdjustment) + " " + attribute2.Name);
									}
								}
							}
						}
					}
				}
				if (i == 1)
				{
					i++;
				}
				i++;
			}
			i = 0;
			offset += 150;
			foreach (AquaticWeapon_TemplateItem item in Template.Gear.AquaticWeapons)
			{
				if (item != null)
				{
					for (int j = 0; j < 2; j++)
					{
						item.SigilsBounds[j] = new Rectangle(item.UpgradeBounds.X, item.UpgradeBounds.Y + 1 + item.UpgradeBounds.Height / 2 * j, item.UpgradeBounds.Width / 2 - 2, item.UpgradeBounds.Height / 2 - 2);
						if ((((Control)SelectionPopUp).get_Visible() && ((Control)SelectionPopUp).get_MouseOver()) || item.Sigils == null)
						{
							continue;
						}
						Rectangle val = item.SigilsBounds[j];
						if (((Rectangle)(ref val)).Contains(mPos) && item.Sigils.Count > j && item.Sigils[j] != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item.Sigils[j])
							{
								CustomTooltip.CurrentObject = item.Sigils[j];
								CustomTooltip.Header = item.Sigils[j].Name;
								CustomTooltip.TooltipContent = new List<string> { item.Sigils[j].Description };
							}
						}
					}
					item.Hovered = ((Rectangle)(ref item.Bounds)).Contains(mPos);
					if ((!((Control)SelectionPopUp).get_Visible() || !((Control)SelectionPopUp).get_MouseOver()) && item.Hovered && item.Stat != null)
					{
						((Control)CustomTooltip).set_Visible(true);
						if (CustomTooltip.CurrentObject != item)
						{
							CustomTooltip.CurrentObject = item;
							CustomTooltip.Header = item.Stat.Name;
							CustomTooltip.TooltipContent = new List<string>();
							API.WeaponItem weapon = Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Greatsword);
							if (weapon != null)
							{
								foreach (API.StatAttribute attribute in item.Stat.Attributes)
								{
									CustomTooltip.TooltipContent.Add("+ " + Math.Round(attribute.Multiplier * weapon.AttributeAdjustment) + " " + attribute.Name);
								}
							}
						}
					}
				}
				if (i == 0)
				{
					i += 2;
				}
				i++;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0604: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0639: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0727: Unknown result type (might be due to invalid IL or missing references)
			//IL_072c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_0745: Unknown result type (might be due to invalid IL or missing references)
			//IL_074c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0754: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_079f: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0805: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0836: Unknown result type (might be due to invalid IL or missing references)
			//IL_0851: Unknown result type (might be due to invalid IL or missing references)
			//IL_085b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_086d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08df: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0900: Unknown result type (might be due to invalid IL or missing references)
			//IL_0903: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0915: Unknown result type (might be due to invalid IL or missing references)
			//IL_0945: Unknown result type (might be due to invalid IL or missing references)
			//IL_095f: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0976: Unknown result type (might be due to invalid IL or missing references)
			//IL_0982: Unknown result type (might be due to invalid IL or missing references)
			//IL_0988: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
			if (Template == null)
			{
				return;
			}
			if (Template.Build.Profession != null && _Profession != Template.Build.Profession.Id)
			{
				ProfessionChanged();
			}
			UpdateStates();
			Color itemColor = default(Color);
			((Color)(ref itemColor))._002Ector(75, 75, 75, 255);
			Color frameColor = default(Color);
			((Color)(ref frameColor))._002Ector(125, 125, 125, 255);
			int i = 0;
			foreach (Armor_TemplateItem item in Template.Gear.Armor)
			{
				if (item != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Armors.Count > i) ? Armors[i].Icon._AsyncTexture.get_Texture() : BuildsManager.s_moduleInstance.TextureManager._Icons[0], item.Bounds, (Rectangle?)((Armors.Count > i) ? Armors[i].Icon._AsyncTexture.get_Texture().get_Bounds() : BuildsManager.s_moduleInstance.TextureManager._Icons[0].get_Bounds()), itemColor, 0f, default(Vector2), (SpriteEffects)0);
					if (item.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(item.Stat.Icon._AsyncTexture), item.StatBounds, (Rectangle?)item.Stat.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item.UpgradeBounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item.Rune == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item.Rune == null) ? _RuneTexture : item.Rune.Icon._AsyncTexture.get_Texture(), item.UpgradeBounds, (Rectangle?)((item.Rune == null) ? _RuneTexture.get_Bounds() : item.Rune.Icon._AsyncTexture.get_Texture().get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				i++;
			}
			i = 0;
			foreach (TemplateItem item2 in Template.Gear.Trinkets)
			{
				if (item2 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item2.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Trinkets.Count > i) ? Trinkets[i].Icon._AsyncTexture.get_Texture() : BuildsManager.s_moduleInstance.TextureManager._Icons[0], item2.Bounds, (Rectangle?)((Trinkets.Count > i) ? Trinkets[i].Icon._AsyncTexture.get_Texture().get_Bounds() : BuildsManager.s_moduleInstance.TextureManager._Icons[0].get_Bounds()), itemColor, 0f, default(Vector2), (SpriteEffects)0);
					if (item2.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(item2.Stat.Icon._AsyncTexture), item2.StatBounds, (Rectangle?)item2.Stat.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
				}
				i++;
			}
			i = 0;
			foreach (Weapon_TemplateItem item4 in Template.Gear.Weapons)
			{
				if (item4 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item4.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item4.WeaponType != API.weaponType.Unkown) ? frameColor : Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item4.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item4.WeaponType].Icon._AsyncTexture.get_Texture() : WeaponSlots[i], item4.Bounds, (Rectangle?)((item4.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item4.WeaponType].Icon._AsyncTexture.get_Texture().get_Bounds() : WeaponSlots[i].get_Bounds()), (item4.WeaponType != API.weaponType.Unkown) ? itemColor : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (item4.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(item4.Stat.Icon._AsyncTexture), item4.StatBounds, (Rectangle?)item4.Stat.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item4.UpgradeBounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item4.Sigil == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item4.Sigil == null) ? _RuneTexture : item4.Sigil.Icon._AsyncTexture.get_Texture(), item4.UpgradeBounds, (Rectangle?)((item4.Sigil == null) ? _RuneTexture.get_Bounds() : item4.Sigil.Icon._AsyncTexture.get_Texture().get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				i++;
			}
			i = 0;
			foreach (AquaticWeapon_TemplateItem item3 in Template.Gear.AquaticWeapons)
			{
				if (item3 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item3.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item3.WeaponType != API.weaponType.Unkown) ? frameColor : Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item3.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item3.WeaponType].Icon._AsyncTexture.get_Texture() : AquaticWeaponSlots[i], item3.Bounds, (Rectangle?)((item3.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item3.WeaponType].Icon._AsyncTexture.get_Texture().get_Bounds() : AquaticWeaponSlots[i].get_Bounds()), (item3.WeaponType != API.weaponType.Unkown) ? itemColor : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (item3.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(item3.Stat.Icon._AsyncTexture), item3.StatBounds, (Rectangle?)item3.Stat.Icon._AsyncTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					for (int j = 0; j < 2; j++)
					{
						API.SigilItem sigil = ((item3.Sigils != null && item3.Sigils.Count > j && item3.Sigils[j] != null && item3.Sigils[j].Id > 0) ? item3.Sigils[j] : null);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item3.SigilsBounds[j], new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (sigil == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (sigil == null) ? _RuneTexture : sigil.Icon._AsyncTexture.get_Texture(), item3.SigilsBounds[j], (Rectangle?)((sigil == null) ? _RuneTexture.get_Bounds() : sigil.Icon._AsyncTexture.get_Texture().get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
				}
				i++;
			}
			BitmapFont font = GameService.Content.get_DefaultFont14();
			TemplateItem lastTrinket = Template.Gear.Trinkets[Template.Gear.Trinkets.Count - 1];
			Texture2D texture = BuildsManager.s_moduleInstance.TextureManager.getEmblem(Emblems.QuestionMark);
			Texture2D mTexture = BuildsManager.s_moduleInstance.TextureManager.getIcon(Icons.Mouse);
			if (lastTrinket == null)
			{
				return;
			}
			int left = ((Rectangle)(ref lastTrinket.Bounds)).get_Left();
			Rectangle localBounds = ((Control)this).get_LocalBounds();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, new Rectangle(left, ((Rectangle)(ref localBounds)).get_Bottom() - font.get_LineHeight() * 3, font.get_LineHeight() * 3, font.get_LineHeight() * 3), (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			i = 0;
			foreach (string s in Instructions)
			{
				int num = ((Rectangle)(ref lastTrinket.Bounds)).get_Left() + font.get_LineHeight() * 3;
				localBounds = ((Control)this).get_LocalBounds();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, mTexture, new Rectangle(num, ((Rectangle)(ref localBounds)).get_Bottom() - font.get_LineHeight() * 3 + i * font.get_LineHeight(), font.get_LineHeight(), font.get_LineHeight()), (Rectangle?)mTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				int num2 = ((Rectangle)(ref lastTrinket.Bounds)).get_Left() + 5 + font.get_LineHeight() * 4;
				localBounds = ((Control)this).get_LocalBounds();
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, s, font, new Rectangle(num2, ((Rectangle)(ref localBounds)).get_Bottom() - font.get_LineHeight() * 3 + i * font.get_LineHeight(), 100, font.get_LineHeight()), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				i++;
			}
		}
	}
}
