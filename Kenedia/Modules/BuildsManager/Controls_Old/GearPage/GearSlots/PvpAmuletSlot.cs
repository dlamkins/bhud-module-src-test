using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.TemplateEntries;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class PvpAmuletSlot : GearSlot
	{
		private Rectangle _titleBounds;

		private readonly ItemControl _runeControl = new ItemControl(new DetailedTexture(784323)
		{
			TextureRegion = new Rectangle(38, 38, 52, 52)
		});

		private Rune? _rune;

		private Rectangle _runeBounds;

		public Rune? Rune
		{
			get
			{
				return _rune;
			}
			set
			{
				Common.SetProperty<Rune>(ref _rune, value, new ValueChangedEventHandler<Rune>(OnRuneChanged));
			}
		}

		public PvpAmuletSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			_runeControl.Parent = this;
			base.ClipsBounds = false;
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle localBounds = base.ItemControl.LocalBounds;
			int upgradeSize = (((Rectangle)(ref localBounds)).get_Size().Y - 4) / 2;
			int iconPadding = 0;
			_ = base.Slot;
			_ = 6;
			int pvpUpgradeSize = 48;
			ItemControl runeControl = _runeControl;
			localBounds = base.ItemControl.LocalBounds;
			runeControl.SetBounds(new Rectangle(((Rectangle)(ref localBounds)).get_Right() + 2 + 5 + iconPadding, (base.ItemControl.LocalBounds.Height - pvpUpgradeSize) / 2, pvpUpgradeSize, pvpUpgradeSize));
			_runeBounds = new Rectangle(_runeControl.Right + 10, _runeControl.Top, base.Width - (_runeControl.Right + 2), _runeControl.Height);
			_titleBounds = new Rectangle(((Rectangle)(ref _runeBounds)).get_Left(), ((Rectangle)(ref _runeBounds)).get_Top() - (Control.Content.DefaultFont16.get_LineHeight() + 2), _runeBounds.Width, Control.Content.DefaultFont16.get_LineHeight());
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, GetDisplayString(Rune?.DisplayText ?? string.Empty), UpgradeFont, _runeBounds, UpgradeColor);
		}

		protected override void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
			base.SetItemToSlotControl(sender, e);
			SetItemFromTemplate();
		}

		protected override void SetItemFromTemplate()
		{
			base.SetItemFromTemplate();
			PvpAmuletTemplateEntry pvpAmulet = base.TemplatePresenter?.Template?[base.Slot] as PvpAmuletTemplateEntry;
			if (pvpAmulet != null)
			{
				Rune = pvpAmulet?.Rune;
				base.Item = pvpAmulet?.PvpAmulet;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(PvpAmulet pvpAmulet)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, pvpAmulet);
				});
			}
			if (_runeControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_runeControl, Blish_HUD.RectangleExtension.Add(new Rectangle(((Rectangle)(ref a)).get_Location(), Point.get_Zero()), _runeControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Rune, delegate(Rune rune)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Rune, rune);
				});
			}
		}

		protected override void CreateSubMenus()
		{
			base.CreateSubMenus();
			CreateSubMenu(() => strings.Reset, () => string.Format(strings.ResetEntry, strings.Amulet + " " + strings.And + " " + strings.Rune), delegate
			{
				base.TemplatePresenter?.Template.SetItem<PvpAmulet>(base.Slot, TemplateSubSlotType.Item, null);
				base.TemplatePresenter?.Template.SetItem<Rune>(base.Slot, TemplateSubSlotType.Rune, null);
			}, new List<(Func<string>, Func<string>, Action)>(2)
			{
				(() => strings.Amulet, () => string.Format(strings.ResetEntry, strings.Amulet), delegate
				{
					base.TemplatePresenter?.Template.SetItem<PvpAmulet>(base.Slot, TemplateSubSlotType.Item, null);
				}),
				(() => strings.Rune, () => string.Format(strings.ResetEntry, strings.Rune), delegate
				{
					base.TemplatePresenter?.Template.SetItem<Rune>(base.Slot, TemplateSubSlotType.Rune, null);
				})
			});
		}

		private void OnRuneChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Rune> e)
		{
			_runeControl.Item = Rune;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Rune = null;
		}
	}
}
