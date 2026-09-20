using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public static class BitAlignmentMatcher
	{
		public readonly struct Row
		{
			public string DisplayName { get; }

			public int Id { get; }

			public Row(string displayName, int id)
			{
				DisplayName = displayName;
				Id = id;
			}
		}

		public static IReadOnlyList<Row> GetRows(AchievementTableEntryDescription description)
		{
			CollectionDescription collection = description as CollectionDescription;
			if (collection == null)
			{
				return (description as ObjectivesDescription)?.EntryList.Select((TableDescriptionEntry e) => new Row(e.DisplayName, 0)).ToList();
			}
			return collection.EntryList.Select((CollectionDescriptionEntry e) => new Row(e.DisplayName, e.Id)).ToList();
		}

		public static int[] ComputeRowToBit(IReadOnlyList<Row> rows, IReadOnlyList<AchievementBit> bits, IReadOnlyDictionary<int, string> skinNamesById, IReadOnlyDictionary<int, string> miniNamesById)
		{
			int[] rowToBit = new int[rows.Count];
			for (int i = 0; i < rowToBit.Length; i++)
			{
				rowToBit[i] = -1;
			}
			if (bits.Count == 0)
			{
				return rowToBit;
			}
			bool[] bitClaimed = new bool[bits.Count];
			string[] bitText = new string[bits.Count];
			int?[] bitItemId = new int?[bits.Count];
			string[] bitResolvedName = new string[bits.Count];
			for (int j = 0; j < bits.Count; j++)
			{
				AchievementBit val = bits[j];
				AchievementTextBit textBit = (AchievementTextBit)(object)((val is AchievementTextBit) ? val : null);
				if (textBit == null)
				{
					AchievementItemBit itemBit = (AchievementItemBit)(object)((val is AchievementItemBit) ? val : null);
					if (itemBit == null)
					{
						AchievementSkinBit skinBit = (AchievementSkinBit)(object)((val is AchievementSkinBit) ? val : null);
						if (skinBit == null)
						{
							AchievementMinipetBit minipetBit = (AchievementMinipetBit)(object)((val is AchievementMinipetBit) ? val : null);
							if (minipetBit != null)
							{
								bitItemId[j] = minipetBit.get_Id();
								if (miniNamesById.TryGetValue(minipetBit.get_Id(), out var miniName))
								{
									bitResolvedName[j] = Normalize(miniName);
								}
							}
						}
						else
						{
							bitItemId[j] = skinBit.get_Id();
							if (skinNamesById.TryGetValue(skinBit.get_Id(), out var skinName))
							{
								bitResolvedName[j] = Normalize(skinName);
							}
						}
					}
					else
					{
						bitItemId[j] = itemBit.get_Id();
					}
				}
				else
				{
					bitText[j] = Normalize(textBit.get_Text());
				}
			}
			for (int row4 = 0; row4 < rows.Count; row4++)
			{
				string name = Normalize(rows[row4].DisplayName);
				if (name == null)
				{
					continue;
				}
				for (int bit = 0; bit < bits.Count; bit++)
				{
					if (!bitClaimed[bit] && bitText[bit] == name)
					{
						rowToBit[row4] = bit;
						bitClaimed[bit] = true;
						break;
					}
				}
			}
			for (int row3 = 0; row3 < rows.Count; row3++)
			{
				if (rowToBit[row3] != -1 || rows[row3].Id <= 0)
				{
					continue;
				}
				for (int bit2 = 0; bit2 < bits.Count; bit2++)
				{
					if (!bitClaimed[bit2] && bitResolvedName[bit2] == null && bitItemId[bit2] == rows[row3].Id)
					{
						rowToBit[row3] = bit2;
						bitClaimed[bit2] = true;
						break;
					}
				}
			}
			for (int row2 = 0; row2 < rows.Count; row2++)
			{
				if (rowToBit[row2] != -1)
				{
					continue;
				}
				string name2 = Normalize(rows[row2].DisplayName);
				if (name2 == null)
				{
					continue;
				}
				for (int bit3 = 0; bit3 < bits.Count; bit3++)
				{
					if (!bitClaimed[bit3] && bitResolvedName[bit3] == name2)
					{
						rowToBit[row2] = bit3;
						bitClaimed[bit3] = true;
						break;
					}
				}
			}
			if (rows.Count == bits.Count)
			{
				for (int row = 0; row < rows.Count; row++)
				{
					if (rowToBit[row] == -1 && !bitClaimed[row])
					{
						rowToBit[row] = row;
						bitClaimed[row] = true;
					}
				}
			}
			return rowToBit;
		}

		public static bool IsIdentity(int[] rowToBit)
		{
			for (int i = 0; i < rowToBit.Length; i++)
			{
				if (rowToBit[i] != i)
				{
					return false;
				}
			}
			return true;
		}

		internal static string Normalize(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			string result = Regex.Replace(value, "\\s+", " ").Trim();
			result = Regex.Replace(result, "\\[\\d+\\]\\s*$", "").TrimEnd();
			if (result.EndsWith("."))
			{
				result = result.Substring(0, result.Length - 1).TrimEnd();
			}
			return result.ToLowerInvariant();
		}
	}
}
