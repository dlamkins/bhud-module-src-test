using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Maestro.Models
{
	public static class InstrumentCatalog
	{
		private static readonly string[] ThreeOctaveLabels;

		private static readonly IReadOnlyList<InstrumentInfo> _all;

		private static readonly Dictionary<InstrumentType, InstrumentInfo> _byType;

		private static readonly IReadOnlyList<InstrumentInfo> _pickable;

		public static IReadOnlyList<InstrumentInfo> All => _all;

		public static IReadOnlyList<InstrumentInfo> Pickable => _pickable;

		static InstrumentCatalog()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			ThreeOctaveLabels = new string[3] { "Lower (-)", "Middle", "Upper (+)" };
			_all = new List<InstrumentInfo>
			{
				new InstrumentInfo(InstrumentType.Piano, "Piano", new Color(79, 155, 224), new Color(53, 122, 192), sharpsEnabled: true, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Harp, "Harp", new Color(107, 194, 136), new Color(62, 154, 99), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Lute, "Lute", new Color(227, 165, 58), new Color(190, 132, 32), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Bass, "Bass", new Color(224, 106, 124), new Color(184, 72, 94), sharpsEnabled: false, 0, 1, new string[2] { "Low", "High" }),
				new InstrumentInfo(InstrumentType.Flute, "Flute", new Color(165, 121, 224), new Color(126, 84, 190), sharpsEnabled: false, -1, 0, new string[2] { "Low", "Middle" }),
				new InstrumentInfo(InstrumentType.Bell, "Bell (3 octaves)", new Color(63, 194, 178), new Color(42, 148, 136), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.BellMagnanimous, "Bell (2 octaves)", new Color(116, 214, 190), new Color(73, 174, 151), sharpsEnabled: false, 0, 1, new string[2] { "Middle", "High" }),
				new InstrumentInfo(InstrumentType.DrumSet, "Drum Set", new Color(198, 110, 64), new Color(160, 82, 45), sharpsEnabled: false, 0, 0, new string[1] { "Kit" }, listedInPickers: true, isPercussion: true)
			};
			_byType = _all.ToDictionary((InstrumentInfo i) => i.Type);
			_pickable = _all.Where((InstrumentInfo i) => i.ListedInPickers).ToList().AsReadOnly();
			foreach (InstrumentType type in Enum.GetValues(typeof(InstrumentType)))
			{
				if (!_byType.ContainsKey(type))
				{
					throw new InvalidOperationException($"InstrumentCatalog is missing a row for InstrumentType.{type}");
				}
			}
		}

		public static InstrumentInfo Get(InstrumentType type)
		{
			return _byType[type];
		}

		public static bool TryFromDisplayName(string displayName, out InstrumentType type)
		{
			foreach (InstrumentInfo info in _all)
			{
				if (info.DisplayName == displayName)
				{
					type = info.Type;
					return true;
				}
			}
			type = InstrumentType.Piano;
			return false;
		}
	}
}
