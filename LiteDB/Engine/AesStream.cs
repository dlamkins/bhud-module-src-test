using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace LiteDB.Engine
{
	public class AesStream : Stream
	{
		private readonly Aes _aes;

		private readonly ICryptoTransform _encryptor;

		private readonly ICryptoTransform _decryptor;

		private readonly string _name;

		private readonly Stream _stream;

		private readonly CryptoStream _reader;

		private readonly CryptoStream _writer;

		private readonly byte[] _decryptedZeroes = new byte[16];

		private static readonly byte[] _emptyContent = new byte[8175];

		public byte[] Salt { get; }

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => _stream.CanSeek;

		public override bool CanWrite => _stream.CanWrite;

		public override long Length => _stream.Length - 8192;

		public override long Position
		{
			get
			{
				return _stream.Position - 8192;
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		public long StreamPosition => _stream.Position;

		public AesStream(string password, Stream stream)
		{
			_stream = stream;
			FileStream fileStream = _stream as FileStream;
			_name = ((fileStream != null) ? Path.GetFileName(fileStream.Name) : null);
			bool isNew = _stream.Length < 8192;
			_stream.Position = 0L;
			try
			{
				if (isNew)
				{
					Salt = NewSalt();
					_stream.WriteByte(1);
					_stream.Write(Salt, 0, 16);
				}
				else
				{
					Salt = new byte[16];
					if (_stream.ReadByte() != 1)
					{
						throw LiteException.FileNotEncrypted();
					}
					_stream.Read(Salt, 0, 16);
				}
				_aes = Aes.Create();
				_aes.Padding = PaddingMode.None;
				_aes.Mode = CipherMode.ECB;
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, Salt);
				using (pdb)
				{
					_aes.Key = pdb.GetBytes(32);
					_aes.IV = pdb.GetBytes(16);
				}
				_encryptor = _aes.CreateEncryptor();
				_decryptor = _aes.CreateDecryptor();
				_reader = (_stream.CanRead ? new CryptoStream(_stream, _decryptor, CryptoStreamMode.Read) : null);
				_writer = (_stream.CanWrite ? new CryptoStream(_stream, _encryptor, CryptoStreamMode.Write) : null);
				_stream.Position = 32L;
				byte[] checkBuffer = new byte[32];
				if (!isNew)
				{
					_stream.Read(checkBuffer, 0, checkBuffer.Length);
					isNew = checkBuffer.All((byte x) => x == 0);
					Array.Clear(checkBuffer, 0, checkBuffer.Length);
					_stream.Position = 32L;
				}
				if (isNew)
				{
					checkBuffer.Fill(1, 0, checkBuffer.Length);
					_writer.Write(checkBuffer, 0, checkBuffer.Length);
					_stream.Position = 8191L;
					_stream.WriteByte(0);
				}
				else
				{
					_reader.Read(checkBuffer, 0, checkBuffer.Length);
					if (!checkBuffer.All((byte x) => x == 1))
					{
						throw LiteException.InvalidPassword();
					}
				}
				_stream.Position = 8192L;
				_stream.FlushToDisk();
				using MemoryStream ms = new MemoryStream(new byte[16]);
				using CryptoStream tempStream = new CryptoStream(ms, _decryptor, CryptoStreamMode.Read);
				tempStream.Read(_decryptedZeroes, 0, _decryptedZeroes.Length);
			}
			catch
			{
				_stream.Dispose();
				throw;
			}
		}

		public override int Read(byte[] array, int offset, int count)
		{
			Constants.ENSURE(count == 8192, "buffer size must be PAGE_SIZE");
			Constants.ENSURE(Position % 8192 == 0, $"AesRead: position must be in PAGE_SIZE module. Position={Position}, File={_name}");
			int result = _reader.Read(array, offset, count);
			if (IsBlank(array, offset))
			{
				array.Fill(0, offset, count);
			}
			return result;
		}

		public override void Write(byte[] array, int offset, int count)
		{
			Constants.ENSURE(count == 8192, "buffer size must be PAGE_SIZE");
			Constants.ENSURE(Position % 8192 == 0, $"AesWrite: position must be in PAGE_SIZE module. Position={Position}, File={_name}");
			_writer.Write(array, offset, count);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_stream?.Dispose();
			_encryptor.Dispose();
			_decryptor.Dispose();
			_aes.Dispose();
		}

		public static byte[] NewSalt()
		{
			byte[] salt = new byte[16];
			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(salt);
			return salt;
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _stream.Seek(offset + 8192, origin);
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value + 8192);
		}

		private unsafe bool IsBlank(byte[] array, int offset)
		{
			fixed (byte* arrayPtr = array)
			{
				fixed (byte* ptr2 = _decryptedZeroes)
				{
					void* vPtr = ptr2;
					ulong* ptr = (ulong*)(arrayPtr + offset);
					ulong* zeroptr = (ulong*)vPtr;
					if (*ptr == *zeroptr)
					{
						return ptr[1] == zeroptr[1];
					}
					return false;
				}
			}
		}
	}
}
