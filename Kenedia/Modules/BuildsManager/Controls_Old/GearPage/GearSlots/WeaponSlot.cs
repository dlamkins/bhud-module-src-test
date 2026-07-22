using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
			DrawColor = Color.White * 0.5f,
			HoverDrawColor = Color.White
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

		private Rectangle _sigilBounds;

		private Rectangle _pvpSigilBounds;

		private Rectangle _infusionBounds;

		public Stat Stat
		{
			[CompilerGenerated]
			get
			{
				return _003CStat_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CStat_003Ek__BackingField, value, delegate(Stat v)
				{
					_003CStat_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Stat>(OnStatChanged));
			}
		}

		public Sigil Sigil
		{
			[CompilerGenerated]
			get
			{
				return _003CSigil_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSigil_003Ek__BackingField, value, delegate(Sigil v)
				{
					_003CSigil_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Sigil>(OnSigilChanged));
			}
		}

		public Sigil PvpSigil
		{
			[CompilerGenerated]
			get
			{
				return _003CPvpSigil_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CPvpSigil_003Ek__BackingField, value, delegate(Sigil v)
				{
					_003CPvpSigil_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Sigil>(OnPvpSigilChanged));
			}
		}

		public Infusion Infusion
		{
			[CompilerGenerated]
			get
			{
				return _003CInfusion_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CInfusion_003Ek__BackingField, value, delegate(Infusion v)
				{
					_003CInfusion_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Infusion>(OnInfusionChanged));
			}
		}

		public WeaponSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			_infusionControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
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
			if (e.NewValue == GameModeType?.PvP)
			{
				_sigilControl.Visible = false;
				_infusionControl.Visible = false;
				_pvpSigilControl.Visible = true;
				base.ItemControl.ShowStat = false;
				if (base.SelectionPanel?.Anchor == _sigilControl && base.SelectionPanel.SubSlotType == GearSubSlotType.Sigil)
				{
					Rectangle b = base.AbsoluteBounds;
					base.SelectionPanel?.SetAnchor(_pvpSigilControl, new Rectangle(b.Location, Point.Zero).Add(_pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
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
				base.SelectionPanel?.SetAnchor(_sigilControl, new Rectangle(b2.Location, Point.Zero).Add(_pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
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
			base.RecalculateLayout();
			int upgradeSize = (base.ItemControl.LocalBounds.Size.Y - 4) / 2;
			TemplateSlotType slot = base.Slot;
			bool flag = ((slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand) ? true : false);
			int iconPadding = (flag ? 7 : 0);
			slot = base.Slot;
			flag = ((slot == TemplateSlotType.OffHand || slot == TemplateSlotType.AltOffHand) ? true : false);
			int textPadding = (flag ? 8 : 5);
			int pvpUpgradeSize = 48;
			int size = Math.Min(base.Width, base.Height);
			int padding = 2;
			_changeWeaponTexture.Bounds = new Rectangle(new Point(base.ItemControl.LocalBounds.Left + padding, padding), new Point((int)((double)(size - padding * 2) / 2.5)));
			_sigilControl.SetBounds(new Rectangle(base.ItemControl.Right + padding, 0, upgradeSize, upgradeSize));
			_infusionControl.SetBounds(new Rectangle(base.ItemControl.Right + padding, base.ItemControl.Bottom - upgradeSize, upgradeSize, upgradeSize));
			_pvpSigilControl.SetBounds(new Rectangle(base.ItemControl.LocalBounds.Right + 2 + 5 + iconPadding, (base.ItemControl.LocalBounds.Height - pvpUpgradeSize) / 2, pvpUpgradeSize, pvpUpgradeSize));
			_pvpSigilBounds = new Rectangle(_pvpSigilControl.Right + 10, _pvpSigilControl.Top, base.Width - (_pvpSigilControl.Right + 2), _pvpSigilControl.Height);
			int x = _sigilControl.Right + textPadding + 4;
			_sigilBounds = new Rectangle(x, _sigilControl.Top - 1, base.Width - x, _sigilControl.Height);
			_infusionBounds = new Rectangle(x, _infusionControl.Top, base.Width - x, _infusionControl.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as WeaponTemplateEntry)?.Weapon?.StatChoices ?? base.Data.Weapons.Values.FirstOrDefault()?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as WeaponTemplateEntry)?.Weapon?.AttributeAdjustment);
			}
			if (_pvpSigilControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_pvpSigilControl, new Rectangle(a.Location, Point.Zero).Add(_pvpSigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.PvpSigil, sigil);
				});
			}
			if (_sigilControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_sigilControl, new Rectangle(a.Location, Point.Zero).Add(_sigilControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Sigil1, sigil);
				});
			}
			if (_infusionControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusionControl, new Rectangle(a.Location, Point.Zero).Add(_infusionControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
			if (_changeWeaponTexture.Hovered || (base.ItemControl.MouseOver && base.TemplatePresenter.IsPvp))
			{
				base.SelectionPanel?.SetAnchor(this, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Weapon weapon)
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
