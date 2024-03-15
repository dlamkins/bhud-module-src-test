using System;
using System.IO;
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
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			using MemoryStream stream = new MemoryStream(data);
			using BinaryReader reader = new BinaryReader(stream);
			reader.ReadByte();
			CombatMessageFlags flags = (CombatMessageFlags)reader.ReadByte();
			Ev ev = null;
			if (flags.HasFlag(CombatMessageFlags.Ev))
			{
				ev = ReadEv(reader);
			}
			Ag src = null;
			if (flags.HasFlag(CombatMessageFlags.Src))
			{
				src = ReadAg(reader);
			}
			Ag dst = null;
			if (flags.HasFlag(CombatMessageFlags.Dst))
			{
				dst = ReadAg(reader);
			}
			string skillName = null;
			if (flags.HasFlag(CombatMessageFlags.SkillName))
			{
				skillName = ReadString(reader);
			}
			ulong id = reader.ReadUInt64();
			ulong revision = reader.ReadUInt64();
			return new CombatEvent(ev, src, dst, skillName, id, revision);
		}

		private static Ev ReadEv(BinaryReader reader)
		{
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			ulong num = reader.ReadUInt64();
			ulong srcAgent = reader.ReadUInt64();
			ulong dstAgent = reader.ReadUInt64();
			int value = reader.ReadInt32();
			int buffDmg = reader.ReadInt32();
			uint overStackValue = reader.ReadUInt32();
			uint skillId = reader.ReadUInt32();
			ushort srcInstId = reader.ReadUInt16();
			ushort dstInstId = reader.ReadUInt16();
			ushort srcMasterInstId = reader.ReadUInt16();
			ushort dstMasterInstId = reader.ReadUInt16();
			byte iff = reader.ReadByte();
			bool buff = reader.ReadByte() == 1;
			byte result = reader.ReadByte();
			byte isActivation = reader.ReadByte();
			byte isBuffRemove = reader.ReadByte();
			bool isNinety = reader.ReadByte() == 1;
			bool isFifty = reader.ReadByte() == 1;
			bool isMoving = reader.ReadByte() == 1;
			byte isStateChange = reader.ReadByte();
			bool isFlanking = reader.ReadByte() == 1;
			bool isShields = reader.ReadByte() == 1;
			bool isOffCycle = reader.ReadByte() == 1;
			byte pad61 = reader.ReadByte();
			byte pad62 = reader.ReadByte();
			byte pad63 = reader.ReadByte();
			byte pad64 = reader.ReadByte();
			return new Ev(num, srcAgent, dstAgent, value, buffDmg, overStackValue, skillId, srcInstId, dstInstId, srcMasterInstId, dstMasterInstId, (IFF)iff, buff, result, (Activation)isActivation, (BuffRemove)isBuffRemove, isNinety, isFifty, isMoving, (StateChange)isStateChange, isFlanking, isShields, isOffCycle, pad61, pad62, pad63, pad64);
		}

		private static Ag ReadAg(BinaryReader reader)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			string text = ReadString(reader);
			ulong id = reader.ReadUInt64();
			uint profession = reader.ReadUInt32();
			uint elite = reader.ReadUInt32();
			uint self = reader.ReadUInt32();
			ushort team = reader.ReadUInt16();
			return new Ag(text, id, profession, elite, self, team);
		}

		private static string ReadString(BinaryReader reader)
		{
			ulong length = reader.ReadUInt64();
			return Encoding.UTF8.GetString(reader.ReadBytes((int)length));
		}
	}
}
