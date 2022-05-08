using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class SelectionPopUp : Control
	{
		public class SelectionEntry
		{
			public object Object;

			public Texture2D Texture;

			public string Header;

			public List<string> Content;

			public List<Texture2D> ContentTextures;

			public Rectangle TextureBounds;

			public Rectangle TextBounds;

			public List<Rectangle> ContentBounds;

			public Rectangle Bounds;

			public Rectangle AbsolutBounds;

			public bool Hovered;
		}

		public enum selectionType
		{
			Runes,
			Sigils,
			Stats,
			Weapons,
			AquaticSigils,
			AquaticWeapons,
			Profession
		}

		private class filterTag
		{
			public string text;

			public bool match;
		}

		private Texture2D Background;

		private TextBox FilterBox;

		public selectionType SelectionType;

		public List<SelectionEntry> List = new List<SelectionEntry>();

		public List<SelectionEntry> FilteredList = new List<SelectionEntry>();

		private object _SelectionTarget;

		public Template Template;

		public _EquipmentSlots Slot = _EquipmentSlots.Unkown;

		public int UpgradeIndex;

		private ContentService ContentService;

		private BitmapFont Font;

		private BitmapFont HeaderFont;

		private Scrollbar Scrollbar;

		public CustomTooltip CustomTooltip;

		public bool Clicked;

		public DateTime LastClick = DateTime.Now;

		private bool UpdateLayouts;

		public API.Profession SelectedProfession;

		public EventHandler Changed;

		public object SelectionTarget
		{
			get
			{
				return _SelectionTarget;
			}
			set
			{
				FilteredList = new List<SelectionEntry>();
				_SelectionTarget = value;
				UpdateLayouts = true;
			}
		}

		public SelectionPopUp(Container parent)
			: this()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			((Control)this).set_Parent(parent);
			((Control)this).set_Visible(false);
			((Control)this).set_ZIndex(997);
			((Control)this).set_Size(new Point(300, 500));
			Background = BuildsManager.TextureManager._Backgrounds[2];
			TextBox val = new TextBox();
			((Control)val).set_Parent(((Control)this).get_Parent());
			((TextInputBase)val).set_PlaceholderText("Search ...");
			((Control)val).set_Width(((Control)this).get_Width() - 6);
			((Control)val).set_ZIndex(998);
			((Control)val).set_Visible(false);
			FilterBox = val;
			((TextInputBase)FilterBox).add_TextChanged((EventHandler<EventArgs>)FilterBox_TextChanged);
			ContentService = new ContentService();
			Font = ContentService.GetFont((FontFace)0, (FontSize)14, (FontStyle)0);
			HeaderFont = ContentService.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				OnChanged();
			});
			((Control)this).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				((Control)FilterBox).set_Location(((Control)this).get_Location().Add(new Point(3, 4)));
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)FilterBox).set_Width(((Control)this).get_Width() - 6);
			});
			((Control)this).add_Hidden((EventHandler<EventArgs>)delegate
			{
				((Control)FilterBox).Hide();
				Clicked = false;
			});
			((Control)this).add_Shown((EventHandler<EventArgs>)delegate
			{
				((Control)FilterBox).Show();
				((TextInputBase)FilterBox).set_Focused(true);
				UpdateLayout();
				Clicked = false;
			});
			((Control)this).add_Disposed((EventHandler<EventArgs>)delegate
			{
				((Control)FilterBox).Dispose();
			});
		}

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			UpdateLayout();
		}

		private void OnChanged()
		{
			if (List == null || List.Count == 0 || !((Control)this).get_Visible())
			{
				return;
			}
			foreach (SelectionEntry entry in FilteredList)
			{
				if (!entry.Hovered)
				{
					continue;
				}
				switch (SelectionType)
				{
				case selectionType.Runes:
				{
					API.RuneItem rune = (API.RuneItem)entry.Object;
					((Armor_TemplateItem)SelectionTarget).Rune = rune;
					Template.Save();
					break;
				}
				case selectionType.Sigils:
				{
					API.SigilItem sigil = (API.SigilItem)entry.Object;
					((Weapon_TemplateItem)SelectionTarget).Sigil = sigil;
					Template.Save();
					break;
				}
				case selectionType.AquaticSigils:
				{
					API.SigilItem aquaSigil = (API.SigilItem)entry.Object;
					((AquaticWeapon_TemplateItem)SelectionTarget).Sigils[UpgradeIndex] = aquaSigil;
					Template.Save();
					break;
				}
				case selectionType.Profession:
					SelectedProfession = (API.Profession)entry.Object;
					((TextInputBase)FilterBox).set_Text((string)null);
					break;
				case selectionType.Stats:
				{
					API.Stat stat = (API.Stat)entry.Object;
					((TemplateItem)SelectionTarget).Stat = stat;
					Template.Save();
					break;
				}
				case selectionType.Weapons:
				{
					API.WeaponItem selectedWeapon = (API.WeaponItem)entry.Object;
					((Weapon_TemplateItem)SelectionTarget).WeaponType = selectedWeapon.WeaponType;
					switch (Slot)
					{
					case _EquipmentSlots.Weapon1_MainHand:
						if (selectedWeapon.Slot == API.weaponSlot.Greatsword && Template.Gear.Weapons[1].WeaponType != API.weaponType.Unkown)
						{
							Template.Gear.Weapons[1].WeaponType = API.weaponType.Unkown;
						}
						break;
					case _EquipmentSlots.Weapon2_MainHand:
						if (selectedWeapon.Slot == API.weaponSlot.Greatsword && Template.Gear.Weapons[3].WeaponType != API.weaponType.Unkown)
						{
							Template.Gear.Weapons[3].WeaponType = API.weaponType.Unkown;
						}
						break;
					}
					Template.Save();
					break;
				}
				case selectionType.AquaticWeapons:
				{
					API.WeaponItem selectedAquaWeapon = (API.WeaponItem)entry.Object;
					((AquaticWeapon_TemplateItem)SelectionTarget).WeaponType = selectedAquaWeapon.WeaponType;
					Template.Save();
					break;
				}
				}
				LastClick = DateTime.Now;
				List = new List<SelectionEntry>();
				FilteredList = new List<SelectionEntry>();
				Clicked = true;
				((Control)this).Hide();
				break;
			}
			Changed?.Invoke(this, EventArgs.Empty);
		}

		private void UpdateLayout()
		{
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			if (List == null || List.Count == 0)
			{
				return;
			}
			int i = 0;
			int size = 42;
			FilteredList = new List<SelectionEntry>();
			if ((SelectionType == selectionType.Weapons || SelectionType == selectionType.AquaticWeapons) && Template != null && Template.Build != null && Template.Build.Profession != null)
			{
				List<string> weapons = new List<string>();
				foreach (API.ProfessionWeapon weapon in Template.Build.Profession.Weapons)
				{
					if (weapon.Specialization != 0 && Template.Build.SpecLines.Find((SpecLine e) => e.Specialization != null && e.Specialization.Id == weapon.Specialization) == null)
					{
						continue;
					}
					switch (Slot)
					{
					case _EquipmentSlots.Weapon1_MainHand:
					case _EquipmentSlots.Weapon2_MainHand:
						if (!weapon.Wielded.Contains(API.weaponHand.Aquatic) && (weapon.Wielded.Contains(API.weaponHand.Mainhand) || weapon.Wielded.Contains(API.weaponHand.TwoHand) || weapon.Wielded.Contains(API.weaponHand.DualWielded)))
						{
							weapons.Add(weapon.Weapon.ToString());
						}
						break;
					case _EquipmentSlots.Weapon1_OffHand:
					case _EquipmentSlots.Weapon2_OffHand:
						if (weapon.Wielded.Contains(API.weaponHand.Offhand) || weapon.Wielded.Contains(API.weaponHand.DualWielded))
						{
							weapons.Add(weapon.Weapon.ToString());
						}
						break;
					case _EquipmentSlots.AquaticWeapon1:
					case _EquipmentSlots.AquaticWeapon2:
						if (weapon.Wielded.Contains(API.weaponHand.Aquatic))
						{
							weapons.Add(weapon.Weapon.ToString());
						}
						break;
					}
				}
				List = List.Where((SelectionEntry e) => weapons.Contains(e.Header)).ToList();
			}
			if (((TextInputBase)FilterBox).get_Text() != null && ((TextInputBase)FilterBox).get_Text() != "")
			{
				IEnumerable<string> filteredTags = from e in ((TextInputBase)FilterBox).get_Text().ToLower().Split(' ')
						.ToList()
					where e.Trim().Length > 0
					select e;
				foreach (SelectionEntry entry2 in List)
				{
					List<filterTag> Tags = new List<filterTag>();
					foreach (string t in filteredTags)
					{
						filterTag tag = new filterTag
						{
							text = t.Trim().ToLower(),
							match = false
						};
						Tags.Add(tag);
						if (entry2.Header.ToLower().Contains(tag.text))
						{
							FilteredList.Add(entry2);
							tag.match = true;
						}
						foreach (string item in entry2.Content)
						{
							string lower = item.ToLower();
							tag.match = (tag.match ? tag.match : lower.Contains(tag.text));
							if (tag.match)
							{
								break;
							}
						}
					}
					if (!FilteredList.Contains(entry2) && Tags.Count == Tags.Where((filterTag e) => e.match).ToList().Count)
					{
						FilteredList.Add(entry2);
					}
				}
			}
			else
			{
				FilteredList = new List<SelectionEntry>(List);
			}
			foreach (SelectionEntry entry in FilteredList)
			{
				entry.AbsolutBounds = new Rectangle(0, ((Control)FilterBox).get_Height() + 5 + i * (size + 5), ((Control)this).get_Width(), size);
				entry.TextureBounds = new Rectangle(0, ((Control)FilterBox).get_Height() + 5 + i * (size + 5), size, size);
				entry.TextBounds = new Rectangle(size + 5, ((Control)FilterBox).get_Height() + i * (size + 5), size, size);
				entry.ContentBounds = new List<Rectangle>();
				int j = 0;
				int statSize = Font.get_LineHeight();
				if (entry.ContentTextures != null && entry.ContentTextures.Count > 0)
				{
					foreach (Texture2D contentTexture in entry.ContentTextures)
					{
						_ = contentTexture;
						entry.ContentBounds.Add(new Rectangle(size + j * statSize, ((Control)FilterBox).get_Height() + Font.get_LineHeight() + 12 + i * (size + 5), statSize, statSize));
						j++;
					}
				}
				i++;
			}
			((Control)this).set_Height(((Control)FilterBox).get_Height() + 5 + Math.Min(10, Math.Max(FilteredList.Count, 1)) * (size + 5));
		}

		private void UpdateStates()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			foreach (SelectionEntry entry in FilteredList)
			{
				entry.Hovered = ((Rectangle)(ref entry.AbsolutBounds)).Contains(((Control)this).get_RelativeMousePosition());
				if (entry.Hovered && SelectionType != selectionType.Weapons && CustomTooltip != null)
				{
					((Control)CustomTooltip).set_Visible(true);
					CustomTooltip.Header = entry.Header;
					CustomTooltip.Content = entry.Content;
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			Clicked = false;
			if (UpdateLayouts)
			{
				UpdateLayout();
				UpdateLayouts = false;
			}
			UpdateStates();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)bounds, new Color(75, 75, 75, 255), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Background, RectangleExtension.Add(bounds, -2, 0, 0, 0), (Rectangle?)bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			int i = 0;
			int size = 42;
			foreach (SelectionEntry entry in FilteredList)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, entry.Texture, entry.TextureBounds, (Rectangle?)entry.Texture.get_Bounds(), entry.Hovered ? Color.get_Orange() : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Header, Font, entry.TextBounds, entry.Hovered ? Color.get_Orange() : Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				if (entry.ContentTextures != null && entry.ContentTextures.Count > 0)
				{
					int j = 0;
					Font.get_LineHeight();
					foreach (Texture2D texture in entry.ContentTextures)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, entry.ContentBounds[j], (Rectangle?)texture.get_Bounds(), entry.Hovered ? Color.get_Orange() : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
						j++;
					}
				}
				else
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Join("; ", entry.Content), Font, new Rectangle(size + 5, Font.get_LineHeight() + ((Control)FilterBox).get_Height() + i * (size + 5), size, size), Color.get_LightGray(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				i++;
			}
		}
	}
}
