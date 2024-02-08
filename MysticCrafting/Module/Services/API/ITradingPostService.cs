using MysticCrafting.Models.TradingPost;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services.API
{
	public interface ITradingPostService : IRecurringService
	{
		TradingPostItemPrices GetItemPrices(int itemId);
	}
}
