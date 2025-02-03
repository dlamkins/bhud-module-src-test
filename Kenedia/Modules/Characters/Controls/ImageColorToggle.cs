using System;
using Blish_HUD.Input;
using Gw2Sharp.Models;

namespace Kenedia.Modules.Characters.Controls
{
	internal class ImageColorToggle : ImageGrayScaled
	{
		private readonly Action<bool> _onChanged;

		public ProfessionType Profession { get; set; }

		public ImageColorToggle()
		{
		}

		public ImageColorToggle(Action<bool> onChanged)
			: this()
		{
			_onChanged = onChanged;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			base.Active = !base.Active;
			_onChanged?.Invoke(base.Active);
		}
	}
}
