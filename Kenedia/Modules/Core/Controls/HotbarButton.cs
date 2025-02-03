using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class HotbarButton : Control, ICheckable
	{
		private readonly DetailedTexture _active = new DetailedTexture(157336);

		private readonly DetailedTexture _inactive = new DetailedTexture(102538);

		public int Index;

		private bool _checked;

		public DetailedTexture Icon { get; set; }

		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				Common.SetProperty(ref _checked, value, new ValueChangedEventHandler<bool>(On_CheckChanged));
			}
		}

		public Action<bool> OnCheckChanged { get; set; }

		public event EventHandler<CheckChangedEvent> CheckedChanged;

		public HotbarButton()
		{
			base.ClipsBounds = true;
		}

		private void On_CheckChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			OnCheckChanged?.Invoke((e.NewValue ? ((byte)1) : ((byte)0)) != 0);
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent((e.NewValue ? ((byte)1) : ((byte)0)) != 0));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Checked = !Checked;
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (Icon != null)
			{
				Icon.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			}
			double offset = 0.7;
			_active.Bounds = new Rectangle(base.Width - (int)((double)base.Width * offset), base.Height - (int)((double)base.Height * offset), base.Width, base.Height);
			_inactive.Bounds = new Rectangle(base.Width - (int)((double)base.Width * offset), base.Height - (int)((double)base.Height * offset), base.Width, base.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Icon?.Draw(this, spriteBatch, base.RelativeMousePosition);
			(Checked ? _active : _inactive).Draw(this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			if (Icon != null)
			{
				Icon.Texture = null;
				Icon.FallBackTexture = null;
				Icon.HoveredTexture = null;
				Icon = null;
			}
		}
	}
}
