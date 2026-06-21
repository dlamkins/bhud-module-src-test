using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class EnabledIconButton : IconButton, IDisposable
	{
		private Texture2D _enabledTexture = Service.Textures!._imgCheck;

		private Texture2D _disabledTexture = Service.Textures!._imgClear;

		private bool _watchValue = true;

		public bool WatchValue
		{
			get
			{
				return _watchValue;
			}
			set
			{
				if (_watchValue != value)
				{
					_watchValue = value;
					SetTexture();
				}
			}
		}

		public event EventHandler<bool>? ValueChanged;

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public EnabledIconButton(bool watchValue, Texture2D? enabledTexture = null, Texture2D? disabledTexture = null)
		{
			_watchValue = watchValue;
			if (enabledTexture != null)
			{
				_enabledTexture = enabledTexture;
			}
			if (disabledTexture != null)
			{
				_disabledTexture = disabledTexture;
			}
			((Control)this).add_Click((EventHandler<MouseEventArgs>)EnabledIconButton_Click);
			SetTexture();
		}

		protected void SetTexture()
		{
			if (_watchValue)
			{
				base.Icon = _enabledTexture;
				((Control)this).set_BasicTooltipText("Click to disable this marker set");
			}
			else
			{
				base.Icon = _disabledTexture;
				((Control)this).set_BasicTooltipText("Click to enable this marker set");
			}
			((Control)this).Invalidate();
		}

		private void EnabledIconButton_Click(object sender, MouseEventArgs e)
		{
			WatchValue = !WatchValue;
			this.ValueChanged?.Invoke(this, WatchValue);
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)EnabledIconButton_Click);
		}
	}
}
