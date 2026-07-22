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

		private Rectangle _runeBounds;

		private Rectangle _infusionBounds;

		public Stat? Stat
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

		public Rune? Rune
		{
			[CompilerGenerated]
			get
			{
				return _003CRune_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CRune_003Ek__BackingField, value, delegate(Rune v)
				{
					_003CRune_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Rune>(OnRuneChanged));
			}
		}

		public Infusion? Infusion
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

		public ArmorSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			_infusionControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_runeControl.Parent = this;
			_infusionControl.Parent = this;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int upgradeSize = (base.ItemControl.Height - 4) / 2;
			int iconPadding = 0;
			int textPadding = ((base.Slot == TemplateSlotType.AquaBreather) ? (upgradeSize + 5) : 5);
			_runeControl.SetBounds(new Rectangle(base.ItemControl.Right + 2 + iconPadding, iconPadding, upgradeSize, upgradeSize));
			_infusionControl.SetBounds(new Rectangle(base.ItemControl.Right + 2 + iconPadding, base.ItemControl.Bottom - (upgradeSize + iconPadding), upgradeSize, upgradeSize));
			int x = _runeControl.LocalBounds.Right + textPadding + 4;
			_runeBounds = new Rectangle(x, _runeControl.LocalBounds.Top - 1, base.Width - x, _runeControl.LocalBounds.Height);
			_infusionBounds = new Rectangle(x, _infusionControl.LocalBounds.Top, base.Width - x, _infusionControl.LocalBounds.Height);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
			else
			{
				Infusion = null;
				Rune = null;
				Stat = null;
			}
		}

		protected override void SetAnchor()
		{
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as ArmorTemplateEntry)?.Armor?.StatChoices ?? base.Data.Armors?.Items?.Values?.FirstOrDefault()?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as ArmorTemplateEntry)?.Armor?.AttributeAdjustment);
			}
			if (_runeControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_runeControl, new Rectangle(a.Location, Point.Zero).Add(_runeControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Rune, delegate(Rune rune)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Rune, rune);
				});
			}
			if (_infusionControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusionControl, new Rectangle(a.Location, Point.Zero).Add(_infusionControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
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
