using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ideka.CustomCombatText
{
	public class ModelTypeScroll : IAreaModelType
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public enum Curve
		{
			[EnumMember(Value = "7yGN8zjaSZO-hubi0O5ZKg")]
			None,
			[EnumMember(Value = "qCV5_z4TQnm2k06GHKxTVg")]
			Left,
			[EnumMember(Value = "M6HqFU0EQxuRcxjouBABiQ")]
			Right
		}

		public AreaType Type => AreaType.Scroll;

		public float MessagePivotX { get; set; }

		public Curve CurveType { get; set; } = Curve.Right;


		public int ScrollSpeed { get; set; } = 90;


		public int MaxQueueSize { get; set; } = 3;


		public AreaViewType CreateView(AreaModel model)
		{
			return new ViewTypeScroll(model);
		}
	}
}
