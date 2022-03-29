using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters
{
	public class ToggleIcon : Image
	{
		public bool isActive;

		public int Id;

		private int __State;

		public List<Texture2D> _Textures = new List<Texture2D>();

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

		public ToggleIcon()
		{
			base.Click += delegate
			{
				Toggle();
				Module.filterCharacterPanel = true;
			};
			base.Size = new Point(32, 32);
			base.Texture = Textures.Icons[1];
		}

		private void _OnStateChanged()
		{
			if (_Textures != null && _Textures.Count > __State)
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
