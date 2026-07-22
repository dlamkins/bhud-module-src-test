using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplateTag
	{
		public static string DefaultName => strings.NewTemplate;

		public string Group
		{
			[CompilerGenerated]
			get
			{
				return _003CGroup_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CGroup_003Ek__BackingField, value, delegate(string v)
				{
					_003CGroup_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<string>(OnGroupChanged));
			}
		}

		public int Priority
		{
			[CompilerGenerated]
			get
			{
				return _003CPriority_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CPriority_003Ek__BackingField, value, delegate(int v)
				{
					_003CPriority_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<int>(OnPriorityChanged));
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CName_003Ek__BackingField, value, delegate(string v)
				{
					_003CName_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<string>(OnNameChanged));
			}
		}

		[JsonIgnore]
		public DetailedTexture Icon { get; set; }

		public int AssetId
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetId_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAssetId_003Ek__BackingField, value, delegate(int v)
				{
					_003CAssetId_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<int>(OnAssetIdChanged));
			}
		}

		public Rectangle? TextureRegion
		{
			[CompilerGenerated]
			get
			{
				return _003CTextureRegion_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTextureRegion_003Ek__BackingField, value, delegate(Rectangle? v)
				{
					_003CTextureRegion_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Rectangle?>(OnTextureRegionChanged));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public TemplateTag()
		{
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			_003CName_003Ek__BackingField = DefaultName;
			Icon = new DetailedTexture(156025)
			{
				TextureRegion = new Rectangle(32, 32, 64, 64)
			};
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
		}

		public TemplateTag(string name)
		{
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			_003CName_003Ek__BackingField = DefaultName;
			Icon = new DetailedTexture(156025)
			{
				TextureRegion = new Rectangle(32, 32, 64, 64)
			};
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
			if (!string.IsNullOrEmpty(name))
			{
				Name = name;
			}
		}

		private void OnAssetIdChanged(object sender, ValueChangedEventArgs<int> e)
		{
			Icon = new DetailedTexture(e.NewValue);
			Icon.TextureRegion = TextureRegion ?? Icon.Texture?.Bounds ?? Rectangle.Empty;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AssetId"));
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Icon"));
		}

		private void OnTextureRegionChanged(object sender, ValueChangedEventArgs<Rectangle?> e)
		{
			DetailedTexture icon = Icon;
			if (icon != null)
			{
				icon.TextureRegion = e.NewValue ?? Icon.Texture?.Bounds ?? Rectangle.Empty;
			}
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextureRegion"));
		}

		private void OnNameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
		}

		private void OnPriorityChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Priority"));
		}

		private void OnGroupChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Group"));
		}

		public string ToJson()
		{
			try
			{
				return JsonConvert.SerializeObject(this);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
