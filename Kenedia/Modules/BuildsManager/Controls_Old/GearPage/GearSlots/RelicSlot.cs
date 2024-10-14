using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.TemplateEntries;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class RelicSlot : GearSlot
	{
		private Rectangle _titleBounds;

		private Rectangle _statBounds;

		private string _relicName = strings.Relic;

		private string _relicDescription;

		public RelicSlot PairedSlot { get; set; }

		public RelicSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			base.ItemControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\relic_slot.png");
			base.ItemControl.Placeholder.TextureRegion = new Rectangle(38, 38, 52, 52);
			ItemColor = Color.get_White();
		}

		public override void RecalculateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int num = ((Rectangle)(ref localBounds)).get_Right() + 10;
			localBounds = base.ItemControl.LocalBounds;
			int num2 = ((Rectangle)(ref localBounds)).get_Top() + 2;
			int width = base.Width;
			localBounds = base.ItemControl.LocalBounds;
			_titleBounds = new Rectangle(num, num2, width - ((Rectangle)(ref localBounds)).get_Left() - 20, Control.Content.DefaultFont16.get_LineHeight());
			localBounds = base.ItemControl.LocalBounds;
			int num3 = ((Rectangle)(ref localBounds)).get_Right() + 10;
			int num4 = ((Rectangle)(ref _titleBounds)).get_Bottom() + 2;
			int width2 = base.Width;
			localBounds = base.ItemControl.LocalBounds;
			_statBounds = new Rectangle(num3, num4, width2 - ((Rectangle)(ref localBounds)).get_Left() - 20, Control.Content.DefaultFont12.get_LineHeight());
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, _relicName, Control.Content.DefaultFont16, _titleBounds, (Color)(((_003F?)base.ItemControl?.Item?.Rarity.GetColor()) ?? (Color.get_White() * 0.5f)));
			spriteBatch.DrawStringOnCtrl(this, _relicDescription, Control.Content.DefaultFont12, _statBounds, Color.get_White(), wrap: false, HorizontalAlignment.Left, VerticalAlignment.Top);
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			PveRelicTemplateEntry pveRelic = base.TemplatePresenter?.Template?[base.Slot] as PveRelicTemplateEntry;
			if (pveRelic != null)
			{
				base.Item = pveRelic.Relic;
				_relicName = pveRelic?.Relic?.Name ?? strings.Relic;
				_relicDescription = pveRelic?.Relic?.Description;
				return;
			}
			PvpRelicTemplateEntry pvpRelic = base.TemplatePresenter?.Template?[base.Slot] as PvpRelicTemplateEntry;
			if (pvpRelic != null)
			{
				base.Item = pvpRelic.Relic;
				_relicName = pvpRelic?.Relic?.Name ?? strings.Relic;
				_relicDescription = pvpRelic?.Relic?.Description;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Relic relic)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, relic);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Relic), delegate
			{
				base.TemplatePresenter?.Template?.SetItem<Relic>(base.Slot, TemplateSubSlotType.Item, null);
			});
		}

		protected override void GameModeChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<GameModeType> e)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			Control a = base.SelectionPanel?.Anchor;
			if (a != null && (base.Children?.Contains(a) ?? false))
			{
				if (((e.NewValue == GameModeType?.PvP && base.Slot == TemplateSlotType.PveRelic) || (e.NewValue == GameModeType?.PvE && base.Slot == TemplateSlotType.PvpRelic)) && PairedSlot != null)
				{
					Rectangle b = PairedSlot.AbsoluteBounds;
					base.SelectionPanel?.SetAnchor(PairedSlot.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref b)).get_Location(), Point.get_Zero()), PairedSlot.ItemControl.LocalBounds), SelectionTypes.Items, PairedSlot.Slot, GearSubSlotType.Item, delegate(Relic relic)
					{
						base.TemplatePresenter.Template?.SetItem(PairedSlot.Slot, TemplateSubSlotType.Item, relic);
					});
				}
			}
			else
			{
				base.GameModeChanged(sender, e);
			}
		}
	}
}
