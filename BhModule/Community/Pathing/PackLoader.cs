using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using TmfLib;
using TmfLib.Reader;

namespace BhModule.Community.Pathing
{
	public class PackLoader
	{
		private static readonly Logger Logger = Logger.GetLogger<PackLoader>();

		private const int LOAD_RETRY_COUNTS = 3;

		private readonly int _mapId;

		private readonly IEnumerable<Pack> _packs;

		private readonly IPackCollection _packCollection;

		private readonly PackReaderSettings _packReaderSettings;

		private readonly IRootPackState _packState;

		private Thread _loadThread;

		public bool Running { get; private set; }

		public PackLoader(int mapId, IEnumerable<Pack> packs, IPackCollection packCollection, PackReaderSettings packReaderSettings, IRootPackState packState)
		{
			_mapId = mapId;
			_packs = packs;
			_packCollection = packCollection;
			_packReaderSettings = packReaderSettings;
			_packState = packState;
		}

		public void Start()
		{
			if (!Running)
			{
				_loadThread = new Thread(Run);
				_loadThread.Start();
			}
		}

		private async void Run()
		{
			Running = true;
			await LoadMapFromEachPack();
			Running = false;
		}

		private async Task LoadMapFromEachPack(int retry = 3)
		{
			try
			{
				foreach (Pack pack in _packs)
				{
					await pack.LoadMapAsync(_mapId, _packCollection, _packReaderSettings);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Loading pack failed.");
				if (retry > 0)
				{
					PackLoader packLoader = this;
					int num = retry - 1;
					retry = num;
					await packLoader.LoadMapFromEachPack(num);
				}
				Logger.Error($"Loading pack failed after {3} attempts.");
			}
			await _packState.LoadPackCollection(_packCollection);
		}
	}
}
