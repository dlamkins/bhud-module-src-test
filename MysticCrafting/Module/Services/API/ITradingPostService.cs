using MysticCrafting.Models.TradingPost;

namespace MysticCrafting.Module.Services.API
{
	public interface ITradingPostService : IApiService
	{
		TradingPostItemPrices GetItemPrices(int itemId);
	}
}
