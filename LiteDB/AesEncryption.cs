using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LiteDB
{
	internal class AesEncryption : IDisposable
	{
		private Aes _aes;

		public AesEncryption(string password, byte[] salt)
		{
			_aes = Aes.Create();
			_aes.Padding = PaddingMode.Zeros;
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
			using (pdb)
			{
				_aes.Key = pdb.GetBytes(32);
				_aes.IV = pdb.GetBytes(16);
			}
		}

		public byte[] Encrypt(byte[] bytes)
		{
			using ICryptoTransform encryptor = _aes.CreateEncryptor();
			using MemoryStream stream = new MemoryStream();
			using CryptoStream crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
			crypto.Write(bytes, 0, bytes.Length);
			crypto.FlushFinalBlock();
			stream.Position = 0L;
			byte[] encrypted = new byte[stream.Length];
			stream.Read(encrypted, 0, encrypted.Length);
			return encrypted;
		}

		public byte[] Decrypt(byte[] encryptedValue, int offset = 0, int count = -1)
		{
			using ICryptoTransform decryptor = _aes.CreateDecryptor();
			using MemoryStream stream = new MemoryStream();
			using CryptoStream crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write);
			crypto.Write(encryptedValue, offset, (count == -1) ? encryptedValue.Length : count);
			crypto.FlushFinalBlock();
			stream.Position = 0L;
			byte[] decryptedBytes = new byte[stream.Length];
			stream.Read(decryptedBytes, 0, decryptedBytes.Length);
			return decryptedBytes;
		}

		public static byte[] HashSHA1(string password)
		{
			return SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		public static byte[] Salt(int maxLength = 16)
		{
			byte[] salt = new byte[maxLength];
			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			using (rng)
			{
				rng.GetBytes(salt);
				return salt;
			}
		}

		public void Dispose()
		{
			if (_aes != null)
			{
				_aes.Dispose();
				_aes = null;
			}
		}
	}
}
