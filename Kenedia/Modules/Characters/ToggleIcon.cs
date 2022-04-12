using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
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
			: this()
		{
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Toggle();
				Module.filterCharacterPanel = true;
			});
			((Control)this).set_Size(new Point(32, 32));
			((Image)this).set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[1]));
		}

		private void _OnStateChanged()
		{
			if (_Textures != null && _Textures.Count > __State)
			{
				((Image)this).set_Texture(AsyncTexture2D.op_Implicit(_Textures[__State]));
			}
		}

		public int Toggle()
		{
			_State = ((_State + 1 <= _MaxState - 1) ? (_State + 1) : 0);
			return _State;
		}
	}
}
