using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal_Search_Module.Controls.SearchResultItems;

namespace Universal_Search_Module.Services.SearchHandler
{
	public abstract class SearchHandler<T> : SearchHandler
	{
		protected abstract HashSet<T> SearchItems { get; }

		protected abstract string GetSearchableProperty(T item);

		protected abstract SearchResultItem CreateSearchResultItem(T item);

		public override IEnumerable<SearchResultItem> Search(string searchText)
		{
			List<WordScoreResult<T>> diffs = new List<WordScoreResult<T>>();
			foreach (T item in SearchItems)
			{
				string name = GetSearchableProperty(item);
				int score = ((!name.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase)) ? ((!name.ToUpper().Contains(searchText.ToUpper())) ? StringUtil.ComputeLevenshteinDistance(searchText.ToLower(), name.Substring(0, Math.Min(searchText.Length, name.Length)).ToLower()) : 3) : 0);
				diffs.Add(new WordScoreResult<T>(item, score));
			}
			return from x in (from x in diffs
					orderby x.DiffScore, GetSearchableProperty(x.Result).Length
					select x).Take(3)
				select CreateSearchResultItem(x.Result);
		}
	}
	public abstract class SearchHandler
	{
		public const int MAX_RESULT_COUNT = 3;

		public abstract string Name { get; }

		public abstract string Prefix { get; }

		public abstract Task Initialize(Action<string> progress);

		public abstract IEnumerable<SearchResultItem> Search(string searchText);
	}
}
