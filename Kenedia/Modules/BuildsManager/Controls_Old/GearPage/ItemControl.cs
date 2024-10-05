using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage
{
	public class ItemControl : Control
	{
		private readonly DetailedTexture _texture = new DetailedTexture();

		private readonly DetailedTexture _statTexture = new DetailedTexture();

		private DetailedTexture _placeholder = new DetailedTexture();

		private BaseItem? _item;

		private Stat? _stat;

		private int _frameThickness = 2;

		private Color _frameColor = Color.get_White() * 0.15f;

		public bool ShowStat { get; set; } = true;


		public BaseItem? Item
		{
			get
			{
				return _item;
			}
			set
			{
				Common.SetProperty<BaseItem>(ref _item, value, new ValueChangedEventHandler<BaseItem>(ApplyItem));
			}
		}

		public Stat? Stat
		{
			get
			{
				return _stat;
			}
			set
			{
				Common.SetProperty<Stat>(ref _stat, value, new ValueChangedEventHandler<Stat>(ApplyStat));
			}
		}

		public Color TextureColor
		{
			get
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				return (Color)(((_003F?)_texture.DrawColor) ?? Color.get_White());
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_texture.DrawColor = value;
			}
		}

		public DetailedTexture Placeholder
		{
			get
			{
				return _placeholder;
			}
			set
			{
				Common.SetProperty(ref _placeholder, value, new ValueChangedEventHandler<DetailedTexture>(ApplyPlaceholder));
			}
		}

		public bool CaptureInput { get; set; } = true;


		public ItemControl()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			base.Tooltip = new ItemTooltip
			{
				SetLocalizedComment = () => Environment.NewLine + strings.ItemControlClickToCopyItem
			};
		}

		public ItemControl(DetailedTexture placeholder)
			: this()
		{
			Placeholder = placeholder;
		}

		private void ApplyPlaceholder(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<DetailedTexture> e)
		{
		}

		private void ApplyStat(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			_statTexture.Texture = Stat?.Icon;
			ItemTooltip itemTooltip = base.Tooltip as ItemTooltip;
			if (itemTooltip != null)
			{
				itemTooltip.Stat = Stat;
			}
		}

		private void ApplyItem(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<BaseItem> e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			_frameColor = Item?.Rarity.GetColor() ?? (Color.get_White() * 0.15f);
			_texture.Texture = Item?.Icon;
			BaseItem item = Item;
			if (item != null && item.Icon != null)
			{
				int padding = item.Icon.Width / 16;
				_texture.TextureRegion = new Rectangle(padding, padding, Item!.Icon.Width - padding * 2, Item!.Icon.Height - padding * 2);
			}
			ItemTooltip itemTooltip = base.Tooltip as ItemTooltip;
			if (itemTooltip == null)
			{
				return;
			}
			itemTooltip.Item = Item;
			ItemTooltip itemTooltip2 = itemTooltip;
			Func<string> setLocalizedComment;
			switch (Item?.Type)
			{
			case ItemType.Armor:
			case ItemType.Back:
			case ItemType.Trinket:
			case ItemType.Weapon:
				setLocalizedComment = () => Environment.NewLine + strings.ItemControlClickToCopyItem + Environment.NewLine + strings.ItemControlClickToCopyStat;
				break;
			default:
				setLocalizedComment = () => Environment.NewLine + strings.ItemControlClickToCopyItem;
				break;
			}
			itemTooltip2.SetLocalizedComment = setLocalizedComment;
		}

		public override void RecalculateLayout()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int size = Math.Min(base.Width, base.Height);
			int padding = 3;
			_frameThickness = CalculateFrameThickness();
			DetailedTexture statTexture = _statTexture;
			Rectangle bounds = _texture.Bounds;
			statTexture.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Center().Add(new Point(-padding, -padding)), new Point((size - padding * 2) / 2));
			_texture.Bounds = new Rectangle(_frameThickness, _frameThickness, base.Width - _frameThickness * 2, base.Height - _frameThickness * 2);
			_placeholder.Bounds = new Rectangle(_frameThickness, _frameThickness, base.Width - _frameThickness * 2, base.Height - _frameThickness * 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			if (Item == null)
			{
				_placeholder.Draw(this, spriteBatch, base.RelativeMousePosition, TextureColor);
			}
			else
			{
				_texture.Draw(this, spriteBatch, base.RelativeMousePosition, TextureColor);
			}
			spriteBatch.DrawFrame(this, bounds, _frameColor, _frameThickness);
			if (ShowStat)
			{
				_statTexture.Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		private int CalculateFrameThickness()
		{
			int size = Math.Min(base.Width, base.Height);
			return Math.Max(2, size / 24);
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (Control.Input.Keyboard.KeysDown.Contains((Keys)162))
			{
				string s = ((!_statTexture.Hovered) ? Item?.Name : Stat?.Name);
				if (!string.IsNullOrEmpty(s))
				{
					await ClipboardUtil.WindowsClipboardService.SetTextAsync(s);
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (!CaptureInput)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}

		protected override void DisposeControl()
		{
			Item = null;
			Stat = null;
			_texture?.Dispose();
			_statTexture?.Dispose();
			_placeholder?.Dispose();
			base.DisposeControl();
		}
	}
}
