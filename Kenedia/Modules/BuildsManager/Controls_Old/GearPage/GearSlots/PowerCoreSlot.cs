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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			base.ItemControl.Placeholder.Texture = AsyncTexture2D.FromAssetId(2630946);
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
			spriteBatch.DrawStringOnCtrl(this, _powerCoreName, Control.Content.DefaultFont16, _titleBounds, (Color)(((_003F?)base.ItemControl?.Item?.Rarity.GetColor()) ?? (Color.get_White() * 0.5f)));
			spriteBatch.DrawStringOnCtrl(this, _powerCoreDescription, Control.Content.DefaultFont12, _statBounds, Color.get_White(), wrap: false, HorizontalAlignment.Left, VerticalAlignment.Top);
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(PowerCore powerCore)
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
