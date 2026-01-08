using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

		[IteratorStateMachine(typeof(_003CModelsToViews_003Ed__12))]
		private static IEnumerable<AreaView> ModelsToViews(IEnumerable<AreaModel> models)
		{
			return new _003CModelsToViews_003Ed__12(-2)
			{
				_003C_003E3__models = models
			};
		}

		[IteratorStateMachine(typeof(_003CViewsToModels_003Ed__13))]
		private static IEnumerable<AreaModel> ViewsToModels(IEnumerable<AreaView> views)
		{
			return new _003CViewsToModels_003Ed__13(-2)
			{
				_003C_003E3__views = views
			};
		}
	}
}
