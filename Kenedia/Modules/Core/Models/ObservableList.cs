using System.Collections.Generic;
using System.ComponentModel;

namespace Kenedia.Modules.Core.Models
{
	public class ObservableList<T> : List<T>, INotifyPropertyChanged
	{
		public new T this[int i]
		{
			get
			{
				return base[i];
			}
			set
			{
				T oldValue = base[i];
				if (!base[i].Equals(value))
				{
					base[i] = value;
					OnPropertyChanged(this, new PropertyChangedEventArgs($"{i}"));
					OnItemChanged(this, new ValueChangedEventArgs<T>(oldValue, value));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event ValueChangedEventHandler<T> ItemChanged;

		public virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.PropertyChanged?.Invoke(sender, e);
		}

		public virtual void OnItemChanged(object sender, ValueChangedEventArgs<T> e)
		{
			this.ItemChanged?.Invoke(sender, e);
		}
	}
}
