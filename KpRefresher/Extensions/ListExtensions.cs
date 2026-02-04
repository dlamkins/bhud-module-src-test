using System.Collections.Generic;
using System.Linq;
using KpRefresher.Domain;
using KpRefresher.Domain.Attributes;

namespace KpRefresher.Extensions
{
	public static class ListExtensions
	{
		public static List<(Token, int)> SortByEncounter(this List<(Token, int)> data)
		{
			return (from t in data
				orderby t.Item1.GetAttribute<SortAttribute>().CategoryOrder, t.Item1.GetAttribute<SortAttribute>().EncounterOrder
				select t).ToList();
		}
	}
}
