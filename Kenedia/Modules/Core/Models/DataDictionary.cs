using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Kenedia.Modules.Core.Converter;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public class DataDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDataDictionary
	{
		[JsonConverter(typeof(SemverVersionConverter))]
		public Version Version { get; set; } = new Version("0.0.0", false);


		public string FilePath { get; }

		public string FileName => Path.GetFileNameWithoutExtension(FilePath);

		public Func<Task> OnUpdate { get; set; }

		public DataDictionary()
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown


		public DataDictionary(string filePath)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			FilePath = filePath;
		}

		public DataDictionary(string filePath, Func<Task> update)
			: this(filePath)
		{
			OnUpdate = update;
		}

		public bool IsOutdated(Version version)
		{
			return Version < version;
		}

		public virtual async Task<bool> Load()
		{
			if (!File.Exists(FilePath))
			{
				return false;
			}
			using FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			using StreamReader reader = new StreamReader(stream);
			string content = await reader.ReadToEndAsync();
			if (string.IsNullOrWhiteSpace(content))
			{
				return false;
			}
			DataDictionaryDto<TKey, TValue> dto = JsonConvert.DeserializeObject<DataDictionaryDto<TKey, TValue>>(content, SerializerSettings.Default);
			if (dto == null)
			{
				return false;
			}
			Clear();
			foreach (KeyValuePair<TKey, TValue> kv in dto.Data)
			{
				Add(kv.Key, kv.Value);
			}
			Version = dto.Version;
			return true;
		}

		public virtual async Task Save()
		{
			string content = JsonConvert.SerializeObject(new DataDictionaryDto<TKey, TValue>
			{
				Version = Version,
				Data = new Dictionary<TKey, TValue>(this)
			}, SerializerSettings.Default);
			using FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
			using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			await writer.WriteAsync(content);
		}

		public virtual async Task Update(Version? version = null)
		{
			try
			{
				if (OnUpdate != null)
				{
					await OnUpdate();
				}
				if (version != (Version)null)
				{
					Version = version;
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<DataDictionary<TKey, TValue>>().Warn($"{ex}");
			}
		}
	}
}
