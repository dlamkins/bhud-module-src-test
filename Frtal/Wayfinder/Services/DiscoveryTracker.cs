using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Frtal.Wayfinder.Models;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Services
{
	public class DiscoveryTracker
	{
		private static readonly Logger Logger = Logger.GetLogger<DiscoveryTracker>();

		private readonly string _dir;

		private readonly HashSet<string> _seen = new HashSet<string>();

		private int _mapId = -1;

		public int SeenCount => _seen.Count;

		public DiscoveryTracker(string moduleDirectory)
		{
			_dir = Path.Combine(moduleDirectory ?? ".", "discovery");
			try
			{
				Directory.CreateDirectory(_dir);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Could not create the tracking directory.");
			}
		}

		public bool IsSeen(string id)
		{
			return _seen.Contains(id);
		}

		public void SetSeen(string id, bool seen)
		{
			if (!string.IsNullOrEmpty(id) && (seen ? _seen.Add(id) : _seen.Remove(id)))
			{
				Save();
			}
		}

		public void MarkAll(IEnumerable<CompassTarget> targets)
		{
			bool changed = false;
			foreach (CompassTarget t in targets)
			{
				if (_seen.Add(t.Id))
				{
					changed = true;
				}
			}
			if (changed)
			{
				Save();
			}
		}

		public void MarkIds(IEnumerable<string> ids)
		{
			bool changed = false;
			foreach (string id in ids)
			{
				if (!string.IsNullOrEmpty(id) && _seen.Add(id))
				{
					changed = true;
				}
			}
			if (changed)
			{
				Save();
			}
		}

		public void ClearAll()
		{
			if (_seen.Count != 0)
			{
				_seen.Clear();
				Save();
			}
		}

		public void SetMap(int mapId)
		{
			if (mapId == _mapId)
			{
				return;
			}
			_mapId = mapId;
			_seen.Clear();
			try
			{
				string path = FilePath();
				if (!File.Exists(path))
				{
					return;
				}
				string[] array = File.ReadAllLines(path);
				foreach (string line in array)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						_seen.Add(line.Trim());
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load tracking data.");
			}
		}

		public void MarkNearby(Vector2 playerCont, IReadOnlyList<CompassTarget> targets, float thresholdContinent)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			bool changed = false;
			float sq = thresholdContinent * thresholdContinent;
			foreach (CompassTarget t in targets)
			{
				if (!_seen.Contains(t.Id) && Vector2.DistanceSquared(playerCont, t.ContinentPosition) <= sq)
				{
					_seen.Add(t.Id);
					changed = true;
				}
			}
			if (changed)
			{
				Save();
			}
		}

		private void Save()
		{
			try
			{
				File.WriteAllLines(FilePath(), _seen);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to save tracking data.");
			}
		}

		private string FilePath()
		{
			return Path.Combine(_dir, $"{_mapId}.txt");
		}
	}
}
