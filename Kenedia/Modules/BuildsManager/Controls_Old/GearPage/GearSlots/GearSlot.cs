using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots
{
	public class GearSlot : Container
	{
		private TemplateSlotType _slot = TemplateSlotType.None;

		protected int MaxTextLength = 52;

		protected Color StatColor = Color.get_White();

		protected Color UpgradeColor = Color.get_Orange();

		protected Color InfusionColor = new Color(153, 238, 221);

		protected Color ItemColor = Color.get_Gray();

		protected BitmapFont StatFont = Control.Content.DefaultFont16;

		protected BitmapFont UpgradeFont = Control.Content.DefaultFont18;

		protected BitmapFont InfusionFont = Control.Content.DefaultFont12;

		private TemplatePresenter _templatePresenter;

		protected ItemControl ItemControl { get; } = new ItemControl();


		public BaseItem Item
		{
			get
			{
				return ItemControl.Item;
			}
			set
			{
				if (value != ItemControl.Item)
				{
					_ = ItemControl.Item;
					ItemControl.Item = value;
				}
			}
		}

		public TemplateSlotType Slot
		{
			get
			{
				return _slot;
			}
			set
			{
				Common.SetProperty(ref _slot, value, new Action(ApplySlot));
			}
		}

		public SelectionPanel SelectionPanel { get; }

		public Data Data { get; }

		public List<GearSlot> SlotGroup { get; set; }

		protected TemplatePresenter TemplatePresenter
		{
			get
			{
				return _templatePresenter;
			}
			private set
			{
				Common.SetProperty(ref _templatePresenter, value, new ValueChangedEventHandler<TemplatePresenter>(OnTemplatePresenterChanged));
			}
		}

		public GearSlot(TemplateSlotType gearSlot, Container parent, TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			SelectionPanel = selectionPanel;
			Data = data;
			Slot = gearSlot;
			base.Parent = parent;
			base.Size = new Point(380, 64);
			base.ClipsBounds = true;
			base.Menu = new ContextMenuStrip();
			CreateSubMenus();
			ItemControl.Parent = this;
			SetItemFromTemplate();
			Data.Loaded += new EventHandler(Data_Loaded);
			if (Data.IsLoaded)
			{
				OnDataLoaded();
			}
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			OnDataLoaded();
		}

		protected virtual void OnDataLoaded()
		{
		}

		protected virtual void OnTemplatePresenterChanged(object sender, ValueChangedEventArgs<TemplatePresenter> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.GameModeChanged -= new ValueChangedEventHandler<GameModeType>(GameModeChanged);
				e.OldValue!.TemplateSlotChanged -= new TemplateSlotChangedEventHandler(TemplateSlotChanged);
				e.OldValue!.TemplateChanged -= new ValueChangedEventHandler<Template>(TemplateChanged);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.GameModeChanged += new ValueChangedEventHandler<GameModeType>(GameModeChanged);
				e.NewValue!.TemplateSlotChanged += new TemplateSlotChangedEventHandler(TemplateSlotChanged);
				e.NewValue!.TemplateChanged += new ValueChangedEventHandler<Template>(TemplateChanged);
			}
		}

		private void TemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			SetItemFromTemplate();
		}

		private void TemplateSlotChanged(object sender, TemplateSlotChangedEventArgs e)
		{
			if (e.Slot == Slot)
			{
				SetItemToSlotControl(sender, e);
			}
		}

		protected virtual void GameModeChanged(object sender, ValueChangedEventArgs<GameModeType> e)
		{
			Control a = SelectionPanel?.Anchor;
			if (a != null && (base.Children?.Contains(a) ?? false))
			{
				SelectionPanel?.ResetAnchor();
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
		}

		protected virtual void ApplySlot()
		{
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			if (new Dictionary<TemplateSlotType, int>
			{
				{
					TemplateSlotType.AquaBreather,
					156308
				},
				{
					TemplateSlotType.Head,
					156307
				},
				{
					TemplateSlotType.Shoulder,
					156311
				},
				{
					TemplateSlotType.Chest,
					156297
				},
				{
					TemplateSlotType.Hand,
					156306
				},
				{
					TemplateSlotType.Leg,
					156309
				},
				{
					TemplateSlotType.Foot,
					156300
				},
				{
					TemplateSlotType.MainHand,
					156316
				},
				{
					TemplateSlotType.OffHand,
					156320
				},
				{
					TemplateSlotType.Aquatic,
					156313
				},
				{
					TemplateSlotType.AltMainHand,
					156316
				},
				{
					TemplateSlotType.AltOffHand,
					156320
				},
				{
					TemplateSlotType.AltAquatic,
					156313
				},
				{
					TemplateSlotType.Back,
					156293
				},
				{
					TemplateSlotType.Amulet,
					156310
				},
				{
					TemplateSlotType.Accessory_1,
					156298
				},
				{
					TemplateSlotType.Accessory_2,
					156299
				},
				{
					TemplateSlotType.Ring_1,
					156301
				},
				{
					TemplateSlotType.Ring_2,
					156302
				},
				{
					TemplateSlotType.PvpAmulet,
					784322
				}
			}.TryGetValue(Slot, out var assetId))
			{
				ItemControl.Placeholder.Texture = AsyncTexture2D.FromAssetId(assetId);
				ItemControl.Placeholder.TextureRegion = new Rectangle(38, 38, 52, 52);
			}
			if (Slot.IsArmor() || Slot.IsWeapon() || Slot.IsJewellery())
			{
				ItemControl.TextureColor = Color.get_Gray();
			}
			RecalculateLayout();
		}

		protected string GetDisplayString(string s)
		{
			if (s.Length <= MaxTextLength)
			{
				return s;
			}
			return s.Substring(0, MaxTextLength) + "...";
		}

		public override void RecalculateLayout()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int size = Math.Min(base.Width, base.Height);
			ItemControl itemControl = ItemControl;
			Rectangle absoluteBounds = base.AbsoluteBounds;
			itemControl.Location = ((Rectangle)(ref absoluteBounds)).get_Location();
			ItemControl.Size = new Point(size);
		}

		protected virtual void SetItemToSlotControl(object sender, TemplateSlotChangedEventArgs e)
		{
		}

		protected virtual void SetItemFromTemplate()
		{
		}

		protected void CreateSubMenu(Func<string> menuGroupName, Func<string> menuGroupTooltip = null, Action menuGroupAction = null, List<(Func<string> text, Func<string> tooltip, Action action)> menuItems = null)
		{
			if (menuItems == null)
			{
				base.Menu.AddMenuItem(new ContextMenuItem(menuGroupName, menuGroupAction, menuGroupTooltip));
				return;
			}
			ContextMenuStrip contextMenuStrip2 = (base.Menu.AddMenuItem(new ContextMenuItem(menuGroupName, menuGroupAction, menuGroupTooltip)).Submenu = new ContextMenuStrip());
			ContextMenuStrip menuGroup = contextMenuStrip2;
			foreach (var (text, tooltip, action) in menuItems ?? new List<(Func<string>, Func<string>, Action)>())
			{
				menuGroup.AddMenuItem(new ContextMenuItem(text, action, tooltip));
			}
		}

		protected virtual void CreateSubMenus()
		{
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			ItemControl?.Dispose();
		}
	}
}
