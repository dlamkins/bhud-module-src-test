using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Manlaan.CommanderMarkers.Utils
{
	public record MenuViewItem
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(MenuViewItem);
			}
		}

		public Func<MenuItem, IView> ViewFunction { get; }

		public MenuItem MenuItem { get; }

		public MenuViewItem(MenuItem MenuItem, Func<MenuItem, IView> ViewFunction)
		{
			this.ViewFunction = ViewFunction;
			this.MenuItem = MenuItem;
			base._002Ector();
		}

		[CompilerGenerated]
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

		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ViewFunction = ");
			builder.Append(ViewFunction);
			builder.Append(", MenuItem = ");
			builder.Append(MenuItem);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<Func<MenuItem, IView>>.Default.GetHashCode(ViewFunction)) * -1521134295 + EqualityComparer<MenuItem>.Default.GetHashCode(MenuItem);
		}

		[CompilerGenerated]
		public virtual bool Equals(MenuViewItem? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<Func<MenuItem, IView>>.Default.Equals(ViewFunction, other!.ViewFunction))
				{
					return EqualityComparer<MenuItem>.Default.Equals(MenuItem, other!.MenuItem);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected MenuViewItem(MenuViewItem original)
		{
			ViewFunction = original.ViewFunction;
			MenuItem = original.MenuItem;
		}

		[CompilerGenerated]
		public void Deconstruct(out MenuItem MenuItem, out Func<MenuItem, IView> ViewFunction)
		{
			MenuItem = this.MenuItem;
			ViewFunction = this.ViewFunction;
		}
	}
}
