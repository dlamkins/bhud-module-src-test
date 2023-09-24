using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class IconPicker : FlowPanel
	{
		protected List<(int, Texture2D, IconButton)> state = new List<(int, Texture2D, IconButton)>();

		protected int _selectedItem = -1;

		public event EventHandler<int>? IconSelectionChanged;

		public IconPicker()
			: this()
		{
		}

		public void SelectItem(int select)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			_selectedItem = select;
			foreach (var e in state)
			{
				if (e.Item1 == select)
				{
					e.Item3.Checked = true;
					((Control)e.Item3).set_Opacity(1f);
					((Control)e.Item3).set_Size(new Point(32, 33));
				}
				else
				{
					e.Item3.Checked = false;
					((Control)e.Item3).set_Opacity(0.3f);
					((Control)e.Item3).set_Size(new Point(28, 28));
				}
			}
			this.IconSelectionChanged?.Invoke(this, select);
		}

		public void LoadList(List<(int, Texture2D)> textureList)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			state.Clear();
			((Container)this).get_Children().Clear();
			foreach (var texture in textureList)
			{
				IconButton iconButton = new IconButton();
				((Control)iconButton).set_Parent((Container)(object)this);
				iconButton.Icon = texture.Item2;
				iconButton.Checked = false;
				((Control)iconButton).set_Opacity(0.3f);
				((Control)iconButton).set_Size(new Point(28, 28));
				IconButton btn = iconButton;
				((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectItem(texture.Item1);
				});
				state.Add((texture.Item1, texture.Item2, btn));
			}
		}
	}
}
