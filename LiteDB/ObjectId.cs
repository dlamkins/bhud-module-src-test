using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace LiteDB
{
	public class ObjectId : IComparable<ObjectId>, IEquatable<ObjectId>
	{
		private static readonly int _machine;

		private static readonly short _pid;

		private static int _increment;

		public static ObjectId Empty => new ObjectId();

		public int Timestamp { get; }

		public int Machine { get; }

		public short Pid { get; }

		public int Increment { get; }

		public DateTime CreationTime => BsonValue.UnixEpoch.AddSeconds(Timestamp);

		public ObjectId()
		{
			Timestamp = 0;
			Machine = 0;
			Pid = 0;
			Increment = 0;
		}

		public ObjectId(int timestamp, int machine, short pid, int increment)
		{
			Timestamp = timestamp;
			Machine = machine;
			Pid = pid;
			Increment = increment;
		}

		public ObjectId(ObjectId from)
		{
			Timestamp = from.Timestamp;
			Machine = from.Machine;
			Pid = from.Pid;
			Increment = from.Increment;
		}

		public ObjectId(string value)
			: this(FromHex(value))
		{
		}

		public ObjectId(byte[] bytes, int startIndex = 0)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			Timestamp = (bytes[startIndex] << 24) + (bytes[startIndex + 1] << 16) + (bytes[startIndex + 2] << 8) + bytes[startIndex + 3];
			Machine = (bytes[startIndex + 4] << 16) + (bytes[startIndex + 5] << 8) + bytes[startIndex + 6];
			Pid = (short)((bytes[startIndex + 7] << 8) + bytes[startIndex + 8]);
			Increment = (bytes[startIndex + 9] << 16) + (bytes[startIndex + 10] << 8) + bytes[startIndex + 11];
		}

		private static byte[] FromHex(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length != 24)
			{
				throw new ArgumentException($"ObjectId strings should be 24 hex characters, got {value.Length} : \"{value}\"");
			}
			byte[] bytes = new byte[12];
			for (int i = 0; i < 24; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
			}
			return bytes;
		}

		public bool Equals(ObjectId other)
		{
			if (other != null && Timestamp == other.Timestamp && Machine == other.Machine && Pid == other.Pid)
			{
				return Increment == other.Increment;
			}
			return false;
		}

		public override bool Equals(object other)
		{
			return Equals(other as ObjectId);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = 37 * hash + Timestamp.GetHashCode();
			hash = 37 * hash + Machine.GetHashCode();
			hash = 37 * hash + Pid.GetHashCode();
			return 37 * hash + Increment.GetHashCode();
		}

		public int CompareTo(ObjectId other)
		{
			int r = Timestamp.CompareTo(other.Timestamp);
			if (r != 0)
			{
				return r;
			}
			r = Machine.CompareTo(other.Machine);
			if (r != 0)
			{
				return r;
			}
			r = Pid.CompareTo(other.Pid);
			if (r != 0)
			{
				if (r >= 0)
				{
					return 1;
				}
				return -1;
			}
			return Increment.CompareTo(other.Increment);
		}

		public void ToByteArray(byte[] bytes, int startIndex)
		{
			bytes[startIndex] = (byte)(Timestamp >> 24);
			bytes[startIndex + 1] = (byte)(Timestamp >> 16);
			bytes[startIndex + 2] = (byte)(Timestamp >> 8);
			bytes[startIndex + 3] = (byte)Timestamp;
			bytes[startIndex + 4] = (byte)(Machine >> 16);
			bytes[startIndex + 5] = (byte)(Machine >> 8);
			bytes[startIndex + 6] = (byte)Machine;
			bytes[startIndex + 7] = (byte)(Pid >> 8);
			bytes[startIndex + 8] = (byte)Pid;
			bytes[startIndex + 9] = (byte)(Increment >> 16);
			bytes[startIndex + 10] = (byte)(Increment >> 8);
			bytes[startIndex + 11] = (byte)Increment;
		}

		public byte[] ToByteArray()
		{
			byte[] bytes = new byte[12];
			ToByteArray(bytes, 0);
			return bytes;
		}

		public override string ToString()
		{
			return BitConverter.ToString(ToByteArray()).Replace("-", "").ToLower();
		}

		public static bool operator ==(ObjectId lhs, ObjectId rhs)
		{
			if ((object)lhs == null)
			{
				return (object)rhs == null;
			}
			if ((object)rhs == null)
			{
				return false;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ObjectId lhs, ObjectId rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator >=(ObjectId lhs, ObjectId rhs)
		{
			return lhs.CompareTo(rhs) >= 0;
		}

		public static bool operator >(ObjectId lhs, ObjectId rhs)
		{
			return lhs.CompareTo(rhs) > 0;
		}

		public static bool operator <(ObjectId lhs, ObjectId rhs)
		{
			return lhs.CompareTo(rhs) < 0;
		}

		public static bool operator <=(ObjectId lhs, ObjectId rhs)
		{
			return lhs.CompareTo(rhs) <= 0;
		}

		static ObjectId()
		{
			_machine = (GetMachineHash() + 10000) & 0xFFFFFF;
			_increment = new Random().Next();
			try
			{
				_pid = (short)GetCurrentProcessId();
			}
			catch (SecurityException)
			{
				_pid = 0;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int GetCurrentProcessId()
		{
			return new Random().Next(0, 5000);
		}

		private static int GetMachineHash()
		{
			string hostName = "SOMENAME";
			return 0xFFFFFF & hostName.GetHashCode();
		}

		public static ObjectId NewObjectId()
		{
			long num = (long)Math.Floor((DateTime.UtcNow - BsonValue.UnixEpoch).TotalSeconds);
			return new ObjectId(increment: Interlocked.Increment(ref _increment) & 0xFFFFFF, timestamp: (int)num, machine: _machine, pid: _pid);
		}
	}
}
