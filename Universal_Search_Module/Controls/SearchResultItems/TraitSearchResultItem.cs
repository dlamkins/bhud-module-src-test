using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;

namespace Universal_Search_Module.Controls.SearchResultItems
{
	public class TraitSearchResultItem : SearchResultItem
	{
		private Trait _trait;

		public Trait Trait
		{
			get
			{
				return _trait;
			}
			set
			{
				if (((Control)this).SetProperty<Trait>(ref _trait, value, false, "Trait") && _trait != null)
				{
					_ = _trait.Icon;
					base.Icon = Control.get_Content().GetRenderServiceTexture((string)_trait.Icon);
					base.Name = _trait.Name;
					base.Description = StringUtil.SanitizeTraitDescription(_trait.Description);
				}
			}
		}

		protected override string ChatLink => GenerateChatLink(Trait);

		protected override Tooltip BuildTooltip()
		{
			return (Tooltip)(object)new TraitTooltip(Trait);
		}

		private static string GenerateChatLink(Trait trait)
		{
			List<byte> result = new List<byte> { 7 };
			result.AddRange(BitConverter.GetBytes(trait.Id));
			return "[&" + Convert.ToBase64String(result.ToArray()) + "]";
		}
	}
}
