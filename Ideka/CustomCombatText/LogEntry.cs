using System;
using System.Collections.Generic;
using Blish_HUD.GameServices.ArcDps.V2.Models;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public class LogEntry
	{
		public CombatCallback Combat { get; init; }

		public MessageContext Context { get; init; }

		[JsonConverter(typeof(DateTimeAsUnixMillisecondsJC))]
		public DateTime Timestamp { get; init; }

		public LogEntry(CombatCallback cbt, MessageContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			Combat = cbt;
			Context = context;
			Timestamp = DateTime.UtcNow;
			base._002Ector();
		}

		public (CombatCallback cbt, IEnumerable<Message> messages) ProcessAndInterpret()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return (Combat, MessageContext.Interpret(Combat, Context));
		}
	}
}
