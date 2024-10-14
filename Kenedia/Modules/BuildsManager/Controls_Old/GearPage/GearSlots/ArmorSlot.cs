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
	public class ArmorSlot : GearSlot
	{
		private readonly ItemControl _runeControl = new ItemControl(new DetailedTexture(784323)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _infusionControl = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Stat? _stat;

		private Rune? _rune;

		private Infusion? _infusion;

		private Rectangle _runeBounds;

		private Rectangle _infusionBounds;

		public Stat? Stat
		{
			get
			{
				return _stat;
			}
			set
			{
				Common.SetProperty<Stat>(ref _stat, value, new ValueChangedEventHandler<Stat>(OnStatChanged));
			}
		}

		public Rune? Rune
		{
			get
			{
				return _rune;
			}
			set
			{
				Common.SetProperty<Rune>(ref _rune, value, new ValueChangedEventHandler<Rune>(OnRuneChanged));
			}
		}

		public Infusion? Infusion
		{
			get
			{
				return _infusion;
			}
			set
			{
				Common.SetProperty<Infusion>(ref _infusion, value, new ValueChangedEventHandler<Infusion>(OnInfusionChanged));
			}
		}

		public ArmorSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			_infusionControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_runeControl.Parent = this;
			_infusionControl.Parent = this;
		}

		public override void RecalculateLayout()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int upgradeSize = (base.ItemControl.Height - 4) / 2;
			int iconPadding = 0;
			int textPadding = ((base.Slot == TemplateSlotType.AquaBreather) ? (upgradeSize + 5) : 5);
			_runeControl.SetBounds(new Rectangle(base.ItemControl.Right + 2 + iconPadding, iconPadding, upgradeSize, upgradeSize));
			_infusionControl.SetBounds(new Rectangle(base.ItemControl.Right + 2 + iconPadding, base.ItemControl.Bottom - (upgradeSize + iconPadding), upgradeSize, upgradeSize));
			Rectangle localBounds = _runeControl.LocalBounds;
			int x = ((Rectangle)(ref localBounds)).get_Right() + textPadding + 4;
			localBounds = _runeControl.LocalBounds;
			_runeBounds = new Rectangle(x, ((Rectangle)(ref localBounds)).get_Top() - 1, base.Width - x, _runeControl.LocalBounds.Height);
			localBounds = _infusionControl.LocalBounds;
			_infusionBounds = new Rectangle(x, ((Rectangle)(ref localBounds)).get_Top(), base.Width - x, _infusionControl.LocalBounds.Height);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (base.TemplatePresenter.IsPve)
			{
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Rune?.DisplayText ?? string.Empty), UpgradeFont, _runeBounds, UpgradeColor);
				spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Infusion?.DisplayText ?? string.Empty), InfusionFont, _infusionBounds, InfusionColor, wrap: true);
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
			ArmorTemplateEntry armor = base.TemplatePresenter?.Template?[base.Slot] as ArmorTemplateEntry;
			if (armor != null)
			{
				Infusion = armor?.Infusion1;
				Rune = armor?.Rune;
				Stat = armor?.Stat;
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
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as ArmorTemplateEntry).Armor?.StatChoices ?? base.Data.Armors?.Items?.Values?.FirstOrDefault()?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as ArmorTemplateEntry).Armor?.AttributeAdjustment);
			}
			if (_runeControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_runeControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _runeControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Rune, delegate(Rune rune)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Rune, rune);
				});
			}
			if (_infusionControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusionControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusionControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Stat + ", " + strings.Rune + " " + strings.And + " " + strings.Infusion), delegate
			{
				base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template.SetItem<Rune>(base.Slot, TemplateSubSlotType.Rune, null);
				base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
			}, new List<(Func<string>, Func<string>, Action)>(3)
			{
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Rune, () => string.Format(strings.ResetEntry, strings.Rune), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Rune>(base.Slot, TemplateSubSlotType.Rune, null);
				}),
				(() => strings.Infusion, () => string.Format(strings.ResetEntry, strings.Infusion), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Stat + ", " + strings.Rune + " " + strings.And + " " + strings.Infusion + " " + strings.EmptyArmorSlots), delegate
			{
				SetGroupStat(Stat);
				SetGroupRune(Rune, overrideExisting: false);
				SetGroupInfusion(Infusion);
			}, new List<(Func<string>, Func<string>, Action)>(3)
			{
				(() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyArmorSlots), delegate
				{
					SetGroupStat(Stat);
				}),
				(() => strings.Rune, () => string.Format(strings.FillEntry, strings.Rune + " " + strings.EmptyArmorSlots), delegate
				{
					SetGroupRune(Rune, overrideExisting: false);
				}),
				(() => strings.Infusion, () => string.Format(strings.FillEntry, strings.Infusion + " " + strings.EmptyArmorSlots), delegate
				{
					SetGroupInfusion(Infusion);
				})
			});
			CreateSubMenu(() => strings.Override, () => string.Format(strings.OverrideEntry, strings.Stat + ", " + strings.Rune + " " + strings.And + " " + strings.Infusions + " " + strings.ArmorSlots), delegate
			{
				SetGroupStat(Stat, overrideExisting: true);
				SetGroupRune(Rune, overrideExisting: true);
				SetGroupInfusion(Infusion, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(3)
			{
				(() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.ArmorSlots), delegate
				{
					SetGroupStat(Stat, overrideExisting: true);
				}),
				(() => strings.Rune, () => string.Format(strings.OverrideEntry, strings.Rune + " " + strings.ArmorSlots), delegate
				{
					SetGroupRune(Rune, overrideExisting: true);
				}),
				(() => strings.Infusion, () => string.Format(strings.OverrideEntry, strings.Infusion + " " + strings.ArmorSlots), delegate
				{
					SetGroupInfusion(Infusion, overrideExisting: true);
				})
			});
			CreateSubMenu(() => string.Format(strings.ResetAll, strings.Armors), () => string.Format(strings.ResetEntry, strings.Stats + ", " + strings.Runes + " " + strings.And + " " + strings.Infusions + " " + strings.ArmorSlots), delegate
			{
				SetGroupStat(null, overrideExisting: true);
				SetGroupRune(null, overrideExisting: true);
				SetGroupInfusion(null, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(3)
			{
				(() => strings.Stats, () => string.Format(strings.ResetAll, strings.Stats + " " + strings.ArmorSlots), delegate
				{
					SetGroupStat(null, overrideExisting: true);
				}),
				(() => strings.Runes, () => string.Format(strings.ResetAll, strings.Runes + " " + strings.ArmorSlots), delegate
				{
					SetGroupRune(null, overrideExisting: true);
				}),
				(() => strings.Infusions, () => string.Format(strings.ResetAll, strings.Infusions + " " + strings.ArmorSlots), delegate
				{
					SetGroupInfusion(null, overrideExisting: true);
				})
			});
		}

		private void SetGroupStat(Stat stat = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Stat, stat, overrideExisting);
		}

		private void SetGroupRune(Rune rune, bool overrideExisting)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Rune, rune, overrideExisting);
		}

		private void SetGroupInfusion(Infusion infusion = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Infusion1, infusion, overrideExisting);
		}

		private void OnStatChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			base.ItemControl.Stat = Stat;
		}

		private void OnRuneChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Rune> e)
		{
			_runeControl.Item = Rune;
		}

		private void OnInfusionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusionControl.Item = Infusion;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Rune = null;
			Infusion = null;
		}
	}
}
