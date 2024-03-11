using System;
using System.Text;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;

namespace Ideka.CustomCombatText.Bridge
{
	public static class CombatParser
	{
		[Flags]
		private enum CombatMessageFlags
		{
			Ev = 0x1,
			Src = 0x2,
			Dst = 0x4,
			SkillName = 0x8
		}

		public static CombatEvent ProcessCombat(byte[] data)
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			CombatMessageFlags flags = (CombatMessageFlags)data[1];
			int offset = 2;
			Ev ev = null;
			if (flags.HasFlag(CombatMessageFlags.Ev))
			{
				(ev, offset) = ParseEv(data, offset);
			}
			Ag src = null;
			if (flags.HasFlag(CombatMessageFlags.Src))
			{
				(src, offset) = ParseAg(data, offset);
			}
			Ag dst = null;
			if (flags.HasFlag(CombatMessageFlags.Dst))
			{
				(dst, offset) = ParseAg(data, offset);
			}
			string skillName = null;
			if (flags.HasFlag(CombatMessageFlags.SkillName))
			{
				(skillName, offset) = ParseString(data, offset);
			}
			(ulong, int) tuple5 = U64(data, offset);
			ulong id = tuple5.Item1;
			offset = tuple5.Item2;
			ulong revision = U64(data, offset).Item1;
			return new CombatEvent(ev, src, dst, skillName, id, revision);
		}

		private static (Ev, int) ParseEv(byte[] data, int offset)
		{
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			ulong time;
			(time, offset) = U64(data, offset);
			ulong srcAgent;
			(srcAgent, offset) = U64(data, offset);
			ulong dstAgent;
			(dstAgent, offset) = U64(data, offset);
			int value;
			(value, offset) = I32(data, offset);
			int buffDmg;
			(buffDmg, offset) = I32(data, offset);
			uint overStackValue;
			(overStackValue, offset) = U32(data, offset);
			uint skillId;
			(skillId, offset) = U32(data, offset);
			ushort srcInstId;
			(srcInstId, offset) = U16(data, offset);
			ushort dstInstId;
			(dstInstId, offset) = U16(data, offset);
			ushort srcMasterInstId;
			(srcMasterInstId, offset) = U16(data, offset);
			ushort dstMasterInstId;
			(dstMasterInstId, offset) = U16(data, offset);
			byte iff;
			(iff, offset) = U8(data, offset);
			bool buff;
			(buff, offset) = B(data, offset);
			byte result;
			(result, offset) = U8(data, offset);
			byte isActivation;
			(isActivation, offset) = U8(data, offset);
			byte isBuffRemove;
			(isBuffRemove, offset) = U8(data, offset);
			bool isNinety;
			(isNinety, offset) = B(data, offset);
			bool isFifty;
			(isFifty, offset) = B(data, offset);
			bool isMoving;
			(isMoving, offset) = B(data, offset);
			byte isStateChange;
			(isStateChange, offset) = U8(data, offset);
			bool isFlanking;
			(isFlanking, offset) = B(data, offset);
			bool isShields;
			(isShields, offset) = B(data, offset);
			bool isOffCycle;
			(isOffCycle, offset) = B(data, offset);
			byte pad61;
			(pad61, offset) = U8(data, offset);
			byte pad62;
			(pad62, offset) = U8(data, offset);
			byte pad63;
			(pad63, offset) = U8(data, offset);
			byte pad64;
			(pad64, offset) = U8(data, offset);
			return (new Ev(time, srcAgent, dstAgent, value, buffDmg, overStackValue, skillId, srcInstId, dstInstId, srcMasterInstId, dstMasterInstId, (IFF)iff, buff, result, (Activation)isActivation, (BuffRemove)isBuffRemove, isNinety, isFifty, isMoving, (StateChange)isStateChange, isFlanking, isShields, isOffCycle, pad61, pad62, pad63, pad64), offset);
		}

		private static (Ag, int) ParseAg(byte[] data, int offset)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			string name;
			(name, offset) = ParseString(data, offset);
			ulong id;
			(id, offset) = U64(data, offset);
			uint profession;
			(profession, offset) = U32(data, offset);
			uint elite;
			(elite, offset) = U32(data, offset);
			uint self;
			(self, offset) = U32(data, offset);
			ushort team;
			(team, offset) = U16(data, offset);
			return (new Ag(name, id, profession, elite, self, team), offset);
		}

		private static (string, int) ParseString(byte[] data, int offset)
		{
			ulong length;
			(length, offset) = U64(data, offset);
			return (Encoding.UTF8.GetString(data, offset, (int)length), offset + (int)length);
		}

		private static (ulong, int) U64(byte[] data, int offset)
		{
			return (BitConverter.ToUInt64(data, offset), offset + 8);
		}

		private static (uint, int) U32(byte[] data, int offset)
		{
			return (BitConverter.ToUInt32(data, offset), offset + 4);
		}

		private static (int, int) I32(byte[] data, int offset)
		{
			return (BitConverter.ToInt32(data, offset), offset + 4);
		}

		private static (ushort, int) U16(byte[] data, int offset)
		{
			return (BitConverter.ToUInt16(data, offset), offset + 2);
		}

		private static (byte, int) U8(byte[] data, int offset)
		{
			return (data[offset], offset + 1);
		}

		private static (bool, int) B(byte[] data, int offset)
		{
			return (data[offset] != 0, offset + 1);
		}
	}
}
