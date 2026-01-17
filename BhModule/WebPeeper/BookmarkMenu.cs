using System.ComponentModel;
using System.Reflection;
using Blish_HUD.Controls;

namespace BhModule.WebPeeper
{
	public class BookmarkMenu : Menu
	{
		private static readonly FieldInfo _childPropertyChangedField = typeof(Control).GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);

		private const int ItemHeight = 40;

		public bool ChildrenSeparated { get; private set; }

		public BookmarkMenu()
			: this()
		{
			((Menu)this).set_MenuItemHeight(40);
		}

		public void SeparateChildren()
		{
			foreach (Control child in ((Container)this)._children)
			{
				if (_childPropertyChangedField.GetValue(child) is PropertyChangedEventHandler)
				{
					_childPropertyChangedField.SetValue(child, null);
				}
			}
			ChildrenSeparated = true;
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			if (ChildrenSeparated && ((Container)this)._children.get_Count() <= 1)
			{
				ChildrenSeparated = false;
			}
			((Container)this).OnChildRemoved(e);
		}

		public override void RecalculateLayout()
		{
			if (!ChildrenSeparated)
			{
				((Menu)this).RecalculateLayout();
			}
		}
	}
}
