using System;
using System.Runtime.CompilerServices;
using System.Text;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Manlaan.CommanderMarkers.Utils
{
	public record MenuViewItem
	{
		public Func<MenuItem, IView> ViewFunction { get; }

		public MenuItem MenuItem { get; }

		public MenuViewItem(MenuItem MenuItem, Func<MenuItem, IView> ViewFunction)
		{
			this.ViewFunction = ViewFunction;
			this.MenuItem = MenuItem;
			base._002Ector();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MenuViewItem");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ViewFunction = ");
			builder.Append(ViewFunction);
			builder.Append(", MenuItem = ");
			builder.Append(MenuItem);
			return true;
		}

		public void Deconstruct(out MenuItem MenuItem, out Func<MenuItem, IView> ViewFunction)
		{
			MenuItem = this.MenuItem;
			ViewFunction = this.ViewFunction;
		}
	}
}
