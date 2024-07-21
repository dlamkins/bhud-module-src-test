using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
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
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
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
				((Control)this).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new RecipeSourceTooltipView(recipeSource)));
				Locked = !ServiceContainer.PlayerUnlocksService.RecipeUnlocked(recipeSource.Recipe);
				return;
			}
			TradingPostSource tpSource = itemSource as TradingPostSource;
			if (tpSource != null)
			{
				((Control)this).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new TradingPostSourceTooltipView(tpSource)));
				return;
			}
			VendorSource vendorSource = itemSource as VendorSource;
			if (vendorSource != null)
			{
				((Control)this).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new VendorSourceTooltipView(vendorSource)));
			}
			else
			{
				((Control)this).set_BasicTooltipText(itemSource.DisplayName);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!Active)
			{
				Active = !Active;
			}
			ServiceContainer.AudioService.PlayMenuItemClick();
			((Control)this).OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			base.Paint(spriteBatch, bounds);
			Color iconColor = (Active ? Color.get_LightYellow() : (Color.get_LightGray() * 0.6f));
			Thickness padding;
			if (base.Icon != null)
			{
				Texture2D obj = AsyncTexture2D.op_Implicit(base.Icon);
				padding = ((Control)this).get_Padding();
				int num = (int)((Thickness)(ref padding)).get_Left() + 3;
				padding = ((Control)this).get_Padding();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj, new Rectangle(num, (int)((Thickness)(ref padding)).get_Top() + 2, ((Control)this).get_Size().X - 5, ((Control)this).get_Size().Y - 5), iconColor);
			}
			if (Active)
			{
				padding = ((Control)this).get_Padding();
				int num2 = (int)((Thickness)(ref padding)).get_Left();
				padding = ((Control)this).get_Padding();
				spriteBatch.DrawFrame((Control)(object)this, new Rectangle(num2, (int)((Thickness)(ref padding)).get_Top(), ((Control)this).get_Size().X, ((Control)this).get_Size().Y), Colors.ColonialWhite * 0.5f, 2);
			}
			if (Locked)
			{
				Texture2D obj2 = AsyncTexture2D.op_Implicit(_smallLockIcon);
				padding = ((Control)this).get_Padding();
				int num3 = (int)((Thickness)(ref padding)).get_Left() + 18;
				padding = ((Control)this).get_Padding();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, obj2, new Rectangle(num3, (int)((Thickness)(ref padding)).get_Top() + 17, 10, 11), Color.get_White());
			}
		}

		protected override void DisposeControl()
		{
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
