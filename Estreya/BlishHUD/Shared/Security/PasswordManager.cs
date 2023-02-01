using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Utils;

namespace Estreya.BlishHUD.Shared.Security
{
	public class PasswordManager
	{
		private static readonly Logger Logger = Logger.GetLogger<PasswordManager>();

		private byte[] _passwordEntroy;

		private readonly string _directoryPath;

		public PasswordManager(string directoryPath)
		{
			_directoryPath = Path.Combine(directoryPath, "credentials");
		}

		public void InitializeEntropy(byte[] data)
		{
			_passwordEntroy = data;
		}

		public async Task Save(string key, byte[] data, bool silent = false)
		{
			byte[] protectedData = EncryptData(data, silent);
			if (protectedData != null)
			{
				await WritePasswordFile(key, protectedData);
			}
		}

		public async Task<byte[]> Retrive(string key, bool silent = false)
		{
			return DecryptData(await ReadPasswordFile(key), silent);
		}

		private async Task WritePasswordFile(string key, byte[] data)
		{
			string dataString = Convert.ToBase64String(data);
			Directory.CreateDirectory(_directoryPath);
			await FileUtil.WriteStringAsync(Path.Combine(_directoryPath, key + ".pwd"), dataString);
		}

		private async Task<byte[]> ReadPasswordFile(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException("key", "Key can't be null.");
			}
			string filePath = Path.Combine(_directoryPath, key + ".pwd");
			if (!File.Exists(filePath))
			{
				return null;
			}
			string fileContent = await FileUtil.ReadStringAsync(filePath);
			if (string.IsNullOrWhiteSpace(fileContent))
			{
				return null;
			}
			return Convert.FromBase64String(fileContent);
		}

		private byte[] EncryptData(byte[] data, bool silent = false)
		{
			if (_passwordEntroy == null)
			{
				throw new ArgumentException("Entroy was not initialized.");
			}
			if (data == null)
			{
				return null;
			}
			try
			{
				return ProtectedData.Protect(data, _passwordEntroy, DataProtectionScope.CurrentUser);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to encrypt data:");
				if (!silent)
				{
					throw;
				}
			}
			return null;
		}

		private byte[] DecryptData(byte[] data, bool silent = false)
		{
			if (_passwordEntroy == null)
			{
				throw new ArgumentException("Entroy was not initialized.");
			}
			if (data == null)
			{
				return null;
			}
			try
			{
				return ProtectedData.Unprotect(data, _passwordEntroy, DataProtectionScope.CurrentUser);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to decrypt data:");
				if (!silent)
				{
					throw;
				}
			}
			return null;
		}
	}
}
