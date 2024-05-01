using System;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public class MessageContext
	{
		public ulong SelfId { get; set; }

		public ushort SelfInstId { get; set; }

		public ulong TargetId { get; set; } = ulong.MaxValue;


		public ushort TargetInstId { get; set; } = ushort.MaxValue;


		public MessageContext Clone()
		{
			return JsonConvert.DeserializeObject<MessageContext>(JsonConvert.SerializeObject(this)) ?? throw new Exception();
		}
	}
}
