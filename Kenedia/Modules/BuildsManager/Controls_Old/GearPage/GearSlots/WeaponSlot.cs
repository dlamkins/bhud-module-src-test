using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.TemplateEntries;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class WeaponSlot : GearSlot
	{
		private readonly DetailedTexture _changeWeaponTexture = new DetailedTexture(2338896, 2338895)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24),
			DrawColor = Color.get_White() * 0.5f,
			HoverDrawColor = Color.get_White()
		};

		private readonly ItemControl _sigilControl = new ItemControl(new DetailedTexture(784324)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _pvpSigilControl = new ItemControl(new DetailedTexture(784324)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		})
		{
			Visible = false
		};

		private readonly ItemControl _infusionControl = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Stat _stat;

		private Sigil _sigil;

		private Sigil _pvpSigil;

		private Infusion _infusion;

		private Rectangle _sigilBounds;

		private Rectangle _pvpSigilBounds;

		private Rectangle _infusionBounds;

		public Stat Stat
		{
			get
			{
				return _stat;
			}
			set
			{
				Common.SetProperty(ref _stat, value, new ValueChangedEventHandler<Stat>(OnStatChanged));
			}
		}

		public Sigil Sigil
		{
			get
			{
				return _sigil;
			}
			set
			{
				Common.SetProperty(ref _sigil, value, new ValueChangedEventHandler<Sigil>(OnSigilChanged));
			}
		}

		public Sigil PvpSigil
		{
			get
			{
				return _pvpSigil;
			}
			set
			{
				Common.SetProperty(ref _pvpSigil, value, new ValueChangedEventHandler<Sigil>(OnPvpSigilChanged));
			}
		}

		public Infusion Infusion
		{
			get
			{
				return _infusion;
			}
			set
			{
				Common.SetProperty(ref _infusion, value, new ValueChangedEventHandler<Infusion>(OnInfusionChanged));
			}
		}

		public WeaponSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			_infusionControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_sigilControl.Parent = this;
			_pvpSigilControl.Parent = this;
			_infusionControl.Parent = this;
		}

		private void AdjustForOtherSlot()
		{
			Weapon weapon = base.Item as Weapon;
			if (weapon != null && weapon.WeaponType.IsTwoHanded())
			{
				ItemControl itemControl = base.ItemControl;
				TemplateSlotType slot = base.Slot;
				bool flag = ((slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand) ? true : false);
				itemControl.Opacity = (flag ? 0.5f : 1f);
			}
			else
			{
				base.ItemControl.Opacity = 1f;
			}
		}

		protected override void GameModeChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<GameModeType> e)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			if (e.NewValue == GameModeType?.PvP)
			{
				_sigilControl.Visible = false;
				_infusionControl.Visible = false;
				_pvpSigilControl.Visible = true;
				base.ItemControl.ShowStat = false;
				if (base.SelectionPanel?.Anchor == _sigilControl && base.SelectionPanel.SubSlotType == GearSubSlotType.Sigil)
				{
					Rectangle b = base.AbsoluteBounds;
					base.SelectionPanel?.SetAnchor(_pvpSigilControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref b)).get_Location(), Point.get_Zero()), _pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
					{
						PvpSigil = sigil;
					});
				}
				else
				{
					base.GameModeChanged(sender, e);
				}
				return;
			}
			_sigilControl.Visible = true;
			_infusionControl.Visible = true;
			_pvpSigilControl.Visible = false;
			base.ItemControl.ShowStat = true;
			if (base.SelectionPanel?.Anchor == _pvpSigilControl && base.SelectionPanel.SubSlotType == GearSubSlotType.Sigil)
			{
				Rectangle b2 = base.AbsoluteBounds;
				base.SelectionPanel?.SetAnchor(_sigilControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref b2)).get_Location(), Point.get_Zero()), _pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					Sigil = sigil;
				});
			}
			else
			{
				base.GameModeChanged(sender, e);
			}
		}

		private void SetGroupPvpSigil(Sigil sigil = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.PvpSigil, sigil, overrideExisting);
		}

		private void SetGroupStat(Stat stat = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Stat, stat, overrideExisting);
		}

		private void SetGroupSigil(Sigil sigil = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Sigil1, sigil, overrideExisting);
		}

		private void SetGroupInfusion(Infusion infusion = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Infusion1, infusion, overrideExisting);
		}

		private void SetGroupWeapon(Weapon item = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Item, item, overrideExisting);
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int upgradeSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 2;
			TemplateSlotType slot = base.Slot;
			bool flag = ((slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand) ? true : false);
			int iconPadding = (flag ? 7 : 0);
			slot = base.Slot;
			flag = ((slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand) ? true : false);
			int textPadding = (flag ? 8 : 5);
			int pvpUpgradeSize = 48;
			int size = Math.Min(base.Width, base.Height);
			int padding = 2;
			DetailedTexture changeWeaponTexture = _changeWeaponTexture;
			localBounds = base.ItemControl.LocalBounds;
			changeWeaponTexture.Bounds = new Rectangle(new Point(((Rectangle)(ref localBounds)).get_Left() + padding, padding), new Point((int)((double)(size - padding * 2) / 2.5)));
			_sigilControl.SetBounds(new Rectangle(base.ItemControl.Right + padding, 0, upgradeSize, upgradeSize));
			_infusionControl.SetBounds(new Rectangle(base.ItemControl.Right + padding, base.ItemControl.Bottom - upgradeSize, upgradeSize, upgradeSize));
			ItemControl pvpSigilControl = _pvpSigilControl;
			localBounds = base.ItemControl.LocalBounds;
			pvpSigilControl.SetBounds(new Rectangle(((Rectangle)(ref localBounds)).get_Right() + 2 + 5 + iconPadding, (base.ItemControl.LocalBounds.Height - pvpUpgradeSize) / 2, pvpUpgradeSize, pvpUpgradeSize));
			_pvpSigilBounds = new Rectangle(_pvpSigilControl.Right + 10, _pvpSigilControl.Top, base.Width - (_pvpSigilControl.Right + 2), _pvpSigilControl.Height);
			int x = _sigilControl.Right + textPadding + 4;
			_sigilBounds = new Rectangle(x, _sigilControl.Top - 1, base.Width - x, _sigilControl.Height);
			_infusionBounds = new Rectangle(x, _infusionControl.Top, base.Width - x, _infusionControl.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (base.TemplatePresenter.IsPve)
			{
				_changeWeaponTexture.Draw(this, spriteBatch, base.RelativeMousePosition);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Sigil?.DisplayText ?? string.Empty), UpgradeFont, _sigilBounds, UpgradeColor);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Infusion?.DisplayText ?? string.Empty), InfusionFont, _infusionBounds, InfusionColor, wrap: true);
			}
			else if (base.TemplatePresenter.IsPvp)
			{
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(PvpSigil?.DisplayText ?? string.Empty), UpgradeFont, _pvpSigilBounds, UpgradeColor);
			}
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			WeaponTemplateEntry weapon = base.TemplatePresenter?.Template?[base.Slot] as WeaponTemplateEntry;
			if (weapon != null)
			{
				base.Item = weapon?.Weapon;
				Infusion = weapon?.Infusion1;
				Sigil = weapon?.Sigil1;
				PvpSigil = weapon?.PvpSigil;
				Stat = weapon?.Stat;
				AdjustForOtherSlot();
			}
			else
			{
				base.Item = null;
				Infusion = null;
				Sigil = null;
				PvpSigil = null;
				Stat = null;
			}
		}

		protected override void SetAnchor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			Rectangle a = base.AbsoluteBounds;
			object obj;
			if (!base.Slot.IsOffhand())
			{
				obj = null;
			}
			else
			{
				WeaponTemplateEntry weapon2 = base.TemplatePresenter?.Template?[base.Slot] as WeaponTemplateEntry;
				obj = ((weapon2 != null) ? weapon2 : null);
			}
			WeaponTemplateEntry entry = (WeaponTemplateEntry)obj;
			if (base.ItemControl.MouseOver && base.TemplatePresenter.IsPve && ((!(entry?.Weapon?.WeaponType.IsTwoHanded())) ?? true))
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as WeaponTemplateEntry)?.Weapon?.StatChoices ?? base.Data.Weapons.Values.FirstOrDefault()?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as WeaponTemplateEntry)?.Weapon?.AttributeAdjustment);
			}
			if (_pvpSigilControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_pvpSigilControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.PvpSigil, sigil);
				});
			}
			if (_sigilControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_sigilControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _sigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Sigil1, sigil);
				});
			}
			if (_infusionControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusionControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusionControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
			if (_changeWeaponTexture.Hovered || (base.ItemControl.MouseOver && base.TemplatePresenter.IsPvp))
			{
				base.SelectionPanel?.SetAnchor(this, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Weapon weapon)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, weapon);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusion), delegate
			{
				base.TemplatePresenter?.Template.SetItem<Weapon>(base.Slot, TemplateSubSlotType.Item, null);
				base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil1, null);
				base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.PvpSigil, null);
				base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
			}, new List<(Func<string>, Func<string>, Action)>(5)
			{
				(() => strings.Weapon, () => string.Format(strings.ResetEntry, strings.Weapon), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Weapon>(base.Slot, TemplateSubSlotType.Item, null);
				}),
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Sigil, () => string.Format(strings.ResetEntry, strings.Sigil), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil1, null);
				}),
				(() => strings.PvpSigil, () => string.Format(strings.ResetEntry, strings.PvpSigil), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.PvpSigil, null);
				}),
				(() => strings.Infusion, () => string.Format(strings.ResetEntry, strings.Infusion), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusion + " " + strings.EmptyWeaponSlots), delegate
			{
				SetGroupWeapon(base.Item as Weapon);
				SetGroupStat(Stat);
				SetGroupSigil(Sigil);
				SetGroupPvpSigil(PvpSigil);
				SetGroupInfusion(Infusion);
			}, new List<(Func<string>, Func<string>, Action)>(5)
			{
				(() => strings.Weapon, () => string.Format(strings.FillEntry, strings.Weapon + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupWeapon(base.Item as Weapon);
				}),
				(() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupStat(Stat);
				}),
				(() => strings.Sigil, () => string.Format(strings.FillEntry, strings.Sigil + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupSigil(Sigil);
				}),
				(() => strings.PvpSigil, () => string.Format(strings.FillEntry, strings.PvpSigil + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupPvpSigil(PvpSigil);
				}),
				(() => strings.Infusion, () => string.Format(strings.FillEntry, strings.Infusion + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupInfusion(Infusion);
				})
			});
			CreateSubMenu(() => strings.Override, () => string.Format(strings.OverrideEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusion + " " + strings.WeaponSlots), delegate
			{
				SetGroupWeapon(base.Item as Weapon, overrideExisting: true);
				SetGroupStat(Stat, overrideExisting: true);
				SetGroupSigil(Sigil, overrideExisting: true);
				SetGroupPvpSigil(PvpSigil, overrideExisting: true);
				SetGroupInfusion(Infusion, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(5)
			{
				(() => strings.Weapon, () => string.Format(strings.OverrideEntry, strings.Weapons + " " + strings.WeaponSlots), delegate
				{
					SetGroupWeapon(base.Item as Weapon, overrideExisting: true);
				}),
				(() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.WeaponSlots), delegate
				{
					SetGroupStat(Stat, overrideExisting: true);
				}),
				(() => strings.Sigil, () => string.Format(strings.OverrideEntry, strings.Sigil + " " + strings.WeaponSlots), delegate
				{
					SetGroupSigil(Sigil, overrideExisting: true);
				}),
				(() => strings.PvpSigil, () => string.Format(strings.OverrideEntry, strings.PvpSigil + " " + strings.WeaponSlots), delegate
				{
					SetGroupPvpSigil(PvpSigil, overrideExisting: true);
				}),
				(() => strings.Infusion, () => string.Format(strings.OverrideEntry, strings.Infusion + " " + strings.WeaponSlots), delegate
				{
					SetGroupInfusion(Infusion, overrideExisting: true);
				})
			});
			CreateSubMenu(() => string.Format(strings.ResetAll, strings.Weapons), () => string.Format(strings.ResetEntry, strings.Weapons + ", " + strings.Stats + " , " + strings.Sigils + " " + strings.And + " " + strings.Infusions + " " + strings.WeaponSlots), delegate
			{
				SetGroupWeapon(null, overrideExisting: true);
				SetGroupStat(null, overrideExisting: true);
				SetGroupSigil(null, overrideExisting: true);
				SetGroupPvpSigil(null, overrideExisting: true);
				SetGroupInfusion(null, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(5)
			{
				(() => strings.Weapons, () => string.Format(strings.ResetAll, strings.Weapons + " " + strings.WeaponSlots), delegate
				{
					SetGroupWeapon(null, overrideExisting: true);
				}),
				(() => strings.Stats, () => string.Format(strings.ResetAll, strings.Stats + " " + strings.WeaponSlots), delegate
				{
					SetGroupStat(null, overrideExisting: true);
				}),
				(() => strings.Sigils, () => string.Format(strings.ResetAll, strings.Sigils + " " + strings.WeaponSlots), delegate
				{
					SetGroupSigil(null, overrideExisting: true);
				}),
				(() => strings.PvpSigils, () => string.Format(strings.ResetAll, strings.PvpSigils + " " + strings.WeaponSlots), delegate
				{
					SetGroupPvpSigil(null, overrideExisting: true);
				}),
				(() => strings.Infusions, () => string.Format(strings.ResetAll, strings.Infusions + " " + strings.WeaponSlots), delegate
				{
					SetGroupInfusion(null, overrideExisting: true);
				})
			});
		}

		private void OnStatChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			base.ItemControl.Stat = Stat;
		}

		private void OnSigilChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Sigil> e)
		{
			_sigilControl.Item = Sigil;
		}

		private void OnPvpSigilChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Sigil> e)
		{
			_pvpSigilControl.Item = PvpSigil;
		}

		private void OnInfusionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusionControl.Item = Infusion;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Sigil = null;
			PvpSigil = null;
			Infusion = null;
		}
	}
}
