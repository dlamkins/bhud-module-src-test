using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
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
	public class AquaticWeaponSlot : GearSlot
	{
		private readonly ItemControl _infusion1Control = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _infusion2Control = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _sigil1Control = new ItemControl(new DetailedTexture(784324)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _sigil2Control = new ItemControl(new DetailedTexture(784324)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly DetailedTexture _changeWeaponTexture = new DetailedTexture(2338896, 2338895)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24),
			DrawColor = Color.get_White() * 0.5f,
			HoverDrawColor = Color.get_White()
		};

		private Rectangle _sigil1Bounds;

		private Rectangle _sigil2Bounds;

		private Rectangle _infusion1Bounds;

		private Rectangle _infusion2Bounds;

		private Stat _stat;

		private Sigil _sigil1;

		private Sigil _sigil2;

		private Infusion _infusion1;

		private Infusion _infusion2;

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

		public Sigil Sigil1
		{
			get
			{
				return _sigil1;
			}
			set
			{
				Common.SetProperty(ref _sigil1, value, new ValueChangedEventHandler<Sigil>(OnSigil1Changed));
			}
		}

		public Sigil Sigil2
		{
			get
			{
				return _sigil2;
			}
			set
			{
				Common.SetProperty(ref _sigil2, value, new ValueChangedEventHandler<Sigil>(OnSigil2Changed));
			}
		}

		public Infusion Infusion1
		{
			get
			{
				return _infusion1;
			}
			set
			{
				Common.SetProperty(ref _infusion1, value, new ValueChangedEventHandler<Infusion>(OnInfusion1Changed));
			}
		}

		public Infusion Infusion2
		{
			get
			{
				return _infusion2;
			}
			set
			{
				Common.SetProperty(ref _infusion2, value, new ValueChangedEventHandler<Infusion>(OnInfusion2Changed));
			}
		}

		public AquaticWeaponSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel)
			: base(gearSlot, parent, templatePresenter, selectionPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			_infusion1Control.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_infusion2Control.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_sigil1Control.Parent = this;
			_sigil2Control.Parent = this;
			_infusion1Control.Parent = this;
			_infusion2Control.Parent = this;
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
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int upgradeSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 2;
			int textPadding = ((base.Slot == TemplateSlotType.AquaBreather) ? (upgradeSize + 5) : 5);
			int size = Math.Min(base.Width, base.Height);
			int padding = 2;
			DetailedTexture changeWeaponTexture = _changeWeaponTexture;
			localBounds = base.ItemControl.LocalBounds;
			changeWeaponTexture.Bounds = new Rectangle(new Point(((Rectangle)(ref localBounds)).get_Left() + padding, padding), new Point((int)((double)(size - padding * 2) / 2.5)));
			_sigil1Control.SetBounds(new Rectangle(base.ItemControl.Right + padding, 0, upgradeSize, upgradeSize));
			_sigil2Control.SetBounds(new Rectangle(base.ItemControl.Right + padding + upgradeSize + padding, 0, upgradeSize, upgradeSize));
			_infusion1Control.SetBounds(new Rectangle(base.ItemControl.Right + padding, base.ItemControl.Bottom - upgradeSize, upgradeSize, upgradeSize));
			_infusion2Control.SetBounds(new Rectangle(base.ItemControl.Right + padding + upgradeSize + padding, base.ItemControl.Bottom - upgradeSize, upgradeSize, upgradeSize));
			int upgradeWidth = (base.Width - (_sigil2Control.Right + 2)) / 2;
			int x = _sigil2Control.Right + textPadding + 4;
			_sigil1Bounds = new Rectangle(x, _sigil1Control.Top, upgradeWidth, _sigil1Control.Height);
			_sigil2Bounds = new Rectangle(x + upgradeWidth, _sigil1Control.Top, upgradeWidth, _sigil1Control.Height);
			_infusion1Bounds = new Rectangle(x, _infusion1Control.Top, upgradeWidth, _infusion1Control.Height);
			_infusion2Bounds = new Rectangle(x + upgradeWidth, _infusion1Control.Top, upgradeWidth, _infusion1Control.Height);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (base.TemplatePresenter.IsPve)
			{
				_changeWeaponTexture.Draw(this, spriteBatch, base.RelativeMousePosition);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Sigil1?.DisplayText ?? string.Empty), UpgradeFont, _sigil1Bounds, UpgradeColor);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Sigil2?.DisplayText ?? string.Empty), UpgradeFont, _sigil2Bounds, UpgradeColor);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Infusion1?.DisplayText ?? string.Empty), InfusionFont, _infusion1Bounds, InfusionColor, wrap: true);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Infusion2?.DisplayText ?? string.Empty), InfusionFont, _infusion2Bounds, InfusionColor, wrap: true);
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
			AquaticWeaponTemplateEntry aquaticWeapon = base.TemplatePresenter?.Template?[base.Slot] as AquaticWeaponTemplateEntry;
			if (aquaticWeapon != null)
			{
				base.Item = aquaticWeapon.Item;
				Infusion1 = aquaticWeapon?.Infusion1;
				Infusion2 = aquaticWeapon?.Infusion2;
				Sigil1 = aquaticWeapon?.Sigil1;
				Sigil2 = aquaticWeapon?.Sigil2;
				Stat = aquaticWeapon?.Stat;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as AquaticWeaponTemplateEntry).Weapon?.StatChoices ?? BuildsManager.Data.Weapons.Values.FirstOrDefault()?.StatChoices, (base.TemplatePresenter?.Template[base.Slot] as AquaticWeaponTemplateEntry).Weapon?.AttributeAdjustment);
			}
			if (_sigil1Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_sigil1Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _sigil1Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Sigil1, sigil);
				});
			}
			if (_sigil2Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_sigil2Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _sigil2Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Sigil, delegate(Sigil sigil)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Sigil2, sigil);
				});
			}
			if (_infusion1Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusion1Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusion1Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
			if (_infusion2Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusion2Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusion2Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion2, infusion);
				});
			}
			if (_changeWeaponTexture.Hovered)
			{
				base.SelectionPanel?.SetAnchor(this, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Weapon item)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, item);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusions), delegate
			{
				base.TemplatePresenter?.Template.SetItem<Weapon>(base.Slot, TemplateSubSlotType.Item, null);
				base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil1, null);
				base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil2, null);
				base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
				base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion2, null);
			}, new List<(Func<string>, Func<string>, Action)>(4)
			{
				(() => strings.Weapon, () => string.Format(strings.ResetEntry, strings.Weapon), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Weapon>(base.Slot, TemplateSubSlotType.Item, null);
				}),
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Sigil, () => string.Format(strings.ResetEntry, strings.Sigils), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil1, null);
					base.TemplatePresenter?.Template.SetItem<Sigil>(base.Slot, TemplateSubSlotType.Sigil2, null);
				}),
				(() => strings.Infusions, () => string.Format(strings.ResetEntry, strings.Weapon), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
					base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion2, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusions + " " + strings.EmptyWeaponSlots), delegate
			{
				SetGroupWeapon(base.Item as Weapon);
				SetGroupStat(Stat);
				SetGroupSigil(Sigil1);
				SetGroupInfusion(Infusion1);
			}, new List<(Func<string>, Func<string>, Action)>(4)
			{
				(() => strings.Weapon, () => string.Format(strings.FillEntry, strings.Weapon + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupWeapon(base.Item as Weapon);
				}),
				(() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupStat(Stat);
				}),
				(() => strings.Sigil, () => string.Format(strings.FillEntry, strings.Sigils + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupSigil(Sigil1);
				}),
				(() => strings.Infusions, () => string.Format(strings.FillEntry, strings.Infusions + " " + strings.EmptyWeaponSlots), delegate
				{
					SetGroupInfusion(Infusion1);
				})
			});
			CreateSubMenu(() => strings.Override, () => string.Format(strings.OverrideEntry, strings.Weapon + ", " + strings.Stat + ", " + strings.Sigils + " " + strings.And + " " + strings.Infusions + " " + strings.WeaponSlots), delegate
			{
				SetGroupWeapon(base.Item as Weapon, overrideExisting: true);
				SetGroupStat(Stat, overrideExisting: true);
				SetGroupSigil(Sigil1, overrideExisting: true);
				SetGroupInfusion(Infusion1, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(4)
			{
				(() => strings.Weapon, () => string.Format(strings.OverrideEntry, strings.Weapons + " " + strings.WeaponSlots), delegate
				{
					SetGroupWeapon(base.Item as Weapon, overrideExisting: true);
				}),
				(() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.WeaponSlots), delegate
				{
					SetGroupStat(Stat, overrideExisting: true);
				}),
				(() => strings.Sigil, () => string.Format(strings.FillEntry, strings.Sigils + " " + strings.WeaponSlots), delegate
				{
					SetGroupSigil(Sigil1, overrideExisting: true);
				}),
				(() => strings.Infusions, () => string.Format(strings.OverrideEntry, strings.Infusions + " " + strings.WeaponSlots), delegate
				{
					SetGroupInfusion(Infusion1, overrideExisting: true);
				})
			});
			CreateSubMenu(() => string.Format(strings.ResetAll, strings.Weapons), () => string.Format(strings.ResetEntry, strings.Weapons + ", " + strings.Stats + " , " + strings.Sigils + " " + strings.And + " " + strings.Infusions + " " + strings.WeaponSlots), delegate
			{
				SetGroupWeapon(null, overrideExisting: true);
				SetGroupStat(null, overrideExisting: true);
				SetGroupSigil(null, overrideExisting: true);
				SetGroupInfusion(null, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(4)
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

		private void OnSigil2Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Sigil> e)
		{
			_sigil2Control.Item = Sigil2;
		}

		private void OnSigil1Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Sigil> e)
		{
			_sigil1Control.Item = Sigil1;
		}

		private void OnInfusion1Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusion1Control.Item = Infusion1;
		}

		private void OnInfusion2Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusion2Control.Item = Infusion2;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Sigil1 = null;
			Sigil2 = null;
			Infusion1 = null;
			Infusion2 = null;
		}
	}
}
