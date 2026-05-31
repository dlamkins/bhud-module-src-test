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
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			ThreeOctaveLabels = new string[3] { "Lower (-)", "Middle", "Upper (+)" };
			_all = new List<InstrumentInfo>
			{
				new InstrumentInfo(InstrumentType.Piano, "Piano", new Color(126, 200, 227), new Color(90, 176, 208), sharpsEnabled: true, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Harp, "Harp", new Color(184, 212, 168), new Color(140, 196, 144), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Lute, "Lute", new Color(232, 193, 112), new Color(212, 166, 86), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.Bass, "Bass", new Color(212, 132, 140), new Color(192, 112, 120), sharpsEnabled: false, 0, 1, new string[2] { "Low", "High" }),
				new InstrumentInfo(InstrumentType.Flute, "Flute", new Color(175, 160, 220), new Color(135, 118, 190), sharpsEnabled: false, -1, 0, new string[2] { "Low", "Middle" }),
				new InstrumentInfo(InstrumentType.Bell, "Bell (3 octaves)", new Color(150, 196, 190), new Color(108, 156, 150), sharpsEnabled: false, -1, 1, ThreeOctaveLabels),
				new InstrumentInfo(InstrumentType.BellMagnanimous, "Bell (2 octaves)", new Color(176, 208, 200), new Color(130, 176, 168), sharpsEnabled: false, 0, 1, new string[2] { "Middle", "High" })
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
