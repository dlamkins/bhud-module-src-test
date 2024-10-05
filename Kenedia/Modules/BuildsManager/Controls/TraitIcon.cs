using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TraitIcon : DetailedTexture
	{
		private Trait _trait;

		public Trait Trait
		{
			get
			{
				return _trait;
			}
			set
			{
				Common.SetProperty(ref _trait, value, new Action(ApplyTrait));
			}
		}

		public bool Selected { get; set; }

		private void ApplyTrait()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			base.Texture = Trait?.Icon;
			if (Trait != null && Trait.Icon != null)
			{
				int padding = Trait.Icon.Width / 16;
				base.TextureRegion = new Rectangle(padding, padding, Trait.Icon.Width - padding * 2, Trait.Icon.Height - padding * 2);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			Trait = null;
		}
	}
}
