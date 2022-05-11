using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;

namespace Nekres.Stream_Out
{
	internal static class Gw2Util
	{
		private static readonly Color Gold = Color.FromArgb(210, 180, 66);

		private static readonly Color Silver = Color.FromArgb(153, 153, 153);

		private static readonly Color Copper = Color.FromArgb(190, 100, 35);

		private static readonly Color Karma = Color.FromArgb(220, 80, 190);

		public static async Task GenerateCoinsImage(string filePath, int coins, bool overwrite = true)
		{
			if (!overwrite && File.Exists(filePath))
			{
				return;
			}
			int copper = coins % 100;
			coins = (coins - copper) / 100;
			int silver = coins % 100;
			int gold = (coins - silver) / 100;
			int toDisplay = ((gold > 0) ? 3 : ((silver <= 0) ? 1 : 2));
			Font font = new Font("Arial", 12f);
			Size copperSize = copper.ToString().Measure(font);
			Size silverSize = silver.ToString().Measure(font);
			Size goldSize = gold.ToString().Measure(font);
			int fontHeight = Math.Max(Math.Max(silverSize.Height, goldSize.Height), copperSize.Height);
			Stream copperIconStream = StreamOutModule.Instance.ContentsManager.GetFileStream("copper_coin.png");
			Bitmap copperIcon = new Bitmap(copperIconStream).FitToHeight(fontHeight - 5);
			Stream silverIconStream = StreamOutModule.Instance.ContentsManager.GetFileStream("silver_coin.png");
			Bitmap silverIcon = new Bitmap(silverIconStream).FitToHeight(fontHeight - 5);
			Stream goldIconStream = StreamOutModule.Instance.ContentsManager.GetFileStream("gold_coin.png");
			Bitmap goldIcon = new Bitmap(goldIconStream).FitToHeight(fontHeight - 5);
			int margin = 5;
			int width = copperSize.Width + copperIcon.Width;
			if (toDisplay > 1)
			{
				width += margin + silverSize.Width + silverIcon.Width;
			}
			if (toDisplay > 2)
			{
				width += margin + goldSize.Width + goldIcon.Width;
			}
			int height = Math.Max(fontHeight, Math.Max(Math.Max(silverIcon.Height, goldIcon.Height), copperIcon.Height));
			using (Bitmap bitmap = new Bitmap(width, height))
			{
				using (Graphics canvas = Graphics.FromImage(bitmap))
				{
					canvas.SetHighestQuality();
					int x = 0;
					for (int toDraw = toDisplay; toDraw > 0; toDraw--)
					{
						Color color;
						Size size;
						int value;
						Bitmap icon;
						switch (toDraw)
						{
						case 3:
							color = Gold;
							size = goldSize;
							value = gold;
							icon = goldIcon;
							break;
						case 2:
							color = Silver;
							size = silverSize;
							value = silver;
							icon = silverIcon;
							break;
						default:
							color = Copper;
							size = copperSize;
							value = copper;
							icon = copperIcon;
							break;
						}
						using (SolidBrush brush = new SolidBrush(color))
						{
							canvas.DrawString(value.ToString(), font, brush, x, height / 2 - size.Height / 2);
						}
						x += toDraw switch
						{
							2 => silverSize.Width, 
							3 => goldSize.Width, 
							_ => copperSize.Width, 
						};
						canvas.DrawImage(icon, new Rectangle(x, height / 2 - icon.Height / 2, icon.Width, icon.Width), new Rectangle(0, 0, icon.Width, icon.Width), GraphicsUnit.Pixel);
						x += toDraw switch
						{
							2 => silverIcon.Width, 
							3 => goldIcon.Width, 
							_ => copperIcon.Width, 
						} + margin;
					}
					canvas.Flush();
					canvas.Save();
				}
				await bitmap.SaveOnNetworkShare(filePath, ImageFormat.Png);
			}
			copperIcon.Dispose();
			copperIconStream.Close();
			silverIcon.Dispose();
			silverIconStream.Close();
			goldIcon.Dispose();
			goldIconStream.Close();
			font.Dispose();
		}

		public static async Task GenerateKarmaImage(string filePath, int karma, bool overwrite = true)
		{
			if (!overwrite && File.Exists(filePath))
			{
				return;
			}
			Font font = new Font("Arial", 12f);
			string karmaStr = karma.ToString("N0", GameService.Overlay.CultureInfo());
			Size karmaSize = karmaStr.Measure(font);
			Stream karmaIconStream = StreamOutModule.Instance.ContentsManager.GetFileStream("karma.png");
			Bitmap karmaIcon = new Bitmap(karmaIconStream).FitToHeight(karmaSize.Height);
			int height = Math.Max(karmaSize.Height, karmaIcon.Height);
			using (Bitmap bitmap = new Bitmap(karmaSize.Width + karmaIcon.Width, height))
			{
				using (Graphics canvas = Graphics.FromImage(bitmap))
				{
					canvas.SetHighestQuality();
					using (SolidBrush karmaBrush = new SolidBrush(Karma))
					{
						canvas.DrawString(karmaStr, font, karmaBrush, 0f, karmaSize.Height / 2 - karmaIcon.Height / 2);
					}
					canvas.DrawImage(karmaIcon, new Rectangle(karmaSize.Width, height / 2 - karmaIcon.Height / 2, karmaIcon.Width, karmaIcon.Width), new Rectangle(0, 0, karmaIcon.Width, karmaIcon.Width), GraphicsUnit.Pixel);
					canvas.Flush();
					canvas.Save();
				}
				await bitmap.SaveOnNetworkShare(filePath, ImageFormat.Png);
			}
			karmaIcon.Dispose();
			karmaIconStream.Close();
			font.Dispose();
		}

		public static async Task GeneratePvpTierImage(string filePath, int tier, int maxTiers, bool overwrite = true)
		{
			if (!overwrite && File.Exists(filePath))
			{
				return;
			}
			Stream tierIconFilledStream = StreamOutModule.Instance.ContentsManager.GetFileStream("1495585.png");
			Bitmap tierIconFilled = new Bitmap(tierIconFilledStream);
			Stream tierIconEmptyStream = StreamOutModule.Instance.ContentsManager.GetFileStream("1495584.png");
			Bitmap tierIconEmpty = new Bitmap(tierIconEmptyStream);
			int width = maxTiers * (Math.Max(tierIconFilled.Width, tierIconEmpty.Width) + 2);
			int height = Math.Max(tierIconFilled.Height, tierIconEmpty.Height);
			using (Bitmap bitmap = new Bitmap(width, height))
			{
				using (Graphics canvas = Graphics.FromImage(bitmap))
				{
					canvas.SetHighestQuality();
					for (int drawn = 0; drawn < maxTiers; drawn++)
					{
						int margin = ((drawn > 0) ? (drawn * 2) : 0);
						Bitmap tierIcon = ((drawn < tier) ? tierIconFilled : tierIconEmpty);
						canvas.DrawImage(tierIcon, new Rectangle(drawn * tierIcon.Width + margin, height / 2 - tierIcon.Height / 2, tierIcon.Width, tierIcon.Width), new Rectangle(0, 0, tierIcon.Width, tierIcon.Width), GraphicsUnit.Pixel);
					}
					canvas.Flush();
					canvas.Save();
				}
				await bitmap.SaveOnNetworkShare(filePath, ImageFormat.Png);
			}
			tierIconFilled.Dispose();
			tierIconFilledStream.Close();
			tierIconEmpty.Dispose();
			tierIconEmptyStream.Close();
		}

		public static DateTime GetDailyResetTime()
		{
			DateTime nextDay = DateTime.UtcNow.AddDays(1.0);
			return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 2, 0, 0).ToUniversalTime();
		}
	}
}
