using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.State
{
	public class CachedMumbleStates : ManagedState
	{
		private int _lastTick = -1;

		private bool _needsUpdate = true;

		private double _mapCenterX;

		private double _mapCenterY;

		private bool _isMapOpen;

		private bool _isCompassRotationEnabled;

		private double _compassRotation;

		public double MapCenterX => GetTickCachedValue(ref _mapCenterX);

		public double MapCenterY => GetTickCachedValue(ref _mapCenterY);

		public bool IsMapOpen => GetTickCachedValue(ref _isMapOpen);

		public bool IsCompassRotationEnabled => GetTickCachedValue(ref _isCompassRotationEnabled);

		public double CompassRotation => GetTickCachedValue(ref _compassRotation);

		private static bool UpdateIsMapOpen()
		{
			return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		private T GetTickCachedValue<T>(ref T value)
		{
			if (_needsUpdate)
			{
				UpdateCache();
			}
			return value;
		}

		private void UpdateCache()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
			_mapCenterX = ((Coordinates2)(ref mapCenter)).get_X();
			_mapCenterY = ((Coordinates2)(ref mapCenter)).get_Y();
			_isMapOpen = GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			_isCompassRotationEnabled = GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled();
			_compassRotation = GameService.Gw2Mumble.get_UI().get_CompassRotation();
			_needsUpdate = false;
		}

		public CachedMumbleStates(IRootPackState rootPackState)
			: base(rootPackState)
		{
		}

		public override Task Reload()
		{
			return Task.CompletedTask;
		}

		public override void Update(GameTime gameTime)
		{
			int newTick = GameService.Gw2Mumble.get_Tick();
			if (GameService.Gw2Mumble.get_Tick() != _lastTick)
			{
				_lastTick = newTick;
				_needsUpdate = true;
			}
		}

		protected override Task<bool> Initialize()
		{
			return Task.FromResult(result: true);
		}

		public override Task Unload()
		{
			return Task.CompletedTask;
		}
	}
}
