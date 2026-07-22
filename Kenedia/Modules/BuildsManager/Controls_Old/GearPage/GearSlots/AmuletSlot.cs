using System;
using System.Collections.Generic;
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

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class AmuletSlot : GearSlot
	{
		private readonly ItemControl _enrichmentControl = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

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

		public Enrichment Enrichment
		{
			[CompilerGenerated]
			get
			{
				return _003CEnrichment_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CEnrichment_003Ek__BackingField, value, delegate(Enrichment v)
				{
					_003CEnrichment_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Enrichment>(OnEnrichmentChanged));
			}
		}

		public AmuletSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			_enrichmentControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_enrichmentControl.Parent = this;
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			base.ItemControl.Item = base.Data.Trinkets[92991];
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int infusionSize = (base.ItemControl.LocalBounds.Size.Y - 4) / 3;
			_enrichmentControl.SetBounds(new Rectangle(base.ItemControl.LocalBounds.Right + 1, base.ItemControl.LocalBounds.Top, infusionSize, infusionSize));
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
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as AmuletTemplateEntry)?.Amulet?.StatChoices ?? base.Data.Trinkets?[92991]?.StatChoices ?? Array.Empty<int>(), (base.TemplatePresenter?.Template[base.Slot] as AmuletTemplateEntry)?.Amulet?.AttributeAdjustment);
			}
			if (_enrichmentControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_enrichmentControl, new Rectangle(a.Location, Point.Zero).Add(_enrichmentControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Enrichment, delegate(Enrichment enrichment)
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
