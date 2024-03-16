using System;
using System.Runtime.CompilerServices;
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

		public static CombatEvent ProcessCombat(byte[] data, int offset)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			byte data2 = ReadUInt8(data, ref offset);
			Ev ev = (HasFlag((CombatMessageFlags)data2, CombatMessageFlags.Ev) ? ReadEv(data, ref offset) : null);
			Ag src = (HasFlag((CombatMessageFlags)data2, CombatMessageFlags.Src) ? ReadAg(data, ref offset) : null);
			Ag dst = (HasFlag((CombatMessageFlags)data2, CombatMessageFlags.Dst) ? ReadAg(data, ref offset) : null);
			string skillName = (HasFlag((CombatMessageFlags)data2, CombatMessageFlags.SkillName) ? ReadString(data, ref offset) : null);
			ulong id = ReadUInt64(data, ref offset);
			ulong revision = ReadUInt64(data, ref offset);
			return new CombatEvent(ev, src, dst, skillName, id, revision);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool HasFlag(CombatMessageFlags data, CombatMessageFlags flag)
		{
			return (data & flag) == flag;
		}

		private static Ev ReadEv(byte[] data, ref int offset)
		{
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			ulong num = ReadUInt64(data, ref offset);
			ulong srcAgent = ReadUInt64(data, ref offset);
			ulong dstAgent = ReadUInt64(data, ref offset);
			int value = ReadInt32(data, ref offset);
			int buffDmg = ReadInt32(data, ref offset);
			uint overStackValue = ReadUInt32(data, ref offset);
			uint skillId = ReadUInt32(data, ref offset);
			ushort srcInstId = ReadUInt16(data, ref offset);
			ushort dstInstId = ReadUInt16(data, ref offset);
			ushort srcMasterInstId = ReadUInt16(data, ref offset);
			ushort dstMasterInstId = ReadUInt16(data, ref offset);
			byte iff = ReadUInt8(data, ref offset);
			bool buff = ReadBool(data, ref offset);
			byte result = ReadUInt8(data, ref offset);
			byte isActivation = ReadUInt8(data, ref offset);
			byte isBuffRemove = ReadUInt8(data, ref offset);
			bool isNinety = ReadBool(data, ref offset);
			bool isFifty = ReadBool(data, ref offset);
			bool isMoving = ReadBool(data, ref offset);
			byte isStateChange = ReadUInt8(data, ref offset);
			bool isFlanking = ReadBool(data, ref offset);
			bool isShields = ReadBool(data, ref offset);
			bool isOffCycle = ReadBool(data, ref offset);
			byte pad61 = ReadUInt8(data, ref offset);
			byte pad62 = ReadUInt8(data, ref offset);
			byte pad63 = ReadUInt8(data, ref offset);
			byte pad64 = ReadUInt8(data, ref offset);
			return new Ev(num, srcAgent, dstAgent, value, buffDmg, overStackValue, skillId, srcInstId, dstInstId, srcMasterInstId, dstMasterInstId, (IFF)iff, buff, result, (Activation)isActivation, (BuffRemove)isBuffRemove, isNinety, isFifty, isMoving, (StateChange)isStateChange, isFlanking, isShields, isOffCycle, pad61, pad62, pad63, pad64);
		}

		private static Ag ReadAg(byte[] data, ref int offset)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			string text = ReadString(data, ref offset);
			ulong id = ReadUInt64(data, ref offset);
			uint profession = ReadUInt32(data, ref offset);
			uint elite = ReadUInt32(data, ref offset);
			uint self = ReadUInt32(data, ref offset);
			ushort team = ReadUInt16(data, ref offset);
			return new Ag(text, id, profession, elite, self, team);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string ReadString(byte[] data, ref int offset)
		{
			ulong length = ReadUInt64(data, ref offset);
			string @string = Encoding.UTF8.GetString(data, offset, (int)length);
			offset += (int)length;
			return @string;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ulong ReadUInt64(byte[] data, ref int offset)
		{
			ulong result = BitConverter.ToUInt64(data, offset);
			offset += 8;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint ReadUInt32(byte[] data, ref int offset)
		{
			uint result = BitConverter.ToUInt32(data, offset);
			offset += 4;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int ReadInt32(byte[] data, ref int offset)
		{
			int result = BitConverter.ToInt32(data, offset);
			offset += 4;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ushort ReadUInt16(byte[] data, ref int offset)
		{
			ushort result = BitConverter.ToUInt16(data, offset);
			offset += 2;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte ReadUInt8(byte[] data, ref int offset)
		{
			byte result = data[offset];
			offset++;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ReadBool(byte[] data, ref int offset)
		{
			bool result = data[offset] != 0;
			offset++;
			return result;
		}
	}
}
