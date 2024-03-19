using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule.Util
{
	public static class ControlExtensions
	{
		private const float ButtonUnhoveredOpacity = 0.7f;

		private const float ButtonHoveredOpacity = 1f;

		private const float ButtonHoveredAnimationDuration = 0.2f;

		public static readonly Point ImageButtonSmallSize = new Point(24, 24);

		public static void ConvertToButton(Control image, float unhoveredOpacity = 0.7f, float hoveredOpacity = 1f)
		{
			image.add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Control>(image, (object)new
				{
					Opacity = hoveredOpacity
				}, 0.2f, 0f, true);
			});
			image.add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Control>(image, (object)new
				{
					Opacity = unhoveredOpacity
				}, 0.2f, 0f, true);
			});
			image.set_Opacity(unhoveredOpacity);
		}
	}
}
