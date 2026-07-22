using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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
			base.ItemControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths, StaticHosting>.ModuleInstance.ContentsManager.GetTexture("textures\\relic_slot.png");
			base.ItemControl.Placeholder.TextureRegion = new Rectangle(38, 38, 52, 52);
			ItemColor = Color.White;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_titleBounds = new Rectangle(base.ItemControl.LocalBounds.Right + 10, base.ItemControl.LocalBounds.Top + 2, base.Width - base.ItemControl.LocalBounds.Left - 20, Control.Content.DefaultFont16.LineHeight);
			_statBounds = new Rectangle(base.ItemControl.LocalBounds.Right + 10, _titleBounds.Bottom + 2, base.Width - base.ItemControl.LocalBounds.Left - 20, Control.Content.DefaultFont12.LineHeight);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, _relicName, Control.Content.DefaultFont16, _titleBounds, base.ItemControl?.Item?.Rarity.GetColor() ?? (Color.White * 0.5f));
			spriteBatch.DrawStringOnCtrl(this, _relicDescription, Control.Content.DefaultFont12, _statBounds, Color.White, wrap: false, HorizontalAlignment.Left, VerticalAlignment.Top);
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
			else
			{
				_relicName = strings.Relic;
				_relicDescription = string.Empty;
			}
		}

		protected override void SetAnchor()
		{
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Relic relic)
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
			Control a = base.SelectionPanel?.Anchor;
			if (a != null && (base.Children?.Contains(a) ?? false))
			{
				if (((e.NewValue == GameModeType?.PvP && base.Slot == TemplateSlotType.PveRelic) || (e.NewValue == GameModeType?.PvE && base.Slot == TemplateSlotType.PvpRelic)) && PairedSlot != null)
				{
					Rectangle b = PairedSlot.AbsoluteBounds;
					base.SelectionPanel?.SetAnchor(PairedSlot.ItemControl, new Rectangle(b.Location, Point.Zero).Add(PairedSlot.ItemControl.LocalBounds), SelectionTypes.Items, PairedSlot.Slot, GearSubSlotType.Item, delegate(Relic relic)
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
