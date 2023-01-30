using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableCoinEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableCoinEntry>
	{
		private readonly Gw2ApiManager gw2ApiManager;

		private readonly AsyncTexture2D copperTexture;

		private readonly AsyncTexture2D silverTexture;

		private readonly AsyncTexture2D goldTexture;

		public AchievementTableCoinEntryFactory(Gw2ApiManager gw2ApiManager, ContentsManager contentsManager)
		{
			this.gw2ApiManager = gw2ApiManager;
			copperTexture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("Copper_coin.png"));
			silverTexture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("Silver_coin.png"));
			goldTexture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("Gold_coin.png"));
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableCoinEntry entry)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel outerPanel = val;
			Label val2 = new Label();
			val2.set_Text((entry?.ItemId.ToString() ?? string.Empty) + ": " + (entry?.Type.ToString() ?? string.Empty));
			val2.set_AutoSizeHeight(true);
			val2.set_WrapText(true);
			Task.Run(async delegate
			{
				CommercePrices price = await ((IBulkExpandableClient<CommercePrices, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Commerce()
					.get_Prices()).GetAsync(entry.ItemId, default(CancellationToken));
				(int, int, int) sellPrice = ConvertIntoCoinParts(price.get_Sells().get_UnitPrice());
				(int, int, int) buyPrice = ConvertIntoCoinParts(price.get_Buys().get_UnitPrice());
				FormattedLabelBuilder formattedLabel = new FormattedLabelBuilder();
				if (entry.Type == CollectionAchievementTable.CollectionAchievementTableCoinEntry.TradingPostType.Sell)
				{
					formattedLabel.CreatePart($"{sellPrice.Item1}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(goldTexture).SetSuffixImageSize(new Point(15, 15));
					}).CreatePart($" {sellPrice.Item2}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(silverTexture).SetSuffixImageSize(new Point(15, 15));
					}).CreatePart($" {sellPrice.Item3}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(copperTexture).SetSuffixImageSize(new Point(15, 15));
					});
				}
				else
				{
					formattedLabel.CreatePart($"{buyPrice.Item1}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(goldTexture).SetSuffixImageSize(new Point(15, 15));
					}).CreatePart($" {buyPrice.Item2}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(silverTexture).SetSuffixImageSize(new Point(15, 15));
					}).CreatePart($" {buyPrice.Item3}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
					{
						//IL_0015: Unknown result type (might be due to invalid IL or missing references)
						x.SetSuffixImage(copperTexture).SetSuffixImageSize(new Point(15, 15));
					});
				}
				formattedLabel.AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)1);
				FormattedLabel label = formattedLabel.Build();
				((Control)outerPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate(object s, ResizedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)label).set_Width(e.get_CurrentSize().X);
				});
				((Control)label).set_Parent((Container)(object)outerPanel);
				((Control)label).set_Width(((Control)outerPanel).get_Width());
			});
			return (Control)(object)outerPanel;
		}

		private (int Gold, int Silver, int Copper) ConvertIntoCoinParts(int copper)
		{
			int copperCoins = copper % 100;
			int silver = copper / 100 % 100;
			return (copper / 100 / 100, silver, copperCoins);
		}
	}
}
