using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;

namespace Charr.Timers_BlishHUD
{
	internal class SortedZipArchiveReader : IDataReader, IDisposable
	{
		private readonly ZipArchive _archive;

		private readonly string _archivePath;

		private readonly string _subPath;

		private readonly Mutex _exclusiveStreamAccessMutex;

		public string PhysicalPath => _archivePath;

		public SortedZipArchiveReader(string archivePath, string subPath = "")
		{
			if (!File.Exists(archivePath))
			{
				throw new FileNotFoundException("Archive path not found.", archivePath);
			}
			_archivePath = archivePath;
			_subPath = subPath;
			_exclusiveStreamAccessMutex = new Mutex(initiallyOwned: false);
			_archive = ZipFile.OpenRead(archivePath);
		}

		public IDataReader GetSubPath(string subPath)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			return (IDataReader)new ZipArchiveReader(_archivePath, Path.Combine(subPath));
		}

		public string GetPathRepresentation(string relativeFilePath = null)
		{
			return _archivePath + "[" + Path.GetFileName(Path.Combine(_subPath, relativeFilePath ?? string.Empty)) + "]";
		}

		public void LoadOnFileType(Action<Stream, IDataReader> loadFileFunc, string fileExtension = "", IProgress<string> progress = null)
		{
			foreach (ZipArchiveEntry entry in from file in (from e in _archive.get_Entries()
					where e.get_Name().EndsWith(fileExtension ?? "", StringComparison.OrdinalIgnoreCase)
					select e).ToList()
				orderby file.get_Name()
				select file)
			{
				progress?.Report($"Loading {entry.get_Name()}...");
				Stream entryStream = GetFileStream(entry.get_FullName());
				loadFileFunc(entryStream, (IDataReader)(object)this);
			}
		}

		public bool FileExists(string filePath)
		{
			return _archive.get_Entries().Any((ZipArchiveEntry entry) => string.Equals(GetUniformFileName(entry.get_FullName()), GetUniformFileName(Path.Combine(_subPath, filePath)), StringComparison.OrdinalIgnoreCase));
		}

		public List<ZipArchiveEntry> GetValidFileEntries(string fileExtension)
		{
			return (from e in _archive.get_Entries()
				where e.get_Name().EndsWith(fileExtension ?? "", StringComparison.OrdinalIgnoreCase)
				select e).ToList();
		}

		private string GetUniformFileName(string filePath)
		{
			return filePath.Replace("\\", "/").Replace("//", "/").Trim();
		}

		private ZipArchiveEntry GetArchiveEntry(string filePath)
		{
			string cleanFilePath = GetUniformFileName(Path.Combine(_subPath, filePath));
			foreach (ZipArchiveEntry zipEntry in _archive.get_Entries())
			{
				string cleanZipEntry = GetUniformFileName(zipEntry.get_FullName());
				if (string.Equals(cleanFilePath, cleanZipEntry, StringComparison.OrdinalIgnoreCase))
				{
					return zipEntry;
				}
			}
			return null;
		}

		public Stream GetFileStream(string filePath)
		{
			ZipArchiveEntry fileEntry;
			if ((fileEntry = GetArchiveEntry(filePath)) != null)
			{
				_exclusiveStreamAccessMutex.WaitOne();
				MemoryStream memStream = new MemoryStream();
				using (Stream entryStream = fileEntry.Open())
				{
					entryStream.CopyTo(memStream);
				}
				memStream.Position = 0L;
				_exclusiveStreamAccessMutex.ReleaseMutex();
				return memStream;
			}
			return null;
		}

		public byte[] GetFileBytes(string filePath)
		{
			using (MemoryStream fileStream = GetFileStream(filePath) as MemoryStream)
			{
				if (fileStream != null)
				{
					return fileStream.ToArray();
				}
			}
			return null;
		}

		public int GetFileBytes(string filePath, out byte[] fileBuffer)
		{
			fileBuffer = null;
			using (MemoryStream fileStream = GetFileStream(filePath) as MemoryStream)
			{
				if (fileStream != null)
				{
					fileBuffer = fileStream.GetBuffer();
					return (int)fileStream.Length;
				}
			}
			return 0;
		}

		public async Task<Stream> GetFileStreamAsync(string filePath)
		{
			return await Task.FromResult(GetFileStream(filePath));
		}

		public async Task<byte[]> GetFileBytesAsync(string filePath)
		{
			return await Task.FromResult(GetFileBytes(filePath));
		}

		public void DeleteRoot()
		{
			Dispose();
			File.Delete(_archivePath);
		}

		public void Dispose()
		{
			ZipArchive archive = _archive;
			if (archive != null)
			{
				archive.Dispose();
			}
		}
	}
}
