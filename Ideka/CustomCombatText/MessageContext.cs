using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD.GameServices.ArcDps.V2.Models;

namespace Ideka.CustomCombatText
{
	public class MessageContext
	{
		private static readonly MessageContext Default = new MessageContext();

		public ulong SelfId { get; set; }

		public ushort SelfInstId { get; set; }

		public ulong TargetId { get; set; } = ulong.MaxValue;


		public ushort TargetInstId { get; set; } = ushort.MaxValue;


		public static LogEntry Log(CombatCallback cbt)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new LogEntry(cbt, (MessageContext)Default.MemberwiseClone());
		}

		[IteratorStateMachine(typeof(_003CInterpret_003Ed__18))]
		public static IEnumerable<Message> Interpret(CombatCallback cbt, MessageContext? context = null)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return new _003CInterpret_003Ed__18(-2)
			{
				_003C_003E3__cbt = cbt,
				_003C_003E3__context = context
			};
		}
	}
}
