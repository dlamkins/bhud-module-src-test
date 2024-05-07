using System;
using System.Collections.Generic;
using Blish_HUD.ArcDps.Models;
using Ideka.CustomCombatText.Bridge;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public class LogEntry
	{
		public byte[] Data { get; init; }

		public MessageContext Context { get; init; }

		[JsonConverter(typeof(DateTimeAsUnixMillisecondsJC))]
		public DateTime Timestamp { get; init; }

		public LogEntry(byte[] data, MessageContext context)
		{
			Data = data;
			Context = context;
			Timestamp = DateTime.UtcNow;
			base._002Ector();
		}

		public (CombatEvent cbt, IEnumerable<Message> messages) ProcessAndInterpret()
		{
			CombatEvent obj = CombatParser.ProcessCombat(Data, 1);
			return (obj, MessageContext.Interpret(obj, Context));
		}
	}
}
