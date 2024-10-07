using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Gw2Sharp.Models;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplateCollection : IEnumerable<Template>, IEnumerable
	{
		private ObservableCollection<Template> _templates = new ObservableCollection<Template>();

		public NotifyCollectionChangedEventHandler? CollectionChanged;

		public int Count => _templates.Count;

		public event PropertyChangedEventHandler? TemplateChanged;

		public TemplateCollection()
		{
			_templates.CollectionChanged += OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(sender, e);
		}

		public void Add(Template template)
		{
			if (template != null)
			{
				_templates.Add(template);
				template.LastModifiedChanged += new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
				template.NameChanged += new ValueChangedEventHandler<string>(Template_NameChanged);
				template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
			}
		}

		private void Template_ProfessionChanged(object sender, ValueChangedEventArgs<ProfessionType> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("Profession"));
		}

		private void Template_NameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("Name"));
		}

		private void Template_LastModifiedChanged(object sender, ValueChangedEventArgs<string> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("LastModified"));
		}

		public bool Remove(Template template)
		{
			if (template == null)
			{
				return false;
			}
			template.LastModifiedChanged -= new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
			template.NameChanged -= new ValueChangedEventHandler<string>(Template_NameChanged);
			template.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
			return _templates.Remove(template);
		}

		public void Clear()
		{
			_templates.Clear();
		}

		public IEnumerator<Template> GetEnumerator()
		{
			return _templates.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public string GetNewName(string name)
		{
			string name2 = name;
			if (_templates.All((Template t) => t.Name != name2))
			{
				return name2;
			}
			for (int i = 1; i < int.MaxValue; i++)
			{
				string newName = $"{name2} ({i})";
				if (_templates.All((Template t) => t.Name != newName))
				{
					return newName;
				}
			}
			return name2;
		}
	}
}
