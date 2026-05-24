using System;
using System.Collections.Generic;

namespace DavidRice.BlishHud.MidiControl.Input
{
	public static class KeyToScanCode
	{
		private static readonly Dictionary<string, uint> Map = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase)
		{
			["1"] = 2u,
			["2"] = 3u,
			["3"] = 4u,
			["4"] = 5u,
			["5"] = 6u,
			["6"] = 7u,
			["7"] = 8u,
			["8"] = 9u,
			["9"] = 10u,
			["0"] = 11u,
			["A"] = 30u,
			["B"] = 48u,
			["C"] = 46u,
			["D"] = 32u,
			["E"] = 18u,
			["F"] = 33u,
			["G"] = 34u,
			["H"] = 35u,
			["I"] = 23u,
			["J"] = 36u,
			["K"] = 37u,
			["L"] = 38u,
			["M"] = 50u,
			["N"] = 49u,
			["O"] = 24u,
			["P"] = 25u,
			["Q"] = 16u,
			["R"] = 19u,
			["S"] = 31u,
			["T"] = 20u,
			["U"] = 22u,
			["V"] = 47u,
			["W"] = 17u,
			["X"] = 45u,
			["Y"] = 21u,
			["Z"] = 44u,
			["F1"] = 59u,
			["F2"] = 60u,
			["F3"] = 61u,
			["F4"] = 62u,
			["F5"] = 63u,
			["F6"] = 64u,
			["F7"] = 65u,
			["F8"] = 66u,
			["F9"] = 67u,
			["F10"] = 68u,
			["F11"] = 87u,
			["F12"] = 88u,
			["SPACE"] = 57u,
			["ENTER"] = 28u,
			["ESC"] = 1u,
			["ESCAPE"] = 1u,
			["TAB"] = 15u,
			["BACKSPACE"] = 14u
		};

		public static uint? For(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (Map.TryGetValue(key, out var sc))
			{
				return sc;
			}
			return null;
		}

		public static string? GetKeyName(uint scanCode)
		{
			foreach (KeyValuePair<string, uint> kvp in Map)
			{
				if (kvp.Value == scanCode)
				{
					return kvp.Key;
				}
			}
			return null;
		}
	}
}
