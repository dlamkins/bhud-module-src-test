using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters
{
	public class ToggleImage : Image
	{
		public bool isActive;

		public int Id;

		private int __State;

		public Texture2D[] _Textures;

		public int _State
		{
			get
			{
				return __State;
			}
			set
			{
				__State = value;
				_OnStateChanged();
			}
		}

		public int _MaxState { get; set; }

		public event EventHandler _StateChanged;

		private void _OnStateChanged()
		{
			if (_Textures != null && _Textures.Length > __State)
			{
				base.Texture = _Textures[__State];
			}
		}

		public int Toggle()
		{
			_State = ((_State + 1 <= _MaxState - 1) ? (_State + 1) : 0);
			return _State;
		}
	}
}
