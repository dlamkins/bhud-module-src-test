using System.Collections.Generic;
using MysticCrafting.Models.Commerce;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingCurrencyRepository : LoggingRepository, ICurrencyRepository, IRepository
	{
		private readonly ICurrencyRepository _currencyRepository;

		public LoggingCurrencyRepository(ICurrencyRepository currencyRepository)
			: base(currencyRepository)
		{
			_currencyRepository = currencyRepository;
		}

		public MysticCurrency GetCoinCurrency()
		{
			return _currencyRepository.GetCoinCurrency();
		}

		public IList<MysticCurrency> GetCurrencies()
		{
			return _currencyRepository.GetCurrencies();
		}

		public MysticCurrency GetCurrency(int id)
		{
			return _currencyRepository.GetCurrency(id);
		}

		public MysticCurrency GetCurrency(string name)
		{
			return _currencyRepository.GetCurrency(name);
		}
	}
}
