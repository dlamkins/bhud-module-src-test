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
	public class RingSlot : GearSlot
	{
		private readonly ItemControl _infusion1Control = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _infusion2Control = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private readonly ItemControl _infusion3Control = new ItemControl(new DetailedTexture
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Stat? _stat;

		private Infusion? _infusion1;

		private Infusion? _infusion2;

		private Infusion? _infusion3;

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

		public Infusion? Infusion1
		{
			get
			{
				return _infusion1;
			}
			set
			{
				Common.SetProperty<Infusion>(ref _infusion1, value, new ValueChangedEventHandler<Infusion>(OnInfusion1Changed));
			}
		}

		public Infusion? Infusion2
		{
			get
			{
				return _infusion2;
			}
			set
			{
				Common.SetProperty<Infusion>(ref _infusion2, value, new ValueChangedEventHandler<Infusion>(OnInfusion2Changed));
			}
		}

		public Infusion? Infusion3
		{
			get
			{
				return _infusion3;
			}
			set
			{
				Common.SetProperty<Infusion>(ref _infusion3, value, new ValueChangedEventHandler<Infusion>(OnInfusion3Changed));
			}
		}

		public RingSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			_infusion1Control.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_infusion2Control.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_infusion3Control.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\infusionslot.png");
			_infusion1Control.Parent = this;
			_infusion2Control.Parent = this;
			_infusion3Control.Parent = this;
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			base.ItemControl.Item = base.Data.Trinkets[91234];
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
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int infusionSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 3;
			ItemControl infusion1Control = _infusion1Control;
			localBounds = base.ItemControl.LocalBounds;
			int num = ((Rectangle)(ref localBounds)).get_Right() + 2;
			localBounds = base.ItemControl.LocalBounds;
			infusion1Control.SetBounds(new Rectangle(num, ((Rectangle)(ref localBounds)).get_Top() + 0, infusionSize, infusionSize));
			ItemControl infusion2Control = _infusion2Control;
			localBounds = base.ItemControl.LocalBounds;
			int num2 = ((Rectangle)(ref localBounds)).get_Right() + 2;
			localBounds = base.ItemControl.LocalBounds;
			infusion2Control.SetBounds(new Rectangle(num2, ((Rectangle)(ref localBounds)).get_Top() + (infusionSize + 2), infusionSize, infusionSize));
			ItemControl infusion3Control = _infusion3Control;
			localBounds = base.ItemControl.LocalBounds;
			int num3 = ((Rectangle)(ref localBounds)).get_Right() + 2;
			localBounds = base.ItemControl.LocalBounds;
			infusion3Control.SetBounds(new Rectangle(num3, ((Rectangle)(ref localBounds)).get_Top() + (infusionSize + 2) * 2, infusionSize, infusionSize));
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			RingTemplateEntry ring = base.TemplatePresenter?.Template?[base.Slot] as RingTemplateEntry;
			if (ring != null)
			{
				Infusion1 = ring?.Infusion1;
				Infusion2 = ring?.Infusion2;
				Infusion3 = ring?.Infusion3;
				Stat = ring?.Stat;
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
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Stats, base.Slot, GearSubSlotType.None, delegate(Stat stat)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Stat, stat);
				}, (base.TemplatePresenter?.Template[base.Slot] as RingTemplateEntry).Ring?.StatChoices, (base.TemplatePresenter?.Template[base.Slot] as RingTemplateEntry).Ring?.AttributeAdjustment);
			}
			if (_infusion1Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusion1Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusion1Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion1, infusion);
				});
			}
			if (_infusion2Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusion2Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusion2Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion2, infusion);
				});
			}
			if (_infusion3Control.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_infusion3Control, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _infusion3Control.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Infusion, delegate(Infusion infusion)
				{
					base.TemplatePresenter?.Template?.SetItem(base.Slot, TemplateSubSlotType.Infusion3, infusion);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Stat + " " + strings.And + " " + strings.Infusions), delegate
			{
				base.TemplatePresenter?.Template?.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
				base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion2, null);
				base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion3, null);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.ResetEntry, strings.Stat), delegate
				{
					base.TemplatePresenter?.Template?.SetItem<Stat>(base.Slot, TemplateSubSlotType.Stat, null);
				}),
				(() => strings.Infusions, () => string.Format(strings.ResetEntry, strings.Infusions), delegate
				{
					base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion1, null);
					base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion2, null);
					base.TemplatePresenter?.Template?.SetItem<Infusion>(base.Slot, TemplateSubSlotType.Infusion3, null);
				})
			});
			CreateSubMenu(() => strings.Fill, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.And + " " + strings.Infusions + " " + strings.EmptyJewellerySlots), delegate
			{
				SetGroupStat(Stat);
				SetGroupInfusion(Infusion1);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.FillEntry, strings.Stat + " " + strings.EmptyJewellerySlots), delegate
				{
					SetGroupStat(Stat);
				}),
				(() => strings.Infusions, () => string.Format(strings.FillEntry, strings.Infusions + " " + strings.EmptyJewellerySlots), delegate
				{
					SetGroupInfusion(Infusion1);
				})
			});
			CreateSubMenu(() => strings.Override, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.And + " " + strings.Infusions + " " + strings.JewellerySlots), delegate
			{
				SetGroupStat(Stat, overrideExisting: true);
				SetGroupInfusion(Infusion1, overrideExisting: true);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Stat, () => string.Format(strings.OverrideEntry, strings.Stat + " " + strings.JewellerySlots), delegate
				{
					SetGroupStat(Stat, overrideExisting: true);
				}),
				(() => strings.Infusions, () => string.Format(strings.OverrideEntry, strings.Infusions + " " + strings.JewellerySlots), delegate
				{
					SetGroupInfusion(Infusion1, overrideExisting: true);
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

		private void OnStatChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			base.ItemControl.Stat = Stat;
		}

		private void OnInfusion1Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusion1Control.Item = Infusion1;
		}

		private void OnInfusion2Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusion2Control.Item = Infusion2;
		}

		private void OnInfusion3Changed(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Infusion> e)
		{
			_infusion3Control.Item = Infusion3;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Stat = null;
			Infusion1 = null;
			Infusion2 = null;
			Infusion3 = null;
		}
	}
}
