using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class ImageSpinner : Panel
	{
		private readonly Point defaultLoadingSpinnerSize;

		private readonly LoadingSpinner loadingSpinner;

		public Image Image { get; private set; }

		public Color Tint
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return Image.get_Tint();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Image.set_Tint(value);
			}
		}

		public ImageSpinner(AsyncTexture2D texture)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate
			{
				((Control)loadingSpinner).Hide();
				((Control)Image).Show();
			});
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Visible(false);
			val.set_Texture(texture);
			((Control)val).set_Width(((Control)this).get_Width());
			((Control)val).set_Height(((Control)this).get_Height());
			Image = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Visible(true);
			loadingSpinner = val2;
			defaultLoadingSpinnerSize = ((Control)loadingSpinner).get_Size();
			((Control)loadingSpinner).set_Size(new Point(Math.Min(((Control)this).get_Width(), defaultLoadingSpinnerSize.X), Math.Min(((Control)this).get_Height(), defaultLoadingSpinnerSize.Y)));
			((Control)loadingSpinner).set_Location(new Point(((Control)this).get_Width() / 2 - ((Control)loadingSpinner).get_Width() / 2, ((Control)this).get_Height() / 2 - ((Control)loadingSpinner).get_Height() / 2));
			if (texture.get_Texture() != Textures.get_TransparentPixel())
			{
				((Control)Image).Show();
				((Control)loadingSpinner).Hide();
			}
		}

		public override void RecalculateLayout()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).set_Size(new Point(Math.Min(((Control)this).get_Width(), defaultLoadingSpinnerSize.X), Math.Min(((Control)this).get_Height(), defaultLoadingSpinnerSize.Y)));
				((Control)loadingSpinner).set_Location(new Point(((Control)this).get_Width() / 2 - ((Control)loadingSpinner).get_Width() / 2, ((Control)this).get_Height() / 2 - ((Control)loadingSpinner).get_Height() / 2));
			}
			if (Image != null)
			{
				((Control)Image).set_Width(((Control)this).get_Width());
				((Control)Image).set_Height(((Control)this).get_Height());
			}
		}

		protected override void DisposeControl()
		{
			((Control)Image).Dispose();
			((Control)loadingSpinner).Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
