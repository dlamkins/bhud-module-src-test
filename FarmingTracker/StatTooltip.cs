using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class StatTooltip : DisposableTooltip
	{
		public StatTooltip(Stat stat, long? customStatProfitInCopper, AsyncTexture2D statIconTexture, PanelType panelType, Services services)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel rootFlowPanel = val;
			BitmapFont font = services.FontService.Fonts[(FontSize)16];
			switch (stat.Details.State)
			{
			case ApiStatDetailsState.SetByApi:
				AddTitle(stat, statIconTexture, (Container)(object)rootFlowPanel);
				AddDescription(stat, font, (Container)(object)rootFlowPanel);
				if (panelType != PanelType.IgnoredItems)
				{
					StatTooltipService.AddProfitTable(stat, customStatProfitInCopper, font, services, (Container)(object)rootFlowPanel);
					StatTooltipService.AddText("\nRight click for more options.", font, (Container)(object)rootFlowPanel);
				}
				else
				{
					StatTooltipService.AddText("\nLeft click to unignore this item.", font, (Container)(object)rootFlowPanel);
				}
				break;
			case ApiStatDetailsState.GoldCoinCustomStat:
			case ApiStatDetailsState.SilveCoinCustomStat:
			case ApiStatDetailsState.CopperCoinCustomStat:
				StatTooltipService.AddText($"{stat.Count}\nChanges in 'raw gold'.\nIn other words coins spent or gained.", font, (Container)(object)rootFlowPanel);
				break;
			case ApiStatDetailsState.MissingBecauseUnknownByApi:
				StatTooltipService.AddText($"Unknown item/currency (ID: {stat.ApiId})\n" + "GW2 API has no information about it.\nThis issue typically occurs for items related to renown hearts.", font, (Container)(object)rootFlowPanel);
				if (panelType == PanelType.SummaryRegularItems)
				{
					StatTooltipService.AddText("\nRight click to search its ID in the wiki in your default browser.", font, (Container)(object)rootFlowPanel);
				}
				else
				{
					StatTooltipService.AddText("\nLeft click to unignore this item.", font, (Container)(object)rootFlowPanel);
				}
				break;
			case ApiStatDetailsState.MissingBecauseApiNotCalledYet:
			{
				string errorMessage = $"Module error: API was not called for stat. Stats like that should not be displayed (id: {stat.ApiId}).";
				Module.Logger.Error(errorMessage);
				StatTooltipService.AddText(errorMessage, font, (Container)(object)rootFlowPanel);
				break;
			}
			default:
				Module.Logger.Error(Helper.CreateSwitchCaseNotFoundMessage(stat.Details.State, "ApiStatDetailsState", "use error tooltip"));
				StatTooltipService.AddText("Module error: unexpected state", font, (Container)(object)rootFlowPanel);
				break;
			}
		}

		private static void AddTitle(Stat stat, AsyncTexture2D statIconTexture, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel iconAndNamePanel = val;
			Image val2 = new Image(statIconTexture);
			((Control)val2).set_Size(new Point(50));
			((Control)val2).set_Parent((Container)(object)iconAndNamePanel);
			Image iconImage = val2;
			Color rarityColor = ColorService.GetRarityTextColor(stat.Details.Rarity);
			Label val3 = new Label();
			val3.set_Text($"{stat.Count} {stat.Details.Name}");
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val3.set_TextColor(rarityColor);
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Parent((Container)(object)iconAndNamePanel);
			Label nameLabel = val3;
			((Control)nameLabel).set_Left(((Control)iconImage).get_Right() + 5);
			((Control)nameLabel).set_Top((((Control)iconImage).get_Height() - ((Control)nameLabel).get_Height()) / 2);
		}

		private static void AddDescription(Stat stat, BitmapFont font, Container parent)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			if (!string.IsNullOrWhiteSpace(stat.Details.Description))
			{
				Label val = new Label();
				val.set_Text(stat.Details.Description);
				val.set_Font(font);
				val.set_AutoSizeHeight(true);
				((Control)val).set_Parent(parent);
				Label description = val;
				if (stat.Details.Description.Length > 36)
				{
					((Control)description).set_Width(450);
					description.set_WrapText(true);
				}
				else
				{
					description.set_AutoSizeWidth(true);
				}
			}
		}
	}
}
