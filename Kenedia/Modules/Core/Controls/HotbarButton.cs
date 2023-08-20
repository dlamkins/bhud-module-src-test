using System;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class HotbarButton : Control, ICheckable
	{
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
				Common.SetProperty(ref _checked, value, OnCheckChanged);
			}
		}

		public event EventHandler<CheckChangedEvent> CheckedChanged;

		public HotbarButton()
			: this()
		{
			((Control)this).set_ClipsBounds(true);
		}

		private void OnCheckChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			this.CheckedChanged?.Invoke(this, new CheckChangedEvent((e.NewValue ? ((byte)1) : ((byte)0)) != 0));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (Icon != null)
			{
				Icon.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Icon?.Dispose();
		}
	}
}
