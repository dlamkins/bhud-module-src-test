using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplateCollection : IEnumerable<Template>, IEnumerable
	{
		private ObservableCollection<Template> _templates = new ObservableCollection<Template>();

		public NotifyCollectionChangedEventHandler? CollectionChanged;

		public bool IsLoaded { get; private set; }

		public int Count => _templates.Count;

		public Logger Logger { get; }

		public Paths Paths { get; }

		public TemplateFactory TemplateFactory { get; }

		public TemplateConverter TemplateConverter { get; }

		public event PropertyChangedEventHandler? TemplateChanged;

		public event EventHandler? Loaded;

		public TemplateCollection(Logger logger, Paths paths, TemplateFactory templateFactory, TemplateConverter templateConverter)
		{
			Logger = logger;
			Paths = paths;
			TemplateFactory = templateFactory;
			TemplateConverter = templateConverter;
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
				AttachTemplate(template);
			}
		}

		private void AttachTemplate(Template template)
		{
			template.LastModifiedChanged += new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
			template.NameChanged += new ValueChangedEventHandler<string>(Template_NameChanged);
			template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
			template.EliteSpecializationChanged += new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
		}

		private void DetachTemplate(Template template)
		{
			template.LastModifiedChanged -= new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
			template.NameChanged -= new ValueChangedEventHandler<string>(Template_NameChanged);
			template.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
			template.EliteSpecializationChanged -= new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
		}

		private void Template_ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("Profession"));
		}

		private void Template_EliteSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("EliteSpecialization"));
		}

		private void Template_NameChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("Name"));
		}

		private void Template_LastModifiedChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			this.TemplateChanged?.Invoke(sender, new PropertyChangedEventArgs("LastModified"));
		}

		public bool Remove(Template template)
		{
			if (template == null)
			{
				return false;
			}
			DetachTemplate(template);
			return _templates.Remove(template);
		}

		public void Clear()
		{
			foreach (Template template in _templates)
			{
				DetachTemplate(template);
			}
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

		public async Task Load()
		{
			Logger.Info("LoadTemplates");
			IsLoaded = false;
			_templates.CollectionChanged -= OnCollectionChanged;
			Stopwatch time = new Stopwatch();
			time.Start();
			try
			{
				string[] templateFiles = Directory.GetFiles(Paths.TemplatesPath);
				Clear();
				JsonSerializerSettings settings = new JsonSerializerSettings();
				settings.get_Converters().Add((JsonConverter)(object)TemplateConverter);
				Logger.Info($"Loading {templateFiles.Length} Templates ...");
				string[] array = templateFiles;
				foreach (string file in array)
				{
					using StreamReader reader = new StreamReader(file);
					Template template = JsonConvert.DeserializeObject<Template>(await reader.ReadToEndAsync(), settings);
					template.SaveRequested = false;
					_templates.Add(template);
					AttachTemplate(template);
				}
				if (_templates.Count == 0)
				{
					Template template2 = TemplateFactory.CreateTemplate();
					_templates.Add(template2);
					AttachTemplate(template2);
				}
				time.Stop();
				Logger.Info($"Time to load {templateFiles.Length} templates {time.ElapsedMilliseconds}ms. {_templates.Count} out of {templateFiles.Length} templates got loaded.");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex.Message);
				Logger.Warn("Loading Templates failed!");
			}
			_templates.CollectionChanged += OnCollectionChanged;
			IsLoaded = true;
			this.Loaded?.Invoke(this, EventArgs.Empty);
		}
	}
}
