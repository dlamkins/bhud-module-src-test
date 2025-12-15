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
		[JsonProperty("Name")]
		private string _name = DefaultName;

		[JsonProperty("TextureRegion")]
		private Rectangle? _textureRegion = new Rectangle(0, 0, 32, 32);

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

		[JsonIgnore]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Common.SetProperty(ref _name, value, new ValueChangedEventHandler<string>(OnNameChanged));
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

		[JsonIgnore]
		public Rectangle? TextureRegion
		{
			get
			{
				return _textureRegion;
			}
			set
			{
				Common.SetProperty(ref _textureRegion, value, new ValueChangedEventHandler<Rectangle?>(OnTextureRegionChanged));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public TemplateTag()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			Icon = new DetailedTexture(156025)
			{
				TextureRegion = new Rectangle(32, 32, 64, 64)
			};
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
		}

		public TemplateTag(string name)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			Icon = new DetailedTexture(156025)
			{
				TextureRegion = new Rectangle(32, 32, 64, 64)
			};
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
			if (!string.IsNullOrEmpty(name))
			{
				_name = name;
			}
		}

		private void OnAssetIdChanged(object sender, ValueChangedEventArgs<int> e)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Icon = new DetailedTexture(e.NewValue);
			Icon.TextureRegion = (Rectangle)(((_003F?)TextureRegion) ?? Icon.Texture?.Bounds ?? Rectangle.get_Empty());
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AssetId"));
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Icon"));
		}

		private void OnTextureRegionChanged(object sender, ValueChangedEventArgs<Rectangle?> e)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			DetailedTexture icon = Icon;
			if (icon != null)
			{
				icon.TextureRegion = (Rectangle)(((_003F?)e.NewValue) ?? Icon.Texture?.Bounds ?? Rectangle.get_Empty());
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
				return JsonConvert.SerializeObject((object)this);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
