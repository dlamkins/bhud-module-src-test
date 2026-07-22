using System;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TraitIcon : DetailedTexture
	{
		public Trait Trait
		{
			[CompilerGenerated]
			get
			{
				return _003CTrait_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTrait_003Ek__BackingField, value, delegate(Trait v)
				{
					_003CTrait_003Ek__BackingField = v;
				}, new Action(ApplyTrait));
			}
		}

		public bool Selected { get; set; }

		private void ApplyTrait()
		{
			base.Texture = TexturesService.GetAsyncTexture(Trait?.IconAssetId);
			if (Trait != null && base.Texture != null)
			{
				int padding = base.Texture.Width / 16;
				base.TextureRegion = new Rectangle(padding, padding, base.Texture.Width - padding * 2, base.Texture.Height - padding * 2);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			Trait = null;
		}
	}
}
