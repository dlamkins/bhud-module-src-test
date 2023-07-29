using System.Collections.Generic;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	[JsonConverter(typeof(ClearConverter))]
	public sealed class Clear : BaseResponse
	{
		public string Name { get; set; }

		public List<Boss> Encounters { get; set; }
	}
}
