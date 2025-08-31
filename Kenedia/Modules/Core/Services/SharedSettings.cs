using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.Services
{
	[DataContract]
	public class SharedSettings : INotifyPropertyChanged
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
				Common.SetProperty(ref _windowOffset, value, new ValueChangedEventHandler<RectangleDimensions>(OnPropertyChanged));
			}
		}

		public bool Check { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(object sender, ValueChangedEventArgs<RectangleDimensions> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
			if (_loaded)
			{
				Save();
			}
		}

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
				SharedSettings source = JsonConvert.DeserializeObject<SharedSettings>(await reader.ReadToEndAsync(), SerializerSettings.Default);
				WindowOffset = source.WindowOffset;
				_loaded = true;
			}
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
	}
}
