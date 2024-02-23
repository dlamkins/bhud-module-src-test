using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Ideka.BHUDCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ideka.CustomCombatText
{
	internal class LocalData
	{
		private static readonly Logger Logger = Logger.GetLogger<LocalData>();

		private readonly JsonSerializerSettings _serializerSettings;

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
			File.WriteAllText(Path.Combine(CTextModule.BasePath, CTextModule.ViewsDataPath), JsonConvert.SerializeObject((object)models, _serializerSettings));
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

		public LocalData()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_TypeNameHandling((TypeNameHandling)0);
			val.set_Formatting((Formatting)1);
			val.set_ContractResolver((IContractResolver)(object)new ContractResolver());
			val.get_Converters().Add((JsonConverter)(object)new Vector2Converter());
			val.get_Converters().Add((JsonConverter)(object)new TimeSpanConverter());
			_serializerSettings = val;
			base._002Ector();
		}
	}
}
