using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Newtonsoft.Json;

namespace rp.spark.Services
{
	internal static class FileStore
	{
		public static void EnsureDirectory(string directory, Logger logger, string description)
		{
			try
			{
				Directory.CreateDirectory(directory);
			}
			catch (UnauthorizedAccessException ex2)
			{
				BlishWarnings.FileSaveBlocked(ex2, directory, "create the " + description + " directory");
				logger.Warn("Failed to create {description} directory ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex2)
				});
				throw;
			}
			catch (Exception ex)
			{
				logger.Warn("Failed to create {description} directory ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex)
				});
				throw;
			}
		}

		public static IReadOnlyList<string> GetFiles(string directory, Logger logger, string description)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(directory))
				{
					return new List<string>();
				}
				return Directory.GetFiles(directory, "*.json").OrderBy((string path) => path, StringComparer.OrdinalIgnoreCase).ToList();
			}
			catch (DirectoryNotFoundException)
			{
				return new List<string>();
			}
			catch (UnauthorizedAccessException ex2)
			{
				BlishWarnings.FileSaveBlocked(ex2, directory, "read " + description + " files");
				logger.Warn("Failed to enumerate {description} JSON files ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex2)
				});
				return new List<string>();
			}
			catch (Exception ex)
			{
				logger.Warn("Failed to enumerate {description} JSON files ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex)
				});
				return new List<string>();
			}
		}

		public static T ReadFile<T>(string path, Logger logger, string description) where T : class
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					return null;
				}
				return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
			}
			catch (FileNotFoundException)
			{
				return null;
			}
			catch (DirectoryNotFoundException)
			{
				return null;
			}
			catch (UnauthorizedAccessException ex2)
			{
				BlishWarnings.FileSaveBlocked(ex2, path, "read " + description + " data");
				logger.Warn("Failed to read {description} JSON file ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex2)
				});
				return null;
			}
			catch (Exception ex)
			{
				logger.Warn("Failed to read {description} JSON file ({errorType}).", new object[2]
				{
					description,
					ErrorType(ex)
				});
				return null;
			}
		}

		public static bool TryWrite(string path, object value, Logger logger, string description)
		{
			return TryWriteFile(path, logger, description, "JSON", delegate
			{
				string text = JsonConvert.SerializeObject(value, (Formatting)1);
				WriteText(path, text);
			});
		}

		public static bool TryWriteText(string path, string text, Logger logger, string description)
		{
			return TryWriteFile(path, logger, description, "text", delegate
			{
				WriteText(path, text ?? string.Empty);
			});
		}

		public static bool TryWriteBytes(string path, byte[] bytes, Logger logger, string description)
		{
			return TryWriteFile(path, logger, description, "binary", delegate
			{
				if (bytes == null)
				{
					throw new InvalidOperationException("Cannot write bytes without file content.");
				}
				WriteBytes(path, bytes);
			});
		}

		private static bool TryWriteFile(string path, Logger logger, string description, string fileKind, Action writeFile)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					throw new InvalidOperationException("Cannot write " + fileKind + " without a safe file path.");
				}
				writeFile();
				return true;
			}
			catch (UnauthorizedAccessException ex2)
			{
				BlishWarnings.FileSaveBlocked(ex2, path, "save " + description + " data");
				logger.Warn("Failed to write {description} {fileKind} file ({errorType}).", new object[3]
				{
					description,
					fileKind,
					ErrorType(ex2)
				});
				return false;
			}
			catch (Exception ex)
			{
				logger.Warn("Failed to write {description} {fileKind} file ({errorType}).", new object[3]
				{
					description,
					fileKind,
					ErrorType(ex)
				});
				return false;
			}
		}

		public static string GetSafePath(string directory, string key)
		{
			if (string.IsNullOrWhiteSpace(directory) || string.IsNullOrWhiteSpace(key))
			{
				return null;
			}
			string safeFileName = GetSafeFileName(key.Trim()) + ".json";
			string fullPath2 = Path.GetFullPath(directory);
			string fullPath = Path.GetFullPath(Path.Combine(fullPath2, safeFileName));
			string requiredPrefix = fullPath2.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			if (!fullPath.StartsWith(requiredPrefix, StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("JSON path invalid.");
			}
			return fullPath;
		}

		private static void WriteText(string path, string text)
		{
			AtomicWrite(path, delegate(string temporaryPath)
			{
				File.WriteAllText(temporaryPath, text ?? string.Empty);
			});
		}

		private static void WriteBytes(string path, byte[] bytes)
		{
			AtomicWrite(path, delegate(string temporaryPath)
			{
				File.WriteAllBytes(temporaryPath, bytes);
			});
		}

		private static void AtomicWrite(string path, Action<string> writeTemporaryFile)
		{
			string directory = Path.GetDirectoryName(path);
			if (!string.IsNullOrWhiteSpace(directory))
			{
				Directory.CreateDirectory(directory);
			}
			string temporaryPath = $"{path}.{Guid.NewGuid():N}.tmp";
			writeTemporaryFile(temporaryPath);
			ReplaceFile(temporaryPath, path);
		}

		private static void ReplaceFile(string temporaryPath, string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Replace(temporaryPath, path, null);
				}
				else
				{
					File.Move(temporaryPath, path);
				}
			}
			catch
			{
				if (!File.Exists(temporaryPath))
				{
					throw;
				}
				File.Copy(temporaryPath, path, overwrite: true);
				File.Delete(temporaryPath);
			}
		}

		private static string GetSafeFileName(string value)
		{
			string safe = value ?? string.Empty;
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char invalidChar in invalidFileNameChars)
			{
				safe = safe.Replace(invalidChar, '_');
			}
			if (!string.IsNullOrWhiteSpace(safe))
			{
				return safe;
			}
			return "unknown";
		}

		private static string ErrorType(Exception ex)
		{
			return ex?.GetType().Name ?? "Unknown";
		}
	}
}
