using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DanceDanceRotationModule.Views
{
	public class NotesWindow : DdrWindowBase
	{
		public const int InitialWidth = 800;

		public const int InitialHeight = 400;

		public NotesOrientation Orientation { get; private set; }

		public NotesWindow(NotesOrientation orientation)
			: this(Resources.Instance.WindowBackgroundEmptyTexture, new Rectangle(40, 26, 913, 691), new Rectangle(40, 26, 913, 691))
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			base.Title = "Dance Dance Rotation";
			base.Emblem = Resources.Instance.DdrLogoEmblemTexture;
			base.CanResize = true;
			base.CanCloseWithEscape = false;
			((Control)this).set_Size(new Point(800, 400));
			base.SavesPosition = true;
			base.SavesSize = true;
			base.Id = "DDR_MainView_ID_" + orientation;
			Orientation = orientation;
		}

		public NotesWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(background, windowRegion, contentRegion);
		}

		public NotesWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion)
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)


		public NotesWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(background, windowRegion, contentRegion, windowSize);
		}

		public NotesWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion, windowSize)
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)


		public void Show(IView view)
		{
			ShowView(view);
			((Control)this).Show();
		}

		public void ToggleWindow(IView view)
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
			}
			else
			{
				Show(view);
			}
		}

		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			return new Point(MathHelper.Clamp(newSize.X, OrientationExtensions.IsVertical(Orientation) ? 280 : 400, OrientationExtensions.IsVertical(Orientation) ? 800 : 2048), MathHelper.Clamp(newSize.Y, OrientationExtensions.IsVertical(Orientation) ? 460 : 280, OrientationExtensions.IsVertical(Orientation) ? 2048 : 800));
		}

		private Rectangle GetInitialWindowSize(NotesOrientation orientation)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			switch (orientation)
			{
			case NotesOrientation.RightToLeft:
			case NotesOrientation.LeftToRight:
				return new Rectangle(40, 26, 913, 691);
			default:
				return new Rectangle(40, 26, 691, 913);
			}
		}
	}
}
