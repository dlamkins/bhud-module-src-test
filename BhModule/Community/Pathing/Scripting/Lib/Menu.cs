using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Menu
	{
		private readonly List<Menu> _menus = new List<Menu>();

		public string Name { get; }

		public Func<Menu, LuaResult> OnClick { get; }

		public bool CanCheck { get; set; }

		public bool Checked { get; set; }

		public IReadOnlyCollection<Menu> Menus => _menus.AsReadOnly();

		public Menu(string name, Func<Menu, LuaResult> onClick, bool canCheck = false, bool @checked = false)
		{
			Name = name;
			OnClick = onClick;
			CanCheck = canCheck;
			Checked = @checked;
		}

		public Menu Add(string name, Func<Menu, LuaResult> onClick)
		{
			return Add(name, onClick, canCheck: false, @checked: false);
		}

		public Menu Add(string name, Func<Menu, LuaResult> onClick, bool canCheck, bool @checked)
		{
			foreach (Menu menu in _menus)
			{
				if (string.Equals(menu.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return menu;
				}
			}
			Menu newMenu = new Menu(name, onClick, canCheck, @checked);
			_menus.Add(newMenu);
			return newMenu;
		}

		public void Remove(Menu menuRef)
		{
			_menus.Remove(menuRef);
		}

		internal ContextMenuStripItem BuildMenu()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(Name);
			val.set_CanCheck(CanCheck);
			val.set_Checked(Checked);
			ContextMenuStripItem menu = val;
			((Control)menu).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Checked = menu.get_Checked();
				OnClick?.Invoke(this);
			});
			if (_menus.Any())
			{
				ContextMenuStrip subMenu = new ContextMenuStrip();
				foreach (Menu item in _menus)
				{
					subMenu.AddMenuItem(item.BuildMenu());
				}
				menu.set_Submenu(subMenu);
			}
			return menu;
		}
	}
}
