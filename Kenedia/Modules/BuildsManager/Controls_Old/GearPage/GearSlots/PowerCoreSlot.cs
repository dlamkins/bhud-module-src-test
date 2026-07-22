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
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class PowerCoreSlot : GearSlot
	{
		private Rectangle _titleBounds;

		private Rectangle _statBounds;

		private string _powerCoreName = strings.PowerCore;

		private string _powerCoreDescription;

		public PowerCoreSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			base.ItemControl.Placeholder.Texture = AsyncTexture2D.FromAssetId(2630946);
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
			spriteBatch.DrawStringOnCtrl(this, _powerCoreName, Control.Content.DefaultFont16, _titleBounds, base.ItemControl?.Item?.Rarity.GetColor() ?? (Color.White * 0.5f));
			spriteBatch.DrawStringOnCtrl(this, _powerCoreDescription, Control.Content.DefaultFont12, _statBounds, Color.White, wrap: false, HorizontalAlignment.Left, VerticalAlignment.Top);
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			PowerCoreTemplateEntry powerCore = base.TemplatePresenter?.Template?[base.Slot] as PowerCoreTemplateEntry;
			if (powerCore != null)
			{
				base.Item = powerCore.Item;
				_powerCoreName = powerCore?.PowerCore?.Name ?? strings.PowerCore;
				_powerCoreDescription = powerCore?.PowerCore?.Description ?? string.Empty;
			}
			else
			{
				_powerCoreName = strings.PowerCore;
				_powerCoreDescription = string.Empty;
			}
		}

		protected override void SetAnchor()
		{
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(PowerCore powerCore)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, powerCore);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.PowerCore), delegate
			{
				base.TemplatePresenter?.Template?.SetItem<PowerCore>(base.Slot, TemplateSubSlotType.Item, null);
			});
		}
	}
}
