using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;
using Universal_Search_Module.Controls.SearchResultItems;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Services.SearchHandler
{
	public class SkillSearchHandler : SearchHandler<Skill>
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly HashSet<Skill> _skills = new HashSet<Skill>();

		public override string Name => Common.SearchHandler_Skills;

		public override string Prefix => "s";

		protected override HashSet<Skill> SearchItems => _skills;

		public SkillSearchHandler(Gw2ApiManager gw2ApiManager)
		{
			_gw2ApiManager = gw2ApiManager;
		}

		public override async Task Initialize(Action<string> progress)
		{
			progress(Common.SearchHandler_Skills_SkillLoading);
			HashSet<Skill> skills = _skills;
			skills.UnionWith(await _gw2ApiManager.get_Gw2ApiClient().V2.Skills.AllAsync());
		}

		protected override SearchResultItem CreateSearchResultItem(Skill item)
		{
			return new SkillSearchResultItem
			{
				Skill = item
			};
		}

		protected override string GetSearchableProperty(Skill item)
		{
			return item.Name;
		}
	}
}
