using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	[DebuggerDisplay("Name = {ItemSource.DisplayName}")]
	public class ItemTab : ImageButton
	{
		private bool _active;

		private readonly AsyncTexture2D _smallLockIcon;

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;
				if (_active)
				{
					this.Activated?.Invoke(this, new CheckChangedEvent(_active));
				}
			}
		}

		public IItemSource ItemSource { get; }

		public bool Locked { get; set; }

		public event EventHandler<CheckChangedEvent> Activated;

		public ItemTab(IItemSource itemSource)
		{
			ItemSource = itemSource;
			base.Icon = itemSource.Icon;
			base.Texture = ServiceContainer.TextureRepository.Textures.ItemTabBackground;
			base.HoverTexture = ServiceContainer.TextureRepository.Textures.ItemTabBackgroundHover;
			_smallLockIcon = ServiceContainer.TextureRepository.GetRefTexture("993724_trimmed.png");
			RecipeSource recipeSource = itemSource as RecipeSource;
			if (recipeSource != null)
			{
				base.Tooltip = new DisposableTooltip(new RecipeSourceTooltipView(recipeSource));
				Locked = !ServiceContainer.PlayerUnlocksService.RecipeUnlocked(recipeSource.Recipe);
				return;
			}
			TradingPostSource tpSource = itemSource as TradingPostSource;
			if (tpSource != null)
			{
				base.Tooltip = new DisposableTooltip(new TradingPostSourceTooltipView(tpSource));
				return;
			}
			VendorSource vendorSource = itemSource as VendorSource;
			if (vendorSource != null)
			{
				base.Tooltip = new DisposableTooltip(new VendorSourceTooltipView(vendorSource));
			}
			else
			{
				base.BasicTooltipText = itemSource.DisplayName;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!Active)
			{
				Active = !Active;
			}
			ServiceContainer.AudioService.PlayMenuItemClick();
			base.OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.Paint(spriteBatch, bounds);
			Color iconColor = (Active ? Color.LightYellow : (Color.LightGray * 0.6f));
			if (base.Icon != null)
			{
				spriteBatch.DrawOnCtrl(this, base.Icon, new Rectangle((int)base.Padding.Left + 3, (int)base.Padding.Top + 2, base.Size.X - 5, base.Size.Y - 5), iconColor);
			}
			if (Active)
			{
				spriteBatch.DrawFrame(this, new Rectangle((int)base.Padding.Left, (int)base.Padding.Top, base.Size.X, base.Size.Y), ContentService.Colors.ColonialWhite * 0.5f, 2);
			}
			if (Locked)
			{
				spriteBatch.DrawOnCtrl(this, _smallLockIcon, new Rectangle((int)base.Padding.Left + 18, (int)base.Padding.Top + 17, 10, 11), Color.White);
			}
		}

		protected override void DisposeControl()
		{
			base.Tooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
