using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using DeimosOilSummary.Models;

namespace DeimosOilSummary.Services
{
	public static class EvtcOilParser
	{
		private enum PlayerLifeStatus
		{
			Alive,
			Downed,
			Dead
		}

		private sealed class AgentInfo
		{
			public ulong Address { get; set; }

			public bool IsPlayer { get; set; }

			public string CharacterName { get; set; }

			public string AccountName { get; set; }
		}

		public const int DeimosSpeciesId = 17154;

		private static readonly HashSet<uint> OilSkillIds = new HashSet<uint> { 37716u, 72779u };

		private const byte ResultBlock = 3;

		private const byte ResultEvade = 4;

		private const byte ResultAbsorb = 6;

		private const byte ResultBlind = 7;

		private const long HitDebounceMs = 900L;

		private const uint PlayerEliteSentinel = uint.MaxValue;

		private const byte StateChangeUp = 3;

		private const byte StateChangeDead = 4;

		private const byte StateChangeDown = 5;

		public static OilHitSummary Parse(string path)
		{
			OilHitSummary summary = new OilHitSummary
			{
				LogPath = path,
				LogTime = File.GetLastWriteTime(path)
			};
			try
			{
				using Stream raw = OpenEvtcStream(path);
				using BinaryReader reader = new BinaryReader(raw, Encoding.UTF8, leaveOpen: false);
				return ParseReader(reader, summary);
			}
			catch (Exception ex)
			{
				summary.Success = false;
				summary.ErrorMessage = ex.Message;
				return summary;
			}
		}

		private static OilHitSummary ParseReader(BinaryReader reader, OilHitSummary summary)
		{
			byte[] magic = reader.ReadBytes(4);
			if (magic.Length < 4 || magic[0] != 69 || magic[1] != 86 || magic[2] != 84 || magic[3] != 67)
			{
				summary.Success = false;
				summary.ErrorMessage = "Not an EVTC file.";
				return summary;
			}
			reader.ReadBytes(8);
			byte revision = reader.ReadByte();
			ushort encounterId = reader.ReadUInt16();
			reader.ReadByte();
			if (encounterId != 17154)
			{
				summary.Success = false;
				summary.ErrorMessage = $"Not a Deimos log (species {encounterId}).";
				return summary;
			}
			if (revision != 1)
			{
				summary.Success = false;
				summary.ErrorMessage = $"Unsupported EVTC revision {revision} (need 1).";
				return summary;
			}
			uint agentCount = reader.ReadUInt32();
			Dictionary<ulong, AgentInfo> agentsByAddress = new Dictionary<ulong, AgentInfo>();
			for (uint j = 0u; j < agentCount; j++)
			{
				AgentInfo agent = ReadAgent(reader);
				agentsByAddress[agent.Address] = agent;
			}
			uint skillCount = reader.ReadUInt32();
			for (uint i = 0u; i < skillCount; i++)
			{
				reader.ReadUInt32();
				reader.ReadBytes(64);
			}
			Dictionary<ulong, int> hitCounts = new Dictionary<ulong, int>();
			Dictionary<ulong, long> lastHitTime = new Dictionary<ulong, long>();
			Dictionary<ushort, ulong> addressByInstId = new Dictionary<ushort, ulong>();
			Dictionary<ulong, PlayerLifeStatus> playerStatus = new Dictionary<ulong, PlayerLifeStatus>();
			Dictionary<ushort, OilFirstTouch> firstTouchByOil = new Dictionary<ushort, OilFirstTouch>();
			List<ushort> firstTouchOrder = new List<ushort>();
			while (reader.BaseStream.Position + 64 <= reader.BaseStream.Length)
			{
				long time = reader.ReadInt64();
				ulong srcAgent = reader.ReadUInt64();
				ulong dstAgent = reader.ReadUInt64();
				int value = reader.ReadInt32();
				int buffDmg = reader.ReadInt32();
				reader.ReadUInt32();
				uint skillId = reader.ReadUInt32();
				ushort srcInstId = reader.ReadUInt16();
				ushort dstInstId = reader.ReadUInt16();
				reader.ReadUInt16();
				reader.ReadUInt16();
				reader.ReadByte();
				byte buff = reader.ReadByte();
				byte result = reader.ReadByte();
				byte isActivation = reader.ReadByte();
				reader.ReadByte();
				reader.ReadByte();
				reader.ReadByte();
				reader.ReadByte();
				byte isStateChange = reader.ReadByte();
				reader.ReadByte();
				reader.ReadByte();
				byte isOffcycle = reader.ReadByte();
				reader.ReadUInt32();
				if (srcInstId != 0 && srcAgent != 0L)
				{
					addressByInstId[srcInstId] = srcAgent;
				}
				if (dstInstId != 0 && dstAgent != 0L)
				{
					addressByInstId[dstInstId] = dstAgent;
				}
				switch (isStateChange)
				{
				case 3:
				case 4:
				case 5:
				{
					if (srcAgent != 0L && agentsByAddress.TryGetValue(srcAgent, out var statusAgent) && statusAgent.IsPlayer)
					{
						switch (isStateChange)
						{
						case 3:
							playerStatus[srcAgent] = PlayerLifeStatus.Alive;
							break;
						case 5:
							playerStatus[srcAgent] = PlayerLifeStatus.Downed;
							break;
						default:
							playerStatus[srcAgent] = PlayerLifeStatus.Dead;
							break;
						}
					}
					break;
				}
				case 0:
				{
					if (isActivation != 0 || !OilSkillIds.Contains(skillId))
					{
						break;
					}
					ulong playerAddress = dstAgent;
					if (playerAddress == 0L && dstInstId != 0 && addressByInstId.TryGetValue(dstInstId, out var mapped))
					{
						playerAddress = mapped;
					}
					if (playerAddress == 0L || !agentsByAddress.TryGetValue(playerAddress, out var player) || !player.IsPlayer || isOffcycle != 0 || (playerStatus.TryGetValue(playerAddress, out var life) && life != 0) || result == 3 || result == 4 || result == 6 || result == 7)
					{
						break;
					}
					bool isDirect = buff == 0 && value > 0;
					bool isConditionTick = buff == 1 && buffDmg > 0;
					if (!isDirect && !isConditionTick)
					{
						break;
					}
					if (srcInstId != 0 && !firstTouchByOil.ContainsKey(srcInstId))
					{
						firstTouchByOil[srcInstId] = new OilFirstTouch
						{
							OilNumber = firstTouchOrder.Count + 1,
							CharacterName = player.CharacterName,
							AccountName = player.AccountName,
							TimeMs = time
						};
						firstTouchOrder.Add(srcInstId);
					}
					if (!lastHitTime.TryGetValue(playerAddress, out var previous) || time - previous >= 900)
					{
						lastHitTime[playerAddress] = time;
						if (!hitCounts.ContainsKey(playerAddress))
						{
							hitCounts[playerAddress] = 0;
						}
						hitCounts[playerAddress]++;
					}
					break;
				}
				}
			}
			summary.FirstTouches = firstTouchOrder.Select((ushort id) => firstTouchByOil[id]).ToList();
			summary.Players = (from p in hitCounts.Select(delegate(KeyValuePair<ulong, int> kv)
				{
					AgentInfo agentInfo = agentsByAddress[kv.Key];
					return new PlayerOilCount
					{
						CharacterName = agentInfo.CharacterName,
						AccountName = agentInfo.AccountName,
						HitCount = kv.Value
					};
				})
				orderby p.HitCount descending
				select p).ThenBy((PlayerOilCount p) => p.DisplayName, StringComparer.OrdinalIgnoreCase).ToList();
			summary.TotalHits = summary.Players.Sum((PlayerOilCount p) => p.HitCount);
			summary.Success = true;
			return summary;
		}

		private static AgentInfo ReadAgent(BinaryReader reader)
		{
			ulong address = reader.ReadUInt64();
			reader.ReadUInt32();
			uint isElite = reader.ReadUInt32();
			reader.ReadUInt16();
			reader.ReadUInt16();
			reader.ReadUInt16();
			reader.ReadUInt16();
			reader.ReadInt16();
			reader.ReadUInt16();
			byte[] nameBytes = reader.ReadBytes(68);
			string[] parts = Encoding.UTF8.GetString(nameBytes).Split(new char[1], StringSplitOptions.None);
			string character = ((parts.Length != 0) ? parts[0].Trim() : string.Empty);
			string account = ((parts.Length > 1) ? parts[1].Trim() : string.Empty);
			return new AgentInfo
			{
				Address = address,
				IsPlayer = (isElite != uint.MaxValue),
				CharacterName = character,
				AccountName = account
			};
		}

		private static Stream OpenEvtcStream(string path)
		{
			string ext = Path.GetExtension(path)?.ToLowerInvariant();
			if (ext == ".zevtc" || ext == ".zip")
			{
				MemoryStream ms = new MemoryStream();
				ZipArchive zip = ZipFile.OpenRead(path);
				try
				{
					using Stream entryStream = (zip.get_Entries().FirstOrDefault((ZipArchiveEntry e) => !string.IsNullOrEmpty(e.get_Name())) ?? throw new InvalidDataException("Empty .zevtc archive.")).Open();
					entryStream.CopyTo(ms);
				}
				finally
				{
					((IDisposable)zip)?.Dispose();
				}
				ms.Position = 0L;
				return ms;
			}
			return File.OpenRead(path);
		}
	}
}
