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
				if (!base[i].Equals(value))
				{
					base[i] = value;
					OnPropertyChanged(this, new PropertyChangedEventArgs($"{i}"));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.PropertyChanged?.Invoke(sender, e);
		}
	}
}
