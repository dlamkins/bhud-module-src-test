using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.Services
{
	[DataContract]
	public class SharedSettings
	{
		private bool _loaded;

		private string _path;

		private RectangleDimensions _windowOffset = new RectangleDimensions(8, 31, -8, -8);

		[DataMember]
		public RectangleDimensions WindowOffset
		{
			get
			{
				return _windowOffset;
			}
			set
			{
				SetValue(ref _windowOffset, value);
			}
		}

		public bool Check { get; set; }

		public async Task Load(string p, bool force = false)
		{
			if (!(!_loaded || force))
			{
				return;
			}
			_path = p;
			if (File.Exists(_path) && await FileExtension.WaitForFileUnlock(_path))
			{
				using StreamReader reader = File.OpenText(_path);
				JsonConvert.DeserializeObject<SharedSettings>(await reader.ReadToEndAsync(), SerializerSettings.Default);
			}
			_loaded = true;
		}

		private async void Save()
		{
			string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
			if (await FileExtension.WaitForFileUnlock(_path))
			{
				using StreamWriter writer = new StreamWriter(_path);
				await writer.WriteAsync(json);
			}
		}

		private void SetValue<T>(ref T prop, T value)
		{
			if (!object.Equals(prop, value))
			{
				prop = value;
				if (_loaded)
				{
					Save();
				}
			}
		}
	}
}
