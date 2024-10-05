using Gw2Sharp.ChatLinks;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class ChatLinkExtension
	{
		public static string CreateChatLink(this Trait trait)
		{
			TraitChatLink obj = new TraitChatLink
			{
				TraitId = trait.Id
			};
			byte[] bytes = obj.ToArray();
			obj.Parse(bytes);
			return obj.ToString();
		}
	}
}
