using System.Collections.Generic;
using Ideka.BHUDCommon.AnchoredRect;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public sealed class AreaModel
	{
		public string Name { get; set; } = "";


		public bool Enabled { get; set; } = true;


		public Anchoring Anchoring { get; set; } = new Anchoring();


		public List<AreaModel> Children { get; set; } = new List<AreaModel>();


		public List<MessageReceiver> Receivers { get; set; } = new List<MessageReceiver>();


		[JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
		public IAreaModelType ModelType { get; set; } = new ModelTypeContainer();


		[JsonIgnore]
		public string Describe
		{
			get
			{
				if (!string.IsNullOrEmpty(Name))
				{
					return Name;
				}
				return "(unnamed)";
			}
		}
	}
}
