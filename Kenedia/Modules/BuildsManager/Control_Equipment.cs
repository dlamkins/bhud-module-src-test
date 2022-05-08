using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_Equipment : Control
	{
		private Template _Template;

		public double Scale;

		private Texture2D _RuneTexture;

		private List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();

		private List<API.ArmorItem> Armors = new List<API.ArmorItem>();

		private List<API.WeaponItem> Weapons = new List<API.WeaponItem>();

		private List<Texture2D> WeaponSlots = new List<Texture2D>();

		private List<Texture2D> AquaticWeaponSlots = new List<Texture2D>();

		private List<SelectionPopUp.SelectionEntry> Stats_Selection = new List<SelectionPopUp.SelectionEntry>();

		private List<SelectionPopUp.SelectionEntry> Sigils_Selection = new List<SelectionPopUp.SelectionEntry>();

		private List<SelectionPopUp.SelectionEntry> Runes_Selection = new List<SelectionPopUp.SelectionEntry>();

		private List<SelectionPopUp.SelectionEntry> Weapons_Selection = new List<SelectionPopUp.SelectionEntry>();

		private List<string> Instructions = new List<string>();

		private string _Profession;

		public CustomTooltip CustomTooltip;

		public SelectionPopUp SelectionPopUp;

		public EventHandler Changed;

		public Template Template
		{
			get
			{
				return BuildsManager.ModuleInstance.Selected_Template;
			}
			set
			{
				if (value != null)
				{
					_Template = value;
					UpdateTemplate();
					UpdateLayout();
				}
			}
		}

		public Control_Equipment(Container parent, Template template)
			: this()
		{
			((Control)this).set_Parent(parent);
			_Template = template;
			_RuneTexture = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getEquipTexture(_EquipmentTextures.Rune), 37, 37, 54, 54);
			Trinkets = new List<API.TrinketItem>();
			foreach (API.TrinketItem item5 in BuildsManager.Data.Trinkets)
			{
				Trinkets.Add(item5);
			}
			WeaponSlots = new List<Texture2D>
			{
				BuildsManager.TextureManager._EquipSlotTextures[6],
				BuildsManager.TextureManager._EquipSlotTextures[7],
				BuildsManager.TextureManager._EquipSlotTextures[6],
				BuildsManager.TextureManager._EquipSlotTextures[7]
			};
			AquaticWeaponSlots = new List<Texture2D>
			{
				BuildsManager.TextureManager._EquipSlotTextures[9],
				BuildsManager.TextureManager._EquipSlotTextures[9]
			};
			Weapons = new List<API.WeaponItem>();
			foreach (API.WeaponItem weapon in BuildsManager.Data.Weapons)
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
				CustomTooltip = CustomTooltip,
				Template = Template
			};
			SelectionPopUp selectionPopUp = SelectionPopUp;
			selectionPopUp.Changed = (EventHandler)Delegate.Combine(selectionPopUp.Changed, (EventHandler)delegate
			{
				OnChanged();
			});
			((Control)this).add_Disposed((EventHandler<EventArgs>)delegate
			{
				((Control)CustomTooltip).Dispose();
				((Control)SelectionPopUp).Dispose();
			});
			foreach (API.RuneItem item4 in BuildsManager.Data.Runes)
			{
				Runes_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item4,
					Texture = item4.Icon.Texture,
					Header = item4.Name,
					Content = item4.Bonuses
				});
			}
			foreach (API.SigilItem item3 in BuildsManager.Data.Sigils)
			{
				Sigils_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item3,
					Texture = item3.Icon.Texture,
					Header = item3.Name,
					Content = new List<string> { item3.Description }
				});
			}
			foreach (API.WeaponItem item2 in Weapons)
			{
				Weapons_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item2,
					Texture = item2.Icon.Texture,
					Header = item2.WeaponType.ToString(),
					Content = new List<string> { "" }
				});
			}
			foreach (API.Stat item in BuildsManager.Data.Stats)
			{
				Stats_Selection.Add(new SelectionPopUp.SelectionEntry
				{
					Object = item,
					Texture = item.Icon.Texture,
					Header = item.Name,
					Content = item.Attributes.Select((API.StatAttribute e) => "+ " + e.Name).ToList(),
					ContentTextures = item.Attributes.Select((API.StatAttribute e) => e.Icon.Texture).ToList()
				});
			}
			Instructions = new List<string> { "Left Click to select Stat/Upgrade", "Alt + Right Click to select Weapon", "Right Click to copy Stat/Upgrade Name" };
			((Control)this).add_Shown((EventHandler<EventArgs>)delegate
			{
				UpdateLayout();
			});
			ProfessionChanged();
			UpdateLayout();
			BuildsManager.ModuleInstance.Selected_Template_Changed += ModuleInstance_Selected_Template_Changed;
		}

		private void ModuleInstance_Selected_Template_Changed(object sender, EventArgs e)
		{
			UpdateLayout();
		}

		private void OnChanged()
		{
			BuildsManager.ModuleInstance.Selected_Template.Save();
			Changed?.Invoke(this, EventArgs.Empty);
		}

		private void OnGlobalClick(object sender, MouseEventArgs m)
		{
			if (!((Control)this).get_MouseOver() && !((Control)SelectionPopUp).get_MouseOver())
			{
				((Control)SelectionPopUp).Hide();
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
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
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
						if (item6.Slot == _EquipmentSlots.Weapon1_OffHand)
						{
							canSelect = Template.Gear.Weapons[0].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) != 1;
						}
						if (item6.Slot == _EquipmentSlots.Weapon2_OffHand)
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
			string text = "";
			foreach (Weapon_TemplateItem item4 in Template.Gear.Weapons)
			{
				if (item4.Hovered && item4.Stat != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item4.Stat.Name);
					text = item4.Stat.Name;
				}
				if (((Rectangle)(ref item4.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()) && item4.Sigil != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item4.Sigil.Name);
					text = item4.Sigil.Name;
				}
			}
			foreach (AquaticWeapon_TemplateItem item3 in Template.Gear.AquaticWeapons)
			{
				if (item3.Hovered && item3.Stat != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item3.Stat.Name);
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
							ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item3.Sigils[i].Name);
							text = item3.Sigils[i].Name;
						}
					}
				}
			}
			foreach (Armor_TemplateItem item2 in Template.Gear.Armor)
			{
				if (item2.Hovered && item2.Stat != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item2.Stat.Name);
					text = item2.Stat.Name;
				}
				if (((Rectangle)(ref item2.UpgradeBounds)).Contains(((Control)this).get_RelativeMousePosition()) && item2.Rune != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item2.Rune.Name);
					text = item2.Rune.Name;
				}
			}
			foreach (TemplateItem item in Template.Gear.Trinkets)
			{
				if (item.Hovered && item.Stat != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(item.Stat.Name);
					text = item.Stat.Name;
				}
			}
			if (BuildsManager.ModuleInstance.PasteOnCopy.get_Value())
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
					if (item2.Slot == _EquipmentSlots.Weapon1_OffHand)
					{
						canSelect = Template.Gear.Weapons[0].WeaponType == API.weaponType.Unkown || (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) != 1;
					}
					if (item2.Slot == _EquipmentSlots.Weapon2_OffHand)
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
			API.armorWeight armorWeight = API.armorWeight.Heavy;
			switch (id)
			{
			case "Elementalist":
			case "Necromancer":
			case "Mesmer":
				armorWeight = API.armorWeight.Light;
				break;
			case "Ranger":
			case "Thief":
			case "Engineer":
				armorWeight = API.armorWeight.Medium;
				break;
			default:
				armorWeight = API.armorWeight.Heavy;
				break;
			}
			Armors = new List<API.ArmorItem>
			{
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem(),
				new API.ArmorItem()
			};
			foreach (API.ArmorItem armor in BuildsManager.Data.Armors)
			{
				if (armor.ArmorWeight == armorWeight)
				{
					Armors[(int)armor.Slot] = armor;
				}
			}
			SelectionPopUp.Template = Template;
		}

		private void UpdateLayout()
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
			//IL_087a: Unknown result type (might be due to invalid IL or missing references)
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
							CustomTooltip.Content = new List<string>();
							foreach (API.StatAttribute attribute4 in item4.Stat.Attributes)
							{
								CustomTooltip.Content.Add("+ " + ((double)attribute4.Value + Math.Round(attribute4.Multiplier * Trinkets[i].AttributeAdjustment)) + " " + attribute4.Name);
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
								CustomTooltip.Content = item3.Rune.Bonuses;
							}
						}
						else if (item3.Hovered && item3.Stat != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item3)
							{
								CustomTooltip.CurrentObject = item3;
								CustomTooltip.Header = item3.Stat.Name;
								CustomTooltip.Content = new List<string>();
								foreach (API.StatAttribute attribute3 in item3.Stat.Attributes)
								{
									CustomTooltip.Content.Add("+ " + Math.Round(attribute3.Multiplier * Armors[i].AttributeAdjustment) + " " + attribute3.Name);
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
								CustomTooltip.Content = new List<string> { item2.Sigil.Description };
							}
						}
						else if (item2.Hovered && item2.Stat != null)
						{
							((Control)CustomTooltip).set_Visible(true);
							if (CustomTooltip.CurrentObject != item2)
							{
								CustomTooltip.CurrentObject = item2;
								CustomTooltip.Header = item2.Stat.Name;
								CustomTooltip.Content = new List<string>();
								bool twoHanded = false;
								if (item2.Slot == _EquipmentSlots.Weapon1_MainHand || item2.Slot == _EquipmentSlots.Weapon1_OffHand)
								{
									twoHanded = Template.Gear.Weapons[0].WeaponType != API.weaponType.Unkown && (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[0].WeaponType.ToString()) == 1;
								}
								if (item2.Slot == _EquipmentSlots.Weapon2_MainHand || item2.Slot == _EquipmentSlots.Weapon2_OffHand)
								{
									twoHanded = Template.Gear.Weapons[2].WeaponType != API.weaponType.Unkown && (int)Enum.Parse(typeof(API.weaponSlot), Template.Gear.Weapons[2].WeaponType.ToString()) == 1;
								}
								API.WeaponItem weapon2 = (twoHanded ? Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Greatsword) : Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Axe));
								if (weapon2 != null)
								{
									foreach (API.StatAttribute attribute2 in item2.Stat.Attributes)
									{
										CustomTooltip.Content.Add("+ " + Math.Round(attribute2.Multiplier * weapon2.AttributeAdjustment) + " " + attribute2.Name);
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
								CustomTooltip.CurrentObject = item;
								CustomTooltip.Header = item.Sigils[j].Name;
								CustomTooltip.Content = new List<string> { item.Sigils[j].Description };
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
							CustomTooltip.Content = new List<string>();
							API.WeaponItem weapon = Weapons.Find((API.WeaponItem e) => e.WeaponType == API.weaponType.Greatsword);
							if (weapon != null)
							{
								foreach (API.StatAttribute attribute in item.Stat.Attributes)
								{
									CustomTooltip.Content.Add("+ " + Math.Round(attribute.Multiplier * weapon.AttributeAdjustment) + " " + attribute.Name);
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
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0613: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Unknown result type (might be due to invalid IL or missing references)
			//IL_0751: Unknown result type (might be due to invalid IL or missing references)
			//IL_0774: Unknown result type (might be due to invalid IL or missing references)
			//IL_0788: Unknown result type (might be due to invalid IL or missing references)
			//IL_078f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0797: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_0870: Unknown result type (might be due to invalid IL or missing references)
			//IL_0875: Unknown result type (might be due to invalid IL or missing references)
			//IL_087a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0888: Unknown result type (might be due to invalid IL or missing references)
			//IL_088b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0897: Unknown result type (might be due to invalid IL or missing references)
			//IL_089d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0900: Unknown result type (might be due to invalid IL or missing references)
			//IL_0906: Unknown result type (might be due to invalid IL or missing references)
			//IL_093c: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09da: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
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
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Armors.Count > i) ? Armors[i].Icon.Texture : BuildsManager.TextureManager._Icons[0], item.Bounds, (Rectangle?)((Armors.Count > i) ? Armors[i].Icon.Texture.get_Bounds() : BuildsManager.TextureManager._Icons[0].get_Bounds()), itemColor, 0f, default(Vector2), (SpriteEffects)0);
					if (item.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, item.Stat.Icon.Texture, item.StatBounds, (Rectangle?)item.Stat.Icon.Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item.UpgradeBounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item.Rune == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item.Rune == null) ? _RuneTexture : item.Rune.Icon.Texture, item.UpgradeBounds, (Rectangle?)((item.Rune == null) ? _RuneTexture.get_Bounds() : item.Rune.Icon.Texture.get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				i++;
			}
			i = 0;
			foreach (TemplateItem item2 in Template.Gear.Trinkets)
			{
				if (item2 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item2.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Trinkets.Count > i) ? Trinkets[i].Icon.Texture : BuildsManager.TextureManager._Icons[0], item2.Bounds, (Rectangle?)((Trinkets.Count > i) ? Trinkets[i].Icon.Texture.get_Bounds() : BuildsManager.TextureManager._Icons[0].get_Bounds()), itemColor, 0f, default(Vector2), (SpriteEffects)0);
					if (item2.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, item2.Stat.Icon.Texture, item2.StatBounds, (Rectangle?)item2.Stat.Icon.Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
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
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item4.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item4.WeaponType].Icon.Texture : WeaponSlots[i], item4.Bounds, (Rectangle?)((item4.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item4.WeaponType].Icon.Texture.get_Bounds() : WeaponSlots[i].get_Bounds()), (item4.WeaponType != API.weaponType.Unkown) ? itemColor : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (item4.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, item4.Stat.Icon.Texture, item4.StatBounds, (Rectangle?)item4.Stat.Icon.Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item4.UpgradeBounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item4.Sigil == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item4.Sigil == null) ? _RuneTexture : item4.Sigil.Icon.Texture, item4.UpgradeBounds, (Rectangle?)((item4.Sigil == null) ? _RuneTexture.get_Bounds() : item4.Sigil.Icon.Texture.get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				i++;
			}
			i = 0;
			foreach (AquaticWeapon_TemplateItem item3 in Template.Gear.AquaticWeapons)
			{
				if (item3 != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item3.Bounds, new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (item3.WeaponType != API.weaponType.Unkown) ? frameColor : Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (item3.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item3.WeaponType].Icon.Texture : AquaticWeaponSlots[i], item3.Bounds, (Rectangle?)((item3.WeaponType != API.weaponType.Unkown) ? Weapons[(int)item3.WeaponType].Icon.Texture.get_Bounds() : AquaticWeaponSlots[i].get_Bounds()), (item3.WeaponType != API.weaponType.Unkown) ? itemColor : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (item3.Stat != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, item3.Stat.Icon.Texture, item3.StatBounds, (Rectangle?)item3.Stat.Icon.Texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					for (int j = 0; j < 2; j++)
					{
						API.SigilItem sigil = ((item3.Sigils != null && item3.Sigils.Count > j && item3.Sigils[j] != null && item3.Sigils[j].Id > 0) ? item3.Sigils[j] : null);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), RectangleExtension.Add(item3.SigilsBounds[j], new Rectangle(-1, -1, 2, 2)), (Rectangle?)Rectangle.get_Empty(), (sigil == null) ? Color.get_Transparent() : frameColor, 0f, default(Vector2), (SpriteEffects)0);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (sigil == null) ? _RuneTexture : sigil.Icon.Texture, item3.SigilsBounds[j], (Rectangle?)((sigil == null) ? _RuneTexture.get_Bounds() : sigil.Icon.Texture.get_Bounds()), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
				}
				i++;
			}
			BitmapFont font = new ContentService().GetFont((FontFace)0, (FontSize)14, (FontStyle)0);
			TemplateItem lastTrinket = Template.Gear.Trinkets[Template.Gear.Trinkets.Count - 1];
			Texture2D texture = BuildsManager.TextureManager.getEmblem(_Emblems.QuestionMark);
			Texture2D mTexture = BuildsManager.TextureManager.getIcon(_Icons.Mouse);
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