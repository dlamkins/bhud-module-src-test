using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class StatContainer : Control
	{
		private readonly Services _services;

		private readonly string _countText;

		private readonly BitmapFont _countFont;

		private readonly Color _countColor;

		private readonly AsyncTexture2D _inventorySlotTexture;

		private readonly Color _countBackgroundColor;

		private readonly Rectangle _countTextBounds;

		private readonly Rectangle _countBackgroundBounds;

		private readonly Color _rarityBorderColor;

		private readonly int _rarityBorderLength;

		private readonly float _statIconOpacity;

		private readonly AsyncTexture2D _statIconTexture;

		private readonly Rectangle _inventorySlotBounds;

		private readonly Rectangle _statIconBounds;

		private const int STAT_ICON_MARGIN = 1;

		private const int INVENTORY_SLOT_MARGIN = 1;

		public StatContainer(Stat stat, PanelType panelType, Model model, Services services)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			Stat stat2 = stat;
			Model model2 = model;
			Services services2 = services;
			((Control)this)._002Ector();
			_services = services2;
			_countText = stat2.Signed_Count.ToString();
			BitmapFont countFont = (_countFont = services2.FontService.Fonts[services2.SettingService.CountFontSizeSetting.get_Value()]);
			_countBackgroundColor = services2.SettingService.CountBackgroundColorSetting.get_Value().GetColor() * ((float)services2.SettingService.CountBackgroundOpacitySetting.get_Value() / 255f);
			_countColor = ((stat2.Signed_Count.Value >= 0) ? services2.SettingService.PositiveCountTextColorSetting.get_Value().GetColor() : services2.SettingService.NegativeCountTextColorSetting.get_Value().GetColor());
			_inventorySlotTexture = services2.TextureService.InventorySlotBackgroundTexture;
			_statIconTexture = GetStatIconTexture(stat2, services2);
			_rarityBorderColor = ColorService.GetRarityBorderColor(stat2.Details.Rarity);
			_statIconOpacity = ((stat2.Signed_Count.Value > 0) ? 1f : ((float)services2.SettingService.NegativeCountIconOpacitySetting.get_Value() / 255f));
			((Control)this).set_Tooltip((Tooltip)(object)new StatTooltip(stat2, _statIconTexture, panelType, services2));
			int statIconOrigin = 2;
			int statIconSize = (int)services2.SettingService.StatIconSizeSetting.get_Value();
			int inventorySlotSize = statIconSize + 2;
			((Control)this).set_Size(new Point(inventorySlotSize + 2));
			_rarityBorderLength = inventorySlotSize;
			_inventorySlotBounds = new Rectangle(1, 1, inventorySlotSize, inventorySlotSize);
			_statIconBounds = new Rectangle(statIconOrigin, statIconOrigin, statIconSize, statIconSize);
			_countTextBounds = new Rectangle(statIconOrigin, statIconOrigin + 1, statIconSize - 5, statIconSize - 2);
			_countBackgroundBounds = new Rectangle(statIconOrigin, statIconOrigin, statIconSize, countFont.get_LineHeight());
			if (panelType == PanelType.IgnoredStats)
			{
				return;
			}
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				StatContextMenuStrip contextMenuStrip = new StatContextMenuStrip(stat2, panelType, model2, services2);
				((Control)contextMenuStrip).add_Hidden((EventHandler<EventArgs>)delegate
				{
					((Control)contextMenuStrip).Dispose();
				});
				contextMenuStrip.Show(GameService.Input.get_Mouse().get_Position());
			});
		}

		private static AsyncTexture2D GetStatIconTexture(Stat stat, Services services)
		{
			return (AsyncTexture2D)(stat.Details.State switch
			{
				StatApiDetailsState.GoldCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.GoldCoinTexture), 
				StatApiDetailsState.SilveCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.SilverCoinTexture), 
				StatApiDetailsState.CopperCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.CopperCoinTexture), 
				_ => services.TextureService.GetTextureFromAssetCacheOrFallback(stat.Details.IconAssetId), 
			});
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			DrawInventorySlot(spriteBatch);
			DrawStatIcon(spriteBatch);
			DrawCountBackground(spriteBatch);
			DrawCountText(spriteBatch);
			if (_services.SettingService.RarityIconBorderIsVisibleSetting.get_Value())
			{
				DrawRarityBorder(spriteBatch);
			}
		}

		private void DrawInventorySlot(SpriteBatch spriteBatch)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_inventorySlotTexture), _inventorySlotBounds, (Rectangle?)_inventorySlotTexture.get_Texture().get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		private void DrawStatIcon(SpriteBatch spriteBatch)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_statIconTexture), _statIconBounds, (Rectangle?)_statIconTexture.get_Texture().get_Bounds(), Color.get_White() * _statIconOpacity, 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		private void DrawCountBackground(SpriteBatch spriteBatch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _countBackgroundBounds, (Rectangle?)Rectangle.get_Empty(), _countBackgroundColor);
		}

		private void DrawCountText(SpriteBatch spriteBatch)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _countText, _countFont, _countTextBounds, _countColor, false, true, 1, _services.SettingService.CountHoritzontalAlignmentSetting.get_Value(), (VerticalAlignment)0);
		}

		private void DrawRarityBorder(SpriteBatch spriteBatch)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			int thickness = 2;
			int left = 1;
			int top = 1;
			int right = left + _rarityBorderLength - thickness;
			int bottom = top + _rarityBorderLength - thickness;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(left, top, _rarityBorderLength, thickness), (Rectangle?)Rectangle.get_Empty(), _rarityBorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(left, top, thickness, _rarityBorderLength), (Rectangle?)Rectangle.get_Empty(), _rarityBorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(left, bottom, _rarityBorderLength, thickness), (Rectangle?)Rectangle.get_Empty(), _rarityBorderColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(right, top, thickness, _rarityBorderLength), (Rectangle?)Rectangle.get_Empty(), _rarityBorderColor);
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
