using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TagGroup
	{
		public static string DefaultName => strings.GroupNotDefined;

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

		public static TagGroup Empty { get; internal set; } = new TagGroup();


		public event PropertyAndValueChangedEventHandler? PropertyChanged;

		public TagGroup()
		{
			_003CPriority_003Ek__BackingField = 1;
			_003CName_003Ek__BackingField = DefaultName;
			Icon = new DetailedTexture(156025)
			{
				TextureRegion = new Rectangle(44, 48, 43, 46)
			};
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
		}

		public TagGroup(string name)
			: this()
		{
			if (!string.IsNullOrEmpty(name))
			{
				Name = name;
			}
		}

		private void OnAssetIdChanged(object sender, ValueChangedEventArgs<int> e)
		{
			Icon = new DetailedTexture(e.NewValue);
			Icon.TextureRegion = TextureRegion ?? Icon.Texture?.Bounds ?? Rectangle.Empty;
			this.PropertyChanged?.Invoke(this, new PropertyAndValueChangedEventArgs("AssetId", e.OldValue, e.NewValue));
		}

		private void OnTextureRegionChanged(object sender, ValueChangedEventArgs<Rectangle?> e)
		{
			if (Icon != null)
			{
				Icon.TextureRegion = e.NewValue ?? Icon.Texture?.Bounds ?? Rectangle.Empty;
			}
			this.PropertyChanged?.Invoke(this, new PropertyAndValueChangedEventArgs("TextureRegion", e.OldValue, e.NewValue));
		}

		private void OnNameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyAndValueChangedEventArgs("Name", e.OldValue, e.NewValue));
		}

		private void OnPriorityChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.PropertyChanged?.Invoke(this, new PropertyAndValueChangedEventArgs("Priority", e.OldValue, e.NewValue));
		}
	}
}
