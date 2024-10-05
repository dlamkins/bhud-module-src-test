using System.ComponentModel;
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
		[JsonProperty("AssetId")]
		private int _assetId = 156025;

		[JsonProperty("Name")]
		private string _name = DefaultName;

		[JsonProperty("TextureRegion")]
		private Rectangle? _textureRegion = new Rectangle(0, 0, 32, 32);

		[JsonProperty("Priority")]
		private int _priority = 1;

		[JsonProperty("Group")]
		private string _group = string.Empty;

		public static string DefaultName => strings.NewTemplate;

		[JsonIgnore]
		public string Group
		{
			get
			{
				return _group;
			}
			set
			{
				Common.SetProperty(ref _group, value, new ValueChangedEventHandler<string>(OnGroupChanged));
			}
		}

		[JsonIgnore]
		public int Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				Common.SetProperty(ref _priority, value, new ValueChangedEventHandler<int>(OnPriorityChanged));
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
		public DetailedTexture Icon { get; set; } = new DetailedTexture((AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Tag, "Tag"));


		[JsonIgnore]
		public int AssetId
		{
			get
			{
				return _assetId;
			}
			set
			{
				Common.SetProperty(ref _assetId, value, new ValueChangedEventHandler<int>(OnAssetIdChanged));
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
		}//IL_001d: Unknown result type (might be due to invalid IL or missing references)


		public TemplateTag(string name)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
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
	}
}
