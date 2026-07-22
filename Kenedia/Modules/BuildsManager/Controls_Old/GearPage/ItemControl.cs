using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
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

		private int _frameThickness = 2;

		private Color _frameColor = Color.White * 0.15f;

		public bool ShowStat { get; set; } = true;


		public BaseItem? Item
		{
			[CompilerGenerated]
			get
			{
				return _003CItem_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CItem_003Ek__BackingField, value, delegate(BaseItem v)
				{
					_003CItem_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<BaseItem>(ApplyItem));
			}
		}

		public Stat? Stat
		{
			[CompilerGenerated]
			get
			{
				return _003CStat_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CStat_003Ek__BackingField, value, delegate(Stat v)
				{
					_003CStat_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Stat>(ApplyStat));
			}
		}

		public Color TextureColor
		{
			get
			{
				return _texture.DrawColor ?? Color.White;
			}
			set
			{
				_texture.DrawColor = value;
			}
		}

		public DetailedTexture Placeholder
		{
			[CompilerGenerated]
			get
			{
				return _003CPlaceholder_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CPlaceholder_003Ek__BackingField, value, delegate(DetailedTexture v)
				{
					_003CPlaceholder_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<DetailedTexture>(ApplyPlaceholder));
			}
		}

		public bool CaptureInput { get; set; }

		public ItemControl()
		{
			_003CPlaceholder_003Ek__BackingField = new DetailedTexture();
			CaptureInput = true;
			base._002Ector();
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
			_statTexture.Texture = Stat?.Icon.Texture;
			_statTexture.TextureRegion = Stat?.Icon.TextureRegion ?? Rectangle.Empty;
			ItemTooltip itemTooltip = base.Tooltip as ItemTooltip;
			if (itemTooltip != null)
			{
				itemTooltip.Stat = Stat;
			}
		}

		private void ApplyItem(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<BaseItem> e)
		{
			_frameColor = Item?.Rarity.GetColor() ?? (Color.White * 0.15f);
			_texture.Texture = TexturesService.GetAsyncTexture(Item?.AssetId);
			AsyncTexture2D icon = TexturesService.GetAsyncTexture(Item?.AssetId);
			if (icon != null)
			{
				int padding = icon.Width / 16;
				_texture.TextureRegion = new Rectangle(padding, padding, icon.Width - padding * 2, icon.Height - padding * 2);
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
			base.RecalculateLayout();
			int size = Math.Min(base.Width, base.Height);
			int padding = 3;
			_frameThickness = CalculateFrameThickness();
			_statTexture.Bounds = new Rectangle(_texture.Bounds.Center.Add(new Point(-padding, -padding)), new Point((size - padding * 2) / 2));
			_texture.Bounds = new Rectangle(_frameThickness, _frameThickness, base.Width - _frameThickness * 2, base.Height - _frameThickness * 2);
			Placeholder.Bounds = new Rectangle(_frameThickness, _frameThickness, base.Width - _frameThickness * 2, base.Height - _frameThickness * 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Item == null)
			{
				Placeholder.Draw(this, spriteBatch, base.RelativeMousePosition, TextureColor);
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
			if (Control.Input.Keyboard.KeysDown.Contains(Keys.LeftControl))
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
			Placeholder?.Dispose();
			base.DisposeControl();
		}
	}
}
