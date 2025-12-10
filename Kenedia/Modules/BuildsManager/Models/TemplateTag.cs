using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
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

		[JsonIgnore]
		public string Group
		{
			[CompilerGenerated]
			get
			{
				return _003CGroup_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CGroup_003Ek__BackingField, value, new ValueChangedEventHandler<string>(OnGroupChanged));
			}
		}

		[JsonIgnore]
		public int Priority
		{
			[CompilerGenerated]
			get
			{
				return _003CPriority_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CPriority_003Ek__BackingField, value, new ValueChangedEventHandler<int>(OnPriorityChanged));
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

		[JsonIgnore]
		public int AssetId
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetId_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CAssetId_003Ek__BackingField, value, new ValueChangedEventHandler<int>(OnAssetIdChanged));
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
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			Icon = new DetailedTexture((AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Tag, "Tag"));
			_003CAssetId_003Ek__BackingField = 156025;
			base._002Ector();
		}

		public TemplateTag(string name)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_003CGroup_003Ek__BackingField = string.Empty;
			_003CPriority_003Ek__BackingField = 1;
			Icon = new DetailedTexture((AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Tag, "Tag"));
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
		}

		private void OnTextureRegionChanged(object sender, ValueChangedEventArgs<Rectangle?> e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (Icon != null)
			{
				Icon.TextureRegion = (Rectangle)(((_003F?)e.NewValue) ?? Icon.Texture?.Bounds ?? Rectangle.get_Empty());
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
