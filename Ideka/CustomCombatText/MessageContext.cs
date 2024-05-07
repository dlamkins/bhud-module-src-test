using System;
using System.Collections.Generic;
using Blish_HUD.ArcDps.Models;

namespace Ideka.CustomCombatText
{
	public class MessageContext
	{
		private static readonly MessageContext Default = new MessageContext();

		public ulong SelfId { get; set; }

		public ushort SelfInstId { get; set; }

		public ulong TargetId { get; set; } = ulong.MaxValue;


		public ushort TargetInstId { get; set; } = ushort.MaxValue;


		public static LogEntry Log(byte[] raw)
		{
			return new LogEntry(raw, (MessageContext)Default.MemberwiseClone());
		}

		public static IEnumerable<Message> Interpret(CombatEvent cbt, MessageContext? context = null)
		{
			MessageContext ctx = context ?? Default;
			if (cbt.get_Ev() == null)
			{
				Ag src2 = cbt.get_Src();
				if (src2 != null && src2.get_Elite() == 1)
				{
					ctx.TargetId = cbt.get_Src().get_Id();
				}
			}
			Ev ev = cbt.get_Ev();
			if (ev == null)
			{
				yield break;
			}
			Ag src3 = cbt.get_Src();
			if (src3 != null && src3.get_Self() == 1)
			{
				ctx.SelfId = cbt.get_Src().get_Id();
				ctx.SelfInstId = ev.get_SrcInstId();
			}
			Ag dst2 = cbt.get_Dst();
			if (dst2 != null && dst2.get_Self() == 1)
			{
				ctx.SelfId = cbt.get_Dst().get_Id();
				ctx.SelfInstId = ev.get_DstInstId();
			}
			Ag src4 = cbt.get_Src();
			if (((src4 != null) ? new ulong?(src4.get_Id()) : null) == ctx.TargetId)
			{
				ctx.TargetInstId = ev.get_SrcInstId();
			}
			Ag dst3 = cbt.get_Dst();
			if (((dst3 != null) ? new ulong?(dst3.get_Id()) : null) == ctx.TargetId)
			{
				ctx.TargetInstId = ev.get_DstInstId();
			}
			Ag dst = cbt.get_Dst();
			if (dst == null)
			{
				yield break;
			}
			Ag src = cbt.get_Src();
			if (src == null || (int)ev.get_IsStateChange() != 0 || (int)ev.get_IsActivation() != 0 || (int)ev.get_IsBuffRemove() != 0)
			{
				yield break;
			}
			HashSet<EventResult> results = new HashSet<EventResult>();
			int barrier = (int)ev.get_OverStackValue();
			int value;
			if (ev.get_Buff())
			{
				value = ev.get_BuffDmg();
				if (value > 0)
				{
					if (barrier > 0)
					{
						value -= barrier;
						results.Add(EventResult.Barrier);
					}
					if (value > 0)
					{
						results.Add(EventResult.HealTick);
					}
				}
				else if (value < 0)
				{
					if (barrier < 0)
					{
						value -= barrier;
					}
					if (barrier <= 0 || value < 0)
					{
						switch (ev.get_SkillId())
						{
						case 736u:
							results.Add(EventResult.Bleeding);
							break;
						case 737u:
							results.Add(EventResult.Burning);
							break;
						case 723u:
							results.Add(EventResult.Poison);
							break;
						case 861u:
							results.Add(EventResult.Confusion);
							break;
						case 19426u:
							results.Add(EventResult.Torment);
							break;
						default:
							results.Add(EventResult.DamageTick);
							break;
						}
					}
				}
			}
			else
			{
				value = ev.get_Value();
				if (ev.get_Result() == 10)
				{
					if (ev.get_SkillId() != 23276)
					{
						results.Add(EventResult.Breakbar);
					}
					value /= 10;
				}
				else if (value > 0)
				{
					if (barrier > 0)
					{
						value -= barrier;
						results.Add(EventResult.Barrier);
					}
					if (value > 0)
					{
						results.Add(EventResult.Heal);
					}
				}
				else
				{
					if (barrier < 0)
					{
						value -= barrier;
					}
					if (barrier <= 0 || value < 0)
					{
						switch (ev.get_Result())
						{
						case 0:
							results.Add(EventResult.Strike);
							break;
						case 1:
							results.Add(EventResult.Crit);
							break;
						case 2:
							results.Add(EventResult.Glance);
							break;
						case 3:
							results.Add(EventResult.Block);
							break;
						case 4:
							results.Add(EventResult.Evade);
							break;
						case 6:
							results.Add(EventResult.Invuln);
							break;
						case 7:
							results.Add(EventResult.Miss);
							break;
						case 5:
							results.Add(EventResult.Interrupt);
							break;
						}
					}
				}
			}
			HashSet<MessageCategory> categories = new HashSet<MessageCategory>();
			if (src.get_Self() == 1)
			{
				categories.Add(MessageCategory.PlayerOut);
			}
			else if (ev.get_SrcMasterInstId() == ctx.SelfInstId)
			{
				categories.Add(MessageCategory.PetOut);
			}
			if (dst.get_Self() == 1)
			{
				categories.Add(MessageCategory.PlayerIn);
			}
			else if (ev.get_DstMasterInstId() == ctx.SelfInstId)
			{
				categories.Add(MessageCategory.PetIn);
			}
			foreach (EventResult result in results)
			{
				int effectiveValue = ((result == EventResult.Barrier) ? barrier : value);
				int effectiveBarrier = ((result != EventResult.Barrier) ? barrier : 0);
				foreach (MessageCategory category in categories)
				{
					yield return new Message(cbt, ctx)
					{
						Category = category,
						Result = result,
						Value = Math.Abs(effectiveValue),
						Barrier = Math.Abs(effectiveBarrier)
					};
				}
			}
		}
	}
}
