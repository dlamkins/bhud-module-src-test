using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
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

		private Rectangle _runeBounds;

		public Rune? Rune
		{
			[CompilerGenerated]
			get
			{
				return _003CRune_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CRune_003Ek__BackingField, value, delegate(Rune v)
				{
					_003CRune_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Rune>(OnRuneChanged));
			}
		}

		public PvpAmuletSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
			: base(gearSlot, parent, templatePresenter, selectionPanel, data)
		{
			_runeControl.Parent = this;
			base.ClipsBounds = false;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int upgradeSize = (base.ItemControl.LocalBounds.Size.Y - 4) / 2;
			int iconPadding = 0;
			_ = base.Slot;
			_ = 6;
			int pvpUpgradeSize = 48;
			_runeControl.SetBounds(new Rectangle(base.ItemControl.LocalBounds.Right + 2 + 5 + iconPadding, (base.ItemControl.LocalBounds.Height - pvpUpgradeSize) / 2, pvpUpgradeSize, pvpUpgradeSize));
			_runeBounds = new Rectangle(_runeControl.Right + 10, _runeControl.Top, base.Width - (_runeControl.Right + 2), _runeControl.Height);
			_titleBounds = new Rectangle(_runeBounds.Left, _runeBounds.Top - (Control.Content.DefaultFont16.LineHeight + 2), _runeBounds.Width, Control.Content.DefaultFont16.LineHeight);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
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

		protected override void SetAnchor()
		{
			Rectangle a = base.AbsoluteBounds;
			if (base.ItemControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(base.ItemControl, new Rectangle(a.Location, Point.Zero).Add(base.ItemControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Item, delegate(PvpAmulet pvpAmulet)
				{
					base.TemplatePresenter.Template?.SetItem(base.Slot, TemplateSubSlotType.Item, pvpAmulet);
				});
			}
			if (_runeControl.MouseOver)
			{
				base.SelectionPanel?.SetAnchor(_runeControl, new Rectangle(a.Location, Point.Zero).Add(_runeControl.LocalBounds), SelectionTypes.Items, base.Slot, GearSubSlotType.Rune, delegate(Rune rune)
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
