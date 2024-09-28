using System;
using System.IO;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Microsoft.Xna.Framework.Graphics;

namespace FarmingTracker
{
	public class TextureService : IDisposable
	{
		public const int MISSING_ASSET_ID = 0;

		public Texture2D WindowEmblemTexture { get; }

		public Texture2D DrfTexture { get; }

		public Texture2D FilterTabIconTexture { get; }

		public Texture2D SortTabIconTexture { get; }

		public Texture2D TimelineTabIconTexture { get; }

		public Texture2D SummaryTabIconTexture { get; }

		public Texture2D CustomStatProfitTabIconTexture { get; }

		public Texture2D IgnoredItemsTabIconTexture { get; }

		public Texture2D IgnoredItemsPanelIconTexture { get; }

		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public Texture2D FallbackTexture { get; }

		public Texture2D OpenLinkTexture { get; }

		public Texture2D GoldCoinTexture { get; }

		public Texture2D SilverCoinTexture { get; }

		public Texture2D CopperCoinTexture { get; }

		public AsyncTexture2D SmallGoldCoinTexture { get; }

		public AsyncTexture2D SmallSilverCoinTexture { get; }

		public AsyncTexture2D SmallCopperCoinTexture { get; }

		public AsyncTexture2D SettingsTabIconTexture { get; }

		public AsyncTexture2D DebugTabIconTexture { get; }

		public AsyncTexture2D WindowBackgroundTexture { get; }

		public AsyncTexture2D InventorySlotBackgroundTexture { get; }

		public AsyncTexture2D MerchantTexture { get; }

		public AsyncTexture2D ItemsTexture { get; }

		public AsyncTexture2D FavoriteTexture { get; }

		public AsyncTexture2D TradingPostTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			WindowEmblemTexture = contentsManager.GetTexture("window-emblem.png");
			DrfTexture = contentsManager.GetTexture("drf.png");
			FilterTabIconTexture = contentsManager.GetTexture("filter-tab-icon.png");
			SortTabIconTexture = contentsManager.GetTexture("sort-tab-icon.png");
			TimelineTabIconTexture = contentsManager.GetTexture("timeline-tab-icon.png");
			SummaryTabIconTexture = contentsManager.GetTexture("summary-tab-icon.png");
			CustomStatProfitTabIconTexture = contentsManager.GetTexture("custom-stat-profit-tab-icon.png");
			IgnoredItemsTabIconTexture = contentsManager.GetTexture("ignored-items-tab-icon.png");
			IgnoredItemsPanelIconTexture = contentsManager.GetTexture("ignored-items-panel-icon.png");
			CornerIconTexture = contentsManager.GetTexture("corner-icon.png");
			CornerIconHoverTexture = contentsManager.GetTexture("corner-icon-hover.png");
			GoldCoinTexture = contentsManager.GetTexture("coin-gold.png");
			SilverCoinTexture = contentsManager.GetTexture("coin-silver.png");
			CopperCoinTexture = contentsManager.GetTexture("coin-copper.png");
			OpenLinkTexture = contentsManager.GetTexture("open-link.png");
			FallbackTexture = contentsManager.GetTexture("fallback_157084.png");
			DebugTabIconTexture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("debug-tab-icon.png"));
			SmallGoldCoinTexture = GetTextureFromAssetCacheOrFallback(156904);
			SmallSilverCoinTexture = GetTextureFromAssetCacheOrFallback(156907);
			SmallCopperCoinTexture = GetTextureFromAssetCacheOrFallback(156902);
			SettingsTabIconTexture = GetTextureFromAssetCacheOrFallback(156737);
			WindowBackgroundTexture = GetTextureFromAssetCacheOrFallback(155997);
			InventorySlotBackgroundTexture = GetTextureFromAssetCacheOrFallback(1318622);
			MerchantTexture = GetTextureFromAssetCacheOrFallback(156761);
			ItemsTexture = GetTextureFromAssetCacheOrFallback(157098);
			FavoriteTexture = GetTextureFromAssetCacheOrFallback(156331);
			TradingPostTexture = GetTextureFromAssetCacheOrFallback(255379);
		}

		public void Dispose()
		{
			Texture2D windowEmblemTexture = WindowEmblemTexture;
			if (windowEmblemTexture != null)
			{
				((GraphicsResource)windowEmblemTexture).Dispose();
			}
			Texture2D drfTexture = DrfTexture;
			if (drfTexture != null)
			{
				((GraphicsResource)drfTexture).Dispose();
			}
			Texture2D filterTabIconTexture = FilterTabIconTexture;
			if (filterTabIconTexture != null)
			{
				((GraphicsResource)filterTabIconTexture).Dispose();
			}
			Texture2D sortTabIconTexture = SortTabIconTexture;
			if (sortTabIconTexture != null)
			{
				((GraphicsResource)sortTabIconTexture).Dispose();
			}
			Texture2D timelineTabIconTexture = TimelineTabIconTexture;
			if (timelineTabIconTexture != null)
			{
				((GraphicsResource)timelineTabIconTexture).Dispose();
			}
			Texture2D summaryTabIconTexture = SummaryTabIconTexture;
			if (summaryTabIconTexture != null)
			{
				((GraphicsResource)summaryTabIconTexture).Dispose();
			}
			Texture2D customStatProfitTabIconTexture = CustomStatProfitTabIconTexture;
			if (customStatProfitTabIconTexture != null)
			{
				((GraphicsResource)customStatProfitTabIconTexture).Dispose();
			}
			Texture2D ignoredItemsTabIconTexture = IgnoredItemsTabIconTexture;
			if (ignoredItemsTabIconTexture != null)
			{
				((GraphicsResource)ignoredItemsTabIconTexture).Dispose();
			}
			Texture2D ignoredItemsPanelIconTexture = IgnoredItemsPanelIconTexture;
			if (ignoredItemsPanelIconTexture != null)
			{
				((GraphicsResource)ignoredItemsPanelIconTexture).Dispose();
			}
			Texture2D cornerIconTexture = CornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			Texture2D cornerIconHoverTexture = CornerIconHoverTexture;
			if (cornerIconHoverTexture != null)
			{
				((GraphicsResource)cornerIconHoverTexture).Dispose();
			}
			Texture2D goldCoinTexture = GoldCoinTexture;
			if (goldCoinTexture != null)
			{
				((GraphicsResource)goldCoinTexture).Dispose();
			}
			Texture2D silverCoinTexture = SilverCoinTexture;
			if (silverCoinTexture != null)
			{
				((GraphicsResource)silverCoinTexture).Dispose();
			}
			Texture2D copperCoinTexture = CopperCoinTexture;
			if (copperCoinTexture != null)
			{
				((GraphicsResource)copperCoinTexture).Dispose();
			}
			Texture2D openLinkTexture = OpenLinkTexture;
			if (openLinkTexture != null)
			{
				((GraphicsResource)openLinkTexture).Dispose();
			}
			Texture2D fallbackTexture = FallbackTexture;
			if (fallbackTexture != null)
			{
				((GraphicsResource)fallbackTexture).Dispose();
			}
			AsyncTexture2D debugTabIconTexture = DebugTabIconTexture;
			if (debugTabIconTexture != null)
			{
				debugTabIconTexture.Dispose();
			}
		}

		public static int GetIconAssetId(RenderUrl icon)
		{
			return int.Parse(Path.GetFileNameWithoutExtension(((RenderUrl)(ref icon)).get_Url().AbsoluteUri));
		}

		public AsyncTexture2D GetTextureFromAssetCacheOrFallback(int assetId)
		{
			if (assetId == 0)
			{
				return AsyncTexture2D.op_Implicit(FallbackTexture);
			}
			try
			{
				AsyncTexture2D texture = default(AsyncTexture2D);
				if (AsyncTexture2D.TryFromAssetId(assetId, ref texture))
				{
					return texture;
				}
			}
			catch (Exception)
			{
				return AsyncTexture2D.op_Implicit(FallbackTexture);
			}
			return AsyncTexture2D.op_Implicit(FallbackTexture);
		}
	}
}
