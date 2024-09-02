using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class StatContainer : Container
	{
		private readonly StatTooltip _statTooltip;

		public StatContainer(Stat stat, PanelType panelType, SafeList<int> ignoredItemApiIds, SafeList<int> favoriteItemApiIds, Services services)
			: this()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			int iconSize = (int)services.SettingService.StatIconSizeSetting.get_Value();
			int iconMargin = 1;
			int backgroundSize = iconSize + 2 * iconMargin;
			int backgroundMargin = 1;
			int rarityBorderThickness = 2;
			int rarityBorderLength = backgroundSize;
			int rarityBorderLeftOrTopLocation = backgroundMargin;
			int rarityBorderRightOrBottomLocation = rarityBorderLeftOrTopLocation + rarityBorderLength - rarityBorderThickness;
			((Control)this).set_Size(new Point(backgroundSize + 2 * backgroundMargin));
			AsyncTexture2D statIconTexture = GetStatIconTexture(stat, services);
			StatTooltip statTooltip = (_statTooltip = new StatTooltip(stat, statIconTexture, panelType, services));
			Image val = new Image(services.TextureService.InventorySlotBackgroundTexture);
			((Control)val).set_Tooltip((Tooltip)(object)statTooltip);
			((Control)val).set_Size(new Point(backgroundSize));
			((Control)val).set_Location(new Point(backgroundMargin));
			((Control)val).set_Parent((Container)(object)this);
			Image val2 = new Image(statIconTexture);
			((Control)val2).set_Tooltip((Tooltip)(object)statTooltip);
			((Control)val2).set_Opacity((stat.Count > 0) ? 1f : ((float)services.SettingService.NegativeCountIconOpacitySetting.get_Value() / 255f));
			((Control)val2).set_Size(new Point(iconSize));
			((Control)val2).set_Location(new Point(backgroundMargin + iconMargin));
			((Control)val2).set_Parent((Container)(object)this);
			Label val3 = new Label();
			val3.set_Text(stat.Count.ToString());
			((Control)val3).set_Tooltip((Tooltip)(object)statTooltip);
			val3.set_Font(services.FontService.Fonts[services.SettingService.CountFontSizeSetting.get_Value()]);
			val3.set_TextColor((stat.Count >= 0) ? services.SettingService.PositiveCountTextColorSetting.get_Value().GetColor() : services.SettingService.NegativeCountTextColorSetting.get_Value().GetColor());
			val3.set_HorizontalAlignment(services.SettingService.CountHoritzontalAlignmentSetting.get_Value());
			((Control)val3).set_BackgroundColor(services.SettingService.CountBackgroundColorSetting.get_Value().GetColor() * ((float)services.SettingService.CountBackgroundOpacitySetting.get_Value() / 255f));
			val3.set_StrokeText(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Width(iconSize - 5);
			((Control)val3).set_Location(new Point(backgroundMargin + iconMargin, backgroundMargin + iconMargin + 1));
			((Control)val3).set_Parent((Container)(object)this);
			if (services.SettingService.RarityIconBorderIsVisibleSetting.get_Value())
			{
				AddRarityBorder(stat.Details.Rarity, rarityBorderLeftOrTopLocation, rarityBorderRightOrBottomLocation, rarityBorderThickness, rarityBorderLength, statTooltip);
			}
			if (panelType == PanelType.IgnoredItems)
			{
				return;
			}
			((Control)this).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				StatContextMenuStrip contextMenuStrip = new StatContextMenuStrip(stat, panelType, ignoredItemApiIds, favoriteItemApiIds, services);
				((Control)contextMenuStrip).add_Hidden((EventHandler<EventArgs>)delegate
				{
					((Control)contextMenuStrip).Dispose();
				});
				((ContextMenuStrip)contextMenuStrip).Show(GameService.Input.get_Mouse().get_Position());
			});
		}

		private static AsyncTexture2D GetStatIconTexture(Stat stat, Services services)
		{
			return (AsyncTexture2D)(stat.Details.State switch
			{
				ApiStatDetailsState.GoldCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.GoldCoinTexture), 
				ApiStatDetailsState.SilveCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.SilverCoinTexture), 
				ApiStatDetailsState.CopperCoinCustomStat => AsyncTexture2D.op_Implicit(services.TextureService.CopperCoinTexture), 
				_ => services.TextureService.GetTextureFromAssetCacheOrFallback(stat.Details.IconAssetId), 
			});
		}

		private void AddRarityBorder(ItemRarity rarity, int borderLeftOrTopLocation, int borderRightOrBottomLocation, int borderThickness, int borderLength, StatTooltip tooltip)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			Color borderColor = ColorService.GetRarityBorderColor(rarity);
			new BorderContainer(new Point(borderLeftOrTopLocation), new Point(borderThickness, borderLength), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderRightOrBottomLocation, borderLeftOrTopLocation), new Point(borderThickness, borderLength), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderLeftOrTopLocation), new Point(borderLength, borderThickness), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
			new BorderContainer(new Point(borderLeftOrTopLocation, borderRightOrBottomLocation), new Point(borderLength, borderThickness), borderColor, (Tooltip)(object)tooltip, (Container)(object)this);
		}

		protected override void DisposeControl()
		{
			StatTooltip statTooltip = _statTooltip;
			if (statTooltip != null)
			{
				((Control)statTooltip).Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
