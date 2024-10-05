using System;
using System.Collections.Generic;
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

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class AccessoireSlot : GearSlot
	{
		private readonly ItemControl _infusionControl = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Stat? _stat;

		private Infusion? _infusion;

		public Stat Stat
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

		public Infusion Infusion
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

		public AccessoireSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel)
			: base(gearSlot, parent, templatePresenter, selectionPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_infusionControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			base.ItemControl.Item = BuildsManager.Data.Trinkets[81908];
			_infusionControl.Parent = this;
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int infusionSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 3;
			_infusionControl.SetBounds(new Rectangle(base.ItemControl.Right + 1, base.ItemControl.Top, infusionSize, infusionSize));
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			AccessoireTemplateEntry accessoire = base.TemplatePresenter?.Template?[base.Slot] as AccessoireTemplateEntry;
			if (accessoire != null)
			{
				Infusion = accessoire?.Infusion1;
				Stat = accessoire?.Stat;
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
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as AccessoireTemplateEntry).Accessoire?.StatChoices, (base.TemplatePresenter?.Template[base.Slot] as AccessoireTemplateEntry).Accessoire?.AttributeAdjustment);
			}
			if (_infusionControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusionControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusionControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Stat + " " + strings.And + " " + strings.Infusion), delegate
			{
				base.TemplatePresenter?.Template?.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template?.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Infusion, () => string.Format(strings.ResetEntry, strings.Infusion), delegate
				{
					base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.And + " " + strings.Infusion + " " + strings.EmptyJewellerySlots), delegate
			{
				SetGroupStat(Stat);
				SetGroupInfusion(Infusion);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyJewellerySlots), delegate
				{
					SetGroupStat(Stat);
				}),
				(() => strings.Infusion, () => string.Format(strings.FillEntry, strings.Infusion + " " + strings.EmptyJewellerySlots), delegate
				{
					SetGroupInfusion(Infusion);
				})
			});
			CreateSubMenu(() => strings.Override, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.And + " " + strings.Infusion + " " + strings.JewellerySlots), delegate
			{
				SetGroupStat(Stat, overrideExisting: true);
				SetGroupInfusion(Infusion, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.JewellerySlots), delegate
				{
					SetGroupStat(Stat, overrideExisting: true);
				}),
				(() => strings.Infusion, () => string.Format(strings.OverrideEntry, strings.Infusion + " " + strings.JewellerySlots), delegate
				{
					SetGroupInfusion(Infusion, overrideExisting: true);
				})
			});
			CreateSubMenu(() => string.Format(strings.ResetAll, strings.Jewellery), () => string.Format(strings.ResetEntry, strings.Stats + ", " + strings.Enrichment + " " + strings.And + " " + strings.Infusions + " " + strings.JewellerySlots), delegate
			{
				SetGroupStat(null, overrideExisting: true);
				SetGroupInfusion(null, overrideExisting: true);
				base.TemplatePresenter.Template?.SetGroup<Enrichment>(base.Slot, TemplateSubSlotType.Enrichment, null, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(3)
			{
				(() => strings.Stats, () => string.Format(strings.ResetEntry, strings.Stats + " " + strings.JewellerySlots), delegate
				{
					SetGroupStat(null, overrideExisting: true);
				}),
				(() => strings.Infusions, () => string.Format(strings.ResetEntry, strings.Infusions + " " + strings.JewellerySlots), delegate
				{
					SetGroupInfusion(null, overrideExisting: true);
				}),
				(() => strings.Enrichment, () => string.Format(strings.ResetEntry, strings.Enrichment + " " + strings.JewellerySlots), delegate
				{
					base.TemplatePresenter.Template?.SetGroup<Enrichment>(base.Slot, TemplateSubSlotType.Enrichment, null, overrideExisting: true);
				})
			});
		}

		private void SetGroupStat(Stat stat = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Stat, stat, overrideExisting);
		}

		private void SetGroupInfusion(Infusion infusion = null, bool overrideExisting = false)
		{
			base.TemplatePresenter.Template?.SetGroup(base.Slot, TemplateSubSlotType.Infusion1, infusion, overrideExisting);
		}

		private void OnInfusionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusionControl.Item = Infusion;
		}

		private void OnStatChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			base.ItemControl.Stat = Stat;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Infusion = null;
		}
	}
}
