using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal abstract class TaskIndicatorBase : Control
	{
		private static readonly Texture2D StopIcon = GameService.Content.GetTexture("common/154982");

		private static readonly Texture2D MouseIcon = GameService.Content.GetTexture("156734");

		private bool _paused;

		private bool _attachToCursor;

		private Point _mousePos => GameService.Input.get_Mouse().get_Position();

		public bool Paused
		{
			get
			{
				return _paused;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _paused, value, false, "Paused");
			}
		}

		public bool AttachToCursor
		{
			get
			{
				return _attachToCursor;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _attachToCursor, value, false, "AttachToCursor");
			}
		}

		protected TaskIndicatorBase(bool attachToCursor = true)
			: this()
		{
			AttachToCursor = attachToCursor;
			((Control)this).set_ZIndex(1000);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		private void Update()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (_attachToCursor)
			{
				((Control)this).set_Location(new Point(_mousePos.X + ((Control)this).get_Width() / 2, _mousePos.Y - ((Control)this).get_Height() / 2));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			Update();
			if (_paused)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MouseIcon, new Rectangle((bounds.Width - MouseIcon.get_Width()) / 2, (bounds.Height - MouseIcon.get_Height()) / 2, MouseIcon.get_Width(), MouseIcon.get_Height()), (Rectangle?)MouseIcon.get_Bounds());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, StopIcon, new Rectangle((bounds.Width - StopIcon.get_Width()) / 2, (bounds.Height - StopIcon.get_Height()) / 2, StopIcon.get_Width(), StopIcon.get_Height()), (Rectangle?)StopIcon.get_Bounds(), Color.get_White() * 0.65f);
			}
		}
	}
}
