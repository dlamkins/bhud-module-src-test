using System;
using System.Collections.Generic;
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

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class AmuletSlot : GearSlot
	{
		private readonly ItemControl _enrichmentControl = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Stat _stat;

		private Enrichment _enrichment;

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

		public Enrichment Enrichment
		{
			get
			{
				return _enrichment;
			}
			set
			{
				Common.SetProperty(ref _enrichment, value, new ValueChangedEventHandler<Enrichment>(OnEnrichmentChanged));
			}
		}

		public AmuletSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_enrichmentControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_enrichmentControl.Parent = this;
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			base.ItemControl.Item = base.Data.Trinkets[92991];
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int infusionSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 3;
			ItemControl enrichmentControl = _enrichmentControl;
			localBounds = base.ItemControl.LocalBounds;
			int num = ((Rectangle)(ref localBounds)).get_Right() + 1;
			localBounds = base.ItemControl.LocalBounds;
			enrichmentControl.SetBounds(new Rectangle(num, ((Rectangle)(ref localBounds)).get_Top(), infusionSize, infusionSize));
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			AmuletTemplateEntry amulet = base.TemplatePresenter?.Template?[base.Slot] as AmuletTemplateEntry;
			if (amulet != null)
			{
				Enrichment = amulet?.Enrichment;
				Stat = amulet?.Stat;
			}
			else
			{
				Enrichment = null;
				Stat = null;
			}
		}

		protected override void SetAnchor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as AmuletTemplateEntry)?.Amulet?.StatChoices ?? base.Data.Trinkets?[92991]?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as AmuletTemplateEntry)?.Amulet?.AttributeAdjustment);
			}
			if (_enrichmentControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_enrichmentControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _enrichmentControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Enrichment, delegate(Enrichment enrichment)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Enrichment, enrichment);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Stat + " " + strings.And + " " + strings.Enrichment), delegate
			{
				base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template.SetItem<Enrichment>(base.Slot, TemplateSubSlotType.Enrichment, null);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Enrichment, () => string.Format(strings.ResetEntry, strings.Enrichment), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Enrichment>(base.Slot, TemplateSubSlotType.Enrichment, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyJewellerySlots), delegate
			{
				SetGroupStat(Stat);
			}, new List<(Func<string>, Func<string>, Action)>(1) { (() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyJewellerySlots), delegate
			{
				SetGroupStat(Stat);
			}) });
			CreateSubMenu(() => strings.Override, () => string.Format(strings.Override, strings.Stat + " " + strings.JewellerySlots), delegate
			{
				SetGroupStat(Stat, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(1) { (() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.JewellerySlots), delegate
			{
				SetGroupStat(Stat, overrideExisting: true);
			}) });
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

		private void OnEnrichmentChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Enrichment> e)
		{
			_enrichmentControl.Item = Enrichment;
		}

		private void OnStatChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			base.ItemControl.Stat = Stat;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Enrichment = null;
		}
	}
}
