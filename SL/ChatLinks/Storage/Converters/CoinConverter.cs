using System;
using System.Linq.Expressions;
using GuildWars2;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SL.ChatLinks.Storage.Converters
{
	public class CoinConverter : ValueConverter<Coin, int>
	{
		public CoinConverter()
			: base((Expression<Func<Coin, int>>)((Coin coin) => coin.Amount), (Expression<Func<int, Coin>>)((int value) => new Coin(value)), (ConverterMappingHints)null)
		{
		}
	}
}
