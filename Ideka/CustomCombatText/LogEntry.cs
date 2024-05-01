using System;
using System.Collections.Generic;
using Blish_HUD.ArcDps.Models;
using Ideka.CustomCombatText.Bridge;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public readonly struct LogEntry
	{
		public byte[] Data { get; }

		public MessageContext Context { get; }

		[JsonConverter(typeof(DateTimeAsUnixMillisecondsJC))]
		public DateTime Timestamp { get; }

		public LogEntry(byte[] data, MessageContext context)
		{
			Data = data;
			Context = context;
			Timestamp = DateTime.UtcNow;
		}

		public (CombatEvent cbt, IEnumerable<Message> messages) ProcessAndInterpret()
		{
			CombatEvent obj = CombatParser.ProcessCombat(Data, 1);
			return (obj, Message.Interpret(obj, Context));
		}
	}
}
