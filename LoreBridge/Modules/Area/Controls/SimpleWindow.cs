using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Area.Controls
{
	public abstract class SimpleWindow : Container, IWindow
	{
		private readonly Tween _animFade;

		private bool _canClose = true;

		private bool _canCloseWithEscape = true;

		private double _lastWindowInteract;

		private bool _topMost;

		public override int ZIndex
		{
			get
			{
				return ((Control)this)._zIndex + WindowBase2.GetZIndex((IWindow)(object)this);
			}
			set
			{
				((Control)this).SetProperty<int>(ref ((Control)this)._zIndex, value, false, "ZIndex");
			}
		}

		public bool CanClose
		{
			get
			{
				return _canClose;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canClose, value, false, "CanClose");
			}
		}

		public bool CanCloseWithEscape
		{
			get
			{
				return _canCloseWithEscape;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _canCloseWithEscape, value, false, "CanCloseWithEscape");
			}
		}

		public bool TopMost
		{
			get
			{
				return _topMost;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _topMost, value, false, "TopMost");
			}
		}

		double LastInteraction => _lastWindowInteract;

		protected SimpleWindow()
			: this()
		{
			((Control)this).set_Opacity(0f);
			((Control)this).set_Visible(false);
			((Control)this)._zIndex = 41;
			((Control)this).set_ClipsBounds(false);
			_animFade = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<SimpleWindow>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Repeat(-1).Reflect();
			_animFade.Pause();
			_animFade.OnComplete((Action)delegate
			{
				_animFade.Pause();
				if (((Control)this)._opacity <= 0f)
				{
					((Control)this).set_Visible(false);
				}
			});
		}

		public override void Show()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			BringWindowToFront();
			if (!((Control)this).get_Visible())
			{
				((Control)this).set_Location(new Point(MathHelper.Clamp(((Control)this)._location.X, 0, ((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 64), MathHelper.Clamp(((Control)this)._location.Y, 0, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 64)));
				((Control)this).set_Opacity(0f);
				((Control)this).set_Visible(true);
				_animFade.Resume();
			}
		}

		public override void Hide()
		{
			if (((Control)this).get_Visible())
			{
				_animFade.Resume();
			}
		}

		public void BringWindowToFront()
		{
			_lastWindowInteract = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		public void ToggleWindow()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
			}
			else
			{
				((Control)this).Show();
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			CalculateWindow();
		}

		private void CalculateWindow()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_ContentRegion(new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()));
		}
	}
}
