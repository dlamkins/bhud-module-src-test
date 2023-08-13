using System;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public sealed class OriginalUce : Token
	{
		[JsonProperty("at_date")]
		public DateTime AtDate { get; set; }

		public OriginalUce()
		{
			base.Id = 81743;
			base.Name = ProofLogix.Instance.Resources.GetItem(base.Id).Name;
		}
	}
}
