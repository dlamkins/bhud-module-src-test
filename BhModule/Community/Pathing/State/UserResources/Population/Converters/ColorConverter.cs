using System;
using BhModule.Community.Pathing.Utility;
using Microsoft.Xna.Framework;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace BhModule.Community.Pathing.State.UserResources.Population.Converters
{
	public class ColorConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(Color);
		}

		public object? ReadYaml(IParser parser, Type type)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new ValueOnlyAttribute(parser.Consume<Scalar>().Value).GetValueAsColor();
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Color val = (Color)(value ?? ((object)Color.get_White()));
			string rawOut = ((Color)(ref val)).get_PackedValue().ToString("X");
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, rawOut));
		}
	}
}
