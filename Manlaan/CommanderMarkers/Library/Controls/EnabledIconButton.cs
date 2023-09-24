using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class EnabledIconButton : IconButton, IDisposable
	{
		private Texture2D _enabledTexture = Service.Textures!._imgCheck;

		private Texture2D _disabledTexture = Service.Textures!._imgClear;

		private static readonly BitmapFont _font = GameService.Content.get_DefaultFont16();

		private bool _watchValue = true;

		public bool WatchValue
		{
			get
			{
				return _watchValue;
			}
			set
			{
				_watchValue = value;
				SetTexture();
			}
		}

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
				base.Icon = _disabledTexture;
				((Control)this).set_BasicTooltipText("disable");
			}
			else
			{
				base.Icon = _enabledTexture;
				((Control)this).set_BasicTooltipText("enable");
			}
			((Control)this).Invalidate();
		}

		private void EnabledIconButton_Click(object sender, MouseEventArgs e)
		{
			_watchValue = !_watchValue;
			SetTexture();
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)EnabledIconButton_Click);
		}
	}
}
