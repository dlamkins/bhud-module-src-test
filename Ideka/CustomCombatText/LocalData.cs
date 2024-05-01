using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Ideka.BHUDCommon;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	internal class LocalData
	{
		private static readonly Logger Logger = Logger.GetLogger<LocalData>();

		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.None,
			Formatting = Formatting.Indented,
			ContractResolver = new ContractResolver(),
			Converters = 
			{
				(JsonConverter)new Vector2Converter(),
				(JsonConverter)new TimeSpanConverter()
			}
		};

		public AreaViewBase AreaViewParent { get; } = new AreaViewBase();


		public IEnumerable<AreaView> RootAreaViews => AreaViewParent.GetAreaViewChildren();

		public event Action? ViewsReloaded;

		public void ReloadViews()
		{
			List<AreaModel> models = null;
			try
			{
				models = JsonConvert.DeserializeObject<List<AreaModel>>(CTextModule.ExtractAndRead(CTextModule.ViewsDataPath), _serializerSettings);
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to deserialize message areas.");
			}
			if (models == null)
			{
				models = new List<AreaModel>();
			}
			AreaViewParent.ClearChildren();
			foreach (AreaView child in ModelsToViews(models))
			{
				AreaViewParent.AddChild(child);
			}
			SaveViews();
			this.ViewsReloaded?.Invoke();
		}

		public void SaveViews()
		{
			List<AreaModel> models = new List<AreaModel>(ViewsToModels(RootAreaViews));
			File.WriteAllText(Path.Combine(CTextModule.BasePath, CTextModule.ViewsDataPath), JsonConvert.SerializeObject(models, _serializerSettings));
		}

		private static IEnumerable<AreaView> ModelsToViews(IEnumerable<AreaModel> models)
		{
			foreach (AreaModel model in models)
			{
				if (model == null)
				{
					Logger.Warn("Found null model, ignoring.");
					continue;
				}
				AreaViewType viewType = model.ModelType.CreateView(model);
				if (viewType == null)
				{
					Logger.Warn("Found invalid view type for: \"" + model.Describe + "\", ignoring.");
					continue;
				}
				AreaView view = new AreaView(viewType);
				foreach (AreaView child in ModelsToViews(model.Children))
				{
					view.AddChild(child);
				}
				model.Children.Clear();
				yield return view;
			}
		}

		private static IEnumerable<AreaModel> ViewsToModels(IEnumerable<AreaView> views)
		{
			foreach (AreaView view in views)
			{
				view.Model.Children.Clear();
				foreach (AreaModel child in ViewsToModels(view.GetChildren()))
				{
					view.Model.Children.Add(child);
				}
				yield return view.Model;
			}
		}
	}
}
