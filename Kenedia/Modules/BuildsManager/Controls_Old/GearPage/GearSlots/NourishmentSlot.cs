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
	public class NourishmentSlot : GearSlot
	{
		private Rectangle _titleBounds;

		private Rectangle _statBounds;

		public NourishmentSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel)
			: base(gearSlot, parent, templatePresenter, selectionPanel)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			base.ItemControl.Placeholder.Texture = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\foodslot.png");
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
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, base.ItemControl?.Item?.Name ?? strings.Nourishment, Control.Content.DefaultFont16, _titleBounds, (Color)(((_003F?)base.ItemControl?.Item?.Rarity.GetColor()) ?? (Color.get_White() * 0.5f)));
			spriteBatch.DrawStringOnCtrl(this, (base.ItemControl?.Item as Nourishment)?.Details.Description ?? base.ItemControl?.Item?.Description, Control.Content.DefaultFont12, _statBounds, Color.get_White(), wrap: false, HorizontalAlignment.Left, VerticalAlignment.Top);
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
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(Nourishment nourishment)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, nourishment);
				});
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
			NourishmentTemplateEntry nourishment = base.TemplatePresenter?.Template?[base.Slot] as NourishmentTemplateEntry;
			if (nourishment != null)
			{
				base.Item = nourishment.Item;
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Nourishment), delegate
			{
				base.TemplatePresenter?.Template?.SetItem<Nourishment>(base.Slot, TemplateSubSlotType.Item, null);
			});
		}
	}
}
